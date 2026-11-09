using System;
using Cairo;
using Jaunt.Behaviors;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace Jaunt.Hud
{
	// Token: 0x02000009 RID: 9
	public class HudIconRenderer : IRenderer, IDisposable
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003C RID: 60 RVA: 0x0000359B File Offset: 0x0000179B
		public static JauntModSystem ModSystem
		{
			get
			{
				return JauntModSystem.Instance;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600003D RID: 61 RVA: 0x000035A2 File Offset: 0x000017A2
		public double RenderOrder
		{
			get
			{
				return 1.0;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600003E RID: 62 RVA: 0x000035AD File Offset: 0x000017AD
		public int RenderRange
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000035B0 File Offset: 0x000017B0
		public HudIconRenderer(ICoreClientAPI api)
		{
			this.capi = api;
			api.Event.RegisterRenderer(this, 10, null);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000035DC File Offset: 0x000017DC
		public void Initialize()
		{
			if (!this.texturesDict.ContainsKey("empty"))
			{
				LoadedTexture empty = new LoadedTexture(this.capi);
				ImageSurface surface = new ImageSurface(0, (int)HudIconRenderer.ModSystem.Config.IconSize, (int)HudIconRenderer.ModSystem.Config.IconSize);
				this.capi.Gui.LoadOrUpdateCairoTexture(surface, true, ref empty);
				surface.Dispose();
				this.texturesDict.Add("empty", empty);
			}
			this.listenerId = this.capi.Event.RegisterGameTickListener(new Action<float>(this.OnGameTick), 100, 0);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003680 File Offset: 0x00001880
		public void RegisterTexture(AssetLocation assetLocation)
		{
			if (assetLocation == null || assetLocation.Path == null || assetLocation.Path.Length == 0)
			{
				return;
			}
			string loc = AssetLocationExtensions.ToNonNullString(assetLocation.Clone().WithPathPrefixOnce("textures/"));
			if (this.texturesDict.ContainsKey(loc))
			{
				return;
			}
			int size = (int)Math.Ceiling((double)((float)((int)HudIconRenderer.ModSystem.Config.IconSize) * RuntimeEnv.GUIScale));
			LoadedTexture texture = this.capi.Gui.LoadSvg(loc, size, size, size, size, new int?(-1));
			if (texture == null)
			{
				return;
			}
			this.texturesDict.Add(loc, texture);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000371C File Offset: 0x0000191C
		private bool CanRender()
		{
			return HudIconRenderer.ModSystem.Config.ShowGaitIcon && this.activeTexture != null;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000373C File Offset: 0x0000193C
		public void OnRenderFrame(float dt, EnumRenderStage stage)
		{
			if (!this.CanRender())
			{
				return;
			}
			float width = (float)GuiElement.scaled((double)HudIconRenderer.ModSystem.Config.IconSize);
			float height = (float)GuiElement.scaled((double)HudIconRenderer.ModSystem.Config.IconSize);
			float x = (float)(this.capi.Render.FrameWidth / 2) - width / 2f + (float)GuiElement.scaled((double)HudIconRenderer.ModSystem.Config.IconOffsetX);
			float y = (float)this.capi.Render.FrameHeight - height + (float)GuiElement.scaled((double)HudIconRenderer.ModSystem.Config.IconOffsetY);
			this.capi.Render.RenderTexture(this.activeTexture.TextureId, (double)x, (double)y, (double)width, (double)height, 50f, null);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003808 File Offset: 0x00001A08
		protected void OnGameTick(float dt)
		{
			IMountableSeat mountedOn = this.capi.World.Player.Entity.MountedOn;
			object obj;
			if (mountedOn == null)
			{
				obj = null;
			}
			else
			{
				IMountable mountSupplier = mountedOn.MountSupplier;
				if (mountSupplier == null)
				{
					obj = null;
				}
				else
				{
					Entity onEntity = mountSupplier.OnEntity;
					if (onEntity == null)
					{
						obj = null;
					}
					else
					{
						EntityBehaviorGait behavior = onEntity.GetBehavior<EntityBehaviorGait>();
						obj = ((behavior != null) ? behavior.CurrentGait : null);
					}
				}
			}
			JauntGaitMeta jauntGait = obj as JauntGaitMeta;
			if (jauntGait != null && jauntGait.IconTexture != null)
			{
				LoadedTexture value;
				this.activeTexture = (this.texturesDict.TryGetValue(jauntGait.IconTexture, ref value) ? value : this.texturesDict["empty"]);
				return;
			}
			this.activeTexture = this.texturesDict["empty"];
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000038BC File Offset: 0x00001ABC
		public void Dispose()
		{
			this.capi.Event.UnregisterRenderer(this, 10);
			this.capi.Event.UnregisterGameTickListener(this.listenerId);
			foreach (LoadedTexture loadedTexture in this.texturesDict.Values)
			{
				loadedTexture.Dispose();
			}
			this.texturesDict = null;
		}

		// Token: 0x04000027 RID: 39
		private ICoreClientAPI capi;

		// Token: 0x04000028 RID: 40
		private FastSmallDictionary<string, LoadedTexture> texturesDict = new FastSmallDictionary<string, LoadedTexture>(6);

		// Token: 0x04000029 RID: 41
		private LoadedTexture activeTexture;

		// Token: 0x0400002A RID: 42
		private long listenerId;
	}
}
