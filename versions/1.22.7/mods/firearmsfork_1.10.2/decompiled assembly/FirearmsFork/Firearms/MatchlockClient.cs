using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Firearms
{
	// Token: 0x02000015 RID: 21
	[NullableContext(1)]
	[Nullable(0)]
	public class MatchlockClient : MuzzleloaderClient
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x0000498B File Offset: 0x00002B8B
		public MatchlockClient(ICoreClientAPI api, Item item) : base(api, item)
		{
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004995 File Offset: 0x00002B95
		protected override bool ShootCallback(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			base.SetState<MuzzleloaderState>(MuzzleloaderState.Aim, true);
			return true;
		}
	}
}
