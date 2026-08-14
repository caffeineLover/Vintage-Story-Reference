using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000012 RID: 18
	public struct ImDrawCmdPtr
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002179 File Offset: 0x00000379
		public unsafe readonly ImDrawCmd* NativePtr { get; }

		// Token: 0x06000042 RID: 66 RVA: 0x00002181 File Offset: 0x00000381
		public unsafe ImDrawCmdPtr(ImDrawCmd* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000218A File Offset: 0x0000038A
		public unsafe ImDrawCmdPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002198 File Offset: 0x00000398
		public unsafe static implicit operator ImDrawCmdPtr(ImDrawCmd* nativePtr)
		{
			return new ImDrawCmdPtr(nativePtr);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000021A0 File Offset: 0x000003A0
		public unsafe static implicit operator ImDrawCmd*(ImDrawCmdPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000021A9 File Offset: 0x000003A9
		public static implicit operator ImDrawCmdPtr(IntPtr nativePtr)
		{
			return new ImDrawCmdPtr(nativePtr);
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000021B1 File Offset: 0x000003B1
		public unsafe ref Vector4 ClipRect
		{
			get
			{
				return Unsafe.AsRef<Vector4>((void*)(&this.NativePtr->ClipRect));
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000048 RID: 72 RVA: 0x000021C4 File Offset: 0x000003C4
		public unsafe ref IntPtr TextureId
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->TextureId));
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000049 RID: 73 RVA: 0x000021D7 File Offset: 0x000003D7
		public unsafe ref uint VtxOffset
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->VtxOffset));
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004A RID: 74 RVA: 0x000021EA File Offset: 0x000003EA
		public unsafe ref uint IdxOffset
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->IdxOffset));
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600004B RID: 75 RVA: 0x000021FD File Offset: 0x000003FD
		public unsafe ref uint ElemCount
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->ElemCount));
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002210 File Offset: 0x00000410
		public unsafe ref IntPtr UserCallback
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->UserCallback));
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002223 File Offset: 0x00000423
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00002235 File Offset: 0x00000435
		public unsafe IntPtr UserCallbackData
		{
			get
			{
				return (IntPtr)this.NativePtr->UserCallbackData;
			}
			set
			{
				this.NativePtr->UserCallbackData = (void*)value;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002248 File Offset: 0x00000448
		public void Destroy()
		{
			ImGuiNative.ImDrawCmd_destroy(this.NativePtr);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002255 File Offset: 0x00000455
		public IntPtr GetTexID()
		{
			return ImGuiNative.ImDrawCmd_GetTexID(this.NativePtr);
		}
	}
}
