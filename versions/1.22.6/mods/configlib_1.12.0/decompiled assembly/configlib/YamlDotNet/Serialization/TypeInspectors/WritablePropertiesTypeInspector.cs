using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x0200011A RID: 282
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WritablePropertiesTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x0600062B RID: 1579 RVA: 0x00015BCB File Offset: 0x00013DCB
		public WritablePropertiesTypeInspector(ITypeResolver typeResolver) : this(typeResolver, false)
		{
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00015BD5 File Offset: 0x00013DD5
		public WritablePropertiesTypeInspector(ITypeResolver typeResolver, bool includeNonPublicProperties)
		{
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			this.includeNonPublicProperties = includeNonPublicProperties;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00015BFA File Offset: 0x00013DFA
		private static bool IsValidProperty(PropertyInfo property)
		{
			return property.CanWrite && property.GetSetMethod(true).GetParameters().Length == 1;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00015C17 File Offset: 0x00013E17
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return Enumerable.ToArray<IPropertyDescriptor>(Enumerable.Select<PropertyInfo, IPropertyDescriptor>(Enumerable.Where<PropertyInfo>(type.GetProperties(this.includeNonPublicProperties), new Func<PropertyInfo, bool>(WritablePropertiesTypeInspector.IsValidProperty)), (PropertyInfo p) => new WritablePropertiesTypeInspector.ReflectionPropertyDescriptor(p, this.typeResolver)));
		}

		// Token: 0x040002CA RID: 714
		private readonly ITypeResolver typeResolver;

		// Token: 0x040002CB RID: 715
		private readonly bool includeNonPublicProperties;

		// Token: 0x0200011B RID: 283
		[Nullable(0)]
		private sealed class ReflectionPropertyDescriptor : IPropertyDescriptor
		{
			// Token: 0x06000630 RID: 1584 RVA: 0x00015C5A File Offset: 0x00013E5A
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
			// (get) Token: 0x06000631 RID: 1585 RVA: 0x00015C95 File Offset: 0x00013E95
			public string Name
			{
				get
				{
					return this.propertyInfo.Name;
				}
			}

			// Token: 0x170000B1 RID: 177
			// (get) Token: 0x06000632 RID: 1586 RVA: 0x00015CA2 File Offset: 0x00013EA2
			public Type Type
			{
				get
				{
					return this.propertyInfo.PropertyType;
				}
			}

			// Token: 0x170000B2 RID: 178
			// (get) Token: 0x06000633 RID: 1587 RVA: 0x00015CAF File Offset: 0x00013EAF
			// (set) Token: 0x06000634 RID: 1588 RVA: 0x00015CB7 File Offset: 0x00013EB7
			[Nullable(2)]
			public Type TypeOverride { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x170000B3 RID: 179
			// (get) Token: 0x06000635 RID: 1589 RVA: 0x00015CC0 File Offset: 0x00013EC0
			// (set) Token: 0x06000636 RID: 1590 RVA: 0x00015CC8 File Offset: 0x00013EC8
			public int Order { get; set; }

			// Token: 0x170000B4 RID: 180
			// (get) Token: 0x06000637 RID: 1591 RVA: 0x00015CD1 File Offset: 0x00013ED1
			public bool CanWrite
			{
				get
				{
					return this.propertyInfo.CanWrite;
				}
			}

			// Token: 0x170000B5 RID: 181
			// (get) Token: 0x06000638 RID: 1592 RVA: 0x00015CDE File Offset: 0x00013EDE
			// (set) Token: 0x06000639 RID: 1593 RVA: 0x00015CE6 File Offset: 0x00013EE6
			public ScalarStyle ScalarStyle { get; set; }

			// Token: 0x0600063A RID: 1594 RVA: 0x00015CEF File Offset: 0x00013EEF
			public void Write(object target, [Nullable(2)] object value)
			{
				this.propertyInfo.SetValue(target, value, null);
			}

			// Token: 0x0600063B RID: 1595 RVA: 0x00015CFF File Offset: 0x00013EFF
			[NullableContext(0)]
			[return: Nullable(2)]
			public T GetCustomAttribute<T>() where T : Attribute
			{
				return (T)((object)Enumerable.FirstOrDefault<Attribute>(this.propertyInfo.GetAllCustomAttributes<T>()));
			}

			// Token: 0x0600063C RID: 1596 RVA: 0x00015D18 File Offset: 0x00013F18
			public IObjectDescriptor Read(object target)
			{
				object obj = this.propertyInfo.ReadValue(target);
				Type type = this.TypeOverride ?? this.typeResolver.Resolve(this.Type, obj);
				return new ObjectDescriptor(obj, type, this.Type, this.ScalarStyle);
			}

			// Token: 0x040002CC RID: 716
			private readonly PropertyInfo propertyInfo;

			// Token: 0x040002CD RID: 717
			private readonly ITypeResolver typeResolver;
		}
	}
}
