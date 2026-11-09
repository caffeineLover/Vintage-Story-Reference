using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F5 RID: 245
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ImpaleSystemServer : IDisposable
	{
		// Token: 0x06000C52 RID: 3154 RVA: 0x0003D998 File Offset: 0x0003BB98
		public ImpaleSystemServer(ICoreServerAPI api)
		{
			this._api = api;
			this._channel = api.Network.RegisterChannel("CombatOverhaul:impale").RegisterMessageType<ImpaleAttachPacket>().RegisterMessageType<ImpaleReleasePacket>().RegisterMessageType<ImpaleThrowRequestPacket>().SetMessageHandler<ImpaleThrowRequestPacket>(new NetworkClientMessageHandler<ImpaleThrowRequestPacket>(this.HandlePacket));
			api.Event.OnEntityDeath += new EntityDeathDelegate(this.OnEntityDeath);
			api.Event.OnEntityDespawn += new EntityDespawnDelegate(this.OnEntityDespawn);
			api.Event.PlayerDisconnect += new PlayerDelegate(this.OnPlayerDisconnect);
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0003DA5C File Offset: 0x0003BC5C
		public void TryAttach(MeleeDamagePacket packet, Entity attacker, Entity target, [Nullable(2)] ItemSlot weaponSlot, bool damageReceived)
		{
			if (!packet.ImpaleEnabled || !damageReceived || !target.Alive)
			{
				return;
			}
			if (!ImpaleAccess.IsDevImpaleSpear(weaponSlot))
			{
				return;
			}
			if (!(attacker is EntityPlayer))
			{
				return;
			}
			if (!ImpaleSystemServer.IsValidTarget(target, packet))
			{
				return;
			}
			ImpaleKey key = new ImpaleKey(attacker.EntityId, packet.MainHand);
			if (this._states.ContainsKey(key))
			{
				return;
			}
			if (this._targets.ContainsKey(target.EntityId))
			{
				return;
			}
			string text;
			if (weaponSlot == null)
			{
				text = null;
			}
			else
			{
				ItemStack itemstack = weaponSlot.Itemstack;
				if (itemstack == null)
				{
					text = null;
				}
				else
				{
					CollectibleObject collectible = itemstack.Collectible;
					if (collectible == null)
					{
						text = null;
					}
					else
					{
						AssetLocation code = collectible.Code;
						text = ((code != null) ? code.ToString() : null);
					}
				}
			}
			string weaponCode = text ?? "";
			if (weaponCode.Length == 0)
			{
				return;
			}
			ImpaleSystemServer.ImpaleState impaleState = new ImpaleSystemServer.ImpaleState();
			impaleState.AttackerEntityId = attacker.EntityId;
			impaleState.TargetEntityId = target.EntityId;
			impaleState.MainHand = packet.MainHand;
			impaleState.WeaponCode = weaponCode;
			impaleState.HoldOffset = ImpaleSystemServer.NormalizeOffset(packet.ImpaleHoldOffset, new float[]
			{
				default(float),
				default(float),
				1.25f
			});
			ImpaleSystemServer.ImpaleState impaleState2 = impaleState;
			float[] impaleTargetOffset = packet.ImpaleTargetOffset;
			float[] array = new float[3];
			array[1] = -0.6f;
			impaleState2.TargetOffset = ImpaleSystemServer.NormalizeOffset(impaleTargetOffset, array);
			impaleState.ThrowVelocity = ImpaleSystemServer.ClampThrowVelocity(packet.ImpaleThrowVelocity);
			impaleState.ThrowUpBias = packet.ImpaleThrowUpBias;
			ImpaleSystemServer.ImpaleState state = impaleState;
			this._states[key] = state;
			this._targets[target.EntityId] = key;
			this.EnsureTickListener();
			target.WatchedAttributes.SetLong("combatOverhaulImpaledBy", attacker.EntityId);
			target.WatchedAttributes.SetBool("combatOverhaulImpaledMainHand", packet.MainHand);
			target.WatchedAttributes.MarkPathDirty("combatOverhaulImpaledBy");
			this._channel.BroadcastPacket<ImpaleAttachPacket>(new ImpaleAttachPacket
			{
				AttackerEntityId = attacker.EntityId,
				TargetEntityId = target.EntityId,
				MainHand = packet.MainHand
			}, Array.Empty<IServerPlayer>());
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0003DC40 File Offset: 0x0003BE40
		public void Dispose()
		{
			if (this._tickListener != 0L)
			{
				this._api.Event.UnregisterGameTickListener(this._tickListener);
				this._tickListener = 0L;
			}
			this._api.Event.OnEntityDeath -= new EntityDeathDelegate(this.OnEntityDeath);
			this._api.Event.OnEntityDespawn -= new EntityDespawnDelegate(this.OnEntityDespawn);
			this._api.Event.PlayerDisconnect -= new PlayerDelegate(this.OnPlayerDisconnect);
			this.CopyStateKeys(this._clearKeysScratch);
			foreach (ImpaleKey key in this._clearKeysScratch)
			{
				this.Clear(key, false, Vec3d.Zero);
			}
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0003DD20 File Offset: 0x0003BF20
		private void HandlePacket(IServerPlayer player, ImpaleThrowRequestPacket packet)
		{
			ImpaleKey key = new ImpaleKey(player.Entity.EntityId, packet.MainHand);
			Vec3d direction = ImpaleSystemServer.ReadDirection(packet.Direction, player.Entity.ServerPos.GetViewVector());
			this.Clear(key, true, direction);
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0003DD6C File Offset: 0x0003BF6C
		private void OnTick(float dt)
		{
			if (this._states.Count == 0)
			{
				this.StopTickListenerIfIdle();
				return;
			}
			this.CopyStateKeys(this._tickKeysScratch);
			foreach (ImpaleKey key in this._tickKeysScratch)
			{
				ImpaleSystemServer.ImpaleState state;
				if (this._states.TryGetValue(key, ref state))
				{
					Entity attacker = this._api.World.GetEntityById(state.AttackerEntityId);
					Entity target = this._api.World.GetEntityById(state.TargetEntityId);
					EntityAgent agent = attacker as EntityAgent;
					if (agent != null)
					{
						EntityAgent targetAgent = target as EntityAgent;
						if (targetAgent != null && attacker.Alive && target.Alive)
						{
							ItemSlot itemSlot = state.MainHand ? agent.RightHandItemSlot : agent.LeftHandItemSlot;
							string text;
							if (itemSlot == null)
							{
								text = null;
							}
							else
							{
								ItemStack itemstack = itemSlot.Itemstack;
								if (itemstack == null)
								{
									text = null;
								}
								else
								{
									CollectibleObject collectible = itemstack.Collectible;
									if (collectible == null)
									{
										text = null;
									}
									else
									{
										AssetLocation code = collectible.Code;
										text = ((code != null) ? code.ToString() : null);
									}
								}
							}
							if ((text ?? "") != state.WeaponCode)
							{
								this.Clear(key, false, Vec3d.Zero);
								continue;
							}
							Vec3d position = ImpaleSystemServer.GetAttachedPosition(agent, target, state);
							targetAgent.Controls.StopAllMovement();
							targetAgent.ServerControls.StopAllMovement();
							target.ServerPos.SetPos(position);
							target.Pos.SetFrom(target.ServerPos);
							target.ServerPos.Motion.Set(0.0, 0.0, 0.0);
							target.Pos.Motion.Set(0.0, 0.0, 0.0);
							continue;
						}
					}
					this.Clear(key, false, Vec3d.Zero);
				}
			}
			this.StopTickListenerIfIdle();
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x0003DF80 File Offset: 0x0003C180
		private void Clear(ImpaleKey key, bool applyVelocity, Vec3d direction)
		{
			ImpaleSystemServer.ImpaleState state;
			if (!this._states.Remove(key, ref state))
			{
				return;
			}
			this._targets.Remove(state.TargetEntityId);
			Entity target = this._api.World.GetEntityById(state.TargetEntityId);
			if (target != null)
			{
				target.WatchedAttributes.RemoveAttribute("combatOverhaulImpaledBy");
				target.WatchedAttributes.RemoveAttribute("combatOverhaulImpaledMainHand");
				target.WatchedAttributes.MarkAllDirty();
				if (applyVelocity && target.Alive)
				{
					Vec3d velocity = direction.Clone();
					if (velocity.LengthSq() < 0.0001)
					{
						velocity = new Vec3d(0.0, 0.0, -1.0);
					}
					velocity.Normalize();
					velocity.Y += (double)state.ThrowUpBias;
					if (velocity.LengthSq() > 0.0001)
					{
						velocity.Normalize();
					}
					velocity.Mul((double)ImpaleSystemServer.ToEntityMotion(state.ThrowVelocity));
					target.ServerPos.Motion.Set(velocity);
					target.Pos.Motion.Set(velocity);
				}
			}
			this._channel.BroadcastPacket<ImpaleReleasePacket>(new ImpaleReleasePacket
			{
				AttackerEntityId = state.AttackerEntityId,
				TargetEntityId = state.TargetEntityId,
				MainHand = state.MainHand
			}, Array.Empty<IServerPlayer>());
			this.StopTickListenerIfIdle();
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x0003E0EB File Offset: 0x0003C2EB
		private void OnEntityDeath(Entity entity, DamageSource damageSource)
		{
			this.ClearEntity(entity.EntityId);
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x0003E0F9 File Offset: 0x0003C2F9
		private void OnEntityDespawn(Entity entity, EntityDespawnData reasonData)
		{
			this.ClearEntity(entity.EntityId);
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0003E107 File Offset: 0x0003C307
		private void OnPlayerDisconnect(IServerPlayer player)
		{
			this.ClearEntity(player.Entity.EntityId);
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0003E11C File Offset: 0x0003C31C
		private void ClearEntity(long entityId)
		{
			this.CopyStateKeys(this._clearKeysScratch);
			foreach (ImpaleKey key in this._clearKeysScratch)
			{
				ImpaleSystemServer.ImpaleState state;
				if (this._states.TryGetValue(key, ref state) && (state.AttackerEntityId == entityId || state.TargetEntityId == entityId))
				{
					this.Clear(key, false, Vec3d.Zero);
				}
			}
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x0003E1A4 File Offset: 0x0003C3A4
		private static Vec3d GetAttachedPosition(EntityAgent attacker, Entity target, ImpaleSystemServer.ImpaleState state)
		{
			Vec3f view = attacker.ServerPos.GetViewVector();
			Vec3d forward = new Vec3d((double)view.X, (double)view.Y, (double)view.Z);
			if (forward.LengthSq() < 0.0001)
			{
				forward = new Vec3d(0.0, 0.0, -1.0);
			}
			forward.Normalize();
			Vec3d right = new Vec3d(-forward.Z, 0.0, forward.X);
			if (right.LengthSq() < 0.0001)
			{
				right = new Vec3d(1.0, 0.0, 0.0);
			}
			right.Normalize();
			Vec3d up = new Vec3d(0.0, 1.0, 0.0);
			float[] hold = state.HoldOffset;
			float[] targetOffset = state.TargetOffset;
			Vec3d vec3d = attacker.ServerPos.XYZ.Add(0.0, attacker.LocalEyePos.Y * 0.7, 0.0).AddCopy(right * (hold[0] + targetOffset[0])).AddCopy(up * (hold[1] + targetOffset[1])).AddCopy(forward * (hold[2] + targetOffset[2]));
			vec3d.Y -= (double)target.SelectionBox.YSize * 0.25;
			return vec3d;
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x0003E330 File Offset: 0x0003C530
		private static bool IsValidTarget(Entity target, MeleeDamagePacket packet)
		{
			if (target is EntityPlayer)
			{
				return false;
			}
			EntityAgent agent = target as EntityAgent;
			if (agent == null)
			{
				return false;
			}
			if (!target.IsCreature || !target.Alive)
			{
				return false;
			}
			if (agent.MountedOn != null)
			{
				return false;
			}
			float volume = target.SelectionBox.XSize * target.SelectionBox.YSize * target.SelectionBox.ZSize;
			return volume > 0f && volume <= packet.ImpaleMaxTargetVolume && (!((packet.ImpaleTargetFilter.Length == 0) ? "HostileOnly" : packet.ImpaleTargetFilter).Equals("HostileOnly", 5) || ImpaleSystemServer.IsHostile(target));
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0003E3D8 File Offset: 0x0003C5D8
		private static bool IsHostile(Entity target)
		{
			EntityProperties properties = target.Properties;
			string text;
			if (properties == null)
			{
				text = null;
			}
			else
			{
				EntityServerProperties server = properties.Server;
				if (server == null)
				{
					text = null;
				}
				else
				{
					SpawnConditions spawnConditions = server.SpawnConditions;
					if (spawnConditions == null)
					{
						text = null;
					}
					else
					{
						RuntimeSpawnConditions runtime = spawnConditions.Runtime;
						text = ((runtime != null) ? runtime.Group : null);
					}
				}
			}
			string text2 = text ?? "";
			EntityProperties properties2 = target.Properties;
			string text3;
			if (properties2 == null)
			{
				text3 = null;
			}
			else
			{
				EntityServerProperties server2 = properties2.Server;
				if (server2 == null)
				{
					text3 = null;
				}
				else
				{
					SpawnConditions spawnConditions2 = server2.SpawnConditions;
					if (spawnConditions2 == null)
					{
						text3 = null;
					}
					else
					{
						WorldGenSpawnConditions worldgen = spawnConditions2.Worldgen;
						text3 = ((worldgen != null) ? worldgen.Group : null);
					}
				}
			}
			string worldgenGroup = text3 ?? "";
			return text2.Equals("hostile", 5) || worldgenGroup.Equals("hostile", 5);
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x0003E47F File Offset: 0x0003C67F
		private static float[] NormalizeOffset([Nullable(2)] float[] value, float[] fallback)
		{
			if (value == null || value.Length < 3)
			{
				return fallback;
			}
			return new float[]
			{
				value[0],
				value[1],
				value[2]
			};
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x0003E4A4 File Offset: 0x0003C6A4
		private static float ClampThrowVelocity(float throwVelocity)
		{
			return MathF.Max(0f, MathF.Min(10f, throwVelocity));
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0003E4BB File Offset: 0x0003C6BB
		private static float ToEntityMotion(float throwVelocity)
		{
			return ImpaleSystemServer.ClampThrowVelocity(throwVelocity) * 0.05f;
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x0003E4C9 File Offset: 0x0003C6C9
		private static Vec3d ReadDirection([Nullable(2)] float[] value, Vec3f fallback)
		{
			if (value == null || value.Length < 3)
			{
				return new Vec3d((double)fallback.X, (double)fallback.Y, (double)fallback.Z);
			}
			return new Vec3d((double)value[0], (double)value[1], (double)value[2]);
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x0003E500 File Offset: 0x0003C700
		private void EnsureTickListener()
		{
			if (this._tickListener != 0L)
			{
				return;
			}
			this._tickListener = this._api.Event.RegisterGameTickListener(new Action<float>(this.OnTick), 20, 0);
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0003E530 File Offset: 0x0003C730
		private void StopTickListenerIfIdle()
		{
			if (this._tickListener == 0L || this._states.Count > 0)
			{
				return;
			}
			this._api.Event.UnregisterGameTickListener(this._tickListener);
			this._tickListener = 0L;
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x0003E568 File Offset: 0x0003C768
		private void CopyStateKeys(List<ImpaleKey> buffer)
		{
			buffer.Clear();
			foreach (ImpaleKey key in this._states.Keys)
			{
				buffer.Add(key);
			}
		}

		// Token: 0x040005EA RID: 1514
		private const float MaxThrowVelocitySetting = 10f;

		// Token: 0x040005EB RID: 1515
		private const float ThrowVelocityToEntityMotion = 0.05f;

		// Token: 0x040005EC RID: 1516
		private readonly ICoreServerAPI _api;

		// Token: 0x040005ED RID: 1517
		private readonly IServerNetworkChannel _channel;

		// Token: 0x040005EE RID: 1518
		private readonly Dictionary<ImpaleKey, ImpaleSystemServer.ImpaleState> _states = new Dictionary<ImpaleKey, ImpaleSystemServer.ImpaleState>();

		// Token: 0x040005EF RID: 1519
		private readonly Dictionary<long, ImpaleKey> _targets = new Dictionary<long, ImpaleKey>();

		// Token: 0x040005F0 RID: 1520
		private readonly List<ImpaleKey> _tickKeysScratch = new List<ImpaleKey>();

		// Token: 0x040005F1 RID: 1521
		private readonly List<ImpaleKey> _clearKeysScratch = new List<ImpaleKey>();

		// Token: 0x040005F2 RID: 1522
		private long _tickListener;

		// Token: 0x02000261 RID: 609
		[Nullable(0)]
		private sealed class ImpaleState
		{
			// Token: 0x17000549 RID: 1353
			// (get) Token: 0x06001701 RID: 5889 RVA: 0x0006BD48 File Offset: 0x00069F48
			// (set) Token: 0x06001702 RID: 5890 RVA: 0x0006BD50 File Offset: 0x00069F50
			public long AttackerEntityId { get; set; }

			// Token: 0x1700054A RID: 1354
			// (get) Token: 0x06001703 RID: 5891 RVA: 0x0006BD59 File Offset: 0x00069F59
			// (set) Token: 0x06001704 RID: 5892 RVA: 0x0006BD61 File Offset: 0x00069F61
			public long TargetEntityId { get; set; }

			// Token: 0x1700054B RID: 1355
			// (get) Token: 0x06001705 RID: 5893 RVA: 0x0006BD6A File Offset: 0x00069F6A
			// (set) Token: 0x06001706 RID: 5894 RVA: 0x0006BD72 File Offset: 0x00069F72
			public bool MainHand { get; set; }

			// Token: 0x1700054C RID: 1356
			// (get) Token: 0x06001707 RID: 5895 RVA: 0x0006BD7B File Offset: 0x00069F7B
			// (set) Token: 0x06001708 RID: 5896 RVA: 0x0006BD83 File Offset: 0x00069F83
			public string WeaponCode { get; set; } = "";

			// Token: 0x1700054D RID: 1357
			// (get) Token: 0x06001709 RID: 5897 RVA: 0x0006BD8C File Offset: 0x00069F8C
			// (set) Token: 0x0600170A RID: 5898 RVA: 0x0006BD94 File Offset: 0x00069F94
			public float[] HoldOffset { get; set; } = new float[]
			{
				default(float),
				default(float),
				1.25f
			};

			// Token: 0x1700054E RID: 1358
			// (get) Token: 0x0600170B RID: 5899 RVA: 0x0006BD9D File Offset: 0x00069F9D
			// (set) Token: 0x0600170C RID: 5900 RVA: 0x0006BDA5 File Offset: 0x00069FA5
			public float[] TargetOffset { get; set; }

			// Token: 0x1700054F RID: 1359
			// (get) Token: 0x0600170D RID: 5901 RVA: 0x0006BDAE File Offset: 0x00069FAE
			// (set) Token: 0x0600170E RID: 5902 RVA: 0x0006BDB6 File Offset: 0x00069FB6
			public float ThrowVelocity { get; set; }

			// Token: 0x17000550 RID: 1360
			// (get) Token: 0x0600170F RID: 5903 RVA: 0x0006BDBF File Offset: 0x00069FBF
			// (set) Token: 0x06001710 RID: 5904 RVA: 0x0006BDC7 File Offset: 0x00069FC7
			public float ThrowUpBias { get; set; }

			// Token: 0x06001711 RID: 5905 RVA: 0x0006BDD0 File Offset: 0x00069FD0
			public ImpaleState()
			{
				float[] array = new float[3];
				array[1] = -0.6f;
				this.TargetOffset = array;
				this.ThrowVelocity = 5f;
				this.ThrowUpBias = 0.15f;
				base..ctor();
			}
		}
	}
}
