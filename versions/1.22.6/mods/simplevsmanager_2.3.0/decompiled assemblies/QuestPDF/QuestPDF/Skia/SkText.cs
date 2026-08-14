using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace QuestPDF.Skia
{
	// Token: 0x02000021 RID: 33
	internal sealed class SkText : IDisposable
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00003544 File Offset: 0x00001744
		// (set) Token: 0x060000AD RID: 173 RVA: 0x0000354C File Offset: 0x0000174C
		public IntPtr Instance { get; private set; }

		// Token: 0x060000AE RID: 174 RVA: 0x00003555 File Offset: 0x00001755
		[NullableContext(2)]
		public SkText(string text)
		{
			this.Instance = SkText.MarshalFromManagedToNative(text);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000356C File Offset: 0x0000176C
		~SkText()
		{
			this.WarnThatFinalizerIsReached<SkText>();
			this.Dispose();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000035A0 File Offset: 0x000017A0
		[NullableContext(2)]
		public unsafe static IntPtr MarshalFromManagedToNative(string text)
		{
			if (text == null)
			{
				return IntPtr.Zero;
			}
			int byteCount = Encoding.UTF8.GetByteCount(text);
			IntPtr intPtr = Marshal.AllocHGlobal(byteCount + 1);
			char* ptr;
			if (text == null)
			{
				ptr = null;
			}
			else
			{
				fixed (char* ptr2 = text.GetPinnableReference())
				{
					ptr = ptr2;
				}
			}
			char* ptr3 = ptr;
			byte* ptr4 = intPtr;
			Encoding.UTF8.GetBytes(ptr3, text.Length, ptr4, byteCount);
			char* ptr2 = null;
			Marshal.WriteByte(intPtr, byteCount, 0);
			return intPtr;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000035FC File Offset: 0x000017FC
		[NullableContext(1)]
		public static implicit operator IntPtr(SkText text)
		{
			return text.Instance;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003604 File Offset: 0x00001804
		public void Dispose()
		{
			if (this.Instance != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.Instance);
				this.Instance = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}
}
