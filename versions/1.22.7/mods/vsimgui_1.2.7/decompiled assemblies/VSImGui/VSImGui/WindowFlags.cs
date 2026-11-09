using System;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Newtonsoft.Json;

namespace VSImGui
{
	// Token: 0x02000017 RID: 23
	[NullableContext(1)]
	[Nullable(0)]
	[JsonObject(0)]
	public struct WindowFlags
	{
		// Token: 0x060001EF RID: 495 RVA: 0x0000BA63 File Offset: 0x00009C63
		public WindowFlags()
		{
			this.WindowFlagsValue = 0;
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000BA6C File Offset: 0x00009C6C
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x0000BA74 File Offset: 0x00009C74
		[JsonIgnore]
		public ImGuiWindowFlags WindowFlagsValue { readonly get; set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000BA7D File Offset: 0x00009C7D
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000BA88 File Offset: 0x00009C88
		[JsonIgnore]
		public bool None
		{
			get
			{
				return this.WindowFlagsValue == 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? 0 : this.WindowFlagsValue);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000BA9C File Offset: 0x00009C9C
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000BAA9 File Offset: 0x00009CA9
		public bool NoTitleBar
		{
			get
			{
				return (this.WindowFlagsValue & 1) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 1) : (this.WindowFlagsValue & -2));
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000BAC7 File Offset: 0x00009CC7
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000BAD4 File Offset: 0x00009CD4
		public bool NoResize
		{
			get
			{
				return (this.WindowFlagsValue & 2) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 2) : (this.WindowFlagsValue & -3));
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000BAF2 File Offset: 0x00009CF2
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000BAFF File Offset: 0x00009CFF
		public bool NoMove
		{
			get
			{
				return (this.WindowFlagsValue & 4) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 4) : (this.WindowFlagsValue & -5));
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000BB1D File Offset: 0x00009D1D
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000BB2A File Offset: 0x00009D2A
		public bool NoScrollbar
		{
			get
			{
				return (this.WindowFlagsValue & 8) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 8) : (this.WindowFlagsValue & -9));
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000BB48 File Offset: 0x00009D48
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000BB56 File Offset: 0x00009D56
		public bool NoScrollWithMouse
		{
			get
			{
				return (this.WindowFlagsValue & 16) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 16) : (this.WindowFlagsValue & -17));
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000BB75 File Offset: 0x00009D75
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000BB83 File Offset: 0x00009D83
		public bool NoCollapse
		{
			get
			{
				return (this.WindowFlagsValue & 32) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 32) : (this.WindowFlagsValue & -33));
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000BBA2 File Offset: 0x00009DA2
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000BBB0 File Offset: 0x00009DB0
		public bool AlwaysAutoResize
		{
			get
			{
				return (this.WindowFlagsValue & 64) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 64) : (this.WindowFlagsValue & -65));
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000BBCF File Offset: 0x00009DCF
		// (set) Token: 0x06000203 RID: 515 RVA: 0x0000BBE0 File Offset: 0x00009DE0
		public bool NoBackground
		{
			get
			{
				return (this.WindowFlagsValue & 128) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 128) : (this.WindowFlagsValue & -129));
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000BC05 File Offset: 0x00009E05
		// (set) Token: 0x06000205 RID: 517 RVA: 0x0000BC16 File Offset: 0x00009E16
		public bool NoSavedSettings
		{
			get
			{
				return (this.WindowFlagsValue & 256) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 256) : (this.WindowFlagsValue & -257));
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000BC3B File Offset: 0x00009E3B
		// (set) Token: 0x06000207 RID: 519 RVA: 0x0000BC4C File Offset: 0x00009E4C
		public bool NoMouseInputs
		{
			get
			{
				return (this.WindowFlagsValue & 512) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 512) : (this.WindowFlagsValue & -513));
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000BC71 File Offset: 0x00009E71
		// (set) Token: 0x06000209 RID: 521 RVA: 0x0000BC82 File Offset: 0x00009E82
		public bool MenuBar
		{
			get
			{
				return (this.WindowFlagsValue & 1024) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 1024) : (this.WindowFlagsValue & -1025));
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000BCA7 File Offset: 0x00009EA7
		// (set) Token: 0x0600020B RID: 523 RVA: 0x0000BCB8 File Offset: 0x00009EB8
		public bool HorizontalScrollbar
		{
			get
			{
				return (this.WindowFlagsValue & 2048) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 2048) : (this.WindowFlagsValue & -2049));
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000BCDD File Offset: 0x00009EDD
		// (set) Token: 0x0600020D RID: 525 RVA: 0x0000BCEE File Offset: 0x00009EEE
		public bool NoFocusOnAppearing
		{
			get
			{
				return (this.WindowFlagsValue & 4096) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 4096) : (this.WindowFlagsValue & -4097));
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000BD13 File Offset: 0x00009F13
		// (set) Token: 0x0600020F RID: 527 RVA: 0x0000BD24 File Offset: 0x00009F24
		public bool NoBringToFrontOnFocus
		{
			get
			{
				return (this.WindowFlagsValue & 8192) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 8192) : (this.WindowFlagsValue & -8193));
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000BD49 File Offset: 0x00009F49
		// (set) Token: 0x06000211 RID: 529 RVA: 0x0000BD5A File Offset: 0x00009F5A
		public bool AlwaysVerticalScrollbar
		{
			get
			{
				return (this.WindowFlagsValue & 16384) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 16384) : (this.WindowFlagsValue & -16385));
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000BD7F File Offset: 0x00009F7F
		// (set) Token: 0x06000213 RID: 531 RVA: 0x0000BD90 File Offset: 0x00009F90
		public bool AlwaysHorizontalScrollbar
		{
			get
			{
				return (this.WindowFlagsValue & 32768) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 32768) : (this.WindowFlagsValue & -32769));
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000BDB5 File Offset: 0x00009FB5
		// (set) Token: 0x06000215 RID: 533 RVA: 0x0000BDC6 File Offset: 0x00009FC6
		public bool AlwaysUseWindowPadding
		{
			get
			{
				return (this.WindowFlagsValue & 65536) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 65536) : (this.WindowFlagsValue & -65537));
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000BDEB File Offset: 0x00009FEB
		// (set) Token: 0x06000217 RID: 535 RVA: 0x0000BDFC File Offset: 0x00009FFC
		public bool NoNavInputs
		{
			get
			{
				return (this.WindowFlagsValue & 262144) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 262144) : (this.WindowFlagsValue & -262145));
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000BE21 File Offset: 0x0000A021
		// (set) Token: 0x06000219 RID: 537 RVA: 0x0000BE32 File Offset: 0x0000A032
		public bool NoNavFocus
		{
			get
			{
				return (this.WindowFlagsValue & 524288) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 524288) : (this.WindowFlagsValue & -524289));
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000BE57 File Offset: 0x0000A057
		// (set) Token: 0x0600021B RID: 539 RVA: 0x0000BE68 File Offset: 0x0000A068
		public bool UnsavedDocument
		{
			get
			{
				return (this.WindowFlagsValue & 1048576) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 1048576) : (this.WindowFlagsValue & -1048577));
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000BE8D File Offset: 0x0000A08D
		// (set) Token: 0x0600021D RID: 541 RVA: 0x0000BE9E File Offset: 0x0000A09E
		public bool NoDocking
		{
			get
			{
				return (this.WindowFlagsValue & 2097152) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 2097152) : (this.WindowFlagsValue & -2097153));
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000BEC3 File Offset: 0x0000A0C3
		// (set) Token: 0x0600021F RID: 543 RVA: 0x0000BED4 File Offset: 0x0000A0D4
		public bool NoNav
		{
			get
			{
				return (this.WindowFlagsValue & 786432) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 786432) : (this.WindowFlagsValue & -786433));
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000BEF9 File Offset: 0x0000A0F9
		// (set) Token: 0x06000221 RID: 545 RVA: 0x0000BF07 File Offset: 0x0000A107
		public bool NoDecoration
		{
			get
			{
				return (this.WindowFlagsValue & 43) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 43) : (this.WindowFlagsValue & -44));
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000BF26 File Offset: 0x0000A126
		// (set) Token: 0x06000223 RID: 547 RVA: 0x0000BF37 File Offset: 0x0000A137
		public bool NoInputs
		{
			get
			{
				return (this.WindowFlagsValue & 786944) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 786944) : (this.WindowFlagsValue & -786945));
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000BF5C File Offset: 0x0000A15C
		// (set) Token: 0x06000225 RID: 549 RVA: 0x0000BF6D File Offset: 0x0000A16D
		public bool NavFlattened
		{
			get
			{
				return (this.WindowFlagsValue & 8388608) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 8388608) : (this.WindowFlagsValue & -8388609));
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000BF92 File Offset: 0x0000A192
		// (set) Token: 0x06000227 RID: 551 RVA: 0x0000BFA3 File Offset: 0x0000A1A3
		public bool ChildWindow
		{
			get
			{
				return (this.WindowFlagsValue & 16777216) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 16777216) : (this.WindowFlagsValue & -16777217));
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000BFC8 File Offset: 0x0000A1C8
		// (set) Token: 0x06000229 RID: 553 RVA: 0x0000BFD9 File Offset: 0x0000A1D9
		public bool Tooltip
		{
			get
			{
				return (this.WindowFlagsValue & 33554432) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 33554432) : (this.WindowFlagsValue & -33554433));
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000BFFE File Offset: 0x0000A1FE
		// (set) Token: 0x0600022B RID: 555 RVA: 0x0000C00F File Offset: 0x0000A20F
		public bool Popup
		{
			get
			{
				return (this.WindowFlagsValue & 67108864) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 67108864) : (this.WindowFlagsValue & -67108865));
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000C034 File Offset: 0x0000A234
		// (set) Token: 0x0600022D RID: 557 RVA: 0x0000C045 File Offset: 0x0000A245
		public bool Modal
		{
			get
			{
				return (this.WindowFlagsValue & 134217728) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 134217728) : (this.WindowFlagsValue & -134217729));
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000C06A File Offset: 0x0000A26A
		// (set) Token: 0x0600022F RID: 559 RVA: 0x0000C07B File Offset: 0x0000A27B
		public bool ChildMenu
		{
			get
			{
				return (this.WindowFlagsValue & 268435456) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 268435456) : (this.WindowFlagsValue & -268435457));
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000C0A0 File Offset: 0x0000A2A0
		// (set) Token: 0x06000231 RID: 561 RVA: 0x0000C0B1 File Offset: 0x0000A2B1
		public bool DockNodeHost
		{
			get
			{
				return (this.WindowFlagsValue & 536870912) > 0;
			}
			set
			{
				this.WindowFlagsValue = (value ? (this.WindowFlagsValue | 536870912) : (this.WindowFlagsValue & -536870913));
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000C0D8 File Offset: 0x0000A2D8
		public void DrawEditor(string name)
		{
			int dir = this.WindowFlagsValue;
			if (ImGui.CollapsingHeader(MemoryExtensions.AsSpan(name), 32))
			{
				ImGui.Indent();
				for (int index = 0; index < WindowFlags.WindowFlagsEnum.Length; index++)
				{
					ImGui.CheckboxFlags(MemoryExtensions.AsSpan(WindowFlags.WindowFlagsEnum[index] + "##" + name), ref dir, WindowFlags.WindowFlagsEnumValues[index]);
				}
				ImGui.Unindent();
			}
			this.WindowFlagsValue = dir;
		}

		// Token: 0x04000041 RID: 65
		public static readonly string[] WindowFlagsEnum = new string[]
		{
			"NoTitleBar",
			"NoResize",
			"NoMove",
			"NoScrollbar",
			"NoScrollWithMouse",
			"NoCollapse",
			"AlwaysAutoResize",
			"NoBackground",
			"NoSavedSettings",
			"NoMouseInputs",
			"MenuBar",
			"HorizontalScrollbar",
			"NoFocusOnAppearing",
			"NoBringToFrontOnFocus",
			"AlwaysVerticalScrollbar",
			"AlwaysHorizontalScrollbar",
			"AlwaysUseWindowPadding",
			"NoNavInputs",
			"NoNavFocus",
			"UnsavedDocument",
			"NoDocking",
			"NoNav",
			"NoDecoration",
			"NoInputs",
			"Tooltip",
			"Modal",
			"DockNodeHost"
		};

		// Token: 0x04000042 RID: 66
		public static readonly int[] WindowFlagsEnumValues = new int[]
		{
			1,
			2,
			4,
			8,
			16,
			32,
			64,
			128,
			256,
			512,
			1024,
			2048,
			4096,
			8192,
			16384,
			32768,
			65536,
			262144,
			524288,
			1048576,
			2097152,
			786432,
			43,
			786944,
			33554432,
			134217728,
			536870912
		};
	}
}
