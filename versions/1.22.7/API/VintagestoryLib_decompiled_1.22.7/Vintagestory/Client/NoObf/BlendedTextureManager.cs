using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;

namespace Vintagestory.Client.NoObf;

public class BlendedTextureManager
{
	public const int DirN = 0;

	public const int DirS = 1;

	public const int DirE = 2;

	public const int DirW = 3;

	public const int DirNE = 4;

	public const int DirSE = 5;

	public const int DirSW = 6;

	public const int DirNW = 7;

	private const int DirCount = 8;

	private static readonly string[] DirNames = new string[8] { "N", "S", "E", "W", "NE", "SE", "SW", "NW" };

	private static readonly float[,] OverlayUVOffsets = new float[8, 2]
	{
		{
			1f / 3f,
			2f / 3f
		},
		{
			1f / 3f,
			0f
		},
		{
			0f,
			1f / 3f
		},
		{
			2f / 3f,
			1f / 3f
		},
		{
			0f,
			2f / 3f
		},
		{ 0f, 0f },
		{
			2f / 3f,
			0f
		},
		{
			2f / 3f,
			2f / 3f
		}
	};

	private static readonly int[] RotationToRemapIndex = new int[4] { 2, 1, 0, 3 };

	private static readonly int[][] RotatedUVs = new int[4][]
	{
		new int[8] { 0, 0, 1, 0, 1, 1, 0, 1 },
		new int[8] { 1, 0, 1, 1, 0, 1, 0, 0 },
		new int[8] { 1, 1, 0, 1, 0, 0, 1, 0 },
		new int[8] { 0, 1, 0, 0, 1, 0, 1, 1 }
	};

	private readonly ClientMain game;

	private readonly Dictionary<string, int> cache = new Dictionary<string, int>(StringComparer.Ordinal);

	private readonly HashSet<int> blendedSubIds = new HashSet<int>();

	private MeshRef[] rotatedQuads;

	private bool refreshScheduled;

	private long refreshListenerId;

	public int CacheCount => cache.Count;

	public BlendedTextureManager(ClientMain game)
	{
		this.game = game;
		InitRotatedQuads();
	}

	private void InitRotatedQuads()
	{
		int[] verts = new int[12]
		{
			-1, -1, 0, 1, -1, 0, 1, 1, 0, -1,
			1, 0
		};
		int[] indices = new int[6] { 0, 1, 2, 0, 2, 3 };
		rotatedQuads = new MeshRef[4];
		for (int r = 0; r < 4; r++)
		{
			MeshData m = new MeshData(4, 6, withNormals: false, withUv: true, withRgba: false, withFlags: false);
			for (int i = 0; i < 4; i++)
			{
				m.AddVertex(verts[i * 3], verts[i * 3 + 1], verts[i * 3 + 2], RotatedUVs[r][i * 2], RotatedUVs[r][i * 2 + 1]);
			}
			for (int j = 0; j < 6; j++)
			{
				m.AddIndex(indices[j]);
			}
			rotatedQuads[r] = game.Platform.UploadMesh(m);
		}
	}

	public bool IsBlendedTexture(int textureSubId)
	{
		return blendedSubIds.Contains(textureSubId);
	}

	public bool TryGetCached(string key, out int textureSubId)
	{
		return cache.TryGetValue(key, out textureSubId);
	}

	public void ClearCache()
	{
		cache.Clear();
		blendedSubIds.Clear();
	}

	public int GetOrCreateBlendedTexture(int baseSubId, int[] bleedSubIds, int[] priorities, int[] overlaySubIds, TextureAtlasManager atlasManager, int rotationSteps = 0)
	{
		string key = BuildCacheKey(baseSubId, bleedSubIds, priorities, overlaySubIds, rotationSteps);
		if (cache.TryGetValue(key, out var cached))
		{
			return cached;
		}
		if (!HasAnyOverlayBlend(bleedSubIds, priorities, overlaySubIds))
		{
			return baseSubId;
		}
		TextureAtlasPosition[] positions = atlasManager.TextureAtlasPositionsByTextureSubId;
		if (positions == null || baseSubId < 0 || baseSubId >= positions.Length)
		{
			return baseSubId;
		}
		TextureAtlasPosition basePos = positions[baseSubId];
		if (basePos == null)
		{
			return baseSubId;
		}
		int atlasW = atlasManager.Size.Width;
		int atlasH = atlasManager.Size.Height;
		int texW = (int)((basePos.x2 - basePos.x1) * (float)atlasW);
		int texH = (int)((basePos.y2 - basePos.y1) * (float)atlasH);
		if (texW <= 0 || texH <= 0)
		{
			return baseSubId;
		}
		if (!atlasManager.AllocateTextureSpace(texW, texH, out var newSubId, out var newTexPos))
		{
			return baseSubId;
		}
		newTexPos.AvgColor = basePos.AvgColor;
		newTexPos.RndColors = basePos.RndColors;
		blendedSubIds.Add(newSubId);
		cache[key] = newSubId;
		List<(int dir, int priority)> activeBlends = CollectActiveOverlayBlends(bleedSubIds, priorities, overlaySubIds);
		game.EnqueueMainThreadTask(delegate
		{
			ComposeOnGPU(atlasManager, basePos, newTexPos, activeBlends, overlaySubIds, rotationSteps);
		}, "blendedtexture-compose");
		ScheduleChunkRefresh();
		return newSubId;
	}

