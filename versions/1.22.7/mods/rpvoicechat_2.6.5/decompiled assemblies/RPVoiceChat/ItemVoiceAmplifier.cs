using System;
using RPVoiceChat;
using RPVoiceChat.Audio;
using RPVoiceChat.Config;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

// Token: 0x02000011 RID: 17
public class ItemVoiceAmplifier : Item
{
	// Token: 0x0600005C RID: 92 RVA: 0x00003E54 File Offset: 0x00002054
	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		JsonObject attr = this.Attributes;
		if (attr != null)
		{
			JsonObject jsonObject = attr["applySoundEffect"];
			this.soundEffectName = ((jsonObject != null) ? jsonObject.AsString(null) : null);
			JsonObject jsonObject2 = attr["ignoreDistanceReduction"];
			this.ignoreDistanceReduction = (jsonObject2 != null && jsonObject2.AsBool(false));
			JsonObject jsonObject3 = attr["wallThicknessOverride"];
			this.wallThicknessOverride = ((jsonObject3 != null) ? jsonObject3.AsFloat(-1f) : -1f);
			JsonObject jsonObject4 = attr["globalBroadcast"];
			this.globalBroadcast = (jsonObject4 != null && jsonObject4.AsBool(false));
		}
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00003EF8 File Offset: 0x000020F8
	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		if (!firstEvent || this.isAmplifierActive)
		{
			handling = 4;
			return;
		}
		ICoreClientAPI capi = byEntity.Api as ICoreClientAPI;
		if (capi != null && byEntity == capi.World.Player.Entity)
		{
			MicrophoneManager microphoneManager = RPVoiceChatClient.MicrophoneManagerInstance;
			AudioOutputManager audioOutputManager = RPVoiceChatClient.AudioOutputManagerInstance;
			if (microphoneManager != null)
			{
				this.originalVoiceLevel = microphoneManager.GetVoiceLevel();
				this.originalTransmissionRange = microphoneManager.GetTransmissionRange();
				JsonObject jsonObject;
				if (slot == null)
				{
					jsonObject = null;
				}
				else
				{
					ItemStack itemstack = slot.Itemstack;
					if (itemstack == null)
					{
						jsonObject = null;
					}
					else
					{
						CollectibleObject collectible = itemstack.Collectible;
						jsonObject = ((collectible != null) ? collectible.Attributes : null);
					}
				}
				JsonObject attr = jsonObject;
				microphoneManager.SetVoiceLevel(VoiceLevel.Shouting);
				if (attr != null)
				{
					JsonObject jsonObject2 = attr["voiceRangeBlocks"];
					if (jsonObject2 != null && jsonObject2.Exists)
					{
						int defaultRangeBlocks = attr["voiceRangeBlocks"].AsInt(50);
						int rangeBlocks = this.GetConfiguredVoiceRange(this.api, defaultRangeBlocks);
						microphoneManager.SetTransmissionRange(rangeBlocks);
					}
				}
				if (this.ignoreDistanceReduction)
				{
					microphoneManager.SetIgnoreDistanceReduction(true);
				}
				if (this.wallThicknessOverride >= 0f)
				{
					microphoneManager.SetWallThicknessOverride(this.wallThicknessOverride);
				}
				if (this.globalBroadcast)
				{
					microphoneManager.SetGlobalBroadcast(true);
				}
				this.isAmplifierActive = true;
			}
			if (!string.IsNullOrEmpty(this.soundEffectName) && audioOutputManager != null)
			{
				string playerUID = capi.World.Player.PlayerUID;
				audioOutputManager.ApplyEffectToPlayer(playerUID, this.soundEffectName);
			}
		}
		handling = 4;
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00004050 File Offset: 0x00002250
	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		if (!this.isAmplifierActive)
		{
			return;
		}
		ICoreClientAPI capi = byEntity.Api as ICoreClientAPI;
		if (capi != null && byEntity == capi.World.Player.Entity)
		{
			MicrophoneManager microphoneManager = RPVoiceChatClient.MicrophoneManagerInstance;
			AudioOutputManager audioOutputManager = RPVoiceChatClient.AudioOutputManagerInstance;
			if (microphoneManager != null)
			{
				microphoneManager.SetVoiceLevel(this.originalVoiceLevel);
				microphoneManager.SetTransmissionRange(this.originalTransmissionRange);
				microphoneManager.SetIgnoreDistanceReduction(false);
				microphoneManager.ResetWallThicknessOverride();
				microphoneManager.ResetGlobalBroadcast();
			}
			if (!string.IsNullOrEmpty(this.soundEffectName) && audioOutputManager != null)
			{
				string playerUID = capi.World.Player.PlayerUID;
				audioOutputManager.ClearEffectForPlayer(playerUID);
			}
			this.isAmplifierActive = false;
		}
	}

	// Token: 0x0600005F RID: 95 RVA: 0x000040F2 File Offset: 0x000022F2
	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		return this.isAmplifierActive;
	}

	// Token: 0x06000060 RID: 96 RVA: 0x000040FA File Offset: 0x000022FA
	public override void OnHeldIdle(ItemSlot slot, EntityAgent byEntity)
	{
		if (this.isAmplifierActive)
		{
			this.OnHeldInteractStop(0f, slot, byEntity, null, null);
		}
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00004113 File Offset: 0x00002313
	private int GetConfiguredVoiceRange(ICoreAPI api, int defaultRange)
	{
		if (api.Side != 1)
		{
			return defaultRange;
		}
		if (base.GetType().Name.ToLower() == "voiceamplifier")
		{
			return this.GetVoiceAmplifierRange();
		}
		return defaultRange;
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00004144 File Offset: 0x00002344
	private int GetVoiceAmplifierRange()
	{
		AssetLocation code = this.Code;
		string text;
		if (code == null)
		{
			text = null;
		}
		else
		{
			string text2 = code.ToString();
			text = ((text2 != null) ? text2.ToLower() : null);
		}
		if ((text ?? "") == "megaphone")
		{
			return ServerConfigManager.MegaphoneAudibleDistance;
		}
		JsonObject attributes = this.Attributes;
		if (attributes == null)
		{
			return 125;
		}
		return attributes["voiceRangeBlocks"].AsInt(125);
	}

	// Token: 0x0400001C RID: 28
	private string soundEffectName;

	// Token: 0x0400001D RID: 29
	private bool isAmplifierActive;

	// Token: 0x0400001E RID: 30
	private VoiceLevel originalVoiceLevel;

	// Token: 0x0400001F RID: 31
	private int originalTransmissionRange;

	// Token: 0x04000020 RID: 32
	private bool ignoreDistanceReduction;

	// Token: 0x04000021 RID: 33
	private float wallThicknessOverride = -1f;

	// Token: 0x04000022 RID: 34
	private bool globalBroadcast;
}
