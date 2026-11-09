using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Helpers;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000E7 RID: 231
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlAttributeOverrides
	{
		// Token: 0x06000530 RID: 1328 RVA: 0x00013538 File Offset: 0x00011738
		[return: MaybeNull]
		public T GetAttribute<[Nullable(0)] T>(Type type, string member) where T : Attribute
		{
			List<YamlAttributeOverrides.AttributeMapping> list;
			if (this.overrides.TryGetValue(new YamlAttributeOverrides.AttributeKey(typeof(T), member), ref list))
			{
				int num = 0;
				YamlAttributeOverrides.AttributeMapping attributeMapping = null;
				foreach (YamlAttributeOverrides.AttributeMapping attributeMapping2 in list)
				{
					int num2 = attributeMapping2.Matches(type);
					if (num2 > num)
					{
						num = num2;
						attributeMapping = attributeMapping2;
					}
				}
				if (num > 0)
				{
					return (T)((object)attributeMapping.Attribute);
				}
			}
			return default(T);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x000135D4 File Offset: 0x000117D4
		public void Add(Type type, string member, Attribute attribute)
		{
			YamlAttributeOverrides.AttributeMapping attributeMapping = new YamlAttributeOverrides.AttributeMapping(type, attribute);
			YamlAttributeOverrides.AttributeKey attributeKey = new YamlAttributeOverrides.AttributeKey(attribute.GetType(), member);
			List<YamlAttributeOverrides.AttributeMapping> list;
			if (!this.overrides.TryGetValue(attributeKey, ref list))
			{
				list = new List<YamlAttributeOverrides.AttributeMapping>();
				this.overrides.Add(attributeKey, list);
			}
			else if (list.Contains(attributeMapping))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(43, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Attribute (");
				defaultInterpolatedStringHandler.AppendFormatted<Attribute>(attribute);
				defaultInterpolatedStringHandler.AppendLiteral(") already set for Type ");
				defaultInterpolatedStringHandler.AppendFormatted(type.FullName);
				defaultInterpolatedStringHandler.AppendLiteral(", Member ");
				defaultInterpolatedStringHandler.AppendFormatted(member);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			list.Add(attributeMapping);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00013684 File Offset: 0x00011884
		public YamlAttributeOverrides Clone()
		{
			YamlAttributeOverrides yamlAttributeOverrides = new YamlAttributeOverrides();
			foreach (KeyValuePair<YamlAttributeOverrides.AttributeKey, List<YamlAttributeOverrides.AttributeMapping>> keyValuePair in this.overrides)
			{
				foreach (YamlAttributeOverrides.AttributeMapping attributeMapping in keyValuePair.Value)
				{
					yamlAttributeOverrides.Add(attributeMapping.RegisteredType, keyValuePair.Key.PropertyName, attributeMapping.Attribute);
				}
			}
			return yamlAttributeOverrides;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00013734 File Offset: 0x00011934
		public void Add<[Nullable(2)] TClass>(Expression<Func<TClass, object>> propertyAccessor, Attribute attribute)
		{
			PropertyInfo propertyInfo = propertyAccessor.AsProperty();
			this.Add(typeof(TClass), propertyInfo.Name, attribute);
		}

		// Token: 0x0400024C RID: 588
		private readonly Dictionary<YamlAttributeOverrides.AttributeKey, List<YamlAttributeOverrides.AttributeMapping>> overrides = new Dictionary<YamlAttributeOverrides.AttributeKey, List<YamlAttributeOverrides.AttributeMapping>>();

		// Token: 0x020000E8 RID: 232
		[Nullable(0)]
		private struct AttributeKey
		{
			// Token: 0x06000535 RID: 1333 RVA: 0x00013772 File Offset: 0x00011972
			public AttributeKey(Type attributeType, string propertyName)
			{
				this.AttributeType = attributeType;
				this.PropertyName = propertyName;
			}

			// Token: 0x06000536 RID: 1334 RVA: 0x00013784 File Offset: 0x00011984
			[NullableContext(2)]
			public override bool Equals(object obj)
			{
				if (obj is YamlAttributeOverrides.AttributeKey)
				{
					YamlAttributeOverrides.AttributeKey attributeKey = (YamlAttributeOverrides.AttributeKey)obj;
					if (this.AttributeType.Equals(attributeKey.AttributeType))
					{
						return this.PropertyName.Equals(attributeKey.PropertyName);
					}
				}
				return false;
			}

			// Token: 0x06000537 RID: 1335 RVA: 0x000137C6 File Offset: 0x000119C6
			public override int GetHashCode()
			{
				return HashCode.CombineHashCodes(this.AttributeType.GetHashCode(), this.PropertyName.GetHashCode());
			}

			// Token: 0x0400024D RID: 589
			public readonly Type AttributeType;

			// Token: 0x0400024E RID: 590
			public readonly string PropertyName;
		}

		// Token: 0x020000E9 RID: 233
		[Nullable(0)]
		private sealed class AttributeMapping
		{
			// Token: 0x06000538 RID: 1336 RVA: 0x000137E3 File Offset: 0x000119E3
			public AttributeMapping(Type registeredType, Attribute attribute)
			{
				this.RegisteredType = registeredType;
				this.Attribute = attribute;
			}

			// Token: 0x06000539 RID: 1337 RVA: 0x000137FC File Offset: 0x000119FC
			[NullableContext(2)]
			public override bool Equals(object obj)
			{
				YamlAttributeOverrides.AttributeMapping attributeMapping = obj as YamlAttributeOverrides.AttributeMapping;
				return attributeMapping != null && this.RegisteredType.Equals(attributeMapping.RegisteredType) && this.Attribute.Equals(attributeMapping.Attribute);
			}

			// Token: 0x0600053A RID: 1338 RVA: 0x00013839 File Offset: 0x00011A39
			public override int GetHashCode()
			{
				return HashCode.CombineHashCodes(this.RegisteredType.GetHashCode(), this.Attribute.GetHashCode());
			}

			// Token: 0x0600053B RID: 1339 RVA: 0x00013858 File Offset: 0x00011A58
			public int Matches(Type matchType)
			{
				int num = 0;
				Type type = matchType;
				while (type != null)
				{
					num++;
					if (type == this.RegisteredType)
					{
						return num;
					}
					type = type.BaseType();
				}
				if (Enumerable.Contains<Type>(matchType.GetInterfaces(), this.RegisteredType))
				{
					return num;
				}
				return 0;
			}

			// Token: 0x0400024F RID: 591
			public readonly Type RegisteredType;

			// Token: 0x04000250 RID: 592
			public readonly Attribute Attribute;
		}
	}
}
