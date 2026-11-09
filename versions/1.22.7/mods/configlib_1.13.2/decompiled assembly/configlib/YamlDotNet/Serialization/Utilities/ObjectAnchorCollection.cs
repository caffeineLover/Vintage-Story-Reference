using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x020000FC RID: 252
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ObjectAnchorCollection
	{
		// Token: 0x06000589 RID: 1417 RVA: 0x0001425F File Offset: 0x0001245F
		public void Add(string anchor, object @object)
		{
			this.objectsByAnchor.Add(anchor, @object);
			if (@object != null)
			{
				this.anchorsByObject.Add(@object, anchor);
			}
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001427E File Offset: 0x0001247E
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

		// Token: 0x04000271 RID: 625
		private readonly IDictionary<string, object> objectsByAnchor = new Dictionary<string, object>();

		// Token: 0x04000272 RID: 626
		private readonly IDictionary<object, string> anchorsByObject = new Dictionary<object, string>();
	}
}
