using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommunityToolkit.Mvvm.Input
{
	// Token: 0x02000023 RID: 35
	public interface IAsyncRelayCommand : IRelayCommand, ICommand, INotifyPropertyChanged
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000EA RID: 234
		[Nullable(2)]
		Task ExecutionTask { [NullableContext(2)] get; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000EB RID: 235
		bool CanBeCanceled { get; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000EC RID: 236
		bool IsCancellationRequested { get; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000ED RID: 237
		bool IsRunning { get; }

		// Token: 0x060000EE RID: 238
		[NullableContext(1)]
		Task ExecuteAsync([Nullable(2)] object parameter);

		// Token: 0x060000EF RID: 239
		void Cancel();
	}
}
