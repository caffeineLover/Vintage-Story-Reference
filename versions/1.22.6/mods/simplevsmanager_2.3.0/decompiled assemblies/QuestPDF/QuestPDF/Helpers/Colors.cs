using System;
using QuestPDF.Infrastructure;

namespace QuestPDF.Helpers
{
	// Token: 0x02000071 RID: 113
	public static class Colors
	{
		// Token: 0x0400019B RID: 411
		public static readonly Color Black = new Color(4278190080U);

		// Token: 0x0400019C RID: 412
		public static readonly Color White = new Color(uint.MaxValue);

		// Token: 0x0400019D RID: 413
		public static readonly Color Transparent = new Color(0U);

		// Token: 0x0200016E RID: 366
		public static class Red
		{
			// Token: 0x040004B6 RID: 1206
			public static readonly Color Lighten5 = new Color(4294962158U);

			// Token: 0x040004B7 RID: 1207
			public static readonly Color Lighten4 = new Color(4294954450U);

			// Token: 0x040004B8 RID: 1208
			public static readonly Color Lighten3 = new Color(4293892762U);

			// Token: 0x040004B9 RID: 1209
			public static readonly Color Lighten2 = new Color(4293227379U);

			// Token: 0x040004BA RID: 1210
			public static readonly Color Lighten1 = new Color(4293874512U);

			// Token: 0x040004BB RID: 1211
			public static readonly Color Medium = new Color(4294198070U);

			// Token: 0x040004BC RID: 1212
			public static readonly Color Darken1 = new Color(4293212469U);

			// Token: 0x040004BD RID: 1213
			public static readonly Color Darken2 = new Color(4292030255U);

			// Token: 0x040004BE RID: 1214
			public static readonly Color Darken3 = new Color(4291176488U);

			// Token: 0x040004BF RID: 1215
			public static readonly Color Darken4 = new Color(4290190364U);

			// Token: 0x040004C0 RID: 1216
			public static readonly Color Accent1 = new Color(4294937216U);

			// Token: 0x040004C1 RID: 1217
			public static readonly Color Accent2 = new Color(4294922834U);

			// Token: 0x040004C2 RID: 1218
			public static readonly Color Accent3 = new Color(4294907716U);

			// Token: 0x040004C3 RID: 1219
			public static readonly Color Accent4 = new Color(4292149248U);
		}

		// Token: 0x0200016F RID: 367
		public static class Pink
		{
			// Token: 0x040004C4 RID: 1220
			public static readonly Color Lighten5 = new Color(4294763756U);

			// Token: 0x040004C5 RID: 1221
			public static readonly Color Lighten4 = new Color(4294491088U);

			// Token: 0x040004C6 RID: 1222
			public static readonly Color Lighten3 = new Color(4294217649U);

			// Token: 0x040004C7 RID: 1223
			public static readonly Color Lighten2 = new Color(4293943954U);

			// Token: 0x040004C8 RID: 1224
			public static readonly Color Lighten1 = new Color(4293673082U);

			// Token: 0x040004C9 RID: 1225
			public static readonly Color Medium = new Color(4293467747U);

			// Token: 0x040004CA RID: 1226
			public static readonly Color Darken1 = new Color(4292352864U);

			// Token: 0x040004CB RID: 1227
			public static readonly Color Darken2 = new Color(4290910299U);

			// Token: 0x040004CC RID: 1228
			public static readonly Color Darken3 = new Color(4289533015U);

			// Token: 0x040004CD RID: 1229
			public static readonly Color Darken4 = new Color(4287106639U);

			// Token: 0x040004CE RID: 1230
			public static readonly Color Accent1 = new Color(4294934699U);

			// Token: 0x040004CF RID: 1231
			public static readonly Color Accent2 = new Color(4294918273U);

			// Token: 0x040004D0 RID: 1232
			public static readonly Color Accent3 = new Color(4294246487U);

			// Token: 0x040004D1 RID: 1233
			public static readonly Color Accent4 = new Color(4291105122U);
		}

		// Token: 0x02000170 RID: 368
		public static class Purple
		{
			// Token: 0x040004D2 RID: 1234
			public static readonly Color Lighten5 = new Color(4294174197U);

			// Token: 0x040004D3 RID: 1235
			public static readonly Color Lighten4 = new Color(4292984551U);

			// Token: 0x040004D4 RID: 1236
			public static readonly Color Lighten3 = new Color(4291728344U);

