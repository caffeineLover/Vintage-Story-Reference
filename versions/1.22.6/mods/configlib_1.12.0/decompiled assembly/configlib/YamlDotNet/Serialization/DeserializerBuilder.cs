using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.BufferedDeserialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;
using YamlDotNet.Serialization.NodeTypeResolvers;
using YamlDotNet.Serialization.ObjectFactories;
using YamlDotNet.Serialization.Schemas;
using YamlDotNet.Serialization.TypeInspectors;
using YamlDotNet.Serialization.TypeResolvers;
using YamlDotNet.Serialization.Utilities;
using YamlDotNet.Serialization.ValueDeserializers;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000084 RID: 132
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	[RequiresDynamicCode("This builder configures the deserializer to use reflection which is not compatible with ahead-of-time compilation or assembly trimming. You need to use the code generator/analyzer to generate static code and use the 'StaticDeserializerBuilder' object instead of this one.")]
	public sealed class DeserializerBuilder : BuilderSkeleton<DeserializerBuilder>
	{
		// Token: 0x0600034E RID: 846 RVA: 0x00010014 File Offset: 0x0000E214
		public DeserializerBuilder() : base(new StaticTypeResolver())
		{
			this.typeMappings = new Dictionary<Type, Type>();
			this.objectFactory = new Lazy<IObjectFactory>(() => new DefaultObjectFactory(this.typeMappings, this.settings), true);
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
			this.typeInspectorFactories.Add(typeof(YamlAttributeOverridesInspector), delegate(ITypeInspector inner)
			{
				if (this.overrides == null)
				{
					return inner;
				}
				return new YamlAttributeOverridesInspector(inner, this.overrides.Clone());
			});
			this.typeInspectorFactories.Add(typeof(ReadableAndWritablePropertiesTypeInspector), (ITypeInspector inner) => new ReadableAndWritablePropertiesTypeInspector(inner));
			LazyComponentRegistrationList<Nothing, INodeDeserializer> lazyComponentRegistrationList = new LazyComponentRegistrationList<Nothing, INodeDeserializer>();
			lazyComponentRegistrationList.Add(typeof(YamlConvertibleNodeDeserializer), (Nothing _) => new YamlConvertibleNodeDeserializer(this.objectFactory.Value));
			lazyComponentRegistrationList.Add(typeof(YamlSerializableNodeDeserializer), (Nothing _) => new YamlSerializableNodeDeserializer(this.objectFactory.Value));
			lazyComponentRegistrationList.Add(typeof(TypeConverterNodeDeserializer), (Nothing _) => new TypeConverterNodeDeserializer(base.BuildTypeConverters()));
			lazyComponentRegistrationList.Add(typeof(NullNodeDeserializer), (Nothing _) => new NullNodeDeserializer());
			lazyComponentRegistrationList.Add(typeof(ScalarNodeDeserializer), (Nothing _) => new ScalarNodeDeserializer(this.attemptUnknownTypeDeserialization, this.typeConverter, this.yamlFormatter));
			lazyComponentRegistrationList.Add(typeof(ArrayNodeDeserializer), (Nothing _) => new ArrayNodeDeserializer());
			lazyComponentRegistrationList.Add(typeof(DictionaryNodeDeserializer), (Nothing _) => new DictionaryNodeDeserializer(this.objectFactory.Value, this.duplicateKeyChecking));
			lazyComponentRegistrationList.Add(typeof(CollectionNodeDeserializer), (Nothing _) => new CollectionNodeDeserializer(this.objectFactory.Value));
			lazyComponentRegistrationList.Add(typeof(EnumerableNodeDeserializer), (Nothing _) => new EnumerableNodeDeserializer());
			lazyComponentRegistrationList.Add(typeof(ObjectNodeDeserializer), (Nothing _) => new ObjectNodeDeserializer(this.objectFactory.Value, this.BuildTypeInspector(), this.ignoreUnmatched, this.duplicateKeyChecking, this.typeConverter));
			this.nodeDeserializerFactories = lazyComponentRegistrationList;
			LazyComponentRegistrationList<Nothing, INodeTypeResolver> lazyComponentRegistrationList2 = new LazyComponentRegistrationList<Nothing, INodeTypeResolver>();
			lazyComponentRegistrationList2.Add(typeof(MappingNodeTypeResolver), (Nothing _) => new MappingNodeTypeResolver(this.typeMappings));
			lazyComponentRegistrationList2.Add(typeof(YamlConvertibleTypeResolver), (Nothing _) => new YamlConvertibleTypeResolver());
			lazyComponentRegistrationList2.Add(typeof(YamlSerializableTypeResolver), (Nothing _) => new YamlSerializableTypeResolver());
			lazyComponentRegistrationList2.Add(typeof(TagNodeTypeResolver), (Nothing _) => new TagNodeTypeResolver(this.tagMappings));
			lazyComponentRegistrationList2.Add(typeof(PreventUnknownTagsNodeTypeResolver), (Nothing _) => new PreventUnknownTagsNodeTypeResolver());
			lazyComponentRegistrationList2.Add(typeof(DefaultContainersNodeTypeResolver), (Nothing _) => new DefaultContainersNodeTypeResolver());
			this.nodeTypeResolverFactories = lazyComponentRegistrationList2;
			this.typeConverter = new ReflectionTypeConverter();
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600034F RID: 847 RVA: 0x0000E3B0 File Offset: 0x0000C5B0
		protected override DeserializerBuilder Self
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0001041C File Offset: 0x0000E61C
		internal ITypeInspector BuildTypeInspector()
		{
			ITypeInspector typeInspector = new WritablePropertiesTypeInspector(this.typeResolver, this.includeNonPublicProperties);
			if (!this.ignoreFields)
			{
				typeInspector = new CompositeTypeInspector(new ITypeInspector[]
				{
					new ReadableFieldsTypeInspector(this.typeResolver),
					typeInspector
				});
			}
			return this.typeInspectorFactories.BuildComponentChain(typeInspector);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0001046D File Offset: 0x0000E66D
		public DeserializerBuilder WithAttemptingUnquotedStringTypeDeserialization()
		{
			this.attemptUnknownTypeDeserialization = true;
			return this;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00010478 File Offset: 0x0000E678
		public DeserializerBuilder WithObjectFactory(IObjectFactory objectFactory)
		{
			if (objectFactory == null)
			{
				throw new ArgumentNullException("objectFactory");
			}
			this.objectFactory = new Lazy<IObjectFactory>(() => objectFactory, true);
			return this;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x000104BE File Offset: 0x0000E6BE
		public DeserializerBuilder WithObjectFactory(Func<Type, object> objectFactory)
		{
			if (objectFactory == null)
			{
				throw new ArgumentNullException("objectFactory");
			}
			return this.WithObjectFactory(new LambdaObjectFactory(objectFactory));
		}

		// Token: 0x06000354 RID: 852 RVA: 0x000104DA File Offset: 0x0000E6DA
		public DeserializerBuilder WithNodeDeserializer(INodeDeserializer nodeDeserializer)
		{
			return this.WithNodeDeserializer(nodeDeserializer, delegate(IRegistrationLocationSelectionSyntax<INodeDeserializer> w)
			{
				w.OnTop();
			});
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00010504 File Offset: 0x0000E704
		public DeserializerBuilder WithNodeDeserializer(INodeDeserializer nodeDeserializer, Action<IRegistrationLocationSelectionSyntax<INodeDeserializer>> where)
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

		// Token: 0x06000356 RID: 854 RVA: 0x00010568 File Offset: 0x0000E768
		public DeserializerBuilder WithNodeDeserializer<[Nullable(0)] TNodeDeserializer>(WrapperFactory<INodeDeserializer, TNodeDeserializer> nodeDeserializerFactory, Action<ITrackingRegistrationLocationSelectionSyntax<INodeDeserializer>> where) where TNodeDeserializer : INodeDeserializer
		{
			DeserializerBuilder.<>c__DisplayClass18_0<TNodeDeserializer> CS$<>8__locals1 = new DeserializerBuilder.<>c__DisplayClass18_0<TNodeDeserializer>();
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

		// Token: 0x06000357 RID: 855 RVA: 0x000105CB File Offset: 0x0000E7CB
		public DeserializerBuilder WithoutNodeDeserializer<[Nullable(0)] TNodeDeserializer>() where TNodeDeserializer : INodeDeserializer
		{
			return this.WithoutNodeDeserializer(typeof(TNodeDeserializer));
		}

		// Token: 0x06000358 RID: 856 RVA: 0x000105DD File Offset: 0x0000E7DD
		public DeserializerBuilder WithoutNodeDeserializer(Type nodeDeserializerType)
		{
			if (nodeDeserializerType == null)
			{
				throw new ArgumentNullException("nodeDeserializerType");
			}
			this.nodeDeserializerFactories.Remove(nodeDeserializerType);
			return this;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00010600 File Offset: 0x0000E800
		public DeserializerBuilder WithTypeDiscriminatingNodeDeserializer(Action<ITypeDiscriminatingNodeDeserializerOptions> configureTypeDiscriminatingNodeDeserializerOptions, int maxDepth = -1, int maxLength = -1)
		{
			TypeDiscriminatingNodeDeserializerOptions typeDiscriminatingNodeDeserializerOptions = new TypeDiscriminatingNodeDeserializerOptions();
			configureTypeDiscriminatingNodeDeserializerOptions.Invoke(typeDiscriminatingNodeDeserializerOptions);
			TypeDiscriminatingNodeDeserializer nodeDeserializer = new TypeDiscriminatingNodeDeserializer(this.nodeDeserializerFactories.BuildComponentList<INodeDeserializer>(), typeDiscriminatingNodeDeserializerOptions.discriminators, maxDepth, maxLength);
			return this.WithNodeDeserializer(nodeDeserializer, delegate(IRegistrationLocationSelectionSyntax<INodeDeserializer> s)
			{
				s.Before<DictionaryNodeDeserializer>();
			});
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00010659 File Offset: 0x0000E859
		public DeserializerBuilder WithNodeTypeResolver(INodeTypeResolver nodeTypeResolver)
		{
			return this.WithNodeTypeResolver(nodeTypeResolver, delegate(IRegistrationLocationSelectionSyntax<INodeTypeResolver> w)
			{
				w.OnTop();
			});
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00010684 File Offset: 0x0000E884
		public DeserializerBuilder WithNodeTypeResolver(INodeTypeResolver nodeTypeResolver, Action<IRegistrationLocationSelectionSyntax<INodeTypeResolver>> where)
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

		// Token: 0x0600035C RID: 860 RVA: 0x000106E8 File Offset: 0x0000E8E8
		public DeserializerBuilder WithNodeTypeResolver<[Nullable(0)] TNodeTypeResolver>(WrapperFactory<INodeTypeResolver, TNodeTypeResolver> nodeTypeResolverFactory, Action<ITrackingRegistrationLocationSelectionSyntax<INodeTypeResolver>> where) where TNodeTypeResolver : INodeTypeResolver
		{
			DeserializerBuilder.<>c__DisplayClass24_0<TNodeTypeResolver> CS$<>8__locals1 = new DeserializerBuilder.<>c__DisplayClass24_0<TNodeTypeResolver>();
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

		// Token: 0x0600035D RID: 861 RVA: 0x0001074B File Offset: 0x0000E94B
		public DeserializerBuilder WithoutNodeTypeResolver<[Nullable(0)] TNodeTypeResolver>() where TNodeTypeResolver : INodeTypeResolver
		{
			return this.WithoutNodeTypeResolver(typeof(TNodeTypeResolver));
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0001075D File Offset: 0x0000E95D
		public DeserializerBuilder WithoutNodeTypeResolver(Type nodeTypeResolverType)
		{
			if (nodeTypeResolverType == null)
			{
				throw new ArgumentNullException("nodeTypeResolverType");
			}
			this.nodeTypeResolverFactories.Remove(nodeTypeResolverType);
			return this;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00010780 File Offset: 0x0000E980
		public override DeserializerBuilder WithTagMapping(TagName tag, Type type)
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

		// Token: 0x06000360 RID: 864 RVA: 0x00010828 File Offset: 0x0000EA28
		public DeserializerBuilder WithTypeMapping<[Nullable(2)] TInterface, [Nullable(0)] TConcrete>() where TConcrete : TInterface
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

		// Token: 0x06000361 RID: 865 RVA: 0x000108D4 File Offset: 0x0000EAD4
		public DeserializerBuilder WithoutTagMapping(TagName tag)
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

		// Token: 0x06000362 RID: 866 RVA: 0x0001093B File Offset: 0x0000EB3B
		public DeserializerBuilder IgnoreUnmatchedProperties()
		{
			this.ignoreUnmatched = true;
			return this;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00010945 File Offset: 0x0000EB45
		public DeserializerBuilder WithDuplicateKeyChecking()
		{
			this.duplicateKeyChecking = true;
			return this;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0001094F File Offset: 0x0000EB4F
		public IDeserializer Build()
		{
			return Deserializer.FromValueDeserializer(this.BuildValueDeserializer());
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0001095C File Offset: 0x0000EB5C
		public IValueDeserializer BuildValueDeserializer()
		{
			return new AliasValueDeserializer(new NodeValueDeserializer(this.nodeDeserializerFactories.BuildComponentList<INodeDeserializer>(), this.nodeTypeResolverFactories.BuildComponentList<INodeTypeResolver>(), this.typeConverter));
		}

		// Token: 0x040001A6 RID: 422
		private Lazy<IObjectFactory> objectFactory;

		// Token: 0x040001A7 RID: 423
		private readonly LazyComponentRegistrationList<Nothing, INodeDeserializer> nodeDeserializerFactories;

		// Token: 0x040001A8 RID: 424
		private readonly LazyComponentRegistrationList<Nothing, INodeTypeResolver> nodeTypeResolverFactories;

		// Token: 0x040001A9 RID: 425
		private readonly Dictionary<TagName, Type> tagMappings;

		// Token: 0x040001AA RID: 426
		private readonly Dictionary<Type, Type> typeMappings;

		// Token: 0x040001AB RID: 427
		private readonly ITypeConverter typeConverter;

		// Token: 0x040001AC RID: 428
		private bool ignoreUnmatched;

		// Token: 0x040001AD RID: 429
		private bool duplicateKeyChecking;

		// Token: 0x040001AE RID: 430
		private bool attemptUnknownTypeDeserialization;
	}
}
