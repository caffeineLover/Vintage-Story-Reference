using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace Vintagestory.Client.NoObf;

public class GuiDialogTickProfiler : GuiDialog
{
	private ProfileEntryRange root1sSum = new ProfileEntryRange();

	private int frames;

	private int maxLines = 20;

	public override string ToggleKeyCombinationCode => "tickprofiler";

	public override bool Focusable => false;

	public override EnumDialogType DialogType => EnumDialogType.HUD;

	public GuiDialogTickProfiler(ICoreClientAPI capi)
		: base(capi)
	{
		root1sSum.Code = "root";
		capi.Event.RegisterGameTickListener(OnEverySecond, 1000);
		CairoFont font = CairoFont.WhiteDetailText();
		double lineHeight = font.GetFontExtents().Height * font.LineHeightMultiplier / (double)RuntimeEnv.GUIScale;
		ElementBounds textBounds = ElementBounds.Fixed(EnumDialogArea.None, 0.0, 0.0, 450.0, 30.0 + (double)maxLines * lineHeight);
		ElementBounds overlayBounds = textBounds.ForkBoundingParent(5.0, 5.0, 5.0, 5.0);
		ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.RightMiddle).WithFixedAlignmentOffset(0.0 - GuiStyle.DialogToScreenPadding, 0.0);
		base.SingleComposer = capi.Gui.CreateCompo("tickprofiler", dialogBounds).AddGameOverlay(overlayBounds).AddDynamicText("", font, textBounds, "text")
			.Compose();
	}

	private void OnEverySecond(float dt)
	{
		if (IsOpened())
		{
			StringBuilder sb = new StringBuilder();
			ticksToString(root1sSum, sb);
			string text = sb.ToString();
			string[] lines = text.Split('\n');
			if (lines.Length > maxLines)
			{
				text = string.Join("\n", lines, 0, maxLines);
			}
			base.SingleComposer.GetDynamicText("text").SetNewText(text, autoHeight: true);
			frames = 0;
			root1sSum = new ProfileEntryRange();
			root1sSum.Code = "root";
		}
	}

	private void ticksToString(ProfileEntryRange entry, StringBuilder strib, string indent = "")
	{
		double timeMS = (double)entry.ElapsedTicks / (double)Stopwatch.Frequency * 1000.0 / (double)frames;
		if (timeMS < 0.2)
		{
			return;
		}
		string code = ((entry.Code.Length > 37) ? ("..." + entry.Code?.Substring(Math.Max(0, entry.Code.Length - 40))) : entry.Code);
		strib.AppendLine(indent + $"{timeMS:0.00}ms, {entry.CallCount:####} calls/s, {code}");
		List<ProfileEntryRange> profiles = new List<ProfileEntryRange>();
		if (entry.Marks != null)
		{
			profiles.AddRange(entry.Marks.Select((KeyValuePair<string, ProfileEntry> e) => new ProfileEntryRange
			{
				ElapsedTicks = e.Value.ElapsedTicks,
				Code = e.Key,
				CallCount = e.Value.CallCount
			}));
		}
		if (entry.ChildRanges != null)
		{
			profiles.AddRange(entry.ChildRanges.Values);
		}
		foreach (ProfileEntryRange prof in profiles.OrderByDescending((ProfileEntryRange profileEntryRange) => profileEntryRange.ElapsedTicks).Take(25))
		{
			ticksToString(prof, strib, indent + "  ");
		}
	}

	public override void OnFinalizeFrame(float dt)
	{
		if (IsOpened())
		{
			ProfileEntryRange entry = ScreenManager.FrameProfiler.PrevRootEntry;
			if (entry != null)
			{
				sumUpTickCosts(entry, root1sSum);
			}
			frames++;
			base.OnFinalizeFrame(dt);
		}
	}

	private void sumUpTickCosts(ProfileEntryRange entry, ProfileEntryRange sumEntry)
	{
		sumEntry.ElapsedTicks += entry.ElapsedTicks;
		sumEntry.CallCount += entry.CallCount;
		if (entry.Marks != null)
		{
			if (sumEntry.Marks == null)
			{
				sumEntry.Marks = new Dictionary<string, ProfileEntry>();
			}
			foreach (KeyValuePair<string, ProfileEntry> val in entry.Marks)
			{
				if (!sumEntry.Marks.TryGetValue(val.Key, out var sumMark))
				{
					ProfileEntry profileEntry = (sumEntry.Marks[val.Key] = new ProfileEntry(val.Value.ElapsedTicks, val.Value.CallCount));
					sumMark = profileEntry;
				}
				sumMark.ElapsedTicks += val.Value.ElapsedTicks;
				sumMark.CallCount += val.Value.CallCount;
			}
		}
		if (entry.ChildRanges == null)
		{
			return;
		}
		if (sumEntry.ChildRanges == null)
		{
			sumEntry.ChildRanges = new Dictionary<string, ProfileEntryRange>();
		}
		foreach (KeyValuePair<string, ProfileEntryRange> val2 in entry.ChildRanges)
		{
			if (!sumEntry.ChildRanges.TryGetValue(val2.Key, out var sumChild))
			{
				ProfileEntryRange profileEntryRange = (sumEntry.ChildRanges[val2.Key] = new ProfileEntryRange());
				sumChild = profileEntryRange;
				sumChild.Code = val2.Key;
			}
			sumUpTickCosts(val2.Value, sumChild);
		}
	}

	public override void OnGuiOpened()
	{
		base.OnGuiOpened();
		ScreenManager.FrameProfiler.Enabled = true;
	}

	public override void OnGuiClosed()
	{
		base.OnGuiClosed();
		ScreenManager.FrameProfiler.Enabled = false;
	}
}
