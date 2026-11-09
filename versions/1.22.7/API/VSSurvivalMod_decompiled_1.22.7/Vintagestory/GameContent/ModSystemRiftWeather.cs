using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class ModSystemRiftWeather : ModSystem
{
	private ICoreServerAPI sapi;

	private Dictionary<string, EntityProperties> drifterProps = new Dictionary<string, EntityProperties>();

	private Dictionary<string, int> defaultSpawnCaps = new Dictionary<string, int>();

	private CurrentPattern curPattern;

	private RiftWeatherConfig config;

	private Dictionary<string, SpawnPattern> patternsByCode = new Dictionary<string, SpawnPattern>();

	public bool Enabled = true;

	public SpawnPattern CurrentPattern => patternsByCode[curPattern.Code];

	public double CurrentPatternUntilHours => curPattern.UntilTotalHours;

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return true;
	}

	public override void Start(ICoreAPI api)
	{
		api.Network.RegisterChannel("riftWeather").RegisterMessageType<SpawnPatternPacket>();
	}

	public override void StartClientSide(ICoreClientAPI api)
	{
		api.Network.GetChannel("riftWeather").SetMessageHandler<SpawnPatternPacket>(onPacket);
		api.ModLoader.GetModSystem<CharacterExtraDialogs>().OnEnvText += ModSystemDrifterWeather_OnEnvText;
	}

	private void ModSystemDrifterWeather_OnEnvText(StringBuilder sb)
	{
		if (curPattern != null)
		{
			sb.AppendLine();
			sb.Append(Lang.Get("Rift activity: {0}", Lang.Get("rift-activity-" + curPattern.Code)));
		}
	}

	private void onPacket(SpawnPatternPacket msg)
	{
		curPattern = msg.Pattern;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		IAsset asset = api.Assets.TryGet("config/riftweather.json");
		if (asset == null)
		{
			api.Logger.Warning("config/riftweather.json file missing. Riftweather will be disabled.");
			Enabled = false;
			return;
		}
		config = asset.ToObject<RiftWeatherConfig>();
		SpawnPattern[] patterns = config.Patterns;
		foreach (SpawnPattern p in patterns)
		{
			patternsByCode[p.Code] = p;
		}
		sapi = api;
		base.StartServerSide(api);
		api.Event.RegisterGameTickListener(onServerTick, 2000);
		api.Event.ServerRunPhase(EnumServerRunPhase.RunGame, onRunGame);
		api.Event.SaveGameLoaded += Event_SaveGameLoaded;
		api.Event.GameWorldSave += Event_GameWorldSave;
		api.Event.SaveGameCreated += Event_SaveGameCreated;
		api.Event.PlayerJoin += Event_PlayerJoin;
		sapi.ChatCommands.Create("dweather").WithDescription("Show the current rift activity").RequiresPrivilege(Privilege.controlserver)
			.HandleWith((TextCommandCallingArgs _) => TextCommandResult.Success("Current rift activity: " + curPattern.Code + "\nwill continue for " + (int)(curPattern.UntilTotalHours - sapi.World.Calendar.ElapsedHours + 0.5) + " more hours"))
			.BeginSubCommand("reset")
			.WithDescription("Reset the current rift activity")
			.HandleWith(delegate
			{
				choosePattern();
				return TextCommandResult.Success("Rift activity reset");
			});
	}

	private void Event_PlayerJoin(IServerPlayer byPlayer)
	{
		sapi.Network.GetChannel("riftWeather").SendPacket(new SpawnPatternPacket
		{
			Pattern = curPattern
		}, byPlayer);
	}

	private void Event_SaveGameCreated()
	{
		choosePattern();
	}

	private void Event_GameWorldSave()
	{
		sapi.WorldManager.SaveGame.StoreData("riftweather", SerializerUtil.Serialize(curPattern));
	}

	private void Event_SaveGameLoaded()
	{
		try
		{
			byte[] data = sapi.WorldManager.SaveGame.GetData("riftweather");
			string savegameVersion = sapi.WorldManager.SaveGame.LastSavedGameVersion;
			if (data != null && !GameVersion.IsLowerVersionThan(savegameVersion, "1.21.2"))
			{
				curPattern = SerializerUtil.Deserialize<CurrentPattern>(data);
			}
		}
		catch
		{
		}
	}

	private void choosePattern()
	{
		float weightSum = 0f;
		List<SpawnPattern> patterns = new List<SpawnPattern>();
		double elapsedHours = sapi.World.Calendar.ElapsedHours;
		for (int i = 0; i < config.Patterns.Length; i++)
		{
			SpawnPattern pattern = config.Patterns[i];
			if (pattern.StartTotalHours <= elapsedHours)
			{
				patterns.Add(pattern);
				weightSum += pattern.Chance;
			}
		}
		SpawnPattern chosenPattern = patterns[patterns.Count - 1];
		float val = (float)sapi.World.Rand.NextDouble() * weightSum;
		foreach (SpawnPattern pattern2 in patterns)
		{
			val -= pattern2.Chance;
			if (val <= 0f)
			{
				chosenPattern = pattern2;
				break;
			}
		}
		curPattern = new CurrentPattern
		{
			Code = chosenPattern.Code,
			UntilTotalHours = elapsedHours + (double)chosenPattern.DurationHours.nextFloat(1f, sapi.World.Rand)
		};
		sapi.Network.GetChannel("riftWeather").BroadcastPacket(new SpawnPatternPacket
		{
			Pattern = curPattern
		});
	}

	private void onRunGame()
	{
		if (curPattern == null)
		{
			choosePattern();
		}
		foreach (EntityProperties type in sapi.World.EntityTypes)
		{
			if (type.Code.Path == "drifter-normal" || type.Code.Path == "drifter-deep")
			{
				drifterProps[type.Code.Path] = type;
				defaultSpawnCaps[type.Code.Path] = type.Server.SpawnConditions.Runtime.MaxQuantity;
				break;
			}
		}
	}

	private void onServerTick(float dt)
	{
		if (drifterProps == null)
		{
			return;
		}
		float qmul = patternsByCode[curPattern.Code].MobSpawnMul;
		foreach (KeyValuePair<string, EntityProperties> val in drifterProps)
		{
			val.Value.Server.SpawnConditions.Runtime.MaxQuantity = Math.Max(0, (int)((float)defaultSpawnCaps[val.Key] * qmul));
		}
		if (curPattern.UntilTotalHours < sapi.World.Calendar.ElapsedHours)
		{
			choosePattern();
		}
	}
}
