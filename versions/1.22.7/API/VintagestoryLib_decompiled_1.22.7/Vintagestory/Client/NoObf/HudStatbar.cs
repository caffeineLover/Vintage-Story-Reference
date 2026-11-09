using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.Client.NoObf;

public class HudStatbar : HudElement
{
	private float lastHealth;

	private float lastMaxHealth;

	private float lastOxygen;

	private float lastMaxOxygen;

	private float lastSaturation;

	private float lastMaxSaturation;

	private GuiElementStatbar healthbar;

	private GuiElementStatbar oxygenbar;

	private GuiElementStatbar saturationbar;

	private float lastPreviousHealthValue;

	private float? lastFutureHealth;

	private float lastHealthChangeVelocity;

	private long previousHealthHasChangedTotalMs;

	public override double InputOrder => 1.0;

	public override string ToggleKeyCombinationCode => null;

	public override bool Focusable => false;

	public HudStatbar(ICoreClientAPI capi)
		: base(capi)
	{
		capi.Event.RegisterGameTickListener(OnGameTick, 20);
		capi.Event.RegisterGameTickListener(OnFlashStatbars, 2500);
	}

	private void OnGameTick(float dt)
	{
		UpdateHealth();
		UpdateOxygen();
		UpdateSaturation();
	}

	private void OnFlashStatbars(float dt)
	{
		EnumGameMode gm = capi.World.Player.WorldData.CurrentGameMode;
		bool flash = gm != EnumGameMode.Creative && gm != EnumGameMode.Spectator;
		ITreeAttribute healthTree = capi.World.Player.Entity.WatchedAttributes.GetTreeAttribute("health");
		if (healthTree != null && healthbar != null && (double?)(healthTree.TryGetFloat("currenthealth") / healthTree.TryGetFloat("maxhealth")) < 0.2)
		{
			healthbar.ShouldFlash = flash;
		}
		ITreeAttribute hungerTree = capi.World.Player.Entity.WatchedAttributes.GetTreeAttribute("hunger");
		if (hungerTree != null && saturationbar != null && (double?)(hungerTree.TryGetFloat("currentsaturation") / hungerTree.TryGetFloat("maxsaturation")) < 0.2)
		{
			saturationbar.ShouldFlash = flash;
		}
		if (capi.World.Player.Entity.WatchedAttributes.GetTreeAttribute("oxygen") != null && (double?)(hungerTree.TryGetFloat("currentoxygen") / hungerTree.TryGetFloat("maxoxygen")) < 0.2)
		{
			saturationbar.ShouldFlash = flash;
		}
	}

	private void UpdateHealth()
	{
		ITreeAttribute healthTree = capi.World.Player.Entity.WatchedAttributes.GetTreeAttribute("health");
		if (healthTree == null)
		{
			return;
		}
		float? health = healthTree.TryGetFloat("currenthealth");
		float? futureHealth = healthTree.TryGetFloat("futureHealth");
		float previousHealthValue = healthTree.GetFloat("previousHealthValue");
		float healthChangeVelocity = healthTree.GetFloat("healthChangeVelocity");
		float? maxHealth = healthTree.TryGetFloat("maxhealth");
		if (!health.HasValue || !maxHealth.HasValue || (lastHealth == health && lastMaxHealth == maxHealth && previousHealthValue == lastPreviousHealthValue && lastHealthChangeVelocity == healthChangeVelocity && lastFutureHealth == futureHealth) || healthbar == null)
		{
			return;
		}
		bool num = (double)Math.Abs(lastPreviousHealthValue - previousHealthValue) > 0.01;
		float? prevHealthValueDisplay = previousHealthValue;
		if (num)
		{
			if (capi.InWorldEllapsedMilliseconds - previousHealthHasChangedTotalMs < 2000)
			{
				prevHealthValueDisplay = lastPreviousHealthValue;
			}
			previousHealthHasChangedTotalMs = capi.InWorldEllapsedMilliseconds;
		}
		healthbar.SetFutureValues(futureHealth, healthChangeVelocity);
		healthbar.SetPrevValue(prevHealthValueDisplay, previousHealthHasChangedTotalMs, () => capi.InWorldEllapsedMilliseconds);
		healthbar.SetLineInterval(1f);
		healthbar.SetValues(health.Value, 0f, maxHealth.Value);
		lastHealth = health.Value;
		lastMaxHealth = maxHealth.Value;
		lastFutureHealth = futureHealth;
		lastHealthChangeVelocity = healthChangeVelocity;
		lastPreviousHealthValue = previousHealthValue;
	}

