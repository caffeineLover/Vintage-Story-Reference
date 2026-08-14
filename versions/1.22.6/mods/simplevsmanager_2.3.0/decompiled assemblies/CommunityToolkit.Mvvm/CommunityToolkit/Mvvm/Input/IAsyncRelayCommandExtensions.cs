using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input.Internals;

namespace CommunityToolkit.Mvvm.Input
{
	// Token: 0x02000022 RID: 34
	public static class IAsyncRelayCommandExtensions
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x00005208 File Offset: 0x00003408
		[NullableContext(1)]
		public static ICommand CreateCancelCommand(this IAsyncRelayCommand command)
		{
			ArgumentNullException.ThrowIfNull(command, "command");
			ICancellationAwareCommand cancellationAwareCommand = command as ICancellationAwareCommand;
			if (cancellationAwareCommand != null && !cancellationAwareCommand.IsCancellationSupported)
			{
				return DisabledCommand.Instance;
			}
			return new CancelCommand(command);
		}
	}
}
