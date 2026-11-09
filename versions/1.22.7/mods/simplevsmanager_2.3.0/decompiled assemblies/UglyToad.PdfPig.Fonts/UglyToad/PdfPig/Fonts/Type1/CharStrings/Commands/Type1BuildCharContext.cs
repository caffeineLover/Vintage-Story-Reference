using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands
{
	// Token: 0x02000017 RID: 23
	internal class Type1BuildCharContext
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00005F6E File Offset: 0x0000416E
		public IReadOnlyDictionary<int, Type1CharStrings.CommandSequence> Subroutines { get; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00005F76 File Offset: 0x00004176
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00005F7E File Offset: 0x0000417E
		public double WidthX { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00005F87 File Offset: 0x00004187
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00005F8F File Offset: 0x0000418F
		public double WidthY { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00005F98 File Offset: 0x00004198
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00005FA0 File Offset: 0x000041A0
		public double LeftSideBearingX { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00005FA9 File Offset: 0x000041A9
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00005FB1 File Offset: 0x000041B1
		public double LeftSideBearingY { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00005FBA File Offset: 0x000041BA
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00005FC2 File Offset: 0x000041C2
		public bool IsFlexing { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00005FCB File Offset: 0x000041CB
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00005FD3 File Offset: 0x000041D3
		public List<PdfSubpath> Path { get; private set; } = new List<PdfSubpath>();

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00005FDC File Offset: 0x000041DC
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x00005FE4 File Offset: 0x000041E4
		public PdfPoint CurrentPosition { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00005FED File Offset: 0x000041ED
		public CharStringStack Stack { get; } = new CharStringStack();

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00005FF5 File Offset: 0x000041F5
		public CharStringStack PostscriptStack { get; } = new CharStringStack();

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00005FFD File Offset: 0x000041FD
		public List<PdfPoint> FlexPoints { get; } = new List<PdfPoint>();

		// Token: 0x060000A9 RID: 169 RVA: 0x00006008 File Offset: 0x00004208
		public Type1BuildCharContext(IReadOnlyDictionary<int, Type1CharStrings.CommandSequence> subroutines, Func<int, IReadOnlyList<PdfSubpath>> characterByIndexFactory, Func<string, IReadOnlyList<PdfSubpath>> characterByNameFactory)
		{
			if (characterByIndexFactory == null)
			{
				throw new ArgumentNullException("characterByIndexFactory");
			}
			this.characterByIndexFactory = characterByIndexFactory;
			if (characterByNameFactory == null)
			{
				throw new ArgumentNullException("characterByNameFactory");
			}
			this.characterByNameFactory = characterByNameFactory;
			if (subroutines == null)
			{
				throw new ArgumentNullException("subroutines");
			}
			this.Subroutines = subroutines;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00006089 File Offset: 0x00004289
		public void AddFlexPoint(PdfPoint point)
		{
			this.FlexPoints.Add(point);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00006097 File Offset: 0x00004297
		public IReadOnlyList<PdfSubpath> GetCharacter(int characterCode)
		{
			return this.characterByIndexFactory.Invoke(characterCode);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000060A5 File Offset: 0x000042A5
		public IReadOnlyList<PdfSubpath> GetCharacter(string characterName)
		{
			return this.characterByNameFactory.Invoke(characterName);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000060B3 File Offset: 0x000042B3
		public void SetPath(IReadOnlyList<PdfSubpath> path)
		{
			List<PdfSubpath> list = Enumerable.ToList<PdfSubpath>(path);
			if (list == null)
			{
				throw new ArgumentNullException("path");
			}
			this.Path = list;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000060D0 File Offset: 0x000042D0
		public void ClearFlexPoints()
		{
			this.FlexPoints.Clear();
		}

		// Token: 0x04000067 RID: 103
		private readonly Func<int, IReadOnlyList<PdfSubpath>> characterByIndexFactory;

		// Token: 0x04000068 RID: 104
		private readonly Func<string, IReadOnlyList<PdfSubpath>> characterByNameFactory;
	}
}
