using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RayEngine.Debug
{
    internal struct ProfileResult
    {
        public string Name;

        public long Start;
        public long ElapsedTime;
        public int ThreadID;
    }

    internal struct InstrumentationSession
    {
        public string Name;
    }

    internal class Instrumentor : IDisposable
    {
        private InstrumentationSession? CurrentSession = null;
        private StreamWriter? OutputFile;
        private object Mutex = new object();

        private static Instrumentor? Instance;

        ~Instrumentor()
        {
            Dispose();
        }

        public void Dispose()
        {
            EndSession();
        }

        public void BeginSession(string name, string filepath = "results.json")
        {
            lock (Mutex)
            {
                if (CurrentSession is not null)
                {
                    Console.WriteLine($"Instrumentor::BeginSession('{name}') when session '{CurrentSession?.Name}' is already open.");
                    InternalEndSession();
                }

                FileStream file = File.Create(filepath);
                OutputFile = new StreamWriter(file);

                CurrentSession = new InstrumentationSession() { Name = name };
                WriteHeader();
            }
        }

        public void EndSession()
        {
            lock (Mutex)
            {
                InternalEndSession();
            }
        }

        public void WriteProfile(ProfileResult result)
        {
            StringBuilder json = new StringBuilder();
            json.AppendLine($",{{");
            json.AppendLine($"\"cat\":\"function\",");
            json.AppendLine($"\"dur\":{result.ElapsedTime:0.000},");
            json.AppendLine($"\"name\":\"{result.Name}\",");
            json.AppendLine($"\"ph\":\"X\",");
            json.AppendLine($"\"pid\":0,");
            json.AppendLine($"\"tid\":{result.ThreadID},");
            json.AppendLine($"\"ts\":{result.Start}");
            json.AppendLine($"}}");

            lock (Mutex)
            {
                if (CurrentSession is not null)
                {
                    OutputFile?.Write(json.ToString());
                    OutputFile?.Flush();
                }
            }
        }

        public static ref Instrumentor Get()
        {            
            Instance ??= new Instrumentor();
            return ref Instance;
        }

        private void WriteHeader()
        {
            OutputFile?.Write("{\"otherData\": {},\"traceEvents\":[{}");
            OutputFile?.Flush();
        }

        private void WriteFooter()
        {
            OutputFile?.Write("]}");
            OutputFile?.Flush();
        }

        private void InternalEndSession()
        {
            if (CurrentSession is not null)
            {
                WriteFooter();
                OutputFile?.Close();
                CurrentSession = null;
            }
        }
    }

    public class InstrumentationTimer : IDisposable
    {
        private string Name;
        private long StartTimepoint;
        private bool Stopped;

        public InstrumentationTimer(string name)
        {
#if DEBUG_WITH_PROFILER
            Name = name;
            Stopped = false;
            StartTimepoint = DateTimeOffset.UtcNow.UtcTicks / (TimeSpan.TicksPerMillisecond / 1000);
#endif
        }

        ~InstrumentationTimer()
        {
            Dispose();
        }

        public void Dispose()
        {
#if DEBUG_WITH_PROFILER
            if (!Stopped)
                Stop();
#endif
        }

        public void Stop()
        {
#if DEBUG_WITH_PROFILER
            var endTimepoint = DateTimeOffset.UtcNow.UtcTicks / (TimeSpan.TicksPerMillisecond / 1000);
            var elapsedTime = endTimepoint - StartTimepoint;

            Instrumentor.Get().WriteProfile(new ProfileResult() { Name = Name, Start = StartTimepoint, ElapsedTime = elapsedTime, ThreadID = Thread.CurrentThread.ManagedThreadId });

            Stopped = true;
#endif
        }
    }

    public static class Profiler
    {
        public static void BeginSession(string name, string filepath)
        {
#if DEBUG_WITH_PROFILER
            Instrumentor.Get().BeginSession(name, filepath);
#endif
        }
        public static void EndSession()
        {
#if DEBUG_WITH_PROFILER
            Instrumentor.Get().EndSession();
#endif
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static InstrumentationTimer Function()
        {
#if DEBUG_WITH_PROFILER
            var st = new StackTrace();
            var sf = st.GetFrame(1);
            string name = "???";
            if (sf is not null)
            {
                var method = sf.GetMethod();
                name = $"{method?.DeclaringType?.Name}::{method?.Name}";
            }

            InstrumentationTimer timer = new InstrumentationTimer(name);
            return timer;
#else
            return new InstrumentationTimer("???");
#endif
        }

        public static InstrumentationTimer Scope(string name)
        {
#if DEBUG_WITH_PROFILER
            var st = new StackTrace();
            var sf = st.GetFrame(1);
            string timerName = name;
            if (sf is not null)
            {
                var method = sf.GetMethod();
                name = $"{method?.DeclaringType?.Name}::{method?.Name}->{name}";
            }

            InstrumentationTimer timer = new InstrumentationTimer(name);
            return timer;
#else
            return new InstrumentationTimer(name);
#endif
        }
    }
}
