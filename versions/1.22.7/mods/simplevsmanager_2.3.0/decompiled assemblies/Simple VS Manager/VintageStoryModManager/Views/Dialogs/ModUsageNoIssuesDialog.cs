using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000031 RID: 49
	[NullableContext(1)]
	[Nullable(0)]
	public class ModUsageNoIssuesDialog : Window, IComponentConnector
	{
		// Token: 0x060004C3 RID: 1219 RVA: 0x00022974 File Offset: 0x00020B74
		public ModUsageNoIssuesDialog(IEnumerable<ModUsageVoteCandidateViewModel> candidates)
		{
			this.InitializeComponent();
			if (candidates == null)
			{
				throw new ArgumentNullException("candidates");
			}
			this._candidates = Enumerable.ToList<ModUsageVoteCandidateViewModel>(Enumerable.Where<ModUsageVoteCandidateViewModel>(candidates, (ModUsageVoteCandidateViewModel candidate) => candidate != null));
			base.DataContext = this;
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x000229DD File Offset: 0x00020BDD
		public IReadOnlyList<ModUsageVoteCandidateViewModel> Candidates
		{
			get
			{
				return this._candidates;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x000229E5 File Offset: 0x00020BE5
		// (set) Token: 0x060004C6 RID: 1222 RVA: 0x000229ED File Offset: 0x00020BED
		public IReadOnlyList<ModUsageVoteCandidateViewModel> SelectedCandidates { get; private set; } = Array.Empty<ModUsageVoteCandidateViewModel>();

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x000229F6 File Offset: 0x00020BF6
		// (set) Token: 0x060004C8 RID: 1224 RVA: 0x000229FE File Offset: 0x00020BFE
		public ModUsageNoIssuesDialogResult Result { get; private set; }

		// Token: 0x060004C9 RID: 1225 RVA: 0x00022A08 File Offset: 0x00020C08
		private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
		{
			List<ModUsageVoteCandidateViewModel> list = Enumerable.ToList<ModUsageVoteCandidateViewModel>(Enumerable.Where<ModUsageVoteCandidateViewModel>(this._candidates, (ModUsageVoteCandidateViewModel candidate) => candidate.IsSelected));
			if (list.Count == 0)
			{
				this.StatusTextBlock.Text = "Select at least one mod to submit votes.";
				return;
			}
			this.SelectedCandidates = list;
			this.Result = ModUsageNoIssuesDialogResult.SubmitVotes;
			base.DialogResult = new bool?(true);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00022A78 File Offset: 0x00020C78
		private void RemindMeLaterButton_OnClick(object sender, RoutedEventArgs e)
		{
			this.SelectedCandidates = Array.Empty<ModUsageVoteCandidateViewModel>();
			this.Result = ModUsageNoIssuesDialogResult.RemindLater;
			base.DialogResult = new bool?(false);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00022A98 File Offset: 0x00020C98
		private void NeverAskAgainButton_OnClick(object sender, RoutedEventArgs e)
		{
			this.SelectedCandidates = Array.Empty<ModUsageVoteCandidateViewModel>();
			this.Result = ModUsageNoIssuesDialogResult.DisableTracking;
			base.DialogResult = new bool?(true);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00022AB8 File Offset: 0x00020CB8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/modusagenoissuesdialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00022AE8 File Offset: 0x00020CE8
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.ModListView = (ListView)target;
				return;
			case 2:
				this.StatusTextBlock = (TextBlock)target;
				return;
			case 3:
				((Button)target).Click += new RoutedEventHandler(this.NeverAskAgainButton_OnClick);
				return;
			case 4:
				((Button)target).Click += new RoutedEventHandler(this.RemindMeLaterButton_OnClick);
				return;
			case 5:
				this.SubmitButton = (Button)target;
				this.SubmitButton.Click += new RoutedEventHandler(this.SubmitButton_OnClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x0400028E RID: 654
		private readonly List<ModUsageVoteCandidateViewModel> _candidates;

		// Token: 0x04000291 RID: 657
		[Nullable(0)]
		internal ListView ModListView;

		// Token: 0x04000292 RID: 658
		[Nullable(0)]
		internal TextBlock StatusTextBlock;

		// Token: 0x04000293 RID: 659
		[Nullable(0)]
		internal Button SubmitButton;

		// Token: 0x04000294 RID: 660
		private bool _contentLoaded;
	}
}
