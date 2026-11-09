using System;
using System.Runtime.InteropServices;

namespace OpenTK.Audio.OpenAL.Native
{
	// Token: 0x0200004A RID: 74
	internal class ConstCharPtrMarshaler : ICustomMarshaler
	{
		// Token: 0x06000105 RID: 261 RVA: 0x000073DF File Offset: 0x000055DF
		public void CleanUpManagedData(object ManagedObj)
		{
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000073E1 File Offset: 0x000055E1
		public void CleanUpNativeData(IntPtr pNativeData)
		{
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000073E3 File Offset: 0x000055E3
		public int GetNativeDataSize()
		{
			return IntPtr.Size;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000073EC File Offset: 0x000055EC
		public IntPtr MarshalManagedToNative(object ManagedObj)
		{
			string text = ManagedObj as string;
			if (text != null)
			{
				return Marshal.StringToHGlobalAnsi(text);
			}
			throw new ArgumentException(string.Format("{0} only supports marshaling of strings. Got '{1}'", "ConstCharPtrMarshaler", ManagedObj.GetType()));
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00007424 File Offset: 0x00005624
		public object MarshalNativeToManaged(IntPtr pNativeData)
		{
			return Marshal.PtrToStringAnsi(pNativeData);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000742C File Offset: 0x0000562C
		public static ICustomMarshaler GetInstance(string cookie)
		{
			return ConstCharPtrMarshaler.Instance;
		}

		// Token: 0x04000230 RID: 560
		private static readonly ConstCharPtrMarshaler Instance = new ConstCharPtrMarshaler();
	}
}
