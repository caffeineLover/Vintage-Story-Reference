using System;
using System.ComponentModel;
using ABI.Microsoft.UI.Xaml.Data;
using WinRT;

namespace ABI.System.ComponentModel
{
	// Token: 0x02000149 RID: 329
	[EditorBrowsable(1)]
	internal struct PropertyChangedEventArgs
	{
		// Token: 0x06000A86 RID: 2694 RVA: 0x0001D66C File Offset: 0x0001B86C
		public static IObjectReference CreateMarshaler(PropertyChangedEventArgs value)
		{
			if (value == null)
			{
				return null;
			}
			IObjectReference objectReference;
			return PropertyChangedEventArgs.ActivationFactory.Instance.CreateInstance(value.PropertyName, null, out objectReference);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0001D694 File Offset: 0x0001B894
		public static ObjectReferenceValue CreateMarshaler2(PropertyChangedEventArgs value)
		{
			if (value == null)
			{
				return default(ObjectReferenceValue);
			}
			return PropertyChangedEventArgs.ActivationFactory.Instance.CreateInstance(value.PropertyName);
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0001D6BE File Offset: 0x0001B8BE
		public static IntPtr GetAbi(IObjectReference m)
		{
			if (m == null)
			{
				return IntPtr.Zero;
			}
			return m.ThisPtr;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x0001D6D0 File Offset: 0x0001B8D0
		public unsafe static PropertyChangedEventArgs FromAbi(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}
			IntPtr zero = IntPtr.Zero;
			PropertyChangedEventArgs result;
			try
			{
				method get_PropertyName_ = ((IntPtr*)((void*)ptr))->get_PropertyName_0;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), ptr, &zero, get_PropertyName_));
				result = new PropertyChangedEventArgs(MarshalString.FromAbi(zero));
			}
			finally
			{
				MarshalString.DisposeAbi(zero);
			}
			return result;
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0001D734 File Offset: 0x0001B934
		public unsafe static void CopyManaged(PropertyChangedEventArgs o, IntPtr dest)
		{
			*(IntPtr*)dest.ToPointer() = PropertyChangedEventArgs.CreateMarshaler2(o).Detach();
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0001D758 File Offset: 0x0001B958
		public static IntPtr FromManaged(PropertyChangedEventArgs value)
		{
			if (value == null)
			{
				return IntPtr.Zero;
			}
			return PropertyChangedEventArgs.CreateMarshaler2(value).Detach();
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0001D77C File Offset: 0x0001B97C
		public static void DisposeMarshaler(IObjectReference m)
		{
			if (m != null)
			{
				m.Dispose();
			}
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0001D787 File Offset: 0x0001B987
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0001D78F File Offset: 0x0001B98F
		public static string GetGuidSignature()
		{
			return "rc(Microsoft.UI.Xaml.Data.NotifyPropertyChangedEventArgs;{4f33a9a0-5cf4-47a4-b16f-d7faaf17457e})";
		}

		// Token: 0x02000361 RID: 865
		private sealed class ActivationFactory : BaseActivationFactory
		{
			// Token: 0x0600186E RID: 6254 RVA: 0x000440BA File Offset: 0x000422BA
			public ActivationFactory() : base("Microsoft.UI.Xaml.Data", "Microsoft.UI.Xaml.Data.PropertyChangedEventArgs")
			{
			}

			// Token: 0x040008A5 RID: 2213
			internal static WinRTPropertyChangedEventArgsRuntimeClassFactory Instance = new PropertyChangedEventArgs.ActivationFactory()._As<WinRTPropertyChangedEventArgsRuntimeClassFactory.Vftbl>();
		}
	}
}
