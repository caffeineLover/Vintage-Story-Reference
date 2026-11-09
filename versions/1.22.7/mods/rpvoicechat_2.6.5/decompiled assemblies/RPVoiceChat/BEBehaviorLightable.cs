using System;
using RPVoiceChat.GameContent;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

// Token: 0x02000005 RID: 5
public class BEBehaviorLightable : BlockEntityBehavior, IPointLight
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600000F RID: 15 RVA: 0x000026B4 File Offset: 0x000008B4
	// (set) Token: 0x06000010 RID: 16 RVA: 0x000026BC File Offset: 0x000008BC
	public Vec3d Pos { get; private set; }

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000011 RID: 17 RVA: 0x000026C5 File Offset: 0x000008C5
	// (set) Token: 0x06000012 RID: 18 RVA: 0x000026CD File Offset: 0x000008CD
	public Vec3f Color { get; private set; }

	// Token: 0x06000013 RID: 19 RVA: 0x000026D8 File Offset: 0x000008D8
	public BEBehaviorLightable(BlockEntity blockEntity) : base(blockEntity)
	{
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002728 File Offset: 0x00000928
	public void SetLightActive(bool active)
	{
		if (this.isLightActive == active)
		{
			if (active)
			{
				ICoreAPI api = this.Blockentity.Api;
				if ((api != null && api.Side == 2) || this.capi != null)
				{
					this.UpdateLight();
				}
			}
			return;
		}
		this.isLightActive = active;
		ICoreAPI api2 = this.Blockentity.Api;
		if (api2 != null && api2.Side == 1)
		{
			this.Blockentity.MarkDirty(true, null);
		}
		ICoreAPI api3 = this.Blockentity.Api;
		if ((api3 != null && api3.Side == 2) || this.capi != null)
		{
			this.UpdateLight();
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000027C6 File Offset: 0x000009C6
	public void SetLightColor(Vec3f color)
	{
		this.lightColor = color;
		if (this.isLightActive)
		{
			ICoreAPI api = this.Blockentity.Api;
			if (api != null && api.Side == 2)
			{
				this.UpdateLight();
			}
		}
		this.Blockentity.MarkDirty(false, null);
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002808 File Offset: 0x00000A08
	public void SetLightLevel(float level)
	{
		this.lightLevel = Math.Max(0f, Math.Min(1f, level));
		if (this.isLightActive)
		{
			ICoreAPI api = this.Blockentity.Api;
			if (api != null && api.Side == 2)
			{
				this.UpdateLight();
			}
		}
		this.Blockentity.MarkDirty(false, null);
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000017 RID: 23 RVA: 0x00002867 File Offset: 0x00000A67
	public bool IsLightActive
	{
		get
		{
			return this.isLightActive;
		}
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002870 File Offset: 0x00000A70
	public void SetClientPulseFactor(float factor)
	{
		this.clientPulseFactor = Math.Max(0f, factor);
		if (this.isLightActive)
		{
			ICoreAPI api = this.Blockentity.Api;
			if ((api != null && api.Side == 2) || this.capi != null)
			{
				this.UpdateLight();
			}
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x000028C0 File Offset: 0x00000AC0
	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		if (properties != null)
		{
			if (properties["lightLevel"].Exists)
			{
				this.lightLevel = properties["lightLevel"].AsFloat(1f);
			}
			if (properties["lightColorX"].Exists || properties["lightColorY"].Exists || properties["lightColorZ"].Exists)
			{
				this.lightColor = new Vec3f(properties["lightColorX"].AsFloat(1f), properties["lightColorY"].AsFloat(0.9f), properties["lightColorZ"].AsFloat(0.7f));
			}
			if (properties["lightIntensityScale"].Exists)
			{
				this.lightIntensityScale = GameMath.Clamp(properties["lightIntensityScale"].AsFloat(1f), 0.1f, 25f);
			}
		}
		if (api.Side == 2)
		{
			this.capi = (api as ICoreClientAPI);
			this.UpdateLight();
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000029E0 File Offset: 0x00000BE0
	private Vec3d GetLightOrigin()
	{
		IBlockEntityWithCustomLightPosition custom = this.Blockentity as IBlockEntityWithCustomLightPosition;
		if (custom == null)
		{
			return this.Blockentity.Pos.ToWorldCenter();
		}
		return custom.GetLightOrigin();
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002A14 File Offset: 0x00000C14
	private void UpdateLight()
	{
		if (this.capi == null)
		{
			return;
		}
		if (!this.isLightActive)
		{
			if (this.lightInRenderList)
			{
				this.capi.Render.RemovePointLight(this);
				this.lightInRenderList = false;
			}
			return;
		}
		float s = this.lightLevel * this.lightIntensityScale * this.clientPulseFactor;
		Vec3f c = new Vec3f(this.lightColor.Z * s, this.lightColor.Y * s, this.lightColor.X * s);
		if (this.lightInRenderList)
		{
			this.Pos = this.GetLightOrigin();
			this.Color = c;
			return;
		}
		this.Pos = this.GetLightOrigin();
		this.Color = c;
		this.capi.Render.AddPointLight(this);
		this.lightInRenderList = true;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002ADC File Offset: 0x00000CDC
	public override void OnBlockRemoved()
	{
		base.OnBlockRemoved();
		this.RemoveLight();
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002AEA File Offset: 0x00000CEA
	public override void OnBlockUnloaded()
	{
		base.OnBlockUnloaded();
		this.RemoveLight();
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002AF8 File Offset: 0x00000CF8
	private void RemoveLight()
	{
		if (this.capi != null && this.lightInRenderList)
		{
			this.capi.Render.RemovePointLight(this);
			this.lightInRenderList = false;
		}
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002B24 File Offset: 0x00000D24
	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		this.isLightActive = tree.GetBool("isLightActive", false);
		if (tree.HasAttribute("lightColorX") || tree.HasAttribute("lightColorY") || tree.HasAttribute("lightColorZ"))
		{
			this.lightColor = new Vec3f(tree.GetFloat("lightColorX", this.lightColor.X), tree.GetFloat("lightColorY", this.lightColor.Y), tree.GetFloat("lightColorZ", this.lightColor.Z));
		}
		if (tree.HasAttribute("lightLevel"))
		{
			this.lightLevel = tree.GetFloat("lightLevel", this.lightLevel);
		}
		if (tree.HasAttribute("lightIntensityScale"))
		{
			this.lightIntensityScale = GameMath.Clamp(tree.GetFloat("lightIntensityScale", this.lightIntensityScale), 0.1f, 25f);
		}
		if (this.capi == null)
		{
			this.capi = ((((worldAccessForResolve != null) ? worldAccessForResolve.Api : null) as ICoreClientAPI) ?? (this.Blockentity.Api as ICoreClientAPI));
		}
		if (this.capi != null && worldAccessForResolve != null && worldAccessForResolve.Side == 2)
		{
			this.capi.Event.EnqueueMainThreadTask(new Action(this.UpdateLight), "rpvoicechat:UpdatePointLight");
		}
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002C80 File Offset: 0x00000E80
	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetBool("isLightActive", this.isLightActive);
		tree.SetFloat("lightColorX", this.lightColor.X);
		tree.SetFloat("lightColorY", this.lightColor.Y);
		tree.SetFloat("lightColorZ", this.lightColor.Z);
		tree.SetFloat("lightLevel", this.lightLevel);
		tree.SetFloat("lightIntensityScale", this.lightIntensityScale);
	}

	// Token: 0x04000005 RID: 5
	private bool isLightActive;

	// Token: 0x04000006 RID: 6
	private bool lightInRenderList;

	// Token: 0x04000007 RID: 7
	private Vec3f lightColor = new Vec3f(1f, 0.9f, 0.7f);

	// Token: 0x04000008 RID: 8
	private float lightLevel = 1f;

	// Token: 0x04000009 RID: 9
	private float lightIntensityScale = 1f;

	// Token: 0x0400000A RID: 10
	private float clientPulseFactor = 1f;

	// Token: 0x0400000B RID: 11
	private ICoreClientAPI capi;
}
