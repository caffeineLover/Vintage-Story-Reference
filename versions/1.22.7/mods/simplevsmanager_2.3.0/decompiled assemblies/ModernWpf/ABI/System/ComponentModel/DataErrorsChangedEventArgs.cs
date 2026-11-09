using System;
using System.ComponentModel;
using ABI.Microsoft.UI.Xaml.Data;
using WinRT;

namespace ABI.System.ComponentModel
{
	// Token: 0x02000145 RID: 325
	[EditorBrowsable(1)]
	internal struct DataErrorsChangedEventArgs
	{
		// Token: 0x06000A73 RID: 2675 RVA: 0x0001D32A File Offset: 0x0001B52A
		public static IObjectReference CreateMarshaler(DataErrorsChangedEventArgs value)
		{
			if (value == null)
			{
				return null;
			}
			return DataErrorsChangedEventArgs.ActivationFactory.Instance.CreateInstance(value.PropertyName);
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0001D344 File Offset: 0x0001B544
		public static ObjectReferenceValue CreateMarshaler2(DataErrorsChangedEventArgs value)
		{
			if (value == null)
			{
				return default(ObjectReferenceValue);
			}
			return DataErrorsChangedEventArgs.ActivationFactory.Instance.CreateInstanceForMarshaling(value.PropertyName);
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0001D36E File Offset: 0x0001B56E
		public static IntPtr GetAbi(IObjectReference m)
		{
			if (m == null)
			{
				return IntPtr.Zero;
			}
			return m.ThisPtr;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0001D380 File Offset: 0x0001B580
		public unsafe static DataErrorsChangedEventArgs FromAbi(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}
			IntPtr zero = IntPtr.Zero;
			DataErrorsChangedEventArgs result;
			try
			{
				method get_PropertyName_ = ((IntPtr*)((void*)ptr))->get_PropertyName_0;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), ptr, &zero, get_PropertyName_));
				result = new DataErrorsChangedEventArgs(MarshalString.FromAbi(zero));
			}
			finally
			{
				MarshalString.DisposeAbi(zero);
			}
			return result;
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0001D3E4 File Offset: 0x0001B5E4
		public unsafe static void CopyManaged(DataErrorsChangedEventArgs o, IntPtr dest)
		{
			*(IntPtr*)dest.ToPointer() = DataErrorsChangedEventArgs.CreateMarshaler2(o).Detach();
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0001D408 File Offset: 0x0001B608
		public static IntPtr FromManaged(DataErrorsChangedEventArgs value)
		{
			if (value == null)
			{
				return IntPtr.Zero;
			}
			return DataErrorsChangedEventArgs.CreateMarshaler2(value).Detach();
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0001D42C File Offset: 0x0001B62C
		public static void DisposeMarshaler(IObjectReference m)
		{
			if (m != null)
			{
				m.Dispose();
			}
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0001D437 File Offset: 0x0001B637
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0001D43F File Offset: 0x0001B63F
		public static string GetGuidSignature()
		{
			return "rc(Microsoft.UI.Xaml.Data.DataErrorsChangedEventArgs;{d026dd64-5f26-5f15-a86a-0dec8a431796})";
		}

		// Token: 0x02000358 RID: 856
		private sealed class ActivationFactory : BaseActivationFactory
		{
			// Token: 0x06001848 RID: 6216 RVA: 0x00043C7E File Offset: 0x00041E7E
			public ActivationFactory() : base("Microsoft.UI.Xaml.Data", "Microsoft.UI.Xaml.Data.DataErrorsChangedEventArgs")
			{
			}

			// Token: 0x04000895 RID: 2197
			internal static WinRTDataErrorsChangedEventArgsRuntimeClassFactory Instance = new DataErrorsChangedEventArgs.ActivationFactory()._As<WinRTDataErrorsChangedEventArgsRuntimeClassFactory.Vftbl>();
		}
	}
}
