using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace ModernWpf.Media.Animation
{
	// Token: 0x02000242 RID: 578
	public class CubicBezierEase : EasingFunctionBase
	{
		// Token: 0x060010E0 RID: 4320 RVA: 0x000314DC File Offset: 0x0002F6DC
		static CubicBezierEase()
		{
			EasingFunctionBase.EasingModeProperty.OverrideMetadata(typeof(CubicBezierEase), new PropertyMetadata(0));
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060010E1 RID: 4321 RVA: 0x000315A8 File Offset: 0x0002F7A8
		// (set) Token: 0x060010E2 RID: 4322 RVA: 0x000315BA File Offset: 0x0002F7BA
		public Point ControlPoint1
		{
			get
			{
				return (Point)base.GetValue(CubicBezierEase.ControlPoint1Property);
			}
			set
			{
				base.SetValue(CubicBezierEase.ControlPoint1Property, value);
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x000315CD File Offset: 0x0002F7CD
		// (set) Token: 0x060010E4 RID: 4324 RVA: 0x000315DF File Offset: 0x0002F7DF
		public Point ControlPoint2
		{
			get
			{
				return (Point)base.GetValue(CubicBezierEase.ControlPoint2Property);
			}
			set
			{
				base.SetValue(CubicBezierEase.ControlPoint2Property, value);
			}
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x000315F2 File Offset: 0x0002F7F2
		protected override Freezable CreateInstanceCore()
		{
			return new CubicBezierEase();
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x000315F9 File Offset: 0x0002F7F9
		protected override double EaseInCore(double normalizedTime)
		{
			return this.GetSplineProgress(normalizedTime);
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00031602 File Offset: 0x0002F802
		protected override void OnChanged()
		{
			this._isDirty = true;
			base.OnChanged();
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00031611 File Offset: 0x0002F811
		private double GetSplineProgress(double linearProgress)
		{
			base.ReadPreamble();
			if (this._isDirty)
			{
				this.Build();
			}
			if (!this._isSpecified)
			{
				return linearProgress;
			}
			this.SetParameterFromX(linearProgress);
			return CubicBezierEase.GetBezierValue(this._By, this._Cy, this._parameter);
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00031650 File Offset: 0x0002F850
		private void Build()
		{
			Point controlPoint = this.ControlPoint1;
			Point controlPoint2 = this.ControlPoint2;
			if (controlPoint == new Point(0.0, 0.0) && controlPoint2 == new Point(1.0, 1.0))
			{
				this._isSpecified = false;
			}
			else
			{
				this._isSpecified = true;
				this._parameter = 0.0;
				this._Bx = 3.0 * controlPoint.X;
				this._Cx = 3.0 * controlPoint2.X;
				this._Cx_Bx = 2.0 * (this._Cx - this._Bx);
				this._three_Cx = 3.0 - this._Cx;
				this._By = 3.0 * controlPoint.Y;
				this._Cy = 3.0 * controlPoint2.Y;
			}
			this._isDirty = false;
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x00031764 File Offset: 0x0002F964
		private static double GetBezierValue(double b, double c, double t)
		{
			double num = 1.0 - t;
			double num2 = t * t;
			return b * t * num * num + c * num2 * num + num2 * t;
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x00031794 File Offset: 0x0002F994
		private void GetXAndDx(double t, out double x, out double dx)
		{
			double num = 1.0 - t;
			double num2 = t * t;
			double num3 = num * num;
			x = this._Bx * t * num3 + this._Cx * num2 * num + num2 * t;
			dx = this._Bx * num3 + this._Cx_Bx * num * t + this._three_Cx * num2;
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x000317F0 File Offset: 0x0002F9F0
		private void SetParameterFromX(double time)
		{
			double num = 0.0;
			double num2 = 1.0;
			if (time == 0.0)
			{
				this._parameter = 0.0;
				return;
			}
			if (time == 1.0)
			{
				this._parameter = 1.0;
				return;
			}
			while (num2 - num > 1E-06)
			{
				double num3;
				double num4;
				this.GetXAndDx(this._parameter, out num3, out num4);
				double num5 = Math.Abs(num4);
				if (num3 > time)
				{
					num2 = this._parameter;
				}
				else
				{
					num = this._parameter;
				}
				if (Math.Abs(num3 - time) < 0.001 * num5)
				{
					break;
				}
				if (num5 > 1E-06)
				{
					double num6 = this._parameter - (num3 - time) / num4;
					if (num6 >= num2)
					{
						this._parameter = (this._parameter + num2) / 2.0;
					}
					else if (num6 <= num)
					{
						this._parameter = (this._parameter + num) / 2.0;
					}
					else
					{
						this._parameter = num6;
					}
				}
				else
				{
					this._parameter = (num + num2) / 2.0;
				}
			}
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00031918 File Offset: 0x0002FB18
		private static void OnControlPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		// Token: 0x04000488 RID: 1160
		public static readonly DependencyProperty ControlPoint1Property = DependencyProperty.Register("ControlPoint1", typeof(Point), typeof(CubicBezierEase), new PropertyMetadata(new Point(0.0, 0.0), new PropertyChangedCallback(CubicBezierEase.OnControlPointChanged)));

		// Token: 0x04000489 RID: 1161
		public static readonly DependencyProperty ControlPoint2Property = DependencyProperty.Register("ControlPoint2", typeof(Point), typeof(CubicBezierEase), new PropertyMetadata(new Point(1.0, 1.0), new PropertyChangedCallback(CubicBezierEase.OnControlPointChanged)));

		// Token: 0x0400048A RID: 1162
		private bool _isSpecified;

		// Token: 0x0400048B RID: 1163
		private bool _isDirty;

		// Token: 0x0400048C RID: 1164
		private double _parameter;

		// Token: 0x0400048D RID: 1165
		private double _Bx;

		// Token: 0x0400048E RID: 1166
		private double _Cx;

		// Token: 0x0400048F RID: 1167
		private double _Cx_Bx;

		// Token: 0x04000490 RID: 1168
		private double _three_Cx;

		// Token: 0x04000491 RID: 1169
		private double _By;

		// Token: 0x04000492 RID: 1170
		private double _Cy;

		// Token: 0x04000493 RID: 1171
		private const double accuracy = 0.001;

		// Token: 0x04000494 RID: 1172
		private const double fuzz = 1E-06;
	}
}
