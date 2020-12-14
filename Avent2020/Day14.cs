﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Avent2020
{
    class Day14
    {

        public void Run()
        {
            Part2();
        }

        Regex regMask = new Regex(@"mask = ([X01]*)");
        Regex regMem = new Regex(@"mem\[(\d+)\] = (\d+)");

        void Part1()
        {
            var input = Input;
            var lines = input.Split("\r\n");
            var addressDict = new Dictionary<long, long>();
            var mask = "";
            foreach (var line in lines)
            {
                var matchRegMask = regMask.Match(line);
                if (matchRegMask.Success)
                {
                    mask = matchRegMask.Groups[1].Value;
                    continue;
                }

                var matchRegMem = regMem.Match(line);
                if (!matchRegMem.Success)
                    throw new Exception($"No match for line: {line}");

                var address = long.Parse(matchRegMem.Groups[1].Value);
                var val = long.Parse(matchRegMem.Groups[2].Value);
                var charIndex = mask.Length - 1;
                while (charIndex >= 0)
                {
                    var bitIndx = mask.Length - charIndex - 1;
                    switch (mask[charIndex])
                    {
                        case 'X':
                            break;
                        case '0':
                            val &= ~((long)1 << bitIndx);
                            break;
                        case '1':
                            val |= (long)1 << bitIndx;
                            break;
                    }
                    charIndex--;
                }

                //Console.WriteLine($"value at address: {address} is {val}.");
                if (addressDict.ContainsKey(address))
                {
                    Console.WriteLine($"Overwriting value at address: {address}");
                }
                addressDict[address] = val;
            }

            long sum = 0;
            foreach (var val in addressDict.Values)
            {
                sum += val;
            }
            Console.WriteLine($"Sum of all values is: {sum}");

        }

        void Part2()
        {
            var input = Input;
            var lines = input.Split("\r\n");
            var addressDict = new Dictionary<long, long>();
            var mask = "";
            foreach (var line in lines)
            {
                var matchRegMask = regMask.Match(line);
                if (matchRegMask.Success)
                {
                    mask = matchRegMask.Groups[1].Value;
                    continue;
                }

                var matchRegMem = regMem.Match(line);
                if (!matchRegMem.Success)
                    throw new Exception($"No match for line: {line}");

                var address = long.Parse(matchRegMem.Groups[1].Value);
                var val = long.Parse(matchRegMem.Groups[2].Value);
                SetAddress(mask, address, val, 0, addressDict);                
            }

            long sum = 0;
            foreach (var val in addressDict.Values)
            {
                sum += val;
            }
            Console.WriteLine($"Sum of all values is: {sum}");

        }

        void SetAddress(string mask, long address, long value, int maskIndex, Dictionary<long, long> addressDict)
        {
            if (maskIndex == mask.Length)
            {
                addressDict[address] = value;
                return;
            }

            var shiftIndex = (mask.Length - maskIndex - 1);
            var addr0 = address & ~((long)1 << shiftIndex);
            var addr1 = address | ((long)1 << shiftIndex);
            switch (mask[maskIndex])
            {
                case 'X':                    
                    SetAddress(mask, addr0, value, maskIndex + 1, addressDict);                    
                    SetAddress(mask, addr1, value, maskIndex + 1, addressDict);
                    break;
                case '0':
                    SetAddress(mask, address, value, maskIndex + 1, addressDict);
                    break;
                case '1':
                    SetAddress(mask, addr1, value, maskIndex + 1, addressDict);
                    break;
            }
        }

        string TestInput1 = @"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0";

        string TestInput2 = @"mask = 000000000000000000000000000000X1001X
mem[42] = 100
mask = 00000000000000000000000000000000X0XX
mem[26] = 1";

        string Input = @"mask = X1011100000X111X01001000001110X00000
mem[4616] = 8311689
mem[8936] = 240
mem[58007] = 369724
mask = 10X0111X01X0XX110X10100X1001X000010X
mem[41137] = 232605
mem[33757] = 1437435
mask = X0011110100X011X01000X1000X1000X0100
mem[12518] = 27521105
mem[25203] = 3177
mem[33089] = 636515
mem[39100] = 494341
mem[59321] = 16252
mem[55061] = 1075
mask = 1000X11X01X0X01111XX10110001X1110111
mem[16129] = 15646
mem[35714] = 30634
mem[14485] = 6847
mask = 10011110110000X10X0X1010XX1010100X00
mem[2308] = 704945037
mem[41472] = 19000
mem[34542] = 334200
mask = 11011X00X000001X0X00XX000011X001000X
mem[49552] = 397346
mem[52482] = 244619
mem[40387] = 339372
mem[24387] = 1184014
mask = 1000101XXXX100X1X1X10001X01000111101
mem[42901] = 58252697
mem[3604] = 2970
mem[38806] = 78120216
mask = 1XX01X1111000010011101001X1110XXXX11
mem[16713] = 3710
mem[47208] = 490
mem[2864] = 835240
mem[30001] = 92649470
mask = 1X1101XX0XX1X011010X1000100011010X01
mem[12306] = 9094
mem[50276] = 737
mem[44950] = 312069
mask = 100X11101X00001X010X11XX00011010011X
mem[9068] = 2331
mem[34800] = 29860461
mem[18114] = 4912
mem[38010] = 2912
mem[59634] = 2460
mem[18826] = 3632988
mem[24839] = 87354329
mask = 0100X1101X01001101010X11000XX01000X0
mem[45994] = 1032
mem[15104] = 353
mem[43012] = 3302
mem[3101] = 3195
mem[58848] = 504541
mem[48036] = 3990
mask = 101011111100X010010X0X0X110111X10110
mem[55860] = 425881800
mem[49212] = 127188344
mem[16320] = 45285651
mask = 10X01110010X000X11000001000X0010X100
mem[10403] = 4668
mem[64] = 79964
mem[52226] = 263
mask = 1001X110110X0011010110001X01X0X01100
mem[13086] = 15318901
mem[14412] = 75226298
mask = XX0011XX010000X11100000100010X1X0100
mem[23984] = 1042686
mem[43191] = 960403
mem[304] = 17453
mem[43446] = 312310
mem[3612] = 7039
mem[24387] = 1076710
mem[35072] = 35503
mask = 10001X10110X0XXX01010101X00110000X0X
mem[40240] = 74221428
mem[19881] = 1055
mem[61105] = 2705392
mem[36207] = 117170
mem[34315] = 1069384
mem[24067] = 9021444
mask = 100X1X1111000001X0010XX11X01101001X0
mem[33794] = 809
mem[62607] = 265070
mem[59634] = 49282
mem[61442] = 364
mem[37003] = 628813
mem[7946] = 644992
mask = 1101X1000X0X001111X00111000100101001
mem[31631] = 5264178
mem[54754] = 1554399
mem[23468] = 4729
mask = 000111XX10X0001101X0X1001010X1XXX110
mem[50043] = 4139464
mem[49775] = 61509506
mem[53862] = 156448
mem[13334] = 87
mem[18854] = 9282
mem[34330] = 30468246
mask = 00011110100X00X1X1X000X11001101001X0
mem[31487] = 906990
mem[52518] = 305801
mem[49859] = 295219
mem[31776] = 1816407
mem[1947] = 7357561
mem[46508] = 4130
mem[17050] = 30862
mask = 10001110110X000X0X01X10X00111000101X
mem[9293] = 1384
mem[38283] = 2143609
mem[34969] = 5179
mem[63542] = 11640
mem[27523] = 1345
mask = 100111X0000000X1X10000100010100000X0
mem[10736] = 216826655
mem[45201] = 17886838
mem[10901] = 638
mem[32804] = 195074
mask = 1101X1X0XX00111101X0000111111X110001
mem[39181] = 416963
mem[9249] = 23378
mem[31817] = 28320828
mem[2935] = 456520
mem[13116] = 502217
mem[43105] = 1629110
mask = 1XXX111X1100X01X010110001X01101X1100
mem[50732] = 3451
mem[15501] = 841008
mem[17518] = 59713011
mem[20719] = 597200
mem[304] = 4690251
mem[54185] = 1478
mem[64845] = 15752175
mask = 110111000000X0X00X001X111000X1100010
mem[8552] = 30412318
mem[13116] = 1108685
mem[6392] = 779855579
mem[37617] = 3473324
mem[51729] = 51848
mem[57424] = 23802
mem[15479] = 17
mask = X00111101XX001111010011110X001000X01
mem[25195] = 129734
mem[11914] = 2701
mem[2864] = 25833
mask = 1001111X11X10XXX0X00101010110X00X001
mem[41424] = 106207900
mem[48133] = 43924223
mem[49702] = 179742
mask = 10011X111101000X00XX1X0X101110001001
mem[34101] = 640437195
mem[41745] = 203723
mask = 110111X011011011X100101X10100100000X
mem[348] = 627305
mem[12497] = 7501
mem[805] = 3988
mem[60636] = 184971943
mask = 100111001000001X0100X10011X00001XXX0
mem[50944] = 202043
mem[34140] = 15663180
mem[2200] = 1479
mem[27465] = 82898391
mask = 000X11111X10001101X00X01101X0001011X
mem[37944] = 6895727
mem[61573] = 409349871
mem[50342] = 61513
mem[13583] = 22407
mem[59968] = 102419
mem[2136] = 114207754
mem[30421] = 997
mask = 100111101X0X0011010X1XX01XX1X010010X
mem[13083] = 207
mem[10383] = 31056
mem[59321] = 402034
mem[22966] = 20228
mem[23372] = 779
mem[25895] = 1614
mem[11288] = 3047
mask = 1101110000000001XX0X111X1001XXX1110X
mem[31200] = 6875882
mem[26216] = 947
mem[5932] = 72207364
mem[64126] = 1260
mem[24890] = 3898
mem[61396] = 2240803
mask = 10001X101X0XX0110X01010011011X001110
mem[27866] = 740
mem[19123] = 32418
mem[23286] = 13542
mem[50342] = 34812801
mem[19362] = 12615968
mem[23756] = 107442819
mask = 100X111X1100X11X0X0X000X1X1001110110
mem[2772] = 3055
mem[8755] = 581
mem[60512] = 906058
mem[22390] = 3925820
mem[15876] = 280704
mem[40784] = 1550
mask = X000101X1100001011X10001000110010X00
mem[182] = 466813
mem[15844] = 21827
mem[4813] = 19124
mem[49362] = 2261
mask = 11011X00110X1X10010X0X110111101010XX
mem[37402] = 839
mem[35835] = 3289
mem[21867] = 10856
mem[32498] = 1600115
mem[35408] = 547736
mask = 1111110000010X1X11X00100X0X110101X01
mem[1140] = 1767723
mem[18180] = 11108349
mem[19129] = 115
mem[7955] = 103275871
mask = 1000101X1X00000X0XX1100001X11X1100X0
mem[39668] = 7209001
mem[57657] = 873105
mem[21665] = 441551
mem[20895] = 390
mem[42377] = 98696816
mem[38513] = 18489
mask = 1001101X110X000XX001110X10011X10110X
mem[63570] = 15065690
mem[3660] = 49298
mem[27417] = 427585
mem[37005] = 58080842
mem[21145] = 45587769
mem[59405] = 235561467
mask = 1XX100000000X001010X0001010X00010010
mem[61696] = 13179282
mem[60388] = 14504403
mem[54528] = 139910709
mem[10543] = 2877271
mem[44950] = 1250
mask = 100111100X000111110000X0000X00X001X1
mem[34901] = 4089
mem[7491] = 4875619
mem[30899] = 358791983
mem[63542] = 48357
mem[55865] = 289
mask = XX0X1110100100X10100010010XX00100100
mem[1939] = 9240
mem[13700] = 884
mem[56638] = 5690182
mem[38174] = 2173
mem[52518] = 54887
mask = 1X001X111100X0X10X01XX001X11101X1100
mem[13834] = 15183481
mem[65481] = 29968
mem[23863] = 467501
mem[19360] = 167115
mem[7268] = 8208487
mem[14485] = 158
mask = 100111101100001XX1011000001X0X100000
mem[8868] = 484089989
mem[53350] = 4089002
mem[57545] = 6025444
mem[28491] = 45516050
mask = 1000111XX10000X00111X101X0X010101XXX
mem[39822] = 451680949
mem[57061] = 370534242
mask = 10001X10110X0010X111XX0100X11010X11X
mem[16975] = 239223476
mem[21172] = 15004
mem[14883] = 71129165
mask = 10X0X1111100X0100111010101X110000110
mem[20805] = 1015445496
mem[14229] = 217524
mem[40036] = 2501
mem[41119] = 17208160
mem[56826] = 349928
mem[57555] = 884
mask = 000X11101000001101XX1000000000100X00
mem[35523] = 32624
mem[36012] = 9499537
mem[38756] = 421
mem[14024] = 190459520
mem[13308] = 1286846
mem[37150] = 4205580
mask = 1X01111011000X1X0100100X011001110100
mem[40710] = 2279
mem[34963] = 2267645
mem[12759] = 643
mem[17704] = 5597582
mem[30411] = 351996998
mem[16505] = 272444
mask = 10X111101X010X1X0X0010011001000X0110
mem[39356] = 22255
mem[48815] = 1638
mem[34690] = 420690
mem[16320] = 104253
mem[53422] = 477
mask = 100011X0XX01XX10111111010X0010011111
mem[54065] = 24449
mem[10993] = 12705817
mem[47121] = 284443
mem[36383] = 512188
mem[18814] = 159439379
mem[17770] = 99104
mask = 110111000X000X010101X11X101X00000X11
mem[51615] = 735360
mem[62469] = 15939
mask = 100X1110XX000X11X100100X00000X00X10X
mem[14772] = 38635
mem[29236] = 13420
mem[62613] = 47790
mem[39212] = 167702
mem[8936] = 106407539
mem[38934] = 332
mask = 0X00111010010011X10X0X0XX001X010000X
mem[63621] = 996
mem[56538] = 52673058
mem[37764] = 995346
mem[32132] = 103636989
mem[15733] = 13086
mem[44188] = 194198
mask = 0101010010000011X11010011X1X0X101X01
mem[46447] = 784
mem[13324] = 3401
mem[22200] = 165702
mem[7955] = 649
mem[15844] = 2690647
mem[64010] = 1096
mask = 11X1X1X001X11X11010XX01000X011010010
mem[24806] = 1221
mem[53291] = 1348383
mem[55875] = 25945782
mem[4492] = 1904
mem[59333] = 2401690
mem[47121] = 1839078
mask = 10011110X10100X1X10X0010110010110110
mem[40541] = 11787096
mem[62086] = 2967
mem[28799] = 4008988
mem[64917] = 268674
mem[18114] = 2655847
mem[45757] = 57333538
mem[44645] = 644597
mask = X000111000000X1111XX10X1000X0111X100
mem[10448] = 1060
mem[31631] = 77787880
mem[42148] = 6070584
mem[57082] = 485245396
mem[52482] = 21478
mask = 100XX11011000X11010X0010X00X10X0110X
mem[2017] = 14320
mem[3101] = 714880
mem[33316] = 105398
mem[28361] = 127078137
mem[23616] = 10309
mem[16594] = 13326311
mask = X001111001X0XX1101X01011X0110X1X1100
mem[43153] = 29487947
mem[46646] = 3335247
mem[17123] = 27270587
mem[5225] = 16921674
mem[30789] = 418871729
mask = 110X011010001111011000X111011XXX0100
mem[10985] = 214477
mem[13116] = 27310
mem[5115] = 1416
mem[12596] = 453023
mem[11737] = 24916
mem[14311] = 6470
mem[10361] = 300
mask = 000X11XX100000110110111110100X1101XX
mem[22040] = 1941817
mem[38002] = 7734399
mem[48736] = 31798852
mem[47426] = 204659941
mem[11288] = 15341
mask = 10X10X000001001X1100X010X011100X1001
mem[42377] = 129247876
mem[27828] = 50890049
mask = 10001110010000X1110010101011X101X01X
mem[57528] = 3471
mem[23114] = 96086679
mem[8446] = 38698
mem[14844] = 792092
mem[42506] = 31779
mem[64009] = 10029
mem[42731] = 172808
mask = 1101XX000000X0X1010010000001000X00XX
mem[426] = 11039
mem[23258] = 556641472
mem[51919] = 15137974
mem[21761] = 530
mem[21377] = 850024656
mem[59501] = 531065557
mask = 10001X10010X0X1101X00010100X1011X001
mem[43269] = 9515743
mem[426] = 5602
mem[2308] = 381075
mem[57933] = 56242190
mem[48133] = 597263
mask = 10X111X010X10011X1011000100100111100
mem[53350] = 383807
mem[37410] = 1044
mem[62086] = 14451
mask = X001111010XXX01101001001XX1001101000
mem[12756] = 551
mem[1027] = 656489115
mem[60702] = 336416
mem[42396] = 60622
mem[3436] = 1377859
mem[17518] = 1451
mask = 10011110100X00110100100X000XX00001XX
mem[42037] = 79130078
mem[34850] = 3303
mask = XX011X1010010011010110000X0100100X1X
mem[30581] = 949124
mem[18437] = 1990679
mem[2340] = 174075070
mem[46975] = 5827
mem[9421] = 107926
mem[28836] = 6636869
mask = 1X01X100X00X001XX1000X0X000110X00001
mem[53904] = 2238757
mem[22157] = 13388
mem[59321] = 31451373
mem[31277] = 110705865
mask = 100X1111X100001101X1X11100X010101101
mem[64929] = 5596232
mem[42822] = 12360220
mem[49391] = 650
mem[527] = 1337719
mem[56173] = 44948559
mem[64845] = 51705729
mask = 100X11X0X10X0XX101000000101X0010X11X
mem[63581] = 62572875
mem[52795] = 23120259
mem[38228] = 737964538
mem[45105] = 480
mem[33059] = 273619504
mem[60636] = 73
mask = 1X0111X01100001101001X00X00000100110
mem[2592] = 114883
mem[61691] = 2066
mem[51873] = 208
mask = 110X100X110X10XX1100101XX01X10111X01
mem[28341] = 144281151
mem[36729] = 56472321
mem[63409] = 10778
mem[21172] = 63301
mem[1101] = 3785
mask = 100111100X00001101X01X00001110X01XX1
mem[36813] = 990295316
mem[44821] = 179862368
mem[45561] = 12722
mem[54859] = 19017266
mem[50564] = 167811030
mem[43704] = 126046154
mask = 10001X10X1000011X1000X00100100110010
mem[42747] = 1659054
mem[10403] = 568236
mem[45045] = 43419
mem[31809] = 59431
mem[49382] = 16398
mem[56638] = 387921470
mask = 11X1X1000001X01011X0X1X0XX1010001001
mem[15844] = 133202
mem[59321] = 81765
mem[19566] = 8456988
mem[51120] = 1362
mask = 1001X11XX0000011X10011100X0010000001
mem[49173] = 1284
mem[3884] = 8094
mem[32207] = 12943
mem[32916] = 43847
mem[45] = 78615226
mem[47659] = 4559
mask = 10011X101X000111X0X000101010001X0X01
mem[47504] = 3997682
mem[630] = 49259614
mem[23114] = 688
mem[62188] = 12822166
mem[65077] = 459034
mask = X101010XX000X01101X010X0011100101000
mem[26001] = 1543
mem[48133] = 288946
mem[53422] = 18943982
mask = 1X00XXX1110X00110101X0111100X010000X
mem[33738] = 196926637
mem[37664] = 623
mem[48029] = 5133
mem[23732] = 23735
mem[30329] = 212916554
mem[8552] = 119946627
mem[6631] = 1907
mask = 1XX11100X000001X01X01100X10X000100X0
mem[1703] = 775689020
mem[2780] = 3545
mem[2818] = 234302
mask = 1001X1XX100000110100000001X0001X0101
mem[61417] = 120523
mem[13083] = 2308
mem[32789] = 3997976
mem[50116] = 778868811
mem[8936] = 61495467
mem[12206] = 208
mask = 100111X0X00000X1010000100000001011XX
mem[9421] = 2972812
mem[34256] = 2423170
mem[36762] = 36668
mem[19241] = 64537
mem[31618] = 1055988588
mask = X00X101111000X0100010XX0X111X01010X0
mem[49552] = 27782759
mem[9251] = 4816
mem[14980] = 3017
mask = X0X10000X000X00X01X1010111111X11X110
mem[40421] = 51130
mem[60996] = 34278
mem[64961] = 414
mask = 1000111X1100001001X1X10XX0010X0XXX00
mem[52359] = 763212311
mem[44171] = 9117448
mem[49702] = 8926626
mem[2466] = 2011885
mem[10736] = 383225
mem[38555] = 14408818
mem[19362] = 677100
mask = 100011X1110XX010011X1100X001X001X11X
mem[60702] = 1681285
mem[33941] = 249
mem[33060] = 48065
mem[6660] = 11635
mask = 10X1111X1X000011010X1X0XX10000110X00
mem[33592] = 7818
mem[20643] = 5562857
mem[61417] = 482
mem[25002] = 18611
mask = 1X011000110010X0X100101XX111X1X100X1
mem[15042] = 1048663
mem[46450] = 906445926
mem[65481] = 325
mem[8090] = 145570
mem[46447] = 2513
mask = 11X101X01X00XX11X10000000X11X00X0000
mem[9686] = 6175651
mem[55574] = 42723
mem[37505] = 2089
mem[64151] = 6581
mem[24387] = 3109
mem[56931] = 0
mask = 11011100XX0X1X1X010010010X1X10X000X0
mem[42506] = 1351
mem[15066] = 30271
mem[426] = 4269668
mem[20864] = 120804232
mem[14675] = 1430376
mask = X00X111X110000110100X000101100101000
mem[16379] = 14955
mem[63542] = 294779946
mem[43782] = 14023312
mem[50116] = 68098604
mem[6346] = 2828888
mask = 1XX0X1100110XX11X0101011100000100100
mem[27465] = 802232
mem[46780] = 18542342
mem[19044] = 4213
mem[39202] = 76554568
mem[33173] = 68587
mask = 10X1111111110101X100001010X1000001X1
mem[57043] = 971606118
mem[46661] = 8089
mem[38403] = 32188546
mem[18704] = 630312774
mem[16594] = 54138487
mask = 1XX1100X00X0001001000000000X000100X1
mem[33536] = 5526445
mem[3755] = 929
mem[5733] = 77809
mem[14065] = 15212249
mem[34315] = 1058941
mask = X00111101X000011010XX0X110100X01X00X
mem[33910] = 15336
mem[29561] = 16371540
mem[10901] = 273
mask = 100011X101000001110X01001X0011X001X0
mem[23567] = 675387
mem[7604] = 2997382
mem[16594] = 858806
mem[47216] = 549
mem[15386] = 126092";
    }    
}
