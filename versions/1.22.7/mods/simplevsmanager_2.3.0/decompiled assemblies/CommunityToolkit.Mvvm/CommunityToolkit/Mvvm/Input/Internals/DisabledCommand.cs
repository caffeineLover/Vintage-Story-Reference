using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CommunityToolkit.Mvvm.Input.Internals
{
	// Token: 0x0200002A RID: 42
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class DisabledCommand : ICommand
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600010D RID: 269 RVA: 0x0000569F File Offset: 0x0000389F
		// (remove) Token: 0x0600010E RID: 270 RVA: 0x000056A1 File Offset: 0x000038A1
		public event EventHandler CanExecuteChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000056A3 File Offset: 0x000038A3
		[Nullable(1)]
		public static DisabledCommand Instance { [NullableContext(1)] get; } = new DisabledCommand();

		// Token: 0x06000110 RID: 272 RVA: 0x000056AA File Offset: 0x000038AA
		public bool CanExecute(object parameter)
		{
			return false;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000056AD File Offset: 0x000038AD
		public void Execute(object parameter)
		{
		}
	}
}
