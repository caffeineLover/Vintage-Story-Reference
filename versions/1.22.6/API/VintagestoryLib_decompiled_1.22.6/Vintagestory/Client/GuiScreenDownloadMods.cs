using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;
using System.Threading;
using System.Threading.Tasks;
using Cairo;
using Vintagestory.API.Client;
using Vintagestory.API.Config;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;
using Vintagestory.ModDb;

namespace Vintagestory.Client;

public class GuiScreenDownloadMods : GuiScreen
{
	private class ModInstallState(string modIdentifier, string? modVersion)
	{
		public string ModIdentifier = modIdentifier;

		public string? ModVersion = modVersion;

		public ModDbUtil.ModInstallInformation Info;

		public GuiElementDynamicText? StatusEl;
	}

	private readonly ModInstallState[] modInstallStates;

	private readonly List<string> modsToDownload;

	private readonly ServerConnectData? sourceServerConnection;

	private readonly StartServerArgs? sourceServerArgs;

	private ModDbUtil modDbUtil;

	private static ModInstallState? stateDummy;

	private int failedMods;

	public static GuiScreenDownloadMods ForInstallRequest(string modToDownload, ScreenManager screenManager, GuiScreen parentScreen)
	{
		string title = Lang.Get("downloadmods-title-selectinstall");
		string text = Lang.Get("downloadmods-selectinstall", modToDownload);
		string dataPathMods = GamePaths.DataPathMods;
		int num = 1;
		List<string> list = new List<string>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<string> span = CollectionsMarshal.AsSpan(list);
		int index = 0;
		span[index] = modToDownload;
		return new GuiScreenDownloadMods(null, null, dataPathMods, list, title, text, screenManager, parentScreen);
	}

	public static GuiScreenDownloadMods ForServerMods(ServerConnectData sourceServer, StartServerArgs serverArgs, List<string> modsToDownload, ScreenManager screenManager, GuiScreen parentScreen)
	{
		string installPath;
		string title;
		string text;
		if (sourceServer.Host == null)
		{
			installPath = GamePaths.DataPathMods;
			title = Lang.Get("downloadmods-title-dependencyinstall");
			text = Lang.Get("downloadmods-dependencyinstall", string.Join(", ", modsToDownload));
		}
		else
		{
			installPath = System.IO.Path.Combine(GamePaths.DataPathServerMods, GamePaths.ReplaceInvalidChars(sourceServer.Host + "-" + sourceServer.Port));
			title = Lang.Get("downloadmods-title-serverinstall");
			text = Lang.Get("downloadmods-serverinstall", modsToDownload.Count);
		}
		return new GuiScreenDownloadMods(sourceServer, serverArgs, installPath, modsToDownload, title, text, screenManager, parentScreen);
	}