			// Token: 0x040004D5 RID: 1237
			public static readonly Color Lighten2 = new Color(4290406600U);

			// Token: 0x040004D6 RID: 1238
			public static readonly Color Lighten1 = new Color(4289415100U);

			// Token: 0x040004D7 RID: 1239
			public static readonly Color Medium = new Color(4288423856U);

			// Token: 0x040004D8 RID: 1240
			public static readonly Color Darken1 = new Color(4287505578U);

			// Token: 0x040004D9 RID: 1241
			public static readonly Color Darken2 = new Color(4286259106U);

			// Token: 0x040004DA RID: 1242
			public static readonly Color Darken3 = new Color(4285143962U);

			// Token: 0x040004DB RID: 1243
			public static readonly Color Darken4 = new Color(4283045004U);

			// Token: 0x040004DC RID: 1244
			public static readonly Color Accent1 = new Color(4293558524U);

			// Token: 0x040004DD RID: 1245
			public static readonly Color Accent2 = new Color(4292886779U);

			// Token: 0x040004DE RID: 1246
			public static readonly Color Accent3 = new Color(4292149497U);

			// Token: 0x040004DF RID: 1247
			public static readonly Color Accent4 = new Color(4289331455U);
		}

		// Token: 0x02000171 RID: 369
		public static class DeepPurple
		{
			// Token: 0x040004E0 RID: 1248
			public static readonly Color Lighten5 = new Color(4293781494U);

			// Token: 0x040004E1 RID: 1249
			public static readonly Color Lighten4 = new Color(4291937513U);

			// Token: 0x040004E2 RID: 1250
			public static readonly Color Lighten3 = new Color(4289961435U);

			// Token: 0x040004E3 RID: 1251
			public static readonly Color Lighten2 = new Color(4287985101U);

			// Token: 0x040004E4 RID: 1252
			public static readonly Color Lighten1 = new Color(4286470082U);

			// Token: 0x040004E5 RID: 1253
			public static readonly Color Medium = new Color(4284955319U);

			// Token: 0x040004E6 RID: 1254
			public static readonly Color Darken1 = new Color(4284364209U);

			// Token: 0x040004E7 RID: 1255
			public static readonly Color Darken2 = new Color(4283510184U);

			// Token: 0x040004E8 RID: 1256
			public static readonly Color Darken3 = new Color(4282722208U);

			// Token: 0x040004E9 RID: 1257
			public static readonly Color Darken4 = new Color(4281408402U);

			// Token: 0x040004EA RID: 1258
			public static readonly Color Accent1 = new Color(4289956095U);

			// Token: 0x040004EB RID: 1259
			public static readonly Color Accent2 = new Color(4286336511U);

			// Token: 0x040004EC RID: 1260
			public static readonly Color Accent3 = new Color(4284817407U);

			// Token: 0x040004ED RID: 1261
			public static readonly Color Accent4 = new Color(4284612842U);
		}

		// Token: 0x02000172 RID: 370
		public static class Indigo
		{
			// Token: 0x040004EE RID: 1262
			public static readonly Color Lighten5 = new Color(4293454582U);

			// Token: 0x040004EF RID: 1263
			public static readonly Color Lighten4 = new Color(4291152617U);

			// Token: 0x040004F0 RID: 1264
			public static readonly Color Lighten3 = new Color(4288653530U);

			// Token: 0x040004F1 RID: 1265
			public static readonly Color Lighten2 = new Color(4286154443U);

			// Token: 0x040004F2 RID: 1266
			public static readonly Color Lighten1 = new Color(4284246976U);

			// Token: 0x040004F3 RID: 1267
			public static readonly Color Medium = new Color(4282339765U);

			// Token: 0x040004F4 RID: 1268
			public static readonly Color Darken1 = new Color(4281944491U);

			// Token: 0x040004F5 RID: 1269
			public static readonly Color Darken2 = new Color(4281352095U);

			// Token: 0x040004F6 RID: 1270
			public static readonly Color Darken3 = new Color(4280825235U);

			// Token: 0x040004F7 RID: 1271
			public static readonly Color Darken4 = new Color(4279903102U);

			// Token: 0x040004F8 RID: 1272
			public static readonly Color Accent1 = new Color(4287405823U);

			// Token: 0x040004F9 RID: 1273
			public static readonly Color Accent2 = new Color(4283657726U);

