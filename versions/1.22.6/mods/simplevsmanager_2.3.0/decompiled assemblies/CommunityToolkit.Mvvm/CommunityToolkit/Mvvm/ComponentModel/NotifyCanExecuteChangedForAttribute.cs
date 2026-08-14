using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.ComponentModel
{
	// Token: 0x0200002E RID: 46
	[NullableContext(1)]
	[Nullable(0)]
	[AttributeUsage(384, AllowMultiple = true, Inherited = false)]
	public sealed class NotifyCanExecuteChangedForAttribute : Attribute
	{
		// Token: 0x06000122 RID: 290 RVA: 0x000057BF File Offset: 0x000039BF
		public NotifyCanExecuteChangedForAttribute(string commandName)
		{
			this.CommandNames = new string[]
			{
				commandName
			};
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000057D7 File Offset: 0x000039D7
		public NotifyCanExecuteChangedForAttribute(string commandName, params string[] otherCommandNames)
		{
			this.CommandNames = Enumerable.ToArray<string>(Enumerable.Concat<string>(new string[]
			{
				commandName
			}, otherCommandNames));
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000124 RID: 292 RVA: 0x000057FA File Offset: 0x000039FA
		public string[] CommandNames { get; }
	}
}
