using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x02000113 RID: 275
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ReadableFieldsTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x06000601 RID: 1537 RVA: 0x00015763 File Offset: 0x00013963
		public ReadableFieldsTypeInspector(ITypeResolver typeResolver)
		{
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00015781 File Offset: 0x00013981
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return Enumerable.Select<FieldInfo, IPropertyDescriptor>(type.GetPublicFields(), (FieldInfo p) => new ReadableFieldsTypeInspector.ReflectionFieldDescriptor(p, this.typeResolver));
		}

		// Token: 0x040002BA RID: 698
		private readonly ITypeResolver typeResolver;

		// Token: 0x02000114 RID: 276
		[Nullable(0)]
		private sealed class ReflectionFieldDescriptor : IPropertyDescriptor
		{
			// Token: 0x06000604 RID: 1540 RVA: 0x000157A8 File Offset: 0x000139A8
			public ReflectionFieldDescriptor(FieldInfo fieldInfo, ITypeResolver typeResolver)
			{
				this.fieldInfo = fieldInfo;
				this.typeResolver = typeResolver;
				this.ScalarStyle = ScalarStyle.Any;
			}

			// Token: 0x170000A4 RID: 164
			// (get) Token: 0x06000605 RID: 1541 RVA: 0x000157C5 File Offset: 0x000139C5
			public string Name
			{
				get
				{
					return this.fieldInfo.Name;
				}
			}

			// Token: 0x170000A5 RID: 165
			// (get) Token: 0x06000606 RID: 1542 RVA: 0x000157D2 File Offset: 0x000139D2
			public Type Type
			{
				get
				{
					return this.fieldInfo.FieldType;
				}
			}

			// Token: 0x170000A6 RID: 166
			// (get) Token: 0x06000607 RID: 1543 RVA: 0x000157DF File Offset: 0x000139DF
			// (set) Token: 0x06000608 RID: 1544 RVA: 0x000157E7 File Offset: 0x000139E7
			[Nullable(2)]
			public Type TypeOverride { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x170000A7 RID: 167
			// (get) Token: 0x06000609 RID: 1545 RVA: 0x000157F0 File Offset: 0x000139F0
			// (set) Token: 0x0600060A RID: 1546 RVA: 0x000157F8 File Offset: 0x000139F8
			public int Order { get; set; }

			// Token: 0x170000A8 RID: 168
			// (get) Token: 0x0600060B RID: 1547 RVA: 0x00015801 File Offset: 0x00013A01
			public bool CanWrite
			{
				get
				{
					return !this.fieldInfo.IsInitOnly;
				}
			}

			// Token: 0x170000A9 RID: 169
			// (get) Token: 0x0600060C RID: 1548 RVA: 0x00015811 File Offset: 0x00013A11
			// (set) Token: 0x0600060D RID: 1549 RVA: 0x00015819 File Offset: 0x00013A19
			public ScalarStyle ScalarStyle { get; set; }

			// Token: 0x0600060E RID: 1550 RVA: 0x00015822 File Offset: 0x00013A22
			public void Write(object target, [Nullable(2)] object value)
			{
				this.fieldInfo.SetValue(target, value);
			}

			// Token: 0x0600060F RID: 1551 RVA: 0x00015831 File Offset: 0x00013A31
			[NullableContext(0)]
			[return: Nullable(2)]
			public T GetCustomAttribute<T>() where T : Attribute
			{
				return (T)((object)Enumerable.FirstOrDefault<object>(this.fieldInfo.GetCustomAttributes(typeof(T), true)));
			}

			// Token: 0x06000610 RID: 1552 RVA: 0x00015854 File Offset: 0x00013A54
			public IObjectDescriptor Read(object target)
			{
				object value = this.fieldInfo.GetValue(target);
				Type type = this.TypeOverride ?? this.typeResolver.Resolve(this.Type, value);
				return new ObjectDescriptor(value, type, this.Type, this.ScalarStyle);
			}

			// Token: 0x040002BB RID: 699
			private readonly FieldInfo fieldInfo;

			// Token: 0x040002BC RID: 700
			private readonly ITypeResolver typeResolver;
		}
	}
}
