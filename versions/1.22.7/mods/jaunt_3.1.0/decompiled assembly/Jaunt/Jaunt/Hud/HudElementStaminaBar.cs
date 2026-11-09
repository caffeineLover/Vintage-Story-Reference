using System;
using Jaunt.Behaviors;
using Jaunt.Config;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Jaunt.Hud
{
	// Token: 0x02000008 RID: 8
	public class HudElementStaminaBar : HudElement
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002FB4 File Offset: 0x000011B4
		public static JauntModSystem ModSystem
		{
			get
			{
				return JauntModSystem.Instance;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002FBB File Offset: 0x000011BB
		public float StaminaBarWidthMultiplier
		{
			get
			{
				return HudElementStaminaBar.ModSystem.Config.StaminaBarWidthMultiplier;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002FCC File Offset: 0x000011CC
		public float StaminaBarXOffset
		{
			get
			{
				return HudElementStaminaBar.ModSystem.Config.StaminaBarXOffset;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002FDD File Offset: 0x000011DD
		public float StaminaBarYOffset
		{
			get
			{
				return HudElementStaminaBar.ModSystem.Config.StaminaBarYOffset;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002FF0 File Offset: 0x000011F0
		public StaminaBarPlacement StaminaBarLocation
		{
			get
			{
				StaminaBarPlacement location;
				if (!Enum.TryParse<StaminaBarPlacement>(HudElementStaminaBar.ModSystem.Config.StaminaBarLocation, ref location))
				{
					return StaminaBarPlacement.AboveHealth;
				}
				return location;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00003018 File Offset: 0x00001218
		public override double InputOrder
		{
			get
			{
				return 1.0;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00003023 File Offset: 0x00001223
		private static bool HideStaminaOnFull
		{
			get
			{
				JauntConfig config = HudElementStaminaBar.ModSystem.Config;
				return config == null || config.HideStaminaOnFull;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000303C File Offset: 0x0000123C
		public HudElementStaminaBar(ICoreClientAPI capi) : base(capi)
		{
			this.ComposeGuis();
			capi.Event.RegisterGameTickListener(new Action<float>(this.OnGameTick), 100, 0);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000309C File Offset: 0x0000129C
		public void OnGameTick(float dt)
		{
			if (!HudElementStaminaBar.ModSystem.Config.EnableStamina || this.capi.World.Player.WorldData.CurrentGameMode == 3)
			{
				return;
			}
			this.UpdateStamina();
			this._tickCounter++;
			if (this._tickCounter >= 2)
			{
				this.CheckFlash();
				this._tickCounter = 0;
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003102 File Offset: 0x00001302
		private void CheckFlash()
		{
			if (this._statbar == null)
			{
				return;
			}
			if (this._lastCurrentStamina < this._lastMaxStamina * 0.3f)
			{
				this._statbar.ShouldFlash = true;
				return;
			}
			this._statbar.ShouldFlash = false;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000313C File Offset: 0x0000133C
		private void UpdateStamina()
		{
			EntityPlayer entity = this.capi.World.Player.Entity;
			object obj;
			if (entity == null)
			{
				obj = null;
			}
			else
			{
				IMountableSeat mountedOn = entity.MountedOn;
				obj = ((mountedOn != null) ? mountedOn.Entity : null);
			}
			EntityAgent ea = obj as EntityAgent;
			if (ea == null)
			{
				return;
			}
			ITreeAttribute staminaTree = ea.WatchedAttributes.GetTreeAttribute(HudElementStaminaBar.ModSystem.ModId + ":stamina");
			if (staminaTree == null)
			{
				return;
			}
			if (this._statbar == null)
			{
				return;
			}
			float stamina = staminaTree.GetFloat("currentstamina", 0f);
			float maxStamina = staminaTree.GetFloat("maxstamina", 0f);
			if (stamina != this._lastCurrentStamina)
			{
				if (maxStamina != this._lastMaxStamina)
				{
					float newLineInterval = Math.Max(100f, maxStamina / 100f);
					if (Math.Abs(newLineInterval - this._lastLineInterval) > 0.001f)
					{
						this._statbar.SetLineInterval(newLineInterval);
						this._lastLineInterval = newLineInterval;
					}
				}
				this._statbar.SetValues(stamina, 0f, maxStamina);
				this._lastCurrentStamina = stamina;
				this._lastMaxStamina = maxStamina;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003240 File Offset: 0x00001440
		private void ComposeGuis()
		{
			double[] staminaBarColor = new double[]
			{
				0.85,
				0.65,
				0.0,
				0.5
			};
			ElementBounds statsBarBounds;
			EnumDialogArea dialogArea;
			double alignmentOffsetX;
			double alignmentOffsetY;
			bool isRight;
			switch (this.StaminaBarLocation)
			{
			case StaminaBarPlacement.AboveHunger:
				statsBarBounds = new ElementBounds
				{
					Alignment = 7,
					BothSizing = 0,
					fixedWidth = 850.0,
					fixedHeight = 100.0
				}.WithFixedAlignmentOffset(0.0, 5.0);
				dialogArea = 9;
				alignmentOffsetX = -2.0 + (double)this.StaminaBarXOffset;
				alignmentOffsetY = (double)(-16f + this.StaminaBarYOffset);
				isRight = true;
				goto IL_187;
			case StaminaBarPlacement.AboveTemporal:
				statsBarBounds = new ElementBounds
				{
					Alignment = 7,
					BothSizing = 0,
					fixedWidth = 850.0,
					fixedHeight = 200.0
				}.WithFixedAlignmentOffset(0.0, 5.0);
				dialogArea = 5;
				alignmentOffsetX = 0.0 + (double)this.StaminaBarXOffset;
				alignmentOffsetY = (double)(-100f + this.StaminaBarYOffset);
				isRight = false;
				goto IL_187;
			}
			statsBarBounds = new ElementBounds
			{
				Alignment = 7,
				BothSizing = 0,
				fixedWidth = 850.0,
				fixedHeight = 100.0
			}.WithFixedAlignmentOffset(0.0, 5.0);
			dialogArea = 1;
			alignmentOffsetX = 1.0 + (double)this.StaminaBarXOffset;
			alignmentOffsetY = (double)(-16f + this.StaminaBarYOffset);
			isRight = false;
			IL_187:
			ElementBounds staminaBarBounds = ElementStdBounds.Statbar(dialogArea, (double)(348.5f * this.StaminaBarWidthMultiplier)).WithFixedAlignmentOffset(alignmentOffsetX, alignmentOffsetY).WithFixedHeight(10.0);
			ElementBounds staminaBarParentBounds = statsBarBounds.FlatCopy().FixedGrow(0.0, 20.0);
			GuiComposer composer = this.capi.Gui.CreateCompo(HudElementStaminaBar.ModSystem.ModId + ":staminastatbar", staminaBarParentBounds);
			this._statbar = new GuiElementStatbar(composer.Api, staminaBarBounds, staminaBarColor, isRight, false);
			composer.BeginChildElements(statsBarBounds).AddInteractiveElement(this._statbar, HudElementStaminaBar.ModSystem.ModId + ":staminastatsbar").EndChildElements().Compose(true);
			this.Composers[HudElementStaminaBar.ModSystem.ModId + ":staminabar"] = composer;
			this.TryOpen();
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000034B9 File Offset: 0x000016B9
		public override void OnOwnPlayerDataReceived()
		{
			this.ComposeGuis();
			this.UpdateStamina();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000034C8 File Offset: 0x000016C8
		public override void OnRenderGUI(float deltaTime)
		{
			if (!HudElementStaminaBar.ModSystem.Config.EnableStamina)
			{
				return;
			}
			EntityPlayer entity = this.capi.World.Player.Entity;
			object obj;
			if (entity == null)
			{
				obj = null;
			}
			else
			{
				IMountableSeat mountedOn = entity.MountedOn;
				obj = ((mountedOn != null) ? mountedOn.Entity : null);
			}
			EntityAgent ea = obj as EntityAgent;
			if (ea == null)
			{
				return;
			}
			if (!ea.HasBehavior<EntityBehaviorJauntStamina>())
			{
				return;
			}
			ITreeAttribute staminaTree = ea.WatchedAttributes.GetTreeAttribute(HudElementStaminaBar.ModSystem.ModId + ":stamina");
			if (staminaTree == null)
			{
				return;
			}
			float stamina = staminaTree.GetFloat("currentstamina", 0f);
			float maxStamina = staminaTree.GetFloat("maxstamina", 0f);
			if (stamina == maxStamina && HudElementStaminaBar.HideStaminaOnFull)
			{
				return;
			}
			if (!HudElementStaminaBar.ModSystem.Config.EnableStamina)
			{
				return;
			}
			base.OnRenderGUI(deltaTime);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003592 File Offset: 0x00001792
		public override bool TryClose()
		{
			return false;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003595 File Offset: 0x00001795
		public override bool ShouldReceiveKeyboardEvents()
		{
			return false;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00003598 File Offset: 0x00001798
		public override bool Focusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000021 RID: 33
		private GuiElementStatbar _statbar;

		// Token: 0x04000022 RID: 34
		private int _tickCounter;

		// Token: 0x04000023 RID: 35
		private float _lastCurrentStamina = -1f;

		// Token: 0x04000024 RID: 36
		private float _lastMaxStamina = -1f;

		// Token: 0x04000025 RID: 37
		private float _lastLineInterval = -1f;

		// Token: 0x04000026 RID: 38
		public bool ShowStaminaBar = true;
	}
}
