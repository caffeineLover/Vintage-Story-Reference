using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using ABI.Microsoft.UI.Xaml.Interop;
using WinRT;

namespace ABI.System.Collections.Specialized
{
	// Token: 0x02000152 RID: 338
	[EditorBrowsable(1)]
	internal struct NotifyCollectionChangedEventArgs
	{
		// Token: 0x06000AC4 RID: 2756 RVA: 0x0001E0B8 File Offset: 0x0001C2B8
		public static IObjectReference CreateMarshaler(NotifyCollectionChangedEventArgs value)
		{
			if (value == null)
			{
				return null;
			}
			IObjectReference objectReference;
			return NotifyCollectionChangedEventArgs.ActivationFactory.Instance.CreateInstanceWithAllParameters(value.Action, value.NewItems, value.OldItems, value.NewStartingIndex, value.OldStartingIndex, null, out objectReference);
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0001E0F8 File Offset: 0x0001C2F8
		public static ObjectReferenceValue CreateMarshaler2(NotifyCollectionChangedEventArgs value)
		{
			if (value == null)
			{
				return default(ObjectReferenceValue);
			}
			return NotifyCollectionChangedEventArgs.ActivationFactory.Instance.CreateInstanceWithAllParameters(value.Action, value.NewItems, value.OldItems, value.NewStartingIndex, value.OldStartingIndex);
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0001E13A File Offset: 0x0001C33A
		public static IntPtr GetAbi(IObjectReference m)
		{
			if (m == null)
			{
				return IntPtr.Zero;
			}
			return m.ThisPtr;
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x0001E14C File Offset: 0x0001C34C
		public static NotifyCollectionChangedEventArgs FromAbi(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}
			INotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs = INotifyCollectionChangedEventArgs.FromAbi(ptr);
			return NotifyCollectionChangedEventArgs.CreateNotifyCollectionChangedEventArgs(notifyCollectionChangedEventArgs.Action, notifyCollectionChangedEventArgs.NewItems, notifyCollectionChangedEventArgs.OldItems, notifyCollectionChangedEventArgs.NewStartingIndex, notifyCollectionChangedEventArgs.OldStartingIndex);
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x0001E198 File Offset: 0x0001C398
		private static NotifyCollectionChangedEventArgs CreateNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int newStartingIndex, int oldStartingIndex)
		{
			NotifyCollectionChangedEventArgs result;
			switch (action)
			{
			case 0:
				result = new NotifyCollectionChangedEventArgs(action, newItems, newStartingIndex);
				break;
			case 1:
				result = new NotifyCollectionChangedEventArgs(action, oldItems, oldStartingIndex);
				break;
			case 2:
				result = new NotifyCollectionChangedEventArgs(action, newItems, oldItems, newStartingIndex);
				break;
			case 3:
				result = new NotifyCollectionChangedEventArgs(action, newItems, newStartingIndex, oldStartingIndex);
				break;
			case 4:
				result = new NotifyCollectionChangedEventArgs(4);
				break;
			default:
				throw new ArgumentException();
			}
			return result;
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x0001E204 File Offset: 0x0001C404
		public unsafe static void CopyManaged(NotifyCollectionChangedEventArgs o, IntPtr dest)
		{
			*(IntPtr*)dest.ToPointer() = NotifyCollectionChangedEventArgs.CreateMarshaler2(o).Detach();
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0001E228 File Offset: 0x0001C428
		public static IntPtr FromManaged(NotifyCollectionChangedEventArgs value)
		{
			if (value == null)
			{
				return IntPtr.Zero;
			}
			return NotifyCollectionChangedEventArgs.CreateMarshaler2(value).Detach();
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0001E24C File Offset: 0x0001C44C
		public static void DisposeMarshaler(IObjectReference m)
		{
			if (m != null)
			{
				m.Dispose();
			}
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0001E257 File Offset: 0x0001C457
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0001E25F File Offset: 0x0001C45F
		public static string GetGuidSignature()
		{
			return "rc(Microsoft.UI.Xaml.Interop.NotifyCollectionChangedEventArgs;{4cf68d33-e3f2-4964-b85e-945b4f7e2f21})";
		}

		// Token: 0x0200037A RID: 890
		private sealed class ActivationFactory : BaseActivationFactory
		{
			// Token: 0x06001907 RID: 6407 RVA: 0x0004504E File Offset: 0x0004324E
			public ActivationFactory() : base("Microsoft.UI.Xaml.Interop", "Microsoft.UI.Xaml.Interop.NotifyCollectionChangedEventArgs")
			{
			}

			// Token: 0x040008CA RID: 2250
			internal static WinRTNotifyCollectionChangedEventArgsRuntimeClassFactory Instance = new NotifyCollectionChangedEventArgs.ActivationFactory()._As<WinRTNotifyCollectionChangedEventArgsRuntimeClassFactory.Vftbl>();
		}
	}
}
