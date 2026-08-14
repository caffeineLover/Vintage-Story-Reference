using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core
{
	// Token: 0x020000D4 RID: 212
	[NullableContext(1)]
	[Nullable(0)]
	public static class ParserExtensions
	{
		// Token: 0x06000561 RID: 1377 RVA: 0x00013724 File Offset: 0x00011924
		public static T Consume<[Nullable(0)] T>(this IParser parser) where T : ParsingEvent
		{
			T result = parser.Require<T>();
			parser.MoveNext();
			return result;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00013740 File Offset: 0x00011940
		public static bool TryConsume<[Nullable(0)] T>(this IParser parser, [MaybeNullWhen(false)] out T @event) where T : ParsingEvent
		{
			if (parser.Accept(out @event))
			{
				parser.MoveNext();
				return true;
			}
			return false;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00013758 File Offset: 0x00011958
		public static T Require<[Nullable(0)] T>(this IParser parser) where T : ParsingEvent
		{
			T result;
			if (parser.Accept(out result))
			{
				return result;
			}
			ParsingEvent parsingEvent = parser.Current;
			if (parsingEvent == null)
			{
				throw new YamlException("Expected '" + typeof(T).Name + "', got nothing.");
			}
			Mark start = parsingEvent.Start;
			Mark end = parsingEvent.End;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(26, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Expected '");
			defaultInterpolatedStringHandler.AppendFormatted(typeof(T).Name);
			defaultInterpolatedStringHandler.AppendLiteral("', got '");
			defaultInterpolatedStringHandler.AppendFormatted(parsingEvent.GetType().Name);
			defaultInterpolatedStringHandler.AppendLiteral("' (at ");
			defaultInterpolatedStringHandler.AppendFormatted<Mark>(parsingEvent.Start);
			defaultInterpolatedStringHandler.AppendLiteral(").");
			throw new YamlException(ref start, ref end, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00013830 File Offset: 0x00011A30
		public static bool Accept<[Nullable(0)] T>(this IParser parser, [MaybeNullWhen(false)] out T @event) where T : ParsingEvent
		{
			if (parser.Current == null && !parser.MoveNext())
			{
				throw new EndOfStreamException();
			}
			T t = parser.Current as T;
			if (t != null)
			{
				@event = t;
				return true;
			}
			@event = default(T);
			return false;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00013880 File Offset: 0x00011A80
		public static void SkipThisAndNestedEvents(this IParser parser)
		{
			int num = 0;
			do
			{
				ParsingEvent parsingEvent = parser.Consume<ParsingEvent>();
				num += parsingEvent.NestingIncrease;
			}
			while (num > 0);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x000138A3 File Offset: 0x00011AA3
		[Obsolete("Please use Consume<T>() instead")]
		public static T Expect<[Nullable(0)] T>(this IParser parser) where T : ParsingEvent
		{
			return parser.Consume<T>();
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x000138AC File Offset: 0x00011AAC
		[Obsolete("Please use TryConsume<T>(out var evt) instead")]
		[return: Nullable(2)]
		[return: MaybeNull]
		public static T Allow<[Nullable(0)] T>(this IParser parser) where T : ParsingEvent
		{
			T result;
			if (!parser.TryConsume(out result))
			{
				return default(T);
			}
			return result;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000138D0 File Offset: 0x00011AD0
		[Obsolete("Please use Accept<T>(out var evt) instead")]
		[return: Nullable(2)]
		[return: MaybeNull]
		public static T Peek<[Nullable(0)] T>(this IParser parser) where T : ParsingEvent
		{
			T result;
			if (!parser.Accept(out result))
			{
				return default(T);
			}
			return result;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x000138F4 File Offset: 0x00011AF4
		[Obsolete("Please use TryConsume<T>(out var evt) or Accept<T>(out var evt) instead")]
		public static bool Accept<[Nullable(0)] T>(this IParser parser) where T : ParsingEvent
		{
			T t;
			return parser.Accept(out t);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001390C File Offset: 0x00011B0C
		public static bool TryFindMappingEntry(this IParser parser, Func<Scalar, bool> selector, [Nullable(2)] [MaybeNullWhen(false)] out Scalar key, [Nullable(2)] [MaybeNullWhen(false)] out ParsingEvent value)
		{
			MappingStart mappingStart;
			if (parser.TryConsume(out mappingStart))
			{
				while (parser.Current != null)
				{
					ParsingEvent parsingEvent = parser.Current;
					Scalar scalar = parsingEvent as Scalar;
					if (scalar == null)
					{
						if (!(parsingEvent is MappingStart) && !(parsingEvent is SequenceStart))
						{
							parser.MoveNext();
						}
						else
						{
							parser.SkipThisAndNestedEvents();
						}
					}
					else
					{
						bool flag = selector.Invoke(scalar);
						parser.MoveNext();
						if (flag)
						{
							value = parser.Current;
							key = scalar;
							return true;
						}
						parser.SkipThisAndNestedEvents();
					}
				}
			}
			key = null;
			value = null;
			return false;
		}
	}
}
