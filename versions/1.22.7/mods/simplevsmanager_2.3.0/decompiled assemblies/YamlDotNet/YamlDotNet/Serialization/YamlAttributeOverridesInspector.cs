using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.TypeInspectors;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000040 RID: 64
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlAttributeOverridesInspector : ReflectionTypeInspector
	{
		// Token: 0x060001E1 RID: 481 RVA: 0x00006812 File Offset: 0x00004A12
		public YamlAttributeOverridesInspector(ITypeInspector innerTypeDescriptor, YamlAttributeOverrides overrides)
		{
			this.innerTypeDescriptor = innerTypeDescriptor;
			this.overrides = overrides;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00006828 File Offset: 0x00004A28
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			IEnumerable<IPropertyDescriptor> enumerable = this.innerTypeDescriptor.GetProperties(type, container);
			if (this.overrides != null)
			{
				enumerable = Enumerable.Select<IPropertyDescriptor, IPropertyDescriptor>(enumerable, (IPropertyDescriptor p) => new YamlAttributeOverridesInspector.OverridePropertyDescriptor(p, this.overrides, type));
			}
			return enumerable;
		}

		// Token: 0x0400006E RID: 110
		private readonly ITypeInspector innerTypeDescriptor;

		// Token: 0x0400006F RID: 111
		private readonly YamlAttributeOverrides overrides;

		// Token: 0x0200015C RID: 348
		[Nullable(0)]
		public sealed class OverridePropertyDescriptor : IPropertyDescriptor
		{
			// Token: 0x0600079B RID: 1947 RVA: 0x00018CD5 File Offset: 0x00016ED5
			public OverridePropertyDescriptor(IPropertyDescriptor baseDescriptor, YamlAttributeOverrides overrides, Type classType)
			{
				this.baseDescriptor = baseDescriptor;
				this.overrides = overrides;
				this.classType = classType;
			}

			// Token: 0x170000E7 RID: 231
			// (get) Token: 0x0600079C RID: 1948 RVA: 0x00018CF2 File Offset: 0x00016EF2
			public string Name
			{
				get
				{
					return this.baseDescriptor.Name;
				}
			}

			// Token: 0x170000E8 RID: 232
			// (get) Token: 0x0600079D RID: 1949 RVA: 0x00018CFF File Offset: 0x00016EFF
			public bool Required
			{
				get
				{
					return this.baseDescriptor.Required;
				}
			}

			// Token: 0x170000E9 RID: 233
			// (get) Token: 0x0600079E RID: 1950 RVA: 0x00018D0C File Offset: 0x00016F0C
			public bool AllowNulls
			{
				get
				{
					return this.baseDescriptor.AllowNulls;
				}
			}

			// Token: 0x170000EA RID: 234
			// (get) Token: 0x0600079F RID: 1951 RVA: 0x00018D19 File Offset: 0x00016F19
			public bool CanWrite
			{
				get
				{
					return this.baseDescriptor.CanWrite;
				}
			}

			// Token: 0x170000EB RID: 235
			// (get) Token: 0x060007A0 RID: 1952 RVA: 0x00018D26 File Offset: 0x00016F26
			public Type Type
			{
				get
				{
					return this.baseDescriptor.Type;
				}
			}

			// Token: 0x170000EC RID: 236
			// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00018D33 File Offset: 0x00016F33
			// (set) Token: 0x060007A2 RID: 1954 RVA: 0x00018D40 File Offset: 0x00016F40
			[Nullable(2)]
			public Type TypeOverride
			{
				[NullableContext(2)]
				get
				{
					return this.baseDescriptor.TypeOverride;
				}
				[NullableContext(2)]
				set
				{
					this.baseDescriptor.TypeOverride = value;
				}
			}

			// Token: 0x170000ED RID: 237
			// (get) Token: 0x060007A3 RID: 1955 RVA: 0x00018D4E File Offset: 0x00016F4E
			[Nullable(2)]
			public Type ConverterType
			{
				[NullableContext(2)]
				get
				{
					YamlConverterAttribute customAttribute = this.GetCustomAttribute<YamlConverterAttribute>();
					return ((customAttribute != null) ? customAttribute.ConverterType : null) ?? this.baseDescriptor.ConverterType;
				}
			}

			// Token: 0x170000EE RID: 238
			// (get) Token: 0x060007A4 RID: 1956 RVA: 0x00018D71 File Offset: 0x00016F71
			// (set) Token: 0x060007A5 RID: 1957 RVA: 0x00018D7E File Offset: 0x00016F7E
			public int Order
			{
				get
				{
					return this.baseDescriptor.Order;
				}
				set
				{
					this.baseDescriptor.Order = value;
				}
			}

			// Token: 0x170000EF RID: 239
			// (get) Token: 0x060007A6 RID: 1958 RVA: 0x00018D8C File Offset: 0x00016F8C
			// (set) Token: 0x060007A7 RID: 1959 RVA: 0x00018D99 File Offset: 0x00016F99
			public ScalarStyle ScalarStyle
			{
				get
				{
					return this.baseDescriptor.ScalarStyle;
				}
				set
				{
					this.baseDescriptor.ScalarStyle = value;
				}
			}

			// Token: 0x060007A8 RID: 1960 RVA: 0x00018DA7 File Offset: 0x00016FA7
			public void Write(object target, [Nullable(2)] object value)
			{
				this.baseDescriptor.Write(target, value);
			}

			// Token: 0x060007A9 RID: 1961 RVA: 0x00018DB8 File Offset: 0x00016FB8
			[NullableContext(0)]
			[return: Nullable(2)]
			public T GetCustomAttribute<T>() where T : Attribute
			{
				T attribute = this.overrides.GetAttribute<T>(this.classType, this.Name);
				T result;
				if ((result = attribute) == null)
				{
					result = this.baseDescriptor.GetCustomAttribute<T>();
				}
				return result;
			}

			// Token: 0x060007AA RID: 1962 RVA: 0x00018DF2 File Offset: 0x00016FF2
			public IObjectDescriptor Read(object target)
			{
				return this.baseDescriptor.Read(target);
			}

			// Token: 0x040002AB RID: 683
			private readonly IPropertyDescriptor baseDescriptor;

			// Token: 0x040002AC RID: 684
			private readonly YamlAttributeOverrides overrides;

			// Token: 0x040002AD RID: 685
			private readonly Type classType;
		}
	}
}