			// Token: 0x040004FA RID: 1274
			public static readonly Color Accent3 = new Color(4282211070U);

			// Token: 0x040004FB RID: 1275
			public static readonly Color Accent4 = new Color(4281356286U);
		}

		// Token: 0x02000173 RID: 371
		public static class Blue
		{
			// Token: 0x040004FC RID: 1276
			public static readonly Color Lighten5 = new Color(4293128957U);

			// Token: 0x040004FD RID: 1277
			public static readonly Color Lighten4 = new Color(4290502395U);

			// Token: 0x040004FE RID: 1278
			public static readonly Color Lighten3 = new Color(4287679225U);

			// Token: 0x040004FF RID: 1279
			public static readonly Color Lighten2 = new Color(4284790262U);

			// Token: 0x04000500 RID: 1280
			public static readonly Color Lighten1 = new Color(4282557941U);

			// Token: 0x04000501 RID: 1281
			public static readonly Color Medium = new Color(4280391411U);

			// Token: 0x04000502 RID: 1282
			public static readonly Color Darken1 = new Color(4280191205U);

			// Token: 0x04000503 RID: 1283
			public static readonly Color Darken2 = new Color(4279858898U);

			// Token: 0x04000504 RID: 1284
			public static readonly Color Darken3 = new Color(4279592384U);

			// Token: 0x04000505 RID: 1285
			public static readonly Color Darken4 = new Color(4279060385U);

			// Token: 0x04000506 RID: 1286
			public static readonly Color Accent1 = new Color(4286755327U);

			// Token: 0x04000507 RID: 1287
			public static readonly Color Accent2 = new Color(4282682111U);

			// Token: 0x04000508 RID: 1288
			public static readonly Color Accent3 = new Color(4280908287U);

			// Token: 0x04000509 RID: 1289
			public static readonly Color Accent4 = new Color(4280902399U);
		}

		// Token: 0x02000174 RID: 372
		public static class LightBlue
		{
			// Token: 0x0400050A RID: 1290
			public static readonly Color Lighten5 = new Color(4292998654U);

			// Token: 0x0400050B RID: 1291
			public static readonly Color Lighten4 = new Color(4289979900U);

			// Token: 0x0400050C RID: 1292
			public static readonly Color Lighten3 = new Color(4286698746U);

			// Token: 0x0400050D RID: 1293
			public static readonly Color Lighten2 = new Color(4283417591U);

			// Token: 0x0400050E RID: 1294
			public static readonly Color Lighten1 = new Color(4280923894U);

			// Token: 0x0400050F RID: 1295
			public static readonly Color Medium = new Color(4278430196U);

			// Token: 0x04000510 RID: 1296
			public static readonly Color Darken1 = new Color(4278426597U);

			// Token: 0x04000511 RID: 1297
			public static readonly Color Darken2 = new Color(4278356177U);

			// Token: 0x04000512 RID: 1298
			public static readonly Color Darken3 = new Color(4278351805U);

			// Token: 0x04000513 RID: 1299
			public static readonly Color Darken4 = new Color(4278278043U);

			// Token: 0x04000514 RID: 1300
			public static readonly Color Accent1 = new Color(4286634239U);

			// Token: 0x04000515 RID: 1301
			public static readonly Color Accent2 = new Color(4282434815U);

			// Token: 0x04000516 RID: 1302
			public static readonly Color Accent3 = new Color(4278235391U);

			// Token: 0x04000517 RID: 1303
			public static readonly Color Accent4 = new Color(4278227434U);
		}

		// Token: 0x02000175 RID: 373
		public static class Cyan
		{
			// Token: 0x04000518 RID: 1304
			public static readonly Color Lighten5 = new Color(4292933626U);

			// Token: 0x04000519 RID: 1305
			public static readonly Color Lighten4 = new Color(4289915890U);

			// Token: 0x0400051A RID: 1306
			public static readonly Color Lighten3 = new Color(4286635754U);

			// Token: 0x0400051B RID: 1307
			public static readonly Color Lighten2 = new Color(4283289825U);

			// Token: 0x0400051C RID: 1308
			public static readonly Color Lighten1 = new Color(4280731354U);

			// Token: 0x0400051D RID: 1309
			public static readonly Color Medium = new Color(4278238420U);

			// Token: 0x0400051E RID: 1310
			public static readonly Color Darken1 = new Color(4278234305U);

