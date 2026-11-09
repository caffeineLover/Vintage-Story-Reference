using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Functions.Type4
{
	// Token: 0x020001B4 RID: 436
	internal sealed class Operators
	{
		// Token: 0x06000A8B RID: 2699 RVA: 0x0003487C File Offset: 0x00032A7C
		public Operators()
		{
			this.operators.Add("add", Operators.ADD);
			this.operators.Add("abs", Operators.ABS);
			this.operators.Add("atan", Operators.ATAN);
			this.operators.Add("ceiling", Operators.CEILING);
			this.operators.Add("cos", Operators.COS);
			this.operators.Add("cvi", Operators.CVI);
			this.operators.Add("cvr", Operators.CVR);
			this.operators.Add("div", Operators.DIV);
			this.operators.Add("exp", Operators.EXP);
			this.operators.Add("floor", Operators.FLOOR);
			this.operators.Add("idiv", Operators.IDIV);
			this.operators.Add("ln", Operators.LN);
			this.operators.Add("log", Operators.LOG);
			this.operators.Add("mod", Operators.MOD);
			this.operators.Add("mul", Operators.MUL);
			this.operators.Add("neg", Operators.NEG);
			this.operators.Add("round", Operators.ROUND);
			this.operators.Add("sin", Operators.SIN);
			this.operators.Add("sqrt", Operators.SQRT);
			this.operators.Add("sub", Operators.SUB);
			this.operators.Add("truncate", Operators.TRUNCATE);
			this.operators.Add("and", Operators.AND);
			this.operators.Add("bitshift", Operators.BITSHIFT);
			this.operators.Add("eq", Operators.EQ);
			this.operators.Add("false", Operators.FALSE);
			this.operators.Add("ge", Operators.GE);
			this.operators.Add("gt", Operators.GT);
			this.operators.Add("le", Operators.LE);
			this.operators.Add("lt", Operators.LT);
			this.operators.Add("ne", Operators.NE);
			this.operators.Add("not", Operators.NOT);
			this.operators.Add("or", Operators.OR);
			this.operators.Add("true", Operators.TRUE);
			this.operators.Add("xor", Operators.XOR);
			this.operators.Add("if", Operators.IF);
			this.operators.Add("ifelse", Operators.IFELSE);
			this.operators.Add("copy", Operators.COPY);
			this.operators.Add("dup", Operators.DUP);
			this.operators.Add("exch", Operators.EXCH);
			this.operators.Add("index", Operators.INDEX);
			this.operators.Add("pop", Operators.POP);
			this.operators.Add("roll", Operators.ROLL);
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00034C0C File Offset: 0x00032E0C
		public Operator GetOperator(string operatorName)
		{
			return this.operators[operatorName];
		}

		// Token: 0x04000521 RID: 1313
		private static readonly Operator ABS = new ArithmeticOperators.Abs();

		// Token: 0x04000522 RID: 1314
		private static readonly Operator ADD = new ArithmeticOperators.Add();

		// Token: 0x04000523 RID: 1315
		private static readonly Operator ATAN = new ArithmeticOperators.Atan();

		// Token: 0x04000524 RID: 1316
		private static readonly Operator CEILING = new ArithmeticOperators.Ceiling();

		// Token: 0x04000525 RID: 1317
		private static readonly Operator COS = new ArithmeticOperators.Cos();

		// Token: 0x04000526 RID: 1318
		private static readonly Operator CVI = new ArithmeticOperators.Cvi();

		// Token: 0x04000527 RID: 1319
		private static readonly Operator CVR = new ArithmeticOperators.Cvr();

		// Token: 0x04000528 RID: 1320
		private static readonly Operator DIV = new ArithmeticOperators.Div();

		// Token: 0x04000529 RID: 1321
		private static readonly Operator EXP = new ArithmeticOperators.Exp();

		// Token: 0x0400052A RID: 1322
		private static readonly Operator FLOOR = new ArithmeticOperators.Floor();

		// Token: 0x0400052B RID: 1323
		private static readonly Operator IDIV = new ArithmeticOperators.IDiv();

		// Token: 0x0400052C RID: 1324
		private static readonly Operator LN = new ArithmeticOperators.Ln();

		// Token: 0x0400052D RID: 1325
		private static readonly Operator LOG = new ArithmeticOperators.Log();

		// Token: 0x0400052E RID: 1326
		private static readonly Operator MOD = new ArithmeticOperators.Mod();

		// Token: 0x0400052F RID: 1327
		private static readonly Operator MUL = new ArithmeticOperators.Mul();

		// Token: 0x04000530 RID: 1328
		private static readonly Operator NEG = new ArithmeticOperators.Neg();

		// Token: 0x04000531 RID: 1329
		private static readonly Operator ROUND = new ArithmeticOperators.Round();

		// Token: 0x04000532 RID: 1330
		private static readonly Operator SIN = new ArithmeticOperators.Sin();

		// Token: 0x04000533 RID: 1331
		private static readonly Operator SQRT = new ArithmeticOperators.Sqrt();

		// Token: 0x04000534 RID: 1332
		private static readonly Operator SUB = new ArithmeticOperators.Sub();

		// Token: 0x04000535 RID: 1333
		private static readonly Operator TRUNCATE = new ArithmeticOperators.Truncate();

		// Token: 0x04000536 RID: 1334
		private static readonly Operator AND = new BitwiseOperators.And();

		// Token: 0x04000537 RID: 1335
		private static readonly Operator BITSHIFT = new BitwiseOperators.Bitshift();

		// Token: 0x04000538 RID: 1336
		private static readonly Operator EQ = new RelationalOperators.Eq();

		// Token: 0x04000539 RID: 1337
		private static readonly Operator FALSE = new BitwiseOperators.False();

		// Token: 0x0400053A RID: 1338
		private static readonly Operator GE = new RelationalOperators.Ge();

		// Token: 0x0400053B RID: 1339
		private static readonly Operator GT = new RelationalOperators.Gt();

		// Token: 0x0400053C RID: 1340
		private static readonly Operator LE = new RelationalOperators.Le();

		// Token: 0x0400053D RID: 1341
		private static readonly Operator LT = new RelationalOperators.Lt();

		// Token: 0x0400053E RID: 1342
		private static readonly Operator NE = new RelationalOperators.Ne();

		// Token: 0x0400053F RID: 1343
		private static readonly Operator NOT = new BitwiseOperators.Not();

		// Token: 0x04000540 RID: 1344
		private static readonly Operator OR = new BitwiseOperators.Or();

		// Token: 0x04000541 RID: 1345
		private static readonly Operator TRUE = new BitwiseOperators.True();

		// Token: 0x04000542 RID: 1346
		private static readonly Operator XOR = new BitwiseOperators.Xor();

		// Token: 0x04000543 RID: 1347
		private static readonly Operator IF = new ConditionalOperators.If();

		// Token: 0x04000544 RID: 1348
		private static readonly Operator IFELSE = new ConditionalOperators.IfElse();

		// Token: 0x04000545 RID: 1349
		private static readonly Operator COPY = new StackOperators.Copy();

		// Token: 0x04000546 RID: 1350
		private static readonly Operator DUP = new StackOperators.Dup();

		// Token: 0x04000547 RID: 1351
		private static readonly Operator EXCH = new StackOperators.Exch();

		// Token: 0x04000548 RID: 1352
		private static readonly Operator INDEX = new StackOperators.Index();

		// Token: 0x04000549 RID: 1353
		private static readonly Operator POP = new StackOperators.Pop();

		// Token: 0x0400054A RID: 1354
		private static readonly Operator ROLL = new StackOperators.Roll();

		// Token: 0x0400054B RID: 1355
		private readonly Dictionary<string, Operator> operators = new Dictionary<string, Operator>();
	}
}
