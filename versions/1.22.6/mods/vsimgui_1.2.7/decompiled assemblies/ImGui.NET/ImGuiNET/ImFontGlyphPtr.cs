using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000029 RID: 41
	public struct ImFontGlyphPtr
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00004299 File Offset: 0x00002499
		public unsafe readonly ImFontGlyph* NativePtr { get; }

		// Token: 0x0600019D RID: 413 RVA: 0x000042A1 File Offset: 0x000024A1
		public unsafe ImFontGlyphPtr(ImFontGlyph* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000042AA File Offset: 0x000024AA
		public unsafe ImFontGlyphPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000042B8 File Offset: 0x000024B8
		public unsafe static implicit operator ImFontGlyphPtr(ImFontGlyph* nativePtr)
		{
			return new ImFontGlyphPtr(nativePtr);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000042C0 File Offset: 0x000024C0
		public unsafe static implicit operator ImFontGlyph*(ImFontGlyphPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000042C9 File Offset: 0x000024C9
		public static implicit operator ImFontGlyphPtr(IntPtr nativePtr)
		{
			return new ImFontGlyphPtr(nativePtr);
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x000042D1 File Offset: 0x000024D1
		public unsafe ref uint Colored
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->Colored));
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x000042E4 File Offset: 0x000024E4
		public unsafe ref uint Visible
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->Visible));
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x000042F7 File Offset: 0x000024F7
		public unsafe ref uint Codepoint
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->Codepoint));
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000430A File Offset: 0x0000250A
		public unsafe ref float AdvanceX
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->AdvanceX));
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000431D File Offset: 0x0000251D
		public unsafe ref float X0
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->X0));
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00004330 File Offset: 0x00002530
		public unsafe ref float Y0
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->Y0));
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00004343 File Offset: 0x00002543
		public unsafe ref float X1
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->X1));
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00004356 File Offset: 0x00002556
		public unsafe ref float Y1
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->Y1));
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00004369 File Offset: 0x00002569
		public unsafe ref float U0
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->U0));
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000437C File Offset: 0x0000257C
		public unsafe ref float V0
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->V0));
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000438F File Offset: 0x0000258F
		public unsafe ref float U1
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->U1));
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001AD RID: 429 RVA: 0x000043A2 File Offset: 0x000025A2
		public unsafe ref float V1
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->V1));
			}
		}
	}
}
