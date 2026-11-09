using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200000A RID: 10
	public class BEFirework : BlockEntity
	{
		// Token: 0x0600004C RID: 76 RVA: 0x0000423C File Offset: 0x0000243C
		static BEFirework()
		{
			BEFirework.smallSparks = new SimpleParticleProperties(1f, 1f, ColorUtil.ToRgba(255, 255, 233, 0), new Vec3d(), new Vec3d(), new Vec3f(-3f, 5f, -3f), new Vec3f(3f, 8f, 3f), 0.03f, 1f, 0.05f, 0.15f, 0)
			{
				VertexFlags = 255
			};
			BEFirework.smallSparks.AddPos.Set(-0.10000000149011612, -0.30000001192092896, -0.10000000149011612);
			BEFirework.smallSparks.SizeEvolve = new EvolvingNatFloat(1, -0.05f);
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600004D RID: 77 RVA: 0x0000430E File Offset: 0x0000250E
		public virtual float FuseTimeSeconds
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00004315 File Offset: 0x00002515
		public virtual EnumBlastType BlastType
		{
			get
			{
				return this.blastType;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004F RID: 79 RVA: 0x0000431D File Offset: 0x0000251D
		public virtual float BlastRadius
		{
			get
			{
				return this.blastRadius;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00004325 File Offset: 0x00002525
		public virtual float InjureRadius
		{
			get
			{
				return this.injureRadius;
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00004330 File Offset: 0x00002530
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.capi = (api as ICoreClientAPI);
			this.RegisterGameTickListener(new Action<float>(this.OnTick), 50, 0);
			if (this.fuseSound == null && api.Side == 2)
			{
				this.fuseSound = ((IClientWorldAccessor)api.World).LoadSound(new SoundParams
				{
					Location = new AssetLocation("game:sounds/effect/fuse"),
					ShouldLoop = true,
					Position = this.Pos.ToVec3f().Add(0.5f, 0.25f, 0.5f),
					DisposeOnFinish = false,
					Volume = 0.2f,
					Range = 16f
				});
			}
			if (base.Block.LastCodePart(0) == "fountain")
			{
				if (this.fountainSound == null && api.Side == 2)
				{
					this.fountainSound = ((IClientWorldAccessor)api.World).LoadSound(new SoundParams
					{
						Location = new AssetLocation("primitivesurvival:sounds/fireworks/fountain"),
						ShouldLoop = true,
						Position = this.Pos.ToVec3f().Add(0.5f, 0.25f, 0.5f),
						DisposeOnFinish = false,
						Volume = 0.7f,
						Range = 16f
					});
				}
			}
			else if (base.Block.LastCodePart(0) == "beehive")
			{
				if (this.beehiveSound == null && api.Side == 2)
				{
					this.beehiveSound = ((IClientWorldAccessor)api.World).LoadSound(new SoundParams
					{
						Location = new AssetLocation("primitivesurvival:sounds/fireworks/beehive"),
						ShouldLoop = true,
						Position = this.Pos.ToVec3f().Add(0.5f, 0.25f, 0.5f),
						DisposeOnFinish = false,
						Volume = 0.9f,
						Range = 16f
					});
				}
			}
			else if (base.Block.LastCodePart(0) == "bamboobarrage")
			{
				if (this.bamboobarrageSound == null && api.Side == 2)
				{
					this.bamboobarrageSound = ((IClientWorldAccessor)api.World).LoadSound(new SoundParams
					{
						Location = new AssetLocation("primitivesurvival:sounds/fireworks/bamboobarrage"),
						ShouldLoop = true,
						Position = this.Pos.ToVec3f().Add(0.5f, 0.25f, 0.5f),
						DisposeOnFinish = false,
						Volume = 0.9f,
						Range = 16f
					});
				}
			}
			else if (base.Block.LastCodePart(0) == "cherrybomb" && this.cherrybombSound == null && api.Side == 2)
			{
				this.cherrybombSound = ((IClientWorldAccessor)api.World).LoadSound(new SoundParams
				{
					Location = new AssetLocation("primitivesurvival:sounds/fireworks/cherrybomb"),
					ShouldLoop = false,
					Position = this.Pos.ToVec3f().Add(0.5f, 0.25f, 0.5f),
					DisposeOnFinish = false,
					Volume = 0.9f,
					Range = 16f
				});
			}
			this.blastRadius = (float)base.Block.Attributes["blastRadius"].AsInt(4);
			this.injureRadius = (float)base.Block.Attributes["injureRadius"].AsInt(8);
			this.blastType = base.Block.Attributes["blastType"].AsInt(0);
			if (base.Block.LastCodePart(0) == "missile" || base.Block.LastCodePart(0) == "boomer")
			{
				this.storedInterval = 0.04;
			}
			else if (base.Block.LastCodePart(0) == "cherrybomb")
			{
				this.storedInterval = 0.02;
			}
			else
			{
				this.storedInterval = 0.004;
			}
			this.interval = this.storedInterval;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004776 File Offset: 0x00002976
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			base.OnBlockPlaced(byItemStack);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000477F File Offset: 0x0000297F
		private void InitializeTimer()
		{
			if (EnumAppSideExtensions.IsClient(this.Api.Side))
			{
				this.capi.Event.RegisterAsyncParticleSpawner(new ContinousParticleSpawnTaskDelegate(this.UpdateParticles));
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000047B0 File Offset: 0x000029B0
		private bool UpdateParticles(float dt, IAsyncParticleManager manager)
		{
			if (this.killOffThread)
			{
				return false;
			}
			this.interval -= (double)dt;
			if (this.interval > 0.0)
			{
				return true;
			}
			this.interval = this.storedInterval;
			this.GenerateParticles(manager);
			return base.Block.LastCodePart(0) != "missile" && base.Block.LastCodePart(0) != "boomer";
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004830 File Offset: 0x00002A30
		private SimpleParticleProperties BuildPrimaryParticles(string type)
		{
			SimpleParticleProperties particles;
			if (type == "missile")
			{
				int color = ColorUtil.ToRgba(BEFirework.rand.Next(100, 255), BEFirework.rand.Next(100, 255), BEFirework.rand.Next(100, 255), BEFirework.rand.Next(100, 255));
				particles = new SimpleParticleProperties(1f, 1f, color, new Vec3d(0.5, 1.5, 0.5), new Vec3d(), new Vec3f(1.5f, 40f, 1.5f), new Vec3f(), (float)(BEFirework.rand.NextDouble() * 0.25 + 0.10000000149011612), (float)(BEFirework.rand.NextDouble() * 3.0 + 0.0), 5f, 5f, 1);
				particles.MinPos.Add(this.Pos);
				particles.AddPos.Set(new Vec3d(0.0, 0.0, 0.0));
				particles.AddVelocity.Set(new Vec3f(-1.5f, 10f, -1.5f));
				particles.SizeEvolve = new EvolvingNatFloat(3, 3f);
				particles.VertexFlags = 255;
				particles.WithTerrainCollision = true;
				particles.RandomVelocityChange = true;
			}
			else if (type == "fountain")
			{
				int color2 = ColorUtil.ToRgba(255, 50, BEFirework.rand.Next(200, 240), 0);
				particles = new SimpleParticleProperties(30f, 100f, color2, new Vec3d(0.5, 2.0, 0.5), new Vec3d(), new Vec3f(0.5f, 7f, 0.5f), new Vec3f(), (float)(BEFirework.rand.NextDouble() * 0.5 + 0.0), (float)(BEFirework.rand.NextDouble() * 0.30000001192092896 + 0.0), 0.1f, 1.2f, 1);
				particles.MinPos.Add(this.Pos);
				particles.AddPos.Set(new Vec3d(0.0, -1.5, 0.0));
				particles.AddVelocity.Set(new Vec3f(-1f, -3f, -1f));
				particles.VertexFlags = 255;
				particles.SizeEvolve = new EvolvingNatFloat(3, 0.3f);
				particles.WithTerrainCollision = true;
				particles.RandomVelocityChange = true;
			}
			else if (type == "beehive")
			{
				int color3 = ColorUtil.ToRgba(BEFirework.rand.Next(100, 255), 255, 255, 10);
				particles = new SimpleParticleProperties(4f, 1f, color3, new Vec3d(0.5, 0.5, 0.5), new Vec3d(), new Vec3f(4f, 20f, 4f), new Vec3f(), (float)(BEFirework.rand.NextDouble() * 0.25 + 0.10000000149011612), (float)(BEFirework.rand.NextDouble() * 3.0 + 0.0), 4f, 4f, 1);
				particles.MinPos.Add(this.Pos);
				particles.AddPos.Set(new Vec3d(0.0, 0.0, 0.0));
				particles.AddVelocity.Set(new Vec3f(-8f, 30f, -8f));
				particles.SizeEvolve = new EvolvingNatFloat(3, 3f);
				particles.VertexFlags = 255;
				particles.WithTerrainCollision = true;
				particles.RandomVelocityChange = true;
			}
			else if (type == "boomer")
			{
				int color4 = ColorUtil.ToRgba(BEFirework.rand.Next(140, 255), BEFirework.rand.Next(100, 255), BEFirework.rand.Next(100, 255), BEFirework.rand.Next(100, 255));
				particles = new SimpleParticleProperties(1000f, 0f, color4, new Vec3d(0.5, 0.5, 0.5), new Vec3d(), new Vec3f(3f, 5f, 3f), new Vec3f(), (float)(BEFirework.rand.NextDouble() * 0.949999988079071 + 3.200000047683716), 0f, 1.8f, 0.8f, 1);
				particles.MinPos.Add(this.Pos);
				particles.AddPos.Set(new Vec3d(0.0, 0.0, 0.0));
				particles.AddVelocity.Set(new Vec3f(-6f, -6f, -6f));
				particles.SizeEvolve = new EvolvingNatFloat(3, 0.3f);
				particles.VertexFlags = 255;
				particles.SelfPropelled = true;
				particles.ShouldDieInLiquid = true;
				particles.WindAffected = true;
				particles.WindAffectednes = 0.5f;
				particles.WithTerrainCollision = true;
				particles.RandomVelocityChange = true;
			}
			else if (type == "bamboobarrage")
			{
				int color5 = ColorUtil.ToRgba(BEFirework.rand.Next(150, 255), 255, BEFirework.rand.Next(100, 120), BEFirework.rand.Next(100, 120));
				particles = new SimpleParticleProperties(1f, 1f, color5, new Vec3d(0.5, 1.5, 0.5), new Vec3d(), new Vec3f(5f, 60f, 5f), new Vec3f(), (float)(BEFirework.rand.NextDouble() * 0.25 + 1.5), (float)(BEFirework.rand.NextDouble() * 0.800000011920929 + 0.30000001192092896), 1f, 1f, 0);
				particles.MinPos.Add(this.Pos);
				particles.AddPos.Set(new Vec3d(0.0, 0.0, 0.0));
				particles.AddVelocity.Set(new Vec3f(-10f, -10f, -10f));
				particles.SizeEvolve = new EvolvingNatFloat(3, 6f);
				particles.VertexFlags = 255;
				particles.WithTerrainCollision = true;
				particles.RandomVelocityChange = true;
			}
			else
			{
				int color6 = ColorUtil.ToRgba(BEFirework.rand.Next(100, 255), BEFirework.rand.Next(100, 255), BEFirework.rand.Next(100, 255), BEFirework.rand.Next(100, 255));
				particles = new SimpleParticleProperties(1f, 1f, color6, new Vec3d(0.5, 1.0, 0.5), new Vec3d(), new Vec3f(13.5f, 15f, 13.5f), new Vec3f(), (float)(BEFirework.rand.NextDouble() * 0.029999999329447746), (float)(BEFirework.rand.NextDouble() * 3.0 + 0.0), 5f, 8f, 1);
				particles.MinPos.Add(this.Pos);
				particles.AddPos.Set(new Vec3d(0.0, 0.0, 0.0));
				particles.AddVelocity.Set(new Vec3f(-27f, 10f, -27f));
				particles.SizeEvolve = new EvolvingNatFloat(3, 3f);
				particles.VertexFlags = 255;
				particles.SelfPropelled = true;
				particles.WindAffected = true;
				particles.WindAffectednes = 0.2f;
				particles.RandomVelocityChange = true;
			}
			return particles;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000050DC File Offset: 0x000032DC
		private void GenerateParticles(IAsyncParticleManager manager)
		{
			SimpleParticleProperties particles = this.BuildPrimaryParticles(base.Block.LastCodePart(0));
			if (base.Block.LastCodePart(0) == "missile")
			{
				AdvancedParticleProperties[] death = new AdvancedParticleProperties[]
				{
					this.BuildDeathMissileParticles()
				};
				SimpleParticleProperties simpleParticleProperties = particles;
				IParticlePropertiesProvider[] deathParticles = death;
				simpleParticleProperties.DeathParticles = deathParticles;
			}
			else if (base.Block.LastCodePart(0) == "fountain")
			{
				AdvancedParticleProperties[] death2 = new AdvancedParticleProperties[]
				{
					this.BuildDeathFountainParticles()
				};
				SimpleParticleProperties simpleParticleProperties2 = particles;
				IParticlePropertiesProvider[] deathParticles = death2;
				simpleParticleProperties2.DeathParticles = deathParticles;
			}
			else if (base.Block.LastCodePart(0) == "beehive")
			{
				AdvancedParticleProperties[] death3 = new AdvancedParticleProperties[]
				{
					this.BuildDeathBeehiveParticles()
				};
				SimpleParticleProperties simpleParticleProperties3 = particles;
				IParticlePropertiesProvider[] deathParticles = death3;
				simpleParticleProperties3.DeathParticles = deathParticles;
			}
			else if (!(base.Block.LastCodePart(0) == "boomer"))
			{
				if (base.Block.LastCodePart(0) == "bamboobarrage")
				{
					AdvancedParticleProperties[] death4 = new AdvancedParticleProperties[]
					{
						this.BuildDeathBambooBarrageParticles()
					};
					SimpleParticleProperties simpleParticleProperties4 = particles;
					IParticlePropertiesProvider[] deathParticles = death4;
					simpleParticleProperties4.DeathParticles = deathParticles;
				}
				else
				{
					AdvancedParticleProperties[] death5 = new AdvancedParticleProperties[]
					{
						this.BuildDeathCherryBombParticles()
					};
					SimpleParticleProperties simpleParticleProperties5 = particles;
					IParticlePropertiesProvider[] deathParticles = death5;
					simpleParticleProperties5.DeathParticles = deathParticles;
				}
			}
			manager.Spawn(particles);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00005210 File Offset: 0x00003410
		private static BlockPos[] AreaAround(BlockPos pos)
		{
			return new BlockPos[]
			{
				pos.WestCopy(1),
				pos.SouthCopy(1),
				pos.EastCopy(1),
				pos.NorthCopy(1),
				pos.WestCopy(1).UpCopy(1),
				pos.SouthCopy(1).UpCopy(1),
				pos.EastCopy(1).UpCopy(1),
				pos.NorthCopy(1).UpCopy(1),
				pos.WestCopy(1).DownCopy(1),
				pos.SouthCopy(1).DownCopy(1),
				pos.EastCopy(1).DownCopy(1),
				pos.NorthCopy(1).DownCopy(1)
			};
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000052D0 File Offset: 0x000034D0
		private void OnTick(float dt)
		{
			if (this.IsLit)
			{
				this.remainingSeconds -= dt;
				int lightTime = (int)this.FuseTimeSeconds - 1;
				if (base.Block.LastCodePart(0) == "missile" || base.Block.LastCodePart(0) == "boomer")
				{
					lightTime = 1;
				}
				if (this.Api.Side == 1 && this.remainingSeconds < (float)lightTime && !this.NeibChecked)
				{
					this.NeibChecked = true;
					BlockPos[] neibBlockPos = BEFirework.AreaAround(this.Pos);
					if (neibBlockPos != null)
					{
						foreach (BlockPos neib in neibBlockPos)
						{
							if (neib != null && this.IgnitedPlayer != null)
							{
								BEFuse befuse = this.IgnitedPlayer.Entity.World.BlockAccessor.GetBlockEntity(neib) as BEFuse;
								if (befuse != null)
								{
									befuse.OnIgnite(this.IgnitedPlayer);
								}
							}
						}
					}
				}
				if (this.remainingSeconds <= 0f)
				{
					base.Block.SpawnBlockBrokenParticles(this.Pos, null);
					this.Api.World.BlockAccessor.BreakBlock(this.Pos, null, 0f);
				}
				if (this.Api.Side == 1 && this.remainingSeconds <= 0f)
				{
					this.Combust(dt);
				}
				if (this.Api.Side == 2)
				{
					BEFirework.smallSparks.MinPos.Set((double)this.Pos.X + 0.45, (double)this.Pos.Y + 0.53, (double)this.Pos.Z + 0.45);
					this.Api.World.SpawnParticles(BEFirework.smallSparks, null);
				}
				if (base.Block.LastCodePart(0) == "missile")
				{
					if ((double)this.remainingSeconds < 0.2)
					{
						if (this.Api.Side == 2)
						{
							this.InitializeTimer();
							return;
						}
						int rndSound = BEFirework.rand.Next(1) + 1;
						this.Api.World.PlaySoundAt(new AssetLocation("primitivesurvival:sounds/fireworks/whistlebang" + rndSound.ToString()), (double)((float)this.Pos.X + 0.5f), (double)((float)this.Pos.Y + 0.5f), (double)((float)this.Pos.Z + 0.5f), null, true, 32f, 1f);
						return;
					}
				}
				else if (base.Block.LastCodePart(0) == "fountain")
				{
					if (this.Api.Side == 2)
					{
						if (!this.particleInitialized)
						{
							this.InitializeTimer();
							this.particleInitialized = true;
							this.fountainSound.Start();
						}
						if ((double)this.remainingSeconds < 0.1)
						{
							this.killOffThread = true;
							this.fountainSound.Stop();
							return;
						}
					}
				}
				else if (base.Block.LastCodePart(0) == "beehive")
				{
					if (this.Api.Side == 2)
					{
						if (!this.particleInitialized)
						{
							this.InitializeTimer();
							this.particleInitialized = true;
							this.beehiveSound.Start();
						}
						if ((double)this.remainingSeconds < 0.1)
						{
							this.killOffThread = true;
							this.beehiveSound.Stop();
							return;
						}
					}
				}
				else if (base.Block.LastCodePart(0) == "boomer")
				{
					if ((double)this.remainingSeconds < 0.2 && this.Api.Side == 2)
					{
						this.InitializeTimer();
						return;
					}
				}
				else if (base.Block.LastCodePart(0) == "bamboobarrage")
				{
					if (this.Api.Side == 2)
					{
						if (!this.particleInitialized)
						{
							this.InitializeTimer();
							this.particleInitialized = true;
							this.bamboobarrageSound.Start();
						}
						if ((double)this.remainingSeconds < 0.1)
						{
							this.killOffThread = true;
							this.bamboobarrageSound.Stop();
							return;
						}
					}
				}
				else if (this.Api.Side == 2)
				{
					if (!this.particleInitialized)
					{
						this.InitializeTimer();
						this.particleInitialized = true;
						this.cherrybombSound.Start();
					}
					if ((double)this.remainingSeconds < 0.1)
					{
						this.killOffThread = true;
						this.cherrybombSound.Stop();
					}
				}
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00005744 File Offset: 0x00003944
		private void Combust(float dt)
		{
			if (base.Block.LastCodePart(0) == "missile" || base.Block.LastCodePart(0) == "boomer")
			{
				((IServerWorldAccessor)this.Api.World).CreateExplosion(this.Pos, this.BlastType, (double)this.BlastRadius, (double)this.InjureRadius, 1f, null);
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000057B8 File Offset: 0x000039B8
		internal void OnBlockExploded(BlockPos pos, string ignitedByPlayerUid)
		{
			if (this.Api.Side == 1 && (!this.IsLit || (double)this.remainingSeconds > 0.3))
			{
				this.Api.World.RegisterCallback(new Action<float>(this.Combust), 250);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005B RID: 91 RVA: 0x0000580F File Offset: 0x00003A0F
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00005817 File Offset: 0x00003A17
		public bool IsLit { get; private set; }

		// Token: 0x0600005D RID: 93 RVA: 0x00005820 File Offset: 0x00003A20
		internal void OnIgnite(IPlayer byPlayer)
		{
			if (this.IsLit)
			{
				return;
			}
			if (this.Api.Side == 2)
			{
				this.fuseSound.Start();
			}
			this.IsLit = true;
			this.NeibChecked = false;
			this.remainingSeconds = this.FuseTimeSeconds;
			if (base.Block.LastCodePart(0) == "missile" || base.Block.LastCodePart(0) == "boomer")
			{
				this.remainingSeconds = 2f;
			}
			this.ignitedByPlayerUid = ((byPlayer != null) ? byPlayer.PlayerUID : null);
			this.IgnitedPlayer = byPlayer;
			this.MarkDirty(false, null);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000058C8 File Offset: 0x00003AC8
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			this.remainingSeconds = tree.GetFloat("remainingSeconds", 0f);
			this.IsLit = (tree.GetInt("lit", 0) > 0);
			this.ignitedByPlayerUid = tree.GetString("ignitedByPlayerUid", null);
			if (!this.IsLit)
			{
				ICoreAPI api = this.Api;
				if (api != null && api.Side == 2)
				{
					this.fuseSound.Stop();
				}
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00005944 File Offset: 0x00003B44
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetFloat("remainingSeconds", this.remainingSeconds);
			tree.SetInt("lit", (this.IsLit > false) ? 1 : 0);
			tree.SetString("ignitedByPlayerUid", this.ignitedByPlayerUid);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00005984 File Offset: 0x00003B84
		private AdvancedParticleProperties BuildDeathMissileParticles()
		{
			AdvancedParticleProperties advancedParticleProperties = new AdvancedParticleProperties
			{
				GravityEffect = NatFloat.createUniform(0.28f, 0.1f),
				Size = NatFloat.createUniform(0.5f, 1.5f),
				LifeLength = NatFloat.createUniform(0.5f, 0.2f),
				Quantity = NatFloat.createUniform(800f, 100f),
				Velocity = new NatFloat[]
				{
					NatFloat.createUniform(5f, -10f),
					NatFloat.createUniform(3f, -10f),
					NatFloat.createUniform(5f, -10f)
				},
				ParticleModel = 0
			};
			advancedParticleProperties.HsvaColor = null;
			advancedParticleProperties.Color = ColorUtil.ToRgba(BEFirework.rand.Next(150, 255), BEFirework.rand.Next(0, 255), BEFirework.rand.Next(0, 255), BEFirework.rand.Next(50, 255));
			advancedParticleProperties.basePos = this.Pos.ToVec3d().AddCopy(0.5, 1.5, 0.5);
			advancedParticleProperties.basePos = advancedParticleProperties.basePos.Add(0.5 + 0.0 * BEFirework.rand.NextDouble(), 1.5 + 0.0 * BEFirework.rand.NextDouble(), 0.5 + 0.0 * BEFirework.rand.NextDouble());
			advancedParticleProperties.SizeEvolve = new EvolvingNatFloat(3, 1f);
			advancedParticleProperties.VertexFlags = 255;
			advancedParticleProperties.TerrainCollision = true;
			advancedParticleProperties.WindAffectednes = 0.1f;
			return advancedParticleProperties;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00005B58 File Offset: 0x00003D58
		private AdvancedParticleProperties BuildDeathFountainParticles()
		{
			AdvancedParticleProperties advancedParticleProperties = new AdvancedParticleProperties
			{
				GravityEffect = NatFloat.createUniform(-0.1f, -0.2f),
				Size = NatFloat.createUniform(0.2f, 0.1f),
				LifeLength = NatFloat.createUniform(0.3f, 0.1f),
				Quantity = NatFloat.createUniform(1f, 0f),
				Velocity = new NatFloat[]
				{
					NatFloat.createUniform(0.3f, -0.6f),
					NatFloat.createUniform(1.1f, 0.1f),
					NatFloat.createUniform(0.3f, -0.6f)
				},
				ParticleModel = 0
			};
			advancedParticleProperties.HsvaColor = null;
			advancedParticleProperties.Color = ColorUtil.ToRgba(122, 22, BEFirework.rand.Next(122, 255), 22);
			advancedParticleProperties.basePos = this.Pos.ToVec3d().AddCopy(0f, 0f, 0f);
			advancedParticleProperties.basePos = advancedParticleProperties.basePos.Add(0.0 + 1.0 * BEFirework.rand.NextDouble(), 0.0 + 0.0 * BEFirework.rand.NextDouble(), 0.0 + 1.0 * BEFirework.rand.NextDouble());
			advancedParticleProperties.VertexFlags = 194;
			advancedParticleProperties.TerrainCollision = true;
			advancedParticleProperties.WindAffectednes = 0.2f;
			advancedParticleProperties.RandomVelocityChange = true;
			return advancedParticleProperties;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00005CE8 File Offset: 0x00003EE8
		private AdvancedParticleProperties BuildDeathBeehiveParticles()
		{
			AdvancedParticleProperties advancedParticleProperties = new AdvancedParticleProperties
			{
				GravityEffect = NatFloat.createUniform(-0.1f, 0f),
				Size = NatFloat.createUniform(0.5f, 1.5f),
				LifeLength = NatFloat.createUniform(1.3f, 0f),
				Quantity = NatFloat.createUniform(10f, 0f),
				Velocity = new NatFloat[]
				{
					NatFloat.createUniform(5f, -10f),
					NatFloat.createUniform(3f, -10f),
					NatFloat.createUniform(5f, -10f)
				},
				ParticleModel = 0
			};
			advancedParticleProperties.HsvaColor = null;
			advancedParticleProperties.Color = ColorUtil.ToRgba(BEFirework.rand.Next(150, 255), BEFirework.rand.Next(220, 255), BEFirework.rand.Next(220, 255), 50);
			advancedParticleProperties.basePos = this.Pos.ToVec3d().AddCopy(0.5, 1.5, 0.5);
			advancedParticleProperties.basePos = advancedParticleProperties.basePos.Add(0.5 + 0.0 * BEFirework.rand.NextDouble(), 1.5 + 0.0 * BEFirework.rand.NextDouble(), 0.5 + 0.0 * BEFirework.rand.NextDouble());
			advancedParticleProperties.SizeEvolve = new EvolvingNatFloat(3, 1f);
			advancedParticleProperties.VertexFlags = 255;
			advancedParticleProperties.TerrainCollision = true;
			advancedParticleProperties.WindAffectednes = 0.1f;
			advancedParticleProperties.RandomVelocityChange = true;
			return advancedParticleProperties;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00005EBC File Offset: 0x000040BC
		private AdvancedParticleProperties BuildDeathBambooBarrageParticles()
		{
			AdvancedParticleProperties advancedParticleProperties = new AdvancedParticleProperties
			{
				GravityEffect = NatFloat.createUniform(0.28f, 0.1f),
				Size = NatFloat.createUniform(1f, 2f),
				LifeLength = NatFloat.createUniform(0.1f, 0.2f),
				Quantity = NatFloat.createUniform(100f, 50f),
				Velocity = new NatFloat[]
				{
					NatFloat.createUniform(5f, -10f),
					NatFloat.createUniform(3f, -10f),
					NatFloat.createUniform(5f, -10f)
				},
				ParticleModel = 1
			};
			advancedParticleProperties.HsvaColor = null;
			advancedParticleProperties.Color = ColorUtil.ToRgba(BEFirework.rand.Next(150, 255), 255, BEFirework.rand.Next(100, 120), BEFirework.rand.Next(100, 120));
			advancedParticleProperties.basePos = this.Pos.ToVec3d().AddCopy(0.5, 1.5, 0.5);
			advancedParticleProperties.basePos = advancedParticleProperties.basePos.Add(0.5 + 0.0 * BEFirework.rand.NextDouble(), 1.5 + 0.0 * BEFirework.rand.NextDouble(), 0.5 + 0.0 * BEFirework.rand.NextDouble());
			advancedParticleProperties.SizeEvolve = new EvolvingNatFloat(3, 1f);
			advancedParticleProperties.VertexFlags = 255;
			advancedParticleProperties.TerrainCollision = true;
			advancedParticleProperties.WindAffectednes = 0.1f;
			return advancedParticleProperties;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00006080 File Offset: 0x00004280
		private AdvancedParticleProperties BuildDeathCherryBombParticles()
		{
			AdvancedParticleProperties advancedParticleProperties = new AdvancedParticleProperties
			{
				GravityEffect = NatFloat.createUniform(0.28f, 0f),
				Size = NatFloat.createUniform(0.2f, 0.5f),
				LifeLength = NatFloat.createUniform(0.1f, 0.05f),
				Quantity = NatFloat.createUniform(300f, 700f),
				Velocity = new NatFloat[]
				{
					NatFloat.createUniform(3f, -6f),
					NatFloat.createUniform(3f, -6f),
					NatFloat.createUniform(3f, -6f)
				},
				ParticleModel = 0
			};
			advancedParticleProperties.HsvaColor = null;
			int r = 255;
			int g = 255;
			int b = 255;
			int flip = BEFirework.rand.Next(7);
			if (flip == 0 || flip == 3 || flip == 5)
			{
				r = 0;
			}
			if (flip == 1 || flip == 3 || flip == 4)
			{
				g = 0;
			}
			if (flip == 2 || flip == 4 || flip == 5)
			{
				b = 0;
			}
			advancedParticleProperties.Color = ColorUtil.ToRgba(255, r, g, b);
			advancedParticleProperties.basePos = this.Pos.ToVec3d().AddCopy(0.5, 1.5, 0.5);
			advancedParticleProperties.basePos = advancedParticleProperties.basePos.Add(0.5 + 0.0 * BEFirework.rand.NextDouble(), 1.5 + 0.0 * BEFirework.rand.NextDouble(), 0.5 + 0.0 * BEFirework.rand.NextDouble());
			advancedParticleProperties.VertexFlags = 255;
			advancedParticleProperties.WindAffectednes = 0.1f;
			advancedParticleProperties.RandomVelocityChange = true;
			return advancedParticleProperties;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00006254 File Offset: 0x00004454
		~BEFirework()
		{
			if (this.fuseSound != null)
			{
				this.fuseSound.Dispose();
			}
			if (this.fountainSound != null)
			{
				this.fountainSound.Dispose();
			}
			if (this.beehiveSound != null)
			{
				this.beehiveSound.Dispose();
			}
			if (this.bamboobarrageSound != null)
			{
				this.bamboobarrageSound.Dispose();
			}
			if (this.particleInitialized)
			{
				this.killOffThread = true;
				this.InitializeTimer();
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000062DC File Offset: 0x000044DC
		public override void OnBlockRemoved()
		{
			base.OnBlockRemoved();
			if (this.fuseSound != null)
			{
				this.fuseSound.Stop();
			}
			if (this.fountainSound != null)
			{
				this.fountainSound.Stop();
			}
			if (this.beehiveSound != null)
			{
				this.beehiveSound.Stop();
			}
			if (this.bamboobarrageSound != null)
			{
				this.bamboobarrageSound.Stop();
			}
			if (this.particleInitialized)
			{
				this.killOffThread = true;
				this.InitializeTimer();
			}
		}

		// Token: 0x04000024 RID: 36
		private float remainingSeconds;

		// Token: 0x04000025 RID: 37
		private string ignitedByPlayerUid;

		// Token: 0x04000026 RID: 38
		private float blastRadius;

		// Token: 0x04000027 RID: 39
		private float injureRadius;

		// Token: 0x04000028 RID: 40
		private EnumBlastType blastType;

		// Token: 0x04000029 RID: 41
		private ILoadedSound fuseSound;

		// Token: 0x0400002A RID: 42
		private ILoadedSound fountainSound;

		// Token: 0x0400002B RID: 43
		private ILoadedSound beehiveSound;

		// Token: 0x0400002C RID: 44
		private ILoadedSound bamboobarrageSound;

		// Token: 0x0400002D RID: 45
		private ILoadedSound cherrybombSound;

		// Token: 0x0400002E RID: 46
		public static SimpleParticleProperties smallSparks;

		// Token: 0x0400002F RID: 47
		public IPlayer IgnitedPlayer;

		// Token: 0x04000030 RID: 48
		public ICoreClientAPI capi;

		// Token: 0x04000031 RID: 49
		private static readonly Random rand = new Random();

		// Token: 0x04000032 RID: 50
		private double storedInterval;

		// Token: 0x04000033 RID: 51
		private double interval;

		// Token: 0x04000034 RID: 52
		private bool killOffThread;

		// Token: 0x04000035 RID: 53
		private bool particleInitialized;

		// Token: 0x04000036 RID: 54
		public bool NeibChecked;
	}
}
