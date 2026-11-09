using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ProtoBuf;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Common;

namespace Vintagestory.Server;

[ProtoContract]
public class ServerChunk : WorldChunk, IServerChunk, IWorldChunk, IWithFastSerialize
{
	public static Stopwatch ReadWriteStopWatch;

	[ThreadStatic]
	private static FastMemoryStream reusableSerializationStream;

	[ThreadStatic]
	private static List<Entity> reusableSerializationList;

	[ProtoMember(6)]
	[CustomFastSerializer]
	private List<byte[]> EntitiesSerialized;

	[ProtoMember(7)]
	[CustomFastSerializer]
	public int BlockEntitiesCount;

	[ProtoMember(8)]
	[CustomFastSerializer]
	private List<byte[]> BlockEntitiesSerialized;

	[ProtoMember(9)]
	private Dictionary<string, byte[]> moddata;

	[ProtoMember(10)]
	protected HashSet<int> lightPositions;

	[ProtoMember(11)]
	public Dictionary<string, byte[]> ServerSideModdata;

	[ProtoMember(12)]
	public string GameVersionCreated;

	[ProtoMember(13)]
	public bool EmptyBeforeSave;

	[ProtoMember(14)]
	[CustomFastSerializer]
	public byte[] DecorsSerialized;

	[ProtoMember(15)]
	public int savedCompressionVersion;

	[ProtoMember(17)]
	public int BlocksPlaced;

	[ProtoMember(18)]
	public int BlocksRemoved;

	public ServerMapChunk serverMapChunk;

	public bool DirtyForSaving;

	[ProtoMember(1)]
	internal new byte[] blocksCompressed
	{
		get
		{
			return base.blocksCompressed;
		}
		set
		{
			base.blocksCompressed = value;
		}
	}

	[ProtoMember(2)]
	internal new byte[] lightCompressed
	{
		get
		{
			return base.lightCompressed;
		}
		set
		{
			base.lightCompressed = value;
		}
	}

	[ProtoMember(3)]
	internal new byte[] lightSatCompressed
	{
		get
		{
			return base.lightSatCompressed;
		}
		set
		{
			base.lightSatCompressed = value;
		}
	}

	[ProtoMember(16)]
	internal byte[] liquidsCompressed
	{
		get
		{
			return base.fluidsCompressed;
		}
		set
		{
			base.fluidsCompressed = value;
		}
	}

	[ProtoMember(5)]
	[CustomFastSerializer]
	public new int EntitiesCount
	{
		get
		{
			return base.EntitiesCount;
		}
		set
		{
			base.EntitiesCount = value;
		}
	}

	public override IMapChunk MapChunk => serverMapChunk;

	public bool NotAtEdge
	{
		get
		{
			if (serverMapChunk != null)
			{
				return serverMapChunk.NeighboursLoaded.Value() == 511;
			}
			return false;
		}
	}

	public override Dictionary<string, byte[]> ModData
	{
		get
		{
			return moddata;
		}
		set
		{
			if (value == null)
			{
				throw new NullReferenceException("ModData must not be set to null");
			}
			moddata = value;
		}
	}

	public override HashSet<int> LightPositions
	{
		get
		{
			return lightPositions;
		}
		set
		{
			lightPositions = value;
		}
	}

	string IServerChunk.GameVersionCreated => GameVersionCreated;

	int IServerChunk.BlocksPlaced => BlocksPlaced;

	int IServerChunk.BlocksRemoved => BlocksRemoved;

	static ServerChunk()
	{
		ReadWriteStopWatch = new Stopwatch();
		ReadWriteStopWatch.Start();
	}

	private ServerChunk()
	{
	}

	public static ServerChunk CreateNew(ChunkDataPool datapool)
	{
		ServerChunk serverChunk = new ServerChunk();
		serverChunk.datapool = datapool;
		serverChunk.PotentialBlockOrLightingChanges = true;
		serverChunk.chunkdataVersion = 2;
		serverChunk.chunkdata = datapool.Request();
		serverChunk.GameVersionCreated = "1.22.7";
		serverChunk.lightPositions = new HashSet<int>();
		serverChunk.moddata = new Dictionary<string, byte[]>();
		serverChunk.ServerSideModdata = new Dictionary<string, byte[]>();
		serverChunk.LiveModData = new Dictionary<string, object>();
		serverChunk.MaybeBlocks = datapool.OnlyAirBlocksData;
		serverChunk.MarkModified();
		return serverChunk;
	}

