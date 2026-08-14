using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.BufferedDeserialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;
using YamlDotNet.Serialization.NodeTypeResolvers;
using YamlDotNet.Serialization.ObjectFactories;
using YamlDotNet.Serialization.Schemas;
using YamlDotNet.Serialization.TypeInspectors;
using YamlDotNet.Serialization.Utilities;
using YamlDotNet.Serialization.ValueDeserializers;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000D6 RID: 214
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	public sealed class StaticDeserializerBuilder : StaticBuilderSkeleton<StaticDeserializerBuilder>
	{
		// Token: 0x060004DF RID: 1247 RVA: 0x000127F4 File Offset: 0x000109F4
		public StaticDeserializerBuilder(StaticContext context) : base(context.GetTypeResolver())
		{
			this.context = context;
			this.factory = context.GetFactory();
			this.typeMappings = new Dictionary<Type, Type>();
			Dictionary<TagName, Type> dictionary = new Dictionary<TagName, Type>();
			dictionary.Add(FailsafeSchema.Tags.Map, typeof(Dictionary<object, object>));
			dictionary.Add(FailsafeSchema.Tags.Str, typeof(string));
			dictionary.Add(JsonSchema.Tags.Bool, typeof(bool));
			dictionary.Add(JsonSchema.Tags.Float, typeof(double));
			dictionary.Add(JsonSchema.Tags.Int, typeof(int));
			dictionary.Add(DefaultSchema.Tags.Timestamp, typeof(DateTime));
			this.tagMappings = dictionary;
			this.typeInspectorFactories.Add(typeof(CachedTypeInspector), (ITypeInspector inner) => new CachedTypeInspector(inner));
			this.typeInspectorFactories.Add(typeof(NamingConventionTypeInspector), delegate(ITypeInspector inner)
			{
				if (!(this.namingConvention is NullNamingConvention))
				{
					return new NamingConventionTypeInspector(inner, this.namingConvention);
				}
				return inner;
			});
			this.typeInspectorFactories.Add(typeof(YamlAttributesTypeInspector), (ITypeInspector inner) => new YamlAttributesTypeInspector(inner));
			LazyComponentRegistrationList<Nothing, INodeDeserializer> lazyComponentRegistrationList = new LazyComponentRegistrationList<Nothing, INodeDeserializer>();
			lazyComponentRegistrationList.Add(typeof(YamlConvertibleNodeDeserializer), (Nothing _) => new YamlConvertibleNodeDeserializer(this.factory));
			lazyComponentRegistrationList.Add(typeof(YamlSerializableNodeDeserializer), (Nothing _) => new YamlSerializableNodeDeserializer(this.factory));
			lazyComponentRegistrationList.Add(typeof(TypeConverterNodeDeserializer), (Nothing _) => new TypeConverterNodeDeserializer(base.BuildTypeConverters()));
			lazyComponentRegistrationList.Add(typeof(NullNodeDeserializer), (Nothing _) => new NullNodeDeserializer());
			lazyComponentRegistrationList.Add(typeof(ScalarNodeDeserializer), (Nothing _) => new ScalarNodeDeserializer(this.attemptUnknownTypeDeserialization, this.typeConverter, this.yamlFormatter));
			lazyComponentRegistrationList.Add(typeof(StaticArrayNodeDeserializer), (Nothing _) => new StaticArrayNodeDeserializer(this.factory));
			lazyComponentRegistrationList.Add(typeof(StaticDictionaryNodeDeserializer), (Nothing _) => new StaticDictionaryNodeDeserializer(this.factory, this.duplicateKeyChecking));
			lazyComponentRegistrationList.Add(typeof(StaticCollectionNodeDeserializer), (Nothing _) => new StaticCollectionNodeDeserializer(this.factory));
			lazyComponentRegistrationList.Add(typeof(ObjectNodeDeserializer), (Nothing _) => new ObjectNodeDeserializer(this.factory, this.BuildTypeInspector(), this.ignoreUnmatched, this.duplicateKeyChecking, this.typeConverter));
			this.nodeDeserializerFactories = lazyComponentRegistrationList;
			LazyComponentRegistrationList<Nothing, INodeTypeResolver> lazyComponentRegistrationList2 = new LazyComponentRegistrationList<Nothing, INodeTypeResolver>();
			lazyComponentRegistrationList2.Add(typeof(MappingNodeTypeResolver), (Nothing _) => new MappingNodeTypeResolver(this.typeMappings));
			lazyComponentRegistrationList2.Add(typeof(YamlConvertibleTypeResolver), (Nothing _) => new YamlConvertibleTypeResolver());
			lazyComponentRegistrationList2.Add(typeof(YamlSerializableTypeResolver), (Nothing _) => new YamlSerializableTypeResolver());
			lazyComponentRegistrationList2.Add(typeof(TagNodeTypeResolver), (Nothing _) => new TagNodeTypeResolver(this.tagMappings));
			lazyComponentRegistrationList2.Add(typeof(PreventUnknownTagsNodeTypeResolver), (Nothing _) => new PreventUnknownTagsNodeTypeResolver());
			lazyComponentRegistrationList2.Add(typeof(DefaultContainersNodeTypeResolver), (Nothing _) => new DefaultContainersNodeTypeResolver());
			this.nodeTypeResolverFactories = lazyComponentRegistrationList2;
			this.typeConverter = new NullTypeConverter();
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x0000E3B0 File Offset: 0x0000C5B0
		protected override StaticDeserializerBuilder Self
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00012B64 File Offset: 0x00010D64
		internal ITypeInspector BuildTypeInspector()
		{
			ITypeInspector typeInspector = this.context.GetTypeInspector();
			return this.typeInspectorFactories.BuildComponentChain(typeInspector);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00012B89 File Offset: 0x00010D89
		public StaticDeserializerBuilder WithAttemptingUnquotedStringTypeDeserialization()
		{
			this.attemptUnknownTypeDeserialization = true;
			return this;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00012B93 File Offset: 0x00010D93
		public StaticDeserializerBuilder WithNodeDeserializer(INodeDeserializer nodeDeserializer)
		{
			return this.WithNodeDeserializer(nodeDeserializer, delegate(IRegistrationLocationSelectionSyntax<INodeDeserializer> w)
			{
				w.OnTop();
			});
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00012BBC File Offset: 0x00010DBC
		public StaticDeserializerBuilder WithNodeDeserializer(INodeDeserializer nodeDeserializer, Action<IRegistrationLocationSelectionSyntax<INodeDeserializer>> where)
		{
			if (nodeDeserializer == null)
			{
				throw new ArgumentNullException("nodeDeserializer");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.nodeDeserializerFactories.CreateRegistrationLocationSelector(nodeDeserializer.GetType(), (Nothing _) => nodeDeserializer));
			return this;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00012C20 File Offset: 0x00010E20
		public StaticDeserializerBuilder WithNodeDeserializer<[Nullable(0)] TNodeDeserializer>(WrapperFactory<INodeDeserializer, TNodeDeserializer> nodeDeserializerFactory, Action<ITrackingRegistrationLocationSelectionSyntax<INodeDeserializer>> where) where TNodeDeserializer : INodeDeserializer
		{
			StaticDeserializerBuilder.<>c__DisplayClass17_0<TNodeDeserializer> CS$<>8__locals1 = new StaticDeserializerBuilder.<>c__DisplayClass17_0<TNodeDeserializer>();
			CS$<>8__locals1.nodeDeserializerFactory = nodeDeserializerFactory;
			if (CS$<>8__locals1.nodeDeserializerFactory == null)
			{
				throw new ArgumentNullException("nodeDeserializerFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.nodeDeserializerFactories.CreateTrackingRegistrationLocationSelector(typeof(TNodeDeserializer), new Func<INodeDeserializer, Nothing, INodeDeserializer>(CS$<>8__locals1.<WithNodeDeserializer>b__0)));
			return this;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00012C83 File Offset: 0x00010E83
		public StaticDeserializerBuilder WithoutNodeDeserializer<[Nullable(0)] TNodeDeserializer>() where TNodeDeserializer : INodeDeserializer
		{
			return this.WithoutNodeDeserializer(typeof(TNodeDeserializer));
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00012C95 File Offset: 0x00010E95
		public StaticDeserializerBuilder WithoutNodeDeserializer(Type nodeDeserializerType)
		{
			if (nodeDeserializerType == null)
			{
				throw new ArgumentNullException("nodeDeserializerType");
			}
			this.nodeDeserializerFactories.Remove(nodeDeserializerType);
			return this;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00012CB8 File Offset: 0x00010EB8
		public StaticDeserializerBuilder WithTypeDiscriminatingNodeDeserializer(Action<ITypeDiscriminatingNodeDeserializerOptions> configureTypeDiscriminatingNodeDeserializerOptions, int maxDepth = -1, int maxLength = -1)
		{
			TypeDiscriminatingNodeDeserializerOptions typeDiscriminatingNodeDeserializerOptions = new TypeDiscriminatingNodeDeserializerOptions();
			configureTypeDiscriminatingNodeDeserializerOptions.Invoke(typeDiscriminatingNodeDeserializerOptions);
			TypeDiscriminatingNodeDeserializer nodeDeserializer = new TypeDiscriminatingNodeDeserializer(this.nodeDeserializerFactories.BuildComponentList<INodeDeserializer>(), typeDiscriminatingNodeDeserializerOptions.discriminators, maxDepth, maxLength);
			return this.WithNodeDeserializer(nodeDeserializer, delegate(IRegistrationLocationSelectionSyntax<INodeDeserializer> s)
			{
				s.Before<DictionaryNodeDeserializer>();
			});
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00012D11 File Offset: 0x00010F11
		public StaticDeserializerBuilder WithNodeTypeResolver(INodeTypeResolver nodeTypeResolver)
		{
			return this.WithNodeTypeResolver(nodeTypeResolver, delegate(IRegistrationLocationSelectionSyntax<INodeTypeResolver> w)
			{
				w.OnTop();
			});
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00012D3C File Offset: 0x00010F3C
		public StaticDeserializerBuilder WithNodeTypeResolver(INodeTypeResolver nodeTypeResolver, Action<IRegistrationLocationSelectionSyntax<INodeTypeResolver>> where)
		{
			if (nodeTypeResolver == null)
			{
				throw new ArgumentNullException("nodeTypeResolver");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.nodeTypeResolverFactories.CreateRegistrationLocationSelector(nodeTypeResolver.GetType(), (Nothing _) => nodeTypeResolver));
			return this;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00012DA0 File Offset: 0x00010FA0
		public StaticDeserializerBuilder WithNodeTypeResolver<[Nullable(0)] TNodeTypeResolver>(WrapperFactory<INodeTypeResolver, TNodeTypeResolver> nodeTypeResolverFactory, Action<ITrackingRegistrationLocationSelectionSyntax<INodeTypeResolver>> where) where TNodeTypeResolver : INodeTypeResolver
		{
			StaticDeserializerBuilder.<>c__DisplayClass23_0<TNodeTypeResolver> CS$<>8__locals1 = new StaticDeserializerBuilder.<>c__DisplayClass23_0<TNodeTypeResolver>();
			CS$<>8__locals1.nodeTypeResolverFactory = nodeTypeResolverFactory;
			if (CS$<>8__locals1.nodeTypeResolverFactory == null)
			{
				throw new ArgumentNullException("nodeTypeResolverFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.nodeTypeResolverFactories.CreateTrackingRegistrationLocationSelector(typeof(TNodeTypeResolver), new Func<INodeTypeResolver, Nothing, INodeTypeResolver>(CS$<>8__locals1.<WithNodeTypeResolver>b__0)));
			return this;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00012E03 File Offset: 0x00011003
		public StaticDeserializerBuilder WithoutNodeTypeResolver<[Nullable(0)] TNodeTypeResolver>() where TNodeTypeResolver : INodeTypeResolver
		{
			return this.WithoutNodeTypeResolver(typeof(TNodeTypeResolver));
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00012E15 File Offset: 0x00011015
		public StaticDeserializerBuilder WithoutNodeTypeResolver(Type nodeTypeResolverType)
		{
			if (nodeTypeResolverType == null)
			{
				throw new ArgumentNullException("nodeTypeResolverType");
			}
			this.nodeTypeResolverFactories.Remove(nodeTypeResolverType);
			return this;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00012E38 File Offset: 0x00011038
		public override StaticDeserializerBuilder WithTagMapping(TagName tag, Type type)
		{
			if (tag.IsEmpty)
			{
				throw new ArgumentException("Non-specific tags cannot be maped");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Type type2;
			if (this.tagMappings.TryGetValue(tag, ref type2))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(48, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Type already has a registered type '");
				defaultInterpolatedStringHandler.AppendFormatted(type2.FullName);
				defaultInterpolatedStringHandler.AppendLiteral("' for tag '");
				defaultInterpolatedStringHandler.AppendFormatted<TagName>(tag);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), "tag");
			}
			this.tagMappings.Add(tag, type);
			return this;
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00012EE0 File Offset: 0x000110E0
		public StaticDeserializerBuilder WithTypeMapping<[Nullable(2)] TInterface, [Nullable(0)] TConcrete>() where TConcrete : TInterface
		{
			Type typeFromHandle = typeof(TInterface);
			Type typeFromHandle2 = typeof(TConcrete);
			if (!typeFromHandle.IsAssignableFrom(typeFromHandle2))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(44, 2);
				defaultInterpolatedStringHandler.AppendLiteral("The type '");
				defaultInterpolatedStringHandler.AppendFormatted(typeFromHandle2.Name);
				defaultInterpolatedStringHandler.AppendLiteral("' does not implement interface '");
				defaultInterpolatedStringHandler.AppendFormatted(typeFromHandle.Name);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (this.typeMappings.ContainsKey(typeFromHandle))
			{
				this.typeMappings[typeFromHandle] = typeFromHandle2;
			}
			else
			{
				this.typeMappings.Add(typeFromHandle, typeFromHandle2);
			}
			return this;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00012F8C File Offset: 0x0001118C
		public StaticDeserializerBuilder WithoutTagMapping(TagName tag)
		{
			if (tag.IsEmpty)
			{
				throw new ArgumentException("Non-specific tags cannot be maped");
			}
			if (!this.tagMappings.Remove(tag))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(24, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tag '");
				defaultInterpolatedStringHandler.AppendFormatted<TagName>(tag);
				defaultInterpolatedStringHandler.AppendLiteral("' is not registered");
				throw new KeyNotFoundException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00012FF3 File Offset: 0x000111F3
		public StaticDeserializerBuilder IgnoreUnmatchedProperties()
		{
			this.ignoreUnmatched = true;
			return this;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00012FFD File Offset: 0x000111FD
		public StaticDeserializerBuilder WithDuplicateKeyChecking()
		{
			this.duplicateKeyChecking = true;
			return this;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00013007 File Offset: 0x00011207
		public IDeserializer Build()
		{
			return Deserializer.FromValueDeserializer(this.BuildValueDeserializer());
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00013014 File Offset: 0x00011214
		public IValueDeserializer BuildValueDeserializer()
		{
			return new AliasValueDeserializer(new NodeValueDeserializer(this.nodeDeserializerFactories.BuildComponentList<INodeDeserializer>(), this.nodeTypeResolverFactories.BuildComponentList<INodeTypeResolver>(), this.typeConverter));
		}

		// Token: 0x04000232 RID: 562
		private readonly StaticContext context;

		// Token: 0x04000233 RID: 563
		private readonly StaticObjectFactory factory;

		// Token: 0x04000234 RID: 564
		private readonly LazyComponentRegistrationList<Nothing, INodeDeserializer> nodeDeserializerFactories;

		// Token: 0x04000235 RID: 565
		private readonly LazyComponentRegistrationList<Nothing, INodeTypeResolver> nodeTypeResolverFactories;

		// Token: 0x04000236 RID: 566
		private readonly Dictionary<TagName, Type> tagMappings;

		// Token: 0x04000237 RID: 567
		private readonly ITypeConverter typeConverter;

		// Token: 0x04000238 RID: 568
		private readonly Dictionary<Type, Type> typeMappings;

		// Token: 0x04000239 RID: 569
		private bool ignoreUnmatched;

		// Token: 0x0400023A RID: 570
		private bool duplicateKeyChecking;

		// Token: 0x0400023B RID: 571
		private bool attemptUnknownTypeDeserialization;
	}
}
