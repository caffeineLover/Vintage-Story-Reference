using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000CA RID: 202
	[NullableContext(1)]
	[Nullable(0)]
	internal class ElementPositionLocator : ContainerElement
	{
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x0000F4BF File Offset: 0x0000D6BF
		// (set) Token: 0x06000637 RID: 1591 RVA: 0x0000F4C7 File Offset: 0x0000D6C7
		public string Id { get; set; }

		// Token: 0x06000638 RID: 1592 RVA: 0x0000F4D0 File Offset: 0x0000D6D0
		internal override void Draw(Size availableSpace)
		{
			base.Draw(availableSpace);
			Matrix4x4 matrix4x = base.Canvas.GetCurrentMatrix().ToMatrix4x4();
			float num = new Vector2(matrix4x.M11, matrix4x.M12).Length();
			float num2 = new Vector2(matrix4x.M21, matrix4x.M22).Length();
			Vector2 vector = Vector2.Transform(Vector2.Zero, matrix4x);
			PageElementLocation location = new PageElementLocation
			{
				Id = this.Id,
				PageNumber = base.PageContext.CurrentPage,
				Width = availableSpace.Width * num,
				Height = availableSpace.Height * num2,
				X = vector.X,
				Y = vector.Y,
				Transform = matrix4x
			};
			base.PageContext.CaptureContentPosition(location);
		}
	}
}
