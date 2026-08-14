using System;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000C0 RID: 192
	[DynamicInterfaceCastableImplementation]
	[Guid("1F6DB258-E803-48A1-9546-EB7353398884")]
	internal interface IAsyncActionWithProgress<TProgress> : Windows.Foundation.IAsyncActionWithProgress<TProgress>, Windows.Foundation.IAsyncInfo
	{
		// Token: 0x06000776 RID: 1910 RVA: 0x00012BB8 File Offset: 0x00010DB8
		void GetResults()
		{
			IAsyncActionWithProgressMethods<TProgress>.GetResults((ObjectReference<IAsyncActionWithProgress<TProgress>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncActionWithProgress<TProgress>).TypeHandle));
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x00012BDE File Offset: 0x00010DDE
		// (set) Token: 0x06000778 RID: 1912 RVA: 0x00012C04 File Offset: 0x00010E04
		Windows.Foundation.AsyncActionProgressHandler<TProgress> Windows.Foundation.IAsyncActionWithProgress<!0>.Progress
		{
			get
			{
				return IAsyncActionWithProgressMethods<TProgress>.get_Progress((ObjectReference<IAsyncActionWithProgress<TProgress>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncActionWithProgress<!0>).TypeHandle));
			}
			set
			{
				IAsyncActionWithProgressMethods<TProgress>.set_Progress((ObjectReference<IAsyncActionWithProgress<TProgress>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncActionWithProgress<!0>).TypeHandle), value);
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x00012C2B File Offset: 0x00010E2B
		// (set) Token: 0x0600077A RID: 1914 RVA: 0x00012C51 File Offset: 0x00010E51
		Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress> Windows.Foundation.IAsyncActionWithProgress<!0>.Completed
		{
			get
			{
				return IAsyncActionWithProgressMethods<TProgress>.get_Completed((ObjectReference<IAsyncActionWithProgress<TProgress>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncActionWithProgress<!0>).TypeHandle));
			}
			set
			{
				IAsyncActionWithProgressMethods<TProgress>.set_Completed((ObjectReference<IAsyncActionWithProgress<TProgress>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncActionWithProgress<!0>).TypeHandle), value);
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00012C78 File Offset: 0x00010E78
		void Cancel()
		{
			((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Cancel();
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00012C8A File Offset: 0x00010E8A
		void Close()
		{
			((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Close();
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x00012C9C File Offset: 0x00010E9C
		Exception Windows.Foundation.IAsyncInfo.ErrorCode
		{
			get
			{
				return ((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).ErrorCode;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x00012CAE File Offset: 0x00010EAE
		uint Windows.Foundation.IAsyncInfo.Id
		{
			get
			{
				return ((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Id;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x00012CC0 File Offset: 0x00010EC0
		AsyncStatus Windows.Foundation.IAsyncInfo.Status
		{
			get
			{
				return ((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Status;
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00012CD2 File Offset: 0x00010ED2
		static IAsyncActionWithProgress()
		{
			IAsyncActionWithProgress<TProgress>.PIID = IAsyncActionWithProgress<TProgress>.Vftbl.PIID;
		}

		// Token: 0x04000178 RID: 376
		public static Guid PIID;

		// Token: 0x020002FF RID: 767
		[Guid("1F6DB258-E803-48A1-9546-EB7353398884")]
		public struct Vftbl
		{
			// Token: 0x17000454 RID: 1108
			// (get) Token: 0x060016BC RID: 5820 RVA: 0x000404A9 File Offset: 0x0003E6A9
			// (set) Token: 0x060016BD RID: 5821 RVA: 0x000404B1 File Offset: 0x0003E6B1
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

			// Token: 0x17000455 RID: 1109
			// (get) Token: 0x060016BE RID: 5822 RVA: 0x000404BA File Offset: 0x0003E6BA
			// (set) Token: 0x060016BF RID: 5823 RVA: 0x000404C2 File Offset: 0x0003E6C2
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

			// Token: 0x17000456 RID: 1110
			// (get) Token: 0x060016C0 RID: 5824 RVA: 0x000404CB File Offset: 0x0003E6CB
			// (set) Token: 0x060016C1 RID: 5825 RVA: 0x000404D3 File Offset: 0x0003E6D3
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

			// Token: 0x17000457 RID: 1111
			// (get) Token: 0x060016C2 RID: 5826 RVA: 0x000404DC File Offset: 0x0003E6DC
			// (set) Token: 0x060016C3 RID: 5827 RVA: 0x000404E4 File Offset: 0x0003E6E4
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

			// Token: 0x17000458 RID: 1112
			// (get) Token: 0x060016C4 RID: 5828 RVA: 0x000404ED File Offset: 0x0003E6ED
			// (set) Token: 0x060016C5 RID: 5829 RVA: 0x000404F5 File Offset: 0x0003E6F5
			public method GetResults_4
			{
				get
				{
					return this._GetResults_4;
				}
				set
				{
					this._GetResults_4 = value;
				}
			}

			// Token: 0x060016C6 RID: 5830 RVA: 0x00040500 File Offset: 0x0003E700
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				this = default(IAsyncActionWithProgress<TProgress>.Vftbl);
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.put_Progress_0 = (void*)ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.get_Progress_1 = (void*)ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.put_Completed_2 = (void*)ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.get_Completed_3 = (void*)ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.GetResults_4 = (void*)ptr[(IntPtr)10 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
			}

			// Token: 0x060016C7 RID: 5831 RVA: 0x000405B0 File Offset: 0x0003E7B0
			unsafe static Vftbl()
			{
				IAsyncActionWithProgress<TProgress>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(IAsyncActionWithProgress<TProgress>));
				IAsyncActionWithProgress<TProgress>.Vftbl.DelegateCache = new Delegate[5];
				IAsyncActionWithProgress<TProgress>.Vftbl.AbiToProjectionVftable = new IAsyncActionWithProgress<TProgress>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					_put_Progress_0 = (void*)Marshal.GetFunctionPointerForDelegate(IAsyncActionWithProgress<TProgress>.Vftbl.DelegateCache[0] = new IAsyncActionWithProgress_Delegates.put_Progress_0(IAsyncActionWithProgress<TProgress>.Vftbl.Do_Abi_put_Progress_0)),
					_get_Progress_1 = (void*)Marshal.GetFunctionPointerForDelegate(IAsyncActionWithProgress<TProgress>.Vftbl.DelegateCache[1] = new IAsyncActionWithProgress_Delegates.get_Progress_1(IAsyncActionWithProgress<TProgress>.Vftbl.Do_Abi_get_Progress_1)),
					_put_Completed_2 = (void*)Marshal.GetFunctionPointerForDelegate(IAsyncActionWithProgress<TProgress>.Vftbl.DelegateCache[2] = new IAsyncActionWithProgress_Delegates.put_Completed_2(IAsyncActionWithProgress<TProgress>.Vftbl.Do_Abi_put_Completed_2)),
					_get_Completed_3 = (void*)Marshal.GetFunctionPointerForDelegate(IAsyncActionWithProgress<TProgress>.Vftbl.DelegateCache[3] = new IAsyncActionWithProgress_Delegates.get_Completed_3(IAsyncActionWithProgress<TProgress>.Vftbl.Do_Abi_get_Completed_3)),
					_GetResults_4 = (void*)Marshal.GetFunctionPointerForDelegate(IAsyncActionWithProgress<TProgress>.Vftbl.DelegateCache[4] = new IAsyncActionWithProgress_Delegates.GetResults_4(IAsyncActionWithProgress<TProgress>.Vftbl.Do_Abi_GetResults_4))
				};
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IAsyncActionWithProgress<TProgress>.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 5));
				Marshal.StructureToPtr<IInspectable.Vftbl>(IAsyncActionWithProgress<TProgress>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IAsyncActionWithProgress<TProgress>.Vftbl.AbiToProjectionVftable._put_Progress_0;
				ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IAsyncActionWithProgress<TProgress>.Vftbl.AbiToProjectionVftable._get_Progress_1;
				ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IAsyncActionWithProgress<TProgress>.Vftbl.AbiToProjectionVftable._put_Completed_2;
				ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IAsyncActionWithProgress<TProgress>.Vftbl.AbiToProjectionVftable._get_Completed_3;
				ptr[(IntPtr)10 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IAsyncActionWithProgress<TProgress>.Vftbl.AbiToProjectionVftable._GetResults_4;
				IAsyncActionWithProgress<TProgress>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060016C8 RID: 5832 RVA: 0x00040788 File Offset: 0x0003E988
			private static int Do_Abi_GetResults_4(IntPtr thisPtr)
			{
				try
				{
					ComWrappersSupport.FindObject<Windows.Foundation.IAsyncActionWithProgress<TProgress>>(thisPtr).GetResults();
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060016C9 RID: 5833 RVA: 0x000407C4 File Offset: 0x0003E9C4
			private static int Do_Abi_put_Progress_0(IntPtr thisPtr, IntPtr handler)
			{
				try
				{
					ComWrappersSupport.FindObject<Windows.Foundation.IAsyncActionWithProgress<TProgress>>(thisPtr).Progress = AsyncActionProgressHandler<TProgress>.FromAbi(handler);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060016CA RID: 5834 RVA: 0x00040808 File Offset: 0x0003EA08
			private unsafe static int Do_Abi_get_Progress_1(IntPtr thisPtr, IntPtr* __return_value__)
			{
				*__return_value__ = default(IntPtr);
				try
				{
					Windows.Foundation.AsyncActionProgressHandler<TProgress> progress = ComWrappersSupport.FindObject<Windows.Foundation.IAsyncActionWithProgress<TProgress>>(thisPtr).Progress;
					*__return_value__ = AsyncActionProgressHandler<TProgress>.FromManaged(progress);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060016CB RID: 5835 RVA: 0x00040858 File Offset: 0x0003EA58
			private static int Do_Abi_put_Completed_2(IntPtr thisPtr, IntPtr handler)
			{
				try
				{
					ComWrappersSupport.FindObject<Windows.Foundation.IAsyncActionWithProgress<TProgress>>(thisPtr).Completed = AsyncActionWithProgressCompletedHandler<TProgress>.FromAbi(handler);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060016CC RID: 5836 RVA: 0x0004089C File Offset: 0x0003EA9C
			private unsafe static int Do_Abi_get_Completed_3(IntPtr thisPtr, IntPtr* __return_value__)
			{
				*__return_value__ = default(IntPtr);
				try
				{
					Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress> completed = ComWrappersSupport.FindObject<Windows.Foundation.IAsyncActionWithProgress<TProgress>>(thisPtr).Completed;
					*__return_value__ = AsyncActionWithProgressCompletedHandler<TProgress>.FromManaged(completed);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040007A7 RID: 1959
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040007A8 RID: 1960
			private unsafe void* _put_Progress_0;

			// Token: 0x040007A9 RID: 1961
			private unsafe void* _get_Progress_1;

			// Token: 0x040007AA RID: 1962
			private unsafe void* _put_Completed_2;

			// Token: 0x040007AB RID: 1963
			private unsafe void* _get_Completed_3;

			// Token: 0x040007AC RID: 1964
			private unsafe void* _GetResults_4;

			// Token: 0x040007AD RID: 1965
			public static Guid PIID;

			// Token: 0x040007AE RID: 1966
			private static readonly IAsyncActionWithProgress<TProgress>.Vftbl AbiToProjectionVftable;

			// Token: 0x040007AF RID: 1967
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x040007B0 RID: 1968
			private static Delegate[] DelegateCache;
		}
	}
}
