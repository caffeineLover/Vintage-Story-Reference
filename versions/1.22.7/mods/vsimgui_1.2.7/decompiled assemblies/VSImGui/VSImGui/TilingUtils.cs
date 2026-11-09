using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace VSImGui
{
	// Token: 0x0200000D RID: 13
	public static class TilingUtils
	{
		// Token: 0x06000188 RID: 392 RVA: 0x0000AA62 File Offset: 0x00008C62
		[NullableContext(1)]
		public static void TileWindowWithTexture(ICoreClientAPI api, AssetLocation texturePath, Vector2 textureSize, TilingUtils.TextureRenderLevel layer = TilingUtils.TextureRenderLevel.Window)
		{
			TilingUtils.TileWindowWithTexture(api.Render.GetOrLoadTexture(texturePath), textureSize, layer);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000AA78 File Offset: 0x00008C78
		public static void TileWindowWithTexture(int texture, Vector2 textureSize, TilingUtils.TextureRenderLevel layer = TilingUtils.TextureRenderLevel.Window)
		{
			Vector2 dimensions;
			dimensions..ctor(ImGui.GetWindowWidth(), ImGui.GetWindowHeight());
			ValueTuple<int, float> valueTuple = TilingUtils.CalcTextures(textureSize.X, dimensions.X);
			int widthCount = valueTuple.Item1;
			float widthSize = valueTuple.Item2;
			ValueTuple<int, float> valueTuple2 = TilingUtils.CalcTextures(textureSize.Y, dimensions.Y);
			int heightCount = valueTuple2.Item1;
			float heightSize = valueTuple2.Item2;
			Vector2 offset;
			offset..ctor(0f, 0f);
			for (int x = 0; x < widthCount; x++)
			{
				for (int y = 0; y < heightCount; y++)
				{
					TilingUtils.AddTextureToLayer(texture, layer, offset, 1f, 1f, textureSize);
					offset.Y += textureSize.Y;
				}
				TilingUtils.AddTextureToLayer(texture, layer, offset, 1f, heightSize, textureSize);
				offset.Y = 0f;
				offset.X += textureSize.X;
			}
			for (int y2 = 0; y2 < heightCount; y2++)
			{
				TilingUtils.AddTextureToLayer(texture, layer, offset, widthSize, 1f, textureSize);
				offset.Y += textureSize.Y;
			}
			TilingUtils.AddTextureToLayer(texture, layer, offset, widthSize, heightSize, textureSize);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000AB98 File Offset: 0x00008D98
		private static void AddTextureToLayer(int texture, TilingUtils.TextureRenderLevel layer, Vector2 offset, float width, float height, Vector2 textureSize)
		{
			textureSize.X *= width;
			textureSize.Y *= height;
			switch (layer)
			{
			case TilingUtils.TextureRenderLevel.Background:
				ImGui.GetBackgroundDrawList().AddImage((IntPtr)texture, ImGui.GetWindowPos() + offset, ImGui.GetWindowPos() + offset + textureSize, new Vector2(0f, 0f), new Vector2(width, height));
				return;
			case TilingUtils.TextureRenderLevel.Window:
				ImGui.GetWindowDrawList().AddImage((IntPtr)texture, ImGui.GetWindowPos() + offset, ImGui.GetWindowPos() + offset + textureSize, new Vector2(0f, 0f), new Vector2(width, height));
				return;
			case TilingUtils.TextureRenderLevel.Foreground:
				ImGui.GetForegroundDrawList().AddImage((IntPtr)texture, ImGui.GetWindowPos() + offset, ImGui.GetWindowPos() + offset + textureSize, new Vector2(0f, 0f), new Vector2(width, height));
				return;
			default:
				return;
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000AC9C File Offset: 0x00008E9C
		[return: TupleElementNames(new string[]
		{
			"count",
			"lastSize"
		})]
		private static ValueTuple<int, float> CalcTextures(float textureSize, float windowSize)
		{
			int count = (int)MathF.Floor(windowSize / textureSize);
			float lastSize = (windowSize - (float)count * textureSize) / textureSize;
			return new ValueTuple<int, float>(count, lastSize);
		}

		// Token: 0x0200002F RID: 47
		public enum TextureRenderLevel
		{
			// Token: 0x04000066 RID: 102
			Background,
			// Token: 0x04000067 RID: 103
			Window,
			// Token: 0x04000068 RID: 104
			Foreground
		}
	}
}
