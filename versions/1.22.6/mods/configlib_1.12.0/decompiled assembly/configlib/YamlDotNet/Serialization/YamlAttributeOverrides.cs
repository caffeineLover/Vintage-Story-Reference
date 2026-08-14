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
	// Token: 0x020000EB RID: 235
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlAttributeOverrides
	{
		// Token: 0x0600056C RID: 1388 RVA: 0x00013E80 File Offset: 0x00012080
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

		// Token: 0x0600056D RID: 1389 RVA: 0x00013F1C File Offset: 0x0001211C
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

		// Token: 0x0600056E RID: 1390 RVA: 0x00013FCC File Offset: 0x000121CC
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

		// Token: 0x0600056F RID: 1391 RVA: 0x0001407C File Offset: 0x0001227C
		public void Add<[Nullable(2)] TClass>(Expression<Func<TClass, object>> propertyAccessor, Attribute attribute)
		{
			PropertyInfo propertyInfo = propertyAccessor.AsProperty();
			this.Add(typeof(TClass), propertyInfo.Name, attribute);
		}

		// Token: 0x0400027A RID: 634
		private readonly Dictionary<YamlAttributeOverrides.AttributeKey, List<YamlAttributeOverrides.AttributeMapping>> overrides = new Dictionary<YamlAttributeOverrides.AttributeKey, List<YamlAttributeOverrides.AttributeMapping>>();

		// Token: 0x020000EC RID: 236
		[Nullable(0)]
		private struct AttributeKey
		{
			// Token: 0x06000571 RID: 1393 RVA: 0x000140BA File Offset: 0x000122BA
			public AttributeKey(Type attributeType, string propertyName)
			{
				this.AttributeType = attributeType;
				this.PropertyName = propertyName;
			}

			// Token: 0x06000572 RID: 1394 RVA: 0x000140CC File Offset: 0x000122CC
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

			// Token: 0x06000573 RID: 1395 RVA: 0x0001410E File Offset: 0x0001230E
			public override int GetHashCode()
			{
				return HashCode.CombineHashCodes(this.AttributeType.GetHashCode(), this.PropertyName.GetHashCode());
			}

			// Token: 0x0400027B RID: 635
			public readonly Type AttributeType;

			// Token: 0x0400027C RID: 636
			public readonly string PropertyName;
		}

		// Token: 0x020000ED RID: 237
		[Nullable(0)]
		private sealed class AttributeMapping
		{
			// Token: 0x06000574 RID: 1396 RVA: 0x0001412B File Offset: 0x0001232B
			public AttributeMapping(Type registeredType, Attribute attribute)
			{
				this.RegisteredType = registeredType;
				this.Attribute = attribute;
			}

			// Token: 0x06000575 RID: 1397 RVA: 0x00014144 File Offset: 0x00012344
			[NullableContext(2)]
			public override bool Equals(object obj)
			{
				YamlAttributeOverrides.AttributeMapping attributeMapping = obj as YamlAttributeOverrides.AttributeMapping;
				return attributeMapping != null && this.RegisteredType.Equals(attributeMapping.RegisteredType) && this.Attribute.Equals(attributeMapping.Attribute);
			}

			// Token: 0x06000576 RID: 1398 RVA: 0x00014181 File Offset: 0x00012381
			public override int GetHashCode()
			{
				return HashCode.CombineHashCodes(this.RegisteredType.GetHashCode(), this.Attribute.GetHashCode());
			}

			// Token: 0x06000577 RID: 1399 RVA: 0x000141A0 File Offset: 0x000123A0
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

			// Token: 0x0400027D RID: 637
			public readonly Type RegisteredType;

			// Token: 0x0400027E RID: 638
			public readonly Attribute Attribute;
		}
	}
}
