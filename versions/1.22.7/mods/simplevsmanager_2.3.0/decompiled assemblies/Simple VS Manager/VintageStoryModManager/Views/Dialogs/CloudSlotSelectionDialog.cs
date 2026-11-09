using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000022 RID: 34
	public class CloudSlotSelectionDialog : Window, IComponentConnector
	{
		// Token: 0x06000439 RID: 1081 RVA: 0x000208AC File Offset: 0x0001EAAC
		[NullableContext(1)]
		public CloudSlotSelectionDialog(Window owner, IEnumerable<CloudModlistSlot> slots, string title, string prompt, [Nullable(2)] string defaultSlotKey = null)
		{
			this.InitializeComponent();
			base.Owner = owner;
			base.Title = title;
			this.PromptTextBlock.Text = prompt;
			this._slots = (((slots != null) ? Enumerable.ToList<CloudModlistSlot>(slots) : null) ?? new List<CloudModlistSlot>());
			this.SlotsListBox.ItemsSource = this._slots;
			if (!string.IsNullOrWhiteSpace(defaultSlotKey))
			{
				int num = this._slots.FindIndex((CloudModlistSlot slot) => string.Equals(slot.SlotKey, defaultSlotKey, 5));
				if (num >= 0)
				{
					this.SlotsListBox.SelectedIndex = num;
				}
			}
			if (this.SlotsListBox.SelectedIndex < 0 && this._slots.Count > 0)
			{
				this.SlotsListBox.SelectedIndex = 0;
			}
			this.UpdateSelectButtonState();
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x0002097D File Offset: 0x0001EB7D
		[Nullable(2)]
		public CloudModlistSlot SelectedSlot
		{
			[NullableContext(2)]
			get
			{
				return this.SlotsListBox.SelectedItem as CloudModlistSlot;
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0002098F File Offset: 0x0001EB8F
		[NullableContext(1)]
		private void SelectButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (this.SelectedSlot == null)
			{
				return;
			}
			base.DialogResult = new bool?(true);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x000209A6 File Offset: 0x0001EBA6
		[NullableContext(1)]
		private void SlotsListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (this.SelectedSlot == null)
			{
				return;
			}
			base.DialogResult = new bool?(true);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x000209BD File Offset: 0x0001EBBD
		[NullableContext(1)]
		private void SlotsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.UpdateSelectButtonState();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000209C5 File Offset: 0x0001EBC5
		private void UpdateSelectButtonState()
		{
			this.SelectButton.IsEnabled = (this.SelectedSlot != null);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000209E0 File Offset: 0x0001EBE0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/cloudslotselectiondialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00020A10 File Offset: 0x0001EC10
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.PromptTextBlock = (TextBlock)target;
				return;
			case 2:
				this.SlotsListBox = (ListBox)target;
				this.SlotsListBox.MouseDoubleClick += new MouseButtonEventHandler(this.SlotsListBox_OnMouseDoubleClick);
				this.SlotsListBox.SelectionChanged += new SelectionChangedEventHandler(this.SlotsListBox_OnSelectionChanged);
				return;
			case 3:
				this.SelectButton = (Button)target;
				this.SelectButton.Click += new RoutedEventHandler(this.SelectButton_OnClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x04000243 RID: 579
		[Nullable(1)]
		private readonly List<CloudModlistSlot> _slots;

		// Token: 0x04000244 RID: 580
		internal TextBlock PromptTextBlock;

		// Token: 0x04000245 RID: 581
		internal ListBox SlotsListBox;

		// Token: 0x04000246 RID: 582
		internal Button SelectButton;

		// Token: 0x04000247 RID: 583
		private bool _contentLoaded;
	}
}
