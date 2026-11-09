using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImGuiNET
{
	// Token: 0x0200003D RID: 61
	public struct ImGuiIO
	{
		// Token: 0x040001D4 RID: 468
		public ImGuiConfigFlags ConfigFlags;

		// Token: 0x040001D5 RID: 469
		public ImGuiBackendFlags BackendFlags;

		// Token: 0x040001D6 RID: 470
		public Vector2 DisplaySize;

		// Token: 0x040001D7 RID: 471
		public float DeltaTime;

		// Token: 0x040001D8 RID: 472
		public float IniSavingRate;

		// Token: 0x040001D9 RID: 473
		public unsafe byte* IniFilename;

		// Token: 0x040001DA RID: 474
		public unsafe byte* LogFilename;

		// Token: 0x040001DB RID: 475
		public unsafe void* UserData;

		// Token: 0x040001DC RID: 476
		public unsafe ImFontAtlas* Fonts;

		// Token: 0x040001DD RID: 477
		public float FontGlobalScale;

		// Token: 0x040001DE RID: 478
		public byte FontAllowUserScaling;

		// Token: 0x040001DF RID: 479
		public unsafe ImFont* FontDefault;

		// Token: 0x040001E0 RID: 480
		public Vector2 DisplayFramebufferScale;

		// Token: 0x040001E1 RID: 481
		public byte ConfigDockingNoSplit;

		// Token: 0x040001E2 RID: 482
		public byte ConfigDockingWithShift;

		// Token: 0x040001E3 RID: 483
		public byte ConfigDockingAlwaysTabBar;

		// Token: 0x040001E4 RID: 484
		public byte ConfigDockingTransparentPayload;

		// Token: 0x040001E5 RID: 485
		public byte ConfigViewportsNoAutoMerge;

		// Token: 0x040001E6 RID: 486
		public byte ConfigViewportsNoTaskBarIcon;

		// Token: 0x040001E7 RID: 487
		public byte ConfigViewportsNoDecoration;

		// Token: 0x040001E8 RID: 488
		public byte ConfigViewportsNoDefaultParent;

		// Token: 0x040001E9 RID: 489
		public byte MouseDrawCursor;

		// Token: 0x040001EA RID: 490
		public byte ConfigMacOSXBehaviors;

		// Token: 0x040001EB RID: 491
		public byte ConfigInputTrickleEventQueue;

		// Token: 0x040001EC RID: 492
		public byte ConfigInputTextCursorBlink;

		// Token: 0x040001ED RID: 493
		public byte ConfigInputTextEnterKeepActive;

		// Token: 0x040001EE RID: 494
		public byte ConfigDragClickToInputText;

		// Token: 0x040001EF RID: 495
		public byte ConfigWindowsResizeFromEdges;

		// Token: 0x040001F0 RID: 496
		public byte ConfigWindowsMoveFromTitleBarOnly;

		// Token: 0x040001F1 RID: 497
		public float ConfigMemoryCompactTimer;

		// Token: 0x040001F2 RID: 498
		public float MouseDoubleClickTime;

		// Token: 0x040001F3 RID: 499
		public float MouseDoubleClickMaxDist;

		// Token: 0x040001F4 RID: 500
		public float MouseDragThreshold;

		// Token: 0x040001F5 RID: 501
		public float KeyRepeatDelay;

		// Token: 0x040001F6 RID: 502
		public float KeyRepeatRate;

		// Token: 0x040001F7 RID: 503
		public byte ConfigDebugBeginReturnValueOnce;

		// Token: 0x040001F8 RID: 504
		public byte ConfigDebugBeginReturnValueLoop;

		// Token: 0x040001F9 RID: 505
		public byte ConfigDebugIgnoreFocusLoss;

		// Token: 0x040001FA RID: 506
		public byte ConfigDebugIniSettings;

		// Token: 0x040001FB RID: 507
		public unsafe byte* BackendPlatformName;

		// Token: 0x040001FC RID: 508
		public unsafe byte* BackendRendererName;

		// Token: 0x040001FD RID: 509
		public unsafe void* BackendPlatformUserData;

		// Token: 0x040001FE RID: 510
		public unsafe void* BackendRendererUserData;

		// Token: 0x040001FF RID: 511
		public unsafe void* BackendLanguageUserData;

		// Token: 0x04000200 RID: 512
		public IntPtr GetClipboardTextFn;

		// Token: 0x04000201 RID: 513
		public IntPtr SetClipboardTextFn;

		// Token: 0x04000202 RID: 514
		public unsafe void* ClipboardUserData;

		// Token: 0x04000203 RID: 515
		public IntPtr SetPlatformImeDataFn;

		// Token: 0x04000204 RID: 516
		public unsafe void* _UnusedPadding;

		// Token: 0x04000205 RID: 517
		public ushort PlatformLocaleDecimalPoint;

		// Token: 0x04000206 RID: 518
		public byte WantCaptureMouse;

		// Token: 0x04000207 RID: 519
		public byte WantCaptureKeyboard;

		// Token: 0x04000208 RID: 520
		public byte WantTextInput;

		// Token: 0x04000209 RID: 521
		public byte WantSetMousePos;

		// Token: 0x0400020A RID: 522
		public byte WantSaveIniSettings;

		// Token: 0x0400020B RID: 523
		public byte NavActive;

		// Token: 0x0400020C RID: 524
		public byte NavVisible;

		// Token: 0x0400020D RID: 525
		public float Framerate;

		// Token: 0x0400020E RID: 526
		public int MetricsRenderVertices;

		// Token: 0x0400020F RID: 527
		public int MetricsRenderIndices;

		// Token: 0x04000210 RID: 528
		public int MetricsRenderWindows;

		// Token: 0x04000211 RID: 529
		public int MetricsActiveWindows;

		// Token: 0x04000212 RID: 530
		public int MetricsActiveAllocations;

		// Token: 0x04000213 RID: 531
		public Vector2 MouseDelta;

		// Token: 0x04000214 RID: 532
		[FixedBuffer(typeof(int), 652)]
		public ImGuiIO.<KeyMap>e__FixedBuffer KeyMap;

		// Token: 0x04000215 RID: 533
		[FixedBuffer(typeof(byte), 652)]
		public ImGuiIO.<KeysDown>e__FixedBuffer KeysDown;

		// Token: 0x04000216 RID: 534
		[FixedBuffer(typeof(float), 16)]
		public ImGuiIO.<NavInputs>e__FixedBuffer NavInputs;

		// Token: 0x04000217 RID: 535
		public IntPtr Ctx;

		// Token: 0x04000218 RID: 536
		public Vector2 MousePos;

		// Token: 0x04000219 RID: 537
		[FixedBuffer(typeof(byte), 5)]
		public ImGuiIO.<MouseDown>e__FixedBuffer MouseDown;

		// Token: 0x0400021A RID: 538
		public float MouseWheel;

		// Token: 0x0400021B RID: 539
		public float MouseWheelH;

		// Token: 0x0400021C RID: 540
		public ImGuiMouseSource MouseSource;

		// Token: 0x0400021D RID: 541
		public uint MouseHoveredViewport;

		// Token: 0x0400021E RID: 542
		public byte KeyCtrl;

		// Token: 0x0400021F RID: 543
		public byte KeyShift;

		// Token: 0x04000220 RID: 544
		public byte KeyAlt;

		// Token: 0x04000221 RID: 545
		public byte KeySuper;

		// Token: 0x04000222 RID: 546
		public ImGuiKey KeyMods;

		// Token: 0x04000223 RID: 547
		public ImGuiKeyData KeysData_0;

		// Token: 0x04000224 RID: 548
		public ImGuiKeyData KeysData_1;

		// Token: 0x04000225 RID: 549
		public ImGuiKeyData KeysData_2;

		// Token: 0x04000226 RID: 550
		public ImGuiKeyData KeysData_3;

		// Token: 0x04000227 RID: 551
		public ImGuiKeyData KeysData_4;

		// Token: 0x04000228 RID: 552
		public ImGuiKeyData KeysData_5;

		// Token: 0x04000229 RID: 553
		public ImGuiKeyData KeysData_6;

		// Token: 0x0400022A RID: 554
		public ImGuiKeyData KeysData_7;

		// Token: 0x0400022B RID: 555
		public ImGuiKeyData KeysData_8;

		// Token: 0x0400022C RID: 556
		public ImGuiKeyData KeysData_9;

		// Token: 0x0400022D RID: 557
		public ImGuiKeyData KeysData_10;

		// Token: 0x0400022E RID: 558
		public ImGuiKeyData KeysData_11;

		// Token: 0x0400022F RID: 559
		public ImGuiKeyData KeysData_12;

		// Token: 0x04000230 RID: 560
		public ImGuiKeyData KeysData_13;

		// Token: 0x04000231 RID: 561
		public ImGuiKeyData KeysData_14;

		// Token: 0x04000232 RID: 562
		public ImGuiKeyData KeysData_15;

		// Token: 0x04000233 RID: 563
		public ImGuiKeyData KeysData_16;

		// Token: 0x04000234 RID: 564
		public ImGuiKeyData KeysData_17;

		// Token: 0x04000235 RID: 565
		public ImGuiKeyData KeysData_18;

		// Token: 0x04000236 RID: 566
		public ImGuiKeyData KeysData_19;

		// Token: 0x04000237 RID: 567
		public ImGuiKeyData KeysData_20;

		// Token: 0x04000238 RID: 568
		public ImGuiKeyData KeysData_21;

		// Token: 0x04000239 RID: 569
		public ImGuiKeyData KeysData_22;

		// Token: 0x0400023A RID: 570
		public ImGuiKeyData KeysData_23;

		// Token: 0x0400023B RID: 571
		public ImGuiKeyData KeysData_24;

		// Token: 0x0400023C RID: 572
		public ImGuiKeyData KeysData_25;

		// Token: 0x0400023D RID: 573
		public ImGuiKeyData KeysData_26;

		// Token: 0x0400023E RID: 574
		public ImGuiKeyData KeysData_27;

		// Token: 0x0400023F RID: 575
		public ImGuiKeyData KeysData_28;

		// Token: 0x04000240 RID: 576
		public ImGuiKeyData KeysData_29;

		// Token: 0x04000241 RID: 577
		public ImGuiKeyData KeysData_30;

		// Token: 0x04000242 RID: 578
		public ImGuiKeyData KeysData_31;

		// Token: 0x04000243 RID: 579
		public ImGuiKeyData KeysData_32;

		// Token: 0x04000244 RID: 580
		public ImGuiKeyData KeysData_33;

		// Token: 0x04000245 RID: 581
		public ImGuiKeyData KeysData_34;

		// Token: 0x04000246 RID: 582
		public ImGuiKeyData KeysData_35;

		// Token: 0x04000247 RID: 583
		public ImGuiKeyData KeysData_36;

		// Token: 0x04000248 RID: 584
		public ImGuiKeyData KeysData_37;

		// Token: 0x04000249 RID: 585
		public ImGuiKeyData KeysData_38;

		// Token: 0x0400024A RID: 586
		public ImGuiKeyData KeysData_39;

		// Token: 0x0400024B RID: 587
		public ImGuiKeyData KeysData_40;

		// Token: 0x0400024C RID: 588
		public ImGuiKeyData KeysData_41;

		// Token: 0x0400024D RID: 589
		public ImGuiKeyData KeysData_42;

		// Token: 0x0400024E RID: 590
		public ImGuiKeyData KeysData_43;

		// Token: 0x0400024F RID: 591
		public ImGuiKeyData KeysData_44;

		// Token: 0x04000250 RID: 592
		public ImGuiKeyData KeysData_45;

		// Token: 0x04000251 RID: 593
		public ImGuiKeyData KeysData_46;

		// Token: 0x04000252 RID: 594
		public ImGuiKeyData KeysData_47;

		// Token: 0x04000253 RID: 595
		public ImGuiKeyData KeysData_48;

		// Token: 0x04000254 RID: 596
		public ImGuiKeyData KeysData_49;

		// Token: 0x04000255 RID: 597
		public ImGuiKeyData KeysData_50;

		// Token: 0x04000256 RID: 598
		public ImGuiKeyData KeysData_51;

		// Token: 0x04000257 RID: 599
		public ImGuiKeyData KeysData_52;

		// Token: 0x04000258 RID: 600
		public ImGuiKeyData KeysData_53;

		// Token: 0x04000259 RID: 601
		public ImGuiKeyData KeysData_54;

		// Token: 0x0400025A RID: 602
		public ImGuiKeyData KeysData_55;

		// Token: 0x0400025B RID: 603
		public ImGuiKeyData KeysData_56;

		// Token: 0x0400025C RID: 604
		public ImGuiKeyData KeysData_57;

		// Token: 0x0400025D RID: 605
		public ImGuiKeyData KeysData_58;

		// Token: 0x0400025E RID: 606
		public ImGuiKeyData KeysData_59;

		// Token: 0x0400025F RID: 607
		public ImGuiKeyData KeysData_60;

		// Token: 0x04000260 RID: 608
		public ImGuiKeyData KeysData_61;

		// Token: 0x04000261 RID: 609
		public ImGuiKeyData KeysData_62;

		// Token: 0x04000262 RID: 610
		public ImGuiKeyData KeysData_63;

		// Token: 0x04000263 RID: 611
		public ImGuiKeyData KeysData_64;

		// Token: 0x04000264 RID: 612
		public ImGuiKeyData KeysData_65;

		// Token: 0x04000265 RID: 613
		public ImGuiKeyData KeysData_66;

		// Token: 0x04000266 RID: 614
		public ImGuiKeyData KeysData_67;

		// Token: 0x04000267 RID: 615
		public ImGuiKeyData KeysData_68;

		// Token: 0x04000268 RID: 616
		public ImGuiKeyData KeysData_69;

		// Token: 0x04000269 RID: 617
		public ImGuiKeyData KeysData_70;

		// Token: 0x0400026A RID: 618
		public ImGuiKeyData KeysData_71;

		// Token: 0x0400026B RID: 619
		public ImGuiKeyData KeysData_72;

		// Token: 0x0400026C RID: 620
		public ImGuiKeyData KeysData_73;

		// Token: 0x0400026D RID: 621
		public ImGuiKeyData KeysData_74;

		// Token: 0x0400026E RID: 622
		public ImGuiKeyData KeysData_75;

		// Token: 0x0400026F RID: 623
		public ImGuiKeyData KeysData_76;

		// Token: 0x04000270 RID: 624
		public ImGuiKeyData KeysData_77;

		// Token: 0x04000271 RID: 625
		public ImGuiKeyData KeysData_78;

		// Token: 0x04000272 RID: 626
		public ImGuiKeyData KeysData_79;

		// Token: 0x04000273 RID: 627
		public ImGuiKeyData KeysData_80;

		// Token: 0x04000274 RID: 628
		public ImGuiKeyData KeysData_81;

		// Token: 0x04000275 RID: 629
		public ImGuiKeyData KeysData_82;

		// Token: 0x04000276 RID: 630
		public ImGuiKeyData KeysData_83;

		// Token: 0x04000277 RID: 631
		public ImGuiKeyData KeysData_84;

		// Token: 0x04000278 RID: 632
		public ImGuiKeyData KeysData_85;

		// Token: 0x04000279 RID: 633
		public ImGuiKeyData KeysData_86;

		// Token: 0x0400027A RID: 634
		public ImGuiKeyData KeysData_87;

		// Token: 0x0400027B RID: 635
		public ImGuiKeyData KeysData_88;

		// Token: 0x0400027C RID: 636
		public ImGuiKeyData KeysData_89;

		// Token: 0x0400027D RID: 637
		public ImGuiKeyData KeysData_90;

		// Token: 0x0400027E RID: 638
		public ImGuiKeyData KeysData_91;

		// Token: 0x0400027F RID: 639
		public ImGuiKeyData KeysData_92;

		// Token: 0x04000280 RID: 640
		public ImGuiKeyData KeysData_93;

		// Token: 0x04000281 RID: 641
		public ImGuiKeyData KeysData_94;

		// Token: 0x04000282 RID: 642
		public ImGuiKeyData KeysData_95;

		// Token: 0x04000283 RID: 643
		public ImGuiKeyData KeysData_96;

		// Token: 0x04000284 RID: 644
		public ImGuiKeyData KeysData_97;

		// Token: 0x04000285 RID: 645
		public ImGuiKeyData KeysData_98;

		// Token: 0x04000286 RID: 646
		public ImGuiKeyData KeysData_99;

		// Token: 0x04000287 RID: 647
		public ImGuiKeyData KeysData_100;

		// Token: 0x04000288 RID: 648
		public ImGuiKeyData KeysData_101;

		// Token: 0x04000289 RID: 649
		public ImGuiKeyData KeysData_102;

		// Token: 0x0400028A RID: 650
		public ImGuiKeyData KeysData_103;

		// Token: 0x0400028B RID: 651
		public ImGuiKeyData KeysData_104;

		// Token: 0x0400028C RID: 652
		public ImGuiKeyData KeysData_105;

		// Token: 0x0400028D RID: 653
		public ImGuiKeyData KeysData_106;

		// Token: 0x0400028E RID: 654
		public ImGuiKeyData KeysData_107;

		// Token: 0x0400028F RID: 655
		public ImGuiKeyData KeysData_108;

		// Token: 0x04000290 RID: 656
		public ImGuiKeyData KeysData_109;

		// Token: 0x04000291 RID: 657
		public ImGuiKeyData KeysData_110;

		// Token: 0x04000292 RID: 658
		public ImGuiKeyData KeysData_111;

		// Token: 0x04000293 RID: 659
		public ImGuiKeyData KeysData_112;

		// Token: 0x04000294 RID: 660
		public ImGuiKeyData KeysData_113;

		// Token: 0x04000295 RID: 661
		public ImGuiKeyData KeysData_114;

		// Token: 0x04000296 RID: 662
		public ImGuiKeyData KeysData_115;

		// Token: 0x04000297 RID: 663
		public ImGuiKeyData KeysData_116;

		// Token: 0x04000298 RID: 664
		public ImGuiKeyData KeysData_117;

		// Token: 0x04000299 RID: 665
		public ImGuiKeyData KeysData_118;

		// Token: 0x0400029A RID: 666
		public ImGuiKeyData KeysData_119;

		// Token: 0x0400029B RID: 667
		public ImGuiKeyData KeysData_120;

		// Token: 0x0400029C RID: 668
		public ImGuiKeyData KeysData_121;

		// Token: 0x0400029D RID: 669
		public ImGuiKeyData KeysData_122;

		// Token: 0x0400029E RID: 670
		public ImGuiKeyData KeysData_123;

		// Token: 0x0400029F RID: 671
		public ImGuiKeyData KeysData_124;

		// Token: 0x040002A0 RID: 672
		public ImGuiKeyData KeysData_125;

		// Token: 0x040002A1 RID: 673
		public ImGuiKeyData KeysData_126;

		// Token: 0x040002A2 RID: 674
		public ImGuiKeyData KeysData_127;

		// Token: 0x040002A3 RID: 675
		public ImGuiKeyData KeysData_128;

		// Token: 0x040002A4 RID: 676
		public ImGuiKeyData KeysData_129;

		// Token: 0x040002A5 RID: 677
		public ImGuiKeyData KeysData_130;

		// Token: 0x040002A6 RID: 678
		public ImGuiKeyData KeysData_131;

		// Token: 0x040002A7 RID: 679
		public ImGuiKeyData KeysData_132;

		// Token: 0x040002A8 RID: 680
		public ImGuiKeyData KeysData_133;

		// Token: 0x040002A9 RID: 681
		public ImGuiKeyData KeysData_134;

		// Token: 0x040002AA RID: 682
		public ImGuiKeyData KeysData_135;

		// Token: 0x040002AB RID: 683
		public ImGuiKeyData KeysData_136;

		// Token: 0x040002AC RID: 684
		public ImGuiKeyData KeysData_137;

		// Token: 0x040002AD RID: 685
		public ImGuiKeyData KeysData_138;

		// Token: 0x040002AE RID: 686
		public ImGuiKeyData KeysData_139;

		// Token: 0x040002AF RID: 687
		public ImGuiKeyData KeysData_140;

		// Token: 0x040002B0 RID: 688
		public ImGuiKeyData KeysData_141;

		// Token: 0x040002B1 RID: 689
		public ImGuiKeyData KeysData_142;

		// Token: 0x040002B2 RID: 690
		public ImGuiKeyData KeysData_143;

		// Token: 0x040002B3 RID: 691
		public ImGuiKeyData KeysData_144;

		// Token: 0x040002B4 RID: 692
		public ImGuiKeyData KeysData_145;

		// Token: 0x040002B5 RID: 693
		public ImGuiKeyData KeysData_146;

		// Token: 0x040002B6 RID: 694
		public ImGuiKeyData KeysData_147;

		// Token: 0x040002B7 RID: 695
		public ImGuiKeyData KeysData_148;

		// Token: 0x040002B8 RID: 696
		public ImGuiKeyData KeysData_149;

		// Token: 0x040002B9 RID: 697
		public ImGuiKeyData KeysData_150;

		// Token: 0x040002BA RID: 698
		public ImGuiKeyData KeysData_151;

		// Token: 0x040002BB RID: 699
		public ImGuiKeyData KeysData_152;

		// Token: 0x040002BC RID: 700
		public ImGuiKeyData KeysData_153;

		// Token: 0x040002BD RID: 701
		public ImGuiKeyData KeysData_154;

		// Token: 0x040002BE RID: 702
		public ImGuiKeyData KeysData_155;

		// Token: 0x040002BF RID: 703
		public ImGuiKeyData KeysData_156;

		// Token: 0x040002C0 RID: 704
		public ImGuiKeyData KeysData_157;

		// Token: 0x040002C1 RID: 705
		public ImGuiKeyData KeysData_158;

		// Token: 0x040002C2 RID: 706
		public ImGuiKeyData KeysData_159;

		// Token: 0x040002C3 RID: 707
		public ImGuiKeyData KeysData_160;

		// Token: 0x040002C4 RID: 708
		public ImGuiKeyData KeysData_161;

		// Token: 0x040002C5 RID: 709
		public ImGuiKeyData KeysData_162;

		// Token: 0x040002C6 RID: 710
		public ImGuiKeyData KeysData_163;

		// Token: 0x040002C7 RID: 711
		public ImGuiKeyData KeysData_164;

		// Token: 0x040002C8 RID: 712
		public ImGuiKeyData KeysData_165;

		// Token: 0x040002C9 RID: 713
		public ImGuiKeyData KeysData_166;

		// Token: 0x040002CA RID: 714
		public ImGuiKeyData KeysData_167;

		// Token: 0x040002CB RID: 715
		public ImGuiKeyData KeysData_168;

		// Token: 0x040002CC RID: 716
		public ImGuiKeyData KeysData_169;

		// Token: 0x040002CD RID: 717
		public ImGuiKeyData KeysData_170;

		// Token: 0x040002CE RID: 718
		public ImGuiKeyData KeysData_171;

		// Token: 0x040002CF RID: 719
		public ImGuiKeyData KeysData_172;

		// Token: 0x040002D0 RID: 720
		public ImGuiKeyData KeysData_173;

		// Token: 0x040002D1 RID: 721
		public ImGuiKeyData KeysData_174;

		// Token: 0x040002D2 RID: 722
		public ImGuiKeyData KeysData_175;

		// Token: 0x040002D3 RID: 723
		public ImGuiKeyData KeysData_176;

		// Token: 0x040002D4 RID: 724
		public ImGuiKeyData KeysData_177;

		// Token: 0x040002D5 RID: 725
		public ImGuiKeyData KeysData_178;

		// Token: 0x040002D6 RID: 726
		public ImGuiKeyData KeysData_179;

		// Token: 0x040002D7 RID: 727
		public ImGuiKeyData KeysData_180;

		// Token: 0x040002D8 RID: 728
		public ImGuiKeyData KeysData_181;

		// Token: 0x040002D9 RID: 729
		public ImGuiKeyData KeysData_182;

		// Token: 0x040002DA RID: 730
		public ImGuiKeyData KeysData_183;

		// Token: 0x040002DB RID: 731
		public ImGuiKeyData KeysData_184;

		// Token: 0x040002DC RID: 732
		public ImGuiKeyData KeysData_185;

		// Token: 0x040002DD RID: 733
		public ImGuiKeyData KeysData_186;

		// Token: 0x040002DE RID: 734
		public ImGuiKeyData KeysData_187;

		// Token: 0x040002DF RID: 735
		public ImGuiKeyData KeysData_188;

		// Token: 0x040002E0 RID: 736
		public ImGuiKeyData KeysData_189;

		// Token: 0x040002E1 RID: 737
		public ImGuiKeyData KeysData_190;

		// Token: 0x040002E2 RID: 738
		public ImGuiKeyData KeysData_191;

		// Token: 0x040002E3 RID: 739
		public ImGuiKeyData KeysData_192;

		// Token: 0x040002E4 RID: 740
		public ImGuiKeyData KeysData_193;

		// Token: 0x040002E5 RID: 741
		public ImGuiKeyData KeysData_194;

		// Token: 0x040002E6 RID: 742
		public ImGuiKeyData KeysData_195;

		// Token: 0x040002E7 RID: 743
		public ImGuiKeyData KeysData_196;

		// Token: 0x040002E8 RID: 744
		public ImGuiKeyData KeysData_197;

		// Token: 0x040002E9 RID: 745
		public ImGuiKeyData KeysData_198;

		// Token: 0x040002EA RID: 746
		public ImGuiKeyData KeysData_199;

		// Token: 0x040002EB RID: 747
		public ImGuiKeyData KeysData_200;

		// Token: 0x040002EC RID: 748
		public ImGuiKeyData KeysData_201;

		// Token: 0x040002ED RID: 749
		public ImGuiKeyData KeysData_202;

		// Token: 0x040002EE RID: 750
		public ImGuiKeyData KeysData_203;

		// Token: 0x040002EF RID: 751
		public ImGuiKeyData KeysData_204;

		// Token: 0x040002F0 RID: 752
		public ImGuiKeyData KeysData_205;

		// Token: 0x040002F1 RID: 753
		public ImGuiKeyData KeysData_206;

		// Token: 0x040002F2 RID: 754
		public ImGuiKeyData KeysData_207;

		// Token: 0x040002F3 RID: 755
		public ImGuiKeyData KeysData_208;

		// Token: 0x040002F4 RID: 756
		public ImGuiKeyData KeysData_209;

		// Token: 0x040002F5 RID: 757
		public ImGuiKeyData KeysData_210;

		// Token: 0x040002F6 RID: 758
		public ImGuiKeyData KeysData_211;

		// Token: 0x040002F7 RID: 759
		public ImGuiKeyData KeysData_212;

		// Token: 0x040002F8 RID: 760
		public ImGuiKeyData KeysData_213;

		// Token: 0x040002F9 RID: 761
		public ImGuiKeyData KeysData_214;

		// Token: 0x040002FA RID: 762
		public ImGuiKeyData KeysData_215;

		// Token: 0x040002FB RID: 763
		public ImGuiKeyData KeysData_216;

		// Token: 0x040002FC RID: 764
		public ImGuiKeyData KeysData_217;

		// Token: 0x040002FD RID: 765
		public ImGuiKeyData KeysData_218;

		// Token: 0x040002FE RID: 766
		public ImGuiKeyData KeysData_219;

		// Token: 0x040002FF RID: 767
		public ImGuiKeyData KeysData_220;

		// Token: 0x04000300 RID: 768
		public ImGuiKeyData KeysData_221;

		// Token: 0x04000301 RID: 769
		public ImGuiKeyData KeysData_222;

		// Token: 0x04000302 RID: 770
		public ImGuiKeyData KeysData_223;

		// Token: 0x04000303 RID: 771
		public ImGuiKeyData KeysData_224;

		// Token: 0x04000304 RID: 772
		public ImGuiKeyData KeysData_225;

		// Token: 0x04000305 RID: 773
		public ImGuiKeyData KeysData_226;

		// Token: 0x04000306 RID: 774
		public ImGuiKeyData KeysData_227;

		// Token: 0x04000307 RID: 775
		public ImGuiKeyData KeysData_228;

		// Token: 0x04000308 RID: 776
		public ImGuiKeyData KeysData_229;

		// Token: 0x04000309 RID: 777
		public ImGuiKeyData KeysData_230;

		// Token: 0x0400030A RID: 778
		public ImGuiKeyData KeysData_231;

		// Token: 0x0400030B RID: 779
		public ImGuiKeyData KeysData_232;

		// Token: 0x0400030C RID: 780
		public ImGuiKeyData KeysData_233;

		// Token: 0x0400030D RID: 781
		public ImGuiKeyData KeysData_234;

		// Token: 0x0400030E RID: 782
		public ImGuiKeyData KeysData_235;

		// Token: 0x0400030F RID: 783
		public ImGuiKeyData KeysData_236;

		// Token: 0x04000310 RID: 784
		public ImGuiKeyData KeysData_237;

		// Token: 0x04000311 RID: 785
		public ImGuiKeyData KeysData_238;

		// Token: 0x04000312 RID: 786
		public ImGuiKeyData KeysData_239;

		// Token: 0x04000313 RID: 787
		public ImGuiKeyData KeysData_240;

		// Token: 0x04000314 RID: 788
		public ImGuiKeyData KeysData_241;

		// Token: 0x04000315 RID: 789
		public ImGuiKeyData KeysData_242;

		// Token: 0x04000316 RID: 790
		public ImGuiKeyData KeysData_243;

		// Token: 0x04000317 RID: 791
		public ImGuiKeyData KeysData_244;

		// Token: 0x04000318 RID: 792
		public ImGuiKeyData KeysData_245;

		// Token: 0x04000319 RID: 793
		public ImGuiKeyData KeysData_246;

		// Token: 0x0400031A RID: 794
		public ImGuiKeyData KeysData_247;

		// Token: 0x0400031B RID: 795
		public ImGuiKeyData KeysData_248;

		// Token: 0x0400031C RID: 796
		public ImGuiKeyData KeysData_249;

		// Token: 0x0400031D RID: 797
		public ImGuiKeyData KeysData_250;

		// Token: 0x0400031E RID: 798
		public ImGuiKeyData KeysData_251;

		// Token: 0x0400031F RID: 799
		public ImGuiKeyData KeysData_252;

		// Token: 0x04000320 RID: 800
		public ImGuiKeyData KeysData_253;

		// Token: 0x04000321 RID: 801
		public ImGuiKeyData KeysData_254;

		// Token: 0x04000322 RID: 802
		public ImGuiKeyData KeysData_255;

		// Token: 0x04000323 RID: 803
		public ImGuiKeyData KeysData_256;

		// Token: 0x04000324 RID: 804
		public ImGuiKeyData KeysData_257;

		// Token: 0x04000325 RID: 805
		public ImGuiKeyData KeysData_258;

		// Token: 0x04000326 RID: 806
		public ImGuiKeyData KeysData_259;

		// Token: 0x04000327 RID: 807
		public ImGuiKeyData KeysData_260;

		// Token: 0x04000328 RID: 808
		public ImGuiKeyData KeysData_261;

		// Token: 0x04000329 RID: 809
		public ImGuiKeyData KeysData_262;

		// Token: 0x0400032A RID: 810
		public ImGuiKeyData KeysData_263;

		// Token: 0x0400032B RID: 811
		public ImGuiKeyData KeysData_264;

		// Token: 0x0400032C RID: 812
		public ImGuiKeyData KeysData_265;

		// Token: 0x0400032D RID: 813
		public ImGuiKeyData KeysData_266;

		// Token: 0x0400032E RID: 814
		public ImGuiKeyData KeysData_267;

		// Token: 0x0400032F RID: 815
		public ImGuiKeyData KeysData_268;

		// Token: 0x04000330 RID: 816
		public ImGuiKeyData KeysData_269;

		// Token: 0x04000331 RID: 817
		public ImGuiKeyData KeysData_270;

		// Token: 0x04000332 RID: 818
		public ImGuiKeyData KeysData_271;

		// Token: 0x04000333 RID: 819
		public ImGuiKeyData KeysData_272;

		// Token: 0x04000334 RID: 820
		public ImGuiKeyData KeysData_273;

		// Token: 0x04000335 RID: 821
		public ImGuiKeyData KeysData_274;

		// Token: 0x04000336 RID: 822
		public ImGuiKeyData KeysData_275;

		// Token: 0x04000337 RID: 823
		public ImGuiKeyData KeysData_276;

		// Token: 0x04000338 RID: 824
		public ImGuiKeyData KeysData_277;

		// Token: 0x04000339 RID: 825
		public ImGuiKeyData KeysData_278;

		// Token: 0x0400033A RID: 826
		public ImGuiKeyData KeysData_279;

		// Token: 0x0400033B RID: 827
		public ImGuiKeyData KeysData_280;

		// Token: 0x0400033C RID: 828
		public ImGuiKeyData KeysData_281;

		// Token: 0x0400033D RID: 829
		public ImGuiKeyData KeysData_282;

		// Token: 0x0400033E RID: 830
		public ImGuiKeyData KeysData_283;

		// Token: 0x0400033F RID: 831
		public ImGuiKeyData KeysData_284;

		// Token: 0x04000340 RID: 832
		public ImGuiKeyData KeysData_285;

		// Token: 0x04000341 RID: 833
		public ImGuiKeyData KeysData_286;

		// Token: 0x04000342 RID: 834
		public ImGuiKeyData KeysData_287;

		// Token: 0x04000343 RID: 835
		public ImGuiKeyData KeysData_288;

		// Token: 0x04000344 RID: 836
		public ImGuiKeyData KeysData_289;

		// Token: 0x04000345 RID: 837
		public ImGuiKeyData KeysData_290;

		// Token: 0x04000346 RID: 838
		public ImGuiKeyData KeysData_291;

		// Token: 0x04000347 RID: 839
		public ImGuiKeyData KeysData_292;

		// Token: 0x04000348 RID: 840
		public ImGuiKeyData KeysData_293;

		// Token: 0x04000349 RID: 841
		public ImGuiKeyData KeysData_294;

		// Token: 0x0400034A RID: 842
		public ImGuiKeyData KeysData_295;

		// Token: 0x0400034B RID: 843
		public ImGuiKeyData KeysData_296;

		// Token: 0x0400034C RID: 844
		public ImGuiKeyData KeysData_297;

		// Token: 0x0400034D RID: 845
		public ImGuiKeyData KeysData_298;

		// Token: 0x0400034E RID: 846
		public ImGuiKeyData KeysData_299;

		// Token: 0x0400034F RID: 847
		public ImGuiKeyData KeysData_300;

		// Token: 0x04000350 RID: 848
		public ImGuiKeyData KeysData_301;

		// Token: 0x04000351 RID: 849
		public ImGuiKeyData KeysData_302;

		// Token: 0x04000352 RID: 850
		public ImGuiKeyData KeysData_303;

		// Token: 0x04000353 RID: 851
		public ImGuiKeyData KeysData_304;

		// Token: 0x04000354 RID: 852
		public ImGuiKeyData KeysData_305;

		// Token: 0x04000355 RID: 853
		public ImGuiKeyData KeysData_306;

		// Token: 0x04000356 RID: 854
		public ImGuiKeyData KeysData_307;

		// Token: 0x04000357 RID: 855
		public ImGuiKeyData KeysData_308;

		// Token: 0x04000358 RID: 856
		public ImGuiKeyData KeysData_309;

		// Token: 0x04000359 RID: 857
		public ImGuiKeyData KeysData_310;

		// Token: 0x0400035A RID: 858
		public ImGuiKeyData KeysData_311;

		// Token: 0x0400035B RID: 859
		public ImGuiKeyData KeysData_312;

		// Token: 0x0400035C RID: 860
		public ImGuiKeyData KeysData_313;

		// Token: 0x0400035D RID: 861
		public ImGuiKeyData KeysData_314;

		// Token: 0x0400035E RID: 862
		public ImGuiKeyData KeysData_315;

		// Token: 0x0400035F RID: 863
		public ImGuiKeyData KeysData_316;

		// Token: 0x04000360 RID: 864
		public ImGuiKeyData KeysData_317;

		// Token: 0x04000361 RID: 865
		public ImGuiKeyData KeysData_318;

		// Token: 0x04000362 RID: 866
		public ImGuiKeyData KeysData_319;

		// Token: 0x04000363 RID: 867
		public ImGuiKeyData KeysData_320;

		// Token: 0x04000364 RID: 868
		public ImGuiKeyData KeysData_321;

		// Token: 0x04000365 RID: 869
		public ImGuiKeyData KeysData_322;

		// Token: 0x04000366 RID: 870
		public ImGuiKeyData KeysData_323;

		// Token: 0x04000367 RID: 871
		public ImGuiKeyData KeysData_324;

		// Token: 0x04000368 RID: 872
		public ImGuiKeyData KeysData_325;

		// Token: 0x04000369 RID: 873
		public ImGuiKeyData KeysData_326;

		// Token: 0x0400036A RID: 874
		public ImGuiKeyData KeysData_327;

		// Token: 0x0400036B RID: 875
		public ImGuiKeyData KeysData_328;

		// Token: 0x0400036C RID: 876
		public ImGuiKeyData KeysData_329;

		// Token: 0x0400036D RID: 877
		public ImGuiKeyData KeysData_330;

		// Token: 0x0400036E RID: 878
		public ImGuiKeyData KeysData_331;

		// Token: 0x0400036F RID: 879
		public ImGuiKeyData KeysData_332;

		// Token: 0x04000370 RID: 880
		public ImGuiKeyData KeysData_333;

		// Token: 0x04000371 RID: 881
		public ImGuiKeyData KeysData_334;

		// Token: 0x04000372 RID: 882
		public ImGuiKeyData KeysData_335;

		// Token: 0x04000373 RID: 883
		public ImGuiKeyData KeysData_336;

		// Token: 0x04000374 RID: 884
		public ImGuiKeyData KeysData_337;

		// Token: 0x04000375 RID: 885
		public ImGuiKeyData KeysData_338;

		// Token: 0x04000376 RID: 886
		public ImGuiKeyData KeysData_339;

		// Token: 0x04000377 RID: 887
		public ImGuiKeyData KeysData_340;

		// Token: 0x04000378 RID: 888
		public ImGuiKeyData KeysData_341;

		// Token: 0x04000379 RID: 889
		public ImGuiKeyData KeysData_342;

		// Token: 0x0400037A RID: 890
		public ImGuiKeyData KeysData_343;

		// Token: 0x0400037B RID: 891
		public ImGuiKeyData KeysData_344;

		// Token: 0x0400037C RID: 892
		public ImGuiKeyData KeysData_345;

		// Token: 0x0400037D RID: 893
		public ImGuiKeyData KeysData_346;

		// Token: 0x0400037E RID: 894
		public ImGuiKeyData KeysData_347;

		// Token: 0x0400037F RID: 895
		public ImGuiKeyData KeysData_348;

		// Token: 0x04000380 RID: 896
		public ImGuiKeyData KeysData_349;

		// Token: 0x04000381 RID: 897
		public ImGuiKeyData KeysData_350;

		// Token: 0x04000382 RID: 898
		public ImGuiKeyData KeysData_351;

		// Token: 0x04000383 RID: 899
		public ImGuiKeyData KeysData_352;

		// Token: 0x04000384 RID: 900
		public ImGuiKeyData KeysData_353;

		// Token: 0x04000385 RID: 901
		public ImGuiKeyData KeysData_354;

		// Token: 0x04000386 RID: 902
		public ImGuiKeyData KeysData_355;

		// Token: 0x04000387 RID: 903
		public ImGuiKeyData KeysData_356;

		// Token: 0x04000388 RID: 904
		public ImGuiKeyData KeysData_357;

		// Token: 0x04000389 RID: 905
		public ImGuiKeyData KeysData_358;

		// Token: 0x0400038A RID: 906
		public ImGuiKeyData KeysData_359;

		// Token: 0x0400038B RID: 907
		public ImGuiKeyData KeysData_360;

		// Token: 0x0400038C RID: 908
		public ImGuiKeyData KeysData_361;

		// Token: 0x0400038D RID: 909
		public ImGuiKeyData KeysData_362;

		// Token: 0x0400038E RID: 910
		public ImGuiKeyData KeysData_363;

		// Token: 0x0400038F RID: 911
		public ImGuiKeyData KeysData_364;

		// Token: 0x04000390 RID: 912
		public ImGuiKeyData KeysData_365;

		// Token: 0x04000391 RID: 913
		public ImGuiKeyData KeysData_366;

		// Token: 0x04000392 RID: 914
		public ImGuiKeyData KeysData_367;

		// Token: 0x04000393 RID: 915
		public ImGuiKeyData KeysData_368;

		// Token: 0x04000394 RID: 916
		public ImGuiKeyData KeysData_369;

		// Token: 0x04000395 RID: 917
		public ImGuiKeyData KeysData_370;

		// Token: 0x04000396 RID: 918
		public ImGuiKeyData KeysData_371;

		// Token: 0x04000397 RID: 919
		public ImGuiKeyData KeysData_372;

		// Token: 0x04000398 RID: 920
		public ImGuiKeyData KeysData_373;

		// Token: 0x04000399 RID: 921
		public ImGuiKeyData KeysData_374;

		// Token: 0x0400039A RID: 922
		public ImGuiKeyData KeysData_375;

		// Token: 0x0400039B RID: 923
		public ImGuiKeyData KeysData_376;

		// Token: 0x0400039C RID: 924
		public ImGuiKeyData KeysData_377;

		// Token: 0x0400039D RID: 925
		public ImGuiKeyData KeysData_378;

		// Token: 0x0400039E RID: 926
		public ImGuiKeyData KeysData_379;

		// Token: 0x0400039F RID: 927
		public ImGuiKeyData KeysData_380;

		// Token: 0x040003A0 RID: 928
		public ImGuiKeyData KeysData_381;

		// Token: 0x040003A1 RID: 929
		public ImGuiKeyData KeysData_382;

		// Token: 0x040003A2 RID: 930
		public ImGuiKeyData KeysData_383;

		// Token: 0x040003A3 RID: 931
		public ImGuiKeyData KeysData_384;

		// Token: 0x040003A4 RID: 932
		public ImGuiKeyData KeysData_385;

		// Token: 0x040003A5 RID: 933
		public ImGuiKeyData KeysData_386;

		// Token: 0x040003A6 RID: 934
		public ImGuiKeyData KeysData_387;

		// Token: 0x040003A7 RID: 935
		public ImGuiKeyData KeysData_388;

		// Token: 0x040003A8 RID: 936
		public ImGuiKeyData KeysData_389;

		// Token: 0x040003A9 RID: 937
		public ImGuiKeyData KeysData_390;

		// Token: 0x040003AA RID: 938
		public ImGuiKeyData KeysData_391;

		// Token: 0x040003AB RID: 939
		public ImGuiKeyData KeysData_392;

		// Token: 0x040003AC RID: 940
		public ImGuiKeyData KeysData_393;

		// Token: 0x040003AD RID: 941
		public ImGuiKeyData KeysData_394;

		// Token: 0x040003AE RID: 942
		public ImGuiKeyData KeysData_395;

		// Token: 0x040003AF RID: 943
		public ImGuiKeyData KeysData_396;

		// Token: 0x040003B0 RID: 944
		public ImGuiKeyData KeysData_397;

		// Token: 0x040003B1 RID: 945
		public ImGuiKeyData KeysData_398;

		// Token: 0x040003B2 RID: 946
		public ImGuiKeyData KeysData_399;

		// Token: 0x040003B3 RID: 947
		public ImGuiKeyData KeysData_400;

		// Token: 0x040003B4 RID: 948
		public ImGuiKeyData KeysData_401;

		// Token: 0x040003B5 RID: 949
		public ImGuiKeyData KeysData_402;

		// Token: 0x040003B6 RID: 950
		public ImGuiKeyData KeysData_403;

		// Token: 0x040003B7 RID: 951
		public ImGuiKeyData KeysData_404;

		// Token: 0x040003B8 RID: 952
		public ImGuiKeyData KeysData_405;

		// Token: 0x040003B9 RID: 953
		public ImGuiKeyData KeysData_406;

		// Token: 0x040003BA RID: 954
		public ImGuiKeyData KeysData_407;

		// Token: 0x040003BB RID: 955
		public ImGuiKeyData KeysData_408;

		// Token: 0x040003BC RID: 956
		public ImGuiKeyData KeysData_409;

		// Token: 0x040003BD RID: 957
		public ImGuiKeyData KeysData_410;

		// Token: 0x040003BE RID: 958
		public ImGuiKeyData KeysData_411;

		// Token: 0x040003BF RID: 959
		public ImGuiKeyData KeysData_412;

		// Token: 0x040003C0 RID: 960
		public ImGuiKeyData KeysData_413;

		// Token: 0x040003C1 RID: 961
		public ImGuiKeyData KeysData_414;

		// Token: 0x040003C2 RID: 962
		public ImGuiKeyData KeysData_415;

		// Token: 0x040003C3 RID: 963
		public ImGuiKeyData KeysData_416;

		// Token: 0x040003C4 RID: 964
		public ImGuiKeyData KeysData_417;

		// Token: 0x040003C5 RID: 965
		public ImGuiKeyData KeysData_418;

		// Token: 0x040003C6 RID: 966
		public ImGuiKeyData KeysData_419;

		// Token: 0x040003C7 RID: 967
		public ImGuiKeyData KeysData_420;

		// Token: 0x040003C8 RID: 968
		public ImGuiKeyData KeysData_421;

		// Token: 0x040003C9 RID: 969
		public ImGuiKeyData KeysData_422;

		// Token: 0x040003CA RID: 970
		public ImGuiKeyData KeysData_423;

		// Token: 0x040003CB RID: 971
		public ImGuiKeyData KeysData_424;

		// Token: 0x040003CC RID: 972
		public ImGuiKeyData KeysData_425;

		// Token: 0x040003CD RID: 973
		public ImGuiKeyData KeysData_426;

		// Token: 0x040003CE RID: 974
		public ImGuiKeyData KeysData_427;

		// Token: 0x040003CF RID: 975
		public ImGuiKeyData KeysData_428;

		// Token: 0x040003D0 RID: 976
		public ImGuiKeyData KeysData_429;

		// Token: 0x040003D1 RID: 977
		public ImGuiKeyData KeysData_430;

		// Token: 0x040003D2 RID: 978
		public ImGuiKeyData KeysData_431;

		// Token: 0x040003D3 RID: 979
		public ImGuiKeyData KeysData_432;

		// Token: 0x040003D4 RID: 980
		public ImGuiKeyData KeysData_433;

		// Token: 0x040003D5 RID: 981
		public ImGuiKeyData KeysData_434;

		// Token: 0x040003D6 RID: 982
		public ImGuiKeyData KeysData_435;

		// Token: 0x040003D7 RID: 983
		public ImGuiKeyData KeysData_436;

		// Token: 0x040003D8 RID: 984
		public ImGuiKeyData KeysData_437;

		// Token: 0x040003D9 RID: 985
		public ImGuiKeyData KeysData_438;

		// Token: 0x040003DA RID: 986
		public ImGuiKeyData KeysData_439;

		// Token: 0x040003DB RID: 987
		public ImGuiKeyData KeysData_440;

		// Token: 0x040003DC RID: 988
		public ImGuiKeyData KeysData_441;

		// Token: 0x040003DD RID: 989
		public ImGuiKeyData KeysData_442;

		// Token: 0x040003DE RID: 990
		public ImGuiKeyData KeysData_443;

		// Token: 0x040003DF RID: 991
		public ImGuiKeyData KeysData_444;

		// Token: 0x040003E0 RID: 992
		public ImGuiKeyData KeysData_445;

		// Token: 0x040003E1 RID: 993
		public ImGuiKeyData KeysData_446;

		// Token: 0x040003E2 RID: 994
		public ImGuiKeyData KeysData_447;

		// Token: 0x040003E3 RID: 995
		public ImGuiKeyData KeysData_448;

		// Token: 0x040003E4 RID: 996
		public ImGuiKeyData KeysData_449;

		// Token: 0x040003E5 RID: 997
		public ImGuiKeyData KeysData_450;

		// Token: 0x040003E6 RID: 998
		public ImGuiKeyData KeysData_451;

		// Token: 0x040003E7 RID: 999
		public ImGuiKeyData KeysData_452;

		// Token: 0x040003E8 RID: 1000
		public ImGuiKeyData KeysData_453;

		// Token: 0x040003E9 RID: 1001
		public ImGuiKeyData KeysData_454;

		// Token: 0x040003EA RID: 1002
		public ImGuiKeyData KeysData_455;

		// Token: 0x040003EB RID: 1003
		public ImGuiKeyData KeysData_456;

		// Token: 0x040003EC RID: 1004
		public ImGuiKeyData KeysData_457;

		// Token: 0x040003ED RID: 1005
		public ImGuiKeyData KeysData_458;

		// Token: 0x040003EE RID: 1006
		public ImGuiKeyData KeysData_459;

		// Token: 0x040003EF RID: 1007
		public ImGuiKeyData KeysData_460;

		// Token: 0x040003F0 RID: 1008
		public ImGuiKeyData KeysData_461;

		// Token: 0x040003F1 RID: 1009
		public ImGuiKeyData KeysData_462;

		// Token: 0x040003F2 RID: 1010
		public ImGuiKeyData KeysData_463;

		// Token: 0x040003F3 RID: 1011
		public ImGuiKeyData KeysData_464;

		// Token: 0x040003F4 RID: 1012
		public ImGuiKeyData KeysData_465;

		// Token: 0x040003F5 RID: 1013
		public ImGuiKeyData KeysData_466;

		// Token: 0x040003F6 RID: 1014
		public ImGuiKeyData KeysData_467;

		// Token: 0x040003F7 RID: 1015
		public ImGuiKeyData KeysData_468;

		// Token: 0x040003F8 RID: 1016
		public ImGuiKeyData KeysData_469;

		// Token: 0x040003F9 RID: 1017
		public ImGuiKeyData KeysData_470;

		// Token: 0x040003FA RID: 1018
		public ImGuiKeyData KeysData_471;

		// Token: 0x040003FB RID: 1019
		public ImGuiKeyData KeysData_472;

		// Token: 0x040003FC RID: 1020
		public ImGuiKeyData KeysData_473;

		// Token: 0x040003FD RID: 1021
		public ImGuiKeyData KeysData_474;

		// Token: 0x040003FE RID: 1022
		public ImGuiKeyData KeysData_475;

		// Token: 0x040003FF RID: 1023
		public ImGuiKeyData KeysData_476;

		// Token: 0x04000400 RID: 1024
		public ImGuiKeyData KeysData_477;

		// Token: 0x04000401 RID: 1025
		public ImGuiKeyData KeysData_478;

		// Token: 0x04000402 RID: 1026
		public ImGuiKeyData KeysData_479;

		// Token: 0x04000403 RID: 1027
		public ImGuiKeyData KeysData_480;

		// Token: 0x04000404 RID: 1028
		public ImGuiKeyData KeysData_481;

		// Token: 0x04000405 RID: 1029
		public ImGuiKeyData KeysData_482;

		// Token: 0x04000406 RID: 1030
		public ImGuiKeyData KeysData_483;

		// Token: 0x04000407 RID: 1031
		public ImGuiKeyData KeysData_484;

		// Token: 0x04000408 RID: 1032
		public ImGuiKeyData KeysData_485;

		// Token: 0x04000409 RID: 1033
		public ImGuiKeyData KeysData_486;

		// Token: 0x0400040A RID: 1034
		public ImGuiKeyData KeysData_487;

		// Token: 0x0400040B RID: 1035
		public ImGuiKeyData KeysData_488;

		// Token: 0x0400040C RID: 1036
		public ImGuiKeyData KeysData_489;

		// Token: 0x0400040D RID: 1037
		public ImGuiKeyData KeysData_490;

		// Token: 0x0400040E RID: 1038
		public ImGuiKeyData KeysData_491;

		// Token: 0x0400040F RID: 1039
		public ImGuiKeyData KeysData_492;

		// Token: 0x04000410 RID: 1040
		public ImGuiKeyData KeysData_493;

		// Token: 0x04000411 RID: 1041
		public ImGuiKeyData KeysData_494;

		// Token: 0x04000412 RID: 1042
		public ImGuiKeyData KeysData_495;

		// Token: 0x04000413 RID: 1043
		public ImGuiKeyData KeysData_496;

		// Token: 0x04000414 RID: 1044
		public ImGuiKeyData KeysData_497;

		// Token: 0x04000415 RID: 1045
		public ImGuiKeyData KeysData_498;

		// Token: 0x04000416 RID: 1046
		public ImGuiKeyData KeysData_499;

		// Token: 0x04000417 RID: 1047
		public ImGuiKeyData KeysData_500;

		// Token: 0x04000418 RID: 1048
		public ImGuiKeyData KeysData_501;

		// Token: 0x04000419 RID: 1049
		public ImGuiKeyData KeysData_502;

		// Token: 0x0400041A RID: 1050
		public ImGuiKeyData KeysData_503;

		// Token: 0x0400041B RID: 1051
		public ImGuiKeyData KeysData_504;

		// Token: 0x0400041C RID: 1052
		public ImGuiKeyData KeysData_505;

		// Token: 0x0400041D RID: 1053
		public ImGuiKeyData KeysData_506;

		// Token: 0x0400041E RID: 1054
		public ImGuiKeyData KeysData_507;

		// Token: 0x0400041F RID: 1055
		public ImGuiKeyData KeysData_508;

		// Token: 0x04000420 RID: 1056
		public ImGuiKeyData KeysData_509;

		// Token: 0x04000421 RID: 1057
		public ImGuiKeyData KeysData_510;

		// Token: 0x04000422 RID: 1058
		public ImGuiKeyData KeysData_511;

		// Token: 0x04000423 RID: 1059
		public ImGuiKeyData KeysData_512;

		// Token: 0x04000424 RID: 1060
		public ImGuiKeyData KeysData_513;

		// Token: 0x04000425 RID: 1061
		public ImGuiKeyData KeysData_514;

		// Token: 0x04000426 RID: 1062
		public ImGuiKeyData KeysData_515;

		// Token: 0x04000427 RID: 1063
		public ImGuiKeyData KeysData_516;

		// Token: 0x04000428 RID: 1064
		public ImGuiKeyData KeysData_517;

		// Token: 0x04000429 RID: 1065
		public ImGuiKeyData KeysData_518;

		// Token: 0x0400042A RID: 1066
		public ImGuiKeyData KeysData_519;

		// Token: 0x0400042B RID: 1067
		public ImGuiKeyData KeysData_520;

		// Token: 0x0400042C RID: 1068
		public ImGuiKeyData KeysData_521;

		// Token: 0x0400042D RID: 1069
		public ImGuiKeyData KeysData_522;

		// Token: 0x0400042E RID: 1070
		public ImGuiKeyData KeysData_523;

		// Token: 0x0400042F RID: 1071
		public ImGuiKeyData KeysData_524;

		// Token: 0x04000430 RID: 1072
		public ImGuiKeyData KeysData_525;

		// Token: 0x04000431 RID: 1073
		public ImGuiKeyData KeysData_526;

		// Token: 0x04000432 RID: 1074
		public ImGuiKeyData KeysData_527;

		// Token: 0x04000433 RID: 1075
		public ImGuiKeyData KeysData_528;

		// Token: 0x04000434 RID: 1076
		public ImGuiKeyData KeysData_529;

		// Token: 0x04000435 RID: 1077
		public ImGuiKeyData KeysData_530;

		// Token: 0x04000436 RID: 1078
		public ImGuiKeyData KeysData_531;

		// Token: 0x04000437 RID: 1079
		public ImGuiKeyData KeysData_532;

		// Token: 0x04000438 RID: 1080
		public ImGuiKeyData KeysData_533;

		// Token: 0x04000439 RID: 1081
		public ImGuiKeyData KeysData_534;

		// Token: 0x0400043A RID: 1082
		public ImGuiKeyData KeysData_535;

		// Token: 0x0400043B RID: 1083
		public ImGuiKeyData KeysData_536;

		// Token: 0x0400043C RID: 1084
		public ImGuiKeyData KeysData_537;

		// Token: 0x0400043D RID: 1085
		public ImGuiKeyData KeysData_538;

		// Token: 0x0400043E RID: 1086
		public ImGuiKeyData KeysData_539;

		// Token: 0x0400043F RID: 1087
		public ImGuiKeyData KeysData_540;

		// Token: 0x04000440 RID: 1088
		public ImGuiKeyData KeysData_541;

		// Token: 0x04000441 RID: 1089
		public ImGuiKeyData KeysData_542;

		// Token: 0x04000442 RID: 1090
		public ImGuiKeyData KeysData_543;

		// Token: 0x04000443 RID: 1091
		public ImGuiKeyData KeysData_544;

		// Token: 0x04000444 RID: 1092
		public ImGuiKeyData KeysData_545;

		// Token: 0x04000445 RID: 1093
		public ImGuiKeyData KeysData_546;

		// Token: 0x04000446 RID: 1094
		public ImGuiKeyData KeysData_547;

		// Token: 0x04000447 RID: 1095
		public ImGuiKeyData KeysData_548;

		// Token: 0x04000448 RID: 1096
		public ImGuiKeyData KeysData_549;

		// Token: 0x04000449 RID: 1097
		public ImGuiKeyData KeysData_550;

		// Token: 0x0400044A RID: 1098
		public ImGuiKeyData KeysData_551;

		// Token: 0x0400044B RID: 1099
		public ImGuiKeyData KeysData_552;

		// Token: 0x0400044C RID: 1100
		public ImGuiKeyData KeysData_553;

		// Token: 0x0400044D RID: 1101
		public ImGuiKeyData KeysData_554;

		// Token: 0x0400044E RID: 1102
		public ImGuiKeyData KeysData_555;

		// Token: 0x0400044F RID: 1103
		public ImGuiKeyData KeysData_556;

		// Token: 0x04000450 RID: 1104
		public ImGuiKeyData KeysData_557;

		// Token: 0x04000451 RID: 1105
		public ImGuiKeyData KeysData_558;

		// Token: 0x04000452 RID: 1106
		public ImGuiKeyData KeysData_559;

		// Token: 0x04000453 RID: 1107
		public ImGuiKeyData KeysData_560;

		// Token: 0x04000454 RID: 1108
		public ImGuiKeyData KeysData_561;

		// Token: 0x04000455 RID: 1109
		public ImGuiKeyData KeysData_562;

		// Token: 0x04000456 RID: 1110
		public ImGuiKeyData KeysData_563;

		// Token: 0x04000457 RID: 1111
		public ImGuiKeyData KeysData_564;

		// Token: 0x04000458 RID: 1112
		public ImGuiKeyData KeysData_565;

		// Token: 0x04000459 RID: 1113
		public ImGuiKeyData KeysData_566;

		// Token: 0x0400045A RID: 1114
		public ImGuiKeyData KeysData_567;

		// Token: 0x0400045B RID: 1115
		public ImGuiKeyData KeysData_568;

		// Token: 0x0400045C RID: 1116
		public ImGuiKeyData KeysData_569;

		// Token: 0x0400045D RID: 1117
		public ImGuiKeyData KeysData_570;

		// Token: 0x0400045E RID: 1118
		public ImGuiKeyData KeysData_571;

		// Token: 0x0400045F RID: 1119
		public ImGuiKeyData KeysData_572;

		// Token: 0x04000460 RID: 1120
		public ImGuiKeyData KeysData_573;

		// Token: 0x04000461 RID: 1121
		public ImGuiKeyData KeysData_574;

		// Token: 0x04000462 RID: 1122
		public ImGuiKeyData KeysData_575;

		// Token: 0x04000463 RID: 1123
		public ImGuiKeyData KeysData_576;

		// Token: 0x04000464 RID: 1124
		public ImGuiKeyData KeysData_577;

		// Token: 0x04000465 RID: 1125
		public ImGuiKeyData KeysData_578;

		// Token: 0x04000466 RID: 1126
		public ImGuiKeyData KeysData_579;

		// Token: 0x04000467 RID: 1127
		public ImGuiKeyData KeysData_580;

		// Token: 0x04000468 RID: 1128
		public ImGuiKeyData KeysData_581;

		// Token: 0x04000469 RID: 1129
		public ImGuiKeyData KeysData_582;

		// Token: 0x0400046A RID: 1130
		public ImGuiKeyData KeysData_583;

		// Token: 0x0400046B RID: 1131
		public ImGuiKeyData KeysData_584;

		// Token: 0x0400046C RID: 1132
		public ImGuiKeyData KeysData_585;

		// Token: 0x0400046D RID: 1133
		public ImGuiKeyData KeysData_586;

		// Token: 0x0400046E RID: 1134
		public ImGuiKeyData KeysData_587;

		// Token: 0x0400046F RID: 1135
		public ImGuiKeyData KeysData_588;

		// Token: 0x04000470 RID: 1136
		public ImGuiKeyData KeysData_589;

		// Token: 0x04000471 RID: 1137
		public ImGuiKeyData KeysData_590;

		// Token: 0x04000472 RID: 1138
		public ImGuiKeyData KeysData_591;

		// Token: 0x04000473 RID: 1139
		public ImGuiKeyData KeysData_592;

		// Token: 0x04000474 RID: 1140
		public ImGuiKeyData KeysData_593;

		// Token: 0x04000475 RID: 1141
		public ImGuiKeyData KeysData_594;

		// Token: 0x04000476 RID: 1142
		public ImGuiKeyData KeysData_595;

		// Token: 0x04000477 RID: 1143
		public ImGuiKeyData KeysData_596;

		// Token: 0x04000478 RID: 1144
		public ImGuiKeyData KeysData_597;

		// Token: 0x04000479 RID: 1145
		public ImGuiKeyData KeysData_598;

		// Token: 0x0400047A RID: 1146
		public ImGuiKeyData KeysData_599;

		// Token: 0x0400047B RID: 1147
		public ImGuiKeyData KeysData_600;

		// Token: 0x0400047C RID: 1148
		public ImGuiKeyData KeysData_601;

		// Token: 0x0400047D RID: 1149
		public ImGuiKeyData KeysData_602;

		// Token: 0x0400047E RID: 1150
		public ImGuiKeyData KeysData_603;

		// Token: 0x0400047F RID: 1151
		public ImGuiKeyData KeysData_604;

		// Token: 0x04000480 RID: 1152
		public ImGuiKeyData KeysData_605;

		// Token: 0x04000481 RID: 1153
		public ImGuiKeyData KeysData_606;

		// Token: 0x04000482 RID: 1154
		public ImGuiKeyData KeysData_607;

		// Token: 0x04000483 RID: 1155
		public ImGuiKeyData KeysData_608;

		// Token: 0x04000484 RID: 1156
		public ImGuiKeyData KeysData_609;

		// Token: 0x04000485 RID: 1157
		public ImGuiKeyData KeysData_610;

		// Token: 0x04000486 RID: 1158
		public ImGuiKeyData KeysData_611;

		// Token: 0x04000487 RID: 1159
		public ImGuiKeyData KeysData_612;

		// Token: 0x04000488 RID: 1160
		public ImGuiKeyData KeysData_613;

		// Token: 0x04000489 RID: 1161
		public ImGuiKeyData KeysData_614;

		// Token: 0x0400048A RID: 1162
		public ImGuiKeyData KeysData_615;

		// Token: 0x0400048B RID: 1163
		public ImGuiKeyData KeysData_616;

		// Token: 0x0400048C RID: 1164
		public ImGuiKeyData KeysData_617;

		// Token: 0x0400048D RID: 1165
		public ImGuiKeyData KeysData_618;

		// Token: 0x0400048E RID: 1166
		public ImGuiKeyData KeysData_619;

		// Token: 0x0400048F RID: 1167
		public ImGuiKeyData KeysData_620;

		// Token: 0x04000490 RID: 1168
		public ImGuiKeyData KeysData_621;

		// Token: 0x04000491 RID: 1169
		public ImGuiKeyData KeysData_622;

		// Token: 0x04000492 RID: 1170
		public ImGuiKeyData KeysData_623;

		// Token: 0x04000493 RID: 1171
		public ImGuiKeyData KeysData_624;

		// Token: 0x04000494 RID: 1172
		public ImGuiKeyData KeysData_625;

		// Token: 0x04000495 RID: 1173
		public ImGuiKeyData KeysData_626;

		// Token: 0x04000496 RID: 1174
		public ImGuiKeyData KeysData_627;

		// Token: 0x04000497 RID: 1175
		public ImGuiKeyData KeysData_628;

		// Token: 0x04000498 RID: 1176
		public ImGuiKeyData KeysData_629;

		// Token: 0x04000499 RID: 1177
		public ImGuiKeyData KeysData_630;

		// Token: 0x0400049A RID: 1178
		public ImGuiKeyData KeysData_631;

		// Token: 0x0400049B RID: 1179
		public ImGuiKeyData KeysData_632;

		// Token: 0x0400049C RID: 1180
		public ImGuiKeyData KeysData_633;

		// Token: 0x0400049D RID: 1181
		public ImGuiKeyData KeysData_634;

		// Token: 0x0400049E RID: 1182
		public ImGuiKeyData KeysData_635;

		// Token: 0x0400049F RID: 1183
		public ImGuiKeyData KeysData_636;

		// Token: 0x040004A0 RID: 1184
		public ImGuiKeyData KeysData_637;

		// Token: 0x040004A1 RID: 1185
		public ImGuiKeyData KeysData_638;

		// Token: 0x040004A2 RID: 1186
		public ImGuiKeyData KeysData_639;

		// Token: 0x040004A3 RID: 1187
		public ImGuiKeyData KeysData_640;

		// Token: 0x040004A4 RID: 1188
		public ImGuiKeyData KeysData_641;

		// Token: 0x040004A5 RID: 1189
		public ImGuiKeyData KeysData_642;

		// Token: 0x040004A6 RID: 1190
		public ImGuiKeyData KeysData_643;

		// Token: 0x040004A7 RID: 1191
		public ImGuiKeyData KeysData_644;

		// Token: 0x040004A8 RID: 1192
		public ImGuiKeyData KeysData_645;

		// Token: 0x040004A9 RID: 1193
		public ImGuiKeyData KeysData_646;

		// Token: 0x040004AA RID: 1194
		public ImGuiKeyData KeysData_647;

		// Token: 0x040004AB RID: 1195
		public ImGuiKeyData KeysData_648;

		// Token: 0x040004AC RID: 1196
		public ImGuiKeyData KeysData_649;

		// Token: 0x040004AD RID: 1197
		public ImGuiKeyData KeysData_650;

		// Token: 0x040004AE RID: 1198
		public ImGuiKeyData KeysData_651;

		// Token: 0x040004AF RID: 1199
		public byte WantCaptureMouseUnlessPopupClose;

		// Token: 0x040004B0 RID: 1200
		public Vector2 MousePosPrev;

		// Token: 0x040004B1 RID: 1201
		public Vector2 MouseClickedPos_0;

		// Token: 0x040004B2 RID: 1202
		public Vector2 MouseClickedPos_1;

		// Token: 0x040004B3 RID: 1203
		public Vector2 MouseClickedPos_2;

		// Token: 0x040004B4 RID: 1204
		public Vector2 MouseClickedPos_3;

		// Token: 0x040004B5 RID: 1205
		public Vector2 MouseClickedPos_4;

		// Token: 0x040004B6 RID: 1206
		[FixedBuffer(typeof(double), 5)]
		public ImGuiIO.<MouseClickedTime>e__FixedBuffer MouseClickedTime;

		// Token: 0x040004B7 RID: 1207
		[FixedBuffer(typeof(byte), 5)]
		public ImGuiIO.<MouseClicked>e__FixedBuffer MouseClicked;

		// Token: 0x040004B8 RID: 1208
		[FixedBuffer(typeof(byte), 5)]
		public ImGuiIO.<MouseDoubleClicked>e__FixedBuffer MouseDoubleClicked;

		// Token: 0x040004B9 RID: 1209
		[FixedBuffer(typeof(ushort), 5)]
		public ImGuiIO.<MouseClickedCount>e__FixedBuffer MouseClickedCount;

		// Token: 0x040004BA RID: 1210
		[FixedBuffer(typeof(ushort), 5)]
		public ImGuiIO.<MouseClickedLastCount>e__FixedBuffer MouseClickedLastCount;

		// Token: 0x040004BB RID: 1211
		[FixedBuffer(typeof(byte), 5)]
		public ImGuiIO.<MouseReleased>e__FixedBuffer MouseReleased;

		// Token: 0x040004BC RID: 1212
		[FixedBuffer(typeof(byte), 5)]
		public ImGuiIO.<MouseDownOwned>e__FixedBuffer MouseDownOwned;

		// Token: 0x040004BD RID: 1213
		[FixedBuffer(typeof(byte), 5)]
		public ImGuiIO.<MouseDownOwnedUnlessPopupClose>e__FixedBuffer MouseDownOwnedUnlessPopupClose;

		// Token: 0x040004BE RID: 1214
		public byte MouseWheelRequestAxisSwap;

		// Token: 0x040004BF RID: 1215
		[FixedBuffer(typeof(float), 5)]
		public ImGuiIO.<MouseDownDuration>e__FixedBuffer MouseDownDuration;

		// Token: 0x040004C0 RID: 1216
		[FixedBuffer(typeof(float), 5)]
		public ImGuiIO.<MouseDownDurationPrev>e__FixedBuffer MouseDownDurationPrev;

		// Token: 0x040004C1 RID: 1217
		public Vector2 MouseDragMaxDistanceAbs_0;

		// Token: 0x040004C2 RID: 1218
		public Vector2 MouseDragMaxDistanceAbs_1;

		// Token: 0x040004C3 RID: 1219
		public Vector2 MouseDragMaxDistanceAbs_2;

		// Token: 0x040004C4 RID: 1220
		public Vector2 MouseDragMaxDistanceAbs_3;

		// Token: 0x040004C5 RID: 1221
		public Vector2 MouseDragMaxDistanceAbs_4;

		// Token: 0x040004C6 RID: 1222
		[FixedBuffer(typeof(float), 5)]
		public ImGuiIO.<MouseDragMaxDistanceSqr>e__FixedBuffer MouseDragMaxDistanceSqr;

		// Token: 0x040004C7 RID: 1223
		public float PenPressure;

		// Token: 0x040004C8 RID: 1224
		public byte AppFocusLost;

		// Token: 0x040004C9 RID: 1225
		public byte AppAcceptingEvents;

		// Token: 0x040004CA RID: 1226
		public sbyte BackendUsingLegacyKeyArrays;

		// Token: 0x040004CB RID: 1227
		public byte BackendUsingLegacyNavInputArray;

		// Token: 0x040004CC RID: 1228
		public ushort InputQueueSurrogate;

		// Token: 0x040004CD RID: 1229
		public ImVector InputQueueCharacters;

		// Token: 0x0200008D RID: 141
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 2608)]
		public struct <KeyMap>e__FixedBuffer
		{
			// Token: 0x040007ED RID: 2029
			public int FixedElementField;
		}

		// Token: 0x0200008E RID: 142
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 652)]
		public struct <KeysDown>e__FixedBuffer
		{
			// Token: 0x040007EE RID: 2030
			public byte FixedElementField;
		}

		// Token: 0x0200008F RID: 143
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 5)]
		public struct <MouseClicked>e__FixedBuffer
		{
			// Token: 0x040007EF RID: 2031
			public byte FixedElementField;
		}

		// Token: 0x02000090 RID: 144
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 10)]
		public struct <MouseClickedCount>e__FixedBuffer
		{
			// Token: 0x040007F0 RID: 2032
			public ushort FixedElementField;
		}

		// Token: 0x02000091 RID: 145
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 10)]
		public struct <MouseClickedLastCount>e__FixedBuffer
		{
			// Token: 0x040007F1 RID: 2033
			public ushort FixedElementField;
		}

		// Token: 0x02000092 RID: 146
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 40)]
		public struct <MouseClickedTime>e__FixedBuffer
		{
			// Token: 0x040007F2 RID: 2034
			public double FixedElementField;
		}

		// Token: 0x02000093 RID: 147
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 5)]
		public struct <MouseDoubleClicked>e__FixedBuffer
		{
			// Token: 0x040007F3 RID: 2035
			public byte FixedElementField;
		}

		// Token: 0x02000094 RID: 148
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 5)]
		public struct <MouseDown>e__FixedBuffer
		{
			// Token: 0x040007F4 RID: 2036
			public byte FixedElementField;
		}

		// Token: 0x02000095 RID: 149
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 20)]
		public struct <MouseDownDuration>e__FixedBuffer
		{
			// Token: 0x040007F5 RID: 2037
			public float FixedElementField;
		}

		// Token: 0x02000096 RID: 150
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 20)]
		public struct <MouseDownDurationPrev>e__FixedBuffer
		{
			// Token: 0x040007F6 RID: 2038
			public float FixedElementField;
		}

		// Token: 0x02000097 RID: 151
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 5)]
		public struct <MouseDownOwned>e__FixedBuffer
		{
			// Token: 0x040007F7 RID: 2039
			public byte FixedElementField;
		}

		// Token: 0x02000098 RID: 152
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 5)]
		public struct <MouseDownOwnedUnlessPopupClose>e__FixedBuffer
		{
			// Token: 0x040007F8 RID: 2040
			public byte FixedElementField;
		}

		// Token: 0x02000099 RID: 153
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 20)]
		public struct <MouseDragMaxDistanceSqr>e__FixedBuffer
		{
			// Token: 0x040007F9 RID: 2041
			public float FixedElementField;
		}

		// Token: 0x0200009A RID: 154
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 5)]
		public struct <MouseReleased>e__FixedBuffer
		{
			// Token: 0x040007FA RID: 2042
			public byte FixedElementField;
		}

		// Token: 0x0200009B RID: 155
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 64)]
		public struct <NavInputs>e__FixedBuffer
		{
			// Token: 0x040007FB RID: 2043
			public float FixedElementField;
		}
	}
}
