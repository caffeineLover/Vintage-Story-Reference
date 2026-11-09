using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using WinRT;

namespace ABI.System.Collections.Specialized
{
	// Token: 0x02000150 RID: 336
	[DynamicInterfaceCastableImplementation]
	[EditorBrowsable(1)]
	[Guid("530155E1-28A5-5693-87CE-30724D95A06D")]
	internal interface INotifyCollectionChanged : INotifyCollectionChanged
	{
		// Token: 0x06000ABF RID: 2751 RVA: 0x0001E027 File Offset: 0x0001C227
		internal static ObjectReference<INotifyCollectionChanged.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<INotifyCollectionChanged.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0001E030 File Offset: 0x0001C230
		private static NotifyCollectionChangedEventSource _CollectionChanged(IWinRTObject _this)
		{
			ObjectReference<INotifyCollectionChanged.Vftbl> _obj = (ObjectReference<INotifyCollectionChanged.Vftbl>)_this.GetObjectReferenceForType(typeof(INotifyCollectionChanged).TypeHandle);
			return (NotifyCollectionChangedEventSource)_this.GetOrCreateTypeHelperData(typeof(INotifyCollectionChanged).TypeHandle, () => new NotifyCollectionChangedEventSource(_obj, _obj.Vftbl.add_CollectionChanged_0, _obj.Vftbl.remove_CollectionChanged_1));
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000AC1 RID: 2753 RVA: 0x0001E089 File Offset: 0x0001C289
		// (remove) Token: 0x06000AC2 RID: 2754 RVA: 0x0001E09C File Offset: 0x0001C29C
		event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
		{
			add
			{
				INotifyCollectionChanged._CollectionChanged((IWinRTObject)this).Subscribe(value);
			}
			remove
			{
				INotifyCollectionChanged._CollectionChanged((IWinRTObject)this).Unsubscribe(value);
			}
		}

		// Token: 0x02000378 RID: 888
		[Guid("530155E1-28A5-5693-87CE-30724D95A06D")]
		public struct Vftbl
		{
			// Token: 0x170004BA RID: 1210
			// (get) Token: 0x060018FC RID: 6396 RVA: 0x00044E82 File Offset: 0x00043082
			// (set) Token: 0x060018FD RID: 6397 RVA: 0x00044E8A File Offset: 0x0004308A
			public method add_CollectionChanged_0
			{
				get
				{
					return this._add_CollectionChanged_0;
				}
				set
				{
					this._add_CollectionChanged_0 = value;
				}
			}

			// Token: 0x170004BB RID: 1211
			// (get) Token: 0x060018FE RID: 6398 RVA: 0x00044E93 File Offset: 0x00043093
			// (set) Token: 0x060018FF RID: 6399 RVA: 0x00044E9B File Offset: 0x0004309B
			public method remove_CollectionChanged_1
			{
				get
				{
					return this._remove_CollectionChanged_1;
				}
				set
				{
					this._remove_CollectionChanged_1 = value;
				}
			}

			// Token: 0x06001900 RID: 6400 RVA: 0x00044EA4 File Offset: 0x000430A4
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(INotifyCollectionChanged.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 2));
				Marshal.StructureToPtr<INotifyCollectionChanged.Vftbl>(INotifyCollectionChanged.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				INotifyCollectionChanged.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001901 RID: 6401 RVA: 0x00044F24 File Offset: 0x00043124
			private static ConditionalWeakTable<INotifyCollectionChanged, EventRegistrationTokenTable<NotifyCollectionChangedEventHandler>> MakeConditionalWeakTable()
			{
				Interlocked.CompareExchange<ConditionalWeakTable<INotifyCollectionChanged, EventRegistrationTokenTable<NotifyCollectionChangedEventHandler>>>(ref INotifyCollectionChanged.Vftbl._collectionChanged_TokenTables, new ConditionalWeakTable<INotifyCollectionChanged, EventRegistrationTokenTable<NotifyCollectionChangedEventHandler>>(), null);
				return INotifyCollectionChanged.Vftbl._collectionChanged_TokenTables;
			}

			// Token: 0x170004BC RID: 1212
			// (get) Token: 0x06001902 RID: 6402 RVA: 0x00044F3E File Offset: 0x0004313E
			private static ConditionalWeakTable<INotifyCollectionChanged, EventRegistrationTokenTable<NotifyCollectionChangedEventHandler>> _CollectionChanged_TokenTables
			{
				get
				{
					return INotifyCollectionChanged.Vftbl._collectionChanged_TokenTables ?? INotifyCollectionChanged.Vftbl.MakeConditionalWeakTable();
				}
			}

			// Token: 0x06001903 RID: 6403 RVA: 0x00044F50 File Offset: 0x00043150
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_add_CollectionChanged_0(IntPtr thisPtr, IntPtr handler, EventRegistrationToken* token)
			{
				*token = default(EventRegistrationToken);
				int result;
				try
				{
					INotifyCollectionChanged notifyCollectionChanged = ComWrappersSupport.FindObject<INotifyCollectionChanged>(thisPtr);
					NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler = NotifyCollectionChangedEventHandler.FromAbi(handler);
					*token = INotifyCollectionChanged.Vftbl._CollectionChanged_TokenTables.GetOrCreateValue(notifyCollectionChanged).AddEventHandler(notifyCollectionChangedEventHandler);
					notifyCollectionChanged.CollectionChanged += notifyCollectionChangedEventHandler;
					result = 0;
				}
				catch (Exception ex)
				{
					result = ex.HResult;
				}
				return result;
			}

			// Token: 0x06001904 RID: 6404 RVA: 0x00044FB0 File Offset: 0x000431B0
			[UnmanagedCallersOnly]
			private static int Do_Abi_remove_CollectionChanged_1(IntPtr thisPtr, EventRegistrationToken token)
			{
				int result;
				try
				{
					INotifyCollectionChanged notifyCollectionChanged = ComWrappersSupport.FindObject<INotifyCollectionChanged>(thisPtr);
					EventRegistrationTokenTable<NotifyCollectionChangedEventHandler> eventRegistrationTokenTable;
					NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler;
					if (notifyCollectionChanged != null && INotifyCollectionChanged.Vftbl._CollectionChanged_TokenTables.TryGetValue(notifyCollectionChanged, ref eventRegistrationTokenTable) && eventRegistrationTokenTable.RemoveEventHandler(token, ref notifyCollectionChangedEventHandler))
					{
						notifyCollectionChanged.CollectionChanged -= notifyCollectionChangedEventHandler;
					}
					result = 0;
				}
				catch (Exception ex)
				{
					result = ex.HResult;
				}
				return result;
			}

			// Token: 0x040008C3 RID: 2243
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040008C4 RID: 2244
			private method _add_CollectionChanged_0;

			// Token: 0x040008C5 RID: 2245
			private method _remove_CollectionChanged_1;

			// Token: 0x040008C6 RID: 2246
			private static readonly INotifyCollectionChanged.Vftbl AbiToProjectionVftable = new INotifyCollectionChanged.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_add_CollectionChanged_0 = ldftn(Do_Abi_add_CollectionChanged_0),
				_remove_CollectionChanged_1 = ldftn(Do_Abi_remove_CollectionChanged_1)
			};

			// Token: 0x040008C7 RID: 2247
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x040008C8 RID: 2248
			private static volatile ConditionalWeakTable<INotifyCollectionChanged, EventRegistrationTokenTable<NotifyCollectionChangedEventHandler>> _collectionChanged_TokenTables;
		}
	}
}
