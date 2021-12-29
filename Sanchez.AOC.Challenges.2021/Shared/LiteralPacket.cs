using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sanchez.AOC.Challenges._2021.Shared
{
    [DebuggerDisplay("Literal Packet = {Value}")]
    public class LiteralPacket : Packet
    {
        public ulong Value { get; }

        public LiteralPacket(int version, int typeId, PacketData input) : base(version, typeId)
        {
            Value = ReadInLiteralGroupToNum(input);
        }

        public ulong ReadInLiteralGroupToNum(PacketData input) => input.ToNumber(ReadInLiteralGroup(input));
        public IEnumerable<bool> ReadInLiteralGroup(PacketData input)
        {
            var bits = new List<bool>();
            while (true)
            {
                var startingBit = input.ReadBit();

                var readBits = input.ReadBits(4);
                bits.AddRange(readBits);

                if (!startingBit)
                    break;
            }
            return bits;
        }

        public override ulong Solve()
        {
            return Value;
        }
    }
}
