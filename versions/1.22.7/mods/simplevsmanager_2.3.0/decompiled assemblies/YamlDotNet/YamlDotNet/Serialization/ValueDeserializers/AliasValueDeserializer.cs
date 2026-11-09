using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.ValueDeserializers
{
	// Token: 0x02000048 RID: 72
	public sealed class AliasValueDeserializer : IValueDeserializer
	{
		// Token: 0x0600020D RID: 525 RVA: 0x00006BF8 File Offset: 0x00004DF8
		[NullableContext(1)]
		public AliasValueDeserializer(IValueDeserializer innerDeserializer)
		{
			if (innerDeserializer == null)
			{
				throw new ArgumentNullException("innerDeserializer");
			}
			this.innerDeserializer = innerDeserializer;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00006C18 File Offset: 0x00004E18
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
			AliasValueDeserializer.AliasState aliasState3 = state.Get<AliasValueDeserializer.AliasState>();
			AliasValueDeserializer.ValuePromise valuePromise2;
			if (!aliasState3.TryGetValue(anchorAlias.Value, ref valuePromise2))
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

		// Token: 0x0400007D RID: 125
		[Nullable(1)]
		private readonly IValueDeserializer innerDeserializer;

		// Token: 0x02000160 RID: 352
		[Nullable(new byte[]
		{
			0,
			1
		})]
		private sealed class AliasState : Dictionary<AnchorName, AliasValueDeserializer.ValuePromise>, IPostDeserializationCallback
		{
			// Token: 0x060007B6 RID: 1974 RVA: 0x00018EF8 File Offset: 0x000170F8
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

		// Token: 0x02000161 RID: 353
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class ValuePromise : IValuePromise
		{
			// Token: 0x14000002 RID: 2
			// (add) Token: 0x060007B8 RID: 1976 RVA: 0x00018FAC File Offset: 0x000171AC
			// (remove) Token: 0x060007B9 RID: 1977 RVA: 0x00018FE4 File Offset: 0x000171E4
			public event Action<object> ValueAvailable;

			// Token: 0x170000F0 RID: 240
			// (get) Token: 0x060007BA RID: 1978 RVA: 0x00019019 File Offset: 0x00017219
			// (set) Token: 0x060007BB RID: 1979 RVA: 0x00019021 File Offset: 0x00017221
			public bool HasValue { get; private set; }

			// Token: 0x060007BC RID: 1980 RVA: 0x0001902A File Offset: 0x0001722A
			[NullableContext(1)]
			public ValuePromise(AnchorAlias alias)
			{
				this.Alias = alias;
			}

			// Token: 0x060007BD RID: 1981 RVA: 0x00019039 File Offset: 0x00017239
			public ValuePromise(object value)
			{
				this.HasValue = true;
				this.value = value;
			}

			// Token: 0x170000F1 RID: 241
			// (get) Token: 0x060007BE RID: 1982 RVA: 0x0001904F File Offset: 0x0001724F
			// (set) Token: 0x060007BF RID: 1983 RVA: 0x0001906A File Offset: 0x0001726A
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

			// Token: 0x040002B9 RID: 697
			private object value;

			// Token: 0x040002BA RID: 698
			public readonly AnchorAlias Alias;
		}
	}
}
