using System;
using System.Windows.Input;

namespace CommunityToolkit.Mvvm.Input
{
	// Token: 0x02000025 RID: 37
	public interface IRelayCommand : ICommand
	{
		// Token: 0x060000F1 RID: 241
		void NotifyCanExecuteChanged();
	}
}
