using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia
{
	// Token: 0x02000024 RID: 36
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Utf8StringMarshaller : ICustomMarshaler
	{
		// Token: 0x060000BB RID: 187 RVA: 0x00003730 File Offset: 0x00001930
		public static ICustomMarshaler GetInstance([Nullable(2)] string cookie)
		{
			return Utf8StringMarshaller.Instance;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003737 File Offset: 0x00001937
		public void CleanUpManagedData(object managedObj)
		{
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00003739 File Offset: 0x00001939
		public void CleanUpNativeData(IntPtr pNativeData)
		{
			Marshal.FreeHGlobal(pNativeData);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003741 File Offset: 0x00001941
		public int GetNativeDataSize()
		{
			return -1;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003744 File Offset: 0x00001944
		public IntPtr MarshalManagedToNative(object managedObject)
		{
			return SkText.MarshalFromManagedToNative(managedObject as string);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003751 File Offset: 0x00001951
		public object MarshalNativeToManaged(IntPtr pNativeData)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000078 RID: 120
		private static readonly Utf8StringMarshaller Instance = new Utf8StringMarshaller();
	}
}
