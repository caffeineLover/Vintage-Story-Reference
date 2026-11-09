using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000088 RID: 136
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class EventInfo
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000359 RID: 857 RVA: 0x000102E5 File Offset: 0x0000E4E5
		public IObjectDescriptor Source { get; }

		// Token: 0x0600035A RID: 858 RVA: 0x000102ED File Offset: 0x0000E4ED
		protected EventInfo(IObjectDescriptor source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			this.Source = source;
		}
	}
}
