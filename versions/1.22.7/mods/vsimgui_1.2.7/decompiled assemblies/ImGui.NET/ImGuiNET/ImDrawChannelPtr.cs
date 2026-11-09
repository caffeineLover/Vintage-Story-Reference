using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000010 RID: 16
	public struct ImDrawChannelPtr
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002118 File Offset: 0x00000318
		public unsafe readonly ImDrawChannel* NativePtr { get; }

		// Token: 0x0600003A RID: 58 RVA: 0x00002120 File Offset: 0x00000320
		public unsafe ImDrawChannelPtr(ImDrawChannel* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002129 File Offset: 0x00000329
		public unsafe ImDrawChannelPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002137 File Offset: 0x00000337
		public unsafe static implicit operator ImDrawChannelPtr(ImDrawChannel* nativePtr)
		{
			return new ImDrawChannelPtr(nativePtr);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000213F File Offset: 0x0000033F
		public unsafe static implicit operator ImDrawChannel*(ImDrawChannelPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002148 File Offset: 0x00000348
		public static implicit operator ImDrawChannelPtr(IntPtr nativePtr)
		{
			return new ImDrawChannelPtr(nativePtr);
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002150 File Offset: 0x00000350
		public unsafe ImPtrVector<ImDrawCmdPtr> _CmdBuffer
		{
			get
			{
				return new ImPtrVector<ImDrawCmdPtr>(this.NativePtr->_CmdBuffer, Unsafe.SizeOf<ImDrawCmd>());
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002167 File Offset: 0x00000367
		public unsafe ImVector<ushort> _IdxBuffer
		{
			get
			{
				return new ImVector<ushort>(this.NativePtr->_IdxBuffer);
			}
		}
	}
}
