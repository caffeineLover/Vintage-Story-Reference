using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x020001A6 RID: 422
[CompilerGenerated]
internal sealed class <PrivateImplementationDetails>
{
	// Token: 0x06001464 RID: 5220 RVA: 0x000662F3 File Offset: 0x000644F3
	internal static ReadOnlySpan<TElement> InlineArrayAsReadOnlySpan<TBuffer, TElement>(in TBuffer buffer, int length)
	{
		return MemoryMarshal.CreateReadOnlySpan<TElement>(Unsafe.As<TBuffer, TElement>(Unsafe.AsRef<TBuffer>(ref buffer)), length);
	}

	// Token: 0x06001465 RID: 5221 RVA: 0x00066306 File Offset: 0x00064506
	internal static ref TElement InlineArrayElementRef<TBuffer, TElement>(ref TBuffer buffer, int index)
	{
		return Unsafe.Add<TElement>(Unsafe.As<TBuffer, TElement>(ref buffer), index);
	}

	// Token: 0x04000A32 RID: 2610 RVA: 0x00119D98 File Offset: 0x00117F98
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 013D7F168D3B12112FE4238FC5CE39FA78F4B3793460B1061FF57A55C66AA0B9;

	// Token: 0x04000A33 RID: 2611 RVA: 0x00119DC0 File Offset: 0x00117FC0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 052A44DB00BD72330FA15DF1147F612D4D751544466EDA58568181BEC7C8DE47;

	// Token: 0x04000A34 RID: 2612 RVA: 0x00119DE8 File Offset: 0x00117FE8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=20 103AA39F0B961D5EAE755D2ACBFA5B74950B143869815D1A97C8AC50A1D9AC2C;

	// Token: 0x04000A35 RID: 2613 RVA: 0x00119E00 File Offset: 0x00118000
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=32 117835226246310F0C286BE099C9F4C49A64157FF8635B364FB20E580E5D2C32;

	// Token: 0x04000A36 RID: 2614 RVA: 0x00119E20 File Offset: 0x00118020
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 1834F731F9ACF0AE965B6EA9E4CCA18FA34AA73579C0BB2483A23681237B2651;

	// Token: 0x04000A37 RID: 2615 RVA: 0x00119E48 File Offset: 0x00118048
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 19CE46A2F86CE772249370DD982C3FCCD71D858CB35069C101938488D94C7B62;

	// Token: 0x04000A38 RID: 2616 RVA: 0x00119E70 File Offset: 0x00118070
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 1DD400919E03C365431212CB83D9E7F704B905F7187C39F78AC35462E57A60D3;

	// Token: 0x04000A39 RID: 2617 RVA: 0x00119E98 File Offset: 0x00118098
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=16 225C9BA434D2B9D5AAAB73F3B2D837E84394E03D258EF6342AE06405C8CFFF37;

	// Token: 0x04000A3A RID: 2618 RVA: 0x00119EA8 File Offset: 0x001180A8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 2A18BA5323BE45A282C095A26AB125C90C53E5CBBFA2D43C69A9EAFD3B4EAF83;

	// Token: 0x04000A3B RID: 2619 RVA: 0x00119ED0 File Offset: 0x001180D0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 37C819AA6CD5913A53A0BEA7B88EE5F43EF2ACB0FF5054AB3F7A418500378587;

	// Token: 0x04000A3C RID: 2620 RVA: 0x00119EF8 File Offset: 0x001180F8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=24 3D17BC28FA192E3C3A3DC282879E74A712CEED2905C82F9A33A6AF1D3A6A79B5;

	// Token: 0x04000A3D RID: 2621 RVA: 0x00119F10 File Offset: 0x00118110
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=24 4574373090FA084909E46D55796E0D99C5661F412304DD1A55A1186840B21F82;

