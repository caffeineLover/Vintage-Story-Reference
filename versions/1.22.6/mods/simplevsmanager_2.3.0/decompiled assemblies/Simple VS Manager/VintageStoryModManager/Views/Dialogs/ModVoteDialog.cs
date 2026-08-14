using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using VintageStoryModManager.Models;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000032 RID: 50
	public class ModVoteDialog : Window, IComponentConnector
	{
		// Token: 0x060004CE RID: 1230 RVA: 0x00022B88 File Offset: 0x00020D88
		[NullableContext(1)]
		public ModVoteDialog(ModListItemViewModel mod, ModVersionVoteSummary summary, [Nullable(new byte[]
		{
			1,
			2,
			1,
			2
		})] Func<ModVersionVoteOption?, string, Task<ModVersionVoteSummary>> submitVoteAsync)
		{
			this.InitializeComponent();
			ArgumentNullException.ThrowIfNull(mod, "mod");
			if (summary == null)
			{
				throw new ArgumentNullException("summary");
			}
			this._summary = summary;
			if (submitVoteAsync == null)
			{
				throw new ArgumentNullException("submitVoteAsync");
			}
			this._submitVoteAsync = submitVoteAsync;
			base.Title = string.Format(CultureInfo.CurrentCulture, "User reports for {0}", mod.DisplayName);
			this.TitleTextBlock.Text = base.Title;
			this.ModVersionTextBlock.Text = (string.IsNullOrWhiteSpace(mod.VersionDisplay) ? "Mod version: Unknown" : string.Format(CultureInfo.CurrentCulture, "Mod version: {0}", mod.VersionDisplay));
			string vintageStoryVersion = this._summary.VintageStoryVersion;
			if (string.IsNullOrWhiteSpace(vintageStoryVersion))
			{
				this.GameVersionTextBlock.Text = "Vintage Story version: Unknown";
			}
			else
			{
				this.GameVersionTextBlock.Text = string.Format(CultureInfo.CurrentCulture, "Vintage Story version: {0}", vintageStoryVersion);
			}
			this.UpdateOptionButtons();
			this.StatusTextBlock.Text = this.BuildStatusText();
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00022C90 File Offset: 0x00020E90
		private void UpdateOptionButtons()
		{
			this.UpdateOptionButton(this.FullyFunctionalButton, ModVersionVoteOption.FullyFunctional);
			this.UpdateOptionButton(this.NoIssuesButton, ModVersionVoteOption.NoIssuesSoFar);
			this.UpdateOptionButton(this.SomeIssuesButton, ModVersionVoteOption.SomeIssuesButWorks);
			this.UpdateOptionButton(this.NotFunctionalButton, ModVersionVoteOption.NotFunctional);
			this.UpdateOptionButton(this.CrashesButton, ModVersionVoteOption.CrashesOrFreezesGame);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00022CE0 File Offset: 0x00020EE0
		[NullableContext(1)]
		private void UpdateOptionButton(Button button, ModVersionVoteOption option)
		{
			int count = this._summary.Counts.GetCount(option);
			string text = (count == 1) ? "1 vote" : string.Format(CultureInfo.CurrentCulture, "{0} votes", count);
			button.Content = string.Format(CultureInfo.CurrentCulture, "{0} — {1}", option.ToDisplayString(), text);
			ModVersionVoteOption? userVote = this._summary.UserVote;
			button.FontWeight = ((userVote.GetValueOrDefault() == option & userVote != null) ? FontWeights.Bold : FontWeights.Normal);
			button.IsEnabled = !this._isSubmitting;
			button.ToolTip = (option.RequiresComment() ? "Selecting this option will ask you to choose a reason." : null);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00022D98 File Offset: 0x00020F98
		[NullableContext(1)]
		private string BuildStatusText()
		{
			int totalVotes = this._summary.TotalVotes;
			string text = (totalVotes == 1) ? "1 total vote" : string.Format(CultureInfo.CurrentCulture, "{0} total votes", totalVotes);
			return string.Format(CultureInfo.CurrentCulture, "{0}. Click an option to submit or update your vote.", text);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00022DE4 File Offset: 0x00020FE4
		[NullableContext(1)]
		private void VoteButton_OnClick(object sender, RoutedEventArgs e)
		{
			ModVoteDialog.<VoteButton_OnClick>d__9 <VoteButton_OnClick>d__;
			<VoteButton_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<VoteButton_OnClick>d__.<>4__this = this;
			<VoteButton_OnClick>d__.sender = sender;
			<VoteButton_OnClick>d__.<>1__state = -1;
			<VoteButton_OnClick>d__.<>t__builder.Start<ModVoteDialog.<VoteButton_OnClick>d__9>(ref <VoteButton_OnClick>d__);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00022E24 File Offset: 0x00021024
		[NullableContext(2)]
		private string PromptForReason(ModVersionVoteOption option, string existingReason)
		{
			IReadOnlyList<string> readOnlyList;
			if (option != ModVersionVoteOption.NotFunctional)
			{
				if (option != ModVersionVoteOption.CrashesOrFreezesGame)
				{
					readOnlyList = Array.Empty<string>();
				}
				else
				{
					readOnlyList = ModVoteDialog.CrashReasons;
				}
			}
			else
			{
				readOnlyList = ModVoteDialog.NotFunctionalReasons;
			}
			IReadOnlyList<string> readOnlyList2 = readOnlyList;
			string text;
			if (option != ModVersionVoteOption.NotFunctional)
			{
				if (option != ModVersionVoteOption.CrashesOrFreezesGame)
				{
					text = "Share more details";
				}
				else
				{
					text = "Tell us about the crash";
				}
			}
			else
			{
				text = "Why is the mod not functional?";
			}
			string title = text;
			if (option != ModVersionVoteOption.NotFunctional)
			{
				if (option != ModVersionVoteOption.CrashesOrFreezesGame)
				{
					text = "Pick the option that best matches your experience.";
				}
				else
				{
					text = "Pick the option that best matches the crash or freeze you experienced.";
				}
			}
			else
			{
				text = "Pick the option that best matches why the mod isn't working for you.";
			}
			string description = text;
			string selectedReason = null;
			if (!string.IsNullOrWhiteSpace(existingReason) && readOnlyList2.IndexOf(existingReason) >= 0)
			{
				selectedReason = existingReason;
			}
			ModVoteReasonDialog modVoteReasonDialog = new ModVoteReasonDialog(title, description, readOnlyList2, selectedReason)
			{
				Owner = this
			};
			if (!modVoteReasonDialog.ShowDialog().GetValueOrDefault())
			{
				return null;
			}
			return modVoteReasonDialog.SelectedReason;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00022EE8 File Offset: 0x000210E8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/modvotedialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00022F18 File Offset: 0x00021118
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.TitleTextBlock = (TextBlock)target;
				return;
			case 2:
				this.ModVersionTextBlock = (TextBlock)target;
				return;
			case 3:
				this.GameVersionTextBlock = (TextBlock)target;
				return;
			case 4:
				this.FullyFunctionalButton = (Button)target;
				this.FullyFunctionalButton.Click += new RoutedEventHandler(this.VoteButton_OnClick);
				return;
			case 5:
				this.NoIssuesButton = (Button)target;
				this.NoIssuesButton.Click += new RoutedEventHandler(this.VoteButton_OnClick);
				return;
			case 6:
				this.SomeIssuesButton = (Button)target;
				this.SomeIssuesButton.Click += new RoutedEventHandler(this.VoteButton_OnClick);
				return;
			case 7:
				this.NotFunctionalButton = (Button)target;
				this.NotFunctionalButton.Click += new RoutedEventHandler(this.VoteButton_OnClick);
				return;
			case 8:
				this.CrashesButton = (Button)target;
				this.CrashesButton.Click += new RoutedEventHandler(this.VoteButton_OnClick);
				return;
			case 9:
				this.StatusTextBlock = (TextBlock)target;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x04000295 RID: 661
		[Nullable(1)]
		private static readonly IReadOnlyList<string> NotFunctionalReasons = new string[]
		{
			"The main function of the mod doesn't work",
			"It has major performance issues",
			"GUI or visual issues that makes it unplayable",
			"Supposed to work in multiplayer but doesn't",
			"Other"
		};

		// Token: 0x04000296 RID: 662
		[Nullable(1)]
		private static readonly IReadOnlyList<string> CrashReasons = new string[]
		{
			"Crash with error on game start",
			"Crash with error during gameplay",
			"Crash/freeze straight to desktop on game start",
			"Crash/freeze straight to desktop during gameplay",
			"Breaks so much you might as well call it a crash"
		};

		// Token: 0x04000297 RID: 663
		[Nullable(new byte[]
		{
			1,
			2,
			1,
			2
		})]
		private readonly Func<ModVersionVoteOption?, string, Task<ModVersionVoteSummary>> _submitVoteAsync;

		// Token: 0x04000298 RID: 664
		private bool _isSubmitting;

		// Token: 0x04000299 RID: 665
		[Nullable(1)]
		private ModVersionVoteSummary _summary;

		// Token: 0x0400029A RID: 666
		internal TextBlock TitleTextBlock;

		// Token: 0x0400029B RID: 667
		internal TextBlock ModVersionTextBlock;

		// Token: 0x0400029C RID: 668
		internal TextBlock GameVersionTextBlock;

		// Token: 0x0400029D RID: 669
		internal Button FullyFunctionalButton;

		// Token: 0x0400029E RID: 670
		internal Button NoIssuesButton;

		// Token: 0x0400029F RID: 671
		internal Button SomeIssuesButton;

		// Token: 0x040002A0 RID: 672
		internal Button NotFunctionalButton;

		// Token: 0x040002A1 RID: 673
		internal Button CrashesButton;

		// Token: 0x040002A2 RID: 674
		internal TextBlock StatusTextBlock;

		// Token: 0x040002A3 RID: 675
		private bool _contentLoaded;
	}
}
