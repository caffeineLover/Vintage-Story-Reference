using System;
using ABI.System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000DE RID: 222
	internal static class IUriRuntimeClassMethods
	{
		// Token: 0x0600082D RID: 2093 RVA: 0x00016AE8 File Offset: 0x00014CE8
		public unsafe static bool Equals(IObjectReference _obj, Uri pUri)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			byte b = 0;
			bool result;
			try
			{
				objectReferenceValue = Uri.CreateMarshaler2(pUri);
				method system.Int32_u0020(System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)21 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), ref b, system.Int32_u0020(System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = (b > 0);
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
			}
			return result;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x00016B58 File Offset: 0x00014D58
		public unsafe static Uri CombineUri(IObjectReference _obj, string relativeUri)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			Uri result;
			try
			{
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(relativeUri);
				try
				{
					fixed (char* ptr = pinnable.GetPinnableReference())
					{
						char* ptr2 = ptr;
						method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)22 * (IntPtr)sizeof(method));
						ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
						result = Uri.FromAbi(intPtr);
					}
				}
				finally
				{
					char* ptr = null;
				}
			}
			finally
			{
				Uri.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00016BE8 File Offset: 0x00014DE8
		public unsafe static string get_AbsoluteUri(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalString.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00016C48 File Offset: 0x00014E48
		public unsafe static string get_DisplayUri(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalString.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00016CA8 File Offset: 0x00014EA8
		public unsafe static string get_Domain(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)8 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalString.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00016D08 File Offset: 0x00014F08
		public unsafe static string get_Extension(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)9 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalString.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00016D68 File Offset: 0x00014F68
		public unsafe static string get_Fragment(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)10 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalString.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00016DC8 File Offset: 0x00014FC8
		public unsafe static string get_Host(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)11 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalString.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00016E28 File Offset: 0x00015028
		public unsafe static string get_Password(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)12 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalString.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00016E88 File Offset: 0x00015088
		public unsafe static string get_Path(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)13 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalString.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00016EE8 File Offset: 0x000150E8
		public unsafe static int get_Port(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			int result = 0;
			method system.Int32_u0020(System.IntPtr,System.Int32&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)19 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,System.Int32&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00016F24 File Offset: 0x00015124
		public unsafe static string get_Query(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)14 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalString.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00016F84 File Offset: 0x00015184
		public unsafe static Windows.Foundation.WwwFormUrlDecoder get_QueryParsed(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			Windows.Foundation.WwwFormUrlDecoder result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)15 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = WwwFormUrlDecoder.FromAbi(intPtr);
			}
			finally
			{
				WwwFormUrlDecoder.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00016FE4 File Offset: 0x000151E4
		public unsafe static string get_RawUri(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)16 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalString.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00017044 File Offset: 0x00015244
		public unsafe static string get_SchemeName(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)17 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalString.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x000170A4 File Offset: 0x000152A4
		public unsafe static bool get_Suspicious(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			method system.Int32_u0020(System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)20 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref b, system.Int32_u0020(System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return b > 0;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x000170E0 File Offset: 0x000152E0
		public unsafe static string get_UserName(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)18 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalString.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
			}
			return result;
		}
	}
}
