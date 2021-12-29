using System;
using System.Collections.Generic;
using System.Linq;

namespace Sanchez.AOC.Challenges._2021.Shared
{
    public class ProductPacket : OperatorPacket
    {
        public ProductPacket(int version, int typeId, IList<Packet> children) : base(version, typeId, children) { }

        public override ulong Solve()
        {
            return ChildrenPackets
                .Select(x => x.Solve())
                .Aggregate((a, b) => a * b);
        }
    }
}
