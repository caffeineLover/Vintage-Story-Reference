using System;
using System.Text;

namespace ImGuiNET
{
	// Token: 0x0200002B RID: 43
	public struct ImFontGlyphRangesBuilderPtr
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001AE RID: 430 RVA: 0x000043B5 File Offset: 0x000025B5
		public unsafe readonly ImFontGlyphRangesBuilder* NativePtr { get; }

		// Token: 0x060001AF RID: 431 RVA: 0x000043BD File Offset: 0x000025BD
		public unsafe ImFontGlyphRangesBuilderPtr(ImFontGlyphRangesBuilder* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x000043C6 File Offset: 0x000025C6
		public unsafe ImFontGlyphRangesBuilderPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x000043D4 File Offset: 0x000025D4
		public unsafe static implicit operator ImFontGlyphRangesBuilderPtr(ImFontGlyphRangesBuilder* nativePtr)
		{
			return new ImFontGlyphRangesBuilderPtr(nativePtr);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000043DC File Offset: 0x000025DC
		public unsafe static implicit operator ImFontGlyphRangesBuilder*(ImFontGlyphRangesBuilderPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000043E5 File Offset: 0x000025E5
		public static implicit operator ImFontGlyphRangesBuilderPtr(IntPtr nativePtr)
		{
			return new ImFontGlyphRangesBuilderPtr(nativePtr);
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x000043ED File Offset: 0x000025ED
		public unsafe ImVector<uint> UsedChars
		{
			get
			{
				return new ImVector<uint>(this.NativePtr->UsedChars);
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000043FF File Offset: 0x000025FF
		public void AddChar(ushort c)
		{
			ImGuiNative.ImFontGlyphRangesBuilder_AddChar(this.NativePtr, c);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00004410 File Offset: 0x00002610
		public unsafe void AddRanges(IntPtr ranges)
		{
			ushort* ranges2 = (ushort*)ranges.ToPointer();
			ImGuiNative.ImFontGlyphRangesBuilder_AddRanges(this.NativePtr, ranges2);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00004434 File Offset: 0x00002634
		public unsafe void AddText(ReadOnlySpan<char> text)
		{
			int num = 0;
			byte* ptr;
			if (text != null)
			{
				num = Encoding.UTF8.GetByteCount(text);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(text, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte* text_end = null;
			ImGuiNative.ImFontGlyphRangesBuilder_AddText(this.NativePtr, ptr, text_end);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000044A8 File Offset: 0x000026A8
		public unsafe void BuildRanges(out ImVector out_ranges)
		{
			fixed (ImVector* ptr = &out_ranges)
			{
				ImVector* out_ranges2 = ptr;
				ImGuiNative.ImFontGlyphRangesBuilder_BuildRanges(this.NativePtr, out_ranges2);
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000044C9 File Offset: 0x000026C9
		public void Clear()
		{
			ImGuiNative.ImFontGlyphRangesBuilder_Clear(this.NativePtr);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000044D6 File Offset: 0x000026D6
		public void Destroy()
		{
			ImGuiNative.ImFontGlyphRangesBuilder_destroy(this.NativePtr);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000044E3 File Offset: 0x000026E3
		public bool GetBit(uint n)
		{
			return ImGuiNative.ImFontGlyphRangesBuilder_GetBit(this.NativePtr, n) > 0;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000044F4 File Offset: 0x000026F4
		public void SetBit(uint n)
		{
			ImGuiNative.ImFontGlyphRangesBuilder_SetBit(this.NativePtr, n);
		}
	}
}
