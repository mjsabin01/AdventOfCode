﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avent2022;

internal class Day03
{
    public void Run()
    {
        var lines = Input.Split("\r\n");
        Part2(lines);
    }

    public void Part1(string[] lines)
    {
        var total = 0;
        foreach (var line in lines) 
        {   
            int partLength = line.Length / 2;
            var part1 = line.Substring(0, partLength);
            var part2 = line.Substring(partLength);

            var part1Chars = new HashSet<char>();
            foreach (var c in part1) 
            {
                part1Chars.Add(c);
            }

            var commonChar = part2.First(part1Chars.Contains);
            total += GetPriority(commonChar);
        }

        Console.WriteLine($"The total of the commmon chars is: {total}");
    }

    int GetPriority(char c)
    {
        if (c >= 'a' && c <= 'z')
            return c - 'a' + 1;
        return c - 'A' + 27;
    }

    public void Part2(string[] lines)
    {
        var total = 0;
        for (int groupIndex = 0; groupIndex < lines.Length / 3; groupIndex++)
        {
            var part1 = lines[groupIndex * 3].ToHashSet();
            var part2 = lines[groupIndex * 3 + 1].ToHashSet();
            var part3 = lines[groupIndex * 3 + 2].ToHashSet();

            var common = part1.Intersect(part2).Intersect(part3).First();
            var val = GetPriority(common);
            Console.WriteLine($"Common char in group {groupIndex} is {common}, with a value of {val}.");
            total += val;
        }

        Console.WriteLine($"Total of groups is: {total}");
    }

    #region TestInput

    public string TestInput =
        @"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw";

    #endregion

    #region Input

