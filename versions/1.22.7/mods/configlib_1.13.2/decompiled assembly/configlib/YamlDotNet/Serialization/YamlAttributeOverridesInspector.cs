using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.TypeInspectors;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000EA RID: 234
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlAttributeOverridesInspector : TypeInspectorSkeleton
	{
		// Token: 0x0600053C RID: 1340 RVA: 0x000138A5 File Offset: 0x00011AA5
		public YamlAttributeOverridesInspector(ITypeInspector innerTypeDescriptor, YamlAttributeOverrides overrides)
		{
			this.innerTypeDescriptor = innerTypeDescriptor;
			this.overrides = overrides;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x000138BC File Offset: 0x00011ABC
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			IEnumerable<IPropertyDescriptor> enumerable = this.innerTypeDescriptor.GetProperties(type, container);
			if (this.overrides != null)
			{
				enumerable = Enumerable.Select<IPropertyDescriptor, IPropertyDescriptor>(enumerable, (IPropertyDescriptor p) => new YamlAttributeOverridesInspector.OverridePropertyDescriptor(p, this.overrides, type));
			}
			return enumerable;
		}

		// Token: 0x04000251 RID: 593
		private readonly ITypeInspector innerTypeDescriptor;

		// Token: 0x04000252 RID: 594
		private readonly YamlAttributeOverrides overrides;

		// Token: 0x020000EB RID: 235
		[Nullable(0)]
		public sealed class OverridePropertyDescriptor : IPropertyDescriptor
		{
			// Token: 0x0600053E RID: 1342 RVA: 0x0001390C File Offset: 0x00011B0C
			public OverridePropertyDescriptor(IPropertyDescriptor baseDescriptor, YamlAttributeOverrides overrides, Type classType)
			{
				this.baseDescriptor = baseDescriptor;
				this.overrides = overrides;
				this.classType = classType;
			}

			// Token: 0x1700008F RID: 143
			// (get) Token: 0x0600053F RID: 1343 RVA: 0x00013929 File Offset: 0x00011B29
			public string Name
			{
				get
				{
					return this.baseDescriptor.Name;
				}
			}

			// Token: 0x17000090 RID: 144
			// (get) Token: 0x06000540 RID: 1344 RVA: 0x00013936 File Offset: 0x00011B36
			public bool CanWrite
			{
				get
				{
					return this.baseDescriptor.CanWrite;
				}
			}

			// Token: 0x17000091 RID: 145
			// (get) Token: 0x06000541 RID: 1345 RVA: 0x00013943 File Offset: 0x00011B43
			public Type Type
			{
				get
				{
					return this.baseDescriptor.Type;
				}
			}

			// Token: 0x17000092 RID: 146
			// (get) Token: 0x06000542 RID: 1346 RVA: 0x00013950 File Offset: 0x00011B50
			// (set) Token: 0x06000543 RID: 1347 RVA: 0x0001395D File Offset: 0x00011B5D
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

			// Token: 0x17000093 RID: 147
			// (get) Token: 0x06000544 RID: 1348 RVA: 0x0001396B File Offset: 0x00011B6B
			// (set) Token: 0x06000545 RID: 1349 RVA: 0x00013978 File Offset: 0x00011B78
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

			// Token: 0x17000094 RID: 148
			// (get) Token: 0x06000546 RID: 1350 RVA: 0x00013986 File Offset: 0x00011B86
			// (set) Token: 0x06000547 RID: 1351 RVA: 0x00013993 File Offset: 0x00011B93
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

			// Token: 0x06000548 RID: 1352 RVA: 0x000139A1 File Offset: 0x00011BA1
			public void Write(object target, [Nullable(2)] object value)
			{
				this.baseDescriptor.Write(target, value);
			}

			// Token: 0x06000549 RID: 1353 RVA: 0x000139B0 File Offset: 0x00011BB0
			[NullableContext(0)]
			[return: Nullable(2)]
			public T GetCustomAttribute<T>() where T : Attribute
			{
				T result;
				if ((result = this.overrides.GetAttribute<T>(this.classType, this.Name)) == null)
				{
					result = this.baseDescriptor.GetCustomAttribute<T>();
				}
				return result;
			}

			// Token: 0x0600054A RID: 1354 RVA: 0x000139DD File Offset: 0x00011BDD
			public IObjectDescriptor Read(object target)
			{
				return this.baseDescriptor.Read(target);
			}

			// Token: 0x04000253 RID: 595
			private readonly IPropertyDescriptor baseDescriptor;

			// Token: 0x04000254 RID: 596
			private readonly YamlAttributeOverrides overrides;

			// Token: 0x04000255 RID: 597
			private readonly Type classType;
		}
	}
}
