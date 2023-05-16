using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayEngine.Core
{
    public static class Time
    {
        public static double GetTime()
        {
            return Raylib.GetTime();
        }
    }

    public class Timestep
    {
        private double Time;

        public Timestep(double time = 0.0)
        {
            Time = time;
        }

        public static implicit operator double(Timestep ts) => ts.Time;
        public static implicit operator Timestep(double time) => new Timestep(time);

        public double GetSeconds() => Time;
        public double GetMilliseconds() => Time * 1000.0;
    }
}
