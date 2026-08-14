using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x02000115 RID: 277
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ReadablePropertiesTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x06000611 RID: 1553 RVA: 0x0001589E File Offset: 0x00013A9E
		public ReadablePropertiesTypeInspector(ITypeResolver typeResolver) : this(typeResolver, false)
		{
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x000158A8 File Offset: 0x00013AA8
		public ReadablePropertiesTypeInspector(ITypeResolver typeResolver, bool includeNonPublicProperties)
		{
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			this.includeNonPublicProperties = includeNonPublicProperties;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x000158CD File Offset: 0x00013ACD
		private static bool IsValidProperty(PropertyInfo property)
		{
			return property.CanRead && property.GetGetMethod(true).GetParameters().Length == 0;
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x000158E9 File Offset: 0x00013AE9
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return Enumerable.Select<PropertyInfo, IPropertyDescriptor>(Enumerable.Where<PropertyInfo>(type.GetProperties(this.includeNonPublicProperties), new Func<PropertyInfo, bool>(ReadablePropertiesTypeInspector.IsValidProperty)), (PropertyInfo p) => new ReadablePropertiesTypeInspector.ReflectionPropertyDescriptor(p, this.typeResolver));
		}

		// Token: 0x040002C0 RID: 704
		private readonly ITypeResolver typeResolver;

		// Token: 0x040002C1 RID: 705
		private readonly bool includeNonPublicProperties;

		// Token: 0x02000116 RID: 278
		[Nullable(0)]
		private sealed class ReflectionPropertyDescriptor : IPropertyDescriptor
		{
			// Token: 0x06000616 RID: 1558 RVA: 0x00015927 File Offset: 0x00013B27
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
			// (get) Token: 0x06000617 RID: 1559 RVA: 0x00015962 File Offset: 0x00013B62
			public string Name
			{
				get
				{
					return this.propertyInfo.Name;
				}
			}

			// Token: 0x170000AB RID: 171
			// (get) Token: 0x06000618 RID: 1560 RVA: 0x0001596F File Offset: 0x00013B6F
			public Type Type
			{
				get
				{
					return this.propertyInfo.PropertyType;
				}
			}

			// Token: 0x170000AC RID: 172
			// (get) Token: 0x06000619 RID: 1561 RVA: 0x0001597C File Offset: 0x00013B7C
			// (set) Token: 0x0600061A RID: 1562 RVA: 0x00015984 File Offset: 0x00013B84
			[Nullable(2)]
			public Type TypeOverride { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x170000AD RID: 173
			// (get) Token: 0x0600061B RID: 1563 RVA: 0x0001598D File Offset: 0x00013B8D
			// (set) Token: 0x0600061C RID: 1564 RVA: 0x00015995 File Offset: 0x00013B95
			public int Order { get; set; }

			// Token: 0x170000AE RID: 174
			// (get) Token: 0x0600061D RID: 1565 RVA: 0x0001599E File Offset: 0x00013B9E
			public bool CanWrite
			{
				get
				{
					return this.propertyInfo.CanWrite;
				}
			}

			// Token: 0x170000AF RID: 175
			// (get) Token: 0x0600061E RID: 1566 RVA: 0x000159AB File Offset: 0x00013BAB
			// (set) Token: 0x0600061F RID: 1567 RVA: 0x000159B3 File Offset: 0x00013BB3
			public ScalarStyle ScalarStyle { get; set; }

			// Token: 0x06000620 RID: 1568 RVA: 0x000159BC File Offset: 0x00013BBC
			public void Write(object target, [Nullable(2)] object value)
			{
				this.propertyInfo.SetValue(target, value, null);
			}

			// Token: 0x06000621 RID: 1569 RVA: 0x000159CC File Offset: 0x00013BCC
			[NullableContext(0)]
			[return: Nullable(2)]
			public T GetCustomAttribute<T>() where T : Attribute
			{
				return (T)((object)Enumerable.FirstOrDefault<Attribute>(this.propertyInfo.GetAllCustomAttributes<T>()));
			}

			// Token: 0x06000622 RID: 1570 RVA: 0x000159E4 File Offset: 0x00013BE4
			public IObjectDescriptor Read(object target)
			{
				object obj = this.propertyInfo.ReadValue(target);
				Type type = this.TypeOverride ?? this.typeResolver.Resolve(this.Type, obj);
				return new ObjectDescriptor(obj, type, this.Type, this.ScalarStyle);
			}

			// Token: 0x040002C2 RID: 706
			private readonly PropertyInfo propertyInfo;

			// Token: 0x040002C3 RID: 707
			private readonly ITypeResolver typeResolver;
		}
	}
}
