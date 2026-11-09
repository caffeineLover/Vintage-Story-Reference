using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000BA RID: 186
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PropertyDescriptor : IPropertyDescriptor
	{
		// Token: 0x06000413 RID: 1043 RVA: 0x00010B9A File Offset: 0x0000ED9A
		public PropertyDescriptor(IPropertyDescriptor baseDescriptor)
		{
			this.baseDescriptor = baseDescriptor;
			this.Name = baseDescriptor.Name;
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x00010BB5 File Offset: 0x0000EDB5
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x00010BBD File Offset: 0x0000EDBD
		public string Name { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x00010BC6 File Offset: 0x0000EDC6
		public Type Type
		{
			get
			{
				return this.baseDescriptor.Type;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x00010BD3 File Offset: 0x0000EDD3
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x00010BE0 File Offset: 0x0000EDE0
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
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00010BEE File Offset: 0x0000EDEE
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x00010BF6 File Offset: 0x0000EDF6
		public int Order { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00010BFF File Offset: 0x0000EDFF
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x00010C0C File Offset: 0x0000EE0C
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
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00010C1A File Offset: 0x0000EE1A
		public bool CanWrite
		{
			get
			{
				return this.baseDescriptor.CanWrite;
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00010C27 File Offset: 0x0000EE27
		public void Write(object target, [Nullable(2)] object value)
		{
			this.baseDescriptor.Write(target, value);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00010C36 File Offset: 0x0000EE36
		[NullableContext(0)]
		[return: Nullable(2)]
		public T GetCustomAttribute<T>() where T : Attribute
		{
			return this.baseDescriptor.GetCustomAttribute<T>();
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00010C43 File Offset: 0x0000EE43
		public IObjectDescriptor Read(object target)
		{
			return this.baseDescriptor.Read(target);
		}

		// Token: 0x040001C2 RID: 450
		private readonly IPropertyDescriptor baseDescriptor;
	}
}
