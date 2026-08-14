using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000CE RID: 206
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class StaticBuilderSkeleton<[Nullable(0)] TBuilder> where TBuilder : StaticBuilderSkeleton<TBuilder>
	{
		// Token: 0x060004BA RID: 1210 RVA: 0x00012434 File Offset: 0x00010634
		internal StaticBuilderSkeleton(ITypeResolver typeResolver)
		{
			this.namingConvention = NullNamingConvention.Instance;
			this.yamlFormatter = YamlFormatter.Default;
			base..ctor();
			LazyComponentRegistrationList<Nothing, IYamlTypeConverter> lazyComponentRegistrationList = new LazyComponentRegistrationList<Nothing, IYamlTypeConverter>();
			Type typeFromHandle = typeof(GuidConverter);
			Func<Nothing, IYamlTypeConverter> func;
			if ((func = StaticBuilderSkeleton<TBuilder>.<>c.<>9__7_0) == null)
			{
				func = (StaticBuilderSkeleton<TBuilder>.<>c.<>9__7_0 = new Func<Nothing, IYamlTypeConverter>(StaticBuilderSkeleton<TBuilder>.<>c.<>9.<.ctor>b__7_0));
			}
			lazyComponentRegistrationList.Add(typeFromHandle, func);
			this.typeConverterFactories = lazyComponentRegistrationList;
			this.typeInspectorFactories = new LazyComponentRegistrationList<ITypeInspector, ITypeInspector>();
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			this.settings = new Settings();
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060004BB RID: 1211
		protected abstract TBuilder Self { get; }

		// Token: 0x060004BC RID: 1212 RVA: 0x000124C3 File Offset: 0x000106C3
		public TBuilder WithNamingConvention(INamingConvention namingConvention)
		{
			if (namingConvention == null)
			{
				throw new ArgumentNullException("namingConvention");
			}
			this.namingConvention = namingConvention;
			return this.Self;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x000124E1 File Offset: 0x000106E1
		public TBuilder WithTypeResolver(ITypeResolver typeResolver)
		{
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			return this.Self;
		}

		// Token: 0x060004BE RID: 1214
		public abstract TBuilder WithTagMapping(TagName tag, Type type);

		// Token: 0x060004BF RID: 1215 RVA: 0x000124FF File Offset: 0x000106FF
		public TBuilder WithTypeConverter(IYamlTypeConverter typeConverter)
		{
			Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>> action;
			if ((action = StaticBuilderSkeleton<TBuilder>.<>c.<>9__13_0) == null)
			{
				action = (StaticBuilderSkeleton<TBuilder>.<>c.<>9__13_0 = new Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>>(StaticBuilderSkeleton<TBuilder>.<>c.<>9.<WithTypeConverter>b__13_0));
			}
			return this.WithTypeConverter(typeConverter, action);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00012528 File Offset: 0x00010728
		public TBuilder WithTypeConverter(IYamlTypeConverter typeConverter, Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>> where)
		{
			StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass14_0 CS$<>8__locals1 = new StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass14_0();
			CS$<>8__locals1.typeConverter = typeConverter;
			if (CS$<>8__locals1.typeConverter == null)
			{
				throw new ArgumentNullException("typeConverter");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.typeConverterFactories.CreateRegistrationLocationSelector(CS$<>8__locals1.typeConverter.GetType(), new Func<Nothing, IYamlTypeConverter>(CS$<>8__locals1.<WithTypeConverter>b__0)));
			return this.Self;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00012594 File Offset: 0x00010794
		public TBuilder WithTypeConverter<[Nullable(0)] TYamlTypeConverter>(WrapperFactory<IYamlTypeConverter, IYamlTypeConverter> typeConverterFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IYamlTypeConverter>> where) where TYamlTypeConverter : IYamlTypeConverter
		{
			StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass15_0<TYamlTypeConverter> CS$<>8__locals1 = new StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass15_0<TYamlTypeConverter>();
			CS$<>8__locals1.typeConverterFactory = typeConverterFactory;
			if (CS$<>8__locals1.typeConverterFactory == null)
			{
				throw new ArgumentNullException("typeConverterFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.typeConverterFactories.CreateTrackingRegistrationLocationSelector(typeof(TYamlTypeConverter), new Func<IYamlTypeConverter, Nothing, IYamlTypeConverter>(CS$<>8__locals1.<WithTypeConverter>b__0)));
			return this.Self;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x000125FC File Offset: 0x000107FC
		public TBuilder WithoutTypeConverter<[Nullable(0)] TYamlTypeConverter>() where TYamlTypeConverter : IYamlTypeConverter
		{
			return this.WithoutTypeConverter(typeof(TYamlTypeConverter));
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001260E File Offset: 0x0001080E
		public TBuilder WithoutTypeConverter(Type converterType)
		{
			if (converterType == null)
			{
				throw new ArgumentNullException("converterType");
			}
			this.typeConverterFactories.Remove(converterType);
			return this.Self;
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00012636 File Offset: 0x00010836
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(Func<ITypeInspector, TTypeInspector> typeInspectorFactory) where TTypeInspector : ITypeInspector
		{
			Action<IRegistrationLocationSelectionSyntax<ITypeInspector>> action;
			if ((action = StaticBuilderSkeleton<TBuilder>.<>c__18<TTypeInspector>.<>9__18_0) == null)
			{
				action = (StaticBuilderSkeleton<TBuilder>.<>c__18<TTypeInspector>.<>9__18_0 = new Action<IRegistrationLocationSelectionSyntax<ITypeInspector>>(StaticBuilderSkeleton<TBuilder>.<>c__18<TTypeInspector>.<>9.<WithTypeInspector>b__18_0));
			}
			return this.WithTypeInspector<TTypeInspector>(typeInspectorFactory, action);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00012660 File Offset: 0x00010860
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(Func<ITypeInspector, TTypeInspector> typeInspectorFactory, Action<IRegistrationLocationSelectionSyntax<ITypeInspector>> where) where TTypeInspector : ITypeInspector
		{
			StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass19_0<TTypeInspector> CS$<>8__locals1 = new StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass19_0<TTypeInspector>();
			CS$<>8__locals1.typeInspectorFactory = typeInspectorFactory;
			if (CS$<>8__locals1.typeInspectorFactory == null)
			{
				throw new ArgumentNullException("typeInspectorFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.typeInspectorFactories.CreateRegistrationLocationSelector(typeof(TTypeInspector), new Func<ITypeInspector, ITypeInspector>(CS$<>8__locals1.<WithTypeInspector>b__0)));
			return this.Self;
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000126C8 File Offset: 0x000108C8
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(WrapperFactory<ITypeInspector, ITypeInspector, TTypeInspector> typeInspectorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<ITypeInspector>> where) where TTypeInspector : ITypeInspector
		{
			StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass20_0<TTypeInspector> CS$<>8__locals1 = new StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass20_0<TTypeInspector>();
			CS$<>8__locals1.typeInspectorFactory = typeInspectorFactory;
			if (CS$<>8__locals1.typeInspectorFactory == null)
			{
				throw new ArgumentNullException("typeInspectorFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.typeInspectorFactories.CreateTrackingRegistrationLocationSelector(typeof(TTypeInspector), new Func<ITypeInspector, ITypeInspector, ITypeInspector>(CS$<>8__locals1.<WithTypeInspector>b__0)));
			return this.Self;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00012730 File Offset: 0x00010930
		public TBuilder WithoutTypeInspector<[Nullable(0)] TTypeInspector>() where TTypeInspector : ITypeInspector
		{
			return this.WithoutTypeInspector(typeof(TTypeInspector));
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00012742 File Offset: 0x00010942
		public TBuilder WithoutTypeInspector(Type inspectorType)
		{
			if (inspectorType == null)
			{
				throw new ArgumentNullException("inspectorType");
			}
			this.typeInspectorFactories.Remove(inspectorType);
			return this.Self;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0001276A File Offset: 0x0001096A
		public TBuilder WithYamlFormatter(YamlFormatter formatter)
		{
			if (formatter == null)
			{
				throw new ArgumentNullException("formatter");
			}
			this.yamlFormatter = formatter;
			return this.Self;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00012788 File Offset: 0x00010988
		protected IEnumerable<IYamlTypeConverter> BuildTypeConverters()
		{
			return this.typeConverterFactories.BuildComponentList<IYamlTypeConverter>();
		}

		// Token: 0x04000222 RID: 546
		internal INamingConvention namingConvention;

		// Token: 0x04000223 RID: 547
		internal ITypeResolver typeResolver;

		// Token: 0x04000224 RID: 548
		internal readonly LazyComponentRegistrationList<Nothing, IYamlTypeConverter> typeConverterFactories;

		// Token: 0x04000225 RID: 549
		internal readonly LazyComponentRegistrationList<ITypeInspector, ITypeInspector> typeInspectorFactories;

		// Token: 0x04000226 RID: 550
		internal bool includeNonPublicProperties;

		// Token: 0x04000227 RID: 551
		internal Settings settings;

		// Token: 0x04000228 RID: 552
		internal YamlFormatter yamlFormatter;
	}
}
