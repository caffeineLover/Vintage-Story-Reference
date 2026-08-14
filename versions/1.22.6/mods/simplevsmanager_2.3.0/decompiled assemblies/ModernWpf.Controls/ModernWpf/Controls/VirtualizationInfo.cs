using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000C5 RID: 197
	internal class VirtualizationInfo
	{
		// Token: 0x060009B2 RID: 2482 RVA: 0x0002222A File Offset: 0x0002042A
		public VirtualizationInfo()
		{
			this.ArrangeBounds = ItemsRepeater.InvalidRect;
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x0002225A File Offset: 0x0002045A
		// (set) Token: 0x060009B4 RID: 2484 RVA: 0x00022262 File Offset: 0x00020462
		public ElementOwner Owner { get; private set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x0002226B File Offset: 0x0002046B
		// (set) Token: 0x060009B6 RID: 2486 RVA: 0x00022273 File Offset: 0x00020473
		public int Index { get; private set; } = -1;

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x0002227C File Offset: 0x0002047C
		public bool IsPinned
		{
			get
			{
				return this.m_pinCounter > 0U;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x00022287 File Offset: 0x00020487
		public bool IsHeldByLayout
		{
			get
			{
				return this.Owner == ElementOwner.Layout;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x00022292 File Offset: 0x00020492
		public bool IsRealized
		{
			get
			{
				return this.IsHeldByLayout || this.Owner == ElementOwner.PinnedPool;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x000222A7 File Offset: 0x000204A7
		public bool IsInUniqueIdResetPool
		{
			get
			{
				return this.Owner == ElementOwner.UniqueIdResetPool;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x000222B2 File Offset: 0x000204B2
		// (set) Token: 0x060009BC RID: 2492 RVA: 0x000222BA File Offset: 0x000204BA
		public bool MustClearDataContext { get; set; }

		// Token: 0x060009BD RID: 2493 RVA: 0x000222C3 File Offset: 0x000204C3
		public void MoveOwnershipToLayoutFromElementFactory(int index, string uniqueId)
		{
			this.Owner = ElementOwner.Layout;
			this.Index = index;
			this.UniqueId = uniqueId;
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x000222DA File Offset: 0x000204DA
		public void MoveOwnershipToLayoutFromUniqueIdResetPool()
		{
			this.Owner = ElementOwner.Layout;
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x000222E3 File Offset: 0x000204E3
		public void MoveOwnershipToLayoutFromPinnedPool()
		{
			this.Owner = ElementOwner.Layout;
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x000222EC File Offset: 0x000204EC
		public void MoveOwnershipToElementFactory()
		{
			this.Owner = ElementOwner.ElementFactory;
			this.m_pinCounter = 0U;
			this.Index = -1;
			this.UniqueId = string.Empty;
			this.ArrangeBounds = ItemsRepeater.InvalidRect;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x00022319 File Offset: 0x00020519
		public void MoveOwnershipToUniqueIdResetPoolFromLayout()
		{
			this.Owner = ElementOwner.UniqueIdResetPool;
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x00022322 File Offset: 0x00020522
		public void MoveOwnershipToAnimator()
		{
			this.Owner = ElementOwner.Animator;
			this.Index = -1;
			this.m_pinCounter = 0U;
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00022339 File Offset: 0x00020539
		public void MoveOwnershipToPinnedPool()
		{
			this.Owner = ElementOwner.PinnedPool;
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x00022344 File Offset: 0x00020544
		public uint AddPin()
		{
			if (!this.IsRealized)
			{
				throw new InvalidOperationException("You can't pin an unrealized element.");
			}
			uint num = this.m_pinCounter + 1U;
			this.m_pinCounter = num;
			return num;
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x00022378 File Offset: 0x00020578
		public uint RemovePin()
		{
			if (!this.IsRealized)
			{
				throw new InvalidOperationException("You can't unpin an unrealized element.");
			}
			if (!this.IsPinned)
			{
				throw new InvalidOperationException("UnpinElement was called more often than PinElement.");
			}
			uint num = this.m_pinCounter - 1U;
			this.m_pinCounter = num;
			return num;
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x000223BC File Offset: 0x000205BC
		public void UpdateIndex(int newIndex)
		{
			this.Index = newIndex;
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x000223C5 File Offset: 0x000205C5
		// (set) Token: 0x060009C8 RID: 2504 RVA: 0x000223CD File Offset: 0x000205CD
		public Rect ArrangeBounds { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060009C9 RID: 2505 RVA: 0x000223D6 File Offset: 0x000205D6
		// (set) Token: 0x060009CA RID: 2506 RVA: 0x000223DE File Offset: 0x000205DE
		public string UniqueId { get; private set; } = string.Empty;

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060009CB RID: 2507 RVA: 0x000223E7 File Offset: 0x000205E7
		// (set) Token: 0x060009CC RID: 2508 RVA: 0x000223EF File Offset: 0x000205EF
		public bool KeepAlive { get; set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060009CD RID: 2509 RVA: 0x000223F8 File Offset: 0x000205F8
		// (set) Token: 0x060009CE RID: 2510 RVA: 0x00022400 File Offset: 0x00020600
		public bool AutoRecycleCandidate { get; set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060009CF RID: 2511 RVA: 0x00022409 File Offset: 0x00020609
		// (set) Token: 0x060009D0 RID: 2512 RVA: 0x00022411 File Offset: 0x00020611
		public Size DesiredSize { get; set; } = Size.Empty;

		// Token: 0x0400041F RID: 1055
		private uint m_pinCounter;
	}
}
