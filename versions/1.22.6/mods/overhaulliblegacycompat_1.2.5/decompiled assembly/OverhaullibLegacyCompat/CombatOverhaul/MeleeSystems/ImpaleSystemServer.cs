using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000EA RID: 234
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ImpaleSystemServer : IDisposable
	{
		// Token: 0x06000BF6 RID: 3062 RVA: 0x0003C9A8 File Offset: 0x0003ABA8
		public ImpaleSystemServer(ICoreServerAPI api)
		{
			this._api = api;
			this._channel = api.Network.RegisterChannel("CombatOverhaul:impale").RegisterMessageType<ImpaleAttachPacket>().RegisterMessageType<ImpaleReleasePacket>().RegisterMessageType<ImpaleThrowRequestPacket>().SetMessageHandler<ImpaleThrowRequestPacket>(new NetworkClientMessageHandler<ImpaleThrowRequestPacket>(this.HandlePacket));
			api.Event.OnEntityDeath += new EntityDeathDelegate(this.OnEntityDeath);
			api.Event.OnEntityDespawn += new EntityDespawnDelegate(this.OnEntityDespawn);
			api.Event.PlayerDisconnect += new PlayerDelegate(this.OnPlayerDisconnect);
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x0003CA6C File Offset: 0x0003AC6C
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

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0003CC50 File Offset: 0x0003AE50
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

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0003CD30 File Offset: 0x0003AF30
		private void HandlePacket(IServerPlayer player, ImpaleThrowRequestPacket packet)
		{
			ImpaleKey key = new ImpaleKey(player.Entity.EntityId, packet.MainHand);
			Vec3d direction = ImpaleSystemServer.ReadDirection(packet.Direction, player.Entity.ServerPos.GetViewVector());
			this.Clear(key, true, direction);
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0003CD7C File Offset: 0x0003AF7C
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

		// Token: 0x06000BFB RID: 3067 RVA: 0x0003CF90 File Offset: 0x0003B190
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

		// Token: 0x06000BFC RID: 3068 RVA: 0x0003D0FB File Offset: 0x0003B2FB
		private void OnEntityDeath(Entity entity, DamageSource damageSource)
		{
			this.ClearEntity(entity.EntityId);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x0003D109 File Offset: 0x0003B309
		private void OnEntityDespawn(Entity entity, EntityDespawnData reasonData)
		{
			this.ClearEntity(entity.EntityId);
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0003D117 File Offset: 0x0003B317
		private void OnPlayerDisconnect(IServerPlayer player)
		{
			this.ClearEntity(player.Entity.EntityId);
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x0003D12C File Offset: 0x0003B32C
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

		// Token: 0x06000C00 RID: 3072 RVA: 0x0003D1B4 File Offset: 0x0003B3B4
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

		// Token: 0x06000C01 RID: 3073 RVA: 0x0003D340 File Offset: 0x0003B540
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

		// Token: 0x06000C02 RID: 3074 RVA: 0x0003D3E8 File Offset: 0x0003B5E8
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

		// Token: 0x06000C03 RID: 3075 RVA: 0x0003D48F File Offset: 0x0003B68F
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

		// Token: 0x06000C04 RID: 3076 RVA: 0x0003D4B4 File Offset: 0x0003B6B4
		private static float ClampThrowVelocity(float throwVelocity)
		{
			return MathF.Max(0f, MathF.Min(10f, throwVelocity));
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0003D4CB File Offset: 0x0003B6CB
		private static float ToEntityMotion(float throwVelocity)
		{
			return ImpaleSystemServer.ClampThrowVelocity(throwVelocity) * 0.05f;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x0003D4D9 File Offset: 0x0003B6D9
		private static Vec3d ReadDirection([Nullable(2)] float[] value, Vec3f fallback)
		{
			if (value == null || value.Length < 3)
			{
				return new Vec3d((double)fallback.X, (double)fallback.Y, (double)fallback.Z);
			}
			return new Vec3d((double)value[0], (double)value[1], (double)value[2]);
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0003D510 File Offset: 0x0003B710
		private void EnsureTickListener()
		{
			if (this._tickListener != 0L)
			{
				return;
			}
			this._tickListener = this._api.Event.RegisterGameTickListener(new Action<float>(this.OnTick), 20, 0);
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0003D540 File Offset: 0x0003B740
		private void StopTickListenerIfIdle()
		{
			if (this._tickListener == 0L || this._states.Count > 0)
			{
				return;
			}
			this._api.Event.UnregisterGameTickListener(this._tickListener);
			this._tickListener = 0L;
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0003D578 File Offset: 0x0003B778
		private void CopyStateKeys(List<ImpaleKey> buffer)
		{
			buffer.Clear();
			foreach (ImpaleKey key in this._states.Keys)
			{
				buffer.Add(key);
			}
		}

		// Token: 0x040005C8 RID: 1480
		private const float MaxThrowVelocitySetting = 10f;

		// Token: 0x040005C9 RID: 1481
		private const float ThrowVelocityToEntityMotion = 0.05f;

		// Token: 0x040005CA RID: 1482
		private readonly ICoreServerAPI _api;

		// Token: 0x040005CB RID: 1483
		private readonly IServerNetworkChannel _channel;

		// Token: 0x040005CC RID: 1484
		private readonly Dictionary<ImpaleKey, ImpaleSystemServer.ImpaleState> _states = new Dictionary<ImpaleKey, ImpaleSystemServer.ImpaleState>();

		// Token: 0x040005CD RID: 1485
		private readonly Dictionary<long, ImpaleKey> _targets = new Dictionary<long, ImpaleKey>();

		// Token: 0x040005CE RID: 1486
		private readonly List<ImpaleKey> _tickKeysScratch = new List<ImpaleKey>();

		// Token: 0x040005CF RID: 1487
		private readonly List<ImpaleKey> _clearKeysScratch = new List<ImpaleKey>();

		// Token: 0x040005D0 RID: 1488
		private long _tickListener;

		// Token: 0x02000252 RID: 594
		[Nullable(0)]
		private sealed class ImpaleState
		{
			// Token: 0x17000527 RID: 1319
			// (get) Token: 0x06001686 RID: 5766 RVA: 0x0006A284 File Offset: 0x00068484
			// (set) Token: 0x06001687 RID: 5767 RVA: 0x0006A28C File Offset: 0x0006848C
			public long AttackerEntityId { get; set; }

			// Token: 0x17000528 RID: 1320
			// (get) Token: 0x06001688 RID: 5768 RVA: 0x0006A295 File Offset: 0x00068495
			// (set) Token: 0x06001689 RID: 5769 RVA: 0x0006A29D File Offset: 0x0006849D
			public long TargetEntityId { get; set; }

			// Token: 0x17000529 RID: 1321
			// (get) Token: 0x0600168A RID: 5770 RVA: 0x0006A2A6 File Offset: 0x000684A6
			// (set) Token: 0x0600168B RID: 5771 RVA: 0x0006A2AE File Offset: 0x000684AE
			public bool MainHand { get; set; }

			// Token: 0x1700052A RID: 1322
			// (get) Token: 0x0600168C RID: 5772 RVA: 0x0006A2B7 File Offset: 0x000684B7
			// (set) Token: 0x0600168D RID: 5773 RVA: 0x0006A2BF File Offset: 0x000684BF
			public string WeaponCode { get; set; } = "";

			// Token: 0x1700052B RID: 1323
			// (get) Token: 0x0600168E RID: 5774 RVA: 0x0006A2C8 File Offset: 0x000684C8
			// (set) Token: 0x0600168F RID: 5775 RVA: 0x0006A2D0 File Offset: 0x000684D0
			public float[] HoldOffset { get; set; } = new float[]
			{
				default(float),
				default(float),
				1.25f
			};

			// Token: 0x1700052C RID: 1324
			// (get) Token: 0x06001690 RID: 5776 RVA: 0x0006A2D9 File Offset: 0x000684D9
			// (set) Token: 0x06001691 RID: 5777 RVA: 0x0006A2E1 File Offset: 0x000684E1
			public float[] TargetOffset { get; set; }

			// Token: 0x1700052D RID: 1325
			// (get) Token: 0x06001692 RID: 5778 RVA: 0x0006A2EA File Offset: 0x000684EA
			// (set) Token: 0x06001693 RID: 5779 RVA: 0x0006A2F2 File Offset: 0x000684F2
			public float ThrowVelocity { get; set; }

			// Token: 0x1700052E RID: 1326
			// (get) Token: 0x06001694 RID: 5780 RVA: 0x0006A2FB File Offset: 0x000684FB
			// (set) Token: 0x06001695 RID: 5781 RVA: 0x0006A303 File Offset: 0x00068503
			public float ThrowUpBias { get; set; }

			// Token: 0x06001696 RID: 5782 RVA: 0x0006A30C File Offset: 0x0006850C
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
