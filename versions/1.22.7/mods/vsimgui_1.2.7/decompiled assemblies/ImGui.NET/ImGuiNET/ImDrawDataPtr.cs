using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000016 RID: 22
	public struct ImDrawDataPtr
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005A RID: 90 RVA: 0x000022D3 File Offset: 0x000004D3
		public unsafe readonly ImDrawData* NativePtr { get; }

		// Token: 0x0600005B RID: 91 RVA: 0x000022DB File Offset: 0x000004DB
		public unsafe ImDrawDataPtr(ImDrawData* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000022E4 File Offset: 0x000004E4
		public unsafe ImDrawDataPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000022F2 File Offset: 0x000004F2
		public unsafe static implicit operator ImDrawDataPtr(ImDrawData* nativePtr)
		{
			return new ImDrawDataPtr(nativePtr);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000022FA File Offset: 0x000004FA
		public unsafe static implicit operator ImDrawData*(ImDrawDataPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002303 File Offset: 0x00000503
		public static implicit operator ImDrawDataPtr(IntPtr nativePtr)
		{
			return new ImDrawDataPtr(nativePtr);
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000060 RID: 96 RVA: 0x0000230B File Offset: 0x0000050B
		public unsafe ref bool Valid
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->Valid));
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000061 RID: 97 RVA: 0x0000231E File Offset: 0x0000051E
		public unsafe ref int CmdListsCount
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->CmdListsCount));
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002331 File Offset: 0x00000531
		public unsafe ref int TotalIdxCount
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->TotalIdxCount));
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002344 File Offset: 0x00000544
		public unsafe ref int TotalVtxCount
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->TotalVtxCount));
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002357 File Offset: 0x00000557
		public unsafe ImVector<ImDrawListPtr> CmdLists
		{
			get
			{
				return new ImVector<ImDrawListPtr>(this.NativePtr->CmdLists);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00002369 File Offset: 0x00000569
		public unsafe ref Vector2 DisplayPos
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->DisplayPos));
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000066 RID: 102 RVA: 0x0000237C File Offset: 0x0000057C
		public unsafe ref Vector2 DisplaySize
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->DisplaySize));
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000067 RID: 103 RVA: 0x0000238F File Offset: 0x0000058F
		public unsafe ref Vector2 FramebufferScale
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->FramebufferScale));
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000068 RID: 104 RVA: 0x000023A2 File Offset: 0x000005A2
		public unsafe ImGuiViewportPtr OwnerViewport
		{
			get
			{
				return new ImGuiViewportPtr(this.NativePtr->OwnerViewport);
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000023B4 File Offset: 0x000005B4
		public unsafe void AddDrawList(ImDrawListPtr draw_list)
		{
			ImDrawList* nativePtr = draw_list.NativePtr;
			ImGuiNative.ImDrawData_AddDrawList(this.NativePtr, nativePtr);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000023D5 File Offset: 0x000005D5
		public void Clear()
		{
			ImGuiNative.ImDrawData_Clear(this.NativePtr);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000023E2 File Offset: 0x000005E2
		public void DeIndexAllBuffers()
		{
			ImGuiNative.ImDrawData_DeIndexAllBuffers(this.NativePtr);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000023EF File Offset: 0x000005EF
		public void Destroy()
		{
			ImGuiNative.ImDrawData_destroy(this.NativePtr);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000023FC File Offset: 0x000005FC
		public void ScaleClipRects(Vector2 fb_scale)
		{
			ImGuiNative.ImDrawData_ScaleClipRects(this.NativePtr, fb_scale);
		}
	}
}
