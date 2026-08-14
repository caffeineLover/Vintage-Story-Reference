using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x0200004B RID: 75
	public struct ImGuiOnceUponAFramePtr
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x00014C2F File Offset: 0x00012E2F
		public unsafe readonly ImGuiOnceUponAFrame* NativePtr { get; }

		// Token: 0x060007B0 RID: 1968 RVA: 0x00014C37 File Offset: 0x00012E37
		public unsafe ImGuiOnceUponAFramePtr(ImGuiOnceUponAFrame* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00014C40 File Offset: 0x00012E40
		public unsafe ImGuiOnceUponAFramePtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00014C4E File Offset: 0x00012E4E
		public unsafe static implicit operator ImGuiOnceUponAFramePtr(ImGuiOnceUponAFrame* nativePtr)
		{
			return new ImGuiOnceUponAFramePtr(nativePtr);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00014C56 File Offset: 0x00012E56
		public unsafe static implicit operator ImGuiOnceUponAFrame*(ImGuiOnceUponAFramePtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00014C5F File Offset: 0x00012E5F
		public static implicit operator ImGuiOnceUponAFramePtr(IntPtr nativePtr)
		{
			return new ImGuiOnceUponAFramePtr(nativePtr);
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00014C67 File Offset: 0x00012E67
		public unsafe ref int RefFrame
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->RefFrame));
			}
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00014C7A File Offset: 0x00012E7A
		public void Destroy()
		{
			ImGuiNative.ImGuiOnceUponAFrame_destroy(this.NativePtr);
		}
	}
}
