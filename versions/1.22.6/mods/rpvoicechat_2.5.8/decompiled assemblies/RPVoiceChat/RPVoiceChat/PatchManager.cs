using System;
using HarmonyLib;
using Vintagestory.API.Common;

namespace RPVoiceChat
{
	// Token: 0x0200001C RID: 28
	internal class PatchManager : IDisposable
	{
		// Token: 0x0600008E RID: 142 RVA: 0x00004C78 File Offset: 0x00002E78
		public PatchManager(string harmonyId)
		{
			this.harmony = new Harmony(harmonyId);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004C8C File Offset: 0x00002E8C
		public void Patch(ICoreAPI api)
		{
			if ((api.Side & 2) != null)
			{
				this.PatchClient();
			}
			if ((api.Side & 1) != null)
			{
				this.PatchServer();
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004CB0 File Offset: 0x00002EB0
		private void PatchClient()
		{
			EntityNameTagRendererRegistryPatch.Patch(this.harmony);
			GuiDialogCreateCharacterPatch.Patch(this.harmony);
			GuiElementSliderPatch.Patch(this.harmony);
			LoadedSoundNativePatch.Patch(this.harmony);
			SystemNetworkProcessPatch.Patch(this.harmony);
			HammerWeldingPatch.Patch(this.harmony);
			HelveHammerPatch.Patch(this.harmony);
			BlockCanPlacePatch.Patch(this.harmony);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004D15 File Offset: 0x00002F15
		private void PatchServer()
		{
			NetworkAPIPatch.Patch(this.harmony);
			TcpNetServerPatch.Patch(this.harmony);
			HelveHammerPatch.Patch(this.harmony);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004D38 File Offset: 0x00002F38
		public void Unpatch()
		{
			this.harmony.UnpatchAll(this.harmony.Id);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004D50 File Offset: 0x00002F50
		public void Dispose()
		{
			this.Unpatch();
		}

		// Token: 0x0400002D RID: 45
		private Harmony harmony;
	}
}
