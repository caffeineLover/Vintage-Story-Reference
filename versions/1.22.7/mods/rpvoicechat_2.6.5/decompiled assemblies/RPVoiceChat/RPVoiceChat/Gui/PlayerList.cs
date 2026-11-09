using System;
using System.Linq;
using RPVoiceChat.DB;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000090 RID: 144
	public class PlayerList : GuiElementContainer, IExtendedGuiElement
	{
		// Token: 0x060003BB RID: 955 RVA: 0x00014064 File Offset: 0x00012264
		public PlayerList(ICoreClientAPI capi, ClientSettingsRepository clientSettingsRepository, GuiDialog parrent) : base(capi, new ElementBounds())
		{
			this.settingsRepository = clientSettingsRepository;
			this.parrentDialog = parrent;
			this.playerEntryBounds = ElementBounds.Fixed(0.0, 0.0, 0.0, 20.0);
			this.UnscaledCellHorPadding = 0;
			this.unscaledCellSpacing = 0;
			this.UnscaledCellVerPadding = 0;
			this.parrentDialog.OnOpened += new Action(this.OnDialogOpened);
			this.parrentDialog.OnClosed += new Action(this.OnDialogClosed);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00014108 File Offset: 0x00012308
		public void Init(string elementKey, ElementBounds bounds, GuiComposer composer)
		{
			this.key = elementKey;
			this.Bounds = ElementBounds.Fixed(0.0, 0.0, 420.0, 0.0).WithFixedPadding(0.0, 7.5);
			this.Bounds.IsDrawingSurface = true;
			this._clipBounds = bounds.FlatCopy().WithFixedSize(420.0, 452.5).FixedGrow(10.0, 0.0);
			GuiElementClipHelpler.BeginClip(composer, this._clipBounds.FlatCopy());
		}

		// Token: 0x060003BD RID: 957 RVA: 0x000141BA File Offset: 0x000123BA
		public void OnAdd(GuiComposer composer)
		{
			GuiElementClipHelpler.EndClip(composer);
			this.scrollbar = this.CreateScrollbar();
			composer.AddInteractiveElement(this.scrollbar, null);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x000141E0 File Offset: 0x000123E0
		public void SetupElement()
		{
			if (!this.IsDisplayed())
			{
				return;
			}
			this.ResetElement();
			foreach (IPlayer player in this.api.World.AllOnlinePlayers)
			{
				this.AddPlayer(player);
			}
			this.UpdateScrollbarHeights();
			this.RedrawElement();
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00014234 File Offset: 0x00012434
		private void ResetElement()
		{
			this.Dispose();
			this.Elements.Clear();
			this.playerEntryBounds = ElementBounds.Fixed(0.0, 0.0, 0.0, 20.0);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00014284 File Offset: 0x00012484
		private void RedrawElement()
		{
			this.HideScrollbar();
			if ((double)Enumerable.Count<GuiElement>(this.Elements, (GuiElement e) => e is NamedSlider) > 15.0)
			{
				this.ShowScrollbar();
			}
			this.parrentDialog.SingleComposer.ReCompose();
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000142E4 File Offset: 0x000124E4
		private void AddPlayer(IPlayer player)
		{
			string playerName = UIUtils.TrimText(player.PlayerName, 200.0, this.font);
			string playerId = player.PlayerUID;
			string sliderKey = playerId + "_volume";
			if (this.Elements.Exists(delegate(GuiElement e)
			{
				NamedSlider slider = e as NamedSlider;
				return slider != null && slider.name == sliderKey;
			}))
			{
				return;
			}
			ElementBounds nameBounds = this.playerEntryBounds.FlatCopy().WithFixedWidth(200.0).WithFixedOffset(0.0, 2.0);
			ElementBounds volumeBounds = nameBounds.RightCopy(20.0, 0.0, 0.0, 0.0).WithFixedWidth(200.0);
			GuiElementStaticText nameLabel = new GuiElementStaticText(this.api, playerName, this.font.Orientation, nameBounds, this.font);
			NamedSlider volumeSlider = new NamedSlider(this.api, sliderKey, new ActionConsumable<int, string>(this.SlidePlayerVolume), volumeBounds);
			float gain = this.settingsRepository.GetPlayerGain(playerId);
			volumeSlider.SetValues((int)(gain * 100f), 0, 100, 1, "%");
			base.Add(nameLabel, -1);
			base.Add(volumeSlider, -1);
			this.playerEntryBounds = this.playerEntryBounds.BelowCopy(0.0, 10.0, 0.0, 0.0);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00014460 File Offset: 0x00012660
		private bool SlidePlayerVolume(int gain, string sliderKey)
		{
			string playerId = sliderKey.Split("_", 0)[0];
			this.settingsRepository.SetPlayerGain(playerId, gain);
			return true;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0001448C File Offset: 0x0001268C
		private GuiElementScrollbar CreateScrollbar()
		{
			ElementBounds scrollbarBounds = this.InsideClipBounds.RightCopy(0.0, 0.0, 0.0, 0.0).WithParent(null).WithFixedSize(5.0, 448.5).WithFixedPadding(0.0, 2.0);
			return new GuiElementScrollbar(this.api, new Action<float>(this.OnNewScrollbarValue), scrollbarBounds);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00014515 File Offset: 0x00012715
		private void OnNewScrollbarValue(float value)
		{
			this.Bounds.fixedY = (double)(0f - value);
			this.Bounds.CalcWorldBounds();
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00014538 File Offset: 0x00012738
		private void UpdateScrollbarHeights()
		{
			if (this.scrollbar == null)
			{
				return;
			}
			this.scrollbar.Bounds.CalcWorldBounds();
			int playerEntryCount = Enumerable.Count<GuiElement>(this.Elements, (GuiElement e) => e is NamedSlider);
			int visibleHeight = 452;
			int totalHeight = (int)(30.0 * (double)playerEntryCount + 7.5);
			this.scrollbar.SetHeights((float)visibleHeight, (float)totalHeight);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x000145B8 File Offset: 0x000127B8
		private void HideScrollbar()
		{
			this.InsideClipBounds.WithFixedSize(this._clipBounds.fixedWidth + this.scrollbar.Bounds.OuterWidth, this._clipBounds.fixedHeight + 6.0);
			this.scrollbar.Bounds.fixedY = -100000.0;
			this.InsideClipBounds.WithFixedPadding(0.0, 0.0);
			this.Bounds.fixedOffsetY = 0.0;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00014650 File Offset: 0x00012850
		private void ShowScrollbar()
		{
			this.InsideClipBounds.WithFixedSize(this._clipBounds.fixedWidth, this._clipBounds.fixedHeight);
			this.scrollbar.Bounds.fixedY = this._clipBounds.fixedY;
			this.InsideClipBounds.WithFixedPadding(0.0, 3.0);
			this.Bounds.fixedOffsetY = -3.0;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x000146CC File Offset: 0x000128CC
		private void OnDialogOpened()
		{
			this.api.Event.PlayerJoin += new PlayerEventDelegate(this.OnPlayerJoin);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x000146EA File Offset: 0x000128EA
		private void OnDialogClosed()
		{
			this.api.Event.PlayerJoin -= new PlayerEventDelegate(this.OnPlayerJoin);
			this.settingsRepository.Save();
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00014713 File Offset: 0x00012913
		private void OnPlayerJoin(IPlayer player)
		{
			if (!this.IsDisplayed())
			{
				return;
			}
			this.AddPlayer(player);
			this.UpdateScrollbarHeights();
			this.RedrawElement();
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00014731 File Offset: 0x00012931
		private bool IsDisplayed()
		{
			return this.parrentDialog.IsOpened() && this.key != null && this.parrentDialog.SingleComposer.GetElement(this.key) != null;
		}

		// Token: 0x040001E2 RID: 482
		private const double playerListWidth = 420.0;

		// Token: 0x040001E3 RID: 483
		private const double playerListVisibleHeight = 452.5;

		// Token: 0x040001E4 RID: 484
		private const double playerEntryHeight = 20.0;

		// Token: 0x040001E5 RID: 485
		private const double playerEntryDeltaY = 10.0;

		// Token: 0x040001E6 RID: 486
		private const double playerEntryHeightWithOffset = 30.0;

		// Token: 0x040001E7 RID: 487
		private const double _playerEntriesYPadding = 7.5;

		// Token: 0x040001E8 RID: 488
		private const double _playerEntriesYOffset = 2.0;

		// Token: 0x040001E9 RID: 489
		private const double maxPlayerEntriesOnScreen = 15.0;

		// Token: 0x040001EA RID: 490
		private const double playerNameWidth = 200.0;

		// Token: 0x040001EB RID: 491
		private const double playerVolumeLeftPadding = 20.0;

		// Token: 0x040001EC RID: 492
		private const double playerVolumeWidth = 200.0;

		// Token: 0x040001ED RID: 493
		private const double scrollbarYPadding = 2.0;

		// Token: 0x040001EE RID: 494
		private const double scrollbarLeftPadding = 10.0;

		// Token: 0x040001EF RID: 495
		private const double scrollbarWidth = 5.0;

		// Token: 0x040001F0 RID: 496
		private const double scrollbarHeight = 448.5;

		// Token: 0x040001F1 RID: 497
		private const double _fixOOSSliderHandleClip = 2.5;

		// Token: 0x040001F2 RID: 498
		private const double _fixHSBSliderHandleClip = 6.0;

		// Token: 0x040001F3 RID: 499
		private CairoFont font = CairoFont.WhiteSmallText();

		// Token: 0x040001F4 RID: 500
		private ClientSettingsRepository settingsRepository;

		// Token: 0x040001F5 RID: 501
		private GuiDialog parrentDialog;

		// Token: 0x040001F6 RID: 502
		private ElementBounds playerEntryBounds;

		// Token: 0x040001F7 RID: 503
		private GuiElementScrollbar scrollbar;

		// Token: 0x040001F8 RID: 504
		private string key;

		// Token: 0x040001F9 RID: 505
		private ElementBounds _clipBounds;
	}
}
