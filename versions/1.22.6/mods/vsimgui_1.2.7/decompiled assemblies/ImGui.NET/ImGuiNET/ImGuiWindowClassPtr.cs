using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000074 RID: 116
	public struct ImGuiWindowClassPtr
	{
		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x000160A8 File Offset: 0x000142A8
		public unsafe readonly ImGuiWindowClass* NativePtr { get; }

		// Token: 0x060008C8 RID: 2248 RVA: 0x000160B0 File Offset: 0x000142B0
		public unsafe ImGuiWindowClassPtr(ImGuiWindowClass* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x000160B9 File Offset: 0x000142B9
		public unsafe ImGuiWindowClassPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x000160C7 File Offset: 0x000142C7
		public unsafe static implicit operator ImGuiWindowClassPtr(ImGuiWindowClass* nativePtr)
		{
			return new ImGuiWindowClassPtr(nativePtr);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x000160CF File Offset: 0x000142CF
		public unsafe static implicit operator ImGuiWindowClass*(ImGuiWindowClassPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x000160D8 File Offset: 0x000142D8
		public static implicit operator ImGuiWindowClassPtr(IntPtr nativePtr)
		{
			return new ImGuiWindowClassPtr(nativePtr);
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x000160E0 File Offset: 0x000142E0
		public unsafe ref uint ClassId
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->ClassId));
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x000160F3 File Offset: 0x000142F3
		public unsafe ref uint ParentViewportId
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->ParentViewportId));
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x00016106 File Offset: 0x00014306
		public unsafe ref ImGuiViewportFlags ViewportFlagsOverrideSet
		{
			get
			{
				return Unsafe.AsRef<ImGuiViewportFlags>((void*)(&this.NativePtr->ViewportFlagsOverrideSet));
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x00016119 File Offset: 0x00014319
		public unsafe ref ImGuiViewportFlags ViewportFlagsOverrideClear
		{
			get
			{
				return Unsafe.AsRef<ImGuiViewportFlags>((void*)(&this.NativePtr->ViewportFlagsOverrideClear));
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x0001612C File Offset: 0x0001432C
		public unsafe ref ImGuiTabItemFlags TabItemFlagsOverrideSet
		{
			get
			{
				return Unsafe.AsRef<ImGuiTabItemFlags>((void*)(&this.NativePtr->TabItemFlagsOverrideSet));
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0001613F File Offset: 0x0001433F
		public unsafe ref ImGuiDockNodeFlags DockNodeFlagsOverrideSet
		{
			get
			{
				return Unsafe.AsRef<ImGuiDockNodeFlags>((void*)(&this.NativePtr->DockNodeFlagsOverrideSet));
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x00016152 File Offset: 0x00014352
		public unsafe ref bool DockingAlwaysTabBar
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->DockingAlwaysTabBar));
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x00016165 File Offset: 0x00014365
		public unsafe ref bool DockingAllowUnclassed
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->DockingAllowUnclassed));
			}
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x00016178 File Offset: 0x00014378
		public void Destroy()
		{
			ImGuiNative.ImGuiWindowClass_destroy(this.NativePtr);
		}
	}
}
