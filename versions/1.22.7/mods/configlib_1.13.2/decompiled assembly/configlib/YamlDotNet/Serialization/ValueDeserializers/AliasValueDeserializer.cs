using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.ValueDeserializers
{
	// Token: 0x020000F4 RID: 244
	public sealed class AliasValueDeserializer : IValueDeserializer
	{
		// Token: 0x06000574 RID: 1396 RVA: 0x00013D4E File Offset: 0x00011F4E
		[NullableContext(1)]
		public AliasValueDeserializer(IValueDeserializer innerDeserializer)
		{
			if (innerDeserializer == null)
			{
				throw new ArgumentNullException("innerDeserializer");
			}
			this.innerDeserializer = innerDeserializer;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00013D6C File Offset: 0x00011F6C
		[NullableContext(1)]
		[return: Nullable(2)]
		public object DeserializeValue(IParser parser, Type expectedType, SerializerState state, IValueDeserializer nestedObjectDeserializer)
		{
			AnchorAlias anchorAlias;
			if (!parser.TryConsume(out anchorAlias))
			{
				AnchorName anchorName = AnchorName.Empty;
				NodeEvent nodeEvent;
				if (parser.Accept(out nodeEvent) && !nodeEvent.Anchor.IsEmpty)
				{
					anchorName = nodeEvent.Anchor;
					AliasValueDeserializer.AliasState aliasState = state.Get<AliasValueDeserializer.AliasState>();
					if (!aliasState.ContainsKey(anchorName))
					{
						aliasState[anchorName] = new AliasValueDeserializer.ValuePromise(new AnchorAlias(anchorName));
					}
				}
				object obj = this.innerDeserializer.DeserializeValue(parser, expectedType, state, nestedObjectDeserializer);
				if (!anchorName.IsEmpty)
				{
					AliasValueDeserializer.AliasState aliasState2 = state.Get<AliasValueDeserializer.AliasState>();
					AliasValueDeserializer.ValuePromise valuePromise;
					if (!aliasState2.TryGetValue(anchorName, ref valuePromise))
					{
						aliasState2.Add(anchorName, new AliasValueDeserializer.ValuePromise(obj));
					}
					else if (!valuePromise.HasValue)
					{
						valuePromise.Value = obj;
					}
					else
					{
						aliasState2[anchorName] = new AliasValueDeserializer.ValuePromise(obj);
					}
				}
				return obj;
			}
			AliasValueDeserializer.ValuePromise valuePromise2;
			if (!state.Get<AliasValueDeserializer.AliasState>().TryGetValue(anchorAlias.Value, ref valuePromise2))
			{
				Mark start = anchorAlias.Start;
				Mark end = anchorAlias.End;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(41, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Alias $");
				defaultInterpolatedStringHandler.AppendFormatted<AnchorName>(anchorAlias.Value);
				defaultInterpolatedStringHandler.AppendLiteral(" cannot precede anchor declaration");
				throw new AnchorNotFoundException(ref start, ref end, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (!valuePromise2.HasValue)
			{
				return valuePromise2;
			}
			return valuePromise2.Value;
		}

		// Token: 0x04000266 RID: 614
		[Nullable(1)]
		private readonly IValueDeserializer innerDeserializer;

		// Token: 0x020000F5 RID: 245
		[Nullable(new byte[]
		{
			0,
			1
		})]
		private sealed class AliasState : Dictionary<AnchorName, AliasValueDeserializer.ValuePromise>, IPostDeserializationCallback
		{
			// Token: 0x06000576 RID: 1398 RVA: 0x00013EAC File Offset: 0x000120AC
			public void OnDeserialization()
			{
				foreach (AliasValueDeserializer.ValuePromise valuePromise in base.Values)
				{
					if (!valuePromise.HasValue)
					{
						AnchorAlias alias = valuePromise.Alias;
						Mark start = alias.Start;
						Mark end = alias.End;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Anchor '");
						defaultInterpolatedStringHandler.AppendFormatted<AnchorName>(alias.Value);
						defaultInterpolatedStringHandler.AppendLiteral("' not found");
						throw new AnchorNotFoundException(ref start, ref end, defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
		}

		// Token: 0x020000F6 RID: 246
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class ValuePromise : IValuePromise
		{
			// Token: 0x14000013 RID: 19
			// (add) Token: 0x06000578 RID: 1400 RVA: 0x00013F60 File Offset: 0x00012160
			// (remove) Token: 0x06000579 RID: 1401 RVA: 0x00013F98 File Offset: 0x00012198
			public event Action<object> ValueAvailable;

			// Token: 0x1700009F RID: 159
			// (get) Token: 0x0600057A RID: 1402 RVA: 0x00013FCD File Offset: 0x000121CD
			// (set) Token: 0x0600057B RID: 1403 RVA: 0x00013FD5 File Offset: 0x000121D5
			public bool HasValue { get; private set; }

			// Token: 0x0600057C RID: 1404 RVA: 0x00013FDE File Offset: 0x000121DE
			[NullableContext(1)]
			public ValuePromise(AnchorAlias alias)
			{
				this.Alias = alias;
			}

			// Token: 0x0600057D RID: 1405 RVA: 0x00013FED File Offset: 0x000121ED
			public ValuePromise(object value)
			{
				this.HasValue = true;
				this.value = value;
			}

			// Token: 0x170000A0 RID: 160
			// (get) Token: 0x0600057E RID: 1406 RVA: 0x00014003 File Offset: 0x00012203
			// (set) Token: 0x0600057F RID: 1407 RVA: 0x0001401E File Offset: 0x0001221E
			public object Value
			{
				get
				{
					if (!this.HasValue)
					{
						throw new InvalidOperationException("Value not set");
					}
					return this.value;
				}
				set
				{
					if (this.HasValue)
					{
						throw new InvalidOperationException("Value already set");
					}
					this.HasValue = true;
					this.value = value;
					Action<object> valueAvailable = this.ValueAvailable;
					if (valueAvailable == null)
					{
						return;
					}
					valueAvailable.Invoke(value);
				}
			}

			// Token: 0x04000269 RID: 617
			private object value;

			// Token: 0x0400026A RID: 618
			public readonly AnchorAlias Alias;
		}
	}
}
