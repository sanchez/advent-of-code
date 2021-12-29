using System;
namespace Sanchez.AOC.Challenges._2021.Shared
{
    public abstract class Packet
    {
        public int Version { get; }
        public int TypeId { get; }

        public Packet(int version, int typeId)
        {
            Version = version;
            TypeId = typeId;
        }

        public abstract ulong Solve();
    }
}
