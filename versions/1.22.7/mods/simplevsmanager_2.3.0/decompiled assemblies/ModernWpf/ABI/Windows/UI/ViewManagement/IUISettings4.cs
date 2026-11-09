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
	// Token: 0x020000A2 RID: 162
	[Guid("52BB3002-919B-4D6B-9B78-8DD66FF4B93B")]
	internal interface IUISettings4 : Windows.UI.ViewManagement.IUISettings4
	{
		// Token: 0x0600066A RID: 1642 RVA: 0x0000E648 File Offset: 0x0000C848
		unsafe static IUISettings4()
		{
			*(IInspectable.Vftbl*)((void*)IUISettings4.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IUISettings4.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_AdvancedEffectsEnabled_0);
			*(IntPtr*)((byte*)((void*)IUISettings4.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_add_AdvancedEffectsEnabledChanged_1);
			*(IntPtr*)((byte*)((void*)IUISettings4.AbiToProjectionVftablePtr) + (IntPtr)8 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_remove_AdvancedEffectsEnabledChanged_2);
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0000E6E0 File Offset: 0x0000C8E0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_AdvancedEffectsEnabled_0(IntPtr thisPtr, byte* value)
		{
			*value = 0;
			try
			{
				*value = (ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings4>(thisPtr).AdvancedEffectsEnabled ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0000E72C File Offset: 0x0000C92C
		private static ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings4, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> MakeAdvancedEffectsEnabledChangedTable()
		{
			Interlocked.CompareExchange<ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings4, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>>>(ref IUISettings4._AdvancedEffectsEnabledChanged_tokenTables, new ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings4, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>>(), null);
			return IUISettings4._AdvancedEffectsEnabledChanged_tokenTables;
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x0000E746 File Offset: 0x0000C946
		ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings4, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> _AdvancedEffectsEnabledChanged_TokenTables
		{
			get
			{
				return IUISettings4._AdvancedEffectsEnabledChanged_tokenTables ?? IUISettings4.MakeAdvancedEffectsEnabledChangedTable();
			}
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0000E758 File Offset: 0x0000C958
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_add_AdvancedEffectsEnabledChanged_1(IntPtr thisPtr, IntPtr handler, EventRegistrationToken* cookie)
		{
			*cookie = default(EventRegistrationToken);
			int result;
			try
			{
				Windows.UI.ViewManagement.IUISettings4 iuisettings = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings4>(thisPtr);
				Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object> typedEventHandler = ABI.Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>.FromAbi(handler);
				*cookie = IUISettings4._AdvancedEffectsEnabledChanged_TokenTables.GetOrCreateValue(iuisettings).AddEventHandler(typedEventHandler);
				iuisettings.AdvancedEffectsEnabledChanged += typedEventHandler;
				result = 0;
			}
			catch (Exception ex)
			{
				result = ex.HResult;
			}
			return result;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0000E7B8 File Offset: 0x0000C9B8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private static int Do_Abi_remove_AdvancedEffectsEnabledChanged_2(IntPtr thisPtr, EventRegistrationToken cookie)
		{
			int result;
			try
			{
				Windows.UI.ViewManagement.IUISettings4 iuisettings = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings4>(thisPtr);
				EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>> eventRegistrationTokenTable;
				Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object> value;
				if (iuisettings != null && IUISettings4._AdvancedEffectsEnabledChanged_TokenTables.TryGetValue(iuisettings, ref eventRegistrationTokenTable) && eventRegistrationTokenTable.RemoveEventHandler(cookie, ref value))
				{
					iuisettings.AdvancedEffectsEnabledChanged -= value;
				}
				result = 0;
			}
			catch (Exception ex)
			{
				result = ex.HResult;
			}
			return result;
		}

		// Token: 0x04000130 RID: 304
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IUISettings4), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 3);

		// Token: 0x04000131 RID: 305
		private static volatile ConditionalWeakTable<Windows.UI.ViewManagement.IUISettings4, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> _AdvancedEffectsEnabledChanged_tokenTables;
	}
}
