using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.EventEmitters;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.ObjectFactories;
using YamlDotNet.Serialization.ObjectGraphTraversalStrategies;
using YamlDotNet.Serialization.ObjectGraphVisitors;
using YamlDotNet.Serialization.TypeInspectors;
using YamlDotNet.Serialization.TypeResolvers;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000C0 RID: 192
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	[RequiresDynamicCode("This builder configures the serializer to use reflection which is not compatible with ahead-of-time compilation or assembly trimming. You need to use the code generator/analyzer to generate static code and use the 'StaticSerializerBuilder' object instead of this one.")]
	public sealed class SerializerBuilder : BuilderSkeleton<SerializerBuilder>
	{
		// Token: 0x06000467 RID: 1127 RVA: 0x00011720 File Offset: 0x0000F920
		public SerializerBuilder() : base(new DynamicTypeResolver())
		{
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
			LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> lazyComponentRegistrationList = new LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>>();
			lazyComponentRegistrationList.Add(typeof(AnchorAssigner), (IEnumerable<IYamlTypeConverter> typeConverters) => new AnchorAssigner(typeConverters));
			this.preProcessingPhaseObjectGraphVisitorFactories = lazyComponentRegistrationList;
			LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> lazyComponentRegistrationList2 = new LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>>();
			lazyComponentRegistrationList2.Add(typeof(CustomSerializationObjectGraphVisitor), (EmissionPhaseObjectGraphVisitorArgs args) => new CustomSerializationObjectGraphVisitor(args.InnerVisitor, args.TypeConverters, args.NestedObjectSerializer));
			lazyComponentRegistrationList2.Add(typeof(AnchorAssigningObjectGraphVisitor), (EmissionPhaseObjectGraphVisitorArgs args) => new AnchorAssigningObjectGraphVisitor(args.InnerVisitor, args.EventEmitter, args.GetPreProcessingPhaseObjectGraphVisitor<AnchorAssigner>()));
			lazyComponentRegistrationList2.Add(typeof(DefaultValuesObjectGraphVisitor), (EmissionPhaseObjectGraphVisitorArgs args) => new DefaultValuesObjectGraphVisitor(this.defaultValuesHandlingConfiguration, args.InnerVisitor, new DefaultObjectFactory()));
			lazyComponentRegistrationList2.Add(typeof(CommentsObjectGraphVisitor), (EmissionPhaseObjectGraphVisitorArgs args) => new CommentsObjectGraphVisitor(args.InnerVisitor));
			this.emissionPhaseObjectGraphVisitorFactories = lazyComponentRegistrationList2;
			this.eventEmitterFactories = new LazyComponentRegistrationList<IEventEmitter, IEventEmitter>
			{
				{
					typeof(TypeAssigningEventEmitter),
					(IEventEmitter inner) => new TypeAssigningEventEmitter(inner, false, this.tagMappings, this.quoteNecessaryStrings, this.quoteYaml1_1Strings, this.defaultScalarStyle, this.yamlFormatter)
				}
			};
			this.objectFactory = new DefaultObjectFactory();
			this.objectGraphTraversalStrategyFactory = ((ITypeInspector typeInspector, ITypeResolver typeResolver, IEnumerable<IYamlTypeConverter> typeConverters, int maximumRecursion) => new FullObjectGraphTraversalStrategy(typeInspector, typeResolver, maximumRecursion, this.namingConvention, this.objectFactory));
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0000E3B0 File Offset: 0x0000C5B0
		protected override SerializerBuilder Self
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00011932 File Offset: 0x0000FB32
		public SerializerBuilder WithQuotingNecessaryStrings(bool quoteYaml1_1Strings = false)
		{
			this.quoteNecessaryStrings = true;
			this.quoteYaml1_1Strings = quoteYaml1_1Strings;
			return this;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00011943 File Offset: 0x0000FB43
		public SerializerBuilder WithDefaultScalarStyle(ScalarStyle style)
		{
			this.defaultScalarStyle = style;
			return this;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00011950 File Offset: 0x0000FB50
		public SerializerBuilder WithMaximumRecursion(int maximumRecursion)
		{
			if (maximumRecursion <= 0)
			{
				string text = "maximumRecursion";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(79, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The maximum recursion specified (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(maximumRecursion);
				defaultInterpolatedStringHandler.AppendLiteral(") is invalid. It should be a positive integer.");
				throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.maximumRecursion = maximumRecursion;
			return this;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000119A5 File Offset: 0x0000FBA5
		public SerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(Func<IEventEmitter, TEventEmitter> eventEmitterFactory) where TEventEmitter : IEventEmitter
		{
			Action<IRegistrationLocationSelectionSyntax<IEventEmitter>> where;
			if ((where = SerializerBuilder.<>c__18<TEventEmitter>.<>9__18_0) == null)
			{
				where = (SerializerBuilder.<>c__18<TEventEmitter>.<>9__18_0 = new Action<IRegistrationLocationSelectionSyntax<IEventEmitter>>(SerializerBuilder.<>c__18<TEventEmitter>.<>9.<WithEventEmitter>b__18_0));
			}
			return this.WithEventEmitter<TEventEmitter>(eventEmitterFactory, where);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000119D0 File Offset: 0x0000FBD0
		public SerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(Func<IEventEmitter, TEventEmitter> eventEmitterFactory, Action<IRegistrationLocationSelectionSyntax<IEventEmitter>> where) where TEventEmitter : IEventEmitter
		{
			SerializerBuilder.<>c__DisplayClass19_0<TEventEmitter> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass19_0<TEventEmitter>();
			CS$<>8__locals1.eventEmitterFactory = eventEmitterFactory;
			if (CS$<>8__locals1.eventEmitterFactory == null)
			{
				throw new ArgumentNullException("eventEmitterFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.eventEmitterFactories.CreateRegistrationLocationSelector(typeof(TEventEmitter), new Func<IEventEmitter, IEventEmitter>(CS$<>8__locals1.<WithEventEmitter>b__0)));
			return this.Self;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00011A38 File Offset: 0x0000FC38
		public SerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(WrapperFactory<IEventEmitter, IEventEmitter, TEventEmitter> eventEmitterFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IEventEmitter>> where) where TEventEmitter : IEventEmitter
		{
			SerializerBuilder.<>c__DisplayClass20_0<TEventEmitter> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass20_0<TEventEmitter>();
			CS$<>8__locals1.eventEmitterFactory = eventEmitterFactory;
			if (CS$<>8__locals1.eventEmitterFactory == null)
			{
				throw new ArgumentNullException("eventEmitterFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.eventEmitterFactories.CreateTrackingRegistrationLocationSelector(typeof(TEventEmitter), new Func<IEventEmitter, IEventEmitter, IEventEmitter>(CS$<>8__locals1.<WithEventEmitter>b__0)));
			return this.Self;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00011AA0 File Offset: 0x0000FCA0
		public SerializerBuilder WithoutEventEmitter<[Nullable(0)] TEventEmitter>() where TEventEmitter : IEventEmitter
		{
			return this.WithoutEventEmitter(typeof(TEventEmitter));
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00011AB2 File Offset: 0x0000FCB2
		public SerializerBuilder WithoutEventEmitter(Type eventEmitterType)
		{
			if (eventEmitterType == null)
			{
				throw new ArgumentNullException("eventEmitterType");
			}
			this.eventEmitterFactories.Remove(eventEmitterType);
			return this;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00011AD8 File Offset: 0x0000FCD8
		public override SerializerBuilder WithTagMapping(TagName tag, Type type)
		{
			if (tag.IsEmpty)
			{
				throw new ArgumentException("Non-specific tags cannot be maped");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			TagName tagName;
			if (this.tagMappings.TryGetValue(type, ref tagName))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(48, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Type already has a registered tag '");
				defaultInterpolatedStringHandler.AppendFormatted<TagName>(tagName);
				defaultInterpolatedStringHandler.AppendLiteral("' for type '");
				defaultInterpolatedStringHandler.AppendFormatted(type.FullName);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), "type");
			}
			this.tagMappings.Add(type, tag);
			return this;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00011B80 File Offset: 0x0000FD80
		public SerializerBuilder WithoutTagMapping(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!this.tagMappings.Remove(type))
			{
				throw new KeyNotFoundException("Tag for type '" + type.FullName + "' is not registered");
			}
			return this;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00011BC0 File Offset: 0x0000FDC0
		public SerializerBuilder EnsureRoundtrip()
		{
			this.objectGraphTraversalStrategyFactory = ((ITypeInspector typeInspector, ITypeResolver typeResolver, IEnumerable<IYamlTypeConverter> typeConverters, int maximumRecursion) => new RoundtripObjectGraphTraversalStrategy(typeConverters, typeInspector, typeResolver, maximumRecursion, this.namingConvention, this.settings, this.objectFactory));
			this.WithEventEmitter<TypeAssigningEventEmitter>(([Nullable(1)] IEventEmitter inner) => new TypeAssigningEventEmitter(inner, true, this.tagMappings, this.quoteNecessaryStrings, this.quoteYaml1_1Strings, this.defaultScalarStyle, this.yamlFormatter), delegate(IRegistrationLocationSelectionSyntax<IEventEmitter> loc)
			{
				loc.InsteadOf<TypeAssigningEventEmitter>();
			});
			return base.WithTypeInspector<ReadableAndWritablePropertiesTypeInspector>(([Nullable(1)] ITypeInspector inner) => new ReadableAndWritablePropertiesTypeInspector(inner), delegate(IRegistrationLocationSelectionSyntax<ITypeInspector> loc)
			{
				loc.OnBottom();
			});
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00011C55 File Offset: 0x0000FE55
		public SerializerBuilder DisableAliases()
		{
			this.preProcessingPhaseObjectGraphVisitorFactories.Remove(typeof(AnchorAssigner));
			this.emissionPhaseObjectGraphVisitorFactories.Remove(typeof(AnchorAssigningObjectGraphVisitor));
			return this;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00011C82 File Offset: 0x0000FE82
		[Obsolete("The default behavior is now to always emit default values, thefore calling this method has no effect. This behavior is now controlled by ConfigureDefaultValuesHandling.", true)]
		public SerializerBuilder EmitDefaults()
		{
			return this.ConfigureDefaultValuesHandling(DefaultValuesHandling.Preserve);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00011C8B File Offset: 0x0000FE8B
		public SerializerBuilder ConfigureDefaultValuesHandling(DefaultValuesHandling configuration)
		{
			this.defaultValuesHandlingConfiguration = configuration;
			return this;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00011C98 File Offset: 0x0000FE98
		public SerializerBuilder JsonCompatible()
		{
			this.emitterSettings = this.emitterSettings.WithMaxSimpleKeyLength(int.MaxValue).WithoutAnchorName();
			return base.WithTypeConverter(new GuidConverter(true), delegate(IRegistrationLocationSelectionSyntax<IYamlTypeConverter> w)
			{
				w.InsteadOf<GuidConverter>();
			}).WithTypeConverter(new DateTimeConverter(1, null, true, Array.Empty<string>())).WithTypeConverter(new DateOnlyConverter(null, true, Array.Empty<string>())).WithTypeConverter(new TimeOnlyConverter(null, true, Array.Empty<string>())).WithEventEmitter<JsonEventEmitter>(([Nullable(1)] IEventEmitter inner) => new JsonEventEmitter(inner, this.yamlFormatter), delegate(IRegistrationLocationSelectionSyntax<IEventEmitter> loc)
			{
				loc.InsteadOf<TypeAssigningEventEmitter>();
			});
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00011D4F File Offset: 0x0000FF4F
		public SerializerBuilder WithNewLine(string newLine)
		{
			this.emitterSettings = this.emitterSettings.WithNewLine(newLine);
			return this;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00011D64 File Offset: 0x0000FF64
		public SerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(TObjectGraphVisitor objectGraphVisitor) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where;
			if ((where = SerializerBuilder.<>c__31<TObjectGraphVisitor>.<>9__31_0) == null)
			{
				where = (SerializerBuilder.<>c__31<TObjectGraphVisitor>.<>9__31_0 = new Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>>(SerializerBuilder.<>c__31<TObjectGraphVisitor>.<>9.<WithPreProcessingPhaseObjectGraphVisitor>b__31_0));
			}
			return this.WithPreProcessingPhaseObjectGraphVisitor<TObjectGraphVisitor>(objectGraphVisitor, where);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00011D8C File Offset: 0x0000FF8C
		public SerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(TObjectGraphVisitor objectGraphVisitor, Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			SerializerBuilder.<>c__DisplayClass32_0<TObjectGraphVisitor> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass32_0<TObjectGraphVisitor>();
			CS$<>8__locals1.objectGraphVisitor = objectGraphVisitor;
			if (CS$<>8__locals1.objectGraphVisitor == null)
			{
				throw new ArgumentNullException("objectGraphVisitor");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.preProcessingPhaseObjectGraphVisitorFactories.CreateRegistrationLocationSelector(typeof(TObjectGraphVisitor), new Func<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>>(CS$<>8__locals1.<WithPreProcessingPhaseObjectGraphVisitor>b__0)));
			return this;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00011DF4 File Offset: 0x0000FFF4
		public SerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(WrapperFactory<IObjectGraphVisitor<Nothing>, TObjectGraphVisitor> objectGraphVisitorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			SerializerBuilder.<>c__DisplayClass33_0<TObjectGraphVisitor> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass33_0<TObjectGraphVisitor>();
			CS$<>8__locals1.objectGraphVisitorFactory = objectGraphVisitorFactory;
			if (CS$<>8__locals1.objectGraphVisitorFactory == null)
			{
				throw new ArgumentNullException("objectGraphVisitorFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.preProcessingPhaseObjectGraphVisitorFactories.CreateTrackingRegistrationLocationSelector(typeof(TObjectGraphVisitor), new Func<IObjectGraphVisitor<Nothing>, IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>>(CS$<>8__locals1.<WithPreProcessingPhaseObjectGraphVisitor>b__0)));
			return this;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00011E57 File Offset: 0x00010057
		public SerializerBuilder WithoutPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>() where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			return this.WithoutPreProcessingPhaseObjectGraphVisitor(typeof(TObjectGraphVisitor));
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00011E69 File Offset: 0x00010069
		public SerializerBuilder WithoutPreProcessingPhaseObjectGraphVisitor(Type objectGraphVisitorType)
		{
			if (objectGraphVisitorType == null)
			{
				throw new ArgumentNullException("objectGraphVisitorType");
			}
			this.preProcessingPhaseObjectGraphVisitorFactories.Remove(objectGraphVisitorType);
			return this;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00011E8C File Offset: 0x0001008C
		public SerializerBuilder WithObjectGraphTraversalStrategyFactory(ObjectGraphTraversalStrategyFactory objectGraphTraversalStrategyFactory)
		{
			this.objectGraphTraversalStrategyFactory = objectGraphTraversalStrategyFactory;
			return this;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00011E96 File Offset: 0x00010096
		public SerializerBuilder WithEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(Func<EmissionPhaseObjectGraphVisitorArgs, TObjectGraphVisitor> objectGraphVisitorFactory) where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>> where;
			if ((where = SerializerBuilder.<>c__37<TObjectGraphVisitor>.<>9__37_0) == null)
			{
				where = (SerializerBuilder.<>c__37<TObjectGraphVisitor>.<>9__37_0 = new Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>>(SerializerBuilder.<>c__37<TObjectGraphVisitor>.<>9.<WithEmissionPhaseObjectGraphVisitor>b__37_0));
			}
			return this.WithEmissionPhaseObjectGraphVisitor<TObjectGraphVisitor>(objectGraphVisitorFactory, where);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00011EC0 File Offset: 0x000100C0
		public SerializerBuilder WithEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(Func<EmissionPhaseObjectGraphVisitorArgs, TObjectGraphVisitor> objectGraphVisitorFactory, Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			SerializerBuilder.<>c__DisplayClass38_0<TObjectGraphVisitor> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass38_0<TObjectGraphVisitor>();
			CS$<>8__locals1.objectGraphVisitorFactory = objectGraphVisitorFactory;
			if (CS$<>8__locals1.objectGraphVisitorFactory == null)
			{
				throw new ArgumentNullException("objectGraphVisitorFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.emissionPhaseObjectGraphVisitorFactories.CreateRegistrationLocationSelector(typeof(TObjectGraphVisitor), new Func<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>>(CS$<>8__locals1.<WithEmissionPhaseObjectGraphVisitor>b__0)));
			return this;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00011F24 File Offset: 0x00010124
		public SerializerBuilder WithEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(WrapperFactory<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>, TObjectGraphVisitor> objectGraphVisitorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			SerializerBuilder.<>c__DisplayClass39_0<TObjectGraphVisitor> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass39_0<TObjectGraphVisitor>();
			CS$<>8__locals1.objectGraphVisitorFactory = objectGraphVisitorFactory;
			if (CS$<>8__locals1.objectGraphVisitorFactory == null)
			{
				throw new ArgumentNullException("objectGraphVisitorFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.emissionPhaseObjectGraphVisitorFactories.CreateTrackingRegistrationLocationSelector(typeof(TObjectGraphVisitor), new Func<IObjectGraphVisitor<IEmitter>, EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>>(CS$<>8__locals1.<WithEmissionPhaseObjectGraphVisitor>b__0)));
			return this;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00011F87 File Offset: 0x00010187
		public SerializerBuilder WithoutEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>() where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			return this.WithoutEmissionPhaseObjectGraphVisitor(typeof(TObjectGraphVisitor));
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00011F99 File Offset: 0x00010199
		public SerializerBuilder WithoutEmissionPhaseObjectGraphVisitor(Type objectGraphVisitorType)
		{
			if (objectGraphVisitorType == null)
			{
				throw new ArgumentNullException("objectGraphVisitorType");
			}
			this.emissionPhaseObjectGraphVisitorFactories.Remove(objectGraphVisitorType);
			return this;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00011FBC File Offset: 0x000101BC
		public SerializerBuilder WithIndentedSequences()
		{
			this.emitterSettings = this.emitterSettings.WithIndentedSequences();
			return this;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00011FD0 File Offset: 0x000101D0
		public ISerializer Build()
		{
			return Serializer.FromValueSerializer(this.BuildValueSerializer(), this.emitterSettings);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00011FE4 File Offset: 0x000101E4
		public IValueSerializer BuildValueSerializer()
		{
			IEnumerable<IYamlTypeConverter> typeConverters = base.BuildTypeConverters();
			ITypeInspector typeInspector = this.BuildTypeInspector();
			IObjectGraphTraversalStrategy traversalStrategy = this.objectGraphTraversalStrategyFactory(typeInspector, this.typeResolver, typeConverters, this.maximumRecursion);
			IEventEmitter eventEmitter = this.eventEmitterFactories.BuildComponentChain(new WriterEventEmitter());
			return new SerializerBuilder.ValueSerializer(traversalStrategy, eventEmitter, typeConverters, this.preProcessingPhaseObjectGraphVisitorFactories.Clone(), this.emissionPhaseObjectGraphVisitorFactories.Clone());
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00012048 File Offset: 0x00010248
		internal ITypeInspector BuildTypeInspector()
		{
			ITypeInspector typeInspector = new ReadablePropertiesTypeInspector(this.typeResolver, this.includeNonPublicProperties);
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

		// Token: 0x040001F5 RID: 501
		private ObjectGraphTraversalStrategyFactory objectGraphTraversalStrategyFactory;

		// Token: 0x040001F6 RID: 502
		private readonly LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories;

		// Token: 0x040001F7 RID: 503
		private readonly LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories;

		// Token: 0x040001F8 RID: 504
		private readonly LazyComponentRegistrationList<IEventEmitter, IEventEmitter> eventEmitterFactories;

		// Token: 0x040001F9 RID: 505
		private readonly IDictionary<Type, TagName> tagMappings = new Dictionary<Type, TagName>();

		// Token: 0x040001FA RID: 506
		private readonly IObjectFactory objectFactory;

		// Token: 0x040001FB RID: 507
		private int maximumRecursion = 50;

		// Token: 0x040001FC RID: 508
		private EmitterSettings emitterSettings = EmitterSettings.Default;

		// Token: 0x040001FD RID: 509
		private DefaultValuesHandling defaultValuesHandlingConfiguration;

		// Token: 0x040001FE RID: 510
		private ScalarStyle defaultScalarStyle;

		// Token: 0x040001FF RID: 511
		private bool quoteNecessaryStrings;

		// Token: 0x04000200 RID: 512
		private bool quoteYaml1_1Strings;

		// Token: 0x020000C1 RID: 193
		[Nullable(0)]
		private class ValueSerializer : IValueSerializer
		{
			// Token: 0x06000490 RID: 1168 RVA: 0x0001219B File Offset: 0x0001039B
			public ValueSerializer(IObjectGraphTraversalStrategy traversalStrategy, IEventEmitter eventEmitter, IEnumerable<IYamlTypeConverter> typeConverters, LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories, LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories)
			{
				this.traversalStrategy = traversalStrategy;
				this.eventEmitter = eventEmitter;
				this.typeConverters = typeConverters;
				this.preProcessingPhaseObjectGraphVisitorFactories = preProcessingPhaseObjectGraphVisitorFactories;
				this.emissionPhaseObjectGraphVisitorFactories = emissionPhaseObjectGraphVisitorFactories;
			}

			// Token: 0x06000491 RID: 1169 RVA: 0x000121C8 File Offset: 0x000103C8
			[NullableContext(2)]
			public void SerializeValue([Nullable(1)] IEmitter emitter, object value, Type type)
			{
				Type type2 = type ?? ((value != null) ? value.GetType() : typeof(object));
				Type staticType = type ?? typeof(object);
				ObjectDescriptor graph = new ObjectDescriptor(value, type2, staticType);
				List<IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitors = this.preProcessingPhaseObjectGraphVisitorFactories.BuildComponentList(this.typeConverters);
				foreach (IObjectGraphVisitor<Nothing> visitor in preProcessingPhaseObjectGraphVisitors)
				{
					this.traversalStrategy.Traverse<Nothing>(graph, visitor, default(Nothing));
				}
				IObjectGraphVisitor<IEmitter> visitor2 = this.emissionPhaseObjectGraphVisitorFactories.BuildComponentChain(new EmittingObjectGraphVisitor(this.eventEmitter), (IObjectGraphVisitor<IEmitter> inner) => new EmissionPhaseObjectGraphVisitorArgs(inner, this.eventEmitter, preProcessingPhaseObjectGraphVisitors, this.typeConverters, new ObjectSerializer(base.<SerializeValue>g__NestedObjectSerializer|0)));
				this.traversalStrategy.Traverse<IEmitter>(graph, visitor2, emitter);
			}

			// Token: 0x04000201 RID: 513
			private readonly IObjectGraphTraversalStrategy traversalStrategy;

			// Token: 0x04000202 RID: 514
			private readonly IEventEmitter eventEmitter;

			// Token: 0x04000203 RID: 515
			private readonly IEnumerable<IYamlTypeConverter> typeConverters;

			// Token: 0x04000204 RID: 516
			private readonly LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories;

			// Token: 0x04000205 RID: 517
			private readonly LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories;
		}
	}
}
