using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000036 RID: 54
	public class ModVoteReasonDialog : Window, IComponentConnector
	{
		// Token: 0x060004F3 RID: 1267 RVA: 0x00023548 File Offset: 0x00021748
		[NullableContext(1)]
		public ModVoteReasonDialog(string title, string description, IReadOnlyList<string> reasons, [Nullable(2)] string selectedReason)
		{
			this.InitializeComponent();
			ArgumentNullException.ThrowIfNull(reasons, "reasons");
			this.TitleTextBlock.Text = title;
			this.DescriptionTextBlock.Text = description;
			this._reasons = reasons;
			this.ReasonComboBox.ItemsSource = this._reasons;
			if (!string.IsNullOrWhiteSpace(selectedReason))
			{
				int num = this._reasons.IndexOf(selectedReason);
				if (num >= 0)
				{
					this.ReasonComboBox.SelectedIndex = num;
				}
			}
			base.Loaded += delegate(object _, RoutedEventArgs _)
			{
				this.ReasonComboBox.Focus();
			};
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x000235D5 File Offset: 0x000217D5
		[Nullable(2)]
		public string SelectedReason
		{
			[NullableContext(2)]
			get
			{
				return this.ReasonComboBox.SelectedItem as string;
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x000235E7 File Offset: 0x000217E7
		[NullableContext(1)]
		private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (this.SelectedReason == null)
			{
				return;
			}
			base.DialogResult = new bool?(true);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x000235FE File Offset: 0x000217FE
		[NullableContext(1)]
		private void CancelButton_OnClick(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0002360C File Offset: 0x0002180C
		[NullableContext(1)]
		private void ReasonComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.SubmitButton.IsEnabled = (this.SelectedReason != null);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00023628 File Offset: 0x00021828
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/modvotereasondialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00023658 File Offset: 0x00021858
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
				this.DescriptionTextBlock = (TextBlock)target;
				return;
			case 3:
				this.ReasonComboBox = (ComboBox)target;
				this.ReasonComboBox.SelectionChanged += new SelectionChangedEventHandler(this.ReasonComboBox_OnSelectionChanged);
				return;
			case 4:
				((Button)target).Click += new RoutedEventHandler(this.CancelButton_OnClick);
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

		// Token: 0x040002AB RID: 683
		[Nullable(1)]
		private readonly IReadOnlyList<string> _reasons;

		// Token: 0x040002AC RID: 684
		internal TextBlock TitleTextBlock;

		// Token: 0x040002AD RID: 685
		internal TextBlock DescriptionTextBlock;

		// Token: 0x040002AE RID: 686
		internal ComboBox ReasonComboBox;

		// Token: 0x040002AF RID: 687
		internal Button SubmitButton;

		// Token: 0x040002B0 RID: 688
		private bool _contentLoaded;
	}
}
