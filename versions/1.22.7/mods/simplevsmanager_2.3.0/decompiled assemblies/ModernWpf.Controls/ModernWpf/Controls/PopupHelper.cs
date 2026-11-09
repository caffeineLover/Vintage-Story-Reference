using System;
using System.Windows.Controls.Primitives;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x0200002E RID: 46
	internal static class PopupHelper
	{
		// Token: 0x060001AF RID: 431 RVA: 0x0000651C File Offset: 0x0000471C
		public static void Reposition(this Popup popup)
		{
			if (popup == null)
			{
				throw new ArgumentNullException("popup");
			}
			PopupPositioner positioner = PopupPositioner.GetPositioner(popup);
			if (positioner != null)
			{
				positioner.Reposition();
				return;
			}
			Action<Popup> value = PopupHelper.s_reposition.Value;
			if (value != null)
			{
				value.Invoke(popup);
				return;
			}
			double horizontalOffset = popup.HorizontalOffset;
			popup.SetCurrentValue(Popup.HorizontalOffsetProperty, horizontalOffset + 0.1);
			popup.InvalidateProperty(Popup.HorizontalOffsetProperty);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000658C File Offset: 0x0000478C
		private static Action<Popup> CreateRepositionDelegate()
		{
			Action<Popup> result;
			try
			{
				result = DelegateHelper.CreateDelegate<Action<Popup>>(typeof(Popup), "Reposition", 36);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04000088 RID: 136
		private static readonly Lazy<Action<Popup>> s_reposition = new Lazy<Action<Popup>>(new Func<Action<Popup>>(PopupHelper.CreateRepositionDelegate));
	}
}
