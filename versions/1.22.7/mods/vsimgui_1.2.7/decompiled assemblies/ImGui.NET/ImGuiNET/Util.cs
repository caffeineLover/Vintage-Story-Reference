using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ImGuiNET
{
	// Token: 0x0200008A RID: 138
	internal static class Util
	{
		// Token: 0x0600092E RID: 2350 RVA: 0x000166F0 File Offset: 0x000148F0
		public unsafe static string StringFromPtr(byte* ptr)
		{
			int num = 0;
			while (ptr[num] != 0)
			{
				num++;
			}
			return Encoding.UTF8.GetString(ptr, num);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00016718 File Offset: 0x00014918
		internal unsafe static bool AreStringsEqual(byte* a, int aLength, byte* b)
		{
			for (int i = 0; i < aLength; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return b[aLength] == 0;
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x00016746 File Offset: 0x00014946
		internal unsafe static byte* Allocate(int byteCount)
		{
			return (byte*)((void*)Marshal.AllocHGlobal(byteCount));
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00016753 File Offset: 0x00014953
		internal unsafe static void Free(byte* ptr)
		{
			Marshal.FreeHGlobal((IntPtr)((void*)ptr));
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00016760 File Offset: 0x00014960
		internal unsafe static int CalcSizeInUtf8(ReadOnlySpan<char> s, int start, int length)
		{
			if (start < 0 || length < 0 || start + length > s.Length)
			{
				throw new ArgumentOutOfRangeException();
			}
			fixed (char* pinnableReference = s.GetPinnableReference())
			{
				char* ptr = pinnableReference;
				return Encoding.UTF8.GetByteCount(ptr + start, length);
			}
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x000167A4 File Offset: 0x000149A4
		internal unsafe static int GetUtf8(ReadOnlySpan<char> s, byte* utf8Bytes, int utf8ByteCount)
		{
			if (s.IsEmpty)
			{
				return 0;
			}
			fixed (char* pinnableReference = s.GetPinnableReference())
			{
				char* ptr = pinnableReference;
				return Encoding.UTF8.GetBytes(ptr, s.Length, utf8Bytes, utf8ByteCount);
			}
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x000167DC File Offset: 0x000149DC
		internal unsafe static int GetUtf8(string s, byte* utf8Bytes, int utf8ByteCount)
		{
			char* ptr;
			if (s == null)
			{
				ptr = null;
			}
			else
			{
				fixed (char* pinnableReference = s.GetPinnableReference())
				{
					ptr = pinnableReference;
				}
			}
			char* ptr2 = ptr;
			return Encoding.UTF8.GetBytes(ptr2, s.Length, utf8Bytes, utf8ByteCount);
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00016810 File Offset: 0x00014A10
		internal unsafe static int GetUtf8(ReadOnlySpan<char> s, int start, int length, byte* utf8Bytes, int utf8ByteCount)
		{
			if (start < 0 || length < 0 || start + length > s.Length)
			{
				throw new ArgumentOutOfRangeException();
			}
			fixed (char* pinnableReference = s.GetPinnableReference())
			{
				char* ptr = pinnableReference;
				return Encoding.UTF8.GetBytes(ptr + start, length, utf8Bytes, utf8ByteCount);
			}
		}

		// Token: 0x040007EA RID: 2026
		internal const int StackAllocationSizeLimit = 2048;
	}
}
