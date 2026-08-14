using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI;
using WinRT;
using WinRT.Interop;

namespace ABI.Windows.UI
{
	// Token: 0x02000095 RID: 149
	[EditorBrowsable(1)]
	internal struct Colors
	{
		// Token: 0x0600050D RID: 1293 RVA: 0x0000751A File Offset: 0x0000571A
		public static IObjectReference CreateMarshaler(Windows.UI.Colors obj)
		{
			if (obj != null)
			{
				return MarshalInspectable<Windows.UI.Colors>.CreateMarshaler<IUnknownVftbl>(obj, GuidGenerator.GetIID(typeof(Windows.UI.IColors).GetHelperType()), true);
			}
			return null;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0000753C File Offset: 0x0000573C
		public static ObjectReferenceValue CreateMarshaler2(Windows.UI.Colors obj)
		{
			return MarshalInspectable<object>.CreateMarshaler2(obj, GuidGenerator.GetIID(typeof(Windows.UI.IColors).GetHelperType()), true);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00007559 File Offset: 0x00005759
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<object>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0000756A File Offset: 0x0000576A
		public static Windows.UI.Colors FromAbi(IntPtr thisPtr)
		{
			return Windows.UI.Colors.FromAbi(thisPtr);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00007574 File Offset: 0x00005774
		public static IntPtr FromManaged(Windows.UI.Colors obj)
		{
			if (obj != null)
			{
				return Colors.CreateMarshaler2(obj).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00007598 File Offset: 0x00005798
		public static MarshalInterfaceHelper<Windows.UI.Colors>.MarshalerArray CreateMarshalerArray(Windows.UI.Colors[] array)
		{
			return MarshalInterfaceHelper<Windows.UI.Colors>.CreateMarshalerArray2(array, (Windows.UI.Colors o) => Colors.CreateMarshaler2(o));
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x000075BF File Offset: 0x000057BF
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.UI.Colors>.GetAbiArray(box);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x000075C7 File Offset: 0x000057C7
		public static Windows.UI.Colors[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.UI.Colors>.FromAbiArray(box, new Func<IntPtr, Windows.UI.Colors>(Colors.FromAbi));
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x000075DB File Offset: 0x000057DB
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(Windows.UI.Colors[] array)
		{
			return MarshalInterfaceHelper<Windows.UI.Colors>.FromManagedArray(array, (Windows.UI.Colors o) => Colors.FromManaged(o));
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00007602 File Offset: 0x00005802
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInspectable<object>.DisposeMarshaler(value);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0000760A File Offset: 0x0000580A
		public static void DisposeMarshalerArray(MarshalInterfaceHelper<Windows.UI.Colors>.MarshalerArray array)
		{
			MarshalInterfaceHelper<Windows.UI.Colors>.DisposeMarshalerArray(array);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00007617 File Offset: 0x00005817
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0000761F File Offset: 0x0000581F
		public static void DisposeAbiArray(object box)
		{
			MarshalInspectable<object>.DisposeAbiArray(box);
		}
	}
}
