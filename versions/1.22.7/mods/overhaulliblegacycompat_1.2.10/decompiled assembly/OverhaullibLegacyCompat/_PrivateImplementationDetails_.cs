using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x020001B4 RID: 436
[CompilerGenerated]
internal sealed class <PrivateImplementationDetails>
{
	// Token: 0x060014DC RID: 5340 RVA: 0x00067D9B File Offset: 0x00065F9B
	internal static ReadOnlySpan<TElement> InlineArrayAsReadOnlySpan<TBuffer, TElement>(in TBuffer buffer, int length)
	{
		return MemoryMarshal.CreateReadOnlySpan<TElement>(Unsafe.As<TBuffer, TElement>(Unsafe.AsRef<TBuffer>(ref buffer)), length);
	}

	// Token: 0x060014DD RID: 5341 RVA: 0x00067DAE File Offset: 0x00065FAE
	internal static ref TElement InlineArrayElementRef<TBuffer, TElement>(ref TBuffer buffer, int index)
	{
		return Unsafe.Add<TElement>(Unsafe.As<TBuffer, TElement>(ref buffer), index);
	}

	// Token: 0x04000A5A RID: 2650 RVA: 0x00120408 File Offset: 0x0011E608
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 013D7F168D3B12112FE4238FC5CE39FA78F4B3793460B1061FF57A55C66AA0B9;

	// Token: 0x04000A5B RID: 2651 RVA: 0x00120430 File Offset: 0x0011E630
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 052A44DB00BD72330FA15DF1147F612D4D751544466EDA58568181BEC7C8DE47;

	// Token: 0x04000A5C RID: 2652 RVA: 0x00120458 File Offset: 0x0011E658
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=20 103AA39F0B961D5EAE755D2ACBFA5B74950B143869815D1A97C8AC50A1D9AC2C;

	// Token: 0x04000A5D RID: 2653 RVA: 0x00120470 File Offset: 0x0011E670
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=32 117835226246310F0C286BE099C9F4C49A64157FF8635B364FB20E580E5D2C32;

	// Token: 0x04000A5E RID: 2654 RVA: 0x00120490 File Offset: 0x0011E690
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 1834F731F9ACF0AE965B6EA9E4CCA18FA34AA73579C0BB2483A23681237B2651;

	// Token: 0x04000A5F RID: 2655 RVA: 0x001204B8 File Offset: 0x0011E6B8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 19CE46A2F86CE772249370DD982C3FCCD71D858CB35069C101938488D94C7B62;

	// Token: 0x04000A60 RID: 2656 RVA: 0x001204E0 File Offset: 0x0011E6E0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 1DD400919E03C365431212CB83D9E7F704B905F7187C39F78AC35462E57A60D3;

	// Token: 0x04000A61 RID: 2657 RVA: 0x00120508 File Offset: 0x0011E708
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=16 225C9BA434D2B9D5AAAB73F3B2D837E84394E03D258EF6342AE06405C8CFFF37;

	// Token: 0x04000A62 RID: 2658 RVA: 0x00120518 File Offset: 0x0011E718
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 2A18BA5323BE45A282C095A26AB125C90C53E5CBBFA2D43C69A9EAFD3B4EAF83;

	// Token: 0x04000A63 RID: 2659 RVA: 0x00120540 File Offset: 0x0011E740
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 37C819AA6CD5913A53A0BEA7B88EE5F43EF2ACB0FF5054AB3F7A418500378587;

	// Token: 0x04000A64 RID: 2660 RVA: 0x00120568 File Offset: 0x0011E768
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=24 3D17BC28FA192E3C3A3DC282879E74A712CEED2905C82F9A33A6AF1D3A6A79B5;

	// Token: 0x04000A65 RID: 2661 RVA: 0x00120580 File Offset: 0x0011E780
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=24 4574373090FA084909E46D55796E0D99C5661F412304DD1A55A1186840B21F82;

