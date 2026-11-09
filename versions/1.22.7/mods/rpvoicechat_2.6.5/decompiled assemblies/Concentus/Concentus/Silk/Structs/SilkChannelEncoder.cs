using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Silk.Enums;

namespace Concentus.Silk.Structs
{
	// Token: 0x02000048 RID: 72
	internal class SilkChannelEncoder
	{
		// Token: 0x06000147 RID: 327 RVA: 0x00014D4C File Offset: 0x00012F4C
		internal SilkChannelEncoder()
		{
			for (int i = 0; i < 3; i++)
			{
				this.indices_LBRR[i] = new SideInfoIndices();
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00014E50 File Offset: 0x00013050
		internal void Reset()
		{
			Arrays.MemSetInt(this.In_HP_State, 0, 2);
			this.variable_HP_smth1_Q15 = 0;
			this.variable_HP_smth2_Q15 = 0;
			this.sLP.Reset();
			this.sVAD.Reset();
			this.sNSQ.Reset();
			Arrays.MemSetShort(this.prev_NLSFq_Q15, 0, 16);
			this.speech_activity_Q8 = 0;
			this.allow_bandwidth_switch = 0;
			this.LBRRprevLastGainIndex = 0;
			this.prevSignalType = 0;
			this.prevLag = 0;
			this.pitch_LPC_win_length = 0;
			this.max_pitch_lag = 0;
			this.API_fs_Hz = 0;
			this.prev_API_fs_Hz = 0;
			this.maxInternal_fs_Hz = 0;
			this.minInternal_fs_Hz = 0;
			this.desiredInternal_fs_Hz = 0;
			this.fs_kHz = 0;
			this.nb_subfr = 0;
			this.frame_length = 0;
			this.subfr_length = 0;
			this.ltp_mem_length = 0;
			this.la_pitch = 0;
			this.la_shape = 0;
			this.shapeWinLength = 0;
			this.TargetRate_bps = 0;
			this.PacketSize_ms = 0;
			this.PacketLoss_perc = 0;
			this.frameCounter = 0;
			this.Complexity = 0;
			this.nStatesDelayedDecision = 0;
			this.useInterpolatedNLSFs = 0;
			this.shapingLPCOrder = 0;
			this.predictLPCOrder = 0;
			this.pitchEstimationComplexity = 0;
			this.pitchEstimationLPCOrder = 0;
			this.pitchEstimationThreshold_Q16 = 0;
			this.LTPQuantLowComplexity = 0;
			this.mu_LTP_Q9 = 0;
			this.sum_log_gain_Q7 = 0;
			this.NLSF_MSVQ_Survivors = 0;
			this.first_frame_after_reset = 0;
			this.controlled_since_last_payload = 0;
			this.warping_Q16 = 0;
			this.useCBR = 0;
			this.prefillFlag = 0;
			this.pitch_lag_low_bits_iCDF = null;
			this.pitch_contour_iCDF = null;
			this.psNLSF_CB = null;
			Arrays.MemSetInt(this.input_quality_bands_Q15, 0, 4);
			this.input_tilt_Q15 = 0;
			this.SNR_dB_Q7 = 0;
			Arrays.MemSetSbyte(this.VAD_flags, 0, 3);
			this.LBRR_flag = 0;
			Arrays.MemSetInt(this.LBRR_flags, 0, 3);
			this.indices.Reset();
			Arrays.MemSetSbyte(this.pulses, 0, 320);
			Arrays.MemSetShort(this.inputBuf, 0, 322);
			this.inputBufIx = 0;
			this.nFramesPerPacket = 0;
			this.nFramesEncoded = 0;
			this.nChannelsAPI = 0;
			this.nChannelsInternal = 0;
			this.channelNb = 0;
			this.frames_since_onset = 0;
			this.ec_prevSignalType = 0;
			this.ec_prevLagIndex = 0;
			this.resampler_state.Reset();
			this.useDTX = 0;
			this.inDTX = 0;
			this.noSpeechCounter = 0;
			this.useInBandFEC = 0;
			this.LBRR_enabled = 0;
			this.LBRR_GainIncreases = 0;
			for (int i = 0; i < 3; i++)
			{
				this.indices_LBRR[i].Reset();
				Arrays.MemSetSbyte(this.pulses_LBRR[i], 0, 320);
			}
			this.sShape.Reset();
			this.sPrefilt.Reset();
			Arrays.MemSetShort(this.x_buf, 0, 720);
			this.LTPCorr_Q15 = 0;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00015114 File Offset: 0x00013314
		internal int silk_control_encoder(EncControlState encControl, int TargetRate_bps, int allow_bw_switch, int channelNb, int force_fs_kHz)
		{
			int result = SilkError.SILK_NO_ERROR;
			this.useDTX = encControl.useDTX;
			this.useCBR = encControl.useCBR;
			this.API_fs_Hz = encControl.API_sampleRate;
			this.maxInternal_fs_Hz = encControl.maxInternalSampleRate;
			this.minInternal_fs_Hz = encControl.minInternalSampleRate;
			this.desiredInternal_fs_Hz = encControl.desiredInternalSampleRate;
			this.useInBandFEC = encControl.useInBandFEC;
			this.nChannelsAPI = encControl.nChannelsAPI;
			this.nChannelsInternal = encControl.nChannelsInternal;
			this.allow_bandwidth_switch = allow_bw_switch;
			this.channelNb = channelNb;
			if (this.controlled_since_last_payload != 0 && this.prefillFlag == 0)
			{
				if (this.API_fs_Hz != this.prev_API_fs_Hz && this.fs_kHz > 0)
				{
					result = this.silk_setup_resamplers(this.fs_kHz);
				}
				return result;
			}
			int num = this.silk_control_audio_bandwidth(encControl);
			if (force_fs_kHz != 0)
			{
				num = force_fs_kHz;
			}
			result = this.silk_setup_resamplers(num);
			result = this.silk_setup_fs(num, encControl.payloadSize_ms);
			result = this.silk_setup_complexity(encControl.complexity);
			this.PacketLoss_perc = encControl.packetLossPercentage;
			result = this.silk_setup_LBRR(TargetRate_bps);
			this.controlled_since_last_payload = 1;
			return result;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00015228 File Offset: 0x00013428
		private int silk_setup_resamplers(int fs_kHz)
		{
			int num = 0;
			if (this.fs_kHz != fs_kHz || this.prev_API_fs_Hz != this.API_fs_Hz)
			{
				if (this.fs_kHz == 0)
				{
					num += Resampler.silk_resampler_init(this.resampler_state, this.API_fs_Hz, fs_kHz * 1000, 1);
				}
				else
				{
					int num2 = Inlines.silk_LSHIFT(this.nb_subfr * 5, 1) + 5;
					int inLen = num2 * this.fs_kHz;
					SilkResamplerState s = new SilkResamplerState();
					num += Resampler.silk_resampler_init(s, Inlines.silk_SMULBB(this.fs_kHz, 1000), this.API_fs_Hz, 0);
					int num3 = num2 * Inlines.silk_DIV32_16(this.API_fs_Hz, 1000);
					short[] array = new short[num3];
					num += Resampler.silk_resampler(s, array, 0, this.x_buf, 0, inLen);
					num += Resampler.silk_resampler_init(this.resampler_state, this.API_fs_Hz, Inlines.silk_SMULBB(fs_kHz, 1000), 1);
					num += Resampler.silk_resampler(this.resampler_state, this.x_buf, 0, array, 0, num3);
				}
			}
			this.prev_API_fs_Hz = this.API_fs_Hz;
			return num;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00015348 File Offset: 0x00013548
		private int silk_setup_fs(int fs_kHz, int PacketSize_ms)
		{
			int result = SilkError.SILK_NO_ERROR;
			if (PacketSize_ms != this.PacketSize_ms)
			{
				if (PacketSize_ms != 10 && PacketSize_ms != 20 && PacketSize_ms != 40 && PacketSize_ms != 60)
				{
					result = SilkError.SILK_ENC_PACKET_SIZE_NOT_SUPPORTED;
				}
				if (PacketSize_ms <= 10)
				{
					this.nFramesPerPacket = 1;
					this.nb_subfr = ((PacketSize_ms == 10) ? 2 : 1);
					this.frame_length = Inlines.silk_SMULBB(PacketSize_ms, fs_kHz);
					this.pitch_LPC_win_length = Inlines.silk_SMULBB(14, fs_kHz);
					if (this.fs_kHz == 8)
					{
						this.pitch_contour_iCDF = Concentus.Silk.Tables.silk_pitch_contour_10_ms_NB_iCDF;
					}
					else
					{
						this.pitch_contour_iCDF = Concentus.Silk.Tables.silk_pitch_contour_10_ms_iCDF;
					}
				}
				else
				{
					this.nFramesPerPacket = Inlines.silk_DIV32_16(PacketSize_ms, 20);
					this.nb_subfr = 4;
					this.frame_length = Inlines.silk_SMULBB(20, fs_kHz);
					this.pitch_LPC_win_length = Inlines.silk_SMULBB(24, fs_kHz);
					if (this.fs_kHz == 8)
					{
						this.pitch_contour_iCDF = Concentus.Silk.Tables.silk_pitch_contour_NB_iCDF;
					}
					else
					{
						this.pitch_contour_iCDF = Concentus.Silk.Tables.silk_pitch_contour_iCDF;
					}
				}
				this.PacketSize_ms = PacketSize_ms;
				this.TargetRate_bps = 0;
			}
			if (this.fs_kHz != fs_kHz)
			{
				this.sShape.Reset();
				this.sPrefilt.Reset();
				this.sNSQ.Reset();
				Arrays.MemSetShort(this.prev_NLSFq_Q15, 0, 16);
				Arrays.MemSetInt(this.sLP.In_LP_State, 0, 2);
				this.inputBufIx = 0;
				this.nFramesEncoded = 0;
				this.TargetRate_bps = 0;
				this.prevLag = 100;
				this.first_frame_after_reset = 1;
				this.sPrefilt.lagPrev = 100;
				this.sShape.LastGainIndex = 10;
				this.sNSQ.lagPrev = 100;
				this.sNSQ.prev_gain_Q16 = 65536;
				this.prevSignalType = 0;
				this.fs_kHz = fs_kHz;
				if (this.fs_kHz == 8)
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
				if (this.fs_kHz == 8 || this.fs_kHz == 12)
				{
					this.predictLPCOrder = 10;
					this.psNLSF_CB = Concentus.Silk.Tables.silk_NLSF_CB_NB_MB;
				}
				else
				{
					this.predictLPCOrder = 16;
					this.psNLSF_CB = Concentus.Silk.Tables.silk_NLSF_CB_WB;
				}
				this.subfr_length = 5 * fs_kHz;
				this.frame_length = Inlines.silk_SMULBB(this.subfr_length, this.nb_subfr);
				this.ltp_mem_length = Inlines.silk_SMULBB(20, fs_kHz);
				this.la_pitch = Inlines.silk_SMULBB(2, fs_kHz);
				this.max_pitch_lag = Inlines.silk_SMULBB(18, fs_kHz);
				if (this.nb_subfr == 4)
				{
					this.pitch_LPC_win_length = Inlines.silk_SMULBB(24, fs_kHz);
				}
				else
				{
					this.pitch_LPC_win_length = Inlines.silk_SMULBB(14, fs_kHz);
				}
				if (this.fs_kHz == 16)
				{
					this.mu_LTP_Q9 = 10;
					this.pitch_lag_low_bits_iCDF = Concentus.Silk.Tables.silk_uniform8_iCDF;
				}
				else if (this.fs_kHz == 12)
				{
					this.mu_LTP_Q9 = 13;
					this.pitch_lag_low_bits_iCDF = Concentus.Silk.Tables.silk_uniform6_iCDF;
				}
				else
				{
					this.mu_LTP_Q9 = 15;
					this.pitch_lag_low_bits_iCDF = Concentus.Silk.Tables.silk_uniform4_iCDF;
				}
			}
			return result;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0001563C File Offset: 0x0001383C
		private int silk_setup_complexity(int Complexity)
		{
			int result = 0;
			if (Complexity < 2)
			{
				this.pitchEstimationComplexity = 0;
				this.pitchEstimationThreshold_Q16 = 52429;
				this.pitchEstimationLPCOrder = 6;
				this.shapingLPCOrder = 8;
				this.la_shape = 3 * this.fs_kHz;
				this.nStatesDelayedDecision = 1;
				this.useInterpolatedNLSFs = 0;
				this.LTPQuantLowComplexity = 1;
				this.NLSF_MSVQ_Survivors = 2;
				this.warping_Q16 = 0;
			}
			else if (Complexity < 4)
			{
				this.pitchEstimationComplexity = 1;
				this.pitchEstimationThreshold_Q16 = 49807;
				this.pitchEstimationLPCOrder = 8;
				this.shapingLPCOrder = 10;
				this.la_shape = 5 * this.fs_kHz;
				this.nStatesDelayedDecision = 1;
				this.useInterpolatedNLSFs = 0;
				this.LTPQuantLowComplexity = 0;
				this.NLSF_MSVQ_Survivors = 4;
				this.warping_Q16 = 0;
			}
			else if (Complexity < 6)
			{
				this.pitchEstimationComplexity = 1;
				this.pitchEstimationThreshold_Q16 = 48497;
				this.pitchEstimationLPCOrder = 10;
				this.shapingLPCOrder = 12;
				this.la_shape = 5 * this.fs_kHz;
				this.nStatesDelayedDecision = 2;
				this.useInterpolatedNLSFs = 1;
				this.LTPQuantLowComplexity = 0;
				this.NLSF_MSVQ_Survivors = 8;
				this.warping_Q16 = this.fs_kHz * 983;
			}
			else if (Complexity < 8)
			{
				this.pitchEstimationComplexity = 1;
				this.pitchEstimationThreshold_Q16 = 47186;
				this.pitchEstimationLPCOrder = 12;
				this.shapingLPCOrder = 14;
				this.la_shape = 5 * this.fs_kHz;
				this.nStatesDelayedDecision = 3;
				this.useInterpolatedNLSFs = 1;
				this.LTPQuantLowComplexity = 0;
				this.NLSF_MSVQ_Survivors = 16;
				this.warping_Q16 = this.fs_kHz * 983;
			}
			else
			{
				this.pitchEstimationComplexity = 2;
				this.pitchEstimationThreshold_Q16 = 45875;
				this.pitchEstimationLPCOrder = 16;
				this.shapingLPCOrder = 16;
				this.la_shape = 5 * this.fs_kHz;
				this.nStatesDelayedDecision = 4;
				this.useInterpolatedNLSFs = 1;
				this.LTPQuantLowComplexity = 0;
				this.NLSF_MSVQ_Survivors = 32;
				this.warping_Q16 = this.fs_kHz * 983;
			}
			this.pitchEstimationLPCOrder = Inlines.silk_min_int(this.pitchEstimationLPCOrder, this.predictLPCOrder);
			this.shapeWinLength = 5 * this.fs_kHz + 2 * this.la_shape;
			this.Complexity = Complexity;
			return result;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00015860 File Offset: 0x00013A60
		private int silk_setup_LBRR(int TargetRate_bps)
		{
			int silk_NO_ERROR = SilkError.SILK_NO_ERROR;
			int lbrr_enabled = this.LBRR_enabled;
			this.LBRR_enabled = 0;
			if (this.useInBandFEC != 0 && this.PacketLoss_perc > 0)
			{
				int num;
				if (this.fs_kHz == 8)
				{
					num = 12000;
				}
				else if (this.fs_kHz == 12)
				{
					num = 14000;
				}
				else
				{
					num = 16000;
				}
				num = Inlines.silk_SMULWB(Inlines.silk_MUL(num, 125 - Inlines.silk_min(this.PacketLoss_perc, 25)), 655);
				if (TargetRate_bps > num)
				{
					if (lbrr_enabled == 0)
					{
						this.LBRR_GainIncreases = 7;
					}
					else
					{
						this.LBRR_GainIncreases = Inlines.silk_max_int(7 - Inlines.silk_SMULWB(this.PacketLoss_perc, 26214), 2);
					}
					this.LBRR_enabled = 1;
				}
			}
			return silk_NO_ERROR;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00015914 File Offset: 0x00013B14
		internal int silk_control_audio_bandwidth(EncControlState encControl)
		{
			int num = this.fs_kHz;
			int num2 = Inlines.silk_SMULBB(num, 1000);
			if (num2 == 0)
			{
				num2 = Inlines.silk_min(this.desiredInternal_fs_Hz, this.API_fs_Hz);
				num = Inlines.silk_DIV32_16(num2, 1000);
			}
			else if (num2 > this.API_fs_Hz || num2 > this.maxInternal_fs_Hz || num2 < this.minInternal_fs_Hz)
			{
				num2 = this.API_fs_Hz;
				num2 = Inlines.silk_min(num2, this.maxInternal_fs_Hz);
				num2 = Inlines.silk_max(num2, this.minInternal_fs_Hz);
				num = Inlines.silk_DIV32_16(num2, 1000);
			}
			else
			{
				if (this.sLP.transition_frame_no >= 256)
				{
					this.sLP.mode = 0;
				}
				if (this.allow_bandwidth_switch != 0 || encControl.opusCanSwitch != 0)
				{
					if (Inlines.silk_SMULBB(this.fs_kHz, 1000) > this.desiredInternal_fs_Hz)
					{
						if (this.sLP.mode == 0)
						{
							this.sLP.transition_frame_no = 256;
							Arrays.MemSetInt(this.sLP.In_LP_State, 0, 2);
						}
						if (encControl.opusCanSwitch != 0)
						{
							this.sLP.mode = 0;
							num = ((this.fs_kHz == 16) ? 12 : 8);
						}
						else if (this.sLP.transition_frame_no <= 0)
						{
							encControl.switchReady = 1;
							encControl.maxBits -= encControl.maxBits * 5 / (encControl.payloadSize_ms + 5);
						}
						else
						{
							this.sLP.mode = -2;
						}
					}
					else if (Inlines.silk_SMULBB(this.fs_kHz, 1000) < this.desiredInternal_fs_Hz)
					{
						if (encControl.opusCanSwitch != 0)
						{
							num = ((this.fs_kHz == 8) ? 12 : 16);
							this.sLP.transition_frame_no = 0;
							Arrays.MemSetInt(this.sLP.In_LP_State, 0, 2);
							this.sLP.mode = 1;
						}
						else if (this.sLP.mode == 0)
						{
							encControl.switchReady = 1;
							encControl.maxBits -= encControl.maxBits * 5 / (encControl.payloadSize_ms + 5);
						}
						else
						{
							this.sLP.mode = 1;
						}
					}
					else if (this.sLP.mode < 0)
					{
						this.sLP.mode = 1;
					}
				}
			}
			return num;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00015B54 File Offset: 0x00013D54
		internal int silk_control_SNR(int TargetRate_bps)
		{
			int silk_NO_ERROR = SilkError.SILK_NO_ERROR;
			TargetRate_bps = Inlines.silk_LIMIT(TargetRate_bps, 5000, 80000);
			if (TargetRate_bps != this.TargetRate_bps)
			{
				this.TargetRate_bps = TargetRate_bps;
				int[] array;
				if (this.fs_kHz == 8)
				{
					array = Concentus.Silk.Tables.silk_TargetRate_table_NB;
				}
				else if (this.fs_kHz == 12)
				{
					array = Concentus.Silk.Tables.silk_TargetRate_table_MB;
				}
				else
				{
					array = Concentus.Silk.Tables.silk_TargetRate_table_WB;
				}
				if (this.nb_subfr == 2)
				{
					TargetRate_bps -= 2200;
				}
				for (int i = 1; i < 8; i++)
				{
					if (TargetRate_bps <= array[i])
					{
						int a = Inlines.silk_DIV32(Inlines.silk_LSHIFT(TargetRate_bps - array[i - 1], 6), array[i] - array[i - 1]);
						this.SNR_dB_Q7 = Inlines.silk_LSHIFT((int)Concentus.Silk.Tables.silk_SNR_table_Q1[i - 1], 6) + Inlines.silk_MUL(a, (int)(Concentus.Silk.Tables.silk_SNR_table_Q1[i] - Concentus.Silk.Tables.silk_SNR_table_Q1[i - 1]));
						break;
					}
				}
			}
			return silk_NO_ERROR;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00015C28 File Offset: 0x00013E28
		internal void silk_encode_do_VAD()
		{
			VoiceActivityDetection.silk_VAD_GetSA_Q8(this, this.inputBuf, 1);
			if (this.speech_activity_Q8 < 13)
			{
				this.indices.signalType = 0;
				this.noSpeechCounter++;
				if (this.noSpeechCounter < 10)
				{
					this.inDTX = 0;
				}
				else if (this.noSpeechCounter > 30)
				{
					this.noSpeechCounter = 10;
					this.inDTX = 0;
				}
				this.VAD_flags[this.nFramesEncoded] = 0;
				return;
			}
			this.noSpeechCounter = 0;
			this.inDTX = 0;
			this.indices.signalType = 1;
			this.VAD_flags[this.nFramesEncoded] = 1;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00015CD0 File Offset: 0x00013ED0
		internal int silk_encode_frame(BoxedValueInt pnBytesOut, EntropyCoder psRangeEnc, Span<byte> encodedDataOut, int condCoding, int maxBits, int useCBR)
		{
			SilkEncoderControl silkEncoderControl = new SilkEncoderControl();
			int result = 0;
			EntropyCoder entropyCoder = new EntropyCoder();
			EntropyCoder entropyCoder2 = new EntropyCoder();
			SilkNSQState silkNSQState = new SilkNSQState();
			SilkNSQState silkNSQState2 = new SilkNSQState();
			sbyte lastGainIndex = 0;
			int num4;
			int num3;
			int num2;
			int num = num2 = (num3 = (num4 = 0));
			SideInfoIndices sideInfoIndices = this.indices;
			int num5 = this.frameCounter;
			this.frameCounter = num5 + 1;
			sideInfoIndices.Seed = (sbyte)(num5 & 3);
			int num6 = this.ltp_mem_length;
			this.sLP.silk_LP_variable_cutoff(this.inputBuf, 1, this.frame_length);
			Arrays.MemCopy(this.inputBuf, 1, this.x_buf, num6 + 5 * this.fs_kHz, this.frame_length);
			if (this.prefillFlag == 0)
			{
				short[] array = new short[this.la_pitch + this.frame_length + this.ltp_mem_length];
				int pitch_res_ptr = this.ltp_mem_length;
				FindPitchLags.silk_find_pitch_lags(this, silkEncoderControl, array, this.x_buf, num6);
				NoiseShapeAnalysis.silk_noise_shape_analysis(this, silkEncoderControl, array, pitch_res_ptr, this.x_buf, num6);
				FindPredCoefs.silk_find_pred_coefs(this, silkEncoderControl, array, this.x_buf, num6, condCoding);
				ProcessGains.silk_process_gains(this, silkEncoderControl, condCoding);
				int[] array2 = new int[this.frame_length];
				Filters.silk_prefilter(this, silkEncoderControl, array2, this.x_buf, num6);
				this.silk_LBRR_encode(silkEncoderControl, array2, condCoding);
				int num7 = 6;
				short num8 = 256;
				int num9 = 0;
				int num10 = 0;
				int num11 = GainQuantization.silk_gains_ID(this.indices.GainsIndices, this.nb_subfr);
				int num12 = -1;
				int num13 = -1;
				entropyCoder.Assign(psRangeEnc);
				silkNSQState.Assign(this.sNSQ);
				sbyte seed = this.indices.Seed;
				short num14 = this.ec_prevLagIndex;
				int num15 = this.ec_prevSignalType;
				byte[] array3 = new byte[1275];
				int num16 = 0;
				int num17;
				for (;;)
				{
					if (num11 == num12)
					{
						num17 = num2;
					}
					else if (num11 == num13)
					{
						num17 = num;
					}
					else
					{
						if (num16 > 0)
						{
							psRangeEnc.Assign(entropyCoder);
							this.sNSQ.Assign(silkNSQState);
							this.indices.Seed = seed;
							this.ec_prevLagIndex = num14;
							this.ec_prevSignalType = num15;
						}
						if (this.nStatesDelayedDecision > 1 || this.warping_Q16 > 0)
						{
							this.sNSQ.silk_NSQ_del_dec(this, this.indices, array2, this.pulses, silkEncoderControl.PredCoef_Q12, silkEncoderControl.LTPCoef_Q14, silkEncoderControl.AR2_Q13, silkEncoderControl.HarmShapeGain_Q14, silkEncoderControl.Tilt_Q14, silkEncoderControl.LF_shp_Q14, silkEncoderControl.Gains_Q16, silkEncoderControl.pitchL, silkEncoderControl.Lambda_Q10, silkEncoderControl.LTP_scale_Q14);
						}
						else
						{
							this.sNSQ.silk_NSQ(this, this.indices, array2, this.pulses, silkEncoderControl.PredCoef_Q12, silkEncoderControl.LTPCoef_Q14, silkEncoderControl.AR2_Q13, silkEncoderControl.HarmShapeGain_Q14, silkEncoderControl.Tilt_Q14, silkEncoderControl.LF_shp_Q14, silkEncoderControl.Gains_Q16, silkEncoderControl.pitchL, silkEncoderControl.Lambda_Q10, silkEncoderControl.LTP_scale_Q14);
						}
						EncodeIndices.silk_encode_indices(this, psRangeEnc, encodedDataOut, this.nFramesEncoded, 0, condCoding);
						EncodePulses.silk_encode_pulses(psRangeEnc, encodedDataOut, (int)this.indices.signalType, (int)this.indices.quantOffsetType, this.pulses, this.frame_length);
						num17 = psRangeEnc.tell();
						if (useCBR == 0 && num16 == 0 && num17 <= maxBits)
						{
							goto IL_561;
						}
					}
					if (num16 == num7)
					{
						break;
					}
					if (num17 > maxBits)
					{
						if (num9 == 0 && num16 >= 2)
						{
							silkEncoderControl.Lambda_Q10 = Inlines.silk_ADD_RSHIFT32(silkEncoderControl.Lambda_Q10, silkEncoderControl.Lambda_Q10, 1);
							num10 = 0;
							num13 = -1;
						}
						else
						{
							num10 = 1;
							num = num17;
							num4 = (int)num8;
							num13 = num11;
						}
					}
					else
					{
						if (num17 >= maxBits - 5)
						{
							goto IL_561;
						}
						num9 = 1;
						num2 = num17;
						num3 = (int)num8;
						if (num11 != num12)
						{
							num12 = num11;
							entropyCoder2.Assign(psRangeEnc);
							encodedDataOut.Slice(0, (int)psRangeEnc.offs).CopyTo(array3);
							silkNSQState2.Assign(this.sNSQ);
							lastGainIndex = this.sShape.LastGainIndex;
						}
					}
					if ((num9 & num10) == 0)
					{
						int num18 = Inlines.silk_log2lin(Inlines.silk_LSHIFT(num17 - maxBits, 7) / this.frame_length + 2048);
						num18 = Inlines.silk_min_32(num18, 131072);
						if (num17 > maxBits)
						{
							num18 = Inlines.silk_max_32(num18, 85197);
						}
						num8 = (short)Inlines.silk_SMULWB(num18, (int)num8);
					}
					else
					{
						num8 = (short)(num3 + Inlines.silk_DIV32_16(Inlines.silk_MUL(num4 - num3, maxBits - num2), num - num2));
						if ((int)num8 > Inlines.silk_ADD_RSHIFT32(num3, num4 - num3, 2))
						{
							num8 = (short)Inlines.silk_ADD_RSHIFT32(num3, num4 - num3, 2);
						}
						else if ((int)num8 < Inlines.silk_SUB_RSHIFT32(num4, num4 - num3, 2))
						{
							num8 = (short)Inlines.silk_SUB_RSHIFT32(num4, num4 - num3, 2);
						}
					}
					for (int i = 0; i < this.nb_subfr; i++)
					{
						silkEncoderControl.Gains_Q16[i] = Inlines.silk_LSHIFT_SAT32(Inlines.silk_SMULWB(silkEncoderControl.GainsUnq_Q16[i], (int)num8), 8);
					}
					this.sShape.LastGainIndex = silkEncoderControl.lastGainIndexPrev;
					BoxedValueSbyte boxedValueSbyte = new BoxedValueSbyte(this.sShape.LastGainIndex);
					GainQuantization.silk_gains_quant(this.indices.GainsIndices, silkEncoderControl.Gains_Q16, boxedValueSbyte, (condCoding == 2) ? 1 : 0, this.nb_subfr);
					this.sShape.LastGainIndex = boxedValueSbyte.Val;
					num11 = GainQuantization.silk_gains_ID(this.indices.GainsIndices, this.nb_subfr);
					num16++;
				}
				if (num9 != 0 && (num11 == num12 || num17 > maxBits))
				{
					psRangeEnc.Assign(entropyCoder2);
					MemoryExtensions.AsSpan<byte>(array3, 0, (int)entropyCoder2.offs).CopyTo(encodedDataOut);
					this.sNSQ.Assign(silkNSQState2);
					this.sShape.LastGainIndex = lastGainIndex;
				}
			}
			IL_561:
			Arrays.MemMoveShort(this.x_buf, this.frame_length, 0, this.ltp_mem_length + 5 * this.fs_kHz);
			if (this.prefillFlag != 0)
			{
				pnBytesOut.Val = 0;
				return result;
			}
			this.prevLag = silkEncoderControl.pitchL[this.nb_subfr - 1];
			this.prevSignalType = this.indices.signalType;
			this.first_frame_after_reset = 0;
			pnBytesOut.Val = Inlines.silk_RSHIFT(psRangeEnc.tell() + 7, 3);
			return result;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000162B4 File Offset: 0x000144B4
		internal void silk_LBRR_encode(SilkEncoderControl thisCtrl, int[] xfw_Q3, int condCoding)
		{
			int[] array = new int[this.nb_subfr];
			SideInfoIndices sideInfoIndices = this.indices_LBRR[this.nFramesEncoded];
			SilkNSQState silkNSQState = new SilkNSQState();
			if (this.LBRR_enabled != 0 && this.speech_activity_Q8 > 77)
			{
				this.LBRR_flags[this.nFramesEncoded] = 1;
				silkNSQState.Assign(this.sNSQ);
				sideInfoIndices.Assign(this.indices);
				Arrays.MemCopy(thisCtrl.Gains_Q16, 0, array, 0, this.nb_subfr);
				if (this.nFramesEncoded == 0 || this.LBRR_flags[this.nFramesEncoded - 1] == 0)
				{
					this.LBRRprevLastGainIndex = this.sShape.LastGainIndex;
					sideInfoIndices.GainsIndices[0] = (sbyte)((int)sideInfoIndices.GainsIndices[0] + this.LBRR_GainIncreases);
					sideInfoIndices.GainsIndices[0] = (sbyte)Inlines.silk_min_int((int)sideInfoIndices.GainsIndices[0], 63);
				}
				BoxedValueSbyte boxedValueSbyte = new BoxedValueSbyte(this.LBRRprevLastGainIndex);
				GainQuantization.silk_gains_dequant(thisCtrl.Gains_Q16, sideInfoIndices.GainsIndices, boxedValueSbyte, (condCoding == 2) ? 1 : 0, this.nb_subfr);
				this.LBRRprevLastGainIndex = boxedValueSbyte.Val;
				if (this.nStatesDelayedDecision > 1 || this.warping_Q16 > 0)
				{
					silkNSQState.silk_NSQ_del_dec(this, sideInfoIndices, xfw_Q3, this.pulses_LBRR[this.nFramesEncoded], thisCtrl.PredCoef_Q12, thisCtrl.LTPCoef_Q14, thisCtrl.AR2_Q13, thisCtrl.HarmShapeGain_Q14, thisCtrl.Tilt_Q14, thisCtrl.LF_shp_Q14, thisCtrl.Gains_Q16, thisCtrl.pitchL, thisCtrl.Lambda_Q10, thisCtrl.LTP_scale_Q14);
				}
				else
				{
					silkNSQState.silk_NSQ(this, sideInfoIndices, xfw_Q3, this.pulses_LBRR[this.nFramesEncoded], thisCtrl.PredCoef_Q12, thisCtrl.LTPCoef_Q14, thisCtrl.AR2_Q13, thisCtrl.HarmShapeGain_Q14, thisCtrl.Tilt_Q14, thisCtrl.LF_shp_Q14, thisCtrl.Gains_Q16, thisCtrl.pitchL, thisCtrl.Lambda_Q10, thisCtrl.LTP_scale_Q14);
				}
				Arrays.MemCopy(array, 0, thisCtrl.Gains_Q16, 0, this.nb_subfr);
			}
		}

		// Token: 0x040001E4 RID: 484
		internal readonly int[] In_HP_State = new int[2];

		// Token: 0x040001E5 RID: 485
		internal int variable_HP_smth1_Q15;

		// Token: 0x040001E6 RID: 486
		internal int variable_HP_smth2_Q15;

		// Token: 0x040001E7 RID: 487
		internal readonly SilkLPState sLP = new SilkLPState();

		// Token: 0x040001E8 RID: 488
		internal readonly SilkVADState sVAD = new SilkVADState();

		// Token: 0x040001E9 RID: 489
		internal readonly SilkNSQState sNSQ = new SilkNSQState();

		// Token: 0x040001EA RID: 490
		internal readonly short[] prev_NLSFq_Q15 = new short[16];

		// Token: 0x040001EB RID: 491
		internal int speech_activity_Q8;

		// Token: 0x040001EC RID: 492
		internal int allow_bandwidth_switch;

		// Token: 0x040001ED RID: 493
		internal sbyte LBRRprevLastGainIndex;

		// Token: 0x040001EE RID: 494
		internal sbyte prevSignalType;

		// Token: 0x040001EF RID: 495
		internal int prevLag;

		// Token: 0x040001F0 RID: 496
		internal int pitch_LPC_win_length;

		// Token: 0x040001F1 RID: 497
		internal int max_pitch_lag;

		// Token: 0x040001F2 RID: 498
		internal int API_fs_Hz;

		// Token: 0x040001F3 RID: 499
		internal int prev_API_fs_Hz;

		// Token: 0x040001F4 RID: 500
		internal int maxInternal_fs_Hz;

		// Token: 0x040001F5 RID: 501
		internal int minInternal_fs_Hz;

		// Token: 0x040001F6 RID: 502
		internal int desiredInternal_fs_Hz;

		// Token: 0x040001F7 RID: 503
		internal int fs_kHz;

		// Token: 0x040001F8 RID: 504
		internal int nb_subfr;

		// Token: 0x040001F9 RID: 505
		internal int frame_length;

		// Token: 0x040001FA RID: 506
		internal int subfr_length;

		// Token: 0x040001FB RID: 507
		internal int ltp_mem_length;

		// Token: 0x040001FC RID: 508
		internal int la_pitch;

		// Token: 0x040001FD RID: 509
		internal int la_shape;

		// Token: 0x040001FE RID: 510
		internal int shapeWinLength;

		// Token: 0x040001FF RID: 511
		internal int TargetRate_bps;

		// Token: 0x04000200 RID: 512
		internal int PacketSize_ms;

		// Token: 0x04000201 RID: 513
		internal int PacketLoss_perc;

		// Token: 0x04000202 RID: 514
		internal int frameCounter;

		// Token: 0x04000203 RID: 515
		internal int Complexity;

		// Token: 0x04000204 RID: 516
		internal int nStatesDelayedDecision;

		// Token: 0x04000205 RID: 517
		internal int useInterpolatedNLSFs;

		// Token: 0x04000206 RID: 518
		internal int shapingLPCOrder;

		// Token: 0x04000207 RID: 519
		internal int predictLPCOrder;

		// Token: 0x04000208 RID: 520
		internal int pitchEstimationComplexity;

		// Token: 0x04000209 RID: 521
		internal int pitchEstimationLPCOrder;

		// Token: 0x0400020A RID: 522
		internal int pitchEstimationThreshold_Q16;

		// Token: 0x0400020B RID: 523
		internal int LTPQuantLowComplexity;

		// Token: 0x0400020C RID: 524
		internal int mu_LTP_Q9;

		// Token: 0x0400020D RID: 525
		internal int sum_log_gain_Q7;

		// Token: 0x0400020E RID: 526
		internal int NLSF_MSVQ_Survivors;

		// Token: 0x0400020F RID: 527
		internal int first_frame_after_reset;

		// Token: 0x04000210 RID: 528
		internal int controlled_since_last_payload;

		// Token: 0x04000211 RID: 529
		internal int warping_Q16;

		// Token: 0x04000212 RID: 530
		internal int useCBR;

		// Token: 0x04000213 RID: 531
		internal int prefillFlag;

		// Token: 0x04000214 RID: 532
		internal byte[] pitch_lag_low_bits_iCDF;

		// Token: 0x04000215 RID: 533
		internal byte[] pitch_contour_iCDF;

		// Token: 0x04000216 RID: 534
		internal NLSFCodebook psNLSF_CB;

		// Token: 0x04000217 RID: 535
		internal readonly int[] input_quality_bands_Q15 = new int[4];

		// Token: 0x04000218 RID: 536
		internal int input_tilt_Q15;

		// Token: 0x04000219 RID: 537
		internal int SNR_dB_Q7;

		// Token: 0x0400021A RID: 538
		internal readonly sbyte[] VAD_flags = new sbyte[3];

		// Token: 0x0400021B RID: 539
		internal sbyte LBRR_flag;

		// Token: 0x0400021C RID: 540
		internal readonly int[] LBRR_flags = new int[3];

		// Token: 0x0400021D RID: 541
		internal readonly SideInfoIndices indices = new SideInfoIndices();

		// Token: 0x0400021E RID: 542
		internal readonly sbyte[] pulses = new sbyte[320];

		// Token: 0x0400021F RID: 543
		internal readonly short[] inputBuf = new short[322];

		// Token: 0x04000220 RID: 544
		internal int inputBufIx;

		// Token: 0x04000221 RID: 545
		internal int nFramesPerPacket;

		// Token: 0x04000222 RID: 546
		internal int nFramesEncoded;

		// Token: 0x04000223 RID: 547
		internal int nChannelsAPI;

		// Token: 0x04000224 RID: 548
		internal int nChannelsInternal;

		// Token: 0x04000225 RID: 549
		internal int channelNb;

		// Token: 0x04000226 RID: 550
		internal int frames_since_onset;

		// Token: 0x04000227 RID: 551
		internal int ec_prevSignalType;

		// Token: 0x04000228 RID: 552
		internal short ec_prevLagIndex;

		// Token: 0x04000229 RID: 553
		internal readonly SilkResamplerState resampler_state = new SilkResamplerState();

		// Token: 0x0400022A RID: 554
		internal int useDTX;

		// Token: 0x0400022B RID: 555
		internal int inDTX;

		// Token: 0x0400022C RID: 556
		internal int noSpeechCounter;

		// Token: 0x0400022D RID: 557
		internal int useInBandFEC;

		// Token: 0x0400022E RID: 558
		internal int LBRR_enabled;

		// Token: 0x0400022F RID: 559
		internal int LBRR_GainIncreases;

		// Token: 0x04000230 RID: 560
		internal readonly SideInfoIndices[] indices_LBRR = new SideInfoIndices[3];

		// Token: 0x04000231 RID: 561
		internal readonly sbyte[][] pulses_LBRR = Arrays.InitTwoDimensionalArray<sbyte>(3, 320);

		// Token: 0x04000232 RID: 562
		internal readonly SilkShapeState sShape = new SilkShapeState();

		// Token: 0x04000233 RID: 563
		internal readonly SilkPrefilterState sPrefilt = new SilkPrefilterState();

		// Token: 0x04000234 RID: 564
		internal readonly short[] x_buf = new short[720];

		// Token: 0x04000235 RID: 565
		internal int LTPCorr_Q15;
	}
}
