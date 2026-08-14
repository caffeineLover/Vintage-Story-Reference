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
	// Token: 0x0200018C RID: 396
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerDisplay("{Value}")]
	public sealed class YamlScalarNode : YamlNode, IYamlConvertible
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x0001BA4F File Offset: 0x00019C4F
		// (set) Token: 0x06000831 RID: 2097 RVA: 0x0001BA57 File Offset: 0x00019C57
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
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x0001BA73 File Offset: 0x00019C73
		// (set) Token: 0x06000833 RID: 2099 RVA: 0x0001BA7B File Offset: 0x00019C7B
		public ScalarStyle Style { get; set; }

		// Token: 0x06000834 RID: 2100 RVA: 0x0001BA84 File Offset: 0x00019C84
		internal YamlScalarNode(IParser parser, DocumentLoadingState state)
		{
			this.Load(parser, state);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0001BA94 File Offset: 0x00019C94
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

		// Token: 0x06000836 RID: 2102 RVA: 0x0001BB45 File Offset: 0x00019D45
		public YamlScalarNode()
		{
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001BB4D File Offset: 0x00019D4D
		[NullableContext(2)]
		public YamlScalarNode(string value)
		{
			this.Value = value;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0001BB5C File Offset: 0x00019D5C
		internal override void ResolveAliases(DocumentLoadingState state)
		{
			throw new NotSupportedException("Resolving an alias on a scalar node does not make sense");
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0001BB68 File Offset: 0x00019D68
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

		// Token: 0x0600083A RID: 2106 RVA: 0x0001BC0C File Offset: 0x00019E0C
		public override void Accept(IYamlVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001BC18 File Offset: 0x00019E18
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			YamlScalarNode yamlScalarNode = obj as YamlScalarNode;
			return yamlScalarNode != null && object.Equals(base.Tag, yamlScalarNode.Tag) && object.Equals(this.Value, yamlScalarNode.Value);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0001BC60 File Offset: 0x00019E60
		public override int GetHashCode()
		{
			return HashCode.CombineHashCodes(base.Tag.GetHashCode(), this.Value);
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0001BC8C File Offset: 0x00019E8C
		[return: Nullable(2)]
		public static explicit operator string(YamlScalarNode value)
		{
			return value.Value;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0001BC94 File Offset: 0x00019E94
		internal override string ToString(RecursionLevel level)
		{
			return this.Value ?? string.Empty;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0001BCA5 File Offset: 0x00019EA5
		internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
		{
			yield return this;
			yield break;
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x0001BCB5 File Offset: 0x00019EB5
		public override YamlNodeType NodeType
		{
			get
			{
				return YamlNodeType.Scalar;
			}
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0001BCB8 File Offset: 0x00019EB8
		void IYamlConvertible.Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			this.Load(parser, new DocumentLoadingState());
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0001B333 File Offset: 0x00019533
		void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			this.Emit(emitter, new EmitterState());
		}

		// Token: 0x04000388 RID: 904
		private bool _forceImplicitPlain;

		// Token: 0x04000389 RID: 905
		[Nullable(2)]
		private string _value;
	}
}
