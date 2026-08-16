using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using ConfigLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Firearms
{
	// Token: 0x02000003 RID: 3
	[NullableContext(1)]
	[Nullable(0)]
	public class FirearmsModSystem : ModSystem
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x0000208C File Offset: 0x0000028C
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002094 File Offset: 0x00000294
		public FirearmsSettings Settings { get; set; } = new FirearmsSettings();

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000008 RID: 8 RVA: 0x000020A0 File Offset: 0x000002A0
		// (remove) Token: 0x06000009 RID: 9 RVA: 0x000020D8 File Offset: 0x000002D8
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public event Action<FirearmsSettings> SettingsChanged;

		// Token: 0x0600000A RID: 10 RVA: 0x00002110 File Offset: 0x00000310
		public override void Start(ICoreAPI api)
		{
			api.RegisterItemClass("Firearms:Muzzleloader", typeof(MuzzleloaderItem));
			api.RegisterItemClass("Firearms:Matchlock", typeof(MatchlockItem));
			api.RegisterItemClass("Firearms:Musket", typeof(MusketItem));
			api.RegisterItemClass("Firearms:PowderFlask", typeof(PowderFlask));
			api.RegisterItemClass("Firearms:Scoped", typeof(ScopedItem));
			api.RegisterItemClass("Firearms:Revolver", typeof(RevolverItem));
			api.RegisterItemClass("Firearms:Pistol", typeof(PistolItem));
			api.RegisterCollectibleBehaviorClass("Firearms:Wettable", typeof(Wettable));
			api.RegisterCollectibleBehaviorClass("Firearms:Igniteable", typeof(Igniteable));
			if (api.ModLoader.IsModEnabled("configlib"))
			{
				this.SubscribeToConfigChange(api);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021F3 File Offset: 0x000003F3
		public override void StartClientSide(ICoreClientAPI api)
		{
			FirearmsRecoilSystem.Start(api);
			FirearmsRenderVariantSanitizer.Start(api);
			this.ApplyBulletTrailSetting();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002207 File Offset: 0x00000407
		public override void Dispose()
		{
			FirearmsRecoilSystem.Stop();
			FirearmsRenderVariantSanitizer.Stop();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002214 File Offset: 0x00000414
		public override void AssetsFinalize(ICoreAPI api)
		{
			ICoreClientAPI clientApi = api as ICoreClientAPI;
			if (clientApi != null)
			{
				this.CheckStatusClientSide(clientApi);
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002234 File Offset: 0x00000434
		private void SubscribeToConfigChange(ICoreAPI api)
		{
			ConfigLibModSystem system = api.ModLoader.GetModSystem<ConfigLibModSystem>(true);
			system.SettingChanged += delegate(string domain, IConfig config, ISetting setting)
			{
				if (domain != "maltiezfirearms")
				{
					return;
				}
				setting.AssignSettingValue(this.Settings);
				Action<FirearmsSettings> settingsChanged = this.SettingsChanged;
				if (settingsChanged != null)
				{
					settingsChanged.Invoke(this.Settings);
				}
				this.ApplyBulletTrailSetting();
			};
			system.ConfigsLoaded += delegate()
			{
				IConfig config = system.GetConfig("maltiezfirearms");
				if (config != null)
				{
					config.AssignSettingsValues(this.Settings);
				}
				Action<FirearmsSettings> settingsChanged = this.SettingsChanged;
				if (settingsChanged != null)
				{
					settingsChanged.Invoke(this.Settings);
				}
				this.ApplyBulletTrailSetting();
			};
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002290 File Offset: 0x00000490
		private void ApplyBulletTrailSetting()
		{
			Type type2 = Enumerable.FirstOrDefault<Type>(Enumerable.Select<Assembly, Type>(AppDomain.CurrentDomain.GetAssemblies(), (Assembly assembly) => assembly.GetType("CombatOverhaul.RangedSystems.ProjectileTrailRenderer", false)), (Type type) => type != null);
			if (type2 == null)
			{
				return;
			}
			MethodInfo method = type2.GetMethod("SetEnabled", 24);
			if (method == null)
			{
				return;
			}
			method.Invoke(null, new object[]
			{
				this.Settings.EnableBulletTrails
			});
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002328 File Offset: 0x00000528
		private void CheckStatusClientSide(ICoreClientAPI api)
		{
			if (api.Settings.Bool["immersiveFpMode"])
			{
				LoggerUtil.Error(api, this, "Immersive first person mode is enabled. It is not supported. Turn this setting off.");
				this.AnnoyPlayer(api, "(Firearms) Immersive first person mode is enabled. It is not supported. Turn this setting off to prevent this message.", () => api.Settings.Bool["immersiveFpMode"]);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000238C File Offset: 0x0000058C
		private void AnnoyPlayer(ICoreClientAPI api, string message, Func<bool> continueDelegate)
		{
			api.World.RegisterCallback(delegate(float _)
			{
				api.TriggerIngameError(this, "error", message);
				if (continueDelegate.Invoke())
				{
					this.AnnoyPlayer(api, message, continueDelegate);
				}
			}, 5000);
		}
	}
}
