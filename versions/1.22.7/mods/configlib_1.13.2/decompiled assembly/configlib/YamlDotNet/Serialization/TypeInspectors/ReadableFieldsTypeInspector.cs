using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x0200010F RID: 271
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ReadableFieldsTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x060005C5 RID: 1477 RVA: 0x00014E1B File Offset: 0x0001301B
		public ReadableFieldsTypeInspector(ITypeResolver typeResolver)
		{
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00014E39 File Offset: 0x00013039
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return Enumerable.Select<FieldInfo, IPropertyDescriptor>(type.GetPublicFields(), (FieldInfo p) => new ReadableFieldsTypeInspector.ReflectionFieldDescriptor(p, this.typeResolver));
		}

		// Token: 0x0400028C RID: 652
		private readonly ITypeResolver typeResolver;

		// Token: 0x02000110 RID: 272
		[Nullable(0)]
		private sealed class ReflectionFieldDescriptor : IPropertyDescriptor
		{
			// Token: 0x060005C8 RID: 1480 RVA: 0x00014E60 File Offset: 0x00013060
			public ReflectionFieldDescriptor(FieldInfo fieldInfo, ITypeResolver typeResolver)
			{
				this.fieldInfo = fieldInfo;
				this.typeResolver = typeResolver;
				this.ScalarStyle = ScalarStyle.Any;
			}

			// Token: 0x170000A4 RID: 164
			// (get) Token: 0x060005C9 RID: 1481 RVA: 0x00014E7D File Offset: 0x0001307D
			public string Name
			{
				get
				{
					return this.fieldInfo.Name;
				}
			}

			// Token: 0x170000A5 RID: 165
			// (get) Token: 0x060005CA RID: 1482 RVA: 0x00014E8A File Offset: 0x0001308A
			public Type Type
			{
				get
				{
					return this.fieldInfo.FieldType;
				}
			}

			// Token: 0x170000A6 RID: 166
			// (get) Token: 0x060005CB RID: 1483 RVA: 0x00014E97 File Offset: 0x00013097
			// (set) Token: 0x060005CC RID: 1484 RVA: 0x00014E9F File Offset: 0x0001309F
			[Nullable(2)]
			public Type TypeOverride { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x170000A7 RID: 167
			// (get) Token: 0x060005CD RID: 1485 RVA: 0x00014EA8 File Offset: 0x000130A8
			// (set) Token: 0x060005CE RID: 1486 RVA: 0x00014EB0 File Offset: 0x000130B0
			public int Order { get; set; }

			// Token: 0x170000A8 RID: 168
			// (get) Token: 0x060005CF RID: 1487 RVA: 0x00014EB9 File Offset: 0x000130B9
			public bool CanWrite
			{
				get
				{
					return !this.fieldInfo.IsInitOnly;
				}
			}

			// Token: 0x170000A9 RID: 169
			// (get) Token: 0x060005D0 RID: 1488 RVA: 0x00014EC9 File Offset: 0x000130C9
			// (set) Token: 0x060005D1 RID: 1489 RVA: 0x00014ED1 File Offset: 0x000130D1
			public ScalarStyle ScalarStyle { get; set; }

			// Token: 0x060005D2 RID: 1490 RVA: 0x00014EDA File Offset: 0x000130DA
			public void Write(object target, [Nullable(2)] object value)
			{
				this.fieldInfo.SetValue(target, value);
			}

			// Token: 0x060005D3 RID: 1491 RVA: 0x00014EE9 File Offset: 0x000130E9
			[NullableContext(0)]
			[return: Nullable(2)]
			public T GetCustomAttribute<T>() where T : Attribute
			{
				return (T)((object)Enumerable.FirstOrDefault<object>(this.fieldInfo.GetCustomAttributes(typeof(T), true)));
			}

			// Token: 0x060005D4 RID: 1492 RVA: 0x00014F0C File Offset: 0x0001310C
			public IObjectDescriptor Read(object target)
			{
				object value = this.fieldInfo.GetValue(target);
				Type type = this.TypeOverride ?? this.typeResolver.Resolve(this.Type, value);
				return new ObjectDescriptor(value, type, this.Type, this.ScalarStyle);
			}

			// Token: 0x0400028D RID: 653
			private readonly FieldInfo fieldInfo;

			// Token: 0x0400028E RID: 654
			private readonly ITypeResolver typeResolver;
		}
	}
}
