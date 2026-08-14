using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Markup;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000033 RID: 51
	[NullableContext(1)]
	[Nullable(0)]
	public class ModVoteReasonDetailsDialog : Window, IComponentConnector
	{
		// Token: 0x060004D7 RID: 1239 RVA: 0x000230BC File Offset: 0x000212BC
		public ModVoteReasonDetailsDialog(string modName, ModVersionVoteSummary summary)
		{
			this.InitializeComponent();
			ArgumentNullException.ThrowIfNull(summary, "summary");
			string text = string.IsNullOrWhiteSpace(summary.ModVersion) ? "unknown" : summary.ModVersion;
			string text2 = string.IsNullOrWhiteSpace(summary.VintageStoryVersion) ? "unknown" : summary.VintageStoryVersion;
			base.Title = string.Format(CultureInfo.CurrentCulture, "User report reasons for {0}", modName);
			this.VersionDescription = string.Format(CultureInfo.CurrentCulture, "{0} {1} on Vintage Story {2}", modName, text, text2);
			this.ReasonGroups = ModVoteReasonDetailsDialog.BuildReasonGroups(summary);
			base.DataContext = this;
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00023157 File Offset: 0x00021357
		public string VersionDescription { get; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0002315F File Offset: 0x0002135F
		public IReadOnlyList<NegativeVoteReasonGroup> ReasonGroups { get; }

		// Token: 0x060004DA RID: 1242 RVA: 0x00023168 File Offset: 0x00021368
		private static IReadOnlyList<NegativeVoteReasonGroup> BuildReasonGroups(ModVersionVoteSummary summary)
		{
			List<NegativeVoteReasonGroup> list = new List<NegativeVoteReasonGroup>(2);
			ModVoteReasonDetailsDialog.AddGroup(list, ModVersionVoteOption.NotFunctional.ToDisplayString(), summary.Counts.NotFunctional, summary.Comments.NotFunctional);
			ModVoteReasonDetailsDialog.AddGroup(list, ModVersionVoteOption.CrashesOrFreezesGame.ToDisplayString(), summary.Counts.CrashesOrFreezesGame, summary.Comments.CrashesOrFreezesGame);
			return list;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x000231C0 File Offset: 0x000213C0
		private static void AddGroup(ICollection<NegativeVoteReasonGroup> groups, string category, int voteCount, IReadOnlyList<string> comments)
		{
			if (voteCount <= 0)
			{
				return;
			}
			List<NegativeVoteReason> list = new List<NegativeVoteReason>();
			Dictionary<string, NegativeVoteReason> dictionary = new Dictionary<string, NegativeVoteReason>(StringComparer.Ordinal);
			foreach (string text in comments)
			{
				string text2 = (text != null) ? text.Trim() : null;
				if (!string.IsNullOrWhiteSpace(text2))
				{
					NegativeVoteReason negativeVoteReason;
					if (dictionary.TryGetValue(text2, ref negativeVoteReason))
					{
						NegativeVoteReason negativeVoteReason2 = negativeVoteReason;
						int count = negativeVoteReason2.Count;
						negativeVoteReason2.Count = count + 1;
					}
					else
					{
						NegativeVoteReason negativeVoteReason3 = new NegativeVoteReason(text2, 1);
						list.Add(negativeVoteReason3);
						dictionary.Add(text2, negativeVoteReason3);
					}
				}
			}
			int num = Enumerable.Sum<NegativeVoteReason>(list, (NegativeVoteReason reason) => reason.Count);
			if (num < voteCount)
			{
				list.Add(new NegativeVoteReason("No reason was provided.", voteCount - num));
			}
			string text3;
			if (voteCount != 1)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(6, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(voteCount);
				defaultInterpolatedStringHandler.AppendLiteral(" votes");
				text3 = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				text3 = "1 vote";
			}
			string text4 = text3;
			groups.Add(new NegativeVoteReasonGroup(category + " — " + text4, list));
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x000232F4 File Offset: 0x000214F4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/modvotereasondetailsdialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00023324 File Offset: 0x00021524
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x040002A6 RID: 678
		private bool _contentLoaded;
	}
}
