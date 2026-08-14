using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.ComponentModel.__Internals
{
	// Token: 0x02000038 RID: 56
	[EditorBrowsable(1)]
	[Obsolete("This type is not intended to be used directly by user code")]
	public static class __ObservableValidatorHelper
	{
		// Token: 0x06000170 RID: 368 RVA: 0x000068BE File Offset: 0x00004ABE
		[NullableContext(1)]
		[EditorBrowsable(1)]
		[Obsolete("This method is not intended to be called directly by user code")]
		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "This helper is called by generated code from public APIs that have the proper annotations already (and we don't want generated code to produce warnings that developers cannot fix).")]
		public static void ValidateProperty(ObservableValidator instance, [Nullable(2)] object value, string propertyName)
		{
			instance.ValidateProperty(value, propertyName);
		}
	}
}
