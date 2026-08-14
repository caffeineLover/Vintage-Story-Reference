using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200000E RID: 14
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class EventInfo
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003885 File Offset: 0x00001A85
		public IObjectDescriptor Source { get; }

		// Token: 0x06000082 RID: 130 RVA: 0x0000388D File Offset: 0x00001A8D
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
