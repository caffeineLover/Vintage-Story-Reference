using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000ED RID: 237
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ReticleRenderer : IRenderer, IDisposable
	{
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000C15 RID: 3093 RVA: 0x0003D128 File Offset: 0x0003B328
		// (set) Token: 0x06000C16 RID: 3094 RVA: 0x0003D130 File Offset: 0x0003B330
		public WeaponAimingState AimingState { get; set; }

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x0003D139 File Offset: 0x0003B339
		// (set) Token: 0x06000C18 RID: 3096 RVA: 0x0003D141 File Offset: 0x0003B341
		public bool ReticleScaling { get; set; }

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000C19 RID: 3097 RVA: 0x0003D14A File Offset: 0x0003B34A
		// (set) Token: 0x06000C1A RID: 3098 RVA: 0x0003D152 File Offset: 0x0003B352
		public bool ThrowCircle { get; set; }

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x0003D15B File Offset: 0x0003B35B
		// (set) Token: 0x06000C1C RID: 3100 RVA: 0x0003D163 File Offset: 0x0003B363
		public bool DebugRender { get; set; }

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000C1D RID: 3101 RVA: 0x0003D16C File Offset: 0x0003B36C
		// (set) Token: 0x06000C1E RID: 3102 RVA: 0x0003D174 File Offset: 0x0003B374
		public Vector2 AimingPoint { get; set; }

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x0003D17D File Offset: 0x0003B37D
		public double RenderOrder
		{
			get
			{
				return 0.98;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x0003D188 File Offset: 0x0003B388
		public int RenderRange
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0003D190 File Offset: 0x0003B390
		public ReticleRenderer(ICoreClientAPI api)
		{
			this._clientApi = api;
			LoadedTexture blockedReticle = new LoadedTexture(api);
			LoadedTexture partChargeReticle = new LoadedTexture(api);
			LoadedTexture fullChargeReticle = new LoadedTexture(api);
			this._aimTextureThrowCircle = new LoadedTexture(api);
			api.Render.GetOrLoadTexture(new AssetLocation("combatoverhaul", "gui/aiming/default-blocked.png"), ref blockedReticle);
			api.Render.GetOrLoadTexture(new AssetLocation("combatoverhaul", "gui/aiming/default-part.png"), ref partChargeReticle);
			api.Render.GetOrLoadTexture(new AssetLocation("combatoverhaul", "gui/aiming/default-full.png"), ref fullChargeReticle);
			api.Render.GetOrLoadTexture(new AssetLocation("combatoverhaul", "gui/aiming/throw-circle.png"), ref this._aimTextureThrowCircle);
			this._defaultTextures[WeaponAimingState.Blocked] = blockedReticle;
			this._defaultTextures[WeaponAimingState.PartCharge] = partChargeReticle;
			this._defaultTextures[WeaponAimingState.FullCharge] = fullChargeReticle;
			this._currentTextures[WeaponAimingState.Blocked] = blockedReticle;
			this._currentTextures[WeaponAimingState.PartCharge] = partChargeReticle;
			this._currentTextures[WeaponAimingState.FullCharge] = fullChargeReticle;
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0003D2B0 File Offset: 0x0003B4B0
		public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
		{
			if (this.AimingState == WeaponAimingState.None)
			{
				return;
			}
			LoadedTexture texture = this._currentTextures[this.AimingState];
			float reticleScale = this.ReticleScaling ? RuntimeEnv.GUIScale : 1f;
			this._clientApi.Render.Render2DTexture(texture.TextureId, (float)(this._clientApi.Render.FrameWidth / 2) - (float)texture.Width * reticleScale / 2f + this.AimingPoint.X, (float)(this._clientApi.Render.FrameHeight / 2) - (float)texture.Height * reticleScale / 2f + this.AimingPoint.Y, (float)texture.Width * reticleScale, (float)texture.Height * reticleScale, 10000f, null);
			if (this.ThrowCircle)
			{
				this._clientApi.Render.Render2DTexture(this._aimTextureThrowCircle.TextureId, (float)(this._clientApi.Render.FrameWidth / 2 - this._aimTextureThrowCircle.Width / 2), (float)(this._clientApi.Render.FrameHeight / 2 - this._aimTextureThrowCircle.Height / 2), (float)this._aimTextureThrowCircle.Width, (float)this._aimTextureThrowCircle.Height, 10001f, null);
			}
			if (this.DebugRender)
			{
				LoadedTexture debugReticle = this._currentTextures[WeaponAimingState.FullCharge];
				this._clientApi.Render.Render2DTexture(debugReticle.TextureId, (float)(this._clientApi.Render.FrameWidth / 2 - debugReticle.Width / 2) + this.AimingPoint.X, (float)(this._clientApi.Render.FrameHeight / 2 - debugReticle.Height / 2) + this.AimingPoint.Y, (float)debugReticle.Width, (float)debugReticle.Height, 10000f, null);
			}
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x0003D490 File Offset: 0x0003B690
		public void SetReticleTextures(string partCharge = "", string fullCharge = "", string blocked = "")
		{
			this._currentTextures[WeaponAimingState.Blocked] = ((blocked != "") ? this.GetTexture(blocked) : this._defaultTextures[WeaponAimingState.Blocked]);
			this._currentTextures[WeaponAimingState.PartCharge] = ((partCharge != "") ? this.GetTexture(partCharge) : this._defaultTextures[WeaponAimingState.PartCharge]);
			this._currentTextures[WeaponAimingState.FullCharge] = ((fullCharge != "") ? this.GetTexture(fullCharge) : this._defaultTextures[WeaponAimingState.FullCharge]);
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x0003D528 File Offset: 0x0003B728
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			foreach (LoadedTexture loadedTexture in this._defaultTextures.Values)
			{
				loadedTexture.Dispose();
			}
			foreach (LoadedTexture loadedTexture2 in this._loadedTextures.Values)
			{
				loadedTexture2.Dispose();
			}
			this._aimTextureThrowCircle.Dispose();
			this._defaultTextures.Clear();
			this._currentTextures.Clear();
			this._loadedTextures.Clear();
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x0003D600 File Offset: 0x0003B800
		private LoadedTexture GetTexture(string path)
		{
			if (this._loadedTextures.ContainsKey(path))
			{
				return this._loadedTextures[path];
			}
			LoadedTexture texture = new LoadedTexture(this._clientApi);
			this._loadedTextures[path] = texture;
			this._clientApi.Render.GetOrLoadTexture(new AssetLocation(path), ref texture);
			return texture;
		}

		// Token: 0x040005CF RID: 1487
		private readonly Dictionary<WeaponAimingState, LoadedTexture> _defaultTextures = new Dictionary<WeaponAimingState, LoadedTexture>();

		// Token: 0x040005D0 RID: 1488
		private readonly Dictionary<WeaponAimingState, LoadedTexture> _currentTextures = new Dictionary<WeaponAimingState, LoadedTexture>();

		// Token: 0x040005D1 RID: 1489
		private readonly Dictionary<string, LoadedTexture> _loadedTextures = new Dictionary<string, LoadedTexture>();

		// Token: 0x040005D2 RID: 1490
		private readonly LoadedTexture _aimTextureThrowCircle;

		// Token: 0x040005D3 RID: 1491
		private readonly ICoreClientAPI _clientApi;

		// Token: 0x040005D4 RID: 1492
		private bool _disposed;
	}
}
