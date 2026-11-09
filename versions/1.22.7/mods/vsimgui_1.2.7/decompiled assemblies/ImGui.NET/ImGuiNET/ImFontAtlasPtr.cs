using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace ImGuiNET
{
	// Token: 0x02000022 RID: 34
	public struct ImFontAtlasPtr
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000122 RID: 290 RVA: 0x000034A4 File Offset: 0x000016A4
		public unsafe readonly ImFontAtlas* NativePtr { get; }

		// Token: 0x06000123 RID: 291 RVA: 0x000034AC File Offset: 0x000016AC
		public unsafe ImFontAtlasPtr(ImFontAtlas* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000034B5 File Offset: 0x000016B5
		public unsafe ImFontAtlasPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000034C3 File Offset: 0x000016C3
		public unsafe static implicit operator ImFontAtlasPtr(ImFontAtlas* nativePtr)
		{
			return new ImFontAtlasPtr(nativePtr);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000034CB File Offset: 0x000016CB
		public unsafe static implicit operator ImFontAtlas*(ImFontAtlasPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000034D4 File Offset: 0x000016D4
		public static implicit operator ImFontAtlasPtr(IntPtr nativePtr)
		{
			return new ImFontAtlasPtr(nativePtr);
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000128 RID: 296 RVA: 0x000034DC File Offset: 0x000016DC
		public unsafe ref ImFontAtlasFlags Flags
		{
			get
			{
				return Unsafe.AsRef<ImFontAtlasFlags>((void*)(&this.NativePtr->Flags));
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000129 RID: 297 RVA: 0x000034EF File Offset: 0x000016EF
		public unsafe ref IntPtr TexID
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->TexID));
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00003502 File Offset: 0x00001702
		public unsafe ref int TexDesiredWidth
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->TexDesiredWidth));
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00003515 File Offset: 0x00001715
		public unsafe ref int TexGlyphPadding
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->TexGlyphPadding));
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00003528 File Offset: 0x00001728
		public unsafe ref bool Locked
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->Locked));
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600012D RID: 301 RVA: 0x0000353B File Offset: 0x0000173B
		// (set) Token: 0x0600012E RID: 302 RVA: 0x0000354D File Offset: 0x0000174D
		public unsafe IntPtr UserData
		{
			get
			{
				return (IntPtr)this.NativePtr->UserData;
			}
			set
			{
				this.NativePtr->UserData = (void*)value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00003560 File Offset: 0x00001760
		public unsafe ref bool TexReady
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->TexReady));
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00003573 File Offset: 0x00001773
		public unsafe ref bool TexPixelsUseColors
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->TexPixelsUseColors));
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00003586 File Offset: 0x00001786
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00003598 File Offset: 0x00001798
		public unsafe IntPtr TexPixelsAlpha8
		{
			get
			{
				return (IntPtr)((void*)this.NativePtr->TexPixelsAlpha8);
			}
			set
			{
				this.NativePtr->TexPixelsAlpha8 = (byte*)((void*)value);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000133 RID: 307 RVA: 0x000035AB File Offset: 0x000017AB
		// (set) Token: 0x06000134 RID: 308 RVA: 0x000035BD File Offset: 0x000017BD
		public unsafe IntPtr TexPixelsRGBA32
		{
			get
			{
				return (IntPtr)((void*)this.NativePtr->TexPixelsRGBA32);
			}
			set
			{
				this.NativePtr->TexPixelsRGBA32 = (uint*)((void*)value);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000135 RID: 309 RVA: 0x000035D0 File Offset: 0x000017D0
		public unsafe ref int TexWidth
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->TexWidth));
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000136 RID: 310 RVA: 0x000035E3 File Offset: 0x000017E3
		public unsafe ref int TexHeight
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->TexHeight));
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000137 RID: 311 RVA: 0x000035F6 File Offset: 0x000017F6
		public unsafe ref Vector2 TexUvScale
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->TexUvScale));
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00003609 File Offset: 0x00001809
		public unsafe ref Vector2 TexUvWhitePixel
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->TexUvWhitePixel));
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000139 RID: 313 RVA: 0x0000361C File Offset: 0x0000181C
		public unsafe ImVector<ImFontPtr> Fonts
		{
			get
			{
				return new ImVector<ImFontPtr>(this.NativePtr->Fonts);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600013A RID: 314 RVA: 0x0000362E File Offset: 0x0000182E
		public unsafe ImPtrVector<ImFontAtlasCustomRectPtr> CustomRects
		{
			get
			{
				return new ImPtrVector<ImFontAtlasCustomRectPtr>(this.NativePtr->CustomRects, Unsafe.SizeOf<ImFontAtlasCustomRect>());
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00003645 File Offset: 0x00001845
		public unsafe ImPtrVector<ImFontConfigPtr> ConfigData
		{
			get
			{
				return new ImPtrVector<ImFontConfigPtr>(this.NativePtr->ConfigData, Unsafe.SizeOf<ImFontConfig>());
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000365C File Offset: 0x0000185C
		public unsafe RangeAccessor<Vector4> TexUvLines
		{
			get
			{
				return new RangeAccessor<Vector4>((void*)(&this.NativePtr->TexUvLines_0), 64);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00003671 File Offset: 0x00001871
		// (set) Token: 0x0600013E RID: 318 RVA: 0x00003683 File Offset: 0x00001883
		public unsafe IntPtr FontBuilderIO
		{
			get
			{
				return (IntPtr)((void*)this.NativePtr->FontBuilderIO);
			}
			set
			{
				this.NativePtr->FontBuilderIO = (IntPtr*)((void*)value);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00003696 File Offset: 0x00001896
		public unsafe ref uint FontBuilderFlags
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->FontBuilderFlags));
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000140 RID: 320 RVA: 0x000036A9 File Offset: 0x000018A9
		public unsafe ref int PackIdMouseCursors
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->PackIdMouseCursors));
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000141 RID: 321 RVA: 0x000036BC File Offset: 0x000018BC
		public unsafe ref int PackIdLines
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->PackIdLines));
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000036D0 File Offset: 0x000018D0
		public unsafe int AddCustomRectFontGlyph(ImFontPtr font, ushort id, int width, int height, float advance_x)
		{
			ImFont* nativePtr = font.NativePtr;
			Vector2 offset = default(Vector2);
			return ImGuiNative.ImFontAtlas_AddCustomRectFontGlyph(this.NativePtr, nativePtr, id, width, height, advance_x, offset);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00003700 File Offset: 0x00001900
		public unsafe int AddCustomRectFontGlyph(ImFontPtr font, ushort id, int width, int height, float advance_x, Vector2 offset)
		{
			ImFont* nativePtr = font.NativePtr;
			return ImGuiNative.ImFontAtlas_AddCustomRectFontGlyph(this.NativePtr, nativePtr, id, width, height, advance_x, offset);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00003729 File Offset: 0x00001929
		public int AddCustomRectRegular(int width, int height)
		{
			return ImGuiNative.ImFontAtlas_AddCustomRectRegular(this.NativePtr, width, height);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00003738 File Offset: 0x00001938
		public unsafe ImFontPtr AddFont(ImFontConfigPtr font_cfg)
		{
			ImFontConfig* nativePtr = font_cfg.NativePtr;
			return new ImFontPtr(ImGuiNative.ImFontAtlas_AddFont(this.NativePtr, nativePtr));
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00003760 File Offset: 0x00001960
		public unsafe ImFontPtr AddFontDefault()
		{
			ImFontConfig* font_cfg = null;
			return new ImFontPtr(ImGuiNative.ImFontAtlas_AddFontDefault(this.NativePtr, font_cfg));
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00003784 File Offset: 0x00001984
		public unsafe ImFontPtr AddFontDefault(ImFontConfigPtr font_cfg)
		{
			ImFontConfig* nativePtr = font_cfg.NativePtr;
			return new ImFontPtr(ImGuiNative.ImFontAtlas_AddFontDefault(this.NativePtr, nativePtr));
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000037AC File Offset: 0x000019AC
		public unsafe ImFontPtr AddFontFromFileTTF(ReadOnlySpan<char> filename, float size_pixels)
		{
			int num = 0;
			byte* ptr;
			if (filename != null)
			{
				num = Encoding.UTF8.GetByteCount(filename);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(filename, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImFontConfig* font_cfg = null;
			ushort* glyph_ranges = null;
			ImFont* nativePtr = ImGuiNative.ImFontAtlas_AddFontFromFileTTF(this.NativePtr, ptr, size_pixels, font_cfg, glyph_ranges);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return new ImFontPtr(nativePtr);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000382C File Offset: 0x00001A2C
		public unsafe ImFontPtr AddFontFromFileTTF(ReadOnlySpan<char> filename, float size_pixels, ImFontConfigPtr font_cfg)
		{
			int num = 0;
			byte* ptr;
			if (filename != null)
			{
				num = Encoding.UTF8.GetByteCount(filename);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(filename, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImFontConfig* nativePtr = font_cfg.NativePtr;
			ushort* glyph_ranges = null;
			ImFont* nativePtr2 = ImGuiNative.ImFontAtlas_AddFontFromFileTTF(this.NativePtr, ptr, size_pixels, nativePtr, glyph_ranges);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return new ImFontPtr(nativePtr2);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000038B4 File Offset: 0x00001AB4
		public unsafe ImFontPtr AddFontFromFileTTF(ReadOnlySpan<char> filename, float size_pixels, ImFontConfigPtr font_cfg, IntPtr glyph_ranges)
		{
			int num = 0;
			byte* ptr;
			if (filename != null)
			{
				num = Encoding.UTF8.GetByteCount(filename);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(filename, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImFontConfig* nativePtr = font_cfg.NativePtr;
			ushort* glyph_ranges2 = (ushort*)glyph_ranges.ToPointer();
			ImFont* nativePtr2 = ImGuiNative.ImFontAtlas_AddFontFromFileTTF(this.NativePtr, ptr, size_pixels, nativePtr, glyph_ranges2);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return new ImFontPtr(nativePtr2);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00003940 File Offset: 0x00001B40
		public unsafe ImFontPtr AddFontFromMemoryCompressedBase85TTF(ReadOnlySpan<char> compressed_font_data_base85, float size_pixels)
		{
			int num = 0;
			byte* ptr;
			if (compressed_font_data_base85 != null)
			{
				num = Encoding.UTF8.GetByteCount(compressed_font_data_base85);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(compressed_font_data_base85, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImFontConfig* font_cfg = null;
			ushort* glyph_ranges = null;
			ImFont* nativePtr = ImGuiNative.ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(this.NativePtr, ptr, size_pixels, font_cfg, glyph_ranges);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return new ImFontPtr(nativePtr);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000039C0 File Offset: 0x00001BC0
		public unsafe ImFontPtr AddFontFromMemoryCompressedBase85TTF(ReadOnlySpan<char> compressed_font_data_base85, float size_pixels, ImFontConfigPtr font_cfg)
		{
			int num = 0;
			byte* ptr;
			if (compressed_font_data_base85 != null)
			{
				num = Encoding.UTF8.GetByteCount(compressed_font_data_base85);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(compressed_font_data_base85, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImFontConfig* nativePtr = font_cfg.NativePtr;
			ushort* glyph_ranges = null;
			ImFont* nativePtr2 = ImGuiNative.ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(this.NativePtr, ptr, size_pixels, nativePtr, glyph_ranges);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return new ImFontPtr(nativePtr2);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00003A48 File Offset: 0x00001C48
		public unsafe ImFontPtr AddFontFromMemoryCompressedBase85TTF(ReadOnlySpan<char> compressed_font_data_base85, float size_pixels, ImFontConfigPtr font_cfg, IntPtr glyph_ranges)
		{
			int num = 0;
			byte* ptr;
			if (compressed_font_data_base85 != null)
			{
				num = Encoding.UTF8.GetByteCount(compressed_font_data_base85);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(compressed_font_data_base85, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImFontConfig* nativePtr = font_cfg.NativePtr;
			ushort* glyph_ranges2 = (ushort*)glyph_ranges.ToPointer();
			ImFont* nativePtr2 = ImGuiNative.ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(this.NativePtr, ptr, size_pixels, nativePtr, glyph_ranges2);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return new ImFontPtr(nativePtr2);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00003AD4 File Offset: 0x00001CD4
		public unsafe ImFontPtr AddFontFromMemoryCompressedTTF(IntPtr compressed_font_data, int compressed_font_size, float size_pixels)
		{
			void* compressed_font_data2 = compressed_font_data.ToPointer();
			ImFontConfig* font_cfg = null;
			ushort* glyph_ranges = null;
			return new ImFontPtr(ImGuiNative.ImFontAtlas_AddFontFromMemoryCompressedTTF(this.NativePtr, compressed_font_data2, compressed_font_size, size_pixels, font_cfg, glyph_ranges));
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00003B04 File Offset: 0x00001D04
		public unsafe ImFontPtr AddFontFromMemoryCompressedTTF(IntPtr compressed_font_data, int compressed_font_size, float size_pixels, ImFontConfigPtr font_cfg)
		{
			void* compressed_font_data2 = compressed_font_data.ToPointer();
			ImFontConfig* nativePtr = font_cfg.NativePtr;
			ushort* glyph_ranges = null;
			return new ImFontPtr(ImGuiNative.ImFontAtlas_AddFontFromMemoryCompressedTTF(this.NativePtr, compressed_font_data2, compressed_font_size, size_pixels, nativePtr, glyph_ranges));
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00003B3C File Offset: 0x00001D3C
		public unsafe ImFontPtr AddFontFromMemoryCompressedTTF(IntPtr compressed_font_data, int compressed_font_size, float size_pixels, ImFontConfigPtr font_cfg, IntPtr glyph_ranges)
		{
			void* compressed_font_data2 = compressed_font_data.ToPointer();
			ImFontConfig* nativePtr = font_cfg.NativePtr;
			ushort* glyph_ranges2 = (ushort*)glyph_ranges.ToPointer();
			return new ImFontPtr(ImGuiNative.ImFontAtlas_AddFontFromMemoryCompressedTTF(this.NativePtr, compressed_font_data2, compressed_font_size, size_pixels, nativePtr, glyph_ranges2));
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00003B78 File Offset: 0x00001D78
		public unsafe ImFontPtr AddFontFromMemoryTTF(IntPtr font_data, int font_size, float size_pixels)
		{
			void* font_data2 = font_data.ToPointer();
			ImFontConfig* font_cfg = null;
			ushort* glyph_ranges = null;
			return new ImFontPtr(ImGuiNative.ImFontAtlas_AddFontFromMemoryTTF(this.NativePtr, font_data2, font_size, size_pixels, font_cfg, glyph_ranges));
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00003BA8 File Offset: 0x00001DA8
		public unsafe ImFontPtr AddFontFromMemoryTTF(IntPtr font_data, int font_size, float size_pixels, ImFontConfigPtr font_cfg)
		{
			void* font_data2 = font_data.ToPointer();
			ImFontConfig* nativePtr = font_cfg.NativePtr;
			ushort* glyph_ranges = null;
			return new ImFontPtr(ImGuiNative.ImFontAtlas_AddFontFromMemoryTTF(this.NativePtr, font_data2, font_size, size_pixels, nativePtr, glyph_ranges));
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00003BE0 File Offset: 0x00001DE0
		public unsafe ImFontPtr AddFontFromMemoryTTF(IntPtr font_data, int font_size, float size_pixels, ImFontConfigPtr font_cfg, IntPtr glyph_ranges)
		{
			void* font_data2 = font_data.ToPointer();
			ImFontConfig* nativePtr = font_cfg.NativePtr;
			ushort* glyph_ranges2 = (ushort*)glyph_ranges.ToPointer();
			return new ImFontPtr(ImGuiNative.ImFontAtlas_AddFontFromMemoryTTF(this.NativePtr, font_data2, font_size, size_pixels, nativePtr, glyph_ranges2));
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00003C1A File Offset: 0x00001E1A
		public bool Build()
		{
			return ImGuiNative.ImFontAtlas_Build(this.NativePtr) > 0;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00003C2C File Offset: 0x00001E2C
		public unsafe void CalcCustomRectUV(ImFontAtlasCustomRectPtr rect, out Vector2 out_uv_min, out Vector2 out_uv_max)
		{
			ImFontAtlasCustomRect* nativePtr = rect.NativePtr;
			fixed (Vector2* ptr = &out_uv_min)
			{
				Vector2* out_uv_min2 = ptr;
				fixed (Vector2* ptr2 = &out_uv_max)
				{
					Vector2* out_uv_max2 = ptr2;
					ImGuiNative.ImFontAtlas_CalcCustomRectUV(this.NativePtr, nativePtr, out_uv_min2, out_uv_max2);
				}
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00003C62 File Offset: 0x00001E62
		public void Clear()
		{
			ImGuiNative.ImFontAtlas_Clear(this.NativePtr);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00003C6F File Offset: 0x00001E6F
		public void ClearFonts()
		{
			ImGuiNative.ImFontAtlas_ClearFonts(this.NativePtr);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00003C7C File Offset: 0x00001E7C
		public void ClearInputData()
		{
			ImGuiNative.ImFontAtlas_ClearInputData(this.NativePtr);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00003C89 File Offset: 0x00001E89
		public void ClearTexData()
		{
			ImGuiNative.ImFontAtlas_ClearTexData(this.NativePtr);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00003C96 File Offset: 0x00001E96
		public void Destroy()
		{
			ImGuiNative.ImFontAtlas_destroy(this.NativePtr);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00003CA3 File Offset: 0x00001EA3
		public ImFontAtlasCustomRectPtr GetCustomRectByIndex(int index)
		{
			return new ImFontAtlasCustomRectPtr(ImGuiNative.ImFontAtlas_GetCustomRectByIndex(this.NativePtr, index));
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00003CB6 File Offset: 0x00001EB6
		public unsafe IntPtr GetGlyphRangesChineseFull()
		{
			return (IntPtr)((void*)ImGuiNative.ImFontAtlas_GetGlyphRangesChineseFull(this.NativePtr));
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00003CC8 File Offset: 0x00001EC8
		public unsafe IntPtr GetGlyphRangesChineseSimplifiedCommon()
		{
			return (IntPtr)((void*)ImGuiNative.ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(this.NativePtr));
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00003CDA File Offset: 0x00001EDA
		public unsafe IntPtr GetGlyphRangesCyrillic()
		{
			return (IntPtr)((void*)ImGuiNative.ImFontAtlas_GetGlyphRangesCyrillic(this.NativePtr));
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00003CEC File Offset: 0x00001EEC
		public unsafe IntPtr GetGlyphRangesDefault()
		{
			return (IntPtr)((void*)ImGuiNative.ImFontAtlas_GetGlyphRangesDefault(this.NativePtr));
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00003CFE File Offset: 0x00001EFE
		public unsafe IntPtr GetGlyphRangesGreek()
		{
			return (IntPtr)((void*)ImGuiNative.ImFontAtlas_GetGlyphRangesGreek(this.NativePtr));
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00003D10 File Offset: 0x00001F10
		public unsafe IntPtr GetGlyphRangesJapanese()
		{
			return (IntPtr)((void*)ImGuiNative.ImFontAtlas_GetGlyphRangesJapanese(this.NativePtr));
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00003D22 File Offset: 0x00001F22
		public unsafe IntPtr GetGlyphRangesKorean()
		{
			return (IntPtr)((void*)ImGuiNative.ImFontAtlas_GetGlyphRangesKorean(this.NativePtr));
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00003D34 File Offset: 0x00001F34
		public unsafe IntPtr GetGlyphRangesThai()
		{
			return (IntPtr)((void*)ImGuiNative.ImFontAtlas_GetGlyphRangesThai(this.NativePtr));
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00003D46 File Offset: 0x00001F46
		public unsafe IntPtr GetGlyphRangesVietnamese()
		{
			return (IntPtr)((void*)ImGuiNative.ImFontAtlas_GetGlyphRangesVietnamese(this.NativePtr));
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00003D58 File Offset: 0x00001F58
		public unsafe bool GetMouseCursorTexData(ImGuiMouseCursor cursor, out Vector2 out_offset, out Vector2 out_size, out Vector2 out_uv_border, out Vector2 out_uv_fill)
		{
			fixed (Vector2* ptr = &out_offset)
			{
				Vector2* out_offset2 = ptr;
				fixed (Vector2* ptr2 = &out_size)
				{
					Vector2* out_size2 = ptr2;
					fixed (Vector2* ptr3 = &out_uv_border)
					{
						Vector2* out_uv_border2 = ptr3;
						fixed (Vector2* ptr4 = &out_uv_fill)
						{
							Vector2* out_uv_fill2 = ptr4;
							return ImGuiNative.ImFontAtlas_GetMouseCursorTexData(this.NativePtr, cursor, out_offset2, out_size2, out_uv_border2, out_uv_fill2) > 0;
						}
					}
				}
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00003D98 File Offset: 0x00001F98
		public unsafe void GetTexDataAsAlpha8(out byte* out_pixels, out int out_width, out int out_height)
		{
			int* out_bytes_per_pixel = null;
			fixed (byte** ptr = &out_pixels)
			{
				byte** out_pixels2 = ptr;
				fixed (int* ptr2 = &out_width)
				{
					int* out_width2 = ptr2;
					fixed (int* ptr3 = &out_height)
					{
						int* out_height2 = ptr3;
						ImGuiNative.ImFontAtlas_GetTexDataAsAlpha8(this.NativePtr, out_pixels2, out_width2, out_height2, out_bytes_per_pixel);
					}
				}
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00003DD8 File Offset: 0x00001FD8
		public unsafe void GetTexDataAsAlpha8(out byte* out_pixels, out int out_width, out int out_height, out int out_bytes_per_pixel)
		{
			fixed (byte** ptr = &out_pixels)
			{
				byte** out_pixels2 = ptr;
				fixed (int* ptr2 = &out_width)
				{
					int* out_width2 = ptr2;
					fixed (int* ptr3 = &out_height)
					{
						int* out_height2 = ptr3;
						fixed (int* ptr4 = &out_bytes_per_pixel)
						{
							int* out_bytes_per_pixel2 = ptr4;
							ImGuiNative.ImFontAtlas_GetTexDataAsAlpha8(this.NativePtr, out_pixels2, out_width2, out_height2, out_bytes_per_pixel2);
						}
					}
				}
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00003E20 File Offset: 0x00002020
		public unsafe void GetTexDataAsAlpha8(out IntPtr out_pixels, out int out_width, out int out_height)
		{
			int* out_bytes_per_pixel = null;
			fixed (IntPtr* ptr = &out_pixels)
			{
				IntPtr* out_pixels2 = ptr;
				fixed (int* ptr2 = &out_width)
				{
					int* out_width2 = ptr2;
					fixed (int* ptr3 = &out_height)
					{
						int* out_height2 = ptr3;
						ImGuiNative.ImFontAtlas_GetTexDataAsAlpha8(this.NativePtr, out_pixels2, out_width2, out_height2, out_bytes_per_pixel);
					}
				}
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00003E60 File Offset: 0x00002060
		public unsafe void GetTexDataAsAlpha8(out IntPtr out_pixels, out int out_width, out int out_height, out int out_bytes_per_pixel)
		{
			fixed (IntPtr* ptr = &out_pixels)
			{
				IntPtr* out_pixels2 = ptr;
				fixed (int* ptr2 = &out_width)
				{
					int* out_width2 = ptr2;
					fixed (int* ptr3 = &out_height)
					{
						int* out_height2 = ptr3;
						fixed (int* ptr4 = &out_bytes_per_pixel)
						{
							int* out_bytes_per_pixel2 = ptr4;
							ImGuiNative.ImFontAtlas_GetTexDataAsAlpha8(this.NativePtr, out_pixels2, out_width2, out_height2, out_bytes_per_pixel2);
						}
					}
				}
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00003EA8 File Offset: 0x000020A8
		public unsafe void GetTexDataAsRGBA32(out byte* out_pixels, out int out_width, out int out_height)
		{
			int* out_bytes_per_pixel = null;
			fixed (byte** ptr = &out_pixels)
			{
				byte** out_pixels2 = ptr;
				fixed (int* ptr2 = &out_width)
				{
					int* out_width2 = ptr2;
					fixed (int* ptr3 = &out_height)
					{
						int* out_height2 = ptr3;
						ImGuiNative.ImFontAtlas_GetTexDataAsRGBA32(this.NativePtr, out_pixels2, out_width2, out_height2, out_bytes_per_pixel);
					}
				}
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00003EE8 File Offset: 0x000020E8
		public unsafe void GetTexDataAsRGBA32(out byte* out_pixels, out int out_width, out int out_height, out int out_bytes_per_pixel)
		{
			fixed (byte** ptr = &out_pixels)
			{
				byte** out_pixels2 = ptr;
				fixed (int* ptr2 = &out_width)
				{
					int* out_width2 = ptr2;
					fixed (int* ptr3 = &out_height)
					{
						int* out_height2 = ptr3;
						fixed (int* ptr4 = &out_bytes_per_pixel)
						{
							int* out_bytes_per_pixel2 = ptr4;
							ImGuiNative.ImFontAtlas_GetTexDataAsRGBA32(this.NativePtr, out_pixels2, out_width2, out_height2, out_bytes_per_pixel2);
						}
					}
				}
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00003F30 File Offset: 0x00002130
		public unsafe void GetTexDataAsRGBA32(out IntPtr out_pixels, out int out_width, out int out_height)
		{
			int* out_bytes_per_pixel = null;
			fixed (IntPtr* ptr = &out_pixels)
			{
				IntPtr* out_pixels2 = ptr;
				fixed (int* ptr2 = &out_width)
				{
					int* out_width2 = ptr2;
					fixed (int* ptr3 = &out_height)
					{
						int* out_height2 = ptr3;
						ImGuiNative.ImFontAtlas_GetTexDataAsRGBA32(this.NativePtr, out_pixels2, out_width2, out_height2, out_bytes_per_pixel);
					}
				}
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00003F70 File Offset: 0x00002170
		public unsafe void GetTexDataAsRGBA32(out IntPtr out_pixels, out int out_width, out int out_height, out int out_bytes_per_pixel)
		{
			fixed (IntPtr* ptr = &out_pixels)
			{
				IntPtr* out_pixels2 = ptr;
				fixed (int* ptr2 = &out_width)
				{
					int* out_width2 = ptr2;
					fixed (int* ptr3 = &out_height)
					{
						int* out_height2 = ptr3;
						fixed (int* ptr4 = &out_bytes_per_pixel)
						{
							int* out_bytes_per_pixel2 = ptr4;
							ImGuiNative.ImFontAtlas_GetTexDataAsRGBA32(this.NativePtr, out_pixels2, out_width2, out_height2, out_bytes_per_pixel2);
						}
					}
				}
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00003FB7 File Offset: 0x000021B7
		public bool IsBuilt()
		{
			return ImGuiNative.ImFontAtlas_IsBuilt(this.NativePtr) > 0;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00003FC7 File Offset: 0x000021C7
		public void SetTexID(IntPtr id)
		{
			ImGuiNative.ImFontAtlas_SetTexID(this.NativePtr, id);
		}
	}
}
