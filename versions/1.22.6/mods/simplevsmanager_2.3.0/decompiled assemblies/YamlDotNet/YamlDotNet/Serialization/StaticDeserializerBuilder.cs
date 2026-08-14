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
	// Token: 0x0200003B RID: 59
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	public sealed class StaticDeserializerBuilder : StaticBuilderSkeleton<StaticDeserializerBuilder>
	{
		// Token: 0x06000183 RID: 387 RVA: 0x00004FE0 File Offset: 0x000031E0
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
			lazyComponentRegistrationList.Add(typeof(ScalarNodeDeserializer), (Nothing _) => new ScalarNodeDeserializer(this.attemptUnknownTypeDeserialization, this.typeConverter, this.BuildTypeInspector(), this.yamlFormatter, this.enumNamingConvention));
			lazyComponentRegistrationList.Add(typeof(StaticArrayNodeDeserializer), (Nothing _) => new StaticArrayNodeDeserializer(this.factory));
			lazyComponentRegistrationList.Add(typeof(StaticDictionaryNodeDeserializer), (Nothing _) => new StaticDictionaryNodeDeserializer(this.factory, this.duplicateKeyChecking));
			lazyComponentRegistrationList.Add(typeof(StaticCollectionNodeDeserializer), (Nothing _) => new StaticCollectionNodeDeserializer(this.factory));
			lazyComponentRegistrationList.Add(typeof(ObjectNodeDeserializer), (Nothing _) => new ObjectNodeDeserializer(this.factory, this.BuildTypeInspector(), this.ignoreUnmatched, this.duplicateKeyChecking, this.typeConverter, this.enumNamingConvention, this.enforceNullability, this.caseInsensitivePropertyMatching, false, base.BuildTypeConverters()));
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00005353 File Offset: 0x00003553
		protected override StaticDeserializerBuilder Self
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00005358 File Offset: 0x00003558
		public ITypeInspector BuildTypeInspector()
		{
			ITypeInspector typeInspector = this.context.GetTypeInspector();
			return this.typeInspectorFactories.BuildComponentChain(typeInspector);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000537D File Offset: 0x0000357D
		public StaticDeserializerBuilder WithAttemptingUnquotedStringTypeDeserialization()
		{
			this.attemptUnknownTypeDeserialization = true;
			return this;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00005387 File Offset: 0x00003587
		public StaticDeserializerBuilder WithNodeDeserializer(INodeDeserializer nodeDeserializer)
		{
			return this.WithNodeDeserializer(nodeDeserializer, delegate(IRegistrationLocationSelectionSyntax<INodeDeserializer> w)
			{
				w.OnTop();
			});
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000053B0 File Offset: 0x000035B0
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

		// Token: 0x06000189 RID: 393 RVA: 0x00005414 File Offset: 0x00003614
		public StaticDeserializerBuilder WithNodeDeserializer<[Nullable(0)] TNodeDeserializer>(WrapperFactory<INodeDeserializer, TNodeDeserializer> nodeDeserializerFactory, Action<ITrackingRegistrationLocationSelectionSyntax<INodeDeserializer>> where) where TNodeDeserializer : INodeDeserializer
		{
			StaticDeserializerBuilder.<>c__DisplayClass19_0<TNodeDeserializer> CS$<>8__locals1 = new StaticDeserializerBuilder.<>c__DisplayClass19_0<TNodeDeserializer>();
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

		// Token: 0x0600018A RID: 394 RVA: 0x00005477 File Offset: 0x00003677
		public StaticDeserializerBuilder WithCaseInsensitivePropertyMatching()
		{
			this.caseInsensitivePropertyMatching = true;
			return this;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00005481 File Offset: 0x00003681
		public StaticDeserializerBuilder WithEnforceNullability()
		{
			this.enforceNullability = true;
			return this;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000548B File Offset: 0x0000368B
		public StaticDeserializerBuilder WithoutNodeDeserializer<[Nullable(0)] TNodeDeserializer>() where TNodeDeserializer : INodeDeserializer
		{
			return this.WithoutNodeDeserializer(typeof(TNodeDeserializer));
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000549D File Offset: 0x0000369D
		public StaticDeserializerBuilder WithoutNodeDeserializer(Type nodeDeserializerType)
		{
			if (nodeDeserializerType == null)
			{
				throw new ArgumentNullException("nodeDeserializerType");
			}
			this.nodeDeserializerFactories.Remove(nodeDeserializerType);
			return this;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000054C0 File Offset: 0x000036C0
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

		// Token: 0x0600018F RID: 399 RVA: 0x00005519 File Offset: 0x00003719
		public StaticDeserializerBuilder WithNodeTypeResolver(INodeTypeResolver nodeTypeResolver)
		{
			return this.WithNodeTypeResolver(nodeTypeResolver, delegate(IRegistrationLocationSelectionSyntax<INodeTypeResolver> w)
			{
				w.OnTop();
			});
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00005544 File Offset: 0x00003744
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

		// Token: 0x06000191 RID: 401 RVA: 0x000055A8 File Offset: 0x000037A8
		public StaticDeserializerBuilder WithNodeTypeResolver<[Nullable(0)] TNodeTypeResolver>(WrapperFactory<INodeTypeResolver, TNodeTypeResolver> nodeTypeResolverFactory, Action<ITrackingRegistrationLocationSelectionSyntax<INodeTypeResolver>> where) where TNodeTypeResolver : INodeTypeResolver
		{
			StaticDeserializerBuilder.<>c__DisplayClass27_0<TNodeTypeResolver> CS$<>8__locals1 = new StaticDeserializerBuilder.<>c__DisplayClass27_0<TNodeTypeResolver>();
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

		// Token: 0x06000192 RID: 402 RVA: 0x0000560B File Offset: 0x0000380B
		public StaticDeserializerBuilder WithoutNodeTypeResolver<[Nullable(0)] TNodeTypeResolver>() where TNodeTypeResolver : INodeTypeResolver
		{
			return this.WithoutNodeTypeResolver(typeof(TNodeTypeResolver));
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000561D File Offset: 0x0000381D
		public StaticDeserializerBuilder WithoutNodeTypeResolver(Type nodeTypeResolverType)
		{
			if (nodeTypeResolverType == null)
			{
				throw new ArgumentNullException("nodeTypeResolverType");
			}
			this.nodeTypeResolverFactories.Remove(nodeTypeResolverType);
			return this;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00005640 File Offset: 0x00003840
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

		// Token: 0x06000195 RID: 405 RVA: 0x000056E8 File Offset: 0x000038E8
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
			this.typeMappings[typeFromHandle] = typeFromHandle2;
			return this;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00005778 File Offset: 0x00003978
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

		// Token: 0x06000197 RID: 407 RVA: 0x000057DF File Offset: 0x000039DF
		public StaticDeserializerBuilder IgnoreUnmatchedProperties()
		{
			this.ignoreUnmatched = true;
			return this;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000057E9 File Offset: 0x000039E9
		public StaticDeserializerBuilder WithDuplicateKeyChecking()
		{
			this.duplicateKeyChecking = true;
			return this;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000057F3 File Offset: 0x000039F3
		public IDeserializer Build()
		{
			return Deserializer.FromValueDeserializer(this.BuildValueDeserializer());
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00005800 File Offset: 0x00003A00
		public IValueDeserializer BuildValueDeserializer()
		{
			return new AliasValueDeserializer(new NodeValueDeserializer(this.nodeDeserializerFactories.BuildComponentList<INodeDeserializer>(), this.nodeTypeResolverFactories.BuildComponentList<INodeTypeResolver>(), this.typeConverter, this.enumNamingConvention, this.BuildTypeInspector()));
		}

		// Token: 0x04000052 RID: 82
		private readonly StaticContext context;

		// Token: 0x04000053 RID: 83
		private readonly StaticObjectFactory factory;

		// Token: 0x04000054 RID: 84
		private readonly LazyComponentRegistrationList<Nothing, INodeDeserializer> nodeDeserializerFactories;

		// Token: 0x04000055 RID: 85
		private readonly LazyComponentRegistrationList<Nothing, INodeTypeResolver> nodeTypeResolverFactories;

		// Token: 0x04000056 RID: 86
		private readonly Dictionary<TagName, Type> tagMappings;

		// Token: 0x04000057 RID: 87
		private readonly ITypeConverter typeConverter;

		// Token: 0x04000058 RID: 88
		private readonly Dictionary<Type, Type> typeMappings;

		// Token: 0x04000059 RID: 89
		private bool ignoreUnmatched;

		// Token: 0x0400005A RID: 90
		private bool duplicateKeyChecking;

		// Token: 0x0400005B RID: 91
		private bool attemptUnknownTypeDeserialization;

		// Token: 0x0400005C RID: 92
		private bool enforceNullability;

		// Token: 0x0400005D RID: 93
		private bool caseInsensitivePropertyMatching;
	}
}
