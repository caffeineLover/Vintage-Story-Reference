using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CommunityToolkit.Mvvm.Input.Internals
{
	// Token: 0x02000029 RID: 41
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class CancelCommand : ICommand
	{
		// Token: 0x06000107 RID: 263 RVA: 0x000055AE File Offset: 0x000037AE
		[NullableContext(1)]
		public CancelCommand(IAsyncRelayCommand command)
		{
			this.command = command;
			this.command.PropertyChanged += new PropertyChangedEventHandler(this.OnPropertyChanged);
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000108 RID: 264 RVA: 0x000055D4 File Offset: 0x000037D4
		// (remove) Token: 0x06000109 RID: 265 RVA: 0x0000560C File Offset: 0x0000380C
		public event EventHandler CanExecuteChanged;

		// Token: 0x0600010A RID: 266 RVA: 0x00005641 File Offset: 0x00003841
		public bool CanExecute(object parameter)
		{
			return this.command.CanBeCanceled;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000564E File Offset: 0x0000384E
		public void Execute(object parameter)
		{
			this.command.Cancel();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000565C File Offset: 0x0000385C
		[NullableContext(1)]
		private void OnPropertyChanged([Nullable(2)] object sender, PropertyChangedEventArgs e)
		{
			string propertyName = e.PropertyName;
			bool flag = propertyName == null || propertyName == "CanBeCanceled";
			if (flag)
			{
				EventHandler canExecuteChanged = this.CanExecuteChanged;
				if (canExecuteChanged == null)
				{
					return;
				}
				canExecuteChanged.Invoke(this, EventArgs.Empty);
			}
		}

		// Token: 0x0400005C RID: 92
		[Nullable(1)]
		private readonly IAsyncRelayCommand command;
	}
}
