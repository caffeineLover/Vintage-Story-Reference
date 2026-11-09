using System;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace ImPlotNET
{
	// Token: 0x02000016 RID: 22
	public struct ImPlotInputMapPtr
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x0002F80D File Offset: 0x0002DA0D
		public unsafe readonly ImPlotInputMap* NativePtr { get; }

		// Token: 0x06000530 RID: 1328 RVA: 0x0002F815 File Offset: 0x0002DA15
		public unsafe ImPlotInputMapPtr(ImPlotInputMap* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0002F81E File Offset: 0x0002DA1E
		public unsafe ImPlotInputMapPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0002F82C File Offset: 0x0002DA2C
		public unsafe static implicit operator ImPlotInputMapPtr(ImPlotInputMap* nativePtr)
		{
			return new ImPlotInputMapPtr(nativePtr);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0002F834 File Offset: 0x0002DA34
		public unsafe static implicit operator ImPlotInputMap*(ImPlotInputMapPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0002F83D File Offset: 0x0002DA3D
		public static implicit operator ImPlotInputMapPtr(IntPtr nativePtr)
		{
			return new ImPlotInputMapPtr(nativePtr);
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x0002F845 File Offset: 0x0002DA45
		public unsafe ref ImGuiMouseButton Pan
		{
			get
			{
				return Unsafe.AsRef<ImGuiMouseButton>((void*)(&this.NativePtr->Pan));
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x0002F858 File Offset: 0x0002DA58
		public unsafe ref ImGuiModFlags PanMod
		{
			get
			{
				return Unsafe.AsRef<ImGuiModFlags>((void*)(&this.NativePtr->PanMod));
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x0002F86B File Offset: 0x0002DA6B
		public unsafe ref ImGuiMouseButton Fit
		{
			get
			{
				return Unsafe.AsRef<ImGuiMouseButton>((void*)(&this.NativePtr->Fit));
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0002F87E File Offset: 0x0002DA7E
		public unsafe ref ImGuiMouseButton Select
		{
			get
			{
				return Unsafe.AsRef<ImGuiMouseButton>((void*)(&this.NativePtr->Select));
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x0002F891 File Offset: 0x0002DA91
		public unsafe ref ImGuiMouseButton SelectCancel
		{
			get
			{
				return Unsafe.AsRef<ImGuiMouseButton>((void*)(&this.NativePtr->SelectCancel));
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x0002F8A4 File Offset: 0x0002DAA4
		public unsafe ref ImGuiModFlags SelectMod
		{
			get
			{
				return Unsafe.AsRef<ImGuiModFlags>((void*)(&this.NativePtr->SelectMod));
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x0002F8B7 File Offset: 0x0002DAB7
		public unsafe ref ImGuiModFlags SelectHorzMod
		{
			get
			{
				return Unsafe.AsRef<ImGuiModFlags>((void*)(&this.NativePtr->SelectHorzMod));
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0002F8CA File Offset: 0x0002DACA
		public unsafe ref ImGuiModFlags SelectVertMod
		{
			get
			{
				return Unsafe.AsRef<ImGuiModFlags>((void*)(&this.NativePtr->SelectVertMod));
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x0002F8DD File Offset: 0x0002DADD
		public unsafe ref ImGuiMouseButton Menu
		{
			get
			{
				return Unsafe.AsRef<ImGuiMouseButton>((void*)(&this.NativePtr->Menu));
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x0002F8F0 File Offset: 0x0002DAF0
		public unsafe ref ImGuiModFlags OverrideMod
		{
			get
			{
				return Unsafe.AsRef<ImGuiModFlags>((void*)(&this.NativePtr->OverrideMod));
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x0002F903 File Offset: 0x0002DB03
		public unsafe ref ImGuiModFlags ZoomMod
		{
			get
			{
				return Unsafe.AsRef<ImGuiModFlags>((void*)(&this.NativePtr->ZoomMod));
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x0002F916 File Offset: 0x0002DB16
		public unsafe ref float ZoomRate
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->ZoomRate));
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0002F929 File Offset: 0x0002DB29
		public void Destroy()
		{
			ImPlotNative.ImPlotInputMap_destroy(this.NativePtr);
		}
	}
}
