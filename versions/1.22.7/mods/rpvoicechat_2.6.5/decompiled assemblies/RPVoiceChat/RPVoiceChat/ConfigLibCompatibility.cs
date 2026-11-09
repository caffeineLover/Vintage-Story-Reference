using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat
{
	// Token: 0x02000013 RID: 19
	public class ConfigLibCompatibility : ModSystem
	{
		// Token: 0x0600006B RID: 107 RVA: 0x000041BB File Offset: 0x000023BB
		public override double ExecuteOrder()
		{
			return 0.1;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000041C6 File Offset: 0x000023C6
		public override bool ShouldLoad(EnumAppSide forSide)
		{
			return forSide == 2;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000041CC File Offset: 0x000023CC
		public override void StartClientSide(ICoreClientAPI api)
		{
			if (!api.ModLoader.IsModEnabled("configlib"))
			{
				api.Logger.Notification("[RPVoiceChat] ConfigLib not detected. In-game config GUI will not be available.");
				api.Logger.Notification("[RPVoiceChat] You can still edit configs manually in ModConfig folder.");
				return;
			}
			try
			{
				this.RegisterConfigLibIntegration(api);
				api.Logger.Notification("[RPVoiceChat] ConfigLib integration enabled successfully!");
			}
			catch (Exception e)
			{
				api.Logger.Error("[RPVoiceChat] Failed to integrate with ConfigLib: " + e.Message);
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004254 File Offset: 0x00002454
		private void RegisterConfigLibIntegration(ICoreClientAPI api)
		{
		}

		// Token: 0x04000023 RID: 35
		private const string ConfigLibModId = "configlib";
	}
}