	private void ComposeOnGPU(TextureAtlasManager atlasManager, TextureAtlasPosition basePos, TextureAtlasPosition targetPos, List<(int dir, int priority)> activeBlends, int[] overlaySubIds, int rotationSteps)
	{
		if (game.disposed)
		{
			return;
		}
		TextureAtlasPosition[] positions = atlasManager.TextureAtlasPositionsByTextureSubId;
		int atlasW = atlasManager.Size.Width;
		int atlasH = atlasManager.Size.Height;
		float targetPxX = targetPos.x1 * (float)atlasW;
		float targetPxY = targetPos.y1 * (float)atlasH;
		float targetPxW = (targetPos.x2 - targetPos.x1) * (float)atlasW;
		float targetPxH = (targetPos.y2 - targetPos.y1) * (float)atlasH;
		ShaderProgramBase oldprog = ShaderProgramBase.CurrentShaderProgram;
		oldprog?.Stop();
		bool scissorEnabled = game.Platform.GlScissorFlagEnabled;
		if (scissorEnabled)
		{
			game.Platform.GlScissorFlag(enable: false);
		}
		FrameBufferRef fb = TextureAtlasManager.atlasFramebuffer;
		game.Platform.LoadFrameBuffer(fb, targetPos.atlasTextureId);
		game.Platform.GlDisableDepthTest();
		game.Platform.GlToggleBlend(on: false);
		ShaderProgramTexture2texture t2t = ShaderPrograms.Texture2texture;
		t2t.Use();
		t2t.Tex2d2D = basePos.atlasTextureId;
		t2t.Texu = basePos.x1;
		t2t.Texv = basePos.y1;
		t2t.Texw = basePos.x2 - basePos.x1;
		t2t.Texh = basePos.y2 - basePos.y1;
		t2t.AlphaTest = -1f;
		t2t.Xs = targetPxX / (float)fb.Width;
		t2t.Ys = targetPxY / (float)fb.Height;
		t2t.Width = targetPxW / (float)fb.Width;
		t2t.Height = targetPxH / (float)fb.Height;
		game.Platform.RenderMesh(game.quadModel);
		t2t.Stop();
		game.Platform.GlToggleBlend(on: true);
		int uvRot = ((rotationSteps >= 0 && rotationSteps <= 3) ? RotationToRemapIndex[rotationSteps] : 0);
		foreach (var activeBlend in activeBlends)
		{
			int dir = activeBlend.dir;
			int overlayId = GetArrayValue(overlaySubIds, dir, -1);
			if (overlayId >= 0 && overlayId < positions.Length)
			{
				TextureAtlasPosition overlayPos = positions[overlayId];
				if (overlayPos != null)
				{
					float uOff = OverlayUVOffsets[dir, 0];
					float vOff = OverlayUVOffsets[dir, 1];
					float overlayURange = overlayPos.x2 - overlayPos.x1;
					float overlayVRange = overlayPos.y2 - overlayPos.y1;
					float cellU = overlayPos.x1 + uOff * overlayURange;
					float cellV = overlayPos.y1 + vOff * overlayVRange;
					float cellW = 1f / 3f * overlayURange;
					float cellH = 1f / 3f * overlayVRange;
					game.Platform.LoadFrameBuffer(fb, targetPos.atlasTextureId);
					t2t.Use();
					t2t.Tex2d2D = overlayPos.atlasTextureId;
					t2t.Texu = cellU;
					t2t.Texv = cellV;
					t2t.Texw = cellW;
					t2t.Texh = cellH;
					t2t.AlphaTest = 0.001f;
					t2t.Xs = targetPxX / (float)fb.Width;
					t2t.Ys = targetPxY / (float)fb.Height;
					t2t.Width = targetPxW / (float)fb.Width;
					t2t.Height = targetPxH / (float)fb.Height;
					game.Platform.RenderMesh(rotatedQuads[uvRot]);
					t2t.Stop();
				}
			}
		}
		game.Platform.GlEnableDepthTest();
		game.Platform.LoadFrameBuffer((oldprog?.PassName == "gui") ? EnumFrameBuffer.Default : EnumFrameBuffer.Primary);
		if (scissorEnabled)
		{
			game.Platform.GlScissorFlag(enable: true);
		}
		oldprog?.Use();
		atlasManager.RegenMipMaps(targetPos.atlasNumber);
	}

