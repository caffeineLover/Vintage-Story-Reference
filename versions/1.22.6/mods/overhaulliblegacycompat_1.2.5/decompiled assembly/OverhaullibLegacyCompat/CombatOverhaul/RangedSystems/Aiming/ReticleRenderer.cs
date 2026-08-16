using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000E2 RID: 226
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ReticleRenderer : IRenderer, IDisposable
	{
		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x0003C138 File Offset: 0x0003A338
		// (set) Token: 0x06000BBA RID: 3002 RVA: 0x0003C140 File Offset: 0x0003A340
		public WeaponAimingState AimingState { get; set; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x0003C149 File Offset: 0x0003A349
		// (set) Token: 0x06000BBC RID: 3004 RVA: 0x0003C151 File Offset: 0x0003A351
		public bool ReticleScaling { get; set; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x0003C15A File Offset: 0x0003A35A
		// (set) Token: 0x06000BBE RID: 3006 RVA: 0x0003C162 File Offset: 0x0003A362
		public bool ThrowCircle { get; set; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x0003C16B File Offset: 0x0003A36B
		// (set) Token: 0x06000BC0 RID: 3008 RVA: 0x0003C173 File Offset: 0x0003A373
		public bool DebugRender { get; set; }

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x0003C17C File Offset: 0x0003A37C
		// (set) Token: 0x06000BC2 RID: 3010 RVA: 0x0003C184 File Offset: 0x0003A384
		public Vector2 AimingPoint { get; set; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x0003C18D File Offset: 0x0003A38D
		public double RenderOrder
		{
			get
			{
				return 0.98;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x0003C198 File Offset: 0x0003A398
		public int RenderRange
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0003C1A0 File Offset: 0x0003A3A0
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

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0003C2C0 File Offset: 0x0003A4C0
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

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0003C4A0 File Offset: 0x0003A6A0
		public void SetReticleTextures(string partCharge = "", string fullCharge = "", string blocked = "")
		{
			this._currentTextures[WeaponAimingState.Blocked] = ((blocked != "") ? this.GetTexture(blocked) : this._defaultTextures[WeaponAimingState.Blocked]);
			this._currentTextures[WeaponAimingState.PartCharge] = ((partCharge != "") ? this.GetTexture(partCharge) : this._defaultTextures[WeaponAimingState.PartCharge]);
			this._currentTextures[WeaponAimingState.FullCharge] = ((fullCharge != "") ? this.GetTexture(fullCharge) : this._defaultTextures[WeaponAimingState.FullCharge]);
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0003C538 File Offset: 0x0003A738
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

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0003C610 File Offset: 0x0003A810
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

		// Token: 0x040005AD RID: 1453
		private readonly Dictionary<WeaponAimingState, LoadedTexture> _defaultTextures = new Dictionary<WeaponAimingState, LoadedTexture>();

		// Token: 0x040005AE RID: 1454
		private readonly Dictionary<WeaponAimingState, LoadedTexture> _currentTextures = new Dictionary<WeaponAimingState, LoadedTexture>();

		// Token: 0x040005AF RID: 1455
		private readonly Dictionary<string, LoadedTexture> _loadedTextures = new Dictionary<string, LoadedTexture>();

		// Token: 0x040005B0 RID: 1456
		private readonly LoadedTexture _aimTextureThrowCircle;

		// Token: 0x040005B1 RID: 1457
		private readonly ICoreClientAPI _clientApi;

		// Token: 0x040005B2 RID: 1458
		private bool _disposed;
	}
}
