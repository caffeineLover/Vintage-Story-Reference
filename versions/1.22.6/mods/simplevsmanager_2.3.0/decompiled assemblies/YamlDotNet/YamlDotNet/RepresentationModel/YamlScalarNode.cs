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
	// Token: 0x020000AE RID: 174
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerDisplay("{Value}")]
	public sealed class YamlScalarNode : YamlNode, IYamlConvertible
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060003CA RID: 970 RVA: 0x0000D87B File Offset: 0x0000BA7B
		// (set) Token: 0x060003CB RID: 971 RVA: 0x0000D883 File Offset: 0x0000BA83
		[Nullable(2)]
		public string Value
		{
			[NullableContext(2)]
			get
			{
				return this.value;
			}
			[NullableContext(2)]
			set
			{
				if (value == null)
				{
					this.forceImplicitPlain = true;
				}
				else
				{
					this.forceImplicitPlain = false;
				}
				this.value = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060003CC RID: 972 RVA: 0x0000D89F File Offset: 0x0000BA9F
		// (set) Token: 0x060003CD RID: 973 RVA: 0x0000D8A7 File Offset: 0x0000BAA7
		public ScalarStyle Style { get; set; }

		// Token: 0x060003CE RID: 974 RVA: 0x0000D8B0 File Offset: 0x0000BAB0
		internal YamlScalarNode(IParser parser, DocumentLoadingState state)
		{
			this.Load(parser, state);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000D8C0 File Offset: 0x0000BAC0
		[MethodImpl(256)]
		private void Load(IParser parser, DocumentLoadingState state)
		{
			Scalar scalar = parser.Consume<Scalar>();
			base.Load(scalar, state);
			string text = scalar.Value;
			if (scalar.Style == ScalarStyle.Plain && base.Tag.IsEmpty)
			{
				bool flag;
				switch (text.Length)
				{
				case 0:
					flag = true;
					goto IL_93;
				case 1:
					flag = (text == "~");
					goto IL_93;
				case 4:
					flag = (text == "null" || text == "Null" || text == "NULL");
					goto IL_93;
				}
				flag = false;
				IL_93:
				this.forceImplicitPlain = flag;
			}
			this.value = text;
			this.Style = scalar.Style;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000D97A File Offset: 0x0000BB7A
		public YamlScalarNode()
		{
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000D982 File Offset: 0x0000BB82
		[NullableContext(2)]
		public YamlScalarNode(string value)
		{
			this.Value = value;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000D991 File Offset: 0x0000BB91
		internal override void ResolveAliases(DocumentLoadingState state)
		{
			throw new NotSupportedException("Resolving an alias on a scalar node does not make sense");
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000D9A0 File Offset: 0x0000BBA0
		internal override void Emit(IEmitter emitter, EmitterState state)
		{
			TagName tag = base.Tag;
			bool isPlainImplicit = tag.IsEmpty;
			if (this.forceImplicitPlain && this.Style == ScalarStyle.Plain && (this.Value == null || this.Value == ""))
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

		// Token: 0x060003D4 RID: 980 RVA: 0x0000DA44 File Offset: 0x0000BC44
		public override void Accept(IYamlVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000DA50 File Offset: 0x0000BC50
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			YamlScalarNode yamlScalarNode = obj as YamlScalarNode;
			return yamlScalarNode != null && object.Equals(base.Tag, yamlScalarNode.Tag) && object.Equals(this.Value, yamlScalarNode.Value);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000DA98 File Offset: 0x0000BC98
		public override int GetHashCode()
		{
			return HashCode.CombineHashCodes(base.Tag.GetHashCode(), this.Value);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000DAC4 File Offset: 0x0000BCC4
		[return: Nullable(2)]
		public static explicit operator string(YamlScalarNode value)
		{
			return value.Value;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000DACC File Offset: 0x0000BCCC
		internal override string ToString(RecursionLevel level)
		{
			return this.Value ?? string.Empty;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000DADD File Offset: 0x0000BCDD
		internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
		{
			yield return this;
			yield break;
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0000DAED File Offset: 0x0000BCED
		public override YamlNodeType NodeType
		{
			get
			{
				return YamlNodeType.Scalar;
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000DAF0 File Offset: 0x0000BCF0
		void IYamlConvertible.Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			this.Load(parser, new DocumentLoadingState());
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000DAFE File Offset: 0x0000BCFE
		void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			this.Emit(emitter, new EmitterState());
		}

		// Token: 0x04000111 RID: 273
		private bool forceImplicitPlain;

		// Token: 0x04000112 RID: 274
		[Nullable(2)]
		private string value;
	}
}
