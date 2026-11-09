using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;
using WinRT;

namespace ABI.System.Windows.Input
{
	// Token: 0x02000143 RID: 323
	[EditorBrowsable(1)]
	[Guid("E5AF3542-CA67-4081-995B-709DD13792DF")]
	[DynamicInterfaceCastableImplementation]
	internal interface ICommand : ICommand
	{
		// Token: 0x06000A6D RID: 2669 RVA: 0x0001D193 File Offset: 0x0001B393
		public static ObjectReference<ICommand.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<ICommand.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x0001D19C File Offset: 0x0001B39C
		private static EventHandlerEventSource _CanExecuteChanged(IWinRTObject _this)
		{
			ObjectReference<ICommand.Vftbl> _obj = (ObjectReference<ICommand.Vftbl>)_this.GetObjectReferenceForType(typeof(ICommand).TypeHandle);
			return (EventHandlerEventSource)_this.GetOrCreateTypeHelperData(typeof(ICommand).TypeHandle, () => new EventHandlerEventSource(_obj, _obj.Vftbl.add_CanExecuteChanged_0, _obj.Vftbl.remove_CanExecuteChanged_1));
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0001D1F8 File Offset: 0x0001B3F8
		bool CanExecute(object parameter)
		{
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			byte b = 0;
			bool result;
			try
			{
				ObjectReference<ICommand.Vftbl> objectReference = (ObjectReference<ICommand.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(ICommand).TypeHandle);
				IntPtr thisPtr = objectReference.ThisPtr;
				objectReferenceValue = MarshalInspectable<object>.CreateMarshaler2(parameter, true);
				method canExecute_ = objectReference.Vftbl.CanExecute_2;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), ref b, canExecute_));
				result = (b > 0);
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
			}
			return result;
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x0001D284 File Offset: 0x0001B484
		void Execute(object parameter)
		{
			ObjectReference<ICommand.Vftbl> objectReference = (ObjectReference<ICommand.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(ICommand).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			try
			{
				objectReferenceValue = MarshalInspectable<object>.CreateMarshaler2(parameter, true);
				method execute_ = objectReference.Vftbl.Execute_3;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), execute_));
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
			}
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000A71 RID: 2673 RVA: 0x0001D304 File Offset: 0x0001B504
		// (remove) Token: 0x06000A72 RID: 2674 RVA: 0x0001D317 File Offset: 0x0001B517
		event EventHandler ICommand.CanExecuteChanged
		{
			add
			{
				ICommand._CanExecuteChanged((IWinRTObject)this).Subscribe(value);
			}
			remove
			{
				ICommand._CanExecuteChanged((IWinRTObject)this).Unsubscribe(value);
			}
		}

		// Token: 0x02000353 RID: 851
		[Guid("E5AF3542-CA67-4081-995B-709DD13792DF")]
		public struct Vftbl
		{
			// Token: 0x1700049B RID: 1179
			// (get) Token: 0x0600182B RID: 6187 RVA: 0x000439DB File Offset: 0x00041BDB
			// (set) Token: 0x0600182C RID: 6188 RVA: 0x000439E3 File Offset: 0x00041BE3
			public method add_CanExecuteChanged_0
			{
				get
				{
					return this._add_CanExecuteChanged_0;
				}
				set
				{
					this._add_CanExecuteChanged_0 = value;
				}
			}

			// Token: 0x1700049C RID: 1180
			// (get) Token: 0x0600182D RID: 6189 RVA: 0x000439EC File Offset: 0x00041BEC
			// (set) Token: 0x0600182E RID: 6190 RVA: 0x000439F4 File Offset: 0x00041BF4
			public method remove_CanExecuteChanged_1
			{
				get
				{
					return this._remove_CanExecuteChanged_1;
				}
				set
				{
					this._remove_CanExecuteChanged_1 = value;
				}
			}

			// Token: 0x1700049D RID: 1181
			// (get) Token: 0x0600182F RID: 6191 RVA: 0x000439FD File Offset: 0x00041BFD
			// (set) Token: 0x06001830 RID: 6192 RVA: 0x00043A05 File Offset: 0x00041C05
			public method CanExecute_2
			{
				get
				{
					return this._CanExecute_2;
				}
				set
				{
					this._CanExecute_2 = value;
				}
			}

			// Token: 0x1700049E RID: 1182
			// (get) Token: 0x06001831 RID: 6193 RVA: 0x00043A0E File Offset: 0x00041C0E
			// (set) Token: 0x06001832 RID: 6194 RVA: 0x00043A16 File Offset: 0x00041C16
			public method Execute_3
			{
				get
				{
					return this._Execute_3;
				}
				set
				{
					this._Execute_3 = value;
				}
			}

			// Token: 0x06001833 RID: 6195 RVA: 0x00043A20 File Offset: 0x00041C20
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(ICommand.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 4));
				Marshal.StructureToPtr<ICommand.Vftbl>(ICommand.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				ICommand.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001834 RID: 6196 RVA: 0x00043ABC File Offset: 0x00041CBC
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_CanExecute_2(IntPtr thisPtr, IntPtr parameter, byte* result)
			{
				*result = 0;
				try
				{
					*result = (ComWrappersSupport.FindObject<ICommand>(thisPtr).CanExecute(MarshalInspectable<object>.FromAbi(parameter)) ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001835 RID: 6197 RVA: 0x00043B10 File Offset: 0x00041D10
			[UnmanagedCallersOnly]
			private static int Do_Abi_Execute_3(IntPtr thisPtr, IntPtr parameter)
			{
				try
				{
					ComWrappersSupport.FindObject<ICommand>(thisPtr).Execute(MarshalInspectable<object>.FromAbi(parameter));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001836 RID: 6198 RVA: 0x00043B54 File Offset: 0x00041D54
			private static ConditionalWeakTable<ICommand, EventRegistrationTokenTable<EventHandler>> MakeConditionalWeakTable()
			{
				Interlocked.CompareExchange<ConditionalWeakTable<ICommand, EventRegistrationTokenTable<EventHandler>>>(ref ICommand.Vftbl._canExecuteChanged_TokenTables, new ConditionalWeakTable<ICommand, EventRegistrationTokenTable<EventHandler>>(), null);
				return ICommand.Vftbl._canExecuteChanged_TokenTables;
			}

			// Token: 0x1700049F RID: 1183
			// (get) Token: 0x06001837 RID: 6199 RVA: 0x00043B6E File Offset: 0x00041D6E
			private static ConditionalWeakTable<ICommand, EventRegistrationTokenTable<EventHandler>> _CanExecuteChanged_TokenTables
			{
				get
				{
					return ICommand.Vftbl._canExecuteChanged_TokenTables ?? ICommand.Vftbl.MakeConditionalWeakTable();
				}
			}

			// Token: 0x06001838 RID: 6200 RVA: 0x00043B80 File Offset: 0x00041D80
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_add_CanExecuteChanged_0(IntPtr thisPtr, IntPtr handler, EventRegistrationToken* token)
			{
				*token = default(EventRegistrationToken);
				int result;
				try
				{
					ICommand command = ComWrappersSupport.FindObject<ICommand>(thisPtr);
					EventHandler eventHandler = EventHandler.FromAbi(handler);
					*token = ICommand.Vftbl._CanExecuteChanged_TokenTables.GetOrCreateValue(command).AddEventHandler(eventHandler);
					command.CanExecuteChanged += eventHandler;
					result = 0;
				}
				catch (Exception ex)
				{
					result = ex.HResult;
				}
				return result;
			}

			// Token: 0x06001839 RID: 6201 RVA: 0x00043BE0 File Offset: 0x00041DE0
			[UnmanagedCallersOnly]
			private static int Do_Abi_remove_CanExecuteChanged_1(IntPtr thisPtr, EventRegistrationToken token)
			{
				int result;
				try
				{
					ICommand command = ComWrappersSupport.FindObject<ICommand>(thisPtr);
					EventRegistrationTokenTable<EventHandler> eventRegistrationTokenTable;
					EventHandler eventHandler;
					if (command != null && ICommand.Vftbl._CanExecuteChanged_TokenTables.TryGetValue(command, ref eventRegistrationTokenTable) && eventRegistrationTokenTable.RemoveEventHandler(token, ref eventHandler))
					{
						command.CanExecuteChanged -= eventHandler;
					}
					result = 0;
				}
				catch (Exception ex)
				{
					result = ex.HResult;
				}
				return result;
			}

			// Token: 0x0400088C RID: 2188
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400088D RID: 2189
			private unsafe void* _add_CanExecuteChanged_0;

			// Token: 0x0400088E RID: 2190
			private unsafe void* _remove_CanExecuteChanged_1;

			// Token: 0x0400088F RID: 2191
			private unsafe void* _CanExecute_2;

			// Token: 0x04000890 RID: 2192
			private unsafe void* _Execute_3;

			// Token: 0x04000891 RID: 2193
			private static readonly ICommand.Vftbl AbiToProjectionVftable = new ICommand.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_add_CanExecuteChanged_0 = ldftn(Do_Abi_add_CanExecuteChanged_0),
				_remove_CanExecuteChanged_1 = ldftn(Do_Abi_remove_CanExecuteChanged_1),
				_CanExecute_2 = ldftn(Do_Abi_CanExecute_2),
				_Execute_3 = ldftn(Do_Abi_Execute_3)
			};

			// Token: 0x04000892 RID: 2194
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x04000893 RID: 2195
			private static volatile ConditionalWeakTable<ICommand, EventRegistrationTokenTable<EventHandler>> _canExecuteChanged_TokenTables;
		}
	}
}
