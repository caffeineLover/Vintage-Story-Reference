using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat
{
	// Token: 0x02000013 RID: 19
	public class ConfigLibCompatibility : ModSystem
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00004097 File Offset: 0x00002297
		public override double ExecuteOrder()
		{
			return 0.1;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000040A2 File Offset: 0x000022A2
		public override bool ShouldLoad(EnumAppSide forSide)
		{
			return forSide == 2;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000040A8 File Offset: 0x000022A8
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

		// Token: 0x0600006D RID: 109 RVA: 0x00004130 File Offset: 0x00002330
		private void RegisterConfigLibIntegration(ICoreClientAPI api)
		{
		}

		// Token: 0x04000022 RID: 34
		private const string ConfigLibModId = "configlib";
	}
}
