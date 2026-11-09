using System;

namespace Xabe.FFmpeg
{
	// Token: 0x0200002B RID: 43
	public enum AudioCodec
	{
		// Token: 0x040002A6 RID: 678
		_4gv,
		// Token: 0x040002A7 RID: 679
		_8svx_exp,
		// Token: 0x040002A8 RID: 680
		_8svx_fib,
		// Token: 0x040002A9 RID: 681
		aac,
		// Token: 0x040002AA RID: 682
		aac_latm,
		// Token: 0x040002AB RID: 683
		ac3,
		// Token: 0x040002AC RID: 684
		adpcm_4xm,
		// Token: 0x040002AD RID: 685
		adpcm_adx,
		// Token: 0x040002AE RID: 686
		adpcm_afc,
		// Token: 0x040002AF RID: 687
		adpcm_agm,
		// Token: 0x040002B0 RID: 688
		adpcm_aica,
		// Token: 0x040002B1 RID: 689
		adpcm_ct,
		// Token: 0x040002B2 RID: 690
		adpcm_dtk,
		// Token: 0x040002B3 RID: 691
		adpcm_ea,
		// Token: 0x040002B4 RID: 692
		adpcm_ea_maxis_xa,
		// Token: 0x040002B5 RID: 693
		adpcm_ea_r1,
		// Token: 0x040002B6 RID: 694
		adpcm_ea_r2,
		// Token: 0x040002B7 RID: 695
		adpcm_ea_r3,
		// Token: 0x040002B8 RID: 696
		adpcm_ea_xas,
		// Token: 0x040002B9 RID: 697
		adpcm_g722,
		// Token: 0x040002BA RID: 698
		adpcm_g726,
		// Token: 0x040002BB RID: 699
		adpcm_g726le,
		// Token: 0x040002BC RID: 700
		adpcm_ima_amv,
		// Token: 0x040002BD RID: 701
		adpcm_ima_apc,
		// Token: 0x040002BE RID: 702
		adpcm_ima_dat4,
		// Token: 0x040002BF RID: 703
		adpcm_ima_dk3,
		// Token: 0x040002C0 RID: 704
		adpcm_ima_dk4,
		// Token: 0x040002C1 RID: 705
		adpcm_ima_ea_eacs,
		// Token: 0x040002C2 RID: 706
		adpcm_ima_ea_sead,
		// Token: 0x040002C3 RID: 707
		adpcm_ima_iss,
		// Token: 0x040002C4 RID: 708
		adpcm_ima_oki,
		// Token: 0x040002C5 RID: 709
		adpcm_ima_qt,
		// Token: 0x040002C6 RID: 710
		adpcm_ima_rad,
		// Token: 0x040002C7 RID: 711
		adpcm_ima_smjpeg,
		// Token: 0x040002C8 RID: 712
		adpcm_ima_wav,
		// Token: 0x040002C9 RID: 713
		adpcm_ima_ws,
		// Token: 0x040002CA RID: 714
		adpcm_ms,
		// Token: 0x040002CB RID: 715
		adpcm_mtaf,
		// Token: 0x040002CC RID: 716
		adpcm_psx,
		// Token: 0x040002CD RID: 717
		adpcm_sbpro_2,
		// Token: 0x040002CE RID: 718
		adpcm_sbpro_3,
		// Token: 0x040002CF RID: 719
		adpcm_sbpro_4,
		// Token: 0x040002D0 RID: 720
		adpcm_swf,
		// Token: 0x040002D1 RID: 721
		adpcm_thp,
		// Token: 0x040002D2 RID: 722
		adpcm_thp_le,
		// Token: 0x040002D3 RID: 723
		adpcm_vima,
		// Token: 0x040002D4 RID: 724
		adpcm_xa,
		// Token: 0x040002D5 RID: 725
		adpcm_yamaha,
		// Token: 0x040002D6 RID: 726
		alac,
		// Token: 0x040002D7 RID: 727
		amr_nb,
		// Token: 0x040002D8 RID: 728
		amr_wb,
		// Token: 0x040002D9 RID: 729
		ape,
		// Token: 0x040002DA RID: 730
		aptx,
		// Token: 0x040002DB RID: 731
		aptx_hd,
		// Token: 0x040002DC RID: 732
		atrac1,
		// Token: 0x040002DD RID: 733
		atrac3,
		// Token: 0x040002DE RID: 734
		atrac3al,
		// Token: 0x040002DF RID: 735
		atrac3p,
		// Token: 0x040002E0 RID: 736
		atrac3pal,
		// Token: 0x040002E1 RID: 737
		atrac9,
		// Token: 0x040002E2 RID: 738
		avc,
		// Token: 0x040002E3 RID: 739
		binkaudio_dct,
		// Token: 0x040002E4 RID: 740
		binkaudio_rdft,
		// Token: 0x040002E5 RID: 741
		bmv_audio,
		// Token: 0x040002E6 RID: 742
		celt,
		// Token: 0x040002E7 RID: 743
		codec2,
		// Token: 0x040002E8 RID: 744
		comfortnoise,
		// Token: 0x040002E9 RID: 745
		cook,
		// Token: 0x040002EA RID: 746
		dolby_e,
		// Token: 0x040002EB RID: 747
		dsd_lsbf,
		// Token: 0x040002EC RID: 748
		dsd_lsbf_planar,
		// Token: 0x040002ED RID: 749
		dsd_msbf,
		// Token: 0x040002EE RID: 750
		dsd_msbf_planar,
		// Token: 0x040002EF RID: 751
		dsicinaudio,
		// Token: 0x040002F0 RID: 752
		dss_sp,
		// Token: 0x040002F1 RID: 753
		dst,
		// Token: 0x040002F2 RID: 754
		dts,
		// Token: 0x040002F3 RID: 755
		dvaudio,
		// Token: 0x040002F4 RID: 756
		eac3,
		// Token: 0x040002F5 RID: 757
		evrc,
		// Token: 0x040002F6 RID: 758
		flac,
		// Token: 0x040002F7 RID: 759
		g723_1,
		// Token: 0x040002F8 RID: 760
		g729,
		// Token: 0x040002F9 RID: 761
		gremlin_dpcm,
		// Token: 0x040002FA RID: 762
		gsm,
		// Token: 0x040002FB RID: 763
		gsm_ms,
		// Token: 0x040002FC RID: 764
		hcom,
		// Token: 0x040002FD RID: 765
		iac,
		// Token: 0x040002FE RID: 766
		ilbc,
		// Token: 0x040002FF RID: 767
		imc,
		// Token: 0x04000300 RID: 768
		interplay_dpcm,
		// Token: 0x04000301 RID: 769
		interplayacm,
		// Token: 0x04000302 RID: 770
		mace3,
		// Token: 0x04000303 RID: 771
		mace6,
		// Token: 0x04000304 RID: 772
		metasound,
		// Token: 0x04000305 RID: 773
		mlp,
		// Token: 0x04000306 RID: 774
		mp1,
		// Token: 0x04000307 RID: 775
		mp2,
		// Token: 0x04000308 RID: 776
		mp3,
		// Token: 0x04000309 RID: 777
		mp3adu,
		// Token: 0x0400030A RID: 778
		mp3on4,
		// Token: 0x0400030B RID: 779
		mp4als,
		// Token: 0x0400030C RID: 780
		musepack7,
		// Token: 0x0400030D RID: 781
		musepack8,
		// Token: 0x0400030E RID: 782
		nellymoser,
		// Token: 0x0400030F RID: 783
		opus,
		// Token: 0x04000310 RID: 784
		paf_audio,
		// Token: 0x04000311 RID: 785
		pcm_alaw,
		// Token: 0x04000312 RID: 786
		pcm_bluray,
		// Token: 0x04000313 RID: 787
		pcm_dvd,
		// Token: 0x04000314 RID: 788
		pcm_f16le,
		// Token: 0x04000315 RID: 789
		pcm_f24le,
		// Token: 0x04000316 RID: 790
		pcm_f32be,
		// Token: 0x04000317 RID: 791
		pcm_f32le,
		// Token: 0x04000318 RID: 792
		pcm_f64be,
		// Token: 0x04000319 RID: 793
		pcm_f64le,
		// Token: 0x0400031A RID: 794
		pcm_lxf,
		// Token: 0x0400031B RID: 795
		pcm_mulaw,
		// Token: 0x0400031C RID: 796
		pcm_s16be,
		// Token: 0x0400031D RID: 797
		pcm_s16be_planar,
		// Token: 0x0400031E RID: 798
		pcm_s16le,
		// Token: 0x0400031F RID: 799
		pcm_s16le_planar,
		// Token: 0x04000320 RID: 800
		pcm_s24be,
		// Token: 0x04000321 RID: 801
		pcm_s24daud,
		// Token: 0x04000322 RID: 802
		pcm_s24le,
		// Token: 0x04000323 RID: 803
		pcm_s24le_planar,
		// Token: 0x04000324 RID: 804
		pcm_s32be,
		// Token: 0x04000325 RID: 805
		pcm_s32le,
		// Token: 0x04000326 RID: 806
		pcm_s32le_planar,
		// Token: 0x04000327 RID: 807
		pcm_s64be,
		// Token: 0x04000328 RID: 808
		pcm_s64le,
		// Token: 0x04000329 RID: 809
		pcm_s8,
		// Token: 0x0400032A RID: 810
		pcm_s8_planar,
		// Token: 0x0400032B RID: 811
		pcm_u16be,
		// Token: 0x0400032C RID: 812
		pcm_u16le,
		// Token: 0x0400032D RID: 813
		pcm_u24be,
		// Token: 0x0400032E RID: 814
		pcm_u24le,
		// Token: 0x0400032F RID: 815
		pcm_u32be,
		// Token: 0x04000330 RID: 816
		pcm_u32le,
		// Token: 0x04000331 RID: 817
		pcm_u8,
		// Token: 0x04000332 RID: 818
		pcm_vidc,
		// Token: 0x04000333 RID: 819
		pcm_zork,
		// Token: 0x04000334 RID: 820
		qcelp,
		// Token: 0x04000335 RID: 821
		qdm2,
		// Token: 0x04000336 RID: 822
		qdmc,
		// Token: 0x04000337 RID: 823
		ra_144,
		// Token: 0x04000338 RID: 824
		ra_288,
		// Token: 0x04000339 RID: 825
		ralf,
		// Token: 0x0400033A RID: 826
		roq_dpcm,
		// Token: 0x0400033B RID: 827
		s302m,
		// Token: 0x0400033C RID: 828
		sbc,
		// Token: 0x0400033D RID: 829
		sdx2_dpcm,
		// Token: 0x0400033E RID: 830
		shorten,
		// Token: 0x0400033F RID: 831
		sipr,
		// Token: 0x04000340 RID: 832
		smackaudio,
		// Token: 0x04000341 RID: 833
		smv,
		// Token: 0x04000342 RID: 834
		sol_dpcm,
		// Token: 0x04000343 RID: 835
		sonic,
		// Token: 0x04000344 RID: 836
		sonicls,
		// Token: 0x04000345 RID: 837
		speex,
		// Token: 0x04000346 RID: 838
		tak,
		// Token: 0x04000347 RID: 839
		truehd,
		// Token: 0x04000348 RID: 840
		truespeech,
		// Token: 0x04000349 RID: 841
		tta,
		// Token: 0x0400034A RID: 842
		twinvq,
		// Token: 0x0400034B RID: 843
		vmdaudio,
		// Token: 0x0400034C RID: 844
		vorbis,
		// Token: 0x0400034D RID: 845
		wavesynth,
		// Token: 0x0400034E RID: 846
		wavpack,
		// Token: 0x0400034F RID: 847
		westwood_snd1,
		// Token: 0x04000350 RID: 848
		wmalossless,
		// Token: 0x04000351 RID: 849
		wmapro,
		// Token: 0x04000352 RID: 850
		wmav1,
		// Token: 0x04000353 RID: 851
		wmav2,
		// Token: 0x04000354 RID: 852
		wmavoice,
		// Token: 0x04000355 RID: 853
		xan_dpcm,
		// Token: 0x04000356 RID: 854
		xma1,
		// Token: 0x04000357 RID: 855
		xma2,
		// Token: 0x04000358 RID: 856
		libvorbis,
		// Token: 0x04000359 RID: 857
		copy,
		// Token: 0x0400035A RID: 858
		libopus
	}
}