			// Token: 0x0400051F RID: 1311
			public static readonly Color Darken2 = new Color(4278228903U);

			// Token: 0x04000520 RID: 1312
			public static readonly Color Darken3 = new Color(4278223759U);

			// Token: 0x04000521 RID: 1313
			public static readonly Color Darken4 = new Color(4278214756U);

			// Token: 0x04000522 RID: 1314
			public static readonly Color Accent1 = new Color(4286906367U);

			// Token: 0x04000523 RID: 1315
			public static readonly Color Accent2 = new Color(4279828479U);

			// Token: 0x04000524 RID: 1316
			public static readonly Color Accent3 = new Color(4278248959U);

			// Token: 0x04000525 RID: 1317
			public static readonly Color Accent4 = new Color(4278237396U);
		}

		// Token: 0x02000176 RID: 374
		public static class Teal
		{
			// Token: 0x04000526 RID: 1318
			public static readonly Color Lighten5 = new Color(4292932337U);

			// Token: 0x04000527 RID: 1319
			public static readonly Color Lighten4 = new Color(4289912795U);

			// Token: 0x04000528 RID: 1320
			public static readonly Color Lighten3 = new Color(4286630852U);

			// Token: 0x04000529 RID: 1321
			public static readonly Color Lighten2 = new Color(4283283116U);

			// Token: 0x0400052A RID: 1322
			public static readonly Color Lighten1 = new Color(4280723098U);

			// Token: 0x0400052B RID: 1323
			public static readonly Color Medium = new Color(4278228616U);

			// Token: 0x0400052C RID: 1324
			public static readonly Color Darken1 = new Color(4278225275U);

			// Token: 0x0400052D RID: 1325
			public static readonly Color Darken2 = new Color(4278221163U);

			// Token: 0x0400052E RID: 1326
			public static readonly Color Darken3 = new Color(4278217052U);

			// Token: 0x0400052F RID: 1327
			public static readonly Color Darken4 = new Color(4278209856U);

			// Token: 0x04000530 RID: 1328
			public static readonly Color Accent1 = new Color(4289200107U);

			// Token: 0x04000531 RID: 1329
			public static readonly Color Accent2 = new Color(4284809178U);

			// Token: 0x04000532 RID: 1330
			public static readonly Color Accent3 = new Color(4280150454U);

			// Token: 0x04000533 RID: 1331
			public static readonly Color Accent4 = new Color(4278239141U);
		}

		// Token: 0x02000177 RID: 375
		public static class Green
		{
			// Token: 0x04000534 RID: 1332
			public static readonly Color Lighten5 = new Color(4293457385U);

			// Token: 0x04000535 RID: 1333
			public static readonly Color Lighten4 = new Color(4291356361U);

			// Token: 0x04000536 RID: 1334
			public static readonly Color Lighten3 = new Color(4289058471U);

			// Token: 0x04000537 RID: 1335
			public static readonly Color Lighten2 = new Color(4286695300U);

			// Token: 0x04000538 RID: 1336
			public static readonly Color Lighten1 = new Color(4284922730U);

			// Token: 0x04000539 RID: 1337
			public static readonly Color Medium = new Color(4283215696U);

			// Token: 0x0400053A RID: 1338
			public static readonly Color Darken1 = new Color(4282622023U);

			// Token: 0x0400053B RID: 1339
			public static readonly Color Darken2 = new Color(4281896508U);

			// Token: 0x0400053C RID: 1340
			public static readonly Color Darken3 = new Color(4281236786U);

			// Token: 0x0400053D RID: 1341
			public static readonly Color Darken4 = new Color(4279983648U);

			// Token: 0x0400053E RID: 1342
			public static readonly Color Accent1 = new Color(4290377418U);

			// Token: 0x0400053F RID: 1343
			public static readonly Color Accent2 = new Color(4285132974U);

			// Token: 0x04000540 RID: 1344
			public static readonly Color Accent3 = new Color(4278249078U);

			// Token: 0x04000541 RID: 1345
			public static readonly Color Accent4 = new Color(4278241363U);
		}

		// Token: 0x02000178 RID: 376
		public static class LightGreen
		{
			// Token: 0x04000542 RID: 1346
			public static readonly Color Lighten5 = new Color(4294047977U);

			// Token: 0x04000543 RID: 1347
			public static readonly Color Lighten4 = new Color(4292668872U);

