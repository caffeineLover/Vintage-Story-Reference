using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using WinRT;

namespace ABI.System.ComponentModel
{
	// Token: 0x02000148 RID: 328
	[DynamicInterfaceCastableImplementation]
	[Guid("90B17601-B065-586E-83D9-9ADC3A695284")]
	internal interface INotifyPropertyChanged : INotifyPropertyChanged
	{
		// Token: 0x06000A82 RID: 2690 RVA: 0x0001D5E1 File Offset: 0x0001B7E1
		internal static ObjectReference<INotifyPropertyChanged.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<INotifyPropertyChanged.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0001D5EC File Offset: 0x0001B7EC
		private static PropertyChangedEventSource _PropertyChanged(IWinRTObject _this)
		{
			ObjectReference<INotifyPropertyChanged.Vftbl> _obj = (ObjectReference<INotifyPropertyChanged.Vftbl>)_this.GetObjectReferenceForType(typeof(INotifyPropertyChanged).TypeHandle);
			return (PropertyChangedEventSource)_this.GetOrCreateTypeHelperData(typeof(INotifyPropertyChanged).TypeHandle, () => new PropertyChangedEventSource(_obj, _obj.Vftbl.add_PropertyChanged_0, _obj.Vftbl.remove_PropertyChanged_1));
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000A84 RID: 2692 RVA: 0x0001D645 File Offset: 0x0001B845
		// (remove) Token: 0x06000A85 RID: 2693 RVA: 0x0001D658 File Offset: 0x0001B858
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				INotifyPropertyChanged._PropertyChanged((IWinRTObject)this).Subscribe(value);
			}
			remove
			{
				INotifyPropertyChanged._PropertyChanged((IWinRTObject)this).Unsubscribe(value);
			}
		}

		// Token: 0x0200035F RID: 863
		[Guid("90B17601-B065-586E-83D9-9ADC3A695284")]
		public struct Vftbl
		{
			// Token: 0x170004A1 RID: 1185
			// (get) Token: 0x06001863 RID: 6243 RVA: 0x00043EEE File Offset: 0x000420EE
			// (set) Token: 0x06001864 RID: 6244 RVA: 0x00043EF6 File Offset: 0x000420F6
			public method add_PropertyChanged_0
			{
				get
				{
					return this._add_PropertyChanged_0;
				}
				set
				{
					this._add_PropertyChanged_0 = value;
				}
			}

			// Token: 0x170004A2 RID: 1186
			// (get) Token: 0x06001865 RID: 6245 RVA: 0x00043EFF File Offset: 0x000420FF
			// (set) Token: 0x06001866 RID: 6246 RVA: 0x00043F07 File Offset: 0x00042107
			public method remove_PropertyChanged_1
			{
				get
				{
					return this._remove_PropertyChanged_1;
				}
				set
				{
					this._remove_PropertyChanged_1 = value;
				}
			}

			// Token: 0x06001867 RID: 6247 RVA: 0x00043F10 File Offset: 0x00042110
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(INotifyPropertyChanged.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 2));
				Marshal.StructureToPtr<INotifyPropertyChanged.Vftbl>(INotifyPropertyChanged.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				INotifyPropertyChanged.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001868 RID: 6248 RVA: 0x00043F90 File Offset: 0x00042190
			private static ConditionalWeakTable<INotifyPropertyChanged, EventRegistrationTokenTable<PropertyChangedEventHandler>> MakeConditionalWeakTable()
			{
				Interlocked.CompareExchange<ConditionalWeakTable<INotifyPropertyChanged, EventRegistrationTokenTable<PropertyChangedEventHandler>>>(ref INotifyPropertyChanged.Vftbl._PropertyChanged_TokenTablesLazy, new ConditionalWeakTable<INotifyPropertyChanged, EventRegistrationTokenTable<PropertyChangedEventHandler>>(), null);
				return INotifyPropertyChanged.Vftbl._PropertyChanged_TokenTablesLazy;
			}

			// Token: 0x170004A3 RID: 1187
			// (get) Token: 0x06001869 RID: 6249 RVA: 0x00043FAA File Offset: 0x000421AA
			private static ConditionalWeakTable<INotifyPropertyChanged, EventRegistrationTokenTable<PropertyChangedEventHandler>> _PropertyChanged_TokenTables
			{
				get
				{
					return INotifyPropertyChanged.Vftbl._PropertyChanged_TokenTablesLazy ?? INotifyPropertyChanged.Vftbl.MakeConditionalWeakTable();
				}
			}

			// Token: 0x0600186A RID: 6250 RVA: 0x00043FBC File Offset: 0x000421BC
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_add_PropertyChanged_0(IntPtr thisPtr, IntPtr handler, EventRegistrationToken* token)
			{
				*token = default(EventRegistrationToken);
				int result;
				try
				{
					INotifyPropertyChanged notifyPropertyChanged = ComWrappersSupport.FindObject<INotifyPropertyChanged>(thisPtr);
					PropertyChangedEventHandler propertyChangedEventHandler = PropertyChangedEventHandler.FromAbi(handler);
					*token = INotifyPropertyChanged.Vftbl._PropertyChanged_TokenTables.GetOrCreateValue(notifyPropertyChanged).AddEventHandler(propertyChangedEventHandler);
					notifyPropertyChanged.PropertyChanged += propertyChangedEventHandler;
					result = 0;
				}
				catch (Exception ex)
				{
					result = ex.HResult;
				}
				return result;
			}

			// Token: 0x0600186B RID: 6251 RVA: 0x0004401C File Offset: 0x0004221C
			[UnmanagedCallersOnly]
			private static int Do_Abi_remove_PropertyChanged_1(IntPtr thisPtr, EventRegistrationToken token)
			{
				int result;
				try
				{
					INotifyPropertyChanged notifyPropertyChanged = ComWrappersSupport.FindObject<INotifyPropertyChanged>(thisPtr);
					EventRegistrationTokenTable<PropertyChangedEventHandler> eventRegistrationTokenTable;
					PropertyChangedEventHandler propertyChangedEventHandler;
					if (notifyPropertyChanged != null && INotifyPropertyChanged.Vftbl._PropertyChanged_TokenTables.TryGetValue(notifyPropertyChanged, ref eventRegistrationTokenTable) && eventRegistrationTokenTable.RemoveEventHandler(token, ref propertyChangedEventHandler))
					{
						notifyPropertyChanged.PropertyChanged -= propertyChangedEventHandler;
					}
					result = 0;
				}
				catch (Exception ex)
				{
					result = ex.HResult;
				}
				return result;
			}

			// Token: 0x0400089E RID: 2206
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400089F RID: 2207
			private method _add_PropertyChanged_0;

			// Token: 0x040008A0 RID: 2208
			private method _remove_PropertyChanged_1;

			// Token: 0x040008A1 RID: 2209
			private static readonly INotifyPropertyChanged.Vftbl AbiToProjectionVftable = new INotifyPropertyChanged.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_add_PropertyChanged_0 = ldftn(Do_Abi_add_PropertyChanged_0),
				_remove_PropertyChanged_1 = ldftn(Do_Abi_remove_PropertyChanged_1)
			};

			// Token: 0x040008A2 RID: 2210
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x040008A3 RID: 2211
			private static volatile ConditionalWeakTable<INotifyPropertyChanged, EventRegistrationTokenTable<PropertyChangedEventHandler>> _PropertyChanged_TokenTablesLazy;
		}
	}
}
