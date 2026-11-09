using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Helpers;
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
	// Token: 0x02000037 RID: 55
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	[RequiresDynamicCode("This builder configures the serializer to use reflection which is not compatible with ahead-of-time compilation or assembly trimming. You need to use the code generator/analyzer to generate static code and use the 'StaticSerializerBuilder' object instead of this one.")]
	public sealed class SerializerBuilder : BuilderSkeleton<SerializerBuilder>
	{
		// Token: 0x0600013B RID: 315 RVA: 0x00004008 File Offset: 0x00002208
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
					(IEventEmitter inner) => new TypeAssigningEventEmitter(inner, this.tagMappings, this.quoteNecessaryStrings, this.quoteYaml1_1Strings, this.defaultScalarStyle, this.yamlFormatter, this.enumNamingConvention, this.BuildTypeInspector())
				}
			};
			this.objectFactory = new DefaultObjectFactory();
			this.objectGraphTraversalStrategyFactory = ((ITypeInspector typeInspector, ITypeResolver typeResolver, IEnumerable<IYamlTypeConverter> typeConverters, int maximumRecursion) => new FullObjectGraphTraversalStrategy(typeInspector, typeResolver, maximumRecursion, this.namingConvention, this.objectFactory));
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00004220 File Offset: 0x00002420
		protected override SerializerBuilder Self
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00004223 File Offset: 0x00002423
		public SerializerBuilder WithQuotingNecessaryStrings(bool quoteYaml1_1Strings = false)
		{
			this.quoteNecessaryStrings = true;
			this.quoteYaml1_1Strings = quoteYaml1_1Strings;
			return this;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00004234 File Offset: 0x00002434
		public SerializerBuilder WithDefaultScalarStyle(ScalarStyle style)
		{
			this.defaultScalarStyle = style;
			return this;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00004240 File Offset: 0x00002440
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

		// Token: 0x06000140 RID: 320 RVA: 0x00004295 File Offset: 0x00002495
		public SerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(Func<IEventEmitter, TEventEmitter> eventEmitterFactory) where TEventEmitter : IEventEmitter
		{
			Action<IRegistrationLocationSelectionSyntax<IEventEmitter>> where;
			if ((where = SerializerBuilder.<>c__18<TEventEmitter>.<>9__18_0) == null)
			{
				where = (SerializerBuilder.<>c__18<TEventEmitter>.<>9__18_0 = new Action<IRegistrationLocationSelectionSyntax<IEventEmitter>>(SerializerBuilder.<>c__18<TEventEmitter>.<>9.<WithEventEmitter>b__18_0));
			}
			return this.WithEventEmitter<TEventEmitter>(eventEmitterFactory, where);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000042BD File Offset: 0x000024BD
		public SerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(Func<IEventEmitter, ITypeInspector, TEventEmitter> eventEmitterFactory) where TEventEmitter : IEventEmitter
		{
			Action<IRegistrationLocationSelectionSyntax<IEventEmitter>> where;
			if ((where = SerializerBuilder.<>c__19<TEventEmitter>.<>9__19_0) == null)
			{
				where = (SerializerBuilder.<>c__19<TEventEmitter>.<>9__19_0 = new Action<IRegistrationLocationSelectionSyntax<IEventEmitter>>(SerializerBuilder.<>c__19<TEventEmitter>.<>9.<WithEventEmitter>b__19_0));
			}
			return this.WithEventEmitter<TEventEmitter>(eventEmitterFactory, where);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000042E8 File Offset: 0x000024E8
		public SerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(Func<IEventEmitter, TEventEmitter> eventEmitterFactory, Action<IRegistrationLocationSelectionSyntax<IEventEmitter>> where) where TEventEmitter : IEventEmitter
		{
			SerializerBuilder.<>c__DisplayClass20_0<TEventEmitter> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass20_0<TEventEmitter>();
			CS$<>8__locals1.eventEmitterFactory = eventEmitterFactory;
			return this.WithEventEmitter<TEventEmitter>(new Func<IEventEmitter, ITypeInspector, TEventEmitter>(CS$<>8__locals1.<WithEventEmitter>b__0), where);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00004318 File Offset: 0x00002518
		public SerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(Func<IEventEmitter, ITypeInspector, TEventEmitter> eventEmitterFactory, Action<IRegistrationLocationSelectionSyntax<IEventEmitter>> where) where TEventEmitter : IEventEmitter
		{
			SerializerBuilder.<>c__DisplayClass21_0<TEventEmitter> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass21_0<TEventEmitter>();
			CS$<>8__locals1.eventEmitterFactory = eventEmitterFactory;
			CS$<>8__locals1.<>4__this = this;
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

		// Token: 0x06000144 RID: 324 RVA: 0x00004388 File Offset: 0x00002588
		public SerializerBuilder WithEventEmitter<[Nullable(0)] TEventEmitter>(WrapperFactory<IEventEmitter, IEventEmitter, TEventEmitter> eventEmitterFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IEventEmitter>> where) where TEventEmitter : IEventEmitter
		{
			SerializerBuilder.<>c__DisplayClass22_0<TEventEmitter> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass22_0<TEventEmitter>();
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

		// Token: 0x06000145 RID: 325 RVA: 0x000043F0 File Offset: 0x000025F0
		public SerializerBuilder WithoutEventEmitter<[Nullable(0)] TEventEmitter>() where TEventEmitter : IEventEmitter
		{
			return this.WithoutEventEmitter(typeof(TEventEmitter));
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00004402 File Offset: 0x00002602
		public SerializerBuilder WithoutEventEmitter(Type eventEmitterType)
		{
			if (eventEmitterType == null)
			{
				throw new ArgumentNullException("eventEmitterType");
			}
			this.eventEmitterFactories.Remove(eventEmitterType);
			return this;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00004428 File Offset: 0x00002628
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

		// Token: 0x06000148 RID: 328 RVA: 0x000044D0 File Offset: 0x000026D0
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

		// Token: 0x06000149 RID: 329 RVA: 0x00004510 File Offset: 0x00002710
		public SerializerBuilder EnsureRoundtrip()
		{
			this.objectGraphTraversalStrategyFactory = ((ITypeInspector typeInspector, ITypeResolver typeResolver, IEnumerable<IYamlTypeConverter> typeConverters, int maximumRecursion) => new RoundtripObjectGraphTraversalStrategy(typeConverters, typeInspector, typeResolver, maximumRecursion, this.namingConvention, this.settings, this.objectFactory));
			this.WithEventEmitter<TypeAssigningEventEmitter>((IEventEmitter inner) => new TypeAssigningEventEmitter(inner, this.tagMappings, this.quoteNecessaryStrings, this.quoteYaml1_1Strings, this.defaultScalarStyle, this.yamlFormatter, this.enumNamingConvention, this.BuildTypeInspector()), delegate(IRegistrationLocationSelectionSyntax<IEventEmitter> loc)
			{
				loc.InsteadOf<TypeAssigningEventEmitter>();
			});
			return base.WithTypeInspector<ReadableAndWritablePropertiesTypeInspector>((ITypeInspector inner) => new ReadableAndWritablePropertiesTypeInspector(inner), delegate(IRegistrationLocationSelectionSyntax<ITypeInspector> loc)
			{
				loc.OnBottom();
			});
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000045A5 File Offset: 0x000027A5
		public SerializerBuilder DisableAliases()
		{
			this.preProcessingPhaseObjectGraphVisitorFactories.Remove(typeof(AnchorAssigner));
			this.emissionPhaseObjectGraphVisitorFactories.Remove(typeof(AnchorAssigningObjectGraphVisitor));
			return this;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000045D2 File Offset: 0x000027D2
		[Obsolete("The default behavior is now to always emit default values, thefore calling this method has no effect. This behavior is now controlled by ConfigureDefaultValuesHandling.", true)]
		public SerializerBuilder EmitDefaults()
		{
			return this.ConfigureDefaultValuesHandling(DefaultValuesHandling.Preserve);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000045DB File Offset: 0x000027DB
		public SerializerBuilder ConfigureDefaultValuesHandling(DefaultValuesHandling configuration)
		{
			this.defaultValuesHandlingConfiguration = configuration;
			return this;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000045E8 File Offset: 0x000027E8
		public SerializerBuilder JsonCompatible()
		{
			this.emitterSettings = this.emitterSettings.WithMaxSimpleKeyLength(int.MaxValue).WithoutAnchorName().WithUtf16SurrogatePairs();
			return base.WithTypeConverter(new GuidConverter(true), delegate(IRegistrationLocationSelectionSyntax<IYamlTypeConverter> w)
			{
				w.InsteadOf<GuidConverter>();
			}).WithTypeConverter(new DateTime8601Converter(ScalarStyle.DoubleQuoted)).WithTypeConverter(new DateOnlyConverter(null, true, Array.Empty<string>())).WithTypeConverter(new TimeOnlyConverter(null, true, Array.Empty<string>())).WithEventEmitter<JsonEventEmitter>((IEventEmitter inner) => new JsonEventEmitter(inner, this.yamlFormatter, this.enumNamingConvention, this.BuildTypeInspector()), delegate(IRegistrationLocationSelectionSyntax<IEventEmitter> loc)
			{
				loc.InsteadOf<TypeAssigningEventEmitter>();
			});
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000469D File Offset: 0x0000289D
		public SerializerBuilder WithNewLine(string newLine)
		{
			this.emitterSettings = this.emitterSettings.WithNewLine(newLine);
			return this;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000046B2 File Offset: 0x000028B2
		public SerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(TObjectGraphVisitor objectGraphVisitor) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where;
			if ((where = SerializerBuilder.<>c__33<TObjectGraphVisitor>.<>9__33_0) == null)
			{
				where = (SerializerBuilder.<>c__33<TObjectGraphVisitor>.<>9__33_0 = new Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>>(SerializerBuilder.<>c__33<TObjectGraphVisitor>.<>9.<WithPreProcessingPhaseObjectGraphVisitor>b__33_0));
			}
			return this.WithPreProcessingPhaseObjectGraphVisitor<TObjectGraphVisitor>(objectGraphVisitor, where);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000046DA File Offset: 0x000028DA
		public SerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(Func<IEnumerable<IYamlTypeConverter>, TObjectGraphVisitor> objectGraphVisitorFactory) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where;
			if ((where = SerializerBuilder.<>c__34<TObjectGraphVisitor>.<>9__34_0) == null)
			{
				where = (SerializerBuilder.<>c__34<TObjectGraphVisitor>.<>9__34_0 = new Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>>(SerializerBuilder.<>c__34<TObjectGraphVisitor>.<>9.<WithPreProcessingPhaseObjectGraphVisitor>b__34_0));
			}
			return this.WithPreProcessingPhaseObjectGraphVisitor<TObjectGraphVisitor>(objectGraphVisitorFactory, where);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00004704 File Offset: 0x00002904
		public SerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(TObjectGraphVisitor objectGraphVisitor, Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			SerializerBuilder.<>c__DisplayClass35_0<TObjectGraphVisitor> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass35_0<TObjectGraphVisitor>();
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

		// Token: 0x06000152 RID: 338 RVA: 0x0000476C File Offset: 0x0000296C
		public SerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(Func<IEnumerable<IYamlTypeConverter>, TObjectGraphVisitor> objectGraphVisitorFactory, Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			SerializerBuilder.<>c__DisplayClass36_0<TObjectGraphVisitor> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass36_0<TObjectGraphVisitor>();
			CS$<>8__locals1.objectGraphVisitorFactory = objectGraphVisitorFactory;
			if (CS$<>8__locals1.objectGraphVisitorFactory == null)
			{
				throw new ArgumentNullException("objectGraphVisitorFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.preProcessingPhaseObjectGraphVisitorFactories.CreateRegistrationLocationSelector(typeof(TObjectGraphVisitor), new Func<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>>(CS$<>8__locals1.<WithPreProcessingPhaseObjectGraphVisitor>b__0)));
			return this;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000047D0 File Offset: 0x000029D0
		public SerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(WrapperFactory<IObjectGraphVisitor<Nothing>, TObjectGraphVisitor> objectGraphVisitorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			SerializerBuilder.<>c__DisplayClass37_0<TObjectGraphVisitor> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass37_0<TObjectGraphVisitor>();
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

		// Token: 0x06000154 RID: 340 RVA: 0x00004834 File Offset: 0x00002A34
		public SerializerBuilder WithPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(WrapperFactory<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>, TObjectGraphVisitor> objectGraphVisitorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IObjectGraphVisitor<Nothing>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
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
			where.Invoke(this.preProcessingPhaseObjectGraphVisitorFactories.CreateTrackingRegistrationLocationSelector(typeof(TObjectGraphVisitor), new Func<IObjectGraphVisitor<Nothing>, IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>>(CS$<>8__locals1.<WithPreProcessingPhaseObjectGraphVisitor>b__0)));
			return this;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00004897 File Offset: 0x00002A97
		public SerializerBuilder WithoutPreProcessingPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>() where TObjectGraphVisitor : IObjectGraphVisitor<Nothing>
		{
			return this.WithoutPreProcessingPhaseObjectGraphVisitor(typeof(TObjectGraphVisitor));
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000048A9 File Offset: 0x00002AA9
		public SerializerBuilder WithoutPreProcessingPhaseObjectGraphVisitor(Type objectGraphVisitorType)
		{
			if (objectGraphVisitorType == null)
			{
				throw new ArgumentNullException("objectGraphVisitorType");
			}
			this.preProcessingPhaseObjectGraphVisitorFactories.Remove(objectGraphVisitorType);
			return this;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000048CC File Offset: 0x00002ACC
		public SerializerBuilder WithObjectGraphTraversalStrategyFactory(ObjectGraphTraversalStrategyFactory objectGraphTraversalStrategyFactory)
		{
			this.objectGraphTraversalStrategyFactory = objectGraphTraversalStrategyFactory;
			return this;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000048D6 File Offset: 0x00002AD6
		public SerializerBuilder WithEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(Func<EmissionPhaseObjectGraphVisitorArgs, TObjectGraphVisitor> objectGraphVisitorFactory) where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>> where;
			if ((where = SerializerBuilder.<>c__42<TObjectGraphVisitor>.<>9__42_0) == null)
			{
				where = (SerializerBuilder.<>c__42<TObjectGraphVisitor>.<>9__42_0 = new Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>>(SerializerBuilder.<>c__42<TObjectGraphVisitor>.<>9.<WithEmissionPhaseObjectGraphVisitor>b__42_0));
			}
			return this.WithEmissionPhaseObjectGraphVisitor<TObjectGraphVisitor>(objectGraphVisitorFactory, where);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00004900 File Offset: 0x00002B00
		public SerializerBuilder WithEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(Func<EmissionPhaseObjectGraphVisitorArgs, TObjectGraphVisitor> objectGraphVisitorFactory, Action<IRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			SerializerBuilder.<>c__DisplayClass43_0<TObjectGraphVisitor> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass43_0<TObjectGraphVisitor>();
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

		// Token: 0x0600015A RID: 346 RVA: 0x00004964 File Offset: 0x00002B64
		public SerializerBuilder WithEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>(WrapperFactory<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>, TObjectGraphVisitor> objectGraphVisitorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IObjectGraphVisitor<IEmitter>>> where) where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			SerializerBuilder.<>c__DisplayClass44_0<TObjectGraphVisitor> CS$<>8__locals1 = new SerializerBuilder.<>c__DisplayClass44_0<TObjectGraphVisitor>();
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

		// Token: 0x0600015B RID: 347 RVA: 0x000049C7 File Offset: 0x00002BC7
		public SerializerBuilder WithoutEmissionPhaseObjectGraphVisitor<[Nullable(0)] TObjectGraphVisitor>() where TObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
		{
			return this.WithoutEmissionPhaseObjectGraphVisitor(typeof(TObjectGraphVisitor));
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000049D9 File Offset: 0x00002BD9
		public SerializerBuilder WithoutEmissionPhaseObjectGraphVisitor(Type objectGraphVisitorType)
		{
			if (objectGraphVisitorType == null)
			{
				throw new ArgumentNullException("objectGraphVisitorType");
			}
			this.emissionPhaseObjectGraphVisitorFactories.Remove(objectGraphVisitorType);
			return this;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000049FC File Offset: 0x00002BFC
		public SerializerBuilder WithIndentedSequences()
		{
			this.emitterSettings = this.emitterSettings.WithIndentedSequences();
			return this;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00004A10 File Offset: 0x00002C10
		public ISerializer Build()
		{
			if (FsharpHelper.Instance == null)
			{
				FsharpHelper.Instance = new DefaultFsharpHelper();
			}
			return Serializer.FromValueSerializer(this.BuildValueSerializer(), this.emitterSettings);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00004A38 File Offset: 0x00002C38
		public IValueSerializer BuildValueSerializer()
		{
			IEnumerable<IYamlTypeConverter> typeConverters = base.BuildTypeConverters();
			ITypeInspector typeInspector = this.BuildTypeInspector();
			IObjectGraphTraversalStrategy traversalStrategy = this.objectGraphTraversalStrategyFactory(typeInspector, this.typeResolver, typeConverters, this.maximumRecursion);
			IEventEmitter eventEmitter = this.eventEmitterFactories.BuildComponentChain(new WriterEventEmitter());
			return new SerializerBuilder.ValueSerializer(traversalStrategy, eventEmitter, typeConverters, this.preProcessingPhaseObjectGraphVisitorFactories.Clone(), this.emissionPhaseObjectGraphVisitorFactories.Clone());
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00004A9C File Offset: 0x00002C9C
		public ITypeInspector BuildTypeInspector()
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

		// Token: 0x0400003D RID: 61
		private ObjectGraphTraversalStrategyFactory objectGraphTraversalStrategyFactory;

		// Token: 0x0400003E RID: 62
		private readonly LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories;

		// Token: 0x0400003F RID: 63
		private readonly LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories;

		// Token: 0x04000040 RID: 64
		private readonly LazyComponentRegistrationList<IEventEmitter, IEventEmitter> eventEmitterFactories;

		// Token: 0x04000041 RID: 65
		private readonly Dictionary<Type, TagName> tagMappings = new Dictionary<Type, TagName>();

		// Token: 0x04000042 RID: 66
		private readonly IObjectFactory objectFactory;

		// Token: 0x04000043 RID: 67
		private int maximumRecursion = 50;

		// Token: 0x04000044 RID: 68
		private EmitterSettings emitterSettings = EmitterSettings.Default;

		// Token: 0x04000045 RID: 69
		private DefaultValuesHandling defaultValuesHandlingConfiguration;

		// Token: 0x04000046 RID: 70
		private ScalarStyle defaultScalarStyle;

		// Token: 0x04000047 RID: 71
		private bool quoteNecessaryStrings;

		// Token: 0x04000048 RID: 72
		private bool quoteYaml1_1Strings;

		// Token: 0x0200012F RID: 303
		[Nullable(0)]
		private class ValueSerializer : IValueSerializer
		{
			// Token: 0x06000711 RID: 1809 RVA: 0x0001839A File Offset: 0x0001659A
			public ValueSerializer(IObjectGraphTraversalStrategy traversalStrategy, IEventEmitter eventEmitter, IEnumerable<IYamlTypeConverter> typeConverters, LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories, LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories)
			{
				this.traversalStrategy = traversalStrategy;
				this.eventEmitter = eventEmitter;
				this.typeConverters = typeConverters;
				this.preProcessingPhaseObjectGraphVisitorFactories = preProcessingPhaseObjectGraphVisitorFactories;
				this.emissionPhaseObjectGraphVisitorFactories = emissionPhaseObjectGraphVisitorFactories;
			}

			// Token: 0x06000712 RID: 1810 RVA: 0x000183C8 File Offset: 0x000165C8
			[NullableContext(2)]
			public void SerializeValue([Nullable(1)] IEmitter emitter, object value, Type type)
			{
				SerializerBuilder.ValueSerializer.<>c__DisplayClass6_0 CS$<>8__locals1 = new SerializerBuilder.ValueSerializer.<>c__DisplayClass6_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.emitter = emitter;
				Type type2 = type ?? ((value != null) ? value.GetType() : typeof(object));
				Type staticType = type ?? typeof(object);
				ObjectDescriptor graph = new ObjectDescriptor(value, type2, staticType);
				CS$<>8__locals1.preProcessingPhaseObjectGraphVisitors = this.preProcessingPhaseObjectGraphVisitorFactories.BuildComponentList(this.typeConverters);
				IObjectGraphVisitor<IEmitter> visitor = this.emissionPhaseObjectGraphVisitorFactories.BuildComponentChain(new EmittingObjectGraphVisitor(this.eventEmitter), (IObjectGraphVisitor<IEmitter> inner) => new EmissionPhaseObjectGraphVisitorArgs(inner, CS$<>8__locals1.<>4__this.eventEmitter, CS$<>8__locals1.preProcessingPhaseObjectGraphVisitors, CS$<>8__locals1.<>4__this.typeConverters, new ObjectSerializer(base.<SerializeValue>g__NestedObjectSerializer|0)));
				foreach (IObjectGraphVisitor<Nothing> visitor2 in CS$<>8__locals1.preProcessingPhaseObjectGraphVisitors)
				{
					this.traversalStrategy.Traverse<Nothing>(graph, visitor2, default(Nothing), new ObjectSerializer(CS$<>8__locals1.<SerializeValue>g__NestedObjectSerializer|0));
				}
				this.traversalStrategy.Traverse<IEmitter>(graph, visitor, CS$<>8__locals1.emitter, new ObjectSerializer(CS$<>8__locals1.<SerializeValue>g__NestedObjectSerializer|0));
			}

			// Token: 0x04000245 RID: 581
			private readonly IObjectGraphTraversalStrategy traversalStrategy;

			// Token: 0x04000246 RID: 582
			private readonly IEventEmitter eventEmitter;

			// Token: 0x04000247 RID: 583
			private readonly IEnumerable<IYamlTypeConverter> typeConverters;

			// Token: 0x04000248 RID: 584
			private readonly LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories;

			// Token: 0x04000249 RID: 585
			private readonly LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories;
		}
	}
}