	public void RemoveEntitiesAndBlockEntities(IServerWorldAccessor server)
	{
		Entity[] Entities = base.Entities;
		if (Entities != null)
		{
			EntityDespawnData reason = new EntityDespawnData
			{
				Reason = EnumDespawnReason.Unload
			};
			for (int i = 0; i < Entities.Length; i++)
			{
				Entity entity = Entities[i];
				if (entity == null)
				{
					if (i >= EntitiesCount)
					{
						break;
					}
				}
				else if (!(entity is EntityPlayer))
				{
					server.DespawnEntity(entity, reason);
				}
			}
		}
		foreach (KeyValuePair<BlockPos, BlockEntity> blockEntity in BlockEntities)
		{
			blockEntity.Value.OnBlockUnloaded();
		}
	}

	public void ClearData()
	{
		ChunkData oldData = chunkdata;
		PotentialBlockOrLightingChanges = true;
		chunkdataVersion = 2;
		chunkdata = datapool.Request();
		GameVersionCreated = "1.22.7";
		lightPositions = new HashSet<int>();
		moddata = new Dictionary<string, byte[]>();
		ServerSideModdata = new Dictionary<string, byte[]>();
		base.MaybeBlocks = datapool.OnlyAirBlocksData;
		MarkModified();
		base.Empty = true;
		if (oldData != null)
		{
			datapool.Free(oldData);
		}
	}

	public static ServerChunk FromBytes(byte[] serializedChunk, ChunkDataPool datapool, IWorldAccessor worldForResolve)
	{
		if (datapool == null)
		{
			throw new MissingFieldException("datapool cannot be null");
		}
		ServerChunk chunk;
		using (MemoryStream ms = new MemoryStream(serializedChunk))
		{
			chunk = Serializer.Deserialize<ServerChunk>(ms);
		}
		chunk.GameVersionCreated = chunk.GameVersionCreated.DeDuplicate();
		chunk.chunkdataVersion = chunk.savedCompressionVersion;
		chunk.datapool = datapool;
		if (chunk.blocksCompressed == null || chunk.lightCompressed == null || chunk.lightSatCompressed == null)
		{
			chunk.Unpack_MaybeNullData();
		}
		chunk.AfterDeserialization(worldForResolve);
		if (chunk.lightPositions == null)
		{
			chunk.lightPositions = new HashSet<int>();
		}
		if (chunk.moddata == null)
		{
			chunk.moddata = new Dictionary<string, byte[]>();
		}
		if (chunk.ServerSideModdata == null)
		{
			chunk.ServerSideModdata = new Dictionary<string, byte[]>();
		}
		if (chunk.LiveModData == null)
		{
			chunk.LiveModData = new Dictionary<string, object>();
		}
		chunk.MaybeBlocks = datapool.OnlyAirBlocksData;
		return chunk;
	}

	public byte[] ToBytes()
	{
		using FastMemoryStream ms = new FastMemoryStream();
		return ToBytes(ms);
	}

	public byte[] ToBytes(FastMemoryStream ms)
	{
		lock (packUnpackLock)
		{
			if (!IsPacked())
			{
				Pack();
				blocksCompressed = blocksCompressedTmp;
				lightCompressed = lightCompressedTmp;
				lightSatCompressed = lightSatCompressedTmp;
				liquidsCompressed = fluidsCompressedTmp;
				chunkdataVersion = 2;
			}
			savedCompressionVersion = chunkdataVersion;
			BeforeSerializationCommon();
			if (reusableSerializationStream == null)
			{
				reusableSerializationStream = new FastMemoryStream();
			}
			return ((IWithFastSerialize)this).FastSerialize(ms);
		}
	}

