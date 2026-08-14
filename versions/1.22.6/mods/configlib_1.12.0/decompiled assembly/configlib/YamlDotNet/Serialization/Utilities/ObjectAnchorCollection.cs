using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x02000100 RID: 256
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ObjectAnchorCollection
	{
		// Token: 0x060005C5 RID: 1477 RVA: 0x00014BA7 File Offset: 0x00012DA7
		public void Add(string anchor, object @object)
		{
			this.objectsByAnchor.Add(anchor, @object);
			if (@object != null)
			{
				this.anchorsByObject.Add(@object, anchor);
			}
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00014BC6 File Offset: 0x00012DC6
		public bool TryGetAnchor(object @object, [Nullable(2)] [MaybeNullWhen(false)] out string anchor)
		{
			return this.anchorsByObject.TryGetValue(@object, ref anchor);
		}

		// Token: 0x170000A1 RID: 161
		public object this[string anchor]
		{
			get
			{
				object result;
				if (this.objectsByAnchor.TryGetValue(anchor, ref result))
				{
					return result;
				}
				throw new AnchorNotFoundException("The anchor '" + anchor + "' does not exists");
			}
		}

		// Token: 0x0400029F RID: 671
		private readonly IDictionary<string, object> objectsByAnchor = new Dictionary<string, object>();

		// Token: 0x040002A0 RID: 672
		private readonly IDictionary<object, string> anchorsByObject = new Dictionary<object, string>();
	}
}
