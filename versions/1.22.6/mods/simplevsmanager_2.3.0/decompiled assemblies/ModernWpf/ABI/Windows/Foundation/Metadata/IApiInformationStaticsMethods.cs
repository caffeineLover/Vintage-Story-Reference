using System;
using WinRT;

namespace ABI.Windows.Foundation.Metadata
{
	// Token: 0x020000EE RID: 238
	internal static class IApiInformationStaticsMethods
	{
		// Token: 0x0600089D RID: 2205 RVA: 0x000186B4 File Offset: 0x000168B4
		public unsafe static bool IsTypePresent(IObjectReference _obj, string typeName)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			MarshalString.Pinnable pinnable = new MarshalString.Pinnable(typeName);
			fixed (char* pinnableReference = pinnable.GetPinnableReference())
			{
				char* ptr = pinnableReference;
				method system.Int32_u0020(System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), ref b, system.Int32_u0020(System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				return b > 0;
			}
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0001870C File Offset: 0x0001690C
		public unsafe static bool IsMethodPresent(IObjectReference _obj, string typeName, string methodName)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			MarshalString.Pinnable pinnable = new MarshalString.Pinnable(typeName);
			MarshalString.Pinnable pinnable2 = new MarshalString.Pinnable(methodName);
			fixed (char* pinnableReference = pinnable.GetPinnableReference())
			{
				char* ptr = pinnableReference;
				fixed (char* pinnableReference2 = pinnable2.GetPinnableReference())
				{
					char* ptr2 = pinnableReference2;
					method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method));
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), MarshalString.GetAbi(ref pinnable2), ref b, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
					return b > 0;
				}
			}
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00018784 File Offset: 0x00016984
		public unsafe static bool IsMethodPresent(IObjectReference _obj, string typeName, string methodName, uint inputParameterCount)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			MarshalString.Pinnable pinnable = new MarshalString.Pinnable(typeName);
			MarshalString.Pinnable pinnable2 = new MarshalString.Pinnable(methodName);
			fixed (char* pinnableReference = pinnable.GetPinnableReference())
			{
				char* ptr = pinnableReference;
				fixed (char* pinnableReference2 = pinnable2.GetPinnableReference())
				{
					char* ptr2 = pinnableReference2;
					method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.UInt32,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)8 * (IntPtr)sizeof(method));
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr,System.UInt32,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), MarshalString.GetAbi(ref pinnable2), inputParameterCount, ref b, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.UInt32,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
					return b > 0;
				}
			}
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x000187FC File Offset: 0x000169FC
		public unsafe static bool IsEventPresent(IObjectReference _obj, string typeName, string eventName)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			MarshalString.Pinnable pinnable = new MarshalString.Pinnable(typeName);
			MarshalString.Pinnable pinnable2 = new MarshalString.Pinnable(eventName);
			fixed (char* pinnableReference = pinnable.GetPinnableReference())
			{
				char* ptr = pinnableReference;
				fixed (char* pinnableReference2 = pinnable2.GetPinnableReference())
				{
					char* ptr2 = pinnableReference2;
					method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)9 * (IntPtr)sizeof(method));
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), MarshalString.GetAbi(ref pinnable2), ref b, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
					return b > 0;
				}
			}
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00018874 File Offset: 0x00016A74
		public unsafe static bool IsPropertyPresent(IObjectReference _obj, string typeName, string propertyName)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			MarshalString.Pinnable pinnable = new MarshalString.Pinnable(typeName);
			MarshalString.Pinnable pinnable2 = new MarshalString.Pinnable(propertyName);
			fixed (char* pinnableReference = pinnable.GetPinnableReference())
			{
				char* ptr = pinnableReference;
				fixed (char* pinnableReference2 = pinnable2.GetPinnableReference())
				{
					char* ptr2 = pinnableReference2;
					method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)10 * (IntPtr)sizeof(method));
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), MarshalString.GetAbi(ref pinnable2), ref b, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
					return b > 0;
				}
			}
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x000188EC File Offset: 0x00016AEC
		public unsafe static bool IsReadOnlyPropertyPresent(IObjectReference _obj, string typeName, string propertyName)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			MarshalString.Pinnable pinnable = new MarshalString.Pinnable(typeName);
			MarshalString.Pinnable pinnable2 = new MarshalString.Pinnable(propertyName);
			fixed (char* pinnableReference = pinnable.GetPinnableReference())
			{
				char* ptr = pinnableReference;
				fixed (char* pinnableReference2 = pinnable2.GetPinnableReference())
				{
					char* ptr2 = pinnableReference2;
					method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)11 * (IntPtr)sizeof(method));
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), MarshalString.GetAbi(ref pinnable2), ref b, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
					return b > 0;
				}
			}
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00018964 File Offset: 0x00016B64
		public unsafe static bool IsWriteablePropertyPresent(IObjectReference _obj, string typeName, string propertyName)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			MarshalString.Pinnable pinnable = new MarshalString.Pinnable(typeName);
			MarshalString.Pinnable pinnable2 = new MarshalString.Pinnable(propertyName);
			fixed (char* pinnableReference = pinnable.GetPinnableReference())
			{
				char* ptr = pinnableReference;
				fixed (char* pinnableReference2 = pinnable2.GetPinnableReference())
				{
					char* ptr2 = pinnableReference2;
					method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)12 * (IntPtr)sizeof(method));
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), MarshalString.GetAbi(ref pinnable2), ref b, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
					return b > 0;
				}
			}
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x000189DC File Offset: 0x00016BDC
		public unsafe static bool IsEnumNamedValuePresent(IObjectReference _obj, string enumTypeName, string valueName)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			MarshalString.Pinnable pinnable = new MarshalString.Pinnable(enumTypeName);
			MarshalString.Pinnable pinnable2 = new MarshalString.Pinnable(valueName);
			fixed (char* pinnableReference = pinnable.GetPinnableReference())
			{
				char* ptr = pinnableReference;
				fixed (char* pinnableReference2 = pinnable2.GetPinnableReference())
				{
					char* ptr2 = pinnableReference2;
					method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)13 * (IntPtr)sizeof(method));
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), MarshalString.GetAbi(ref pinnable2), ref b, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
					return b > 0;
				}
			}
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00018A54 File Offset: 0x00016C54
		public unsafe static bool IsApiContractPresent(IObjectReference _obj, string contractName, ushort majorVersion)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			MarshalString.Pinnable pinnable = new MarshalString.Pinnable(contractName);
			fixed (char* pinnableReference = pinnable.GetPinnableReference())
			{
				char* ptr = pinnableReference;
				method system.Int32_u0020(System.IntPtr,System.IntPtr,System.UInt16,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)14 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.UInt16,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), majorVersion, ref b, system.Int32_u0020(System.IntPtr,System.IntPtr,System.UInt16,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				return b > 0;
			}
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00018AB0 File Offset: 0x00016CB0
		public unsafe static bool IsApiContractPresent(IObjectReference _obj, string contractName, ushort majorVersion, ushort minorVersion)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			MarshalString.Pinnable pinnable = new MarshalString.Pinnable(contractName);
			fixed (char* pinnableReference = pinnable.GetPinnableReference())
			{
				char* ptr = pinnableReference;
				method system.Int32_u0020(System.IntPtr,System.IntPtr,System.UInt16,System.UInt16,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)15 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.UInt16,System.UInt16,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), majorVersion, minorVersion, ref b, system.Int32_u0020(System.IntPtr,System.IntPtr,System.UInt16,System.UInt16,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				return b > 0;
			}
		}
	}
}
