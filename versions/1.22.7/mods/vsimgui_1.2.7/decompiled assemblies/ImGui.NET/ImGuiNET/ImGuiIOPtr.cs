using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace ImGuiNET
{
	// Token: 0x0200003E RID: 62
	public struct ImGuiIOPtr
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x00013FFE File Offset: 0x000121FE
		public unsafe readonly ImGuiIO* NativePtr { get; }

		// Token: 0x0600048C RID: 1164 RVA: 0x00014006 File Offset: 0x00012206
		public unsafe ImGuiIOPtr(ImGuiIO* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0001400F File Offset: 0x0001220F
		public unsafe ImGuiIOPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0001401D File Offset: 0x0001221D
		public unsafe static implicit operator ImGuiIOPtr(ImGuiIO* nativePtr)
		{
			return new ImGuiIOPtr(nativePtr);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00014025 File Offset: 0x00012225
		public unsafe static implicit operator ImGuiIO*(ImGuiIOPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0001402E File Offset: 0x0001222E
		public static implicit operator ImGuiIOPtr(IntPtr nativePtr)
		{
			return new ImGuiIOPtr(nativePtr);
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x00014036 File Offset: 0x00012236
		public unsafe ref ImGuiConfigFlags ConfigFlags
		{
			get
			{
				return Unsafe.AsRef<ImGuiConfigFlags>((void*)(&this.NativePtr->ConfigFlags));
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x00014049 File Offset: 0x00012249
		public unsafe ref ImGuiBackendFlags BackendFlags
		{
			get
			{
				return Unsafe.AsRef<ImGuiBackendFlags>((void*)(&this.NativePtr->BackendFlags));
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0001405C File Offset: 0x0001225C
		public unsafe ref Vector2 DisplaySize
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->DisplaySize));
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x0001406F File Offset: 0x0001226F
		public unsafe ref float DeltaTime
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->DeltaTime));
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x00014082 File Offset: 0x00012282
		public unsafe ref float IniSavingRate
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->IniSavingRate));
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x00014095 File Offset: 0x00012295
		public unsafe NullTerminatedString IniFilename
		{
			get
			{
				return new NullTerminatedString(this.NativePtr->IniFilename);
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x000140A7 File Offset: 0x000122A7
		public unsafe NullTerminatedString LogFilename
		{
			get
			{
				return new NullTerminatedString(this.NativePtr->LogFilename);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x000140B9 File Offset: 0x000122B9
		// (set) Token: 0x06000499 RID: 1177 RVA: 0x000140CB File Offset: 0x000122CB
		public unsafe IntPtr UserData
		{
			get
			{
				return (IntPtr)this.NativePtr->UserData;
			}
			set
			{
				this.NativePtr->UserData = (void*)value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x000140DE File Offset: 0x000122DE
		public unsafe ImFontAtlasPtr Fonts
		{
			get
			{
				return new ImFontAtlasPtr(this.NativePtr->Fonts);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x000140F0 File Offset: 0x000122F0
		public unsafe ref float FontGlobalScale
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->FontGlobalScale));
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x00014103 File Offset: 0x00012303
		public unsafe ref bool FontAllowUserScaling
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->FontAllowUserScaling));
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x00014116 File Offset: 0x00012316
		public unsafe ImFontPtr FontDefault
		{
			get
			{
				return new ImFontPtr(this.NativePtr->FontDefault);
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x00014128 File Offset: 0x00012328
		public unsafe ref Vector2 DisplayFramebufferScale
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->DisplayFramebufferScale));
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x0001413B File Offset: 0x0001233B
		public unsafe ref bool ConfigDockingNoSplit
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigDockingNoSplit));
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x0001414E File Offset: 0x0001234E
		public unsafe ref bool ConfigDockingWithShift
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigDockingWithShift));
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x00014161 File Offset: 0x00012361
		public unsafe ref bool ConfigDockingAlwaysTabBar
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigDockingAlwaysTabBar));
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x00014174 File Offset: 0x00012374
		public unsafe ref bool ConfigDockingTransparentPayload
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigDockingTransparentPayload));
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x00014187 File Offset: 0x00012387
		public unsafe ref bool ConfigViewportsNoAutoMerge
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigViewportsNoAutoMerge));
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x0001419A File Offset: 0x0001239A
		public unsafe ref bool ConfigViewportsNoTaskBarIcon
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigViewportsNoTaskBarIcon));
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x000141AD File Offset: 0x000123AD
		public unsafe ref bool ConfigViewportsNoDecoration
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigViewportsNoDecoration));
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x000141C0 File Offset: 0x000123C0
		public unsafe ref bool ConfigViewportsNoDefaultParent
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigViewportsNoDefaultParent));
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x000141D3 File Offset: 0x000123D3
		public unsafe ref bool MouseDrawCursor
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->MouseDrawCursor));
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x000141E6 File Offset: 0x000123E6
		public unsafe ref bool ConfigMacOSXBehaviors
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigMacOSXBehaviors));
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x000141F9 File Offset: 0x000123F9
		public unsafe ref bool ConfigInputTrickleEventQueue
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigInputTrickleEventQueue));
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x0001420C File Offset: 0x0001240C
		public unsafe ref bool ConfigInputTextCursorBlink
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigInputTextCursorBlink));
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x0001421F File Offset: 0x0001241F
		public unsafe ref bool ConfigInputTextEnterKeepActive
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigInputTextEnterKeepActive));
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x00014232 File Offset: 0x00012432
		public unsafe ref bool ConfigDragClickToInputText
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigDragClickToInputText));
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00014245 File Offset: 0x00012445
		public unsafe ref bool ConfigWindowsResizeFromEdges
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigWindowsResizeFromEdges));
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00014258 File Offset: 0x00012458
		public unsafe ref bool ConfigWindowsMoveFromTitleBarOnly
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigWindowsMoveFromTitleBarOnly));
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x0001426B File Offset: 0x0001246B
		public unsafe ref float ConfigMemoryCompactTimer
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->ConfigMemoryCompactTimer));
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x0001427E File Offset: 0x0001247E
		public unsafe ref float MouseDoubleClickTime
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->MouseDoubleClickTime));
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00014291 File Offset: 0x00012491
		public unsafe ref float MouseDoubleClickMaxDist
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->MouseDoubleClickMaxDist));
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x000142A4 File Offset: 0x000124A4
		public unsafe ref float MouseDragThreshold
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->MouseDragThreshold));
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x000142B7 File Offset: 0x000124B7
		public unsafe ref float KeyRepeatDelay
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->KeyRepeatDelay));
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x000142CA File Offset: 0x000124CA
		public unsafe ref float KeyRepeatRate
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->KeyRepeatRate));
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x000142DD File Offset: 0x000124DD
		public unsafe ref bool ConfigDebugBeginReturnValueOnce
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigDebugBeginReturnValueOnce));
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x000142F0 File Offset: 0x000124F0
		public unsafe ref bool ConfigDebugBeginReturnValueLoop
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigDebugBeginReturnValueLoop));
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00014303 File Offset: 0x00012503
		public unsafe ref bool ConfigDebugIgnoreFocusLoss
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigDebugIgnoreFocusLoss));
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00014316 File Offset: 0x00012516
		public unsafe ref bool ConfigDebugIniSettings
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->ConfigDebugIniSettings));
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00014329 File Offset: 0x00012529
		public unsafe NullTerminatedString BackendPlatformName
		{
			get
			{
				return new NullTerminatedString(this.NativePtr->BackendPlatformName);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x0001433B File Offset: 0x0001253B
		public unsafe NullTerminatedString BackendRendererName
		{
			get
			{
				return new NullTerminatedString(this.NativePtr->BackendRendererName);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x0001434D File Offset: 0x0001254D
		// (set) Token: 0x060004BC RID: 1212 RVA: 0x0001435F File Offset: 0x0001255F
		public unsafe IntPtr BackendPlatformUserData
		{
			get
			{
				return (IntPtr)this.NativePtr->BackendPlatformUserData;
			}
			set
			{
				this.NativePtr->BackendPlatformUserData = (void*)value;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00014372 File Offset: 0x00012572
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x00014384 File Offset: 0x00012584
		public unsafe IntPtr BackendRendererUserData
		{
			get
			{
				return (IntPtr)this.NativePtr->BackendRendererUserData;
			}
			set
			{
				this.NativePtr->BackendRendererUserData = (void*)value;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x00014397 File Offset: 0x00012597
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x000143A9 File Offset: 0x000125A9
		public unsafe IntPtr BackendLanguageUserData
		{
			get
			{
				return (IntPtr)this.NativePtr->BackendLanguageUserData;
			}
			set
			{
				this.NativePtr->BackendLanguageUserData = (void*)value;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x000143BC File Offset: 0x000125BC
		public unsafe ref IntPtr GetClipboardTextFn
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->GetClipboardTextFn));
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x000143CF File Offset: 0x000125CF
		public unsafe ref IntPtr SetClipboardTextFn
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->SetClipboardTextFn));
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x000143E2 File Offset: 0x000125E2
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x000143F4 File Offset: 0x000125F4
		public unsafe IntPtr ClipboardUserData
		{
			get
			{
				return (IntPtr)this.NativePtr->ClipboardUserData;
			}
			set
			{
				this.NativePtr->ClipboardUserData = (void*)value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00014407 File Offset: 0x00012607
		public unsafe ref IntPtr SetPlatformImeDataFn
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->SetPlatformImeDataFn));
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0001441A File Offset: 0x0001261A
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x0001442C File Offset: 0x0001262C
		public unsafe IntPtr _UnusedPadding
		{
			get
			{
				return (IntPtr)this.NativePtr->_UnusedPadding;
			}
			set
			{
				this.NativePtr->_UnusedPadding = (void*)value;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x0001443F File Offset: 0x0001263F
		public unsafe ref ushort PlatformLocaleDecimalPoint
		{
			get
			{
				return Unsafe.AsRef<ushort>((void*)(&this.NativePtr->PlatformLocaleDecimalPoint));
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00014452 File Offset: 0x00012652
		public unsafe ref bool WantCaptureMouse
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->WantCaptureMouse));
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x00014465 File Offset: 0x00012665
		public unsafe ref bool WantCaptureKeyboard
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->WantCaptureKeyboard));
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x00014478 File Offset: 0x00012678
		public unsafe ref bool WantTextInput
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->WantTextInput));
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x0001448B File Offset: 0x0001268B
		public unsafe ref bool WantSetMousePos
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->WantSetMousePos));
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x0001449E File Offset: 0x0001269E
		public unsafe ref bool WantSaveIniSettings
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->WantSaveIniSettings));
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x000144B1 File Offset: 0x000126B1
		public unsafe ref bool NavActive
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->NavActive));
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x000144C4 File Offset: 0x000126C4
		public unsafe ref bool NavVisible
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->NavVisible));
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x000144D7 File Offset: 0x000126D7
		public unsafe ref float Framerate
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->Framerate));
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x000144EA File Offset: 0x000126EA
		public unsafe ref int MetricsRenderVertices
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->MetricsRenderVertices));
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x000144FD File Offset: 0x000126FD
		public unsafe ref int MetricsRenderIndices
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->MetricsRenderIndices));
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00014510 File Offset: 0x00012710
		public unsafe ref int MetricsRenderWindows
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->MetricsRenderWindows));
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x00014523 File Offset: 0x00012723
		public unsafe ref int MetricsActiveWindows
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->MetricsActiveWindows));
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00014536 File Offset: 0x00012736
		public unsafe ref int MetricsActiveAllocations
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->MetricsActiveAllocations));
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x00014549 File Offset: 0x00012749
		public unsafe ref Vector2 MouseDelta
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->MouseDelta));
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0001455C File Offset: 0x0001275C
		public unsafe RangeAccessor<int> KeyMap
		{
			get
			{
				return new RangeAccessor<int>((void*)(&this.NativePtr->KeyMap.FixedElementField), 652);
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00014579 File Offset: 0x00012779
		public unsafe RangeAccessor<bool> KeysDown
		{
			get
			{
				return new RangeAccessor<bool>((void*)(&this.NativePtr->KeysDown.FixedElementField), 652);
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00014596 File Offset: 0x00012796
		public unsafe RangeAccessor<float> NavInputs
		{
			get
			{
				return new RangeAccessor<float>((void*)(&this.NativePtr->NavInputs.FixedElementField), 16);
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x000145B0 File Offset: 0x000127B0
		public unsafe ref IntPtr Ctx
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Ctx));
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x000145C3 File Offset: 0x000127C3
		public unsafe ref Vector2 MousePos
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->MousePos));
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x000145D6 File Offset: 0x000127D6
		public unsafe RangeAccessor<bool> MouseDown
		{
			get
			{
				return new RangeAccessor<bool>((void*)(&this.NativePtr->MouseDown.FixedElementField), 5);
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x000145EF File Offset: 0x000127EF
		public unsafe ref float MouseWheel
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->MouseWheel));
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00014602 File Offset: 0x00012802
		public unsafe ref float MouseWheelH
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->MouseWheelH));
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00014615 File Offset: 0x00012815
		public unsafe ref ImGuiMouseSource MouseSource
		{
			get
			{
				return Unsafe.AsRef<ImGuiMouseSource>((void*)(&this.NativePtr->MouseSource));
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x00014628 File Offset: 0x00012828
		public unsafe ref uint MouseHoveredViewport
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->MouseHoveredViewport));
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x0001463B File Offset: 0x0001283B
		public unsafe ref bool KeyCtrl
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->KeyCtrl));
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x0001464E File Offset: 0x0001284E
		public unsafe ref bool KeyShift
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->KeyShift));
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00014661 File Offset: 0x00012861
		public unsafe ref bool KeyAlt
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->KeyAlt));
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x00014674 File Offset: 0x00012874
		public unsafe ref bool KeySuper
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->KeySuper));
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x00014687 File Offset: 0x00012887
		public unsafe ref ImGuiKey KeyMods
		{
			get
			{
				return Unsafe.AsRef<ImGuiKey>((void*)(&this.NativePtr->KeyMods));
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x0001469A File Offset: 0x0001289A
		public unsafe RangeAccessor<ImGuiKeyData> KeysData
		{
			get
			{
				return new RangeAccessor<ImGuiKeyData>((void*)(&this.NativePtr->KeysData_0), 652);
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x000146B2 File Offset: 0x000128B2
		public unsafe ref bool WantCaptureMouseUnlessPopupClose
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->WantCaptureMouseUnlessPopupClose));
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x000146C5 File Offset: 0x000128C5
		public unsafe ref Vector2 MousePosPrev
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->MousePosPrev));
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x000146D8 File Offset: 0x000128D8
		public unsafe RangeAccessor<Vector2> MouseClickedPos
		{
			get
			{
				return new RangeAccessor<Vector2>((void*)(&this.NativePtr->MouseClickedPos_0), 5);
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x000146EC File Offset: 0x000128EC
		public unsafe RangeAccessor<double> MouseClickedTime
		{
			get
			{
				return new RangeAccessor<double>((void*)(&this.NativePtr->MouseClickedTime.FixedElementField), 5);
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00014705 File Offset: 0x00012905
		public unsafe RangeAccessor<bool> MouseClicked
		{
			get
			{
				return new RangeAccessor<bool>((void*)(&this.NativePtr->MouseClicked.FixedElementField), 5);
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x0001471E File Offset: 0x0001291E
		public unsafe RangeAccessor<bool> MouseDoubleClicked
		{
			get
			{
				return new RangeAccessor<bool>((void*)(&this.NativePtr->MouseDoubleClicked.FixedElementField), 5);
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x00014737 File Offset: 0x00012937
		public unsafe RangeAccessor<ushort> MouseClickedCount
		{
			get
			{
				return new RangeAccessor<ushort>((void*)(&this.NativePtr->MouseClickedCount.FixedElementField), 5);
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x00014750 File Offset: 0x00012950
		public unsafe RangeAccessor<ushort> MouseClickedLastCount
		{
			get
			{
				return new RangeAccessor<ushort>((void*)(&this.NativePtr->MouseClickedLastCount.FixedElementField), 5);
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x00014769 File Offset: 0x00012969
		public unsafe RangeAccessor<bool> MouseReleased
		{
			get
			{
				return new RangeAccessor<bool>((void*)(&this.NativePtr->MouseReleased.FixedElementField), 5);
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00014782 File Offset: 0x00012982
		public unsafe RangeAccessor<bool> MouseDownOwned
		{
			get
			{
				return new RangeAccessor<bool>((void*)(&this.NativePtr->MouseDownOwned.FixedElementField), 5);
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x0001479B File Offset: 0x0001299B
		public unsafe RangeAccessor<bool> MouseDownOwnedUnlessPopupClose
		{
			get
			{
				return new RangeAccessor<bool>((void*)(&this.NativePtr->MouseDownOwnedUnlessPopupClose.FixedElementField), 5);
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x000147B4 File Offset: 0x000129B4
		public unsafe ref bool MouseWheelRequestAxisSwap
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->MouseWheelRequestAxisSwap));
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x000147C7 File Offset: 0x000129C7
		public unsafe RangeAccessor<float> MouseDownDuration
		{
			get
			{
				return new RangeAccessor<float>((void*)(&this.NativePtr->MouseDownDuration.FixedElementField), 5);
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x000147E0 File Offset: 0x000129E0
		public unsafe RangeAccessor<float> MouseDownDurationPrev
		{
			get
			{
				return new RangeAccessor<float>((void*)(&this.NativePtr->MouseDownDurationPrev.FixedElementField), 5);
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x000147F9 File Offset: 0x000129F9
		public unsafe RangeAccessor<Vector2> MouseDragMaxDistanceAbs
		{
			get
			{
				return new RangeAccessor<Vector2>((void*)(&this.NativePtr->MouseDragMaxDistanceAbs_0), 5);
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x0001480D File Offset: 0x00012A0D
		public unsafe RangeAccessor<float> MouseDragMaxDistanceSqr
		{
			get
			{
				return new RangeAccessor<float>((void*)(&this.NativePtr->MouseDragMaxDistanceSqr.FixedElementField), 5);
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x00014826 File Offset: 0x00012A26
		public unsafe ref float PenPressure
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->PenPressure));
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00014839 File Offset: 0x00012A39
		public unsafe ref bool AppFocusLost
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->AppFocusLost));
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0001484C File Offset: 0x00012A4C
		public unsafe ref bool AppAcceptingEvents
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->AppAcceptingEvents));
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x0001485F File Offset: 0x00012A5F
		public unsafe ref sbyte BackendUsingLegacyKeyArrays
		{
			get
			{
				return Unsafe.AsRef<sbyte>((void*)(&this.NativePtr->BackendUsingLegacyKeyArrays));
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x00014872 File Offset: 0x00012A72
		public unsafe ref bool BackendUsingLegacyNavInputArray
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->BackendUsingLegacyNavInputArray));
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x00014885 File Offset: 0x00012A85
		public unsafe ref ushort InputQueueSurrogate
		{
			get
			{
				return Unsafe.AsRef<ushort>((void*)(&this.NativePtr->InputQueueSurrogate));
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x00014898 File Offset: 0x00012A98
		public unsafe ImVector<ushort> InputQueueCharacters
		{
			get
			{
				return new ImVector<ushort>(this.NativePtr->InputQueueCharacters);
			}
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x000148AC File Offset: 0x00012AAC
		public void AddFocusEvent(bool focused)
		{
			byte focused2 = (focused > false) ? 1 : 0;
			ImGuiNative.ImGuiIO_AddFocusEvent(this.NativePtr, focused2);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x000148CB File Offset: 0x00012ACB
		public void AddInputCharacter(uint c)
		{
			ImGuiNative.ImGuiIO_AddInputCharacter(this.NativePtr, c);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x000148DC File Offset: 0x00012ADC
		public unsafe void AddInputCharactersUTF8(ReadOnlySpan<char> str)
		{
			int num = 0;
			byte* ptr;
			if (str != null)
			{
				num = Encoding.UTF8.GetByteCount(str);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.ImGuiIO_AddInputCharactersUTF8(this.NativePtr, ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001494C File Offset: 0x00012B4C
		public void AddInputCharacterUTF16(ushort c)
		{
			ImGuiNative.ImGuiIO_AddInputCharacterUTF16(this.NativePtr, c);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001495C File Offset: 0x00012B5C
		public void AddKeyAnalogEvent(ImGuiKey key, bool down, float v)
		{
			byte down2 = (down > false) ? 1 : 0;
			ImGuiNative.ImGuiIO_AddKeyAnalogEvent(this.NativePtr, key, down2, v);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00014980 File Offset: 0x00012B80
		public void AddKeyEvent(ImGuiKey key, bool down)
		{
			byte down2 = (down > false) ? 1 : 0;
			ImGuiNative.ImGuiIO_AddKeyEvent(this.NativePtr, key, down2);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x000149A0 File Offset: 0x00012BA0
		public void AddMouseButtonEvent(int button, bool down)
		{
			byte down2 = (down > false) ? 1 : 0;
			ImGuiNative.ImGuiIO_AddMouseButtonEvent(this.NativePtr, button, down2);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x000149C0 File Offset: 0x00012BC0
		public void AddMousePosEvent(float x, float y)
		{
			ImGuiNative.ImGuiIO_AddMousePosEvent(this.NativePtr, x, y);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000149CF File Offset: 0x00012BCF
		public void AddMouseSourceEvent(ImGuiMouseSource source)
		{
			ImGuiNative.ImGuiIO_AddMouseSourceEvent(this.NativePtr, source);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x000149DD File Offset: 0x00012BDD
		public void AddMouseViewportEvent(uint id)
		{
			ImGuiNative.ImGuiIO_AddMouseViewportEvent(this.NativePtr, id);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x000149EB File Offset: 0x00012BEB
		public void AddMouseWheelEvent(float wheel_x, float wheel_y)
		{
			ImGuiNative.ImGuiIO_AddMouseWheelEvent(this.NativePtr, wheel_x, wheel_y);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x000149FA File Offset: 0x00012BFA
		public void ClearEventsQueue()
		{
			ImGuiNative.ImGuiIO_ClearEventsQueue(this.NativePtr);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00014A07 File Offset: 0x00012C07
		public void ClearInputKeys()
		{
			ImGuiNative.ImGuiIO_ClearInputKeys(this.NativePtr);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00014A14 File Offset: 0x00012C14
		public void Destroy()
		{
			ImGuiNative.ImGuiIO_destroy(this.NativePtr);
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00014A24 File Offset: 0x00012C24
		public void SetAppAcceptingEvents(bool accepting_events)
		{
			byte accepting_events2 = (accepting_events > false) ? 1 : 0;
			ImGuiNative.ImGuiIO_SetAppAcceptingEvents(this.NativePtr, accepting_events2);
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00014A44 File Offset: 0x00012C44
		public void SetKeyEventNativeData(ImGuiKey key, int native_keycode, int native_scancode)
		{
			int native_legacy_index = -1;
			ImGuiNative.ImGuiIO_SetKeyEventNativeData(this.NativePtr, key, native_keycode, native_scancode, native_legacy_index);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00014A62 File Offset: 0x00012C62
		public void SetKeyEventNativeData(ImGuiKey key, int native_keycode, int native_scancode, int native_legacy_index)
		{
			ImGuiNative.ImGuiIO_SetKeyEventNativeData(this.NativePtr, key, native_keycode, native_scancode, native_legacy_index);
		}
	}
}
