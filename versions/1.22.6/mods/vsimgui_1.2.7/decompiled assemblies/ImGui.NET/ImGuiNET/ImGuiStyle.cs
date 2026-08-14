using System;
using System.Numerics;

namespace ImGuiNET
{
	// Token: 0x0200005C RID: 92
	public struct ImGuiStyle
	{
		// Token: 0x040005FA RID: 1530
		public float Alpha;

		// Token: 0x040005FB RID: 1531
		public float DisabledAlpha;

		// Token: 0x040005FC RID: 1532
		public Vector2 WindowPadding;

		// Token: 0x040005FD RID: 1533
		public float WindowRounding;

		// Token: 0x040005FE RID: 1534
		public float WindowBorderSize;

		// Token: 0x040005FF RID: 1535
		public Vector2 WindowMinSize;

		// Token: 0x04000600 RID: 1536
		public Vector2 WindowTitleAlign;

		// Token: 0x04000601 RID: 1537
		public ImGuiDir WindowMenuButtonPosition;

		// Token: 0x04000602 RID: 1538
		public float ChildRounding;

		// Token: 0x04000603 RID: 1539
		public float ChildBorderSize;

		// Token: 0x04000604 RID: 1540
		public float PopupRounding;

		// Token: 0x04000605 RID: 1541
		public float PopupBorderSize;

		// Token: 0x04000606 RID: 1542
		public Vector2 FramePadding;

		// Token: 0x04000607 RID: 1543
		public float FrameRounding;

		// Token: 0x04000608 RID: 1544
		public float FrameBorderSize;

		// Token: 0x04000609 RID: 1545
		public Vector2 ItemSpacing;

		// Token: 0x0400060A RID: 1546
		public Vector2 ItemInnerSpacing;

		// Token: 0x0400060B RID: 1547
		public Vector2 CellPadding;

		// Token: 0x0400060C RID: 1548
		public Vector2 TouchExtraPadding;

		// Token: 0x0400060D RID: 1549
		public float IndentSpacing;

		// Token: 0x0400060E RID: 1550
		public float ColumnsMinSpacing;

		// Token: 0x0400060F RID: 1551
		public float ScrollbarSize;

		// Token: 0x04000610 RID: 1552
		public float ScrollbarRounding;

		// Token: 0x04000611 RID: 1553
		public float GrabMinSize;

		// Token: 0x04000612 RID: 1554
		public float GrabRounding;

		// Token: 0x04000613 RID: 1555
		public float LogSliderDeadzone;

		// Token: 0x04000614 RID: 1556
		public float TabRounding;

		// Token: 0x04000615 RID: 1557
		public float TabBorderSize;

		// Token: 0x04000616 RID: 1558
		public float TabMinWidthForCloseButton;

		// Token: 0x04000617 RID: 1559
		public ImGuiDir ColorButtonPosition;

		// Token: 0x04000618 RID: 1560
		public Vector2 ButtonTextAlign;

		// Token: 0x04000619 RID: 1561
		public Vector2 SelectableTextAlign;

		// Token: 0x0400061A RID: 1562
		public float SeparatorTextBorderSize;

		// Token: 0x0400061B RID: 1563
		public Vector2 SeparatorTextAlign;

		// Token: 0x0400061C RID: 1564
		public Vector2 SeparatorTextPadding;

		// Token: 0x0400061D RID: 1565
		public Vector2 DisplayWindowPadding;

		// Token: 0x0400061E RID: 1566
		public Vector2 DisplaySafeAreaPadding;

		// Token: 0x0400061F RID: 1567
		public float DockingSeparatorSize;

		// Token: 0x04000620 RID: 1568
		public float MouseCursorScale;

		// Token: 0x04000621 RID: 1569
		public byte AntiAliasedLines;

		// Token: 0x04000622 RID: 1570
		public byte AntiAliasedLinesUseTex;

		// Token: 0x04000623 RID: 1571
		public byte AntiAliasedFill;

		// Token: 0x04000624 RID: 1572
		public float CurveTessellationTol;

		// Token: 0x04000625 RID: 1573
		public float CircleTessellationMaxError;

		// Token: 0x04000626 RID: 1574
		public Vector4 Colors_0;

		// Token: 0x04000627 RID: 1575
		public Vector4 Colors_1;

		// Token: 0x04000628 RID: 1576
		public Vector4 Colors_2;

		// Token: 0x04000629 RID: 1577
		public Vector4 Colors_3;

		// Token: 0x0400062A RID: 1578
		public Vector4 Colors_4;

		// Token: 0x0400062B RID: 1579
		public Vector4 Colors_5;

