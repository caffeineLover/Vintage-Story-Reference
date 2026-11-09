using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace Vintagestory.Client.NoObf;

public class SystemUnloadChunks : ClientSystem
{
	public override string Name => "uc";

	public SystemUnloadChunks(ClientMain game)
		: base(game)
	{
		game.PacketHandlers[11] = HandleChunkUnload;
	}

	private void HandleChunkUnload(Packet_Server packet)
	{
		int chunkMapSizeX = game.WorldMap.index3dMulX;
		int chunkMapSizeZ = game.WorldMap.index3dMulZ;
		HashSet<Vec2i> horCoods = new HashSet<Vec2i>();
		int count = packet.UnloadChunk.GetXCount();
		for (int i = 0; i < count; i++)
		{
			int cx = packet.UnloadChunk.X[i];
			int cy = packet.UnloadChunk.Y[i];
			int cz = packet.UnloadChunk.Z[i];
			if (cy < 1024)
			{
				horCoods.Add(new Vec2i(cx, cz));
			}
			long posIndex = MapUtil.Index3dL(cx, cy, cz, chunkMapSizeX, chunkMapSizeZ);
			ClientChunk clientchunk = null;
			lock (game.WorldMap.chunksLock)
			{
				game.WorldMap.chunks.TryGetValue(posIndex, out clientchunk);
			}
			if (clientchunk != null)
			{
				UnloadChunk(clientchunk);
				RuntimeStats.chunksUnloaded++;
			}
		}
		game.Logger.VerboseDebug("Entities and pool locations removed. Removing from chunk dict");
		lock (game.WorldMap.chunksLock)
		{
			for (int j = 0; j < count; j++)
			{
				long posIndex2 = MapUtil.Index3dL(packet.UnloadChunk.X[j], packet.UnloadChunk.Y[j], packet.UnloadChunk.Z[j], chunkMapSizeX, chunkMapSizeZ);
				game.WorldMap.chunks.TryGetValue(posIndex2, out var clientchunk2);
				clientchunk2?.Dispose();
				game.WorldMap.chunks.Remove(posIndex2);
			}
		}
		foreach (Vec2i item in horCoods)
		{
			int cx2 = item.X;
			int cz2 = item.Y;
			bool anyfound = false;
			int cy2 = 0;
			while (!anyfound && cy2 < game.WorldMap.ChunkMapSizeY)
			{
				anyfound |= game.WorldMap.GetChunk(cx2, cy2, cz2) != null;
				cy2++;
			}
			if (!anyfound)
			{
				game.WorldMap.MapChunks.Remove(game.WorldMap.MapChunkIndex2D(cx2, cz2));
			}
		}
		ScreenManager.FrameProfiler.Mark("doneUnlCh");
	}

	private void UnloadChunk(ClientChunk clientchunk)
	{
		if (clientchunk == null)
		{
			return;
		}
		clientchunk.RemoveDataPoolLocations(game.chunkRenderer);
		for (int i = 0; i < clientchunk.EntitiesCount; i++)
		{
			Entity entity = clientchunk.Entities[i];
			if (entity != null && (game.EntityPlayer == null || entity.EntityId != game.EntityPlayer.EntityId))
			{
				EntityDespawnData reason = new EntityDespawnData
				{
					Reason = EnumDespawnReason.Unload
				};
				game.eventManager?.TriggerEntityDespawn(entity, reason);
				entity.OnEntityDespawn(reason);
				game.RemoveEntityRenderer(entity);
				game.LoadedEntities.Remove(entity.EntityId);
			}
		}
		BlockEntity[] array = clientchunk.BlockEntities.Values.ToArray();
		for (int j = 0; j < array.Length; j++)
		{
			array[j].OnBlockUnloaded();
		}
	}

	public override void Dispose(ClientMain game)
	{
		foreach (KeyValuePair<long, ClientChunk> chunk in game.WorldMap.chunks)
		{
			UnloadChunk(chunk.Value);
		}
		game.EntityPlayer?.Properties.Client?.Renderer?.Dispose();
	}

	public override EnumClientSystemType GetSystemType()
	{
		return EnumClientSystemType.Misc;
	}
}
