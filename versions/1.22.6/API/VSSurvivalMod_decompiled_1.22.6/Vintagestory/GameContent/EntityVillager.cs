using System;
using System.Collections.Generic;
using System.IO;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class EntityVillager : EntityTradingHumanoid, ITalkUtil
{
	public EntityTalkUtil talkUtil;

	private MusicTrack track;

	private long handlerId;

	private long startLoadingMs;

	private bool wasStopped;

	protected string hairStylingCategory = "nadiyan";

	public Vintagestory.API.Datastructures.OrderedDictionary<string, TraderPersonality> Personalities => base.Properties.Attributes["personalities"].AsObject<Vintagestory.API.Datastructures.OrderedDictionary<string, TraderPersonality>>();

	public string Personality
	{
		get
		{
			return WatchedAttributes.GetString("personality", "balanced");
		}
		set
		{
			Vintagestory.API.Datastructures.OrderedDictionary<string, TraderPersonality> ps = Personalities;
			WatchedAttributes.SetString("personality", value);
			TalkUtil?.SetModifiers(ps[value].ChordDelayMul, ps[value].PitchModifier, ps[value].VolumneModifier);
		}
	}

	public string VoiceSound
	{
		get
		{
			if (!WatchedAttributes.HasAttribute("voiceSound"))
			{
				string[] sounds = base.Properties.Attributes["voiceSounds"].AsArray<string>();
				int index = Api.World.Rand.Next(sounds.Length);
				string sound = sounds[index];
				WatchedAttributes.SetString("voiceSound", sound);
				TalkUtil.soundName = AssetLocation.Create(sound, Code.Domain);
				return sound;
			}
			return WatchedAttributes.GetString("voiceSound");
		}
		set
		{
			WatchedAttributes.SetString("voiceSound", value);
			TalkUtil.soundName = AssetLocation.Create(value, Code.Domain);
		}
	}

	public override EntityTalkUtil TalkUtil => talkUtil;

	public EntityVillager()
	{
		AnimManager = new PersonalizedAnimationManager();
	}

	public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
	{
		(AnimManager as PersonalizedAnimationManager).All = true;
		string personality = null;
		if (api.Side == EnumAppSide.Server)
		{
			JsonObject personalityAttribute = properties.Attributes["personality"];
			personality = ((!personalityAttribute.Exists) ? Personalities.GetKeyAtIndex(api.World.Rand.Next(Personalities.Count)) : personalityAttribute.AsString());
			(AnimManager as PersonalizedAnimationManager).Personality = personality;
			WatchedAttributes.SetString("personality", personality);
		}
		base.Initialize(properties, api, InChunkIndex3d);
		if (api.Side == EnumAppSide.Client)
		{
			personality = Personality;
			talkUtil = new EntityTalkUtil(api as ICoreClientAPI, this, properties.Attributes["isMultiSoundVoice"].AsBool(defaultValue: true));
			TalkUtil.soundName = AssetLocation.Create(VoiceSound, Code.Domain);
		}
		Personality = personality;
	}

	public override void OnReceivedServerPacket(int packetid, byte[] data)
	{
		base.OnReceivedServerPacket(packetid, data);
		if (packetid == 199)
		{
			if (!Alive)
			{
				return;
			}
			TalkUtil.Talk(EnumTalkType.Hurt);
		}
		if (packetid == 198)
		{
			TalkUtil.Talk(EnumTalkType.Death);
		}
		if (packetid == 201)
		{
			if (track != null)
			{
				return;
			}
			SongPacket pkt = SerializerUtil.Deserialize<SongPacket>(data);
			ICoreClientAPI obj = Api as ICoreClientAPI;
			startLoadingMs = Api.World.ElapsedMilliseconds;
			wasStopped = false;
			track = new MusicTrack
			{
				Location = AssetLocation.Create(pkt.SoundLocation),
				Priority = 99f
			};
			obj.StartTrack(track, EnumSoundType.MusicGlitchunaffected, delegate(ILoadedSound s)
			{
				onTrackLoaded(s, pkt.SecondsPassed);
			});
		}
		if (packetid == 202)
		{
			track?.Stop();
			track = null;
			Api.Event.UnregisterCallback(handlerId);
			wasStopped = true;
			TalkUtil.ShouldDoIdleTalk = true;
		}
		if (packetid == 203)
		{
			TalkUtil.Talk((EnumTalkType)SerializerUtil.Deserialize<int>(data));
		}
	}

	private void onTrackLoaded(ILoadedSound sound, float secondsPassed)
	{
		if (track == null)
		{
			sound?.Dispose();
		}
		else
		{
			if (sound == null)
			{
				return;
			}
			track.Sound = sound;
			TalkUtil.ShouldDoIdleTalk = false;
			Api.Event.EnqueueMainThreadTask(delegate
			{
				if (track != null)
				{
					track.loading = true;
				}
			}, "settrackloading");
			long longMsPassed = Api.World.ElapsedMilliseconds - startLoadingMs;
			handlerId = Api.Event.RegisterCallback(delegate
			{
				if (sound.IsDisposed)
				{
					handlerId = 0L;
					track = null;
				}
				else
				{
					if (!wasStopped)
					{
						sound.Start();
						sound.PlaybackPosition = secondsPassed;
					}
					track.loading = false;
				}
			}, (int)Math.Max(0L, 500 - longMsPassed));
		}
	}

	public override void OnEntityDespawn(EntityDespawnData despawn)
	{
		track?.Stop();
		track = null;
		Api.Event.UnregisterCallback(handlerId);
		wasStopped = true;
		base.OnEntityDespawn(despawn);
		if (Api.Side == EnumAppSide.Server)
		{
			Api.Logger.Debug("Villager " + GetName() + " de-spawned at " + base.Pos.AsBlockPos?.ToString() + " for reason " + despawn.Reason);
		}
	}

	public override void OnGameTick(float dt)
	{
		base.OnGameTick(dt);
		if (World.Side != EnumAppSide.Client)
		{
			return;
		}
		TalkUtil.OnGameTick(dt);
		if (track?.Sound != null && track.Sound.IsPlaying)
		{
			ICoreClientAPI capi = Api as ICoreClientAPI;
			Vec3d plrpos = capi.World.Player.Entity?.Pos?.XYZ;
			if (!(plrpos == null))
			{
				float dist = GameMath.Sqrt(plrpos.SquareDistanceTo(base.Pos.X + 0.5, base.Pos.Y + 0.5, base.Pos.Z + 0.5));
				float volume = GameMath.Clamp(1f / (float)Math.Log10(Math.Max(1.0, (double)dist * 0.7)) - 0.8f, 0f, 1f);
				track.Sound.SetVolume(volume);
				track.Sound.SetPitch(GameMath.Clamp(1f - capi.Render.ShaderUniforms.GlitchStrength, 0.1f, 1f));
			}
		}
		else
		{
			TalkUtil.ShouldDoIdleTalk = true;
		}
	}

	protected override int Dialog_DialogTriggers(EntityAgent triggeringEntity, string value, JsonObject data)
	{
		if (value == "openhairstyling")
		{
			base.ConversableBh.Dialog?.TryClose();
			Api.ModLoader.GetModSystem<ModSystemNPCHairStyling>().handleHairstyling(this, triggeringEntity, new string[2] { "standard", hairStylingCategory });
			return 0;
		}
		return base.Dialog_DialogTriggers(triggeringEntity, value, data);
	}

	public override void OnReceivedClientPacket(IServerPlayer player, int packetid, byte[] data)
	{
		if (packetid == 1212)
		{
			interactingWithPlayer.Remove(player.Entity);
			if (WatchedAttributes.GetString("tradingPlayerUID") == player.PlayerUID)
			{
				WatchedAttributes.RemoveAttribute("tradingPlayerUID");
			}
		}
		base.OnReceivedClientPacket(player, packetid, data);
	}

	public override void PlayEntitySound(string type, IPlayer dualCallByPlayer = null)
	{
		if (!(type == "idle") || track == null || !track.IsActive)
		{
			base.PlayEntitySound(type, dualCallByPlayer);
		}
	}

	public override void OnHurt(DamageSource dmgSource, float damage)
	{
		if (World.Side != EnumAppSide.Server)
		{
			return;
		}
		foreach (KeyValuePair<int, IEntityActivity> item in GetBehavior<EntityBehaviorActivityDriven>().ActivitySystem.ActiveActivitiesBySlot)
		{
			item.Value.CurrentAction?.OnHurt(dmgSource, damage);
		}
		base.OnHurt(dmgSource, damage);
	}

	public override bool TryEarlyLoadCollectibleMappings(IWorldAccessor worldForNewMappings, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, int schematicSeed, bool resolveImports)
	{
		bool result = base.TryEarlyLoadCollectibleMappings(worldForNewMappings, oldBlockIdMapping, oldItemIdMapping, schematicSeed, resolveImports);
		string name = "-";
		try
		{
			name = WatchedAttributes.GetTreeAttribute("nametag")?.GetString("name") ?? "-";
		}
		catch (Exception)
		{
		}
		string worldname = ((worldForNewMappings is IServerWorldAccessor swa) ? swa.WorldName : "(unknown)");
		worldForNewMappings.Logger.Log(EnumLogType.Worldgen, "In " + worldname + ", placed villager " + name + " at " + base.Pos.AsBlockPos);
		return result;
	}

	public override void Die(EnumDespawnReason reason = EnumDespawnReason.Death, DamageSource damageSourceForDeath = null)
	{
		if (Api.Side == EnumAppSide.Server)
		{
			string name = "-";
			try
			{
				name = WatchedAttributes.GetTreeAttribute("nametag")?.GetString("name") ?? "-";
			}
			catch (Exception)
			{
			}
			if (reason == EnumDespawnReason.Death || reason == EnumDespawnReason.Removed)
			{
				string worldname = ((Api.World is IServerWorldAccessor swa) ? swa.WorldName : "(unknown)");
				switch (reason)
				{
				case EnumDespawnReason.Death:
					Api?.Logger.Log(EnumLogType.Worldgen, "In " + worldname + ", villager " + name + " killed at " + base.Pos.AsBlockPos);
					break;
				case EnumDespawnReason.Removed:
					Api?.Logger.Log(EnumLogType.Worldgen, "In " + worldname + ", villager " + name + " removed at " + base.Pos.AsBlockPos);
					break;
				}
			}
			else
			{
				Api?.Logger.Debug("Villager " + name + " de-spawned at " + base.Pos.AsBlockPos?.ToString() + " for reason " + reason);
			}
		}
		base.Die(reason, damageSourceForDeath);
	}

	public override void OnEntityLoaded()
	{
		base.OnEntityLoaded();
		if (Api.Side == EnumAppSide.Server)
		{
			if (WatchedAttributes.GetInt("entityDead") == 1)
			{
				Api.Logger.Debug("Villager " + GetName() + " loaded (but dead!) at " + base.Pos.AsBlockPos);
			}
			else
			{
				Api.Logger.Debug("Villager " + GetName() + " loaded at " + base.Pos.AsBlockPos);
			}
		}
	}

	public override void FromBytes(BinaryReader reader, bool forClient)
	{
		base.FromBytes(reader, forClient);
		(AnimManager as PersonalizedAnimationManager).Personality = Personality;
	}

	public override string GetInfoText()
	{
		string text = base.GetInfoText();
		if (Api is ICoreClientAPI capi && capi.Settings.Bool["extendedDebugInfo"])
		{
			text = text.TrimEnd();
			text = text + "\n<font color=\"#bbbbbb\">Personality: " + Personality + "\nVoice: " + VoiceSound + "</font>";
		}
		return text;
	}
}
