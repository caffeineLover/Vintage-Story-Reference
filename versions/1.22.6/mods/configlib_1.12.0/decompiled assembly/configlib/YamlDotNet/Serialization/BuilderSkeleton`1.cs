using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000079 RID: 121
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class BuilderSkeleton<[Nullable(0)] TBuilder> where TBuilder : BuilderSkeleton<TBuilder>
	{
		// Token: 0x06000314 RID: 788 RVA: 0x0000F9D4 File Offset: 0x0000DBD4
		internal BuilderSkeleton(ITypeResolver typeResolver)
		{
			this.namingConvention = NullNamingConvention.Instance;
			this.yamlFormatter = YamlFormatter.Default;
			base..ctor();
			this.overrides = new YamlAttributeOverrides();
			LazyComponentRegistrationList<Nothing, IYamlTypeConverter> lazyComponentRegistrationList = new LazyComponentRegistrationList<Nothing, IYamlTypeConverter>();
			Type typeFromHandle = typeof(GuidConverter);
			Func<Nothing, IYamlTypeConverter> func;
			if ((func = BuilderSkeleton<TBuilder>.<>c.<>9__9_0) == null)
			{
				func = (BuilderSkeleton<TBuilder>.<>c.<>9__9_0 = new Func<Nothing, IYamlTypeConverter>(BuilderSkeleton<TBuilder>.<>c.<>9.<.ctor>b__9_0));
			}
			lazyComponentRegistrationList.Add(typeFromHandle, func);
			Type typeFromHandle2 = typeof(SystemTypeConverter);
			Func<Nothing, IYamlTypeConverter> func2;
			if ((func2 = BuilderSkeleton<TBuilder>.<>c.<>9__9_1) == null)
			{
				func2 = (BuilderSkeleton<TBuilder>.<>c.<>9__9_1 = new Func<Nothing, IYamlTypeConverter>(BuilderSkeleton<TBuilder>.<>c.<>9.<.ctor>b__9_1));
			}
			lazyComponentRegistrationList.Add(typeFromHandle2, func2);
			this.typeConverterFactories = lazyComponentRegistrationList;
			this.typeInspectorFactories = new LazyComponentRegistrationList<ITypeInspector, ITypeInspector>();
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			this.settings = new Settings();
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000315 RID: 789
		protected abstract TBuilder Self { get; }

		// Token: 0x06000316 RID: 790 RVA: 0x0000FA9D File Offset: 0x0000DC9D
		public TBuilder IgnoreFields()
		{
			this.ignoreFields = true;
			return this.Self;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000FAAC File Offset: 0x0000DCAC
		public TBuilder IncludeNonPublicProperties()
		{
			this.includeNonPublicProperties = true;
			return this.Self;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000FABB File Offset: 0x0000DCBB
		public TBuilder EnablePrivateConstructors()
		{
			this.settings.AllowPrivateConstructors = true;
			return this.Self;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000FACF File Offset: 0x0000DCCF
		public TBuilder WithNamingConvention(INamingConvention namingConvention)
		{
			if (namingConvention == null)
			{
				throw new ArgumentNullException("namingConvention");
			}
			this.namingConvention = namingConvention;
			return this.Self;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000FAED File Offset: 0x0000DCED
		public TBuilder WithTypeResolver(ITypeResolver typeResolver)
		{
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			return this.Self;
		}

		// Token: 0x0600031B RID: 795
		public abstract TBuilder WithTagMapping(TagName tag, Type type);

		// Token: 0x0600031C RID: 796 RVA: 0x0000FB0B File Offset: 0x0000DD0B
		public TBuilder WithAttributeOverride<[Nullable(2)] TClass>(Expression<Func<TClass, object>> propertyAccessor, Attribute attribute)
		{
			this.overrides.Add<TClass>(propertyAccessor, attribute);
			return this.Self;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000FB20 File Offset: 0x0000DD20
		public TBuilder WithAttributeOverride(Type type, string member, Attribute attribute)
		{
			this.overrides.Add(type, member, attribute);
			return this.Self;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000FB36 File Offset: 0x0000DD36
		public TBuilder WithTypeConverter(IYamlTypeConverter typeConverter)
		{
			Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>> action;
			if ((action = BuilderSkeleton<TBuilder>.<>c.<>9__20_0) == null)
			{
				action = (BuilderSkeleton<TBuilder>.<>c.<>9__20_0 = new Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>>(BuilderSkeleton<TBuilder>.<>c.<>9.<WithTypeConverter>b__20_0));
			}
			return this.WithTypeConverter(typeConverter, action);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000FB60 File Offset: 0x0000DD60
		public TBuilder WithTypeConverter(IYamlTypeConverter typeConverter, Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>> where)
		{
			BuilderSkeleton<TBuilder>.<>c__DisplayClass21_0 CS$<>8__locals1 = new BuilderSkeleton<TBuilder>.<>c__DisplayClass21_0();
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

		// Token: 0x06000320 RID: 800 RVA: 0x0000FBCC File Offset: 0x0000DDCC
		public TBuilder WithTypeConverter<[Nullable(0)] TYamlTypeConverter>(WrapperFactory<IYamlTypeConverter, IYamlTypeConverter> typeConverterFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IYamlTypeConverter>> where) where TYamlTypeConverter : IYamlTypeConverter
		{
			BuilderSkeleton<TBuilder>.<>c__DisplayClass22_0<TYamlTypeConverter> CS$<>8__locals1 = new BuilderSkeleton<TBuilder>.<>c__DisplayClass22_0<TYamlTypeConverter>();
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

		// Token: 0x06000321 RID: 801 RVA: 0x0000FC34 File Offset: 0x0000DE34
		public TBuilder WithoutTypeConverter<[Nullable(0)] TYamlTypeConverter>() where TYamlTypeConverter : IYamlTypeConverter
		{
			return this.WithoutTypeConverter(typeof(TYamlTypeConverter));
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000FC46 File Offset: 0x0000DE46
		public TBuilder WithoutTypeConverter(Type converterType)
		{
			if (converterType == null)
			{
				throw new ArgumentNullException("converterType");
			}
			this.typeConverterFactories.Remove(converterType);
			return this.Self;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000FC6E File Offset: 0x0000DE6E
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(Func<ITypeInspector, TTypeInspector> typeInspectorFactory) where TTypeInspector : ITypeInspector
		{
			Action<IRegistrationLocationSelectionSyntax<ITypeInspector>> action;
			if ((action = BuilderSkeleton<TBuilder>.<>c__25<TTypeInspector>.<>9__25_0) == null)
			{
				action = (BuilderSkeleton<TBuilder>.<>c__25<TTypeInspector>.<>9__25_0 = new Action<IRegistrationLocationSelectionSyntax<ITypeInspector>>(BuilderSkeleton<TBuilder>.<>c__25<TTypeInspector>.<>9.<WithTypeInspector>b__25_0));
			}
			return this.WithTypeInspector<TTypeInspector>(typeInspectorFactory, action);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000FC98 File Offset: 0x0000DE98
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(Func<ITypeInspector, TTypeInspector> typeInspectorFactory, Action<IRegistrationLocationSelectionSyntax<ITypeInspector>> where) where TTypeInspector : ITypeInspector
		{
			BuilderSkeleton<TBuilder>.<>c__DisplayClass26_0<TTypeInspector> CS$<>8__locals1 = new BuilderSkeleton<TBuilder>.<>c__DisplayClass26_0<TTypeInspector>();
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

		// Token: 0x06000325 RID: 805 RVA: 0x0000FD00 File Offset: 0x0000DF00
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(WrapperFactory<ITypeInspector, ITypeInspector, TTypeInspector> typeInspectorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<ITypeInspector>> where) where TTypeInspector : ITypeInspector
		{
			BuilderSkeleton<TBuilder>.<>c__DisplayClass27_0<TTypeInspector> CS$<>8__locals1 = new BuilderSkeleton<TBuilder>.<>c__DisplayClass27_0<TTypeInspector>();
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

		// Token: 0x06000326 RID: 806 RVA: 0x0000FD68 File Offset: 0x0000DF68
		public TBuilder WithoutTypeInspector<[Nullable(0)] TTypeInspector>() where TTypeInspector : ITypeInspector
		{
			return this.WithoutTypeInspector(typeof(TTypeInspector));
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000FD7A File Offset: 0x0000DF7A
		public TBuilder WithoutTypeInspector(Type inspectorType)
		{
			if (inspectorType == null)
			{
				throw new ArgumentNullException("inspectorType");
			}
			this.typeInspectorFactories.Remove(inspectorType);
			return this.Self;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000FDA2 File Offset: 0x0000DFA2
		public TBuilder WithYamlFormatter(YamlFormatter formatter)
		{
			if (formatter == null)
			{
				throw new ArgumentNullException("formatter");
			}
			this.yamlFormatter = formatter;
			return this.Self;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000FDC0 File Offset: 0x0000DFC0
		protected IEnumerable<IYamlTypeConverter> BuildTypeConverters()
		{
			return this.typeConverterFactories.BuildComponentList<IYamlTypeConverter>();
		}

		// Token: 0x0400018D RID: 397
		internal INamingConvention namingConvention;

		// Token: 0x0400018E RID: 398
		internal ITypeResolver typeResolver;

		// Token: 0x0400018F RID: 399
		internal readonly YamlAttributeOverrides overrides;

		// Token: 0x04000190 RID: 400
		internal readonly LazyComponentRegistrationList<Nothing, IYamlTypeConverter> typeConverterFactories;

		// Token: 0x04000191 RID: 401
		internal readonly LazyComponentRegistrationList<ITypeInspector, ITypeInspector> typeInspectorFactories;

		// Token: 0x04000192 RID: 402
		internal bool ignoreFields;

		// Token: 0x04000193 RID: 403
		internal bool includeNonPublicProperties;

		// Token: 0x04000194 RID: 404
		internal Settings settings;

		// Token: 0x04000195 RID: 405
		internal YamlFormatter yamlFormatter;
	}
}
