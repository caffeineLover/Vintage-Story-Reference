using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x0200002D RID: 45
	public class MessageDialogWindow : Window, IComponentConnector
	{
		// Token: 0x060004A5 RID: 1189 RVA: 0x00022136 File Offset: 0x00020336
		public MessageDialogWindow()
		{
			this.InitializeComponent();
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x00022144 File Offset: 0x00020344
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x0002214C File Offset: 0x0002034C
		public MessageBoxResult Result { get; private set; }

		// Token: 0x060004A8 RID: 1192 RVA: 0x00022158 File Offset: 0x00020358
		[NullableContext(2)]
		public void Initialize([Nullable(1)] string message, [Nullable(1)] string caption, MessageBoxButton buttons, MessageBoxImage icon, MessageDialogExtraButton extraButton = null, MessageDialogButtonContentOverrides buttonContentOverrides = null, string fixedFooterText = null)
		{
			this._buttons = buttons;
			base.Title = caption;
			this.MessageTextBlock.Text = message;
			this.FixedFooterTextBlock.Text = fixedFooterText;
			this.FixedFooterTextBlock.Visibility = (string.IsNullOrWhiteSpace(fixedFooterText) ? 2 : 0);
			this._buttonContentOverrides = buttonContentOverrides;
			this.ConfigureButtons(buttons);
			this.ConfigureExtraButton(extraButton);
			this.ConfigureIcon(icon);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x000221C4 File Offset: 0x000203C4
		private void ConfigureButtons(MessageBoxButton buttons)
		{
			this.ButtonOne.Visibility = 2;
			this.ButtonTwo.Visibility = 2;
			this.ButtonThree.Visibility = 2;
			this.ButtonOne.IsDefault = false;
			this.ButtonTwo.IsDefault = false;
			this.ButtonThree.IsDefault = false;
			this.ButtonExtra.IsDefault = false;
			this.ButtonOne.IsCancel = false;
			this.ButtonTwo.IsCancel = false;
			this.ButtonThree.IsCancel = false;
			this.ButtonExtra.IsCancel = false;
			switch (buttons)
			{
			case 0:
				this.ConfigureButton(this.ButtonOne, 1, "OK", true, true);
				return;
			case 1:
				this.ConfigureButton(this.ButtonOne, 2, "Cancel", false, true);
				this.ConfigureButton(this.ButtonTwo, 1, "OK", true, false);
				return;
			case 3:
				this.ConfigureButton(this.ButtonOne, 2, "Cancel", false, true);
				this.ConfigureButton(this.ButtonTwo, 6, "Yes", true, false);
				this.ConfigureButton(this.ButtonThree, 7, "No", false, false);
				return;
			case 4:
				this.ConfigureButton(this.ButtonOne, 6, "Yes", true, false);
				this.ConfigureButton(this.ButtonTwo, 7, "No", false, false);
				return;
			}
			this.ConfigureButton(this.ButtonOne, 1, "OK", true, true);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0002232C File Offset: 0x0002052C
		[NullableContext(2)]
		private void ConfigureExtraButton(MessageDialogExtraButton extraButton)
		{
			if (extraButton == null)
			{
				this.ButtonExtra.Visibility = 2;
				this.ButtonExtra.Tag = null;
				this._extraButtonCallback = null;
				return;
			}
			this.ButtonExtra.Content = extraButton.Content;
			this.ButtonExtra.Tag = extraButton.Result;
			this.ButtonExtra.Visibility = 0;
			this.ButtonExtra.IsDefault = extraButton.IsDefault;
			this.ButtonExtra.IsCancel = extraButton.IsCancel;
			this._extraButtonCallback = extraButton.OnClick;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x000223BD File Offset: 0x000205BD
		[NullableContext(1)]
		private void ConfigureButton(Button button, MessageBoxResult result, string defaultContent, bool isDefault = false, bool isCancel = false)
		{
			button.Content = this.GetButtonContent(result, defaultContent);
			button.Tag = result;
			button.Visibility = 0;
			button.IsDefault = isDefault;
			button.IsCancel = isCancel;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x000223F0 File Offset: 0x000205F0
		[NullableContext(1)]
		private object GetButtonContent(MessageBoxResult result, string defaultContent)
		{
			MessageDialogButtonContentOverrides buttonContentOverrides = this._buttonContentOverrides;
			string text = (buttonContentOverrides != null) ? buttonContentOverrides.GetContent(result) : null;
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return defaultContent;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0002241C File Offset: 0x0002061C
		private void ConfigureIcon(MessageBoxImage icon)
		{
			ImageSource imageSource;
			if (icon <= 32)
			{
				if (icon == 16)
				{
					imageSource = MessageDialogWindow.ConvertIcon(SystemIcons.Error);
					goto IL_53;
				}
				if (icon == 32)
				{
					imageSource = MessageDialogWindow.ConvertIcon(SystemIcons.Question);
					goto IL_53;
				}
			}
			else
			{
				if (icon == 48)
				{
					imageSource = MessageDialogWindow.ConvertIcon(SystemIcons.Warning);
					goto IL_53;
				}
				if (icon == 64)
				{
					imageSource = MessageDialogWindow.ConvertIcon(SystemIcons.Information);
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

		// Token: 0x060004AE RID: 1198 RVA: 0x000224B2 File Offset: 0x000206B2
		[NullableContext(1)]
		[return: Nullable(2)]
		private static ImageSource ConvertIcon(Icon icon)
		{
			BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(icon.Width, icon.Height));
			bitmapSource.Freeze();
			return bitmapSource;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x000224DC File Offset: 0x000206DC
		[NullableContext(1)]
		private void OnButtonClick(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				object tag = button.Tag;
				if (tag is MessageBoxResult)
				{
					MessageBoxResult result = (MessageBoxResult)tag;
					if (button == this.ButtonExtra)
					{
						Action extraButtonCallback = this._extraButtonCallback;
						if (extraButtonCallback != null)
						{
							extraButtonCallback.Invoke();
						}
					}
					this.Result = result;
					this._resultSet = true;
					base.DialogResult = new bool?(true);
				}
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00022540 File Offset: 0x00020740
		[NullableContext(1)]
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			if (this._resultSet)
			{
				return;
			}
			MessageBoxResult result;
			switch (this._buttons)
			{
			case 0:
				result = 1;
				goto IL_45;
			case 1:
				result = 2;
				goto IL_45;
			case 3:
				result = 2;
				goto IL_45;
			case 4:
				result = 0;
				goto IL_45;
			}
			result = 0;
			IL_45:
			this.Result = result;
			this._resultSet = true;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000225A0 File Offset: 0x000207A0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/messagedialogwindow.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x000225D0 File Offset: 0x000207D0
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
				this.FixedFooterTextBlock = (TextBlock)target;
				return;
			case 4:
				this.ButtonThree = (Button)target;
				this.ButtonThree.Click += new RoutedEventHandler(this.OnButtonClick);
				return;
			case 5:
				this.ButtonTwo = (Button)target;
				this.ButtonTwo.Click += new RoutedEventHandler(this.OnButtonClick);
				return;
			case 6:
				this.ButtonOne = (Button)target;
				this.ButtonOne.Click += new RoutedEventHandler(this.OnButtonClick);
				return;
			case 7:
				this.ButtonExtra = (Button)target;
				this.ButtonExtra.Click += new RoutedEventHandler(this.OnButtonClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x04000275 RID: 629
		[Nullable(2)]
		private MessageDialogButtonContentOverrides _buttonContentOverrides;

		// Token: 0x04000276 RID: 630
		private MessageBoxButton _buttons;

		// Token: 0x04000277 RID: 631
		[Nullable(2)]
		private Action _extraButtonCallback;

		// Token: 0x04000278 RID: 632
		private bool _resultSet;

		// Token: 0x0400027A RID: 634
		internal Image IconImage;

		// Token: 0x0400027B RID: 635
		internal TextBlock MessageTextBlock;

		// Token: 0x0400027C RID: 636
		internal TextBlock FixedFooterTextBlock;

		// Token: 0x0400027D RID: 637
		internal Button ButtonThree;

		// Token: 0x0400027E RID: 638
		internal Button ButtonTwo;

		// Token: 0x0400027F RID: 639
		internal Button ButtonOne;

		// Token: 0x04000280 RID: 640
		internal Button ButtonExtra;

		// Token: 0x04000281 RID: 641
		private bool _contentLoaded;
	}
}
