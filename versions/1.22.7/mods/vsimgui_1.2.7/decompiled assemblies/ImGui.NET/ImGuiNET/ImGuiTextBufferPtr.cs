using System;
using System.Text;

namespace ImGuiNET
{
	// Token: 0x0200006A RID: 106
	public struct ImGuiTextBufferPtr
	{
		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x0001599F File Offset: 0x00013B9F
		public unsafe readonly ImGuiTextBuffer* NativePtr { get; }

		// Token: 0x0600087B RID: 2171 RVA: 0x000159A7 File Offset: 0x00013BA7
		public unsafe ImGuiTextBufferPtr(ImGuiTextBuffer* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x000159B0 File Offset: 0x00013BB0
		public unsafe ImGuiTextBufferPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x000159BE File Offset: 0x00013BBE
		public unsafe static implicit operator ImGuiTextBufferPtr(ImGuiTextBuffer* nativePtr)
		{
			return new ImGuiTextBufferPtr(nativePtr);
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x000159C6 File Offset: 0x00013BC6
		public unsafe static implicit operator ImGuiTextBuffer*(ImGuiTextBufferPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x000159CF File Offset: 0x00013BCF
		public static implicit operator ImGuiTextBufferPtr(IntPtr nativePtr)
		{
			return new ImGuiTextBufferPtr(nativePtr);
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x000159D7 File Offset: 0x00013BD7
		public unsafe ImVector<byte> Buf
		{
			get
			{
				return new ImVector<byte>(this.NativePtr->Buf);
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x000159EC File Offset: 0x00013BEC
		public unsafe void append(ReadOnlySpan<char> str)
		{
			int num = 0;
			byte* ptr;
			if (str != null)
			{
				num = Encoding.UTF8.GetByteCount(str);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte* str_end = null;
			ImGuiNative.ImGuiTextBuffer_append(this.NativePtr, ptr, str_end);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00015A60 File Offset: 0x00013C60
		public unsafe void appendf(ReadOnlySpan<char> fmt)
		{
			int num = 0;
			byte* ptr;
			if (fmt != null)
			{
				num = Encoding.UTF8.GetByteCount(fmt);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(fmt, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.ImGuiTextBuffer_appendf(this.NativePtr, ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00015AD0 File Offset: 0x00013CD0
		public string begin()
		{
			return Util.StringFromPtr(ImGuiNative.ImGuiTextBuffer_begin(this.NativePtr));
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00015AE2 File Offset: 0x00013CE2
		public string c_str()
		{
			return Util.StringFromPtr(ImGuiNative.ImGuiTextBuffer_c_str(this.NativePtr));
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00015AF4 File Offset: 0x00013CF4
		public void clear()
		{
			ImGuiNative.ImGuiTextBuffer_clear(this.NativePtr);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00015B01 File Offset: 0x00013D01
		public void Destroy()
		{
			ImGuiNative.ImGuiTextBuffer_destroy(this.NativePtr);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00015B0E File Offset: 0x00013D0E
		public bool empty()
		{
			return ImGuiNative.ImGuiTextBuffer_empty(this.NativePtr) > 0;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00015B1E File Offset: 0x00013D1E
		public string end()
		{
			return Util.StringFromPtr(ImGuiNative.ImGuiTextBuffer_end(this.NativePtr));
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00015B30 File Offset: 0x00013D30
		public void reserve(int capacity)
		{
			ImGuiNative.ImGuiTextBuffer_reserve(this.NativePtr, capacity);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00015B3E File Offset: 0x00013D3E
		public int size()
		{
			return ImGuiNative.ImGuiTextBuffer_size(this.NativePtr);
		}
	}
}
