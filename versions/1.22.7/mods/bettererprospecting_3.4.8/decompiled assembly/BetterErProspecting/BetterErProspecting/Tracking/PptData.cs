using System;
using System.Runtime.CompilerServices;
using System.Threading;
using ProtoBuf;

namespace BetterErProspecting.Tracking
{
	// Token: 0x02000003 RID: 3
	[ProtoContract]
	public class PptData
	{
		// Token: 0x06000010 RID: 16 RVA: 0x000023C4 File Offset: 0x000005C4
		public void Update(double ppt)
		{
			using (this._lock.EnterScope())
			{
				if (ppt < this.MinPpt)
				{
					this.MinPpt = ppt;
				}
				if (ppt > this.MaxPpt)
				{
					this.MaxPpt = ppt;
				}
			}
		}

		// Token: 0x04000005 RID: 5
		[ProtoMember(1)]
		public double MinPpt = 1000.0;

		// Token: 0x04000006 RID: 6
		[ProtoMember(2)]
		public double MaxPpt;

		// Token: 0x04000007 RID: 7
		[Nullable(1)]
		[ProtoIgnore]
		private readonly Lock _lock = new Lock();
	}
}
