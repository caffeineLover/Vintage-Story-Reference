using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Util;
using Vintagestory.Common;
using Vintagestory.Common.Database;

namespace Vintagestory.Client.NoObf;

public class ClientSystemRelight : ClientSystem
{
	internal ChunkIlluminator chunkIlluminator;

	public override string Name => "relight";

	public ClientSystemRelight(ClientMain game)
		: base(game)
	{
		chunkIlluminator = new ChunkIlluminator(game.WorldMap, new BlockAccessorRelaxed(game.WorldMap, game, synchronize: false, relight: false), game.WorldMap.ClientChunkSize);
	}

	public override void OnBlockTexturesLoaded()
	{
		chunkIlluminator.InitForWorld(game.Blocks, (ushort)game.WorldMap.SunBrightness, game.WorldMap.MapSizeX, game.WorldMap.MapSizeY, game.WorldMap.MapSizeZ);
	}

	public override int SeperateThreadTickIntervalMs()
	{
		return 10;
	}

	public override void OnSeperateThreadGameTick(float dt)
	{
		ProcessLightingQueue();
	}

	public void ProcessLightingQueue()
	{
		EntityPos playerPos = game.player?.Entity?.Pos;
		while (game.WorldMap.LightingTasks.Count > 0)
		{
			UpdateLightingTask task = null;
			lock (game.WorldMap.LightingTasksLock)
			{
				task = game.WorldMap.LightingTasks.Dequeue();
			}
			if (task == null)
			{
				break;
			}
			ProcessLightingTask(playerPos, task);
		}
	}

	private void ProcessLightingTask(EntityPos playerPos, UpdateLightingTask task)
	{
		int posX = task.pos.X;
		int posY = task.pos.InternalY;
		int posZ = task.pos.Z;
		bool isPriorityRelight = playerPos != null && playerPos.SquareDistanceTo(posX, posY, posZ) < 2304f;
		int oldLightAbsorb = 0;
		int newLightAbsorb = 0;
		bool changedLightSource = false;
		HashSet<long> chunksToRedraw = new HashSet<long>();
		chunksToRedraw.Add(chunkIlluminator.GetChunkIndexForPos(posX, posY, posZ));
		if (task.absorbUpdate)
		{
			oldLightAbsorb = task.oldAbsorb;
			newLightAbsorb = task.newAbsorb;
		}
		else if (task.removeLightHsv != null)
		{
			changedLightSource = true;
			chunksToRedraw.AddRange(chunkIlluminator.RemoveBlockLight(task.removeLightHsv, posX, posY, posZ));
		}
		else
		{
			Block block = game.Blocks[task.oldBlockId];
			Block newblock = game.Blocks[task.newBlockId];
			byte[] oldLightHsv = block.GetLightHsv(game.BlockAccessor, task.pos);
			byte[] newLightHsv = newblock.GetLightHsv(game.BlockAccessor, task.pos);
			if (oldLightHsv[2] > 0)
			{
				changedLightSource = true;
				chunksToRedraw.AddRange(chunkIlluminator.RemoveBlockLight(oldLightHsv, posX, posY, posZ));
			}
			if (newLightHsv[2] > 0)
			{
				changedLightSource = true;
				chunksToRedraw.AddRange(chunkIlluminator.PlaceBlockLight(newLightHsv, posX, posY, posZ));
			}
			oldLightAbsorb = block.GetLightAbsorption(game.BlockAccessor, task.pos);
			newLightAbsorb = newblock.GetLightAbsorption(game.BlockAccessor, task.pos);
			if (oldLightHsv[2] == 0 && newLightHsv[2] == 0 && oldLightAbsorb != newLightAbsorb)
			{
				chunksToRedraw.AddRange(chunkIlluminator.UpdateBlockLight(oldLightAbsorb, newLightAbsorb, posX, posY, posZ));
			}
		}
		bool requireRelight = oldLightAbsorb != newLightAbsorb;
		if (requireRelight)
		{
			chunksToRedraw.AddRange(chunkIlluminator.UpdateSunLight(posX, posY, posZ, oldLightAbsorb, newLightAbsorb));
		}
		foreach (long neibindex3d in chunksToRedraw)
		{
			game.WorldMap.SetChunkDirty(neibindex3d, isPriorityRelight);
		}
		if (!(requireRelight || changedLightSource))
		{
			return;
		}
		long baseindex3d = game.WorldMap.ChunkIndex3D(new ChunkPos(task.pos));
		if (!chunksToRedraw.Contains(baseindex3d))
		{
			game.WorldMap.SetChunkDirty(baseindex3d, isPriorityRelight);
		}
		for (int x = -1; x < 2; x++)
		{
			for (int y = -1; y < 2; y++)
			{
				for (int z = -1; z < 2; z++)
				{
					if (z != 0 || y != 0 || x != 0)
					{
						long neibindex3d2 = game.WorldMap.ChunkIndex3D(new ChunkPos((posX + x) / 32, (posY + y) / 32, (posZ + z) / 32));
						if (neibindex3d2 != baseindex3d && !chunksToRedraw.Contains(neibindex3d2))
						{
							game.WorldMap.SetChunkDirty(neibindex3d2, isPriorityRelight, relight: false, edgeOnly: true);
						}
					}
				}
			}
		}
	}

	public override EnumClientSystemType GetSystemType()
	{
		return EnumClientSystemType.Misc;
	}
}
