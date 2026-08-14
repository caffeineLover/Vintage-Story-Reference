using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000EA RID: 234
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TagMappings
	{
		// Token: 0x06000568 RID: 1384 RVA: 0x00013E27 File Offset: 0x00012027
		public TagMappings()
		{
			this.mappings = new Dictionary<string, Type>();
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00013E3A File Offset: 0x0001203A
		public TagMappings(IDictionary<string, Type> mappings)
		{
			this.mappings = new Dictionary<string, Type>(mappings);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00013E4E File Offset: 0x0001204E
		public void Add(string tag, Type mapping)
		{
			this.mappings.Add(tag, mapping);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00013E60 File Offset: 0x00012060
		[return: Nullable(2)]
		internal Type GetMapping(string tag)
		{
			Type result;
			if (!this.mappings.TryGetValue(tag, ref result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x04000279 RID: 633
		private readonly IDictionary<string, Type> mappings;
	}
}
