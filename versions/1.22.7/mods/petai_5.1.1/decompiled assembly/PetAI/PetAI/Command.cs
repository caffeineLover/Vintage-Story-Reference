using System;

namespace PetAI
{
	// Token: 0x0200000E RID: 14
	public class Command
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00003919 File Offset: 0x00001B19
		public EnumCommandType Type { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00003921 File Offset: 0x00001B21
		public string CommandName { get; }

		// Token: 0x06000041 RID: 65 RVA: 0x00003929 File Offset: 0x00001B29
		public Command(EnumCommandType type, string commandName)
		{
			this.CommandName = commandName;
			this.Type = type;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003940 File Offset: 0x00001B40
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is Command))
			{
				return false;
			}
			if (obj == this)
			{
				return true;
			}
			Command command = obj as Command;
			return this.CommandName == command.CommandName && this.Type == command.Type;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000398C File Offset: 0x00001B8C
		public override int GetHashCode()
		{
			return this.CommandName.GetHashCode();
		}
	}
}