			// Token: 0x04000544 RID: 1348
			public static readonly Color Lighten3 = new Color(4291158437U);

			// Token: 0x04000545 RID: 1349
			public static readonly Color Lighten2 = new Color(4289648001U);

			// Token: 0x04000546 RID: 1350
			public static readonly Color Lighten1 = new Color(4288466021U);

			// Token: 0x04000547 RID: 1351
			public static readonly Color Medium = new Color(4287349578U);

			// Token: 0x04000548 RID: 1352
			public static readonly Color Darken1 = new Color(4286362434U);

			// Token: 0x04000549 RID: 1353
			public static readonly Color Darken2 = new Color(4285046584U);

			// Token: 0x0400054A RID: 1354
			public static readonly Color Darken3 = new Color(4283796271U);

			// Token: 0x0400054B RID: 1355
			public static readonly Color Darken4 = new Color(4281559326U);

			// Token: 0x0400054C RID: 1356
			public static readonly Color Accent1 = new Color(4291624848U);

			// Token: 0x0400054D RID: 1357
			public static readonly Color Accent2 = new Color(4289920857U);

			// Token: 0x0400054E RID: 1358
			public static readonly Color Accent3 = new Color(4285988611U);

			// Token: 0x0400054F RID: 1359
			public static readonly Color Accent4 = new Color(4284800279U);
		}

		// Token: 0x02000179 RID: 377
		public static class Lime
		{
			// Token: 0x04000550 RID: 1360
			public static readonly Color Lighten5 = new Color(4294573031U);

			// Token: 0x04000551 RID: 1361
			public static readonly Color Lighten4 = new Color(4293981379U);

			// Token: 0x04000552 RID: 1362
			public static readonly Color Lighten3 = new Color(4293324444U);

			// Token: 0x04000553 RID: 1363
			public static readonly Color Lighten2 = new Color(4292667253U);

			// Token: 0x04000554 RID: 1364
			public static readonly Color Lighten1 = new Color(4292141399U);

			// Token: 0x04000555 RID: 1365
			public static readonly Color Medium = new Color(4291681337U);

			// Token: 0x04000556 RID: 1366
			public static readonly Color Darken1 = new Color(4290824755U);

			// Token: 0x04000557 RID: 1367
			public static readonly Color Darken2 = new Color(4289705003U);

			// Token: 0x04000558 RID: 1368
			public static readonly Color Darken3 = new Color(4288584996U);

			// Token: 0x04000559 RID: 1369
			public static readonly Color Darken4 = new Color(4286740247U);

			// Token: 0x0400055A RID: 1370
			public static readonly Color Accent1 = new Color(4294246273U);

			// Token: 0x0400055B RID: 1371
			public static readonly Color Accent2 = new Color(4293852993U);

			// Token: 0x0400055C RID: 1372
			public static readonly Color Accent3 = new Color(4291231488U);

			// Token: 0x0400055D RID: 1373
			public static readonly Color Accent4 = new Color(4289653248U);
		}

		// Token: 0x0200017A RID: 378
		public static class Yellow
		{
			// Token: 0x0400055E RID: 1374
			public static readonly Color Lighten5 = new Color(4294966759U);

			// Token: 0x0400055F RID: 1375
			public static readonly Color Lighten4 = new Color(4294965700U);

			// Token: 0x04000560 RID: 1376
			public static readonly Color Lighten3 = new Color(4294964637U);

			// Token: 0x04000561 RID: 1377
			public static readonly Color Lighten2 = new Color(4294963574U);

			// Token: 0x04000562 RID: 1378
			public static readonly Color Lighten1 = new Color(4294962776U);

			// Token: 0x04000563 RID: 1379
			public static readonly Color Medium = new Color(4294961979U);

			// Token: 0x04000564 RID: 1380
			public static readonly Color Darken1 = new Color(4294826037U);

			// Token: 0x04000565 RID: 1381
			public static readonly Color Darken2 = new Color(4294688813U);

			// Token: 0x04000566 RID: 1382
			public static readonly Color Darken3 = new Color(4294551589U);

			// Token: 0x04000567 RID: 1383
			public static readonly Color Darken4 = new Color(4294278935U);

			// Token: 0x04000568 RID: 1384
			public static readonly Color Accent1 = new Color(4294967181U);

			// Token: 0x04000569 RID: 1385
			public static readonly Color Accent2 = new Color(4294967040U);