	// Token: 0x04000A3E RID: 2622 RVA: 0x00119F28 File Offset: 0x00118128
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 469D3A6AB3D614286885CC73A9CA7F6C8DDDA9F0BEE9B2033E4A4E73CC4FF810;

	// Token: 0x04000A3F RID: 2623 RVA: 0x00119F50 File Offset: 0x00118150
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 4744145B36941132E1E2EDDB5734CC4E569D6822FED17FF2D23896068A5DAE69;

	// Token: 0x04000A40 RID: 2624 RVA: 0x00119F78 File Offset: 0x00118178
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 4760AABD6CA9676E58815530DD8419B446760DA2F10551B8BE4B782E4AD02C80;

	// Token: 0x04000A41 RID: 2625 RVA: 0x00119FA0 File Offset: 0x001181A0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 47BC2AC3C459AE68F6F21B568204CE35A12EBCF5DA744F3956B1E05985B4DDA8;

	// Token: 0x04000A42 RID: 2626 RVA: 0x00119FC8 File Offset: 0x001181C8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=24 4809A7DDADA932F33398440D01CE6D3AFD48B89DDD589A883E399E040F8CB18A;

	// Token: 0x04000A43 RID: 2627 RVA: 0x00119FE0 File Offset: 0x001181E0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=12 491FCB766F26B9B48138408D1BA0AD017C589655E545147ED9019E6224A9AC84;

	// Token: 0x04000A44 RID: 2628 RVA: 0x00119FF0 File Offset: 0x001181F0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 4B6B051CAEC7D7E61F093AB169EDED47F492D57D884B8A4356CB9D3FB2861A60;

	// Token: 0x04000A45 RID: 2629 RVA: 0x0011A018 File Offset: 0x00118218
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 4CD2DC2A62A71DE39236E897FC27AA00363B50CD1387C05DFAD87B75D2197953;

	// Token: 0x04000A46 RID: 2630 RVA: 0x0011A040 File Offset: 0x00118240
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=20 4F6ADDC9659D6FB90FE94B6688A79F2A1FA8D36EC43F8F3E1D9B6528C448A384;

	// Token: 0x04000A47 RID: 2631 RVA: 0x0011A058 File Offset: 0x00118258
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 549016BC7F3C22E3C8CACAC99B2B07AEC1DD218608DA14948BE16A1DF3F6BB43;

	// Token: 0x04000A48 RID: 2632 RVA: 0x0011A080 File Offset: 0x00118280
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 603FE1A6A067D0D6BA57F1DCBF32F9DB2DBFCDEE5059C66BF8BD1ECE203CFE47;

	// Token: 0x04000A49 RID: 2633 RVA: 0x0011A0A8 File Offset: 0x001182A8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=24 64CB7F79A630C26DE25D466B2480B110507894E6F88F1E1B7F85FA3345B9B3D7;

	// Token: 0x04000A4A RID: 2634 RVA: 0x0011A0C0 File Offset: 0x001182C0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 68D542D9DDB821732F1A426684298C58AD39ADDB1C7D6DD01BC6FB1F447D45A3;

	// Token: 0x04000A4B RID: 2635 RVA: 0x0011A0E8 File Offset: 0x001182E8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 69294FFE6D2E7D9F03D4446F737142CF0B55CF11EAAC101B4DC6F045CDBC3405;

	// Token: 0x04000A4C RID: 2636 RVA: 0x0011A110 File Offset: 0x00118310
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 6F7C27C3DA284F0EB30E00068E31C6CEE96AF1E1CD25AD7F55BBCE598930082D;

	// Token: 0x04000A4D RID: 2637 RVA: 0x0011A138 File Offset: 0x00118338
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 71EA2DB888EEEB414825C39984DD861A9F94552A41BCBE520367FFD2D1654C23;

	// Token: 0x04000A4E RID: 2638 RVA: 0x0011A160 File Offset: 0x00118360
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 81B7FC0C5FC659652FCFED89539780656499CB072763221037D9B3C3223103B7;

