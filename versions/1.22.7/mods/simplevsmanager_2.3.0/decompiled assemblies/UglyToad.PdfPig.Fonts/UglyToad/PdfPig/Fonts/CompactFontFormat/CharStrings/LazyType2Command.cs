using System;
using System.Diagnostics;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.CharStrings
{
	// Token: 0x0200009E RID: 158
	internal class LazyType2Command
	{
		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0001A462 File Offset: 0x00018662
		public string Name { get; }

		// Token: 0x060003C2 RID: 962 RVA: 0x0001A46A File Offset: 0x0001866A
		public LazyType2Command(string name, int minimumStackParameters, Action<Type2BuildCharContext> runCommand)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.Name = name;
			this.minimumStackParameters = minimumStackParameters;
			if (runCommand == null)
			{
				throw new ArgumentNullException("runCommand");
			}
			this.runCommand = runCommand;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0001A4A5 File Offset: 0x000186A5
		[DebuggerStepThrough]
		public void Run(Type2BuildCharContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.Stack.Length < this.minimumStackParameters)
			{
				context.Stack.Clear();
				return;
			}
			this.runCommand.Invoke(context);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0001A4E0 File Offset: 0x000186E0
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x04000359 RID: 857
		private readonly int minimumStackParameters;

		// Token: 0x0400035A RID: 858
		private readonly Action<Type2BuildCharContext> runCommand;
	}
}