			// Token: 0x0400056A RID: 1386
			public static readonly Color Accent3 = new Color(4294961664U);

			// Token: 0x0400056B RID: 1387
			public static readonly Color Accent4 = new Color(4294956544U);
		}

		// Token: 0x0200017B RID: 379
		public static class Amber
		{
			// Token: 0x0400056C RID: 1388
			public static readonly Color Lighten5 = new Color(4294965473U);

			// Token: 0x0400056D RID: 1389
			public static readonly Color Lighten4 = new Color(4294962355U);

			// Token: 0x0400056E RID: 1390
			public static readonly Color Lighten3 = new Color(4294959234U);

			// Token: 0x0400056F RID: 1391
			public static readonly Color Lighten2 = new Color(4294956367U);

			// Token: 0x04000570 RID: 1392
			public static readonly Color Lighten1 = new Color(4294953512U);

			// Token: 0x04000571 RID: 1393
			public static readonly Color Medium = new Color(4294951175U);

			// Token: 0x04000572 RID: 1394
			public static readonly Color Darken1 = new Color(4294947584U);

			// Token: 0x04000573 RID: 1395
			public static readonly Color Darken2 = new Color(4294942720U);

			// Token: 0x04000574 RID: 1396
			public static readonly Color Darken3 = new Color(4294938368U);

			// Token: 0x04000575 RID: 1397
			public static readonly Color Darken4 = new Color(4294930176U);

			// Token: 0x04000576 RID: 1398
			public static readonly Color Accent1 = new Color(4294960511U);

			// Token: 0x04000577 RID: 1399
			public static readonly Color Accent2 = new Color(4294956864U);

			// Token: 0x04000578 RID: 1400
			public static readonly Color Accent3 = new Color(4294951936U);

			// Token: 0x04000579 RID: 1401
			public static readonly Color Accent4 = new Color(4294945536U);
		}

		// Token: 0x0200017C RID: 380
		public static class Orange
		{
			// Token: 0x0400057A RID: 1402
			public static readonly Color Lighten5 = new Color(4294964192U);

			// Token: 0x0400057B RID: 1403
			public static readonly Color Lighten4 = new Color(4294959282U);

			// Token: 0x0400057C RID: 1404
			public static readonly Color Lighten3 = new Color(4294954112U);

			// Token: 0x0400057D RID: 1405
			public static readonly Color Lighten2 = new Color(4294948685U);

			// Token: 0x0400057E RID: 1406
			public static readonly Color Lighten1 = new Color(4294944550U);

			// Token: 0x0400057F RID: 1407
			public static readonly Color Medium = new Color(4294940672U);

			// Token: 0x04000580 RID: 1408
			public static readonly Color Darken1 = new Color(4294675456U);

			// Token: 0x04000581 RID: 1409
			public static readonly Color Darken2 = new Color(4294278144U);

			// Token: 0x04000582 RID: 1410
			public static readonly Color Darken3 = new Color(4293880832U);

			// Token: 0x04000583 RID: 1411
			public static readonly Color Darken4 = new Color(4293284096U);

			// Token: 0x04000584 RID: 1412
			public static readonly Color Accent1 = new Color(4294955392U);

			// Token: 0x04000585 RID: 1413
			public static readonly Color Accent2 = new Color(4294945600U);

			// Token: 0x04000586 RID: 1414
			public static readonly Color Accent3 = new Color(4294938880U);

			// Token: 0x04000587 RID: 1415
			public static readonly Color Accent4 = new Color(4294929664U);
		}

		// Token: 0x0200017D RID: 381
		public static class DeepOrange
		{
			// Token: 0x04000588 RID: 1416
			public static readonly Color Lighten5 = new Color(4294699495U);

			// Token: 0x04000589 RID: 1417
			public static readonly Color Lighten4 = new Color(4294954172U);

			// Token: 0x0400058A RID: 1418
			public static readonly Color Lighten3 = new Color(4294945681U);

			// Token: 0x0400058B RID: 1419
			public static readonly Color Lighten2 = new Color(4294937189U);

			// Token: 0x0400058C RID: 1420
			public static readonly Color Lighten1 = new Color(4294930499U);

			// Token: 0x0400058D RID: 1421
			public static readonly Color Medium = new Color(4294924066U);

			// Token: 0x0400058E RID: 1422
			public static readonly Color Darken1 = new Color(4294201630U);

