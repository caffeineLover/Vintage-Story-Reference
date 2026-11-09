using System;
using System.Collections.Generic;
using System.IO;

namespace Xabe.FFmpeg
{
	// Token: 0x02000014 RID: 20
	public class InputBuilder : IInputBuilder
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004281 File Offset: 0x00002481
		public List<FileInfo> FileList { get; }

		// Token: 0x060000A0 RID: 160 RVA: 0x00004289 File Offset: 0x00002489
		public InputBuilder()
		{
			this.FileList = new List<FileInfo>();
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000429C File Offset: 0x0000249C
		public Func<string, string> PrepareInputFiles(List<string> files, out string directory)
		{
			Guid guid = Guid.NewGuid();
			for (int i = 0; i < files.Count; i++)
			{
				string text = Path.Combine(Path.GetTempPath(), guid.ToString(), this.BuildFileName(i + 1, Path.GetExtension(files[i])));
				if (!Directory.Exists(Path.Combine(Path.GetTempPath(), guid.ToString())))
				{
					Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), guid.ToString()));
				}
				File.Copy(files[i], text);
				this.FileList.Add(new FileInfo(text));
			}
			directory = Path.Combine(Path.GetTempPath(), guid.ToString());
			return (string number) => " -i " + Path.Combine(this.FileList[0].DirectoryName, "img" + number + this.FileList[0].Extension) + " ";
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004374 File Offset: 0x00002574
		private string BuildFileName(int fileIndex, string extension)
		{
			string text = "img_";
			if (fileIndex < 10)
			{
				text = text + string.Format("00{0}", fileIndex) + extension;
			}
			else if (fileIndex < 100)
			{
				text = text + string.Format("0{0}", fileIndex) + extension;
			}
			else
			{
				text = text + string.Format("{0}", fileIndex) + extension;
			}
			return text;
		}
	}
}