	// Token: 0x04000A66 RID: 2662 RVA: 0x00120598 File Offset: 0x0011E798
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 469D3A6AB3D614286885CC73A9CA7F6C8DDDA9F0BEE9B2033E4A4E73CC4FF810;

	// Token: 0x04000A67 RID: 2663 RVA: 0x001205C0 File Offset: 0x0011E7C0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 4744145B36941132E1E2EDDB5734CC4E569D6822FED17FF2D23896068A5DAE69;

	// Token: 0x04000A68 RID: 2664 RVA: 0x001205E8 File Offset: 0x0011E7E8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 4760AABD6CA9676E58815530DD8419B446760DA2F10551B8BE4B782E4AD02C80;

	// Token: 0x04000A69 RID: 2665 RVA: 0x00120610 File Offset: 0x0011E810
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 47BC2AC3C459AE68F6F21B568204CE35A12EBCF5DA744F3956B1E05985B4DDA8;

	// Token: 0x04000A6A RID: 2666 RVA: 0x00120638 File Offset: 0x0011E838
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=24 4809A7DDADA932F33398440D01CE6D3AFD48B89DDD589A883E399E040F8CB18A;

	// Token: 0x04000A6B RID: 2667 RVA: 0x00120650 File Offset: 0x0011E850
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=12 491FCB766F26B9B48138408D1BA0AD017C589655E545147ED9019E6224A9AC84;

	// Token: 0x04000A6C RID: 2668 RVA: 0x00120660 File Offset: 0x0011E860
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 4B6B051CAEC7D7E61F093AB169EDED47F492D57D884B8A4356CB9D3FB2861A60;

	// Token: 0x04000A6D RID: 2669 RVA: 0x00120688 File Offset: 0x0011E888
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 4CD2DC2A62A71DE39236E897FC27AA00363B50CD1387C05DFAD87B75D2197953;

	// Token: 0x04000A6E RID: 2670 RVA: 0x001206B0 File Offset: 0x0011E8B0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=20 4F6ADDC9659D6FB90FE94B6688A79F2A1FA8D36EC43F8F3E1D9B6528C448A384;

	// Token: 0x04000A6F RID: 2671 RVA: 0x001206C8 File Offset: 0x0011E8C8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 549016BC7F3C22E3C8CACAC99B2B07AEC1DD218608DA14948BE16A1DF3F6BB43;

	// Token: 0x04000A70 RID: 2672 RVA: 0x001206F0 File Offset: 0x0011E8F0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 603FE1A6A067D0D6BA57F1DCBF32F9DB2DBFCDEE5059C66BF8BD1ECE203CFE47;

	// Token: 0x04000A71 RID: 2673 RVA: 0x00120718 File Offset: 0x0011E918
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=24 64CB7F79A630C26DE25D466B2480B110507894E6F88F1E1B7F85FA3345B9B3D7;

	// Token: 0x04000A72 RID: 2674 RVA: 0x00120730 File Offset: 0x0011E930
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 68D542D9DDB821732F1A426684298C58AD39ADDB1C7D6DD01BC6FB1F447D45A3;

	// Token: 0x04000A73 RID: 2675 RVA: 0x00120758 File Offset: 0x0011E958
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 69294FFE6D2E7D9F03D4446F737142CF0B55CF11EAAC101B4DC6F045CDBC3405;

	// Token: 0x04000A74 RID: 2676 RVA: 0x00120780 File Offset: 0x0011E980
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 6F7C27C3DA284F0EB30E00068E31C6CEE96AF1E1CD25AD7F55BBCE598930082D;

	// Token: 0x04000A75 RID: 2677 RVA: 0x001207A8 File Offset: 0x0011E9A8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 71EA2DB888EEEB414825C39984DD861A9F94552A41BCBE520367FFD2D1654C23;

	// Token: 0x04000A76 RID: 2678 RVA: 0x001207D0 File Offset: 0x0011E9D0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 81B7FC0C5FC659652FCFED89539780656499CB072763221037D9B3C3223103B7;

