using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VintageStoryModManager.Helpers
{
	// Token: 0x020000C6 RID: 198
	[NullableContext(1)]
	[Nullable(0)]
	public static class HtmlDocumentBehavior
	{
		// Token: 0x06001005 RID: 4101 RVA: 0x00054D64 File Offset: 0x00052F64
		[return: Nullable(2)]
		public static string GetHtml(DependencyObject obj)
		{
			return (string)obj.GetValue(HtmlDocumentBehavior.HtmlProperty);
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x00054D76 File Offset: 0x00052F76
		public static void SetHtml(DependencyObject obj, [Nullable(2)] string value)
		{
			obj.SetValue(HtmlDocumentBehavior.HtmlProperty, value);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x00054D84 File Offset: 0x00052F84
		private static void OnHtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FlowDocumentScrollViewer flowDocumentScrollViewer = d as FlowDocumentScrollViewer;
			if (flowDocumentScrollViewer == null)
			{
				return;
			}
			string html = e.NewValue as string;
			Brush foreground = flowDocumentScrollViewer.Foreground;
			FontFamily fontFamily = flowDocumentScrollViewer.FontFamily;
			double fontSize = flowDocumentScrollViewer.FontSize;
			flowDocumentScrollViewer.Document = HtmlToFlowDocument.Convert(html, foreground, fontFamily, fontSize);
		}

		// Token: 0x04000703 RID: 1795
		public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached("Html", typeof(string), typeof(HtmlDocumentBehavior), new PropertyMetadata(null, new PropertyChangedCallback(HtmlDocumentBehavior.OnHtmlChanged)));
	}
}
