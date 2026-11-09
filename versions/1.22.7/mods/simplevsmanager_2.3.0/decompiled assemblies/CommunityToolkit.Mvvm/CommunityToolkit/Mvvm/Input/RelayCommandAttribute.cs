using System;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Input
{
	// Token: 0x02000021 RID: 33
	[NullableContext(2)]
	[Nullable(0)]
	[AttributeUsage(64, AllowMultiple = false, Inherited = false)]
	public sealed class RelayCommandAttribute : Attribute
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000051BB File Offset: 0x000033BB
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x000051C3 File Offset: 0x000033C3
		public string CanExecute { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x000051CC File Offset: 0x000033CC
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x000051D4 File Offset: 0x000033D4
		public bool AllowConcurrentExecutions { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x000051DD File Offset: 0x000033DD
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x000051E5 File Offset: 0x000033E5
		public bool FlowExceptionsToTaskScheduler { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000051EE File Offset: 0x000033EE
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x000051F6 File Offset: 0x000033F6
		public bool IncludeCancelCommand { get; set; }
	}
}
