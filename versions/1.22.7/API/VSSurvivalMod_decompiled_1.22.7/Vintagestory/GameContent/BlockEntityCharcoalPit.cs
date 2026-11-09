using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockEntityCharcoalPit : BlockEntity
{
	protected ActionBoolReturn<BlockPos, BlockPos, BlockFacing, IWorldChunk> defaultCheckAction;

	private static float BurnHours = 18f;

	protected Dictionary<BlockPos, int> smokeLocations = new Dictionary<BlockPos, int>();

	protected double finishedAfterTotalHours;

	protected double startingAfterTotalHours;

	protected EnumCharcoalPitState state;

	protected string startedByPlayerUid = string.Empty;

	public int charcoalPitId;

	public int fireBlockId;

	public int[] charcoalPileId = new int[8];

	public bool Lit { get; protected set; }

	public virtual int MaxSize { get; set; } = 11;

	protected virtual float PitEfficiency { get; set; } = 1f;

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
		MaxSize = base.Block.Attributes?["maxSize"].AsInt(11) ?? 11;
		charcoalPitId = base.Block.BlockId;
		fireBlockId = Api.World.GetBlock(new AssetLocation("fire")).BlockId;
		for (int i = 0; i < 8; i++)
		{
			Block block = Api.World.GetBlock(new AssetLocation("charcoalpile-" + (i + 1)));
			ArgumentNullException.ThrowIfNull(block, "block");
			charcoalPileId[i] = block.BlockId;
		}
		defaultCheckAction = (BlockPos bpos, BlockPos npos, BlockFacing facing, IWorldChunk chunk) => BlockFirepit.IsFirewoodPile(Api.World, npos) ? true : false;
		if (api.Side == EnumAppSide.Client)
		{
			if (Lit)
			{
				UpdateSmokeLocations();
			}
			RegisterGameTickListener(OnClientTick, 150);
		}
		else
		{
			RegisterGameTickListener(OnServerTick, 3000);
		}
	}

	protected virtual void OnClientTick(float dt)
	{
		if (state != EnumCharcoalPitState.Sealed)
		{
			return;
		}
		AdvancedParticleProperties[] partProps = base.Block?.ParticleProperties;
		if (partProps == null)
		{
			return;
		}
		BlockPos pos = new BlockPos(Pos.dimension);
		foreach (KeyValuePair<BlockPos, int> val in smokeLocations)
		{
			if (Api.World.Rand.NextDouble() < 0.20000000298023224 && partProps.Length != 0)
			{
				pos.Set(val.Key.X, val.Value + 1, val.Key.Z);
				Block upblock = Api.World.BlockAccessor.GetBlock(pos);
				AdvancedParticleProperties particles = partProps[0];
				particles.basePos = BEBehaviorBurning.RandomBlockPos(Api.World.BlockAccessor, pos, upblock, BlockFacing.UP);
				particles.Quantity.avg = 1f;
				Api.World.SpawnParticles(particles);
				particles.Quantity.avg = 0f;
			}
		}
	}

	protected virtual void OnServerTick(float dt)
	{
		if (!Lit)
		{
			return;
		}
		if (startingAfterTotalHours <= Api.World.Calendar.TotalHours && state == EnumCharcoalPitState.Warmup)
		{
			finishedAfterTotalHours = Api.World.Calendar.TotalHours + (double)BurnHours;
			state = EnumCharcoalPitState.Sealed;
			MarkDirty();
		}
		if (state == EnumCharcoalPitState.Warmup)
		{
			return;
		}
		HashSet<BlockPos> holes = FindHolesInPit();
		if (holes == null)
		{
			return;
		}
		EnumCharcoalPitState beforeState = state;
		if (holes.Count > 0)
		{
			state = EnumCharcoalPitState.Unsealed;
			finishedAfterTotalHours = Api.World.Calendar.TotalHours + (double)BurnHours;
			float burnChance = Math.Clamp(1f - 0.1f * (float)(holes.Count - 1), 0.5f, 1f);
			foreach (BlockPos holePos in holes)
			{
				BlockPos firePos = holePos.Copy();
				Block block = Api.World.BlockAccessor.GetBlock(holePos);
				EntityPlayer byEntity = Api.World.PlayerByUid(startedByPlayerUid)?.Entity ?? Api.World.NearestPlayer(Pos.X, Pos.InternalY, Pos.Z)?.Entity;
				IIgnitable ign = block.GetInterface<IIgnitable>(Api.World, holePos);
				bool flag = byEntity != null;
				bool flag2;
				if (flag)
				{
					EnumIgniteState? enumIgniteState = ign?.OnTryIgniteBlock(byEntity, holePos, 10f);
					if (enumIgniteState.HasValue)
					{
						EnumIgniteState valueOrDefault = enumIgniteState.GetValueOrDefault();
						if ((uint)(valueOrDefault - 2) <= 1u)
						{
							flag2 = true;
							goto IL_01dc;
						}
					}
					flag2 = false;
					goto IL_01dc;
				}
				goto IL_01e0;
				IL_01e0:
				if (flag)
				{
					if (Api.World.Rand.NextDouble() < (double)burnChance)
					{
						EnumHandling useless = EnumHandling.PassThrough;
						ign.OnTryIgniteBlockOver(byEntity, holePos, 10f, ref useless);
					}
				}
				else
				{
					if (block.BlockId == 0 || block.BlockId == charcoalPitId)
					{
						continue;
					}
					BlockFacing[] aLLFACES = BlockFacing.ALLFACES;
					foreach (BlockFacing facing in aLLFACES)
					{
						facing.IterateThruFacingOffsets(firePos);
						if (Api.World.BlockAccessor.GetBlock(firePos).BlockId == 0 && Api.World.Rand.NextDouble() < (double)burnChance)
						{
							Api.World.BlockAccessor.SetBlock(fireBlockId, firePos);
							Api.World.BlockAccessor.GetBlockEntity(firePos)?.GetBehavior<BEBehaviorBurning>()?.OnFirePlaced(facing, startedByPlayerUid);
						}
					}
				}
				continue;
				IL_01dc:
				flag = flag2;
				goto IL_01e0;
			}
			MarkDirty();
		}
		else
		{
			state = EnumCharcoalPitState.Sealed;
			if (beforeState != state)
			{
				MarkDirty();
			}
			if (finishedAfterTotalHours <= Api.World.Calendar.TotalHours)
			{
				ConvertPit();
			}
		}
	}

	public void IgniteNow()
	{
		if (!Lit)
		{
			Lit = true;
			startingAfterTotalHours = Api.World.Calendar.TotalHours + 0.5;
			MarkDirty(redrawOnClient: true);
			if (Api.Side == EnumAppSide.Client)
			{
				UpdateSmokeLocations();
			}
		}
	}

	protected bool WalkPit(Action<BlockPos>? bAction, ActionBoolReturn<BlockPos, BlockPos, BlockFacing, IWorldChunk>? nAction)
	{
		HashSet<BlockPos> visitedPositions = new HashSet<BlockPos>();
		Queue<BlockPos> bfsQueue = new Queue<BlockPos>();
		bfsQueue.Enqueue(Pos.Copy());
		visitedPositions.Add(Pos.Copy());
		BlockPos minPos = Pos.Copy();
		BlockPos maxPos = Pos.Copy();
		IWorldChunk chunk = null;
		BlockPos bpos = Pos;
		BlockPos npos = Pos;
		while (bfsQueue.Count > 0)
		{
			bpos = bfsQueue.Dequeue();
			npos = bpos.Copy();
			bAction?.Invoke(bpos);
			BlockFacing[] aLLFACES = BlockFacing.ALLFACES;
			foreach (BlockFacing facing in aLLFACES)
			{
				facing.IterateThruFacingOffsets(npos);
				chunk = Api.World.BlockAccessor.GetChunkAtBlockPos(npos);
				if (chunk == null)
				{
					return false;
				}
				if ((nAction == null || nAction(bpos, npos, facing, chunk)) && InCube(npos, ref minPos, ref maxPos) && !visitedPositions.Contains(npos))
				{
					bfsQueue.Enqueue(npos.Copy());
					visitedPositions.Add(npos.Copy());
				}
			}
		}
		return true;
	}

	protected virtual void ConvertPit()
	{
		Dictionary<BlockPos, Vec3i> quantityPerColumn = new Dictionary<BlockPos, Vec3i>();
		NatFloat firewoodEfficiency = NatFloat.createUniform(0.75f, 0.25f);
		BlockPos bposGround = Pos;
		float totalEfficiency = 0f;
		int firewoodQuantity = 0;
		int bposY = 0;
		if (!WalkPit(delegate(BlockPos bpos)
		{
			bposY = bpos.Y;
			bposGround = bpos.DownCopy(bposY);
			if (quantityPerColumn.TryGetValue(bposGround, out Vec3i value))
			{
				value.Y = Math.Min(value.Y, bposY);
				value.Z = Math.Max(value.Z, bposY);
			}
			else
			{
				Vec3i vec3i = (quantityPerColumn[bposGround] = new Vec3i(0, bposY, bposY));
				value = vec3i;
			}
			firewoodQuantity = (base.Block as BlockCharcoalPit)?.GetFirewoodQuantity(Api.World, bpos, ref firewoodEfficiency) ?? 0;
			totalEfficiency = firewoodEfficiency.nextFloat(PitEfficiency);
			value.X += Math.Clamp(GameMath.RoundRandom(Api.World.Rand, (float)firewoodQuantity / 4f * totalEfficiency), 0, 8);
		}, defaultCheckAction))
		{
			return;
		}
		BlockPos lpos = new BlockPos(Pos.dimension);
		int charcoalQuantity = 0;
		int numPileBlocks = charcoalPileId.Length;
		foreach (KeyValuePair<BlockPos, Vec3i> val in quantityPerColumn)
		{
			lpos.Set(val.Key.X, val.Value.Y, val.Key.Z);
			charcoalQuantity = val.Value.X;
			while (lpos.Y <= val.Value.Z)
			{
				if (BlockFirepit.IsFirewoodPile(Api.World, lpos) || lpos == Pos)
				{
					if (charcoalQuantity > 0)
					{
						Api.World.BlockAccessor.SetBlock(charcoalPileId[GameMath.Clamp(charcoalQuantity, 0, numPileBlocks) - 1], lpos);
						charcoalQuantity -= 8;
					}
					else
					{
						Api.World.BlockAccessor.SetBlock(0, lpos);
					}
				}
				lpos.Up();
			}
		}
	}

	public void Init(IPlayer? player)
	{
		startedByPlayerUid = player?.PlayerUID ?? string.Empty;
	}

	protected virtual HashSet<BlockPos>? FindHolesInPit()
	{
		HashSet<BlockPos> holes = new HashSet<BlockPos>();
		Block nBlock = base.Block;
		bool containsNew = false;
		bool isFirewood = false;
		if (!WalkPit(null, delegate(BlockPos bpos, BlockPos npos, BlockFacing facing, IWorldChunk chunk)
		{
			isFirewood = BlockFirepit.IsFirewoodPile(Api.World, npos);
			containsNew = holes.Contains(npos);
			if (!holes.Contains(bpos) || !containsNew)
			{
				nBlock = chunk.GetLocalBlockAtBlockPos(Api.World, npos);
				if (!isFirewood && nBlock.BlockId != charcoalPitId)
				{
					if (IsCombustible(npos))
					{
						holes.Add(npos.Copy());
						holes.Add(bpos.Copy());
					}
					else if (nBlock.GetLiquidBarrierHeightOnSide(facing.Opposite, npos) != 1f)
					{
						holes.Add(bpos.Copy());
					}
				}
				else if (containsNew && nBlock.BlockId == charcoalPitId)
				{
					holes.Add(bpos);
				}
			}
			return isFirewood ? true : false;
		}))
		{
			return null;
		}
		return holes;
	}

	protected virtual void UpdateSmokeLocations()
	{
		smokeLocations.Clear();
		if (state == EnumCharcoalPitState.Sealed)
		{
			WalkPit(delegate(BlockPos bpos)
			{
				BlockPos key = bpos.DownCopy(bpos.Y);
				smokeLocations.TryGetValue(key, out var value);
				smokeLocations[key] = Math.Max(value, bpos.Y);
			}, defaultCheckAction);
		}
	}

	protected bool InCube(BlockPos npos, ref BlockPos minPos, ref BlockPos maxPos)
	{
		BlockPos nmin = minPos.Copy();
		BlockPos nmax = maxPos.Copy();
		if (npos.X < nmin.X)
		{
			nmin.X = npos.X;
		}
		else if (npos.X > nmax.X)
		{
			nmax.X = npos.X;
		}
		if (npos.Y < nmin.Y)
		{
			nmin.Y = npos.Y;
		}
		else if (npos.Y > nmax.Y)
		{
			nmax.Y = npos.Y;
		}
		if (npos.Z < nmin.Z)
		{
			nmin.Z = npos.Z;
		}
		else if (npos.Z > nmax.Z)
		{
			nmax.Z = npos.Z;
		}
		if (nmax.X - nmin.X + 1 <= MaxSize && nmax.Y - nmin.Y + 1 <= MaxSize && nmax.Z - nmin.Z + 1 <= MaxSize)
		{
			minPos = nmin.Copy();
			maxPos = nmax.Copy();
			return true;
		}
		return false;
	}

	protected bool IsCombustible(BlockPos pos)
	{
		Block block = Api.World.BlockAccessor.GetBlock(pos);
		CombustibleProperties combustibleProps = block.GetCombustibleProperties(Api.World, null, pos);
		if (combustibleProps != null)
		{
			return combustibleProps.BurnDuration > 0f;
		}
		ICombustible bic = block.GetInterface<ICombustible>(Api.World, pos);
		if (bic != null)
		{
			return bic.GetBurnDuration(Api.World, pos) > 0f;
		}
		return false;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
	{
		EnumCharcoalPitState num = state;
		bool beforeLit = Lit;
		base.FromTreeAttributes(tree, worldForResolving);
		finishedAfterTotalHours = tree.GetDouble("finishedAfterTotalHours");
		startingAfterTotalHours = tree.GetDouble("startingAfterTotalHours");
		state = (EnumCharcoalPitState)tree.GetInt("state");
		startedByPlayerUid = tree.GetString("startedByPlayerUid");
		Lit = tree.GetBool("lit", defaultValue: true);
		if (num != state || beforeLit != Lit)
		{
			ICoreAPI api = Api;
			if (api != null && api.Side == EnumAppSide.Client)
			{
				UpdateSmokeLocations();
			}
		}
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetDouble("finishedAfterTotalHours", finishedAfterTotalHours);
		tree.SetDouble("startingAfterTotalHours", startingAfterTotalHours);
		tree.SetInt("state", (int)state);
		tree.SetBool("lit", Lit);
		if (startedByPlayerUid != null)
		{
			tree.SetString("startedByPlayerUid", startedByPlayerUid);
		}
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		double minutesLeft = 60.0 * (startingAfterTotalHours - Api.World.Calendar.TotalHours);
		if (Lit)
		{
			if (minutesLeft <= 0.0)
			{
				dsc.AppendLine(Lang.Get("Lit."));
				return;
			}
			dsc.AppendLine(Lang.Get("lit-starting", (int)minutesLeft));
		}
		else
		{
			dsc.AppendLine(Lang.Get("Unlit."));
		}
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		if (!Lit)
		{
			MeshData litCharcoalMesh = ObjectCacheUtil.GetOrCreate(Api, "litCharcoalMesh", delegate
			{
				((ICoreClientAPI)Api).Tesselator.TesselateShape(base.Block, Shape.TryGet(Api, "shapes/block/wood/firepit/cold-normal.json"), out var modeldata);
				return modeldata;
			});
			mesher.AddMeshData(litCharcoalMesh);
			return true;
		}
		return false;
	}
}
