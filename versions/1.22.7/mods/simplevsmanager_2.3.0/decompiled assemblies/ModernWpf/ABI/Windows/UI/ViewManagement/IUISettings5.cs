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
	// Token: 0x020000A4 RID: 164
	[Guid("5349D588-0CB5-5F05-BD34-706B3231F0BD")]
	internal interface IUISettings5 : Windows.UI.ViewManagement.IUISettings5
	{
		// Token: 0x06000674 RID: 1652 RVA: 0x0000E8B0 File Offset: 0x0000CAB0
		unsafe static IUISettings5()
		{
			*(IInspectable.Vftbl*)((void*)IUISettings5.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IUISettings5.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_AutoHideScrollBars_0);
			*(IntPtr*)((byte*)((void*)IUISettings5.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_add_AutoHideScrollBarsChanged_1);
			*(IntPtr*)((byte*)((void*)IUISettings5.AbiToProjectionVftablePtr) + (IntPtr)8 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_remove_AutoHideScrollBarsChanged_2);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0000E948 File Offset: 0x0000CB48
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_AutoHideScrollBars_0(IntPtr thisPtr, byte* value)
		{
			*value = 0;
			try
			{
				*value = (ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings5>(thisPtr).AutoHideScrollBars ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0000E994 File Offset: 0x0000CB94
		private static ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings5, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>>> MakeAutoHideScrollBarsChangedTable()
		{
			Interlocked.CompareExchange<ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings5, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>>>>(ref IUISettings5._AutoHideScrollBarsChanged_tokenTables, new ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings5, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>>>(), null);
			return IUISettings5._AutoHideScrollBarsChanged_tokenTables;
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x0000E9AE File Offset: 0x0000CBAE
		ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings5, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>>> _AutoHideScrollBarsChanged_TokenTables
		{
			get
			{
				return IUISettings5._AutoHideScrollBarsChanged_tokenTables ?? IUISettings5.MakeAutoHideScrollBarsChangedTable();
			}
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0000E9C0 File Offset: 0x0000CBC0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_add_AutoHideScrollBarsChanged_1(IntPtr thisPtr, IntPtr handler, EventRegistrationToken* token)
		{
			*token = default(EventRegistrationToken);
			int result;
			try
			{
				Windows.UI.ViewManagement.IUISettings5 iuisettings = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings5>(thisPtr);
				Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs> typedEventHandler = ABI.Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>.FromAbi(handler);
				*token = IUISettings5._AutoHideScrollBarsChanged_TokenTables.GetOrCreateValue(iuisettings).AddEventHandler(typedEventHandler);
				iuisettings.AutoHideScrollBarsChanged += typedEventHandler;
				result = 0;
			}
			catch (Exception ex)
			{
				result = ex.HResult;
			}
			return result;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0000EA20 File Offset: 0x0000CC20
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private static int Do_Abi_remove_AutoHideScrollBarsChanged_2(IntPtr thisPtr, EventRegistrationToken token)
		{
			int result;
			try
			{
				Windows.UI.ViewManagement.IUISettings5 iuisettings = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings5>(thisPtr);
				EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>> eventRegistrationTokenTable;
				Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs> value;
				if (iuisettings != null && IUISettings5._AutoHideScrollBarsChanged_TokenTables.TryGetValue(iuisettings, ref eventRegistrationTokenTable) && eventRegistrationTokenTable.RemoveEventHandler(token, ref value))
				{
					iuisettings.AutoHideScrollBarsChanged -= value;
				}
				result = 0;
			}
			catch (Exception ex)
			{
				result = ex.HResult;
			}
			return result;
		}

		// Token: 0x04000133 RID: 307
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IUISettings5), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 3);

		// Token: 0x04000134 RID: 308
		private static volatile ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings5, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>>> _AutoHideScrollBarsChanged_tokenTables;
	}
}
