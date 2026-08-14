using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace Jaunt.Systems
{
	// Token: 0x02000005 RID: 5
	public class FatigueSource
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002793 File Offset: 0x00000993
		// (set) Token: 0x0600001B RID: 27 RVA: 0x000027A5 File Offset: 0x000009A5
		public Vec3d SourcePos
		{
			get
			{
				return this._sourcePos ?? this.GetSourcePosition();
			}
			set
			{
				this._sourcePos = value;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000027AE File Offset: 0x000009AE
		public Vec3d GetSourcePosition()
		{
			if (this.SourceEntity != null)
			{
				return this.SourceEntity.Pos.XYZ;
			}
			return this._sourcePos ?? Vec3d.Zero;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000027D8 File Offset: 0x000009D8
		public Entity GetCauseEntity()
		{
			return this.CauseEntity ?? this.SourceEntity;
		}

		// Token: 0x04000012 RID: 18
		public EnumFatigueSource Source;

		// Token: 0x04000013 RID: 19
		public Entity SourceEntity;

		// Token: 0x04000014 RID: 20
		public Entity CauseEntity;

		// Token: 0x04000015 RID: 21
		public Block SourceBlock;

		// Token: 0x04000016 RID: 22
		private Vec3d _sourcePos;
	}
}