	protected override void UpdateForVersion()
	{
		if (chunkdataVersion < 2)
		{
			chunkdata.UpdateFluids();
		}
		chunkdataVersion = 2;
		DirtyForSaving = true;
		PotentialBlockOrLightingChanges = true;
	}

	public void MarkToPack()
	{
		PotentialBlockOrLightingChanges = true;
	}

	private void BeforeSerializationCommon()
	{
		foreach (KeyValuePair<string, object> var in base.LiveModData)
		{
			SetModdata(var.Key, var.Value);
		}
		EmptyBeforeSave = base.Empty;
	}

	private int GatherEntitiesToSerialize(List<Entity> list)
	{
		int cnt = 0;
		Entity[] Entities = base.Entities;
		for (int i = 0; i < Entities.Length; i++)
		{
			Entity entity = Entities[i];
			if (entity == null)
			{
				if (i >= EntitiesCount)
				{
					break;
				}
			}
			else if (!entity.StoreWithChunk)
			{
				cnt++;
			}
			else
			{
				list.Add(entity);
				cnt++;
			}
		}
		return cnt;
	}

	private void AfterDeserialization(IWorldAccessor worldAccessorForResolve)
	{
		lock (packUnpackLock)
		{
			base.Empty = EmptyBeforeSave;
			if (EntitiesSerialized == null)
			{
				Entities = Array.Empty<Entity>();
				EntitiesCount = 0;
			}
			else
			{
				Entity[] entities = new Entity[EntitiesSerialized.Count];
				Dictionary<string, string> entityRemappings = ((IServerWorldAccessor)worldAccessorForResolve).RemappedEntities;
				int cnt = 0;
				for (int i = 0; i < entities.Length; i++)
				{
					string className = "unknown";
					try
					{
						using MemoryStream ms = new MemoryStream(EntitiesSerialized[i]);
						BinaryReader reader = new BinaryReader(ms);
						className = reader.ReadString();
						Entity entity = ServerMain.ClassRegistry.CreateEntity(className);
						entity.FromBytes(reader, isSync: false, entityRemappings);
						entities[cnt++] = entity;
					}
					catch (Exception ex)
					{
						ServerMain.Logger.Error("Failed loading an entity (type " + className + ") in a chunk. Will discard, sorry. Exception logged to verbose debug.");
						ServerMain.Logger.VerboseDebug("Failed loading an entity in a chunk. Will discard, sorry. Exception: {0}", LoggerBase.CleanStackTrace(ex.ToString()));
					}
				}
				Entities = entities;
				EntitiesCount = cnt;
				EntitiesSerialized = null;
			}
			if (BlockEntitiesSerialized != null)
			{
				foreach (byte[] item in BlockEntitiesSerialized)
				{
					using MemoryStream ms2 = new MemoryStream(item);
					using BinaryReader reader2 = new BinaryReader(ms2);
					string className2;
					try
					{
						className2 = reader2.ReadString();
					}
					catch (Exception)
					{
						ServerMain.Logger.Error("Badly corrupted BlockEntity data in a chunk. Will discard it. Sorry.");
						goto end_IL_0162;
					}
					string blockCode = null;
					try
					{
						TreeAttribute tree = new TreeAttribute();
						tree.FromBytes(reader2);
						BlockEntity entity2 = ServerMain.ClassRegistry.CreateBlockEntity(className2);
						Block block = null;
						blockCode = tree.GetString("blockCode");
						if (blockCode != null)
						{
							block = worldAccessorForResolve.GetBlock(new AssetLocation(blockCode));
						}
						if (block == null)
						{
							block = GetLocalBlockAtBlockPos(worldAccessorForResolve, tree.GetInt("posx"), tree.GetInt("posy"), tree.GetInt("posz"));
							if (block?.Code != null)
							{
								tree.SetString("blockCode", block.Code.ToShortString());
							}
						}
						if (block?.Code == null)
						{
							int posx = tree.GetInt("posx");
							int posy = tree.GetInt("posy");
							int posz = tree.GetInt("posz");
							worldAccessorForResolve.Logger.Notification("Block entity with classname {3} at {0}, {1}, {2} has a block that is null or whose code is null o.O? Won't load this block entity!", posx, posy, posz, className2);
						}
						else
						{
							entity2.CreateBehaviors(block, worldAccessorForResolve);
							entity2.FromTreeAttributes(tree, worldAccessorForResolve);
							BlockEntities[entity2.Pos] = entity2;
						}
					}
					catch (Exception ex3)
					{
						ServerMain.Logger.Error("Failed loading blockentity {0} for block {1} in a chunk. Will discard it. Sorry. Exception logged to verbose debug.", className2, blockCode);
						ServerMain.Logger.VerboseDebug("Failed loading a blockentity in a chunk. Will discard it. Sorry. Exception: {0}", LoggerBase.CleanStackTrace(ex3.ToString()));
					}
					end_IL_0162:;
				}
				BlockEntitiesCount = BlockEntities.Count;
				BlockEntitiesSerialized = null;
			}
			if (DecorsSerialized != null && DecorsSerialized.Length != 0)
			{
				Decors = new Dictionary<int, Block>();
				using (MemoryStream ms3 = new MemoryStream(DecorsSerialized))
				{
					BinaryReader reader3 = new BinaryReader(ms3);
					while (reader3.BaseStream.Position < reader3.BaseStream.Length)
					{
						int index3d = reader3.ReadInt32();
						int blockId = reader3.ReadInt32();
						Block dec = worldAccessorForResolve.GetBlock(blockId);
						Decors.Add(index3d, dec);
					}
				}
				DecorsSerialized = null;
			}
			if (LightPositions == null)
			{
				LightPositions = new HashSet<int>(0);
			}
			if (moddata == null)
			{
				moddata = new Dictionary<string, byte[]>(0);
			}
		}
	}