	private GuiScreenDownloadMods(ServerConnectData? connectionData, StartServerArgs? serverArgs, string installPath, List<string> modsToDownload, string title, string text, ScreenManager screenManager, GuiScreen parentScreen)
		: base(screenManager, parentScreen)
	{
		GuiScreenDownloadMods guiScreenDownloadMods = this;
		sourceServerConnection = connectionData;
		sourceServerArgs = serverArgs;
		for (int i = 0; i < modsToDownload.Count; i++)
		{
			string spec = modsToDownload[i];
			if (spec.EndsWith('*'))
			{
				modsToDownload[i] = spec.Substring(0, spec.IndexOf('@'));
			}
		}
		this.modsToDownload = modsToDownload;
		modInstallStates = modsToDownload.Select(delegate(string text2)
		{
			string[] array = text2.Split('@', 2);
			return new ModInstallState(array[0], (array.Length > 1) ? array[1] : null);
		}).ToArray();
		ScreenManager.GuiComposers.ClearCache();
		ElementBounds dlgBounds = ElementBounds.Fixed(0.0, 0.0, 680.0, 390.0);
		ElementBounds paddedBounds = dlgBounds.FlatCopy().WithFixedOffset(15.0, 15.0).FixedShrink(30.0)
			.WithParent(dlgBounds);
		ElementBounds titleBounds = ElementBounds.FixedSize(paddedBounds.fixedWidth, 30.0);
		ElementBounds bodyBounds = ElementBounds.FixedSize(paddedBounds.fixedWidth, paddedBounds.fixedHeight - 80.0).FixedUnder(titleBounds);
		ElementComposer = ScreenManager.GuiComposers.Create("mainmenu-downloadmods", dlgBounds.WithAlignment(EnumDialogArea.CenterMiddle)).AddShadedDialogBG(ElementBounds.Fill, withTitleBar: false, 5.0, 1f).BeginChildElements(paddedBounds)
			.AddRichtext(title, CairoFont.WhiteSmallishText().WithWeight(FontWeight.Bold), titleBounds, didClickLink, "titleText")
			.AddRichtext(text + "\r\n\r\n" + Lang.Get("downloadmods-disclaimer"), CairoFont.WhiteSmallishText(), bodyBounds, didClickLink, "centertext")
			.AddDynamicText("", CairoFont.WhiteSmallText(), bodyBounds.FlatCopy(), "errortext")
			.AddButton(Lang.Get("Download mods"), onConfirmDisclaimer, ElementBounds.FixedPos(EnumDialogArea.LeftBottom, 0.0, 0.0).WithFixedPadding(10.0, 2.0), EnumButtonStyle.Normal, "confirmButton")
			.AddButton(Lang.Get("Cancel"), goBackToMainMenu, ElementBounds.FixedPos(EnumDialogArea.RightBottom, 0.0, 0.0).WithFixedPadding(10.0, 2.0))
			.EndChildElements()
			.Compose();
		ElementComposer.GetButton("confirmButton").Enabled = false;
		modDbUtil = new ModDbUtil(ScreenManager.api, ClientSettings.ModDbUrl, installPath);
		Task<ModDbUtil.ModInstallInformationResponse> task = modDbUtil.FetchModInstallInformation(modsToDownload, "1.22.6", ignoreRetractions: false);
		task.ContinueWith(delegate(Task<ModDbUtil.ModInstallInformationResponse> t)
		{
			ModDbUtil.ModInstallInformationResponse result = t.Result;
			if (result.Data != null)
			{
				foreach (var (identifier, info) in result.Data)
				{
					guiScreenDownloadMods.getInstallState(identifier).Info = info;
				}
			}
			if (!string.IsNullOrEmpty(result.Error))
			{
				GuiElementRichtext disclaimer = (GuiElementRichtext)guiScreenDownloadMods.ElementComposer.GetElement("centertext");
				ScreenManager.EnqueueMainThreadTask(delegate
				{
					disclaimer.SetNewText("", CairoFont.WhiteSmallText());
				});
				guiScreenDownloadMods.ElementComposer.GetDynamicText("errortext").SetNewText("ModDB Error: " + result.Error, autoHeight: false, forceRedraw: false, async: true);
			}
			else
			{
				guiScreenDownloadMods.ElementComposer.GetButton("confirmButton").Enabled = true;
			}
		}, TaskContinuationOptions.OnlyOnRanToCompletion);
		task.ContinueWith(delegate(Task<ModDbUtil.ModInstallInformationResponse> t)
		{
			GuiElementRichtext disclaimer = (GuiElementRichtext)guiScreenDownloadMods.ElementComposer.GetElement("centertext");
			ScreenManager.EnqueueMainThreadTask(delegate
			{
				disclaimer.SetNewText("", CairoFont.WhiteSmallText());
			});
			guiScreenDownloadMods.ElementComposer.GetDynamicText("errortext").SetNewText("ModDB Error: " + t.Exception.Message + "\n\nSee log for more details.", autoHeight: false, forceRedraw: false, async: true);
			screenManager.api.Logger.Error(t.Exception);
		}, TaskContinuationOptions.OnlyOnFaulted);
	}

	private ModInstallState getInstallState(string identifier)
	{
		ModInstallState[] array = modInstallStates;
		foreach (ModInstallState state in array)
		{
			if (state.ModIdentifier == identifier)
			{
				return state;
			}
		}
		if (stateDummy == null)
		{
			stateDummy = new ModInstallState("", null);
		}
		return stateDummy;
	}

	private bool goBackToMainMenu()
	{
		ScreenManager.StartMainMenu();
		if (sourceServerConnection != null)
		{
			ScreenManager.guiMainmenuLeft.OnMultiplayer();
		}
		return true;
	}

	private bool onConfirmDisclaimer()
	{
		if (!layoutMainScreen())
		{
			beginInstallation();
		}
		return true;
	}

