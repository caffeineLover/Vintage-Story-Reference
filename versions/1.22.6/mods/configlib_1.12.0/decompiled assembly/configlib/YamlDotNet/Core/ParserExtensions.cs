using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core
{
	// Token: 0x020001C7 RID: 455
	[NullableContext(1)]
	[Nullable(0)]
	public static class ParserExtensions
	{
		// Token: 0x06000A25 RID: 2597 RVA: 0x00022083 File Offset: 0x00020283
		public static T Consume<[Nullable(0)] T>(this IParser parser) where T : ParsingEvent
		{
			T result = parser.Require<T>();
			parser.MoveNext();
			return result;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00022092 File Offset: 0x00020292
		public static bool TryConsume<[Nullable(0)] T>(this IParser parser, [MaybeNullWhen(false)] out T @event) where T : ParsingEvent
		{
			if (parser.Accept(out @event))
			{
				parser.MoveNext();
				return true;
			}
			return false;
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x000220A8 File Offset: 0x000202A8
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

		// Token: 0x06000A28 RID: 2600 RVA: 0x00022180 File Offset: 0x00020380
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

		// Token: 0x06000A29 RID: 2601 RVA: 0x000221D0 File Offset: 0x000203D0
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

		// Token: 0x06000A2A RID: 2602 RVA: 0x000221F3 File Offset: 0x000203F3
		[Obsolete("Please use Consume<T>() instead")]
		public static T Expect<[Nullable(0)] T>(this IParser parser) where T : ParsingEvent
		{
			return parser.Consume<T>();
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x000221FC File Offset: 0x000203FC
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

		// Token: 0x06000A2C RID: 2604 RVA: 0x00022220 File Offset: 0x00020420
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

		// Token: 0x06000A2D RID: 2605 RVA: 0x00022244 File Offset: 0x00020444
		[Obsolete("Please use TryConsume<T>(out var evt) or Accept<T>(out var evt) instead")]
		public static bool Accept<[Nullable(0)] T>(this IParser parser) where T : ParsingEvent
		{
			T t;
			return parser.Accept(out t);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0002225C File Offset: 0x0002045C
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
