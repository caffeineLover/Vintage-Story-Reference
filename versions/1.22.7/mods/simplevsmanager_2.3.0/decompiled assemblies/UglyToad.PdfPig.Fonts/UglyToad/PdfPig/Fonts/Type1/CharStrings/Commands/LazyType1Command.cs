using System;
using System.Diagnostics;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands
{
	// Token: 0x02000016 RID: 22
	internal class LazyType1Command
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00005F2B File Offset: 0x0000412B
		public string Name { get; }

		// Token: 0x06000094 RID: 148 RVA: 0x00005F33 File Offset: 0x00004133
		public LazyType1Command(string name, Action<Type1BuildCharContext> runCommand)
		{
			this.Name = name;
			if (runCommand == null)
			{
				throw new ArgumentNullException("runCommand");
			}
			this.runCommand = runCommand;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005F58 File Offset: 0x00004158
		[DebuggerStepThrough]
		public void Run(Type1BuildCharContext context)
		{
			this.runCommand.Invoke(context);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005F66 File Offset: 0x00004166
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x04000065 RID: 101
		private readonly Action<Type1BuildCharContext> runCommand;
	}
}