			// Token: 0x0400058F RID: 1423
			public static readonly Color Darken2 = new Color(4293282329U);

			// Token: 0x04000590 RID: 1424
			public static readonly Color Darken3 = new Color(4292363029U);

			// Token: 0x04000591 RID: 1425
			public static readonly Color Darken4 = new Color(4290721292U);

			// Token: 0x04000592 RID: 1426
			public static readonly Color Accent1 = new Color(4294942336U);

			// Token: 0x04000593 RID: 1427
			public static readonly Color Accent2 = new Color(4294929984U);

			// Token: 0x04000594 RID: 1428
			public static readonly Color Accent3 = new Color(4294917376U);

			// Token: 0x04000595 RID: 1429
			public static readonly Color Accent4 = new Color(4292684800U);
		}

		// Token: 0x0200017E RID: 382
		public static class Brown
		{
			// Token: 0x04000596 RID: 1430
			public static readonly Color Lighten5 = new Color(4293913577U);

			// Token: 0x04000597 RID: 1431
			public static readonly Color Lighten4 = new Color(4292332744U);

			// Token: 0x04000598 RID: 1432
			public static readonly Color Lighten3 = new Color(4290554532U);

			// Token: 0x04000599 RID: 1433
			public static readonly Color Lighten2 = new Color(4288776319U);

			// Token: 0x0400059A RID: 1434
			public static readonly Color Lighten1 = new Color(4287458915U);

			// Token: 0x0400059B RID: 1435
			public static readonly Color Medium = new Color(4286141768U);

			// Token: 0x0400059C RID: 1436
			public static readonly Color Darken1 = new Color(4285353025U);

			// Token: 0x0400059D RID: 1437
			public static readonly Color Darken2 = new Color(4284301367U);

			// Token: 0x0400059E RID: 1438
			public static readonly Color Darken3 = new Color(4283315246U);

			// Token: 0x0400059F RID: 1439
			public static readonly Color Darken4 = new Color(4282263331U);
		}

		// Token: 0x0200017F RID: 383
		public static class Grey
		{
			// Token: 0x040005A0 RID: 1440
			public static readonly Color Lighten5 = new Color(4294638330U);

			// Token: 0x040005A1 RID: 1441
			public static readonly Color Lighten4 = new Color(4294309365U);

			// Token: 0x040005A2 RID: 1442
			public static readonly Color Lighten3 = new Color(4293848814U);

			// Token: 0x040005A3 RID: 1443
			public static readonly Color Lighten2 = new Color(4292927712U);

			// Token: 0x040005A4 RID: 1444
			public static readonly Color Lighten1 = new Color(4290624957U);

			// Token: 0x040005A5 RID: 1445
			public static readonly Color Medium = new Color(4288585374U);

			// Token: 0x040005A6 RID: 1446
			public static readonly Color Darken1 = new Color(4285887861U);

			// Token: 0x040005A7 RID: 1447
			public static readonly Color Darken2 = new Color(4284572001U);

			// Token: 0x040005A8 RID: 1448
			public static readonly Color Darken3 = new Color(4282532418U);

			// Token: 0x040005A9 RID: 1449
			public static readonly Color Darken4 = new Color(4280361249U);
		}

		// Token: 0x02000180 RID: 384
		public static class BlueGrey
		{
			// Token: 0x040005AA RID: 1450
			public static readonly Color Lighten5 = new Color(4293718001U);

			// Token: 0x040005AB RID: 1451
			public static readonly Color Lighten4 = new Color(4291811548U);

			// Token: 0x040005AC RID: 1452
			public static readonly Color Lighten3 = new Color(4289773253U);

			// Token: 0x040005AD RID: 1453
			public static readonly Color Lighten2 = new Color(4287669422U);

			// Token: 0x040005AE RID: 1454
			public static readonly Color Lighten1 = new Color(4286091420U);

			// Token: 0x040005AF RID: 1455
			public static readonly Color Medium = new Color(4284513675U);

			// Token: 0x040005B0 RID: 1456
			public static readonly Color Darken1 = new Color(4283723386U);

			// Token: 0x040005B1 RID: 1457
			public static readonly Color Darken2 = new Color(4282735204U);

			// Token: 0x040005B2 RID: 1458
			public static readonly Color Darken3 = new Color(4281812815U);

			// Token: 0x040005B3 RID: 1459
			public static readonly Color Darken4 = new Color(4280693304U);
		}
	}
}
