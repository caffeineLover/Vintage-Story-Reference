using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x02000111 RID: 273
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ReadablePropertiesTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x060005D5 RID: 1493 RVA: 0x00014F56 File Offset: 0x00013156
		public ReadablePropertiesTypeInspector(ITypeResolver typeResolver) : this(typeResolver, false)
		{
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00014F60 File Offset: 0x00013160
		public ReadablePropertiesTypeInspector(ITypeResolver typeResolver, bool includeNonPublicProperties)
		{
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			this.includeNonPublicProperties = includeNonPublicProperties;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00014F85 File Offset: 0x00013185
		private static bool IsValidProperty(PropertyInfo property)
		{
			return property.CanRead && property.GetGetMethod(true).GetParameters().Length == 0;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00014FA1 File Offset: 0x000131A1
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return Enumerable.Select<PropertyInfo, IPropertyDescriptor>(Enumerable.Where<PropertyInfo>(type.GetProperties(this.includeNonPublicProperties), new Func<PropertyInfo, bool>(ReadablePropertiesTypeInspector.IsValidProperty)), (PropertyInfo p) => new ReadablePropertiesTypeInspector.ReflectionPropertyDescriptor(p, this.typeResolver));
		}

		// Token: 0x04000292 RID: 658
		private readonly ITypeResolver typeResolver;

		// Token: 0x04000293 RID: 659
		private readonly bool includeNonPublicProperties;

		// Token: 0x02000112 RID: 274
		[Nullable(0)]
		private sealed class ReflectionPropertyDescriptor : IPropertyDescriptor
		{
			// Token: 0x060005DA RID: 1498 RVA: 0x00014FDF File Offset: 0x000131DF
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

			// Token: 0x170000AA RID: 170
			// (get) Token: 0x060005DB RID: 1499 RVA: 0x0001501A File Offset: 0x0001321A
			public string Name
			{
				get
				{
					return this.propertyInfo.Name;
				}
			}

			// Token: 0x170000AB RID: 171
			// (get) Token: 0x060005DC RID: 1500 RVA: 0x00015027 File Offset: 0x00013227
			public Type Type
			{
				get
				{
					return this.propertyInfo.PropertyType;
				}
			}

			// Token: 0x170000AC RID: 172
			// (get) Token: 0x060005DD RID: 1501 RVA: 0x00015034 File Offset: 0x00013234
			// (set) Token: 0x060005DE RID: 1502 RVA: 0x0001503C File Offset: 0x0001323C
			[Nullable(2)]
			public Type TypeOverride { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x170000AD RID: 173
			// (get) Token: 0x060005DF RID: 1503 RVA: 0x00015045 File Offset: 0x00013245
			// (set) Token: 0x060005E0 RID: 1504 RVA: 0x0001504D File Offset: 0x0001324D
			public int Order { get; set; }

			// Token: 0x170000AE RID: 174
			// (get) Token: 0x060005E1 RID: 1505 RVA: 0x00015056 File Offset: 0x00013256
			public bool CanWrite
			{
				get
				{
					return this.propertyInfo.CanWrite;
				}
			}

			// Token: 0x170000AF RID: 175
			// (get) Token: 0x060005E2 RID: 1506 RVA: 0x00015063 File Offset: 0x00013263
			// (set) Token: 0x060005E3 RID: 1507 RVA: 0x0001506B File Offset: 0x0001326B
			public ScalarStyle ScalarStyle { get; set; }

			// Token: 0x060005E4 RID: 1508 RVA: 0x00015074 File Offset: 0x00013274
			public void Write(object target, [Nullable(2)] object value)
			{
				this.propertyInfo.SetValue(target, value, null);
			}

			// Token: 0x060005E5 RID: 1509 RVA: 0x00015084 File Offset: 0x00013284
			[NullableContext(0)]
			[return: Nullable(2)]
			public T GetCustomAttribute<T>() where T : Attribute
			{
				return (T)((object)Enumerable.FirstOrDefault<Attribute>(this.propertyInfo.GetAllCustomAttributes<T>()));
			}

			// Token: 0x060005E6 RID: 1510 RVA: 0x0001509C File Offset: 0x0001329C
			public IObjectDescriptor Read(object target)
			{
				object obj = this.propertyInfo.ReadValue(target);
				Type type = this.TypeOverride ?? this.typeResolver.Resolve(this.Type, obj);
				return new ObjectDescriptor(obj, type, this.Type, this.ScalarStyle);
			}

			// Token: 0x04000294 RID: 660
			private readonly PropertyInfo propertyInfo;

			// Token: 0x04000295 RID: 661
			private readonly ITypeResolver typeResolver;
		}
	}
}
