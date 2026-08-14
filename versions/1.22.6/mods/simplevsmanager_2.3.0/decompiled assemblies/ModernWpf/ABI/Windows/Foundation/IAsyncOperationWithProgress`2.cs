using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000C5 RID: 197
	[DynamicInterfaceCastableImplementation]
	[Guid("B5D036D7-E297-498F-BA60-0289E76E23DD")]
	internal interface IAsyncOperationWithProgress<TResult, TProgress> : Windows.Foundation.IAsyncOperationWithProgress<TResult, TProgress>, Windows.Foundation.IAsyncInfo
	{
		// Token: 0x06000796 RID: 1942 RVA: 0x000132F0 File Offset: 0x000114F0
		TResult GetResults()
		{
			return IAsyncOperationWithProgressMethods<TResult, TProgress>.GetResults((ObjectReference<IAsyncOperationWithProgress<TResult, TProgress>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncOperationWithProgress<TResult, TProgress>).TypeHandle));
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x00013316 File Offset: 0x00011516
		// (set) Token: 0x06000798 RID: 1944 RVA: 0x0001333C File Offset: 0x0001153C
		Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress> Windows.Foundation.IAsyncOperationWithProgress<!0, !1>.Progress
		{
			get
			{
				return IAsyncOperationWithProgressMethods<TResult, TProgress>.get_Progress((ObjectReference<IAsyncOperationWithProgress<TResult, TProgress>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncOperationWithProgress<!0, !1>).TypeHandle));
			}
			set
			{
				IAsyncOperationWithProgressMethods<TResult, TProgress>.set_Progress((ObjectReference<IAsyncOperationWithProgress<TResult, TProgress>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncOperationWithProgress<!0, !1>).TypeHandle), value);
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x00013363 File Offset: 0x00011563
		// (set) Token: 0x0600079A RID: 1946 RVA: 0x00013389 File Offset: 0x00011589
		Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress> Windows.Foundation.IAsyncOperationWithProgress<!0, !1>.Completed
		{
			get
			{
				return IAsyncOperationWithProgressMethods<TResult, TProgress>.get_Completed((ObjectReference<IAsyncOperationWithProgress<TResult, TProgress>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncOperationWithProgress<!0, !1>).TypeHandle));
			}
			set
			{
				IAsyncOperationWithProgressMethods<TResult, TProgress>.set_Completed((ObjectReference<IAsyncOperationWithProgress<TResult, TProgress>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncOperationWithProgress<!0, !1>).TypeHandle), value);
			}
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x000133B0 File Offset: 0x000115B0
		void Cancel()
		{
			((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Cancel();
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x000133C2 File Offset: 0x000115C2
		void Close()
		{
			((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Close();
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x000133D4 File Offset: 0x000115D4
		Exception Windows.Foundation.IAsyncInfo.ErrorCode
		{
			get
			{
				return ((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).ErrorCode;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x000133E6 File Offset: 0x000115E6
		uint Windows.Foundation.IAsyncInfo.Id
		{
			get
			{
				return ((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Id;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x000133F8 File Offset: 0x000115F8
		AsyncStatus Windows.Foundation.IAsyncInfo.Status
		{
			get
			{
				return ((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Status;
			}
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0001340A File Offset: 0x0001160A
		static IAsyncOperationWithProgress()
		{
			IAsyncOperationWithProgress<TResult, TProgress>.PIID = IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.PIID;
		}

		// Token: 0x0400017A RID: 378
		public static Guid PIID;

		// Token: 0x02000305 RID: 773
		[Guid("B5D036D7-E297-498F-BA60-0289E76E23DD")]
		public struct Vftbl
		{
			// Token: 0x17000459 RID: 1113
			// (get) Token: 0x060016E1 RID: 5857 RVA: 0x000408EC File Offset: 0x0003EAEC
			// (set) Token: 0x060016E2 RID: 5858 RVA: 0x000408F4 File Offset: 0x0003EAF4
			public method put_Progress_0
			{
				get
				{
					return this._put_Progress_0;
				}
				set
				{
					this._put_Progress_0 = value;
				}
			}

			// Token: 0x1700045A RID: 1114
			// (get) Token: 0x060016E3 RID: 5859 RVA: 0x000408FD File Offset: 0x0003EAFD
			// (set) Token: 0x060016E4 RID: 5860 RVA: 0x00040905 File Offset: 0x0003EB05
			public method get_Progress_1
			{
				get
				{
					return this._get_Progress_1;
				}
				set
				{
					this._get_Progress_1 = value;
				}
			}

			// Token: 0x1700045B RID: 1115
			// (get) Token: 0x060016E5 RID: 5861 RVA: 0x0004090E File Offset: 0x0003EB0E
			// (set) Token: 0x060016E6 RID: 5862 RVA: 0x00040916 File Offset: 0x0003EB16
			public method put_Completed_2
			{
				get
				{
					return this._put_Completed_2;
				}
				set
				{
					this._put_Completed_2 = value;
				}
			}

			// Token: 0x1700045C RID: 1116
			// (get) Token: 0x060016E7 RID: 5863 RVA: 0x0004091F File Offset: 0x0003EB1F
			// (set) Token: 0x060016E8 RID: 5864 RVA: 0x00040927 File Offset: 0x0003EB27
			public method get_Completed_3
			{
				get
				{
					return this._get_Completed_3;
				}
				set
				{
					this._get_Completed_3 = value;
				}
			}

			// Token: 0x060016E9 RID: 5865 RVA: 0x00040930 File Offset: 0x0003EB30
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				this = default(IAsyncOperationWithProgress<TResult, TProgress>.Vftbl);
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.put_Progress_0 = (void*)ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.get_Progress_1 = (void*)ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.put_Completed_2 = (void*)ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.get_Completed_3 = (void*)ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.GetResults_4 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)10 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.GetResults_4_Type);
			}

			// Token: 0x060016EA RID: 5866 RVA: 0x000409E4 File Offset: 0x0003EBE4
			unsafe static Vftbl()
			{
				IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(IAsyncOperationWithProgress<TResult, TProgress>));
				IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.GetResults_4_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<TResult>.AbiType.MakeByRefType(),
					typeof(int)
				});
				IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.DelegateCache = new Delegate[5];
				IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.AbiToProjectionVftable = new IAsyncOperationWithProgress<TResult, TProgress>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					_put_Progress_0 = (void*)Marshal.GetFunctionPointerForDelegate(IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.DelegateCache[0] = new IAsyncOperationWithProgress_Delegates.put_Progress_0(IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.Do_Abi_put_Progress_0)),
					_get_Progress_1 = (void*)Marshal.GetFunctionPointerForDelegate(IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.DelegateCache[1] = new IAsyncOperationWithProgress_Delegates.get_Progress_1(IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.Do_Abi_get_Progress_1)),
					_put_Completed_2 = (void*)Marshal.GetFunctionPointerForDelegate(IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.DelegateCache[2] = new IAsyncOperationWithProgress_Delegates.put_Completed_2(IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.Do_Abi_put_Completed_2)),
					_get_Completed_3 = (void*)Marshal.GetFunctionPointerForDelegate(IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.DelegateCache[3] = new IAsyncOperationWithProgress_Delegates.get_Completed_3(IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.Do_Abi_get_Completed_3)),
					GetResults_4 = Delegate.CreateDelegate(IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.GetResults_4_Type, typeof(IAsyncOperationWithProgress<TResult, TProgress>.Vftbl).GetMethod("Do_Abi_GetResults_4", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<TResult>.AbiType
					}))
				};
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IAsyncOperationWithProgress<TResult, TProgress>.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 5));
				Marshal.StructureToPtr<IInspectable.Vftbl>(IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.AbiToProjectionVftable._put_Progress_0;
				ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.AbiToProjectionVftable._get_Progress_1;
				ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.AbiToProjectionVftable._put_Completed_2;
				ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.AbiToProjectionVftable._get_Completed_3;
				ptr[(IntPtr)10 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.AbiToProjectionVftable.GetResults_4);
				IAsyncOperationWithProgress<TResult, TProgress>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060016EB RID: 5867 RVA: 0x00040C04 File Offset: 0x0003EE04
			private unsafe static int Do_Abi_GetResults_4<TResultAbi>(void* thisPtr, out TResultAbi __return_value__)
			{
				TResult tresult = default(TResult);
				__return_value__ = default(TResultAbi);
				try
				{
					tresult = ComWrappersSupport.FindObject<Windows.Foundation.IAsyncOperationWithProgress<TResult, TProgress>>(new IntPtr(thisPtr)).GetResults();
					__return_value__ = (TResultAbi)((object)Marshaler<TResult>.FromManaged.Invoke(tresult));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060016EC RID: 5868 RVA: 0x00040C6C File Offset: 0x0003EE6C
			private static int Do_Abi_put_Progress_0(IntPtr thisPtr, IntPtr handler)
			{
				try
				{
					ComWrappersSupport.FindObject<Windows.Foundation.IAsyncOperationWithProgress<TResult, TProgress>>(thisPtr).Progress = AsyncOperationProgressHandler<TResult, TProgress>.FromAbi(handler);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060016ED RID: 5869 RVA: 0x00040CB0 File Offset: 0x0003EEB0
			private unsafe static int Do_Abi_get_Progress_1(IntPtr thisPtr, IntPtr* __return_value__)
			{
				*__return_value__ = default(IntPtr);
				try
				{
					Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress> progress = ComWrappersSupport.FindObject<Windows.Foundation.IAsyncOperationWithProgress<TResult, TProgress>>(thisPtr).Progress;
					*__return_value__ = AsyncOperationProgressHandler<TResult, TProgress>.FromManaged(progress);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060016EE RID: 5870 RVA: 0x00040D00 File Offset: 0x0003EF00
			private static int Do_Abi_put_Completed_2(IntPtr thisPtr, IntPtr handler)
			{
				try
				{
					ComWrappersSupport.FindObject<Windows.Foundation.IAsyncOperationWithProgress<TResult, TProgress>>(thisPtr).Completed = AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.FromAbi(handler);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060016EF RID: 5871 RVA: 0x00040D44 File Offset: 0x0003EF44
			private unsafe static int Do_Abi_get_Completed_3(IntPtr thisPtr, IntPtr* __return_value__)
			{
				*__return_value__ = default(IntPtr);
				try
				{
					Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress> completed = ComWrappersSupport.FindObject<Windows.Foundation.IAsyncOperationWithProgress<TResult, TProgress>>(thisPtr).Completed;
					*__return_value__ = AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.FromManaged(completed);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040007B1 RID: 1969
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040007B2 RID: 1970
			private unsafe void* _put_Progress_0;

			// Token: 0x040007B3 RID: 1971
			private unsafe void* _get_Progress_1;

			// Token: 0x040007B4 RID: 1972
			private unsafe void* _put_Completed_2;

			// Token: 0x040007B5 RID: 1973
			private unsafe void* _get_Completed_3;

			// Token: 0x040007B6 RID: 1974
			public Delegate GetResults_4;

			// Token: 0x040007B7 RID: 1975
			public static Guid PIID;

			// Token: 0x040007B8 RID: 1976
			public static readonly Type GetResults_4_Type;

			// Token: 0x040007B9 RID: 1977
			private static readonly IAsyncOperationWithProgress<TResult, TProgress>.Vftbl AbiToProjectionVftable;

			// Token: 0x040007BA RID: 1978
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x040007BB RID: 1979
			private static Delegate[] DelegateCache;
		}
	}
}
