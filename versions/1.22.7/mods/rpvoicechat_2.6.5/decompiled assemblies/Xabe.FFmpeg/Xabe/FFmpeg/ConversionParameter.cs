using System;
using System.Collections.Generic;
using System.Linq;

namespace Xabe.FFmpeg
{
	// Token: 0x02000015 RID: 21
	internal class ConversionParameter
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x00004430 File Offset: 0x00002630
		public ConversionParameter(string parameter, ParameterPosition position = ParameterPosition.PostInput)
		{
			this.Parameter = parameter.Trim() + " ";
			this.Key = Enumerable.First<string>(parameter.Split(new char[]
			{
				' '
			}));
			this.Position = position;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00004483 File Offset: 0x00002683
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x0000448B File Offset: 0x0000268B
		public string Parameter { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00004494 File Offset: 0x00002694
		public string Key { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x0000449C File Offset: 0x0000269C
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x000044A4 File Offset: 0x000026A4
		public ParameterPosition Position { get; set; } = ParameterPosition.PostInput;

		// Token: 0x060000AA RID: 170 RVA: 0x000044B0 File Offset: 0x000026B0
		public override bool Equals(object obj)
		{
			ConversionParameter conversionParameter = obj as ConversionParameter;
			return conversionParameter != null && this.Key == conversionParameter.Key && this.Position == conversionParameter.Position && this.Key != "-i";
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000044FC File Offset: 0x000026FC
		public override int GetHashCode()
		{
			return (495346454 * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Key)) * -1521134295 + this.Position.GetHashCode();
		}
	}
}
