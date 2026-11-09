using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x02000108 RID: 264
	[Guid("1CF2B120-547D-101B-8E65-08002B2BD119")]
	internal class IErrorInfo : WinRT.Interop.IErrorInfo
	{
		// Token: 0x0600094D RID: 2381 RVA: 0x0001A633 File Offset: 0x00018833
		public static ObjectReference<IErrorInfo.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IErrorInfo.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0001A63B File Offset: 0x0001883B
		public static implicit operator IErrorInfo(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IErrorInfo(obj);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0001A648 File Offset: 0x00018848
		public static implicit operator IErrorInfo(ObjectReference<IErrorInfo.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IErrorInfo(obj);
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x0001A655 File Offset: 0x00018855
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0001A662 File Offset: 0x00018862
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0001A66F File Offset: 0x0001886F
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0001A67C File Offset: 0x0001887C
		public IErrorInfo(IObjectReference obj) : this(obj.As<IErrorInfo.Vftbl>())
		{
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0001A68A File Offset: 0x0001888A
		public IErrorInfo(ObjectReference<IErrorInfo.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0001A69C File Offset: 0x0001889C
		public unsafe Guid GetGuid()
		{
			method getGuid_ = this._obj.Vftbl.GetGuid_0;
			Guid result;
			Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Guid*), this.ThisPtr, &result, getGuid_));
			return result;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0001A6D4 File Offset: 0x000188D4
		public unsafe string GetSource()
		{
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method getSource_ = this._obj.Vftbl.GetSource_1;
				Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), this.ThisPtr, &intPtr, getSource_));
				result = ((intPtr != IntPtr.Zero) ? Marshal.PtrToStringBSTR(intPtr) : string.Empty);
			}
			finally
			{
				Marshal.FreeBSTR(intPtr);
			}
			return result;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0001A748 File Offset: 0x00018948
		public unsafe string GetDescription()
		{
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method getDescription_ = this._obj.Vftbl.GetDescription_2;
				Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), this.ThisPtr, &intPtr, getDescription_));
				result = ((intPtr != IntPtr.Zero) ? Marshal.PtrToStringBSTR(intPtr) : string.Empty);
			}
			finally
			{
				Marshal.FreeBSTR(intPtr);
			}
			return result;
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0001A7BC File Offset: 0x000189BC
		public unsafe string GetHelpFile()
		{
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method getHelpFile_ = this._obj.Vftbl.GetHelpFile_3;
				Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), this.ThisPtr, &intPtr, getHelpFile_));
				result = ((intPtr != IntPtr.Zero) ? Marshal.PtrToStringBSTR(intPtr) : string.Empty);
			}
			finally
			{
				Marshal.FreeBSTR(intPtr);
			}
			return result;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0001A830 File Offset: 0x00018A30
		public unsafe string GetHelpFileContent()
		{
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method getHelpFileContent_ = this._obj.Vftbl.GetHelpFileContent_4;
				Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), this.ThisPtr, &intPtr, getHelpFileContent_));
				result = ((intPtr != IntPtr.Zero) ? Marshal.PtrToStringBSTR(intPtr) : string.Empty);
			}
			finally
			{
				Marshal.FreeBSTR(intPtr);
			}
			return result;
		}

		// Token: 0x040001A6 RID: 422
		protected readonly ObjectReference<IErrorInfo.Vftbl> _obj;

		// Token: 0x02000321 RID: 801
		[Guid("1CF2B120-547D-101B-8E65-08002B2BD119")]
		public struct Vftbl
		{
			// Token: 0x17000472 RID: 1138
			// (get) Token: 0x06001775 RID: 6005 RVA: 0x00041DB4 File Offset: 0x0003FFB4
			// (set) Token: 0x06001776 RID: 6006 RVA: 0x00041DBC File Offset: 0x0003FFBC
			public method GetGuid_0
			{
				get
				{
					return this._GetGuid_0;
				}
				set
				{
					this._GetGuid_0 = value;
				}
			}

			// Token: 0x17000473 RID: 1139
			// (get) Token: 0x06001777 RID: 6007 RVA: 0x00041DC5 File Offset: 0x0003FFC5
			// (set) Token: 0x06001778 RID: 6008 RVA: 0x00041DCD File Offset: 0x0003FFCD
			public method GetSource_1
			{
				get
				{
					return this._GetSource_1;
				}
				set
				{
					this._GetSource_1 = value;
				}
			}

			// Token: 0x17000474 RID: 1140
			// (get) Token: 0x06001779 RID: 6009 RVA: 0x00041DD6 File Offset: 0x0003FFD6
			// (set) Token: 0x0600177A RID: 6010 RVA: 0x00041DDE File Offset: 0x0003FFDE
			public method GetDescription_2
			{
				get
				{
					return this._GetDescription_2;
				}
				set
				{
					this._GetDescription_2 = value;
				}
			}

			// Token: 0x17000475 RID: 1141
			// (get) Token: 0x0600177B RID: 6011 RVA: 0x00041DE7 File Offset: 0x0003FFE7
			// (set) Token: 0x0600177C RID: 6012 RVA: 0x00041DEF File Offset: 0x0003FFEF
			public method GetHelpFile_3
			{
				get
				{
					return this._GetHelpFile_3;
				}
				set
				{
					this._GetHelpFile_3 = value;
				}
			}

			// Token: 0x17000476 RID: 1142
			// (get) Token: 0x0600177D RID: 6013 RVA: 0x00041DF8 File Offset: 0x0003FFF8
			// (set) Token: 0x0600177E RID: 6014 RVA: 0x00041E00 File Offset: 0x00040000
			public method GetHelpFileContent_4
			{
				get
				{
					return this._GetHelpFileContent_4;
				}
				set
				{
					this._GetHelpFileContent_4 = value;
				}
			}

			// Token: 0x0600177F RID: 6015 RVA: 0x00041E0C File Offset: 0x0004000C
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)Marshal.AllocCoTaskMem(Marshal.SizeOf<IErrorInfo.Vftbl>()));
				Marshal.StructureToPtr<IErrorInfo.Vftbl>(IErrorInfo.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				IErrorInfo.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001780 RID: 6016 RVA: 0x00041EA0 File Offset: 0x000400A0
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetGuid_0(IntPtr thisPtr, Guid* guid)
			{
				try
				{
					*guid = ComWrappersSupport.FindObject<WinRT.Interop.IErrorInfo>(thisPtr).GetGuid();
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001781 RID: 6017 RVA: 0x00041EE4 File Offset: 0x000400E4
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetSource_1(IntPtr thisPtr, IntPtr* source)
			{
				*source = IntPtr.Zero;
				try
				{
					string source2 = ComWrappersSupport.FindObject<WinRT.Interop.IErrorInfo>(thisPtr).GetSource();
					*source = Marshal.StringToBSTR(source2);
				}
				catch (Exception ex)
				{
					Marshal.FreeBSTR(*source);
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001782 RID: 6018 RVA: 0x00041F38 File Offset: 0x00040138
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetDescription_2(IntPtr thisPtr, IntPtr* description)
			{
				*description = IntPtr.Zero;
				try
				{
					string description2 = ComWrappersSupport.FindObject<WinRT.Interop.IErrorInfo>(thisPtr).GetDescription();
					*description = Marshal.StringToBSTR(description2);
				}
				catch (Exception ex)
				{
					Marshal.FreeBSTR(*description);
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001783 RID: 6019 RVA: 0x00041F8C File Offset: 0x0004018C
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetHelpFile_3(IntPtr thisPtr, IntPtr* helpFile)
			{
				*helpFile = IntPtr.Zero;
				try
				{
					string helpFile2 = ComWrappersSupport.FindObject<WinRT.Interop.IErrorInfo>(thisPtr).GetHelpFile();
					*helpFile = Marshal.StringToBSTR(helpFile2);
				}
				catch (Exception ex)
				{
					Marshal.FreeBSTR(*helpFile);
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001784 RID: 6020 RVA: 0x00041FE0 File Offset: 0x000401E0
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetHelpFileContent_4(IntPtr thisPtr, IntPtr* helpFileContent)
			{
				*helpFileContent = IntPtr.Zero;
				try
				{
					string helpFileContent2 = ComWrappersSupport.FindObject<WinRT.Interop.IErrorInfo>(thisPtr).GetHelpFileContent();
					*helpFileContent = Marshal.StringToBSTR(helpFileContent2);
				}
				catch (Exception ex)
				{
					Marshal.FreeBSTR(*helpFileContent);
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040007FB RID: 2043
			internal IUnknownVftbl IUnknownVftbl;

			// Token: 0x040007FC RID: 2044
			private unsafe void* _GetGuid_0;

			// Token: 0x040007FD RID: 2045
			private unsafe void* _GetSource_1;

			// Token: 0x040007FE RID: 2046
			private unsafe void* _GetDescription_2;

			// Token: 0x040007FF RID: 2047
			private unsafe void* _GetHelpFile_3;

			// Token: 0x04000800 RID: 2048
			private unsafe void* _GetHelpFileContent_4;

			// Token: 0x04000801 RID: 2049
			private static readonly IErrorInfo.Vftbl AbiToProjectionVftable = new IErrorInfo.Vftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				_GetGuid_0 = ldftn(Do_Abi_GetGuid_0),
				_GetSource_1 = ldftn(Do_Abi_GetSource_1),
				_GetDescription_2 = ldftn(Do_Abi_GetDescription_2),
				_GetHelpFile_3 = ldftn(Do_Abi_GetHelpFile_3),
				_GetHelpFileContent_4 = ldftn(Do_Abi_GetHelpFileContent_4)
			};

			// Token: 0x04000802 RID: 2050
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
