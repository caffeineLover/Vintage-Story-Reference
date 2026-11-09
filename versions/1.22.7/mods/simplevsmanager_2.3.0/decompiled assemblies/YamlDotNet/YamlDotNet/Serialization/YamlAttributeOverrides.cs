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
	// Token: 0x0200003F RID: 63
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlAttributeOverrides
	{
		// Token: 0x060001DC RID: 476 RVA: 0x000065D8 File Offset: 0x000047D8
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

		// Token: 0x060001DD RID: 477 RVA: 0x00006674 File Offset: 0x00004874
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

		// Token: 0x060001DE RID: 478 RVA: 0x00006724 File Offset: 0x00004924
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

		// Token: 0x060001DF RID: 479 RVA: 0x000067D4 File Offset: 0x000049D4
		public void Add<[Nullable(2)] TClass>(Expression<Func<TClass, object>> propertyAccessor, Attribute attribute)
		{
			PropertyInfo propertyInfo = propertyAccessor.AsProperty();
			this.Add(typeof(TClass), propertyInfo.Name, attribute);
		}

		// Token: 0x0400006D RID: 109
		private readonly Dictionary<YamlAttributeOverrides.AttributeKey, List<YamlAttributeOverrides.AttributeMapping>> overrides = new Dictionary<YamlAttributeOverrides.AttributeKey, List<YamlAttributeOverrides.AttributeMapping>>();

		// Token: 0x0200015A RID: 346
		[Nullable(0)]
		private readonly struct AttributeKey
		{
			// Token: 0x06000794 RID: 1940 RVA: 0x00018BA1 File Offset: 0x00016DA1
			public AttributeKey(Type attributeType, string propertyName)
			{
				this.AttributeType = attributeType;
				this.PropertyName = propertyName;
			}

			// Token: 0x06000795 RID: 1941 RVA: 0x00018BB4 File Offset: 0x00016DB4
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

			// Token: 0x06000796 RID: 1942 RVA: 0x00018BF6 File Offset: 0x00016DF6
			public override int GetHashCode()
			{
				return HashCode.CombineHashCodes(this.AttributeType.GetHashCode(), this.PropertyName.GetHashCode());
			}

			// Token: 0x040002A7 RID: 679
			public readonly Type AttributeType;

			// Token: 0x040002A8 RID: 680
			public readonly string PropertyName;
		}

		// Token: 0x0200015B RID: 347
		[Nullable(0)]
		private sealed class AttributeMapping
		{
			// Token: 0x06000797 RID: 1943 RVA: 0x00018C13 File Offset: 0x00016E13
			public AttributeMapping(Type registeredType, Attribute attribute)
			{
				this.RegisteredType = registeredType;
				this.Attribute = attribute;
			}

			// Token: 0x06000798 RID: 1944 RVA: 0x00018C2C File Offset: 0x00016E2C
			[NullableContext(2)]
			public override bool Equals(object obj)
			{
				YamlAttributeOverrides.AttributeMapping attributeMapping = obj as YamlAttributeOverrides.AttributeMapping;
				return attributeMapping != null && this.RegisteredType.Equals(attributeMapping.RegisteredType) && this.Attribute.Equals(attributeMapping.Attribute);
			}

			// Token: 0x06000799 RID: 1945 RVA: 0x00018C69 File Offset: 0x00016E69
			public override int GetHashCode()
			{
				return HashCode.CombineHashCodes(this.RegisteredType.GetHashCode(), this.Attribute.GetHashCode());
			}

			// Token: 0x0600079A RID: 1946 RVA: 0x00018C88 File Offset: 0x00016E88
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

			// Token: 0x040002A9 RID: 681
			public readonly Type RegisteredType;

			// Token: 0x040002AA RID: 682
			public readonly Attribute Attribute;
		}
	}
}
