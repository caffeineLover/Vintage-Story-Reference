using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using SimpleExpressionEngine.Nodes;

namespace SimpleExpressionEngine
{
	// Token: 0x02000062 RID: 98
	[NullableContext(1)]
	[Nullable(0)]
	public class MathParser
	{
		// Token: 0x06000292 RID: 658 RVA: 0x0000E5D3 File Offset: 0x0000C7D3
		public MathParser(ITokenizer<float> tokenizer)
		{
			this.mTokenizer = tokenizer;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000E5E2 File Offset: 0x0000C7E2
		public INode<float, float, float> ParseExpression()
		{
			INode<float, float, float> result = this.ParseAddSubtract();
			if (this.mTokenizer.Token != Token.EOF)
			{
				throw new SyntaxException("Unexpected characters at end of expression");
			}
			return result;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000E604 File Offset: 0x0000C804
		private INode<float, float, float> ParseAddSubtract()
		{
			INode<float, float, float> leftOperand = this.ParseMultiplyDivide();
			for (;;)
			{
				Func<float, float, float> operation = null;
				if (this.mTokenizer.Token == Token.Add)
				{
					operation = ((float a, float b) => a + b);
				}
				else if (this.mTokenizer.Token == Token.Subtract)
				{
					operation = ((float a, float b) => a - b);
				}
				if (operation == null)
				{
					break;
				}
				this.mTokenizer.NextToken();
				INode<float, float, float> rightOperand = this.ParseMultiplyDivide();
				leftOperand = new Binary<float, float, float>(leftOperand, rightOperand, operation);
			}
			return leftOperand;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000E69C File Offset: 0x0000C89C
		private INode<float, float, float> ParseMultiplyDivide()
		{
			INode<float, float, float> leftOperand = this.ParseUnary();
			for (;;)
			{
				Func<float, float, float> operation = null;
				if (this.mTokenizer.Token == Token.Multiply)
				{
					operation = ((float a, float b) => a * b);
				}
				else if (this.mTokenizer.Token == Token.Divide)
				{
					operation = ((float a, float b) => a / b);
				}
				if (operation == null)
				{
					break;
				}
				this.mTokenizer.NextToken();
				INode<float, float, float> rightOperand = this.ParseUnary();
				leftOperand = new Binary<float, float, float>(leftOperand, rightOperand, operation);
			}
			return leftOperand;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000E734 File Offset: 0x0000C934
		private INode<float, float, float> ParseUnary()
		{
			while (this.mTokenizer.Token == Token.Add)
			{
				this.mTokenizer.NextToken();
			}
			if (this.mTokenizer.Token == Token.Subtract)
			{
				this.mTokenizer.NextToken();
				return new Unary<float, float, float>(this.ParseUnary(), (float a) => -a);
			}
			return this.ParseNumber();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000E7A8 File Offset: 0x0000C9A8
		private INode<float, float, float> ParseNumber()
		{
			if (this.mTokenizer.Token != Token.Identifier && this.mTokenizer.Token != Token.Number && this.mTokenizer.Token != Token.OpenParenthesis)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(18, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unexpected token: ");
				defaultInterpolatedStringHandler.AppendFormatted<Token>(this.mTokenizer.Token);
				throw new SyntaxException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (this.mTokenizer.Token == Token.Number)
			{
				Value<float, float, float> value = new Value<float, float, float>(this.mTokenizer.Value);
				this.mTokenizer.NextToken();
				return value;
			}
			if (this.mTokenizer.Token == Token.OpenParenthesis)
			{
				this.mTokenizer.NextToken();
				INode<float, float, float> result = this.ParseAddSubtract();
				if (this.mTokenizer.Token != Token.CloseParenthesis)
				{
					throw new SyntaxException("Missing close parenthesis");
				}
				this.mTokenizer.NextToken();
				return result;
			}
			else
			{
				string name = this.mTokenizer.Identifier;
				this.mTokenizer.NextToken();
				if (this.mTokenizer.Token != Token.OpenParenthesis)
				{
					return new Variable<float, float>(name);
				}
				this.mTokenizer.NextToken();
				List<INode<float, float, float>> arguments = new List<INode<float, float, float>>();
				for (;;)
				{
					arguments.Add(this.ParseAddSubtract());
					if (this.mTokenizer.Token != Token.Comma)
					{
						break;
					}
					this.mTokenizer.NextToken();
				}
				if (this.mTokenizer.Token != Token.CloseParenthesis)
				{
					throw new SyntaxException("Missing close parenthesis");
				}
				this.mTokenizer.NextToken();
				return new FunctionCall<float, float>(name, arguments.ToArray());
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000E91D File Offset: 0x0000CB1D
		public static INode<float, float, float> Parse(string str)
		{
			return MathParser.Parse(new FloatTokenizer(new StringReader(str)));
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000E92F File Offset: 0x0000CB2F
		public static INode<float, float, float> Parse(FloatTokenizer tokenizer)
		{
			return new MathParser(tokenizer).ParseExpression();
		}

		// Token: 0x0400013C RID: 316
		private readonly ITokenizer<float> mTokenizer;
	}
}
