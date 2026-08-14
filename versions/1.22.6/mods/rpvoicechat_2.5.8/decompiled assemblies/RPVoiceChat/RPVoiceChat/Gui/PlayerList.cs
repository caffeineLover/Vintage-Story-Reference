using System;
using System.Linq;
using RPVoiceChat.DB;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200006D RID: 109
	public class PlayerList : GuiElementContainer, IExtendedGuiElement
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x0000FFBC File Offset: 0x0000E1BC
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

		// Token: 0x060002FA RID: 762 RVA: 0x00010060 File Offset: 0x0000E260
		public void Init(string elementKey, ElementBounds bounds, GuiComposer composer)
		{
			this.key = elementKey;
			this.Bounds = ElementBounds.Fixed(0.0, 0.0, 420.0, 0.0).WithFixedPadding(0.0, 7.5);
			this.Bounds.IsDrawingSurface = true;
			this._clipBounds = bounds.FlatCopy().WithFixedSize(420.0, 452.5).FixedGrow(10.0, 0.0);
			GuiElementClipHelpler.BeginClip(composer, this._clipBounds.FlatCopy());
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00010112 File Offset: 0x0000E312
		public void OnAdd(GuiComposer composer)
		{
			GuiElementClipHelpler.EndClip(composer);
			this.scrollbar = this.CreateScrollbar();
			composer.AddInteractiveElement(this.scrollbar, null);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00010138 File Offset: 0x0000E338
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

		// Token: 0x060002FD RID: 765 RVA: 0x0001018C File Offset: 0x0000E38C
		private void ResetElement()
		{
			this.Dispose();
			this.Elements.Clear();
			this.playerEntryBounds = ElementBounds.Fixed(0.0, 0.0, 0.0, 20.0);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x000101DC File Offset: 0x0000E3DC
		private void RedrawElement()
		{
			this.HideScrollbar();
			if ((double)Enumerable.Count<GuiElement>(this.Elements, (GuiElement e) => e is NamedSlider) > 15.0)
			{
				this.ShowScrollbar();
			}
			this.parrentDialog.SingleComposer.ReCompose();
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0001023C File Offset: 0x0000E43C
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

		// Token: 0x06000300 RID: 768 RVA: 0x000103B8 File Offset: 0x0000E5B8
		private bool SlidePlayerVolume(int gain, string sliderKey)
		{
			string playerId = sliderKey.Split("_", 0)[0];
			this.settingsRepository.SetPlayerGain(playerId, gain);
			return true;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x000103E4 File Offset: 0x0000E5E4
		private GuiElementScrollbar CreateScrollbar()
		{
			ElementBounds scrollbarBounds = this.InsideClipBounds.RightCopy(0.0, 0.0, 0.0, 0.0).WithParent(null).WithFixedSize(5.0, 448.5).WithFixedPadding(0.0, 2.0);
			return new GuiElementScrollbar(this.api, new Action<float>(this.OnNewScrollbarValue), scrollbarBounds);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0001046D File Offset: 0x0000E66D
		private void OnNewScrollbarValue(float value)
		{
			this.Bounds.fixedY = (double)(0f - value);
			this.Bounds.CalcWorldBounds();
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00010490 File Offset: 0x0000E690
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

		// Token: 0x06000304 RID: 772 RVA: 0x00010510 File Offset: 0x0000E710
		private void HideScrollbar()
		{
			this.InsideClipBounds.WithFixedSize(this._clipBounds.fixedWidth + this.scrollbar.Bounds.OuterWidth, this._clipBounds.fixedHeight + 6.0);
			this.scrollbar.Bounds.fixedY = -100000.0;
			this.InsideClipBounds.WithFixedPadding(0.0, 0.0);
			this.Bounds.fixedOffsetY = 0.0;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000105A8 File Offset: 0x0000E7A8
		private void ShowScrollbar()
		{
			this.InsideClipBounds.WithFixedSize(this._clipBounds.fixedWidth, this._clipBounds.fixedHeight);
			this.scrollbar.Bounds.fixedY = this._clipBounds.fixedY;
			this.InsideClipBounds.WithFixedPadding(0.0, 3.0);
			this.Bounds.fixedOffsetY = -3.0;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00010624 File Offset: 0x0000E824
		private void OnDialogOpened()
		{
			this.api.Event.PlayerJoin += new PlayerEventDelegate(this.OnPlayerJoin);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00010642 File Offset: 0x0000E842
		private void OnDialogClosed()
		{
			this.api.Event.PlayerJoin -= new PlayerEventDelegate(this.OnPlayerJoin);
			this.settingsRepository.Save();
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0001066B File Offset: 0x0000E86B
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

		// Token: 0x06000309 RID: 777 RVA: 0x00010689 File Offset: 0x0000E889
		private bool IsDisplayed()
		{
			return this.parrentDialog.IsOpened() && this.key != null && this.parrentDialog.SingleComposer.GetElement(this.key) != null;
		}

		// Token: 0x04000171 RID: 369
		private const double playerListWidth = 420.0;

		// Token: 0x04000172 RID: 370
		private const double playerListVisibleHeight = 452.5;

		// Token: 0x04000173 RID: 371
		private const double playerEntryHeight = 20.0;

		// Token: 0x04000174 RID: 372
		private const double playerEntryDeltaY = 10.0;

		// Token: 0x04000175 RID: 373
		private const double playerEntryHeightWithOffset = 30.0;

		// Token: 0x04000176 RID: 374
		private const double _playerEntriesYPadding = 7.5;

		// Token: 0x04000177 RID: 375
		private const double _playerEntriesYOffset = 2.0;

		// Token: 0x04000178 RID: 376
		private const double maxPlayerEntriesOnScreen = 15.0;

		// Token: 0x04000179 RID: 377
		private const double playerNameWidth = 200.0;

		// Token: 0x0400017A RID: 378
		private const double playerVolumeLeftPadding = 20.0;

		// Token: 0x0400017B RID: 379
		private const double playerVolumeWidth = 200.0;

		// Token: 0x0400017C RID: 380
		private const double scrollbarYPadding = 2.0;

		// Token: 0x0400017D RID: 381
		private const double scrollbarLeftPadding = 10.0;

		// Token: 0x0400017E RID: 382
		private const double scrollbarWidth = 5.0;

		// Token: 0x0400017F RID: 383
		private const double scrollbarHeight = 448.5;

		// Token: 0x04000180 RID: 384
		private const double _fixOOSSliderHandleClip = 2.5;

		// Token: 0x04000181 RID: 385
		private const double _fixHSBSliderHandleClip = 6.0;

		// Token: 0x04000182 RID: 386
		private CairoFont font = CairoFont.WhiteSmallText();

		// Token: 0x04000183 RID: 387
		private ClientSettingsRepository settingsRepository;

		// Token: 0x04000184 RID: 388
		private GuiDialog parrentDialog;

		// Token: 0x04000185 RID: 389
		private ElementBounds playerEntryBounds;

		// Token: 0x04000186 RID: 390
		private GuiElementScrollbar scrollbar;

		// Token: 0x04000187 RID: 391
		private string key;

		// Token: 0x04000188 RID: 392
		private ElementBounds _clipBounds;
	}
}
