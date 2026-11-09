using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.ViewManagement;
using WinRT;
using WinRT.Interop;

namespace ABI.Windows.UI.ViewManagement
{
	// Token: 0x020000A7 RID: 167
	[EditorBrowsable(1)]
	internal struct UISettings
	{
		// Token: 0x0600067B RID: 1659 RVA: 0x0000EAAA File Offset: 0x0000CCAA
		public static IObjectReference CreateMarshaler(Windows.UI.ViewManagement.UISettings obj)
		{
			if (obj != null)
			{
				return MarshalInspectable<Windows.UI.ViewManagement.UISettings>.CreateMarshaler<IUnknownVftbl>(obj, GuidGenerator.GetIID(typeof(Windows.UI.ViewManagement.IUISettings).GetHelperType()), true);
			}
			return null;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0000EACC File Offset: 0x0000CCCC
		public static ObjectReferenceValue CreateMarshaler2(Windows.UI.ViewManagement.UISettings obj)
		{
			return MarshalInspectable<object>.CreateMarshaler2(obj, GuidGenerator.GetIID(typeof(Windows.UI.ViewManagement.IUISettings).GetHelperType()), true);
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0000EAE9 File Offset: 0x0000CCE9
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<object>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0000EAFA File Offset: 0x0000CCFA
		public static Windows.UI.ViewManagement.UISettings FromAbi(IntPtr thisPtr)
		{
			return Windows.UI.ViewManagement.UISettings.FromAbi(thisPtr);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0000EB04 File Offset: 0x0000CD04
		public static IntPtr FromManaged(Windows.UI.ViewManagement.UISettings obj)
		{
			if (obj != null)
			{
				return UISettings.CreateMarshaler2(obj).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0000EB28 File Offset: 0x0000CD28
		public static MarshalInterfaceHelper<Windows.UI.ViewManagement.UISettings>.MarshalerArray CreateMarshalerArray(Windows.UI.ViewManagement.UISettings[] array)
		{
			return MarshalInterfaceHelper<Windows.UI.ViewManagement.UISettings>.CreateMarshalerArray2(array, (Windows.UI.ViewManagement.UISettings o) => UISettings.CreateMarshaler2(o));
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0000EB4F File Offset: 0x0000CD4F
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.UI.ViewManagement.UISettings>.GetAbiArray(box);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0000EB57 File Offset: 0x0000CD57
		public static Windows.UI.ViewManagement.UISettings[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.UI.ViewManagement.UISettings>.FromAbiArray(box, new Func<IntPtr, Windows.UI.ViewManagement.UISettings>(UISettings.FromAbi));
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0000EB6B File Offset: 0x0000CD6B
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(Windows.UI.ViewManagement.UISettings[] array)
		{
			return MarshalInterfaceHelper<Windows.UI.ViewManagement.UISettings>.FromManagedArray(array, (Windows.UI.ViewManagement.UISettings o) => UISettings.FromManaged(o));
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0000EB92 File Offset: 0x0000CD92
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInspectable<object>.DisposeMarshaler(value);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0000EB9A File Offset: 0x0000CD9A
		public static void DisposeMarshalerArray(MarshalInterfaceHelper<Windows.UI.ViewManagement.UISettings>.MarshalerArray array)
		{
			MarshalInterfaceHelper<Windows.UI.ViewManagement.UISettings>.DisposeMarshalerArray(array);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0000EBA7 File Offset: 0x0000CDA7
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0000EBAF File Offset: 0x0000CDAF
		public static void DisposeAbiArray(object box)
		{
			MarshalInspectable<object>.DisposeAbiArray(box);
		}
	}
}
