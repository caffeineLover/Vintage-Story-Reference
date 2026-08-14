using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x020000AC RID: 172
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlNodeIdentityEqualityComparer : IEqualityComparer<YamlNode>
	{
		// Token: 0x060003C7 RID: 967 RVA: 0x0000D865 File Offset: 0x0000BA65
		public bool Equals([AllowNull] YamlNode x, [AllowNull] YamlNode y)
		{
			return x == y;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000D86B File Offset: 0x0000BA6B
		public int GetHashCode(YamlNode obj)
		{
			return obj.GetHashCode();
		}
	}
}
