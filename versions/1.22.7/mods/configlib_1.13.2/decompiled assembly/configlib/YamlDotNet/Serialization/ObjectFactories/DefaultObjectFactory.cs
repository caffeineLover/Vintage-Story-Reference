using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Callbacks;

namespace YamlDotNet.Serialization.ObjectFactories
{
	// Token: 0x02000130 RID: 304
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefaultObjectFactory : ObjectFactoryBase
	{
		// Token: 0x06000658 RID: 1624 RVA: 0x00016338 File Offset: 0x00014538
		public DefaultObjectFactory() : this(new Dictionary<Type, Type>(), new Settings())
		{
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0001634A File Offset: 0x0001454A
		public DefaultObjectFactory(IDictionary<Type, Type> mappings) : this(mappings, new Settings())
		{
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00016358 File Offset: 0x00014558
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

		// Token: 0x0600065B RID: 1627 RVA: 0x00016578 File Offset: 0x00014778
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

		// Token: 0x0600065C RID: 1628 RVA: 0x00016610 File Offset: 0x00014810
		public override void ExecuteOnDeserialized(object value)
		{
			this.ExecuteState(typeof(OnDeserializedAttribute), value);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00016623 File Offset: 0x00014823
		public override void ExecuteOnDeserializing(object value)
		{
			this.ExecuteState(typeof(OnDeserializingAttribute), value);
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00016636 File Offset: 0x00014836
		public override void ExecuteOnSerialized(object value)
		{
			this.ExecuteState(typeof(OnSerializedAttribute), value);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00016649 File Offset: 0x00014849
		public override void ExecuteOnSerializing(object value)
		{
			this.ExecuteState(typeof(OnSerializingAttribute), value);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0001665C File Offset: 0x0001485C
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

		// Token: 0x06000661 RID: 1633 RVA: 0x00016698 File Offset: 0x00014898
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

		// Token: 0x040002C4 RID: 708
		private readonly Dictionary<Type, Dictionary<Type, MethodInfo[]>> _stateMethods;

		// Token: 0x040002C5 RID: 709
		private readonly Dictionary<Type, Type> DefaultGenericInterfaceImplementations;

		// Token: 0x040002C6 RID: 710
		private readonly Dictionary<Type, Type> DefaultNonGenericInterfaceImplementations;

		// Token: 0x040002C7 RID: 711
		private readonly Settings settings;
	}
}
