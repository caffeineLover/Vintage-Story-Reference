using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x0200001E RID: 30
	public struct ImDrawVertPtr
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00003132 File Offset: 0x00001332
		public unsafe readonly ImDrawVert* NativePtr { get; }

		// Token: 0x060000F2 RID: 242 RVA: 0x0000313A File Offset: 0x0000133A
		public unsafe ImDrawVertPtr(ImDrawVert* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00003143 File Offset: 0x00001343
		public unsafe ImDrawVertPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00003151 File Offset: 0x00001351
		public unsafe static implicit operator ImDrawVertPtr(ImDrawVert* nativePtr)
		{
			return new ImDrawVertPtr(nativePtr);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00003159 File Offset: 0x00001359
		public unsafe static implicit operator ImDrawVert*(ImDrawVertPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00003162 File Offset: 0x00001362
		public static implicit operator ImDrawVertPtr(IntPtr nativePtr)
		{
			return new ImDrawVertPtr(nativePtr);
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x0000316A File Offset: 0x0000136A
		public unsafe ref Vector2 pos
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->pos));
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x0000317D File Offset: 0x0000137D
		public unsafe ref Vector2 uv
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->uv));
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00003190 File Offset: 0x00001390
		public unsafe ref uint col
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->col));
			}
		}
	}
}
