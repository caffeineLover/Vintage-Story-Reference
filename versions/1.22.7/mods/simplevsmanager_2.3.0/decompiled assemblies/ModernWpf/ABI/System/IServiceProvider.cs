using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x0200011D RID: 285
	[EditorBrowsable(1)]
	[Guid("68B3A2DF-8173-539F-B524-C8A2348F5AFB")]
	[DynamicInterfaceCastableImplementation]
	internal interface IServiceProvider : IServiceProvider
	{
		// Token: 0x060009DC RID: 2524 RVA: 0x0001B9B9 File Offset: 0x00019BB9
		internal static ObjectReference<IServiceProvider.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IServiceProvider.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0001B9C4 File Offset: 0x00019BC4
		unsafe object GetService(Type type)
		{
			Type.Marshaler m = default(Type.Marshaler);
			IntPtr intPtr = default(IntPtr);
			char result;
			try
			{
				ObjectReference<IServiceProvider.Vftbl> objectReference = (ObjectReference<IServiceProvider.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IServiceProvider).TypeHandle);
				IntPtr thisPtr = objectReference.ThisPtr;
				m = Type.CreateMarshaler(type);
				method getService_ = objectReference.Vftbl.GetService_0;
				Type.GetAbi(m);
				IntPtr* ptr = &intPtr;
				int hr = getService_;
				calli();
				ExceptionHelpers.ThrowExceptionForHR(hr);
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				Type.DisposeMarshaler(m);
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x02000334 RID: 820
		[Guid("68B3A2DF-8173-539F-B524-C8A2348F5AFB")]
		public struct Vftbl
		{
			// Token: 0x060017CB RID: 6091 RVA: 0x00042804 File Offset: 0x00040A04
			unsafe static Vftbl()
			{
				*(IServiceProvider.Vftbl*)((void*)IServiceProvider.Vftbl.AbiToProjectionVftablePtr) = new IServiceProvider.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					GetService_0 = ldftn(Do_Abi_GetService_0)
				};
			}

			// Token: 0x060017CC RID: 6092 RVA: 0x00042864 File Offset: 0x00040A64
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetService_0(IntPtr thisPtr, Type type, IntPtr* result)
			{
				*result = default(IntPtr);
				try
				{
					object service = ComWrappersSupport.FindObject<IServiceProvider>(thisPtr).GetService(Type.FromAbi(type));
					*result = MarshalInspectable<object>.FromManaged(service, true);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0400082A RID: 2090
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400082B RID: 2091
			public method GetService_0;

			// Token: 0x0400082C RID: 2092
			public static readonly IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IServiceProvider.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 4);
		}
	}
}