	// Token: 0x04000A77 RID: 2679 RVA: 0x001207F8 File Offset: 0x0011E9F8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 84CBEB07E69119314F4D66A1882A638319ACDEA852F31415BC81E9DAEF317553;

	// Token: 0x04000A78 RID: 2680 RVA: 0x00120820 File Offset: 0x0011EA20
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=24 8D7ED34592DAC0E36F34F5CBCDF4B9D86F4F1811E2C2A40751C9D486E50D6684;

	// Token: 0x04000A79 RID: 2681 RVA: 0x00120838 File Offset: 0x0011EA38
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 8E2B48657DCA1373B11B36634DD66E2D2EA1197D13E1DB803182F2A81E7AA7CE;

	// Token: 0x04000A7A RID: 2682 RVA: 0x00120860 File Offset: 0x0011EA60
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 921C803ABFA6AC88F44F7AB19198E5C137D1C7183E8E6912757A6263E8DEE0A5;

	// Token: 0x04000A7B RID: 2683 RVA: 0x00120888 File Offset: 0x0011EA88
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 95FE6555C399427007F1082E232DDA4E016A9F877F8789DB9F041C2A1A2D5116;

	// Token: 0x04000A7C RID: 2684 RVA: 0x001208B0 File Offset: 0x0011EAB0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 9A1215EC071738D0A0954DB20BA6B7EA7050C9B34B0E2A641918A5AAC6EDB5F8;

	// Token: 0x04000A7D RID: 2685 RVA: 0x001208D8 File Offset: 0x0011EAD8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 9CD0ED109706C390999FFA409AE3125FE94BCE7AF81D015938978AC09DE5CE46;

	// Token: 0x04000A7E RID: 2686 RVA: 0x00120900 File Offset: 0x0011EB00
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 A8645E6901D0B630C64106DEE52546EF09814B03831B822129D8F2A7388ACD6A;

	// Token: 0x04000A7F RID: 2687 RVA: 0x00120928 File Offset: 0x0011EB28
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 A9A793B14920FED06762AB61767292E0457FCA9458558E5FEB111BA33F737987;

	// Token: 0x04000A80 RID: 2688 RVA: 0x00120950 File Offset: 0x0011EB50
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 AC9D7C6835ADE9B771494A32BD181FDE7BB25D50746FA6D92E24AA7547F4B84E;

	// Token: 0x04000A81 RID: 2689 RVA: 0x00120978 File Offset: 0x0011EB78
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 ADE676C0BDA1B6CC3F2E65707EC1418DAACC83F4CEF1BDF236D19ECC8DC505A2;

	// Token: 0x04000A82 RID: 2690 RVA: 0x001209A0 File Offset: 0x0011EBA0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 ADFC2036A535F1A50A31094BEFD1F8B16EA581418B4651EC3104C6E39C93703B;

	// Token: 0x04000A83 RID: 2691 RVA: 0x001209C8 File Offset: 0x0011EBC8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 B5457F33F6AA4CB5592E16A7369E03894156C97AF90F8CD9BB93CE834396D628;

	// Token: 0x04000A84 RID: 2692 RVA: 0x001209F0 File Offset: 0x0011EBF0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 C0AE77270051647ED479D3789AC392809A0BAFB929112D67BF5E5A1B944A2AF9;

	// Token: 0x04000A85 RID: 2693 RVA: 0x00120A18 File Offset: 0x0011EC18
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 C928CA6A7DB07EC685DB4FCDA7EAC56D26C71D04783BE2035C90E0F3F0ADE2AF;

	// Token: 0x04000A86 RID: 2694 RVA: 0x00120A40 File Offset: 0x0011EC40
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 D1A5BF6E9EE40B0FA9F95490C4B1035ADBB4E096C0EA855ED02430583757BDDC;

