using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using ABI.Windows.UI;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.UI
{
	// Token: 0x02000019 RID: 25
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[ProjectedRuntimeClass("_default")]
	[ObjectReferenceWrapper("_inner")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal sealed class Colors : ICustomQueryInterface, IWinRTObject, IDynamicInterfaceCastable, IEquatable<Colors>
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00003A68 File Offset: 0x00001C68
		private IntPtr ThisPtr
		{
			get
			{
				if (this._inner != null)
				{
					return this._inner.ThisPtr;
				}
				return ((IWinRTObject)this).NativeObject.ThisPtr;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00003A89 File Offset: 0x00001C89
		private IObjectReference _objRef_global__Windows_UI_IColors
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00003A91 File Offset: 0x00001C91
		private IColors _default
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00003A94 File Offset: 0x00001C94
		public static I As<I>()
		{
			return ActivationFactory<Colors>.AsInterface<I>();
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00003A9B File Offset: 0x00001C9B
		private static IObjectReference Make___objRef_global__Windows_UI_IColorsStatics()
		{
			Interlocked.CompareExchange<IObjectReference>(ref Colors.___objRef_global__Windows_UI_IColorsStatics, ActivationFactory<Colors>.As(GuidGenerator.GetIID(typeof(IColorsStatics).GetHelperType())), null);
			return Colors.___objRef_global__Windows_UI_IColorsStatics;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00003AC9 File Offset: 0x00001CC9
		private static IObjectReference _objRef_global__Windows_UI_IColorsStatics
		{
			get
			{
				return Colors.___objRef_global__Windows_UI_IColorsStatics ?? Colors.Make___objRef_global__Windows_UI_IColorsStatics();
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00003ADB File Offset: 0x00001CDB
		public static Color AliceBlue
		{
			get
			{
				return IColorsStaticsMethods.get_AliceBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00003AE7 File Offset: 0x00001CE7
		public static Color AntiqueWhite
		{
			get
			{
				return IColorsStaticsMethods.get_AntiqueWhite(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00003AF3 File Offset: 0x00001CF3
		public static Color Aqua
		{
			get
			{
				return IColorsStaticsMethods.get_Aqua(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00003AFF File Offset: 0x00001CFF
		public static Color Aquamarine
		{
			get
			{
				return IColorsStaticsMethods.get_Aquamarine(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00003B0B File Offset: 0x00001D0B
		public static Color Azure
		{
			get
			{
				return IColorsStaticsMethods.get_Azure(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00003B17 File Offset: 0x00001D17
		public static Color Beige
		{
			get
			{
				return IColorsStaticsMethods.get_Beige(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00003B23 File Offset: 0x00001D23
		public static Color Bisque
		{
			get
			{
				return IColorsStaticsMethods.get_Bisque(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00003B2F File Offset: 0x00001D2F
		public static Color Black
		{
			get
			{
				return IColorsStaticsMethods.get_Black(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00003B3B File Offset: 0x00001D3B
		public static Color BlanchedAlmond
		{
			get
			{
				return IColorsStaticsMethods.get_BlanchedAlmond(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00003B47 File Offset: 0x00001D47
		public static Color Blue
		{
			get
			{
				return IColorsStaticsMethods.get_Blue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00003B53 File Offset: 0x00001D53
		public static Color BlueViolet
		{
			get
			{
				return IColorsStaticsMethods.get_BlueViolet(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00003B5F File Offset: 0x00001D5F
		public static Color Brown
		{
			get
			{
				return IColorsStaticsMethods.get_Brown(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00003B6B File Offset: 0x00001D6B
		public static Color BurlyWood
		{
			get
			{
				return IColorsStaticsMethods.get_BurlyWood(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00003B77 File Offset: 0x00001D77
		public static Color CadetBlue
		{
			get
			{
				return IColorsStaticsMethods.get_CadetBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00003B83 File Offset: 0x00001D83
		public static Color Chartreuse
		{
			get
			{
				return IColorsStaticsMethods.get_Chartreuse(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00003B8F File Offset: 0x00001D8F
		public static Color Chocolate
		{
			get
			{
				return IColorsStaticsMethods.get_Chocolate(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00003B9B File Offset: 0x00001D9B
		public static Color Coral
		{
			get
			{
				return IColorsStaticsMethods.get_Coral(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00003BA7 File Offset: 0x00001DA7
		public static Color CornflowerBlue
		{
			get
			{
				return IColorsStaticsMethods.get_CornflowerBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00003BB3 File Offset: 0x00001DB3
		public static Color Cornsilk
		{
			get
			{
				return IColorsStaticsMethods.get_Cornsilk(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00003BBF File Offset: 0x00001DBF
		public static Color Crimson
		{
			get
			{
				return IColorsStaticsMethods.get_Crimson(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00003BCB File Offset: 0x00001DCB
		public static Color Cyan
		{
			get
			{
				return IColorsStaticsMethods.get_Cyan(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00003BD7 File Offset: 0x00001DD7
		public static Color DarkBlue
		{
			get
			{
				return IColorsStaticsMethods.get_DarkBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00003BE3 File Offset: 0x00001DE3
		public static Color DarkCyan
		{
			get
			{
				return IColorsStaticsMethods.get_DarkCyan(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00003BEF File Offset: 0x00001DEF
		public static Color DarkGoldenrod
		{
			get
			{
				return IColorsStaticsMethods.get_DarkGoldenrod(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00003BFB File Offset: 0x00001DFB
		public static Color DarkGray
		{
			get
			{
				return IColorsStaticsMethods.get_DarkGray(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00003C07 File Offset: 0x00001E07
		public static Color DarkGreen
		{
			get
			{
				return IColorsStaticsMethods.get_DarkGreen(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00003C13 File Offset: 0x00001E13
		public static Color DarkKhaki
		{
			get
			{
				return IColorsStaticsMethods.get_DarkKhaki(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00003C1F File Offset: 0x00001E1F
		public static Color DarkMagenta
		{
			get
			{
				return IColorsStaticsMethods.get_DarkMagenta(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00003C2B File Offset: 0x00001E2B
		public static Color DarkOliveGreen
		{
			get
			{
				return IColorsStaticsMethods.get_DarkOliveGreen(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00003C37 File Offset: 0x00001E37
		public static Color DarkOrange
		{
			get
			{
				return IColorsStaticsMethods.get_DarkOrange(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00003C43 File Offset: 0x00001E43
		public static Color DarkOrchid
		{
			get
			{
				return IColorsStaticsMethods.get_DarkOrchid(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00003C4F File Offset: 0x00001E4F
		public static Color DarkRed
		{
			get
			{
				return IColorsStaticsMethods.get_DarkRed(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00003C5B File Offset: 0x00001E5B
		public static Color DarkSalmon
		{
			get
			{
				return IColorsStaticsMethods.get_DarkSalmon(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00003C67 File Offset: 0x00001E67
		public static Color DarkSeaGreen
		{
			get
			{
				return IColorsStaticsMethods.get_DarkSeaGreen(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00003C73 File Offset: 0x00001E73
		public static Color DarkSlateBlue
		{
			get
			{
				return IColorsStaticsMethods.get_DarkSlateBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00003C7F File Offset: 0x00001E7F
		public static Color DarkSlateGray
		{
			get
			{
				return IColorsStaticsMethods.get_DarkSlateGray(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00003C8B File Offset: 0x00001E8B
		public static Color DarkTurquoise
		{
			get
			{
				return IColorsStaticsMethods.get_DarkTurquoise(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00003C97 File Offset: 0x00001E97
		public static Color DarkViolet
		{
			get
			{
				return IColorsStaticsMethods.get_DarkViolet(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00003CA3 File Offset: 0x00001EA3
		public static Color DeepPink
		{
			get
			{
				return IColorsStaticsMethods.get_DeepPink(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00003CAF File Offset: 0x00001EAF
		public static Color DeepSkyBlue
		{
			get
			{
				return IColorsStaticsMethods.get_DeepSkyBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00003CBB File Offset: 0x00001EBB
		public static Color DimGray
		{
			get
			{
				return IColorsStaticsMethods.get_DimGray(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00003CC7 File Offset: 0x00001EC7
		public static Color DodgerBlue
		{
			get
			{
				return IColorsStaticsMethods.get_DodgerBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00003CD3 File Offset: 0x00001ED3
		public static Color Firebrick
		{
			get
			{
				return IColorsStaticsMethods.get_Firebrick(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00003CDF File Offset: 0x00001EDF
		public static Color FloralWhite
		{
			get
			{
				return IColorsStaticsMethods.get_FloralWhite(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00003CEB File Offset: 0x00001EEB
		public static Color ForestGreen
		{
			get
			{
				return IColorsStaticsMethods.get_ForestGreen(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00003CF7 File Offset: 0x00001EF7
		public static Color Fuchsia
		{
			get
			{
				return IColorsStaticsMethods.get_Fuchsia(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00003D03 File Offset: 0x00001F03
		public static Color Gainsboro
		{
			get
			{
				return IColorsStaticsMethods.get_Gainsboro(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00003D0F File Offset: 0x00001F0F
		public static Color GhostWhite
		{
			get
			{
				return IColorsStaticsMethods.get_GhostWhite(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00003D1B File Offset: 0x00001F1B
		public static Color Gold
		{
			get
			{
				return IColorsStaticsMethods.get_Gold(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00003D27 File Offset: 0x00001F27
		public static Color Goldenrod
		{
			get
			{
				return IColorsStaticsMethods.get_Goldenrod(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00003D33 File Offset: 0x00001F33
		public static Color Gray
		{
			get
			{
				return IColorsStaticsMethods.get_Gray(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00003D3F File Offset: 0x00001F3F
		public static Color Green
		{
			get
			{
				return IColorsStaticsMethods.get_Green(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00003D4B File Offset: 0x00001F4B
		public static Color GreenYellow
		{
			get
			{
				return IColorsStaticsMethods.get_GreenYellow(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00003D57 File Offset: 0x00001F57
		public static Color Honeydew
		{
			get
			{
				return IColorsStaticsMethods.get_Honeydew(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00003D63 File Offset: 0x00001F63
		public static Color HotPink
		{
			get
			{
				return IColorsStaticsMethods.get_HotPink(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00003D6F File Offset: 0x00001F6F
		public static Color IndianRed
		{
			get
			{
				return IColorsStaticsMethods.get_IndianRed(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00003D7B File Offset: 0x00001F7B
		public static Color Indigo
		{
			get
			{
				return IColorsStaticsMethods.get_Indigo(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00003D87 File Offset: 0x00001F87
		public static Color Ivory
		{
			get
			{
				return IColorsStaticsMethods.get_Ivory(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00003D93 File Offset: 0x00001F93
		public static Color Khaki
		{
			get
			{
				return IColorsStaticsMethods.get_Khaki(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00003D9F File Offset: 0x00001F9F
		public static Color Lavender
		{
			get
			{
				return IColorsStaticsMethods.get_Lavender(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00003DAB File Offset: 0x00001FAB
		public static Color LavenderBlush
		{
			get
			{
				return IColorsStaticsMethods.get_LavenderBlush(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00003DB7 File Offset: 0x00001FB7
		public static Color LawnGreen
		{
			get
			{
				return IColorsStaticsMethods.get_LawnGreen(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00003DC3 File Offset: 0x00001FC3
		public static Color LemonChiffon
		{
			get
			{
				return IColorsStaticsMethods.get_LemonChiffon(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00003DCF File Offset: 0x00001FCF
		public static Color LightBlue
		{
			get
			{
				return IColorsStaticsMethods.get_LightBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00003DDB File Offset: 0x00001FDB
		public static Color LightCoral
		{
			get
			{
				return IColorsStaticsMethods.get_LightCoral(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00003DE7 File Offset: 0x00001FE7
		public static Color LightCyan
		{
			get
			{
				return IColorsStaticsMethods.get_LightCyan(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00003DF3 File Offset: 0x00001FF3
		public static Color LightGoldenrodYellow
		{
			get
			{
				return IColorsStaticsMethods.get_LightGoldenrodYellow(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00003DFF File Offset: 0x00001FFF
		public static Color LightGray
		{
			get
			{
				return IColorsStaticsMethods.get_LightGray(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00003E0B File Offset: 0x0000200B
		public static Color LightGreen
		{
			get
			{
				return IColorsStaticsMethods.get_LightGreen(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00003E17 File Offset: 0x00002017
		public static Color LightPink
		{
			get
			{
				return IColorsStaticsMethods.get_LightPink(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00003E23 File Offset: 0x00002023
		public static Color LightSalmon
		{
			get
			{
				return IColorsStaticsMethods.get_LightSalmon(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00003E2F File Offset: 0x0000202F
		public static Color LightSeaGreen
		{
			get
			{
				return IColorsStaticsMethods.get_LightSeaGreen(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00003E3B File Offset: 0x0000203B
		public static Color LightSkyBlue
		{
			get
			{
				return IColorsStaticsMethods.get_LightSkyBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00003E47 File Offset: 0x00002047
		public static Color LightSlateGray
		{
			get
			{
				return IColorsStaticsMethods.get_LightSlateGray(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00003E53 File Offset: 0x00002053
		public static Color LightSteelBlue
		{
			get
			{
				return IColorsStaticsMethods.get_LightSteelBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00003E5F File Offset: 0x0000205F
		public static Color LightYellow
		{
			get
			{
				return IColorsStaticsMethods.get_LightYellow(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00003E6B File Offset: 0x0000206B
		public static Color Lime
		{
			get
			{
				return IColorsStaticsMethods.get_Lime(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00003E77 File Offset: 0x00002077
		public static Color LimeGreen
		{
			get
			{
				return IColorsStaticsMethods.get_LimeGreen(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00003E83 File Offset: 0x00002083
		public static Color Linen
		{
			get
			{
				return IColorsStaticsMethods.get_Linen(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00003E8F File Offset: 0x0000208F
		public static Color Magenta
		{
			get
			{
				return IColorsStaticsMethods.get_Magenta(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00003E9B File Offset: 0x0000209B
		public static Color Maroon
		{
			get
			{
				return IColorsStaticsMethods.get_Maroon(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00003EA7 File Offset: 0x000020A7
		public static Color MediumAquamarine
		{
			get
			{
				return IColorsStaticsMethods.get_MediumAquamarine(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00003EB3 File Offset: 0x000020B3
		public static Color MediumBlue
		{
			get
			{
				return IColorsStaticsMethods.get_MediumBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00003EBF File Offset: 0x000020BF
		public static Color MediumOrchid
		{
			get
			{
				return IColorsStaticsMethods.get_MediumOrchid(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00003ECB File Offset: 0x000020CB
		public static Color MediumPurple
		{
			get
			{
				return IColorsStaticsMethods.get_MediumPurple(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00003ED7 File Offset: 0x000020D7
		public static Color MediumSeaGreen
		{
			get
			{
				return IColorsStaticsMethods.get_MediumSeaGreen(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00003EE3 File Offset: 0x000020E3
		public static Color MediumSlateBlue
		{
			get
			{
				return IColorsStaticsMethods.get_MediumSlateBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00003EEF File Offset: 0x000020EF
		public static Color MediumSpringGreen
		{
			get
			{
				return IColorsStaticsMethods.get_MediumSpringGreen(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00003EFB File Offset: 0x000020FB
		public static Color MediumTurquoise
		{
			get
			{
				return IColorsStaticsMethods.get_MediumTurquoise(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00003F07 File Offset: 0x00002107
		public static Color MediumVioletRed
		{
			get
			{
				return IColorsStaticsMethods.get_MediumVioletRed(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00003F13 File Offset: 0x00002113
		public static Color MidnightBlue
		{
			get
			{
				return IColorsStaticsMethods.get_MidnightBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00003F1F File Offset: 0x0000211F
		public static Color MintCream
		{
			get
			{
				return IColorsStaticsMethods.get_MintCream(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00003F2B File Offset: 0x0000212B
		public static Color MistyRose
		{
			get
			{
				return IColorsStaticsMethods.get_MistyRose(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00003F37 File Offset: 0x00002137
		public static Color Moccasin
		{
			get
			{
				return IColorsStaticsMethods.get_Moccasin(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00003F43 File Offset: 0x00002143
		public static Color NavajoWhite
		{
			get
			{
				return IColorsStaticsMethods.get_NavajoWhite(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00003F4F File Offset: 0x0000214F
		public static Color Navy
		{
			get
			{
				return IColorsStaticsMethods.get_Navy(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00003F5B File Offset: 0x0000215B
		public static Color OldLace
		{
			get
			{
				return IColorsStaticsMethods.get_OldLace(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00003F67 File Offset: 0x00002167
		public static Color Olive
		{
			get
			{
				return IColorsStaticsMethods.get_Olive(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00003F73 File Offset: 0x00002173
		public static Color OliveDrab
		{
			get
			{
				return IColorsStaticsMethods.get_OliveDrab(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00003F7F File Offset: 0x0000217F
		public static Color Orange
		{
			get
			{
				return IColorsStaticsMethods.get_Orange(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00003F8B File Offset: 0x0000218B
		public static Color OrangeRed
		{
			get
			{
				return IColorsStaticsMethods.get_OrangeRed(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00003F97 File Offset: 0x00002197
		public static Color Orchid
		{
			get
			{
				return IColorsStaticsMethods.get_Orchid(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00003FA3 File Offset: 0x000021A3
		public static Color PaleGoldenrod
		{
			get
			{
				return IColorsStaticsMethods.get_PaleGoldenrod(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00003FAF File Offset: 0x000021AF
		public static Color PaleGreen
		{
			get
			{
				return IColorsStaticsMethods.get_PaleGreen(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00003FBB File Offset: 0x000021BB
		public static Color PaleTurquoise
		{
			get
			{
				return IColorsStaticsMethods.get_PaleTurquoise(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00003FC7 File Offset: 0x000021C7
		public static Color PaleVioletRed
		{
			get
			{
				return IColorsStaticsMethods.get_PaleVioletRed(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00003FD3 File Offset: 0x000021D3
		public static Color PapayaWhip
		{
			get
			{
				return IColorsStaticsMethods.get_PapayaWhip(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00003FDF File Offset: 0x000021DF
		public static Color PeachPuff
		{
			get
			{
				return IColorsStaticsMethods.get_PeachPuff(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00003FEB File Offset: 0x000021EB
		public static Color Peru
		{
			get
			{
				return IColorsStaticsMethods.get_Peru(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00003FF7 File Offset: 0x000021F7
		public static Color Pink
		{
			get
			{
				return IColorsStaticsMethods.get_Pink(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00004003 File Offset: 0x00002203
		public static Color Plum
		{
			get
			{
				return IColorsStaticsMethods.get_Plum(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000400F File Offset: 0x0000220F
		public static Color PowderBlue
		{
			get
			{
				return IColorsStaticsMethods.get_PowderBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000153 RID: 339 RVA: 0x0000401B File Offset: 0x0000221B
		public static Color Purple
		{
			get
			{
				return IColorsStaticsMethods.get_Purple(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00004027 File Offset: 0x00002227
		public static Color Red
		{
			get
			{
				return IColorsStaticsMethods.get_Red(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00004033 File Offset: 0x00002233
		public static Color RosyBrown
		{
			get
			{
				return IColorsStaticsMethods.get_RosyBrown(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000156 RID: 342 RVA: 0x0000403F File Offset: 0x0000223F
		public static Color RoyalBlue
		{
			get
			{
				return IColorsStaticsMethods.get_RoyalBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000404B File Offset: 0x0000224B
		public static Color SaddleBrown
		{
			get
			{
				return IColorsStaticsMethods.get_SaddleBrown(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00004057 File Offset: 0x00002257
		public static Color Salmon
		{
			get
			{
				return IColorsStaticsMethods.get_Salmon(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00004063 File Offset: 0x00002263
		public static Color SandyBrown
		{
			get
			{
				return IColorsStaticsMethods.get_SandyBrown(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600015A RID: 346 RVA: 0x0000406F File Offset: 0x0000226F
		public static Color SeaGreen
		{
			get
			{
				return IColorsStaticsMethods.get_SeaGreen(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000407B File Offset: 0x0000227B
		public static Color SeaShell
		{
			get
			{
				return IColorsStaticsMethods.get_SeaShell(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00004087 File Offset: 0x00002287
		public static Color Sienna
		{
			get
			{
				return IColorsStaticsMethods.get_Sienna(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00004093 File Offset: 0x00002293
		public static Color Silver
		{
			get
			{
				return IColorsStaticsMethods.get_Silver(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600015E RID: 350 RVA: 0x0000409F File Offset: 0x0000229F
		public static Color SkyBlue
		{
			get
			{
				return IColorsStaticsMethods.get_SkyBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600015F RID: 351 RVA: 0x000040AB File Offset: 0x000022AB
		public static Color SlateBlue
		{
			get
			{
				return IColorsStaticsMethods.get_SlateBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000160 RID: 352 RVA: 0x000040B7 File Offset: 0x000022B7
		public static Color SlateGray
		{
			get
			{
				return IColorsStaticsMethods.get_SlateGray(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000161 RID: 353 RVA: 0x000040C3 File Offset: 0x000022C3
		public static Color Snow
		{
			get
			{
				return IColorsStaticsMethods.get_Snow(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000162 RID: 354 RVA: 0x000040CF File Offset: 0x000022CF
		public static Color SpringGreen
		{
			get
			{
				return IColorsStaticsMethods.get_SpringGreen(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000163 RID: 355 RVA: 0x000040DB File Offset: 0x000022DB
		public static Color SteelBlue
		{
			get
			{
				return IColorsStaticsMethods.get_SteelBlue(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000164 RID: 356 RVA: 0x000040E7 File Offset: 0x000022E7
		public static Color Tan
		{
			get
			{
				return IColorsStaticsMethods.get_Tan(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000165 RID: 357 RVA: 0x000040F3 File Offset: 0x000022F3
		public static Color Teal
		{
			get
			{
				return IColorsStaticsMethods.get_Teal(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000166 RID: 358 RVA: 0x000040FF File Offset: 0x000022FF
		public static Color Thistle
		{
			get
			{
				return IColorsStaticsMethods.get_Thistle(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000167 RID: 359 RVA: 0x0000410B File Offset: 0x0000230B
		public static Color Tomato
		{
			get
			{
				return IColorsStaticsMethods.get_Tomato(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00004117 File Offset: 0x00002317
		public static Color Transparent
		{
			get
			{
				return IColorsStaticsMethods.get_Transparent(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00004123 File Offset: 0x00002323
		public static Color Turquoise
		{
			get
			{
				return IColorsStaticsMethods.get_Turquoise(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600016A RID: 362 RVA: 0x0000412F File Offset: 0x0000232F
		public static Color Violet
		{
			get
			{
				return IColorsStaticsMethods.get_Violet(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600016B RID: 363 RVA: 0x0000413B File Offset: 0x0000233B
		public static Color Wheat
		{
			get
			{
				return IColorsStaticsMethods.get_Wheat(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00004147 File Offset: 0x00002347
		public static Color White
		{
			get
			{
				return IColorsStaticsMethods.get_White(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00004153 File Offset: 0x00002353
		public static Color WhiteSmoke
		{
			get
			{
				return IColorsStaticsMethods.get_WhiteSmoke(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600016E RID: 366 RVA: 0x0000415F File Offset: 0x0000235F
		public static Color Yellow
		{
			get
			{
				return IColorsStaticsMethods.get_Yellow(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600016F RID: 367 RVA: 0x0000416B File Offset: 0x0000236B
		public static Color YellowGreen
		{
			get
			{
				return IColorsStaticsMethods.get_YellowGreen(Colors._objRef_global__Windows_UI_IColorsStatics);
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00004177 File Offset: 0x00002377
		public static Colors FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			return MarshalInspectable<Colors>.FromAbi(thisPtr);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000418E File Offset: 0x0000238E
		internal Colors(IObjectReference objRef)
		{
			this._inner = objRef.As(GuidGenerator.GetIID(typeof(IColors).GetHelperType()));
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000041B6 File Offset: 0x000023B6
		public static bool operator ==(Colors x, Colors y)
		{
			return ((x != null) ? x.ThisPtr : IntPtr.Zero) == ((y != null) ? y.ThisPtr : IntPtr.Zero);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000041DD File Offset: 0x000023DD
		public static bool operator !=(Colors x, Colors y)
		{
			return !(x == y);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000041E9 File Offset: 0x000023E9
		public bool Equals(Colors other)
		{
			return this == other;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000041F4 File Offset: 0x000023F4
		public override bool Equals(object obj)
		{
			Colors colors = obj as Colors;
			return colors != null && this == colors;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00004214 File Offset: 0x00002414
		public override int GetHashCode()
		{
			return this.ThisPtr.GetHashCode();
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000422F File Offset: 0x0000242F
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00004232 File Offset: 0x00002432
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000423A File Offset: 0x0000243A
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00004256 File Offset: 0x00002456
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000426A File Offset: 0x0000246A
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00004286 File Offset: 0x00002486
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000429A File Offset: 0x0000249A
		private bool IsOverridableInterface(Guid iid)
		{
			return false;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000042A0 File Offset: 0x000024A0
		CustomQueryInterfaceResult ICustomQueryInterface.GetInterface(ref Guid iid, out IntPtr ppv)
		{
			ppv = IntPtr.Zero;
			if (this.IsOverridableInterface(iid) || InterfaceIIDs.IInspectable_IID == iid)
			{
				return 1;
			}
			if (((IWinRTObject)this).NativeObject.TryAs(iid, out ppv) >= 0)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x04000040 RID: 64
		private IObjectReference _inner;

		// Token: 0x04000041 RID: 65
		private static volatile IObjectReference ___objRef_global__Windows_UI_IColorsStatics;

		// Token: 0x04000042 RID: 66
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x04000043 RID: 67
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;

		// Token: 0x020002B8 RID: 696
		private struct InterfaceTag<I>
		{
		}
	}
}
