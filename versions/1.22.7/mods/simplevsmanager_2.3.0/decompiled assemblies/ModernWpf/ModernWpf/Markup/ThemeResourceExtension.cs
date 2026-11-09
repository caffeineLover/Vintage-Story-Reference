using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ModernWpf.Markup
{
	// Token: 0x02000256 RID: 598
	[TypeConverter(typeof(ThemeResouceExtensionConverter))]
	public class ThemeResourceExtension : DynamicResourceExtension
	{
		// Token: 0x0600112D RID: 4397 RVA: 0x00032B2B File Offset: 0x00030D2B
		public ThemeResourceExtension()
		{
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x00032B33 File Offset: 0x00030D33
		public ThemeResourceExtension(object resourceKey) : base(resourceKey)
		{
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x00032B3C File Offset: 0x00030D3C
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			string text = base.ResourceKey as string;
			if (text != null && text.StartsWith("SystemColor", 4))
			{
				return new Binding(text)
				{
					Source = ThemeResourceExtension.SystemColorsSource.Current
				}.ProvideValue(serviceProvider);
			}
			return base.ProvideValue(serviceProvider);
		}

		// Token: 0x02000411 RID: 1041
		private class SystemColorsSource : INotifyPropertyChanged
		{
			// Token: 0x06001BDA RID: 7130 RVA: 0x0004A9F3 File Offset: 0x00048BF3
			private SystemColorsSource()
			{
				SystemParameters.StaticPropertyChanged += new PropertyChangedEventHandler(this.OnSystemParametersPropertyChanged);
			}

			// Token: 0x1700050B RID: 1291
			// (get) Token: 0x06001BDB RID: 7131 RVA: 0x0004AA0C File Offset: 0x00048C0C
			public static ThemeResourceExtension.SystemColorsSource Current { get; } = new ThemeResourceExtension.SystemColorsSource();

			// Token: 0x1700050C RID: 1292
			// (get) Token: 0x06001BDC RID: 7132 RVA: 0x0004AA13 File Offset: 0x00048C13
			public Color SystemColorButtonFaceColor
			{
				get
				{
					return SystemColors.ControlColor;
				}
			}

			// Token: 0x1700050D RID: 1293
			// (get) Token: 0x06001BDD RID: 7133 RVA: 0x0004AA1A File Offset: 0x00048C1A
			public Color SystemColorButtonTextColor
			{
				get
				{
					return SystemColors.ControlTextColor;
				}
			}

			// Token: 0x1700050E RID: 1294
			// (get) Token: 0x06001BDE RID: 7134 RVA: 0x0004AA21 File Offset: 0x00048C21
			public Color SystemColorGrayTextColor
			{
				get
				{
					return SystemColors.GrayTextColor;
				}
			}

			// Token: 0x1700050F RID: 1295
			// (get) Token: 0x06001BDF RID: 7135 RVA: 0x0004AA28 File Offset: 0x00048C28
			public Color SystemColorHighlightColor
			{
				get
				{
					return SystemColors.HighlightColor;
				}
			}

			// Token: 0x17000510 RID: 1296
			// (get) Token: 0x06001BE0 RID: 7136 RVA: 0x0004AA2F File Offset: 0x00048C2F
			public Color SystemColorHighlightTextColor
			{
				get
				{
					return SystemColors.HighlightTextColor;
				}
			}

			// Token: 0x17000511 RID: 1297
			// (get) Token: 0x06001BE1 RID: 7137 RVA: 0x0004AA36 File Offset: 0x00048C36
			public Color SystemColorHotlightColor
			{
				get
				{
					return SystemColors.HotTrackColor;
				}
			}

			// Token: 0x17000512 RID: 1298
			// (get) Token: 0x06001BE2 RID: 7138 RVA: 0x0004AA3D File Offset: 0x00048C3D
			public Color SystemColorWindowColor
			{
				get
				{
					return SystemColors.WindowColor;
				}
			}

			// Token: 0x17000513 RID: 1299
			// (get) Token: 0x06001BE3 RID: 7139 RVA: 0x0004AA44 File Offset: 0x00048C44
			public Color SystemColorWindowTextColor
			{
				get
				{
					return SystemColors.WindowTextColor;
				}
			}

			// Token: 0x17000514 RID: 1300
			// (get) Token: 0x06001BE4 RID: 7140 RVA: 0x0004AA4B File Offset: 0x00048C4B
			public Color SystemColorActiveCaptionColor
			{
				get
				{
					return SystemColors.ActiveCaptionColor;
				}
			}

			// Token: 0x17000515 RID: 1301
			// (get) Token: 0x06001BE5 RID: 7141 RVA: 0x0004AA52 File Offset: 0x00048C52
			public Color SystemColorInactiveCaptionTextColor
			{
				get
				{
					return SystemColors.InactiveCaptionTextColor;
				}
			}

			// Token: 0x14000023 RID: 35
			// (add) Token: 0x06001BE6 RID: 7142 RVA: 0x0004AA5C File Offset: 0x00048C5C
			// (remove) Token: 0x06001BE7 RID: 7143 RVA: 0x0004AA94 File Offset: 0x00048C94
			public event PropertyChangedEventHandler PropertyChanged;

			// Token: 0x06001BE8 RID: 7144 RVA: 0x0004AAC9 File Offset: 0x00048CC9
			private void OnSystemParametersPropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				if (e.PropertyName == "HighContrast" && SystemParameters.HighContrast)
				{
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged == null)
					{
						return;
					}
					propertyChanged.Invoke(this, new PropertyChangedEventArgs(null));
				}
			}
		}
	}
}
