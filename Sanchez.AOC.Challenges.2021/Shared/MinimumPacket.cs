using System;
using System.Collections.Generic;
using System.Linq;

namespace Sanchez.AOC.Challenges._2021.Shared
{
    public class MinimumPacket : OperatorPacket
    {
        public MinimumPacket(int version, int typeId, IList<Packet> children) : base(version, typeId, children) { }

        public override ulong Solve()
        {
            return ChildrenPackets
                .Select(x => x.Solve())
                .Min();
        }
    }
}
