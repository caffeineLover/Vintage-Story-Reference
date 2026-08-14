using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class EntityThrownItem : EntityProjectileBase
{
	public float collidedAccum;

	public float VerticalImpactBreakChance;

	public float ImpactParticleSize = 1f;

	public int ImpactParticleCount = 20;

	private float xdir;

	private float zdir;

	public override bool IsInteractable => false;

	public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
	{
		base.Initialize(properties, api, InChunkIndex3d);
	}

	public override void OnTesselation(ref Shape entityShape, string shapePathForLogging)
	{
		base.OnTesselation(ref entityShape, shapePathForLogging);
		base.ProjectileStack.ResolveBlockOrItem(Api.World);
		CompositeShape pcshape = ((base.ProjectileStack.Class == EnumItemClass.Item) ? base.ProjectileStack.Item.Shape : base.ProjectileStack.Block.Shape);
		IDictionary<string, CompositeTexture> dictionary;
		if (base.ProjectileStack.Class != EnumItemClass.Item)
		{
			dictionary = base.ProjectileStack.Block.Textures;
		}
		else
		{
			IDictionary<string, CompositeTexture> textures = base.ProjectileStack.Item.Textures;
			dictionary = textures;
		}
		IDictionary<string, CompositeTexture> ptextures = dictionary;
		if (pcshape == null)
		{
			return;
		}
		ICoreClientAPI capi = Api as ICoreClientAPI;
		entityShape = capi.TesselatorManager.GetCachedShape(pcshape.Base);
		IDictionary<string, CompositeTexture> textures2 = base.Properties.Client.Textures;
		foreach (KeyValuePair<string, CompositeTexture> val in ptextures)
		{
			CompositeTexture ownTex = val.Value.Clone();
			textures2[val.Key] = ownTex;
			ownTex.Bake(Api.Assets);
			capi.EntityTextureAtlas.GetOrInsertTexture(ownTex.Baked.TextureFilenames[0], out var textureSubid, out var _);
			ownTex.Baked.TextureSubId = textureSubid;
		}
	}

	public override void OnGameTick(float dt)
	{
		base.OnGameTick(dt);
		if (ShouldDespawn)
		{
			return;
		}
		EntityPos pos = base.Pos;
		base.Stuck = Api.Side == EnumAppSide.Server && pos.Motion.LengthSq() < 0.0001 && World.CollisionTester.IsColliding(World.BlockAccessor, CollisionBox, base.Pos.XYZ);
		if (base.Stuck)
		{
			pos.Pitch = 0f;
			pos.Roll = 0f;
			pos.Motion.X = 0.0;
			pos.Motion.Z = 0.0;
			collidedAccum += dt;
			if (!base.Collectible && collidedAccum > 1f)
			{
				Die();
			}
		}
		else
		{
			pos.Pitch = (float)World.ElapsedMilliseconds / 300f % ((float)Math.PI * 2f);
			pos.Roll = 0f;
			pos.Yaw = (float)World.ElapsedMilliseconds / 400f % ((float)Math.PI * 2f);
		}
		if (World is IServerWorldAccessor && (!base.Stuck || pos.Motion.Length() * 4.0 > 0.10000000149011612))
		{
			Entity entity = World.GetNearestEntity(base.Pos.XYZ, 5f, 5f, (Entity e) => e.EntityId != EntityId && (base.FiredBy == null || e.EntityId != base.FiredBy.EntityId || World.ElapsedMilliseconds - msLaunch >= 500) && e.IsInteractable && e.SelectionBox.ToDouble().Translate(e.Pos.X, e.Pos.Y, e.Pos.Z).ShortestDistanceFrom(base.Pos.X, base.Pos.Y, base.Pos.Z) < 0.4000000059604645);
			if (entity != null)
			{
				DamageSource damageSource = new DamageSource
				{
					Source = ((base.FiredBy is EntityPlayer) ? EnumDamageSource.Player : EnumDamageSource.Entity),
					SourceEntity = this,
					CauseEntity = base.FiredBy,
					Type = base.DamageType,
					DamageTier = base.DamageTier,
					IgnoreInvFrames = base.IgnoreInvFrames,
					YDirKnockbackDiv = 3f
				};
				bool didDamage = false;
				if (entity.ShouldReceiveDamage(damageSource, base.Damage))
				{
					didDamage = entity.ReceiveDamage(damageSource, base.Damage);
				}
				World.PlaySoundAt(new AssetLocation("sounds/thud"), this, null, randomizePitch: false);
				World.SpawnCubeParticles(entity.Pos.XYZ.OffsetCopy(0.0, 0.2, 0.0), base.ProjectileStack, 0.2f, ImpactParticleCount, ImpactParticleSize);
				if (base.FiredBy is EntityPlayer && didDamage)
				{
					World.PlaySoundFor(new AssetLocation("sounds/player/projectilehit"), (base.FiredBy as EntityPlayer).Player, randomizePitch: false, 24f);
				}
				if (World.Rand.NextDouble() > (double)base.DropOnImpactChance)
				{
					Die();
				}
				return;
			}
		}
		beforeCollided = false;
		motionBeforeCollide.Set(pos.Motion.X, pos.Motion.Y, pos.Motion.Z);
	}

	public override void OnCollided()
	{
		EntityPos pos = base.Pos;
		double currentMotionAmount = pos.Motion.Length();
		if (currentMotionAmount == 0.0)
		{
			return;
		}
		if (!beforeCollided && World is IServerWorldAccessor)
		{
			double motionStrength = motionBeforeCollide.Length();
			if (motionStrength == 0.0)
			{
				motionStrength = currentMotionAmount;
			}
			float strength = GameMath.Clamp((float)motionStrength * 4f, 0f, 1f);
			xdir = 0f;
			zdir = 0f;
			if (CollidedHorizontally)
			{
				xdir = ((pos.Motion.X != 0.0) ? 1 : (-1));
				zdir = ((pos.Motion.Z != 0.0) ? 1 : (-1));
				pos.Motion.X = (double)xdir * motionBeforeCollide.X * 0.4000000059604645;
				pos.Motion.Z = (double)zdir * motionBeforeCollide.Z * 0.4000000059604645;
				if (strength > 0.1f && World.Rand.NextDouble() > (double)base.DropOnImpactChance)
				{
					Die();
				}
			}
			if (CollidedVertically)
			{
				bool bounceOrStop = false;
				if (motionBeforeCollide.Y <= -0.01)
				{
					pos.Motion.Y = GameMath.Clamp(motionBeforeCollide.Y * -0.30000001192092896, -0.10000000149011612, 0.10000000149011612);
					bounceOrStop = true;
				}
				if (!bounceOrStop && pos.Motion.Y >= 0.0 && pos.Motion.Y <= 1E-07)
				{
					pos.Motion.X = 0.0;
					pos.Motion.Z = 0.0;
					bounceOrStop = true;
				}
				if (bounceOrStop && strength > 0.1f && World.Rand.NextDouble() > (double)(1f - VerticalImpactBreakChance))
				{
					Die();
				}
			}
			if (strength > 0f)
			{
				World.PlaySoundAt(new AssetLocation("sounds/thud"), this, null, randomizePitch: false, 32f, strength);
			}
			WatchedAttributes.MarkAllDirty();
		}
		beforeCollided = true;
	}

	public override void Die(EnumDespawnReason reason = EnumDespawnReason.Death, DamageSource damageSourceForDeath = null)
	{
		if (reason == EnumDespawnReason.Death && World.Side == EnumAppSide.Server)
		{
			base.ProjectileStack.ResolveBlockOrItem(World);
			World.SpawnCubeParticles(base.Pos.XYZ.OffsetCopy(0.0, 0.2, 0.0), base.ProjectileStack, 0.5f, ImpactParticleCount, ImpactParticleSize, null, new Vec3f(xdir * (float)motionBeforeCollide.X * 8f, 0f, zdir * (float)motionBeforeCollide.Z * 8f));
		}
		base.Die(reason, damageSourceForDeath);
	}

	public override void OnCollideWithLiquid()
	{
		if (motionBeforeCollide.Y < 0.0 && motionBeforeCollide.Y > -0.30000001192092896)
		{
			base.Pos.Motion.Y = GameMath.Clamp(motionBeforeCollide.Y * -0.5, -0.10000000149011612, 0.10000000149011612);
			PositionBeforeFalling.Y = base.Pos.Y + 1.0;
			doSplashEffects(0.5, 3.0);
		}
		else
		{
			base.OnCollideWithLiquid();
		}
	}
}