    public string Input =
        @"ZNNvFWHqLNPZHHqPTHHnTGBhrrpjvmwfMmpfpjBjwpmw
sbdzQgzgssgbglRtmjlwhjBlfrSrMt
zgsCRzJbsdRVQCDbcgLGWWLnZNGVLLZMNZnq
tvHhRtZGMvMHvfsrBBCTRbwbccRc
qznnlpzzDppWlDpQpCrcrwnBNwTZnBTZrn
PdVZJJqVZdllDPFtMjMgLjGMHvSgMF
csbhhVDDvzlVDcbccGGvfRjDHCjNLRHRCLfmnZfR
dFrStSTTmrrrHVfV
MMgQMMTMVTdgWtwTPwSgWSgGbbppJzlplvhBlPbzhlhbzG
FDJSTtSGhpPFDmFTZDpTFPmCBBrHqsCBhgBlqqrqrlRrHH
dQwMtfdzVwWfwctwnfnQCHllzRrsNzrrgNlCgqsr
fLfQnVjfwQfMdfvfnVvWDvtJPFGDpvZGbZpmbSPP
TzzCrJcDrTDdLDCJDvGNPCFqlZWlvNvWpq
RRHfjsQBFsjgjBQsWqGpNvZQqQlPPQPN
VnHBnRVssnnjsSfBwbMSrrbTwJTcwSDF
HJCgHCCFFFVGJWTlbqDdlqTDDpgl
cZccSmLrfZcrmmzSQftdpDtTHdbQTDMQ
NZZccrrBwZRPNNzmcLSSjJhGhVWCnsFnHBjGChsJ
qwwwJHTHqdFDtZBFPfFBZFzM
gVRcLnnWVgggnnnQgVWWNZtZrBfLBzZzBrMPPrZvPv
GQgQSVRtsVnNRGSCdpmwspmbmDpHmhwd
bhNgNfgwpbLMhCZMGQBmDm
FrcHrSllcqcFFMGLBDQlMDTGlT
FVSddRSJRjLwbjJPJw
wzhhrTwwTrSsdHQjjSHnBjQj
gRDCmVgRgMvtMfVMRBBBhWCHQQHGJHZJQZ
NtgVgttVbMNmvsNlpcrLhLTNPw
MCgjsfnscgjjgnGgJHHqHDgdHbGr
QSSmRFPpRtPFQLQRmPzvBzzzDWqrqWWHJGGNrJJbdtVWHJDV
BdSFdLQzRFlSLmQplffwncfscChhcsMj
GfVmfnmJVnNVFhnhGmbmhpHvqjrzHZBjfvrtBHHZrwBt
ddWQldlMdWMlQsLWTLQgMNwBrvjrZjNrwzZjswHqrv
QQdTRcgTRPDlMQlQPQdhcNNnbJmbGpVnGchFmm
CjjZCCZfvWZRHHhRtwhvPN
mrnqlqMqBlSSLnBTLBwmHPPWhPPHtFRPWzwt
rBVTrrMMSMLQBrndGcddWQbbdZfCZJ
LFtdjHjLjLqHqstLTjFLFqNMnMhhZdDDNMVbWdDDbhnZ
CrBpBGnzrzmczcllrphCZZWJMDWRbbZNMDMR
GwgvzpzvrcmBrnfHjTgqTsgHjF
rMPPZcplCZlZPwtSwhtBwCQQzB
FvDGffLqqmQFwmmhzt
TjJjJfHHVDVnHVgZZlQppcVscP
hVcqHwhgwwwjHjjGWbvrbBGrsWVWGn
CttPRpMmPDTWbWltlLBnGl
pZmDFMmPMfnZwqqwfcqJdHgz
bSJWhWJCbGGWJPStWTgRQwzDjgQQjsDW
nFBBVQVrVBrNFMFZVpBBZFZrDgdTldgsRsslsljsRzTRjzns
rMcZcHcBQPvbbHGP
mSfmwqfmzrfHwFfmrwvPHqPmMFRlMDDZBCVVRCVZVlZpMRRR
TWjdTWhTsssLTGsJNWhTQddjRMDMtNNBSCDBllMMBVtDMVRZ
QhWTQcdhjThsdGbTLGjWHmffnmHwnwHrwqmmfcwS
LmrsMQnnpfmMLllvTvqvFFzvFHNN
WGRFVWdwZWZvCbJzcvJNzw
VjGhDtWGSFRGjVVSFdjjDPBfspPnnMBLPLrrpMMm
qqqCCJjtqtqCtqLZspHWBdSrWWSzzbzHFWBldb
GhwwcwPFVDcNFRRGwwzmlBrBWvllrvSzlrcd
DGGhQNNDhTpZZqqLQFQQ
QfZmgQQZCCMLfNrgprdNvvdrTg
hhttsBmBDcFRBlJshJcRrnjnTvNqpddNNqvndp
JtsGJGtGGJJJHDbctllhZHmMwMQSPVPzHSLMPZmV
DScSjZcNBZqjDDcLLfFtPfCfjfPvfv
pTmRlWhdMwTLGwCf
mRdWCVVglWrCmVHVrVCmdrbSzNcBDBqBZDNHqssscNzqNc
sPMHGFMsrPNCPnNS
ffJzllbzpZBllttBtfglgBTbSCVCmmrNFmmbFNvCFLLb
cpZqpfgZZJtJqJJJfWHWhHdHWHjcdRdFHD
ZZPfppvzMrlNBFcvFB
shJgstJwWLVJwcrFFVFrBVNNqFFB
HwWJdLHWWLcQgssHwwSQSQtQzCnZZMpZCmdzZCzpPzpCPRCj
QCpLRbsCCQQLbQzCBQDQBBfTTffWtTctJVRNVtnfwtWV
GvlqqlGlmMrdsvrhmlcTvwJtwNwTvfJfcWTW
lMhgqGhddjqFFCzBBpbsSQpD
JJwGJwVQQwVSsSMhQMQgHfgfTtrrfVTNgNNfrt
dFDWCDdFppvDFmWWWnJTPllHmHlgrqrgggtH
DzFbWjdRpbdFCjjRbnFbQBGhhQBBJZwMhScwZwJz
HttvHpHmpJWtHmFNvlvdMSVdPMtLVCCMMMfcfL
GjgzhGSGSSdCcRMVjMdc
QshbnghgnGDnqsFrNSJFrsNs
wJpjMwzjzdVbzPPVpbCHnqGnBqnsBrNCwgrC
ftTLLDTQtLTGTGtFrgHrvqgQnrvQsCHH
fTcFFfLSfFFcGFllcFhPJPjWWJSjSWzMWPdS
ZjNdmjVQVZmvNNZNNZHWZmWtsJnwTpJJswpWwGqJhJqGpp
FcRRcDblDMLRcRMLFFMDGsJnqhwpqTTJGwnsfnlp
LRBrcLbbgLFgBbFqDvdHQvCCjNzzzVrZdV
BdbLWrgdvgWvVJgWnDfNhVnqhCCpDpcq
tSQPSTSGPMmlMPtQQPJGtGQRCcnqqfnRhCcChDqnCfRScf
jTssPsjMQMmszPjlTtsJdFBFrJzrbJdHZFHdWH
vCccctvvTTtZcgLGcZTbssbMWnpMpmLWqnNjpfPPfPjMPp
wwBBlRBBwDDVFRhFlRhdRRVWPnnpMpffmmffrpWqVNPm
ddhddRzHlQHFJcGsCztTgbNzST
fJctfpVWcnfRLfrRwP
vmmnvDQDZTNTmGGTqTMTvMqwBdLjBvRzBRrRBRLjjBPzBB
GMnmqSTFFQqttcbcJWgsSt
rHNfmfRsmfRGfDNcRmcmMQlLCGSnQwwPPCSnzQlSCl
bsJTBsVhFsVpqFWFgPCwnQwBZzwQzZLlzn
qggTTqvqgqbbTTFqVqgWqvNmmMMRdffftNfMDMmscR
rFWQFszrwjsjFWvshPTCmLZLSTLwSLlgSP
BQbcqVHNVqVpVpmClJgJJHSmZLJm
qBNNNVdDMGBpDcDWsvdQsFrFnjttfj
qGhmttmzhtMvhbrLdSHbdSHRzb
WCBgQJJpjCQlgdHZrfPRPSRbNg
jBTTDjlnjnJDJTQCVntcwtwMSvqcGFDhcvsh
ZTrnTqMWWWnfrddMGJPgPLlPbw
VvmGRVpBpNNmvNvjVjtpNpCNLLLJHHBdgLPdwsdsbLlwwlwb
GmCVSCRVGmpCRVvttmpDrQZfhnzhzqnDWnrZZTQq
DQBZHHtWHzSvZvDQWchgqsqqhrrhhcqrcZ
jdMfwlFfFlTfndwpjjwGnNrqhPTmPSPTPPhmgrPSrh
jlGbwGMdlnJpGFGjpnFCSJzzDDtWHCBBQBvtVC
RrbBWBRRWSRsBBVvsPHZDwSjjPdnHwtPtH
fTgfzMmNJpmJgfllgpjVQtDDndVQpdnHVtPp
gGmlNclTGmGFhLVcVrvLqrvc
QcpCTVCZVcCwLcCVvHvvVsCcNzNNSbPRzsDRDSBlsNNzDRtb
fggMfJqgrWFpmjWMggmrfMWNSbRSPBDbNtJRtPJzlStBbN
gdnmpWGnZvdQCvdv
tqqcLqqDDqNtDrqHrrPWlTlTWZTMzTFzQlMPSZ
pfnpmmppmppRGjwbjmnjwspWbQQQTMWZbCTSZCSQlCllZF
gmpVnGmmmpjDvVLBFqqvrH
LqBvJHZvbHGBHrBtGGQTmSVprVzhpVPDPQzQ
CRdRgwCfhTVDzSdQ
fRCcjgSMjfNgMMLGbGZtvBbGHv
HgvtDDzDpvwgvvqdHPZWdMssTTddSs
rJFrGNFVQmNFVmRnWhhsrTbhwhZTrdTd
VQGBBBVNQClpcBvBwD
PWlSzZGmdmGmlGmhggBpvMjvMjFgPJ
TtLRDtQQfTVcQQQRtBsJFFccFjWhJJFMBs
HqVCNtWHCDwdnlGwGqSr
RwdRJgCJRGGmdMbcGbdnTnTtttLLnptMtMtMqZ
DWsWPFrPqVPPLVCB
zQWWsslsQHFhDSszDSFQzJJJmvcgblRgmNvCJmvNgw
tpmFrWTtRpRTtggsSlnQpsnnlSHPsn
bZwZjNNZGLSrVsGndPPV
NvrcjCfbvvLBDBWfWFgRRm
WWFMgWmMhhwDcMMMDcmLWLtQwwsjbsQHvZHbRjZfsZzH
PTCplTCdSJJCpvPGNSvsbsfHtbQZzdHjQtjjsj
vNGJPpqJvJvqghgFgWFmLD
RlRpLTZCjWRjRWwpRsjHjbSbqMqMvvnbnGMnGGqQCq
gddfDNczmgPthNcDdgPVnbbzbnJrJJGSSVJJQS
BmDmcDmcmhffdBHlRwjRLpwlWQ
prQlfzlWRPzgQWzlMPMRppssHHsDsHjwnHHbWDwwbwjL
vFBJJtZNShJvZFtdSqtmqjTDVHVGDHbwVHDVsDnThH
vcjBZZdZqvCfpzRfcgRp
cggpqgRlSpNsgNggbjjj
ZZSSJVLVLFDZWNGjCWWbCjsF
vZLvfZQQfQtJVJDQShLrLfMmnldmwqwTqqMcMTMTndrm
bQBMtBPddtMFbJFhRGzMfzvnRGRSvWnW
TmHTqlVHwVpQqjmwGvSgSpnLpzfWGWSn
TTrDQCDrrTmDCCCVHHQZBdZFPdsNdFBtFDhtFB
fjpQvNZcGhGGTtQS
DVJzvbVmHbbtSTSTRStzTM
VDvmqllmJfjWlnplNs
ZmdHZJjvQLdRjpmLJrqqZBhhtCschPfBPcrDfPffCD
MWWSMMwnwlSgzWFFgSwzVwzqcfDCfChCbbtssbfDChcD
NMqFTwGqMwgwwgjHRdHRjdmQmQTm
TTqWPCWRhTWqPNjPJMNtrlbJFttQwwrBrlbwlc
GfpSDGZvpQffSHDgggDZrHctFmrHncnnwwbBtBrt
SQGfLsSLZsqMTRNMPT
HdBdnBZJTZBBmsfwwBlh
MjCVjzwqWrfzplzW
vVbqCjjRgjwMbnbGHJScScZHLL
dwwwtCdznvDDFrMrrw
GmWLQmgQmHgcdGcsTgTDqDbSfFWfMDMfbSNqvr
QhTLmVQHLmdLTjGGVptRnZpZBZVRpPpP
CzjFpzRHdtBFBCqNqSbJZWcQJTSbQjMTWZ
wGwVLlGrdVGwDnwsgfMSZvJMbWJcWlvbbMSc
rDfsgggrGnGngsPwdVLfDnmDtzzFNCPHtzCtFHpBRqhPztzR
mrgWzBcDtVCcQcCCdscf
LRJhjRjPZvqSRGhGjLgMCdHpMNwQCpMHpHMS
GRvGJRJjqPZbvGGhRjnqLJWtgFgtzTzDrFnTWrlTlllW
cbmcddlffvbTfvFflpZzsMVNznNVlnqnzqHMNM
StWJBQRWLRWNPNMCswRVHC
BJQBhSWhjSthJQGGWWggJDDDfbdbbfHbddbrFrddvFvv
jFqvqvWZWDtBJrrlrq
TzGcbHcrmVzMGNSmTcGDtBthJCNtsJDlBCghgP
bTrnTccnLSrrTHbnwfLjfdvRRwZFdwfR
drHVrdVDfsDbVsdVDbVqRwbZZwCRCCCJlJThwRgT
jFPcFpBSvtNPzSFcjcQpcQjpThZCRltGRRRJhwCwGhwgwhRm
SQSzPBjjPPSvLqqssdnqLZLMsM
bQTWlWlvQclNwwWlCCLStCRSSjStpj
zVZZDdBnBmgzVsjsLthSpshdCL
DfBnrmBmgzHBfDHmnGrNFCwQvTPvqCTwqTFGbF
srSWJnrbmlWlbhzsWszSvPGwvgDhcjdjjfvhjvGv
BRRQFLtNfQNMpqpQHDjdDjDcZZcvwZZHPH
NLCNCtRQfRttRFRCTqMBqQQrzrbzrlJmVVbsSWmVrTbSzJ
RHLfLcSRTFSghLRHGbwZmMZddgJswZsbMm
ptqjtCzzQztqCjDlBGpDpbMZdwmMbZsdwNmdJpbs
tttzCVllDCtDQnQBVHGHWvWTLWcLSLHf
FVlNnPqbGTHftghggJqf
zLcZWZpWWrcrZLLZDWrwMcrhBFBttChBmBgptChhtFftmf
LZZLrDrrDDMrcwrDwsWFzdTlnGQPQQVbdbnsvnvsVQ
BbPNMJNbQvDbvPLwHflczlwwzf
pZjWZGZjFGdgpnVgZhghdmcflrlswzzcstlrLwhtwc
WZSdqFjqSqSWdGFjZpdMTTDNTvLCRRLLqRQMCN
FqgFGtbgTvRwrLqhvw
JCCWJWCdJMQNNsSWsMPQRDDLDSDLwTrrvnwfDvnD
HdPJlBBHCCQdBMWdTtVbgHczGVGjmtzG
PLlZDLZDsFCvbDQv
HVcTmVmJqVzqczfzbjvvCFMRfCsWjMvR
cqHzTqJTTTTzzmnmrctrBlLlvSlgLdZvSwSlpw
SbMMNJjmgMnJdSSbjVFZVSQrlQfWVQVWZh
PtqDqPGcLHzHpqLcRzRsfQFfZlfRfZfRFVsl
cTDLcqGCzDTqzzDLDzqPTtJvbBJMnmvjbdlmJNvmdgNC
tDJDlZVqJGbvHNQbNFFsFPmLns
ppczpzpffGwfBNLGmn
WShzgTTpWzhWztJJGJSvtvvtjq
TbZFTFScnCZFQRTCqQdBjdJqjBqjjQDB
rmmLpLLfzrlmslMBHvdRddNDDJDrqD
MWwLPzmWfpsMmmlMPMWLwRTZTZnnTcVCcZFCwSnZ
SqmClqHssNWCqPTcWcGhBTchVV
ZnnnDflRpBVTTVhPBZ
DpgfvnvMfCsqlMtSll
ZzLMRZpLMwwppZqnQGvQgBSvlNVlBFFNFVrg
HcqhTmhmdDTPFTJgTTFBSgJN
mccPdDDHbssbtwZMqpbzCRGM
TgqnTltgWqLRSRnlqddngFfrvHvrBTfCCFrFVTvVCf
cwNJmPzQwNzczzNsJGhhHfhrfvVHGvtvVVfC
jjtbtDswcmPWlbgRnRdMZL
TmpTBBwvspTptRmsmTGLQDGRHGgVGLSQSMHQ
ZlPWqjWrzjPqdrlzbrbrwfrWLHVMLnHDMVDQnLQfQfVngQLS
zNwbrrFWbFJpmpmvvt
RMQQMwHMMzcFsWsDrWfcpJpS
LLhZmGVLhVlTZfWWfWpCrDsGSp
VLVTnqjjZngtQRFjvzDM
gmRBpjrpRvCfRCrBgvjHShnbnngbgSJnNsHMHS
ZDPTwGWtqwHhSnbcMNJw
DWGGqtVVqldWZzMzWmvjrjprLRFjRVvvff
tCzVzsVtDFzssnSsgdqJdCNqJhmgmpqq
PZccPGvQfRLMQwNdhpwhNh
jLrcbRjPZBrcPdjRHFlWnVtBFslSWznW
vvvbJbWrLvFWHzZzZRhB
chtwTmCNlRRZzRPT
hmcCssCswrMDGMSrsr
LStGBsQLlllhzMzs
dzVZDNWRDdZNDTZTPvWVhhphpMlfMccRmfnlMlRn
VFvgTrNPdFWNNFNFTzTFFSjSQBCqrtQwSBGLLBGwGL
qGJSJhWStdSfWvSvtGRRnzRDDggrgvnzsmRP
lTTLpcljjGlLlLNBpjwFQDQmRnrRDPrPscRrDDng
NCNjFlHNCTVjpwGqGSVbJddqZZJM
MbWdgvHFlMvmzTzShvmm
tqjqpLsNsrrsjstNLpQrGVhVBzrhVcfmchDcTPVVmc
RqwjqjqsGjjGGQNjGpQZpqRFJgmMHwdbFWgnHMFdwmmCFW
HHHLcCcVHjTHglsB
wDSRwzzRpMSdNSPSwSpRbqvgBsdqlgTvBFBjgFvvgB
RpbzPssDMWwNRbRNRPDsDhJthLQVGLJcctQCJQfQJCLm
WsZgbNgZVCCWbVVVmgZbCCRPccGnzPBqJjzWJBJPzvBvGz
SpfThHtrHFBPPzJvPntj
QHDhhrhpTQpHhQHnfwnTCNlbZCCDLNllZlVsNCNl
QtzJFRQLMRnZcZsfcphlPQ
qSBbjmWSCNmVldSqqSqmjCSZshfwfrPPZZfcPVZfhgsgPg
HqBbHqBGSlNBbltnLLHFJMtRvRTD
tcGtDdMcttttHNBlMctldlwjwwqqCLCwDwZjFCZhmnwC
VrJgvWWsPvRgVgrJQvfQfzgVzZwCbLZmnmwCwZqmnhjZbnLj
sJpffsRWWRJVWWpHltSpnMHGcMTl
zNqRbqSbfdcTLLfS
ZVPzPnVvdLwLDPfF
VWnzQCVWZVMzQRHgqgqrHGtGMp
PbHpWfWPvRfbzWPFfRpPDtBwSHMwCBgDwBjDtMMM
hTTdZQlcnTcmqVTdcddrDgBSwsjjBgqBtsCgMD
hlldTmdJJmJdZvzfFfNJFJgRzR
PJWvJBbWsfLQWsLvmCqHCcNLHqHLLcwDqV
dQztrZrdwHhptqDH
ZrMGjgMSrdzQGQRJPvGGbm
RmjljZChlDZBCRRvlmNSLSqMNLzwLvppwQSQ
sTnVnPrVGsGTPddJrfgQgqLgGpMNQtgNtNzg
sbbTfTdcJPnHbsJfHsdcmDDmmqBZlClmjBRDCZ
CJmHLmHFFCFbHsbJsJqvqhQqLDhQZvnQDZnn
wGwppTjdWPdgFpGcScBqNnNqNhQlDqnDlZZW
pGcgGgTpGjFdwpSFVgSdpPjrMCMffzJzRzztRfHCRsVmtbsz
CgBClZfCflPflNZRvfQswwmwmwQsQhgppdhm
qbzDGrjLLNLDHDqtJmmhhmQdhwpQhhbp
NLGqVqjDjjGrMFrvFWPBRBZnCvfFnT
tbrrHsgsVmmmbtgwVsQRqjJMmqMjQfJfLFLD
ZvlBGzdvjGfRFJQJ
dBppnnBBhdzZncBPlznpnNdWHSsbWthbSCgHrVfgSSwVgr
VRvMtRVFHQLvMRQFQtBctrthshTTgCmhTrgWhWZsZZ
lzJlGBSPPhzjgZsTCr
wJlpJPfDSpwBnddqJDdpPpcvMFHFMvNbvnNMFHHRVVbR
CPShbbdlGCdQqlRPGPdlDWDFzjtFjggCDJgWczfF
mrHrTrrBMBsmNsrwsBpnfpggDDcjjDDpjzFJzzjtJz
BvsNvBLHrrrNvwBTNNsNGbdQhlPGGfqhhRGqLGdl
PSSlPtlStGhPNMtwPMPJzDddnbnDNTDDnJqjbz
FFVHRwVLvFvVrVHrZcLmRHggjDmdDnDnznnznzQjzdmJddbn
WrvgRgcRcRrrcRvgcVrHVrwCCSfsCsGsllhMSSSSMttlSCpG
hBPJqVZTqqPSlGlfddfddZvl
JWWMJCpnMrmztzdjnzld
RbWsrwMrpbRspbWgpwhLJPccNVqLLPSVgVPV
hcTrWqcfhwGfWrWMjHjGvDHPmJMDzF
ZtlsnZZtLBSbSssnbndjDJJFHFHJPHPsHMTHHM
ntRZtSbtZgZStTqchwQfRwNpcq
GfLqrsqQGgPgjjQGVcNvTpTpNFcWPvPPpT
bRnRLnMZFdCMcpvT
RnRhzRlmlhhHhhmhRsqLrfzrGVSrGBSGrL
fbMffwdZsncrGcfG
qDBjSSLqhLBSmDbjqNhqTLjCGrCHGrvcGWcpWcrGWnCrpm
STLDqbhTLqNTNSRhlwZlJlRQFFRwMdPQ
TVVGNFggcjPPJzwvQlRRwRvSlcSc
frsBbWhtSRzSLfRf
qDCqddbsWrqzhsdNmdJNJHjTggFFVV
NTWTDrSdFTLtPTGf
lZqjHlVRvRltLtRWFMtFLL
qvjWzzvVbZpjqllggscdchwDrCphwsdhrD";

    #endregion 
}
