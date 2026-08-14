using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockEntityBellows : BlockEntity
{
	public float PumpingSpeed = 1f;

	protected float animationDuration = 1.3333334f;

	protected long interactStartTotalMs;

	protected float originalAnimationSpeed = 1f;

	protected AnimationMetaData? animation;

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
		animation = base.Block.Attributes?["bellowAnimation"].AsObject<AnimationMetaData>();
		originalAnimationSpeed = animation?.AnimationSpeed ?? 1f;
	}

	public virtual bool Interact(IPlayer byPlayer)
	{
		IWorldAccessor world = Api.World;
		if ((float)(world.ElapsedMilliseconds - interactStartTotalMs) / 1000f < animationDuration / PumpingSpeed)
		{
			return false;
		}
		interactStartTotalMs = world.ElapsedMilliseconds;
		BlockFacing facing = BlockFacing.FromCode(base.Block.Variant["side"]);
		RegisterDelayedCallback(delegate
		{
			BlockPos pos = Pos.AddCopy(facing);
			world.BlockAccessor.GetBlock(pos).GetInterface<IBellowsAirReceiver>(world, pos)?.BlowAirInto(world, pos, 0.2f, facing);
			GetBehavior<BEBehaviorDurability>()?.DamageBlock();
		}, (int)(733f / PumpingSpeed));
		BlockEntityAnimationUtil animUtil = GetBehavior<BEBehaviorAnimatable>()?.animUtil;
		if (animUtil == null || animation == null)
		{
			return true;
		}
		animation.AnimationSpeed = originalAnimationSpeed * PumpingSpeed;
		animUtil.StartAnimation(animation);
		RunningAnimation ranim = animUtil.animator?.GetAnimationState(animation.Code);
		if (ranim != null)
		{
			ranim.SoundPlayedAtIteration = null;
			ranim.CurrentFrame = 0f;
			ranim.Iterations = 0;
		}
		return true;
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		BlockEntityAnimationUtil animUtil = GetBehavior<BEBehaviorAnimatable>()?.animUtil;
		if (animUtil?.animator == null)
		{
			float rotY = BlockFacing.FromCode(base.Block.Variant["side"]).HorizontalAngleIndex * 90 + 180;
			animUtil?.InitializeAnimator("bellows" + base.Block.Code, null, null, new Vec3f(0f, rotY, 0f));
		}
		return base.OnTesselation(mesher, tessThreadTesselator);
	}
}
