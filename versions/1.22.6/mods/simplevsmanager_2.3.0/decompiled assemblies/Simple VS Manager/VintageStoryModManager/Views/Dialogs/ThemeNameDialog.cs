using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using VintageStoryModManager.Services;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x0200003B RID: 59
	[NullableContext(1)]
	[Nullable(0)]
	public class ThemeNameDialog : Window, IComponentConnector
	{
		// Token: 0x06000519 RID: 1305 RVA: 0x00023DFC File Offset: 0x00021FFC
		public ThemeNameDialog([Nullable(2)] string initialName, UserConfigurationService configuration)
		{
			this.InitializeComponent();
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}
			this._configuration = configuration;
			this.ThemeName = (initialName ?? string.Empty);
			base.DataContext = this;
			this.UpdateSaveButtonText();
			this.ThemeNameTextBox.TextChanged += delegate(object s, TextChangedEventArgs e)
			{
				this.UpdateSaveButtonText();
			};
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x00023E5F File Offset: 0x0002205F
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x00023E67 File Offset: 0x00022067
		public string ThemeName { get; set; }

		// Token: 0x0600051C RID: 1308 RVA: 0x00023E70 File Offset: 0x00022070
		private void UpdateSaveButtonText()
		{
			ThemeNameDialog.<>c__DisplayClass6_0 CS$<>8__locals1 = new ThemeNameDialog.<>c__DisplayClass6_0();
			ThemeNameDialog.<>c__DisplayClass6_0 CS$<>8__locals2 = CS$<>8__locals1;
			string text = this.ThemeNameTextBox.Text;
			CS$<>8__locals2.currentText = (((text != null) ? text.Trim() : null) ?? string.Empty);
			if (string.IsNullOrWhiteSpace(CS$<>8__locals1.currentText))
			{
				this.SaveButton.Content = "Save";
				return;
			}
			bool flag = Enumerable.Any<string>(this._configuration.GetAllThemeNames(), (string name) => string.Equals(name, CS$<>8__locals1.currentText, 5));
			this.SaveButton.Content = (flag ? "Replace" : "Save");
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00023F00 File Offset: 0x00022100
		private void OkButton_OnClick(object sender, RoutedEventArgs e)
		{
			string text = this.ThemeNameTextBox.Text;
			string text2 = (text != null) ? text.Trim() : null;
			if (string.IsNullOrWhiteSpace(text2))
			{
				ModManagerMessageBox.Show(this, "Please enter a theme name.", "Simple VS Manager", 0, 48, null, null, null);
				return;
			}
			if (ThemePaletteEditorDialog.ThemeOption.IsBuiltInThemeName(text2))
			{
				ModManagerMessageBox.Show(this, "Cannot save theme with a built-in theme name. Please choose a different name.", "Simple VS Manager", 0, 48, null, null, null);
				return;
			}
			this.ThemeName = text2;
			base.DialogResult = new bool?(true);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00023F78 File Offset: 0x00022178
		private void CancelButton_OnClick(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00023F88 File Offset: 0x00022188
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/themenamedialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00023FB8 File Offset: 0x000221B8
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.ThemeNameTextBox = (TextBox)target;
				return;
			case 2:
				this.SaveButton = (Button)target;
				this.SaveButton.Click += new RoutedEventHandler(this.OkButton_OnClick);
				return;
			case 3:
				((Button)target).Click += new RoutedEventHandler(this.CancelButton_OnClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x040002C3 RID: 707
		private readonly UserConfigurationService _configuration;

		// Token: 0x040002C5 RID: 709
		[Nullable(0)]
		internal TextBox ThemeNameTextBox;

		// Token: 0x040002C6 RID: 710
		[Nullable(0)]
		internal Button SaveButton;

		// Token: 0x040002C7 RID: 711
		private bool _contentLoaded;
	}
}
