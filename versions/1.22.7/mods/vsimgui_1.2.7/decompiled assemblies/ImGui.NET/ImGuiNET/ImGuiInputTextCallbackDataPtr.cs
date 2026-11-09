using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace ImGuiNET
{
	// Token: 0x0200003B RID: 59
	public struct ImGuiInputTextCallbackDataPtr
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x00013DF0 File Offset: 0x00011FF0
		public unsafe readonly ImGuiInputTextCallbackData* NativePtr { get; }

		// Token: 0x06000471 RID: 1137 RVA: 0x00013DF8 File Offset: 0x00011FF8
		public unsafe ImGuiInputTextCallbackDataPtr(ImGuiInputTextCallbackData* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00013E01 File Offset: 0x00012001
		public unsafe ImGuiInputTextCallbackDataPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00013E0F File Offset: 0x0001200F
		public unsafe static implicit operator ImGuiInputTextCallbackDataPtr(ImGuiInputTextCallbackData* nativePtr)
		{
			return new ImGuiInputTextCallbackDataPtr(nativePtr);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00013E17 File Offset: 0x00012017
		public unsafe static implicit operator ImGuiInputTextCallbackData*(ImGuiInputTextCallbackDataPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00013E20 File Offset: 0x00012020
		public static implicit operator ImGuiInputTextCallbackDataPtr(IntPtr nativePtr)
		{
			return new ImGuiInputTextCallbackDataPtr(nativePtr);
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x00013E28 File Offset: 0x00012028
		public unsafe ref IntPtr Ctx
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Ctx));
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x00013E3B File Offset: 0x0001203B
		public unsafe ref ImGuiInputTextFlags EventFlag
		{
			get
			{
				return Unsafe.AsRef<ImGuiInputTextFlags>((void*)(&this.NativePtr->EventFlag));
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x00013E4E File Offset: 0x0001204E
		public unsafe ref ImGuiInputTextFlags Flags
		{
			get
			{
				return Unsafe.AsRef<ImGuiInputTextFlags>((void*)(&this.NativePtr->Flags));
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x00013E61 File Offset: 0x00012061
		// (set) Token: 0x0600047A RID: 1146 RVA: 0x00013E73 File Offset: 0x00012073
		public unsafe IntPtr UserData
		{
			get
			{
				return (IntPtr)this.NativePtr->UserData;
			}
			set
			{
				this.NativePtr->UserData = (void*)value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x00013E86 File Offset: 0x00012086
		public unsafe ref ushort EventChar
		{
			get
			{
				return Unsafe.AsRef<ushort>((void*)(&this.NativePtr->EventChar));
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x00013E99 File Offset: 0x00012099
		public unsafe ref ImGuiKey EventKey
		{
			get
			{
				return Unsafe.AsRef<ImGuiKey>((void*)(&this.NativePtr->EventKey));
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x00013EAC File Offset: 0x000120AC
		// (set) Token: 0x0600047E RID: 1150 RVA: 0x00013EBE File Offset: 0x000120BE
		public unsafe IntPtr Buf
		{
			get
			{
				return (IntPtr)((void*)this.NativePtr->Buf);
			}
			set
			{
				this.NativePtr->Buf = (byte*)((void*)value);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x00013ED1 File Offset: 0x000120D1
		public unsafe ref int BufTextLen
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->BufTextLen));
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x00013EE4 File Offset: 0x000120E4
		public unsafe ref int BufSize
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->BufSize));
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x00013EF7 File Offset: 0x000120F7
		public unsafe ref bool BufDirty
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->BufDirty));
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x00013F0A File Offset: 0x0001210A
		public unsafe ref int CursorPos
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->CursorPos));
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x00013F1D File Offset: 0x0001211D
		public unsafe ref int SelectionStart
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->SelectionStart));
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x00013F30 File Offset: 0x00012130
		public unsafe ref int SelectionEnd
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->SelectionEnd));
			}
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00013F43 File Offset: 0x00012143
		public void ClearSelection()
		{
			ImGuiNative.ImGuiInputTextCallbackData_ClearSelection(this.NativePtr);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00013F50 File Offset: 0x00012150
		public void DeleteChars(int pos, int bytes_count)
		{
			ImGuiNative.ImGuiInputTextCallbackData_DeleteChars(this.NativePtr, pos, bytes_count);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00013F5F File Offset: 0x0001215F
		public void Destroy()
		{
			ImGuiNative.ImGuiInputTextCallbackData_destroy(this.NativePtr);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00013F6C File Offset: 0x0001216C
		public bool HasSelection()
		{
			return ImGuiNative.ImGuiInputTextCallbackData_HasSelection(this.NativePtr) > 0;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00013F7C File Offset: 0x0001217C
		public unsafe void InsertChars(int pos, ReadOnlySpan<char> text)
		{
			int num = 0;
			byte* ptr;
			if (text != null)
			{
				num = Encoding.UTF8.GetByteCount(text);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(text, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte* text_end = null;
			ImGuiNative.ImGuiInputTextCallbackData_InsertChars(this.NativePtr, pos, ptr, text_end);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00013FF1 File Offset: 0x000121F1
		public void SelectAll()
		{
			ImGuiNative.ImGuiInputTextCallbackData_SelectAll(this.NativePtr);
		}
	}
}