	public override void AddEntity(Entity entity)
	{
		base.AddEntity(entity);
		MarkModified();
	}

	public override bool RemoveEntity(long entityId)
	{
		bool num = base.RemoveEntity(entityId);
		if (num)
		{
			MarkModified();
		}
		return num;
	}

	internal Packet_ServerChunk ToPacket(int posX, int posY, int posZ, bool withEntities = false)
	{
		Packet_ServerChunk packet = new Packet_ServerChunk
		{
			X = posX,
			Y = posY,
			Z = posZ
		};
		lock (packUnpackLock)
		{
			foreach (KeyValuePair<string, object> var in base.LiveModData)
			{
				SetModdata(var.Key, var.Value);
			}
			if (chunkdata == null && chunkdataVersion < 2)
			{
				Unpack();
			}
			if (chunkdata != null)
			{
				UpdateEmptyFlag();
				if (PotentialBlockOrLightingChanges)
				{
					chunkdataVersion = 2;
					byte[] bcTmp = null;
					byte[] lcTmp = null;
					byte[] lscTmp = null;
					byte[] lqTmp = null;
					chunkdata.CompressInto(ref bcTmp, ref lcTmp, ref lscTmp, ref lqTmp, chunkdataVersion);
					base.blocksCompressed = bcTmp;
					base.lightCompressed = lcTmp;
					base.lightSatCompressed = lscTmp;
					base.fluidsCompressed = lqTmp;
					PotentialBlockOrLightingChanges = false;
				}
				if (Environment.TickCount - lastReadOrWrite > MagicNum.UncompressedChunkTTL)
				{
					datapool.Free(chunkdata);
					base.MaybeBlocks = datapool.OnlyAirBlocksData;
					chunkdata = null;
				}
			}
			packet.Empty = (base.Empty ? 1 : 0);
			packet.SetBlocks(blocksCompressed);
			packet.SetLight(lightCompressed);
			packet.SetLightSat(lightSatCompressed);
			packet.SetLiquids(liquidsCompressed);
			packet.SetCompver(chunkdataVersion);
		}
		packet.SetModdata(SerializerUtil.Serialize(moddata, reusableSerializationStream ?? (reusableSerializationStream = new FastMemoryStream())));
		if (BlockEntities.Count > 0)
		{
			packet.SetBlockEntities(GetBlockEntitiesPackets());
		}
		if (LightPositions != null && LightPositions.Count > 0)
		{
			packet.SetLightPositions(LightPositions.ToArray());
		}
		if (Decors != null && Decors.Count > 0)
		{
			int[] decorsPos = new int[Decors.Count];
			int[] decorsIds = new int[Decors.Count];
			int j = 0;
			foreach (KeyValuePair<int, Block> val in Decors)
			{
				if (j >= decorsPos.Length)
				{
					break;
				}
				decorsPos[j] = val.Key;
				decorsIds[j] = val.Value.BlockId;
				j++;
			}
			packet.SetDecorsPos(decorsPos);
			packet.SetDecorsIds(decorsIds);
		}
		return packet;
	}

