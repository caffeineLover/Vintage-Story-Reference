using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OpenTK.Audio.OpenAL
{
	// Token: 0x02000047 RID: 71
	public abstract class ALBase
	{
		// Token: 0x060000F6 RID: 246 RVA: 0x0000711E File Offset: 0x0000531E
		[MethodImpl(8)]
		public static void RegisterOpenALResolver()
		{
			ALLoader.RegisterDllResolver();
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00007128 File Offset: 0x00005328
		protected internal static TDelegate LoadDelegate<TDelegate>(string name) where TDelegate : Delegate
		{
			IntPtr procAddress = AL.GetProcAddress(name);
			if (procAddress == IntPtr.Zero)
			{
				MethodInfo method = typeof(TDelegate).GetMethod("Invoke");
				Type returnType = method.ReturnType;
				IEnumerable<ParameterInfo> parameters = method.GetParameters();
				Func<ParameterInfo, Type> func;
				if ((func = ALBase.<>c__1<TDelegate>.<>9__1_0) == null)
				{
					func = (ALBase.<>c__1<TDelegate>.<>9__1_0 = new Func<ParameterInfo, Type>(ALBase.<>c__1<TDelegate>.<>9.<LoadDelegate>b__1_0));
				}
				Type[] array = Enumerable.ToArray<Type>(Enumerable.Select<ParameterInfo, Type>(parameters, func));
				DynamicMethod dynamicMethod = new DynamicMethod("OpenAL_AL_Extension_Null_GetProcAddress_Exception_Delegate_" + Guid.NewGuid().ToString(), returnType, array);
				ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
				ilgenerator.Emit(OpCodes.Ldstr, "This OpenAL function could not be loaded. This likely means that this extension isn't present in the current context.");
				ilgenerator.Emit(OpCodes.Newobj, typeof(Exception).GetConstructor(new Type[]
				{
					typeof(string)
				}));
				ilgenerator.Emit(OpCodes.Throw);
				return (TDelegate)((object)dynamicMethod.CreateDelegate(typeof(TDelegate)));
			}
			return Marshal.GetDelegateForFunctionPointer<TDelegate>(procAddress);
		}
	}
}
