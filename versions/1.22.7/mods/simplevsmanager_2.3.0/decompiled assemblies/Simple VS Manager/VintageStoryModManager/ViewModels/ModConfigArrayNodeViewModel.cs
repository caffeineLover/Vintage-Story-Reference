using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x02000049 RID: 73
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModConfigArrayNodeViewModel : ModConfigContainerNodeViewModel
	{
		// Token: 0x06000757 RID: 1879 RVA: 0x0002E98C File Offset: 0x0002CB8C
		public ModConfigArrayNodeViewModel(string name, ObservableCollection<ModConfigNodeViewModel> children, Func<int> countAccessor, [Nullable(2)] string displayName = null) : base(name, "Array", children, displayName)
		{
			this._countAccessor = countAccessor;
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x0002E9A4 File Offset: 0x0002CBA4
		public int ItemCount
		{
			get
			{
				return this._countAccessor.Invoke();
			}
		}

		// Token: 0x040003C2 RID: 962
		private readonly Func<int> _countAccessor;
	}
}
