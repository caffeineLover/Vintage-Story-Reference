using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class ModSystemSnowShoveling : ModSystem
{
	protected Dictionary<string, Vec3d> snowshovelingplayers = new Dictionary<string, Vec3d>();

	protected ICoreServerAPI? sapi;

	protected ICoreClientAPI? capi;

	protected ILoadedSound? shovellingSound;

	private float lookingAtSnowAccum;

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return true;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		sapi = api;
		api.Event.RegisterGameTickListener(snowShovelTickServer, 75);
		sapi.Event.PlayerDisconnect += OnPlayerPlayerDisconnect;
	}

	public override void StartClientSide(ICoreClientAPI api)
	{
		capi = api;
		api.Event.RegisterGameTickListener(snowShovelTickClient, 75);
	}

	private void OnPlayerPlayerDisconnect(IServerPlayer byPlayer)
	{
		StopSnowShoveling(byPlayer.PlayerUID);
	}

	private void snowShovelTickClient(float dt)
	{
		ArgumentNullException.ThrowIfNull(capi, "capi");
		BlockSelection currentBlockSelection = capi.World.Player.CurrentBlockSelection;
		if (currentBlockSelection != null && currentBlockSelection.Block?.BlockMaterial == EnumBlockMaterial.Snow)
		{
			lookingAtSnowAccum = 1f;
		}
		else
		{
			lookingAtSnowAccum -= dt;
		}
		if (snowshovelingplayers.Count > 0 && capi.World.Player.Entity.Controls.Forward && lookingAtSnowAccum > 0f)
		{
			if (shovellingSound == null)
			{
				shovellingSound = capi.World.LoadSound(new SoundParams
				{
					Location = "sounds/effect/snowshovelling",
					Range = 16f,
					RelativePosition = true,
					ShouldLoop = true,
					DisposeOnFinish = false,
					Volume = 0f
				});
			}
			if (!shovellingSound.IsPlaying)
			{
				shovellingSound.Start();
				shovellingSound.FadeIn(0.2f, null);
			}
		}
		else if (shovellingSound != null && shovellingSound.IsPlaying)
		{
			shovellingSound.FadeOutAndStop(0.4f);
		}
	}

	private void snowShovelTickServer(float dt)
	{
		ArgumentNullException.ThrowIfNull(sapi, "sapi");
		int snowCapacity = 8;
		foreach (KeyValuePair<string, Vec3d> snowshovelingplayer in snowshovelingplayers)
		{
			snowshovelingplayer.Deconstruct(out var key, out var value);
			string uid = key;
			Vec3d prevpos = value;
			IPlayer plr = sapi.World.PlayerByUid(uid);
			if (plr.Entity.BlockSelection == null)
			{
				continue;
			}
			Vec3d nowPos = plr.Entity.BlockSelection.FullPosition;
			if (plr.Entity.Controls.Forward && (double)nowPos.DistanceTo(prevpos) > 0.01)
			{
				Vec3d toDir = plr.Entity.Pos.GetViewVector().ToVec3d();
				toDir.Y = 0.0;
				toDir.Normalize();
				BlockPos fromPos = prevpos.AsBlockPos;
				prevpos.Add((nowPos - prevpos).Normalize().Mul(0.4));
				prevpos.Y = nowPos.Y;
				int snowLayers = Math.Min(snowCapacity, (int)Math.Ceiling(CountSnowLayers(fromPos)));
				if (snowLayers > 3)
				{
					int amount1 = sapi.World.Rand.Next(snowLayers) / 3;
					int amount2 = sapi.World.Rand.Next(snowLayers) / 3;
					ShoveSnowColumn(fromPos, toDir.RotatedCopy((float)Math.PI / 2f), amount1);
					ShoveSnowColumn(fromPos, toDir.RotatedCopy(-(float)Math.PI / 2f), amount2);
				}
				ShoveSnowColumn(fromPos, toDir, snowCapacity);
			}
		}
	}

	protected float CountSnowLayers(BlockPos fromPos)
	{
		ArgumentNullException.ThrowIfNull(sapi, "sapi");
		float snowLevel = sapi.World.BlockAccessor.GetBlock(fromPos).GetSnowLevel(fromPos);
		BlockPos tempPos = fromPos.UpCopy();
		Block aboveBlock = sapi.World.BlockAccessor.GetBlock(tempPos);
		float num = snowLevel + aboveBlock.GetSnowLevel(tempPos);
		tempPos.Down().Down();
		Block belowBlock = sapi.World.BlockAccessor.GetBlock(tempPos);
		return num + belowBlock.GetSnowLevel(tempPos);
	}

	public void ShoveSnowColumn(BlockPos fromPos, Vec3d toDir, float maxLayers)
	{
		if (maxLayers <= 0f)
		{
			return;
		}
		toDir.X = Math.Round(toDir.X);
		toDir.Y = Math.Round(toDir.Y);
		toDir.Z = Math.Round(toDir.Z);
		if (toDir.X == 0.0 && toDir.Z == 0.0)
		{
			return;
		}
		ArgumentNullException.ThrowIfNull(sapi, "sapi");
		IBlockAccessor blockAccessor = sapi.World.BlockAccessor;
		BlockPos currentSourcePos = fromPos;
		BlockPos tempPos = fromPos.UpCopy();
		if (blockAccessor.GetBlock(tempPos).GetSnowLevel(tempPos) > 0f)
		{
			currentSourcePos = fromPos.UpCopy();
			if (blockAccessor.GetBlock(tempPos.Up()).GetSnowLevel(tempPos) > 0f)
			{
				return;
			}
		}
		else if (blockAccessor.GetBlock(fromPos).GetSnowLevel(fromPos) == 0f)
		{
			currentSourcePos = tempPos.Down().Down();
			if (blockAccessor.GetBlock(tempPos).GetSnowLevel(tempPos) <= 0f)
			{
				return;
			}
		}
		float initialSnow = blockAccessor.GetBlock(currentSourcePos).GetSnowLevel(currentSourcePos);
		BlockPos frontPos = fromPos + toDir.AsBlockPos;
		frontPos.Y = Math.Min(frontPos.Y, currentSourcePos.Y);
		bool needsFall = TryMoveSnow(currentSourcePos, frontPos, maxLayers, allowFalling: true);
		spawnSnowParticles(fromPos, toDir);
		float remainingSnow = blockAccessor.GetBlock(currentSourcePos).GetSnowLevel(currentSourcePos);
		maxLayers -= initialSnow - remainingSnow;
		if (remainingSnow == 0f)
		{
			currentSourcePos.Down();
			initialSnow = blockAccessor.GetBlock(currentSourcePos).GetSnowLevel(currentSourcePos);
			needsFall = needsFall || TryMoveSnow(currentSourcePos, frontPos, maxLayers, allowFalling: true);
			remainingSnow = blockAccessor.GetBlock(currentSourcePos).GetSnowLevel(currentSourcePos);
		}
		if (needsFall)
		{
			blockAccessor.GetBlock(frontPos).GetBehavior<BlockBehaviorUnstableFalling>()?.createFallingBlock(sapi.World, frontPos);
		}
		maxLayers -= initialSnow - remainingSnow;
		frontPos.Up();
		TryMoveSnow(currentSourcePos, frontPos, maxLayers, allowFalling: false);
	}

	public bool TryMoveSnow(BlockPos fromPos, BlockPos toPos, float maxLevelMoved, bool allowFalling)
	{
		if (maxLevelMoved <= 0f)
		{
			return false;
		}
		ArgumentNullException.ThrowIfNull(sapi, "sapi");
		Block snowBlock = sapi.World.GetBlock("snowblock") ?? throw new Exception("Expected snow block to be included in game files");
		Block fromBlock = sapi.World.BlockAccessor.GetBlock(fromPos);
		float fromLevel = (float)Math.Ceiling(fromBlock.GetSnowLevel(fromPos));
		Block toBlock = sapi.World.BlockAccessor.GetBlock(toPos);
		float toLevel = (float)Math.Ceiling(toBlock.GetSnowLevel(toPos));
		float toMove = Math.Min(maxLevelMoved, fromLevel);
		Block newlySnowyBlock = ((toBlock.Id == 0) ? snowBlock : toBlock).GetSnowCoveredVariant(toPos, toMove + toLevel);
		Block snowlessBlock = fromBlock.GetSnowCoveredVariant(fromPos, 0f);
		if (snowlessBlock == null)
		{
			return false;
		}
		bool attached = true;
		float remainingSnow = fromLevel;
		if (newlySnowyBlock != null)
		{
			BlockPos downPos = toPos.DownCopy();
			attached = sapi.World.BlockAccessor.GetBlock(downPos).CanAttachBlockAt(sapi.World.BlockAccessor, newlySnowyBlock, downPos, BlockFacing.UP);
			BlockBehaviorUnstableFalling falling = newlySnowyBlock.GetBehavior<BlockBehaviorUnstableFalling>();
			if (falling != null && !attached && (!allowFalling || falling.FallingEntityAlreadyExists(sapi.World, toPos)))
			{
				return false;
			}
			toBlock.PerformSnowLevelUpdate(sapi.World.BlockAccessor, toPos, newlySnowyBlock, toMove + toLevel);
			sapi.World.BlockAccessor.TriggerNeighbourBlockUpdate(toPos);
			float snowLevelAfter = (float)Math.Ceiling(newlySnowyBlock.GetSnowLevel(toPos));
			remainingSnow = fromLevel + toLevel - snowLevelAfter;
		}
		else if (toBlock.IsLiquid())
		{
			remainingSnow = Math.Max(0f, fromLevel - maxLevelMoved);
		}
		if (fromLevel > remainingSnow)
		{
			Block desnowedBlock = fromBlock.GetSnowCoveredVariant(fromPos, remainingSnow) ?? snowlessBlock;
			fromBlock.PerformSnowLevelUpdate(sapi.World.BlockAccessor, fromPos, desnowedBlock, remainingSnow);
			sapi.World.BlockAccessor.TriggerNeighbourBlockUpdate(fromPos);
		}
		return !attached;
	}

	private void spawnSnowParticles(BlockPos fromPos, Vec3d toDir)
	{
		Vec3d uppos = fromPos.AddCopy(0f, 0.2f, 0f).ToVec3d();
		sapi.World.SpawnParticles(30f, -1, uppos, uppos.AddCopy(1f, 0.3f, 1f), new Vec3f(-2f + (float)toDir.X, 0.2f, -2f + (float)toDir.Z), new Vec3f(2f + (float)toDir.X, 2f, 2f + (float)toDir.Z), 2f, 1f, 1f, EnumParticleModel.Cube);
		SimpleParticleProperties dustParticles = FallingBlockParticlesModSystem.dustParticles.Clone(sapi.World);
		dustParticles.Color = -1;
		dustParticles.Color &= 16777215;
		dustParticles.Color |= -1778384896;
		dustParticles.MinPos.Set(uppos);
		dustParticles.MinSize = 0.35f;
		dustParticles.LifeLength = 2f;
		dustParticles.AddPos.Y = 0.15000000596046448;
		dustParticles.MinVelocity.Set(-0.2f, 0f, -0.2f);
		dustParticles.AddVelocity.Set(0.4f, 0f, 0.4f);
		dustParticles.MinQuantity = 1f;
		dustParticles.AddQuantity = 2f;
		sapi.World.SpawnParticles(dustParticles);
	}

	public void BeginSnowShoveling(string uid, Vec3d aimPos)
	{
		snowshovelingplayers[uid] = aimPos;
	}

	public void StopSnowShoveling(string uid)
	{
		snowshovelingplayers.Remove(uid);
	}
}