		// Token: 0x0400062C RID: 1580
		public Vector4 Colors_6;

		// Token: 0x0400062D RID: 1581
		public Vector4 Colors_7;

		// Token: 0x0400062E RID: 1582
		public Vector4 Colors_8;

		// Token: 0x0400062F RID: 1583
		public Vector4 Colors_9;

		// Token: 0x04000630 RID: 1584
		public Vector4 Colors_10;

		// Token: 0x04000631 RID: 1585
		public Vector4 Colors_11;

		// Token: 0x04000632 RID: 1586
		public Vector4 Colors_12;

		// Token: 0x04000633 RID: 1587
		public Vector4 Colors_13;

		// Token: 0x04000634 RID: 1588
		public Vector4 Colors_14;

		// Token: 0x04000635 RID: 1589
		public Vector4 Colors_15;

		// Token: 0x04000636 RID: 1590
		public Vector4 Colors_16;

		// Token: 0x04000637 RID: 1591
		public Vector4 Colors_17;

		// Token: 0x04000638 RID: 1592
		public Vector4 Colors_18;

		// Token: 0x04000639 RID: 1593
		public Vector4 Colors_19;

		// Token: 0x0400063A RID: 1594
		public Vector4 Colors_20;

		// Token: 0x0400063B RID: 1595
		public Vector4 Colors_21;

		// Token: 0x0400063C RID: 1596
		public Vector4 Colors_22;

		// Token: 0x0400063D RID: 1597
		public Vector4 Colors_23;

		// Token: 0x0400063E RID: 1598
		public Vector4 Colors_24;

		// Token: 0x0400063F RID: 1599
		public Vector4 Colors_25;

		// Token: 0x04000640 RID: 1600
		public Vector4 Colors_26;

		// Token: 0x04000641 RID: 1601
		public Vector4 Colors_27;

		// Token: 0x04000642 RID: 1602
		public Vector4 Colors_28;

		// Token: 0x04000643 RID: 1603
		public Vector4 Colors_29;

		// Token: 0x04000644 RID: 1604
		public Vector4 Colors_30;

		// Token: 0x04000645 RID: 1605
		public Vector4 Colors_31;

		// Token: 0x04000646 RID: 1606
		public Vector4 Colors_32;

		// Token: 0x04000647 RID: 1607
		public Vector4 Colors_33;

		// Token: 0x04000648 RID: 1608
		public Vector4 Colors_34;

		// Token: 0x04000649 RID: 1609
		public Vector4 Colors_35;

		// Token: 0x0400064A RID: 1610
		public Vector4 Colors_36;

		// Token: 0x0400064B RID: 1611
		public Vector4 Colors_37;

		// Token: 0x0400064C RID: 1612
		public Vector4 Colors_38;

		// Token: 0x0400064D RID: 1613
		public Vector4 Colors_39;

		// Token: 0x0400064E RID: 1614
		public Vector4 Colors_40;

		// Token: 0x0400064F RID: 1615
		public Vector4 Colors_41;

		// Token: 0x04000650 RID: 1616
		public Vector4 Colors_42;

		// Token: 0x04000651 RID: 1617
		public Vector4 Colors_43;

		// Token: 0x04000652 RID: 1618
		public Vector4 Colors_44;

		// Token: 0x04000653 RID: 1619
		public Vector4 Colors_45;

		// Token: 0x04000654 RID: 1620
		public Vector4 Colors_46;

		// Token: 0x04000655 RID: 1621
		public Vector4 Colors_47;

		// Token: 0x04000656 RID: 1622
		public Vector4 Colors_48;

		// Token: 0x04000657 RID: 1623
		public Vector4 Colors_49;

		// Token: 0x04000658 RID: 1624
		public Vector4 Colors_50;

		// Token: 0x04000659 RID: 1625
		public Vector4 Colors_51;

		// Token: 0x0400065A RID: 1626
		public Vector4 Colors_52;

		// Token: 0x0400065B RID: 1627
		public Vector4 Colors_53;

		// Token: 0x0400065C RID: 1628
		public Vector4 Colors_54;

		// Token: 0x0400065D RID: 1629
		public float HoverStationaryDelay;

		// Token: 0x0400065E RID: 1630
		public float HoverDelayShort;

		// Token: 0x0400065F RID: 1631
		public float HoverDelayNormal;

		// Token: 0x04000660 RID: 1632
		public ImGuiHoveredFlags HoverFlagsForTooltipMouse;

		// Token: 0x04000661 RID: 1633
		public ImGuiHoveredFlags HoverFlagsForTooltipNav;
	}
}
