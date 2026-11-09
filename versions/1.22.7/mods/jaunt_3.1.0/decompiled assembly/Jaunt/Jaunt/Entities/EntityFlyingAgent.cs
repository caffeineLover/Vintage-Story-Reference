using System;

namespace Jaunt.Entities
{
	// Token: 0x0200000A RID: 10
	public class EntityFlyingAgent : EntityJauntAgent
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000046 RID: 70 RVA: 0x0000393C File Offset: 0x00001B3C
		public override bool CanStepPitch
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000047 RID: 71 RVA: 0x0000393F File Offset: 0x00001B3F
		public override bool CanSwivel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003942 File Offset: 0x00001B42
		public override bool CanSwivelNow
		{
			get
			{
				return true;
			}
		}
	}
}
