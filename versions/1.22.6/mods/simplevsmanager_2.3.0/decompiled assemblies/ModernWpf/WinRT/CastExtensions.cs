using System;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x02000188 RID: 392
	internal static class CastExtensions
	{
		// Token: 0x06000C46 RID: 3142 RVA: 0x00021C10 File Offset: 0x0001FE10
		public static TInterface As<TInterface>(this object value)
		{
			IObjectReference objectReference;
			if (typeof(TInterface) == typeof(object) && CastExtensions.TryGetRefForObject(value, false, out objectReference))
			{
				using (objectReference)
				{
					return ComWrappersSupport.CreateRcwForComObject<TInterface>(objectReference.ThisPtr);
				}
			}
			if (value is TInterface)
			{
				return (TInterface)((object)value);
			}
			TInterface result;
			using (IObjectReference refForObject = CastExtensions.GetRefForObject(value))
			{
				result = refForObject.AsInterface<TInterface>();
			}
			return result;
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00021CAC File Offset: 0x0001FEAC
		public static AgileReference<T> AsAgile<T>(this T value) where T : class
		{
			if (value == null)
			{
				return new AgileReference<T>(null);
			}
			object obj = Marshaler<T>.CreateMarshaler2.Invoke(value);
			try
			{
				IObjectReference objectReference = obj as IObjectReference;
				if (objectReference != null)
				{
					return new AgileReference<T>(objectReference);
				}
				if (obj is ObjectReferenceValue)
				{
					ObjectReferenceValue objectReferenceValue = (ObjectReferenceValue)obj;
					return new AgileReference<T>(ref objectReferenceValue);
				}
			}
			finally
			{
				Marshaler<T>.DisposeMarshaler.Invoke(obj);
			}
			throw new InvalidOperationException("Object type is not a projected type: value.");
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x00021D2C File Offset: 0x0001FF2C
		private static bool TryGetRefForObject(object value, bool allowComposed, out IObjectReference reference)
		{
			IObjectReference objectReference;
			if (ComWrappersSupport.TryUnwrapObject(value, out objectReference))
			{
				reference = objectReference.As<IUnknownVftbl>();
				return true;
			}
			if (allowComposed && CastExtensions.TryGetComposedRefForQI(value, out objectReference))
			{
				reference = objectReference.As<IUnknownVftbl>();
				return true;
			}
			reference = null;
			return false;
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00021D68 File Offset: 0x0001FF68
		private static IObjectReference GetRefForObject(object value)
		{
			IObjectReference result;
			if (!CastExtensions.TryGetRefForObject(value, true, out result))
			{
				throw new ArgumentException("Source object type is not a projected type and does not inherit from a projected type.", "value");
			}
			return result;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x00021D94 File Offset: 0x0001FF94
		private static bool TryGetComposedRefForQI(object value, out IObjectReference objRef)
		{
			IWinRTObject winRTObject = value as IWinRTObject;
			if (winRTObject != null)
			{
				objRef = winRTObject.NativeObject;
				return true;
			}
			objRef = null;
			return false;
		}
	}
}
