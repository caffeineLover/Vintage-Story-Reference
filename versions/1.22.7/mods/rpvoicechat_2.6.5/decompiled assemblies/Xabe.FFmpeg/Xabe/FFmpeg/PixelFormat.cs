using System;

namespace Xabe.FFmpeg
{
	// Token: 0x0200000E RID: 14
	public enum PixelFormat
	{
		// Token: 0x040001A5 RID: 421
		yuv420p,
		// Token: 0x040001A6 RID: 422
		yuyv422,
		// Token: 0x040001A7 RID: 423
		rgb24,
		// Token: 0x040001A8 RID: 424
		bgr24,
		// Token: 0x040001A9 RID: 425
		yuv422p,
		// Token: 0x040001AA RID: 426
		yuv444p,
		// Token: 0x040001AB RID: 427
		yuv410p,
		// Token: 0x040001AC RID: 428
		yuv411p,
		// Token: 0x040001AD RID: 429
		gray,
		// Token: 0x040001AE RID: 430
		monow,
		// Token: 0x040001AF RID: 431
		monob,
		// Token: 0x040001B0 RID: 432
		pal8,
		// Token: 0x040001B1 RID: 433
		yuvj420p,
		// Token: 0x040001B2 RID: 434
		yuvj422p,
		// Token: 0x040001B3 RID: 435
		yuvj444p,
		// Token: 0x040001B4 RID: 436
		uyvy422,
		// Token: 0x040001B5 RID: 437
		uyyvyy411,
		// Token: 0x040001B6 RID: 438
		bgr8,
		// Token: 0x040001B7 RID: 439
		bgr4,
		// Token: 0x040001B8 RID: 440
		bgr4_byte,
		// Token: 0x040001B9 RID: 441
		rgb8,
		// Token: 0x040001BA RID: 442
		rgb4,
		// Token: 0x040001BB RID: 443
		rgb4_byte,
		// Token: 0x040001BC RID: 444
		nv12,
		// Token: 0x040001BD RID: 445
		nv21,
		// Token: 0x040001BE RID: 446
		argb,
		// Token: 0x040001BF RID: 447
		rgba,
		// Token: 0x040001C0 RID: 448
		abgr,
		// Token: 0x040001C1 RID: 449
		bgra,
		// Token: 0x040001C2 RID: 450
		gray16be,
		// Token: 0x040001C3 RID: 451
		gray16le,
		// Token: 0x040001C4 RID: 452
		yuv440p,
		// Token: 0x040001C5 RID: 453
		yuvj440p,
		// Token: 0x040001C6 RID: 454
		yuva420p,
		// Token: 0x040001C7 RID: 455
		rgb48be,
		// Token: 0x040001C8 RID: 456
		rgb48le,
		// Token: 0x040001C9 RID: 457
		rgb565be,
		// Token: 0x040001CA RID: 458
		rgb565le,
		// Token: 0x040001CB RID: 459
		rgb555be,
		// Token: 0x040001CC RID: 460
		rgb555le,
		// Token: 0x040001CD RID: 461
		bgr565be,
		// Token: 0x040001CE RID: 462
		bgr565le,
		// Token: 0x040001CF RID: 463
		bgr555be,
		// Token: 0x040001D0 RID: 464
		bgr555le,
		// Token: 0x040001D1 RID: 465
		vaapi_moco,
		// Token: 0x040001D2 RID: 466
		vaapi_idct,
		// Token: 0x040001D3 RID: 467
		vaapi_vld,
		// Token: 0x040001D4 RID: 468
		yuv420p16le,
		// Token: 0x040001D5 RID: 469
		yuv420p16be,
		// Token: 0x040001D6 RID: 470
		yuv422p16le,
		// Token: 0x040001D7 RID: 471
		yuv422p16be,
		// Token: 0x040001D8 RID: 472
		yuv444p16le,
		// Token: 0x040001D9 RID: 473
		yuv444p16be,
		// Token: 0x040001DA RID: 474
		dxva2_vld,
		// Token: 0x040001DB RID: 475
		rgb444le,
		// Token: 0x040001DC RID: 476
		rgb444be,
		// Token: 0x040001DD RID: 477
		bgr444le,
		// Token: 0x040001DE RID: 478
		bgr444be,
		// Token: 0x040001DF RID: 479
		ya8,
		// Token: 0x040001E0 RID: 480
		bgr48be,
		// Token: 0x040001E1 RID: 481
		bgr48le,
		// Token: 0x040001E2 RID: 482
		yuv420p9be,
		// Token: 0x040001E3 RID: 483
		yuv420p9le,
		// Token: 0x040001E4 RID: 484
		yuv420p10be,
		// Token: 0x040001E5 RID: 485
		yuv420p10le,
		// Token: 0x040001E6 RID: 486
		yuv422p10be,
		// Token: 0x040001E7 RID: 487
		yuv422p10le,
		// Token: 0x040001E8 RID: 488
		yuv444p9be,
		// Token: 0x040001E9 RID: 489
		yuv444p9le,
		// Token: 0x040001EA RID: 490
		yuv444p10be,
		// Token: 0x040001EB RID: 491
		yuv444p10le,
		// Token: 0x040001EC RID: 492
		yuv422p9be,
		// Token: 0x040001ED RID: 493
		yuv422p9le,
		// Token: 0x040001EE RID: 494
		gbrp,
		// Token: 0x040001EF RID: 495
		gbrp9be,
		// Token: 0x040001F0 RID: 496
		gbrp9le,
		// Token: 0x040001F1 RID: 497
		gbrp10be,
		// Token: 0x040001F2 RID: 498
		gbrp10le,
		// Token: 0x040001F3 RID: 499
		gbrp16be,
		// Token: 0x040001F4 RID: 500
		gbrp16le,
		// Token: 0x040001F5 RID: 501
		yuva422p,
		// Token: 0x040001F6 RID: 502
		yuva444p,
		// Token: 0x040001F7 RID: 503
		yuva420p9be,
		// Token: 0x040001F8 RID: 504
		yuva420p9le,
		// Token: 0x040001F9 RID: 505
		yuva422p9be,
		// Token: 0x040001FA RID: 506
		yuva422p9le,
		// Token: 0x040001FB RID: 507
		yuva444p9be,
		// Token: 0x040001FC RID: 508
		yuva444p9le,
		// Token: 0x040001FD RID: 509
		yuva420p10be,
		// Token: 0x040001FE RID: 510
		yuva420p10le,
		// Token: 0x040001FF RID: 511
		yuva422p10be,
		// Token: 0x04000200 RID: 512
		yuva422p10le,
		// Token: 0x04000201 RID: 513
		yuva444p10be,
		// Token: 0x04000202 RID: 514
		yuva444p10le,
		// Token: 0x04000203 RID: 515
		yuva420p16be,
		// Token: 0x04000204 RID: 516
		yuva420p16le,
		// Token: 0x04000205 RID: 517
		yuva422p16be,
		// Token: 0x04000206 RID: 518
		yuva422p16le,
		// Token: 0x04000207 RID: 519
		yuva444p16be,
		// Token: 0x04000208 RID: 520
		yuva444p16le,
		// Token: 0x04000209 RID: 521
		vdpau,
		// Token: 0x0400020A RID: 522
		xyz12le,
		// Token: 0x0400020B RID: 523
		xyz12be,
		// Token: 0x0400020C RID: 524
		nv16,
		// Token: 0x0400020D RID: 525
		nv20le,
		// Token: 0x0400020E RID: 526
		nv20be,
		// Token: 0x0400020F RID: 527
		rgba64be,
		// Token: 0x04000210 RID: 528
		rgba64le,
		// Token: 0x04000211 RID: 529
		bgra64be,
		// Token: 0x04000212 RID: 530
		bgra64le,
		// Token: 0x04000213 RID: 531
		yvyu422,
		// Token: 0x04000214 RID: 532
		ya16be,
		// Token: 0x04000215 RID: 533
		ya16le,
		// Token: 0x04000216 RID: 534
		gbrap,
		// Token: 0x04000217 RID: 535
		gbrap16be,
		// Token: 0x04000218 RID: 536
		gbrap16le,
		// Token: 0x04000219 RID: 537
		qsv,
		// Token: 0x0400021A RID: 538
		mmal,
		// Token: 0x0400021B RID: 539
		d3d11va_vld,
		// Token: 0x0400021C RID: 540
		cuda,
		// Token: 0x0400021D RID: 541
		_0rgb,
		// Token: 0x0400021E RID: 542
		rgb0,
		// Token: 0x0400021F RID: 543
		_0bgr,
		// Token: 0x04000220 RID: 544
		bgr0,
		// Token: 0x04000221 RID: 545
		yuv420p12be,
		// Token: 0x04000222 RID: 546
		yuv420p12le,
		// Token: 0x04000223 RID: 547
		yuv420p14be,
		// Token: 0x04000224 RID: 548
		yuv420p14le,
		// Token: 0x04000225 RID: 549
		yuv422p12be,
		// Token: 0x04000226 RID: 550
		yuv422p12le,
		// Token: 0x04000227 RID: 551
		yuv422p14be,
		// Token: 0x04000228 RID: 552
		yuv422p14le,
		// Token: 0x04000229 RID: 553
		yuv444p12be,
		// Token: 0x0400022A RID: 554
		yuv444p12le,
		// Token: 0x0400022B RID: 555
		yuv444p14be,
		// Token: 0x0400022C RID: 556
		yuv444p14le,
		// Token: 0x0400022D RID: 557
		gbrp12be,
		// Token: 0x0400022E RID: 558
		gbrp12le,
		// Token: 0x0400022F RID: 559
		gbrp14be,
		// Token: 0x04000230 RID: 560
		gbrp14le,
		// Token: 0x04000231 RID: 561
		yuvj411p,
		// Token: 0x04000232 RID: 562
		bayer_bggr8,
		// Token: 0x04000233 RID: 563
		bayer_rggb8,
		// Token: 0x04000234 RID: 564
		bayer_gbrg8,
		// Token: 0x04000235 RID: 565
		bayer_grbg8,
		// Token: 0x04000236 RID: 566
		bayer_bggr16le,
		// Token: 0x04000237 RID: 567
		bayer_bggr16be,
		// Token: 0x04000238 RID: 568
		bayer_rggb16le,
		// Token: 0x04000239 RID: 569
		bayer_rggb16be,
		// Token: 0x0400023A RID: 570
		bayer_gbrg16le,
		// Token: 0x0400023B RID: 571
		bayer_gbrg16be,
		// Token: 0x0400023C RID: 572
		bayer_grbg16le,
		// Token: 0x0400023D RID: 573
		bayer_grbg16be,
		// Token: 0x0400023E RID: 574
		xvmc,
		// Token: 0x0400023F RID: 575
		yuv440p10le,
		// Token: 0x04000240 RID: 576
		yuv440p10be,
		// Token: 0x04000241 RID: 577
		yuv440p12le,
		// Token: 0x04000242 RID: 578
		yuv440p12be,
		// Token: 0x04000243 RID: 579
		ayuv64le,
		// Token: 0x04000244 RID: 580
		ayuv64be,
		// Token: 0x04000245 RID: 581
		videotoolbox_vld,
		// Token: 0x04000246 RID: 582
		p010le,
		// Token: 0x04000247 RID: 583
		p010be,
		// Token: 0x04000248 RID: 584
		gbrap12be,
		// Token: 0x04000249 RID: 585
		gbrap12le,
		// Token: 0x0400024A RID: 586
		gbrap10be,
		// Token: 0x0400024B RID: 587
		gbrap10le,
		// Token: 0x0400024C RID: 588
		mediacodec,
		// Token: 0x0400024D RID: 589
		gray12be,
		// Token: 0x0400024E RID: 590
		gray12le,
		// Token: 0x0400024F RID: 591
		gray10be,
		// Token: 0x04000250 RID: 592
		gray10le,
		// Token: 0x04000251 RID: 593
		p016le,
		// Token: 0x04000252 RID: 594
		p016be,
		// Token: 0x04000253 RID: 595
		d3d11,
		// Token: 0x04000254 RID: 596
		gray9be,
		// Token: 0x04000255 RID: 597
		gray9le,
		// Token: 0x04000256 RID: 598
		gbrpf32be,
		// Token: 0x04000257 RID: 599
		gbrpf32le,
		// Token: 0x04000258 RID: 600
		gbrapf32be,
		// Token: 0x04000259 RID: 601
		gbrapf32le,
		// Token: 0x0400025A RID: 602
		drm_prime,
		// Token: 0x0400025B RID: 603
		opencl,
		// Token: 0x0400025C RID: 604
		gray14be,
		// Token: 0x0400025D RID: 605
		gray14le,
		// Token: 0x0400025E RID: 606
		grayf32be,
		// Token: 0x0400025F RID: 607
		grayf32le,
		// Token: 0x04000260 RID: 608
		yuva422p12be,
		// Token: 0x04000261 RID: 609
		yuva422p12le,
		// Token: 0x04000262 RID: 610
		yuva444p12be,
		// Token: 0x04000263 RID: 611
		yuva444p12le,
		// Token: 0x04000264 RID: 612
		nv24,
		// Token: 0x04000265 RID: 613
		nv42
	}
}
