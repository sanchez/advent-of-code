using System;
using System.Collections.Generic;

namespace Sanchez.AOC.Challenges._2021.Shared
{
    public class LessThanPacket : OperatorPacket
    {
        public LessThanPacket(int version, int typeId, IList<Packet> children) : base(version, typeId, children) { }

        public override ulong Solve()
        {
            var left = ChildrenPackets[0].Solve();
            var right = ChildrenPackets[1].Solve();
            return left < right ? 1u : 0u;
        }
    }
}