	// Token: 0x04000A4F RID: 2639 RVA: 0x0011A188 File Offset: 0x00118388
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 84CBEB07E69119314F4D66A1882A638319ACDEA852F31415BC81E9DAEF317553;

	// Token: 0x04000A50 RID: 2640 RVA: 0x0011A1B0 File Offset: 0x001183B0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=24 8D7ED34592DAC0E36F34F5CBCDF4B9D86F4F1811E2C2A40751C9D486E50D6684;

	// Token: 0x04000A51 RID: 2641 RVA: 0x0011A1C8 File Offset: 0x001183C8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 8E2B48657DCA1373B11B36634DD66E2D2EA1197D13E1DB803182F2A81E7AA7CE;

	// Token: 0x04000A52 RID: 2642 RVA: 0x0011A1F0 File Offset: 0x001183F0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 921C803ABFA6AC88F44F7AB19198E5C137D1C7183E8E6912757A6263E8DEE0A5;

	// Token: 0x04000A53 RID: 2643 RVA: 0x0011A218 File Offset: 0x00118418
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 95FE6555C399427007F1082E232DDA4E016A9F877F8789DB9F041C2A1A2D5116;

	// Token: 0x04000A54 RID: 2644 RVA: 0x0011A240 File Offset: 0x00118440
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 9A1215EC071738D0A0954DB20BA6B7EA7050C9B34B0E2A641918A5AAC6EDB5F8;

	// Token: 0x04000A55 RID: 2645 RVA: 0x0011A268 File Offset: 0x00118468
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 9CD0ED109706C390999FFA409AE3125FE94BCE7AF81D015938978AC09DE5CE46;

	// Token: 0x04000A56 RID: 2646 RVA: 0x0011A290 File Offset: 0x00118490
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 A8645E6901D0B630C64106DEE52546EF09814B03831B822129D8F2A7388ACD6A;

	// Token: 0x04000A57 RID: 2647 RVA: 0x0011A2B8 File Offset: 0x001184B8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 A9A793B14920FED06762AB61767292E0457FCA9458558E5FEB111BA33F737987;

	// Token: 0x04000A58 RID: 2648 RVA: 0x0011A2E0 File Offset: 0x001184E0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 AC9D7C6835ADE9B771494A32BD181FDE7BB25D50746FA6D92E24AA7547F4B84E;

	// Token: 0x04000A59 RID: 2649 RVA: 0x0011A308 File Offset: 0x00118508
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 ADE676C0BDA1B6CC3F2E65707EC1418DAACC83F4CEF1BDF236D19ECC8DC505A2;

	// Token: 0x04000A5A RID: 2650 RVA: 0x0011A330 File Offset: 0x00118530
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 ADFC2036A535F1A50A31094BEFD1F8B16EA581418B4651EC3104C6E39C93703B;

	// Token: 0x04000A5B RID: 2651 RVA: 0x0011A358 File Offset: 0x00118558
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 B5457F33F6AA4CB5592E16A7369E03894156C97AF90F8CD9BB93CE834396D628;

	// Token: 0x04000A5C RID: 2652 RVA: 0x0011A380 File Offset: 0x00118580
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 C0AE77270051647ED479D3789AC392809A0BAFB929112D67BF5E5A1B944A2AF9;

	// Token: 0x04000A5D RID: 2653 RVA: 0x0011A3A8 File Offset: 0x001185A8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 C928CA6A7DB07EC685DB4FCDA7EAC56D26C71D04783BE2035C90E0F3F0ADE2AF;

	// Token: 0x04000A5E RID: 2654 RVA: 0x0011A3D0 File Offset: 0x001185D0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 D1A5BF6E9EE40B0FA9F95490C4B1035ADBB4E096C0EA855ED02430583757BDDC;

