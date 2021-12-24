using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2021
{
    internal class Day16
    {
        public void Run()
        {
            var lines = Input.Split("\r\n");
            Part2(lines);
        }

        public void Part1(string[] lines)
        {
            var input = lines[0];
            var bitArray = new BitArray(4 * input.Length);
            var startOffset = 0;
            foreach (var c in input)
            {
                var i = int.Parse(c.ToString(), System.Globalization.NumberStyles.HexNumber);
                bitArray[startOffset] = (i >> 3 & 1) == 1;
                bitArray[startOffset + 1] = (i >> 2 & 1) == 1;
                bitArray[startOffset + 2] = (i >> 1 & 1) == 1;
                bitArray[startOffset + 3] = (i & 1) == 1;
                startOffset += 4;
            }

            var packets = new List<Packet>();
            startOffset = 0;
            while (input.Length - startOffset >= 4)
            {
                var packet = new Packet();
                packets.Add(packet);
                startOffset = LoadPacket(packet, bitArray, startOffset, packets);
            }


            var versionSum = packets.Sum(x => x.Version);
            Console.WriteLine($"Read {packets.Count} with a version sum of {versionSum}");
        }

        int LoadPacket(Packet packet, BitArray bitArray, int startOffset, List<Packet> allPackets)
        {
            packet.Version = GetValue(bitArray, startOffset, 3);
            startOffset += 3;

            packet.TypeId = GetValue(bitArray, startOffset, 3);
            startOffset += 3;

            if (packet.TypeId == 4) // lteral value
            {
                var (literal, endOffset) = GetLiteral(bitArray, startOffset);
                startOffset = endOffset;
                packet.LiteralVal = literal;
            }
            else // operator
            {
                var lengthIdType = bitArray[startOffset++];
                if (lengthIdType)
                {
                    var numberOfSubPackets = GetValue(bitArray, startOffset, 11);
                    startOffset += 11;

                    for (int i = 0; i < numberOfSubPackets; i++)
                    {
                        var subPacket = new Packet();
                        subPacket.Parent = packet;
                        packet.SubPackets.Add(subPacket);
                        allPackets.Add(subPacket);

                        var endOffset = LoadPacket(subPacket, bitArray, startOffset, allPackets);

                        startOffset = endOffset;
                    }
                }
                else
                {
                    var subPacketLength = GetValue(bitArray, startOffset, 15);
                    startOffset += 15;

                    var readSubPacketBits = 0;
                    while (readSubPacketBits < subPacketLength)
                    {
                        var subPacket = new Packet();
                        subPacket.Parent = packet;
                        packet.SubPackets.Add(subPacket);
                        allPackets.Add(subPacket);

                        var endOffset = LoadPacket(subPacket, bitArray, startOffset, allPackets);

                        readSubPacketBits += endOffset - startOffset;
                        startOffset = endOffset;
                    }
                }
            }

            return startOffset;
        }

        (long literal, int endOffset) GetLiteral(BitArray bitArray, int startOffset)
        {
            long literal = 0;
            while (true)
            {
                var isFinal = bitArray[startOffset] == false;
                var nextPart = GetValue(bitArray, startOffset + 1, 4);
                literal = (literal << 4) + nextPart;
                startOffset += 5;
                if (isFinal)
                {
                    break;
                }
            }

            Console.WriteLine("Literal: {0}", literal);
            return (literal, startOffset);
        }

        long GetValue(BitArray bitArray, int startOffset, int numBits)
        {
            long val = 0;
            for (int i = 0; i < numBits; i++)
            {
                val = val +  ((bitArray[startOffset + i] ? 1 : 0) << (numBits - i - 1));
            }

            return val;
        }

        public void Part2(string[] lines)
        {
            var input = lines[0];
            var bitArray = new BitArray(4 * input.Length);
            var startOffset = 0;
            foreach (var c in input)
            {
                var i = int.Parse(c.ToString(), System.Globalization.NumberStyles.HexNumber);
                bitArray[startOffset] = (i >> 3 & 1) == 1;
                bitArray[startOffset + 1] = (i >> 2 & 1) == 1;
                bitArray[startOffset + 2] = (i >> 1 & 1) == 1;
                bitArray[startOffset + 3] = (i & 1) == 1;
                startOffset += 4;
            }

            var packets = new List<Packet>();
            startOffset = 0;
            var topPacket = new Packet();
            while (input.Length - startOffset >= 4)
            {
                packets.Add(topPacket);
                startOffset = LoadPacket(topPacket, bitArray, startOffset, packets);
            }

            var val = topPacket.GetValue();
            Console.WriteLine($"Value of transmition is: {val}");
        }

        class Packet
        {
            public Packet Parent { get; set; }

            public long Version { get; set; }

            public long TypeId { get; set; }

            public long LiteralVal { get; set; } = -1;

            public List<Packet> SubPackets { get; set; } = new List<Packet>();

            public long GetValue()
            {
                if (LiteralVal > -1)
                {
                    return LiteralVal;
                }

                long[] subValues = SubPackets.Select(x => x.GetValue()).ToArray();
                long val = 0;
                switch (TypeId)
                {
                    case 0: // sum
                        val = subValues.Sum(); 
                        break;
                    case 1: // product
                        val = 1;
                        foreach (var subVal in subValues)
                        {
                            val *= subVal;
                        }
                        break;
                    case 2: // min
                        val = subValues.Min();
                        break;
                    case 3: // max
                        val = subValues.Max();
                        break;
                    case 5: // greater than
                        val = subValues[0] > subValues[1] ? 1 : 0;
                        break;
                    case 6: // less than
                        val = subValues[0] < subValues[1] ? 1 : 0;
                        break;

                    case 7: // equal to
                        val = subValues[0] == subValues[1] ? 1 : 0;
                        break;
                }

                return val;
            }
        }

        #region TestInput

        public string TestInput =
            @"CE00C43D881120";

        #endregion

        #region Input

        public string Input =
            @"220D62004EF14266BBC5AB7A824C9C1802B360760094CE7601339D8347E20020264D0804CA95C33E006EA00085C678F31B80010B88319E1A1802D8010D4BC268927FF5EFE7B9C94D0C80281A00552549A7F12239C0892A04C99E1803D280F3819284A801B4CCDDAE6754FC6A7D2F89538510265A3097BDF0530057401394AEA2E33EC127EC3010060529A18B00467B7ABEE992B8DD2BA8D292537006276376799BCFBA4793CFF379D75CA1AA001B11DE6428402693BEBF3CC94A314A73B084A21739B98000010338D0A004CF4DCA4DEC80488F004C0010A83D1D2278803D1722F45F94F9F98029371ED7CFDE0084953B0AD7C633D2FF070C013B004663DA857C4523384F9F5F9495C280050B300660DC3B87040084C2088311C8010C84F1621F080513AC910676A651664698DF62EA401934B0E6003E3396B5BBCCC9921C18034200FC608E9094401C8891A234080330EE31C643004380296998F2DECA6CCC796F65224B5EBBD0003EF3D05A92CE6B1B2B18023E00BCABB4DA84BCC0480302D0056465612919584662F46F3004B401600042E1044D89C200CC4E8B916610B80252B6C2FCCE608860144E99CD244F3C44C983820040E59E654FA6A59A8498025234A471ED629B31D004A4792B54767EBDCD2272A014CC525D21835279FAD49934EDD45802F294ECDAE4BB586207D2C510C8802AC958DA84B400804E314E31080352AA938F13F24E9A8089804B24B53C872E0D24A92D7E0E2019C68061A901706A00720148C404CA08018A0051801000399B00D02A004000A8C402482801E200530058AC010BA8018C00694D4FA2640243CEA7D8028000844648D91A4001088950462BC2E600216607480522B00540010C84914E1E0002111F21143B9BFD6D9513005A4F9FC60AB40109CBB34E5D89C02C82F34413D59EA57279A42958B51006A13E8F60094EF81E66D0E737AE08";

        #endregion 
    }
}
