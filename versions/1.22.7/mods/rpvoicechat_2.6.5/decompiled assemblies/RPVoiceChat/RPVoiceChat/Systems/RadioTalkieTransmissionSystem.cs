using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using RPVoiceChat.Config;
using RPVoiceChat.GameContent.Items;
using RPVoiceChat.Server;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000044 RID: 68
	public class RadioTalkieTransmissionSystem : ModSystem
	{
		// Token: 0x0600018B RID: 395 RVA: 0x0000B470 File Offset: 0x00009670
		public override void StartServerSide(ICoreServerAPI api)
		{
			this.sapi = api;
			this.routing = api.ModLoader.GetModSystem<RadioVoiceRoutingSystem>(true);
			api.Event.RegisterGameTickListener(new Action<float>(this.OnServerTick), 250, 0);
			api.Event.PlayerDisconnect += new PlayerDelegate(this.OnPlayerDisconnect);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000B4CB File Offset: 0x000096CB
		public bool IsTalkieTransmitting(string playerUid)
		{
			return !string.IsNullOrEmpty(playerUid) && this.activeTalkieFrequencies.ContainsKey(playerUid);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000B4E4 File Offset: 0x000096E4
		public void SetTalkieTransmitting(IServerPlayer player, bool transmitting, string frequency)
		{
			if (player == null)
			{
				return;
			}
			if (transmitting && !string.IsNullOrWhiteSpace(frequency))
			{
				string normalized = RadioFrequencyUtil.Normalize(frequency);
				this.activeTalkieFrequencies[player.PlayerUID] = normalized;
				this.ApplyTalkieRoute(player, normalized);
				return;
			}
			string text;
			this.activeTalkieFrequencies.TryRemove(player.PlayerUID, ref text);
			RadioVoiceRoutingSystem radioVoiceRoutingSystem = this.routing;
			if (radioVoiceRoutingSystem == null)
			{
				return;
			}
			radioVoiceRoutingSystem.ClearTalkieRoute(player.PlayerUID);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000B54C File Offset: 0x0000974C
		private void OnPlayerDisconnect(IServerPlayer player)
		{
			if (player == null)
			{
				return;
			}
			string text;
			this.activeTalkieFrequencies.TryRemove(player.PlayerUID, ref text);
			RadioVoiceRoutingSystem radioVoiceRoutingSystem = this.routing;
			if (radioVoiceRoutingSystem == null)
			{
				return;
			}
			radioVoiceRoutingSystem.ClearRoute(player.PlayerUID);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000B588 File Offset: 0x00009788
		private void OnServerTick(float dt)
		{
			if (this.sapi == null || this.routing == null)
			{
				return;
			}
			foreach (KeyValuePair<string, string> entry in this.activeTalkieFrequencies)
			{
				IServerPlayer player = this.sapi.World.PlayerByUid(entry.Key) as IServerPlayer;
				bool flag;
				if (player == null)
				{
					flag = (null != null);
				}
				else
				{
					EntityPlayer entity = player.Entity;
					flag = (((entity != null) ? entity.Pos : null) != null);
				}
				if (!flag || !ItemRadio.IsTalkieActiveInHands(player))
				{
					string text;
					this.activeTalkieFrequencies.TryRemove(entry.Key, ref text);
					this.routing.ClearTalkieRoute(entry.Key);
				}
				else
				{
					this.ApplyTalkieRoute(player, entry.Value);
				}
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000B658 File Offset: 0x00009858
		private void ApplyTalkieRoute(IServerPlayer player, string frequency)
		{
			if (this.routing != null)
			{
				bool flag;
				if (player == null)
				{
					flag = (null != null);
				}
				else
				{
					EntityPlayer entity = player.Entity;
					flag = (((entity != null) ? entity.Pos : null) != null);
				}
				if (flag && !string.IsNullOrEmpty(frequency))
				{
					Vec3d emissionPos = player.Entity.Pos.XYZ;
					this.routing.SetTalkieRoutes(player.PlayerUID, new VoiceRoute[]
					{
						new VoiceRoute(emissionPos, ServerConfigManager.RadioTalkieRangeBlocks, player.Entity.Pos.Dimension, frequency, true)
					});
					return;
				}
			}
		}

		// Token: 0x040000AF RID: 175
		private readonly ConcurrentDictionary<string, string> activeTalkieFrequencies = new ConcurrentDictionary<string, string>();

		// Token: 0x040000B0 RID: 176
		private ICoreServerAPI sapi;

		// Token: 0x040000B1 RID: 177
		private RadioVoiceRoutingSystem routing;
	}
}
