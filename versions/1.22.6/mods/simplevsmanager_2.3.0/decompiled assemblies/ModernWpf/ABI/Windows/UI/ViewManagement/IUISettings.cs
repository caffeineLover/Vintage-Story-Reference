using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using WinRT;

namespace ABI.Windows.UI.ViewManagement
{
	// Token: 0x0200009C RID: 156
	[Guid("85361600-1C63-4627-BCB1-3A89E0BC9C55")]
	internal interface IUISettings : Windows.UI.ViewManagement.IUISettings
	{
		// Token: 0x06000644 RID: 1604 RVA: 0x0000DB3C File Offset: 0x0000BD3C
		unsafe static IUISettings()
		{
			*(IInspectable.Vftbl*)((void*)IUISettings.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IUISettings.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_HandPreference_0);
			*(IntPtr*)((byte*)((void*)IUISettings.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_CursorSize_1);
			*(IntPtr*)((byte*)((void*)IUISettings.AbiToProjectionVftablePtr) + (IntPtr)8 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_ScrollBarSize_2);
			*(IntPtr*)((byte*)((void*)IUISettings.AbiToProjectionVftablePtr) + (IntPtr)9 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_ScrollBarArrowSize_3);
			*(IntPtr*)((byte*)((void*)IUISettings.AbiToProjectionVftablePtr) + (IntPtr)10 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_ScrollBarThumbBoxSize_4);
			*(IntPtr*)((byte*)((void*)IUISettings.AbiToProjectionVftablePtr) + (IntPtr)11 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_MessageDuration_5);
			*(IntPtr*)((byte*)((void*)IUISettings.AbiToProjectionVftablePtr) + (IntPtr)12 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_AnimationsEnabled_6);
			*(IntPtr*)((byte*)((void*)IUISettings.AbiToProjectionVftablePtr) + (IntPtr)13 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_CaretBrowsingEnabled_7);
			*(IntPtr*)((byte*)((void*)IUISettings.AbiToProjectionVftablePtr) + (IntPtr)14 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_CaretBlinkRate_8);
			*(IntPtr*)((byte*)((void*)IUISettings.AbiToProjectionVftablePtr) + (IntPtr)15 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_CaretWidth_9);
			*(IntPtr*)((byte*)((void*)IUISettings.AbiToProjectionVftablePtr) + (IntPtr)16 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DoubleClickTime_10);
			*(IntPtr*)((byte*)((void*)IUISettings.AbiToProjectionVftablePtr) + (IntPtr)17 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_MouseHoverTime_11);
			*(IntPtr*)((byte*)((void*)IUISettings.AbiToProjectionVftablePtr) + (IntPtr)18 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_UIElementColor_12);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0000DCEC File Offset: 0x0000BEEC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_UIElementColor_12(IntPtr thisPtr, UIElementType desiredElement, Windows.UI.Color* value)
		{
			Windows.UI.Color color = default(Windows.UI.Color);
			*value = default(Windows.UI.Color);
			try
			{
				color = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings>(thisPtr).UIElementColor(desiredElement);
				*value = color;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0000DD40 File Offset: 0x0000BF40
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_AnimationsEnabled_6(IntPtr thisPtr, byte* value)
		{
			*value = 0;
			try
			{
				*value = (ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings>(thisPtr).AnimationsEnabled ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0000DD8C File Offset: 0x0000BF8C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_CaretBlinkRate_8(IntPtr thisPtr, uint* value)
		{
			*value = 0U;
			try
			{
				uint caretBlinkRate = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings>(thisPtr).CaretBlinkRate;
				*value = caretBlinkRate;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0000DDD0 File Offset: 0x0000BFD0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_CaretBrowsingEnabled_7(IntPtr thisPtr, byte* value)
		{
			*value = 0;
			try
			{
				*value = (ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings>(thisPtr).CaretBrowsingEnabled ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0000DE1C File Offset: 0x0000C01C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_CaretWidth_9(IntPtr thisPtr, uint* value)
		{
			*value = 0U;
			try
			{
				uint caretWidth = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings>(thisPtr).CaretWidth;
				*value = caretWidth;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0000DE60 File Offset: 0x0000C060
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_CursorSize_1(IntPtr thisPtr, Size* value)
		{
			Size size = default(Size);
			*value = default(Size);
			try
			{
				size = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings>(thisPtr).CursorSize;
				*value = size;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0000DEB4 File Offset: 0x0000C0B4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DoubleClickTime_10(IntPtr thisPtr, uint* value)
		{
			*value = 0U;
			try
			{
				uint doubleClickTime = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings>(thisPtr).DoubleClickTime;
				*value = doubleClickTime;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0000DEF8 File Offset: 0x0000C0F8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_HandPreference_0(IntPtr thisPtr, HandPreference* value)
		{
			*value = HandPreference.LeftHanded;
			try
			{
				HandPreference handPreference = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings>(thisPtr).HandPreference;
				*value = handPreference;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0000DF3C File Offset: 0x0000C13C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_MessageDuration_5(IntPtr thisPtr, uint* value)
		{
			*value = 0U;
			try
			{
				uint messageDuration = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings>(thisPtr).MessageDuration;
				*value = messageDuration;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0000DF80 File Offset: 0x0000C180
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_MouseHoverTime_11(IntPtr thisPtr, uint* value)
		{
			*value = 0U;
			try
			{
				uint mouseHoverTime = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings>(thisPtr).MouseHoverTime;
				*value = mouseHoverTime;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0000DFC4 File Offset: 0x0000C1C4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_ScrollBarArrowSize_3(IntPtr thisPtr, Size* value)
		{
			Size size = default(Size);
			*value = default(Size);
			try
			{
				size = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings>(thisPtr).ScrollBarArrowSize;
				*value = size;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0000E018 File Offset: 0x0000C218
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_ScrollBarSize_2(IntPtr thisPtr, Size* value)
		{
			Size size = default(Size);
			*value = default(Size);
			try
			{
				size = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings>(thisPtr).ScrollBarSize;
				*value = size;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0000E06C File Offset: 0x0000C26C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_ScrollBarThumbBoxSize_4(IntPtr thisPtr, Size* value)
		{
			Size size = default(Size);
			*value = default(Size);
			try
			{
				size = ComWrappersSupport.FindObject<Windows.UI.ViewManagement.IUISettings>(thisPtr).ScrollBarThumbBoxSize;
				*value = size;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x04000128 RID: 296
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IUISettings), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 13);
	}
}
