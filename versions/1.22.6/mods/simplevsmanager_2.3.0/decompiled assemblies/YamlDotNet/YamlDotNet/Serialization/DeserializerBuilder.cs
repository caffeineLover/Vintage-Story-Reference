using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Helpers;
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
	// Token: 0x0200000C RID: 12
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	[RequiresDynamicCode("This builder configures the deserializer to use reflection which is not compatible with ahead-of-time compilation or assembly trimming. You need to use the code generator/analyzer to generate static code and use the 'StaticDeserializerBuilder' object instead of this one.")]
	public sealed class DeserializerBuilder : BuilderSkeleton<DeserializerBuilder>
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00002C68 File Offset: 0x00000E68
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
			lazyComponentRegistrationList.Add(typeof(ScalarNodeDeserializer), (Nothing _) => new ScalarNodeDeserializer(this.attemptUnknownTypeDeserialization, this.typeConverter, this.BuildTypeInspector(), this.yamlFormatter, this.enumNamingConvention));
			lazyComponentRegistrationList.Add(typeof(ArrayNodeDeserializer), (Nothing _) => new ArrayNodeDeserializer(this.enumNamingConvention, this.BuildTypeInspector()));
			lazyComponentRegistrationList.Add(typeof(DictionaryNodeDeserializer), (Nothing _) => new DictionaryNodeDeserializer(this.objectFactory.Value, this.duplicateKeyChecking));
			lazyComponentRegistrationList.Add(typeof(CollectionNodeDeserializer), (Nothing _) => new CollectionNodeDeserializer(this.objectFactory.Value, this.enumNamingConvention, this.BuildTypeInspector()));
			lazyComponentRegistrationList.Add(typeof(EnumerableNodeDeserializer), (Nothing _) => new EnumerableNodeDeserializer());
			lazyComponentRegistrationList.Add(typeof(ObjectNodeDeserializer), (Nothing _) => new ObjectNodeDeserializer(this.objectFactory.Value, this.BuildTypeInspector(), this.ignoreUnmatched, this.duplicateKeyChecking, this.typeConverter, this.enumNamingConvention, this.enforceNullability, this.caseInsensitivePropertyMatching, this.enforceRequiredProperties, base.BuildTypeConverters()));
			lazyComponentRegistrationList.Add(typeof(FsharpListNodeDeserializer), (Nothing _) => new FsharpListNodeDeserializer(this.BuildTypeInspector(), this.enumNamingConvention));
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

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600004F RID: 79 RVA: 0x0000307F File Offset: 0x0000127F
		protected override DeserializerBuilder Self
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003084 File Offset: 0x00001284
		public ITypeInspector BuildTypeInspector()
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

		// Token: 0x06000051 RID: 81 RVA: 0x000030D5 File Offset: 0x000012D5
		public DeserializerBuilder WithAttemptingUnquotedStringTypeDeserialization()
		{
			this.attemptUnknownTypeDeserialization = true;
			return this;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000030E0 File Offset: 0x000012E0
		public DeserializerBuilder WithObjectFactory(IObjectFactory objectFactory)
		{
			if (objectFactory == null)
			{
				throw new ArgumentNullException("objectFactory");
			}
			this.objectFactory = new Lazy<IObjectFactory>(() => objectFactory, true);
			return this;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003126 File Offset: 0x00001326
		public DeserializerBuilder WithObjectFactory(Func<Type, object> objectFactory)
		{
			if (objectFactory == null)
			{
				throw new ArgumentNullException("objectFactory");
			}
			return this.WithObjectFactory(new LambdaObjectFactory(objectFactory));
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003142 File Offset: 0x00001342
		public DeserializerBuilder WithNodeDeserializer(INodeDeserializer nodeDeserializer)
		{
			return this.WithNodeDeserializer(nodeDeserializer, delegate(IRegistrationLocationSelectionSyntax<INodeDeserializer> w)
			{
				w.OnTop();
			});
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000316C File Offset: 0x0000136C
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

		// Token: 0x06000056 RID: 86 RVA: 0x000031D0 File Offset: 0x000013D0
		public DeserializerBuilder WithNodeDeserializer<[Nullable(0)] TNodeDeserializer>(WrapperFactory<INodeDeserializer, TNodeDeserializer> nodeDeserializerFactory, Action<ITrackingRegistrationLocationSelectionSyntax<INodeDeserializer>> where) where TNodeDeserializer : INodeDeserializer
		{
			DeserializerBuilder.<>c__DisplayClass21_0<TNodeDeserializer> CS$<>8__locals1 = new DeserializerBuilder.<>c__DisplayClass21_0<TNodeDeserializer>();
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

		// Token: 0x06000057 RID: 87 RVA: 0x00003233 File Offset: 0x00001433
		public DeserializerBuilder WithoutNodeDeserializer<[Nullable(0)] TNodeDeserializer>() where TNodeDeserializer : INodeDeserializer
		{
			return this.WithoutNodeDeserializer(typeof(TNodeDeserializer));
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003245 File Offset: 0x00001445
		public DeserializerBuilder WithoutNodeDeserializer(Type nodeDeserializerType)
		{
			if (nodeDeserializerType == null)
			{
				throw new ArgumentNullException("nodeDeserializerType");
			}
			this.nodeDeserializerFactories.Remove(nodeDeserializerType);
			return this;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003268 File Offset: 0x00001468
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

		// Token: 0x0600005A RID: 90 RVA: 0x000032C1 File Offset: 0x000014C1
		public DeserializerBuilder WithNodeTypeResolver(INodeTypeResolver nodeTypeResolver)
		{
			return this.WithNodeTypeResolver(nodeTypeResolver, delegate(IRegistrationLocationSelectionSyntax<INodeTypeResolver> w)
			{
				w.OnTop();
			});
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000032EC File Offset: 0x000014EC
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

		// Token: 0x0600005C RID: 92 RVA: 0x00003350 File Offset: 0x00001550
		public DeserializerBuilder WithNodeTypeResolver<[Nullable(0)] TNodeTypeResolver>(WrapperFactory<INodeTypeResolver, TNodeTypeResolver> nodeTypeResolverFactory, Action<ITrackingRegistrationLocationSelectionSyntax<INodeTypeResolver>> where) where TNodeTypeResolver : INodeTypeResolver
		{
			DeserializerBuilder.<>c__DisplayClass27_0<TNodeTypeResolver> CS$<>8__locals1 = new DeserializerBuilder.<>c__DisplayClass27_0<TNodeTypeResolver>();
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

		// Token: 0x0600005D RID: 93 RVA: 0x000033B3 File Offset: 0x000015B3
		public DeserializerBuilder WithCaseInsensitivePropertyMatching()
		{
			this.caseInsensitivePropertyMatching = true;
			return this;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000033BD File Offset: 0x000015BD
		public DeserializerBuilder WithEnforceNullability()
		{
			this.enforceNullability = true;
			return this;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000033C7 File Offset: 0x000015C7
		public DeserializerBuilder WithEnforceRequiredMembers()
		{
			this.enforceRequiredProperties = true;
			return this;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000033D1 File Offset: 0x000015D1
		public DeserializerBuilder WithoutNodeTypeResolver<[Nullable(0)] TNodeTypeResolver>() where TNodeTypeResolver : INodeTypeResolver
		{
			return this.WithoutNodeTypeResolver(typeof(TNodeTypeResolver));
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000033E3 File Offset: 0x000015E3
		public DeserializerBuilder WithoutNodeTypeResolver(Type nodeTypeResolverType)
		{
			if (nodeTypeResolverType == null)
			{
				throw new ArgumentNullException("nodeTypeResolverType");
			}
			this.nodeTypeResolverFactories.Remove(nodeTypeResolverType);
			return this;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003408 File Offset: 0x00001608
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

		// Token: 0x06000063 RID: 99 RVA: 0x000034B0 File Offset: 0x000016B0
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
			if (!this.typeMappings.TryAdd(typeFromHandle, typeFromHandle2))
			{
				this.typeMappings[typeFromHandle] = typeFromHandle2;
			}
			return this;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003550 File Offset: 0x00001750
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

		// Token: 0x06000065 RID: 101 RVA: 0x000035B7 File Offset: 0x000017B7
		public DeserializerBuilder IgnoreUnmatchedProperties()
		{
			this.ignoreUnmatched = true;
			return this;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000035C1 File Offset: 0x000017C1
		public DeserializerBuilder WithDuplicateKeyChecking()
		{
			this.duplicateKeyChecking = true;
			return this;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000035CB File Offset: 0x000017CB
		public IDeserializer Build()
		{
			if (FsharpHelper.Instance == null)
			{
				FsharpHelper.Instance = new DefaultFsharpHelper();
			}
			return Deserializer.FromValueDeserializer(this.BuildValueDeserializer());
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000035EB File Offset: 0x000017EB
		public IValueDeserializer BuildValueDeserializer()
		{
			return new AliasValueDeserializer(new NodeValueDeserializer(this.nodeDeserializerFactories.BuildComponentList<INodeDeserializer>(), this.nodeTypeResolverFactories.BuildComponentList<INodeTypeResolver>(), this.typeConverter, this.enumNamingConvention, this.BuildTypeInspector()));
		}

		// Token: 0x04000015 RID: 21
		private Lazy<IObjectFactory> objectFactory;

		// Token: 0x04000016 RID: 22
		private readonly LazyComponentRegistrationList<Nothing, INodeDeserializer> nodeDeserializerFactories;

		// Token: 0x04000017 RID: 23
		private readonly LazyComponentRegistrationList<Nothing, INodeTypeResolver> nodeTypeResolverFactories;

		// Token: 0x04000018 RID: 24
		private readonly Dictionary<TagName, Type> tagMappings;

		// Token: 0x04000019 RID: 25
		private readonly Dictionary<Type, Type> typeMappings;

		// Token: 0x0400001A RID: 26
		private readonly ITypeConverter typeConverter;

		// Token: 0x0400001B RID: 27
		private bool ignoreUnmatched;

		// Token: 0x0400001C RID: 28
		private bool duplicateKeyChecking;

		// Token: 0x0400001D RID: 29
		private bool attemptUnknownTypeDeserialization;

		// Token: 0x0400001E RID: 30
		private bool enforceNullability;

		// Token: 0x0400001F RID: 31
		private bool caseInsensitivePropertyMatching;

		// Token: 0x04000020 RID: 32
		private bool enforceRequiredProperties;
	}
}
