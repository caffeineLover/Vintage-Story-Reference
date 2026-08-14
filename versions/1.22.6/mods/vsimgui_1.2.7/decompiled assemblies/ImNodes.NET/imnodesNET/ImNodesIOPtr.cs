using System;
using System.Runtime.CompilerServices;

namespace imnodesNET
{
	// Token: 0x02000008 RID: 8
	public struct ImNodesIOPtr
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000064 RID: 100 RVA: 0x000028BC File Offset: 0x00000ABC
		public unsafe readonly ImNodesIO* NativePtr { get; }

		// Token: 0x06000065 RID: 101 RVA: 0x000028C4 File Offset: 0x00000AC4
		public unsafe ImNodesIOPtr(ImNodesIO* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000028CD File Offset: 0x00000ACD
		public unsafe ImNodesIOPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000028DB File Offset: 0x00000ADB
		public unsafe static implicit operator ImNodesIOPtr(ImNodesIO* nativePtr)
		{
			return new ImNodesIOPtr(nativePtr);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000028E3 File Offset: 0x00000AE3
		public unsafe static implicit operator ImNodesIO*(ImNodesIOPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000028EC File Offset: 0x00000AEC
		public static implicit operator ImNodesIOPtr(IntPtr nativePtr)
		{
			return new ImNodesIOPtr(nativePtr);
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000028F4 File Offset: 0x00000AF4
		public unsafe ref EmulateThreeButtonMouse EmulateThreeButtonMouse
		{
			get
			{
				return Unsafe.AsRef<EmulateThreeButtonMouse>((void*)(&this.NativePtr->EmulateThreeButtonMouse));
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002907 File Offset: 0x00000B07
		public unsafe ref LinkDetachWithModifierClick LinkDetachWithModifierClick
		{
			get
			{
				return Unsafe.AsRef<LinkDetachWithModifierClick>((void*)(&this.NativePtr->LinkDetachWithModifierClick));
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600006C RID: 108 RVA: 0x0000291A File Offset: 0x00000B1A
		public unsafe ref MultipleSelectModifier MultipleSelectModifier
		{
			get
			{
				return Unsafe.AsRef<MultipleSelectModifier>((void*)(&this.NativePtr->MultipleSelectModifier));
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600006D RID: 109 RVA: 0x0000292D File Offset: 0x00000B2D
		public unsafe ref int AltMouseButton
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->AltMouseButton));
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00002940 File Offset: 0x00000B40
		public unsafe ref float AutoPanningSpeed
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->AutoPanningSpeed));
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002953 File Offset: 0x00000B53
		public void Destroy()
		{
			imnodesNative.ImNodesIO_destroy(this.NativePtr);
		}
	}
}
