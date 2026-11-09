using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Implementations;
using CombatOverhaul.RangedSystems.Aiming;

namespace Firearms
{
	// Token: 0x0200002D RID: 45
	[NullableContext(1)]
	[Nullable(0)]
	public class RevolverStats : WeaponStats
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000A321 File Offset: 0x00008521
		// (set) Token: 0x0600020F RID: 527 RVA: 0x0000A329 File Offset: 0x00008529
		public bool AutomaticFire { get; set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000A332 File Offset: 0x00008532
		// (set) Token: 0x06000211 RID: 529 RVA: 0x0000A33A File Offset: 0x0000853A
		[Nullable(2)]
		public RevolverLoadStageStats LoadStagesTemplate { [NullableContext(2)] get; [NullableContext(2)] set; } = new RevolverLoadStageStats();

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000A343 File Offset: 0x00008543
		// (set) Token: 0x06000213 RID: 531 RVA: 0x0000A34B File Offset: 0x0000854B
		public RevolverLoadStageStats[] LoadStages { get; set; } = Array.Empty<RevolverLoadStageStats>();

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000A354 File Offset: 0x00008554
		// (set) Token: 0x06000215 RID: 533 RVA: 0x0000A35C File Offset: 0x0000855C
		public RevolverFiringStats FiringStats { get; set; } = new RevolverFiringStats();

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000A365 File Offset: 0x00008565
		// (set) Token: 0x06000217 RID: 535 RVA: 0x0000A36D File Offset: 0x0000856D
		public AimingStatsJson Aiming { get; set; } = new AimingStatsJson();

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000A376 File Offset: 0x00008576
		// (set) Token: 0x06000219 RID: 537 RVA: 0x0000A37E File Offset: 0x0000857E
		public RecoilStats Recoil { get; set; } = RecoilStats.RevolverDefaults();

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000A387 File Offset: 0x00008587
		// (set) Token: 0x0600021B RID: 539 RVA: 0x0000A38F File Offset: 0x0000858F
		public string BulletWildcard { get; set; } = "*:bullet-*";

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000A398 File Offset: 0x00008598
		// (set) Token: 0x0600021D RID: 541 RVA: 0x0000A3A0 File Offset: 0x000085A0
		public string FlaskWildcard { get; set; } = "*powderflask-*";

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000A3A9 File Offset: 0x000085A9
		// (set) Token: 0x0600021F RID: 543 RVA: 0x0000A3B1 File Offset: 0x000085B1
		public string WaddingWildcard { get; set; } = "*linenpatch*";

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000A3BA File Offset: 0x000085BA
		// (set) Token: 0x06000221 RID: 545 RVA: 0x0000A3C2 File Offset: 0x000085C2
		public string LoadingRequirementWildcard { get; set; } = "";

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000A3CB File Offset: 0x000085CB
		// (set) Token: 0x06000223 RID: 547 RVA: 0x0000A3D3 File Offset: 0x000085D3
		public string LoadingRequirementMessage { get; set; } = "maltiezfirearms:requirement-missing-loading-equipment";

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000A3DC File Offset: 0x000085DC
		// (set) Token: 0x06000225 RID: 549 RVA: 0x0000A3E4 File Offset: 0x000085E4
		public bool CancelReloadOnInAir { get; set; } = true;

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000A3ED File Offset: 0x000085ED
		// (set) Token: 0x06000227 RID: 551 RVA: 0x0000A3F5 File Offset: 0x000085F5
		public float ReloadAnimationSpeed { get; set; } = 1f;

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000A3FE File Offset: 0x000085FE
		// (set) Token: 0x06000229 RID: 553 RVA: 0x0000A406 File Offset: 0x00008606
		public bool TwoHanded { get; set; } = true;

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000A40F File Offset: 0x0000860F
		// (set) Token: 0x0600022B RID: 555 RVA: 0x0000A417 File Offset: 0x00008617
		public int BulletsLoadedPerBulletItem { get; set; } = 1;

		// Token: 0x0600022C RID: 556 RVA: 0x0000A420 File Offset: 0x00008620
		public void ApplyLoadStagesTemplate()
		{
			if (this.LoadStagesTemplate == null)
			{
				return;
			}
			for (int index = 0; index < this.LoadStages.Length; index++)
			{
				RevolverLoadStageStats currentStage = this.LoadStages[index];
				currentStage.LoadedAnimation = RevolverStats.ApplyAnimationTemplate(index, this.LoadStagesTemplate.LoadedAnimation, currentStage.LoadedAnimation);
				currentStage.UnloadedAnimation = RevolverStats.ApplyAnimationTemplate(index, this.LoadStagesTemplate.UnloadedAnimation, currentStage.UnloadedAnimation);
				currentStage.LoadingAnimation = RevolverStats.ApplyAnimationTemplate(index, this.LoadStagesTemplate.LoadingAnimation, currentStage.LoadingAnimation);
				currentStage.CockingAnimation = RevolverStats.ApplyAnimationTemplate(index, this.LoadStagesTemplate.CockingAnimation, currentStage.CockingAnimation);
				currentStage.CockedAnimation = RevolverStats.ApplyAnimationTemplate(index, this.LoadStagesTemplate.CockedAnimation, currentStage.CockedAnimation);
				currentStage.ShootAnimation = RevolverStats.ApplyAnimationTemplate(index, this.LoadStagesTemplate.ShootAnimation, currentStage.ShootAnimation);
				currentStage.FiredAnimation = RevolverStats.ApplyAnimationTemplate(index, this.LoadStagesTemplate.FiredAnimation, currentStage.FiredAnimation);
				currentStage.AimAnimation = RevolverStats.ApplyAnimationTemplate(index, this.LoadStagesTemplate.AimAnimation, currentStage.AimAnimation);
				currentStage.StartLoadingAnimation = RevolverStats.ApplyAnimationTemplate(index, this.LoadStagesTemplate.StartLoadingAnimation, currentStage.StartLoadingAnimation);
				currentStage.ContinueLoadingAnimation = RevolverStats.ApplyAnimationTemplate(index, this.LoadStagesTemplate.ContinueLoadingAnimation, currentStage.ContinueLoadingAnimation);
				currentStage.FinishLoadingAnimation = RevolverStats.ApplyAnimationTemplate(index, this.LoadStagesTemplate.FinishLoadingAnimation, currentStage.FinishLoadingAnimation);
				currentStage.CancelLoadingAnimation = RevolverStats.ApplyAnimationTemplate(index, this.LoadStagesTemplate.CancelLoadingAnimation, currentStage.CancelLoadingAnimation);
				currentStage.CylinderPositionAnimation = RevolverStats.ApplyAnimationTemplate(index, this.LoadStagesTemplate.CylinderPositionAnimation, currentStage.CylinderPositionAnimation);
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000A5D1 File Offset: 0x000087D1
		protected static string ApplyAnimationTemplate(int index, string template, string animation)
		{
			if (template == "")
			{
				return animation;
			}
			return template.Replace("{index}", index.ToString());
		}
	}
}
