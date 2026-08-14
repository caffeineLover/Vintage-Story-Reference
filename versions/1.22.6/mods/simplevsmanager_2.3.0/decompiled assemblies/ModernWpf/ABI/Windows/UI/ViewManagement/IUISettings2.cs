using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using ABI.Windows.Foundation;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using WinRT;

namespace ABI.Windows.UI.ViewManagement
{
	// Token: 0x0200009E RID: 158
	[Guid("BAD82401-2721-44F9-BB91-2BB228BE442F")]
	internal interface IUISettings2 : Windows.UI.ViewManagement.IUISettings2
	{
		// Token: 0x06000656 RID: 1622 RVA: 0x0000E164 File Offset: 0x0000C364
		unsafe static IUISettings2()
		{
			*(IInspectable.Vftbl*)((void*)IUISettings2.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IUISettings2.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_TextScaleFactor_0);
			*(IntPtr*)((byte*)((void*)IUISettings2.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_add_TextScaleFactorChanged_1);
			*(IntPtr*)((byte*)((void*)IUISettings2.AbiToProjectionVftablePtr) + (IntPtr)8 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_remove_TextScaleFactorChanged_2);
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0000E1FC File Offset: 0x0000C3FC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_TextScaleFactor_0(IntPtr thisPtr, double* value)
		{
			*value = 0.0;
			try
			{
				double textScaleFactor = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings2>(thisPtr).TextScaleFactor;
				*value = textScaleFactor;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0000E250 File Offset: 0x0000C450
		private static ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings2, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> MakeTextScaleFactorChangedTable()
		{
			Interlocked.CompareExchange<ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings2, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>>>(ref IUISettings2._TextScaleFactorChanged_tokenTables, new ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings2, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>>(), null);
			return IUISettings2._TextScaleFactorChanged_tokenTables;
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x0000E26A File Offset: 0x0000C46A
		ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings2, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> _TextScaleFactorChanged_TokenTables
		{
			get
			{
				return IUISettings2._TextScaleFactorChanged_tokenTables ?? IUISettings2.MakeTextScaleFactorChangedTable();
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0000E27C File Offset: 0x0000C47C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_add_TextScaleFactorChanged_1(IntPtr thisPtr, IntPtr handler, EventRegistrationToken* cookie)
		{
			*cookie = default(EventRegistrationToken);
			int result;
			try
			{
				Windows.UI.ViewManagement.IUISettings2 iuisettings = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings2>(thisPtr);
				Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object> typedEventHandler = ABI.Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>.FromAbi(handler);
				*cookie = IUISettings2._TextScaleFactorChanged_TokenTables.GetOrCreateValue(iuisettings).AddEventHandler(typedEventHandler);
				iuisettings.TextScaleFactorChanged += typedEventHandler;
				result = 0;
			}
			catch (Exception ex)
			{
				result = ex.HResult;
			}
			return result;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0000E2DC File Offset: 0x0000C4DC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private static int Do_Abi_remove_TextScaleFactorChanged_2(IntPtr thisPtr, EventRegistrationToken cookie)
		{
			int result;
			try
			{
				Windows.UI.ViewManagement.IUISettings2 iuisettings = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings2>(thisPtr);
				EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>> eventRegistrationTokenTable;
				Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object> value;
				if (iuisettings != null && IUISettings2._TextScaleFactorChanged_TokenTables.TryGetValue(iuisettings, ref eventRegistrationTokenTable) && eventRegistrationTokenTable.RemoveEventHandler(cookie, ref value))
				{
					iuisettings.TextScaleFactorChanged -= value;
				}
				result = 0;
			}
			catch (Exception ex)
			{
				result = ex.HResult;
			}
			return result;
		}

		// Token: 0x0400012A RID: 298
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IUISettings2), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 3);

		// Token: 0x0400012B RID: 299
		private static volatile ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings2, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> _TextScaleFactorChanged_tokenTables;
	}
}
