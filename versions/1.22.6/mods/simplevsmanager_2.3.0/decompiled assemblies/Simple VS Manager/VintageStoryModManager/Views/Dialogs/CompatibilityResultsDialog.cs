using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000023 RID: 35
	public class CompatibilityResultsDialog : Window, IComponentConnector
	{
		// Token: 0x06000441 RID: 1089 RVA: 0x00020AA8 File Offset: 0x0001ECA8
		[NullableContext(1)]
		public CompatibilityResultsDialog(Window owner, string targetVersion, IReadOnlyList<string> incompatibleMods, IReadOnlyList<string> unknownMods)
		{
			this.InitializeComponent();
			base.Owner = ((owner != null && owner.IsVisible) ? owner : null);
			base.WindowStartupLocation = ((base.Owner == null) ? 1 : 2);
			this.BuildMessage(targetVersion, incompatibleMods ?? Array.Empty<string>(), unknownMods ?? Array.Empty<string>());
			this.ConfigureIcon((incompatibleMods != null && incompatibleMods.Count > 0) ? 48 : 64);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00020B1C File Offset: 0x0001ED1C
		[NullableContext(1)]
		private void BuildMessage(string targetVersion, IReadOnlyList<string> incompatibleMods, IReadOnlyList<string> unknownMods)
		{
			this.MessageTextBlock.Inlines.Clear();
			string text = string.IsNullOrWhiteSpace(targetVersion) ? "Vintage Story version: Unknown." : ("Vintage Story version: " + targetVersion.Trim() + ".");
			this.MessageTextBlock.Inlines.Add(new Run(text));
			this.MessageTextBlock.Inlines.Add(new LineBreak());
			this.MessageTextBlock.Inlines.Add(new LineBreak());
			if (incompatibleMods.Count == 0)
			{
				this.MessageTextBlock.Inlines.Add(new Run("All installed mods appear to support this version."));
			}
			else
			{
				this.MessageTextBlock.Inlines.Add(new Run("Incompatible mods for " + targetVersion + ":"));
				this.MessageTextBlock.Inlines.Add(new LineBreak());
				foreach (string text2 in Enumerable.Where<string>(incompatibleMods, (string name) => !string.IsNullOrWhiteSpace(name)))
				{
					this.MessageTextBlock.Inlines.Add(new Run("• "));
					this.MessageTextBlock.Inlines.Add(new Bold(new Run(text2.Trim())));
					this.MessageTextBlock.Inlines.Add(new LineBreak());
				}
			}
			if (unknownMods.Count > 0)
			{
				if (incompatibleMods.Count > 0)
				{
					this.MessageTextBlock.Inlines.Add(new LineBreak());
				}
				this.MessageTextBlock.Inlines.Add(new Run("Compatibility could not be determined for:"));
				this.MessageTextBlock.Inlines.Add(new LineBreak());
				foreach (string text3 in Enumerable.Where<string>(unknownMods, (string name) => !string.IsNullOrWhiteSpace(name)))
				{
					this.MessageTextBlock.Inlines.Add(new Run("• "));
					this.MessageTextBlock.Inlines.Add(new Run(text3.Trim()));
					this.MessageTextBlock.Inlines.Add(new LineBreak());
				}
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00020DA0 File Offset: 0x0001EFA0
		private void ConfigureIcon(MessageBoxImage icon)
		{
			ImageSource imageSource;
			if (icon <= 32)
			{
				if (icon == 16)
				{
					imageSource = CompatibilityResultsDialog.ConvertIcon(SystemIcons.Error);
					goto IL_53;
				}
				if (icon == 32)
				{
					imageSource = CompatibilityResultsDialog.ConvertIcon(SystemIcons.Question);
					goto IL_53;
				}
			}
			else
			{
				if (icon == 48)
				{
					imageSource = CompatibilityResultsDialog.ConvertIcon(SystemIcons.Warning);
					goto IL_53;
				}
				if (icon == 64)
				{
					imageSource = CompatibilityResultsDialog.ConvertIcon(SystemIcons.Information);
					goto IL_53;
				}
			}
			imageSource = null;
			IL_53:
			ImageSource imageSource2 = imageSource;
			if (imageSource2 == null)
			{
				this.IconImage.Source = null;
				this.IconImage.Visibility = 2;
				return;
			}
			this.IconImage.Source = imageSource2;
			this.IconImage.Visibility = 0;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00020E36 File Offset: 0x0001F036
		[NullableContext(1)]
		[return: Nullable(2)]
		private static ImageSource ConvertIcon(Icon icon)
		{
			BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(icon.Width, icon.Height));
			bitmapSource.Freeze();
			return bitmapSource;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00020E5F File Offset: 0x0001F05F
		[NullableContext(1)]
		private void OkButton_OnClick(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00020E70 File Offset: 0x0001F070
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/compatibilityresultsdialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00020EA0 File Offset: 0x0001F0A0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.IconImage = (Image)target;
				return;
			case 2:
				this.MessageTextBlock = (TextBlock)target;
				return;
			case 3:
				((Button)target).Click += new RoutedEventHandler(this.OkButton_OnClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x04000248 RID: 584
		internal Image IconImage;

		// Token: 0x04000249 RID: 585
		internal TextBlock MessageTextBlock;

		// Token: 0x0400024A RID: 586
		private bool _contentLoaded;
	}
}
