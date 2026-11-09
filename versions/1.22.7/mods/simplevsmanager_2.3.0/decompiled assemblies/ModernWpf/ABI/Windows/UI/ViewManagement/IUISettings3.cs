using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using ABI.Windows.Foundation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using WinRT;

namespace ABI.Windows.UI.ViewManagement
{
	// Token: 0x020000A0 RID: 160
	[Guid("03021BE4-5254-4781-8194-5168F7D06D7B")]
	internal interface IUISettings3 : Windows.UI.ViewManagement.IUISettings3
	{
		// Token: 0x06000660 RID: 1632 RVA: 0x0000E3D8 File Offset: 0x0000C5D8
		unsafe static IUISettings3()
		{
			*(IInspectable.Vftbl*)((void*)IUISettings3.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IUISettings3.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_GetColorValue_0);
			*(IntPtr*)((byte*)((void*)IUISettings3.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_add_ColorValuesChanged_1);
			*(IntPtr*)((byte*)((void*)IUISettings3.AbiToProjectionVftablePtr) + (IntPtr)8 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_remove_ColorValuesChanged_2);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0000E470 File Offset: 0x0000C670
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_GetColorValue_0(IntPtr thisPtr, UIColorType desiredColor, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings3>(thisPtr).GetColorValue(desiredColor);
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0000E4C4 File Offset: 0x0000C6C4
		private static ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings3, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> MakeColorValuesChangedTable()
		{
			Interlocked.CompareExchange<ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings3, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>>>(ref IUISettings3._ColorValuesChanged_tokenTables, new ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings3, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>>(), null);
			return IUISettings3._ColorValuesChanged_tokenTables;
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x0000E4DE File Offset: 0x0000C6DE
		ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings3, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> _ColorValuesChanged_TokenTables
		{
			get
			{
				return IUISettings3._ColorValuesChanged_tokenTables ?? IUISettings3.MakeColorValuesChangedTable();
			}
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0000E4F0 File Offset: 0x0000C6F0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_add_ColorValuesChanged_1(IntPtr thisPtr, IntPtr handler, EventRegistrationToken* cookie)
		{
			*cookie = default(EventRegistrationToken);
			int result;
			try
			{
				Windows.UI.ViewManagement.IUISettings3 iuisettings = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings3>(thisPtr);
				Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object> typedEventHandler = ABI.Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>.FromAbi(handler);
				*cookie = IUISettings3._ColorValuesChanged_TokenTables.GetOrCreateValue(iuisettings).AddEventHandler(typedEventHandler);
				iuisettings.ColorValuesChanged += typedEventHandler;
				result = 0;
			}
			catch (Exception ex)
			{
				result = ex.HResult;
			}
			return result;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0000E550 File Offset: 0x0000C750
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private static int Do_Abi_remove_ColorValuesChanged_2(IntPtr thisPtr, EventRegistrationToken cookie)
		{
			int result;
			try
			{
				Windows.UI.ViewManagement.IUISettings3 iuisettings = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings3>(thisPtr);
				EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>> eventRegistrationTokenTable;
				Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object> value;
				if (iuisettings != null && IUISettings3._ColorValuesChanged_TokenTables.TryGetValue(iuisettings, ref eventRegistrationTokenTable) && eventRegistrationTokenTable.RemoveEventHandler(cookie, ref value))
				{
					iuisettings.ColorValuesChanged -= value;
				}
				result = 0;
			}
			catch (Exception ex)
			{
				result = ex.HResult;
			}
			return result;
		}

		// Token: 0x0400012D RID: 301
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IUISettings3), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 3);

		// Token: 0x0400012E RID: 302
		private static volatile ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings3, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> _ColorValuesChanged_tokenTables;
	}
}
