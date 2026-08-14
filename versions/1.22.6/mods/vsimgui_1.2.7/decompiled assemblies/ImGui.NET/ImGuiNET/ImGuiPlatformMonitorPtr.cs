using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000053 RID: 83
	public struct ImGuiPlatformMonitorPtr
	{
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x000150C0 File Offset: 0x000132C0
		public unsafe readonly ImGuiPlatformMonitor* NativePtr { get; }

		// Token: 0x060007F6 RID: 2038 RVA: 0x000150C8 File Offset: 0x000132C8
		public unsafe ImGuiPlatformMonitorPtr(ImGuiPlatformMonitor* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x000150D1 File Offset: 0x000132D1
		public unsafe ImGuiPlatformMonitorPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x000150DF File Offset: 0x000132DF
		public unsafe static implicit operator ImGuiPlatformMonitorPtr(ImGuiPlatformMonitor* nativePtr)
		{
			return new ImGuiPlatformMonitorPtr(nativePtr);
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x000150E7 File Offset: 0x000132E7
		public unsafe static implicit operator ImGuiPlatformMonitor*(ImGuiPlatformMonitorPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x000150F0 File Offset: 0x000132F0
		public static implicit operator ImGuiPlatformMonitorPtr(IntPtr nativePtr)
		{
			return new ImGuiPlatformMonitorPtr(nativePtr);
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x000150F8 File Offset: 0x000132F8
		public unsafe ref Vector2 MainPos
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->MainPos));
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x0001510B File Offset: 0x0001330B
		public unsafe ref Vector2 MainSize
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->MainSize));
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x0001511E File Offset: 0x0001331E
		public unsafe ref Vector2 WorkPos
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->WorkPos));
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x00015131 File Offset: 0x00013331
		public unsafe ref Vector2 WorkSize
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->WorkSize));
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x00015144 File Offset: 0x00013344
		public unsafe ref float DpiScale
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->DpiScale));
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x00015157 File Offset: 0x00013357
		// (set) Token: 0x06000801 RID: 2049 RVA: 0x00015169 File Offset: 0x00013369
		public unsafe IntPtr PlatformHandle
		{
			get
			{
				return (IntPtr)this.NativePtr->PlatformHandle;
			}
			set
			{
				this.NativePtr->PlatformHandle = (void*)value;
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0001517C File Offset: 0x0001337C
		public void Destroy()
		{
			ImGuiNative.ImGuiPlatformMonitor_destroy(this.NativePtr);
		}
	}
}
