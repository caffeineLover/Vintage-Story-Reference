using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CommunityToolkit.Mvvm.Input
{
	// Token: 0x02000027 RID: 39
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class RelayCommand : IRelayCommand, ICommand
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060000F4 RID: 244 RVA: 0x00005240 File Offset: 0x00003440
		// (remove) Token: 0x060000F5 RID: 245 RVA: 0x00005278 File Offset: 0x00003478
		public event EventHandler CanExecuteChanged;

		// Token: 0x060000F6 RID: 246 RVA: 0x000052AD File Offset: 0x000034AD
		[NullableContext(1)]
		public RelayCommand(Action execute)
		{
			ArgumentNullException.ThrowIfNull(execute, "execute");
			this.execute = execute;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000052C7 File Offset: 0x000034C7
		[NullableContext(1)]
		public RelayCommand(Action execute, Func<bool> canExecute)
		{
			ArgumentNullException.ThrowIfNull(execute, "execute");
			ArgumentNullException.ThrowIfNull(canExecute, "canExecute");
			this.execute = execute;
			this.canExecute = canExecute;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000052F3 File Offset: 0x000034F3
		public void NotifyCanExecuteChanged()
		{
			EventHandler canExecuteChanged = this.CanExecuteChanged;
			if (canExecuteChanged == null)
			{
				return;
			}
			canExecuteChanged.Invoke(this, EventArgs.Empty);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000530B File Offset: 0x0000350B
		[MethodImpl(256)]
		public bool CanExecute(object parameter)
		{
			Func<bool> func = this.canExecute;
			return func == null || func.Invoke();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000531E File Offset: 0x0000351E
		public void Execute(object parameter)
		{
			this.execute.Invoke();
		}

		// Token: 0x04000056 RID: 86
		[Nullable(1)]
		private readonly Action execute;

		// Token: 0x04000057 RID: 87
		private readonly Func<bool> canExecute;
	}
}
