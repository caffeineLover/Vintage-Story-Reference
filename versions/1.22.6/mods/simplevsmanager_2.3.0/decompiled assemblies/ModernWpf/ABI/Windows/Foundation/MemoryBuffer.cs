using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000EA RID: 234
	[EditorBrowsable(1)]
	internal struct MemoryBuffer
	{
		// Token: 0x0600086B RID: 2155 RVA: 0x000181CC File Offset: 0x000163CC
		public static IObjectReference CreateMarshaler(Windows.Foundation.MemoryBuffer obj)
		{
			if (obj != null)
			{
				return MarshalInterface<Windows.Foundation.IMemoryBuffer>.CreateMarshaler(obj);
			}
			return null;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x000181D9 File Offset: 0x000163D9
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.MemoryBuffer obj)
		{
			return MarshalInterface<Windows.Foundation.IMemoryBuffer>.CreateMarshaler2(obj, GuidGenerator.GetIID(typeof(Windows.Foundation.IMemoryBuffer).GetHelperType()));
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x000181F5 File Offset: 0x000163F5
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<object>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00018206 File Offset: 0x00016406
		public static Windows.Foundation.MemoryBuffer FromAbi(IntPtr thisPtr)
		{
			return Windows.Foundation.MemoryBuffer.FromAbi(thisPtr);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00018210 File Offset: 0x00016410
		public static IntPtr FromManaged(Windows.Foundation.MemoryBuffer obj)
		{
			if (obj != null)
			{
				return MemoryBuffer.CreateMarshaler2(obj).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00018234 File Offset: 0x00016434
		public static MarshalInterfaceHelper<Windows.Foundation.MemoryBuffer>.MarshalerArray CreateMarshalerArray(Windows.Foundation.MemoryBuffer[] array)
		{
			return MarshalInterfaceHelper<Windows.Foundation.MemoryBuffer>.CreateMarshalerArray2(array, (Windows.Foundation.MemoryBuffer o) => MemoryBuffer.CreateMarshaler2(o));
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0001825B File Offset: 0x0001645B
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.Foundation.MemoryBuffer>.GetAbiArray(box);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00018263 File Offset: 0x00016463
		public static Windows.Foundation.MemoryBuffer[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.Foundation.MemoryBuffer>.FromAbiArray(box, new Func<IntPtr, Windows.Foundation.MemoryBuffer>(MemoryBuffer.FromAbi));
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00018277 File Offset: 0x00016477
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(Windows.Foundation.MemoryBuffer[] array)
		{
			return MarshalInterfaceHelper<Windows.Foundation.MemoryBuffer>.FromManagedArray(array, (Windows.Foundation.MemoryBuffer o) => MemoryBuffer.FromManaged(o));
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0001829E File Offset: 0x0001649E
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInspectable<object>.DisposeMarshaler(value);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x000182A6 File Offset: 0x000164A6
		public static void DisposeMarshalerArray(MarshalInterfaceHelper<Windows.Foundation.MemoryBuffer>.MarshalerArray array)
		{
			MarshalInterfaceHelper<Windows.Foundation.MemoryBuffer>.DisposeMarshalerArray(array);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x000182B3 File Offset: 0x000164B3
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x000182BB File Offset: 0x000164BB
		public static void DisposeAbiArray(object box)
		{
			MarshalInspectable<object>.DisposeAbiArray(box);
		}
	}
}
