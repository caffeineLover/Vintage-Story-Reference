using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x020000FC RID: 252
	[DynamicInterfaceCastableImplementation]
	[Guid("5917EB53-50B4-4A0D-B309-65862B3F1DBC")]
	internal interface IObservableVector<T> : Windows.Foundation.Collections.IObservableVector<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060008E3 RID: 2275 RVA: 0x00019AF6 File Offset: 0x00017CF6
		// (remove) Token: 0x060008E4 RID: 2276 RVA: 0x00019B2D File Offset: 0x00017D2D
		event Windows.Foundation.Collections.VectorChangedEventHandler<T> Windows.Foundation.Collections.IObservableVector<!0>.VectorChanged
		{
			add
			{
				IObservableVectorMethods<T>.Get_VectorChanged((ObjectReference<IObservableVector<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.Collections.IObservableVector<T>).TypeHandle), (IWinRTObject)this).Item1.Invoke(value);
			}
			remove
			{
				IObservableVectorMethods<T>.Get_VectorChanged((ObjectReference<IObservableVector<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.Collections.IObservableVector<!0>).TypeHandle), (IWinRTObject)this).Item2.Invoke(value);
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x00019B64 File Offset: 0x00017D64
		int ICollection<!0>.Count
		{
			get
			{
				return ((IList<!0>)((IWinRTObject)this)).Count;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x00019B76 File Offset: 0x00017D76
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return ((IList<!0>)((IWinRTObject)this)).IsReadOnly;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x00019B88 File Offset: 0x00017D88
		// (set) Token: 0x060008E8 RID: 2280 RVA: 0x00019B9B File Offset: 0x00017D9B
		T IList<!0>.Item
		{
			get
			{
				return ((IList<!0>)((IWinRTObject)this))[index];
			}
			set
			{
				((IList<!0>)((IWinRTObject)this))[index] = value;
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00019BAF File Offset: 0x00017DAF
		int IndexOf(T item)
		{
			return ((IList<!0>)((IWinRTObject)this)).IndexOf(item);
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00019BC2 File Offset: 0x00017DC2
		void Insert(int index, T item)
		{
			((IList<!0>)((IWinRTObject)this)).Insert(index, item);
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00019BD6 File Offset: 0x00017DD6
		void RemoveAt(int index)
		{
			((IList<!0>)((IWinRTObject)this)).RemoveAt(index);
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00019BE9 File Offset: 0x00017DE9
		void Add(T item)
		{
			((IList<!0>)((IWinRTObject)this)).Add(item);
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00019BFC File Offset: 0x00017DFC
		void Clear()
		{
			((IList<!0>)((IWinRTObject)this)).Clear();
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00019C0E File Offset: 0x00017E0E
		bool Contains(T item)
		{
			return ((IList<!0>)((IWinRTObject)this)).Contains(item);
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00019C21 File Offset: 0x00017E21
		void CopyTo(T[] array, int arrayIndex)
		{
			((IList<!0>)((IWinRTObject)this)).CopyTo(array, arrayIndex);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00019C35 File Offset: 0x00017E35
		bool Remove(T item)
		{
			return ((IList<!0>)((IWinRTObject)this)).Remove(item);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x00019C48 File Offset: 0x00017E48
		IEnumerator<T> GetEnumerator()
		{
			return ((IList<!0>)((IWinRTObject)this)).GetEnumerator();
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00019C5A File Offset: 0x00017E5A
		IEnumerator GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00019C62 File Offset: 0x00017E62
		static IObservableVector()
		{
			IObservableVector<T>.PIID = IObservableVector<T>.Vftbl.PIID;
		}

		// Token: 0x04000199 RID: 409
		public static Guid PIID;

		// Token: 0x02000319 RID: 793
		[Guid("5917EB53-50B4-4A0D-B309-65862B3F1DBC")]
		public struct Vftbl
		{
			// Token: 0x17000467 RID: 1127
			// (get) Token: 0x06001747 RID: 5959 RVA: 0x00041857 File Offset: 0x0003FA57
			// (set) Token: 0x06001748 RID: 5960 RVA: 0x0004185F File Offset: 0x0003FA5F
			public method add_VectorChanged_0
			{
				get
				{
					return this._add_VectorChanged_0;
				}
				set
				{
					this._add_VectorChanged_0 = value;
				}
			}

			// Token: 0x17000468 RID: 1128
			// (get) Token: 0x06001749 RID: 5961 RVA: 0x00041868 File Offset: 0x0003FA68
			// (set) Token: 0x0600174A RID: 5962 RVA: 0x00041870 File Offset: 0x0003FA70
			public method remove_VectorChanged_1
			{
				get
				{
					return this._remove_VectorChanged_1;
				}
				set
				{
					this._remove_VectorChanged_1 = value;
				}
			}

			// Token: 0x0600174B RID: 5963 RVA: 0x0004187C File Offset: 0x0003FA7C
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				this = default(IObservableVector<T>.Vftbl);
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.add_VectorChanged_0 = (void*)ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.remove_VectorChanged_1 = (void*)ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
			}

			// Token: 0x0600174C RID: 5964 RVA: 0x000418E4 File Offset: 0x0003FAE4
			unsafe static Vftbl()
			{
				IObservableVector<T>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(IObservableVector<T>));
				IObservableVector<T>.Vftbl.DelegateCache = new Delegate[2];
				IObservableVector<T>.Vftbl.AbiToProjectionVftable = new IObservableVector<T>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					_add_VectorChanged_0 = (void*)Marshal.GetFunctionPointerForDelegate(IObservableVector<T>.Vftbl.DelegateCache[0] = new IObservableVector_Delegates.add_VectorChanged_0(IObservableVector<T>.Vftbl.Do_Abi_add_VectorChanged_0)),
					_remove_VectorChanged_1 = (void*)Marshal.GetFunctionPointerForDelegate(IObservableVector<T>.Vftbl.DelegateCache[1] = new IObservableVector_Delegates.remove_VectorChanged_1(IObservableVector<T>.Vftbl.Do_Abi_remove_VectorChanged_1))
				};
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IObservableVector<T>.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 2));
				Marshal.StructureToPtr<IInspectable.Vftbl>(IObservableVector<T>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IObservableVector<T>.Vftbl.AbiToProjectionVftable._add_VectorChanged_0;
				ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IObservableVector<T>.Vftbl.AbiToProjectionVftable._remove_VectorChanged_1;
				IObservableVector<T>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x0600174D RID: 5965 RVA: 0x000419F2 File Offset: 0x0003FBF2
			private static ConditionalWeakTable<Windows.Foundation.Collections.IObservableVector<T>, EventRegistrationTokenTable<Windows.Foundation.Collections.VectorChangedEventHandler<T>>> MakeVectorChangedTable()
			{
				Interlocked.CompareExchange<ConditionalWeakTable<Windows.Foundation.Collections.IObservableVector<T>, EventRegistrationTokenTable<Windows.Foundation.Collections.VectorChangedEventHandler<T>>>>(ref IObservableVector<T>.Vftbl._VectorChanged_tokenTables, new ConditionalWeakTable<Windows.Foundation.Collections.IObservableVector<T>, EventRegistrationTokenTable<Windows.Foundation.Collections.VectorChangedEventHandler<T>>>(), null);
				return IObservableVector<T>.Vftbl._VectorChanged_tokenTables;
			}

			// Token: 0x17000469 RID: 1129
			// (get) Token: 0x0600174E RID: 5966 RVA: 0x00041A0C File Offset: 0x0003FC0C
			private static ConditionalWeakTable<Windows.Foundation.Collections.IObservableVector<T>, EventRegistrationTokenTable<Windows.Foundation.Collections.VectorChangedEventHandler<T>>> _VectorChanged_TokenTables
			{
				get
				{
					return IObservableVector<T>.Vftbl._VectorChanged_tokenTables ?? IObservableVector<T>.Vftbl.MakeVectorChangedTable();
				}
			}

			// Token: 0x0600174F RID: 5967 RVA: 0x00041A20 File Offset: 0x0003FC20
			private unsafe static int Do_Abi_add_VectorChanged_0(IntPtr thisPtr, IntPtr vhnd, EventRegistrationToken* __return_value__)
			{
				*__return_value__ = default(EventRegistrationToken);
				int result;
				try
				{
					Windows.Foundation.Collections.IObservableVector<T> observableVector = ComWrappersSupport.FindObject<Windows.Foundation.Collections.IObservableVector<T>>(thisPtr);
					Windows.Foundation.Collections.VectorChangedEventHandler<T> vectorChangedEventHandler = VectorChangedEventHandler<T>.FromAbi(vhnd);
					*__return_value__ = IObservableVector<T>.Vftbl._VectorChanged_TokenTables.GetOrCreateValue(observableVector).AddEventHandler(vectorChangedEventHandler);
					observableVector.VectorChanged += vectorChangedEventHandler;
					result = 0;
				}
				catch (Exception ex)
				{
					result = ex.HResult;
				}
				return result;
			}

			// Token: 0x06001750 RID: 5968 RVA: 0x00041A80 File Offset: 0x0003FC80
			private static int Do_Abi_remove_VectorChanged_1(IntPtr thisPtr, EventRegistrationToken token)
			{
				int result;
				try
				{
					Windows.Foundation.Collections.IObservableVector<T> observableVector = ComWrappersSupport.FindObject<Windows.Foundation.Collections.IObservableVector<T>>(thisPtr);
					EventRegistrationTokenTable<Windows.Foundation.Collections.VectorChangedEventHandler<T>> eventRegistrationTokenTable;
					Windows.Foundation.Collections.VectorChangedEventHandler<T> vectorChangedEventHandler;
					if (observableVector != null && IObservableVector<T>.Vftbl._VectorChanged_TokenTables.TryGetValue(observableVector, ref eventRegistrationTokenTable) && eventRegistrationTokenTable.RemoveEventHandler(token, ref vectorChangedEventHandler))
					{
						observableVector.VectorChanged -= vectorChangedEventHandler;
					}
					result = 0;
				}
				catch (Exception ex)
				{
					result = ex.HResult;
				}
				return result;
			}

			// Token: 0x040007E4 RID: 2020
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040007E5 RID: 2021
			private unsafe void* _add_VectorChanged_0;

			// Token: 0x040007E6 RID: 2022
			private unsafe void* _remove_VectorChanged_1;

			// Token: 0x040007E7 RID: 2023
			public static Guid PIID;

			// Token: 0x040007E8 RID: 2024
			private static readonly IObservableVector<T>.Vftbl AbiToProjectionVftable;

			// Token: 0x040007E9 RID: 2025
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x040007EA RID: 2026
			private static Delegate[] DelegateCache;

			// Token: 0x040007EB RID: 2027
			private static volatile ConditionalWeakTable<Windows.Foundation.Collections.IObservableVector<T>, EventRegistrationTokenTable<Windows.Foundation.Collections.VectorChangedEventHandler<T>>> _VectorChanged_tokenTables;
		}
	}
}