	public static string BuildCacheKey(int baseSubId, int[] bleedSubIds)
	{
		return BuildCacheKey(baseSubId, bleedSubIds, null, null);
	}

	public static string BuildCacheKey(int baseSubId, int[] bleedSubIds, int[] priorities, int[] overlaySubIds, int rotationSteps = 0)
	{
		StringBuilder sb = new StringBuilder(64);
		sb.Append("b:").Append(baseSubId).Append(",r:")
			.Append(rotationSteps);
		if (bleedSubIds == null)
		{
			return sb.ToString();
		}
		for (int dir = 0; dir < 8; dir++)
		{
			int bleedId = GetArrayValue(bleedSubIds, dir, -1);
			int priority = GetArrayValue(priorities, dir, 0);
			if (bleedId >= 0 && priority > 0)
			{
				sb.Append('|').Append(DirNames[dir]).Append(':')
					.Append(bleedId)
					.Append(",p:")
					.Append(priority);
				if (GetArrayValue(overlaySubIds, dir, -1) >= 0)
				{
					sb.Append(",o:").Append(overlaySubIds[dir]);
				}
			}
		}
		return sb.ToString();
	}

	private static bool HasAnyOverlayBlend(int[] bleedSubIds, int[] priorities, int[] overlaySubIds)
	{
		for (int dir = 0; dir < 8; dir++)
		{
			int arrayValue = GetArrayValue(bleedSubIds, dir, -1);
			int priority = GetArrayValue(priorities, dir, 0);
			if (arrayValue >= 0 && priority > 0 && GetArrayValue(overlaySubIds, dir, -1) >= 0)
			{
				return true;
			}
		}
		return false;
	}

	private static List<(int dir, int priority)> CollectActiveOverlayBlends(int[] bleedSubIds, int[] priorities, int[] overlaySubIds)
	{
		List<(int, int)> active = new List<(int, int)>(8);
		for (int dir = 0; dir < 8; dir++)
		{
			int arrayValue = GetArrayValue(bleedSubIds, dir, -1);
			int priority = GetArrayValue(priorities, dir, 0);
			if (arrayValue >= 0 && priority > 0 && GetArrayValue(overlaySubIds, dir, -1) >= 0)
			{
				active.Add((dir, priority));
			}
		}
		if (active.Count > 1)
		{
			active.Sort(((int, int) a, (int, int) b) => (a.Item2 == b.Item2) ? a.Item1.CompareTo(b.Item1) : a.Item2.CompareTo(b.Item2));
		}
		return active;
	}

	private static T GetArrayValue<T>(T[] arr, int index, T defaultValue)
	{
		if (arr == null || index >= arr.Length)
		{
			return defaultValue;
		}
		return arr[index];
	}

	private void ScheduleChunkRefresh()
	{
		if (!refreshScheduled)
		{
			refreshScheduled = true;
			refreshListenerId = game.RegisterGameTickListener(delegate
			{
				RefreshAllChunks();
				game.UnregisterGameTickListener(refreshListenerId);
			}, 50, 2000);
		}
	}

	public void RefreshAllChunks()
	{
		ClientWorldMap map = game.WorldMap;
		if (map?.chunks == null)
		{
			return;
		}
		foreach (long key in map.chunks.Keys)
		{
			int cx = (int)(key % map.index3dMulX);
			int cy = (int)(key / map.index3dMulX % map.index3dMulZ);
			int cz = (int)(key / map.index3dMulX / map.index3dMulZ);
			map.MarkChunkDirty(cx, cy, cz, false, false, null, false);
		}
	}
}
