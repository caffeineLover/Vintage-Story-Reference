using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows.Markup;

namespace XamlGeneratedNamespace
{
	// Token: 0x0200000E RID: 14
	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
	[EditorBrowsable(1)]
	public sealed class GeneratedInternalTypeHelper : InternalTypeHelper
	{
		// Token: 0x06000063 RID: 99 RVA: 0x0000333C File Offset: 0x0000153C
		protected override object CreateInstance(Type type, CultureInfo culture)
		{
			return Activator.CreateInstance(type, 564, null, null, culture);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000334C File Offset: 0x0000154C
		protected override object GetPropertyValue(PropertyInfo propertyInfo, object target, CultureInfo culture)
		{
			return propertyInfo.GetValue(target, 0, null, null, culture);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003359 File Offset: 0x00001559
		protected override void SetPropertyValue(PropertyInfo propertyInfo, object target, object value, CultureInfo culture)
		{
			propertyInfo.SetValue(target, value, 0, null, null, culture);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003368 File Offset: 0x00001568
		protected override Delegate CreateDelegate(Type delegateType, object target, string handler)
		{
			return (Delegate)target.GetType().InvokeMember("_CreateDelegate", 292, null, target, new object[]
			{
				delegateType,
				handler
			}, null);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000033A0 File Offset: 0x000015A0
		protected override void AddEventHandler(EventInfo eventInfo, object target, Delegate handler)
		{
			eventInfo.AddEventHandler(target, handler);
		}
	}
}
