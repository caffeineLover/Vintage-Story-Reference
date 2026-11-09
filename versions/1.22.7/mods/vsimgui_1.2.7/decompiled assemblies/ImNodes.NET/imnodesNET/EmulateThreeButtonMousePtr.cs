using System;

namespace imnodesNET
{
	// Token: 0x02000003 RID: 3
	public struct EmulateThreeButtonMousePtr
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public unsafe readonly EmulateThreeButtonMouse* NativePtr { get; }

		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public unsafe EmulateThreeButtonMousePtr(EmulateThreeButtonMouse* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		public unsafe EmulateThreeButtonMousePtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000206F File Offset: 0x0000026F
		public unsafe static implicit operator EmulateThreeButtonMousePtr(EmulateThreeButtonMouse* nativePtr)
		{
			return new EmulateThreeButtonMousePtr(nativePtr);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002077 File Offset: 0x00000277
		public unsafe static implicit operator EmulateThreeButtonMouse*(EmulateThreeButtonMousePtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002080 File Offset: 0x00000280
		public static implicit operator EmulateThreeButtonMousePtr(IntPtr nativePtr)
		{
			return new EmulateThreeButtonMousePtr(nativePtr);
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002088 File Offset: 0x00000288
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000209A File Offset: 0x0000029A
		public unsafe IntPtr Modifier
		{
			get
			{
				return (IntPtr)((void*)this.NativePtr->Modifier);
			}
			set
			{
				this.NativePtr->Modifier = (byte*)((void*)value);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000020AD File Offset: 0x000002AD
		public void Destroy()
		{
			imnodesNative.EmulateThreeButtonMouse_destroy(this.NativePtr);
		}
	}
}
