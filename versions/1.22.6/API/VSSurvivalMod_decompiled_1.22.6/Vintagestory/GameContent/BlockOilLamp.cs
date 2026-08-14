using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockOilLamp : BlockTorch, IGroundStoredParticleEmitter
{
	protected BlockFacing facing;

	public override void OnLoaded(ICoreAPI api)
	{
		facing = BlockFacing.FromCode(Variant["orientation"]);
		base.OnLoaded(api);
	}

	public override void OnAsyncClientParticleTick(IAsyncParticleManager manager, BlockPos pos, float windAffectednessAtPos, float secondsTicking)
	{
		if (ParticleProperties != null && ParticleProperties.Length != 0)
		{
			for (int i = 0; i < ParticleProperties.Length; i++)
			{
				AdvancedParticleProperties bps = ParticleProperties[i];
				bps.WindAffectednesAtPos = windAffectednessAtPos;
				bps.basePos.X = (float)pos.X + 0.5f;
				bps.basePos.Y = (double)pos.InternalY + 15.0 / 32.0 + (double)(facing.Normalf.Y * 0.25f);
				bps.basePos.Z = (float)pos.Z + 0.5f;
				manager.Spawn(bps);
			}
		}
	}

	public void DoSpawnGSParticles(IAsyncParticleManager manager, BlockPos pos, Vec3f offset)
	{
		float meshAngle = api.World.BlockAccessor.GetBlockEntity<BlockEntityGroundStorage>(pos)?.MeshAngle ?? 0f;
		if (ParticleProperties != null && ParticleProperties.Length != 0)
		{
			Vec3f wickoffset = new Matrixf().RotateY(meshAngle).TransformVector(new Vec4f(-5f / 32f, 0f, -5f / 32f, 1f)).XYZ;
			for (int i = 0; i < ParticleProperties.Length; i++)
			{
				AdvancedParticleProperties bps = ParticleProperties[i];
				bps.WindAffectednesAtPos = 0f;
				bps.basePos.X = (float)pos.X + 0.5f + offset.X + wickoffset.X;
				bps.basePos.Y = (double)pos.InternalY + 9.0 / 64.0 + (double)offset.Y;
				bps.basePos.Z = (float)pos.Z + 0.5f + offset.Z + wickoffset.Z;
				manager.Spawn(bps);
			}
		}
	}

	public bool ShouldSpawnGSParticles(IWorldAccessor world, ItemStack stack)
	{
		return true;
	}
}
