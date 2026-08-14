using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace ImGuiNET
{
	// Token: 0x0200006C RID: 108
	public struct ImGuiTextFilterPtr
	{
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x00015B4B File Offset: 0x00013D4B
		public unsafe readonly ImGuiTextFilter* NativePtr { get; }

		// Token: 0x0600088C RID: 2188 RVA: 0x00015B53 File Offset: 0x00013D53
		public unsafe ImGuiTextFilterPtr(ImGuiTextFilter* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00015B5C File Offset: 0x00013D5C
		public unsafe ImGuiTextFilterPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00015B6A File Offset: 0x00013D6A
		public unsafe static implicit operator ImGuiTextFilterPtr(ImGuiTextFilter* nativePtr)
		{
			return new ImGuiTextFilterPtr(nativePtr);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00015B72 File Offset: 0x00013D72
		public unsafe static implicit operator ImGuiTextFilter*(ImGuiTextFilterPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00015B7B File Offset: 0x00013D7B
		public static implicit operator ImGuiTextFilterPtr(IntPtr nativePtr)
		{
			return new ImGuiTextFilterPtr(nativePtr);
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x00015B83 File Offset: 0x00013D83
		public unsafe RangeAccessor<byte> InputBuf
		{
			get
			{
				return new RangeAccessor<byte>((void*)(&this.NativePtr->InputBuf.FixedElementField), 256);
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x00015BA0 File Offset: 0x00013DA0
		public unsafe ImPtrVector<ImGuiTextRangePtr> Filters
		{
			get
			{
				return new ImPtrVector<ImGuiTextRangePtr>(this.NativePtr->Filters, Unsafe.SizeOf<ImGuiTextRange>());
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x00015BB7 File Offset: 0x00013DB7
		public unsafe ref int CountGrep
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->CountGrep));
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00015BCA File Offset: 0x00013DCA
		public void Build()
		{
			ImGuiNative.ImGuiTextFilter_Build(this.NativePtr);
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00015BD7 File Offset: 0x00013DD7
		public void Clear()
		{
			ImGuiNative.ImGuiTextFilter_Clear(this.NativePtr);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00015BE4 File Offset: 0x00013DE4
		public void Destroy()
		{
			ImGuiNative.ImGuiTextFilter_destroy(this.NativePtr);
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00015BF4 File Offset: 0x00013DF4
		public unsafe bool Draw()
		{
			int byteCount = Encoding.UTF8.GetByteCount("Filter(inc,-exc)");
			byte* ptr;
			if (byteCount > 2048)
			{
				ptr = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf = Util.GetUtf8("Filter(inc,-exc)", ptr, byteCount);
			ptr[utf] = 0;
			float width = 0f;
			int num = (int)ImGuiNative.ImGuiTextFilter_Draw(this.NativePtr, ptr, width);
			if (byteCount > 2048)
			{
				Util.Free(ptr);
			}
			return num != 0;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00015C64 File Offset: 0x00013E64
		public unsafe bool Draw(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float width = 0f;
			int num2 = (int)ImGuiNative.ImGuiTextFilter_Draw(this.NativePtr, ptr, width);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00015CE0 File Offset: 0x00013EE0
		public unsafe bool Draw(ReadOnlySpan<char> label, float width)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.ImGuiTextFilter_Draw(this.NativePtr, ptr, width);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00015D54 File Offset: 0x00013F54
		public bool IsActive()
		{
			return ImGuiNative.ImGuiTextFilter_IsActive(this.NativePtr) > 0;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00015D64 File Offset: 0x00013F64
		public unsafe bool PassFilter(ReadOnlySpan<char> text)
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
			int num2 = (int)ImGuiNative.ImGuiTextFilter_PassFilter(this.NativePtr, ptr, text_end);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}
	}
}
