using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using ABI.System.Collections.Generic;
using WinRT;

namespace ABI.System.ComponentModel
{
	// Token: 0x02000146 RID: 326
	[DynamicInterfaceCastableImplementation]
	[EditorBrowsable(1)]
	[Guid("0EE6C2CC-273E-567D-BC0A-1DD87EE51EBA")]
	internal interface INotifyDataErrorInfo : INotifyDataErrorInfo
	{
		// Token: 0x06000A7C RID: 2684 RVA: 0x0001D446 File Offset: 0x0001B646
		internal static ObjectReference<INotifyDataErrorInfo.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<INotifyDataErrorInfo.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0001D450 File Offset: 0x0001B650
		private static EventSource__EventHandler<DataErrorsChangedEventArgs> _ErrorsChanged(IWinRTObject _this)
		{
			ObjectReference<INotifyDataErrorInfo.Vftbl> _obj = (ObjectReference<INotifyDataErrorInfo.Vftbl>)_this.GetObjectReferenceForType(typeof(INotifyDataErrorInfo).TypeHandle);
			IntPtr thisPtr = _obj.ThisPtr;
			return (EventSource__EventHandler<DataErrorsChangedEventArgs>)_this.GetOrCreateTypeHelperData(typeof(INotifyCollectionChanged).TypeHandle, () => new EventSource__EventHandler<DataErrorsChangedEventArgs>(_obj, _obj.Vftbl.add_ErrorsChanged_1, _obj.Vftbl.remove_ErrorsChanged_2, 0));
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0001D4B8 File Offset: 0x0001B6B8
		unsafe IEnumerable GetErrors(string propertyName)
		{
			ObjectReference<INotifyDataErrorInfo.Vftbl> objectReference = (ObjectReference<INotifyDataErrorInfo.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(INotifyDataErrorInfo).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			char result;
			try
			{
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(propertyName);
				try
				{
					fixed (char* ptr = pinnable.GetPinnableReference())
					{
						char* ptr2 = ptr;
						method getErrors_ = objectReference.Vftbl.GetErrors_3;
						MarshalString.GetAbi(ref pinnable);
						IntPtr* ptr3 = &intPtr;
						int hr = getErrors_;
						calli();
						ExceptionHelpers.ThrowExceptionForHR(hr);
						result = (IEnumerable<object>)IInspectable.FromAbi(intPtr);
					}
				}
				finally
				{
					char* ptr = null;
				}
			}
			finally
			{
				IEnumerable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x0001D568 File Offset: 0x0001B768
		unsafe bool INotifyDataErrorInfo.HasErrors
		{
			get
			{
				ObjectReference<INotifyDataErrorInfo.Vftbl> objectReference = (ObjectReference<INotifyDataErrorInfo.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(INotifyDataErrorInfo).TypeHandle);
				IntPtr thisPtr = objectReference.ThisPtr;
				byte b = 0;
				method get_HasErrors_ = objectReference.Vftbl.get_HasErrors_0;
				thisPtr;
				&b;
				int hr = get_HasErrors_;
				calli();
				ExceptionHelpers.ThrowExceptionForHR(hr);
				return b > 0;
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000A80 RID: 2688 RVA: 0x0001D5BB File Offset: 0x0001B7BB
		// (remove) Token: 0x06000A81 RID: 2689 RVA: 0x0001D5CE File Offset: 0x0001B7CE
		event EventHandler<DataErrorsChangedEventArgs> INotifyDataErrorInfo.ErrorsChanged
		{
			add
			{
				INotifyDataErrorInfo._ErrorsChanged((IWinRTObject)this).Subscribe(value);
			}
			remove
			{
				INotifyDataErrorInfo._ErrorsChanged((IWinRTObject)this).Unsubscribe(value);
			}
		}

		// Token: 0x02000359 RID: 857
		[Guid("0EE6C2CC-273E-567D-BC0A-1DD87EE51EBA")]
		public struct Vftbl
		{
			// Token: 0x0600184A RID: 6218 RVA: 0x00043CA8 File Offset: 0x00041EA8
			unsafe static Vftbl()
			{
				*(INotifyDataErrorInfo.Vftbl*)((void*)INotifyDataErrorInfo.Vftbl.AbiToProjectionVftablePtr) = new INotifyDataErrorInfo.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					get_HasErrors_0 = ldftn(Do_Abi_get_HasErrors_0),
					add_ErrorsChanged_1 = ldftn(Do_Abi_add_ErrorsChanged_1),
					remove_ErrorsChanged_2 = ldftn(Do_Abi_remove_ErrorsChanged_2),
					GetErrors_3 = ldftn(Do_Abi_GetErrors_3)
				};
			}

			// Token: 0x0600184B RID: 6219 RVA: 0x00043D30 File Offset: 0x00041F30
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetErrors_3(IntPtr thisPtr, IntPtr propertyName, IntPtr* result)
			{
				*result = default(IntPtr);
				try
				{
					IEnumerable<object> enumerable = Enumerable.OfType<object>(ComWrappersSupport.FindObject<INotifyDataErrorInfo>(thisPtr).GetErrors(MarshalString.FromAbi(propertyName)));
					*result = IEnumerable<object>.FromManaged(enumerable);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600184C RID: 6220 RVA: 0x00043D88 File Offset: 0x00041F88
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_HasErrors_0(IntPtr thisPtr, byte* value)
			{
				*value = 0;
				try
				{
					*value = (ComWrappersSupport.FindObject<INotifyDataErrorInfo>(thisPtr).HasErrors ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600184D RID: 6221 RVA: 0x00043DD4 File Offset: 0x00041FD4
			private static ConditionalWeakTable<INotifyDataErrorInfo, EventRegistrationTokenTable<EventHandler<DataErrorsChangedEventArgs>>> MakeConditionalWeakTable()
			{
				Interlocked.CompareExchange<ConditionalWeakTable<INotifyDataErrorInfo, EventRegistrationTokenTable<EventHandler<DataErrorsChangedEventArgs>>>>(ref INotifyDataErrorInfo.Vftbl._ErrorsChanged_TokenTablesLazy, new ConditionalWeakTable<INotifyDataErrorInfo, EventRegistrationTokenTable<EventHandler<DataErrorsChangedEventArgs>>>(), null);
				return INotifyDataErrorInfo.Vftbl._ErrorsChanged_TokenTablesLazy;
			}

			// Token: 0x170004A0 RID: 1184
			// (get) Token: 0x0600184E RID: 6222 RVA: 0x00043DEE File Offset: 0x00041FEE
			private static ConditionalWeakTable<INotifyDataErrorInfo, EventRegistrationTokenTable<EventHandler<DataErrorsChangedEventArgs>>> _ErrorsChanged_TokenTables
			{
				get
				{
					return INotifyDataErrorInfo.Vftbl._ErrorsChanged_TokenTablesLazy ?? INotifyDataErrorInfo.Vftbl.MakeConditionalWeakTable();
				}
			}

			// Token: 0x0600184F RID: 6223 RVA: 0x00043E00 File Offset: 0x00042000
			[UnmanagedCallersOnly(CallConvs = new Type[]
			{
				typeof(CallConvStdcall)
			})]
			private unsafe static int Do_Abi_add_ErrorsChanged_1(IntPtr thisPtr, IntPtr handler, EventRegistrationToken* token)
			{
				*token = default(EventRegistrationToken);
				int result;
				try
				{
					INotifyDataErrorInfo notifyDataErrorInfo = ComWrappersSupport.FindObject<INotifyDataErrorInfo>(thisPtr);
					EventHandler<DataErrorsChangedEventArgs> eventHandler = EventHandler<DataErrorsChangedEventArgs>.FromAbi(handler);
					*token = INotifyDataErrorInfo.Vftbl._ErrorsChanged_TokenTables.GetOrCreateValue(notifyDataErrorInfo).AddEventHandler(eventHandler);
					notifyDataErrorInfo.ErrorsChanged += eventHandler;
					result = 0;
				}
				catch (Exception ex)
				{
					result = ex.HResult;
				}
				return result;
			}

			// Token: 0x06001850 RID: 6224 RVA: 0x00043E60 File Offset: 0x00042060
			[UnmanagedCallersOnly(CallConvs = new Type[]
			{
				typeof(CallConvStdcall)
			})]
			private static int Do_Abi_remove_ErrorsChanged_2(IntPtr thisPtr, EventRegistrationToken token)
			{
				int result;
				try
				{
					INotifyDataErrorInfo notifyDataErrorInfo = ComWrappersSupport.FindObject<INotifyDataErrorInfo>(thisPtr);
					EventRegistrationTokenTable<EventHandler<DataErrorsChangedEventArgs>> eventRegistrationTokenTable;
					EventHandler<DataErrorsChangedEventArgs> eventHandler;
					if (notifyDataErrorInfo != null && INotifyDataErrorInfo.Vftbl._ErrorsChanged_TokenTables.TryGetValue(notifyDataErrorInfo, ref eventRegistrationTokenTable) && eventRegistrationTokenTable.RemoveEventHandler(token, ref eventHandler))
					{
						notifyDataErrorInfo.ErrorsChanged -= eventHandler;
					}
					result = 0;
				}
				catch (Exception ex)
				{
					result = ex.HResult;
				}
				return result;
			}

			// Token: 0x04000896 RID: 2198
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000897 RID: 2199
			public method get_HasErrors_0;

			// Token: 0x04000898 RID: 2200
			public method add_ErrorsChanged_1;

			// Token: 0x04000899 RID: 2201
			public method remove_ErrorsChanged_2;

			// Token: 0x0400089A RID: 2202
			public method GetErrors_3;

			// Token: 0x0400089B RID: 2203
			public static readonly IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(INotifyDataErrorInfo.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 4);

			// Token: 0x0400089C RID: 2204
			private static volatile ConditionalWeakTable<INotifyDataErrorInfo, EventRegistrationTokenTable<EventHandler<DataErrorsChangedEventArgs>>> _ErrorsChanged_TokenTablesLazy;
		}
	}
}
