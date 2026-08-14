using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ProtoBuf;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class ChunkMapLayer : RGBMapLayer
{
	public static Dictionary<EnumBlockMaterial, string> defaultMapColorCodes = new Dictionary<EnumBlockMaterial, string>
	{
		{
			EnumBlockMaterial.Soil,
			"land"
		},
		{
			EnumBlockMaterial.Sand,
			"desert"
		},
		{
			EnumBlockMaterial.Ore,
			"land"
		},
		{
			EnumBlockMaterial.Gravel,
			"desert"
		},
		{
			EnumBlockMaterial.Stone,
			"land"
		},
		{
			EnumBlockMaterial.Leaves,
			"forest"
		},
		{
			EnumBlockMaterial.Plant,
			"plant"
		},
		{
			EnumBlockMaterial.Wood,
			"forest"
		},
		{
			EnumBlockMaterial.Snow,
			"glacier"
		},
		{
			EnumBlockMaterial.Water,
			"lake"
		},
		{
			EnumBlockMaterial.Ice,
			"glacier"
		},
		{
			EnumBlockMaterial.Lava,
			"lava"
		}
	};

	public static Vintagestory.API.Datastructures.OrderedDictionary<string, string> hexColorsByCode = new Vintagestory.API.Datastructures.OrderedDictionary<string, string>
	{
		{ "ink", "#483018" },
		{ "settlement", "#856844" },
		{ "wateredge", "#483018" },
		{ "land", "#AC8858" },
		{ "desert", "#C4A468" },
		{ "forest", "#98844C" },
		{ "road", "#805030" },
		{ "plant", "#808650" },
		{ "lake", "#CCC890" },
		{ "lava", "#CCC890" },
		{ "ocean", "#CCC890" },
		{ "glacier", "#E0E0C0" },
		{ "devastation", "#755c3c" }
	};

	public Vintagestory.API.Datastructures.OrderedDictionary<string, int> colorsByCode = new Vintagestory.API.Datastructures.OrderedDictionary<string, int>();

	private int[] colors;

	public byte[] block2Color;

	private const int chunksize = 32;

	private IWorldChunk[] chunksTmp;

	private object chunksToGenLock = new object();

	private UniqueQueue<FastVec2i> chunksToGen = new UniqueQueue<FastVec2i>();

	private ConcurrentDictionary<FastVec2i, MultiChunkMapComponent> loadedMapData = new ConcurrentDictionary<FastVec2i, MultiChunkMapComponent>(2, 16);

	private HashSet<FastVec2i> curVisibleChunks = new HashSet<FastVec2i>();

	private ConcurrentQueue<ReadyMapPiece> readyMapPieces = new ConcurrentQueue<ReadyMapPiece>();

	private MapDB mapdb;

	private ICoreClientAPI capi;

	private bool colorAccurate;

	private float colorRandomizationWeight = 0.6f;

	private float mtThread1secAccum;

	private float genAccum;

	private float diskSaveAccum;

	private Dictionary<FastVec2i, MapPieceDB> toSaveList = new Dictionary<FastVec2i, MapPieceDB>();

	[ThreadStatic]
	private static byte[] shadowMapReusable;

	[ThreadStatic]
	private static byte[] shadowMapBlurReusable;

	public override MapLegendItem[] LegendItems
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override EnumMinMagFilter MinFilter => EnumMinMagFilter.Linear;

	public override EnumMinMagFilter MagFilter => EnumMinMagFilter.Nearest;

	public override string Title => "Terrain";

	public override EnumMapAppSide DataSide => EnumMapAppSide.Client;

	public override string LayerGroupCode => "terrain";

	public string getMapDbFilePath()
	{
		string text = Path.Combine(GamePaths.DataPath, "Maps");
		GamePaths.EnsurePathExists(text);
		return Path.Combine(text, api.World.SavegameIdentifier + ".db");
	}

	public ChunkMapLayer(ICoreAPI api, IWorldMapManager mapSink)
		: base(api, mapSink)
	{
		foreach (KeyValuePair<string, string> val in hexColorsByCode)
		{
			colorsByCode[val.Key] = ColorUtil.ReverseColorBytes(ColorUtil.Hex2Int(val.Value));
		}
		api.Event.ChunkDirty += Event_OnChunkDirty;
		capi = api as ICoreClientAPI;
		if (api.Side == EnumAppSide.Server)
		{
			(api as ICoreServerAPI).Event.DidPlaceBlock += Event_DidPlaceBlock;
		}
		if (api.Side == EnumAppSide.Client)
		{
			colorAccurate = api.World.Config.GetAsBool("colorAccurateWorldmap") || capi.World.Player.Privileges.IndexOf("colorAccurateWorldmap") != -1;
			colorRandomizationWeight = (float)api.World.Config.GetDecimal("colorRandomizationWeight", 0.6000000238418579);
			api.World.Logger.Notification("Loading world map cache db...");
			mapdb = new MapDB(api.World.Logger);
			string errorMessage = null;
			string mapdbfilepath = getMapDbFilePath();
			mapdb.OpenOrCreate(mapdbfilepath, ref errorMessage, requireWriteAccess: true, corruptionProtection: true, doIntegrityCheck: false);
			if (errorMessage != null)
			{
				throw new Exception($"Cannot open {mapdbfilepath}, possibly corrupted. Please fix manually or delete this file to continue playing");
			}
			api.ChatCommands.GetOrCreate("map").BeginSubCommand("purgedb").WithDescription("purge the map db")
				.HandleWith(delegate
				{
					mapdb.Purge();
					return TextCommandResult.Success("Ok, db purged");
				})
				.EndSubCommand()
				.BeginSubCommand("redraw")
				.WithDescription("Redraw the map")
				.HandleWith(OnMapCmdRedraw)
				.EndSubCommand();
		}
	}

	private TextCommandResult OnMapCmdRedraw(TextCommandCallingArgs args)
	{
		foreach (MultiChunkMapComponent value in loadedMapData.Values)
		{
			value.ActuallyDispose();
		}
		loadedMapData.Clear();
		lock (chunksToGenLock)
		{
			foreach (FastVec2i cord in curVisibleChunks)
			{
				chunksToGen.Enqueue(cord.Copy());
			}
		}
		return TextCommandResult.Success("Redrawing map...");
	}

	private void Event_DidPlaceBlock(IServerPlayer byPlayer, int oldblockId, BlockSelection blockSel, ItemStack withItemStack)
	{
		IMapChunk mapchunk = api.World.BlockAccessor.GetMapChunkAtBlockPos(blockSel.Position);
		if (mapchunk != null)
		{
			int lx = blockSel.Position.X % 32;
			int lz = blockSel.Position.Z % 32;
			int y = mapchunk.RainHeightMap[lz * 32 + lx];
			int ly = y % 32;
			IWorldChunk chunk = api.World.BlockAccessor.GetChunkAtBlockPos(blockSel.Position.X, y, blockSel.Position.Z);
			if (chunk != null && chunk.UnpackAndReadBlock((ly * 32 + lz) * 32 + lx, 3) == 0)
			{
				int cx = blockSel.Position.X / 32;
				int cz = blockSel.Position.Z / 32;
				api.World.Logger.Notification("Huh. Found air block in rain map at chunk pos {0}/{1}. That seems invalid, will regenerate rain map", cx, cz);
				rebuildRainmap(cx, cz);
			}
		}
	}

	private void Event_OnChunkDirty(Vec3i chunkCoord, IWorldChunk chunk, EnumChunkDirtyReason reason)
	{
		lock (chunksToGenLock)
		{
			if (mapSink.IsOpened)
			{
				FastVec2i tmpMccoord = new FastVec2i(chunkCoord.X / 3, chunkCoord.Z / 3);
				FastVec2i tmpCoord = new FastVec2i(chunkCoord.X, chunkCoord.Z);
				if (loadedMapData.ContainsKey(tmpMccoord) || curVisibleChunks.Contains(tmpCoord))
				{
					chunksToGen.Enqueue(new FastVec2i(chunkCoord.X, chunkCoord.Z));
					chunksToGen.Enqueue(new FastVec2i(chunkCoord.X, chunkCoord.Z - 1));
					chunksToGen.Enqueue(new FastVec2i(chunkCoord.X - 1, chunkCoord.Z));
					chunksToGen.Enqueue(new FastVec2i(chunkCoord.X, chunkCoord.Z + 1));
					chunksToGen.Enqueue(new FastVec2i(chunkCoord.X + 1, chunkCoord.Z + 1));
				}
			}
		}
	}

	public override void OnLoaded()
	{
		if (api.Side == EnumAppSide.Server)
		{
			return;
		}
		chunksTmp = new IWorldChunk[api.World.BlockAccessor.MapSizeY / 32];
		colors = new int[colorsByCode.Count];
		for (int i = 0; i < colors.Length; i++)
		{
			colors[i] = colorsByCode.GetValueAtIndex(i);
		}
		IList<Block> blocks = api.World.Blocks;
		block2Color = new byte[blocks.Count];
		for (int j = 0; j < block2Color.Length; j++)
		{
			Block block = blocks[j];
			string colorcode = "land";
			if (block?.Attributes != null)
			{
				colorcode = block.Attributes["mapColorCode"].AsString();
				if (colorcode == null && !defaultMapColorCodes.TryGetValue(block.BlockMaterial, out colorcode))
				{
					colorcode = "land";
				}
			}
			block2Color[j] = (byte)colorsByCode.IndexOfKey(colorcode);
			if (colorsByCode.IndexOfKey(colorcode) < 0)
			{
				throw new Exception("No color exists for color code " + colorcode);
			}
		}
	}

	public override void OnMapOpenedClient()
	{
		colorAccurate = api.World.Config.GetAsBool("colorAccurateWorldmap") || capi.World.Player.Privileges.IndexOf("colorAccurateWorldmap") != -1;
	}

	public override void OnMapClosedClient()
	{
		lock (chunksToGenLock)
		{
			chunksToGen.Clear();
		}
		curVisibleChunks.Clear();
	}

	public override void Dispose()
	{
		if (loadedMapData != null)
		{
			foreach (MultiChunkMapComponent value in loadedMapData.Values)
			{
				value?.ActuallyDispose();
			}
		}
		MultiChunkMapComponent.DisposeStatic();
		base.Dispose();
	}

	public override void OnShutDown()
	{
		MultiChunkMapComponent.tmpTexture?.Dispose();
		mapdb?.Dispose();
	}

	public override void OnOffThreadTick(float dt)
	{
		genAccum += dt;
		if ((double)genAccum < 0.1)
		{
			return;
		}
		genAccum = 0f;
		int quantityToGen = chunksToGen.Count;
		while (quantityToGen > 0 && !mapSink.IsShuttingDown)
		{
			quantityToGen--;
			FastVec2i cord;
			lock (chunksToGenLock)
			{
				if (chunksToGen.Count == 0)
				{
					break;
				}
				cord = chunksToGen.Dequeue();
				goto IL_0091;
			}
			IL_0091:
			if (!api.World.BlockAccessor.IsValidPos(cord.X * 32, 1, cord.Y * 32))
			{
				continue;
			}
			IMapChunk mc = api.World.BlockAccessor.GetMapChunk(cord.X, cord.Y);
			if (mc == null)
			{
				try
				{
					MapPieceDB piece = mapdb.GetMapPiece(cord);
					if (piece?.Pixels != null)
					{
						loadFromChunkPixels(cord, piece.Pixels);
					}
				}
				catch (ProtoException)
				{
					api.Logger.Warning("Failed loading map db section {0}/{1}, a protobuf exception was thrown. Will ignore.", cord.X, cord.Y);
				}
				catch (OverflowException)
				{
					api.Logger.Warning("Failed loading map db section {0}/{1}, a overflow exception was thrown. Will ignore.", cord.X, cord.Y);
				}
				continue;
			}
			int[] tintedPixels = GenerateChunkImage(cord, mc, colorAccurate);
			if (tintedPixels == null)
			{
				lock (chunksToGenLock)
				{
					chunksToGen.Enqueue(cord);
				}
			}
			else
			{
				toSaveList[cord.Copy()] = new MapPieceDB
				{
					Pixels = tintedPixels
				};
				loadFromChunkPixels(cord, tintedPixels);
			}
		}
		if (toSaveList.Count > 100 || diskSaveAccum > 4f)
		{
			diskSaveAccum = 0f;
			mapdb.SetMapPieces(toSaveList);
			toSaveList.Clear();
		}
	}

	public override void OnTick(float dt)
	{
		if (!readyMapPieces.IsEmpty)
		{
			int q = Math.Min(readyMapPieces.Count, 200);
			List<MultiChunkMapComponent> modified = new List<MultiChunkMapComponent>();
			while (q-- > 0)
			{
				if (readyMapPieces.TryDequeue(out var mappiece))
				{
					FastVec2i mcord = new FastVec2i(mappiece.Cord.X / 3, mappiece.Cord.Y / 3);
					FastVec2i baseCord = new FastVec2i(mcord.X * 3, mcord.Y * 3);
					MultiChunkMapComponent mccomp = loadedMapData.GetOrAdd(mcord, (FastVec2i fastVec2i) => new MultiChunkMapComponent(api as ICoreClientAPI, baseCord));
					mccomp.setChunk(mappiece.Cord.X - baseCord.X, mappiece.Cord.Y - baseCord.Y, mappiece.Pixels);
					modified.Add(mccomp);
				}
			}
			foreach (MultiChunkMapComponent item in modified)
			{
				item.FinishSetChunks();
			}
		}
		mtThread1secAccum += dt;
		if (!(mtThread1secAccum > 1f))
		{
			return;
		}
		List<FastVec2i> toRemove = new List<FastVec2i>();
		foreach (KeyValuePair<FastVec2i, MultiChunkMapComponent> val in loadedMapData)
		{
			MultiChunkMapComponent mcmp = val.Value;
			if (!mcmp.AnyChunkSet || !mcmp.IsVisible(curVisibleChunks))
			{
				mcmp.TTL -= 1f;
				if (mcmp.TTL <= 0f)
				{
					FastVec2i mccord = val.Key;
					toRemove.Add(mccord);
					mcmp.ActuallyDispose();
				}
			}
			else
			{
				mcmp.TTL = MultiChunkMapComponent.MaxTTL;
			}
		}
		foreach (FastVec2i val2 in toRemove)
		{
			loadedMapData.TryRemove(val2, out var _);
		}
		mtThread1secAccum = 0f;
	}

	public override void Render(GuiElementMap mapElem, float dt)
	{
		if (!base.Active)
		{
			return;
		}
		foreach (KeyValuePair<FastVec2i, MultiChunkMapComponent> loadedMapDatum in loadedMapData)
		{
			loadedMapDatum.Value.Render(mapElem, dt);
		}
	}

	public override void OnMouseMoveClient(MouseEvent args, GuiElementMap mapElem, StringBuilder hoverText)
	{
		if (!base.Active)
		{
			return;
		}
		foreach (KeyValuePair<FastVec2i, MultiChunkMapComponent> loadedMapDatum in loadedMapData)
		{
			loadedMapDatum.Value.OnMouseMove(args, mapElem, hoverText);
		}
	}

	public override void OnMouseUpClient(MouseEvent args, GuiElementMap mapElem)
	{
		if (!base.Active)
		{
			return;
		}
		foreach (KeyValuePair<FastVec2i, MultiChunkMapComponent> loadedMapDatum in loadedMapData)
		{
			loadedMapDatum.Value.OnMouseUpOnElement(args, mapElem);
		}
	}

	private void loadFromChunkPixels(FastVec2i cord, int[] pixels)
	{
		readyMapPieces.Enqueue(new ReadyMapPiece
		{
			Pixels = pixels,
			Cord = cord
		});
	}

	public override void OnViewChangedClient(List<FastVec2i> nowVisible, List<FastVec2i> nowHidden)
	{
		foreach (FastVec2i val in nowVisible)
		{
			curVisibleChunks.Add(val);
		}
		foreach (FastVec2i val2 in nowHidden)
		{
			curVisibleChunks.Remove(val2);
		}
		lock (chunksToGenLock)
		{
			foreach (FastVec2i cord in nowVisible)
			{
				FastVec2i tmpMccoord = new FastVec2i(cord.X / 3, cord.Y / 3);
				int dx = cord.X % 3;
				int dz = cord.Y % 3;
				if (dx >= 0 && dz >= 0 && (!loadedMapData.TryGetValue(tmpMccoord, out var mcomp) || !mcomp.IsChunkSet(dx, dz)))
				{
					chunksToGen.Enqueue(cord.Copy());
				}
			}
		}
		foreach (FastVec2i cord2 in nowHidden)
		{
			if (cord2.X >= 0 && cord2.Y >= 0)
			{
				FastVec2i mcord = new FastVec2i(cord2.X / 3, cord2.Y / 3);
				if (loadedMapData.TryGetValue(mcord, out var mc))
				{
					mc.unsetChunk(cord2.X % 3, cord2.Y % 3);
				}
			}
		}
	}

	private static bool isLake(Block block)
	{
		if (block.BlockMaterial != EnumBlockMaterial.Water)
		{
			if (block.BlockMaterial == EnumBlockMaterial.Ice)
			{
				return block.Code.Path != "glacierice";
			}
			return false;
		}
		return true;
	}

	public int[] GenerateChunkImage(FastVec2i chunkPos, IMapChunk mc, bool colorAccurate = false)
	{
		BlockPos tmpPos = new BlockPos(0);
		Vec2i localpos = new Vec2i();
		for (int cy = 0; cy < chunksTmp.Length; cy++)
		{
			chunksTmp[cy] = capi.World.BlockAccessor.GetChunk(chunkPos.X, cy, chunkPos.Y);
			if (chunksTmp[cy] == null || !(chunksTmp[cy] as IClientChunk).LoadedFromServer)
			{
				return null;
			}
		}
		int[] tintedImage = new int[1024];
		IMapChunk chunkNeibNW = capi.World.BlockAccessor.GetMapChunk(chunkPos.X - 1, chunkPos.Y - 1);
		IMapChunk chunkNeibW = capi.World.BlockAccessor.GetMapChunk(chunkPos.X - 1, chunkPos.Y);
		IMapChunk chunkNeibN = capi.World.BlockAccessor.GetMapChunk(chunkPos.X, chunkPos.Y - 1);
		if (shadowMapReusable == null)
		{
			shadowMapReusable = new byte[tintedImage.Length];
		}
		byte[] shadowMap = shadowMapReusable;
		for (int i = 0; i < shadowMap.Length; i += 4)
		{
			shadowMap[i] = 128;
			shadowMap[i + 1] = 128;
			shadowMap[i + 2] = 128;
			shadowMap[i + 3] = 128;
		}
		for (int j = 0; j < tintedImage.Length; j++)
		{
			int y = mc.RainHeightMap[j];
			int cy2 = y / 32;
			if (cy2 >= chunksTmp.Length)
			{
				continue;
			}
			MapUtil.PosInt2d(j, 32L, localpos);
			int lx = localpos.X;
			int lz = localpos.Y;
			float b = 1f;
			IMapChunk leftTopMapChunk = mc;
			IMapChunk rightTopMapChunk = mc;
			IMapChunk leftBotMapChunk = mc;
			int topX = lx - 1;
			int botX = lx;
			int leftZ = lz - 1;
			int rightZ = lz;
			if (topX < 0 && leftZ < 0)
			{
				leftTopMapChunk = chunkNeibNW;
				rightTopMapChunk = chunkNeibW;
				leftBotMapChunk = chunkNeibN;
			}
			else
			{
				if (topX < 0)
				{
					leftTopMapChunk = chunkNeibW;
					rightTopMapChunk = chunkNeibW;
				}
				if (leftZ < 0)
				{
					leftTopMapChunk = chunkNeibN;
					leftBotMapChunk = chunkNeibN;
				}
			}
			topX = GameMath.Mod(topX, 32);
			leftZ = GameMath.Mod(leftZ, 32);
			int value = ((leftTopMapChunk != null) ? (y - leftTopMapChunk.RainHeightMap[leftZ * 32 + topX]) : 0);
			int rightTop = ((rightTopMapChunk != null) ? (y - rightTopMapChunk.RainHeightMap[rightZ * 32 + topX]) : 0);
			int leftBot = ((leftBotMapChunk != null) ? (y - leftBotMapChunk.RainHeightMap[leftZ * 32 + botX]) : 0);
			float slopedir = Math.Sign(value) + Math.Sign(rightTop) + Math.Sign(leftBot);
			float steepness = Math.Max(Math.Max(Math.Abs(value), Math.Abs(rightTop)), Math.Abs(leftBot));
			int blockId = chunksTmp[cy2].UnpackAndReadBlock(MapUtil.Index3d(lx, y % 32, lz, 32, 32), 3);
			Block block = api.World.Blocks[blockId];
			if (slopedir > 0f)
			{
				b = 1.08f + Math.Min(0.5f, steepness / 10f) / 1.25f;
			}
			if (slopedir < 0f)
			{
				b = 0.92f - Math.Min(0.5f, steepness / 10f) / 1.25f;
			}
			if (block.BlockMaterial == EnumBlockMaterial.Snow && !colorAccurate)
			{
				y--;
				cy2 = y / 32;
				blockId = chunksTmp[cy2].UnpackAndReadBlock(MapUtil.Index3d(localpos.X, y % 32, localpos.Y, 32, 32), 3);
				block = api.World.Blocks[blockId];
			}
			tmpPos.Set(32 * chunkPos.X + localpos.X, y, 32 * chunkPos.Y + localpos.Y);
			if (colorAccurate)
			{
				int color = block.GetColor(capi, tmpPos);
				int rndCol = block.GetRandomColor(capi, tmpPos, BlockFacing.UP, GameMath.MurmurHash3Mod(tmpPos.X, tmpPos.Y, tmpPos.Z, 30));
				rndCol = ((rndCol & 0xFF) << 16) | (((rndCol >> 8) & 0xFF) << 8) | ((rndCol >> 16) & 0xFF);
				int col = ColorUtil.ColorOverlay(color, rndCol, colorRandomizationWeight);
				tintedImage[j] = col;
				shadowMap[j] = (byte)((float)(int)shadowMap[j] * b);
			}
			else if (isLake(block))
			{
				IWorldChunk lChunk = chunksTmp[cy2];
				IWorldChunk rChunk = lChunk;
				IWorldChunk tChunk = lChunk;
				IWorldChunk bChunk = lChunk;
				int leftX = localpos.X - 1;
				int rightX = localpos.X + 1;
				int topY = localpos.Y - 1;
				int bottomY = localpos.Y + 1;
				if (leftX < 0)
				{
					lChunk = capi.World.BlockAccessor.GetChunk(chunkPos.X - 1, cy2, chunkPos.Y);
				}
				if (rightX >= 32)
				{
					rChunk = capi.World.BlockAccessor.GetChunk(chunkPos.X + 1, cy2, chunkPos.Y);
				}
				if (topY < 0)
				{
					tChunk = capi.World.BlockAccessor.GetChunk(chunkPos.X, cy2, chunkPos.Y - 1);
				}
				if (bottomY >= 32)
				{
					bChunk = capi.World.BlockAccessor.GetChunk(chunkPos.X, cy2, chunkPos.Y + 1);
				}
				if (lChunk != null && rChunk != null && tChunk != null && bChunk != null)
				{
					leftX = GameMath.Mod(leftX, 32);
					rightX = GameMath.Mod(rightX, 32);
					topY = GameMath.Mod(topY, 32);
					bottomY = GameMath.Mod(bottomY, 32);
					Block block2 = api.World.Blocks[lChunk.UnpackAndReadBlock(MapUtil.Index3d(leftX, y % 32, localpos.Y, 32, 32), 3)];
					Block rBlock = api.World.Blocks[rChunk.UnpackAndReadBlock(MapUtil.Index3d(rightX, y % 32, localpos.Y, 32, 32), 3)];
					Block tBlock = api.World.Blocks[tChunk.UnpackAndReadBlock(MapUtil.Index3d(localpos.X, y % 32, topY, 32, 32), 3)];
					Block bBlock = api.World.Blocks[bChunk.UnpackAndReadBlock(MapUtil.Index3d(localpos.X, y % 32, bottomY, 32, 32), 3)];
					if (isLake(block2) && isLake(rBlock) && isLake(tBlock) && isLake(bBlock))
					{
						tintedImage[j] = getColor(block, localpos.X, y, localpos.Y);
					}
					else
					{
						tintedImage[j] = colorsByCode["wateredge"];
					}
				}
				else
				{
					tintedImage[j] = getColor(block, localpos.X, y, localpos.Y);
				}
			}
			else
			{
				shadowMap[j] = (byte)((float)(int)shadowMap[j] * b);
				tintedImage[j] = getColor(block, localpos.X, y, localpos.Y);
			}
		}
		if (shadowMapBlurReusable == null)
		{
			shadowMapBlurReusable = new byte[shadowMap.Length];
		}
		byte[] bla = shadowMapBlurReusable;
		for (int k = 0; k < bla.Length; k += 4)
		{
			bla[k] = shadowMap[k];
			bla[k + 1] = shadowMap[k + 1];
			bla[k + 2] = shadowMap[k + 2];
			bla[k + 3] = shadowMap[k + 3];
		}
		BlurTool.Blur(shadowMap, 32, 32, 2);
		float sharpen = 1f;
		for (int l = 0; l < shadowMap.Length; l++)
		{
			float b2 = (float)(int)(((float)(int)shadowMap[l] / 128f - 1f) * 5f) / 5f;
			b2 += ((float)(int)bla[l] / 128f - 1f) * 5f % 1f / 5f;
			tintedImage[l] = ColorUtil.ColorMultiply3Clamped(tintedImage[l], b2 * sharpen + 1f) | -16777216;
		}
		for (int m = 0; m < chunksTmp.Length; m++)
		{
			chunksTmp[m] = null;
		}
		return tintedImage;
	}

	private int getColor(Block block, int x, int y1, int y2)
	{
		byte colorIndex = block2Color[block.Id];
		return colors[colorIndex];
	}

	private void rebuildRainmap(int cx, int cz)
	{
		ICoreServerAPI sapi = api as ICoreServerAPI;
		int ymax = sapi.WorldManager.MapSizeY / sapi.WorldManager.ChunkSize;
		IServerChunk[] column = new IServerChunk[ymax];
		int chunksize = sapi.WorldManager.ChunkSize;
		IMapChunk mapchunk = null;
		for (int cy = 0; cy < ymax; cy++)
		{
			column[cy] = sapi.WorldManager.GetChunk(cx, cy, cz);
			column[cy]?.Unpack_ReadOnly();
			mapchunk = column[cy]?.MapChunk;
		}
		if (mapchunk == null)
		{
			return;
		}
		for (int dx = 0; dx < chunksize; dx++)
		{
			for (int dz = 0; dz < chunksize; dz++)
			{
				for (int dy = sapi.WorldManager.MapSizeY - 1; dy >= 0; dy--)
				{
					IServerChunk chunk = column[dy / chunksize];
					if (chunk != null)
					{
						int index = (dy % chunksize * chunksize + dz) * chunksize + dx;
						if (!sapi.World.Blocks[chunk.Data.GetBlockId(index, 3)].RainPermeable || dy == 0)
						{
							mapchunk.RainHeightMap[dz * chunksize + dx] = (ushort)dy;
							break;
						}
					}
				}
			}
		}
		sapi.WorldManager.ResendMapChunk(cx, cz, onlyIfInRange: true);
		mapchunk.MarkDirty();
	}
}
