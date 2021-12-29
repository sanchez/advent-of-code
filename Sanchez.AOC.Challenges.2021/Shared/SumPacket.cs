using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sanchez.AOC.Challenges._2021.Shared
{
    public class SumPacket : OperatorPacket
    {
        public SumPacket(int version, int typeId, IList<Packet> children) : base(version, typeId, children) { }

        public override ulong Solve()
        {
            return ChildrenPackets
                .Select(x => x.Solve())
                .Aggregate((a, b) => a + b);
        }
    }
}
