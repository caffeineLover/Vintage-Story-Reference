using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000E6 RID: 230
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TagMappings
	{
		// Token: 0x0600052C RID: 1324 RVA: 0x000134DF File Offset: 0x000116DF
		public TagMappings()
		{
			this.mappings = new Dictionary<string, Type>();
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x000134F2 File Offset: 0x000116F2
		public TagMappings(IDictionary<string, Type> mappings)
		{
			this.mappings = new Dictionary<string, Type>(mappings);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00013506 File Offset: 0x00011706
		public void Add(string tag, Type mapping)
		{
			this.mappings.Add(tag, mapping);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00013518 File Offset: 0x00011718
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

		// Token: 0x0400024B RID: 587
		private readonly IDictionary<string, Type> mappings;
	}
}
