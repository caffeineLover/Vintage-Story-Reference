using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x0200005A RID: 90
	[NullableContext(1)]
	[Nullable(0)]
	public class ReadablePropertiesTypeInspector : ReflectionTypeInspector
	{
		// Token: 0x06000248 RID: 584 RVA: 0x00007D15 File Offset: 0x00005F15
		public ReadablePropertiesTypeInspector(ITypeResolver typeResolver) : this(typeResolver, false)
		{
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00007D1F File Offset: 0x00005F1F
		public ReadablePropertiesTypeInspector(ITypeResolver typeResolver, bool includeNonPublicProperties)
		{
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			this.includeNonPublicProperties = includeNonPublicProperties;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00007D44 File Offset: 0x00005F44
		private static bool IsValidProperty(PropertyInfo property)
		{
			return property.CanRead && property.GetGetMethod(true).GetParameters().Length == 0;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00007D60 File Offset: 0x00005F60
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			IEnumerable<PropertyInfo> properties = type.GetProperties(this.includeNonPublicProperties);
			Func<PropertyInfo, bool> func;
			if ((func = ReadablePropertiesTypeInspector.<>O.<0>__IsValidProperty) == null)
			{
				func = (ReadablePropertiesTypeInspector.<>O.<0>__IsValidProperty = new Func<PropertyInfo, bool>(ReadablePropertiesTypeInspector.IsValidProperty));
			}
			return Enumerable.Select<PropertyInfo, IPropertyDescriptor>(Enumerable.Where<PropertyInfo>(properties, func), (PropertyInfo p) => new ReadablePropertiesTypeInspector.ReflectionPropertyDescriptor(p, this.typeResolver));
		}

		// Token: 0x04000091 RID: 145
		private readonly ITypeResolver typeResolver;

		// Token: 0x04000092 RID: 146
		private readonly bool includeNonPublicProperties;

		// Token: 0x0200016C RID: 364
		[Nullable(0)]
		protected class ReflectionPropertyDescriptor : IPropertyDescriptor
		{
			// Token: 0x060007E9 RID: 2025 RVA: 0x00019398 File Offset: 0x00017598
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
				YamlConverterAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<YamlConverterAttribute>(propertyInfo);
				if (customAttribute != null)
				{
					this.ConverterType = customAttribute.ConverterType;
				}
			}

			// Token: 0x170000FB RID: 251
			// (get) Token: 0x060007EA RID: 2026 RVA: 0x000193F4 File Offset: 0x000175F4
			public string Name
			{
				get
				{
					return this.propertyInfo.Name;
				}
			}

			// Token: 0x170000FC RID: 252
			// (get) Token: 0x060007EB RID: 2027 RVA: 0x00019401 File Offset: 0x00017601
			public bool Required
			{
				get
				{
					return this.propertyInfo.IsRequired();
				}
			}

			// Token: 0x170000FD RID: 253
			// (get) Token: 0x060007EC RID: 2028 RVA: 0x0001940E File Offset: 0x0001760E
			public Type Type
			{
				get
				{
					return this.propertyInfo.PropertyType;
				}
			}

			// Token: 0x170000FE RID: 254
			// (get) Token: 0x060007ED RID: 2029 RVA: 0x0001941B File Offset: 0x0001761B
			// (set) Token: 0x060007EE RID: 2030 RVA: 0x00019423 File Offset: 0x00017623
			[Nullable(2)]
			public Type TypeOverride { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x170000FF RID: 255
			// (get) Token: 0x060007EF RID: 2031 RVA: 0x0001942C File Offset: 0x0001762C
			// (set) Token: 0x060007F0 RID: 2032 RVA: 0x00019434 File Offset: 0x00017634
			[Nullable(2)]
			public Type ConverterType { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x17000100 RID: 256
			// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0001943D File Offset: 0x0001763D
			public bool AllowNulls
			{
				get
				{
					return this.propertyInfo.AcceptsNull();
				}
			}

			// Token: 0x17000101 RID: 257
			// (get) Token: 0x060007F2 RID: 2034 RVA: 0x0001944A File Offset: 0x0001764A
			// (set) Token: 0x060007F3 RID: 2035 RVA: 0x00019452 File Offset: 0x00017652
			public int Order { get; set; }

			// Token: 0x17000102 RID: 258
			// (get) Token: 0x060007F4 RID: 2036 RVA: 0x0001945B File Offset: 0x0001765B
			public bool CanWrite
			{
				get
				{
					return this.propertyInfo.CanWrite;
				}
			}

			// Token: 0x17000103 RID: 259
			// (get) Token: 0x060007F5 RID: 2037 RVA: 0x00019468 File Offset: 0x00017668
			// (set) Token: 0x060007F6 RID: 2038 RVA: 0x00019470 File Offset: 0x00017670
			public ScalarStyle ScalarStyle { get; set; }

			// Token: 0x060007F7 RID: 2039 RVA: 0x00019479 File Offset: 0x00017679
			public void Write(object target, [Nullable(2)] object value)
			{
				this.propertyInfo.SetValue(target, value, null);
			}

			// Token: 0x060007F8 RID: 2040 RVA: 0x0001948C File Offset: 0x0001768C
			[NullableContext(0)]
			[return: Nullable(2)]
			public T GetCustomAttribute<T>() where T : Attribute
			{
				Attribute[] allCustomAttributes = this.propertyInfo.GetAllCustomAttributes<T>();
				return (T)((object)Enumerable.FirstOrDefault<Attribute>(allCustomAttributes));
			}

			// Token: 0x060007F9 RID: 2041 RVA: 0x000194B0 File Offset: 0x000176B0
			public IObjectDescriptor Read(object target)
			{
				object obj = this.propertyInfo.ReadValue(target);
				Type type = this.TypeOverride ?? this.typeResolver.Resolve(this.Type, obj);
				return new ObjectDescriptor(obj, type, this.Type, this.ScalarStyle);
			}

			// Token: 0x040002D8 RID: 728
			private readonly PropertyInfo propertyInfo;

			// Token: 0x040002D9 RID: 729
			private readonly ITypeResolver typeResolver;
		}

		// Token: 0x0200016D RID: 365
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040002DE RID: 734
			[Nullable(0)]
			public static Func<PropertyInfo, bool> <0>__IsValidProperty;
		}
	}
}
