using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000020 RID: 32
	public struct ImFontPtr
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000031A3 File Offset: 0x000013A3
		public unsafe readonly ImFont* NativePtr { get; }

		// Token: 0x060000FB RID: 251 RVA: 0x000031AB File Offset: 0x000013AB
		public unsafe ImFontPtr(ImFont* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000031B4 File Offset: 0x000013B4
		public unsafe ImFontPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000031C2 File Offset: 0x000013C2
		public unsafe static implicit operator ImFontPtr(ImFont* nativePtr)
		{
			return new ImFontPtr(nativePtr);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000031CA File Offset: 0x000013CA
		public unsafe static implicit operator ImFont*(ImFontPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000031D3 File Offset: 0x000013D3
		public static implicit operator ImFontPtr(IntPtr nativePtr)
		{
			return new ImFontPtr(nativePtr);
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000100 RID: 256 RVA: 0x000031DB File Offset: 0x000013DB
		public unsafe ImVector<float> IndexAdvanceX
		{
			get
			{
				return new ImVector<float>(this.NativePtr->IndexAdvanceX);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000101 RID: 257 RVA: 0x000031ED File Offset: 0x000013ED
		public unsafe ref float FallbackAdvanceX
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->FallbackAdvanceX));
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00003200 File Offset: 0x00001400
		public unsafe ref float FontSize
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->FontSize));
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00003213 File Offset: 0x00001413
		public unsafe ImVector<ushort> IndexLookup
		{
			get
			{
				return new ImVector<ushort>(this.NativePtr->IndexLookup);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00003225 File Offset: 0x00001425
		public unsafe ImPtrVector<ImFontGlyphPtr> Glyphs
		{
			get
			{
				return new ImPtrVector<ImFontGlyphPtr>(this.NativePtr->Glyphs, Unsafe.SizeOf<ImFontGlyph>());
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000105 RID: 261 RVA: 0x0000323C File Offset: 0x0000143C
		public unsafe ImFontGlyphPtr FallbackGlyph
		{
			get
			{
				return new ImFontGlyphPtr(this.NativePtr->FallbackGlyph);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000324E File Offset: 0x0000144E
		public unsafe ImFontAtlasPtr ContainerAtlas
		{
			get
			{
				return new ImFontAtlasPtr(this.NativePtr->ContainerAtlas);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00003260 File Offset: 0x00001460
		public unsafe ImFontConfigPtr ConfigData
		{
			get
			{
				return new ImFontConfigPtr(this.NativePtr->ConfigData);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00003272 File Offset: 0x00001472
		public unsafe ref short ConfigDataCount
		{
			get
			{
				return Unsafe.AsRef<short>((void*)(&this.NativePtr->ConfigDataCount));
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00003285 File Offset: 0x00001485
		public unsafe ref ushort FallbackChar
		{
			get
			{
				return Unsafe.AsRef<ushort>((void*)(&this.NativePtr->FallbackChar));
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00003298 File Offset: 0x00001498
		public unsafe ref ushort EllipsisChar
		{
			get
			{
				return Unsafe.AsRef<ushort>((void*)(&this.NativePtr->EllipsisChar));
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600010B RID: 267 RVA: 0x000032AB File Offset: 0x000014AB
		public unsafe ref short EllipsisCharCount
		{
			get
			{
				return Unsafe.AsRef<short>((void*)(&this.NativePtr->EllipsisCharCount));
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600010C RID: 268 RVA: 0x000032BE File Offset: 0x000014BE
		public unsafe ref float EllipsisWidth
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->EllipsisWidth));
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000032D1 File Offset: 0x000014D1
		public unsafe ref float EllipsisCharStep
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->EllipsisCharStep));
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000032E4 File Offset: 0x000014E4
		public unsafe ref bool DirtyLookupTables
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->DirtyLookupTables));
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000032F7 File Offset: 0x000014F7
		public unsafe ref float Scale
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->Scale));
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000110 RID: 272 RVA: 0x0000330A File Offset: 0x0000150A
		public unsafe ref float Ascent
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->Ascent));
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000331D File Offset: 0x0000151D
		public unsafe ref float Descent
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->Descent));
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00003330 File Offset: 0x00001530
		public unsafe ref int MetricsTotalSurface
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->MetricsTotalSurface));
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00003343 File Offset: 0x00001543
		public unsafe RangeAccessor<byte> Used4kPagesMap
		{
			get
			{
				return new RangeAccessor<byte>((void*)(&this.NativePtr->Used4kPagesMap.FixedElementField), 2);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000335C File Offset: 0x0000155C
		public unsafe void AddGlyph(ImFontConfigPtr src_cfg, ushort c, float x0, float y0, float x1, float y1, float u0, float v0, float u1, float v1, float advance_x)
		{
			ImFontConfig* nativePtr = src_cfg.NativePtr;
			ImGuiNative.ImFont_AddGlyph(this.NativePtr, nativePtr, c, x0, y0, x1, y1, u0, v0, u1, v1, advance_x);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00003390 File Offset: 0x00001590
		public void AddRemapChar(ushort dst, ushort src)
		{
			byte overwrite_dst = 1;
			ImGuiNative.ImFont_AddRemapChar(this.NativePtr, dst, src, overwrite_dst);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000033B0 File Offset: 0x000015B0
		public void AddRemapChar(ushort dst, ushort src, bool overwrite_dst)
		{
			byte overwrite_dst2 = (overwrite_dst > false) ? 1 : 0;
			ImGuiNative.ImFont_AddRemapChar(this.NativePtr, dst, src, overwrite_dst2);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000033D1 File Offset: 0x000015D1
		public void BuildLookupTable()
		{
			ImGuiNative.ImFont_BuildLookupTable(this.NativePtr);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000033DE File Offset: 0x000015DE
		public void ClearOutputData()
		{
			ImGuiNative.ImFont_ClearOutputData(this.NativePtr);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000033EB File Offset: 0x000015EB
		public void Destroy()
		{
			ImGuiNative.ImFont_destroy(this.NativePtr);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000033F8 File Offset: 0x000015F8
		public ImFontGlyphPtr FindGlyph(ushort c)
		{
			return new ImFontGlyphPtr(ImGuiNative.ImFont_FindGlyph(this.NativePtr, c));
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000340B File Offset: 0x0000160B
		public ImFontGlyphPtr FindGlyphNoFallback(ushort c)
		{
			return new ImFontGlyphPtr(ImGuiNative.ImFont_FindGlyphNoFallback(this.NativePtr, c));
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000341E File Offset: 0x0000161E
		public float GetCharAdvance(ushort c)
		{
			return ImGuiNative.ImFont_GetCharAdvance(this.NativePtr, c);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000342C File Offset: 0x0000162C
		public string GetDebugName()
		{
			return Util.StringFromPtr(ImGuiNative.ImFont_GetDebugName(this.NativePtr));
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000343E File Offset: 0x0000163E
		public void GrowIndex(int new_size)
		{
			ImGuiNative.ImFont_GrowIndex(this.NativePtr, new_size);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000344C File Offset: 0x0000164C
		public bool IsLoaded()
		{
			return ImGuiNative.ImFont_IsLoaded(this.NativePtr) > 0;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000345C File Offset: 0x0000165C
		public unsafe void RenderChar(ImDrawListPtr draw_list, float size, Vector2 pos, uint col, ushort c)
		{
			ImDrawList* nativePtr = draw_list.NativePtr;
			ImGuiNative.ImFont_RenderChar(this.NativePtr, nativePtr, size, pos, col, c);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00003484 File Offset: 0x00001684
		public void SetGlyphVisible(ushort c, bool visible)
		{
			byte visible2 = (visible > false) ? 1 : 0;
			ImGuiNative.ImFont_SetGlyphVisible(this.NativePtr, c, visible2);
		}
	}
}