	// Token: 0x04000A5F RID: 2655 RVA: 0x0011A3F8 File Offset: 0x001185F8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 D6E512DF2D3F9D08017FBB14DBA25F7F03D4F131677180DA444AE6EB5C807149;

	// Token: 0x04000A60 RID: 2656 RVA: 0x0011A420 File Offset: 0x00118620
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=12 D7FEE75CE8379C3A788A16B499C8312F33985B4C0342EB8093FDD9EBBE68CFE5;

	// Token: 0x04000A61 RID: 2657 RVA: 0x0011A430 File Offset: 0x00118630
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 DF3F58118EA36C917B92674DA4D496D19DC59BE5CB7E7CD85C4F22FA265D20B9;

	// Token: 0x04000A62 RID: 2658 RVA: 0x0011A458 File Offset: 0x00118658
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 E0EC8C4F07D20C401E089E932CFF2251FF587736E365B30EF4E892174A687663;

	// Token: 0x04000A63 RID: 2659 RVA: 0x0011A480 File Offset: 0x00118680
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=28 E7358AD4E9478FBC47D8F5D3BB89BF9F3A42E1A32B35AF82F7B7302DD880F882;

	// Token: 0x04000A64 RID: 2660 RVA: 0x0011A4A0 File Offset: 0x001186A0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 E7C1A18503B7C44E357B9961D9F89CE83EA6F12E46996767C21842747FD012B9;

	// Token: 0x04000A65 RID: 2661 RVA: 0x0011A4C8 File Offset: 0x001186C8
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 EADE9DDCF4A7C9F9980372DAEE8CE08C7C6596407BD89A671F6837DED1C516C8;

	// Token: 0x04000A66 RID: 2662 RVA: 0x0011A4F0 File Offset: 0x001186F0
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 F4895FAA7B202F0DA3ECEE3B5DB7A3DE3F9F94CD9EA56F69B6227AD3DEA8EA49;

	// Token: 0x04000A67 RID: 2663 RVA: 0x0011A518 File Offset: 0x00118718
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 F6BD341F36A5B80E89F4FD73E72649C4BC24C06AB3D4A2EF3AAF0DFCA9060E0C;

	// Token: 0x04000A68 RID: 2664 RVA: 0x0011A540 File Offset: 0x00118740
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 FB596729DDD75095A5076843014FFD6370BDE599D3B6B99DA3EAB60FB56316AD;

	// Token: 0x04000A69 RID: 2665 RVA: 0x0011A568 File Offset: 0x00118768
	internal static readonly <PrivateImplementationDetails>.__StaticArrayInitTypeSize=36 FC827F3F53F230FE72EDC962E38D89DBC2468658F651D1C6EFF87902B51460B1;

	// Token: 0x020002DB RID: 731
	[StructLayout(2, Pack = 1, Size = 12)]
	internal struct __StaticArrayInitTypeSize=12
	{
	}

	// Token: 0x020002DC RID: 732
	[StructLayout(2, Pack = 1, Size = 16)]
	internal struct __StaticArrayInitTypeSize=16
	{
	}

	// Token: 0x020002DD RID: 733
	[StructLayout(2, Pack = 1, Size = 20)]
	internal struct __StaticArrayInitTypeSize=20
	{
	}

	// Token: 0x020002DE RID: 734
	[StructLayout(2, Pack = 1, Size = 24)]
	internal struct __StaticArrayInitTypeSize=24
	{
	}

	// Token: 0x020002DF RID: 735
	[StructLayout(2, Pack = 1, Size = 28)]
	internal struct __StaticArrayInitTypeSize=28
	{
	}

	// Token: 0x020002E0 RID: 736
	[StructLayout(2, Pack = 1, Size = 32)]
	internal struct __StaticArrayInitTypeSize=32
	{
	}

	// Token: 0x020002E1 RID: 737
	[StructLayout(2, Pack = 1, Size = 36)]
	internal struct __StaticArrayInitTypeSize=36
	{
	}
}
