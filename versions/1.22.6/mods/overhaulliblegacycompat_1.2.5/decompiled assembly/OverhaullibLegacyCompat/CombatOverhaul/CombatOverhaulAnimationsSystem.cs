using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Animations;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;

namespace CombatOverhaul
{
	// Token: 0x02000017 RID: 23
	[NullableContext(2)]
	[Nullable(0)]
	public class CombatOverhaulAnimationsSystem : ModSystem
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000727D File Offset: 0x0000547D
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00007285 File Offset: 0x00005485
		public AnimationsManager PlayerAnimationsManager { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000140 RID: 320 RVA: 0x0000728E File Offset: 0x0000548E
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00007296 File Offset: 0x00005496
		public DebugWindowManager DebugManager { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000142 RID: 322 RVA: 0x0000729F File Offset: 0x0000549F
		// (set) Token: 0x06000143 RID: 323 RVA: 0x000072A7 File Offset: 0x000054A7
		public ParticleEffectsManager ParticleEffectsManager { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000144 RID: 324 RVA: 0x000072B0 File Offset: 0x000054B0
		// (set) Token: 0x06000145 RID: 325 RVA: 0x000072B8 File Offset: 0x000054B8
		public VanillaAnimationsSystemClient ClientVanillaAnimations { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000146 RID: 326 RVA: 0x000072C1 File Offset: 0x000054C1
		// (set) Token: 0x06000147 RID: 327 RVA: 0x000072C9 File Offset: 0x000054C9
		public VanillaAnimationsSystemServer ServerVanillaAnimations { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000148 RID: 328 RVA: 0x000072D2 File Offset: 0x000054D2
		// (set) Token: 0x06000149 RID: 329 RVA: 0x000072DA File Offset: 0x000054DA
		public AnimationSystemClient ClientTpAnimationSystem { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600014A RID: 330 RVA: 0x000072E3 File Offset: 0x000054E3
		// (set) Token: 0x0600014B RID: 331 RVA: 0x000072EB File Offset: 0x000054EB
		public AnimationSystemServer ServerTpAnimationSystem { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600014C RID: 332 RVA: 0x000072F4 File Offset: 0x000054F4
		public IShaderProgram AnimatedItemShaderProgram
		{
			get
			{
				return this._shaderProgram;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600014D RID: 333 RVA: 0x000072FC File Offset: 0x000054FC
		public IShaderProgram AnimatedItemShaderProgramFirstPerson
		{
			get
			{
				return this._shaderProgramFirstPerson;
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007304 File Offset: 0x00005504
		[NullableContext(1)]
		public override void Start(ICoreAPI api)
		{
			this._api = api;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00007310 File Offset: 0x00005510
		[NullableContext(1)]
		public override void StartClientSide(ICoreClientAPI api)
		{
			api.Event.ReloadShader += new ActionBoolReturn(this.LoadAnimatedItemShaders);
			this.LoadAnimatedItemShaders();
			this.ParticleEffectsManager = new ParticleEffectsManager(api);
			this.PlayerAnimationsManager = new AnimationsManager(api, this.ParticleEffectsManager);
			this.DebugManager = new DebugWindowManager(api, this.ParticleEffectsManager, this.PlayerAnimationsManager);
			this.ClientVanillaAnimations = new VanillaAnimationsSystemClient(api);
			this.ClientTpAnimationSystem = new AnimationSystemClient(api);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00007389 File Offset: 0x00005589
		[NullableContext(1)]
		public override void StartServerSide(ICoreServerAPI api)
		{
			this.ParticleEffectsManager = new ParticleEffectsManager(api);
			this.ServerVanillaAnimations = new VanillaAnimationsSystemServer(api);
			this.ServerTpAnimationSystem = new AnimationSystemServer(api);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000073AF File Offset: 0x000055AF
		[NullableContext(1)]
		public override void AssetsFinalize(ICoreAPI api)
		{
			AnimationsManager playerAnimationsManager = this.PlayerAnimationsManager;
			if (playerAnimationsManager != null)
			{
				playerAnimationsManager.Load();
			}
			if (api is ICoreClientAPI)
			{
				DebugWindowManager debugManager = this.DebugManager;
				if (debugManager == null)
				{
					return;
				}
				debugManager.Load(api as ICoreClientAPI);
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000073E0 File Offset: 0x000055E0
		public override void Dispose()
		{
			ICoreClientAPI clientApi = this._api as ICoreClientAPI;
			if (clientApi != null)
			{
				clientApi.Event.ReloadShader -= new ActionBoolReturn(this.LoadAnimatedItemShaders);
			}
			this.DisposeShaders();
			DebugWindowManager debugManager = this.DebugManager;
			if (debugManager != null)
			{
				debugManager.Dispose();
			}
			this.DebugManager = null;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00007434 File Offset: 0x00005634
		private bool LoadAnimatedItemShaders()
		{
			ICoreClientAPI clientApi = this._api as ICoreClientAPI;
			if (clientApi == null)
			{
				return false;
			}
			this.DisposeShaders();
			this._shaderProgram = (clientApi.Shader.NewShaderProgram() as ShaderProgram);
			this._shaderProgramFirstPerson = (clientApi.Shader.NewShaderProgram() as ShaderProgram);
			if (this._shaderProgram == null || this._shaderProgramFirstPerson == null)
			{
				return false;
			}
			this._shaderProgram.AssetDomain = base.Mod.Info.ModID;
			clientApi.Shader.RegisterFileShaderProgram("customstandard", this.AnimatedItemShaderProgram);
			this._shaderProgram.Compile();
			this._shaderProgramFirstPerson.AssetDomain = base.Mod.Info.ModID;
			clientApi.Shader.RegisterFileShaderProgram("customstandardfirstperson", this.AnimatedItemShaderProgramFirstPerson);
			this._shaderProgramFirstPerson.Compile();
			return true;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00007513 File Offset: 0x00005713
		private void DisposeShaders()
		{
			ShaderProgram shaderProgram = this._shaderProgram;
			if (shaderProgram != null)
			{
				shaderProgram.Dispose();
			}
			this._shaderProgram = null;
			ShaderProgram shaderProgramFirstPerson = this._shaderProgramFirstPerson;
			if (shaderProgramFirstPerson != null)
			{
				shaderProgramFirstPerson.Dispose();
			}
			this._shaderProgramFirstPerson = null;
		}

		// Token: 0x0400009B RID: 155
		private ShaderProgram _shaderProgram;

		// Token: 0x0400009C RID: 156
		private ShaderProgram _shaderProgramFirstPerson;

		// Token: 0x0400009D RID: 157
		private ICoreAPI _api;
	}
}
