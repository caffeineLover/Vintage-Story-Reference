using System;

namespace ImGuiNET
{
	// Token: 0x0200006E RID: 110
	public struct ImGuiTextRangePtr
	{
		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00015DDB File Offset: 0x00013FDB
		public unsafe readonly ImGuiTextRange* NativePtr { get; }

		// Token: 0x0600089D RID: 2205 RVA: 0x00015DE3 File Offset: 0x00013FE3
		public unsafe ImGuiTextRangePtr(ImGuiTextRange* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00015DEC File Offset: 0x00013FEC
		public unsafe ImGuiTextRangePtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00015DFA File Offset: 0x00013FFA
		public unsafe static implicit operator ImGuiTextRangePtr(ImGuiTextRange* nativePtr)
		{
			return new ImGuiTextRangePtr(nativePtr);
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00015E02 File Offset: 0x00014002
		public unsafe static implicit operator ImGuiTextRange*(ImGuiTextRangePtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00015E0B File Offset: 0x0001400B
		public static implicit operator ImGuiTextRangePtr(IntPtr nativePtr)
		{
			return new ImGuiTextRangePtr(nativePtr);
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x00015E13 File Offset: 0x00014013
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x00015E25 File Offset: 0x00014025
		public unsafe IntPtr b
		{
			get
			{
				return (IntPtr)((void*)this.NativePtr->b);
			}
			set
			{
				this.NativePtr->b = (byte*)((void*)value);
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00015E38 File Offset: 0x00014038
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x00015E4A File Offset: 0x0001404A
		public unsafe IntPtr e
		{
			get
			{
				return (IntPtr)((void*)this.NativePtr->e);
			}
			set
			{
				this.NativePtr->e = (byte*)((void*)value);
			}
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00015E5D File Offset: 0x0001405D
		public void Destroy()
		{
			ImGuiNative.ImGuiTextRange_destroy(this.NativePtr);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00015E6A File Offset: 0x0001406A
		public bool empty()
		{
			return ImGuiNative.ImGuiTextRange_empty(this.NativePtr) > 0;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00015E7C File Offset: 0x0001407C
		public unsafe void split(byte separator, out ImVector @out)
		{
			fixed (ImVector* ptr = &@out)
			{
				ImVector* out2 = ptr;
				ImGuiNative.ImGuiTextRange_split(this.NativePtr, separator, out2);
			}
		}
	}
}
