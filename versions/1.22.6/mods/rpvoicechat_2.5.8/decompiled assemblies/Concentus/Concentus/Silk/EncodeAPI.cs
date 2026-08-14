using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Silk.Enums;
using Concentus.Silk.Structs;

namespace Concentus.Silk
{
	// Token: 0x0200001E RID: 30
	internal static class EncodeAPI
	{
		// Token: 0x060000BC RID: 188 RVA: 0x00007E08 File Offset: 0x00006008
		internal static int silk_InitEncoder(SilkEncoder encState, EncControlState encStatus)
		{
			int num = SilkError.SILK_NO_ERROR;
			encState.Reset();
			for (int i = 0; i < 2; i++)
			{
				num += SilkEncoder.silk_init_encoder(encState.state_Fxx[i]);
			}
			encState.nChannelsAPI = 1;
			encState.nChannelsInternal = 1;
			return num + EncodeAPI.silk_QueryEncoder(encState, encStatus);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00007E58 File Offset: 0x00006058
		internal static int silk_QueryEncoder(SilkEncoder encState, EncControlState encStatus)
		{
			int silk_NO_ERROR = SilkError.SILK_NO_ERROR;
			SilkChannelEncoder silkChannelEncoder = encState.state_Fxx[0];
			encStatus.Reset();
			encStatus.nChannelsAPI = encState.nChannelsAPI;
			encStatus.nChannelsInternal = encState.nChannelsInternal;
			encStatus.API_sampleRate = silkChannelEncoder.API_fs_Hz;
			encStatus.maxInternalSampleRate = silkChannelEncoder.maxInternal_fs_Hz;
			encStatus.minInternalSampleRate = silkChannelEncoder.minInternal_fs_Hz;
			encStatus.desiredInternalSampleRate = silkChannelEncoder.desiredInternal_fs_Hz;
			encStatus.payloadSize_ms = silkChannelEncoder.PacketSize_ms;
			encStatus.bitRate = silkChannelEncoder.TargetRate_bps;
			encStatus.packetLossPercentage = silkChannelEncoder.PacketLoss_perc;
			encStatus.complexity = silkChannelEncoder.Complexity;
			encStatus.useInBandFEC = silkChannelEncoder.useInBandFEC;
			encStatus.useDTX = silkChannelEncoder.useDTX;
			encStatus.useCBR = silkChannelEncoder.useCBR;
			encStatus.internalSampleRate = Inlines.silk_SMULBB(silkChannelEncoder.fs_kHz, 1000);
			encStatus.allowBandwidthSwitch = silkChannelEncoder.allow_bandwidth_switch;
			encStatus.inWBmodeWithoutVariableLP = ((silkChannelEncoder.fs_kHz == 16 && silkChannelEncoder.sLP.mode == 0) ? 1 : 0);
			return silk_NO_ERROR;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00007F58 File Offset: 0x00006158
		internal static int silk_Encode(SilkEncoder psEnc, EncControlState encControl, short[] samplesIn, int nSamplesIn, EntropyCoder psRangeEnc, Span<byte> encodedDataOut, BoxedValueInt nBytesOut, int prefillFlag)
		{
			int num = SilkError.SILK_NO_ERROR;
			int payloadSize_ms = 0;
			int complexity = 0;
			int[] array = new int[2];
			nBytesOut.Val = 0;
			if (encControl.reducedDependency != 0)
			{
				psEnc.state_Fxx[0].first_frame_after_reset = 1;
				psEnc.state_Fxx[1].first_frame_after_reset = 1;
			}
			psEnc.state_Fxx[0].nFramesEncoded = (psEnc.state_Fxx[1].nFramesEncoded = 0);
			num += encControl.check_control_input();
			if (num != SilkError.SILK_NO_ERROR)
			{
				return num;
			}
			encControl.switchReady = 0;
			if (encControl.nChannelsInternal > psEnc.nChannelsInternal)
			{
				num += SilkEncoder.silk_init_encoder(psEnc.state_Fxx[1]);
				Arrays.MemSetShort(psEnc.sStereo.pred_prev_Q13, 0, 2);
				Arrays.MemSetShort(psEnc.sStereo.sSide, 0, 2);
				psEnc.sStereo.mid_side_amp_Q0[0] = 0;
				psEnc.sStereo.mid_side_amp_Q0[1] = 1;
				psEnc.sStereo.mid_side_amp_Q0[2] = 0;
				psEnc.sStereo.mid_side_amp_Q0[3] = 1;
				psEnc.sStereo.width_prev_Q14 = 0;
				psEnc.sStereo.smth_width_Q14 = 16384;
				if (psEnc.nChannelsAPI == 2)
				{
					psEnc.state_Fxx[1].resampler_state.Assign(psEnc.state_Fxx[0].resampler_state);
					Arrays.MemCopy(psEnc.state_Fxx[0].In_HP_State, 0, psEnc.state_Fxx[1].In_HP_State, 0, 2);
				}
			}
			int num2 = (encControl.payloadSize_ms != psEnc.state_Fxx[0].PacketSize_ms || psEnc.nChannelsInternal != encControl.nChannelsInternal) ? 1 : 0;
			psEnc.nChannelsAPI = encControl.nChannelsAPI;
			psEnc.nChannelsInternal = encControl.nChannelsInternal;
			int num3 = Inlines.silk_DIV32(100 * nSamplesIn, encControl.API_sampleRate);
			int num4 = (num3 > 1) ? (num3 >> 1) : 1;
			int num5 = 0;
			if (prefillFlag != 0)
			{
				if (num3 != 1)
				{
					return SilkError.SILK_ENC_INPUT_INVALID_NO_OF_SAMPLES;
				}
				for (int i = 0; i < encControl.nChannelsInternal; i++)
				{
					num += SilkEncoder.silk_init_encoder(psEnc.state_Fxx[i]);
				}
				payloadSize_ms = encControl.payloadSize_ms;
				encControl.payloadSize_ms = 10;
				complexity = encControl.complexity;
				encControl.complexity = 0;
				for (int i = 0; i < encControl.nChannelsInternal; i++)
				{
					psEnc.state_Fxx[i].controlled_since_last_payload = 0;
					psEnc.state_Fxx[i].prefillFlag = 1;
				}
			}
			else
			{
				if (num3 * encControl.API_sampleRate != 100 * nSamplesIn || nSamplesIn < 0)
				{
					return SilkError.SILK_ENC_INPUT_INVALID_NO_OF_SAMPLES;
				}
				if (1000 * nSamplesIn > encControl.payloadSize_ms * encControl.API_sampleRate)
				{
					return SilkError.SILK_ENC_INPUT_INVALID_NO_OF_SAMPLES;
				}
			}
			int num6 = Inlines.silk_RSHIFT32(encControl.bitRate, encControl.nChannelsInternal - 1);
			for (int i = 0; i < encControl.nChannelsInternal; i++)
			{
				int force_fs_kHz = (i == 1) ? psEnc.state_Fxx[0].fs_kHz : 0;
				num += psEnc.state_Fxx[i].silk_control_encoder(encControl, num6, psEnc.allowBandwidthSwitch, i, force_fs_kHz);
				if (num != SilkError.SILK_NO_ERROR)
				{
					return num;
				}
				if (psEnc.state_Fxx[i].first_frame_after_reset != 0 || num2 != 0)
				{
					for (int j = 0; j < psEnc.state_Fxx[0].nFramesPerPacket; j++)
					{
						psEnc.state_Fxx[i].LBRR_flags[j] = 0;
					}
				}
				psEnc.state_Fxx[i].inDTX = psEnc.state_Fxx[i].useDTX;
			}
			int num7 = 10 * num3 * psEnc.state_Fxx[0].fs_kHz;
			short[] array2 = new short[Inlines.silk_DIV32_16(num7 * psEnc.state_Fxx[0].API_fs_Hz, (int)((short)(psEnc.state_Fxx[0].fs_kHz * 1000)))];
			int num8 = 0;
			for (;;)
			{
				int num9 = psEnc.state_Fxx[0].frame_length - psEnc.state_Fxx[0].inputBufIx;
				num9 = Inlines.silk_min(num9, num7);
				int num10 = Inlines.silk_DIV32_16(num9 * psEnc.state_Fxx[0].API_fs_Hz, psEnc.state_Fxx[0].fs_kHz * 1000);
				if (encControl.nChannelsAPI == 2 && encControl.nChannelsInternal == 2)
				{
					int nFramesEncoded = psEnc.state_Fxx[0].nFramesEncoded;
					for (int i = 0; i < num10; i++)
					{
						array2[i] = samplesIn[num8 + 2 * i];
					}
					if (psEnc.nPrevChannelsInternal == 1 && nFramesEncoded == 0)
					{
						psEnc.state_Fxx[1].resampler_state.Assign(psEnc.state_Fxx[0].resampler_state);
					}
					num += Resampler.silk_resampler(psEnc.state_Fxx[0].resampler_state, psEnc.state_Fxx[0].inputBuf, psEnc.state_Fxx[0].inputBufIx + 2, array2, 0, num10);
					psEnc.state_Fxx[0].inputBufIx += num9;
					num9 = psEnc.state_Fxx[1].frame_length - psEnc.state_Fxx[1].inputBufIx;
					num9 = Inlines.silk_min(num9, 10 * num3 * psEnc.state_Fxx[1].fs_kHz);
					for (int i = 0; i < num10; i++)
					{
						array2[i] = samplesIn[num8 + 2 * i + 1];
					}
					num += Resampler.silk_resampler(psEnc.state_Fxx[1].resampler_state, psEnc.state_Fxx[1].inputBuf, psEnc.state_Fxx[1].inputBufIx + 2, array2, 0, num10);
					psEnc.state_Fxx[1].inputBufIx += num9;
				}
				else if (encControl.nChannelsAPI == 2 && encControl.nChannelsInternal == 1)
				{
					for (int i = 0; i < num10; i++)
					{
						int a = (int)(samplesIn[num8 + 2 * i] + samplesIn[num8 + 2 * i + 1]);
						array2[i] = (short)Inlines.silk_RSHIFT_ROUND(a, 1);
					}
					num += Resampler.silk_resampler(psEnc.state_Fxx[0].resampler_state, psEnc.state_Fxx[0].inputBuf, psEnc.state_Fxx[0].inputBufIx + 2, array2, 0, num10);
					if (psEnc.nPrevChannelsInternal == 2 && psEnc.state_Fxx[0].nFramesEncoded == 0)
					{
						num += Resampler.silk_resampler(psEnc.state_Fxx[1].resampler_state, psEnc.state_Fxx[1].inputBuf, psEnc.state_Fxx[1].inputBufIx + 2, array2, 0, num10);
						for (int i = 0; i < psEnc.state_Fxx[0].frame_length; i++)
						{
							psEnc.state_Fxx[0].inputBuf[psEnc.state_Fxx[0].inputBufIx + i + 2] = (short)Inlines.silk_RSHIFT((int)(psEnc.state_Fxx[0].inputBuf[psEnc.state_Fxx[0].inputBufIx + i + 2] + psEnc.state_Fxx[1].inputBuf[psEnc.state_Fxx[1].inputBufIx + i + 2]), 1);
						}
					}
					psEnc.state_Fxx[0].inputBufIx += num9;
				}
				else
				{
					Arrays.MemCopy(samplesIn, num8, array2, 0, num10);
					num += Resampler.silk_resampler(psEnc.state_Fxx[0].resampler_state, psEnc.state_Fxx[0].inputBuf, psEnc.state_Fxx[0].inputBufIx + 2, array2, 0, num10);
					psEnc.state_Fxx[0].inputBufIx += num9;
				}
				num8 += num10 * encControl.nChannelsAPI;
				nSamplesIn -= num10;
				psEnc.allowBandwidthSwitch = 0;
				if (psEnc.state_Fxx[0].inputBufIx < psEnc.state_Fxx[0].frame_length)
				{
					break;
				}
				if (psEnc.state_Fxx[0].nFramesEncoded == 0 && prefillFlag == 0)
				{
					byte[] array3 = new byte[2];
					array3[0] = (byte)(256 - Inlines.silk_RSHIFT(256, (psEnc.state_Fxx[0].nFramesPerPacket + 1) * encControl.nChannelsInternal));
					psRangeEnc.enc_icdf(encodedDataOut, 0, array3, 8U);
					for (int i = 0; i < encControl.nChannelsInternal; i++)
					{
						int num11 = 0;
						for (int j = 0; j < psEnc.state_Fxx[i].nFramesPerPacket; j++)
						{
							num11 |= Inlines.silk_LSHIFT(psEnc.state_Fxx[i].LBRR_flags[j], j);
						}
						psEnc.state_Fxx[i].LBRR_flag = ((num11 > 0) ? 1 : 0);
						if (num11 != 0 && psEnc.state_Fxx[i].nFramesPerPacket > 1)
						{
							psRangeEnc.enc_icdf(encodedDataOut, num11 - 1, Tables.silk_LBRR_flags_iCDF_ptr[psEnc.state_Fxx[i].nFramesPerPacket - 2], 8U);
						}
					}
					for (int j = 0; j < psEnc.state_Fxx[0].nFramesPerPacket; j++)
					{
						for (int i = 0; i < encControl.nChannelsInternal; i++)
						{
							if (psEnc.state_Fxx[i].LBRR_flags[j] != 0)
							{
								if (encControl.nChannelsInternal == 2 && i == 0)
								{
									Stereo.silk_stereo_encode_pred(psRangeEnc, encodedDataOut, psEnc.sStereo.predIx[j]);
									if (psEnc.state_Fxx[1].LBRR_flags[j] == 0)
									{
										Stereo.silk_stereo_encode_mid_only(psRangeEnc, encodedDataOut, psEnc.sStereo.mid_only_flags[j]);
									}
								}
								int condCoding;
								if (j > 0 && psEnc.state_Fxx[i].LBRR_flags[j - 1] != 0)
								{
									condCoding = 2;
								}
								else
								{
									condCoding = 0;
								}
								EncodeIndices.silk_encode_indices(psEnc.state_Fxx[i], psRangeEnc, encodedDataOut, j, 1, condCoding);
								EncodePulses.silk_encode_pulses(psRangeEnc, encodedDataOut, (int)psEnc.state_Fxx[i].indices_LBRR[j].signalType, (int)psEnc.state_Fxx[i].indices_LBRR[j].quantOffsetType, psEnc.state_Fxx[i].pulses_LBRR[j], psEnc.state_Fxx[i].frame_length);
							}
						}
					}
					for (int i = 0; i < encControl.nChannelsInternal; i++)
					{
						Arrays.MemSetInt(psEnc.state_Fxx[i].LBRR_flags, 0, 3);
					}
					psEnc.nBitsUsedLBRR = psRangeEnc.tell();
				}
				HPVariableCutoff.silk_HP_variable_cutoff(psEnc.state_Fxx);
				int num12 = Inlines.silk_DIV32_16(Inlines.silk_MUL(encControl.bitRate, encControl.payloadSize_ms), 1000);
				if (prefillFlag == 0)
				{
					num12 -= psEnc.nBitsUsedLBRR;
				}
				num12 = Inlines.silk_DIV32_16(num12, psEnc.state_Fxx[0].nFramesPerPacket);
				if (encControl.payloadSize_ms == 10)
				{
					num6 = Inlines.silk_SMULBB(num12, 100);
				}
				else
				{
					num6 = Inlines.silk_SMULBB(num12, 50);
				}
				num6 -= Inlines.silk_DIV32_16(Inlines.silk_MUL(psEnc.nBitsExceeded, 1000), 500);
				if (prefillFlag == 0 && psEnc.state_Fxx[0].nFramesEncoded > 0)
				{
					int a2 = psRangeEnc.tell() - psEnc.nBitsUsedLBRR - num12 * psEnc.state_Fxx[0].nFramesEncoded;
					num6 -= Inlines.silk_DIV32_16(Inlines.silk_MUL(a2, 1000), 500);
				}
				num6 = Inlines.silk_LIMIT(num6, encControl.bitRate, 5000);
				if (encControl.nChannelsInternal == 2)
				{
					BoxedValueSbyte boxedValueSbyte = new BoxedValueSbyte(psEnc.sStereo.mid_only_flags[psEnc.state_Fxx[0].nFramesEncoded]);
					Stereo.silk_stereo_LR_to_MS(psEnc.sStereo, psEnc.state_Fxx[0].inputBuf, 2, psEnc.state_Fxx[1].inputBuf, 2, psEnc.sStereo.predIx[psEnc.state_Fxx[0].nFramesEncoded], boxedValueSbyte, array, num6, psEnc.state_Fxx[0].speech_activity_Q8, encControl.toMono, psEnc.state_Fxx[0].fs_kHz, psEnc.state_Fxx[0].frame_length);
					psEnc.sStereo.mid_only_flags[psEnc.state_Fxx[0].nFramesEncoded] = boxedValueSbyte.Val;
					if (boxedValueSbyte.Val == 0)
					{
						if (psEnc.prev_decode_only_middle == 1)
						{
							psEnc.state_Fxx[1].sShape.Reset();
							psEnc.state_Fxx[1].sPrefilt.Reset();
							psEnc.state_Fxx[1].sNSQ.Reset();
							Arrays.MemSetShort(psEnc.state_Fxx[1].prev_NLSFq_Q15, 0, 16);
							Arrays.MemSetInt(psEnc.state_Fxx[1].sLP.In_LP_State, 0, 2);
							psEnc.state_Fxx[1].prevLag = 100;
							psEnc.state_Fxx[1].sNSQ.lagPrev = 100;
							psEnc.state_Fxx[1].sShape.LastGainIndex = 10;
							psEnc.state_Fxx[1].prevSignalType = 0;
							psEnc.state_Fxx[1].sNSQ.prev_gain_Q16 = 65536;
							psEnc.state_Fxx[1].first_frame_after_reset = 1;
						}
						psEnc.state_Fxx[1].silk_encode_do_VAD();
					}
					else
					{
						psEnc.state_Fxx[1].VAD_flags[psEnc.state_Fxx[0].nFramesEncoded] = 0;
					}
					if (prefillFlag == 0)
					{
						Stereo.silk_stereo_encode_pred(psRangeEnc, encodedDataOut, psEnc.sStereo.predIx[psEnc.state_Fxx[0].nFramesEncoded]);
						if (psEnc.state_Fxx[1].VAD_flags[psEnc.state_Fxx[0].nFramesEncoded] == 0)
						{
							Stereo.silk_stereo_encode_mid_only(psRangeEnc, encodedDataOut, psEnc.sStereo.mid_only_flags[psEnc.state_Fxx[0].nFramesEncoded]);
						}
					}
				}
				else
				{
					Arrays.MemCopy(psEnc.sStereo.sMid, 0, psEnc.state_Fxx[0].inputBuf, 0, 2);
					Arrays.MemCopy(psEnc.state_Fxx[0].inputBuf, psEnc.state_Fxx[0].frame_length, psEnc.sStereo.sMid, 0, 2);
				}
				psEnc.state_Fxx[0].silk_encode_do_VAD();
				for (int i = 0; i < encControl.nChannelsInternal; i++)
				{
					int num13 = encControl.maxBits;
					if (num4 == 2 && num5 == 0)
					{
						num13 = num13 * 3 / 5;
					}
					else if (num4 == 3)
					{
						if (num5 == 0)
						{
							num13 = num13 * 2 / 5;
						}
						else if (num5 == 1)
						{
							num13 = num13 * 3 / 4;
						}
					}
					int useCBR = (encControl.useCBR != 0 && num5 == num4 - 1) ? 1 : 0;
					int num14;
					if (encControl.nChannelsInternal == 1)
					{
						num14 = num6;
					}
					else
					{
						num14 = array[i];
						if (i == 0 && array[1] > 0)
						{
							useCBR = 0;
							num13 -= encControl.maxBits / (num4 * 2);
						}
					}
					if (num14 > 0)
					{
						psEnc.state_Fxx[i].silk_control_SNR(num14);
						int condCoding2;
						if (psEnc.state_Fxx[0].nFramesEncoded - i <= 0)
						{
							condCoding2 = 0;
						}
						else if (i > 0 && psEnc.prev_decode_only_middle != 0)
						{
							condCoding2 = 1;
						}
						else
						{
							condCoding2 = 2;
						}
						num += psEnc.state_Fxx[i].silk_encode_frame(nBytesOut, psRangeEnc, encodedDataOut, condCoding2, num13, useCBR);
					}
					psEnc.state_Fxx[i].controlled_since_last_payload = 0;
					psEnc.state_Fxx[i].inputBufIx = 0;
					psEnc.state_Fxx[i].nFramesEncoded++;
				}
				psEnc.prev_decode_only_middle = (int)psEnc.sStereo.mid_only_flags[psEnc.state_Fxx[0].nFramesEncoded - 1];
				if (nBytesOut.Val > 0 && psEnc.state_Fxx[0].nFramesEncoded == psEnc.state_Fxx[0].nFramesPerPacket)
				{
					int num15 = 0;
					for (int i = 0; i < encControl.nChannelsInternal; i++)
					{
						for (int j = 0; j < psEnc.state_Fxx[i].nFramesPerPacket; j++)
						{
							num15 = Inlines.silk_LSHIFT(num15, 1);
							num15 |= (int)psEnc.state_Fxx[i].VAD_flags[j];
						}
						num15 = Inlines.silk_LSHIFT(num15, 1);
						num15 |= (int)psEnc.state_Fxx[i].LBRR_flag;
					}
					if (prefillFlag == 0)
					{
						psRangeEnc.enc_patch_initial_bits(encodedDataOut, (uint)num15, (uint)((psEnc.state_Fxx[0].nFramesPerPacket + 1) * encControl.nChannelsInternal));
					}
					if (psEnc.state_Fxx[0].inDTX != 0 && (encControl.nChannelsInternal == 1 || psEnc.state_Fxx[1].inDTX != 0))
					{
						nBytesOut.Val = 0;
					}
					psEnc.nBitsExceeded += nBytesOut.Val * 8;
					psEnc.nBitsExceeded -= Inlines.silk_DIV32_16(Inlines.silk_MUL(encControl.bitRate, encControl.payloadSize_ms), 1000);
					psEnc.nBitsExceeded = Inlines.silk_LIMIT(psEnc.nBitsExceeded, 0, 10000);
					int num16 = Inlines.silk_SMLAWB(13, 3188, psEnc.timeSinceSwitchAllowed_ms);
					if (psEnc.state_Fxx[0].speech_activity_Q8 < num16)
					{
						psEnc.allowBandwidthSwitch = 1;
						psEnc.timeSinceSwitchAllowed_ms = 0;
					}
					else
					{
						psEnc.allowBandwidthSwitch = 0;
						psEnc.timeSinceSwitchAllowed_ms += encControl.payloadSize_ms;
					}
				}
				if (nSamplesIn == 0)
				{
					break;
				}
				num5++;
			}
			psEnc.nPrevChannelsInternal = encControl.nChannelsInternal;
			encControl.allowBandwidthSwitch = psEnc.allowBandwidthSwitch;
			encControl.inWBmodeWithoutVariableLP = ((psEnc.state_Fxx[0].fs_kHz == 16 && psEnc.state_Fxx[0].sLP.mode == 0) ? 1 : 0);
			encControl.internalSampleRate = Inlines.silk_SMULBB(psEnc.state_Fxx[0].fs_kHz, 1000);
			encControl.stereoWidth_Q14 = (int)((encControl.toMono != 0) ? 0 : psEnc.sStereo.smth_width_Q14);
			if (prefillFlag != 0)
			{
				encControl.payloadSize_ms = payloadSize_ms;
				encControl.complexity = complexity;
				for (int i = 0; i < encControl.nChannelsInternal; i++)
				{
					psEnc.state_Fxx[i].controlled_since_last_payload = 0;
					psEnc.state_Fxx[i].prefillFlag = 0;
				}
			}
			return num;
		}
	}
}
