using System;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace Cats
{
	// Token: 0x02000004 RID: 4
	public class Cats : ModSystem
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002526 File Offset: 0x00000726
		public override void Start(ICoreAPI api)
		{
			base.Start(api);
			AiTaskRegistry.Register<AiTaskFreezeNear>("freezeNear");
			AiTaskRegistry.Register<AiTaskPlayWithEntity>("playWithEntity");
		}
	}
}
