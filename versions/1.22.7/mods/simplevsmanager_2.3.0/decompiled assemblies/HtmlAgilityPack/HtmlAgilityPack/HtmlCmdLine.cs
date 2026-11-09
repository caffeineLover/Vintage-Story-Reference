using System;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x0200000B RID: 11
	[NullableContext(1)]
	[Nullable(0)]
	internal class HtmlCmdLine
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00002B9A File Offset: 0x00000D9A
		static HtmlCmdLine()
		{
			HtmlCmdLine.ParseArgs();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002BA8 File Offset: 0x00000DA8
		internal static string GetOption(string name, string def)
		{
			string result = def;
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 1; i < commandLineArgs.Length; i++)
			{
				HtmlCmdLine.GetStringArg(commandLineArgs[i], name, ref result);
			}
			return result;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002BD8 File Offset: 0x00000DD8
		internal static string GetOption(int index, string def)
		{
			string result = def;
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			int num = 0;
			for (int i = 1; i < commandLineArgs.Length; i++)
			{
				if (HtmlCmdLine.GetStringArg(commandLineArgs[i], ref result))
				{
					if (index == num)
					{
						return result;
					}
					result = def;
					num++;
				}
			}
			return result;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002C18 File Offset: 0x00000E18
		internal static bool GetOption(string name, bool def)
		{
			bool result = def;
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 1; i < commandLineArgs.Length; i++)
			{
				HtmlCmdLine.GetBoolArg(commandLineArgs[i], name, ref result);
			}
			return result;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002C48 File Offset: 0x00000E48
		internal static int GetOption(string name, int def)
		{
			int result = def;
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 1; i < commandLineArgs.Length; i++)
			{
				HtmlCmdLine.GetIntArg(commandLineArgs[i], name, ref result);
			}
			return result;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002C78 File Offset: 0x00000E78
		private static void GetBoolArg(string Arg, string Name, ref bool ArgValue)
		{
			if (Arg.Length < Name.Length + 1)
			{
				return;
			}
			if ('/' != Arg.get_Chars(0) && '-' != Arg.get_Chars(0))
			{
				return;
			}
			if (Arg.Substring(1, Name.Length).ToLowerInvariant() == Name.ToLowerInvariant())
			{
				ArgValue = true;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002CD0 File Offset: 0x00000ED0
		private static void GetIntArg(string Arg, string Name, ref int ArgValue)
		{
			if (Arg.Length < Name.Length + 3)
			{
				return;
			}
			if ('/' != Arg.get_Chars(0) && '-' != Arg.get_Chars(0))
			{
				return;
			}
			if (Arg.Substring(1, Name.Length).ToLowerInvariant() == Name.ToLowerInvariant())
			{
				try
				{
					ArgValue = Convert.ToInt32(Arg.Substring(Name.Length + 2, Arg.Length - Name.Length - 2));
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002D60 File Offset: 0x00000F60
		private static bool GetStringArg(string Arg, ref string ArgValue)
		{
			if ('/' == Arg.get_Chars(0) || '-' == Arg.get_Chars(0))
			{
				return false;
			}
			ArgValue = Arg;
			return true;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002D80 File Offset: 0x00000F80
		private static void GetStringArg(string Arg, string Name, ref string ArgValue)
		{
			if (Arg.Length < Name.Length + 3)
			{
				return;
			}
			if ('/' != Arg.get_Chars(0) && '-' != Arg.get_Chars(0))
			{
				return;
			}
			if (Arg.Substring(1, Name.Length).ToLowerInvariant() == Name.ToLowerInvariant())
			{
				ArgValue = Arg.Substring(Name.Length + 2, Arg.Length - Name.Length - 2);
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002DF4 File Offset: 0x00000FF4
		private static void ParseArgs()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 1; i < commandLineArgs.Length; i++)
			{
				HtmlCmdLine.GetBoolArg(commandLineArgs[i], "?", ref HtmlCmdLine.Help);
				HtmlCmdLine.GetBoolArg(commandLineArgs[i], "h", ref HtmlCmdLine.Help);
				HtmlCmdLine.GetBoolArg(commandLineArgs[i], "help", ref HtmlCmdLine.Help);
			}
		}

		// Token: 0x0400001F RID: 31
		internal static bool Help = false;
	}
}