	private void beginInstallation()
	{
		failedMods = 0;
		List<Task> tasks = new List<Task>(modInstallStates.Length);
		ModInstallState[] array = modInstallStates;
		foreach (ModInstallState entry in array)
		{
			if (entry.Info.ErrorCode != 0)
			{
				Interlocked.Increment(ref failedMods);
				continue;
			}
			if (ModDbUtil.IsModBlocked(entry.ModIdentifier, entry.ModVersion, out string blockReason))
			{
				entry.StatusEl.SetNewText(Lang.Get("modloader-blockedmod", blockReason));
				Interlocked.Increment(ref failedMods);
				continue;
			}
			var (enableOutcome, installedVersion) = modDbUtil.MaybeEnableMod(entry.ModIdentifier, entry.ModVersion, deleteOlder: false);
			switch (enableOutcome)
			{
			case ModDbUtil.ModEnableOutcome.AlreadyEnabled:
				entry.StatusEl.SetNewText(Lang.Get("mod-installed-alreadyenabled"));
				continue;
			case ModDbUtil.ModEnableOutcome.NewlyEnabled:
				entry.StatusEl.SetNewText(Lang.Get("mod-installed-willenable"));
				continue;
			case ModDbUtil.ModEnableOutcome.InstalledIsNewer:
				entry.StatusEl.SetNewText(Lang.Get("mod-installed-newer", installedVersion));
				continue;
			case ModDbUtil.ModEnableOutcome.InstalledIsOlder:
				entry.StatusEl.SetNewText(Lang.Get("mod-installed-older", installedVersion));
				break;
			}
			if (entry.Info.FileUrl == null)
			{
				Interlocked.Increment(ref failedMods);
				continue;
			}
			Task t = modDbUtil.DownloadModAsync(entry.Info.FileUrl, entry.Info.FileName, delegate(long progress, long totalSize)
			{
				string newText = Lang.Get((totalSize == 0L) ? "downloading-progress-1" : "downloading-progress-2", progress, totalSize);
				ScreenManager.EnqueueMainThreadTask(delegate
				{
					entry.StatusEl.SetNewText(newText);
				});
			}).ContinueWith(delegate(Task<Exception?> task)
			{
				Exception result = task.Result;
				string status;
				if (result != null)
				{
					Interlocked.Increment(ref failedMods);
					status = result.Message;
				}
				else
				{
					status = Lang.Get("mod-installed-success");
				}
				ScreenManager.EnqueueMainThreadTask(delegate
				{
					entry.StatusEl.SetNewText(status);
				});
			});
			tasks.Add(t);
		}
		Task.WhenAll(tasks).ContinueWith(delegate
		{
			if (failedMods == 0)
			{
				GuiElementTextButton joinBtn = ElementComposer.GetButton("joinBtn");
				ElementComposer.GetButton("joinBtn");
				ScreenManager.EnqueueMainThreadTask(delegate
				{
					joinBtn.Enabled = true;
				});
				ElementComposer.GetDynamicText("titleText").SetNewText(Lang.Get("modinstall-complete"), autoHeight: false, forceRedraw: false, async: true);
			}
			else
			{
				ElementComposer.GetDynamicText("titleText").SetNewText(Lang.Get("modinstall-some-failed"), autoHeight: false, forceRedraw: false, async: true);
			}
		});
	}

