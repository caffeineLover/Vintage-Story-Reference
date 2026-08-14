using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.UI;
using WinRT;

namespace ABI.Windows.UI
{
	// Token: 0x02000099 RID: 153
	[Guid("CFF52E04-CCA6-4614-A17E-754910C84A99")]
	internal interface IColorsStatics : Windows.UI.IColorsStatics
	{
		// Token: 0x060005A8 RID: 1448 RVA: 0x000099E8 File Offset: 0x00007BE8
		unsafe static IColorsStatics()
		{
			*(IInspectable.Vftbl*)((void*)IColorsStatics.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_AliceBlue_0);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_AntiqueWhite_1);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)8 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Aqua_2);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)9 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Aquamarine_3);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)10 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Azure_4);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)11 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Beige_5);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)12 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Bisque_6);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)13 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Black_7);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)14 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_BlanchedAlmond_8);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)15 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Blue_9);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)16 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_BlueViolet_10);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)17 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Brown_11);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)18 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_BurlyWood_12);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)19 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_CadetBlue_13);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)20 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Chartreuse_14);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)21 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Chocolate_15);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)22 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Coral_16);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)23 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_CornflowerBlue_17);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)24 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Cornsilk_18);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)25 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Crimson_19);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)26 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Cyan_20);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)27 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkBlue_21);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)28 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkCyan_22);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)29 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkGoldenrod_23);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)30 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkGray_24);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)31 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkGreen_25);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)32 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkKhaki_26);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)33 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkMagenta_27);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)34 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkOliveGreen_28);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)35 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkOrange_29);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)36 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkOrchid_30);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)37 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkRed_31);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)38 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkSalmon_32);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)39 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkSeaGreen_33);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)40 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkSlateBlue_34);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)41 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkSlateGray_35);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)42 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkTurquoise_36);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)43 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DarkViolet_37);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)44 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DeepPink_38);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)45 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DeepSkyBlue_39);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)46 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DimGray_40);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)47 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DodgerBlue_41);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)48 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Firebrick_42);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)49 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_FloralWhite_43);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)50 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_ForestGreen_44);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)51 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Fuchsia_45);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)52 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Gainsboro_46);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)53 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_GhostWhite_47);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)54 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Gold_48);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)55 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Goldenrod_49);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)56 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Gray_50);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)57 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Green_51);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)58 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_GreenYellow_52);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)59 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Honeydew_53);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)60 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_HotPink_54);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)61 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_IndianRed_55);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)62 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Indigo_56);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)63 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Ivory_57);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)64 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Khaki_58);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)65 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Lavender_59);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)66 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LavenderBlush_60);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)67 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LawnGreen_61);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)68 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LemonChiffon_62);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)69 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LightBlue_63);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)70 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LightCoral_64);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)71 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LightCyan_65);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)72 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LightGoldenrodYellow_66);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)73 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LightGreen_67);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)74 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LightGray_68);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)75 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LightPink_69);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)76 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LightSalmon_70);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)77 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LightSeaGreen_71);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)78 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LightSkyBlue_72);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)79 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LightSlateGray_73);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)80 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LightSteelBlue_74);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)81 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LightYellow_75);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)82 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Lime_76);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)83 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_LimeGreen_77);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)84 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Linen_78);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)85 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Magenta_79);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)86 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Maroon_80);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)87 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_MediumAquamarine_81);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)88 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_MediumBlue_82);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)89 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_MediumOrchid_83);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)90 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_MediumPurple_84);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)91 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_MediumSeaGreen_85);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)92 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_MediumSlateBlue_86);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)93 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_MediumSpringGreen_87);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)94 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_MediumTurquoise_88);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)95 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_MediumVioletRed_89);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)96 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_MidnightBlue_90);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)97 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_MintCream_91);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)98 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_MistyRose_92);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)99 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Moccasin_93);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)100 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_NavajoWhite_94);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)101 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Navy_95);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)102 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_OldLace_96);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)103 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Olive_97);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)104 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_OliveDrab_98);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)105 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Orange_99);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)106 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_OrangeRed_100);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)107 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Orchid_101);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)108 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_PaleGoldenrod_102);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)109 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_PaleGreen_103);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)110 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_PaleTurquoise_104);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)111 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_PaleVioletRed_105);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)112 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_PapayaWhip_106);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)113 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_PeachPuff_107);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)114 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Peru_108);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)115 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Pink_109);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)116 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Plum_110);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)117 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_PowderBlue_111);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)118 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Purple_112);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)119 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Red_113);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)120 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_RosyBrown_114);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)121 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_RoyalBlue_115);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)122 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_SaddleBrown_116);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)123 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Salmon_117);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)124 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_SandyBrown_118);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)125 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_SeaGreen_119);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)126 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_SeaShell_120);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)127 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Sienna_121);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)128 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Silver_122);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)129 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_SkyBlue_123);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)130 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_SlateBlue_124);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)131 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_SlateGray_125);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)132 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Snow_126);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)133 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_SpringGreen_127);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)134 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_SteelBlue_128);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)135 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Tan_129);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)136 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Teal_130);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)137 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Thistle_131);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)138 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Tomato_132);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)139 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Transparent_133);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)140 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Turquoise_134);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)141 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Violet_135);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)142 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Wheat_136);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)143 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_White_137);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)144 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_WhiteSmoke_138);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)145 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Yellow_139);
			*(IntPtr*)((byte*)((void*)IColorsStatics.AbiToProjectionVftablePtr) + (IntPtr)146 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_YellowGreen_140);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0000A9D4 File Offset: 0x00008BD4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_AliceBlue_0(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).AliceBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0000AA28 File Offset: 0x00008C28
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_AntiqueWhite_1(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).AntiqueWhite;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0000AA7C File Offset: 0x00008C7C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Aqua_2(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Aqua;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0000AAD0 File Offset: 0x00008CD0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Aquamarine_3(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Aquamarine;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0000AB24 File Offset: 0x00008D24
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Azure_4(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Azure;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0000AB78 File Offset: 0x00008D78
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Beige_5(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Beige;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0000ABCC File Offset: 0x00008DCC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Bisque_6(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Bisque;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0000AC20 File Offset: 0x00008E20
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Black_7(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Black;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0000AC74 File Offset: 0x00008E74
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_BlanchedAlmond_8(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).BlanchedAlmond;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0000ACC8 File Offset: 0x00008EC8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Blue_9(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Blue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0000AD1C File Offset: 0x00008F1C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_BlueViolet_10(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).BlueViolet;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0000AD70 File Offset: 0x00008F70
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Brown_11(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Brown;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0000ADC4 File Offset: 0x00008FC4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_BurlyWood_12(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).BurlyWood;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0000AE18 File Offset: 0x00009018
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_CadetBlue_13(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).CadetBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0000AE6C File Offset: 0x0000906C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Chartreuse_14(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Chartreuse;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0000AEC0 File Offset: 0x000090C0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Chocolate_15(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Chocolate;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0000AF14 File Offset: 0x00009114
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Coral_16(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Coral;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0000AF68 File Offset: 0x00009168
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_CornflowerBlue_17(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).CornflowerBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0000AFBC File Offset: 0x000091BC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Cornsilk_18(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Cornsilk;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0000B010 File Offset: 0x00009210
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Crimson_19(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Crimson;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0000B064 File Offset: 0x00009264
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Cyan_20(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Cyan;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0000B0B8 File Offset: 0x000092B8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkBlue_21(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0000B10C File Offset: 0x0000930C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkCyan_22(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkCyan;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0000B160 File Offset: 0x00009360
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkGoldenrod_23(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkGoldenrod;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0000B1B4 File Offset: 0x000093B4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkGray_24(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkGray;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0000B208 File Offset: 0x00009408
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkGreen_25(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkGreen;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0000B25C File Offset: 0x0000945C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkKhaki_26(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkKhaki;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0000B2B0 File Offset: 0x000094B0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkMagenta_27(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkMagenta;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0000B304 File Offset: 0x00009504
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkOliveGreen_28(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkOliveGreen;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0000B358 File Offset: 0x00009558
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkOrange_29(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkOrange;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0000B3AC File Offset: 0x000095AC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkOrchid_30(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkOrchid;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0000B400 File Offset: 0x00009600
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkRed_31(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkRed;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0000B454 File Offset: 0x00009654
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkSalmon_32(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkSalmon;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0000B4A8 File Offset: 0x000096A8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkSeaGreen_33(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkSeaGreen;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0000B4FC File Offset: 0x000096FC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkSlateBlue_34(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkSlateBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0000B550 File Offset: 0x00009750
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkSlateGray_35(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkSlateGray;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0000B5A4 File Offset: 0x000097A4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkTurquoise_36(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkTurquoise;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0000B5F8 File Offset: 0x000097F8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DarkViolet_37(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DarkViolet;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0000B64C File Offset: 0x0000984C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DeepPink_38(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DeepPink;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0000B6A0 File Offset: 0x000098A0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DeepSkyBlue_39(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DeepSkyBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0000B6F4 File Offset: 0x000098F4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DimGray_40(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DimGray;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0000B748 File Offset: 0x00009948
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DodgerBlue_41(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).DodgerBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0000B79C File Offset: 0x0000999C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Firebrick_42(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Firebrick;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0000B7F0 File Offset: 0x000099F0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_FloralWhite_43(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).FloralWhite;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0000B844 File Offset: 0x00009A44
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_ForestGreen_44(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).ForestGreen;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0000B898 File Offset: 0x00009A98
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Fuchsia_45(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Fuchsia;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0000B8EC File Offset: 0x00009AEC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Gainsboro_46(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Gainsboro;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0000B940 File Offset: 0x00009B40
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_GhostWhite_47(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).GhostWhite;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0000B994 File Offset: 0x00009B94
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Gold_48(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Gold;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0000B9E8 File Offset: 0x00009BE8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Goldenrod_49(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Goldenrod;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0000BA3C File Offset: 0x00009C3C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Gray_50(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Gray;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0000BA90 File Offset: 0x00009C90
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Green_51(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Green;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0000BAE4 File Offset: 0x00009CE4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_GreenYellow_52(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).GreenYellow;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0000BB38 File Offset: 0x00009D38
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Honeydew_53(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Honeydew;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0000BB8C File Offset: 0x00009D8C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_HotPink_54(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).HotPink;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0000BBE0 File Offset: 0x00009DE0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_IndianRed_55(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).IndianRed;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0000BC34 File Offset: 0x00009E34
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Indigo_56(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Indigo;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0000BC88 File Offset: 0x00009E88
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Ivory_57(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Ivory;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0000BCDC File Offset: 0x00009EDC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Khaki_58(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Khaki;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0000BD30 File Offset: 0x00009F30
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Lavender_59(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Lavender;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0000BD84 File Offset: 0x00009F84
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LavenderBlush_60(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LavenderBlush;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0000BDD8 File Offset: 0x00009FD8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LawnGreen_61(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LawnGreen;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0000BE2C File Offset: 0x0000A02C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LemonChiffon_62(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LemonChiffon;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0000BE80 File Offset: 0x0000A080
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LightBlue_63(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LightBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0000BED4 File Offset: 0x0000A0D4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LightCoral_64(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LightCoral;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0000BF28 File Offset: 0x0000A128
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LightCyan_65(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LightCyan;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0000BF7C File Offset: 0x0000A17C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LightGoldenrodYellow_66(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LightGoldenrodYellow;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0000BFD0 File Offset: 0x0000A1D0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LightGray_68(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LightGray;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0000C024 File Offset: 0x0000A224
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LightGreen_67(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LightGreen;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0000C078 File Offset: 0x0000A278
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LightPink_69(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LightPink;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0000C0CC File Offset: 0x0000A2CC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LightSalmon_70(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LightSalmon;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0000C120 File Offset: 0x0000A320
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LightSeaGreen_71(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LightSeaGreen;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0000C174 File Offset: 0x0000A374
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LightSkyBlue_72(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LightSkyBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0000C1C8 File Offset: 0x0000A3C8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LightSlateGray_73(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LightSlateGray;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0000C21C File Offset: 0x0000A41C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LightSteelBlue_74(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LightSteelBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0000C270 File Offset: 0x0000A470
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LightYellow_75(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LightYellow;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0000C2C4 File Offset: 0x0000A4C4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Lime_76(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Lime;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0000C318 File Offset: 0x0000A518
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_LimeGreen_77(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).LimeGreen;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0000C36C File Offset: 0x0000A56C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Linen_78(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Linen;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0000C3C0 File Offset: 0x0000A5C0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Magenta_79(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Magenta;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0000C414 File Offset: 0x0000A614
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Maroon_80(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Maroon;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0000C468 File Offset: 0x0000A668
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_MediumAquamarine_81(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).MediumAquamarine;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0000C4BC File Offset: 0x0000A6BC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_MediumBlue_82(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).MediumBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0000C510 File Offset: 0x0000A710
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_MediumOrchid_83(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).MediumOrchid;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0000C564 File Offset: 0x0000A764
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_MediumPurple_84(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).MediumPurple;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0000C5B8 File Offset: 0x0000A7B8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_MediumSeaGreen_85(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).MediumSeaGreen;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0000C60C File Offset: 0x0000A80C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_MediumSlateBlue_86(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).MediumSlateBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0000C660 File Offset: 0x0000A860
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_MediumSpringGreen_87(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).MediumSpringGreen;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0000C6B4 File Offset: 0x0000A8B4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_MediumTurquoise_88(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).MediumTurquoise;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0000C708 File Offset: 0x0000A908
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_MediumVioletRed_89(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).MediumVioletRed;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0000C75C File Offset: 0x0000A95C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_MidnightBlue_90(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).MidnightBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0000C7B0 File Offset: 0x0000A9B0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_MintCream_91(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).MintCream;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0000C804 File Offset: 0x0000AA04
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_MistyRose_92(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).MistyRose;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0000C858 File Offset: 0x0000AA58
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Moccasin_93(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Moccasin;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0000C8AC File Offset: 0x0000AAAC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_NavajoWhite_94(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).NavajoWhite;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0000C900 File Offset: 0x0000AB00
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Navy_95(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Navy;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0000C954 File Offset: 0x0000AB54
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_OldLace_96(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).OldLace;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0000C9A8 File Offset: 0x0000ABA8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Olive_97(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Olive;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0000C9FC File Offset: 0x0000ABFC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_OliveDrab_98(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).OliveDrab;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0000CA50 File Offset: 0x0000AC50
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Orange_99(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Orange;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0000CAA4 File Offset: 0x0000ACA4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_OrangeRed_100(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).OrangeRed;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0000CAF8 File Offset: 0x0000ACF8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Orchid_101(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Orchid;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0000CB4C File Offset: 0x0000AD4C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_PaleGoldenrod_102(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).PaleGoldenrod;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0000CBA0 File Offset: 0x0000ADA0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_PaleGreen_103(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).PaleGreen;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0000CBF4 File Offset: 0x0000ADF4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_PaleTurquoise_104(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).PaleTurquoise;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0000CC48 File Offset: 0x0000AE48
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_PaleVioletRed_105(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).PaleVioletRed;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0000CC9C File Offset: 0x0000AE9C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_PapayaWhip_106(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).PapayaWhip;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0000CCF0 File Offset: 0x0000AEF0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_PeachPuff_107(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).PeachPuff;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0000CD44 File Offset: 0x0000AF44
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Peru_108(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Peru;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0000CD98 File Offset: 0x0000AF98
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Pink_109(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Pink;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0000CDEC File Offset: 0x0000AFEC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Plum_110(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Plum;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0000CE40 File Offset: 0x0000B040
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_PowderBlue_111(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).PowderBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x0000CE94 File Offset: 0x0000B094
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Purple_112(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Purple;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0000CEE8 File Offset: 0x0000B0E8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Red_113(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Red;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0000CF3C File Offset: 0x0000B13C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_RosyBrown_114(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).RosyBrown;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0000CF90 File Offset: 0x0000B190
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_RoyalBlue_115(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).RoyalBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0000CFE4 File Offset: 0x0000B1E4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_SaddleBrown_116(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).SaddleBrown;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0000D038 File Offset: 0x0000B238
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Salmon_117(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Salmon;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0000D08C File Offset: 0x0000B28C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_SandyBrown_118(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).SandyBrown;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0000D0E0 File Offset: 0x0000B2E0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_SeaGreen_119(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).SeaGreen;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0000D134 File Offset: 0x0000B334
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_SeaShell_120(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).SeaShell;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0000D188 File Offset: 0x0000B388
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Sienna_121(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Sienna;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0000D1DC File Offset: 0x0000B3DC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Silver_122(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Silver;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0000D230 File Offset: 0x0000B430
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_SkyBlue_123(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).SkyBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0000D284 File Offset: 0x0000B484
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_SlateBlue_124(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).SlateBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0000D2D8 File Offset: 0x0000B4D8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_SlateGray_125(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).SlateGray;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0000D32C File Offset: 0x0000B52C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Snow_126(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Snow;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0000D380 File Offset: 0x0000B580
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_SpringGreen_127(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).SpringGreen;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0000D3D4 File Offset: 0x0000B5D4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_SteelBlue_128(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).SteelBlue;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0000D428 File Offset: 0x0000B628
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Tan_129(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Tan;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0000D47C File Offset: 0x0000B67C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Teal_130(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Teal;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0000D4D0 File Offset: 0x0000B6D0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Thistle_131(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Thistle;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0000D524 File Offset: 0x0000B724
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Tomato_132(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Tomato;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0000D578 File Offset: 0x0000B778
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Transparent_133(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Transparent;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0000D5CC File Offset: 0x0000B7CC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Turquoise_134(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Turquoise;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0000D620 File Offset: 0x0000B820
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Violet_135(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Violet;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0000D674 File Offset: 0x0000B874
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Wheat_136(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Wheat;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0000D6C8 File Offset: 0x0000B8C8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_White_137(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).White;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0000D71C File Offset: 0x0000B91C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_WhiteSmoke_138(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).WhiteSmoke;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0000D770 File Offset: 0x0000B970
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Yellow_139(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).Yellow;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0000D7C4 File Offset: 0x0000B9C4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_YellowGreen_140(IntPtr thisPtr, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.IColorsStatics>(thisPtr).YellowGreen;
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x04000127 RID: 295
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IColorsStatics), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 141);
	}
}
