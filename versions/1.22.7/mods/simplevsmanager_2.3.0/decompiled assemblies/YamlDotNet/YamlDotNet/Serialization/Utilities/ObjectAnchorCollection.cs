using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x0200004D RID: 77
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ObjectAnchorCollection
	{
		// Token: 0x06000216 RID: 534 RVA: 0x00006FAF File Offset: 0x000051AF
		public void Add(string anchor, object @object)
		{
			this.objectsByAnchor.Add(anchor, @object);
			if (@object != null)
			{
				this.anchorsByObject.Add(@object, anchor);
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00006FCE File Offset: 0x000051CE
		public bool TryGetAnchor(object @object, [Nullable(2)] [MaybeNullWhen(false)] out string anchor)
		{
			return this.anchorsByObject.TryGetValue(@object, ref anchor);
		}

		// Token: 0x17000043 RID: 67
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

		// Token: 0x04000083 RID: 131
		private readonly Dictionary<string, object> objectsByAnchor = new Dictionary<string, object>();

		// Token: 0x04000084 RID: 132
		private readonly Dictionary<object, string> anchorsByObject = new Dictionary<object, string>();
	}
}
