using System;
using System.Windows;
using System.Windows.Media;

namespace ModernWpf.Controls
{
	// Token: 0x0200007F RID: 127
	public sealed class PersonPictureTemplateSettings : DependencyObject
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x00016D13 File Offset: 0x00014F13
		// (set) Token: 0x0600063C RID: 1596 RVA: 0x00016D25 File Offset: 0x00014F25
		public ImageBrush ActualImageBrush
		{
			get
			{
				return (ImageBrush)base.GetValue(PersonPictureTemplateSettings.ActualImageBrushProperty);
			}
			internal set
			{
				base.SetValue(PersonPictureTemplateSettings.ActualImageBrushPropertyKey, value);
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x00016D33 File Offset: 0x00014F33
		// (set) Token: 0x0600063E RID: 1598 RVA: 0x00016D45 File Offset: 0x00014F45
		public string ActualInitials
		{
			get
			{
				return (string)base.GetValue(PersonPictureTemplateSettings.ActualInitialsProperty);
			}
			internal set
			{
				base.SetValue(PersonPictureTemplateSettings.ActualInitialsPropertyKey, value);
			}
		}

		// Token: 0x040002E4 RID: 740
		private static readonly DependencyPropertyKey ActualImageBrushPropertyKey = DependencyProperty.RegisterReadOnly("ActualImageBrush", typeof(ImageBrush), typeof(PersonPictureTemplateSettings), null);

		// Token: 0x040002E5 RID: 741
		public static readonly DependencyProperty ActualImageBrushProperty = PersonPictureTemplateSettings.ActualImageBrushPropertyKey.DependencyProperty;

		// Token: 0x040002E6 RID: 742
		private static readonly DependencyPropertyKey ActualInitialsPropertyKey = DependencyProperty.RegisterReadOnly("ActualInitials", typeof(string), typeof(PersonPictureTemplateSettings), new PropertyMetadata(string.Empty));

		// Token: 0x040002E7 RID: 743
		public static readonly DependencyProperty ActualInitialsProperty = PersonPictureTemplateSettings.ActualInitialsPropertyKey.DependencyProperty;
	}
}
