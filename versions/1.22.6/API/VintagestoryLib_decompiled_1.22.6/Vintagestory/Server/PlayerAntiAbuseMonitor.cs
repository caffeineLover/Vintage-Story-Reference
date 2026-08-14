using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.Server;

public class PlayerAntiAbuseMonitor : ServerSystem
{
	public Dictionary<string, PlayerBlockBreaks> BlockBreakEventsByPlayer = new Dictionary<string, PlayerBlockBreaks>();

	public static int BufferSize { get; set; }

	public static int TriggerOnBlockBreakCount { get; set; }

	public static int TriggerOnDurationMs { get; set; }

	public static double BlockBurstAbuseBanDays { get; set; }

	public PlayerAntiAbuseMonitor(ServerMain server)
		: base(server)
	{
		server.EventManager.DidBreakBlock += EventManager_DidBreakBlock;
	}

	public override void OnBeginModsAndConfigReady()
	{
		BufferSize = server.Config.AntiAbuseBufferSize;
		TriggerOnBlockBreakCount = server.Config.AntiAbuseTriggerOnBlockBreakCount;
		TriggerOnDurationMs = server.Config.AntiAbuseTriggerOnDurationMs;
		BlockBurstAbuseBanDays = server.Config.AntiAbuseBlockBurstAbuseBanDays;
		base.OnBeginModsAndConfigReady();
	}

	private void EventManager_DidBreakBlock(IServerPlayer byPlayer, int oldblockId, BlockSelection blockSel)
	{
		PlayerBlockBreaks p = BlockBreakEventsByPlayer[byPlayer.PlayerUID];
		p.TotalEvents++;
		p.EventsRingBuffer[p.LastIndex] = server.ElapsedMilliseconds;
		p.LastIndex = (ushort)((p.LastIndex + 1) % BufferSize);
	}

	public override int GetUpdateInterval()
	{
		return 1000;
	}

	public override void OnServerTick(float dt)
	{
		if (server.Config.AntiAbuse == EnumProtectionLevel.Off)
		{
			return;
		}
		foreach (string uid in new List<string>(BlockBreakEventsByPlayer.Keys))
		{
			if (server.GetClientByUID(uid) == null && server.ElapsedMilliseconds - BlockBreakEventsByPlayer[uid].LogoutTotalMs > 2000)
			{
				BlockBreakEventsByPlayer.Remove(uid);
			}
		}
		foreach (KeyValuePair<string, PlayerBlockBreaks> item in BlockBreakEventsByPlayer)
		{
			item.Deconstruct(out var key, out var value);
			string uid2 = key;
			PlayerBlockBreaks pbreaks = value;
			IPlayer player = server.PlayerByUid(uid2);
			if (player.HasPrivilege(Privilege.gamemode) || player.HasPrivilege(Privilege.controlserver) || pbreaks.TotalEvents < TriggerOnBlockBreakCount)
			{
				continue;
			}
			int searchSize = Math.Min(pbreaks.TotalEvents, BufferSize);
			int firstIndex = GameMath.Mod(pbreaks.LastIndex - searchSize, BufferSize);
			for (int start = 0; start <= searchSize - TriggerOnBlockBreakCount; start++)
			{
				int end = start + TriggerOnBlockBreakCount - 1;
				int startPos = (firstIndex + start) % BufferSize;
				int endPos = (firstIndex + end) % BufferSize;
				if (pbreaks.EventsRingBuffer[endPos] - pbreaks.EventsRingBuffer[startPos] <= TriggerOnDurationMs)
				{
					BlockBreakEventsByPlayer[uid2] = new PlayerBlockBreaks();
					moderatePlayer(uid2);
					break;
				}
			}
		}
	}

	private void moderatePlayer(string uid)
	{
		IServerPlayer player = server.PlayerByUid(uid) as IServerPlayer;
		DateTime untilDate = DateTime.Now.AddDays(BlockBurstAbuseBanDays);
		string logReason = Lang.Get("Player {0}, uid {1} exceeded the block break burst limit of {2} over the course of {3} milliseconds.", player.PlayerName, uid, TriggerOnBlockBreakCount, TriggerOnDurationMs);
		string clientReason = Lang.GetL(player.LanguageCode, "You have exceeded the block break burst limit of this server. You have been banned for {0} days", BlockBurstAbuseBanDays);
		string otherClientReason = Lang.Get("Player {0} has been banned for exceeding the block break burst limit.", player.PlayerName);
		ServerMain.Logger.Audit(logReason);
		server.PlayerDataManager.BanPlayer(player.PlayerName, uid, "anti abuse system", otherClientReason, untilDate);
		ConnectedClient targetClient = server.GetClientByUID(uid);
		if (targetClient != null)
		{
			server.DisconnectPlayer(targetClient, otherClientReason, clientReason);
		}
	}

	public override void OnPlayerJoin(ServerPlayer player)
	{
		if (!BlockBreakEventsByPlayer.ContainsKey(player.PlayerUID))
		{
			BlockBreakEventsByPlayer[player.PlayerUID] = new PlayerBlockBreaks();
		}
	}

	public override void OnPlayerDisconnect(ServerPlayer player)
	{
		if (BlockBreakEventsByPlayer.ContainsKey(player.PlayerUID))
		{
			BlockBreakEventsByPlayer[player.PlayerUID].LogoutTotalMs = server.ElapsedMilliseconds;
		}
	}
}