	// Token: 0x04000A87 RID: 2695 RVA: 0x00120A68 File Offset: 0x0011EC68
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 D6E512DF2D3F9D08017FBB14DBA25F7F03D4F131677180DA444AE6EB5C807149;

	// Token: 0x04000A88 RID: 2696 RVA: 0x00120A90 File Offset: 0x0011EC90
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=12 D7FEE75CE8379C3A788A16B499C8312F33985B4C0342EB8093FDD9EBBE68CFE5;

	// Token: 0x04000A89 RID: 2697 RVA: 0x00120AA0 File Offset: 0x0011ECA0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 DF3F58118EA36C917B92674DA4D496D19DC59BE5CB7E7CD85C4F22FA265D20B9;

	// Token: 0x04000A8A RID: 2698 RVA: 0x00120AC8 File Offset: 0x0011ECC8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 E0EC8C4F07D20C401E089E932CFF2251FF587736E365B30EF4E892174A687663;

	// Token: 0x04000A8B RID: 2699 RVA: 0x00120AF0 File Offset: 0x0011ECF0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=28 E7358AD4E9478FBC47D8F5D3BB89BF9F3A42E1A32B35AF82F7B7302DD880F882;

	// Token: 0x04000A8C RID: 2700 RVA: 0x00120B10 File Offset: 0x0011ED10
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 E7C1A18503B7C44E357B9961D9F89CE83EA6F12E46996767C21842747FD012B9;

	// Token: 0x04000A8D RID: 2701 RVA: 0x00120B38 File Offset: 0x0011ED38
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 EADE9DDCF4A7C9F9980372DAEE8CE08C7C6596407BD89A671F6837DED1C516C8;

	// Token: 0x04000A8E RID: 2702 RVA: 0x00120B60 File Offset: 0x0011ED60
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 F4895FAA7B202F0DA3ECEE3B5DB7A3DE3F9F94CD9EA56F69B6227AD3DEA8EA49;

	// Token: 0x04000A8F RID: 2703 RVA: 0x00120B88 File Offset: 0x0011ED88
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 F6BD341F36A5B80E89F4FD73E72649C4BC24C06AB3D4A2EF3AAF0DFCA9060E0C;

	// Token: 0x04000A90 RID: 2704 RVA: 0x00120BB0 File Offset: 0x0011EDB0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 FB596729DDD75095A5076843014FFD6370BDE599D3B6B99DA3EAB60FB56316AD;

	// Token: 0x04000A91 RID: 2705 RVA: 0x00120BD8 File Offset: 0x0011EDD8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 FC827F3F53F230FE72EDC962E38D89DBC2468658F651D1C6EFF87902B51460B1;

	// Token: 0x020002EE RID: 750
	[StructLayout(2, Pack = 1, Size = 12)]
	internal struct __StaticArrayInitTypeSize=12
	{
	}

	// Token: 0x020002EF RID: 751
	[StructLayout(2, Pack = 1, Size = 16)]
	internal struct __StaticArrayInitTypeSize=16
	{
	}

	// Token: 0x020002F0 RID: 752
	[StructLayout(2, Pack = 1, Size = 20)]
	internal struct __StaticArrayInitTypeSize=20
	{
	}

	// Token: 0x020002F1 RID: 753
	[StructLayout(2, Pack = 1, Size = 24)]
	internal struct __StaticArrayInitTypeSize=24
	{
	}

	// Token: 0x020002F2 RID: 754
	[StructLayout(2, Pack = 1, Size = 28)]
	internal struct __StaticArrayInitTypeSize=28
	{
	}

	// Token: 0x020002F3 RID: 755
	[StructLayout(2, Pack = 1, Size = 32)]
	internal struct __StaticArrayInitTypeSize=32
	{
	}

	// Token: 0x020002F4 RID: 756
	[StructLayout(2, Pack = 1, Size = 36)]
	internal struct __StaticArrayInitTypeSize=36
	{
	}
}
