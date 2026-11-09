using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.ViewManagement;
using WinRT;
using WinRT.Interop;

namespace ABI.Windows.UI.ViewManagement
{
	// Token: 0x020000A8 RID: 168
	[EditorBrowsable(1)]
	internal struct UISettingsAutoHideScrollBarsChangedEventArgs
	{
		// Token: 0x06000688 RID: 1672 RVA: 0x0000EBB7 File Offset: 0x0000CDB7
		public static IObjectReference CreateMarshaler(Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs obj)
		{
			if (obj != null)
			{
				return MarshalInspectable<Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>.CreateMarshaler<IUnknownVftbl>(obj, GuidGenerator.GetIID(typeof(Windows.UI.ViewManagement.IUISettingsAutoHideScrollBarsChangedEventArgs).GetHelperType()), true);
			}
			return null;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0000EBD9 File Offset: 0x0000CDD9
		public static ObjectReferenceValue CreateMarshaler2(Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs obj)
		{
			return MarshalInspectable<object>.CreateMarshaler2(obj, GuidGenerator.GetIID(typeof(Windows.UI.ViewManagement.IUISettingsAutoHideScrollBarsChangedEventArgs).GetHelperType()), true);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0000EBF6 File Offset: 0x0000CDF6
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<object>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0000EC07 File Offset: 0x0000CE07
		public static Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs FromAbi(IntPtr thisPtr)
		{
			return Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs.FromAbi(thisPtr);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0000EC10 File Offset: 0x0000CE10
		public static IntPtr FromManaged(Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs obj)
		{
			if (obj != null)
			{
				return UISettingsAutoHideScrollBarsChangedEventArgs.CreateMarshaler2(obj).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0000EC34 File Offset: 0x0000CE34
		public static MarshalInterfaceHelper<Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>.MarshalerArray CreateMarshalerArray(Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs[] array)
		{
			return MarshalInterfaceHelper<Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>.CreateMarshalerArray2(array, (Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs o) => UISettingsAutoHideScrollBarsChangedEventArgs.CreateMarshaler2(o));
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0000EC5B File Offset: 0x0000CE5B
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>.GetAbiArray(box);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0000EC63 File Offset: 0x0000CE63
		public static Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>.FromAbiArray(box, new Func<IntPtr, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>(UISettingsAutoHideScrollBarsChangedEventArgs.FromAbi));
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0000EC77 File Offset: 0x0000CE77
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs[] array)
		{
			return MarshalInterfaceHelper<Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>.FromManagedArray(array, (Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs o) => UISettingsAutoHideScrollBarsChangedEventArgs.FromManaged(o));
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0000EC9E File Offset: 0x0000CE9E
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInspectable<object>.DisposeMarshaler(value);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0000ECA6 File Offset: 0x0000CEA6
		public static void DisposeMarshalerArray(MarshalInterfaceHelper<Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>.MarshalerArray array)
		{
			MarshalInterfaceHelper<Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>.DisposeMarshalerArray(array);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0000ECB3 File Offset: 0x0000CEB3
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0000ECBB File Offset: 0x0000CEBB
		public static void DisposeAbiArray(object box)
		{
			MarshalInspectable<object>.DisposeAbiArray(box);
		}
	}
}
