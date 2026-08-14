using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ModernWpf.Automation.Peers;

namespace ModernWpf.Controls
{
	// Token: 0x0200007E RID: 126
	public class PersonPicture : Control
	{
		// Token: 0x0600060E RID: 1550 RVA: 0x00016094 File Offset: 0x00014294
		static PersonPicture()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PersonPicture), new FrameworkPropertyMetadata(typeof(PersonPicture)));
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0001630E File Offset: 0x0001450E
		public PersonPicture()
		{
			this.TemplateSettings = new PersonPictureTemplateSettings();
			base.Unloaded += new RoutedEventHandler(this.OnUnloaded);
			base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00016345 File Offset: 0x00014545
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new PersonPictureAutomationPeer(this);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00016350 File Offset: 0x00014550
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.m_initialsTextBlock = (base.GetTemplateChild("InitialsTextBlock") as TextBlock);
			this.m_badgeNumberTextBlock = (base.GetTemplateChild("BadgeNumberTextBlock") as TextBlock);
			this.m_badgeGlyphIcon = (base.GetTemplateChild("BadgeGlyphIcon") as FontIcon);
			this.m_badgingEllipse = (base.GetTemplateChild("BadgingEllipse") as Ellipse);
			this.m_badgingBackgroundEllipse = (base.GetTemplateChild("BadgingBackgroundEllipse") as Ellipse);
			this.UpdateBadge();
			this.UpdateIfReady();
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x000163DD File Offset: 0x000145DD
		private string GetInitials()
		{
			if (!string.IsNullOrEmpty(this.Initials))
			{
				return this.Initials;
			}
			if (!string.IsNullOrEmpty(this.m_displayNameInitials))
			{
				return this.m_displayNameInitials;
			}
			return this.m_contactDisplayNameInitials;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0001640D File Offset: 0x0001460D
		private ImageSource GetImageSource()
		{
			if (this.ProfilePicture != null)
			{
				return this.ProfilePicture;
			}
			return this.m_contactImageSource;
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00016424 File Offset: 0x00014624
		private void UpdateIfReady()
		{
			string initials = this.GetInitials();
			ImageSource imageSource = this.GetImageSource();
			PersonPictureTemplateSettings templateSettings = this.TemplateSettings;
			templateSettings.ActualInitials = initials;
			if (imageSource != null)
			{
				ImageBrush imageBrush = templateSettings.ActualImageBrush;
				if (imageBrush == null)
				{
					imageBrush = new ImageBrush();
					imageBrush.Stretch = 3;
					templateSettings.ActualImageBrush = imageBrush;
				}
				imageBrush.ImageSource = imageSource;
			}
			else
			{
				templateSettings.ActualImageBrush = null;
			}
			if (this.IsGroup)
			{
				VisualStateManager.GoToState(this, "Group", false);
			}
			else if (imageSource != null)
			{
				VisualStateManager.GoToState(this, "Photo", false);
			}
			else if (!string.IsNullOrEmpty(initials))
			{
				VisualStateManager.GoToState(this, "Initials", false);
			}
			else
			{
				VisualStateManager.GoToState(this, "NoPhotoOrInitials", false);
			}
			this.UpdateAutomationName();
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x000164D4 File Offset: 0x000146D4
		private void UpdateBadge()
		{
			if (this.BadgeImageSource != null)
			{
				this.UpdateBadgeImageSource();
			}
			else if (this.BadgeNumber != 0)
			{
				this.UpdateBadgeNumber();
			}
			else if (!string.IsNullOrEmpty(this.BadgeGlyph))
			{
				this.UpdateBadgeGlyph();
			}
			else
			{
				VisualStateManager.GoToState(this, "NoBadge", false);
				TextBlock badgeNumberTextBlock = this.m_badgeNumberTextBlock;
				if (badgeNumberTextBlock != null)
				{
					badgeNumberTextBlock.Text = "";
				}
				FontIcon badgeGlyphIcon = this.m_badgeGlyphIcon;
				if (badgeGlyphIcon != null)
				{
					badgeGlyphIcon.Glyph = "";
				}
			}
			this.UpdateAutomationName();
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00016554 File Offset: 0x00014754
		private void UpdateBadgeNumber()
		{
			if (this.m_badgingEllipse == null || this.m_badgeNumberTextBlock == null)
			{
				return;
			}
			int badgeNumber = this.BadgeNumber;
			if (badgeNumber <= 0)
			{
				VisualStateManager.GoToState(this, "NoBadge", false);
				this.m_badgeNumberTextBlock.Text = "";
				return;
			}
			VisualStateManager.GoToState(this, "BadgeWithoutImageSource", false);
			if (badgeNumber <= 99)
			{
				this.m_badgeNumberTextBlock.Text = badgeNumber.ToString();
				return;
			}
			this.m_badgeNumberTextBlock.Text = "99+";
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x000165D0 File Offset: 0x000147D0
		private void UpdateBadgeGlyph()
		{
			if (this.m_badgingEllipse == null || this.m_badgeGlyphIcon == null)
			{
				return;
			}
			string badgeGlyph = this.BadgeGlyph;
			if (string.IsNullOrEmpty(badgeGlyph))
			{
				VisualStateManager.GoToState(this, "NoBadge", false);
				this.m_badgeGlyphIcon.Glyph = "";
				return;
			}
			VisualStateManager.GoToState(this, "BadgeWithoutImageSource", false);
			this.m_badgeGlyphIcon.Glyph = badgeGlyph;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00016634 File Offset: 0x00014834
		private void UpdateBadgeImageSource()
		{
			if (this.m_badgeImageBrush == null)
			{
				this.m_badgeImageBrush = (base.GetTemplateChild("BadgeImageBrush") as ImageBrush);
			}
			if (this.m_badgingEllipse == null || this.m_badgeImageBrush == null)
			{
				return;
			}
			this.m_badgeImageBrush.ImageSource = this.BadgeImageSource;
			if (this.BadgeImageSource != null)
			{
				VisualStateManager.GoToState(this, "BadgeWithImageSource", false);
				return;
			}
			VisualStateManager.GoToState(this, "NoBadge", false);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x000166A4 File Offset: 0x000148A4
		private void UpdateAutomationName()
		{
			string text;
			if (this.IsGroup)
			{
				text = PersonPicture.ResourceAccessor.GetLocalizedStringResource("GroupName");
			}
			else if (!string.IsNullOrEmpty(this.DisplayName))
			{
				text = this.DisplayName;
			}
			else if (!string.IsNullOrEmpty(this.Initials))
			{
				text = this.Initials;
			}
			else
			{
				text = PersonPicture.ResourceAccessor.GetLocalizedStringResource("PersonName");
			}
			string text2;
			if (this.BadgeNumber > 0)
			{
				if (!string.IsNullOrEmpty(this.BadgeText))
				{
					text2 = string.Format(PersonPicture.ResourceAccessor.GetLocalizedStringResource("BadgeItemTextOverride"), text, this.BadgeNumber, this.BadgeText);
				}
				else
				{
					text2 = string.Format(this.GetLocalizedPluralBadgeItemStringResource(this.BadgeNumber), text, this.BadgeNumber);
				}
			}
			else if (!string.IsNullOrEmpty(this.BadgeGlyph) || this.BadgeImageSource != null)
			{
				if (!string.IsNullOrEmpty(this.BadgeText))
				{
					text2 = string.Format(PersonPicture.ResourceAccessor.GetLocalizedStringResource("BadgeIconTextOverride"), text, this.BadgeText);
				}
				else
				{
					text2 = string.Format(PersonPicture.ResourceAccessor.GetLocalizedStringResource("BadgeIcon"), text);
				}
			}
			else
			{
				text2 = text;
			}
			AutomationProperties.SetName(this, text2);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x000167C8 File Offset: 0x000149C8
		private string GetLocalizedPluralBadgeItemStringResource(int numericValue)
		{
			int num = numericValue % 10;
			string localizedStringResource;
			if (numericValue == 1)
			{
				localizedStringResource = PersonPicture.ResourceAccessor.GetLocalizedStringResource("BadgeItemSingular");
			}
			else if (numericValue == 2)
			{
				localizedStringResource = PersonPicture.ResourceAccessor.GetLocalizedStringResource("BadgeItemPlural7");
			}
			else if (numericValue == 3 || numericValue == 4)
			{
				localizedStringResource = PersonPicture.ResourceAccessor.GetLocalizedStringResource("BadgeItemPlural2");
			}
			else if (numericValue >= 5 && numericValue <= 10)
			{
				localizedStringResource = PersonPicture.ResourceAccessor.GetLocalizedStringResource("BadgeItemPlural5");
			}
			else if (numericValue >= 11 && numericValue <= 19)
			{
				localizedStringResource = PersonPicture.ResourceAccessor.GetLocalizedStringResource("BadgeItemPlural6");
			}
			else if (num == 1)
			{
				localizedStringResource = PersonPicture.ResourceAccessor.GetLocalizedStringResource("BadgeItemPlural1");
			}
			else if (num >= 2 && num <= 4)
			{
				localizedStringResource = PersonPicture.ResourceAccessor.GetLocalizedStringResource("BadgeItemPlural3");
			}
			else
			{
				localizedStringResource = PersonPicture.ResourceAccessor.GetLocalizedStringResource("BadgeItemPlural4");
			}
			return localizedStringResource;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x000168A0 File Offset: 0x00014AA0
		private void PrivateOnPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			DependencyProperty property = args.Property;
			if (property == PersonPicture.BadgeNumberProperty || property == PersonPicture.BadgeGlyphProperty || property == PersonPicture.BadgeImageSourceProperty)
			{
				this.UpdateBadge();
				return;
			}
			if (property == PersonPicture.BadgeTextProperty)
			{
				this.UpdateAutomationName();
				return;
			}
			if (property == PersonPicture.DisplayNameProperty)
			{
				this.OnDisplayNameChanged(args);
				return;
			}
			if (property == PersonPicture.ProfilePictureProperty || property == PersonPicture.InitialsProperty || property == PersonPicture.IsGroupProperty)
			{
				this.UpdateIfReady();
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00016911 File Offset: 0x00014B11
		private void OnDisplayNameChanged(DependencyPropertyChangedEventArgs args)
		{
			this.m_displayNameInitials = InitialsGenerator.InitialsFromDisplayName(this.DisplayName);
			this.UpdateIfReady();
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0001692C File Offset: 0x00014B2C
		private void OnSizeChanged(object sender, SizeChangedEventArgs args)
		{
			bool flag = args.NewSize.Width != args.PreviousSize.Width;
			bool flag2 = args.NewSize.Height != args.PreviousSize.Height;
			double num;
			if (flag && flag2)
			{
				num = ((args.NewSize.Width < args.NewSize.Height) ? args.NewSize.Width : args.NewSize.Height);
			}
			else if (flag)
			{
				num = args.NewSize.Width;
			}
			else
			{
				if (!flag2)
				{
					return;
				}
				num = args.NewSize.Height;
			}
			base.Height = num;
			base.Width = num;
			double fontSize = Math.Max(1.0, base.Width * 0.42);
			TextBlock initialsTextBlock = this.m_initialsTextBlock;
			if (initialsTextBlock != null)
			{
				initialsTextBlock.FontSize = fontSize;
			}
			if (this.m_badgingEllipse != null && this.m_badgingBackgroundEllipse != null && this.m_badgeNumberTextBlock != null && this.m_badgeGlyphIcon != null)
			{
				double num2 = (args.NewSize.Width < args.NewSize.Height) ? args.NewSize.Width : args.NewSize.Height;
				this.m_badgingEllipse.Height = num2 * 0.5;
				this.m_badgingEllipse.Width = num2 * 0.5;
				this.m_badgingBackgroundEllipse.Height = num2 * 0.5;
				this.m_badgingBackgroundEllipse.Width = num2 * 0.5;
				this.m_badgeNumberTextBlock.FontSize = Math.Max(1.0, this.m_badgingEllipse.Height * 0.6);
				this.m_badgeGlyphIcon.FontSize = Math.Max(1.0, this.m_badgingEllipse.Height * 0.6);
			}
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00016B5D File Offset: 0x00014D5D
		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x00016B5F File Offset: 0x00014D5F
		// (set) Token: 0x06000620 RID: 1568 RVA: 0x00016B71 File Offset: 0x00014D71
		public string BadgeGlyph
		{
			get
			{
				return (string)base.GetValue(PersonPicture.BadgeGlyphProperty);
			}
			set
			{
				base.SetValue(PersonPicture.BadgeGlyphProperty, value);
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00016B7F File Offset: 0x00014D7F
		private static void OnBadgeGlyphPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((PersonPicture)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x00016B8D File Offset: 0x00014D8D
		// (set) Token: 0x06000623 RID: 1571 RVA: 0x00016B9F File Offset: 0x00014D9F
		public ImageSource BadgeImageSource
		{
			get
			{
				return (ImageSource)base.GetValue(PersonPicture.BadgeImageSourceProperty);
			}
			set
			{
				base.SetValue(PersonPicture.BadgeImageSourceProperty, value);
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00016BAD File Offset: 0x00014DAD
		private static void OnBadgeImageSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((PersonPicture)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x00016BBB File Offset: 0x00014DBB
		// (set) Token: 0x06000626 RID: 1574 RVA: 0x00016BCD File Offset: 0x00014DCD
		public int BadgeNumber
		{
			get
			{
				return (int)base.GetValue(PersonPicture.BadgeNumberProperty);
			}
			set
			{
				base.SetValue(PersonPicture.BadgeNumberProperty, value);
			}
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00016BE0 File Offset: 0x00014DE0
		private static void OnBadgeNumberPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((PersonPicture)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x00016BEE File Offset: 0x00014DEE
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x00016C00 File Offset: 0x00014E00
		public string BadgeText
		{
			get
			{
				return (string)base.GetValue(PersonPicture.BadgeTextProperty);
			}
			set
			{
				base.SetValue(PersonPicture.BadgeTextProperty, value);
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00016C0E File Offset: 0x00014E0E
		private static void OnBadgeTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((PersonPicture)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x00016C1C File Offset: 0x00014E1C
		// (set) Token: 0x0600062C RID: 1580 RVA: 0x00016C2E File Offset: 0x00014E2E
		public string DisplayName
		{
			get
			{
				return (string)base.GetValue(PersonPicture.DisplayNameProperty);
			}
			set
			{
				base.SetValue(PersonPicture.DisplayNameProperty, value);
			}
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00016C3C File Offset: 0x00014E3C
		private static void OnDisplayNamePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((PersonPicture)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x00016C4A File Offset: 0x00014E4A
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x00016C5C File Offset: 0x00014E5C
		public string Initials
		{
			get
			{
				return (string)base.GetValue(PersonPicture.InitialsProperty);
			}
			set
			{
				base.SetValue(PersonPicture.InitialsProperty, value);
			}
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00016C6A File Offset: 0x00014E6A
		private static void OnInitialsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((PersonPicture)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x00016C78 File Offset: 0x00014E78
		// (set) Token: 0x06000632 RID: 1586 RVA: 0x00016C8A File Offset: 0x00014E8A
		public bool IsGroup
		{
			get
			{
				return (bool)base.GetValue(PersonPicture.IsGroupProperty);
			}
			set
			{
				base.SetValue(PersonPicture.IsGroupProperty, value);
			}
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00016C9D File Offset: 0x00014E9D
		private static void OnIsGroupPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((PersonPicture)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x00016CAB File Offset: 0x00014EAB
		// (set) Token: 0x06000635 RID: 1589 RVA: 0x00016CBD File Offset: 0x00014EBD
		public ImageSource ProfilePicture
		{
			get
			{
				return (ImageSource)base.GetValue(PersonPicture.ProfilePictureProperty);
			}
			set
			{
				base.SetValue(PersonPicture.ProfilePictureProperty, value);
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00016CCB File Offset: 0x00014ECB
		private static void OnProfilePicturePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((PersonPicture)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x00016CD9 File Offset: 0x00014ED9
		// (set) Token: 0x06000638 RID: 1592 RVA: 0x00016CEB File Offset: 0x00014EEB
		public PersonPictureTemplateSettings TemplateSettings
		{
			get
			{
				return (PersonPictureTemplateSettings)base.GetValue(PersonPicture.TemplateSettingsProperty);
			}
			private set
			{
				base.SetValue(PersonPicture.TemplateSettingsPropertyKey, value);
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00016CF9 File Offset: 0x00014EF9
		private static void OnTemplateSettingsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((PersonPicture)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00016D07 File Offset: 0x00014F07
		private static object CoerceStringProperty(DependencyObject d, object baseValue)
		{
			return baseValue ?? string.Empty;
		}

		// Token: 0x040002D0 RID: 720
		private static readonly ResourceAccessor ResourceAccessor = new ResourceAccessor(typeof(PersonPicture));

		// Token: 0x040002D1 RID: 721
		private TextBlock m_initialsTextBlock;

		// Token: 0x040002D2 RID: 722
		private TextBlock m_badgeNumberTextBlock;

		// Token: 0x040002D3 RID: 723
		private FontIcon m_badgeGlyphIcon;

		// Token: 0x040002D4 RID: 724
		private ImageBrush m_badgeImageBrush;

		// Token: 0x040002D5 RID: 725
		private Ellipse m_badgingEllipse;

		// Token: 0x040002D6 RID: 726
		private Ellipse m_badgingBackgroundEllipse;

		// Token: 0x040002D7 RID: 727
		private string m_displayNameInitials;

		// Token: 0x040002D8 RID: 728
		private string m_contactDisplayNameInitials;

		// Token: 0x040002D9 RID: 729
		private ImageSource m_contactImageSource;

		// Token: 0x040002DA RID: 730
		public static readonly DependencyProperty BadgeGlyphProperty = DependencyProperty.Register("BadgeGlyph", typeof(string), typeof(PersonPicture), new PropertyMetadata(string.Empty, new PropertyChangedCallback(PersonPicture.OnBadgeGlyphPropertyChanged), new CoerceValueCallback(PersonPicture.CoerceStringProperty)));

		// Token: 0x040002DB RID: 731
		public static readonly DependencyProperty BadgeImageSourceProperty = DependencyProperty.Register("BadgeImageSource", typeof(ImageSource), typeof(PersonPicture), new PropertyMetadata(null, new PropertyChangedCallback(PersonPicture.OnBadgeImageSourcePropertyChanged)));

		// Token: 0x040002DC RID: 732
		public static readonly DependencyProperty BadgeNumberProperty = DependencyProperty.Register("BadgeNumber", typeof(int), typeof(PersonPicture), new PropertyMetadata(0, new PropertyChangedCallback(PersonPicture.OnBadgeNumberPropertyChanged)));

		// Token: 0x040002DD RID: 733
		public static readonly DependencyProperty BadgeTextProperty = DependencyProperty.Register("BadgeText", typeof(string), typeof(PersonPicture), new PropertyMetadata(string.Empty, new PropertyChangedCallback(PersonPicture.OnBadgeTextPropertyChanged), new CoerceValueCallback(PersonPicture.CoerceStringProperty)));

		// Token: 0x040002DE RID: 734
		public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register("DisplayName", typeof(string), typeof(PersonPicture), new PropertyMetadata(string.Empty, new PropertyChangedCallback(PersonPicture.OnDisplayNamePropertyChanged), new CoerceValueCallback(PersonPicture.CoerceStringProperty)));

		// Token: 0x040002DF RID: 735
		public static readonly DependencyProperty InitialsProperty = DependencyProperty.Register("Initials", typeof(string), typeof(PersonPicture), new PropertyMetadata(string.Empty, new PropertyChangedCallback(PersonPicture.OnInitialsPropertyChanged), new CoerceValueCallback(PersonPicture.CoerceStringProperty)));

		// Token: 0x040002E0 RID: 736
		public static readonly DependencyProperty IsGroupProperty = DependencyProperty.Register("IsGroup", typeof(bool), typeof(PersonPicture), new PropertyMetadata(false, new PropertyChangedCallback(PersonPicture.OnIsGroupPropertyChanged)));

		// Token: 0x040002E1 RID: 737
		public static readonly DependencyProperty ProfilePictureProperty = DependencyProperty.Register("ProfilePicture", typeof(ImageSource), typeof(PersonPicture), new PropertyMetadata(null, new PropertyChangedCallback(PersonPicture.OnProfilePicturePropertyChanged)));

		// Token: 0x040002E2 RID: 738
		private static readonly DependencyPropertyKey TemplateSettingsPropertyKey = DependencyProperty.RegisterReadOnly("TemplateSettings", typeof(PersonPictureTemplateSettings), typeof(PersonPicture), new PropertyMetadata(null, new PropertyChangedCallback(PersonPicture.OnTemplateSettingsPropertyChanged)));

		// Token: 0x040002E3 RID: 739
		public static readonly DependencyProperty TemplateSettingsProperty = PersonPicture.TemplateSettingsPropertyKey.DependencyProperty;
	}
}
