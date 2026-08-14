using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000024 RID: 36
	public struct ImFontAtlasCustomRectPtr
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00003FD5 File Offset: 0x000021D5
		public unsafe readonly ImFontAtlasCustomRect* NativePtr { get; }

		// Token: 0x06000171 RID: 369 RVA: 0x00003FDD File Offset: 0x000021DD
		public unsafe ImFontAtlasCustomRectPtr(ImFontAtlasCustomRect* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00003FE6 File Offset: 0x000021E6
		public unsafe ImFontAtlasCustomRectPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00003FF4 File Offset: 0x000021F4
		public unsafe static implicit operator ImFontAtlasCustomRectPtr(ImFontAtlasCustomRect* nativePtr)
		{
			return new ImFontAtlasCustomRectPtr(nativePtr);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00003FFC File Offset: 0x000021FC
		public unsafe static implicit operator ImFontAtlasCustomRect*(ImFontAtlasCustomRectPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00004005 File Offset: 0x00002205
		public static implicit operator ImFontAtlasCustomRectPtr(IntPtr nativePtr)
		{
			return new ImFontAtlasCustomRectPtr(nativePtr);
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000176 RID: 374 RVA: 0x0000400D File Offset: 0x0000220D
		public unsafe ref ushort Width
		{
			get
			{
				return Unsafe.AsRef<ushort>((void*)(&this.NativePtr->Width));
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00004020 File Offset: 0x00002220
		public unsafe ref ushort Height
		{
			get
			{
				return Unsafe.AsRef<ushort>((void*)(&this.NativePtr->Height));
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00004033 File Offset: 0x00002233
		public unsafe ref ushort X
		{
			get
			{
				return Unsafe.AsRef<ushort>((void*)(&this.NativePtr->X));
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00004046 File Offset: 0x00002246
		public unsafe ref ushort Y
		{
			get
			{
				return Unsafe.AsRef<ushort>((void*)(&this.NativePtr->Y));
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00004059 File Offset: 0x00002259
		public unsafe ref uint GlyphID
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->GlyphID));
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600017B RID: 379 RVA: 0x0000406C File Offset: 0x0000226C
		public unsafe ref float GlyphAdvanceX
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->GlyphAdvanceX));
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600017C RID: 380 RVA: 0x0000407F File Offset: 0x0000227F
		public unsafe ref Vector2 GlyphOffset
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->GlyphOffset));
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00004092 File Offset: 0x00002292
		public unsafe ImFontPtr Font
		{
			get
			{
				return new ImFontPtr(this.NativePtr->Font);
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000040A4 File Offset: 0x000022A4
		public void Destroy()
		{
			ImGuiNative.ImFontAtlasCustomRect_destroy(this.NativePtr);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000040B1 File Offset: 0x000022B1
		public bool IsPacked()
		{
			return ImGuiNative.ImFontAtlasCustomRect_IsPacked(this.NativePtr) > 0;
		}
	}
}
