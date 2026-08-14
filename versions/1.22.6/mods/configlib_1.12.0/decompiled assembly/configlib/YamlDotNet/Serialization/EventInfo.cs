using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200008C RID: 140
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class EventInfo
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00010C2D File Offset: 0x0000EE2D
		public IObjectDescriptor Source { get; }

		// Token: 0x06000396 RID: 918 RVA: 0x00010C35 File Offset: 0x0000EE35
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
