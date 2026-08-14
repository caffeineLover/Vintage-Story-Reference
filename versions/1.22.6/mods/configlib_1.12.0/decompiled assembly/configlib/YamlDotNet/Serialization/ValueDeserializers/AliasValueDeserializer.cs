using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.ValueDeserializers
{
	// Token: 0x020000F8 RID: 248
	public sealed class AliasValueDeserializer : IValueDeserializer
	{
		// Token: 0x060005B0 RID: 1456 RVA: 0x00014696 File Offset: 0x00012896
		[NullableContext(1)]
		public AliasValueDeserializer(IValueDeserializer innerDeserializer)
		{
			if (innerDeserializer == null)
			{
				throw new ArgumentNullException("innerDeserializer");
			}
			this.innerDeserializer = innerDeserializer;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x000146B4 File Offset: 0x000128B4
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

		// Token: 0x04000294 RID: 660
		[Nullable(1)]
		private readonly IValueDeserializer innerDeserializer;

		// Token: 0x020000F9 RID: 249
		[Nullable(new byte[]
		{
			0,
			1
		})]
		private sealed class AliasState : Dictionary<AnchorName, AliasValueDeserializer.ValuePromise>, IPostDeserializationCallback
		{
			// Token: 0x060005B2 RID: 1458 RVA: 0x000147F4 File Offset: 0x000129F4
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

		// Token: 0x020000FA RID: 250
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class ValuePromise : IValuePromise
		{
			// Token: 0x14000013 RID: 19
			// (add) Token: 0x060005B4 RID: 1460 RVA: 0x000148A8 File Offset: 0x00012AA8
			// (remove) Token: 0x060005B5 RID: 1461 RVA: 0x000148E0 File Offset: 0x00012AE0
			public event Action<object> ValueAvailable;

			// Token: 0x1700009F RID: 159
			// (get) Token: 0x060005B6 RID: 1462 RVA: 0x00014915 File Offset: 0x00012B15
			// (set) Token: 0x060005B7 RID: 1463 RVA: 0x0001491D File Offset: 0x00012B1D
			public bool HasValue { get; private set; }

			// Token: 0x060005B8 RID: 1464 RVA: 0x00014926 File Offset: 0x00012B26
			[NullableContext(1)]
			public ValuePromise(AnchorAlias alias)
			{
				this.Alias = alias;
			}

			// Token: 0x060005B9 RID: 1465 RVA: 0x00014935 File Offset: 0x00012B35
			public ValuePromise(object value)
			{
				this.HasValue = true;
				this.value = value;
			}

			// Token: 0x170000A0 RID: 160
			// (get) Token: 0x060005BA RID: 1466 RVA: 0x0001494B File Offset: 0x00012B4B
			// (set) Token: 0x060005BB RID: 1467 RVA: 0x00014966 File Offset: 0x00012B66
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

			// Token: 0x04000297 RID: 663
			private object value;

			// Token: 0x04000298 RID: 664
			public readonly AnchorAlias Alias;
		}
	}
}
