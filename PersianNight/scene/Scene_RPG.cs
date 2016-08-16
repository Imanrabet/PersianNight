using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using GameFramework;
using DxLibDLL;

namespace PersianNight
{
	//================================================================================
	//	
	//================================================================================
	[Serializable]
	public class Scene_RPG : Scene
	{
		private Drawable m_draw_ENEMY;
		private Drawable m_draw_DIN;
		public Wnd_BattleStatus m_Wnd_BattleStatus;
		public Wnd_BattleCommandPanel m_Wnd_BattleCommandPanel;
		private Drawable m_draw_DistanceMe;
		private Drawable m_draw_DistanceEnemy;
		private Drawable m_draw_GameOver;
		private string m_szEnemyName = "";
		private int m_nATB_X = 40;
		private int m_nATB_Y = 96;
		private int m_nATB_WIDTH = 5;
		private int m_nATB_Length;
		private int m_nATB_COLOR;
		private int m_nHpColor = COLOR_BLACK;
		public float fCurrentATB;
		public float fMaxATB;
		//================================================================================
		//	
		//================================================================================
		public override void Initialize()
		{
			//	基底初期化
			base.Initialize();
			//	描画設定
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "砂漠590.jpg"), 1);
			m_fDrawPos_X = 0;
			m_fDrawPos_Y = 0;
			m_nTransFlag = 0;
			m_bMouseAct = false;					//	マウス動作
			m_bMouseButtonLeftClickEnable = false;	//	左クリック動作
			m_bDefaultMouseLeftClickEvent = false;	//	左クリック動作
			m_bDragEnable = false;					//	ドラッグ処理
			m_nDragTopLeftX = 0;					//	ドラッグ範囲
			m_nDragTopLeftY = 0;					//	ドラッグ範囲
			m_nDragBottomRightX = 0;				//	ドラッグ範囲
			m_nDragBottomRightY = 0;				//	ドラッグ範囲

			//	メッセージウィンドウ
			m_Wnd_BattleCommandPanel = new Wnd_BattleCommandPanel();
			m_Wnd_BattleCommandPanel.Initialize();
			AddChildDrawable(m_Wnd_BattleCommandPanel, this);

			//	ステータスウィンドウ
			m_Wnd_BattleStatus = new Wnd_BattleStatus();
			m_Wnd_BattleStatus.Initialize();
			AddChildDrawable(m_Wnd_BattleStatus, this);

