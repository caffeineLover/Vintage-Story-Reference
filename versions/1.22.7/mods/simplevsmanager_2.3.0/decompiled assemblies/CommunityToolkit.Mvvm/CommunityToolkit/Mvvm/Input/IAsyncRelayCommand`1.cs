using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommunityToolkit.Mvvm.Input
{
	// Token: 0x02000024 RID: 36
	public interface IAsyncRelayCommand<[Nullable(2)] in T> : IAsyncRelayCommand, IRelayCommand, ICommand, INotifyPropertyChanged, IRelayCommand<T>
	{
		// Token: 0x060000F0 RID: 240
		[NullableContext(1)]
		Task ExecuteAsync([Nullable(2)] T parameter);
	}
}
