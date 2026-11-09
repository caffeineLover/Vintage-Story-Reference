using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class GuiDialogTrader : GuiDialog
{
	private InventoryTrader traderInventory;

	private EntityAgent owningEntity;

	private double prevPlrAbsFixedX;

	private double prevPlrAbsFixedY;

	private double prevTdrAbsFixedX;

	private double prevTdrAbsFixedY;

	private double notifyPlayerMoneyTextSeconds;

	private double notifyTraderMoneyTextSeconds;

	private int rows = 4;

	private int cols = 4;

	private int curTab;

	private string currentSearchText = string.Empty;

	private int currentPriceMin;

	private int currentPriceMax = 10000;

	private ModSystemAuction auctionSys;

	private InventoryGeneric auctionSlotInv;

	private GuiElementCellList<Auction> listElem;

	private List<Auction> auctions;

	private ElementBounds clipBounds;

	private AuctionCellEntry selectedElem;

	public override string ToggleKeyCombinationCode => null;

	private bool auctionHouseEnabled => capi.World.Config.GetBool("auctionHouse", defaultValue: true);

	public override bool PrefersUngrabbedMouse => false;

	public override float ZSize => 300f;

	public GuiDialogTrader(InventoryTrader traderInventory, EntityAgent owningEntity, ICoreClientAPI capi, int rows = 4, int cols = 4)
		: base(capi)
	{
		auctionSys = capi.ModLoader.GetModSystem<ModSystemAuction>();
		auctionSys.OnCellUpdateClient = delegate
		{
			listElem?.ReloadCells(auctions);
			ApplySearchFilters();
			updateScrollbarBounds();
		};
		auctionSys.curTraderClient = owningEntity as EntityTradingHumanoid;
		this.traderInventory = traderInventory;
		this.owningEntity = owningEntity;
		this.rows = rows;
		this.cols = cols;
		if (!auctionSys.createAuctionSlotByPlayer.TryGetValue(capi.World.Player.PlayerUID, out auctionSlotInv))
		{
			auctionSys.createAuctionSlotByPlayer[capi.World.Player.PlayerUID] = (auctionSlotInv = new InventoryGeneric(1, "auctionslot-" + capi.World.Player.PlayerUID, capi, (int idx, InventoryGeneric inv) => new ItemSlotAuction(inv)));
			auctionSlotInv.OnGetSuitability = (ItemSlot s, ItemSlot t, bool isMerge) => -1f;
		}
		capi.Network.SendPacketClient(capi.World.Player.InventoryManager.OpenInventory(auctionSlotInv));
		Compose();
	}

	public void Compose()
	{
		GuiTab[] tabs = new GuiTab[3]
		{
			new GuiTab
			{
				Name = Lang.Get("Local goods"),
				DataInt = 0
			},
			new GuiTab
			{
				Name = Lang.Get("Auction house"),
				DataInt = 1
			},
			new GuiTab
			{
				Name = Lang.Get("Your Auctions"),
				DataInt = 2
			}
		};
		ElementBounds tabBounds = ElementBounds.Fixed(0.0, -24.0, 400.0, 25.0);
		CairoFont tabFont = CairoFont.WhiteDetailText();
		if (!auctionHouseEnabled)
		{
			tabs = new GuiTab[1]
			{
				new GuiTab
				{
					Name = Lang.Get("Local goods"),
					DataInt = 0
				}
			};
		}
		ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
		bgBounds.BothSizing = ElementSizing.FitToChildren;
		ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.RightMiddle).WithFixedAlignmentOffset(0.0 - GuiStyle.DialogToScreenPadding, 0.0);
		ElementBounds leftButton = ElementBounds.Fixed(EnumDialogArea.LeftFixed, 0.0, 0.0, 0.0, 0.0).WithFixedPadding(8.0, 5.0);
		ElementBounds rightButton = ElementBounds.Fixed(EnumDialogArea.RightFixed, 0.0, 0.0, 0.0, 0.0).WithFixedPadding(8.0, 5.0);
		string traderName = owningEntity.GetBehavior<EntityBehaviorNameTag>().DisplayName;
		string dlgTitle = Lang.GetMatching("tradingwindow-" + owningEntity.Code.Path, traderName);
		if (curTab > 0)
		{
			dlgTitle = Lang.Get("tradertabtitle-" + curTab);
		}
		base.SingleComposer = capi.Gui.CreateCompo("traderdialog-" + owningEntity.EntityId, dialogBounds).AddShadedDialogBG(bgBounds).AddDialogTitleBar(dlgTitle, OnTitleBarClose)
			.AddHorizontalTabs(tabs, tabBounds, OnTabClicked, tabFont, tabFont.Clone().WithColor(GuiStyle.ActiveButtonTextColor), "tabs")
			.BeginChildElements(bgBounds);
		base.SingleComposer.GetHorizontalTabs("tabs").activeElement = curTab;
		if (curTab == 0)
		{
			double pad = GuiElementItemSlotGridBase.unscaledSlotPadding;
			ElementBounds leftTopSlotBounds = ElementStdBounds.SlotGrid(EnumDialogArea.None, pad, 70.0 + pad, cols, rows).FixedGrow(2.0 * pad, 2.0 * pad);
			ElementBounds rightTopSlotBounds = ElementStdBounds.SlotGrid(EnumDialogArea.None, pad + leftTopSlotBounds.fixedWidth + 20.0, 70.0 + pad, cols, rows).FixedGrow(2.0 * pad, 2.0 * pad);
			ElementBounds rightBotSlotBounds = ElementStdBounds.SlotGrid(EnumDialogArea.None, pad + leftTopSlotBounds.fixedWidth + 20.0, 15.0 + pad, cols, 1).FixedGrow(2.0 * pad, 2.0 * pad).FixedUnder(rightTopSlotBounds, 5.0);
			ElementBounds leftBotSlotBounds = ElementStdBounds.SlotGrid(EnumDialogArea.None, pad, 15.0 + pad, cols, 1).FixedGrow(2.0 * pad, 2.0 * pad).FixedUnder(leftTopSlotBounds, 5.0);
			ElementBounds costTextBounds = ElementBounds.Fixed(pad, 85.0 + 2.0 * pad + leftTopSlotBounds.fixedHeight + leftBotSlotBounds.fixedHeight, 200.0, 25.0);
			ElementBounds offerTextBounds = ElementBounds.Fixed(leftTopSlotBounds.fixedWidth + pad + 20.0, 85.0 + 2.0 * pad + leftTopSlotBounds.fixedHeight + leftBotSlotBounds.fixedHeight, 200.0, 25.0);
			ElementBounds traderMoneyBounds = offerTextBounds.FlatCopy().WithFixedOffset(0.0, offerTextBounds.fixedHeight);
			ElementBounds playerMoneyBounds = costTextBounds.FlatCopy().WithFixedOffset(0.0, costTextBounds.fixedHeight);
			CairoFont deliveryTextFont = CairoFont.WhiteDetailText();
			deliveryTextFont.Color[3] *= 0.7;
			base.SingleComposer.AddStaticText(Lang.Get("trader-newgoodsdelivery", (owningEntity as EntityTradingHumanoid).NextRefreshTotalDays()), deliveryTextFont, ElementBounds.Fixed(pad, 20.0 + pad, 430.0, 25.0)).AddStaticText(Lang.Get("You can Buy"), CairoFont.WhiteDetailText(), ElementBounds.Fixed(pad, 50.0 + pad, 200.0, 25.0)).AddStaticText(Lang.Get("You can Sell"), CairoFont.WhiteDetailText(), ElementBounds.Fixed(leftTopSlotBounds.fixedWidth + pad + 20.0, 50.0 + pad, 200.0, 25.0))
				.AddItemSlotGrid(traderInventory, DoSendPacket, cols, new int[rows * cols].Fill((int i) => i), leftTopSlotBounds, "traderSellingSlots")
				.AddItemSlotGrid(traderInventory, DoSendPacket, cols, new int[cols].Fill((int i) => rows * cols + i), leftBotSlotBounds, "playerBuyingSlots")
				.AddItemSlotGrid(traderInventory, DoSendPacket, cols, new int[rows * cols].Fill((int i) => rows * cols + cols + i), rightTopSlotBounds, "traderBuyingSlots")
				.AddItemSlotGrid(traderInventory, DoSendPacket, cols, new int[cols].Fill((int i) => rows * cols + cols + rows * cols + i), rightBotSlotBounds, "playerSellingSlots")
				.AddStaticText(Lang.Get("trader-yourselection"), CairoFont.WhiteDetailText(), ElementBounds.Fixed(pad, 70.0 + 2.0 * pad + leftTopSlotBounds.fixedHeight, 150.0, 25.0))
				.AddStaticText(Lang.Get("trader-youroffer"), CairoFont.WhiteDetailText(), ElementBounds.Fixed(leftTopSlotBounds.fixedWidth + pad + 20.0, 70.0 + 2.0 * pad + leftTopSlotBounds.fixedHeight, 150.0, 25.0))
				.AddDynamicText("", CairoFont.WhiteDetailText(), costTextBounds, "costText")
				.AddDynamicText("", CairoFont.WhiteDetailText(), playerMoneyBounds, "playerMoneyText")
				.AddDynamicText("", CairoFont.WhiteDetailText(), offerTextBounds, "gainText")
				.AddDynamicText("", CairoFont.WhiteDetailText(), traderMoneyBounds, "traderMoneyText")
				.AddSmallButton(Lang.Get("Goodbye!"), OnByeClicked, leftButton.FixedUnder(playerMoneyBounds, 20.0))
				.AddSmallButton(Lang.Get("Buy / Sell"), OnBuySellClicked, rightButton.FixedUnder(traderMoneyBounds, 20.0), EnumButtonStyle.Normal, "buysellButton")
				.EndChildElements()
				.Compose();
			base.SingleComposer.GetButton("buysellButton").Enabled = false;
			CalcAndUpdateAssetsDisplay();
			return;
		}
		ElementBounds stackListBounds = ElementBounds.Fixed(0.0, 25.0, 770.0, 377.0);
		List<Auction> list = ((curTab != 2) ? auctionSys.activeAuctions : auctionSys.ownAuctions);
		auctions = list;
		bool showSearch = auctions.Count > capi.Settings.Int["auctionSearchbarThreshold"];
		ElementBounds searchFieldBounds = ElementBounds.Fixed(0.0, 25.0, 280.0, 30.0);
		double minTextSize = CairoFont.WhiteSmallText().GetTextExtents(Lang.Get("auction-search-pricemin")).Width / (double)RuntimeEnv.GUIScale;
		double maxTextSize = CairoFont.WhiteSmallText().GetTextExtents(Lang.Get("auction-search-pricemax")).Width / (double)RuntimeEnv.GUIScale;
		ElementBounds minTextBounds = ElementBounds.Fixed(0.0, 25.0, 16.0 + minTextSize, 30.0).RightOf(searchFieldBounds).WithFixedOffset(10.0, 3.0);
		ElementBounds priceMin = ElementBounds.Fixed(0.0, 25.0, 80.0, 30.0).RightOf(minTextBounds);
		ElementBounds minGearIcon = ElementBounds.Fixed(0.0, 30.0, 20.0, 30.0).RightOf(priceMin).WithFixedOffset(3.0, 0.0);
		ElementBounds maxTextBounds = ElementBounds.Fixed(0.0, 25.0, 16.0 + maxTextSize, 30.0).RightOf(minGearIcon).WithFixedOffset(25.0, 3.0);
		ElementBounds priceMax = ElementBounds.Fixed(0.0, 25.0, 80.0, 30.0).RightOf(maxTextBounds).WithFixedOffset(0.0, 0.0);
		ElementBounds maxGearIcon = ElementBounds.Fixed(0.0, 30.0, 20.0, 30.0).RightOf(priceMax).WithFixedOffset(3.0, 0.0);
		ElementBounds clearBounds = ElementBounds.Fixed(0.0, 25.0, 30.0, 30.0).RightOf(maxGearIcon).WithFixedOffset(25.0, -2.0);
		if (showSearch && (curTab == 1 || curTab == 2))
		{
			stackListBounds = stackListBounds.FixedUnder(searchFieldBounds).WithFixedOffset(0.0, -10.0);
			double searchbarWidth = searchFieldBounds.fixedWidth + minTextBounds.fixedWidth + priceMin.fixedWidth + maxTextBounds.fixedWidth + priceMax.fixedWidth + maxGearIcon.fixedWidth + clearBounds.fixedWidth;
			stackListBounds.fixedWidth = Math.Max(stackListBounds.fixedWidth, searchbarWidth);
		}
		clipBounds = stackListBounds.ForkBoundingParent();
		ElementBounds insetBounds = stackListBounds.FlatCopy();
		ElementBounds scrollbarBounds = insetBounds.CopyOffsetedSibling(3.0 + stackListBounds.fixedWidth + 7.0).WithFixedWidth(20.0);
		ItemStack gearStack = capi.ModLoader.GetModSystem<ModSystemAuction>().SingleCurrencyStack;
		double fl = CairoFont.WhiteDetailText().UnscaledFontsize;
		RichTextComponentBase[] gearIcon = new RichTextComponentBase[1]
		{
			new ItemstackTextComponent(capi, gearStack, fl * 3.0, 0.0, EnumFloat.Inline)
			{
				VerticalAlign = EnumVerticalAlign.Top,
				offX = 0.0 - GuiElement.scaled(fl * 0.5),
				offY = 0.0 - GuiElement.scaled(fl * 0.75)
			}
		};
		if (curTab == 1)
		{
			base.SingleComposer.AddIf(showSearch).AddTextInput(searchFieldBounds, OnSearchTextChanged, CairoFont.WhiteSmallishText(), "searchField").AddStaticText(Lang.Get("auction-search-pricemin"), CairoFont.WhiteSmallText(), minTextBounds)
				.AddNumberInput(priceMin, OnPriceMinChange, CairoFont.WhiteSmallishText(), "priceMin")
				.AddRichtext(gearIcon, minGearIcon)
				.AddRichtext(Lang.Get("auction-search-pricemax"), CairoFont.WhiteSmallText(), maxTextBounds)
				.AddNumberInput(priceMax, OnPriceMaxChange, CairoFont.WhiteSmallishText(), "priceMax")
				.AddRichtext(gearIcon, maxGearIcon)
				.AddButton("X", OnClear, clearBounds, EnumButtonStyle.Normal, "clearBtn")
				.AddHoverText(Lang.Get("auction-search-reset"), CairoFont.WhiteSmallText(), 150, clearBounds, "clearBtnHover")
				.EndIf()
				.BeginClip(clipBounds)
				.AddInset(insetBounds, 3)
				.AddCellList(stackListBounds, createCell, auctions, "stacklist")
				.EndClip()
				.AddVerticalScrollbar(OnNewScrollbarValue, scrollbarBounds, "scrollbar")
				.AddSmallButton(Lang.Get("Goodbye!"), OnByeClicked, leftButton.FixedUnder(clipBounds, 20.0))
				.AddSmallButton(Lang.Get("Buy"), OnBuyAuctionClicked, rightButton.FixedUnder(clipBounds, 20.0), EnumButtonStyle.Normal, "buyauction");
		}
		if (curTab == 2)
		{
			ElementBounds button = ElementBounds.Fixed(EnumDialogArea.RightFixed, 0.0, 0.0, 0.0, 0.0).WithFixedPadding(8.0, 5.0);
			string placeStr = Lang.Get("Place Auction");
			string cancelStr = Lang.Get("Cancel Auction");
			double placelen = CairoFont.ButtonText().GetTextExtents(placeStr).Width / (double)RuntimeEnv.GUIScale;
			base.SingleComposer.AddIf(showSearch).AddTextInput(searchFieldBounds, OnSearchTextChanged, CairoFont.WhiteSmallishText(), "searchField").AddStaticText(Lang.Get("auction-search-pricemin"), CairoFont.WhiteSmallText(), minTextBounds)
				.AddNumberInput(priceMin, OnPriceMinChange, CairoFont.WhiteSmallishText(), "priceMin")
				.AddRichtext(gearIcon, minGearIcon)
				.AddRichtext(Lang.Get("auction-search-pricemax"), CairoFont.WhiteSmallText(), maxTextBounds)
				.AddNumberInput(priceMax, OnPriceMaxChange, CairoFont.WhiteSmallishText(), "priceMax")
				.AddRichtext(gearIcon, maxGearIcon)
				.AddButton("X", OnClear, clearBounds, EnumButtonStyle.Normal, "clearBtn")
				.AddHoverText(Lang.Get("auction-search-reset"), CairoFont.WhiteSmallText(), 150, clearBounds, "clearBtnHover")
				.EndIf()
				.BeginClip(clipBounds)
				.AddInset(insetBounds, 3)
				.AddCellList(stackListBounds, createCell, auctionSys.ownAuctions, "stacklist")
				.EndClip()
				.AddVerticalScrollbar(OnNewScrollbarValue, scrollbarBounds, "scrollbar")
				.AddSmallButton(Lang.Get("Goodbye!"), OnByeClicked, leftButton.FixedUnder(clipBounds, 20.0))
				.AddSmallButton(Lang.Get("Place Auction"), OnCreateAuction, rightButton.FixedUnder(clipBounds, 20.0), EnumButtonStyle.Normal, "placeAuction")
				.AddSmallButton(cancelStr, OnCancelAuction, button.FlatCopy().FixedUnder(clipBounds, 20.0).WithFixedAlignmentOffset(0.0 - placelen, 0.0), EnumButtonStyle.Normal, "cancelAuction")
				.AddSmallButton(Lang.Get("Collect Funds"), OnCollectFunds, button.FlatCopy().FixedUnder(clipBounds, 20.0).WithFixedAlignmentOffset(0.0 - placelen, 0.0), EnumButtonStyle.Normal, "collectFunds")
				.AddSmallButton(Lang.Get("Retrieve Items"), OnRetrieveItems, button.FixedUnder(clipBounds, 20.0).WithFixedAlignmentOffset(0.0 - placelen, 0.0), EnumButtonStyle.Normal, "retrieveItems");
		}
		if (curTab == 1 || curTab == 2)
		{
			selectedElem = null;
			listElem = base.SingleComposer.GetCellList<Auction>("stacklist");
			listElem.BeforeCalcBounds();
			listElem.UnscaledCellVerPadding = 0;
			listElem.unscaledCellSpacing = 5;
			base.SingleComposer.EndChildElements().Compose();
			updateScrollbarBounds();
			if (showSearch)
			{
				GuiElementTextInput textInput = base.SingleComposer.GetTextInput("searchField");
				textInput.SetPlaceHolderText(Lang.Get("auction-search-placeholder"));
				textInput.SetValue(currentSearchText);
				GuiElementNumberInput numberInput = base.SingleComposer.GetNumberInput("priceMin");
				numberInput.IntMode = true;
				numberInput.SetValue(currentPriceMin);
				GuiElementNumberInput numberInput2 = base.SingleComposer.GetNumberInput("priceMax");
				numberInput2.IntMode = true;
				numberInput2.SetValue(currentPriceMax);
				UpdateClearButton();
				ApplySearchFilters();
			}
			didClickAuctionElem(-1);
		}
	}

	private bool OnClear()
	{
		ResetSearch();
		base.SingleComposer.GetTextInput("searchField")?.SetValue(currentSearchText);
		base.SingleComposer.GetNumberInput("priceMin")?.SetValue(currentPriceMin);
		base.SingleComposer.GetNumberInput("priceMax")?.SetValue(currentPriceMax);
		ApplySearchFilters();
		return true;
	}

	private void ResetSearch()
	{
		currentSearchText = string.Empty;
		currentPriceMin = 0;
		currentPriceMax = 10000;
	}

	private void OnPriceMinChange(string text)
	{
		currentPriceMin = Math.Max(text.ToInt(), 0);
		ApplySearchFilters();
	}

	private void OnPriceMaxChange(string text)
	{
		currentPriceMax = Math.Min(text.ToInt(), 10000);
		ApplySearchFilters();
	}

	private void OnSearchTextChanged(string text)
	{
		if (!(currentSearchText == text))
		{
			currentSearchText = text;
			ApplySearchFilters();
		}
	}

	private void ApplySearchFilters()
	{
		if (curTab != 1 && curTab != 2)
		{
			return;
		}
		UpdateClearButton();
		GuiElementCellList<Auction> stacklist = base.SingleComposer.GetCellList<Auction>("stacklist");
		if (stacklist == null)
		{
			return;
		}
		Regex regex = GuiDialog.RegexFromSearchText(currentSearchText);
		Regex strictRegex = GuiDialog.RegexFromSearchText(currentSearchText, strict: true);
		new List<WeightedAuction>();
		foreach (IGuiElementCell cell in stacklist.elementCells)
		{
			AuctionCellEntry auctionCell = cell as AuctionCellEntry;
			Auction auction = auctionCell.auction;
			auctionCell.weightedAuction = null;
			auctionCell.skipHeightCalcOnce = true;
			if (auction.Price < currentPriceMin || auction.Price > currentPriceMax)
			{
				auctionCell.Visible = false;
				continue;
			}
			string name = auction.ItemStack.GetName();
			int nameMatches = GuiDialog.CountMatches(name, regex);
			int strictNameMatches = GuiDialog.CountMatches(name, strictRegex);
			int sellerMatches = GuiDialog.CountMatches(auction.SellerName, regex);
			int strictSellerMatches = GuiDialog.CountMatches(auction.SellerName, strictRegex);
			auctionCell.Visible = nameMatches > 0 || sellerMatches > 0;
			if (auctionCell.Visible)
			{
				(cell as AuctionCellEntry).weightedAuction = new WeightedAuction
				{
					NameLength = name.Length,
					NameMatches = nameMatches,
					StrictNameMatches = strictNameMatches,
					SellerMatches = sellerMatches,
					StrictSellerMatches = strictSellerMatches
				};
			}
		}
		if (currentSearchText.Length > 0)
		{
			stacklist.elementCells.Sort(delegate(IGuiElementCell a, IGuiElementCell b)
			{
				WeightedAuction weightedAuction = (a as AuctionCellEntry).weightedAuction;
				WeightedAuction weightedAuction2 = (b as AuctionCellEntry).weightedAuction;
				if (weightedAuction == null && weightedAuction2 == null)
				{
					return 0;
				}
				if (weightedAuction == null)
				{
					return 1;
				}
				if (weightedAuction2 == null)
				{
					return -1;
				}
				int num = weightedAuction2.StrictNameMatches - weightedAuction.StrictNameMatches;
				if (num != 0)
				{
					return num;
				}
				int num2 = weightedAuction2.NameMatches - weightedAuction.NameMatches;
				if (num2 != 0)
				{
					return num2;
				}
				int num3 = weightedAuction2.StrictSellerMatches - weightedAuction.StrictSellerMatches;
				if (num3 != 0)
				{
					return num3;
				}
				int num4 = weightedAuction2.SellerMatches - weightedAuction.SellerMatches;
				if (num4 != 0)
				{
					return num4;
				}
				int num5 = weightedAuction.NameLength - weightedAuction2.NameLength;
				return (num5 != 0) ? num5 : 0;
			});
		}
		stacklist.FilterCells((IGuiElementCell guiElementCell) => (guiElementCell as AuctionCellEntry).Visible);
		updateScrollbarBounds();
	}

	private void UpdateClearButton()
	{
		GuiElementHoverText clearBtn = base.SingleComposer.GetHoverText("clearBtnHover");
		if (clearBtn != null)
		{
			bool showClear = !string.IsNullOrWhiteSpace(currentSearchText) || (int)base.SingleComposer.GetNumberInput("priceMin").GetValue() != 0 || (int)base.SingleComposer.GetNumberInput("priceMax").GetValue() != 10000;
			base.SingleComposer.GetButton("clearBtn").Visible = showClear;
			clearBtn.SetAutoDisplay(showClear);
		}
	}

	private void updateScrollbarBounds()
	{
		if (listElem != null)
		{
			GuiElementScrollbar scrollbar = base.SingleComposer.GetScrollbar("scrollbar");
			scrollbar?.Bounds.CalcWorldBounds();
			scrollbar?.SetHeights((float)clipBounds.fixedHeight, (float)listElem.Bounds.fixedHeight);
		}
	}

	private void OnNewScrollbarValue(float value)
	{
		listElem = base.SingleComposer.GetCellList<Auction>("stacklist");
		listElem.Bounds.fixedY = 0f - value;
		listElem.Bounds.CalcWorldBounds();
	}

	private bool OnCancelAuction()
	{
		if (selectedElem?.auction == null)
		{
			return false;
		}
		auctionSys.RetrieveAuctionClient(owningEntity, selectedElem.auction.AuctionId);
		return true;
	}

	private bool OnBuyAuctionClicked()
	{
		if (selectedElem?.auction == null)
		{
			return false;
		}
		object odlg = capi.OpenedGuis.FirstOrDefault((object d) => d is GuiDialogConfirmPurchase);
		if (odlg != null)
		{
			(odlg as GuiDialog).Focus();
			return true;
		}
		new GuiDialogConfirmPurchase(capi, capi.World.Player.Entity, owningEntity, selectedElem.auction).TryOpen();
		return true;
	}

	private bool OnCollectFunds()
	{
		if (selectedElem?.auction == null)
		{
			return false;
		}
		auctionSys.RetrieveAuctionClient(owningEntity, selectedElem.auction.AuctionId);
		return true;
	}

	private bool OnRetrieveItems()
	{
		if (selectedElem?.auction == null)
		{
			return false;
		}
		auctionSys.RetrieveAuctionClient(owningEntity, selectedElem.auction.AuctionId);
		return true;
	}

	private IGuiElementCell createCell(Auction auction, ElementBounds bounds)
	{
		bounds.fixedPaddingY = 0.0;
		return new AuctionCellEntry(capi, auctionSlotInv, bounds, auction, didClickAuctionElem);
	}

	private void didClickAuctionElem(int index)
	{
		if (selectedElem != null)
		{
			selectedElem.Selected = false;
		}
		if (index >= 0)
		{
			selectedElem = base.SingleComposer.GetCellList<Auction>("stacklist").elementCells[index] as AuctionCellEntry;
			selectedElem.Selected = true;
		}
		if (curTab == 2)
		{
			Auction auction = selectedElem?.auction;
			bool sold = (auction != null && auction.State == EnumAuctionState.Sold) || (auction != null && auction.State == EnumAuctionState.SoldRetrieved);
			base.SingleComposer.GetButton("cancelAuction").Visible = auction != null && auction.State == EnumAuctionState.Active;
			base.SingleComposer.GetButton("retrieveItems").Visible = (auction != null && auction.State == EnumAuctionState.Expired) || (sold && auction.SellerUid != capi.World.Player.PlayerUID);
			base.SingleComposer.GetButton("collectFunds").Visible = sold && auction.SellerUid == capi.World.Player.PlayerUID;
		}
	}

	private bool OnCreateAuction()
	{
		object odlg = capi.OpenedGuis.FirstOrDefault((object d) => d is GuiDialogCreateAuction);
		if (odlg != null)
		{
			(odlg as GuiDialog).Focus();
			return true;
		}
		new GuiDialogCreateAuction(capi, owningEntity, auctionSlotInv).TryOpen();
		return true;
	}

	private void OnTabClicked(int tab)
	{
		curTab = tab;
		Compose();
	}

	private void CalcAndUpdateAssetsDisplay()
	{
		int playerAssets = InventoryTrader.GetPlayerAssets(capi.World.Player.Entity);
		base.SingleComposer.GetDynamicText("playerMoneyText")?.SetNewText(Lang.Get("You have {0} Gears", playerAssets));
		int traderAssets = traderInventory.GetTraderAssets();
		base.SingleComposer.GetDynamicText("traderMoneyText")?.SetNewText(Lang.Get("{0} has {1} Gears", owningEntity.GetBehavior<EntityBehaviorNameTag>().DisplayName, traderAssets));
	}

	private void TraderInventory_SlotModified(int slotid)
	{
		int totalCost = traderInventory.GetTotalCost();
		int totalGain = traderInventory.GetTotalGain();
		base.SingleComposer.GetDynamicText("costText")?.SetNewText((totalCost > 0) ? Lang.Get("Total Cost: {0} Gears", totalCost) : "");
		base.SingleComposer.GetDynamicText("gainText")?.SetNewText((totalGain > 0) ? Lang.Get("Total Gain: {0} Gears", totalGain) : "");
		if (base.SingleComposer.GetButton("buysellButton") != null)
		{
			base.SingleComposer.GetButton("buysellButton").Enabled = totalCost > 0 || totalGain > 0;
			CalcAndUpdateAssetsDisplay();
		}
	}

	private bool OnBuySellClicked()
	{
		EnumTransactionResult num = traderInventory.TryBuySell(capi.World.Player);
		if (num == EnumTransactionResult.Success)
		{
			capi.Gui.PlaySound(new AssetLocation("sounds/effect/cashregister"), randomizePitch: false, 0.25f);
			(owningEntity as EntityTradingHumanoid).TalkUtil?.Talk(EnumTalkType.Purchase);
		}
		if (num == EnumTransactionResult.PlayerNotEnoughAssets)
		{
			(owningEntity as EntityTradingHumanoid).TalkUtil?.Talk(EnumTalkType.Complain);
			if (notifyPlayerMoneyTextSeconds <= 0.0)
			{
				prevPlrAbsFixedX = base.SingleComposer.GetDynamicText("playerMoneyText").Bounds.absFixedX;
				prevPlrAbsFixedY = base.SingleComposer.GetDynamicText("playerMoneyText").Bounds.absFixedY;
			}
			notifyPlayerMoneyTextSeconds = 1.5;
		}
		if (num == EnumTransactionResult.TraderNotEnoughAssets)
		{
			(owningEntity as EntityTradingHumanoid).TalkUtil?.Talk(EnumTalkType.Complain);
			if (notifyTraderMoneyTextSeconds <= 0.0)
			{
				prevTdrAbsFixedX = base.SingleComposer.GetDynamicText("traderMoneyText").Bounds.absFixedX;
				prevTdrAbsFixedY = base.SingleComposer.GetDynamicText("traderMoneyText").Bounds.absFixedY;
			}
			notifyTraderMoneyTextSeconds = 1.5;
		}
		if (num == EnumTransactionResult.TraderNotEnoughSupplyOrDemand)
		{
			(owningEntity as EntityTradingHumanoid).TalkUtil?.Talk(EnumTalkType.Complain);
		}
		capi.Network.SendEntityPacket(owningEntity.EntityId, 1000);
		TraderInventory_SlotModified(0);
		CalcAndUpdateAssetsDisplay();
		return true;
	}

	private bool OnByeClicked()
	{
		TryClose();
		return true;
	}

	private void DoSendPacket(object p)
	{
		capi.Network.SendEntityPacket(owningEntity.EntityId, p);
	}

	private void OnTitleBarClose()
	{
		TryClose();
	}

	public override void OnGuiOpened()
	{
		base.OnGuiOpened();
		traderInventory.SlotModified += TraderInventory_SlotModified;
		auctionSys.DidEnterAuctionHouse(owningEntity);
	}

	public override void OnGuiClosed()
	{
		base.OnGuiClosed();
		traderInventory.SlotModified -= TraderInventory_SlotModified;
		(owningEntity as EntityTradingHumanoid).TalkUtil?.Talk(EnumTalkType.Goodbye);
		capi.World.Player.InventoryManager.CloseInventoryAndSync(traderInventory);
		base.SingleComposer.GetSlotGrid("traderSellingSlots")?.OnGuiClosed(capi);
		base.SingleComposer.GetSlotGrid("playerBuyingSlots")?.OnGuiClosed(capi);
		base.SingleComposer.GetSlotGrid("traderBuyingSlots")?.OnGuiClosed(capi);
		base.SingleComposer.GetSlotGrid("playerSellingSlots")?.OnGuiClosed(capi);
		auctionSlotInv[0].Itemstack = null;
		capi.World.Player.InventoryManager.CloseInventoryAndSync(auctionSlotInv);
		auctionSys.DidLeaveAuctionHouse(owningEntity);
	}

	public override void OnBeforeRenderFrame3D(float deltaTime)
	{
		base.OnBeforeRenderFrame3D(deltaTime);
		if (notifyPlayerMoneyTextSeconds > 0.0)
		{
			notifyPlayerMoneyTextSeconds -= deltaTime;
			GuiElementDynamicText elem = base.SingleComposer.GetDynamicText("playerMoneyText");
			if (elem != null)
			{
				if (notifyPlayerMoneyTextSeconds <= 0.0)
				{
					elem.Bounds.absFixedX = prevPlrAbsFixedX;
					elem.Bounds.absFixedY = prevPlrAbsFixedY;
				}
				else
				{
					elem.Bounds.absFixedX = prevPlrAbsFixedX + notifyPlayerMoneyTextSeconds * (capi.World.Rand.NextDouble() * 4.0 - 2.0);
					elem.Bounds.absFixedY = prevPlrAbsFixedY + notifyPlayerMoneyTextSeconds * (capi.World.Rand.NextDouble() * 4.0 - 2.0);
				}
			}
		}
		if (!(notifyTraderMoneyTextSeconds > 0.0))
		{
			return;
		}
		notifyTraderMoneyTextSeconds -= deltaTime;
		GuiElementDynamicText elem2 = base.SingleComposer.GetDynamicText("traderMoneyText");
		if (elem2 != null)
		{
			if (notifyTraderMoneyTextSeconds <= 0.0)
			{
				elem2.Bounds.absFixedX = prevPlrAbsFixedX;
				elem2.Bounds.absFixedY = prevPlrAbsFixedY;
			}
			else
			{
				elem2.Bounds.absFixedX = prevTdrAbsFixedX + notifyTraderMoneyTextSeconds * (capi.World.Rand.NextDouble() * 4.0 - 2.0);
				elem2.Bounds.absFixedY = prevTdrAbsFixedY + notifyTraderMoneyTextSeconds * (capi.World.Rand.NextDouble() * 4.0 - 2.0);
			}
		}
	}
}