			//	距離アイコン
			m_draw_DistanceEnemy = new Drawable();
			m_draw_DistanceEnemy.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "battle_icon_3.png"), 1);
			m_draw_DistanceEnemy.nTransFlag = 1;
			m_draw_DistanceEnemy.fDrawPos_X = 10;
			m_draw_DistanceEnemy.fDrawPos_Y = 10;
			m_draw_DistanceEnemy.bMouseAct = false;					//	マウス動作
			AddChildDrawable(m_draw_DistanceEnemy, this);
			//
			m_draw_DistanceMe = new Drawable();
			m_draw_DistanceMe.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "battle_icon_2.png"), 1);
			m_draw_DistanceMe.nTransFlag = 1;
			m_draw_DistanceMe.fDrawPos_X = 112;
			m_draw_DistanceMe.fDrawPos_Y = 10;
			m_draw_DistanceMe.bMouseAct = false;					//	マウス動作
			AddChildDrawable(m_draw_DistanceMe, this);
			//
			m_draw_DIN = new Drawable();
			m_draw_DIN.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "zジン.png"), 1);
			m_draw_DIN.nTransFlag = 1;
			m_draw_DIN.fDrawPos_X = 0;
			m_draw_DIN.fDrawPos_Y = 0;
			m_draw_DIN.bDoDraw = false;
			m_draw_DIN.bMouseAct = false;					//	マウス動作
			m_draw_DIN.bMouseButtonLeftClickEnable = false;	//	左クリック動作
			m_draw_DIN.bDefaultMouseLeftClickEvent = false;	//	左クリック動作
			AddChildDrawable(m_draw_DIN, this);
			//	
			m_draw_ENEMY = new Drawable();
			m_draw_ENEMY.SetGraphicHandle(-1, 1);
			m_draw_ENEMY.nTransFlag = 1;
			m_draw_ENEMY.fDrawPos_X = 0;
			m_draw_ENEMY.fDrawPos_Y = 0;
			m_draw_ENEMY.bMouseAct = false;					//	マウス動作
			m_draw_ENEMY.bMouseButtonLeftClickEnable = true;	//	左クリック動作
			m_draw_ENEMY.bDefaultMouseLeftClickEvent = true;	//	左クリック動作
			m_draw_ENEMY.InnerPostEventArgs = new InnerPostEventArgs(EnumGUIEvent.CLICK_MEMORY);
			AddChildDrawable(m_draw_ENEMY, this);
			//
			m_draw_GameOver = new Drawable();
			m_draw_GameOver.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "gameover.png"), 1);
			m_draw_GameOver.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "gameover.png"), 2);
			m_draw_GameOver.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "gameover.png"), 3);
			m_draw_GameOver.bDoDraw = false;
			m_draw_GameOver.nTransFlag = 0;
			m_draw_GameOver.fDrawPos_X = 0;
			m_draw_GameOver.fDrawPos_Y = 0;
			m_draw_GameOver.bMouseAct = true;					//	マウス動作
			m_draw_GameOver.bMouseButtonLeftClickEnable = true;	//	左クリック動作
			m_draw_GameOver.bDefaultMouseLeftClickEvent = true;	//	左クリック動作
			m_draw_GameOver.InnerPostEventArgs = new InnerPostEventArgs(EnumGUIEvent.GAME_OVER);
			AddChildDrawable(m_draw_GameOver, this);
			//
			AddEventFunc(EnumPanelEvent.SUBTARGET, (InnerPostEventHandler)SUBTARGET);
			AddEventFunc(EnumPanelEvent.CANCEL_SUBTARGET, (InnerPostEventHandler)CANCEL_SUBTARGET);
			AddEventFunc(EnumPanelEvent.ACTION, (InnerPostEventHandler)ACTION);
		}
		//================================================================================
		//	
		//================================================================================
		public override void Update( )
		{
			base.Update( );
			if ( GamePersianNight.bIsBattle == true )
			{
				fMaxATB = MD_GameConstants.MAX_ACT;
				fCurrentATB = DD_BattleCharacter.GetCharacterParameter(2, EnumCharacterParameter.CURRENT_ATB);
			}
		}
		//================================================================================
		//	
		//================================================================================
		public override void Draw()
		{
			//	
			base.Draw();
			//	ゲームオーバー画面のときはこれ以上表示しない
			if (m_draw_GameOver.bDoDraw == true) return;
			int fh = GamePersianNight.s_nFONT_HANDLE_16;
			//	中ボス戦までの距離
			DX.DrawStringToHandle(150, 10, "中ボスとの距離 " + GamePersianNight.nMiddleBossDistance.ToString(), COLOR_BLACK, fh, COLOR_WHITE);
			//	追跡者との距離
			DX.DrawStringToHandle(150, 30, "追跡者との距離 " + GamePersianNight.nCurrentDistance.ToString(), COLOR_BLACK, fh, COLOR_WHITE);
			//	オアシスまでの距離
			DX.DrawStringToHandle(300, 10, "オアシスまでの距離 " + GamePersianNight.nOassisDistance.ToString(), COLOR_BLACK, fh, COLOR_WHITE);
			//	DOA表示
			DX.DrawBox(25, 45, 126, 53, COLOR_WHITE, 0);
			float f = 100f * ((float)GamePersianNight.nDistanceOnAct / (float)MD_GameConstants.MAX_ACT);
			DX.DrawBox(26, 46, (int)f + 26, 52, COLOR_RED, 1);
			//	アイコン位置更新
			m_draw_DistanceEnemy.fDrawPos_X = 11 + (((float)MD_GameConstants.CHASER_DISTANCE - (float)GamePersianNight.nCurrentDistance) / (float)MD_GameConstants.CHASER_DISTANCE) * 100f;
			//	バトル中なら以下表示
			if ( GamePersianNight.bIsBattle == true )
			{
				//	敵名称
				DX.DrawStringToHandle(5, 60, m_szEnemyName.ToString(), COLOR_BLACK, fh);
				//	敵HP
				DX.DrawStringToHandle(5, 76, "H P: "+DD_BattleCharacter.GetCharacterParameter(2, EnumCharacterParameter.CURRENT_HP).ToString() + " / " + DD_BattleCharacter.GetCharacterParameter(2, EnumCharacterParameter.MAX_HP).ToString(), COLOR_BLACK, fh);
				//	敵ATB
				DX.DrawStringToHandle(5, 92, "ATB: ", COLOR_BLACK, fh);
//				m_nATB_X = m_nDrawPosScreen_X + 45 + 32;
//				m_nATB_Y = m_nDrawPosScreen_Y + INDEX_TO_Y[04] + 4;
//				m_nATB_WIDTH = 5;
				int m_nATB_Length = (int)( fCurrentATB / fMaxATB * 100);
				if (m_nATB_Length >= 100) { m_nATB_Length = 100; m_nATB_COLOR = COLOR_RED; } else { m_nATB_COLOR = COLOR_CYAN; }
				DX.DrawBox(m_nATB_X + 0, m_nATB_Y + 0, m_nATB_X + m_nATB_Length + 0, m_nATB_Y + 0 + m_nATB_WIDTH, m_nATB_COLOR, 1);
				DX.DrawBox(m_nATB_X - 1, m_nATB_Y - 1, m_nATB_X + 0000000000100 + 1, m_nATB_Y + 1 + m_nATB_WIDTH, COLOR_WHITE, 0);
				//	戦利品
				string szDrop = MD_Monsters.GetMonstersFromId(DD_BattleCharacter.GetCharacterParameter(2, EnumCharacterParameter.MONSTER_ID)).GetDropItemName();
				DX.DrawStringToHandle(5, 108, "戦利品：" + szDrop, COLOR_BLACK, fh);
			}
		}
		//================================================================================
		//	
		//================================================================================
		public override void EnterScene()
		{
			base.EnterScene( );
			//ゲームオーバー画面は消す
			m_draw_GameOver.bDoDraw = false;
			//　背景の差し替え
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + GamePersianNight.CurrentScenarios.szBattleBack), 1);
			//	ステータスウィンドウのセットアップ
			m_Wnd_BattleStatus.Setup();
			//	コマンドパネルのセットアップ
			m_Wnd_BattleCommandPanel.Setup();
			//	とりあえず進む場面から
			SetPrimaryInput();
			//	ガベコレ
			GC.Collect( );
		}
		//================================================================================
		/// <summary>ステータスパネルを更新する</summary>
		//================================================================================
		public void StatusUpdate()
		{
			//	ステータスウィンドウのセットアップ
			m_Wnd_BattleStatus.Setup();
		}
		//================================================================================
		/// <summary>バトル突入処理</summary>
		//================================================================================
		public void EnterBattle()
		{
			m_Wnd_BattleCommandPanel.EnterBattle();
			//	敵イメージを設定
			int nMonsterId = DD_BattleCharacter.GetCharacterParameter(2, EnumCharacterParameter.MONSTER_ID);
			string szMonsterImage = MD_Monsters.GetMonstersFromId(nMonsterId).szImage;
			m_draw_ENEMY.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + szMonsterImage), 1);
			//	敵名称を設定
			m_szEnemyName = MD_Monsters.GetMonstersFromId(nMonsterId).szName;
			//	パネルに敵の出現を表示
			m_Wnd_BattleCommandPanel.SetDescription(0, GamePersianNight.CurrentMonster.szName + " があらわれた！");
			//	コマンド更新
			if (GamePersianNight.bIsBoss == false)
			{
				m_Wnd_BattleCommandPanel.SetButtonString(0, "逃げる", EnumPanelEvent.ESCAPE,null);
				m_Wnd_BattleCommandPanel.SetButtonEnable(0, true);
			}
			m_Wnd_BattleCommandPanel.SetButtonString(1, "戦う", EnumPanelEvent.BATTLE,null);
			m_Wnd_BattleCommandPanel.SetButtonEnable(1, true);
		}
		//================================================================================
		/// <summary>オアシス突入処理</summary>
		//================================================================================
		public void EnterOassis()
		{
			//背景を変更
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "オアシス.jpg"), 1);
			//	
			m_Wnd_BattleCommandPanel.SetDescription(0, "オアシスのようだ");
			//	コマンド更新
			m_Wnd_BattleCommandPanel.SetButtonString(0, "休む（HP回復、追跡者との距離-1）", EnumPanelEvent.REST, null);
			m_Wnd_BattleCommandPanel.SetButtonEnable(0, true);
			m_Wnd_BattleCommandPanel.SetButtonString(1, "先に進む", EnumPanelEvent.PROCEED, null);
			m_Wnd_BattleCommandPanel.SetButtonEnable(1, true);
			m_Wnd_BattleCommandPanel.SetButtonString(2, "セーブする", EnumPanelEvent.SAVE, null);
			m_Wnd_BattleCommandPanel.SetButtonEnable(2, true);
		}
		//================================================================================
		/// <summary>ジン突入処理</summary>
		//================================================================================
		public void EnterDin()
		{
			//	パネルを綺麗に
			m_Wnd_BattleCommandPanel.Setup();
			//ジンを表示
			m_draw_DIN.bDoDraw = true;
			//	
			m_Wnd_BattleCommandPanel.SetDescription(0, "助けはいるかい？");
			m_Wnd_BattleCommandPanel.SetDescription(1, "もちろん対価は必要だよ");
			//
			List<MD_Dins> list = MD_Dins.GetDinList(GamePersianNight.CurrentScenarios.nId);
			for (int i = 0; i < 3;i++ )
			{
				//所持金足りてるかチェック
				if (GamePersianNight.nMoney < list[i].nPrice)
				{
					m_Wnd_BattleCommandPanel.SetButtonString(i, list[i].szName + "　　[" + list[i].nPrice + "]", EnumPanelEvent.DIN, list[i].nId, COLOR_RED);
				}
				else
				{
					//フェニックスの尾は１回のみ
					if (list[i].nType == 4 && GamePersianNight.Phoenix.Count != 0)
					{
						m_Wnd_BattleCommandPanel.SetButtonString(i, list[i].szName + "　　[" + list[i].nPrice + "]", EnumPanelEvent.DIN, list[i].nId, COLOR_RED);
					}
					else
					{
						m_Wnd_BattleCommandPanel.SetButtonString(i, list[i].szName + "　　[" + list[i].nPrice + "]", EnumPanelEvent.DIN, list[i].nId, COLOR_BLACK);
					}
				}
				m_Wnd_BattleCommandPanel.SetButtonEnable(i, true);
			}
			m_Wnd_BattleCommandPanel.SetButtonString(3, "もう十分だ", EnumPanelEvent.BYE_DIN,null , COLOR_BLACK);
			m_Wnd_BattleCommandPanel.SetButtonEnable(3, true);
			//	キャラグラ更新
			m_Wnd_BattleCommandPanel.SetCharacterFace(MD_Characters.GetCharactersFromID(9999));
		}
		//================================================================================
		/// <summary>バトル終了処理</summary>
		//================================================================================
		public void ExitBattle()
		{
			//	敵イメージを設定
			m_draw_ENEMY.SetGraphicHandle(-1, 1);
			//	敵名称を設定
			m_szEnemyName = "";
			m_Wnd_BattleStatus.ExitBattle();
		}
		//================================================================================
		/// <summary>オアシス終了処理</summary>
		//================================================================================
		public void ExitOassis()
		{
			//　背景の差し替え
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + GamePersianNight.CurrentScenarios.szBattleBack), 1);
			SetPrimaryInput();
		}
		//================================================================================
		/// <summary>ジン終了処理</summary>
		//================================================================================
		public void ExitDin()
		{
			//	パネルを綺麗に
			m_Wnd_BattleCommandPanel.Setup();
			//ジンを非表示
			m_draw_DIN.bDoDraw = false;
		}
		//================================================================================
		/// <summary>ゲームオーバー画面を表示する</summary>
		//================================================================================
		public void GameOver()
		{
			m_draw_GameOver.bDoDraw = true;
		}
		//================================================================================
		/// <summary>バトル関連表示を消す</summary>
		//================================================================================
		public void ClearBattle()
		{
			//	パネルの更新
			m_Wnd_BattleCommandPanel.Setup();
			//	モンスイメージ消去
			m_draw_ENEMY.SetGraphicHandle(-1, 1);
			//	標準入力状態へ
			SetPrimaryInput();
		}
		//================================================================================
		/// <summary>標準入力状態へ</summary>
		//================================================================================
		public void SetPrimaryInput()
		{
			ClearPanel(true);
			m_Wnd_BattleCommandPanel.SetDescription(0, "主人公");
			m_Wnd_BattleCommandPanel.SetDescription(1, "「どうする？");
			m_Wnd_BattleCommandPanel.SetButtonString(0, "前にすすむ", EnumPanelEvent.PROCEED,null);
			m_Wnd_BattleCommandPanel.SetButtonEnable(0, true);
			m_Wnd_BattleCommandPanel.SetCharacterFace(GamePersianNight.CurrentHero);
		}
		//================================================================================
		/// <summary>バトル入力状態へ</summary>
		//================================================================================
		public void BattleCommandInput(int nDDIndex)
		{
			//	パネルを綺麗に
			m_Wnd_BattleCommandPanel.Setup();
			//	コマンド更新
			m_Wnd_BattleCommandPanel.SetDescription(0, MD_Characters.GetCharactersFromID(DD_BattleCharacter.GetCharacterParameter(nDDIndex, EnumCharacterParameter.CHARACTER_ID)).szName);
			m_Wnd_BattleCommandPanel.SetDescription(1, "「どうする？");
			for (int i = 0; i < 4;i++ )
			{
				int nCharId = DD_BattleCharacter.GetCharacterParameter(nDDIndex, EnumCharacterParameter.CHARACTER_ID);
				int nActionId = MD_Characters.GetCharactersFromID(nCharId).list_ActionId[i];
				if (MD_Actions.GetActionsFromID(nActionId) == null) { GamePersianNight.DebugLog("無効なアクションIDが設定されている (" + nActionId.ToString() + ")\r\n"); continue; }
				string szActionName = MD_Actions.GetActionsFromID(nActionId).szName;
				szActionName += "　(TP: " + MD_Actions.GetActionsFromID(nActionId).nTp.ToString() + " )";
				//サブターゲットのいるコマンドか
				bool bNeedSubTarget = false;
				if (MD_Actions.GetActionsFromID(nActionId).nTarget == 2) bNeedSubTarget = true;
				//TP足りてるかチェック
				if (DD_BattleCharacter.GetCharacterParameter(nDDIndex, EnumCharacterParameter.CURRENT_TP) < MD_Actions.GetActionsFromID(nActionId).nTp)
				{
					m_Wnd_BattleCommandPanel.SetButtonString(i, szActionName, EnumPanelEvent.ACTION, new int[] { nActionId, -1 }, COLOR_RED);
				}
				else
				{
					//サブターゲットが必要ならイベントを差し替える
					if (bNeedSubTarget == true)
					{
						m_Wnd_BattleCommandPanel.SetButtonString(i, szActionName, EnumPanelEvent.SUBTARGET, new int[] { nActionId, -1 }, COLOR_BLACK);
					}
					else
					{
						m_Wnd_BattleCommandPanel.SetButtonString(i, szActionName, EnumPanelEvent.ACTION, new int[] { nActionId, -1 }, COLOR_BLACK);
					}
				}
				m_Wnd_BattleCommandPanel.SetButtonEnable(i, true);
			}
			//	キャラグラ更新
			m_Wnd_BattleCommandPanel.SetCharacterFace(DD_BattleCharacter.GetCharacterParameter(nDDIndex, EnumCharacterParameter.CHARACTER_ID));
		}
		//==================================================================================
		//	サブターゲットコマンドをフックする
		//==================================================================================
		private void SUBTARGET(object sender, InnerPostEventArgs e)
		{
			//ステータス部分のサブターゲット機能をオンにする
			m_Wnd_BattleStatus.SwitchSubtargetFunc(true, (int[])e.oGenericObject);
			//サブターゲット選択中はコマンド隠す
			int[] n = (int[])e.oGenericObject;
			string sz = MD_Actions.GetActionsFromID(n[0]).szName;
			m_Wnd_BattleCommandPanel.DoSubtarget(sz);
		}
		//==================================================================================
		//	サブターゲットキャンセル
		//==================================================================================
		private void CANCEL_SUBTARGET(object sender, InnerPostEventArgs e)
		{
			//ステータス部分のサブターゲット機能をオフにする
			m_Wnd_BattleStatus.SwitchSubtargetFunc(false, null);
			//コマンドパネルの復帰
			m_Wnd_BattleCommandPanel.RevertSubtarget();
		}
		//==================================================================================
		//	サブターゲットから実行されることがあるので、サブターゲット機能をオフする
		//==================================================================================
		private void ACTION(object sender, InnerPostEventArgs e)
		{
			//ステータス部分のサブターゲット機能をオフにする
			m_Wnd_BattleStatus.SwitchSubtargetFunc(false, null);
		}
		//================================================================================
		/// <summary>バトル結果表示</summary>
		//================================================================================
		public void ShowLog(int nLineIndex,string szLog)
		{
			m_Wnd_BattleCommandPanel.SetDescription(nLineIndex, szLog);
		}
		//================================================================================
		/// <summary>パネルをクリック可能に</summary>
		//================================================================================
		public void PanelEnable()
		{
			//パネルをクリック可能に
			m_Wnd_BattleCommandPanel.bMouseAct = true;
			m_Wnd_BattleCommandPanel.InnerPostEventArgs = new GameFramework.InnerPostEventArgs(EnumPanelEvent.RESUME);
		}
		//================================================================================
		/// <summary>パネルのイベントを設定</summary>
		//================================================================================
		public void SetPanelEvent(EnumPanelEvent epe)
		{
			//パネルをクリック可能に
			m_Wnd_BattleCommandPanel.bMouseAct = true;
			m_Wnd_BattleCommandPanel.InnerPostEventArgs = new GameFramework.InnerPostEventArgs(epe);
		}
		//================================================================================
		/// <summary>パネル表示を綺麗に</summary>
		//================================================================================
		public void ClearPanel(bool bIsFaceExist)
		{
			//	パネルを綺麗に
			m_Wnd_BattleCommandPanel.Setup();
			if (bIsFaceExist == false) m_Wnd_BattleCommandPanel.SetCharacterFace(null);
			//パネルクリック不可に戻す
			m_Wnd_BattleCommandPanel.bMouseAct = false;

		}
	}
}
