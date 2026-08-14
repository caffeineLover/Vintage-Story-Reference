using System;

namespace imnodesNET
{
	// Token: 0x02000013 RID: 19
	public struct MultipleSelectModifierPtr
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00002B59 File Offset: 0x00000D59
		public unsafe readonly MultipleSelectModifier* NativePtr { get; }

		// Token: 0x060000E8 RID: 232 RVA: 0x00002B61 File Offset: 0x00000D61
		public unsafe MultipleSelectModifierPtr(MultipleSelectModifier* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00002B6A File Offset: 0x00000D6A
		public unsafe MultipleSelectModifierPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00002B78 File Offset: 0x00000D78
		public unsafe static implicit operator MultipleSelectModifierPtr(MultipleSelectModifier* nativePtr)
		{
			return new MultipleSelectModifierPtr(nativePtr);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00002B80 File Offset: 0x00000D80
		public unsafe static implicit operator MultipleSelectModifier*(MultipleSelectModifierPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00002B89 File Offset: 0x00000D89
		public static implicit operator MultipleSelectModifierPtr(IntPtr nativePtr)
		{
			return new MultipleSelectModifierPtr(nativePtr);
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00002B91 File Offset: 0x00000D91
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00002BA3 File Offset: 0x00000DA3
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

		// Token: 0x060000EF RID: 239 RVA: 0x00002BB6 File Offset: 0x00000DB6
		public void Destroy()
		{
			imnodesNative.MultipleSelectModifier_destroy(this.NativePtr);
		}
	}
}
