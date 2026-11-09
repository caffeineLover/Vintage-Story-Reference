using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk.Structs
{
	// Token: 0x02000047 RID: 71
	internal class SilkChannelDecoder
	{
		// Token: 0x06000140 RID: 320 RVA: 0x000147D0 File Offset: 0x000129D0
		internal void Reset()
		{
			this.prev_gain_Q16 = 0;
			Arrays.MemSetInt(this.exc_Q14, 0, 320);
			Arrays.MemSetInt(this.sLPC_Q14_buf, 0, 16);
			Arrays.MemSetShort(this.outBuf, 0, 480);
			this.lagPrev = 0;
			this.LastGainIndex = 0;
			this.fs_kHz = 0;
			this.fs_API_hz = 0;
			this.nb_subfr = 0;
			this.frame_length = 0;
			this.subfr_length = 0;
			this.ltp_mem_length = 0;
			this.LPC_order = 0;
			Arrays.MemSetShort(this.prevNLSF_Q15, 0, 16);
			this.first_frame_after_reset = 0;
			this.pitch_lag_low_bits_iCDF = null;
			this.pitch_contour_iCDF = null;
			this.nFramesDecoded = 0;
			this.nFramesPerPacket = 0;
			this.ec_prevSignalType = 0;
			this.ec_prevLagIndex = 0;
			Arrays.MemSetInt(this.VAD_flags, 0, 3);
			this.LBRR_flag = 0;
			Arrays.MemSetInt(this.LBRR_flags, 0, 3);
			this.resampler_state.Reset();
			this.psNLSF_CB = null;
			this.indices.Reset();
			this.sCNG.Reset();
			this.lossCnt = 0;
			this.prevSignalType = 0;
			this.sPLC.Reset();
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000148F4 File Offset: 0x00012AF4
		internal int silk_init_decoder()
		{
			this.Reset();
			this.first_frame_after_reset = 1;
			this.prev_gain_Q16 = 65536;
			this.silk_CNG_Reset();
			this.silk_PLC_Reset();
			return 0;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0001491C File Offset: 0x00012B1C
		private void silk_CNG_Reset()
		{
			int num = Inlines.silk_DIV32_16(32767, this.LPC_order + 1);
			int num2 = 0;
			for (int i = 0; i < this.LPC_order; i++)
			{
				num2 += num;
				this.sCNG.CNG_smth_NLSF_Q15[i] = (short)num2;
			}
			this.sCNG.CNG_smth_Gain_Q16 = 0;
			this.sCNG.rand_seed = 3176576;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00014980 File Offset: 0x00012B80
		private void silk_PLC_Reset()
		{
			this.sPLC.pitchL_Q8 = Inlines.silk_LSHIFT(this.frame_length, 7);
			this.sPLC.prevGain_Q16[0] = 65536;
			this.sPLC.prevGain_Q16[1] = 65536;
			this.sPLC.subfr_length = 20;
			this.sPLC.nb_subfr = 2;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000149E4 File Offset: 0x00012BE4
		internal int silk_decoder_set_fs(int fs_kHz, int fs_API_Hz)
		{
			int num = 0;
			this.subfr_length = Inlines.silk_SMULBB(5, fs_kHz);
			int num2 = Inlines.silk_SMULBB(this.nb_subfr, this.subfr_length);
			if (this.fs_kHz != fs_kHz || this.fs_API_hz != fs_API_Hz)
			{
				num += Resampler.silk_resampler_init(this.resampler_state, Inlines.silk_SMULBB(fs_kHz, 1000), fs_API_Hz, 0);
				this.fs_API_hz = fs_API_Hz;
			}
			if (this.fs_kHz != fs_kHz || num2 != this.frame_length)
			{
				if (fs_kHz == 8)
				{
					if (this.nb_subfr == 4)
					{
						this.pitch_contour_iCDF = Concentus.Silk.Tables.silk_pitch_contour_NB_iCDF;
					}
					else
					{
						this.pitch_contour_iCDF = Concentus.Silk.Tables.silk_pitch_contour_10_ms_NB_iCDF;
					}
				}
				else if (this.nb_subfr == 4)
				{
					this.pitch_contour_iCDF = Concentus.Silk.Tables.silk_pitch_contour_iCDF;
				}
				else
				{
					this.pitch_contour_iCDF = Concentus.Silk.Tables.silk_pitch_contour_10_ms_iCDF;
				}
				if (this.fs_kHz != fs_kHz)
				{
					this.ltp_mem_length = Inlines.silk_SMULBB(20, fs_kHz);
					if (fs_kHz == 8 || fs_kHz == 12)
					{
						this.LPC_order = 10;
						this.psNLSF_CB = Concentus.Silk.Tables.silk_NLSF_CB_NB_MB;
					}
					else
					{
						this.LPC_order = 16;
						this.psNLSF_CB = Concentus.Silk.Tables.silk_NLSF_CB_WB;
					}
					if (fs_kHz == 16)
					{
						this.pitch_lag_low_bits_iCDF = Concentus.Silk.Tables.silk_uniform8_iCDF;
					}
					else if (fs_kHz == 12)
					{
						this.pitch_lag_low_bits_iCDF = Concentus.Silk.Tables.silk_uniform6_iCDF;
					}
					else if (fs_kHz == 8)
					{
						this.pitch_lag_low_bits_iCDF = Concentus.Silk.Tables.silk_uniform4_iCDF;
					}
					this.first_frame_after_reset = 1;
					this.lagPrev = 100;
					this.LastGainIndex = 10;
					this.prevSignalType = 0;
					Arrays.MemSetShort(this.outBuf, 0, 480);
					Arrays.MemSetInt(this.sLPC_Q14_buf, 0, 16);
				}
				this.fs_kHz = fs_kHz;
				this.frame_length = num2;
			}
			return num;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00014B70 File Offset: 0x00012D70
		internal int silk_decode_frame(EntropyCoder psRangeDec, ReadOnlySpan<byte> frameData, Span<short> pOut, int pOut_ptr, BoxedValueInt pN, int lostFlag, int condCoding)
		{
			SilkDecoderControl silkDecoderControl = new SilkDecoderControl();
			int result = 0;
			int num = this.frame_length;
			silkDecoderControl.LTP_scale_Q14 = 0;
			if (lostFlag == 0 || (lostFlag == 2 && this.LBRR_flags[this.nFramesDecoded] == 1))
			{
				short[] pulses = new short[num + 16 - 1 & -16];
				DecodeIndices.silk_decode_indices(this, psRangeDec, frameData, this.nFramesDecoded, lostFlag, condCoding);
				DecodePulses.silk_decode_pulses(psRangeDec, frameData, pulses, (int)this.indices.signalType, (int)this.indices.quantOffsetType, this.frame_length);
				DecodeParameters.silk_decode_parameters(this, silkDecoderControl, condCoding);
				DecodeCore.silk_decode_core(this, silkDecoderControl, pOut, pOut_ptr, pulses);
				PLC.silk_PLC(this, silkDecoderControl, pOut, pOut_ptr, 0);
				this.lossCnt = 0;
				this.prevSignalType = (int)this.indices.signalType;
				this.first_frame_after_reset = 0;
			}
			else
			{
				PLC.silk_PLC(this, silkDecoderControl, pOut, pOut_ptr, 1);
			}
			int num2 = this.ltp_mem_length - this.frame_length;
			Arrays.MemMoveShort(this.outBuf, this.frame_length, 0, num2);
			pOut.Slice(pOut_ptr, this.frame_length).CopyTo(MemoryExtensions.AsSpan<short>(this.outBuf, num2));
			CNG.silk_CNG(this, silkDecoderControl, pOut, pOut_ptr, num);
			PLC.silk_PLC_glue_frames(this, pOut, pOut_ptr, num);
			this.lagPrev = silkDecoderControl.pitchL[this.nb_subfr - 1];
			pN.Val = num;
			return result;
		}

		// Token: 0x040001C5 RID: 453
		internal int prev_gain_Q16;

		// Token: 0x040001C6 RID: 454
		internal readonly int[] exc_Q14 = new int[320];

		// Token: 0x040001C7 RID: 455
		internal readonly int[] sLPC_Q14_buf = new int[16];

		// Token: 0x040001C8 RID: 456
		internal readonly short[] outBuf = new short[480];

		// Token: 0x040001C9 RID: 457
		internal int lagPrev;

		// Token: 0x040001CA RID: 458
		internal sbyte LastGainIndex;

		// Token: 0x040001CB RID: 459
		internal int fs_kHz;

		// Token: 0x040001CC RID: 460
		internal int fs_API_hz;

		// Token: 0x040001CD RID: 461
		internal int nb_subfr;

		// Token: 0x040001CE RID: 462
		internal int frame_length;

		// Token: 0x040001CF RID: 463
		internal int subfr_length;

		// Token: 0x040001D0 RID: 464
		internal int ltp_mem_length;

		// Token: 0x040001D1 RID: 465
		internal int LPC_order;

		// Token: 0x040001D2 RID: 466
		internal readonly short[] prevNLSF_Q15 = new short[16];

		// Token: 0x040001D3 RID: 467
		internal int first_frame_after_reset;

		// Token: 0x040001D4 RID: 468
		internal byte[] pitch_lag_low_bits_iCDF;

		// Token: 0x040001D5 RID: 469
		internal byte[] pitch_contour_iCDF;

		// Token: 0x040001D6 RID: 470
		internal int nFramesDecoded;

		// Token: 0x040001D7 RID: 471
		internal int nFramesPerPacket;

		// Token: 0x040001D8 RID: 472
		internal int ec_prevSignalType;

		// Token: 0x040001D9 RID: 473
		internal short ec_prevLagIndex;

		// Token: 0x040001DA RID: 474
		internal readonly int[] VAD_flags = new int[3];

		// Token: 0x040001DB RID: 475
		internal int LBRR_flag;

		// Token: 0x040001DC RID: 476
		internal readonly int[] LBRR_flags = new int[3];

		// Token: 0x040001DD RID: 477
		internal readonly SilkResamplerState resampler_state = new SilkResamplerState();

		// Token: 0x040001DE RID: 478
		internal NLSFCodebook psNLSF_CB;

		// Token: 0x040001DF RID: 479
		internal readonly SideInfoIndices indices = new SideInfoIndices();

		// Token: 0x040001E0 RID: 480
		internal readonly CNGState sCNG = new CNGState();

		// Token: 0x040001E1 RID: 481
		internal int lossCnt;

		// Token: 0x040001E2 RID: 482
		internal int prevSignalType;

		// Token: 0x040001E3 RID: 483
		internal readonly PLCStruct sPLC = new PLCStruct();
	}
}
