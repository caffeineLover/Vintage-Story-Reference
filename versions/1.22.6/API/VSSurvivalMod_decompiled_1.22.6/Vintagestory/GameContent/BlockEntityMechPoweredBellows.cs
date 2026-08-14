using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace Vintagestory.GameContent;

public class BlockEntityMechPoweredBellows : BlockEntityBellows, IRenderer, IDisposable
{
	protected BEBehaviorMPConsumer mpc;

	protected AnimatableRenderer renderer;

	protected BlockEntityAnimationUtil animUtil;

	protected ICoreClientAPI capi;

	protected bool connected;

	public double RenderOrder => 0.0;

	public int RenderRange => 999;

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
		capi = api as ICoreClientAPI;
		animUtil = new BlockEntityAnimationUtil(api, this);
		RegisterGameTickListener(onTick, 500, 2);
	}

	private void onTick(float dt)
	{
		BlockFacing facing = BlockFacing.FromCode(base.Block.Variant["side"]);
		BlockPos powerSourcePos = Pos.DownCopy().AddCopy(facing.Opposite);
		Block armBlock = Api.World.BlockAccessor.GetBlock(powerSourcePos);
		if (armBlock is BlockMPConsumer)
		{
			if (!connected)
			{
				mpc = armBlock.GetBEBehavior<BEBehaviorMPConsumer>(powerSourcePos);
				connected = true;
				capi?.Event.RegisterRenderer(this, EnumRenderStage.Before, "mechpoweredbellows");
				animation.AnimationSpeed = 0f;
				animation.StartFrameOnce = mpc.AngleRad / ((float)Math.PI * 2f) * 60f;
				animUtil.StartAnimation(animation);
			}
		}
		else if (connected)
		{
			mpc = null;
			capi?.Event.UnregisterRenderer(this, EnumRenderStage.Before);
			connected = false;
			animUtil.StopAnimation(animation.Code);
		}
	}

	public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
	{
		if (mpc?.Network != null && animUtil.animator != null)
		{
			RunningAnimation animState = animUtil.animator.GetAnimationState("bellowing");
			if (animState != null)
			{
				animation.AnimationSpeed = mpc.Network.Speed * 2f;
				animState.CurrentFrame = mpc.AngleRad / ((float)Math.PI * 2f) * 60f;
			}
		}
		else
		{
			animation.AnimationSpeed = 0f;
		}
	}

	public override bool Interact(IPlayer byPlayer)
	{
		return false;
	}

	public override void OnBlockUnloaded()
	{
		base.OnBlockUnloaded();
		animUtil?.Dispose();
	}

	public override void OnBlockBroken(IPlayer byPlayer = null)
	{
		base.OnBlockBroken(byPlayer);
		animUtil?.Dispose();
	}

	public override void OnBlockRemoved()
	{
		base.OnBlockRemoved();
		animUtil?.Dispose();
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		if (animUtil?.animator == null)
		{
			BlockFacing facing = BlockFacing.FromCode(base.Block.Variant["side"]);
			Vec3f rot = new Vec3f(0f, -facing.HorizontalAngleIndex * 90, 0f);
			animUtil?.InitializeAnimator("mechpoweredbellows", null, null, rot);
		}
		if (animUtil.activeAnimationsByAnimCode.Count <= 0)
		{
			if (animUtil.animator != null)
			{
				return animUtil.animator.ActiveAnimationCount > 0;
			}
			return false;
		}
		return true;
	}

	public void Dispose()
	{
	}
}
