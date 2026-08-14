using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x0200005D RID: 93
	[NullableContext(1)]
	[Nullable(0)]
	public class WritablePropertiesTypeInspector : ReflectionTypeInspector
	{
		// Token: 0x06000255 RID: 597 RVA: 0x00007FF0 File Offset: 0x000061F0
		public WritablePropertiesTypeInspector(ITypeResolver typeResolver) : this(typeResolver, false)
		{
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00007FFA File Offset: 0x000061FA
		public WritablePropertiesTypeInspector(ITypeResolver typeResolver, bool includeNonPublicProperties)
		{
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			this.includeNonPublicProperties = includeNonPublicProperties;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000801F File Offset: 0x0000621F
		private static bool IsValidProperty(PropertyInfo property)
		{
			return property.CanWrite && property.GetSetMethod(true).GetParameters().Length == 1;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000803C File Offset: 0x0000623C
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			IEnumerable<PropertyInfo> properties = type.GetProperties(this.includeNonPublicProperties);
			Func<PropertyInfo, bool> func;
			if ((func = WritablePropertiesTypeInspector.<>O.<0>__IsValidProperty) == null)
			{
				func = (WritablePropertiesTypeInspector.<>O.<0>__IsValidProperty = new Func<PropertyInfo, bool>(WritablePropertiesTypeInspector.IsValidProperty));
			}
			return Enumerable.ToArray<IPropertyDescriptor>(Enumerable.Select<PropertyInfo, IPropertyDescriptor>(Enumerable.Where<PropertyInfo>(properties, func), (PropertyInfo p) => new WritablePropertiesTypeInspector.ReflectionPropertyDescriptor(p, this.typeResolver)));
		}

		// Token: 0x04000093 RID: 147
		private readonly ITypeResolver typeResolver;

		// Token: 0x04000094 RID: 148
		private readonly bool includeNonPublicProperties;

		// Token: 0x02000170 RID: 368
		[Nullable(0)]
		protected class ReflectionPropertyDescriptor : IPropertyDescriptor
		{
			// Token: 0x06000800 RID: 2048 RVA: 0x00019548 File Offset: 0x00017748
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

			// Token: 0x17000104 RID: 260
			// (get) Token: 0x06000801 RID: 2049 RVA: 0x000195A4 File Offset: 0x000177A4
			public string Name
			{
				get
				{
					return this.propertyInfo.Name;
				}
			}

			// Token: 0x17000105 RID: 261
			// (get) Token: 0x06000802 RID: 2050 RVA: 0x000195B1 File Offset: 0x000177B1
			public bool Required
			{
				get
				{
					return this.propertyInfo.IsRequired();
				}
			}

			// Token: 0x17000106 RID: 262
			// (get) Token: 0x06000803 RID: 2051 RVA: 0x000195BE File Offset: 0x000177BE
			public Type Type
			{
				get
				{
					return this.propertyInfo.PropertyType;
				}
			}

			// Token: 0x17000107 RID: 263
			// (get) Token: 0x06000804 RID: 2052 RVA: 0x000195CB File Offset: 0x000177CB
			// (set) Token: 0x06000805 RID: 2053 RVA: 0x000195D3 File Offset: 0x000177D3
			[Nullable(2)]
			public Type TypeOverride { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x17000108 RID: 264
			// (get) Token: 0x06000806 RID: 2054 RVA: 0x000195DC File Offset: 0x000177DC
			// (set) Token: 0x06000807 RID: 2055 RVA: 0x000195E4 File Offset: 0x000177E4
			[Nullable(2)]
			public Type ConverterType { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x17000109 RID: 265
			// (get) Token: 0x06000808 RID: 2056 RVA: 0x000195ED File Offset: 0x000177ED
			public bool AllowNulls
			{
				get
				{
					return this.propertyInfo.AcceptsNull();
				}
			}

			// Token: 0x1700010A RID: 266
			// (get) Token: 0x06000809 RID: 2057 RVA: 0x000195FA File Offset: 0x000177FA
			// (set) Token: 0x0600080A RID: 2058 RVA: 0x00019602 File Offset: 0x00017802
			public int Order { get; set; }

			// Token: 0x1700010B RID: 267
			// (get) Token: 0x0600080B RID: 2059 RVA: 0x0001960B File Offset: 0x0001780B
			public bool CanWrite
			{
				get
				{
					return this.propertyInfo.CanWrite;
				}
			}

			// Token: 0x1700010C RID: 268
			// (get) Token: 0x0600080C RID: 2060 RVA: 0x00019618 File Offset: 0x00017818
			// (set) Token: 0x0600080D RID: 2061 RVA: 0x00019620 File Offset: 0x00017820
			public ScalarStyle ScalarStyle { get; set; }

			// Token: 0x0600080E RID: 2062 RVA: 0x00019629 File Offset: 0x00017829
			public void Write(object target, [Nullable(2)] object value)
			{
				this.propertyInfo.SetValue(target, value, null);
			}

			// Token: 0x0600080F RID: 2063 RVA: 0x0001963C File Offset: 0x0001783C
			[NullableContext(0)]
			[return: Nullable(2)]
			public T GetCustomAttribute<T>() where T : Attribute
			{
				Attribute[] allCustomAttributes = this.propertyInfo.GetAllCustomAttributes<T>();
				return (T)((object)Enumerable.FirstOrDefault<Attribute>(allCustomAttributes));
			}

			// Token: 0x06000810 RID: 2064 RVA: 0x00019660 File Offset: 0x00017860
			public IObjectDescriptor Read(object target)
			{
				object obj = this.propertyInfo.ReadValue(target);
				Type type = this.TypeOverride ?? this.typeResolver.Resolve(this.Type, obj);
				return new ObjectDescriptor(obj, type, this.Type, this.ScalarStyle);
			}

			// Token: 0x040002E2 RID: 738
			private readonly PropertyInfo propertyInfo;

			// Token: 0x040002E3 RID: 739
			private readonly ITypeResolver typeResolver;
		}

		// Token: 0x02000171 RID: 369
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040002E8 RID: 744
			[Nullable(0)]
			public static Func<PropertyInfo, bool> <0>__IsValidProperty;
		}
	}
}
