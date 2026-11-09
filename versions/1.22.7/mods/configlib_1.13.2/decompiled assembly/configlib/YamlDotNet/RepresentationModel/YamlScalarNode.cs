using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Schemas;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x02000188 RID: 392
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerDisplay("{Value}")]
	public sealed class YamlScalarNode : YamlNode, IYamlConvertible
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x0001B107 File Offset: 0x00019307
		// (set) Token: 0x060007F5 RID: 2037 RVA: 0x0001B10F File Offset: 0x0001930F
		[Nullable(2)]
		public string Value
		{
			[NullableContext(2)]
			get
			{
				return this._value;
			}
			[NullableContext(2)]
			set
			{
				if (value == null)
				{
					this._forceImplicitPlain = true;
				}
				else
				{
					this._forceImplicitPlain = false;
				}
				this._value = value;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0001B12B File Offset: 0x0001932B
		// (set) Token: 0x060007F7 RID: 2039 RVA: 0x0001B133 File Offset: 0x00019333
		public ScalarStyle Style { get; set; }

		// Token: 0x060007F8 RID: 2040 RVA: 0x0001B13C File Offset: 0x0001933C
		internal YamlScalarNode(IParser parser, DocumentLoadingState state)
		{
			this.Load(parser, state);
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0001B14C File Offset: 0x0001934C
		private void Load(IParser parser, DocumentLoadingState state)
		{
			Scalar scalar = parser.Consume<Scalar>();
			base.Load(scalar, state);
			if (scalar.Style == ScalarStyle.Plain && base.Tag.IsEmpty && (scalar.Value == string.Empty || scalar.Value.Equals("null", 2) || scalar.Value.Equals("Null", 2) || scalar.Value.Equals("NULL", 2) || scalar.Value == "~"))
			{
				this._forceImplicitPlain = true;
			}
			this._value = scalar.Value;
			this.Style = scalar.Style;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0001B1FD File Offset: 0x000193FD
		public YamlScalarNode()
		{
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0001B205 File Offset: 0x00019405
		[NullableContext(2)]
		public YamlScalarNode(string value)
		{
			this.Value = value;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0001B214 File Offset: 0x00019414
		internal override void ResolveAliases(DocumentLoadingState state)
		{
			throw new NotSupportedException("Resolving an alias on a scalar node does not make sense");
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0001B220 File Offset: 0x00019420
		internal override void Emit(IEmitter emitter, EmitterState state)
		{
			TagName tag = base.Tag;
			bool isPlainImplicit = tag.IsEmpty;
			if (this._forceImplicitPlain && this.Style == ScalarStyle.Plain && (this.Value == null || this.Value == ""))
			{
				tag = JsonSchema.Tags.Null;
				isPlainImplicit = true;
			}
			else if (tag.IsEmpty && this.Value == null && (this.Style == ScalarStyle.Plain || this.Style == ScalarStyle.Any))
			{
				tag = JsonSchema.Tags.Null;
				isPlainImplicit = true;
			}
			emitter.Emit(new Scalar(base.Anchor, tag, this.Value ?? string.Empty, this.Style, isPlainImplicit, false));
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0001B2C4 File Offset: 0x000194C4
		public override void Accept(IYamlVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0001B2D0 File Offset: 0x000194D0
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			YamlScalarNode yamlScalarNode = obj as YamlScalarNode;
			return yamlScalarNode != null && object.Equals(base.Tag, yamlScalarNode.Tag) && object.Equals(this.Value, yamlScalarNode.Value);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0001B318 File Offset: 0x00019518
		public override int GetHashCode()
		{
			return HashCode.CombineHashCodes(base.Tag.GetHashCode(), this.Value);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0001B344 File Offset: 0x00019544
		[return: Nullable(2)]
		public static explicit operator string(YamlScalarNode value)
		{
			return value.Value;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0001B34C File Offset: 0x0001954C
		internal override string ToString(RecursionLevel level)
		{
			return this.Value ?? string.Empty;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001B35D File Offset: 0x0001955D
		internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
		{
			yield return this;
			yield break;
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x0001B36D File Offset: 0x0001956D
		public override YamlNodeType NodeType
		{
			get
			{
				return YamlNodeType.Scalar;
			}
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001B370 File Offset: 0x00019570
		void IYamlConvertible.Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			this.Load(parser, new DocumentLoadingState());
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001A9EB File Offset: 0x00018BEB
		void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			this.Emit(emitter, new EmitterState());
		}

		// Token: 0x0400035A RID: 858
		private bool _forceImplicitPlain;

		// Token: 0x0400035B RID: 859
		[Nullable(2)]
		private string _value;
	}
}
