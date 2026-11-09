using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000027 RID: 39
	public struct ImFontConfigPtr
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000180 RID: 384 RVA: 0x000040C1 File Offset: 0x000022C1
		public unsafe readonly ImFontConfig* NativePtr { get; }

		// Token: 0x06000181 RID: 385 RVA: 0x000040C9 File Offset: 0x000022C9
		public unsafe ImFontConfigPtr(ImFontConfig* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000040D2 File Offset: 0x000022D2
		public unsafe ImFontConfigPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000040E0 File Offset: 0x000022E0
		public unsafe static implicit operator ImFontConfigPtr(ImFontConfig* nativePtr)
		{
			return new ImFontConfigPtr(nativePtr);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000040E8 File Offset: 0x000022E8
		public unsafe static implicit operator ImFontConfig*(ImFontConfigPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000040F1 File Offset: 0x000022F1
		public static implicit operator ImFontConfigPtr(IntPtr nativePtr)
		{
			return new ImFontConfigPtr(nativePtr);
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000186 RID: 390 RVA: 0x000040F9 File Offset: 0x000022F9
		// (set) Token: 0x06000187 RID: 391 RVA: 0x0000410B File Offset: 0x0000230B
		public unsafe IntPtr FontData
		{
			get
			{
				return (IntPtr)this.NativePtr->FontData;
			}
			set
			{
				this.NativePtr->FontData = (void*)value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0000411E File Offset: 0x0000231E
		public unsafe ref int FontDataSize
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->FontDataSize));
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00004131 File Offset: 0x00002331
		public unsafe ref bool FontDataOwnedByAtlas
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->FontDataOwnedByAtlas));
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00004144 File Offset: 0x00002344
		public unsafe ref int FontNo
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->FontNo));
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00004157 File Offset: 0x00002357
		public unsafe ref float SizePixels
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->SizePixels));
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0000416A File Offset: 0x0000236A
		public unsafe ref int OversampleH
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->OversampleH));
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600018D RID: 397 RVA: 0x0000417D File Offset: 0x0000237D
		public unsafe ref int OversampleV
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->OversampleV));
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00004190 File Offset: 0x00002390
		public unsafe ref bool PixelSnapH
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->PixelSnapH));
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600018F RID: 399 RVA: 0x000041A3 File Offset: 0x000023A3
		public unsafe ref Vector2 GlyphExtraSpacing
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->GlyphExtraSpacing));
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000190 RID: 400 RVA: 0x000041B6 File Offset: 0x000023B6
		public unsafe ref Vector2 GlyphOffset
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->GlyphOffset));
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000191 RID: 401 RVA: 0x000041C9 File Offset: 0x000023C9
		// (set) Token: 0x06000192 RID: 402 RVA: 0x000041DB File Offset: 0x000023DB
		public unsafe IntPtr GlyphRanges
		{
			get
			{
				return (IntPtr)((void*)this.NativePtr->GlyphRanges);
			}
			set
			{
				this.NativePtr->GlyphRanges = (ushort*)((void*)value);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000193 RID: 403 RVA: 0x000041EE File Offset: 0x000023EE
		public unsafe ref float GlyphMinAdvanceX
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->GlyphMinAdvanceX));
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00004201 File Offset: 0x00002401
		public unsafe ref float GlyphMaxAdvanceX
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->GlyphMaxAdvanceX));
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00004214 File Offset: 0x00002414
		public unsafe ref bool MergeMode
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->MergeMode));
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00004227 File Offset: 0x00002427
		public unsafe ref uint FontBuilderFlags
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->FontBuilderFlags));
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000423A File Offset: 0x0000243A
		public unsafe ref float RasterizerMultiply
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->RasterizerMultiply));
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000424D File Offset: 0x0000244D
		public unsafe ref ushort EllipsisChar
		{
			get
			{
				return Unsafe.AsRef<ushort>((void*)(&this.NativePtr->EllipsisChar));
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00004260 File Offset: 0x00002460
		public unsafe RangeAccessor<byte> Name
		{
			get
			{
				return new RangeAccessor<byte>((void*)(&this.NativePtr->Name.FixedElementField), 40);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000427A File Offset: 0x0000247A
		public unsafe ImFontPtr DstFont
		{
			get
			{
				return new ImFontPtr(this.NativePtr->DstFont);
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000428C File Offset: 0x0000248C
		public void Destroy()
		{
			ImGuiNative.ImFontConfig_destroy(this.NativePtr);
		}
	}
}
