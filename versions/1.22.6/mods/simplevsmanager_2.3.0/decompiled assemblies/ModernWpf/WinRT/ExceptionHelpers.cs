using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using ABI.WinRT.Interop;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Markup;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x0200018F RID: 399
	internal static class ExceptionHelpers
	{
		// Token: 0x06000C9A RID: 3226
		[DllImport("oleaut32.dll")]
		private static extern int SetErrorInfo(uint dwReserved, IntPtr perrinfo);

		// Token: 0x06000C9B RID: 3227 RVA: 0x00023C24 File Offset: 0x00021E24
		private static bool Initialize()
		{
			IntPtr intPtr = Platform.LoadLibraryExW("api-ms-win-core-winrt-error-l1-1-1.dll", IntPtr.Zero, 2048U);
			if (intPtr != IntPtr.Zero)
			{
				ExceptionHelpers.roOriginateLanguageException = Platform.GetProcAddress(intPtr, "RoOriginateLanguageException");
				ExceptionHelpers.roReportUnhandledError = Platform.GetProcAddress(intPtr, "RoReportUnhandledError");
			}
			else
			{
				intPtr = Platform.LoadLibraryExW("api-ms-win-core-winrt-error-l1-1-0.dll", IntPtr.Zero, 2048U);
			}
			if (intPtr != IntPtr.Zero)
			{
				ExceptionHelpers.getRestrictedErrorInfo = Platform.GetProcAddress(intPtr, "GetRestrictedErrorInfo");
				ExceptionHelpers.setRestrictedErrorInfo = Platform.GetProcAddress(intPtr, "SetRestrictedErrorInfo");
			}
			return true;
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x00023CB8 File Offset: 0x00021EB8
		public static void ThrowExceptionForHR(int hr)
		{
			if (hr < 0)
			{
				ExceptionHelpers.<ThrowExceptionForHR>g__Throw|20_0(hr);
			}
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x00023CC4 File Offset: 0x00021EC4
		public static Exception GetExceptionForHR(int hr)
		{
			if (hr < 0)
			{
				bool flag;
				return ExceptionHelpers.GetExceptionForHR(hr, false, out flag);
			}
			return null;
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x00023CE0 File Offset: 0x00021EE0
		private static Exception GetExceptionForHR(int hr, bool useGlobalErrorState, out bool restoredExceptionFromGlobalState)
		{
			restoredExceptionFromGlobalState = false;
			ObjectReference<ABI.WinRT.Interop.IErrorInfo.Vftbl> objectReference = null;
			IObjectReference restrictedErrorObject = null;
			string text = null;
			string restrictedError = null;
			string restrictedErrorReference = null;
			string restrictedCapabilitySid = null;
			bool hasRestrictedLanguageErrorObject = false;
			Exception ex;
			if (useGlobalErrorState && ExceptionHelpers.getRestrictedErrorInfo != (UIntPtr)0)
			{
				method system.Int32_u0020(System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = ExceptionHelpers.getRestrictedErrorInfo;
				IntPtr intPtr;
				Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), ref intPtr, system.Int32_u0020(System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				if (intPtr != IntPtr.Zero)
				{
					IObjectReference objectReference2 = ObjectReference<ABI.WinRT.Interop.IRestrictedErrorInfo.Vftbl>.Attach(ref intPtr);
					restrictedErrorObject = objectReference2.As<ABI.WinRT.Interop.IRestrictedErrorInfo.Vftbl>();
					ABI.WinRT.Interop.IRestrictedErrorInfo restrictedErrorInfo = new ABI.WinRT.Interop.IRestrictedErrorInfo(objectReference2);
					int num;
					restrictedErrorInfo.GetErrorDetails(out text, out num, out restrictedError, out restrictedCapabilitySid);
					restrictedErrorReference = restrictedErrorInfo.GetReference();
					ObjectReference<ABI.WinRT.Interop.ILanguageExceptionErrorInfo.Vftbl> obj;
					if (objectReference2.TryAs<ABI.WinRT.Interop.ILanguageExceptionErrorInfo.Vftbl>(out obj) >= 0)
					{
						using (IObjectReference languageException = ((WinRT.Interop.ILanguageExceptionErrorInfo)new ABI.WinRT.Interop.ILanguageExceptionErrorInfo(obj)).GetLanguageException())
						{
							if (languageException != null)
							{
								if (languageException.IsReferenceToManagedObject)
								{
									ex = ComWrappersSupport.FindObject<Exception>(languageException.ThisPtr);
									if (ExceptionHelpers.GetHRForException(ex) == hr)
									{
										restoredExceptionFromGlobalState = true;
										return ex;
									}
								}
								else
								{
									hasRestrictedLanguageErrorObject = true;
								}
							}
							goto IL_E2;
						}
					}
					if (hr == num)
					{
						objectReference2.TryAs<ABI.WinRT.Interop.IErrorInfo.Vftbl>(out objectReference);
					}
				}
			}
			IL_E2:
			using (objectReference)
			{
				if (hr <= -2147009196)
				{
					if (hr <= -2147483624)
					{
						if (hr - -2147483635 > 1 && hr != -2147483624)
						{
							goto IL_182;
						}
					}
					else
					{
						if (hr == -2147023496)
						{
							ex = new COMException("Invalid window handle. (0x80070578)\r\nConsider WindowNative, InitializeWithWindow\r\nSee https://aka.ms/cswinrt/interop#windows-sdk", -2147023496);
							goto IL_1A8;
						}
						if (hr != -2147009196)
						{
							goto IL_182;
						}
					}
					ex = new InvalidOperationException(text);
					goto IL_1A8;
				}
				if (hr <= -2144665580)
				{
					if (hr == -2144665590)
					{
						ex = new XamlParseException();
						goto IL_1A8;
					}
					if (hr == -2144665580)
					{
						ex = new LayoutCycleException();
						goto IL_1A8;
					}
				}
				else
				{
					if (hr == -2144665570)
					{
						ex = new ElementNotEnabledException();
						goto IL_1A8;
					}
					if (hr == -2144665569)
					{
						ex = new ElementNotAvailableException();
						goto IL_1A8;
					}
				}
				IL_182:
				ex = Marshal.GetExceptionForHR(hr, (objectReference != null) ? objectReference.ThisPtr : ((IntPtr)(-1)));
			}
			IL_1A8:
			ex.AddExceptionDataForRestrictedErrorInfo(text, restrictedError, restrictedErrorReference, restrictedCapabilitySid, restrictedErrorObject, hasRestrictedLanguageErrorObject);
			return ex;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00023EC8 File Offset: 0x000220C8
		public unsafe static void SetErrorInfo(Exception ex)
		{
			if (ExceptionHelpers.getRestrictedErrorInfo != (UIntPtr)0 && ExceptionHelpers.setRestrictedErrorInfo != (UIntPtr)0 && ExceptionHelpers.roOriginateLanguageException != (UIntPtr)0)
			{
				IObjectReference objectReference;
				if (ex.TryGetRestrictedLanguageErrorObject(out objectReference))
				{
					using (objectReference)
					{
						method system.Int32_u0020(System.IntPtr) = ExceptionHelpers.setRestrictedErrorInfo;
						object obj = calli(System.Int32(System.IntPtr), objectReference.ThisPtr, system.Int32_u0020(System.IntPtr));
						return;
					}
				}
				string text = ex.Message;
				if (string.IsNullOrEmpty(text))
				{
					text = ex.GetType().FullName;
				}
				IntPtr zero;
				if (Platform.WindowsCreateString(text, text.Length, &zero) != 0)
				{
					zero = IntPtr.Zero;
				}
				using (IObjectReference objectReference3 = ComWrappersSupport.CreateCCWForObject(ex))
				{
					method system.Int32_u0020(System.Int32,System.IntPtr,System.IntPtr) = ExceptionHelpers.roOriginateLanguageException;
					object obj2 = calli(System.Int32(System.Int32,System.IntPtr,System.IntPtr), ExceptionHelpers.GetHRForException(ex), zero, objectReference3.ThisPtr, system.Int32_u0020(System.Int32,System.IntPtr,System.IntPtr));
					return;
				}
			}
			using (IObjectReference objectReference4 = ComWrappersSupport.CreateCCWForObject(new ManagedExceptionErrorInfo(ex)))
			{
				ExceptionHelpers.SetErrorInfo(0U, objectReference4.ThisPtr);
			}
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x00023FE0 File Offset: 0x000221E0
		public static void ReportUnhandledError(Exception ex)
		{
			ExceptionHelpers.SetErrorInfo(ex);
			if (ExceptionHelpers.getRestrictedErrorInfo != (UIntPtr)0 && ExceptionHelpers.roReportUnhandledError != (UIntPtr)0)
			{
				method system.Int32_u0020(System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = ExceptionHelpers.getRestrictedErrorInfo;
				IntPtr intPtr;
				Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), ref intPtr, system.Int32_u0020(System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				using (ObjectReference<IUnknownVftbl> objectReference = ObjectReference<IUnknownVftbl>.Attach(ref intPtr))
				{
					method system.Int32_u0020(System.IntPtr) = ExceptionHelpers.roReportUnhandledError;
					object obj = calli(System.Int32(System.IntPtr), objectReference.ThisPtr, system.Int32_u0020(System.IntPtr));
				}
			}
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x00024050 File Offset: 0x00022250
		public static int GetHRForException(Exception ex)
		{
			int hresult = ex.HResult;
			IObjectReference objectReference;
			if (ex.TryGetRestrictedLanguageErrorObject(out objectReference))
			{
				string text;
				string text2;
				string text3;
				objectReference.AsType<ABI.WinRT.Interop.IRestrictedErrorInfo>().GetErrorDetails(out text, out hresult, out text2, out text3);
			}
			if (hresult == -2146232798)
			{
				return -2147483629;
			}
			return hresult;
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00024090 File Offset: 0x00022290
		internal static void AddExceptionDataForRestrictedErrorInfo(this Exception ex, string description, string restrictedError, string restrictedErrorReference, string restrictedCapabilitySid, IObjectReference restrictedErrorObject, bool hasRestrictedLanguageErrorObject = false)
		{
			IDictionary data = ex.Data;
			if (data != null)
			{
				data.Add("Description", description);
				data.Add("RestrictedDescription", restrictedError);
				data.Add("RestrictedErrorReference", restrictedErrorReference);
				data.Add("RestrictedCapabilitySid", restrictedCapabilitySid);
				data.Add("__RestrictedErrorObjectReference", (restrictedErrorObject == null) ? null : new ExceptionHelpers.__RestrictedErrorObject(restrictedErrorObject));
				data.Add("__HasRestrictedLanguageErrorObject", hasRestrictedLanguageErrorObject);
			}
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x00024104 File Offset: 0x00022304
		internal static bool TryGetRestrictedLanguageErrorObject(this Exception ex, out IObjectReference restrictedErrorObject)
		{
			restrictedErrorObject = null;
			IDictionary data = ex.Data;
			if (data != null && data.Contains("__HasRestrictedLanguageErrorObject"))
			{
				if (data.Contains("__RestrictedErrorObjectReference"))
				{
					ExceptionHelpers.__RestrictedErrorObject _RestrictedErrorObject = data["__RestrictedErrorObjectReference"] as ExceptionHelpers.__RestrictedErrorObject;
					if (_RestrictedErrorObject != null)
					{
						restrictedErrorObject = _RestrictedErrorObject.RealErrorObject;
					}
				}
				return (bool)data["__HasRestrictedLanguageErrorObject"];
			}
			return false;
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00024168 File Offset: 0x00022368
		public static Exception AttachRestrictedErrorInfo(Exception e)
		{
			if (e != null)
			{
				try
				{
					method system.Int32_u0020(System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = ExceptionHelpers.getRestrictedErrorInfo;
					IntPtr intPtr;
					Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), ref intPtr, system.Int32_u0020(System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
					if (intPtr != IntPtr.Zero)
					{
						IObjectReference objectReference = ObjectReference<ABI.WinRT.Interop.IRestrictedErrorInfo.Vftbl>.Attach(ref intPtr);
						ABI.WinRT.Interop.IRestrictedErrorInfo restrictedErrorInfo = new ABI.WinRT.Interop.IRestrictedErrorInfo(objectReference);
						string description;
						int num;
						string restrictedError;
						string restrictedCapabilitySid;
						restrictedErrorInfo.GetErrorDetails(out description, out num, out restrictedError, out restrictedCapabilitySid);
						if (e.HResult == num)
						{
							e.AddExceptionDataForRestrictedErrorInfo(description, restrictedError, restrictedErrorInfo.GetReference(), restrictedCapabilitySid, objectReference.As<ABI.WinRT.Interop.IRestrictedErrorInfo.Vftbl>(), false);
						}
					}
				}
				catch
				{
				}
			}
			return e;
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x000241FC File Offset: 0x000223FC
		[CompilerGenerated]
		internal static void <ThrowExceptionForHR>g__Throw|20_0(int hr)
		{
			bool flag;
			Exception exceptionForHR = ExceptionHelpers.GetExceptionForHR(hr, true, out flag);
			if (flag)
			{
				ExceptionDispatchInfo.Capture(exceptionForHR).Throw();
				return;
			}
			throw exceptionForHR;
		}

		// Token: 0x04000222 RID: 546
		private const int COR_E_OBJECTDISPOSED = -2146232798;

		// Token: 0x04000223 RID: 547
		private const int RO_E_CLOSED = -2147483629;

		// Token: 0x04000224 RID: 548
		internal const int E_BOUNDS = -2147483637;

		// Token: 0x04000225 RID: 549
		internal const int E_CHANGED_STATE = -2147483636;

		// Token: 0x04000226 RID: 550
		private const int E_ILLEGAL_STATE_CHANGE = -2147483635;

		// Token: 0x04000227 RID: 551
		private const int E_ILLEGAL_METHOD_CALL = -2147483634;

		// Token: 0x04000228 RID: 552
		private const int E_ILLEGAL_DELEGATE_ASSIGNMENT = -2147483624;

		// Token: 0x04000229 RID: 553
		private const int APPMODEL_ERROR_NO_PACKAGE = -2147009196;

		// Token: 0x0400022A RID: 554
		internal const int E_XAMLPARSEFAILED = -2144665590;

		// Token: 0x0400022B RID: 555
		internal const int E_LAYOUTCYCLE = -2144665580;

		// Token: 0x0400022C RID: 556
		internal const int E_ELEMENTNOTENABLED = -2144665570;

		// Token: 0x0400022D RID: 557
		internal const int E_ELEMENTNOTAVAILABLE = -2144665569;

		// Token: 0x0400022E RID: 558
		internal const int ERROR_INVALID_WINDOW_HANDLE = -2147023496;

		// Token: 0x0400022F RID: 559
		private static method getRestrictedErrorInfo;

		// Token: 0x04000230 RID: 560
		private static method setRestrictedErrorInfo;

		// Token: 0x04000231 RID: 561
		private static method roOriginateLanguageException;

		// Token: 0x04000232 RID: 562
		private static method roReportUnhandledError;

		// Token: 0x04000233 RID: 563
		private static readonly bool initialized = ExceptionHelpers.Initialize();

		// Token: 0x020003CD RID: 973
		[Serializable]
		internal sealed class __RestrictedErrorObject
		{
			// Token: 0x06001AB4 RID: 6836 RVA: 0x00049020 File Offset: 0x00047220
			internal __RestrictedErrorObject(IObjectReference errorObject)
			{
				this._realErrorObject = errorObject;
			}

			// Token: 0x170004F9 RID: 1273
			// (get) Token: 0x06001AB5 RID: 6837 RVA: 0x0004902F File Offset: 0x0004722F
			public IObjectReference RealErrorObject
			{
				get
				{
					return this._realErrorObject;
				}
			}

			// Token: 0x0400098A RID: 2442
			[NonSerialized]
			private readonly IObjectReference _realErrorObject;
		}
	}
}
