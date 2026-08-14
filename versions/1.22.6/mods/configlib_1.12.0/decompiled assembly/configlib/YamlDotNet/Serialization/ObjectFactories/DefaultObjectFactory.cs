using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Callbacks;

namespace YamlDotNet.Serialization.ObjectFactories
{
	// Token: 0x02000134 RID: 308
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefaultObjectFactory : ObjectFactoryBase
	{
		// Token: 0x06000694 RID: 1684 RVA: 0x00016C80 File Offset: 0x00014E80
		public DefaultObjectFactory() : this(new Dictionary<Type, Type>(), new Settings())
		{
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00016C92 File Offset: 0x00014E92
		public DefaultObjectFactory(IDictionary<Type, Type> mappings) : this(mappings, new Settings())
		{
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00016CA0 File Offset: 0x00014EA0
		public DefaultObjectFactory(IDictionary<Type, Type> mappings, Settings settings)
		{
			Dictionary<Type, Dictionary<Type, MethodInfo[]>> dictionary = new Dictionary<Type, Dictionary<Type, MethodInfo[]>>();
			dictionary.Add(typeof(OnDeserializedAttribute), new Dictionary<Type, MethodInfo[]>());
			dictionary.Add(typeof(OnDeserializingAttribute), new Dictionary<Type, MethodInfo[]>());
			dictionary.Add(typeof(OnSerializedAttribute), new Dictionary<Type, MethodInfo[]>());
			dictionary.Add(typeof(OnSerializingAttribute), new Dictionary<Type, MethodInfo[]>());
			this._stateMethods = dictionary;
			Dictionary<Type, Type> dictionary2 = new Dictionary<Type, Type>();
			dictionary2.Add(typeof(IEnumerable), typeof(List));
			dictionary2.Add(typeof(ICollection), typeof(List));
			dictionary2.Add(typeof(IList), typeof(List));
			dictionary2.Add(typeof(IDictionary), typeof(Dictionary));
			this.DefaultGenericInterfaceImplementations = dictionary2;
			Dictionary<Type, Type> dictionary3 = new Dictionary<Type, Type>();
			dictionary3.Add(typeof(IEnumerable), typeof(List<object>));
			dictionary3.Add(typeof(ICollection), typeof(List<object>));
			dictionary3.Add(typeof(IList), typeof(List<object>));
			dictionary3.Add(typeof(IDictionary), typeof(Dictionary<object, object>));
			this.DefaultNonGenericInterfaceImplementations = dictionary3;
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
				this.DefaultNonGenericInterfaceImplementations.Add(keyValuePair.Key, keyValuePair.Value);
			}
			this.settings = settings;
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00016EC0 File Offset: 0x000150C0
		public override object Create(Type type)
		{
			if (type.IsInterface())
			{
				Type type3;
				if (type.IsGenericType())
				{
					Type type2;
					if (this.DefaultGenericInterfaceImplementations.TryGetValue(type.GetGenericTypeDefinition(), ref type2))
					{
						type = type2.MakeGenericType(type.GetGenericArguments());
					}
				}
				else if (this.DefaultNonGenericInterfaceImplementations.TryGetValue(type, ref type3))
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
				throw new InvalidOperationException("Failed to create an instance of type '" + type.FullName + "'.", ex);
			}
			return result;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00016F58 File Offset: 0x00015158
		public override void ExecuteOnDeserialized(object value)
		{
			this.ExecuteState(typeof(OnDeserializedAttribute), value);
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00016F6B File Offset: 0x0001516B
		public override void ExecuteOnDeserializing(object value)
		{
			this.ExecuteState(typeof(OnDeserializingAttribute), value);
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00016F7E File Offset: 0x0001517E
		public override void ExecuteOnSerialized(object value)
		{
			this.ExecuteState(typeof(OnSerializedAttribute), value);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00016F91 File Offset: 0x00015191
		public override void ExecuteOnSerializing(object value)
		{
			this.ExecuteState(typeof(OnSerializingAttribute), value);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00016FA4 File Offset: 0x000151A4
		private void ExecuteState(Type attributeType, object value)
		{
			if (value == null)
			{
				return;
			}
			Type type = value.GetType();
			MethodInfo[] stateMethods = this.GetStateMethods(attributeType, type);
			for (int i = 0; i < stateMethods.Length; i++)
			{
				stateMethods[i].Invoke(value, null);
			}
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00016FE0 File Offset: 0x000151E0
		private MethodInfo[] GetStateMethods(Type attributeType, Type valueType)
		{
			Dictionary<Type, MethodInfo[]> dictionary = this._stateMethods[attributeType];
			MethodInfo[] array;
			if (dictionary.TryGetValue(valueType, ref array))
			{
				return array;
			}
			array = valueType.GetMethods(52);
			array = Enumerable.ToArray<MethodInfo>(Enumerable.Where<MethodInfo>(array, (MethodInfo x) => Enumerable.Any<object>(x.GetCustomAttributes(attributeType, true))));
			dictionary[valueType] = array;
			return array;
		}

		// Token: 0x040002F2 RID: 754
		private readonly Dictionary<Type, Dictionary<Type, MethodInfo[]>> _stateMethods;

		// Token: 0x040002F3 RID: 755
		private readonly Dictionary<Type, Type> DefaultGenericInterfaceImplementations;

		// Token: 0x040002F4 RID: 756
		private readonly Dictionary<Type, Type> DefaultNonGenericInterfaceImplementations;

		// Token: 0x040002F5 RID: 757
		private readonly Settings settings;
	}
}
