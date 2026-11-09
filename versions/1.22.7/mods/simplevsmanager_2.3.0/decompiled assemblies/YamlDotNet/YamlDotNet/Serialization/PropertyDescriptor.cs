using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000035 RID: 53
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PropertyDescriptor : IPropertyDescriptor
	{
		// Token: 0x06000120 RID: 288 RVA: 0x00003DA6 File Offset: 0x00001FA6
		public PropertyDescriptor(IPropertyDescriptor baseDescriptor)
		{
			this.baseDescriptor = baseDescriptor;
			this.Name = baseDescriptor.Name;
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00003DC1 File Offset: 0x00001FC1
		public bool AllowNulls
		{
			get
			{
				return this.baseDescriptor.AllowNulls;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00003DCE File Offset: 0x00001FCE
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00003DD6 File Offset: 0x00001FD6
		public string Name { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00003DDF File Offset: 0x00001FDF
		public bool Required
		{
			get
			{
				return this.baseDescriptor.Required;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00003DEC File Offset: 0x00001FEC
		public Type Type
		{
			get
			{
				return this.baseDescriptor.Type;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00003DF9 File Offset: 0x00001FF9
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00003E06 File Offset: 0x00002006
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

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00003E14 File Offset: 0x00002014
		[Nullable(2)]
		public Type ConverterType
		{
			[NullableContext(2)]
			get
			{
				return this.baseDescriptor.ConverterType;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00003E21 File Offset: 0x00002021
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00003E29 File Offset: 0x00002029
		public int Order { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00003E32 File Offset: 0x00002032
		// (set) Token: 0x0600012C RID: 300 RVA: 0x00003E3F File Offset: 0x0000203F
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

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00003E4D File Offset: 0x0000204D
		public bool CanWrite
		{
			get
			{
				return this.baseDescriptor.CanWrite;
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00003E5A File Offset: 0x0000205A
		public void Write(object target, [Nullable(2)] object value)
		{
			this.baseDescriptor.Write(target, value);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00003E69 File Offset: 0x00002069
		[NullableContext(0)]
		[return: Nullable(2)]
		public T GetCustomAttribute<T>() where T : Attribute
		{
			return this.baseDescriptor.GetCustomAttribute<T>();
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00003E76 File Offset: 0x00002076
		public IObjectDescriptor Read(object target)
		{
			return this.baseDescriptor.Read(target);
		}

		// Token: 0x04000038 RID: 56
		private readonly IPropertyDescriptor baseDescriptor;
	}
}
