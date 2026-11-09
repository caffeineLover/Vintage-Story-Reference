using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x02000048 RID: 72
	public sealed class ModConfigObjectNodeViewModel : ModConfigContainerNodeViewModel
	{
		// Token: 0x06000756 RID: 1878 RVA: 0x0002E97C File Offset: 0x0002CB7C
		[NullableContext(1)]
		public ModConfigObjectNodeViewModel(string name, ObservableCollection<ModConfigNodeViewModel> children, [Nullable(2)] string displayName = null) : base(name, "Object", children, displayName)
		{
		}
	}
}
