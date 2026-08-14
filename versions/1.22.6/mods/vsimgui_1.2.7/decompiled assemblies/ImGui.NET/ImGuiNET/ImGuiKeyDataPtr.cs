using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000041 RID: 65
	public struct ImGuiKeyDataPtr
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x00014A74 File Offset: 0x00012C74
		public unsafe readonly ImGuiKeyData* NativePtr { get; }

		// Token: 0x06000510 RID: 1296 RVA: 0x00014A7C File Offset: 0x00012C7C
		public unsafe ImGuiKeyDataPtr(ImGuiKeyData* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00014A85 File Offset: 0x00012C85
		public unsafe ImGuiKeyDataPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00014A93 File Offset: 0x00012C93
		public unsafe static implicit operator ImGuiKeyDataPtr(ImGuiKeyData* nativePtr)
		{
			return new ImGuiKeyDataPtr(nativePtr);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00014A9B File Offset: 0x00012C9B
		public unsafe static implicit operator ImGuiKeyData*(ImGuiKeyDataPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00014AA4 File Offset: 0x00012CA4
		public static implicit operator ImGuiKeyDataPtr(IntPtr nativePtr)
		{
			return new ImGuiKeyDataPtr(nativePtr);
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x00014AAC File Offset: 0x00012CAC
		public unsafe ref bool Down
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->Down));
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x00014ABF File Offset: 0x00012CBF
		public unsafe ref float DownDuration
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->DownDuration));
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x00014AD2 File Offset: 0x00012CD2
		public unsafe ref float DownDurationPrev
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->DownDurationPrev));
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x00014AE5 File Offset: 0x00012CE5
		public unsafe ref float AnalogValue
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->AnalogValue));
			}
		}
	}
}
