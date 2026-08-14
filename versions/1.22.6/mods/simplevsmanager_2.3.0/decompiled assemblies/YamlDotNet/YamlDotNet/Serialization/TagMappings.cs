using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200003E RID: 62
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TagMappings
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x0000657F File Offset: 0x0000477F
		public TagMappings()
		{
			this.mappings = new Dictionary<string, Type>();
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00006592 File Offset: 0x00004792
		public TagMappings(IDictionary<string, Type> mappings)
		{
			this.mappings = new Dictionary<string, Type>(mappings);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000065A6 File Offset: 0x000047A6
		public void Add(string tag, Type mapping)
		{
			this.mappings.Add(tag, mapping);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000065B8 File Offset: 0x000047B8
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

		// Token: 0x0400006C RID: 108
		private readonly Dictionary<string, Type> mappings;
	}
}
