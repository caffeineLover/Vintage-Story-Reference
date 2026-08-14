using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000BC RID: 188
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ObjectDescriptor : IObjectDescriptor
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x00011442 File Offset: 0x0000F642
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x0001144A File Offset: 0x0000F64A
		[Nullable(2)]
		public object Value { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x00011453 File Offset: 0x0000F653
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x0001145B File Offset: 0x0000F65B
		public Type Type { get; private set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00011464 File Offset: 0x0000F664
		// (set) Token: 0x06000446 RID: 1094 RVA: 0x0001146C File Offset: 0x0000F66C
		public Type StaticType { get; private set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x00011475 File Offset: 0x0000F675
		// (set) Token: 0x06000448 RID: 1096 RVA: 0x0001147D File Offset: 0x0000F67D
		public ScalarStyle ScalarStyle { get; private set; }

		// Token: 0x06000449 RID: 1097 RVA: 0x00011486 File Offset: 0x0000F686
		public ObjectDescriptor([Nullable(2)] object value, Type type, Type staticType) : this(value, type, staticType, ScalarStyle.Any)
		{
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00011494 File Offset: 0x0000F694
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
