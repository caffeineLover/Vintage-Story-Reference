using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Inputs
{
	// Token: 0x02000070 RID: 112
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DirectionCursorRenderer : IRenderer, IDisposable
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x0001F311 File Offset: 0x0001D511
		// (set) Token: 0x0600054E RID: 1358 RVA: 0x0001F319 File Offset: 0x0001D519
		public bool Show { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x0001F322 File Offset: 0x0001D522
		// (set) Token: 0x06000550 RID: 1360 RVA: 0x0001F32A File Offset: 0x0001D52A
		public int CurrentDirection
		{
			get
			{
				return this._currentDirection;
			}
			set
			{
				this._currentDirection = Math.Clamp(value, 0, 7);
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x0001F33A File Offset: 0x0001D53A
		public float CursorScale
		{
			get
			{
				return this._settings.DirectionsCursorScale;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x0001F347 File Offset: 0x0001D547
		public float Alpha
		{
			get
			{
				return this._settings.DirectionsCursorTransparency;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x0001F354 File Offset: 0x0001D554
		public double RenderOrder
		{
			get
			{
				return 0.98;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x0001F35F File Offset: 0x0001D55F
		public int RenderRange
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001F368 File Offset: 0x0001D568
		public DirectionCursorRenderer(ICoreClientAPI api, Settings settings)
		{
			this._clientApi = api;
			this._settings = settings;
			for (int index = 0; index < 8; index++)
			{
				LoadedTexture cursorTexture = new LoadedTexture(api);
				IRenderAPI render = api.Render;
				string text = "combatoverhaul";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(25, 1);
				defaultInterpolatedStringHandler.AppendLiteral("gui/direction-cursor-");
				defaultInterpolatedStringHandler.AppendFormatted<int>(index);
				defaultInterpolatedStringHandler.AppendLiteral(".png");
				render.GetOrLoadTexture(new AssetLocation(text, defaultInterpolatedStringHandler.ToStringAndClear()), ref cursorTexture);
				this._directionCursorTextures.Add(cursorTexture);
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001F3FC File Offset: 0x0001D5FC
		public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
		{
			if (!this.Show)
			{
				return;
			}
			if (!this._clientApi.Input.MouseGrabbed)
			{
				return;
			}
			LoadedTexture texture = this._directionCursorTextures[this._currentDirection];
			float reticleScale = this.CursorScale * RuntimeEnv.GUIScale * 0.5f;
			this._clientApi.Render.Render2DTexture(texture.TextureId, (float)(this._clientApi.Render.FrameWidth / 2) - (float)texture.Width * reticleScale / 2f, (float)(this._clientApi.Render.FrameHeight / 2) - (float)texture.Height * reticleScale / 2f, (float)texture.Width * reticleScale, (float)texture.Height * reticleScale, 10000f, new Vec4f(1f, 1f, 1f, this.Alpha));
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001F4D8 File Offset: 0x0001D6D8
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			foreach (LoadedTexture loadedTexture in this._directionCursorTextures)
			{
				loadedTexture.Dispose();
			}
			this._directionCursorTextures.Clear();
		}

		// Token: 0x04000233 RID: 563
		public const int NumDirections = 8;

		// Token: 0x04000234 RID: 564
		public const float ScaleMultiplier = 0.5f;

		// Token: 0x04000235 RID: 565
		private readonly List<LoadedTexture> _directionCursorTextures = new List<LoadedTexture>();

		// Token: 0x04000236 RID: 566
		private readonly Settings _settings;

		// Token: 0x04000237 RID: 567
		private readonly ICoreClientAPI _clientApi;

		// Token: 0x04000238 RID: 568
		private int _currentDirection;

		// Token: 0x04000239 RID: 569
		private bool _disposed;
	}
}
