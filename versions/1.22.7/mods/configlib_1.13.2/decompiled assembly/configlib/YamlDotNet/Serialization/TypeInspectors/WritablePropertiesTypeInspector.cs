using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x02000116 RID: 278
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WritablePropertiesTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x060005EF RID: 1519 RVA: 0x00015283 File Offset: 0x00013483
		public WritablePropertiesTypeInspector(ITypeResolver typeResolver) : this(typeResolver, false)
		{
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0001528D File Offset: 0x0001348D
		public WritablePropertiesTypeInspector(ITypeResolver typeResolver, bool includeNonPublicProperties)
		{
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			this.includeNonPublicProperties = includeNonPublicProperties;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x000152B2 File Offset: 0x000134B2
		private static bool IsValidProperty(PropertyInfo property)
		{
			return property.CanWrite && property.GetSetMethod(true).GetParameters().Length == 1;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x000152CF File Offset: 0x000134CF
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return Enumerable.ToArray<IPropertyDescriptor>(Enumerable.Select<PropertyInfo, IPropertyDescriptor>(Enumerable.Where<PropertyInfo>(type.GetProperties(this.includeNonPublicProperties), new Func<PropertyInfo, bool>(WritablePropertiesTypeInspector.IsValidProperty)), (PropertyInfo p) => new WritablePropertiesTypeInspector.ReflectionPropertyDescriptor(p, this.typeResolver)));
		}

		// Token: 0x0400029C RID: 668
		private readonly ITypeResolver typeResolver;

		// Token: 0x0400029D RID: 669
		private readonly bool includeNonPublicProperties;

		// Token: 0x02000117 RID: 279
		[Nullable(0)]
		private sealed class ReflectionPropertyDescriptor : IPropertyDescriptor
		{
			// Token: 0x060005F4 RID: 1524 RVA: 0x00015312 File Offset: 0x00013512
			public ReflectionPropertyDescriptor(PropertyInfo propertyInfo, ITypeResolver typeResolver)
			{
				if (propertyInfo == null)
				{
					throw new ArgumentNullException("propertyInfo");
				}
				this.propertyInfo = propertyInfo;
				if (typeResolver == null)
				{
					throw new ArgumentNullException("typeResolver");
				}
				this.typeResolver = typeResolver;
				this.ScalarStyle = ScalarStyle.Any;
			}

			// Token: 0x170000B0 RID: 176
			// (get) Token: 0x060005F5 RID: 1525 RVA: 0x0001534D File Offset: 0x0001354D
			public string Name
			{
				get
				{
					return this.propertyInfo.Name;
				}
			}

			// Token: 0x170000B1 RID: 177
			// (get) Token: 0x060005F6 RID: 1526 RVA: 0x0001535A File Offset: 0x0001355A
			public Type Type
			{
				get
				{
					return this.propertyInfo.PropertyType;
				}
			}

			// Token: 0x170000B2 RID: 178
			// (get) Token: 0x060005F7 RID: 1527 RVA: 0x00015367 File Offset: 0x00013567
			// (set) Token: 0x060005F8 RID: 1528 RVA: 0x0001536F File Offset: 0x0001356F
			[Nullable(2)]
			public Type TypeOverride { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x170000B3 RID: 179
			// (get) Token: 0x060005F9 RID: 1529 RVA: 0x00015378 File Offset: 0x00013578
			// (set) Token: 0x060005FA RID: 1530 RVA: 0x00015380 File Offset: 0x00013580
			public int Order { get; set; }

			// Token: 0x170000B4 RID: 180
			// (get) Token: 0x060005FB RID: 1531 RVA: 0x00015389 File Offset: 0x00013589
			public bool CanWrite
			{
				get
				{
					return this.propertyInfo.CanWrite;
				}
			}

			// Token: 0x170000B5 RID: 181
			// (get) Token: 0x060005FC RID: 1532 RVA: 0x00015396 File Offset: 0x00013596
			// (set) Token: 0x060005FD RID: 1533 RVA: 0x0001539E File Offset: 0x0001359E
			public ScalarStyle ScalarStyle { get; set; }

			// Token: 0x060005FE RID: 1534 RVA: 0x000153A7 File Offset: 0x000135A7
			public void Write(object target, [Nullable(2)] object value)
			{
				this.propertyInfo.SetValue(target, value, null);
			}

			// Token: 0x060005FF RID: 1535 RVA: 0x000153B7 File Offset: 0x000135B7
			[NullableContext(0)]
			[return: Nullable(2)]
			public T GetCustomAttribute<T>() where T : Attribute
			{
				return (T)((object)Enumerable.FirstOrDefault<Attribute>(this.propertyInfo.GetAllCustomAttributes<T>()));
			}

			// Token: 0x06000600 RID: 1536 RVA: 0x000153D0 File Offset: 0x000135D0
			public IObjectDescriptor Read(object target)
			{
				object obj = this.propertyInfo.ReadValue(target);
				Type type = this.TypeOverride ?? this.typeResolver.Resolve(this.Type, obj);
				return new ObjectDescriptor(obj, type, this.Type, this.ScalarStyle);
			}

			// Token: 0x0400029E RID: 670
			private readonly PropertyInfo propertyInfo;

			// Token: 0x0400029F RID: 671
			private readonly ITypeResolver typeResolver;
		}
	}
}