	internal Packet_Entity[] GetEntitiesPackets()
	{
		Packet_Entity[] packets = new Packet_Entity[(base.Entities != null) ? EntitiesCount : 0];
		if (packets.Length != 0)
		{
			using FastMemoryStream ms = reusableSerializationStream ?? (reusableSerializationStream = new FastMemoryStream());
			ms.Reset();
			BinaryWriter writer = new BinaryWriter(ms);
			int j = 0;
			Entity[] Entities = base.Entities;
			for (int i = 0; i < Entities.Length; i++)
			{
				Entity entity = Entities[i];
				if (entity == null)
				{
					if (i >= EntitiesCount)
					{
						break;
					}
				}
				else
				{
					packets[j++] = ServerPackets.GetEntityPacket(entity, ms, writer);
				}
			}
			if (j != packets.Length)
			{
				Array.Resize(ref packets, j);
			}
		}
		return packets;
	}

	internal Packet_BlockEntity[] GetBlockEntitiesPackets()
	{
		Packet_BlockEntity[] packets = new Packet_BlockEntity[BlockEntities.Count];
		using FastMemoryStream ms = reusableSerializationStream ?? (reusableSerializationStream = new FastMemoryStream());
		ms.Reset();
		BinaryWriter writer = new BinaryWriter(ms);
		int j = 0;
		Dictionary<Type, string> blockEntityRegistry = ServerMain.ClassRegistry.blockEntityTypeToClassnameMapping;
		foreach (BlockEntity be in BlockEntities.Values)
		{
			if (be != null && blockEntityRegistry.TryGetValue(be.GetType(), out var beClass))
			{
				packets[j++] = ServerPackets.getBlockEntityPacket(be, beClass, ms, writer);
			}
		}
		return packets;
	}

	public override void MarkModified()
	{
		base.MarkModified();
		DirtyForSaving = true;
	}

	public void SetServerModdata(string key, byte[] data)
	{
		ServerSideModdata[key] = data;
	}

	public byte[] GetServerModdata(string key)
	{
		ServerSideModdata.TryGetValue(key, out var data);
		return data;
	}

	public void ClearAll(IServerWorldAccessor worldAccessor)
	{
		RemoveEntitiesAndBlockEntities(worldAccessor);
		ClearData();
		BlockEntities?.Clear();
		Decors?.Clear();
		Entities = null;
	}

	private void FastSerializeEntitiesCount(FastMemoryStream ms, int idCount, ref int count, ref int savedPosition)
	{
		if (Entities != null && Entities.Length != 0)
		{
			savedPosition = (int)ms.Position + 1;
			List<Entity> EntitiesToSend = reusableSerializationList ?? (reusableSerializationList = new List<Entity>(EntitiesCount));
			EntitiesToSend.Clear();
			count = GatherEntitiesToSerialize(EntitiesToSend);
			FastSerializer.Write(ms, idCount, count);
		}
	}

