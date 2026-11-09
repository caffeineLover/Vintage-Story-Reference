using System;
using System.Reflection;

namespace ModernWpf
{
	// Token: 0x02000213 RID: 531
	internal static class DelegateHelper
	{
		// Token: 0x06000F9F RID: 3999 RVA: 0x0002C9B5 File Offset: 0x0002ABB5
		public static T CreateDelegate<T>(MethodInfo method) where T : Delegate
		{
			return (T)((object)Delegate.CreateDelegate(typeof(T), method));
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0002C9CC File Offset: 0x0002ABCC
		public static T CreateDelegate<T>(object firstArgument, MethodInfo method) where T : Delegate
		{
			return (T)((object)Delegate.CreateDelegate(typeof(T), firstArgument, method));
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0002C9E4 File Offset: 0x0002ABE4
		public static T CreateDelegate<T>(Type target, string method, BindingFlags bindingAttr = 28) where T : Delegate
		{
			if (bindingAttr == 28)
			{
				return (T)((object)Delegate.CreateDelegate(typeof(T), target, method));
			}
			MethodInfo method2 = target.GetMethod(method, bindingAttr);
			if (method2 != null)
			{
				return DelegateHelper.CreateDelegate<T>(method2);
			}
			return default(T);
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0002CA30 File Offset: 0x0002AC30
		public static T CreateDelegate<T>(object target, string method, BindingFlags bindingAttr = 28) where T : Delegate
		{
			if (bindingAttr == 28)
			{
				return (T)((object)Delegate.CreateDelegate(typeof(T), target, method));
			}
			MethodInfo method2 = target.GetType().GetMethod(method, bindingAttr);
			if (method2 != null)
			{
				return DelegateHelper.CreateDelegate<T>(target, method2);
			}
			return default(T);
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0002CA84 File Offset: 0x0002AC84
		public static Func<TType, TProperty> CreatePropertyGetter<TType, TProperty>(string name, BindingFlags bindingAttr = 28, bool nonPublic = false)
		{
			PropertyInfo property = typeof(TType).GetProperty(name, bindingAttr);
			if (property != null)
			{
				MethodInfo getMethod = property.GetGetMethod(nonPublic);
				if (getMethod != null)
				{
					return DelegateHelper.CreateDelegate<Func<TType, TProperty>>(getMethod);
				}
			}
			return null;
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0002CAC8 File Offset: 0x0002ACC8
		public static Action<TType, TProperty> CreatePropertySetter<TType, TProperty>(string name, BindingFlags bindingAttr = 28, bool nonPublic = false)
		{
			PropertyInfo property = typeof(TType).GetProperty(name, bindingAttr);
			if (property != null)
			{
				MethodInfo setMethod = property.GetSetMethod(nonPublic);
				if (setMethod != null)
				{
					return DelegateHelper.CreateDelegate<Action<TType, TProperty>>(setMethod);
				}
			}
			return null;
		}

		// Token: 0x04000401 RID: 1025
		private const BindingFlags DefaultLookup = 28;
	}
}
