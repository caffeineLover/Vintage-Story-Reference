using System;

namespace ImGuiNET
{
	// Token: 0x02000085 RID: 133
	public struct ImGuiStoragePairPtr
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x00016600 File Offset: 0x00014800
		public unsafe readonly ImGuiStoragePair* NativePtr { get; }

		// Token: 0x06000921 RID: 2337 RVA: 0x00016608 File Offset: 0x00014808
		public unsafe ImGuiStoragePairPtr(ImGuiStoragePair* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00016611 File Offset: 0x00014811
		public unsafe ImGuiStoragePairPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0001661F File Offset: 0x0001481F
		public unsafe static implicit operator ImGuiStoragePairPtr(ImGuiStoragePair* nativePtr)
		{
			return new ImGuiStoragePairPtr(nativePtr);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00016627 File Offset: 0x00014827
		public unsafe static implicit operator ImGuiStoragePair*(ImGuiStoragePairPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00016630 File Offset: 0x00014830
		public static implicit operator ImGuiStoragePairPtr(IntPtr nativePtr)
		{
			return new ImGuiStoragePairPtr(nativePtr);
		}
	}
}
