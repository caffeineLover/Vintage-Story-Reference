using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using WinRT;
using WinRT.Interop;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000BB RID: 187
	[EditorBrowsable(1)]
	internal struct Deferral
	{
		// Token: 0x0600074B RID: 1867 RVA: 0x00012484 File Offset: 0x00010684
		public static IObjectReference CreateMarshaler(Windows.Foundation.Deferral obj)
		{
			if (obj != null)
			{
				return MarshalInspectable<Windows.Foundation.Deferral>.CreateMarshaler<IUnknownVftbl>(obj, GuidGenerator.GetIID(typeof(Windows.Foundation.IDeferral).GetHelperType()), true);
			}
			return null;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x000124A6 File Offset: 0x000106A6
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.Deferral obj)
		{
			return MarshalInspectable<object>.CreateMarshaler2(obj, GuidGenerator.GetIID(typeof(Windows.Foundation.IDeferral).GetHelperType()), true);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x000124C3 File Offset: 0x000106C3
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<object>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x000124D4 File Offset: 0x000106D4
		public static Windows.Foundation.Deferral FromAbi(IntPtr thisPtr)
		{
			return Windows.Foundation.Deferral.FromAbi(thisPtr);
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x000124DC File Offset: 0x000106DC
		public static IntPtr FromManaged(Windows.Foundation.Deferral obj)
		{
			if (obj != null)
			{
				return Deferral.CreateMarshaler2(obj).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00012500 File Offset: 0x00010700
		public static MarshalInterfaceHelper<Windows.Foundation.Deferral>.MarshalerArray CreateMarshalerArray(Windows.Foundation.Deferral[] array)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Deferral>.CreateMarshalerArray2(array, (Windows.Foundation.Deferral o) => Deferral.CreateMarshaler2(o));
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00012527 File Offset: 0x00010727
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Deferral>.GetAbiArray(box);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001252F File Offset: 0x0001072F
		public static Windows.Foundation.Deferral[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Deferral>.FromAbiArray(box, new Func<IntPtr, Windows.Foundation.Deferral>(Deferral.FromAbi));
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00012543 File Offset: 0x00010743
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(Windows.Foundation.Deferral[] array)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Deferral>.FromManagedArray(array, (Windows.Foundation.Deferral o) => Deferral.FromManaged(o));
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0001256A File Offset: 0x0001076A
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInspectable<object>.DisposeMarshaler(value);
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00012572 File Offset: 0x00010772
		public static void DisposeMarshalerArray(MarshalInterfaceHelper<Windows.Foundation.Deferral>.MarshalerArray array)
		{
			MarshalInterfaceHelper<Windows.Foundation.Deferral>.DisposeMarshalerArray(array);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0001257F File Offset: 0x0001077F
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00012587 File Offset: 0x00010787
		public static void DisposeAbiArray(object box)
		{
			MarshalInspectable<object>.DisposeAbiArray(box);
		}
	}
}
