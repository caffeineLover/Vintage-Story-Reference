using System;
using System.Text;
using RPVoiceChat;
using RPVoiceChat.GameContent.Renderers;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

// Token: 0x02000008 RID: 8
public abstract class BEWeldable : BlockEntityContainer
{
	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600002C RID: 44 RVA: 0x00003090 File Offset: 0x00001290
	protected ItemSlot FluxSlot
	{
		get
		{
			return this.Inv[0];
		}
	}

	// Token: 0x0600002D RID: 45 RVA: 0x000030A0 File Offset: 0x000012A0
	public BEWeldable(int numParts)
	{
		this.Inv = new InventoryGeneric(numParts + 1, null, null, null);
		this.FluxMeshRefs = new MeshRef[numParts];
		this.PartMeshRefs = new MeshRef[numParts];
		this.Inv.SlotModified += delegate(int i)
		{
			this.MarkDirty(true, null);
		};
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600002E RID: 46 RVA: 0x00003111 File Offset: 0x00001311
	public override InventoryBase Inventory
	{
		get
		{
			return this.Inv;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600002F RID: 47 RVA: 0x00003119 File Offset: 0x00001319
	public override string InventoryClassName
	{
		get
		{
			throw new Exception("BEWeldable is meant to be inherited but wasn't!");
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00003128 File Offset: 0x00001328
	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
		ICoreClientAPI capi = api as ICoreClientAPI;
		if (capi != null)
		{
			this.Renderer = new WeldableRenderer(capi, this);
			MeshData defaultBlockMesh = capi.TesselatorManager.GetDefaultBlockMesh(base.Block);
			if (defaultBlockMesh != null)
			{
				defaultBlockMesh.Clear();
			}
			this.UpdateMeshRefs();
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003178 File Offset: 0x00001378
	public virtual bool TestReadyToMerge()
	{
		for (int i = 1; i < this.Inv.Count; i++)
		{
			ItemSlot slot = this.Inv[i];
			if (slot.Empty)
			{
				return false;
			}
			if (slot.Itemstack.Collectible.GetTemperature(this.Api.World, slot.Itemstack) < (float)this.WeldingMinTemp)
			{
				return false;
			}
		}
		return !this.FluxSlot.Empty && this.FluxSlot.Itemstack.StackSize >= this.FluxNeeded;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00003208 File Offset: 0x00001408
	public virtual void ShowReadyToMergeErrors()
	{
		if (this.Api.Side != 2)
		{
			return;
		}
		ICoreClientAPI capi = this.Api as ICoreClientAPI;
		for (int i = 1; i < this.Inv.Count; i++)
		{
			ItemSlot slot = this.Inv[i];
			if (slot.Empty)
			{
				capi.TriggerIngameError(capi.World.Player, "missingparts", UIUtils.I18n(this.i18nPrefix + ".MissingParts"));
				return;
			}
			if (slot.Itemstack.Collectible.GetTemperature(this.Api.World, slot.Itemstack) < (float)this.WeldingMinTemp)
			{
				capi.TriggerIngameError(capi.World.Player, "toocold", UIUtils.I18n(this.i18nPrefix + ".TooCold"));
				return;
			}
		}
		if (this.FluxSlot.Empty || this.FluxSlot.Itemstack.StackSize < this.FluxNeeded)
		{
			capi.TriggerIngameError(capi.World.Player, "missingflux", UIUtils.I18n(this.i18nPrefix + ".MissingFlux"));
		}
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00003334 File Offset: 0x00001534
	protected virtual void UpdateMeshRefs()
	{
		ICoreClientAPI capi = this.Api as ICoreClientAPI;
		if (this.Api.Side == 1 || capi == null)
		{
			return;
		}
		int i = 0;
		while (i < this.FluxSlot.StackSize && !this.FluxSlot.Empty)
		{
			MeshData meshdata = this.RenderFlux(i);
			this.FluxMeshRefs[i] = capi.Render.UploadMesh(meshdata);
			i++;
		}
		for (int j = 0; j < this.PartMeshRefs.Length; j++)
		{
			if (!this.Inv[j + 1].Empty)
			{
				MeshData meshdata2 = this.RenderPart(j);
				this.PartMeshRefs[j] = capi.Render.UploadMesh(meshdata2);
			}
		}
	}

	// Token: 0x06000034 RID: 52
	protected abstract MeshData RenderPart(int numPart);

	// Token: 0x06000035 RID: 53
	protected abstract MeshData RenderFlux(int numFlux);

	// Token: 0x06000036 RID: 54 RVA: 0x000033E4 File Offset: 0x000015E4
	public virtual void OnHammerHitOver(IPlayer byPlayer, Vec3d hitPosition)
	{
		if (!this.TestReadyToMerge())
		{
			this.ShowReadyToMergeErrors();
			return;
		}
		this.HammerHits++;
		ItemSlot itemSlot;
		if (byPlayer == null)
		{
			itemSlot = null;
		}
		else
		{
			IPlayerInventoryManager inventoryManager = byPlayer.InventoryManager;
			itemSlot = ((inventoryManager != null) ? inventoryManager.ActiveHotbarSlot : null);
		}
		ItemSlot slot = itemSlot;
		if (slot != null)
		{
			ItemStack itemstack = slot.Itemstack;
			if (itemstack != null)
			{
				CollectibleObject collectible = itemstack.Collectible;
				if (collectible != null)
				{
					collectible.DamageItem(this.Api.World, byPlayer.Entity, slot, 1, true);
				}
			}
		}
		float temp = 1500f;
		for (int i = 1; i < this.Inv.Count; i++)
		{
			ItemSlot partSlot = this.Inv[i];
			if (partSlot.Empty)
			{
				return;
			}
			float partTemp = partSlot.Itemstack.Collectible.GetTemperature(this.Api.World, partSlot.Itemstack);
			temp = Math.Min(temp, partTemp);
		}
		if (temp > 800f && this.Api.Side == 2)
		{
			BlockEntityAnvil.bigMetalSparks.MinPos = this.Pos.ToVec3d().Add(hitPosition.X, hitPosition.Y, hitPosition.Z);
			BlockEntityAnvil.bigMetalSparks.VertexFlags = (int)((byte)GameMath.Clamp((int)(temp - 700f) / 2, 32, 128));
			this.Api.World.SpawnParticles(BlockEntityAnvil.bigMetalSparks, byPlayer);
			BlockEntityAnvil.smallMetalSparks.MinPos = this.Pos.ToVec3d().Add(hitPosition.X, hitPosition.Y, hitPosition.Z);
			BlockEntityAnvil.smallMetalSparks.VertexFlags = (int)((byte)GameMath.Clamp((int)(temp - 770f) / 3, 32, 128));
			this.Api.World.SpawnParticles(BlockEntityAnvil.smallMetalSparks, byPlayer);
		}
		if (this.HammerHits >= this.HammerHitsNeeded && this.Api.Side == 1)
		{
			Block resultingBlock = this.Api.World.GetBlock(new AssetLocation(RPVoiceChatMod.modID, this.ResultingBlockCode));
			ItemStack resultingStack = new ItemStack(resultingBlock, 1);
			resultingStack.Collectible.SetTemperature(this.Api.World, resultingStack, temp, true);
			this.Api.World.GetBlockAccessor(true, true, true, false).SetBlock(resultingBlock.Id, this.Pos, resultingStack);
		}
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00003625 File Offset: 0x00001825
	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
	{
		base.FromTreeAttributes(tree, worldForResolving);
		this.HammerHits = tree.GetInt("hammerHits", 0);
		if (this.Api != null)
		{
			this.UpdateMeshRefs();
		}
	}

	// Token: 0x06000038 RID: 56 RVA: 0x0000364F File Offset: 0x0000184F
	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetInt("hammerHits", this.HammerHits);
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00003669 File Offset: 0x00001869
	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		return true;
	}

	// Token: 0x0600003A RID: 58 RVA: 0x0000366C File Offset: 0x0000186C
	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
		for (int i = 1; i < this.Inv.Count; i++)
		{
			ItemSlot slot = this.Inv[i];
			if (!slot.Empty)
			{
				appendInterpolatedStringHandler..ctor(5, 2, dsc);
				appendInterpolatedStringHandler.AppendFormatted(slot.Itemstack.GetName());
				appendInterpolatedStringHandler.AppendLiteral(" - ");
				appendInterpolatedStringHandler.AppendFormatted<float>(slot.Itemstack.Collectible.GetTemperature(this.Api.World, slot.Itemstack));
				appendInterpolatedStringHandler.AppendLiteral("°C");
				dsc.AppendLine(ref appendInterpolatedStringHandler);
			}
		}
		if (!this.FluxSlot.Empty)
		{
			appendInterpolatedStringHandler..ctor(1, 2, dsc);
			appendInterpolatedStringHandler.AppendFormatted<int>(this.FluxSlot.Itemstack.StackSize);
			appendInterpolatedStringHandler.AppendLiteral(" ");
			appendInterpolatedStringHandler.AppendFormatted(this.FluxSlot.Itemstack.GetName());
			dsc.AppendLine(ref appendInterpolatedStringHandler);
		}
		if (this.TestReadyToMerge())
		{
			dsc.AppendLine(UIUtils.I18n(this.i18nPrefix + ".WeldReady"));
			return;
		}
		dsc.AppendLine(UIUtils.I18n(this.i18nPrefix + ".WeldNotReady"));
	}

	// Token: 0x0600003B RID: 59 RVA: 0x000037AE File Offset: 0x000019AE
	public override void MarkDirty(bool redrawOnClient = false, IPlayer skipPlayer = null)
	{
		base.MarkDirty(redrawOnClient, skipPlayer);
		if (this.Api.Side == 2)
		{
			this.UpdateMeshRefs();
		}
	}

	// Token: 0x0600003C RID: 60 RVA: 0x000037CC File Offset: 0x000019CC
	public override void OnBlockRemoved()
	{
		base.OnBlockRemoved();
		WeldableRenderer renderer = this.Renderer;
		if (renderer != null)
		{
			renderer.Dispose();
		}
		foreach (MeshRef meshRef in this.FluxMeshRefs)
		{
			if (meshRef != null)
			{
				meshRef.Dispose();
			}
		}
		foreach (MeshRef meshRef2 in this.PartMeshRefs)
		{
			if (meshRef2 != null)
			{
				meshRef2.Dispose();
			}
		}
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00003838 File Offset: 0x00001A38
	public override void OnBlockUnloaded()
	{
		base.OnBlockUnloaded();
		WeldableRenderer renderer = this.Renderer;
		if (renderer != null)
		{
			renderer.Dispose();
		}
		foreach (MeshRef meshRef in this.FluxMeshRefs)
		{
			if (meshRef != null)
			{
				meshRef.Dispose();
			}
		}
		foreach (MeshRef meshRef2 in this.PartMeshRefs)
		{
			if (meshRef2 != null)
			{
				meshRef2.Dispose();
			}
		}
	}

	// Token: 0x04000010 RID: 16
	protected string i18nPrefix = "Welding";

	// Token: 0x04000011 RID: 17
	protected int WeldingMinTemp = 550;

	// Token: 0x04000012 RID: 18
	protected int HammerHitsNeeded = 11;

	// Token: 0x04000013 RID: 19
	protected int HammerHits;

	// Token: 0x04000014 RID: 20
	protected int FluxNeeded;

	// Token: 0x04000015 RID: 21
	protected WeldableRenderer Renderer;

	// Token: 0x04000016 RID: 22
	public InventoryGeneric Inv;

	// Token: 0x04000017 RID: 23
	public MeshRef[] FluxMeshRefs;

	// Token: 0x04000018 RID: 24
	public MeshRef[] PartMeshRefs;

	// Token: 0x04000019 RID: 25
	protected string ResultingBlockCode;
}
