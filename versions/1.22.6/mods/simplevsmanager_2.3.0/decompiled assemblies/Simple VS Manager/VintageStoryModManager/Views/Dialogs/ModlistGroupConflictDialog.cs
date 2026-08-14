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
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x0200002F RID: 47
	[NullableContext(1)]
	[Nullable(0)]
	public class ModlistGroupConflictDialog : Window, IComponentConnector
	{
		// Token: 0x060004BD RID: 1213 RVA: 0x00022844 File Offset: 0x00020A44
		public ModlistGroupConflictDialog(Window owner, IReadOnlyList<ModlistGroupConflict> conflicts)
		{
			ArgumentNullException.ThrowIfNull(conflicts, "conflicts");
			this.InitializeComponent();
			base.Owner = owner;
			this.Selections = Enumerable.ToList<ModlistGroupConflictDialog.ConflictSelection>(Enumerable.Select<ModlistGroupConflict, ModlistGroupConflictDialog.ConflictSelection>(conflicts, (ModlistGroupConflict conflict) => new ModlistGroupConflictDialog.ConflictSelection(conflict)));
			base.DataContext = this;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x000228A6 File Offset: 0x00020AA6
		public IReadOnlyList<ModlistGroupConflictDialog.ConflictSelection> Selections { get; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x000228B0 File Offset: 0x00020AB0
		public IReadOnlyDictionary<string, string> Resolutions
		{
			get
			{
				return Enumerable.ToDictionary<ModlistGroupConflictDialog.ConflictSelection, string, string>(this.Selections, (ModlistGroupConflictDialog.ConflictSelection selection) => selection.ModId, (ModlistGroupConflictDialog.ConflictSelection selection) => selection.SelectedChoice.MemberReference, StringComparer.OrdinalIgnoreCase);
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0002290B File Offset: 0x00020B0B
		private void SaveButton_OnClick(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0002291C File Offset: 0x00020B1C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/modlistgroupconflictdialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0002294C File Offset: 0x00020B4C
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				((Button)target).Click += new RoutedEventHandler(this.SaveButton_OnClick);
				return;
			}
			this._contentLoaded = true;
		}

		// Token: 0x04000289 RID: 649
		private bool _contentLoaded;

		// Token: 0x020001D3 RID: 467
		[Nullable(0)]
		public sealed class ConflictSelection
		{
			// Token: 0x06001395 RID: 5013 RVA: 0x000723A4 File Offset: 0x000705A4
			public ConflictSelection(ModlistGroupConflict conflict)
			{
				this.ModId = conflict.ModId;
				this.Choices = conflict.Choices;
				this.SelectedChoice = (Enumerable.FirstOrDefault<ModlistGroupConflictChoice>(conflict.Choices, (ModlistGroupConflictChoice choice) => string.Equals(choice.MemberReference, conflict.SelectedMemberReference, 5)) ?? conflict.Choices[0]);
			}

			// Token: 0x17000445 RID: 1093
			// (get) Token: 0x06001396 RID: 5014 RVA: 0x0007241D File Offset: 0x0007061D
			public string ModId { get; }

			// Token: 0x17000446 RID: 1094
			// (get) Token: 0x06001397 RID: 5015 RVA: 0x00072425 File Offset: 0x00070625
			public IReadOnlyList<ModlistGroupConflictChoice> Choices { get; }

			// Token: 0x17000447 RID: 1095
			// (get) Token: 0x06001398 RID: 5016 RVA: 0x0007242D File Offset: 0x0007062D
			// (set) Token: 0x06001399 RID: 5017 RVA: 0x00072435 File Offset: 0x00070635
			public ModlistGroupConflictChoice SelectedChoice { get; set; }
		}
	}
}