	private bool layoutMainScreen()
	{
		CairoFont semiTransparentFont = CairoFont.WhiteSmallishText().WithOrientation(EnumTextOrientation.Center);
		semiTransparentFont.Color[3] = 0.65;
		ElementBounds dialogBounds = ElementBounds.Fixed(0.0, 0.0, 800.0, 680.0);
		ElementBounds titleBounds = ElementBounds.Fixed(0.0, -50.0, dialogBounds.fixedWidth, 28.0);
		ElementBounds scrollBorderBounds = ElementBounds.FixedSize(dialogBounds.fixedWidth, dialogBounds.fixedHeight - 90.0);
		ElementBounds scrollbarBounds = ElementBounds.FixedSize(10.0, scrollBorderBounds.fixedHeight);
		ElementBounds scrollRegion = scrollBorderBounds.FlatCopy().WithFixedOffset(1.0, 1.0).FixedShrink(2.0);
		scrollRegion.fixedWidth -= scrollbarBounds.fixedWidth;
		CairoFont whiteSmallishFont = CairoFont.WhiteSmallishText();
		CairoFont whiteSmallFont = CairoFont.WhiteSmallText();
		CairoFont redSmallishFont = CairoFont.WhiteSmallText().WithOrientation(EnumTextOrientation.Center);
		redSmallishFont.Color[1] = 0.2;
		redSmallishFont.Color[2] = 0.2;
		string continueButtonText = Lang.Get((sourceServerConnection?.Host == null) ? "Continue" : "Join Server");
		bool retractedReleases = false;
		bool forcedRetractedReleases = false;
		ElementComposer = ScreenManager.GuiComposers.Create("downloadmods", ElementBounds.Fill).AddShadedDialogBG(ElementBounds.Fill, withTitleBar: false, 5.0, 1f).BeginChildElements(dialogBounds.WithAlignment(EnumDialogArea.CenterMiddle))
			.AddDynamicText(Lang.Get("Downloading mods..."), semiTransparentFont, titleBounds, "titleText")
			.AddInset(scrollBorderBounds, 3, 0.8f)
			.BeginClip(scrollRegion);
		ElementBounds containerRegion = scrollRegion.ForkChild();
		GuiElementContainer container = new GuiElementContainer(ElementComposer.Api, containerRegion);
		ElementBounds entryBorderBounds = containerRegion.ForkChild().WithFixedHeight(50.0);
		ModInstallState[] array = modInstallStates;
		foreach (ModInstallState entry in array)
		{
			switch (entry.Info.ErrorCode)
			{
			case 4101:
				retractedReleases = true;
				break;
			case 4032:
			case 4102:
				forcedRetractedReleases = true;
				break;
			}
			ElementBounds entryBounds = entryBorderBounds.FlatCopy().WithFixedOffset(3.0, 3.0).FixedShrink(6.0);
			container.Add(new GuiElementInset(ElementComposer.Api, entryBorderBounds, 4, 0.85f));
			container.Add(new GuiElementParent(ElementComposer.Api, entryBounds));
			ElementBounds identifierBounds = ElementBounds.FixedSize(100.0, 22.0).WithParent(entryBounds);
			container.Add(new GuiElementStaticText(ElementComposer.Api, entry.ModIdentifier, EnumTextOrientation.Left, ElementBounds.FixedSize(200.0, 22.0).WithParent(entryBounds), whiteSmallishFont));
			ElementBounds versionBounds = identifierBounds.BelowCopy();
			container.Add(new GuiElementStaticText(ElementComposer.Api, entry.ModVersion ?? "*", EnumTextOrientation.Left, versionBounds, whiteSmallishFont));
			ElementBounds InitialStatusBounds = ElementBounds.Fixed(identifierBounds.fixedWidth * 2.0, 0.0, entryBounds.fixedWidth - identifierBounds.fixedWidth * 2.0, 22.0).WithParent(entryBounds);
			container.Add(new GuiElementStaticText(ElementComposer.Api, entry.Info.FormatErrorCode(), EnumTextOrientation.Left, InitialStatusBounds, whiteSmallFont));
			container.Add(entry.StatusEl = new GuiElementDynamicText(ElementComposer.Api, "", whiteSmallFont, InitialStatusBounds.BelowCopy()));
			if (!string.IsNullOrEmpty(entry.Info.RetractionReason))
			{
				entry.StatusEl.Text = Lang.Get("modinstall-hover-for-reason");
				string reason = GetHtmlTagRegex().Replace(entry.Info.RetractionReason.Replace("&nbsp;", " "), (Match m) => (!(m.Captures[0].ValueSpan == "br".AsSpan())) ? "" : "\n").Trim();
				container.Add(new GuiElementHoverText(ElementComposer.Api, Lang.Get("mod-retracted-reason", reason), whiteSmallFont, 480, entryBounds.FlatCopy()));
			}
			entryBorderBounds = entryBorderBounds.BelowCopy();
		}
		ElementComposer.AddInteractiveElement(container, "container").EndClip().AddVerticalScrollbar(onContainerScroll, scrollbarBounds.WithAlignment(EnumDialogArea.RightFixed), "scrollbar")
			.AddIf(retractedReleases || forcedRetractedReleases)
			.AddDynamicText(Lang.Get(forcedRetractedReleases ? "modinstall-retracted-releases-forced" : "modinstall-retracted-releases"), redSmallishFont, scrollBorderBounds.BelowCopy(0.0, 5.0).WithFixedHeight(40.0), "retractionWarning")
			.EndIf()
			.AddButton(continueButtonText, onClickContinue, ElementBounds.FixedPos(EnumDialogArea.LeftBottom, 0.0, 0.0).WithFixedPadding(10.0, 2.0), EnumButtonStyle.Normal, "joinBtn")
			.AddIf(retractedReleases || forcedRetractedReleases)
			.AddButton(Lang.Get("Install Anyways"), onClickIgnoreRetractions, ElementBounds.FixedPos(EnumDialogArea.CenterBottom, 0.0, 0.0).WithFixedPadding(10.0, 2.0), EnumButtonStyle.Normal, "ignoreRetractionsBtn")
			.EndIf()
			.AddButton(Lang.Get("Cancel"), goBackToMainMenu, ElementBounds.FixedPos(EnumDialogArea.RightBottom, 0.0, 0.0).WithFixedPadding(10.0, 2.0))
			.EndChildElements()
			.Compose();
		ElementComposer.GetButton("joinBtn").Enabled = false;
		if (forcedRetractedReleases)
		{
			ElementComposer.GetButton("ignoreRetractionsBtn").Enabled = false;
		}
		scrollRegion.CalcWorldBounds();
		ElementComposer.GetScrollbar("scrollbar").SetHeights((float)scrollRegion.fixedHeight, (float)((double)modInstallStates.Count() * 50.0));
		return retractedReleases || forcedRetractedReleases;
	}

