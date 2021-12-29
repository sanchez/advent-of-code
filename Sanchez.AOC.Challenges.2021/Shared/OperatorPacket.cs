using System;
using System.Collections;
using System.Collections.Generic;

namespace Sanchez.AOC.Challenges._2021.Shared
{
    public abstract class OperatorPacket : Packet
    {
        public IList<Packet> ChildrenPackets { get; }

        public OperatorPacket(int version, int typeId, IList<Packet> childrenPackets) : base(version, typeId)
        {
            ChildrenPackets = childrenPackets;
        }
    }
}
