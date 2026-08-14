using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000007 RID: 7
	public class BEBombFuse : BlockEntity
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002B11 File Offset: 0x00000D11
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00002B19 File Offset: 0x00000D19
		public bool CascadeLit { get; set; }

		// Token: 0x0600001C RID: 28 RVA: 0x00002B24 File Offset: 0x00000D24
		static BEBombFuse()
		{
			BEBombFuse.smallSparks.AddPos.Set(0.0625, 0.0, 0.0625);
			BEBombFuse.smallSparks.SizeEvolve = new EvolvingNatFloat(1, -0.05f);
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002BEC File Offset: 0x00000DEC
		public virtual float FuseTimeSeconds
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002BF3 File Offset: 0x00000DF3
		public virtual EnumBlastType BlastType
		{
			get
			{
				return this.blastType;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002BFB File Offset: 0x00000DFB
		public virtual float BlastRadius
		{
			get
			{
				return this.blastRadius;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002C03 File Offset: 0x00000E03
		public virtual float InjureRadius
		{
			get
			{
				return this.injureRadius;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002C0C File Offset: 0x00000E0C
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.RegisterGameTickListener(new Action<float>(this.OnTick), 50, 0);
			if (this.fuseSound == null && api.Side == 2)
			{
				this.fuseSound = ((IClientWorldAccessor)api.World).LoadSound(new SoundParams
				{
					Location = new AssetLocation("game:sounds/effect/fuse"),
					ShouldLoop = true,
					Position = this.Pos.ToVec3f().Add(0.5f, 0.25f, 0.5f),
					DisposeOnFinish = false,
					Volume = 0.1f,
					Range = 16f
				});
			}
			JsonObject attributes = base.Block.Attributes;
			int? num;
			if (attributes == null)
			{
				num = default(int?);
			}
			else
			{
				JsonObject jsonObject = attributes["blastRadius"];
				num = ((jsonObject != null) ? new int?(jsonObject.AsInt(4)) : default(int?));
			}
			int? num2 = num;
			this.blastRadius = (float)num2.GetValueOrDefault(4);
			JsonObject attributes2 = base.Block.Attributes;
			int? num3;
			if (attributes2 == null)
			{
				num3 = default(int?);
			}
			else
			{
				JsonObject jsonObject2 = attributes2["injureRadius"];
				num3 = ((jsonObject2 != null) ? new int?(jsonObject2.AsInt(8)) : default(int?));
			}
			num2 = num3;
			this.injureRadius = (float)num2.GetValueOrDefault(10);
			if (base.Block.Code.Path == "rockblastingbomb")
			{
				this.blastType = base.Block.Attributes["blastType"].AsInt(1);
				return;
			}
			JsonObject attributes3 = base.Block.Attributes;
			int? num4;
			if (attributes3 == null)
			{
				num4 = default(int?);
			}
			else
			{
				JsonObject jsonObject3 = attributes3["blastType"];
				num4 = ((jsonObject3 != null) ? new int?(jsonObject3.AsInt(0)) : default(int?));
			}
			num2 = num4;
			this.blastType = num2.GetValueOrDefault();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002DE4 File Offset: 0x00000FE4
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

		// Token: 0x06000023 RID: 35 RVA: 0x00002EA4 File Offset: 0x000010A4
		private void OnTick(float dt)
		{
			if (this.IsLit)
			{
				this.remainingSeconds -= dt;
				if (this.Api.Side == 1 && this.remainingSeconds < 2f && !this.NeibChecked)
				{
					this.NeibChecked = true;
					BlockPos[] neibBlockPos = BEBombFuse.AreaAround(this.Pos);
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
				if (this.Api.Side == 1 && this.remainingSeconds <= 0f)
				{
					this.Combust(dt);
				}
				if (this.Api.Side == 2)
				{
					BEBombFuse.smallSparks.MinPos.Set((double)this.Pos.X + 0.45, (double)this.Pos.Y + 0.53, (double)this.Pos.Z + 0.45);
					this.Api.World.SpawnParticles(BEBombFuse.smallSparks, null);
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002FF4 File Offset: 0x000011F4
		private void Combust(float dt)
		{
			if (this.NearToClaimedLand())
			{
				this.Api.World.PlaySoundAt(new AssetLocation("game:sounds/effect/extinguish"), (double)this.Pos.X + 0.5, (double)this.Pos.Y, (double)this.Pos.Z + 0.5, null, false, 16f, 1f);
				this.IsLit = false;
				this.MarkDirty(true, null);
				return;
			}
			this.Api.World.BlockAccessor.SetBlock(0, this.Pos);
			((IServerWorldAccessor)this.Api.World).CreateExplosion(this.Pos, this.BlastType, (double)this.BlastRadius, (double)this.InjureRadius, 1f, null);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000030C8 File Offset: 0x000012C8
		private bool NearToClaimedLand()
		{
			int rad = (int)Math.Ceiling((double)this.BlastRadius);
			Cuboidi exploArea = new Cuboidi(this.Pos.AddCopy(-rad, -rad, -rad), this.Pos.AddCopy(rad, rad, rad));
			List<LandClaim> claims = (this.Api as ICoreServerAPI).WorldManager.LandClaims;
			for (int i = 0; i < claims.Count; i++)
			{
				if (claims[i].Intersects(exploArea))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003144 File Offset: 0x00001344
		internal void OnBlockExploded(BlockPos pos, string ignitedByPlayerUid)
		{
			if (this.Api.Side == 1 && (!this.IsLit || (double)this.remainingSeconds > 0.3) && !this.NearToClaimedLand())
			{
				this.Api.World.RegisterCallback(new Action<float>(this.Combust), 250);
				this.CascadeLit = true;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000031AA File Offset: 0x000013AA
		// (set) Token: 0x06000028 RID: 40 RVA: 0x000031B2 File Offset: 0x000013B2
		public bool IsLit { get; private set; }

		// Token: 0x06000029 RID: 41 RVA: 0x000031BC File Offset: 0x000013BC
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
			this.ignitedByPlayerUid = ((byPlayer != null) ? byPlayer.PlayerUID : null);
			this.IgnitedPlayer = byPlayer;
			this.MarkDirty(false, null);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003228 File Offset: 0x00001428
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

		// Token: 0x0600002B RID: 43 RVA: 0x000032A4 File Offset: 0x000014A4
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetFloat("remainingSeconds", this.remainingSeconds);
			tree.SetInt("lit", (this.IsLit > false) ? 1 : 0);
			tree.SetString("ignitedByPlayerUid", this.ignitedByPlayerUid);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000032E4 File Offset: 0x000014E4
		~BEBombFuse()
		{
			if (this.fuseSound != null)
			{
				this.fuseSound.Dispose();
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003320 File Offset: 0x00001520
		public override void OnBlockRemoved()
		{
			base.OnBlockRemoved();
			if (this.fuseSound != null)
			{
				this.fuseSound.Stop();
			}
		}

		// Token: 0x0400000A RID: 10
		private float remainingSeconds;

		// Token: 0x0400000B RID: 11
		private string ignitedByPlayerUid;

		// Token: 0x0400000C RID: 12
		private float blastRadius;

		// Token: 0x0400000D RID: 13
		private float injureRadius;

		// Token: 0x0400000E RID: 14
		private EnumBlastType blastType;

		// Token: 0x0400000F RID: 15
		private ILoadedSound fuseSound;

		// Token: 0x04000010 RID: 16
		public static SimpleParticleProperties smallSparks = new SimpleParticleProperties(1f, 1f, ColorUtil.ToRgba(255, 255, 233, 0), new Vec3d(), new Vec3d(), new Vec3f(-3f, 5f, -3f), new Vec3f(3f, 8f, 3f), 0.03f, 1f, 0.05f, 0.15f, 0)
		{
			VertexFlags = 255
		};

		// Token: 0x04000011 RID: 17
		public IPlayer IgnitedPlayer;

		// Token: 0x04000013 RID: 19
		public bool NeibChecked;
	}
}
