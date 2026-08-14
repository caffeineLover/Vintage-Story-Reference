using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using SimpleExpressionEngine.Nodes;

namespace SimpleExpressionEngine
{
	// Token: 0x02000066 RID: 102
	[NullableContext(1)]
	[Nullable(0)]
	public class MathParser
	{
		// Token: 0x060002CE RID: 718 RVA: 0x0000EF1B File Offset: 0x0000D11B
		public MathParser(ITokenizer<float> tokenizer)
		{
			this.mTokenizer = tokenizer;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000EF2A File Offset: 0x0000D12A
		public INode<float, float, float> ParseExpression()
		{
			INode<float, float, float> result = this.ParseAddSubtract();
			if (this.mTokenizer.Token != Token.EOF)
			{
				throw new SyntaxException("Unexpected characters at end of expression");
			}
			return result;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000EF4C File Offset: 0x0000D14C
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

		// Token: 0x060002D1 RID: 721 RVA: 0x0000EFE4 File Offset: 0x0000D1E4
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

		// Token: 0x060002D2 RID: 722 RVA: 0x0000F07C File Offset: 0x0000D27C
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

		// Token: 0x060002D3 RID: 723 RVA: 0x0000F0F0 File Offset: 0x0000D2F0
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

		// Token: 0x060002D4 RID: 724 RVA: 0x0000F265 File Offset: 0x0000D465
		public static INode<float, float, float> Parse(string str)
		{
			return MathParser.Parse(new FloatTokenizer(new StringReader(str)));
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000F277 File Offset: 0x0000D477
		public static INode<float, float, float> Parse(FloatTokenizer tokenizer)
		{
			return new MathParser(tokenizer).ParseExpression();
		}

		// Token: 0x0400016A RID: 362
		private readonly ITokenizer<float> mTokenizer;
	}
}
