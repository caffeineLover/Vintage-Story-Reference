using System;
using System.Windows.Media;

namespace ModernWpf
{
	// Token: 0x02000216 RID: 534
	internal static class Extensions
	{
		// Token: 0x06000FA9 RID: 4009 RVA: 0x0002CB75 File Offset: 0x0002AD75
		public static GeneralTransform SafeTransformToVisual(this Visual self, Visual visual)
		{
			if (self.FindCommonVisualAncestor(visual) != null)
			{
				return self.TransformToVisual(visual);
			}
			return Transform.Identity;
		}
	}
}
