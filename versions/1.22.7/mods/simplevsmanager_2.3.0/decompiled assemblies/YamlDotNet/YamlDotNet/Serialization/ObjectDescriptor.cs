using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000033 RID: 51
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ObjectDescriptor : IObjectDescriptor
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00003D05 File Offset: 0x00001F05
		// (set) Token: 0x06000113 RID: 275 RVA: 0x00003D0D File Offset: 0x00001F0D
		[Nullable(2)]
		public object Value { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00003D16 File Offset: 0x00001F16
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00003D1E File Offset: 0x00001F1E
		public Type Type { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00003D27 File Offset: 0x00001F27
		// (set) Token: 0x06000117 RID: 279 RVA: 0x00003D2F File Offset: 0x00001F2F
		public Type StaticType { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00003D38 File Offset: 0x00001F38
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00003D40 File Offset: 0x00001F40
		public ScalarStyle ScalarStyle { get; private set; }

		// Token: 0x0600011A RID: 282 RVA: 0x00003D49 File Offset: 0x00001F49
		public ObjectDescriptor([Nullable(2)] object value, Type type, Type staticType) : this(value, type, staticType, ScalarStyle.Any)
		{
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00003D58 File Offset: 0x00001F58
		public ObjectDescriptor([Nullable(2)] object value, Type type, Type staticType, ScalarStyle scalarStyle)
		{
			this.Value = value;
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.Type = type;
			if (staticType == null)
			{
				throw new ArgumentNullException("staticType");
			}
			this.StaticType = staticType;
			this.ScalarStyle = scalarStyle;
		}
	}
}