	private bool onClickIgnoreRetractions()
	{
		ElementComposer.GetButton("ignoreRetractionsBtn").Enabled = false;
		doThing();
		return true;
		async Task doThing()
		{
			ModDbUtil.ModInstallInformationResponse response = await modDbUtil.FetchModInstallInformation(modsToDownload, "1.22.6", ignoreRetractions: true);
			if (response.Data != null)
			{
				foreach (var (ident, info) in response.Data)
				{
					getInstallState(ident).Info = info;
				}
				ScreenManager.EnqueueMainThreadTask(delegate
				{
					if (!layoutMainScreen())
					{
						beginInstallation();
					}
				});
			}
			else
			{
				ElementComposer.GetDynamicText("retractionWarning").SetNewText(response.Error, autoHeight: false, forceRedraw: false, async: true);
				ScreenManager.EnqueueMainThreadTask(delegate
				{
					ElementComposer.GetButton("ignoreRetractionsBtn").Enabled = false;
				});
			}
		}
	}

	private bool onClickContinue()
	{
		ScreenManager.loadMods();
		if (sourceServerConnection == null)
		{
			ScreenManager.StartMainMenu();
		}
		else if (sourceServerConnection.Host == null)
		{
			ScreenManager.ConnectToSingleplayer(sourceServerArgs);
		}
		else
		{
			ScreenManager.ConnectToMultiplayer(sourceServerConnection.HostRaw, sourceServerConnection.ServerPassword);
		}
		return true;
	}

	private void onContainerScroll(float value)
	{
		ElementBounds bounds = ElementComposer.GetContainer("container").Bounds;
		bounds.fixedY = 0f - value;
		bounds.CalcWorldBounds();
	}

	private void didClickLink(LinkTextComponent link)
	{
		ScreenManager.StartMainMenu();
		ScreenManager.EnqueueMainThreadTask(delegate
		{
			ScreenManager.api.Gui.OpenLink(link.Href);
		});
	}

	public override void RenderToDefaultFramebuffer(float dt)
	{
		if (ScreenManager.KeyboardKeyState[50])
		{
			ScreenManager.StartMainMenu();
			return;
		}
		ElementComposer.Render(dt);
		ScreenManager.RenderMainMenuParts(dt, ElementComposer.Bounds, withMainMenu: false);
		if (ScreenManager.mainMenuComposer.MouseOverCursor != null)
		{
			FocusedMouseCursor = ScreenManager.mainMenuComposer.MouseOverCursor;
		}
		ElementComposer.PostRender(dt);
	}

	[GeneratedRegex("</?([\\w\\-]+)(?: .*?)?/?>")]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.13.7005")]
	private static Regex GetHtmlTagRegex()
	{
		return _003CRegexGenerator_g_003EF7C3416659A89DE69D307209EBAB141680463E82FA58DF94FA09D5F330FC4C71E__GetHtmlTagRegex_0.Instance;
	}
}