	private void FastSerializeEntities(FastMemoryStream ms, int idEntitySerialized, ref int count, ref int savedPosition)
	{
		if (Entities == null || Entities.Length == 0)
		{
			return;
		}
		List<Entity> EntitiesToSend = reusableSerializationList;
		FastMemoryStream secondBuffer = reusableSerializationStream;
		if (EntitiesToSend.Count > 0)
		{
			BinaryWriter writer = new BinaryWriter(secondBuffer);
			int failedCount = 0;
			foreach (Entity entity in EntitiesToSend)
			{
				try
				{
					secondBuffer.Reset();
					writer.Write(ServerMain.ClassRegistry.GetEntityClassName(entity.GetType()));
					entity.ToBytes(writer, forClient: false);
					FastSerializer.Write(ms, idEntitySerialized, secondBuffer);
				}
				catch (Exception e)
				{
					ServerMain.Logger.Error("Error thrown trying to serialize entity with code {0}, will not save, sorry!", entity?.Code);
					ServerMain.Logger.Error(e);
					failedCount++;
				}
			}
			if (failedCount > 0 && failedCount <= count)
			{
				ms.WriteAt(savedPosition, count - failedCount, FastSerializer.GetSize(count));
			}
		}
		EntitiesToSend.Clear();
	}

	private void FastSerializeBlockEntitiesCount(FastMemoryStream ms, int idCount, ref int count, ref int savedPosition)
	{
		count = BlockEntities.Count;
		if (count != 0)
		{
			savedPosition = (int)ms.Position + 1;
			FastSerializer.Write(ms, idCount, count);
		}
	}

	private void FastSerializeBlockEntities(FastMemoryStream ms, int idEntitySerialized, ref int count, ref int savedPosition)
	{
		FastMemoryStream secondBuffer = reusableSerializationStream;
		BinaryWriter writer = new BinaryWriter(secondBuffer);
		int failedCount = 0;
		foreach (BlockEntity be in BlockEntities.Values)
		{
			try
			{
				secondBuffer.Reset();
				string classsName = ServerMain.ClassRegistry.blockEntityTypeToClassnameMapping[be.GetType()];
				writer.Write(classsName);
				TreeAttribute tree = new TreeAttribute();
				be.ToTreeAttributes(tree);
				tree.ToBytes(writer);
				FastSerializer.Write(ms, idEntitySerialized, secondBuffer);
			}
			catch (Exception e)
			{
				ServerMain.Logger.Error("Error thrown trying to serialize block entity {0} at {1}, will not save, sorry!", be?.GetType(), be?.Pos);
				ServerMain.Logger.Error(e);
				failedCount++;
			}
		}
		if (failedCount > 0 && failedCount <= count)
		{
			ms.WriteAt(savedPosition, count - failedCount, FastSerializer.GetSize(count));
		}
		BlockEntitiesCount = count - failedCount;
	}

	private void FastSerializeDecors(FastMemoryStream ms, int id, ref int count, ref int savedPosition)
	{
		if (Decors == null || Decors.Count == 0)
		{
			return;
		}
		FastSerializer.WriteTagLengthDelim(ms, id, Decors.Count * 8);
		foreach (KeyValuePair<int, Block> val in Decors)
		{
			Block de = val.Value;
			ms.WriteInt32(val.Key);
			ms.WriteInt32(de.BlockId);
		}
	}

	public byte[] FastSerialize(FastMemoryStream ms)
	{
		ms.Reset();
		int count = 0;
		int position = 0;
		FastSerializer.Write(ms, 1, blocksCompressed);
		FastSerializer.Write(ms, 2, lightCompressed);
		FastSerializer.Write(ms, 3, lightSatCompressed);
		FastSerializeEntitiesCount(ms, 5, ref count, ref position);
		FastSerializeEntities(ms, 6, ref count, ref position);
		FastSerializeBlockEntitiesCount(ms, 7, ref count, ref position);
		FastSerializeBlockEntities(ms, 8, ref count, ref position);
		FastSerializer.Write(ms, 9, moddata);
		FastSerializer.Write(ms, 10, lightPositions);
		FastSerializer.Write(ms, 11, ServerSideModdata);
		FastSerializer.Write(ms, 12, GameVersionCreated);
		FastSerializer.Write(ms, 13, EmptyBeforeSave);
		FastSerializeDecors(ms, 14, ref count, ref position);
		FastSerializer.Write(ms, 15, savedCompressionVersion);
		FastSerializer.Write(ms, 16, liquidsCompressed);
		FastSerializer.Write(ms, 17, BlocksPlaced);
		FastSerializer.Write(ms, 18, BlocksRemoved);
		return ms.ToArray();
	}
}
