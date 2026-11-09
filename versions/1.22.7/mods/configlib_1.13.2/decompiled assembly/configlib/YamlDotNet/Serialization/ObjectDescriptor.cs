using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000B8 RID: 184
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ObjectDescriptor : IObjectDescriptor
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x00010AFA File Offset: 0x0000ECFA
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x00010B02 File Offset: 0x0000ED02
		[Nullable(2)]
		public object Value { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00010B0B File Offset: 0x0000ED0B
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x00010B13 File Offset: 0x0000ED13
		public Type Type { get; private set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00010B1C File Offset: 0x0000ED1C
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x00010B24 File Offset: 0x0000ED24
		public Type StaticType { get; private set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x00010B2D File Offset: 0x0000ED2D
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x00010B35 File Offset: 0x0000ED35
		public ScalarStyle ScalarStyle { get; private set; }

		// Token: 0x0600040D RID: 1037 RVA: 0x00010B3E File Offset: 0x0000ED3E
		public ObjectDescriptor([Nullable(2)] object value, Type type, Type staticType) : this(value, type, staticType, ScalarStyle.Any)
		{
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00010B4C File Offset: 0x0000ED4C
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
