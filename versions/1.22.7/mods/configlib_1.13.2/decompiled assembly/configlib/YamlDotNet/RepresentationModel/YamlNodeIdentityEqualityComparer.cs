using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x02000186 RID: 390
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlNodeIdentityEqualityComparer : IEqualityComparer<YamlNode>
	{
		// Token: 0x060007F1 RID: 2033 RVA: 0x0001B0F9 File Offset: 0x000192F9
		public bool Equals([AllowNull] YamlNode x, [AllowNull] YamlNode y)
		{
			return x == y;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0001B0FF File Offset: 0x000192FF
		public int GetHashCode(YamlNode obj)
		{
			return obj.GetHashCode();
		}
	}
}
