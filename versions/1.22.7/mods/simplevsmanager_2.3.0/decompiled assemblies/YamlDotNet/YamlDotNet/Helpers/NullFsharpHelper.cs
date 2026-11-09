using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization;

namespace YamlDotNet.Helpers
{
	// Token: 0x020000BB RID: 187
	[NullableContext(1)]
	[Nullable(0)]
	public class NullFsharpHelper : IFsharpHelper
	{
		// Token: 0x0600045A RID: 1114 RVA: 0x0000E7AE File Offset: 0x0000C9AE
		[return: Nullable(2)]
		public object CreateFsharpListFromArray(Type t, Type itemsType, Array arr)
		{
			return null;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000E7B1 File Offset: 0x0000C9B1
		[return: Nullable(2)]
		public Type GetOptionUnderlyingType(Type t)
		{
			return null;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000E7B4 File Offset: 0x0000C9B4
		[return: Nullable(2)]
		public object GetValue(IObjectDescriptor objectDescriptor)
		{
			return null;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000E7B7 File Offset: 0x0000C9B7
		public bool IsFsharpListType(Type t)
		{
			return false;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000E7BA File Offset: 0x0000C9BA
		public bool IsOptionType(Type t)
		{
			return false;
		}
	}
}
