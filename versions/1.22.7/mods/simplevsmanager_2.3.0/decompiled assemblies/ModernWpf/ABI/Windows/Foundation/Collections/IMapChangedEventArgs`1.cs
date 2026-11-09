using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x020000F6 RID: 246
	[DynamicInterfaceCastableImplementation]
	[Guid("9939F4DF-050A-4C0F-AA60-77075F9C4777")]
	internal interface IMapChangedEventArgs<K> : Windows.Foundation.Collections.IMapChangedEventArgs<K>
	{
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x00019828 File Offset: 0x00017A28
		CollectionChange Windows.Foundation.Collections.IMapChangedEventArgs<!0>.CollectionChange
		{
			get
			{
				return IMapChangedEventArgsMethods<K>.get_CollectionChange((ObjectReference<IMapChangedEventArgs<K>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.Collections.IMapChangedEventArgs<!0>).TypeHandle));
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0001984E File Offset: 0x00017A4E
		K Windows.Foundation.Collections.IMapChangedEventArgs<!0>.Key
		{
			get
			{
				return IMapChangedEventArgsMethods<K>.get_Key((ObjectReference<IMapChangedEventArgs<K>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.Collections.IMapChangedEventArgs<!0>).TypeHandle));
			}
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00019874 File Offset: 0x00017A74
		static IMapChangedEventArgs()
		{
			IMapChangedEventArgs<K>.PIID = IMapChangedEventArgs<K>.Vftbl.PIID;
		}

		// Token: 0x04000195 RID: 405
		public static Guid PIID;

		// Token: 0x02000312 RID: 786
		[Guid("9939F4DF-050A-4C0F-AA60-77075F9C4777")]
		public struct Vftbl
		{
			// Token: 0x17000463 RID: 1123
			// (get) Token: 0x06001727 RID: 5927 RVA: 0x000412BC File Offset: 0x0003F4BC
			// (set) Token: 0x06001728 RID: 5928 RVA: 0x000412C4 File Offset: 0x0003F4C4
			public method get_CollectionChange_0
			{
				get
				{
					return this._get_CollectionChange_0;
				}
				set
				{
					this._get_CollectionChange_0 = value;
				}
			}

			// Token: 0x06001729 RID: 5929 RVA: 0x000412D0 File Offset: 0x0003F4D0
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				this = default(IMapChangedEventArgs<K>.Vftbl);
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.get_CollectionChange_0 = (void*)ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.get_Key_1 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IMapChangedEventArgs<K>.Vftbl.get_Key_1_Type);
			}

			// Token: 0x0600172A RID: 5930 RVA: 0x0004133C File Offset: 0x0003F53C
			unsafe static Vftbl()
			{
				IMapChangedEventArgs<K>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(IMapChangedEventArgs<K>));
				IMapChangedEventArgs<K>.Vftbl.get_Key_1_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<K>.AbiType.MakeByRefType(),
					typeof(int)
				});
				IMapChangedEventArgs<K>.Vftbl.DelegateCache = new Delegate[2];
				IMapChangedEventArgs<K>.Vftbl.AbiToProjectionVftable = new IMapChangedEventArgs<K>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					_get_CollectionChange_0 = (void*)Marshal.GetFunctionPointerForDelegate(IMapChangedEventArgs<K>.Vftbl.DelegateCache[0] = new IMapChangedEventArgs_Delegates.get_CollectionChange_0(IMapChangedEventArgs<K>.Vftbl.Do_Abi_get_CollectionChange_0)),
					get_Key_1 = Delegate.CreateDelegate(IMapChangedEventArgs<K>.Vftbl.get_Key_1_Type, typeof(IMapChangedEventArgs<K>.Vftbl).GetMethod("Do_Abi_get_Key_1", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<K>.AbiType
					}))
				};
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IMapChangedEventArgs<K>.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 2));
				Marshal.StructureToPtr<IInspectable.Vftbl>(IMapChangedEventArgs<K>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IMapChangedEventArgs<K>.Vftbl.AbiToProjectionVftable._get_CollectionChange_0;
				ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IMapChangedEventArgs<K>.Vftbl.AbiToProjectionVftable.get_Key_1);
				IMapChangedEventArgs<K>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x0600172B RID: 5931 RVA: 0x00041494 File Offset: 0x0003F694
			private unsafe static int Do_Abi_get_CollectionChange_0(IntPtr thisPtr, CollectionChange* __return_value__)
			{
				*__return_value__ = CollectionChange.Reset;
				try
				{
					CollectionChange collectionChange = ComWrappersSupport.FindObject<Windows.Foundation.Collections.IMapChangedEventArgs<K>>(thisPtr).CollectionChange;
					*__return_value__ = collectionChange;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600172C RID: 5932 RVA: 0x000414D8 File Offset: 0x0003F6D8
			private unsafe static int Do_Abi_get_Key_1<KAbi>(void* thisPtr, out KAbi __return_value__)
			{
				K k = default(K);
				__return_value__ = default(KAbi);
				try
				{
					k = ComWrappersSupport.FindObject<Windows.Foundation.Collections.IMapChangedEventArgs<K>>(new IntPtr(thisPtr)).Key;
					__return_value__ = (KAbi)((object)Marshaler<K>.FromManaged.Invoke(k));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040007D2 RID: 2002
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040007D3 RID: 2003
			private unsafe void* _get_CollectionChange_0;

			// Token: 0x040007D4 RID: 2004
			public Delegate get_Key_1;

			// Token: 0x040007D5 RID: 2005
			public static Guid PIID;

			// Token: 0x040007D6 RID: 2006
			public static readonly Type get_Key_1_Type;

			// Token: 0x040007D7 RID: 2007
			private static readonly IMapChangedEventArgs<K>.Vftbl AbiToProjectionVftable;

			// Token: 0x040007D8 RID: 2008
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x040007D9 RID: 2009
			private static Delegate[] DelegateCache;
		}
	}
}
