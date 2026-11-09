using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000C8 RID: 200
	[DynamicInterfaceCastableImplementation]
	[Guid("9FC2B0BB-E446-44E2-AA61-9CAB8F636AF2")]
	internal interface IAsyncOperation<TResult> : Windows.Foundation.IAsyncOperation<TResult>, Windows.Foundation.IAsyncInfo
	{
		// Token: 0x060007A4 RID: 1956 RVA: 0x00013554 File Offset: 0x00011754
		TResult GetResults()
		{
			return IAsyncOperationMethods<TResult>.GetResults((ObjectReference<IAsyncOperation<TResult>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncOperation<TResult>).TypeHandle));
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x0001357A File Offset: 0x0001177A
		// (set) Token: 0x060007A6 RID: 1958 RVA: 0x000135A0 File Offset: 0x000117A0
		Windows.Foundation.AsyncOperationCompletedHandler<TResult> Windows.Foundation.IAsyncOperation<!0>.Completed
		{
			get
			{
				return IAsyncOperationMethods<TResult>.get_Completed((ObjectReference<IAsyncOperation<TResult>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncOperation<!0>).TypeHandle));
			}
			set
			{
				IAsyncOperationMethods<TResult>.set_Completed((ObjectReference<IAsyncOperation<TResult>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncOperation<!0>).TypeHandle), value);
			}
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x000135C7 File Offset: 0x000117C7
		void Cancel()
		{
			((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Cancel();
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x000135D9 File Offset: 0x000117D9
		void Close()
		{
			((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Close();
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x000135EB File Offset: 0x000117EB
		Exception Windows.Foundation.IAsyncInfo.ErrorCode
		{
			get
			{
				return ((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).ErrorCode;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x000135FD File Offset: 0x000117FD
		uint Windows.Foundation.IAsyncInfo.Id
		{
			get
			{
				return ((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Id;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x0001360F File Offset: 0x0001180F
		AsyncStatus Windows.Foundation.IAsyncInfo.Status
		{
			get
			{
				return ((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Status;
			}
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00013621 File Offset: 0x00011821
		static IAsyncOperation()
		{
			IAsyncOperation<TResult>.PIID = IAsyncOperation<TResult>.Vftbl.PIID;
		}

		// Token: 0x0400017B RID: 379
		public static Guid PIID;

		// Token: 0x0200030A RID: 778
		[Guid("9FC2B0BB-E446-44E2-AA61-9CAB8F636AF2")]
		public struct Vftbl
		{
			// Token: 0x1700045D RID: 1117
			// (get) Token: 0x06001700 RID: 5888 RVA: 0x00040D94 File Offset: 0x0003EF94
			// (set) Token: 0x06001701 RID: 5889 RVA: 0x00040D9C File Offset: 0x0003EF9C
			public method put_Completed_0
			{
				get
				{
					return this._put_Completed_0;
				}
				set
				{
					this._put_Completed_0 = value;
				}
			}

			// Token: 0x1700045E RID: 1118
			// (get) Token: 0x06001702 RID: 5890 RVA: 0x00040DA5 File Offset: 0x0003EFA5
			// (set) Token: 0x06001703 RID: 5891 RVA: 0x00040DAD File Offset: 0x0003EFAD
			public method get_Completed_1
			{
				get
				{
					return this._get_Completed_1;
				}
				set
				{
					this._get_Completed_1 = value;
				}
			}

			// Token: 0x06001704 RID: 5892 RVA: 0x00040DB8 File Offset: 0x0003EFB8
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				this = default(IAsyncOperation<TResult>.Vftbl);
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.put_Completed_0 = (void*)ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.get_Completed_1 = (void*)ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.GetResults_2 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IAsyncOperation<TResult>.Vftbl.GetResults_2_Type);
			}

			// Token: 0x06001705 RID: 5893 RVA: 0x00040E3C File Offset: 0x0003F03C
			unsafe static Vftbl()
			{
				IAsyncOperation<TResult>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(IAsyncOperation<TResult>));
				IAsyncOperation<TResult>.Vftbl.GetResults_2_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<TResult>.AbiType.MakeByRefType(),
					typeof(int)
				});
				IAsyncOperation<TResult>.Vftbl.DelegateCache = new Delegate[3];
				IAsyncOperation<TResult>.Vftbl.AbiToProjectionVftable = new IAsyncOperation<TResult>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					_put_Completed_0 = (void*)Marshal.GetFunctionPointerForDelegate(IAsyncOperation<TResult>.Vftbl.DelegateCache[0] = new IAsyncOperation_Delegates.put_Completed_0(IAsyncOperation<TResult>.Vftbl.Do_Abi_put_Completed_0)),
					_get_Completed_1 = (void*)Marshal.GetFunctionPointerForDelegate(IAsyncOperation<TResult>.Vftbl.DelegateCache[1] = new IAsyncOperation_Delegates.get_Completed_1(IAsyncOperation<TResult>.Vftbl.Do_Abi_get_Completed_1)),
					GetResults_2 = Delegate.CreateDelegate(IAsyncOperation<TResult>.Vftbl.GetResults_2_Type, typeof(IAsyncOperation<TResult>.Vftbl).GetMethod("Do_Abi_GetResults_2", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<TResult>.AbiType
					}))
				};
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IAsyncOperation<TResult>.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 3));
				Marshal.StructureToPtr<IInspectable.Vftbl>(IAsyncOperation<TResult>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IAsyncOperation<TResult>.Vftbl.AbiToProjectionVftable._put_Completed_0;
				ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IAsyncOperation<TResult>.Vftbl.AbiToProjectionVftable._get_Completed_1;
				ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IAsyncOperation<TResult>.Vftbl.AbiToProjectionVftable.GetResults_2);
				IAsyncOperation<TResult>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001706 RID: 5894 RVA: 0x00040FD8 File Offset: 0x0003F1D8
			private unsafe static int Do_Abi_GetResults_2<TResultAbi>(void* thisPtr, out TResultAbi __return_value__)
			{
				TResult tresult = default(TResult);
				__return_value__ = default(TResultAbi);
				try
				{
					tresult = ComWrappersSupport.FindObject<Windows.Foundation.IAsyncOperation<TResult>>(new IntPtr(thisPtr)).GetResults();
					__return_value__ = (TResultAbi)((object)Marshaler<TResult>.FromManaged.Invoke(tresult));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001707 RID: 5895 RVA: 0x00041040 File Offset: 0x0003F240
			private static int Do_Abi_put_Completed_0(IntPtr thisPtr, IntPtr handler)
			{
				try
				{
					ComWrappersSupport.FindObject<Windows.Foundation.IAsyncOperation<TResult>>(thisPtr).Completed = AsyncOperationCompletedHandler<TResult>.FromAbi(handler);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001708 RID: 5896 RVA: 0x00041084 File Offset: 0x0003F284
			private unsafe static int Do_Abi_get_Completed_1(IntPtr thisPtr, IntPtr* __return_value__)
			{
				*__return_value__ = default(IntPtr);
				try
				{
					Windows.Foundation.AsyncOperationCompletedHandler<TResult> completed = ComWrappersSupport.FindObject<Windows.Foundation.IAsyncOperation<TResult>>(thisPtr).Completed;
					*__return_value__ = AsyncOperationCompletedHandler<TResult>.FromManaged(completed);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040007BC RID: 1980
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040007BD RID: 1981
			private unsafe void* _put_Completed_0;

			// Token: 0x040007BE RID: 1982
			private unsafe void* _get_Completed_1;

			// Token: 0x040007BF RID: 1983
			public Delegate GetResults_2;

			// Token: 0x040007C0 RID: 1984
			public static Guid PIID;

			// Token: 0x040007C1 RID: 1985
			public static readonly Type GetResults_2_Type;

			// Token: 0x040007C2 RID: 1986
			private static readonly IAsyncOperation<TResult>.Vftbl AbiToProjectionVftable;

			// Token: 0x040007C3 RID: 1987
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x040007C4 RID: 1988
			private static Delegate[] DelegateCache;
		}
	}
}