	private void UpdateOxygen()
	{
		ITreeAttribute oxygenTree = capi.World.Player.Entity.WatchedAttributes.GetTreeAttribute("oxygen");
		if (oxygenTree != null)
		{
			float? oxygen = oxygenTree.TryGetFloat("currentoxygen");
			float? maxOxygen = oxygenTree.TryGetFloat("maxoxygen");
			if (oxygen.HasValue && maxOxygen.HasValue && (lastOxygen != oxygen || lastMaxOxygen != maxOxygen) && oxygenbar != null)
			{
				oxygenbar.SetLineInterval(1000f);
				oxygenbar.SetValues(oxygen.Value, 0f, maxOxygen.Value);
				lastOxygen = oxygen.Value;
				lastMaxOxygen = maxOxygen.Value;
			}
		}
	}

	private void UpdateSaturation()
	{
		ITreeAttribute hungerTree = capi.World.Player.Entity.WatchedAttributes.GetTreeAttribute("hunger");
		if (hungerTree != null)
		{
			float? saturation = hungerTree.TryGetFloat("currentsaturation");
			float? maxSaturation = hungerTree.TryGetFloat("maxsaturation");
			if (saturation.HasValue && maxSaturation.HasValue && (lastSaturation != saturation || lastMaxSaturation != maxSaturation) && saturationbar != null)
			{
				saturationbar.SetLineInterval(100f);
				saturationbar.SetValues(saturation.Value, 0f, maxSaturation.Value);
				lastSaturation = saturation.Value;
				lastMaxSaturation = maxSaturation.Value;
			}
		}
	}

	public override void OnOwnPlayerDataReceived()
	{
		ComposeGuis();
		UpdateSaturation();
	}

	public void ComposeGuis()
	{
		float width = 850f;
		ElementBounds dialogBounds = new ElementBounds
		{
			Alignment = EnumDialogArea.CenterBottom,
			BothSizing = ElementSizing.Fixed,
			fixedWidth = width,
			fixedHeight = 100.0
		}.WithFixedAlignmentOffset(0.0, 5.0);
		ElementBounds healthBarBounds = ElementStdBounds.Statbar(EnumDialogArea.LeftTop, (double)width * 0.41).WithFixedHeight(10.0).WithFixedAlignmentOffset(0.0, 5.0);
		healthBarBounds.WithFixedHeight(10.0);
		ElementBounds oxygenBarBounds = ElementStdBounds.Statbar(EnumDialogArea.LeftTop, (double)width * 0.41).WithFixedHeight(10.0).WithFixedAlignmentOffset(0.0, -15.0);
		oxygenBarBounds.WithFixedHeight(10.0);
		ElementBounds foodBarBounds = ElementStdBounds.Statbar(EnumDialogArea.RightTop, (double)width * 0.41).WithFixedHeight(10.0).WithFixedAlignmentOffset(-1.0, 5.0);
		foodBarBounds.WithFixedHeight(10.0);
		ITreeAttribute hungerTree = capi.World.Player.Entity.WatchedAttributes.GetTreeAttribute("hunger");
		ITreeAttribute healthTree = capi.World.Player.Entity.WatchedAttributes.GetTreeAttribute("health");
		ITreeAttribute oxygenTree = capi.World.Player.Entity.WatchedAttributes.GetTreeAttribute("oxygen");
		Composers["statbar"] = capi.Gui.CreateCompo("inventory-statbar", dialogBounds.FlatCopy().FixedGrow(0.0, 20.0)).BeginChildElements(dialogBounds).AddIf(healthTree != null)
			.AddStatbar(healthBarBounds, GuiStyle.HealthBarColor, "healthstatbar")
			.EndIf()
			.AddIf(oxygenTree != null)
			.AddStatbar(oxygenBarBounds, GuiStyle.OxygenBarColor, hideable: true, "oxygenstatbar")
			.EndIf()
			.AddIf(hungerTree != null)
			.AddInvStatbar(foodBarBounds, GuiStyle.FoodBarColor, "saturationstatbar")
			.EndIf()
			.EndChildElements()
			.Compose();
		healthbar = Composers["statbar"].GetStatbar("healthstatbar");
		oxygenbar = Composers["statbar"].GetStatbar("oxygenstatbar");
		oxygenbar.HideWhenFull = true;
		saturationbar = Composers["statbar"].GetStatbar("saturationstatbar");
		TryOpen();
	}

	public override bool TryClose()
	{
		return false;
	}

	public override bool ShouldReceiveKeyboardEvents()
	{
		return false;
	}

	public override void OnRenderGUI(float deltaTime)
	{
		if (capi.World.Player.WorldData.CurrentGameMode != EnumGameMode.Spectator)
		{
			base.OnRenderGUI(deltaTime);
		}
	}

	protected override void OnFocusChanged(bool on)
	{
	}

	public override void OnMouseDown(MouseEvent args)
	{
	}
}
