using System;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000045 RID: 69
	public class ItemFishingSpear : Item
	{
		// Token: 0x060002B0 RID: 688 RVA: 0x0001F624 File Offset: 0x0001D824
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			CollectibleBehaviorAnimationAuthoritative bh = base.GetCollectibleBehavior<CollectibleBehaviorAnimationAuthoritative>(true);
			if (bh == null)
			{
				api.World.Logger.Warning("Spear {0} uses ItemSpear class, but lacks required AnimationAuthoritative behavior. I'll take the freedom to add this behavior, but please fix json item type.", new object[]
				{
					this.Code
				});
				bh = new CollectibleBehaviorAnimationAuthoritative(this);
				bh.OnLoaded(api);
				this.CollectibleBehaviors = ArrayExtensions.Append<CollectibleBehavior>(this.CollectibleBehaviors, bh);
			}
			bh.OnBeginHitEntity += new CollectibleBehaviorAnimationAuthoritative.OnBeginHitEntityDelegate(this.ItemSpear_OnBeginHitEntity);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0001F69C File Offset: 0x0001D89C
		private void ItemSpear_OnBeginHitEntity(EntityAgent byEntity, ref EnumHandling handling)
		{
			EntityPlayer eplr = byEntity as EntityPlayer;
			ItemSlot slot = (eplr != null) ? eplr.ActiveHandItemSlot : null;
			if (((slot != null) ? slot.Itemstack : null) == null)
			{
				return;
			}
			if (slot.Itemstack.Attributes.GetString("fish", "none") != "none")
			{
				return;
			}
			if (byEntity.World.Side == 1)
			{
				EntitySelection entitySelection = eplr.EntitySelection;
				Entity target = (entitySelection != null) ? entitySelection.Entity : null;
				if (target == null)
				{
					target = byEntity.World.GetNearestEntity(byEntity.Pos.XYZ.Add(0.0, 1.0, 0.0), 1f, 1f, (Entity e) => Enumerable.Any<string>(this.validFishTypes, (string s) => e.Code.Path.Contains(s)));
				}
				if (target != null && byEntity.Attributes.GetInt("didattack", 0) == 0)
				{
					string thisEntityPath = target.Code.Path;
					if (Enumerable.Any<string>(this.validFishTypes, (string s) => thisEntityPath.Contains(s)))
					{
						byEntity.Attributes.SetInt("didattack", 1);
						string fishType = target.FirstCodePart(2) + "-" + target.FirstCodePart(3);
						if (fishType == "mahi-mahi")
						{
							fishType = fishType + "-" + target.FirstCodePart(4);
						}
						slot.Itemstack.Attributes.SetString("fish", fishType);
						if (!byEntity.IsEyesSubmerged() && target.FeetInLiquid)
						{
							byEntity.World.PlaySoundAt(new AssetLocation("game:sounds/effect/water-fill2"), (double)((float)byEntity.Pos.X), (double)((float)byEntity.Pos.Y), (double)((float)byEntity.Pos.Z), null, true, 32f, 1f);
						}
						ICoreAPI api = this.api;
						(this.api as ICoreServerAPI).World.DespawnEntity(target, new EntityDespawnData
						{
							Reason = 7
						});
						slot.Itemstack.Collectible.DamageItem(this.api.World, byEntity, slot, 1, true);
						slot.MarkDirty();
					}
				}
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0001F8C1 File Offset: 0x0001DAC1
		public override string GetHeldTpUseAnimation(ItemSlot activeHotbarSlot, Entity byEntity)
		{
			return null;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0001F8C4 File Offset: 0x0001DAC4
		public override void OnHeldInteractStart(ItemSlot itemslot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			base.OnHeldInteractStart(itemslot, byEntity, blockSel, entitySel, firstEvent, ref handling);
			if (handling == 4)
			{
				return;
			}
			handling = 4;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0001F8E0 File Offset: 0x0001DAE0
		public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			return true;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0001F8E3 File Offset: 0x0001DAE3
		public override bool OnHeldInteractCancel(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, EnumItemUseCancelReason cancelReason)
		{
			return true;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0001F8E8 File Offset: 0x0001DAE8
		public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			if (secondsUsed < 0.35f)
			{
				return;
			}
			string fishType = slot.Itemstack.Attributes.GetString("fish", "none");
			if (fishType == "none")
			{
				return;
			}
			ItemStack itemstack = slot.Itemstack;
			if (itemstack != null)
			{
				ITreeAttribute attributes = itemstack.Attributes;
				if (attributes != null)
				{
					attributes.SetString("fish", "none");
				}
			}
			if (byEntity.World.Side == 2)
			{
				(byEntity.World as IClientWorldAccessor).Api.Event.EnqueueMainThreadTask(delegate()
				{
					slot.Itemstack.TempAttributes.RemoveAttribute("meshRefId");
				}, "remeshspear");
				return;
			}
			AssetLocation fishAsset = new AssetLocation("game:fishraw-freshwater-" + fishType + "-adult");
			Item newItem = byEntity.World.GetItem(new AssetLocation(fishAsset));
			if (newItem == null)
			{
				fishAsset = new AssetLocation("game:fishraw-saltwater-" + fishType + "-adult");
				newItem = byEntity.World.GetItem(new AssetLocation(fishAsset));
			}
			if (newItem == null)
			{
				return;
			}
			ItemStack giveStack = new ItemStack(newItem, 1);
			if (giveStack != null)
			{
				this.api.World.SpawnItemEntity(giveStack, byEntity.Pos.XYZ.AddCopy(0.0, 0.5, 0.0), null);
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0001FA4B File Offset: 0x0001DC4B
		public override void OnHeldAttackStop(float secondsPassed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSelection, EntitySelection entitySel)
		{
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0001FA50 File Offset: 0x0001DC50
		public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
		{
			if (this.Code.Path.Contains("empty"))
			{
				return ArrayExtensions.Append<WorldInteraction>(new WorldInteraction[]
				{
					new WorldInteraction
					{
						ActionLangCode = "handhelp-spearfish",
						MouseButton = 0
					}
				}, base.GetHeldInteractionHelp(inSlot));
			}
			return ArrayExtensions.Append<WorldInteraction>(new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = "handhelp-removefish",
					MouseButton = 2
				}
			}, base.GetHeldInteractionHelp(inSlot));
		}

		// Token: 0x04000120 RID: 288
		private readonly string[] validFishTypes = new string[]
		{
			"salmon",
			"bream-sea",
			"gurnard-cape",
			"haddock-common",
			"hake-silver",
			"herring-atlantic",
			"mackerel-atlantic",
			"pollock-alaska",
			"perch-pacific",
			"barracuda-great",
			"grouper-black",
			"snapper-red",
			"tuna-skipjack",
			"wolf-bering",
			"amberjack-yellowtail",
			"mahi-mahi-common",
			"wreckfish-atlantic",
			"coelacanth-common",
			"sturgeon-atlantic",
			"alewife-shad",
			"chub-river",
			"crappie-black",
			"crappie-white",
			"perch-european",
			"perch-yellow",
			"piranha-black",
			"piranha-red",
			"trout-brown",
			"trout-rainbow",
			"bass-largemouth",
			"bass-smallmouth",
			"carp-common",
			"carp-grass",
			"catfish-blue",
			"catfish-channel",
			"pickerel-chain",
			"salmon-coho",
			"tilapia-nile",
			"tilapia-red",
			"walleye-common",
			"pike-northern",
			"arapaima-arapaima",
			"arapaima-gigas",
			"sheatfish-black",
			"sheatfish-white"
		};
	}
}
