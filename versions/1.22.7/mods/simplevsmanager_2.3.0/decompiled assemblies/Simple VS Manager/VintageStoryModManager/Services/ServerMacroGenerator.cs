using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000084 RID: 132
	[NullableContext(1)]
	[Nullable(0)]
	public static class ServerMacroGenerator
	{
		// Token: 0x06000B89 RID: 2953 RVA: 0x000479BC File Offset: 0x00045BBC
		public static string CreateDefaultMacroName()
		{
			string text = DateTime.UtcNow.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
			return "svsminstall" + text;
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x000479EC File Offset: 0x00045BEC
		public static ServerMacroGenerator.ServerMacroGenerationResult CreateInstallMacro(string filePath, string macroName, [TupleElementNames(new string[]
		{
			"ModId",
			"Version"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] IEnumerable<ValueTuple<string, string>> mods, [Nullable(2)] string description = null, [Nullable(2)] string privilege = null)
		{
			ArgumentNullException.ThrowIfNull(filePath, "filePath");
			ArgumentNullException.ThrowIfNull(mods, "mods");
			string text = ServerMacroGenerator.NormalizeMacroName(string.IsNullOrWhiteSpace(macroName) ? ServerMacroGenerator.CreateDefaultMacroName() : macroName);
			List<string> list = new List<string>();
			foreach (ValueTuple<string, string> valueTuple in mods)
			{
				string text2 = ServerCommandBuilder.TryBuildInstallCommand(valueTuple.Item1, valueTuple.Item2);
				if (!string.IsNullOrWhiteSpace(text2))
				{
					list.Add(text2);
				}
			}
			if (list.Count == 0)
			{
				return ServerMacroGenerator.ServerMacroGenerationResult.Empty;
			}
			ServerMacroGenerator.ServerMacroExport serverMacroExport = new ServerMacroGenerator.ServerMacroExport
			{
				Name = text,
				Privilege = (string.IsNullOrWhiteSpace(privilege) ? "controlserver" : privilege.Trim()),
				Commands = string.Join<string>('\n', list),
				Description = description,
				CreatedByPlayerUid = string.Empty,
				Syntax = string.Empty
			};
			string text3 = JsonSerializer.Serialize<ServerMacroGenerator.ServerMacroExport[]>(new ServerMacroGenerator.ServerMacroExport[]
			{
				serverMacroExport
			}, new JsonSerializerOptions
			{
				WriteIndented = true
			});
			string directoryName = Path.GetDirectoryName(filePath);
			if (!string.IsNullOrWhiteSpace(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			File.WriteAllText(filePath, text3, Encoding.UTF8);
			return new ServerMacroGenerator.ServerMacroGenerationResult(text, list.Count);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00047B3C File Offset: 0x00045D3C
		private static string NormalizeMacroName(string macroName)
		{
			ArgumentNullException.ThrowIfNull(macroName, "macroName");
			StringBuilder stringBuilder = new StringBuilder(macroName.Length);
			for (int i = 0; i < macroName.Length; i++)
			{
				char c = macroName.get_Chars(i);
				if (char.IsLetterOrDigit(c) || c == '-' || c == '_')
				{
					stringBuilder.Append(char.ToLowerInvariant(c));
				}
			}
			if (stringBuilder.Length == 0)
			{
				return ServerMacroGenerator.CreateDefaultMacroName();
			}
			if (!char.IsLetter(stringBuilder.get_Chars(0)))
			{
				stringBuilder.Insert(0, 'm');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000546 RID: 1350
		private const string DefaultPrivilege = "controlserver";

		// Token: 0x02000301 RID: 769
		[Nullable(0)]
		private sealed class ServerMacroExport
		{
			// Token: 0x17000529 RID: 1321
			// (get) Token: 0x060018F2 RID: 6386 RVA: 0x00091764 File Offset: 0x0008F964
			// (set) Token: 0x060018F3 RID: 6387 RVA: 0x0009176C File Offset: 0x0008F96C
			public string Privilege { get; set; } = string.Empty;

			// Token: 0x1700052A RID: 1322
			// (get) Token: 0x060018F4 RID: 6388 RVA: 0x00091775 File Offset: 0x0008F975
			// (set) Token: 0x060018F5 RID: 6389 RVA: 0x0009177D File Offset: 0x0008F97D
			public string Name { get; set; } = string.Empty;

			// Token: 0x1700052B RID: 1323
			// (get) Token: 0x060018F6 RID: 6390 RVA: 0x00091786 File Offset: 0x0008F986
			// (set) Token: 0x060018F7 RID: 6391 RVA: 0x0009178E File Offset: 0x0008F98E
			public string Commands { get; set; } = string.Empty;

			// Token: 0x1700052C RID: 1324
			// (get) Token: 0x060018F8 RID: 6392 RVA: 0x00091797 File Offset: 0x0008F997
			// (set) Token: 0x060018F9 RID: 6393 RVA: 0x0009179F File Offset: 0x0008F99F
			public string CreatedByPlayerUid { get; set; } = string.Empty;

			// Token: 0x1700052D RID: 1325
			// (get) Token: 0x060018FA RID: 6394 RVA: 0x000917A8 File Offset: 0x0008F9A8
			// (set) Token: 0x060018FB RID: 6395 RVA: 0x000917B0 File Offset: 0x0008F9B0
			public string Syntax { get; set; } = string.Empty;

			// Token: 0x1700052E RID: 1326
			// (get) Token: 0x060018FC RID: 6396 RVA: 0x000917B9 File Offset: 0x0008F9B9
			// (set) Token: 0x060018FD RID: 6397 RVA: 0x000917C1 File Offset: 0x0008F9C1
			[Nullable(2)]
			public string Description { [NullableContext(2)] get; [NullableContext(2)] set; }
		}

		// Token: 0x02000302 RID: 770
		[Nullable(0)]
		public readonly struct ServerMacroGenerationResult
		{
			// Token: 0x060018FF RID: 6399 RVA: 0x00091809 File Offset: 0x0008FA09
			public ServerMacroGenerationResult(string macroName, int commandCount)
			{
				this.MacroName = (macroName ?? string.Empty);
				this.CommandCount = ((commandCount < 0) ? 0 : commandCount);
			}

			// Token: 0x1700052F RID: 1327
			// (get) Token: 0x06001900 RID: 6400 RVA: 0x00091829 File Offset: 0x0008FA29
			public string MacroName { get; }

			// Token: 0x17000530 RID: 1328
			// (get) Token: 0x06001901 RID: 6401 RVA: 0x00091831 File Offset: 0x0008FA31
			public int CommandCount { get; }

			// Token: 0x17000531 RID: 1329
			// (get) Token: 0x06001902 RID: 6402 RVA: 0x00091839 File Offset: 0x0008FA39
			public string Command
			{
				get
				{
					if (!string.IsNullOrEmpty(this.MacroName))
					{
						return "/" + this.MacroName;
					}
					return string.Empty;
				}
			}

			// Token: 0x17000532 RID: 1330
			// (get) Token: 0x06001903 RID: 6403 RVA: 0x0009185E File Offset: 0x0008FA5E
			public bool HasMacro
			{
				get
				{
					return this.CommandCount > 0 && !string.IsNullOrEmpty(this.MacroName);
				}
			}

			// Token: 0x04001341 RID: 4929
			public static readonly ServerMacroGenerator.ServerMacroGenerationResult Empty = new ServerMacroGenerator.ServerMacroGenerationResult(string.Empty, 0);
		}
	}
}
