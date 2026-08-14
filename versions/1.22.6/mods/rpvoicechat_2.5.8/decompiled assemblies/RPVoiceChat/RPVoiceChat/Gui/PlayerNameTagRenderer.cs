using System;
using System.Collections.Generic;
using RPVoiceChat.Audio;
using RPVoiceChat.Config;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000075 RID: 117
	public class PlayerNameTagRenderer
	{
		// Token: 0x06000357 RID: 855 RVA: 0x00012F27 File Offset: 0x00011127
		public PlayerNameTagRenderer(ICoreClientAPI api, AudioOutputManager audioOutputManager)
		{
			PlayerNameTagRenderer.Init(api, audioOutputManager);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00012F36 File Offset: 0x00011136
		public static void Init(ICoreClientAPI api, AudioOutputManager audioOutputManager = null)
		{
			PlayerNameTagRenderer.capi = api;
			if (audioOutputManager != null)
			{
				PlayerNameTagRenderer._audioOutputManager = audioOutputManager;
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00012F48 File Offset: 0x00011148
		public static LoadedTexture GetRenderer(EntityPlayer entity, double[] color = null, TextBackground bg = null)
		{
			ICoreClientAPI coreClientAPI = PlayerNameTagRenderer.capi;
			bool flag;
			if (coreClientAPI == null)
			{
				flag = (null != null);
			}
			else
			{
				IGuiAPI gui = coreClientAPI.Gui;
				flag = (((gui != null) ? gui.TextTexture : null) != null);
			}
			if (!flag)
			{
				return null;
			}
			string text;
			if (entity == null)
			{
				text = null;
			}
			else
			{
				EntityBehaviorNameTag behavior = entity.GetBehavior<EntityBehaviorNameTag>();
				text = ((behavior != null) ? behavior.DisplayName : null);
			}
			string playerName = text;
			if (playerName == null || playerName.Length == 0)
			{
				return null;
			}
			SyncedTreeAttribute watchedAttributes = entity.WatchedAttributes;
			if (((watchedAttributes != null) ? watchedAttributes.GetTreeAttribute("nametag") : null) == null)
			{
				return null;
			}
			AudioOutputManager audioOutputManager = PlayerNameTagRenderer._audioOutputManager;
			bool flag2 = audioOutputManager != null && audioOutputManager.IsPlayerTalking(entity.PlayerUID);
			if (color == null)
			{
				color = ColorUtil.WhiteArgbDouble;
			}
			CairoFont textColor = CairoFont.WhiteMediumText().WithColor(color);
			TextBackground textBackground;
			if ((textBackground = ((bg != null) ? bg.Clone() : null)) == null)
			{
				TextBackground textBackground2 = new TextBackground();
				textBackground2.FillColor = GuiStyle.DialogLightBgColor;
				textBackground2.Padding = 3;
				textBackground2.Radius = GuiStyle.ElementBGRadius;
				textBackground2.Shade = true;
				textBackground2.BorderColor = GuiStyle.DialogBorderColor;
				textBackground = textBackground2;
				textBackground2.BorderWidth = 3.0;
			}
			TextBackground textBg = textBackground;
			if (flag2)
			{
				textBg.FillColor = ColorUtil.Hex2Doubles("#529F51", 0.75);
				textBg.BorderColor = new double[]
				{
					default(double),
					1.0,
					default(double),
					0.3
				};
				textBg.BorderWidth = 5.0;
			}
			return PlayerNameTagRenderer.capi.Gui.TextTexture.GenUnscaledTextTexture(playerName, textColor, textBg);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x000130A0 File Offset: 0x000112A0
		private static bool ApplyNametagVisibilitySettings(ITreeAttribute nametagAttribute, bool isTalking)
		{
			if (nametagAttribute == null)
			{
				return false;
			}
			bool forceRender = WorldConfig.GetForceSpeakerNametag();
			bool targetOnly = WorldConfig.GetPlayerNametagTargetedOnly() && (!isTalking || !forceRender);
			if (nametagAttribute.GetBool("showtagonlywhentargeted", false) == targetOnly)
			{
				return false;
			}
			nametagAttribute.SetBool("showtagonlywhentargeted", targetOnly);
			return true;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x000130EC File Offset: 0x000112EC
		public static void SetNametagRenderRange(IPlayer player, int renderRange)
		{
			if (PlayerNameTagRenderer.capi == null)
			{
				return;
			}
			PlayerNameTagRenderer.capi.Event.EnqueueMainThreadTask(delegate()
			{
				IPlayer player2 = player;
				if (((player2 != null) ? player2.Entity : null) == null)
				{
					return;
				}
				ITreeAttribute nametagAttribute = player.Entity.WatchedAttributes.GetTreeAttribute("nametag");
				if (nametagAttribute == null)
				{
					return;
				}
				if (nametagAttribute.GetInt("renderRange", 0) == renderRange)
				{
					return;
				}
				nametagAttribute.SetInt("renderRange", renderRange);
				player.Entity.WatchedAttributes.MarkPathDirty("nametag");
			}, "rpvoicechat:SetNametagRenderRange");
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00013138 File Offset: 0x00011338
		public static void UpdatePlayerNameTag(IPlayer player, bool isTalking)
		{
			if (PlayerNameTagRenderer.capi != null)
			{
				IPlayer player2 = player;
				if (((player2 != null) ? player2.Entity : null) != null)
				{
					PlayerNameTagRenderer.capi.Event.EnqueueMainThreadTask(delegate()
					{
						IPlayer player3 = player;
						EntityPlayer entityPlayer = (player3 != null) ? player3.Entity : null;
						if (entityPlayer == null)
						{
							return;
						}
						string playerUID = entityPlayer.PlayerUID;
						bool wasTalking;
						bool talkStateChanged = !PlayerNameTagRenderer.lastTalkingStateByPlayer.TryGetValue(playerUID, ref wasTalking) || wasTalking != isTalking;
						SyncedTreeAttribute watchedAttributes = entityPlayer.WatchedAttributes;
						ITreeAttribute nametagAttribute = (watchedAttributes != null) ? watchedAttributes.GetTreeAttribute("nametag") : null;
						if (nametagAttribute == null)
						{
							return;
						}
						bool visibilityChanged = PlayerNameTagRenderer.ApplyNametagVisibilitySettings(nametagAttribute, isTalking);
						if (!talkStateChanged && !visibilityChanged)
						{
							return;
						}
						PlayerNameTagRenderer.lastTalkingStateByPlayer[playerUID] = isTalking;
						entityPlayer.WatchedAttributes.MarkPathDirty("nametag");
					}, "rpvoicechat:UpdatePlayerNameTag");
					return;
				}
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00013198 File Offset: 0x00011398
		public static void RefreshAllPlayerNameTags()
		{
			ICoreClientAPI coreClientAPI = PlayerNameTagRenderer.capi;
			if (((coreClientAPI != null) ? coreClientAPI.World : null) == null)
			{
				return;
			}
			PlayerNameTagRenderer.lastTalkingStateByPlayer.Clear();
			PlayerNameTagRenderer.capi.Event.EnqueueMainThreadTask(delegate()
			{
				foreach (IPlayer player in PlayerNameTagRenderer.capi.World.AllOnlinePlayers)
				{
					EntityPlayer entityPlayer = (player != null) ? player.Entity : null;
					if (entityPlayer != null)
					{
						AudioOutputManager audioOutputManager = PlayerNameTagRenderer._audioOutputManager;
						bool isTalking = audioOutputManager != null && audioOutputManager.IsPlayerTalking(player.PlayerUID);
						SyncedTreeAttribute watchedAttributes = entityPlayer.WatchedAttributes;
						ITreeAttribute nametagAttribute = (watchedAttributes != null) ? watchedAttributes.GetTreeAttribute("nametag") : null;
						if (nametagAttribute != null)
						{
							PlayerNameTagRenderer.ApplyNametagVisibilitySettings(nametagAttribute, isTalking);
							PlayerNameTagRenderer.lastTalkingStateByPlayer[player.PlayerUID] = isTalking;
							entityPlayer.WatchedAttributes.MarkPathDirty("nametag");
						}
					}
				}
			}, "rpvoicechat:RefreshAllPlayerNameTags");
		}

		// Token: 0x0600035E RID: 862 RVA: 0x000131F6 File Offset: 0x000113F6
		public static void CleanupPlayerNametagCache(string playerUID)
		{
			if (string.IsNullOrEmpty(playerUID))
			{
				return;
			}
			PlayerNameTagRenderer.lastTalkingStateByPlayer.Remove(playerUID);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0001320D File Offset: 0x0001140D
		public static void CleanupAllNametagCache()
		{
			PlayerNameTagRenderer.lastTalkingStateByPlayer.Clear();
		}

		// Token: 0x040001BA RID: 442
		private static ICoreClientAPI capi;

		// Token: 0x040001BB RID: 443
		private static AudioOutputManager _audioOutputManager;

		// Token: 0x040001BC RID: 444
		private static readonly Dictionary<string, bool> lastTalkingStateByPlayer = new Dictionary<string, bool>();
	}
}
