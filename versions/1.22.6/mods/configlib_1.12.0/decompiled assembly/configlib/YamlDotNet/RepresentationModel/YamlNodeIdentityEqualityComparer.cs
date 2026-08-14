using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x0200018A RID: 394
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlNodeIdentityEqualityComparer : IEqualityComparer<YamlNode>
	{
		// Token: 0x0600082D RID: 2093 RVA: 0x0001BA41 File Offset: 0x00019C41
		public bool Equals([AllowNull] YamlNode x, [AllowNull] YamlNode y)
		{
			return x == y;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0001BA47 File Offset: 0x00019C47
		public int GetHashCode(YamlNode obj)
		{
			return obj.GetHashCode();
		}
	}
}
