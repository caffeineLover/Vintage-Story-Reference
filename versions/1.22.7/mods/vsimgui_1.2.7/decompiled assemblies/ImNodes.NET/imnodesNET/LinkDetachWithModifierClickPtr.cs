using System;

namespace imnodesNET
{
	// Token: 0x02000011 RID: 17
	public struct LinkDetachWithModifierClickPtr
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00002AEF File Offset: 0x00000CEF
		public unsafe readonly LinkDetachWithModifierClick* NativePtr { get; }

		// Token: 0x060000DF RID: 223 RVA: 0x00002AF7 File Offset: 0x00000CF7
		public unsafe LinkDetachWithModifierClickPtr(LinkDetachWithModifierClick* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00002B00 File Offset: 0x00000D00
		public unsafe LinkDetachWithModifierClickPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00002B0E File Offset: 0x00000D0E
		public unsafe static implicit operator LinkDetachWithModifierClickPtr(LinkDetachWithModifierClick* nativePtr)
		{
			return new LinkDetachWithModifierClickPtr(nativePtr);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00002B16 File Offset: 0x00000D16
		public unsafe static implicit operator LinkDetachWithModifierClick*(LinkDetachWithModifierClickPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00002B1F File Offset: 0x00000D1F
		public static implicit operator LinkDetachWithModifierClickPtr(IntPtr nativePtr)
		{
			return new LinkDetachWithModifierClickPtr(nativePtr);
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00002B27 File Offset: 0x00000D27
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00002B39 File Offset: 0x00000D39
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

		// Token: 0x060000E6 RID: 230 RVA: 0x00002B4C File Offset: 0x00000D4C
		public void Destroy()
		{
			imnodesNative.LinkDetachWithModifierClick_destroy(this.NativePtr);
		}
	}
}
