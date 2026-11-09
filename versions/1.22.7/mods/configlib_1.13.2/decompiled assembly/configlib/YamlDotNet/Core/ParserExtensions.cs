using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core
{
	// Token: 0x020001C3 RID: 451
	[NullableContext(1)]
	[Nullable(0)]
	public static class ParserExtensions
	{
		// Token: 0x060009E9 RID: 2537 RVA: 0x0002173B File Offset: 0x0001F93B
		public static T Consume<[Nullable(0)] T>(this IParser parser) where T : ParsingEvent
		{
			T result = parser.Require<T>();
			parser.MoveNext();
			return result;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0002174A File Offset: 0x0001F94A
		public static bool TryConsume<[Nullable(0)] T>(this IParser parser, [MaybeNullWhen(false)] out T @event) where T : ParsingEvent
		{
			if (parser.Accept(out @event))
			{
				parser.MoveNext();
				return true;
			}
			return false;
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x00021760 File Offset: 0x0001F960
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

		// Token: 0x060009EC RID: 2540 RVA: 0x00021838 File Offset: 0x0001FA38
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

		// Token: 0x060009ED RID: 2541 RVA: 0x00021888 File Offset: 0x0001FA88
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

		// Token: 0x060009EE RID: 2542 RVA: 0x000218AB File Offset: 0x0001FAAB
		[Obsolete("Please use Consume<T>() instead")]
		public static T Expect<[Nullable(0)] T>(this IParser parser) where T : ParsingEvent
		{
			return parser.Consume<T>();
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x000218B4 File Offset: 0x0001FAB4
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

		// Token: 0x060009F0 RID: 2544 RVA: 0x000218D8 File Offset: 0x0001FAD8
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

		// Token: 0x060009F1 RID: 2545 RVA: 0x000218FC File Offset: 0x0001FAFC
		[Obsolete("Please use TryConsume<T>(out var evt) or Accept<T>(out var evt) instead")]
		public static bool Accept<[Nullable(0)] T>(this IParser parser) where T : ParsingEvent
		{
			T t;
			return parser.Accept(out t);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00021914 File Offset: 0x0001FB14
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
