using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x02000059 RID: 89
	[NullableContext(1)]
	[Nullable(0)]
	public class ReadableFieldsTypeInspector : ReflectionTypeInspector
	{
		// Token: 0x06000245 RID: 581 RVA: 0x00007CD0 File Offset: 0x00005ED0
		public ReadableFieldsTypeInspector(ITypeResolver typeResolver)
		{
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00007CEE File Offset: 0x00005EEE
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return Enumerable.Select<FieldInfo, IPropertyDescriptor>(type.GetPublicFields(), (FieldInfo p) => new ReadableFieldsTypeInspector.ReflectionFieldDescriptor(p, this.typeResolver));
		}

		// Token: 0x04000090 RID: 144
		private readonly ITypeResolver typeResolver;

		// Token: 0x0200016B RID: 363
		[Nullable(0)]
		protected class ReflectionFieldDescriptor : IPropertyDescriptor
		{
			// Token: 0x060007D9 RID: 2009 RVA: 0x00019250 File Offset: 0x00017450
			public ReflectionFieldDescriptor(FieldInfo fieldInfo, ITypeResolver typeResolver)
			{
				this.fieldInfo = fieldInfo;
				this.typeResolver = typeResolver;
				YamlConverterAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<YamlConverterAttribute>(fieldInfo);
				if (customAttribute != null)
				{
					this.ConverterType = customAttribute.ConverterType;
				}
				this.ScalarStyle = ScalarStyle.Any;
			}

			// Token: 0x170000F2 RID: 242
			// (get) Token: 0x060007DA RID: 2010 RVA: 0x0001928E File Offset: 0x0001748E
			public string Name
			{
				get
				{
					return this.fieldInfo.Name;
				}
			}

			// Token: 0x170000F3 RID: 243
			// (get) Token: 0x060007DB RID: 2011 RVA: 0x0001929B File Offset: 0x0001749B
			public bool Required
			{
				get
				{
					return this.fieldInfo.IsRequired();
				}
			}

			// Token: 0x170000F4 RID: 244
			// (get) Token: 0x060007DC RID: 2012 RVA: 0x000192A8 File Offset: 0x000174A8
			public Type Type
			{
				get
				{
					return this.fieldInfo.FieldType;
				}
			}

			// Token: 0x170000F5 RID: 245
			// (get) Token: 0x060007DD RID: 2013 RVA: 0x000192B5 File Offset: 0x000174B5
			[Nullable(2)]
			public Type ConverterType { [NullableContext(2)] get; }

			// Token: 0x170000F6 RID: 246
			// (get) Token: 0x060007DE RID: 2014 RVA: 0x000192BD File Offset: 0x000174BD
			// (set) Token: 0x060007DF RID: 2015 RVA: 0x000192C5 File Offset: 0x000174C5
			[Nullable(2)]
			public Type TypeOverride { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x170000F7 RID: 247
			// (get) Token: 0x060007E0 RID: 2016 RVA: 0x000192CE File Offset: 0x000174CE
			public bool AllowNulls
			{
				get
				{
					return this.fieldInfo.AcceptsNull();
				}
			}

			// Token: 0x170000F8 RID: 248
			// (get) Token: 0x060007E1 RID: 2017 RVA: 0x000192DB File Offset: 0x000174DB
			// (set) Token: 0x060007E2 RID: 2018 RVA: 0x000192E3 File Offset: 0x000174E3
			public int Order { get; set; }

			// Token: 0x170000F9 RID: 249
			// (get) Token: 0x060007E3 RID: 2019 RVA: 0x000192EC File Offset: 0x000174EC
			public bool CanWrite
			{
				get
				{
					return !this.fieldInfo.IsInitOnly;
				}
			}

			// Token: 0x170000FA RID: 250
			// (get) Token: 0x060007E4 RID: 2020 RVA: 0x000192FC File Offset: 0x000174FC
			// (set) Token: 0x060007E5 RID: 2021 RVA: 0x00019304 File Offset: 0x00017504
			public ScalarStyle ScalarStyle { get; set; }

			// Token: 0x060007E6 RID: 2022 RVA: 0x0001930D File Offset: 0x0001750D
			public void Write(object target, [Nullable(2)] object value)
			{
				this.fieldInfo.SetValue(target, value);
			}

			// Token: 0x060007E7 RID: 2023 RVA: 0x0001931C File Offset: 0x0001751C
			[NullableContext(0)]
			[return: Nullable(2)]
			public T GetCustomAttribute<T>() where T : Attribute
			{
				object[] customAttributes = this.fieldInfo.GetCustomAttributes(typeof(T), true);
				return (T)((object)Enumerable.FirstOrDefault<object>(customAttributes));
			}

			// Token: 0x060007E8 RID: 2024 RVA: 0x0001934C File Offset: 0x0001754C
			public IObjectDescriptor Read(object target)
			{
				object value = this.fieldInfo.GetValue(target);
				Type type = this.TypeOverride ?? this.typeResolver.Resolve(this.Type, value);
				return new ObjectDescriptor(value, type, this.Type, this.ScalarStyle);
			}

			// Token: 0x040002D2 RID: 722
			private readonly FieldInfo fieldInfo;

			// Token: 0x040002D3 RID: 723
			private readonly ITypeResolver typeResolver;
		}
	}
}
