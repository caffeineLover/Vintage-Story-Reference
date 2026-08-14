using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000BE RID: 190
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PropertyDescriptor : IPropertyDescriptor
	{
		// Token: 0x0600044F RID: 1103 RVA: 0x000114E2 File Offset: 0x0000F6E2
		public PropertyDescriptor(IPropertyDescriptor baseDescriptor)
		{
			this.baseDescriptor = baseDescriptor;
			this.Name = baseDescriptor.Name;
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x000114FD File Offset: 0x0000F6FD
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x00011505 File Offset: 0x0000F705
		public string Name { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x0001150E File Offset: 0x0000F70E
		public Type Type
		{
			get
			{
				return this.baseDescriptor.Type;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x0001151B File Offset: 0x0000F71B
		// (set) Token: 0x06000454 RID: 1108 RVA: 0x00011528 File Offset: 0x0000F728
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

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x00011536 File Offset: 0x0000F736
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x0001153E File Offset: 0x0000F73E
		public int Order { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00011547 File Offset: 0x0000F747
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x00011554 File Offset: 0x0000F754
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

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00011562 File Offset: 0x0000F762
		public bool CanWrite
		{
			get
			{
				return this.baseDescriptor.CanWrite;
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0001156F File Offset: 0x0000F76F
		public void Write(object target, [Nullable(2)] object value)
		{
			this.baseDescriptor.Write(target, value);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0001157E File Offset: 0x0000F77E
		[NullableContext(0)]
		[return: Nullable(2)]
		public T GetCustomAttribute<T>() where T : Attribute
		{
			return this.baseDescriptor.GetCustomAttribute<T>();
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0001158B File Offset: 0x0000F78B
		public IObjectDescriptor Read(object target)
		{
			return this.baseDescriptor.Read(target);
		}

		// Token: 0x040001F0 RID: 496
		private readonly IPropertyDescriptor baseDescriptor;
	}
}
