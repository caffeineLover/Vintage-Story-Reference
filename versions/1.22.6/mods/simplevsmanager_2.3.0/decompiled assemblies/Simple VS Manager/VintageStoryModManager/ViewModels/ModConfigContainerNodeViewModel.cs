using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x02000047 RID: 71
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ModConfigContainerNodeViewModel : ModConfigNodeViewModel
	{
		// Token: 0x06000753 RID: 1875 RVA: 0x0002E913 File Offset: 0x0002CB13
		protected ModConfigContainerNodeViewModel(string name, string typeDisplay, ObservableCollection<ModConfigNodeViewModel> children, [Nullable(2)] string displayName = null) : base(name, typeDisplay, displayName)
		{
			this._children = children;
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x0002E926 File Offset: 0x0002CB26
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public override ObservableCollection<ModConfigNodeViewModel> Children
		{
			[return: Nullable(new byte[]
			{
				2,
				1
			})]
			get
			{
				return this._children;
			}
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0002E930 File Offset: 0x0002CB30
		public override void ApplyChanges()
		{
			foreach (ModConfigNodeViewModel modConfigNodeViewModel in this._children)
			{
				modConfigNodeViewModel.ApplyChanges();
			}
		}

		// Token: 0x040003C1 RID: 961
		private readonly ObservableCollection<ModConfigNodeViewModel> _children;
	}
}
