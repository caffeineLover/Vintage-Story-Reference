using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Callbacks;

namespace YamlDotNet.Serialization.ObjectFactories
{
	// Token: 0x0200006D RID: 109
	[NullableContext(1)]
	[Nullable(0)]
	public class DefaultObjectFactory : ObjectFactoryBase
	{
		// Token: 0x060002A6 RID: 678 RVA: 0x00008FE7 File Offset: 0x000071E7
		public DefaultObjectFactory() : this(new Dictionary<Type, Type>(), new Settings())
		{
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00008FF9 File Offset: 0x000071F9
		public DefaultObjectFactory(IDictionary<Type, Type> mappings) : this(mappings, new Settings())
		{
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00009008 File Offset: 0x00007208
		public DefaultObjectFactory(IDictionary<Type, Type> mappings, Settings settings)
		{
			Dictionary<Type, ConcurrentDictionary<Type, MethodInfo[]>> dictionary = new Dictionary<Type, ConcurrentDictionary<Type, MethodInfo[]>>();
			dictionary.Add(typeof(OnDeserializedAttribute), new ConcurrentDictionary<Type, MethodInfo[]>());
			dictionary.Add(typeof(OnDeserializingAttribute), new ConcurrentDictionary<Type, MethodInfo[]>());
			dictionary.Add(typeof(OnSerializedAttribute), new ConcurrentDictionary<Type, MethodInfo[]>());
			dictionary.Add(typeof(OnSerializingAttribute), new ConcurrentDictionary<Type, MethodInfo[]>());
			this.stateMethods = dictionary;
			Dictionary<Type, Type> dictionary2 = new Dictionary<Type, Type>();
			dictionary2.Add(typeof(IEnumerable), typeof(List));
			dictionary2.Add(typeof(ICollection), typeof(List));
			dictionary2.Add(typeof(IList), typeof(List));
			dictionary2.Add(typeof(IDictionary), typeof(Dictionary));
			this.defaultGenericInterfaceImplementations = dictionary2;
			dictionary2 = new Dictionary<Type, Type>();
			dictionary2.Add(typeof(IEnumerable), typeof(List<object>));
			dictionary2.Add(typeof(ICollection), typeof(List<object>));
			dictionary2.Add(typeof(IList), typeof(List<object>));
			dictionary2.Add(typeof(IDictionary), typeof(Dictionary<object, object>));
			this.defaultNonGenericInterfaceImplementations = dictionary2;
			base..ctor();
			foreach (KeyValuePair<Type, Type> keyValuePair in mappings)
			{
				if (!keyValuePair.Key.IsAssignableFrom(keyValuePair.Value))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Type '");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(keyValuePair.Value);
					defaultInterpolatedStringHandler.AppendLiteral("' does not implement type '");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(keyValuePair.Key);
					defaultInterpolatedStringHandler.AppendLiteral("'.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.defaultNonGenericInterfaceImplementations.Add(keyValuePair.Key, keyValuePair.Value);
			}
			this.settings = settings;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000922C File Offset: 0x0000742C
		public override object Create(Type type)
		{
			if (type.IsInterface())
			{
				Type type3;
				if (type.IsGenericType())
				{
					Type type2;
					if (this.defaultGenericInterfaceImplementations.TryGetValue(type.GetGenericTypeDefinition(), ref type2))
					{
						type = type2.MakeGenericType(type.GetGenericArguments());
					}
				}
				else if (this.defaultNonGenericInterfaceImplementations.TryGetValue(type, ref type3))
				{
					type = type3;
				}
			}
			object result;
			try
			{
				result = Activator.CreateInstance(type, this.settings.AllowPrivateConstructors);
			}
			catch (Exception ex)
			{
				string text = "Failed to create an instance of type '" + type.FullName + "'.";
				throw new InvalidOperationException(text, ex);
			}
			return result;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x000092C8 File Offset: 0x000074C8
		public override void ExecuteOnDeserialized(object value)
		{
			this.ExecuteState(typeof(OnDeserializedAttribute), value);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x000092DB File Offset: 0x000074DB
		public override void ExecuteOnDeserializing(object value)
		{
			this.ExecuteState(typeof(OnDeserializingAttribute), value);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x000092EE File Offset: 0x000074EE
		public override void ExecuteOnSerialized(object value)
		{
			this.ExecuteState(typeof(OnSerializedAttribute), value);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00009301 File Offset: 0x00007501
		public override void ExecuteOnSerializing(object value)
		{
			this.ExecuteState(typeof(OnSerializingAttribute), value);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00009314 File Offset: 0x00007514
		private void ExecuteState(Type attributeType, object value)
		{
			if (value == null)
			{
				return;
			}
			Type type = value.GetType();
			MethodInfo[] array = this.GetStateMethods(attributeType, type);
			foreach (MethodInfo methodInfo in array)
			{
				methodInfo.Invoke(value, null);
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00009354 File Offset: 0x00007554
		private MethodInfo[] GetStateMethods(Type attributeType, Type valueType)
		{
			ConcurrentDictionary<Type, MethodInfo[]> concurrentDictionary = this.stateMethods[attributeType];
			Func<MethodInfo, bool> <>9__1;
			return concurrentDictionary.GetOrAdd(valueType, delegate(Type type)
			{
				MethodInfo[] methods = type.GetMethods(52);
				IEnumerable<MethodInfo> enumerable = methods;
				Func<MethodInfo, bool> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = ((MethodInfo x) => x.GetCustomAttributes(attributeType, true).Length != 0));
				}
				return Enumerable.ToArray<MethodInfo>(Enumerable.Where<MethodInfo>(enumerable, func));
			});
		}

		// Token: 0x040000A9 RID: 169
		private readonly Dictionary<Type, ConcurrentDictionary<Type, MethodInfo[]>> stateMethods;

		// Token: 0x040000AA RID: 170
		private readonly Dictionary<Type, Type> defaultGenericInterfaceImplementations;

		// Token: 0x040000AB RID: 171
		private readonly Dictionary<Type, Type> defaultNonGenericInterfaceImplementations;

		// Token: 0x040000AC RID: 172
		private readonly Settings settings;
	}
}
