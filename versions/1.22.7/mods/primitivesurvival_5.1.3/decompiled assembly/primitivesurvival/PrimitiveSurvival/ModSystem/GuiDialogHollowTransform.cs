using System;
using System.Text;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Client;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000040 RID: 64
	public class GuiDialogHollowTransform : GuiDialog
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0001D661 File Offset: 0x0001B861
		// (set) Token: 0x06000284 RID: 644 RVA: 0x0001D684 File Offset: 0x0001B884
		private ModelTransform TargetTransform
		{
			get
			{
				BehaviorInTreeHollowTransform behavior = this.oldCollectible.GetBehavior<BehaviorInTreeHollowTransform>();
				return ((behavior != null) ? behavior.Transform : null) ?? ModelTransform.NoTransform;
			}
			set
			{
				if (!this.oldCollectible.HasBehavior<BehaviorInTreeHollowTransform>(false))
				{
					this.oldCollectible.CollectibleBehaviors = ArrayExtensions.Append<CollectibleBehavior>(this.oldCollectible.CollectibleBehaviors, new BehaviorInTreeHollowTransform(this.oldCollectible));
				}
				this.oldCollectible.GetBehavior<BehaviorInTreeHollowTransform>().Transform = value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0001D6D6 File Offset: 0x0001B8D6
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0001D6D9 File Offset: 0x0001B8D9
		public override bool PrefersUngrabbedMouse
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0001D6DC File Offset: 0x0001B8DC
		public GuiDialogHollowTransform(ICoreClientAPI capi) : base(capi)
		{
			if (this.devToolsEnabled)
			{
				this.capi = capi;
				capi.ChatCommands.Create("hollowTfEdit").WithDescription("Edit the model transform for the item displayed in the currently targeted Tree Hollow.").HandleWith(new OnCommandDelegate(this.OnCommandTransformEditor)).RequiresPrivilege(Privilege.useblock).Validate();
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0001D754 File Offset: 0x0001B954
		private TextCommandResult OnCommandTransformEditor(TextCommandCallingArgs args)
		{
			BlockSelection currentBlockSelection = this.capi.World.Player.CurrentBlockSelection;
			BlockPos blockPos = (currentBlockSelection != null) ? currentBlockSelection.Position : null;
			if (blockPos != null)
			{
				BETreeHollowGrown treeHollowGrown = this.capi.World.BlockAccessor.GetBlockEntity(blockPos) as BETreeHollowGrown;
				if (treeHollowGrown != null)
				{
					CollectibleObject collectible = treeHollowGrown.GetDisplayedCollectible();
					if (collectible != null)
					{
						this.currentTransform = new ModelTransform
						{
							Rotation = new Vec3f(),
							Translation = new Vec3f()
						};
						this.oldCollectible = collectible;
						this.oldPos = blockPos;
						this.originalTransform = this.TargetTransform;
						this.TargetTransform = (this.currentTransform = this.originalTransform.Clone());
						this.TryOpen();
						return TextCommandResult.Success("Opened Tree Hollow Transform dialog.", null);
					}
				}
			}
			return TextCommandResult.Error("Must be targeting a Tree Hollow that contains an item to use that command.", "");
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0001D833 File Offset: 0x0001BA33
		public override void OnGuiOpened()
		{
			this.ComposeDialog();
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0001D83C File Offset: 0x0001BA3C
		private void ComposeDialog()
		{
			base.ClearComposers();
			ElementBounds line = ElementBounds.Fixed(0.0, 22.0, 500.0, 20.0);
			ElementBounds inputBnds = ElementBounds.Fixed(0.0, 11.0, 230.0, 30.0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(1).WithFixedAlignmentOffset(110.0 + GuiStyle.DialogToScreenPadding, GuiStyle.DialogToScreenPadding);
			ElementBounds textAreaBounds = ElementBounds.FixedSize(500.0, 200.0);
			ElementBounds clippingBounds = ElementBounds.FixedSize(500.0, 200.0);
			ElementBounds btnBounds = ElementBounds.FixedSize(200.0, 20.0).WithAlignment(4).WithFixedPadding(10.0, 2.0);
			base.SingleComposer = GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddSmallButton(GuiElementClipHelpler.EndClip(GuiComposerHelpers.AddTextArea(GuiElementClipHelpler.BeginClip(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddSwitch(GuiComposerHelpers.AddSlider(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddSlider(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddSlider(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddSlider(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddNumberInput(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddNumberInput(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddNumberInput(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddNumberInput(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddNumberInput(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddNumberInput(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("transformeditor", dialogBounds), bgBounds, true, 5.0, 0.75f), "Transform Editor (Tree Hollows)", new Action(this.OnTitleBarClose), null, null, null).BeginChildElements(bgBounds), "Translation X", CairoFont.WhiteDetailText(), line = line.FlatCopy().WithFixedWidth(230.0), null), inputBnds = inputBnds.BelowCopy(0.0, 0.0, 0.0, 0.0), new Action<string>(this.OnTranslateX), CairoFont.WhiteDetailText(), "translatex"), "Origin X", CairoFont.WhiteDetailText(), line.RightCopy(40.0, 0.0, 0.0, 0.0), null), inputBnds.RightCopy(40.0, 0.0, 0.0, 0.0), new Action<string>(this.OnOriginX), CairoFont.WhiteDetailText(), "originx"), "Translation Y", CairoFont.WhiteDetailText(), line = line.BelowCopy(0.0, 33.0, 0.0, 0.0), null), inputBnds = inputBnds.BelowCopy(0.0, 22.0, 0.0, 0.0), new Action<string>(this.OnTranslateY), CairoFont.WhiteDetailText(), "translatey"), "Origin Y", CairoFont.WhiteDetailText(), line.RightCopy(40.0, 0.0, 0.0, 0.0), null), inputBnds.RightCopy(40.0, 0.0, 0.0, 0.0), new Action<string>(this.OnOriginY), CairoFont.WhiteDetailText(), "originy"), "Translation Z", CairoFont.WhiteDetailText(), line = line.BelowCopy(0.0, 32.0, 0.0, 0.0), null), inputBnds = inputBnds.BelowCopy(0.0, 22.0, 0.0, 0.0), new Action<string>(this.OnTranslateZ), CairoFont.WhiteDetailText(), "translatez"), "Origin Z", CairoFont.WhiteDetailText(), line.RightCopy(40.0, 0.0, 0.0, 0.0), null), inputBnds.RightCopy(40.0, 0.0, 0.0, 0.0), new Action<string>(this.OnOriginZ), CairoFont.WhiteDetailText(), "originz"), "Rotation X", CairoFont.WhiteDetailText(), line = line.BelowCopy(0.0, 33.0, 0.0, 0.0).WithFixedWidth(500.0), null), new ActionConsumable<int>(this.OnRotateX), inputBnds = inputBnds.BelowCopy(0.0, 22.0, 0.0, 0.0).WithFixedWidth(500.0), "rotatex"), "Rotation Y", CairoFont.WhiteDetailText(), line = line.BelowCopy(0.0, 32.0, 0.0, 0.0), null), new ActionConsumable<int>(this.OnRotateY), inputBnds = inputBnds.BelowCopy(0.0, 22.0, 0.0, 0.0), "rotatey"), "Rotation Z", CairoFont.WhiteDetailText(), line = line.BelowCopy(0.0, 32.0, 0.0, 0.0), null), new ActionConsumable<int>(this.OnRotateZ), inputBnds = inputBnds.BelowCopy(0.0, 22.0, 0.0, 0.0), "rotatez"), "Scale", CairoFont.WhiteDetailText(), line = line.BelowCopy(0.0, 32.0, 0.0, 0.0), null), new ActionConsumable<int>(this.OnScale), inputBnds = inputBnds.BelowCopy(0.0, 22.0, 0.0, 0.0), "scale"), new Action<bool>(this.onFlipXAxis), inputBnds = inputBnds.BelowCopy(0.0, 10.0, 0.0, 0.0), "flipx", 20.0, 4.0), "Flip on X-Axis", CairoFont.WhiteDetailText(), inputBnds.RightCopy(10.0, 1.0, 0.0, 0.0).WithFixedWidth(200.0), null), "Json Code (added as a behavior to the collectible)", CairoFont.WhiteDetailText(), line.BelowCopy(0.0, 72.0, 0.0, 0.0), null), clippingBounds.FixedUnder(inputBnds, 37.0)), textAreaBounds, null, CairoFont.WhiteSmallText(), "textarea")), "Close & Apply", new ActionConsumable(this.OnApplyJson), btnBounds = btnBounds.FlatCopy().FixedUnder(clippingBounds, 15.0), 2, null), "Copy JSON", new ActionConsumable(this.OnCopyJson), btnBounds.FlatCopy().WithAlignment(12).WithFixedPadding(10.0, 2.0), 2, null).EndChildElements().Compose(true);
			GuiComposerHelpers.GetTextInput(base.SingleComposer, "translatex").SetValue(this.currentTransform.Translation.X.ToString(GlobalConstants.DefaultCultureInfo), true);
			GuiComposerHelpers.GetTextInput(base.SingleComposer, "translatey").SetValue(this.currentTransform.Translation.Y.ToString(GlobalConstants.DefaultCultureInfo), true);
			GuiComposerHelpers.GetTextInput(base.SingleComposer, "translatez").SetValue(this.currentTransform.Translation.Z.ToString(GlobalConstants.DefaultCultureInfo), true);
			GuiComposerHelpers.GetTextInput(base.SingleComposer, "originx").SetValue(this.currentTransform.Origin.X.ToString(GlobalConstants.DefaultCultureInfo), true);
			GuiComposerHelpers.GetTextInput(base.SingleComposer, "originy").SetValue(this.currentTransform.Origin.Y.ToString(GlobalConstants.DefaultCultureInfo), true);
			GuiComposerHelpers.GetTextInput(base.SingleComposer, "originz").SetValue(this.currentTransform.Origin.Z.ToString(GlobalConstants.DefaultCultureInfo), true);
			GuiComposerHelpers.GetSlider(base.SingleComposer, "rotatex").SetValues((int)this.currentTransform.Rotation.X, -180, 180, 1, "");
			GuiComposerHelpers.GetSlider(base.SingleComposer, "rotatey").SetValues((int)this.currentTransform.Rotation.Y, -180, 180, 1, "");
			GuiComposerHelpers.GetSlider(base.SingleComposer, "rotatez").SetValues((int)this.currentTransform.Rotation.Z, -180, 180, 1, "");
			GuiComposerHelpers.GetSlider(base.SingleComposer, "scale").SetValues((int)Math.Abs(100f * this.currentTransform.ScaleXYZ.X), 25, 600, 1, "");
			GuiComposerHelpers.GetSwitch(base.SingleComposer, "flipx").On = (this.currentTransform.ScaleXYZ.X < 0f);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0001E249 File Offset: 0x0001C449
		private void onFlipXAxis(bool on)
		{
			ModelTransform modelTransform = this.currentTransform;
			modelTransform.ScaleXYZ.X = modelTransform.ScaleXYZ.X * -1f;
			this.updateJson();
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0001E26A File Offset: 0x0001C46A
		private void OnOriginX(string val)
		{
			this.currentTransform.Origin.X = StringUtil.ToFloat(val, 0f);
			this.updateJson();
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0001E28D File Offset: 0x0001C48D
		private void OnOriginY(string val)
		{
			this.currentTransform.Origin.Y = StringUtil.ToFloat(val, 0f);
			this.updateJson();
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0001E2B0 File Offset: 0x0001C4B0
		private void OnOriginZ(string val)
		{
			this.currentTransform.Origin.Z = StringUtil.ToFloat(val, 0f);
			this.updateJson();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0001E2D4 File Offset: 0x0001C4D4
		private bool OnApplyJson()
		{
			this.TargetTransform = (this.originalTransform = this.currentTransform);
			this.currentTransform = null;
			this.TryClose();
			return true;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0001E305 File Offset: 0x0001C505
		private bool OnCopyJson()
		{
			ScreenManager.Platform.XPlatInterface.SetClipboardText(this.getJson());
			return true;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0001E31D File Offset: 0x0001C51D
		private void updateJson()
		{
			GuiComposerHelpers.GetTextArea(base.SingleComposer, "textarea").SetValue(this.getJson(), true);
			this.capi.World.BlockAccessor.MarkBlockDirty(this.oldPos, null);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0001E358 File Offset: 0x0001C558
		private string getJson()
		{
			StringBuilder json = new StringBuilder();
			ModelTransform def = ModelTransform.NoTransform;
			string indent = "\t";
			json.Append("{\n");
			json.Append(indent + "\"name\": \"intreeHollowTransform\",\n");
			json.Append(indent + "\"properties\":\n");
			json.Append(indent + "{\n");
			indent = "\t\t";
			bool added = false;
			if (this.currentTransform.Translation.X != def.Translation.X || this.currentTransform.Translation.Y != def.Translation.Y || this.currentTransform.Translation.Z != def.Translation.Z)
			{
				json.Append(string.Format(GlobalConstants.DefaultCultureInfo, indent + "translation: {{ x: {0}, y: {1}, z: {2} }}", this.currentTransform.Translation.X, this.currentTransform.Translation.Y, this.currentTransform.Translation.Z));
				added = true;
			}
			if (this.currentTransform.Rotation.X != def.Rotation.X || this.currentTransform.Rotation.Y != def.Rotation.Y || this.currentTransform.Rotation.Z != def.Rotation.Z)
			{
				if (added)
				{
					json.Append(",\n");
				}
				json.Append(string.Format(GlobalConstants.DefaultCultureInfo, indent + "rotation: {{ x: {0}, y: {1}, z: {2} }}", this.currentTransform.Rotation.X, this.currentTransform.Rotation.Y, this.currentTransform.Rotation.Z));
				added = true;
			}
			if (this.currentTransform.Origin.X != def.Origin.X || this.currentTransform.Origin.Y != def.Origin.Y || this.currentTransform.Origin.Z != def.Origin.Z)
			{
				if (added)
				{
					json.Append(",\n");
				}
				json.Append(string.Format(GlobalConstants.DefaultCultureInfo, indent + "origin: {{ x: {0}, y: {1}, z: {2} }}", this.currentTransform.Origin.X, this.currentTransform.Origin.Y, this.currentTransform.Origin.Z));
				added = true;
			}
			if (this.currentTransform.ScaleXYZ.X != def.ScaleXYZ.X)
			{
				if (added)
				{
					json.Append(",\n");
				}
				if (this.currentTransform.ScaleXYZ.X != this.currentTransform.ScaleXYZ.Y || this.currentTransform.ScaleXYZ.X != this.currentTransform.ScaleXYZ.Z)
				{
					json.Append(string.Format(GlobalConstants.DefaultCultureInfo, indent + "scaleXyz: {{ x: {0}, y: {1}, z: {2} }}", this.currentTransform.ScaleXYZ.X, this.currentTransform.ScaleXYZ.Y, this.currentTransform.ScaleXYZ.Z));
				}
				else
				{
					json.Append(string.Format(GlobalConstants.DefaultCultureInfo, indent + "scale: {0}", this.currentTransform.ScaleXYZ.X));
				}
			}
			indent = "\t";
			json.Append("\n");
			json.Append(indent + "}\n");
			json.Append("}");
			string jsonstr = json.ToString();
			TreeAttribute treeAttribute = new TreeAttribute();
			treeAttribute.SetString("json", jsonstr);
			return treeAttribute.GetString("json", null);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0001E754 File Offset: 0x0001C954
		private bool OnScale(int val)
		{
			this.currentTransform.Scale = (float)val / 100f;
			if (GuiComposerHelpers.GetSwitch(base.SingleComposer, "flipx").On)
			{
				ModelTransform modelTransform = this.currentTransform;
				modelTransform.ScaleXYZ.X = modelTransform.ScaleXYZ.X * -1f;
			}
			this.updateJson();
			return true;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0001E7AB File Offset: 0x0001C9AB
		private bool OnRotateX(int deg)
		{
			this.currentTransform.Rotation.X = (float)deg;
			this.updateJson();
			return true;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0001E7C6 File Offset: 0x0001C9C6
		private bool OnRotateY(int deg)
		{
			this.currentTransform.Rotation.Y = (float)deg;
			this.updateJson();
			return true;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0001E7E1 File Offset: 0x0001C9E1
		private bool OnRotateZ(int deg)
		{
			this.currentTransform.Rotation.Z = (float)deg;
			this.updateJson();
			return true;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0001E7FC File Offset: 0x0001C9FC
		private void OnTranslateX(string val)
		{
			this.currentTransform.Translation.X = StringUtil.ToFloat(val, 0f);
			this.updateJson();
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0001E81F File Offset: 0x0001CA1F
		private void OnTranslateY(string val)
		{
			this.currentTransform.Translation.Y = StringUtil.ToFloat(val, 0f);
			this.updateJson();
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0001E842 File Offset: 0x0001CA42
		private void OnTranslateZ(string val)
		{
			this.currentTransform.Translation.Z = StringUtil.ToFloat(val, 0f);
			this.updateJson();
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0001E865 File Offset: 0x0001CA65
		private void OnTitleBarClose()
		{
			this.TryClose();
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0001E86E File Offset: 0x0001CA6E
		public override void OnGuiClosed()
		{
			base.OnGuiClosed();
			if (this.oldCollectible != null)
			{
				this.TargetTransform = this.originalTransform;
			}
			this.capi.World.BlockAccessor.MarkBlockDirty(this.oldPos, null);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0001E8A6 File Offset: 0x0001CAA6
		public override void OnMouseWheel(MouseWheelEventArgs args)
		{
			base.OnMouseWheel(args);
			args.SetHandled(true);
		}

		// Token: 0x0400011B RID: 283
		private ModelTransform originalTransform;

		// Token: 0x0400011C RID: 284
		private CollectibleObject oldCollectible;

		// Token: 0x0400011D RID: 285
		private BlockPos oldPos;

		// Token: 0x0400011E RID: 286
		private ModelTransform currentTransform = new ModelTransform();

		// Token: 0x0400011F RID: 287
		private readonly bool devToolsEnabled = ModConfig.Loaded.TreeHollowsEnableDeveloperTools;
	}
}
