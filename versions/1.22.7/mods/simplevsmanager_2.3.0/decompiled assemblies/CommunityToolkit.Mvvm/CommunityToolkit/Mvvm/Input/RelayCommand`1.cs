using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CommunityToolkit.Mvvm.Input
{
	// Token: 0x02000028 RID: 40
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class RelayCommand<T> : IRelayCommand<T>, IRelayCommand, ICommand
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000FB RID: 251 RVA: 0x0000532C File Offset: 0x0000352C
		// (remove) Token: 0x060000FC RID: 252 RVA: 0x00005364 File Offset: 0x00003564
		public event EventHandler CanExecuteChanged;

		// Token: 0x060000FD RID: 253 RVA: 0x00005399 File Offset: 0x00003599
		public RelayCommand([Nullable(new byte[]
		{
			1,
			2
		})] Action<T> execute)
		{
			ArgumentNullException.ThrowIfNull(execute, "execute");
			this.execute = execute;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000053B3 File Offset: 0x000035B3
		public RelayCommand([Nullable(new byte[]
		{
			1,
			2
		})] Action<T> execute, [Nullable(new byte[]
		{
			1,
			2
		})] Predicate<T> canExecute)
		{
			ArgumentNullException.ThrowIfNull(execute, "execute");
			ArgumentNullException.ThrowIfNull(canExecute, "canExecute");
			this.execute = execute;
			this.canExecute = canExecute;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000053DF File Offset: 0x000035DF
		public void NotifyCanExecuteChanged()
		{
			EventHandler canExecuteChanged = this.CanExecuteChanged;
			if (canExecuteChanged == null)
			{
				return;
			}
			canExecuteChanged.Invoke(this, EventArgs.Empty);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000053F7 File Offset: 0x000035F7
		[MethodImpl(256)]
		public bool CanExecute(T parameter)
		{
			Predicate<T> predicate = this.canExecute;
			return predicate == null || predicate.Invoke(parameter);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000540C File Offset: 0x0000360C
		public bool CanExecute(object parameter)
		{
			if (parameter == null && default(T) != null)
			{
				return false;
			}
			T t;
			if (!RelayCommand<T>.TryGetCommandArgument(parameter, ref t))
			{
				RelayCommand<T>.ThrowArgumentExceptionForInvalidCommandArgument(parameter);
			}
			return this.CanExecute(t);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005445 File Offset: 0x00003645
		[MethodImpl(256)]
		public void Execute(T parameter)
		{
			this.execute.Invoke(parameter);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005454 File Offset: 0x00003654
		public void Execute(object parameter)
		{
			T t;
			if (!RelayCommand<T>.TryGetCommandArgument(parameter, ref t))
			{
				RelayCommand<T>.ThrowArgumentExceptionForInvalidCommandArgument(parameter);
			}
			this.Execute(t);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005478 File Offset: 0x00003678
		[MethodImpl(256)]
		internal static bool TryGetCommandArgument(object parameter, out T result)
		{
			if (parameter == null && default(T) == null)
			{
				result = default(T);
				return true;
			}
			if (parameter is T)
			{
				T t = (T)((object)parameter);
				result = t;
				return true;
			}
			result = default(T);
			return false;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000054C1 File Offset: 0x000036C1
		[DoesNotReturn]
		internal static void ThrowArgumentExceptionForInvalidCommandArgument(object parameter)
		{
			throw RelayCommand<T>.<ThrowArgumentExceptionForInvalidCommandArgument>g__GetException|13_0(parameter);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000054CC File Offset: 0x000036CC
		[NullableContext(1)]
		[CompilerGenerated]
		[MethodImpl(8)]
		internal static Exception <ThrowArgumentExceptionForInvalidCommandArgument>g__GetException|13_0([Nullable(2)] object parameter)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (parameter == null)
			{
				defaultInterpolatedStringHandler..ctor(90, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Parameter \"");
				defaultInterpolatedStringHandler.AppendFormatted("parameter");
				defaultInterpolatedStringHandler.AppendLiteral("\" (object) must not be null, as the command type requires an argument of type ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(".");
				return new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), "parameter");
			}
			defaultInterpolatedStringHandler..ctor(92, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Parameter \"");
			defaultInterpolatedStringHandler.AppendFormatted("parameter");
			defaultInterpolatedStringHandler.AppendLiteral("\" (object) cannot be of type ");
			defaultInterpolatedStringHandler.AppendFormatted<Type>(parameter.GetType());
			defaultInterpolatedStringHandler.AppendLiteral(", as the command type requires an argument of type ");
			defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
			defaultInterpolatedStringHandler.AppendLiteral(".");
			return new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), "parameter");
		}

		// Token: 0x04000059 RID: 89
		[Nullable(new byte[]
		{
			1,
			2
		})]
		private readonly Action<T> execute;

		// Token: 0x0400005A RID: 90
		private readonly Predicate<T> canExecute;
	}
}
