using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sanchez.AOC.Challenges._2021.Shared
{
    public class PacketData
    {
        public bool[] Input { get; }
        public int Position { get; set; }

        public PacketData(bool[] input)
        {
            Input = input;
            Position = 0;
        }

        public bool CanReadMore()
        {
            return Position < (Input.Length - 1);
        }

        public bool ReadBit()
        {
            return Input[Position++];
        }

        public bool[] ReadBits(int length)
        {
            var readEnd = Position + length;
            if (readEnd > Input.Length)
                throw new Exception("Oh no!");
            var res = Input[Position..readEnd];
            Position += length;
            return res;
        }

        public bool PeakBit()
        {
            return Input[Position];
        }

        public ulong ToNumber(IEnumerable<bool> s) =>
            (ulong)s.Reverse()
            .Select((c, i) => c ? Math.Pow(2, i) : 0)
            .Aggregate((a, b) => a + b);

        public int ReadInVersion() => (int)ToNumber(ReadBits(3));
        public int ReadInTypeId() => (int)ToNumber(ReadBits(3));

        IEnumerable<Packet> ReadChildrenPackets()
        {
            var lengthTypeId = ReadBit();
            if (lengthTypeId)
            {
                var noSubPackets = (int)ToNumber(ReadBits(11));
                var newPackets = Enumerable.Range(0, noSubPackets)
                    .Select(_ => ReadInPacket())
                    .ToArray();

                return newPackets;
            }
            else
            {
                var totalLengthInBits = (int)ToNumber(ReadBits(15));
                var newData = new PacketData(ReadBits(totalLengthInBits));
                var newPackets = newData.ReadInPackets().ToArray();

                return newPackets;
            }
        }

        public Packet ReadInPacket()
        {
            if (!CanReadMore())
                return null;

            var version = ReadInVersion();
            var typeId = ReadInTypeId();

            if (typeId == 4)
                return new LiteralPacket(version, typeId, this);

            var childrenPackets = ReadChildrenPackets().ToArray();

            if (typeId == 0)
                return new SumPacket(version, typeId, childrenPackets);
            if (typeId == 1)
                return new ProductPacket(version, typeId, childrenPackets);

            if (typeId == 2)
                return new MinimumPacket(version, typeId, childrenPackets);
            if (typeId == 3)
                return new MaximumPacket(version, typeId, childrenPackets);

            if (typeId == 5)
                return new GreaterThanPacket(version, typeId, childrenPackets);
            if (typeId == 6)
                return new LessThanPacket(version, typeId, childrenPackets);
            if (typeId == 7)
                return new EqualToPacket(version, typeId, childrenPackets);

            return null;
        }

        public IEnumerable<Packet> ReadInPackets()
        {
            var packets = new List<Packet>();
            while (CanReadMore())
            {
                var packet = ReadInPacket();
                if (packet == null)
                    break;
                packets.Add(packet);
            }
            return packets;
        }
    }
}
