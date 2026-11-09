using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CommunityToolkit.Mvvm.Input
{
	// Token: 0x02000026 RID: 38
	[NullableContext(2)]
	public interface IRelayCommand<in T> : IRelayCommand, ICommand
	{
		// Token: 0x060000F2 RID: 242
		bool CanExecute(T parameter);

		// Token: 0x060000F3 RID: 243
		void Execute(T parameter);
	}
}
