using System;
using System.Runtime.CompilerServices;
using System.Text;
using ImGuiNET;
using Newtonsoft.Json;
using VSImGui.API;

namespace VSImGui
{
	// Token: 0x0200000A RID: 10
	[NullableContext(1)]
	[Nullable(0)]
	[JsonObject(0)]
	public class Style
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002F61 File Offset: 0x00001161
		[JsonIgnore]
		internal ImGuiStyle NativeStyle
		{
			get
			{
				return this.mNativeStyle;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002F69 File Offset: 0x00001169
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00002F71 File Offset: 0x00001171
		[JsonIgnore]
		public Value2 Paddings
		{
			private get
			{
				return this.PaddingWindow;
			}
			set
			{
				this.PaddingWindow = value;
				this.PaddingFrame = value;
				this.PaddingCell = value;
				this.PaddingTouchExtra = value;
				this.PaddingSeparatorText = value;
				this.PaddingDisplayWindow = value;
				this.PaddingDisplaySafeArea = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002FA4 File Offset: 0x000011A4
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002FB6 File Offset: 0x000011B6
		public Value2 PaddingWindow
		{
			get
			{
				return this.mNativeStyle.WindowPadding;
			}
			set
			{
				this.mNativeStyle.WindowPadding = value.Value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002FCA File Offset: 0x000011CA
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00002FDC File Offset: 0x000011DC
		public Value2 PaddingFrame
		{
			get
			{
				return this.mNativeStyle.FramePadding;
			}
			set
			{
				this.mNativeStyle.FramePadding = value.Value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002FF0 File Offset: 0x000011F0
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00003002 File Offset: 0x00001202
		public Value2 PaddingCell
		{
			get
			{
				return this.mNativeStyle.CellPadding;
			}
			set
			{
				this.mNativeStyle.CellPadding = value.Value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00003016 File Offset: 0x00001216
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00003028 File Offset: 0x00001228
		public Value2 PaddingTouchExtra
		{
			get
			{
				return this.mNativeStyle.TouchExtraPadding;
			}
			set
			{
				this.mNativeStyle.TouchExtraPadding = value.Value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000064 RID: 100 RVA: 0x0000303C File Offset: 0x0000123C
		// (set) Token: 0x06000065 RID: 101 RVA: 0x0000304E File Offset: 0x0000124E
		public Value2 PaddingSeparatorText
		{
			get
			{
				return this.mNativeStyle.SeparatorTextPadding;
			}
			set
			{
				this.mNativeStyle.SeparatorTextPadding = value.Value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003062 File Offset: 0x00001262
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00003074 File Offset: 0x00001274
		public Value2 PaddingDisplayWindow
		{
			get
			{
				return this.mNativeStyle.DisplayWindowPadding;
			}
			set
			{
				this.mNativeStyle.DisplayWindowPadding = value.Value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003088 File Offset: 0x00001288
		// (set) Token: 0x06000069 RID: 105 RVA: 0x0000309A File Offset: 0x0000129A
		public Value2 PaddingDisplaySafeArea
		{
			get
			{
				return this.mNativeStyle.DisplaySafeAreaPadding;
			}
			set
			{
				this.mNativeStyle.DisplaySafeAreaPadding = value.Value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000030AE File Offset: 0x000012AE
		// (set) Token: 0x0600006B RID: 107 RVA: 0x000030C0 File Offset: 0x000012C0
		public Value2 SpacingItem
		{
			get
			{
				return this.mNativeStyle.ItemSpacing;
			}
			set
			{
				this.mNativeStyle.ItemSpacing = value.Value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600006C RID: 108 RVA: 0x000030D4 File Offset: 0x000012D4
		// (set) Token: 0x0600006D RID: 109 RVA: 0x000030E6 File Offset: 0x000012E6
		public Value2 SpacingItemInner
		{
			get
			{
				return this.mNativeStyle.ItemInnerSpacing;
			}
			set
			{
				this.mNativeStyle.ItemInnerSpacing = value.Value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000030FA File Offset: 0x000012FA
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00003107 File Offset: 0x00001307
		public float SpacingIndent
		{
			get
			{
				return this.mNativeStyle.IndentSpacing;
			}
			set
			{
				this.mNativeStyle.IndentSpacing = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003115 File Offset: 0x00001315
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00003122 File Offset: 0x00001322
		public float SpacingColumnsMin
		{
			get
			{
				return this.mNativeStyle.ColumnsMinSpacing;
			}
			set
			{
				this.mNativeStyle.ColumnsMinSpacing = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003130 File Offset: 0x00001330
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00003138 File Offset: 0x00001338
		[JsonIgnore]
		public float Borders
		{
			private get
			{
				return this.BorderWindow;
			}
			set
			{
				this.BorderWindow = value;
				this.BorderChild = value;
				this.BorderPopup = value;
				this.BorderFrame = value;
				this.BorderTab = value;
				this.BorderSeparatorText = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00003164 File Offset: 0x00001364
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00003171 File Offset: 0x00001371
		public float BorderWindow
		{
			get
			{
				return this.mNativeStyle.WindowBorderSize;
			}
			set
			{
				this.mNativeStyle.WindowBorderSize = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000076 RID: 118 RVA: 0x0000317F File Offset: 0x0000137F
		// (set) Token: 0x06000077 RID: 119 RVA: 0x0000318C File Offset: 0x0000138C
		public float BorderChild
		{
			get
			{
				return this.mNativeStyle.ChildBorderSize;
			}
			set
			{
				this.mNativeStyle.ChildBorderSize = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000078 RID: 120 RVA: 0x0000319A File Offset: 0x0000139A
		// (set) Token: 0x06000079 RID: 121 RVA: 0x000031A7 File Offset: 0x000013A7
		public float BorderPopup
		{
			get
			{
				return this.mNativeStyle.PopupBorderSize;
			}
			set
			{
				this.mNativeStyle.PopupBorderSize = value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600007A RID: 122 RVA: 0x000031B5 File Offset: 0x000013B5
		// (set) Token: 0x0600007B RID: 123 RVA: 0x000031C2 File Offset: 0x000013C2
		public float BorderFrame
		{
			get
			{
				return this.mNativeStyle.FrameBorderSize;
			}
			set
			{
				this.mNativeStyle.FrameBorderSize = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600007C RID: 124 RVA: 0x000031D0 File Offset: 0x000013D0
		// (set) Token: 0x0600007D RID: 125 RVA: 0x000031DD File Offset: 0x000013DD
		public float BorderTab
		{
			get
			{
				return this.mNativeStyle.TabBorderSize;
			}
			set
			{
				this.mNativeStyle.TabBorderSize = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000031EB File Offset: 0x000013EB
		// (set) Token: 0x0600007F RID: 127 RVA: 0x000031F8 File Offset: 0x000013F8
		public float BorderSeparatorText
		{
			get
			{
				return this.mNativeStyle.SeparatorTextBorderSize;
			}
			set
			{
				this.mNativeStyle.SeparatorTextBorderSize = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003206 File Offset: 0x00001406
		// (set) Token: 0x06000081 RID: 129 RVA: 0x0000320E File Offset: 0x0000140E
		[JsonIgnore]
		public float Rounding
		{
			private get
			{
				return this.RoundingWindow;
			}
			set
			{
				this.RoundingWindow = value;
				this.RoundingChild = value;
				this.RoundingPopup = value;
				this.RoundingFrame = value;
				this.RoundingScrollbar = value;
				this.RoundingGrab = value;
				this.RoundingTab = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003241 File Offset: 0x00001441
		// (set) Token: 0x06000083 RID: 131 RVA: 0x0000324E File Offset: 0x0000144E
		public float RoundingWindow
		{
			get
			{
				return this.mNativeStyle.WindowRounding;
			}
			set
			{
				this.mNativeStyle.WindowRounding = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000084 RID: 132 RVA: 0x0000325C File Offset: 0x0000145C
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00003269 File Offset: 0x00001469
		public float RoundingChild
		{
			get
			{
				return this.mNativeStyle.ChildRounding;
			}
			set
			{
				this.mNativeStyle.ChildRounding = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003277 File Offset: 0x00001477
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00003284 File Offset: 0x00001484
		public float RoundingPopup
		{
			get
			{
				return this.mNativeStyle.PopupRounding;
			}
			set
			{
				this.mNativeStyle.PopupRounding = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003292 File Offset: 0x00001492
		// (set) Token: 0x06000089 RID: 137 RVA: 0x0000329F File Offset: 0x0000149F
		public float RoundingFrame
		{
			get
			{
				return this.mNativeStyle.FrameRounding;
			}
			set
			{
				this.mNativeStyle.FrameRounding = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600008A RID: 138 RVA: 0x000032AD File Offset: 0x000014AD
		// (set) Token: 0x0600008B RID: 139 RVA: 0x000032BA File Offset: 0x000014BA
		public float RoundingScrollbar
		{
			get
			{
				return this.mNativeStyle.ScrollbarRounding;
			}
			set
			{
				this.mNativeStyle.ScrollbarRounding = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000032C8 File Offset: 0x000014C8
		// (set) Token: 0x0600008D RID: 141 RVA: 0x000032D5 File Offset: 0x000014D5
		public float RoundingGrab
		{
			get
			{
				return this.mNativeStyle.GrabRounding;
			}
			set
			{
				this.mNativeStyle.GrabRounding = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000032E3 File Offset: 0x000014E3
		// (set) Token: 0x0600008F RID: 143 RVA: 0x000032F0 File Offset: 0x000014F0
		public float RoundingTab
		{
			get
			{
				return this.mNativeStyle.TabRounding;
			}
			set
			{
				this.mNativeStyle.TabRounding = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000032FE File Offset: 0x000014FE
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00003310 File Offset: 0x00001510
		public Value2 SizeWindowMin
		{
			get
			{
				return this.mNativeStyle.WindowMinSize;
			}
			set
			{
				this.mNativeStyle.WindowMinSize = value.Value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00003324 File Offset: 0x00001524
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00003331 File Offset: 0x00001531
		public float SizeScrollbar
		{
			get
			{
				return this.mNativeStyle.ScrollbarSize;
			}
			set
			{
				this.mNativeStyle.ScrollbarSize = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000333F File Offset: 0x0000153F
		// (set) Token: 0x06000095 RID: 149 RVA: 0x0000334C File Offset: 0x0000154C
		public float SizeGrabMin
		{
			get
			{
				return this.mNativeStyle.GrabMinSize;
			}
			set
			{
				this.mNativeStyle.GrabMinSize = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000096 RID: 150 RVA: 0x0000335A File Offset: 0x0000155A
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00003362 File Offset: 0x00001562
		[JsonIgnore]
		public Value2 Aligns
		{
			private get
			{
				return this.AlignWindowTitle;
			}
			set
			{
				this.AlignWindowTitle = value;
				this.AlignButtonText = value;
				this.AlignSelectableText = value;
				this.AlignSeparatorText = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00003380 File Offset: 0x00001580
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00003392 File Offset: 0x00001592
		public Value2 AlignWindowTitle
		{
			get
			{
				return this.mNativeStyle.WindowTitleAlign;
			}
			set
			{
				this.mNativeStyle.WindowTitleAlign = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000033A5 File Offset: 0x000015A5
		// (set) Token: 0x0600009B RID: 155 RVA: 0x000033B7 File Offset: 0x000015B7
		public Value2 AlignButtonText
		{
			get
			{
				return this.mNativeStyle.ButtonTextAlign;
			}
			set
			{
				this.mNativeStyle.ButtonTextAlign = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600009C RID: 156 RVA: 0x000033CA File Offset: 0x000015CA
		// (set) Token: 0x0600009D RID: 157 RVA: 0x000033DC File Offset: 0x000015DC
		public Value2 AlignSelectableText
		{
			get
			{
				return this.mNativeStyle.SelectableTextAlign;
			}
			set
			{
				this.mNativeStyle.SelectableTextAlign = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600009E RID: 158 RVA: 0x000033EF File Offset: 0x000015EF
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00003401 File Offset: 0x00001601
		public Value2 AlignSeparatorText
		{
			get
			{
				return this.mNativeStyle.SeparatorTextAlign;
			}
			set
			{
				this.mNativeStyle.SeparatorTextAlign = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00003414 File Offset: 0x00001614
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00003421 File Offset: 0x00001621
		public float Alpha
		{
			get
			{
				return this.mNativeStyle.Alpha;
			}
			set
			{
				this.mNativeStyle.Alpha = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x0000342F File Offset: 0x0000162F
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x0000343C File Offset: 0x0000163C
		public float DisabledAlpha
		{
			get
			{
				return this.mNativeStyle.DisabledAlpha;
			}
			set
			{
				this.mNativeStyle.DisabledAlpha = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x0000344A File Offset: 0x0000164A
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x0000346A File Offset: 0x0000166A
		public bool AntiAliasedLines
		{
			get
			{
				return BitConverter.ToBoolean(new byte[]
				{
					this.mNativeStyle.AntiAliasedLines
				});
			}
			set
			{
				this.mNativeStyle.AntiAliasedLines = ((value > false) ? 1 : 0);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x0000347C File Offset: 0x0000167C
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x0000349C File Offset: 0x0000169C
		public bool AntiAliasedLinesUseTex
		{
			get
			{
				return BitConverter.ToBoolean(new byte[]
				{
					this.mNativeStyle.AntiAliasedLinesUseTex
				});
			}
			set
			{
				this.mNativeStyle.AntiAliasedLinesUseTex = ((value > false) ? 1 : 0);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000034AE File Offset: 0x000016AE
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x000034CE File Offset: 0x000016CE
		public bool AntiAliasedFill
		{
			get
			{
				return BitConverter.ToBoolean(new byte[]
				{
					this.mNativeStyle.AntiAliasedFill
				});
			}
			set
			{
				this.mNativeStyle.AntiAliasedFill = ((value > false) ? 1 : 0);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000034E0 File Offset: 0x000016E0
		// (set) Token: 0x060000AB RID: 171 RVA: 0x000034ED File Offset: 0x000016ED
		public float CurveTessellationTol
		{
			get
			{
				return this.mNativeStyle.CurveTessellationTol;
			}
			set
			{
				this.mNativeStyle.CurveTessellationTol = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000034FB File Offset: 0x000016FB
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00003508 File Offset: 0x00001708
		public float CircleTessellationMaxError
		{
			get
			{
				return this.mNativeStyle.CircleTessellationMaxError;
			}
			set
			{
				this.mNativeStyle.CircleTessellationMaxError = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00003516 File Offset: 0x00001716
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00003523 File Offset: 0x00001723
		public float HoverStationaryDelay
		{
			get
			{
				return this.mNativeStyle.HoverStationaryDelay;
			}
			set
			{
				this.mNativeStyle.HoverStationaryDelay = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00003531 File Offset: 0x00001731
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x0000353E File Offset: 0x0000173E
		public float HoverDelayShort
		{
			get
			{
				return this.mNativeStyle.HoverDelayShort;
			}
			set
			{
				this.mNativeStyle.HoverDelayShort = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x0000354C File Offset: 0x0000174C
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x00003559 File Offset: 0x00001759
		public float HoverDelayNormal
		{
			get
			{
				return this.mNativeStyle.HoverDelayNormal;
			}
			set
			{
				this.mNativeStyle.HoverDelayNormal = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00003567 File Offset: 0x00001767
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00003574 File Offset: 0x00001774
		public ImGuiHoveredFlags HoverFlagsForTooltipMouse
		{
			get
			{
				return this.mNativeStyle.HoverFlagsForTooltipMouse;
			}
			set
			{
				this.mNativeStyle.HoverFlagsForTooltipMouse = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00003582 File Offset: 0x00001782
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x0000358F File Offset: 0x0000178F
		public ImGuiHoveredFlags HoverFlagsForTooltipNav
		{
			get
			{
				return this.mNativeStyle.HoverFlagsForTooltipNav;
			}
			set
			{
				this.mNativeStyle.HoverFlagsForTooltipNav = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000359D File Offset: 0x0000179D
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x000035AA File Offset: 0x000017AA
		public ImGuiDir WindowMenuButtonPosition
		{
			get
			{
				return this.mNativeStyle.WindowMenuButtonPosition;
			}
			set
			{
				this.mNativeStyle.WindowMenuButtonPosition = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000035B8 File Offset: 0x000017B8
		// (set) Token: 0x060000BB RID: 187 RVA: 0x000035C5 File Offset: 0x000017C5
		public ImGuiDir ColorButtonPosition
		{
			get
			{
				return this.mNativeStyle.ColorButtonPosition;
			}
			set
			{
				this.mNativeStyle.ColorButtonPosition = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000035D3 File Offset: 0x000017D3
		// (set) Token: 0x060000BD RID: 189 RVA: 0x000035E0 File Offset: 0x000017E0
		public float LogSliderDeadzone
		{
			get
			{
				return this.mNativeStyle.LogSliderDeadzone;
			}
			set
			{
				this.mNativeStyle.LogSliderDeadzone = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000BE RID: 190 RVA: 0x000035EE File Offset: 0x000017EE
		// (set) Token: 0x060000BF RID: 191 RVA: 0x000035FB File Offset: 0x000017FB
		public float TabMinWidthForCloseButton
		{
			get
			{
				return this.mNativeStyle.TabMinWidthForCloseButton;
			}
			set
			{
				this.mNativeStyle.TabMinWidthForCloseButton = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00003609 File Offset: 0x00001809
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00003616 File Offset: 0x00001816
		public float MouseCursorScale
		{
			get
			{
				return this.mNativeStyle.MouseCursorScale;
			}
			set
			{
				this.mNativeStyle.MouseCursorScale = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00003624 File Offset: 0x00001824
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x0000362C File Offset: 0x0000182C
		[JsonIgnore]
		public Value4 Colors
		{
			private get
			{
				return this.ColorBackgroundWindow;
			}
			set
			{
				this.ColorBackgroundWindow = value;
				this.ColorBackgroundChild = value;
				this.ColorBackgroundPopup = value;
				this.ColorBackgroundFrame = value;
				this.ColorBackgroundFrameHovered = value;
				this.ColorBackgroundFrameActive = value;
				this.ColorBackgroundTitle = value;
				this.ColorBackgroundTitleActive = value;
				this.ColorBackgroundTitleCollapsed = value;
				this.ColorBackgroundMenuBar = value;
				this.ColorBackgroundScrollbar = value;
				this.ColorBackgroundDockingEmpty = value;
				this.ColorBackgroundTableHeader = value;
				this.ColorBackgroundTableRow = value;
				this.ColorBackgroundTableRowAlt = value;
				this.ColorBackgroundTextSelected = value;
				this.ColorBackgroundNavWindowingDim = value;
				this.ColorBackgroundModalWindowDim = value;
				this.ColorText = value;
				this.ColorTextDisabled = value;
				this.ColorBorder = value;
				this.ColorBorderShadow = value;
				this.ColorTableBorderStrong = value;
				this.ColorTableBorderLight = value;
				this.ColorScrollbarGrab = value;
				this.ColorScrollbarGrabHovered = value;
				this.ColorScrollbarGrabActive = value;
				this.ColorSliderGrab = value;
				this.ColorSliderGrabActive = value;
				this.ColorButton = value;
				this.ColorButtonHovered = value;
				this.ColorButtonActive = value;
				this.ColorHeader = value;
				this.ColorHeaderHovered = value;
				this.ColorHeaderActive = value;
				this.ColorSeparator = value;
				this.ColorSeparatorHovered = value;
				this.ColorSeparatorActive = value;
				this.ColorResizeGrip = value;
				this.ColorResizeGripHovered = value;
				this.ColorResizeGripActive = value;
				this.ColorTab = value;
				this.ColorTabHovered = value;
				this.ColorTabActive = value;
				this.ColorTabUnfocused = value;
				this.ColorTabUnfocusedActive = value;
				this.ColorPlotLines = value;
				this.ColorPlotLinesHovered = value;
				this.ColorPlotHistogram = value;
				this.ColorPlotHistogramHovered = value;
				this.ColorCheckMark = value;
				this.ColorDockingPreview = value;
				this.ColorDragDropTarget = value;
				this.ColorNavHighlight = value;
				this.ColorNavWindowingHighlight = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000037BA File Offset: 0x000019BA
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x000037C4 File Offset: 0x000019C4
		[JsonIgnore]
		public Value4 ColorsActive
		{
			private get
			{
				return this.ColorBackgroundFrameActive;
			}
			set
			{
				this.ColorBackgroundFrameActive = value;
				this.ColorBackgroundTitleActive = value;
				this.ColorScrollbarGrabActive = value;
				this.ColorSliderGrabActive = value;
				this.ColorButtonActive = value;
				this.ColorHeaderActive = value;
				this.ColorSeparatorActive = value;
				this.ColorResizeGripActive = value;
				this.ColorTabActive = value;
				this.ColorTabUnfocusedActive = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00003817 File Offset: 0x00001A17
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00003820 File Offset: 0x00001A20
		[JsonIgnore]
		public Value4 ColorsHovered
		{
			private get
			{
				return this.ColorBackgroundFrameHovered;
			}
			set
			{
				this.ColorBackgroundFrameHovered = value;
				this.ColorScrollbarGrabHovered = value;
				this.ColorButtonHovered = value;
				this.ColorHeaderHovered = value;
				this.ColorSeparatorHovered = value;
				this.ColorResizeGripHovered = value;
				this.ColorTabHovered = value;
				this.ColorPlotLinesHovered = value;
				this.ColorPlotHistogramHovered = value;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x0000386C File Offset: 0x00001A6C
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00003874 File Offset: 0x00001A74
		[JsonIgnore]
		public Value4 ColorsBackground
		{
			private get
			{
				return this.ColorBackgroundWindow;
			}
			set
			{
				this.ColorBackgroundWindow = value;
				this.ColorBackgroundChild = value;
				this.ColorBackgroundPopup = value;
				this.ColorBackgroundFrame = value;
				this.ColorBackgroundFrameHovered = value;
				this.ColorBackgroundFrameActive = value;
				this.ColorBackgroundTitle = value;
				this.ColorBackgroundTitleActive = value;
				this.ColorBackgroundTitleCollapsed = value;
				this.ColorBackgroundMenuBar = value;
				this.ColorBackgroundScrollbar = value;
				this.ColorBackgroundDockingEmpty = value;
				this.ColorBackgroundTableHeader = value;
				this.ColorBackgroundTableRow = value;
				this.ColorBackgroundTableRowAlt = value;
				this.ColorBackgroundTextSelected = value;
				this.ColorBackgroundNavWindowingDim = value;
				this.ColorBackgroundModalWindowDim = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000CA RID: 202 RVA: 0x000038FF File Offset: 0x00001AFF
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00003907 File Offset: 0x00001B07
		[JsonIgnore]
		public Value4 ColorsBackgroundActive
		{
			private get
			{
				return this.ColorBackgroundFrameActive;
			}
			set
			{
				this.ColorBackgroundFrameActive = value;
				this.ColorBackgroundTitleActive = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00003917 File Offset: 0x00001B17
		// (set) Token: 0x060000CD RID: 205 RVA: 0x0000391F File Offset: 0x00001B1F
		[JsonIgnore]
		public Value4 ColorsBackgroundHovered
		{
			private get
			{
				return this.ColorBackgroundFrameHovered;
			}
			set
			{
				this.ColorBackgroundFrameHovered = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00003928 File Offset: 0x00001B28
		// (set) Token: 0x060000CF RID: 207 RVA: 0x0000393A File Offset: 0x00001B3A
		public Value4 ColorBackgroundWindow
		{
			get
			{
				return this.mNativeStyle.Colors_2;
			}
			set
			{
				this.mNativeStyle.Colors_2 = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x0000394D File Offset: 0x00001B4D
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x0000395F File Offset: 0x00001B5F
		public Value4 ColorBackgroundChild
		{
			get
			{
				return this.mNativeStyle.Colors_3;
			}
			set
			{
				this.mNativeStyle.Colors_3 = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00003972 File Offset: 0x00001B72
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00003984 File Offset: 0x00001B84
		public Value4 ColorBackgroundPopup
		{
			get
			{
				return this.mNativeStyle.Colors_4;
			}
			set
			{
				this.mNativeStyle.Colors_4 = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00003997 File Offset: 0x00001B97
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x000039A9 File Offset: 0x00001BA9
		public Value4 ColorBackgroundFrame
		{
			get
			{
				return this.mNativeStyle.Colors_7;
			}
			set
			{
				this.mNativeStyle.Colors_7 = value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x000039BC File Offset: 0x00001BBC
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x000039CE File Offset: 0x00001BCE
		public Value4 ColorBackgroundFrameHovered
		{
			get
			{
				return this.mNativeStyle.Colors_8;
			}
			set
			{
				this.mNativeStyle.Colors_8 = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x000039E1 File Offset: 0x00001BE1
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x000039F3 File Offset: 0x00001BF3
		public Value4 ColorBackgroundFrameActive
		{
			get
			{
				return this.mNativeStyle.Colors_9;
			}
			set
			{
				this.mNativeStyle.Colors_9 = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00003A06 File Offset: 0x00001C06
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00003A18 File Offset: 0x00001C18
		public Value4 ColorBackgroundTitle
		{
			get
			{
				return this.mNativeStyle.Colors_10;
			}
			set
			{
				this.mNativeStyle.Colors_10 = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00003A2B File Offset: 0x00001C2B
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00003A3D File Offset: 0x00001C3D
		public Value4 ColorBackgroundTitleActive
		{
			get
			{
				return this.mNativeStyle.Colors_11;
			}
			set
			{
				this.mNativeStyle.Colors_11 = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00003A50 File Offset: 0x00001C50
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00003A62 File Offset: 0x00001C62
		public Value4 ColorBackgroundTitleCollapsed
		{
			get
			{
				return this.mNativeStyle.Colors_12;
			}
			set
			{
				this.mNativeStyle.Colors_12 = value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00003A75 File Offset: 0x00001C75
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00003A87 File Offset: 0x00001C87
		public Value4 ColorBackgroundMenuBar
		{
			get
			{
				return this.mNativeStyle.Colors_13;
			}
			set
			{
				this.mNativeStyle.Colors_13 = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00003A9A File Offset: 0x00001C9A
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00003AAC File Offset: 0x00001CAC
		public Value4 ColorBackgroundScrollbar
		{
			get
			{
				return this.mNativeStyle.Colors_14;
			}
			set
			{
				this.mNativeStyle.Colors_14 = value;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00003ABF File Offset: 0x00001CBF
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00003AD1 File Offset: 0x00001CD1
		public Value4 ColorBackgroundDockingEmpty
		{
			get
			{
				return this.mNativeStyle.Colors_39;
			}
			set
			{
				this.mNativeStyle.Colors_39 = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00003AE4 File Offset: 0x00001CE4
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00003AF6 File Offset: 0x00001CF6
		public Value4 ColorBackgroundTableHeader
		{
			get
			{
				return this.mNativeStyle.Colors_44;
			}
			set
			{
				this.mNativeStyle.Colors_44 = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00003B09 File Offset: 0x00001D09
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00003B1B File Offset: 0x00001D1B
		public Value4 ColorBackgroundTableRow
		{
			get
			{
				return this.mNativeStyle.Colors_47;
			}
			set
			{
				this.mNativeStyle.Colors_47 = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00003B2E File Offset: 0x00001D2E
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00003B40 File Offset: 0x00001D40
		public Value4 ColorBackgroundTableRowAlt
		{
			get
			{
				return this.mNativeStyle.Colors_48;
			}
			set
			{
				this.mNativeStyle.Colors_48 = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00003B53 File Offset: 0x00001D53
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00003B65 File Offset: 0x00001D65
		public Value4 ColorBackgroundTextSelected
		{
			get
			{
				return this.mNativeStyle.Colors_49;
			}
			set
			{
				this.mNativeStyle.Colors_49 = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00003B78 File Offset: 0x00001D78
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00003B8A File Offset: 0x00001D8A
		public Value4 ColorBackgroundNavWindowingDim
		{
			get
			{
				return this.mNativeStyle.Colors_53;
			}
			set
			{
				this.mNativeStyle.Colors_53 = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00003B9D File Offset: 0x00001D9D
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00003BAF File Offset: 0x00001DAF
		public Value4 ColorBackgroundModalWindowDim
		{
			get
			{
				return this.mNativeStyle.Colors_54;
			}
			set
			{
				this.mNativeStyle.Colors_54 = value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00003BC2 File Offset: 0x00001DC2
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00003BD4 File Offset: 0x00001DD4
		public Value4 ColorText
		{
			get
			{
				return this.mNativeStyle.Colors_0;
			}
			set
			{
				this.mNativeStyle.Colors_0 = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00003BE7 File Offset: 0x00001DE7
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00003BF9 File Offset: 0x00001DF9
		public Value4 ColorTextDisabled
		{
			get
			{
				return this.mNativeStyle.Colors_1;
			}
			set
			{
				this.mNativeStyle.Colors_1 = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00003C0C File Offset: 0x00001E0C
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00003C1E File Offset: 0x00001E1E
		public Value4 ColorBorder
		{
			get
			{
				return this.mNativeStyle.Colors_5;
			}
			set
			{
				this.mNativeStyle.Colors_5 = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00003C31 File Offset: 0x00001E31
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00003C43 File Offset: 0x00001E43
		public Value4 ColorBorderShadow
		{
			get
			{
				return this.mNativeStyle.Colors_6;
			}
			set
			{
				this.mNativeStyle.Colors_6 = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00003C56 File Offset: 0x00001E56
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00003C68 File Offset: 0x00001E68
		public Value4 ColorTableBorderStrong
		{
			get
			{
				return this.mNativeStyle.Colors_45;
			}
			set
			{
				this.mNativeStyle.Colors_45 = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00003C7B File Offset: 0x00001E7B
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00003C8D File Offset: 0x00001E8D
		public Value4 ColorTableBorderLight
		{
			get
			{
				return this.mNativeStyle.Colors_46;
			}
			set
			{
				this.mNativeStyle.Colors_46 = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00003CA0 File Offset: 0x00001EA0
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00003CB2 File Offset: 0x00001EB2
		public Value4 ColorScrollbarGrab
		{
			get
			{
				return this.mNativeStyle.Colors_15;
			}
			set
			{
				this.mNativeStyle.Colors_15 = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00003CC5 File Offset: 0x00001EC5
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00003CD7 File Offset: 0x00001ED7
		public Value4 ColorScrollbarGrabHovered
		{
			get
			{
				return this.mNativeStyle.Colors_16;
			}
			set
			{
				this.mNativeStyle.Colors_16 = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00003CEA File Offset: 0x00001EEA
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00003CFC File Offset: 0x00001EFC
		public Value4 ColorScrollbarGrabActive
		{
			get
			{
				return this.mNativeStyle.Colors_17;
			}
			set
			{
				this.mNativeStyle.Colors_17 = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00003D0F File Offset: 0x00001F0F
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00003D21 File Offset: 0x00001F21
		public Value4 ColorSliderGrab
		{
			get
			{
				return this.mNativeStyle.Colors_19;
			}
			set
			{
				this.mNativeStyle.Colors_19 = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00003D34 File Offset: 0x00001F34
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00003D46 File Offset: 0x00001F46
		public Value4 ColorSliderGrabActive
		{
			get
			{
				return this.mNativeStyle.Colors_20;
			}
			set
			{
				this.mNativeStyle.Colors_20 = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00003D59 File Offset: 0x00001F59
		// (set) Token: 0x06000109 RID: 265 RVA: 0x00003D6B File Offset: 0x00001F6B
		public Value4 ColorButton
		{
			get
			{
				return this.mNativeStyle.Colors_21;
			}
			set
			{
				this.mNativeStyle.Colors_21 = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00003D7E File Offset: 0x00001F7E
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00003D90 File Offset: 0x00001F90
		public Value4 ColorButtonHovered
		{
			get
			{
				return this.mNativeStyle.Colors_22;
			}
			set
			{
				this.mNativeStyle.Colors_22 = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00003DA3 File Offset: 0x00001FA3
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00003DB5 File Offset: 0x00001FB5
		public Value4 ColorButtonActive
		{
			get
			{
				return this.mNativeStyle.Colors_23;
			}
			set
			{
				this.mNativeStyle.Colors_23 = value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00003DC8 File Offset: 0x00001FC8
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00003DDA File Offset: 0x00001FDA
		public Value4 ColorHeader
		{
			get
			{
				return this.mNativeStyle.Colors_24;
			}
			set
			{
				this.mNativeStyle.Colors_24 = value;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00003DED File Offset: 0x00001FED
		// (set) Token: 0x06000111 RID: 273 RVA: 0x00003DFF File Offset: 0x00001FFF
		public Value4 ColorHeaderHovered
		{
			get
			{
				return this.mNativeStyle.Colors_25;
			}
			set
			{
				this.mNativeStyle.Colors_25 = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00003E12 File Offset: 0x00002012
		// (set) Token: 0x06000113 RID: 275 RVA: 0x00003E24 File Offset: 0x00002024
		public Value4 ColorHeaderActive
		{
			get
			{
				return this.mNativeStyle.Colors_26;
			}
			set
			{
				this.mNativeStyle.Colors_26 = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00003E37 File Offset: 0x00002037
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00003E49 File Offset: 0x00002049
		public Value4 ColorSeparator
		{
			get
			{
				return this.mNativeStyle.Colors_27;
			}
			set
			{
				this.mNativeStyle.Colors_27 = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00003E5C File Offset: 0x0000205C
		// (set) Token: 0x06000117 RID: 279 RVA: 0x00003E6E File Offset: 0x0000206E
		public Value4 ColorSeparatorHovered
		{
			get
			{
				return this.mNativeStyle.Colors_28;
			}
			set
			{
				this.mNativeStyle.Colors_28 = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00003E81 File Offset: 0x00002081
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00003E93 File Offset: 0x00002093
		public Value4 ColorSeparatorActive
		{
			get
			{
				return this.mNativeStyle.Colors_29;
			}
			set
			{
				this.mNativeStyle.Colors_29 = value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00003EA6 File Offset: 0x000020A6
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00003EB8 File Offset: 0x000020B8
		public Value4 ColorResizeGrip
		{
			get
			{
				return this.mNativeStyle.Colors_30;
			}
			set
			{
				this.mNativeStyle.Colors_30 = value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00003ECB File Offset: 0x000020CB
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00003EDD File Offset: 0x000020DD
		public Value4 ColorResizeGripHovered
		{
			get
			{
				return this.mNativeStyle.Colors_31;
			}
			set
			{
				this.mNativeStyle.Colors_31 = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00003EF0 File Offset: 0x000020F0
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00003F02 File Offset: 0x00002102
		public Value4 ColorResizeGripActive
		{
			get
			{
				return this.mNativeStyle.Colors_32;
			}
			set
			{
				this.mNativeStyle.Colors_32 = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00003F15 File Offset: 0x00002115
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00003F27 File Offset: 0x00002127
		public Value4 ColorTab
		{
			get
			{
				return this.mNativeStyle.Colors_33;
			}
			set
			{
				this.mNativeStyle.Colors_33 = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00003F3A File Offset: 0x0000213A
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00003F4C File Offset: 0x0000214C
		public Value4 ColorTabHovered
		{
			get
			{
				return this.mNativeStyle.Colors_34;
			}
			set
			{
				this.mNativeStyle.Colors_34 = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00003F5F File Offset: 0x0000215F
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00003F71 File Offset: 0x00002171
		public Value4 ColorTabActive
		{
			get
			{
				return this.mNativeStyle.Colors_35;
			}
			set
			{
				this.mNativeStyle.Colors_35 = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00003F84 File Offset: 0x00002184
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00003F96 File Offset: 0x00002196
		public Value4 ColorTabUnfocused
		{
			get
			{
				return this.mNativeStyle.Colors_36;
			}
			set
			{
				this.mNativeStyle.Colors_36 = value;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00003FA9 File Offset: 0x000021A9
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00003FBB File Offset: 0x000021BB
		public Value4 ColorTabUnfocusedActive
		{
			get
			{
				return this.mNativeStyle.Colors_37;
			}
			set
			{
				this.mNativeStyle.Colors_37 = value;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00003FCE File Offset: 0x000021CE
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00003FE0 File Offset: 0x000021E0
		public Value4 ColorPlotLines
		{
			get
			{
				return this.mNativeStyle.Colors_40;
			}
			set
			{
				this.mNativeStyle.Colors_40 = value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00003FF3 File Offset: 0x000021F3
		// (set) Token: 0x0600012D RID: 301 RVA: 0x00004005 File Offset: 0x00002205
		public Value4 ColorPlotLinesHovered
		{
			get
			{
				return this.mNativeStyle.Colors_41;
			}
			set
			{
				this.mNativeStyle.Colors_41 = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00004018 File Offset: 0x00002218
		// (set) Token: 0x0600012F RID: 303 RVA: 0x0000402A File Offset: 0x0000222A
		public Value4 ColorPlotHistogram
		{
			get
			{
				return this.mNativeStyle.Colors_42;
			}
			set
			{
				this.mNativeStyle.Colors_42 = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000130 RID: 304 RVA: 0x0000403D File Offset: 0x0000223D
		// (set) Token: 0x06000131 RID: 305 RVA: 0x0000404F File Offset: 0x0000224F
		public Value4 ColorPlotHistogramHovered
		{
			get
			{
				return this.mNativeStyle.Colors_43;
			}
			set
			{
				this.mNativeStyle.Colors_43 = value;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00004062 File Offset: 0x00002262
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00004074 File Offset: 0x00002274
		public Value4 ColorCheckMark
		{
			get
			{
				return this.mNativeStyle.Colors_18;
			}
			set
			{
				this.mNativeStyle.Colors_18 = value;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00004087 File Offset: 0x00002287
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00004099 File Offset: 0x00002299
		public Value4 ColorDockingPreview
		{
			get
			{
				return this.mNativeStyle.Colors_38;
			}
			set
			{
				this.mNativeStyle.Colors_38 = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000136 RID: 310 RVA: 0x000040AC File Offset: 0x000022AC
		// (set) Token: 0x06000137 RID: 311 RVA: 0x000040BE File Offset: 0x000022BE
		public Value4 ColorDragDropTarget
		{
			get
			{
				return this.mNativeStyle.Colors_50;
			}
			set
			{
				this.mNativeStyle.Colors_50 = value;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000138 RID: 312 RVA: 0x000040D1 File Offset: 0x000022D1
		// (set) Token: 0x06000139 RID: 313 RVA: 0x000040E3 File Offset: 0x000022E3
		public Value4 ColorNavHighlight
		{
			get
			{
				return this.mNativeStyle.Colors_51;
			}
			set
			{
				this.mNativeStyle.Colors_51 = value;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600013A RID: 314 RVA: 0x000040F6 File Offset: 0x000022F6
		// (set) Token: 0x0600013B RID: 315 RVA: 0x00004108 File Offset: 0x00002308
		public Value4 ColorNavWindowingHighlight
		{
			get
			{
				return this.mNativeStyle.Colors_52;
			}
			set
			{
				this.mNativeStyle.Colors_52 = value;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000411B File Offset: 0x0000231B
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00004124 File Offset: 0x00002324
		[TupleElementNames(new string[]
		{
			"name",
			"size"
		})]
		[Nullable(new byte[]
		{
			0,
			1
		})]
		[JsonIgnore]
		public ValueTuple<string, int> Font
		{
			[return: TupleElementNames(new string[]
			{
				"name",
				"size"
			})]
			[return: Nullable(new byte[]
			{
				0,
				1
			})]
			get
			{
				return this.mFontKey;
			}
			[param: TupleElementNames(new string[]
			{
				"name",
				"size"
			})]
			[param: Nullable(new byte[]
			{
				0,
				1
			})]
			set
			{
				if (value.Item1 == null || value.Item1 == "")
				{
					return;
				}
				this.mFontKey = value;
				this.mFontLoaded = FontManager.Loaded.ContainsKey(value);
				if (this.mFontLoaded)
				{
					this.mFont = FontManager.Loaded[value];
				}
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000417D File Offset: 0x0000237D
		// (set) Token: 0x0600013F RID: 319 RVA: 0x0000418A File Offset: 0x0000238A
		public string FontName
		{
			get
			{
				return this.Font.Item1;
			}
			set
			{
				this.Font = new ValueTuple<string, int>(value, this.FontSize);
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000140 RID: 320 RVA: 0x0000419E File Offset: 0x0000239E
		// (set) Token: 0x06000141 RID: 321 RVA: 0x000041AB File Offset: 0x000023AB
		public int FontSize
		{
			get
			{
				return this.Font.Item2;
			}
			set
			{
				this.Font = new ValueTuple<string, int>(this.FontName, value);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000142 RID: 322 RVA: 0x000041BF File Offset: 0x000023BF
		[JsonIgnore]
		public bool FontLoaded
		{
			get
			{
				return this.mFontLoaded;
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000041C7 File Offset: 0x000023C7
		public Style(Style style) : this(style.NativeStyle, style.FontName, style.FontSize)
		{
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000041E1 File Offset: 0x000023E1
		public Style(ImGuiStyle style, string fontName, int fontSize = 14)
		{
			this.mNativeStyle = style;
			this.Font = new ValueTuple<string, int>(fontName, fontSize);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00004200 File Offset: 0x00002400
		public unsafe Style(string fontName, int fontSize = 14)
		{
			this.mNativeStyle = *ImGui.GetStyle().NativePtr;
			this.Font = new ValueTuple<string, int>(fontName, fontSize);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00004238 File Offset: 0x00002438
		public unsafe Style()
		{
			this.mNativeStyle = *ImGui.GetStyle().NativePtr;
			this.Font = new ValueTuple<string, int>("", 14);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00004278 File Offset: 0x00002478
		internal unsafe void Push()
		{
			if (this.mPushed)
			{
				return;
			}
			this.mPushed = true;
			this.mPrevStyle = *ImGui.GetStyle().NativePtr;
			if (this.FontLoaded)
			{
				ImGui.PushFont(this.mFont);
				this.mFontPushed = true;
			}
			Style.ApplyStyle(this.mNativeStyle);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000042D2 File Offset: 0x000024D2
		internal void Pop()
		{
			if (!this.mPushed)
			{
				return;
			}
			this.mPushed = false;
			if (this.mFontPushed)
			{
				ImGui.PopFont();
				this.mFontPushed = false;
			}
			Style.ApplyStyle(this.mPrevStyle);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00004304 File Offset: 0x00002504
		internal unsafe static void ApplyStyle(in ImGuiStyle style)
		{
			ref ImGuiStyle currentStyle = ref *ImGui.GetStyle().NativePtr;
			currentStyle.Alpha = style.Alpha;
			currentStyle.DisabledAlpha = style.DisabledAlpha;
			currentStyle.WindowPadding = style.WindowPadding;
			currentStyle.WindowRounding = style.WindowRounding;
			currentStyle.WindowBorderSize = style.WindowBorderSize;
			currentStyle.WindowMinSize = style.WindowMinSize;
			currentStyle.WindowTitleAlign = style.WindowTitleAlign;
			currentStyle.WindowMenuButtonPosition = style.WindowMenuButtonPosition;
			currentStyle.ChildRounding = style.ChildRounding;
			currentStyle.ChildBorderSize = style.ChildBorderSize;
			currentStyle.PopupRounding = style.PopupRounding;
			currentStyle.PopupBorderSize = style.PopupBorderSize;
			currentStyle.FramePadding = style.FramePadding;
			currentStyle.FrameRounding = style.FrameRounding;
			currentStyle.FrameBorderSize = style.FrameBorderSize;
			currentStyle.ItemSpacing = style.ItemSpacing;
			currentStyle.ItemInnerSpacing = style.ItemInnerSpacing;
			currentStyle.CellPadding = style.CellPadding;
			currentStyle.TouchExtraPadding = style.TouchExtraPadding;
			currentStyle.IndentSpacing = style.IndentSpacing;
			currentStyle.ColumnsMinSpacing = style.ColumnsMinSpacing;
			currentStyle.ScrollbarSize = style.ScrollbarSize;
			currentStyle.ScrollbarRounding = style.ScrollbarRounding;
			currentStyle.GrabMinSize = style.GrabMinSize;
			currentStyle.GrabRounding = style.GrabRounding;
			currentStyle.LogSliderDeadzone = style.LogSliderDeadzone;
			currentStyle.TabRounding = style.TabRounding;
			currentStyle.TabBorderSize = style.TabBorderSize;
			currentStyle.TabMinWidthForCloseButton = style.TabMinWidthForCloseButton;
			currentStyle.ColorButtonPosition = style.ColorButtonPosition;
			currentStyle.ButtonTextAlign = style.ButtonTextAlign;
			currentStyle.SelectableTextAlign = style.SelectableTextAlign;
			currentStyle.SeparatorTextBorderSize = style.SeparatorTextBorderSize;
			currentStyle.SeparatorTextAlign = style.SeparatorTextAlign;
			currentStyle.SeparatorTextPadding = style.SeparatorTextPadding;
			currentStyle.DisplayWindowPadding = style.DisplayWindowPadding;
			currentStyle.DisplaySafeAreaPadding = style.DisplaySafeAreaPadding;
			currentStyle.MouseCursorScale = style.MouseCursorScale;
			currentStyle.AntiAliasedLines = style.AntiAliasedLines;
			currentStyle.AntiAliasedLinesUseTex = style.AntiAliasedLinesUseTex;
			currentStyle.AntiAliasedFill = style.AntiAliasedFill;
			currentStyle.CurveTessellationTol = style.CurveTessellationTol;
			currentStyle.CircleTessellationMaxError = style.CircleTessellationMaxError;
			currentStyle.Colors_0 = style.Colors_0;
			currentStyle.Colors_1 = style.Colors_1;
			currentStyle.Colors_2 = style.Colors_2;
			currentStyle.Colors_3 = style.Colors_3;
			currentStyle.Colors_4 = style.Colors_4;
			currentStyle.Colors_5 = style.Colors_5;
			currentStyle.Colors_6 = style.Colors_6;
			currentStyle.Colors_7 = style.Colors_7;
			currentStyle.Colors_8 = style.Colors_8;
			currentStyle.Colors_9 = style.Colors_9;
			currentStyle.Colors_10 = style.Colors_10;
			currentStyle.Colors_11 = style.Colors_11;
			currentStyle.Colors_12 = style.Colors_12;
			currentStyle.Colors_13 = style.Colors_13;
			currentStyle.Colors_14 = style.Colors_14;
			currentStyle.Colors_15 = style.Colors_15;
			currentStyle.Colors_16 = style.Colors_16;
			currentStyle.Colors_17 = style.Colors_17;
			currentStyle.Colors_18 = style.Colors_18;
			currentStyle.Colors_19 = style.Colors_19;
			currentStyle.Colors_20 = style.Colors_20;
			currentStyle.Colors_21 = style.Colors_21;
			currentStyle.Colors_22 = style.Colors_22;
			currentStyle.Colors_23 = style.Colors_23;
			currentStyle.Colors_24 = style.Colors_24;
			currentStyle.Colors_25 = style.Colors_25;
			currentStyle.Colors_26 = style.Colors_26;
			currentStyle.Colors_27 = style.Colors_27;
			currentStyle.Colors_28 = style.Colors_28;
			currentStyle.Colors_29 = style.Colors_29;
			currentStyle.Colors_30 = style.Colors_30;
			currentStyle.Colors_31 = style.Colors_31;
			currentStyle.Colors_32 = style.Colors_32;
			currentStyle.Colors_33 = style.Colors_33;
			currentStyle.Colors_34 = style.Colors_34;
			currentStyle.Colors_35 = style.Colors_35;
			currentStyle.Colors_36 = style.Colors_36;
			currentStyle.Colors_37 = style.Colors_37;
			currentStyle.Colors_38 = style.Colors_38;
			currentStyle.Colors_39 = style.Colors_39;
			currentStyle.Colors_40 = style.Colors_40;
			currentStyle.Colors_41 = style.Colors_41;
			currentStyle.Colors_42 = style.Colors_42;
			currentStyle.Colors_43 = style.Colors_43;
			currentStyle.Colors_44 = style.Colors_44;
			currentStyle.Colors_45 = style.Colors_45;
			currentStyle.Colors_46 = style.Colors_46;
			currentStyle.Colors_47 = style.Colors_47;
			currentStyle.Colors_48 = style.Colors_48;
			currentStyle.Colors_49 = style.Colors_49;
			currentStyle.Colors_50 = style.Colors_50;
			currentStyle.Colors_51 = style.Colors_51;
			currentStyle.Colors_52 = style.Colors_52;
			currentStyle.Colors_53 = style.Colors_53;
			currentStyle.Colors_54 = style.Colors_54;
			currentStyle.HoverStationaryDelay = style.HoverStationaryDelay;
			currentStyle.HoverDelayShort = style.HoverDelayShort;
			currentStyle.HoverDelayNormal = style.HoverDelayNormal;
			currentStyle.HoverFlagsForTooltipMouse = style.HoverFlagsForTooltipMouse;
			currentStyle.HoverFlagsForTooltipNav = style.HoverFlagsForTooltipNav;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000047F4 File Offset: 0x000029F4
		public virtual void SetFrom(Style style)
		{
			this.mPrevStyle = style.mPrevStyle;
			this.mNativeStyle = style.mNativeStyle;
			this.mFont = style.mFont;
			if (style.mFontKey.Item1 != null)
			{
				this.mFontKey = new ValueTuple<string, int>((string)style.mFontKey.Item1.Clone(), style.mFontKey.Item2);
			}
			this.mPushed = style.mPushed;
			this.mFontPushed = style.mFontPushed;
			this.mFontLoaded = style.mFontLoaded;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00004884 File Offset: 0x00002A84
		public virtual string ToCode()
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine("VSImGui.Style style = new();");
			this.OtherPropertiesToCode(result);
			this.FloatPropertiesToCode(result);
			this.Value2PropertiesToCode(result);
			this.Value4PropertiesToCode(result);
			return result.ToString();
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000048C8 File Offset: 0x00002AC8
		protected void FloatPropertiesToCode(StringBuilder result)
		{
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.IndentSpacing, this.mNativeStyle.IndentSpacing))
			{
				appendInterpolatedStringHandler..ctor(24, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.SpacingIndent = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.SpacingIndent);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.ColumnsMinSpacing, this.mNativeStyle.ColumnsMinSpacing))
			{
				appendInterpolatedStringHandler..ctor(28, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.SpacingColumnsMin = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.SpacingColumnsMin);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.WindowBorderSize, this.mNativeStyle.WindowBorderSize))
			{
				appendInterpolatedStringHandler..ctor(23, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.BorderWindow = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.BorderWindow);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.ChildBorderSize, this.mNativeStyle.ChildBorderSize))
			{
				appendInterpolatedStringHandler..ctor(22, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.BorderChild = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.BorderChild);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.PopupBorderSize, this.mNativeStyle.PopupBorderSize))
			{
				appendInterpolatedStringHandler..ctor(22, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.BorderPopup = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.BorderPopup);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.FrameBorderSize, this.mNativeStyle.FrameBorderSize))
			{
				appendInterpolatedStringHandler..ctor(22, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.BorderFrame = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.BorderFrame);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.TabBorderSize, this.mNativeStyle.TabBorderSize))
			{
				appendInterpolatedStringHandler..ctor(20, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.BorderTab = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.BorderTab);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.SeparatorTextBorderSize, this.mNativeStyle.SeparatorTextBorderSize))
			{
				appendInterpolatedStringHandler..ctor(30, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.BorderSeparatorText = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.BorderSeparatorText);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.WindowRounding, this.mNativeStyle.WindowRounding))
			{
				appendInterpolatedStringHandler..ctor(25, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.RoundingWindow = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.RoundingWindow);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.ChildRounding, this.mNativeStyle.ChildRounding))
			{
				appendInterpolatedStringHandler..ctor(24, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.RoundingChild = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.RoundingChild);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.PopupRounding, this.mNativeStyle.PopupRounding))
			{
				appendInterpolatedStringHandler..ctor(24, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.RoundingPopup = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.RoundingPopup);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.FrameRounding, this.mNativeStyle.FrameRounding))
			{
				appendInterpolatedStringHandler..ctor(24, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.RoundingFrame = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.RoundingFrame);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.ScrollbarRounding, this.mNativeStyle.ScrollbarRounding))
			{
				appendInterpolatedStringHandler..ctor(28, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.RoundingScrollbar = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.RoundingScrollbar);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.GrabRounding, this.mNativeStyle.GrabRounding))
			{
				appendInterpolatedStringHandler..ctor(23, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.RoundingGrab = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.RoundingGrab);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.TabRounding, this.mNativeStyle.TabRounding))
			{
				appendInterpolatedStringHandler..ctor(22, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.RoundingTab = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.RoundingTab);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.ScrollbarSize, this.mNativeStyle.ScrollbarSize))
			{
				appendInterpolatedStringHandler..ctor(24, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.SizeScrollbar = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.SizeScrollbar);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.GrabMinSize, this.mNativeStyle.GrabMinSize))
			{
				appendInterpolatedStringHandler..ctor(22, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.SizeGrabMin = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.SizeGrabMin);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.Alpha, this.mNativeStyle.Alpha))
			{
				appendInterpolatedStringHandler..ctor(16, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.Alpha = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.Alpha);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.DisabledAlpha, this.mNativeStyle.DisabledAlpha))
			{
				appendInterpolatedStringHandler..ctor(24, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.DisabledAlpha = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.DisabledAlpha);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.CurveTessellationTol, this.mNativeStyle.CurveTessellationTol))
			{
				appendInterpolatedStringHandler..ctor(31, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.CurveTessellationTol = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.CurveTessellationTol);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.CircleTessellationMaxError, this.mNativeStyle.CircleTessellationMaxError))
			{
				appendInterpolatedStringHandler..ctor(37, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.CircleTessellationMaxError = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.CircleTessellationMaxError);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.HoverStationaryDelay, this.mNativeStyle.HoverStationaryDelay))
			{
				appendInterpolatedStringHandler..ctor(31, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.HoverStationaryDelay = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.HoverStationaryDelay);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.HoverDelayShort, this.mNativeStyle.HoverDelayShort))
			{
				appendInterpolatedStringHandler..ctor(26, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.HoverDelayShort = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.HoverDelayShort);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.HoverDelayNormal, this.mNativeStyle.HoverDelayNormal))
			{
				appendInterpolatedStringHandler..ctor(27, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.HoverDelayNormal = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.HoverDelayNormal);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.LogSliderDeadzone, this.mNativeStyle.LogSliderDeadzone))
			{
				appendInterpolatedStringHandler..ctor(28, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.LogSliderDeadzone = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.LogSliderDeadzone);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.TabMinWidthForCloseButton, this.mNativeStyle.TabMinWidthForCloseButton))
			{
				appendInterpolatedStringHandler..ctor(36, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.TabMinWidthForCloseButton = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.TabMinWidthForCloseButton);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (!ComparisonUtils.CompareFloats(this.mPrevStyle.MouseCursorScale, this.mNativeStyle.MouseCursorScale))
			{
				appendInterpolatedStringHandler..ctor(27, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.MouseCursorScale = ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.MouseCursorScale);
				appendInterpolatedStringHandler.AppendLiteral("f;");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005220 File Offset: 0x00003420
		protected void Value2PropertiesToCode(StringBuilder result)
		{
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
			if (this.mPrevStyle.WindowPadding != this.mNativeStyle.WindowPadding)
			{
				appendInterpolatedStringHandler..ctor(26, 2, result);
				appendInterpolatedStringHandler.AppendLiteral("style.PaddingWindow = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.PaddingWindow.X);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.PaddingWindow.Y);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.FramePadding != this.mNativeStyle.FramePadding)
			{
				appendInterpolatedStringHandler..ctor(25, 2, result);
				appendInterpolatedStringHandler.AppendLiteral("style.PaddingFrame = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.PaddingFrame.X);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.PaddingFrame.Y);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.CellPadding != this.mNativeStyle.CellPadding)
			{
				appendInterpolatedStringHandler..ctor(24, 2, result);
				appendInterpolatedStringHandler.AppendLiteral("style.PaddingCell = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.PaddingCell.X);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.PaddingCell.Y);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.TouchExtraPadding != this.mNativeStyle.TouchExtraPadding)
			{
				appendInterpolatedStringHandler..ctor(30, 2, result);
				appendInterpolatedStringHandler.AppendLiteral("style.PaddingTouchExtra = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.PaddingTouchExtra.X);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.PaddingTouchExtra.Y);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.SeparatorTextPadding != this.mNativeStyle.SeparatorTextPadding)
			{
				appendInterpolatedStringHandler..ctor(33, 2, result);
				appendInterpolatedStringHandler.AppendLiteral("style.PaddingSeparatorText = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.PaddingSeparatorText.X);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.PaddingSeparatorText.Y);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.DisplayWindowPadding != this.mNativeStyle.DisplayWindowPadding)
			{
				appendInterpolatedStringHandler..ctor(33, 2, result);
				appendInterpolatedStringHandler.AppendLiteral("style.PaddingDisplayWindow = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.PaddingDisplayWindow.X);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.PaddingDisplayWindow.Y);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.DisplaySafeAreaPadding != this.mNativeStyle.DisplaySafeAreaPadding)
			{
				appendInterpolatedStringHandler..ctor(35, 2, result);
				appendInterpolatedStringHandler.AppendLiteral("style.PaddingDisplaySafeArea = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.PaddingDisplaySafeArea.X);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.PaddingDisplaySafeArea.Y);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.ItemSpacing != this.mNativeStyle.ItemSpacing)
			{
				appendInterpolatedStringHandler..ctor(24, 2, result);
				appendInterpolatedStringHandler.AppendLiteral("style.SpacingItem = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.SpacingItem.X);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.SpacingItem.Y);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.ItemInnerSpacing != this.mNativeStyle.ItemInnerSpacing)
			{
				appendInterpolatedStringHandler..ctor(29, 2, result);
				appendInterpolatedStringHandler.AppendLiteral("style.SpacingItemInner = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.SpacingItemInner.X);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.SpacingItemInner.Y);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.WindowMinSize != this.mNativeStyle.WindowMinSize)
			{
				appendInterpolatedStringHandler..ctor(26, 2, result);
				appendInterpolatedStringHandler.AppendLiteral("style.SizeWindowMin = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.SizeWindowMin.X);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.SizeWindowMin.Y);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.WindowTitleAlign != this.mNativeStyle.WindowTitleAlign)
			{
				appendInterpolatedStringHandler..ctor(29, 2, result);
				appendInterpolatedStringHandler.AppendLiteral("style.AlignWindowTitle = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.AlignWindowTitle.X);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.AlignWindowTitle.Y);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.ButtonTextAlign != this.mNativeStyle.ButtonTextAlign)
			{
				appendInterpolatedStringHandler..ctor(28, 2, result);
				appendInterpolatedStringHandler.AppendLiteral("style.AlignButtonText = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.AlignButtonText.X);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.AlignButtonText.Y);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.SelectableTextAlign != this.mNativeStyle.SelectableTextAlign)
			{
				appendInterpolatedStringHandler..ctor(32, 2, result);
				appendInterpolatedStringHandler.AppendLiteral("style.AlignSelectableText = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.AlignSelectableText.X);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.AlignSelectableText.Y);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.SeparatorTextAlign != this.mNativeStyle.SeparatorTextAlign)
			{
				appendInterpolatedStringHandler..ctor(31, 2, result);
				appendInterpolatedStringHandler.AppendLiteral("style.AlignSeparatorText = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.AlignSeparatorText.X);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.AlignSeparatorText.Y);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000593C File Offset: 0x00003B3C
		protected void Value4PropertiesToCode(StringBuilder result)
		{
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
			if (this.mPrevStyle.Colors_2 != this.mNativeStyle.Colors_2)
			{
				appendInterpolatedStringHandler..ctor(36, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundWindow = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundWindow.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundWindow.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundWindow.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundWindow.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_3 != this.mNativeStyle.Colors_3)
			{
				appendInterpolatedStringHandler..ctor(35, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundChild = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundChild.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundChild.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundChild.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundChild.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_4 != this.mNativeStyle.Colors_4)
			{
				appendInterpolatedStringHandler..ctor(35, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundPopup = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundPopup.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundPopup.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundPopup.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundPopup.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_7 != this.mNativeStyle.Colors_7)
			{
				appendInterpolatedStringHandler..ctor(35, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundFrame = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundFrame.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundFrame.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundFrame.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundFrame.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_8 != this.mNativeStyle.Colors_8)
			{
				appendInterpolatedStringHandler..ctor(42, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundFrameHovered = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundFrameHovered.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundFrameHovered.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundFrameHovered.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundFrameHovered.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_9 != this.mNativeStyle.Colors_9)
			{
				appendInterpolatedStringHandler..ctor(41, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundFrameActive = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundFrameActive.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundFrameActive.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundFrameActive.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundFrameActive.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_10 != this.mNativeStyle.Colors_10)
			{
				appendInterpolatedStringHandler..ctor(35, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundTitle = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTitle.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTitle.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTitle.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTitle.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_11 != this.mNativeStyle.Colors_11)
			{
				appendInterpolatedStringHandler..ctor(41, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundTitleActive = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTitleActive.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTitleActive.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTitleActive.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTitleActive.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_12 != this.mNativeStyle.Colors_12)
			{
				appendInterpolatedStringHandler..ctor(44, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundTitleCollapsed = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTitleCollapsed.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTitleCollapsed.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTitleCollapsed.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTitleCollapsed.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_13 != this.mNativeStyle.Colors_13)
			{
				appendInterpolatedStringHandler..ctor(37, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundMenuBar = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundMenuBar.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundMenuBar.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundMenuBar.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundMenuBar.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_14 != this.mNativeStyle.Colors_14)
			{
				appendInterpolatedStringHandler..ctor(39, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundScrollbar = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundScrollbar.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundScrollbar.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundScrollbar.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundScrollbar.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_39 != this.mNativeStyle.Colors_39)
			{
				appendInterpolatedStringHandler..ctor(42, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundDockingEmpty = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundDockingEmpty.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundDockingEmpty.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundDockingEmpty.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundDockingEmpty.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_44 != this.mNativeStyle.Colors_44)
			{
				appendInterpolatedStringHandler..ctor(41, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundTableHeader = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTableHeader.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTableHeader.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTableHeader.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTableHeader.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_47 != this.mNativeStyle.Colors_47)
			{
				appendInterpolatedStringHandler..ctor(38, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundTableRow = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTableRow.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTableRow.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTableRow.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTableRow.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_48 != this.mNativeStyle.Colors_48)
			{
				appendInterpolatedStringHandler..ctor(41, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundTableRowAlt = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTableRowAlt.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTableRowAlt.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTableRowAlt.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTableRowAlt.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_49 != this.mNativeStyle.Colors_49)
			{
				appendInterpolatedStringHandler..ctor(42, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundTextSelected = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTextSelected.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTextSelected.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTextSelected.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundTextSelected.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_53 != this.mNativeStyle.Colors_53)
			{
				appendInterpolatedStringHandler..ctor(45, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundNavWindowingDim = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundNavWindowingDim.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundNavWindowingDim.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundNavWindowingDim.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundNavWindowingDim.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_54 != this.mNativeStyle.Colors_54)
			{
				appendInterpolatedStringHandler..ctor(44, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBackgroundModalWindowDim = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundModalWindowDim.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundModalWindowDim.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundModalWindowDim.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBackgroundModalWindowDim.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_0 != this.mNativeStyle.Colors_0)
			{
				appendInterpolatedStringHandler..ctor(24, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorText = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorText.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorText.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorText.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorText.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_1 != this.mNativeStyle.Colors_1)
			{
				appendInterpolatedStringHandler..ctor(32, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorTextDisabled = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTextDisabled.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTextDisabled.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTextDisabled.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTextDisabled.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_5 != this.mNativeStyle.Colors_5)
			{
				appendInterpolatedStringHandler..ctor(26, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBorder = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBorder.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBorder.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBorder.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBorder.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_6 != this.mNativeStyle.Colors_6)
			{
				appendInterpolatedStringHandler..ctor(32, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorBorderShadow = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBorderShadow.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBorderShadow.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBorderShadow.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorBorderShadow.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_45 != this.mNativeStyle.Colors_45)
			{
				appendInterpolatedStringHandler..ctor(37, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorTableBorderStrong = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTableBorderStrong.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTableBorderStrong.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTableBorderStrong.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTableBorderStrong.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_46 != this.mNativeStyle.Colors_46)
			{
				appendInterpolatedStringHandler..ctor(36, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorTableBorderLight = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTableBorderLight.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTableBorderLight.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTableBorderLight.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTableBorderLight.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_15 != this.mNativeStyle.Colors_15)
			{
				appendInterpolatedStringHandler..ctor(33, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorScrollbarGrab = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorScrollbarGrab.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorScrollbarGrab.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorScrollbarGrab.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorScrollbarGrab.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_16 != this.mNativeStyle.Colors_16)
			{
				appendInterpolatedStringHandler..ctor(40, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorScrollbarGrabHovered = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorScrollbarGrabHovered.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorScrollbarGrabHovered.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorScrollbarGrabHovered.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorScrollbarGrabHovered.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_17 != this.mNativeStyle.Colors_17)
			{
				appendInterpolatedStringHandler..ctor(39, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorScrollbarGrabActive = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorScrollbarGrabActive.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorScrollbarGrabActive.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorScrollbarGrabActive.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorScrollbarGrabActive.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_19 != this.mNativeStyle.Colors_19)
			{
				appendInterpolatedStringHandler..ctor(30, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorSliderGrab = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSliderGrab.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSliderGrab.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSliderGrab.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSliderGrab.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_20 != this.mNativeStyle.Colors_20)
			{
				appendInterpolatedStringHandler..ctor(36, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorSliderGrabActive = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSliderGrabActive.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSliderGrabActive.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSliderGrabActive.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSliderGrabActive.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_21 != this.mNativeStyle.Colors_21)
			{
				appendInterpolatedStringHandler..ctor(26, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorButton = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorButton.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorButton.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorButton.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorButton.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_22 != this.mNativeStyle.Colors_22)
			{
				appendInterpolatedStringHandler..ctor(33, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorButtonHovered = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorButtonHovered.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorButtonHovered.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorButtonHovered.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorButtonHovered.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_23 != this.mNativeStyle.Colors_23)
			{
				appendInterpolatedStringHandler..ctor(32, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorButtonActive = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorButtonActive.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorButtonActive.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorButtonActive.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorButtonActive.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_24 != this.mNativeStyle.Colors_24)
			{
				appendInterpolatedStringHandler..ctor(26, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorHeader = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorHeader.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorHeader.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorHeader.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorHeader.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_25 != this.mNativeStyle.Colors_25)
			{
				appendInterpolatedStringHandler..ctor(33, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorHeaderHovered = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorHeaderHovered.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorHeaderHovered.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorHeaderHovered.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorHeaderHovered.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_26 != this.mNativeStyle.Colors_26)
			{
				appendInterpolatedStringHandler..ctor(32, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorHeaderActive = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorHeaderActive.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorHeaderActive.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorHeaderActive.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorHeaderActive.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_27 != this.mNativeStyle.Colors_27)
			{
				appendInterpolatedStringHandler..ctor(29, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorSeparator = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSeparator.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSeparator.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSeparator.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSeparator.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_28 != this.mNativeStyle.Colors_28)
			{
				appendInterpolatedStringHandler..ctor(36, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorSeparatorHovered = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSeparatorHovered.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSeparatorHovered.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSeparatorHovered.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSeparatorHovered.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_29 != this.mNativeStyle.Colors_29)
			{
				appendInterpolatedStringHandler..ctor(35, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorSeparatorActive = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSeparatorActive.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSeparatorActive.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSeparatorActive.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorSeparatorActive.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_30 != this.mNativeStyle.Colors_30)
			{
				appendInterpolatedStringHandler..ctor(30, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorResizeGrip = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorResizeGrip.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorResizeGrip.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorResizeGrip.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorResizeGrip.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_31 != this.mNativeStyle.Colors_31)
			{
				appendInterpolatedStringHandler..ctor(37, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorResizeGripHovered = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorResizeGripHovered.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorResizeGripHovered.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorResizeGripHovered.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorResizeGripHovered.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_32 != this.mNativeStyle.Colors_32)
			{
				appendInterpolatedStringHandler..ctor(36, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorResizeGripActive = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorResizeGripActive.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorResizeGripActive.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorResizeGripActive.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorResizeGripActive.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_33 != this.mNativeStyle.Colors_33)
			{
				appendInterpolatedStringHandler..ctor(23, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorTab = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTab.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTab.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTab.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTab.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_34 != this.mNativeStyle.Colors_34)
			{
				appendInterpolatedStringHandler..ctor(30, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorTabHovered = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabHovered.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabHovered.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabHovered.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabHovered.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_35 != this.mNativeStyle.Colors_35)
			{
				appendInterpolatedStringHandler..ctor(29, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorTabActive = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabActive.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabActive.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabActive.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabActive.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_36 != this.mNativeStyle.Colors_36)
			{
				appendInterpolatedStringHandler..ctor(32, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorTabUnfocused = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabUnfocused.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabUnfocused.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabUnfocused.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabUnfocused.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_37 != this.mNativeStyle.Colors_37)
			{
				appendInterpolatedStringHandler..ctor(38, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorTabUnfocusedActive = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabUnfocusedActive.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabUnfocusedActive.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabUnfocusedActive.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorTabUnfocusedActive.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_40 != this.mNativeStyle.Colors_40)
			{
				appendInterpolatedStringHandler..ctor(29, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorPlotLines = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotLines.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotLines.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotLines.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotLines.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_41 != this.mNativeStyle.Colors_41)
			{
				appendInterpolatedStringHandler..ctor(36, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorPlotLinesHovered = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotLinesHovered.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotLinesHovered.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotLinesHovered.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotLinesHovered.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_42 != this.mNativeStyle.Colors_42)
			{
				appendInterpolatedStringHandler..ctor(33, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorPlotHistogram = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotHistogram.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotHistogram.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotHistogram.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotHistogram.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_43 != this.mNativeStyle.Colors_43)
			{
				appendInterpolatedStringHandler..ctor(40, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorPlotHistogramHovered = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotHistogramHovered.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotHistogramHovered.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotHistogramHovered.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorPlotHistogramHovered.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_18 != this.mNativeStyle.Colors_18)
			{
				appendInterpolatedStringHandler..ctor(29, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorCheckMark = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorCheckMark.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorCheckMark.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorCheckMark.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorCheckMark.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_38 != this.mNativeStyle.Colors_38)
			{
				appendInterpolatedStringHandler..ctor(34, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorDockingPreview = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorDockingPreview.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorDockingPreview.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorDockingPreview.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorDockingPreview.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_50 != this.mNativeStyle.Colors_50)
			{
				appendInterpolatedStringHandler..ctor(34, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorDragDropTarget = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorDragDropTarget.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorDragDropTarget.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorDragDropTarget.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorDragDropTarget.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_51 != this.mNativeStyle.Colors_51)
			{
				appendInterpolatedStringHandler..ctor(32, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorNavHighlight = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorNavHighlight.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorNavHighlight.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorNavHighlight.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorNavHighlight.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.Colors_52 != this.mNativeStyle.Colors_52)
			{
				appendInterpolatedStringHandler..ctor(41, 4, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorNavWindowingHighlight = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorNavWindowingHighlight.R);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorNavWindowingHighlight.G);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorNavWindowingHighlight.B);
				appendInterpolatedStringHandler.AppendLiteral(",");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.ColorNavWindowingHighlight.A);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000083D4 File Offset: 0x000065D4
		protected void OtherPropertiesToCode(StringBuilder result)
		{
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
			if (this.mPrevStyle.AntiAliasedLines != this.mNativeStyle.AntiAliasedLines)
			{
				appendInterpolatedStringHandler..ctor(26, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.AntiAliasedLines = ");
				appendInterpolatedStringHandler.AppendFormatted<bool>(this.AntiAliasedLines);
				appendInterpolatedStringHandler.AppendLiteral(";");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.AntiAliasedLinesUseTex != this.mNativeStyle.AntiAliasedLinesUseTex)
			{
				appendInterpolatedStringHandler..ctor(32, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.AntiAliasedLinesUseTex = ");
				appendInterpolatedStringHandler.AppendFormatted<bool>(this.AntiAliasedLinesUseTex);
				appendInterpolatedStringHandler.AppendLiteral(";");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.AntiAliasedFill != this.mNativeStyle.AntiAliasedFill)
			{
				appendInterpolatedStringHandler..ctor(26, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.AntiAliasedLines = ");
				appendInterpolatedStringHandler.AppendFormatted<bool>(this.AntiAliasedFill);
				appendInterpolatedStringHandler.AppendLiteral(";");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.FontName != null && this.FontName != "")
			{
				appendInterpolatedStringHandler..ctor(20, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.FontName = \"");
				appendInterpolatedStringHandler.AppendFormatted(this.FontName);
				appendInterpolatedStringHandler.AppendLiteral("\";");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.FontName != null && this.FontName != "")
			{
				appendInterpolatedStringHandler..ctor(18, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.FontSize = ");
				appendInterpolatedStringHandler.AppendFormatted<int>(this.FontSize);
				appendInterpolatedStringHandler.AppendLiteral(";");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.HoverFlagsForTooltipMouse != this.mNativeStyle.HoverFlagsForTooltipMouse)
			{
				appendInterpolatedStringHandler..ctor(56, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.HoverFlagsForTooltipMouse = (ImGuiHoveredFlags)0x");
				appendInterpolatedStringHandler.AppendFormatted(this.HoverFlagsForTooltipMouse.ToString("x"));
				appendInterpolatedStringHandler.AppendLiteral(";");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.HoverFlagsForTooltipNav != this.mNativeStyle.HoverFlagsForTooltipNav)
			{
				appendInterpolatedStringHandler..ctor(54, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.HoverFlagsForTooltipNav = (ImGuiHoveredFlags)0x");
				appendInterpolatedStringHandler.AppendFormatted(this.HoverFlagsForTooltipNav.ToString("x"));
				appendInterpolatedStringHandler.AppendLiteral(";");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.WindowMenuButtonPosition != this.mNativeStyle.WindowMenuButtonPosition)
			{
				appendInterpolatedStringHandler..ctor(46, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.WindowMenuButtonPosition = (ImGuiDir)0x");
				appendInterpolatedStringHandler.AppendFormatted(this.WindowMenuButtonPosition.ToString("x"));
				appendInterpolatedStringHandler.AppendLiteral(";");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mPrevStyle.ColorButtonPosition != this.mNativeStyle.ColorButtonPosition)
			{
				appendInterpolatedStringHandler..ctor(41, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.ColorButtonPosition = (ImGuiDir)0x");
				appendInterpolatedStringHandler.AppendFormatted(this.ColorButtonPosition.ToString("x"));
				appendInterpolatedStringHandler.AppendLiteral(";");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
		}

		// Token: 0x0400001B RID: 27
		private ImGuiStyle mPrevStyle;

		// Token: 0x0400001C RID: 28
		private ImGuiStyle mNativeStyle;

		// Token: 0x0400001D RID: 29
		private ImFontPtr mFont;

		// Token: 0x0400001E RID: 30
		[TupleElementNames(new string[]
		{
			"name",
			"size"
		})]
		[Nullable(new byte[]
		{
			0,
			1
		})]
		private ValueTuple<string, int> mFontKey;

		// Token: 0x0400001F RID: 31
		private bool mPushed;

		// Token: 0x04000020 RID: 32
		private bool mFontPushed;

		// Token: 0x04000021 RID: 33
		private bool mFontLoaded;
	}
}
