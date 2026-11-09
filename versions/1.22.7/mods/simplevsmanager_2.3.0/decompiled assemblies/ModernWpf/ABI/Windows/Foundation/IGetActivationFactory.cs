using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000CF RID: 207
	[DynamicInterfaceCastableImplementation]
	[Guid("4EDB8EE2-96DD-49A7-94F7-4607DDAB8E3C")]
	internal interface IGetActivationFactory : Windows.Foundation.IGetActivationFactory
	{
		// Token: 0x060007B5 RID: 1973 RVA: 0x000138D0 File Offset: 0x00011AD0
		unsafe static IGetActivationFactory()
		{
			*(IInspectable.Vftbl*)((void*)IGetActivationFactory.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IGetActivationFactory.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_GetActivationFactory_0);
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00013930 File Offset: 0x00011B30
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_GetActivationFactory_0(IntPtr thisPtr, IntPtr activatableClassId, IntPtr* factory)
		{
			*factory = default(IntPtr);
			try
			{
				object activationFactory = ComWrappersSupport.FindObject<Windows.Foundation.IGetActivationFactory>(thisPtr).GetActivationFactory(MarshalString.FromAbi(activatableClassId));
				*factory = MarshalInspectable<object>.FromManaged(activationFactory, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00013984 File Offset: 0x00011B84
		object GetActivationFactory(string activatableClassId)
		{
			return IGetActivationFactoryMethods.GetActivationFactory(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IGetActivationFactory).TypeHandle), activatableClassId);
		}

		// Token: 0x0400017E RID: 382
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IGetActivationFactory), sizeof(IInspectable.Vftbl) + sizeof(IntPtr));
	}
}
