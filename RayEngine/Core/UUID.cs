using RayEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayEngine.Core
{
    public class UUID
    {
        public static readonly UUID Null = new UUID(0);

        private readonly ulong _UUID;

        public UUID()
        {
            _UUID = (ulong)RMath.Rand.NextInt64(long.MinValue, long.MaxValue);
        }

        public UUID(ulong uUID)
        {
            _UUID = uUID;
        }

        public static implicit operator ulong(UUID uuid) => uuid._UUID;

        public override int GetHashCode() => _UUID.GetHashCode();
        public override string ToString() => _UUID.ToString();
    }
}
