using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.TypeInspectors;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000EE RID: 238
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlAttributeOverridesInspector : TypeInspectorSkeleton
	{
		// Token: 0x06000578 RID: 1400 RVA: 0x000141ED File Offset: 0x000123ED
		public YamlAttributeOverridesInspector(ITypeInspector innerTypeDescriptor, YamlAttributeOverrides overrides)
		{
			this.innerTypeDescriptor = innerTypeDescriptor;
			this.overrides = overrides;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00014204 File Offset: 0x00012404
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			IEnumerable<IPropertyDescriptor> enumerable = this.innerTypeDescriptor.GetProperties(type, container);
			if (this.overrides != null)
			{
				enumerable = Enumerable.Select<IPropertyDescriptor, IPropertyDescriptor>(enumerable, (IPropertyDescriptor p) => new YamlAttributeOverridesInspector.OverridePropertyDescriptor(p, this.overrides, type));
			}
			return enumerable;
		}

		// Token: 0x0400027F RID: 639
		private readonly ITypeInspector innerTypeDescriptor;

		// Token: 0x04000280 RID: 640
		private readonly YamlAttributeOverrides overrides;

		// Token: 0x020000EF RID: 239
		[Nullable(0)]
		public sealed class OverridePropertyDescriptor : IPropertyDescriptor
		{
			// Token: 0x0600057A RID: 1402 RVA: 0x00014254 File Offset: 0x00012454
			public OverridePropertyDescriptor(IPropertyDescriptor baseDescriptor, YamlAttributeOverrides overrides, Type classType)
			{
				this.baseDescriptor = baseDescriptor;
				this.overrides = overrides;
				this.classType = classType;
			}

			// Token: 0x1700008F RID: 143
			// (get) Token: 0x0600057B RID: 1403 RVA: 0x00014271 File Offset: 0x00012471
			public string Name
			{
				get
				{
					return this.baseDescriptor.Name;
				}
			}

			// Token: 0x17000090 RID: 144
			// (get) Token: 0x0600057C RID: 1404 RVA: 0x0001427E File Offset: 0x0001247E
			public bool CanWrite
			{
				get
				{
					return this.baseDescriptor.CanWrite;
				}
			}

			// Token: 0x17000091 RID: 145
			// (get) Token: 0x0600057D RID: 1405 RVA: 0x0001428B File Offset: 0x0001248B
			public Type Type
			{
				get
				{
					return this.baseDescriptor.Type;
				}
			}

			// Token: 0x17000092 RID: 146
			// (get) Token: 0x0600057E RID: 1406 RVA: 0x00014298 File Offset: 0x00012498
			// (set) Token: 0x0600057F RID: 1407 RVA: 0x000142A5 File Offset: 0x000124A5
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
			// (get) Token: 0x06000580 RID: 1408 RVA: 0x000142B3 File Offset: 0x000124B3
			// (set) Token: 0x06000581 RID: 1409 RVA: 0x000142C0 File Offset: 0x000124C0
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
			// (get) Token: 0x06000582 RID: 1410 RVA: 0x000142CE File Offset: 0x000124CE
			// (set) Token: 0x06000583 RID: 1411 RVA: 0x000142DB File Offset: 0x000124DB
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

			// Token: 0x06000584 RID: 1412 RVA: 0x000142E9 File Offset: 0x000124E9
			public void Write(object target, [Nullable(2)] object value)
			{
				this.baseDescriptor.Write(target, value);
			}

			// Token: 0x06000585 RID: 1413 RVA: 0x000142F8 File Offset: 0x000124F8
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

			// Token: 0x06000586 RID: 1414 RVA: 0x00014325 File Offset: 0x00012525
			public IObjectDescriptor Read(object target)
			{
				return this.baseDescriptor.Read(target);
			}

			// Token: 0x04000281 RID: 641
			private readonly IPropertyDescriptor baseDescriptor;

			// Token: 0x04000282 RID: 642
			private readonly YamlAttributeOverrides overrides;

			// Token: 0x04000283 RID: 643
			private readonly Type classType;
		}
	}
}
