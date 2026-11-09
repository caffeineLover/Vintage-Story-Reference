using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000014 RID: 20
	public struct ImDrawCmdHeaderPtr
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002262 File Offset: 0x00000462
		public unsafe readonly ImDrawCmdHeader* NativePtr { get; }

		// Token: 0x06000052 RID: 82 RVA: 0x0000226A File Offset: 0x0000046A
		public unsafe ImDrawCmdHeaderPtr(ImDrawCmdHeader* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002273 File Offset: 0x00000473
		public unsafe ImDrawCmdHeaderPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002281 File Offset: 0x00000481
		public unsafe static implicit operator ImDrawCmdHeaderPtr(ImDrawCmdHeader* nativePtr)
		{
			return new ImDrawCmdHeaderPtr(nativePtr);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002289 File Offset: 0x00000489
		public unsafe static implicit operator ImDrawCmdHeader*(ImDrawCmdHeaderPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002292 File Offset: 0x00000492
		public static implicit operator ImDrawCmdHeaderPtr(IntPtr nativePtr)
		{
			return new ImDrawCmdHeaderPtr(nativePtr);
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000057 RID: 87 RVA: 0x0000229A File Offset: 0x0000049A
		public unsafe ref Vector4 ClipRect
		{
			get
			{
				return Unsafe.AsRef<Vector4>((void*)(&this.NativePtr->ClipRect));
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000022AD File Offset: 0x000004AD
		public unsafe ref IntPtr TextureId
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->TextureId));
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000059 RID: 89 RVA: 0x000022C0 File Offset: 0x000004C0
		public unsafe ref uint VtxOffset
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->VtxOffset));
			}
		}
	}
}
