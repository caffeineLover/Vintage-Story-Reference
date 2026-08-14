using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x0200002B RID: 43
	internal interface IControlProtected
	{
		// Token: 0x060001AA RID: 426
		DependencyObject GetTemplateChild(string childName);
	}
}
