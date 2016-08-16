using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using GameFramework;
using DxLibDLL;

namespace PersianNight
{
	public enum EnumGUIEvent
	{
		CLICK_START,
		CLICK_SELECT_SCENARIO,
		GAME_OVER,
		EQUIP_CHANGE,	//ジェネリックオブジェクトのindex詳細 0:DDキャラindex 1:カテゴリ 2:ローテート方向

		RETURN_TO_TITLE,
		EVENT_EXIT,
		CLICK_SELECT,
		CLICK_LOAD,
		GAME_CLEAR,
		CLICK_MEMORY,
		CLICK_SELECT_MEMORY,
		CLICK_CGMODE
	}
	public enum EnumScenarioStatus
	{
		ADV,
		RPG
	}
	[Serializable]
	//==================================================================================
	//	
	//==================================================================================
	public class GamePersianNight : GameMain
	{
		//	リソースパス
		static public string s_szGraphicPath = ".\\image\\";
		static public string s_szSoundPath = ".\\sound\\";
		static public string s_sztextPath = ".\\text\\";
		static public string s_szCsvPath = ".\\csv\\";
		static public int DEFAULT_FONT_SIZE = 24;	//	標準フォントサイズ
		//	フォントハンドル
		static public int s_nFONT_HANDLE_12;
		static public int s_nFONT_HANDLE_16;
		static public int s_nFONT_HANDLE_24;
		//	スクリーンサイズ
		static public int s_nSCREEN_WIDTH = 800;
		static public int s_nSCREEN_HEIGHT = 480;
		//	シーンオブジェクト
		private Scene_TITLE m_Scene_TITLE;		//	タイトル画面
		private Scene_RPG m_Scene_RPG;	//	バトル画面
		private Scene_SELECT_SCENARIO m_Scene_SELECT_SCENARIO;	//	シナリオ選択画面
		private Scene_ADV m_Scene_ADV;

		/// <summary>セーブデータ</summary>
		private DD_SaveData m_DD_SaveData = new DD_SaveData( );
		static public int s_nHANDLE_BGM_01_mp3;
		static public int s_nHANDLE_BGM_ERO2_mp3;
		static public int s_nHANDLE_BGM_SELECT_mp3;
		static public int s_nHANDLE_SE_CLICK;
		static public int s_nHANDLE_SE_CLEAR;
		static public int s_nHANDLE_SE_GAMEOVER;
		static public int s_nHANDLE_SE_ENGINE_HIGH;
		static public int s_nHANDLE_SE_ENGINE_LOW;
		//==================================================================================
		//	動的データ
		//==================================================================================
		static private string s_szCurrentScriptFileName = "";	//	現在の実行スクリプトファイル名
		static private List<int> s_list_CleardScenarioId;		//	クリア済シナリオリスト
		static private int s_nCurrentScenarioId;				//	現在実行中のシナリオID
		static private int s_nCurrentWalk;						//	現在の歩数
		/// <summary>現在までのトータル歩行数</summary>
		static public int nCurrentWalk { get { return s_nCurrentWalk; } }
		static private int s_nMiddleBossDistance;
		/// <summary>中ボスまでの距離</summary>
		static public int nMiddleBossDistance { get { return s_nMiddleBossDistance; } }
		static private bool s_bIsGamePlaying = false;			//	現在シナリオ攻略中かどうか
		static private EnumScenarioStatus s_EnumScenarioStatus;
		static private EnumADVStatus s_CurrentEnumADVStatus;
		/// <summary>追跡者との距離</summary>
		static private int s_nCurrentDistance;
		/// <summary>追跡者との距離</summary>
		static public int nCurrentDistance { get { return s_nCurrentDistance; } }
		static private int s_nDistanceOnAct;					//	１行動ごとに縮まる追跡者との距離蓄積
		/// <summary>オアシスまでの距離</summary>
		static public int nOassisDistance;
		/// <summary>１行動ごとに縮まる追跡者との距離蓄積</summary>
		static public int nDistanceOnAct { get { return s_nDistanceOnAct; } }
		static private MD_Scenarios s_CurrentScenarios;			//	実行するシナリオ
		static public MD_Scenarios CurrentScenarios { get { return s_CurrentScenarios; } }
		static private MD_Characters s_CurrentHero;				//	このシナリオでの主人公
		static public MD_Characters CurrentHero { get { return s_CurrentHero; } }
		static private MD_Characters s_CurrentHeroin;				//	このシナリオでのヒロイン
		static public MD_Characters CurrentHeroin { get { return s_CurrentHeroin; } }
		/// <summary>現在のシナリオでのイベントリスト</summary>
		static private List<MD_Events> s_CurrentScenarioEvents;
		/// <summary>現在発生しているイベント</summary>
		static private MD_Events s_CuurentEvent;
		/// <summary>現在発生しているイベント</summary>
		static public MD_Events CuurentEvent { get { return s_CuurentEvent; } }
		/// <summary>次回発生イベントのindex</summary>
		static private int s_nNextEventIndex;
		static private int s_nMoney = 0;						//	所持金
		static public int nMoney { get { return s_nMoney; } }
		static private int s_nPartyActGauge = 0;
		/// <summary>パーティACTゲージ。行動ごとに少しずつたまり、ゲージマックスまで貯まると追跡者との距離が１縮む</summary>
		static public int nPartyActGauge { get { return s_nPartyActGauge; } }
		static private MD_Monsters s_CurrentMonster = null;
		/// <summary>遭遇中の敵ベースデータ</summary>
		static public MD_Monsters CurrentMonster { get { return s_CurrentMonster; } }
		/// <summary>コマンド実行中のDDインデックス</summary>
		static private int s_nCommandCharaIndex;
		/// <summary>コマンド実行のターゲットとなるDDインデックス</summary>
		static private List<int> s_list_CommandTargetIndex;
		static private int s_nLogIndex;
		/// <summary>次回遭遇ボスのインデックス</summary>
		static private int s_nBossIndex;
		/// <summary>フェニックスの尾。count=0なら持ってない。count=1なら持ってる true=未使用 false=使用済</summary>
		static private List<bool> s_Phoenix;
		/// <summary>フェニックスの尾。count=0なら持ってない。count=1なら持ってる true=未使用 false=使用済</summary>
		static public List<bool> Phoenix { get { return s_Phoenix; } }
		//==================================================================================
		//	バトル用動的データ
		//==================================================================================
		/// <summary>攻撃側ダメージ値</summary>
		static private int s_nBattle_A_Damage;
		/// <summary>攻撃側ATK</summary>
		static private int s_nBattle_A_Atk;
		/// <summary>攻撃側武器D補正</summary>
		static private int s_nBattle_A_WeaponDamage;
		/// <summary>攻撃側武器ATK補正</summary>
		static private int s_nBattle_A_WeaponAtk;
		/// <summary>防御側DEF</summary>
		static private int s_nBattle_D_Def;
		/// <summary>防御側防具DEF補正</summary>
		static private int s_nBattle_D_ArmorDef;
		/// <summary>アクションID</summary>
		static private int s_nActionId;
		//==================================================================================
		//	ステータス管理用動的データ
		//==================================================================================
		/// <summary>バトル中かどうか</summary>
		static public bool bIsBattle = false;
		/// <summary>ポーズ中かどうか。ポーズ中はATBは増加しない</summary>
		static public bool bIsPause = false;
		/// <summary>コマンド選択待機フラグ。ATBバーの更新のためだけにある</summary>
		static public bool bIsReadySelectCommand = false;
		/// <summary>ボス戦</summary>
		static public bool bIsBoss = false;
		//==================================================================================
		//	
		//==================================================================================
		public override void Initialize()
		{
			SplashForm splashForm = new SplashForm();
			splashForm.Show();		//	スプラッシュ表示
			___bDEBUG___ = true;	//	デバッグモード設定
			InitDebugWindow();		//	デバッグウィンドウ生成
			string szSubTitle = "";
			if (___bDEBUG___ == true) szSubTitle = " for debug";
			//------------------------------------------------
			//	DXLIB関連初期化
			//------------------------------------------------
			base.m_szGAME_TITLE = "ペルシアンナイト ver 0.01" + szSubTitle;		//	ウインドウタイトル設定
			DX.SetDoubleStartValidFlag(1);
			base.Initialize();
			DX.SetMouseDispFlag( 1 );
			DX.SetGraphMode(s_nSCREEN_WIDTH, s_nSCREEN_HEIGHT, 16);		//	画面モード変更
//			DX.ChangeFont("モトヤLマルベリ3等幅");	//	font.dat のファイル名をフォントファイルとしてGameFramework側で事前にロードしている
			int n = DX.ChangeFont("源暎ゴシックM Regular");	//	font.dat のファイル名をフォントファイルとしてGameFramework側で事前にロードしている
			DX.ChangeFontType(DX.DX_FONTTYPE_ANTIALIASING);
			DX.SetFontSize(DEFAULT_FONT_SIZE);
			DX.SRand(Environment.TickCount);
			DX.SetUseDXArchiveFlag(1);
			//------------------------------------------------
			//	NOW LOADING
			//------------------------------------------------
			DX.SetDrawScreen(DX.DX_SCREEN_BACK);
			DX.ClsDrawScreen();
			DX.DrawString(0, 0, "NOW LOADING.....", 0xFFFFFF);
			DX.ScreenFlip();
			//------------------------------------------------
			//	高速フォントテスト
			//------------------------------------------------
//			s_nFONT_HANDLE_12 = DX.CreateFontToHandle("モトヤLマルベリ3等幅", 12, -1, DX.DX_FONTTYPE_ANTIALIASING);
//			s_nFONT_HANDLE_16 = DX.CreateFontToHandle("モトヤLマルベリ3等幅", 16, -1, DX.DX_FONTTYPE_ANTIALIASING);
//			s_nFONT_HANDLE_24 = DX.CreateFontToHandle("モトヤLマルベリ3等幅", 24, -1, DX.DX_FONTTYPE_ANTIALIASING);
//			s_nFONT_HANDLE_16 = DX.CreateFontToHandle("源暎ゴシックM Regular", 15, 0, DX.DX_FONTTYPE_ANTIALIASING | DX.DX_FONTTYPE_EDGE);
			s_nFONT_HANDLE_16 = DX.CreateFontToHandle("源暎ゴシックM Regular", 15, 0, DX.DX_FONTTYPE_ANTIALIASING);
			//------------------------------------------------
			//	サウンド関連の初期化
			//------------------------------------------------
			s_nHANDLE_BGM_01_mp3 = DX.LoadSoundMem( s_szSoundPath + "arara.ogg" );
			//------------------------------------------------
			//	イベント登録
			//------------------------------------------------
			AddEventFunc(EnumGUIEvent.CLICK_START, (InnerPostEventHandler)CLICK_START);
			AddEventFunc(EnumGUIEvent.CLICK_LOAD, (InnerPostEventHandler)CLICK_LOAD);
			AddEventFunc(EnumGUIEvent.CLICK_SELECT_SCENARIO, (InnerPostEventHandler)CLICK_SELECT_SCENARIO);
			AddEventFunc(EnumGUIEvent.EVENT_EXIT, (InnerPostEventHandler)EVENT_EXIT);
			AddEventFunc(EnumPanelEvent.PROCEED, (InnerPostEventHandler)PROCEED);
			AddEventFunc(EnumPanelEvent.ESCAPE, (InnerPostEventHandler)ESCAPE);
			AddEventFunc(EnumPanelEvent.BATTLE, (InnerPostEventHandler)BATTLE);
			AddEventFunc(EnumPanelEvent.ACTION, (InnerPostEventHandler)ACTION);
			AddEventFunc(EnumPanelEvent.RESUME, (InnerPostEventHandler)RESUME);
			AddEventFunc(EnumPanelEvent.ENEMY_DEAD, (InnerPostEventHandler)ENEMY_DEAD);
			AddEventFunc(EnumPanelEvent.PARTY_DEAD, (InnerPostEventHandler)PARTY_DEAD);
			AddEventFunc(EnumGUIEvent.GAME_OVER, (InnerPostEventHandler)GAME_OVER);
			AddEventFunc(EnumGUIEvent.EQUIP_CHANGE, (InnerPostEventHandler)EQUIP_CHANGE);
			AddEventFunc(EnumPanelEvent.REST, (InnerPostEventHandler)REST);
			AddEventFunc(EnumPanelEvent.DIN, (InnerPostEventHandler)DIN);
			AddEventFunc(EnumPanelEvent.BYE_DIN, (InnerPostEventHandler)BYE_DIN);
			AddEventFunc(EnumPanelEvent.SAVE, (InnerPostEventHandler)SAVE);

			//------------------------------------------------
			//	動的データ初期化
			//------------------------------------------------
			s_list_CleardScenarioId = new List<int>();
			s_list_CommandTargetIndex = new List<int>();
			s_Phoenix = new List<bool>();
			s_CurrentScenarioEvents = new List<MD_Events>();
			//------------------------------------------------
			//	セーブデータ読み込み
			//------------------------------------------------
//			if( File.Exists( "savedat" ) == true )
//			{
//				FileStream fs = new FileStream( "savedat", FileMode.Open, FileAccess.Read );
//				BinaryFormatter f = new BinaryFormatter( );
//				object obj = f.Deserialize( fs );
//				fs.Close( );
//				m_SaveData = ( SaveData )obj;
//			}
			//------------------------------------------------
			//	マスターデータデータの初期化
			//------------------------------------------------
			if (MD_GameConstants.Initialize() == false) DebugLog("game_constants インスタンス化失敗.\r\n");
			if (MD_Actions.Initialize() == false) DebugLog("actions インスタンス化失敗.\r\n");
			if (MD_Characters.Initialize() == false) DebugLog("characters インスタンス化失敗.\r\n");
			if (MD_Dins.Initialize() == false) DebugLog("dins インスタンス化失敗.\r\n");
			if (MD_EnemyPopups.Initialize() == false) DebugLog("enemy_popups インスタンス化失敗.\r\n");
			if (MD_Events.Initialize() == false) DebugLog("events インスタンス化失敗.\r\n");
			if (MD_Items.Initialize() == false) DebugLog("items インスタンス化失敗.\r\n");
			if (MD_LevelTables.Initialize() == false) DebugLog("level_tables インスタンス化失敗.\r\n");
			if (MD_Monsters.Initialize() == false) DebugLog("monsters インスタンス化失敗.\r\n");
			if (MD_Scenarios.Initialize() == false) DebugLog("scenarios インスタンス化失敗.\r\n");

			//------------------------------------------------
			//	各シーン初期化
			//------------------------------------------------

			m_Scene_TITLE = new Scene_TITLE ( );
			AddScene(m_Scene_TITLE);
			m_Scene_TITLE.Initialize();

			m_Scene_RPG = new Scene_RPG();
			AddScene(m_Scene_RPG);
			m_Scene_RPG.Initialize();

			m_Scene_SELECT_SCENARIO = new Scene_SELECT_SCENARIO();
			AddScene(m_Scene_SELECT_SCENARIO);
			m_Scene_SELECT_SCENARIO.Initialize();

			m_Scene_ADV = new Scene_ADV();
			AddScene(m_Scene_ADV);
			m_Scene_ADV.Initialize();

			//------------------------------------------------
			//	シーン開始
			//------------------------------------------------
			splashForm.Close();
			ChangeScene ( m_Scene_TITLE );

		}
		//==================================================================================
		//	
		//==================================================================================
		public override void Dispose()
		{
			base.Dispose();
			DX.DeleteFontToHandle(s_nFONT_HANDLE_12);
			DX.DeleteFontToHandle(s_nFONT_HANDLE_16);
			DX.DeleteFontToHandle(s_nFONT_HANDLE_24);
		}
		//==================================================================================
		//	
		//==================================================================================
		public override void Update()
		{
			base.Update();
			//	戦闘中ならATB進行
			if (bIsBattle == true && bIsPause == false && bIsReadySelectCommand == false )
			{
				DD_BattleCharacter.IncATB();		//ATBを増加
				//	コマンドスタックにキャラが積まれていたらATBバーを反映させるために１サイクル待つコマンド選択待機フラグを立てる
				s_nCommandCharaIndex = DD_BattleCharacter.ExistCommandStackCharacter();
				if (s_nCommandCharaIndex != -1) bIsReadySelectCommand = true;
				return;
			}
			//	コマンド選択待機フラグが立っていればコマンド選択を行う
			if ( bIsReadySelectCommand == true )
			{
				bIsReadySelectCommand = false;		//	コマンド選択待機フラグを解除
				bIsPause = true;					//	ポーズ状態にしてATB増加を止める
				s_nCommandCharaIndex = DD_BattleCharacter.ExistCommandStackCharacter();	//	コマンドスタックからインデックスを再取得
				//	戦闘不能になってないかチェック
				if(DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex,EnumCharacterParameter.CURRENT_HP)<=0)
				{
					DD_BattleCharacter.ResetATB(s_nCommandCharaIndex);	//ATBをリセット
					DD_BattleCharacter.DeleteCommandStackFirst();		//戦闘不能ならスタックから削除
				}
				//	ターンで終了するバフの更新
				DD_BattleCharacter.UpadateBuffTurn(s_nCommandCharaIndex);
				//TP増加
				DD_BattleCharacter.IncTp(s_nCommandCharaIndex, MD_GameConstants.TP_ATB_INC);
				//	コマンド選択
				if (s_nCommandCharaIndex == 0 || s_nCommandCharaIndex == 1)		//	プレイヤーキャラか敵か
				{
					DD_BattleCharacter.DeleteCommandStackFirst();				//	スタックから戦闘を削除する
					m_Scene_RPG.BattleCommandInput(s_nCommandCharaIndex);	//	戦闘コマンド入力状態へ
				}
				else
				{
					//	敵キャラクターの場合
					DD_BattleCharacter.DeleteCommandStackFirst();				//	スタックから戦闘を削除する
					//	可能性のあるアクションをリスト化
					List<int> list = new List<int>();
					list.Add(MD_Monsters.GetMonstersFromId(DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.MONSTER_ID)).nActionId_1);
					list.Add(MD_Monsters.GetMonstersFromId(DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.MONSTER_ID)).nActionId_2);
					list.Add(MD_Monsters.GetMonstersFromId(DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.MONSTER_ID)).nActionId_3);
					list.Add(MD_Monsters.GetMonstersFromId(DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.MONSTER_ID)).nActionId_4);
					//	アクションをランダムで選択
					int nac = list[DX.GetRand(3)];
					InnerPostEventArgs ipea = new InnerPostEventArgs(null);
					ipea.oGenericObject = new int[]{nac,-1};
					ACTION(null, ipea);											//	敵の場合の攻撃実行
				}
			}
		}
		//==================================================================================
		//	
		//==================================================================================
		private void CLICK_START(object sender, InnerPostEventArgs e)
		{
//			DX.PlaySoundMem( GameHighace.s_nHANDLE_SE_CLICK, DX.DX_PLAYTYPE_NORMAL );
			DebugLog("EnumGUIEvent.CLICK_START\r\n");
			DebugLog("シナリオ選択画面へ.\r\n");
			ChangeScene(m_Scene_SELECT_SCENARIO);
			return;
		}
		//==================================================================================
		//	
		//==================================================================================
		private void CLICK_SELECT_SCENARIO(object sender, InnerPostEventArgs e)
		{
			DebugLog("EnumGUIEvent.CLICK_SELECT_SCENARIO\r\n");
			DebugLog("シナリオ選択押下.\r\n");
			int nScenarioId = -1;
			switch ((int)(e.oGenericObject))
			{
				case 1: nScenarioId = MD_GameConstants.SCENARIO_1; break;
				case 2: nScenarioId = MD_GameConstants.SCENARIO_2; break;
				case 3: nScenarioId = MD_GameConstants.SCENARIO_3; break;
				case 4: nScenarioId = MD_GameConstants.SCENARIO_4; break;
				default: break;
			}
			//	シナリオ初期化
			InitScenario(nScenarioId);
			//	OPシナリオがある場合OPシナリオ実行
			if (s_szCurrentScriptFileName != null)
			{
				DebugLog(s_szCurrentScriptFileName + "\r\n");
				s_EnumScenarioStatus = EnumScenarioStatus.ADV;
				s_CurrentEnumADVStatus = EnumADVStatus.OP;
				ChangeScene(m_Scene_ADV);
			}else
			{
				s_CurrentEnumADVStatus = EnumADVStatus.OP;
				EVENT_EXIT(sender, e);
				return;
			}
			return;
		}
		//==================================================================================
		//	
		//==================================================================================
		private void PROCEED(object sender, InnerPostEventArgs e)
		{
			//もしオアシスならオアシスを抜ける処理
			if( nOassisDistance == 0)
			{
				nOassisDistance = MD_GameConstants.OASSIS_DISTANCE;
				m_Scene_RPG.ExitOassis();
			}
			//	一歩進む
			//	DOAを加算
			s_nDistanceOnAct += MD_GameConstants.ACT_INC;
			//	もしDOAがしきい値を超えていたら追跡者との距離が１縮む
			if(s_nDistanceOnAct >= MD_GameConstants.MAX_ACT)
			{
				s_nDistanceOnAct = 0;
				s_nCurrentDistance--;
				//	TODO　もし追跡者との距離が０ならゲームオーバー
				if( s_nCurrentDistance <= 0 )
				{
					GAME_OVER(null, null);
					return;
				}
			}
			//	歩行累計を加算
			s_nCurrentWalk++;
			//	ボスとの距離も縮まる
			s_nMiddleBossDistance--;
			if( s_nMiddleBossDistance == 0)
			{
				if(s_nBossIndex >= 20) s_nBossIndex = 19;
				//	中ボス戦発生
				//現在のシナリオの中ボス設定
				List<int> list = MD_Scenarios.GetScenariosFromId(s_nCurrentScenarioId).list_BossId;
				s_CurrentMonster = MD_Monsters.GetMonstersFromId(list[s_nBossIndex]);
				//	動的戦闘データに敵を設定
				DD_BattleCharacter.ClearEnemy();
				DD_BattleCharacter.SetEnemy(list[s_nBossIndex]);
				//	コマンドユニットスタックを初期化
				DD_BattleCharacter.ClearCommandStack();
				bIsBattle = true;	//	ステータスを戦闘中状態へ更新
				bIsPause = true;	//	逃げるか戦闘か入力待ち
				bIsBoss = true;
				//	戦闘開始
				m_Scene_RPG.EnterBattle();
				return;
			}
			//	オアシスまでの距離も縮まる
			nOassisDistance--;
			if( nOassisDistance == 0)
			{
				EnterOassis();
				return;
			}
			//	エンカウントチェック
			//	TODO
			if( s_CurrentScenarios.nEnemyProb <= DX.GetRand(100))
			{
				//	出現する敵を決定
				if (PopUpEnemy() == false) return;	//	出現する雑魚がいなければポップしない
				bIsBattle = true;	//	ステータスを戦闘中状態へ更新
				bIsPause = true;	//	逃げるか戦闘か入力待ち
				//	戦闘開始
				m_Scene_RPG.EnterBattle();
			}
		}
		/// <summary>雑魚敵をポップさせる</summary>
		private bool PopUpEnemy()
		{
			//	出現する可能性のある敵のリストアップ
			List<int> list = MD_EnemyPopups.GetEnemyIdList(s_CurrentScenarios.nId, s_nCurrentWalk);
			//	該当する敵がいなければポップしない
			if (list.Count == 0) return false;
			//	敵を確定
			int nMonsId = list[DX.GetRand(list.Count-1)];
			s_CurrentMonster = MD_Monsters.GetMonstersFromId(nMonsId);
			if( s_CurrentMonster==null)
			{
				DebugLog("モンスターの設定がない\r\n");
				return false;
			}
			//	動的戦闘データに敵を設定
			DD_BattleCharacter.ClearEnemy();
			DD_BattleCharacter.SetEnemy(nMonsId);
			//	コマンドユニットスタックを初期化
			DD_BattleCharacter.ClearCommandStack();
			return true;
		}
		//==================================================================================
		/// <summary>オアシス処理</summary>
		//==================================================================================
		private void EnterOassis()
		{
			m_Scene_RPG.EnterOassis();
		}
		//==================================================================================
		//	
		//==================================================================================
		private void ESCAPE(object sender, InnerPostEventArgs e)
		{
			//	戦闘準備をキャンセル
			DD_BattleCharacter.ClearEnemy();
			//	ステータスを更新
			bIsBattle = false;
			bIsPause = true;
			//	基本入力状態へ
			m_Scene_RPG.ClearBattle();
		}
		//==================================================================================
		//	
		//==================================================================================
		private void BATTLE(object sender, InnerPostEventArgs e)
		{
			//	ステータスを更新
			bIsBattle = true;
			bIsPause = false;
			//パネル表示を綺麗に
			m_Scene_RPG.ClearPanel(false);
		}
		//==================================================================================
		//	
		//==================================================================================
		private void ACTION(object sender, InnerPostEventArgs e)
		{
			//ターゲットリストをクリア
			s_list_CommandTargetIndex.Clear();
			//アクションIDを保存
			int[] ntmp = (int[])e.oGenericObject;
			s_nActionId = ntmp[0];
			int nActionTargetType = MD_Actions.GetActionsFromID(s_nActionId).nTarget;
			//1:自分自身 2:味方のいずれか 3:味方全体 4:敵のいずれか 5:敵全体
			switch(nActionTargetType)
			{
				case 1: s_list_CommandTargetIndex.Add(s_nCommandCharaIndex); break;
				case 2: s_list_CommandTargetIndex.Add(ntmp[1]); break;
				case 3:
					if (s_nCommandCharaIndex == 0 || s_nCommandCharaIndex == 1) { s_list_CommandTargetIndex.Add(0); s_list_CommandTargetIndex.Add(1); break; }
					else { s_list_CommandTargetIndex.Add(2); break; }
				case 4:
					if (s_nCommandCharaIndex == 0 || s_nCommandCharaIndex == 1) { s_list_CommandTargetIndex.Add(2);break; }
					else { s_list_CommandTargetIndex.Add(DD_BattleCharacter.GetRandPartyTarget()); break; }
				case 5:
					if (s_nCommandCharaIndex == 0 || s_nCommandCharaIndex == 1) { s_list_CommandTargetIndex.Add(2); break; }
					else { s_list_CommandTargetIndex.Add(0); s_list_CommandTargetIndex.Add(1); break; }
				default: break;
			}
			//	アクションがプレイヤーならDOAを加算（戦闘中の行動も加算される）
			if (s_nCommandCharaIndex == 0 || s_nCommandCharaIndex == 1) s_nDistanceOnAct += MD_GameConstants.ACT_INC;
			//	もしDOAがしきい値を超えていたら追跡者との距離が１縮む
			if (s_nDistanceOnAct >= MD_GameConstants.MAX_ACT)
			{
				s_nDistanceOnAct = 0;
				s_nCurrentDistance--;
				//	TODO　もし追跡者との距離が０ならゲームオーバー
			}
			//	パネルキレイに
			m_Scene_RPG.ClearPanel(true);
			//TP消費
			DD_BattleCharacter.DecTp(s_nCommandCharaIndex, MD_Actions.GetActionsFromID(s_nActionId).nTp);
			//1:攻撃 2:回復 3:防御 4:特殊 5:魔法攻撃
			switch(MD_Actions.GetActionsFromID(s_nActionId).nType)
			{
				case 1: ACTION_ATTACK(); break;
				case 2: ACTION_HEAL(); break;
				case 3: ACTION_DEFENCE(); break;
				case 4: ACTION_OTHER(); break;
				case 5: ACTION_MAGIC(); break;
				default: break;
			}
			//パネルをクリック可能に
			m_Scene_RPG.PanelEnable();
			//敵が死んだら戦闘終了
			if (DD_BattleCharacter.GetCharacterParameter(2, EnumCharacterParameter.CURRENT_HP) == 0)
			{
				s_nLogIndex+=2;
				m_Scene_RPG.ShowLog(s_nLogIndex, DD_BattleCharacter.GetDDBattleCharacter(2).szName + "を倒した！！");
				m_Scene_RPG.SetPanelEvent(EnumPanelEvent.ENEMY_DEAD);
				//経験値取得
				for (int i = 0; i < 2; i++)
				{
					if (DD_BattleCharacter.IncExp(i, MD_Monsters.GetMonstersFromId(DD_BattleCharacter.GetCharacterParameter(2, EnumCharacterParameter.MONSTER_ID)).nExp) == true)
					{
						s_nLogIndex++;
						m_Scene_RPG.ShowLog(s_nLogIndex, DD_BattleCharacter.GetDDBattleCharacter(i).szName + "のレベルがあがった！");
					}
				}
				//お金ドロップ
				int nGold = MD_Monsters.GetMonstersFromId(DD_BattleCharacter.GetCharacterParameter(2,EnumCharacterParameter.MONSTER_ID)).nDropGold;
				s_nMoney += (int)(((float)(DX.GetRand(40) + 80) / 100f) * (float)nGold);
				//アイテムドロップ
				if (DX.GetRand(100) <= MD_Monsters.GetMonstersFromId(DD_BattleCharacter.GetCharacterParameter(2, EnumCharacterParameter.MONSTER_ID)).nDropItemProb)
				{
					int nDropItemId = MD_Monsters.GetMonstersFromId(DD_BattleCharacter.GetCharacterParameter(2, EnumCharacterParameter.MONSTER_ID)).nDropItemId;
					s_nLogIndex++;
					m_Scene_RPG.ShowLog(s_nLogIndex, "敵は" + MD_Items.GetItemsFromID(nDropItemId).szName + "を持っていた！！");
					DD_ItemBox.AddItem(nDropItemId, -1);
				}
			}
			//味方が両方死んだらゲームオーバー
			if( DD_BattleCharacter.GetCharacterParameter(0, EnumCharacterParameter.CURRENT_HP) == 0 && DD_BattleCharacter.GetCharacterParameter(1, EnumCharacterParameter.CURRENT_HP) == 0)
			{
				//フェニックスの尾があれば復活
				if (s_Phoenix.Count != 0)
				{
					if (s_Phoenix[0] == true)
					{
						s_nLogIndex++;
						m_Scene_RPG.ShowLog(s_nLogIndex, "聖なる光が二人を照らし、力がみなぎった・・・・・");
						DD_BattleCharacter.HealToCharacter(0, 100000);
						DD_BattleCharacter.HealToCharacter(1, 100000);
						s_Phoenix[0] = false;
					}
					else
					{
						s_nLogIndex++;
						m_Scene_RPG.ShowLog(s_nLogIndex, "全滅してしまった・・・・・");
						m_Scene_RPG.SetPanelEvent(EnumPanelEvent.PARTY_DEAD);
					}
				}
				else
				{
					s_nLogIndex++;
					m_Scene_RPG.ShowLog(s_nLogIndex, "全滅してしまった・・・・・");
					m_Scene_RPG.SetPanelEvent(EnumPanelEvent.PARTY_DEAD);
				}
			}
		}
		//==================================================================================
		//
		//==================================================================================
		private void ACTION_ATTACK()
		{
			//挑発効果中かつターゲットが単体なら挑発対象をターゲットにする
			if (DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.VOKE_TURN) > 0 && s_list_CommandTargetIndex.Count == 1)
			{
				s_list_CommandTargetIndex[0] = DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.VOKE_INDEX);
			}
			//TP増加
			DD_BattleCharacter.IncTp(s_nCommandCharaIndex, MD_GameConstants.TP_ATTACK_INC);
			m_Scene_RPG.ShowLog(0, DD_BattleCharacter.GetDDBattleCharacter(s_nCommandCharaIndex).szName + "の" + MD_Actions.GetActionsFromID(s_nActionId).szName + "！");
			for(int i=0;i<s_list_CommandTargetIndex.Count;i++)
			{
				CalcAttack(s_list_CommandTargetIndex[i]);	//	攻防関数用パラメータの取得
				//攻防関数実行　FinalDamage = Damage * ( ATK / DEF )
				float fTotalDamage = s_nBattle_A_Damage + s_nBattle_A_WeaponDamage;
				float fTotalATK = s_nBattle_A_Atk + s_nBattle_A_WeaponAtk;
				float fTotalDEF = s_nBattle_D_Def + s_nBattle_D_ArmorDef;
				float fFinalDamage = fTotalDamage * (fTotalATK / fTotalDEF);
				//ダメージ減衰処理
				if (DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.PREVENT_DAMAGE_TURN) > 0)
				{
					float fPrevent = DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.PREVENT_DAMAGE);
					fTotalDamage = fTotalDamage * (fPrevent / 100f);
				}
				//ダメージキャップ処理
				if (fFinalDamage > fTotalDamage * MD_GameConstants.MAX_DAMAGE_x) fFinalDamage = fTotalDamage * MD_GameConstants.MAX_DAMAGE_x;
				//ダメージ反映
				DD_BattleCharacter.DmageToCharacter(s_list_CommandTargetIndex[i], (int)fFinalDamage);
				//ログ反映
				m_Scene_RPG.ShowLog(i + 1, "→　" + DD_BattleCharacter.GetDDBattleCharacter(s_list_CommandTargetIndex[i]).szName + "に" + (int)fFinalDamage + "ダメージ！！");
				s_nLogIndex = i;
			}
		}
		//==================================================================================
		//
		//==================================================================================
		private void ACTION_MAGIC()
		{
			//挑発効果中かつターゲットが単体なら挑発対象をターゲットにする
			if (DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.VOKE_TURN) > 0 && s_list_CommandTargetIndex.Count == 1)
			{
				s_list_CommandTargetIndex[0] = DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.VOKE_INDEX);
			}
			//TP増加
			DD_BattleCharacter.IncTp(s_nCommandCharaIndex, MD_GameConstants.TP_ATTACK_INC);
			m_Scene_RPG.ShowLog(0, DD_BattleCharacter.GetDDBattleCharacter(s_nCommandCharaIndex).szName + "の" + MD_Actions.GetActionsFromID(s_nActionId).szName + "！");
			for (int i = 0; i < s_list_CommandTargetIndex.Count; i++)
			{
				//	攻防関数用パラメータの取得
				float fMagicDamage = DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.CURRENT_MDAMAGE);
				fMagicDamage += DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.CURRENT_WEAPON_MAGIC_DAMAGE);
				fMagicDamage += MD_Actions.GetActionsFromID(s_nActionId).nMagicDamage;
				float fMagicAtk = DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.CURRENT_MATK);
				fMagicAtk += DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.CURRENT_WEAPON_MAGIC_ATK);
				fMagicAtk += MD_Actions.GetActionsFromID(s_nActionId).nMagicAtk;
				float fMagicDef = DD_BattleCharacter.GetCharacterParameter(s_list_CommandTargetIndex[i], EnumCharacterParameter.CURRENT_MDEF);
				fMagicDef += DD_BattleCharacter.GetCharacterParameter(s_list_CommandTargetIndex[i], EnumCharacterParameter.CURRENT_ARMOR_MAGIC_DEF);
				//攻防関数実行　FinalDamage = Damage * ( ATK / DEF )
				float fFinalDamage = fMagicDamage * (fMagicAtk / fMagicDef);
				//ダメージキャップ処理
				if (fFinalDamage > fMagicDamage * MD_GameConstants.MAX_DAMAGE_x) fFinalDamage = fMagicDamage * MD_GameConstants.MAX_DAMAGE_x;
				//ダメージ反映
				DD_BattleCharacter.DmageToCharacter(s_list_CommandTargetIndex[i], (int)fFinalDamage);
				//ログ反映
				m_Scene_RPG.ShowLog(i + 1, "→　" + DD_BattleCharacter.GetDDBattleCharacter(s_list_CommandTargetIndex[i]).szName + "に" + (int)fFinalDamage + "の魔法ダメージ！！");
				s_nLogIndex = i;
			}
		}
		//==================================================================================
		//
		//==================================================================================
		private void ACTION_HEAL()
		{
			m_Scene_RPG.ShowLog(0, DD_BattleCharacter.GetDDBattleCharacter(s_nCommandCharaIndex).szName + "の" + MD_Actions.GetActionsFromID(s_nActionId).szName + "！");
			for (int i = 0; i < s_list_CommandTargetIndex.Count; i++)
			{
				int n = DD_BattleCharacter.HealToCharacter(s_list_CommandTargetIndex[i], MD_Actions.GetActionsFromID(s_nActionId).nHeal);
				//ログ反映
				m_Scene_RPG.ShowLog(i + 1, "→　" + DD_BattleCharacter.GetDDBattleCharacter(s_list_CommandTargetIndex[i]).szName + "を" + n + "回復！！");
			}
		}
		//==================================================================================
		//
		//==================================================================================
		private void ACTION_DEFENCE()
		{
			m_Scene_RPG.ShowLog(0, DD_BattleCharacter.GetDDBattleCharacter(s_nCommandCharaIndex).szName + "の" + MD_Actions.GetActionsFromID(s_nActionId).szName + "！");
			for (int i = 0; i < s_list_CommandTargetIndex.Count; i++)
			{
				int nPreventDamage = MD_Actions.GetActionsFromID(s_nActionId).nPreventDamage;			//防御効果値取得
				int nPreventDamageTurn = MD_Actions.GetActionsFromID(s_nActionId).nPreventDamageTurn;	//防御効果ターン取得
				//効果設定
				DD_BattleCharacter.PreventDamageBuffToCharacter(s_list_CommandTargetIndex[i], nPreventDamage, nPreventDamageTurn);
				//ログ反映
				m_Scene_RPG.ShowLog(i + 1, "→　" + DD_BattleCharacter.GetDDBattleCharacter(s_list_CommandTargetIndex[i]).szName + "にダメージ減衰効果！！");
			}
		}
		//==================================================================================
		//
		//==================================================================================
		private void ACTION_OTHER()
		{
			switch(s_nActionId)
			{
				case 1002: ACTION_OTHER_VOKE(); break;
				default: break;
			}
		}
		//==================================================================================
		//
		//==================================================================================
		private void ACTION_OTHER_VOKE()
		{
			m_Scene_RPG.ShowLog(0, DD_BattleCharacter.GetDDBattleCharacter(s_nCommandCharaIndex).szName + "の" + MD_Actions.GetActionsFromID(s_nActionId).szName + "！");
			for (int i = 0; i < s_list_CommandTargetIndex.Count; i++)
			{
				int nVokeTurn = MD_Actions.GetActionsFromID(s_nActionId).nVokeTurn;
				//効果設定
				DD_BattleCharacter.VokeBuffToCharacter(s_list_CommandTargetIndex[i], s_nCommandCharaIndex, nVokeTurn);
				//ログ反映
				m_Scene_RPG.ShowLog(i + 1, "→　" + DD_BattleCharacter.GetDDBattleCharacter(s_list_CommandTargetIndex[i]).szName + "に挑発効果！！");
			}

		}
		//==================================================================================
		//
		//==================================================================================
		private void CalcAttack(int nTargetIndex)
		{
			//ダメージ取得
			s_nBattle_A_Damage = DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.CURRENT_DAMAGE);
			//攻撃力取得
			s_nBattle_A_Atk = DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.CURRENT_ATK);
			//武器ダメージ補正取得
			s_nBattle_A_WeaponDamage = DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.CURRENT_WEAPON_DAMAGE);
			//武器攻撃力補正取得
			s_nBattle_A_WeaponAtk = DD_BattleCharacter.GetCharacterParameter(s_nCommandCharaIndex, EnumCharacterParameter.CURRENT_WEAPON_ATK);
			//防御力取得
			s_nBattle_D_Def = DD_BattleCharacter.GetCharacterParameter(nTargetIndex, EnumCharacterParameter.CURRENT_DEF);
			//防具補正取得
			s_nBattle_D_ArmorDef = DD_BattleCharacter.GetCharacterParameter(nTargetIndex, EnumCharacterParameter.CURRENT_ARMOR_DEF); ;
		}
		//==================================================================================
		//	ログ表示後のクリック
		//==================================================================================
		private void RESUME(object sender, InnerPostEventArgs e)
		{
			//パネルをクリアし
			m_Scene_RPG.ClearPanel(false);
			//攻撃者のATBをリセット
			DD_BattleCharacter.ResetATB(s_nCommandCharaIndex);
			//攻撃者をリセット
			s_nCommandCharaIndex = -1;
			//ステータスを変更
			bIsPause = false;
		}
		//==================================================================================
		/// <summary>敵死亡</summary>
		//==================================================================================
		private void ENEMY_DEAD(object sender, InnerPostEventArgs e)
		{
			bIsBattle = false;	//	ステータスを非戦闘中状態へ更新
			m_Scene_RPG.ExitBattle();
			m_Scene_RPG.ClearBattle();
			//戦闘終了処理
			DD_BattleCharacter.CloseBattle();
			//ボス戦じゃなかった場合
			if (bIsBoss == false)
			{
				//通常モードへ
				m_Scene_RPG.SetPrimaryInput();
			}
			else
			{
				//ボス戦の場合
				bIsBoss = false;	//フラグを落とす
				//ボス距離をリセット
				s_nMiddleBossDistance = MD_GameConstants.BOSS_DISTANCE;
				//ジンモードへ
				m_Scene_RPG.EnterDin();
			}
		}
		//==================================================================================
		/// <summary>パーティ全滅</summary>
		//==================================================================================
		private void PARTY_DEAD(object sender, InnerPostEventArgs e)
		{
			bIsBattle = false;	//	ステータスを非戦闘中状態へ更新
			m_Scene_RPG.ExitBattle();
			m_Scene_RPG.ClearBattle();
			//戦闘終了処理
			DD_BattleCharacter.CloseBattle();
			//通常モードへ
			m_Scene_RPG.SetPrimaryInput();
			//ゲームオーバー画面
			m_Scene_RPG.GameOver();
		}
		//==================================================================================
		/// <summary>装備変更</summary>
		//==================================================================================
		private void EQUIP_CHANGE(object sender, InnerPostEventArgs e)
		{
			//戦闘中は変更不可
			if (bIsBattle == true) return;
			int[] nargs = (int[])e.oGenericObject;
			//ジェネリックオブジェクトのindex詳細 0:DDキャラindex 1:カテゴリ 2:ローテート方向
			DD_ItemBox.Rotate(nargs[2], nargs[1], nargs[0]);
			//ステータス画面変更
			m_Scene_RPG.StatusUpdate();
		}
		//==================================================================================
		/// <summary>オアシスで休憩</summary>
		//==================================================================================
		private void REST(object sender, InnerPostEventArgs e)
		{
			DD_BattleCharacter.HealToCharacter(0, 1000);
			DD_BattleCharacter.HealToCharacter(1, 1000);
			s_nCurrentDistance--;
		}
		//==================================================================================
		/// <summary>セーブ</summary>
		//==================================================================================
		private void SAVE(object sender, InnerPostEventArgs e)
		{
			m_DD_SaveData.m_szCurrentScriptFileName = s_szCurrentScriptFileName;
			m_DD_SaveData.m_list_CleardScenarioId = s_list_CleardScenarioId;
			m_DD_SaveData.m_nCurrentScenarioId = s_nCurrentScenarioId;
			m_DD_SaveData.m_nCurrentWalk = s_nCurrentWalk;
			m_DD_SaveData.m_nMiddleBossDistance = s_nMiddleBossDistance;
			m_DD_SaveData.m_bIsGamePlaying = s_bIsGamePlaying;
			m_DD_SaveData.m_EnumScenarioStatus = s_EnumScenarioStatus;
			m_DD_SaveData.m_CurrentEnumADVStatus = s_CurrentEnumADVStatus;
			m_DD_SaveData.m_nCurrentDistance = s_nCurrentDistance;
			m_DD_SaveData.m_nDistanceOnAct = s_nDistanceOnAct;
			m_DD_SaveData.nOassisDistance = nOassisDistance;
			m_DD_SaveData.m_CurrentScenarios = s_CurrentScenarios;
			m_DD_SaveData.m_CurrentHero = s_CurrentHero;
			m_DD_SaveData.m_CurrentHeroin = s_CurrentHeroin;
			m_DD_SaveData.m_CurrentScenarioEvents = s_CurrentScenarioEvents;
			m_DD_SaveData.m_CuurentEvent = s_CuurentEvent;
			m_DD_SaveData.m_nNextEventIndex = s_nNextEventIndex;
			m_DD_SaveData.m_nMoney = s_nMoney;
			m_DD_SaveData.m_nPartyActGauge = s_nPartyActGauge;
			m_DD_SaveData.m_CurrentMonster = s_CurrentMonster;
			m_DD_SaveData.m_nCommandCharaIndex = s_nCommandCharaIndex;
			m_DD_SaveData.m_list_CommandTargetIndex = s_list_CommandTargetIndex;
			m_DD_SaveData.m_nLogIndex = s_nLogIndex;
			m_DD_SaveData.m_nBossIndex = s_nBossIndex;
			m_DD_SaveData.m_Phoenix = s_Phoenix;
			m_DD_SaveData.m_nBattle_A_Damage = s_nBattle_A_Damage;
			m_DD_SaveData.m_nBattle_A_Atk = s_nBattle_A_Atk;
			m_DD_SaveData.m_nBattle_A_WeaponDamage = s_nBattle_A_WeaponDamage;
			m_DD_SaveData.m_nBattle_A_WeaponAtk = s_nBattle_A_WeaponAtk;
			m_DD_SaveData.m_nBattle_D_Def = s_nBattle_D_Def;
			m_DD_SaveData.m_nBattle_D_ArmorDef = s_nBattle_D_ArmorDef;
			m_DD_SaveData.m_nActionId = s_nActionId;
			m_DD_SaveData.bIsBattle = bIsBattle;
			m_DD_SaveData.bIsPause = bIsPause;
			m_DD_SaveData.bIsReadySelectCommand = bIsReadySelectCommand;
			m_DD_SaveData.bIsBoss = bIsBoss;
			//DD_BattleCharacter
			m_DD_SaveData.s_list_CommandStackCharacter = DD_BattleCharacter.s_list_CommandStackCharacter;
			m_DD_SaveData.s_list_DD_BattleCharacter = DD_BattleCharacter.s_list_DD_BattleCharacter;
			//DD_ItemBox
			m_DD_SaveData.s_list_InBoxItems = DD_ItemBox.s_list_InBoxItems;
			m_DD_SaveData.s_nIndexCounter = DD_ItemBox.s_nIndexCounter;
			//Scene_RPG
//			m_DD_SaveData.m_Wnd_BattleStatus = m_Scene_RPG.m_Wnd_BattleStatus;
//			m_DD_SaveData.m_Wnd_BattleCommandPanel = m_Scene_RPG.m_Wnd_BattleCommandPanel;
//			m_DD_SaveData.fCurrentATB = m_Scene_RPG.fCurrentATB;
//			m_DD_SaveData.fMaxATB = m_Scene_RPG.fMaxATB;

			FileStream fs = new FileStream("savedat", FileMode.Create, FileAccess.Write);
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(fs, m_DD_SaveData);
			fs.Close();
		}
		//==================================================================================
		/// <summary>ロード</summary>
		//==================================================================================
		private void CLICK_LOAD(object sender, InnerPostEventArgs e)
		{
			if( File.Exists( "savedat" ) == true )
			{
				FileStream fs = new FileStream( "savedat", FileMode.Open, FileAccess.Read );
				BinaryFormatter f = new BinaryFormatter( );
				object obj = f.Deserialize( fs );
				fs.Close( );
				m_DD_SaveData = ( DD_SaveData )obj;
			}
			s_szCurrentScriptFileName = m_DD_SaveData.m_szCurrentScriptFileName;
			s_list_CleardScenarioId = m_DD_SaveData.m_list_CleardScenarioId;
			s_nCurrentScenarioId = m_DD_SaveData.m_nCurrentScenarioId;
			s_nCurrentWalk = m_DD_SaveData.m_nCurrentWalk;
			s_nMiddleBossDistance = m_DD_SaveData.m_nMiddleBossDistance;
			s_bIsGamePlaying = m_DD_SaveData.m_bIsGamePlaying;
			s_EnumScenarioStatus = m_DD_SaveData.m_EnumScenarioStatus;
			s_CurrentEnumADVStatus = m_DD_SaveData.m_CurrentEnumADVStatus;
			s_nCurrentDistance = m_DD_SaveData.m_nCurrentDistance;
			s_nDistanceOnAct = m_DD_SaveData.m_nDistanceOnAct;
			nOassisDistance = m_DD_SaveData.nOassisDistance;
			s_CurrentScenarios = m_DD_SaveData.m_CurrentScenarios;
			s_CurrentHero = m_DD_SaveData.m_CurrentHero;
			s_CurrentHeroin = m_DD_SaveData.m_CurrentHeroin;
			s_CurrentScenarioEvents = m_DD_SaveData.m_CurrentScenarioEvents;
			s_CuurentEvent = m_DD_SaveData.m_CuurentEvent;
			s_nNextEventIndex = m_DD_SaveData.m_nNextEventIndex;
			s_nMoney = m_DD_SaveData.m_nMoney;
			s_nPartyActGauge = m_DD_SaveData.m_nPartyActGauge;
			s_CurrentMonster = m_DD_SaveData.m_CurrentMonster;
			s_nCommandCharaIndex = m_DD_SaveData.m_nCommandCharaIndex;
			s_list_CommandTargetIndex = m_DD_SaveData.m_list_CommandTargetIndex;
			s_nLogIndex = m_DD_SaveData.m_nLogIndex;
			s_nBossIndex = m_DD_SaveData.m_nBossIndex;
			s_Phoenix = m_DD_SaveData.m_Phoenix;
			s_nBattle_A_Damage = m_DD_SaveData.m_nBattle_A_Damage;
			s_nBattle_A_Atk = m_DD_SaveData.m_nBattle_A_Atk;
			s_nBattle_A_WeaponDamage = m_DD_SaveData.m_nBattle_A_WeaponDamage;
			s_nBattle_A_WeaponAtk = m_DD_SaveData.m_nBattle_A_WeaponAtk;
			s_nBattle_D_Def = m_DD_SaveData.m_nBattle_D_Def;
			s_nBattle_D_ArmorDef = m_DD_SaveData.m_nBattle_D_ArmorDef;
			s_nActionId = m_DD_SaveData.m_nActionId;
			bIsBattle = m_DD_SaveData.bIsBattle;
			bIsPause = m_DD_SaveData.bIsPause;
			bIsReadySelectCommand = m_DD_SaveData.bIsReadySelectCommand;
			bIsBoss = m_DD_SaveData.bIsBoss;
			//DD_BattleCharacter
			DD_BattleCharacter.s_list_CommandStackCharacter = m_DD_SaveData.s_list_CommandStackCharacter;
			DD_BattleCharacter.s_list_DD_BattleCharacter = m_DD_SaveData.s_list_DD_BattleCharacter;
			//DD_ItemBox
			DD_ItemBox.s_list_InBoxItems = m_DD_SaveData.s_list_InBoxItems;
			DD_ItemBox.s_nIndexCounter = m_DD_SaveData.s_nIndexCounter;
			//Scene_RPG
//			m_Scene_RPG.m_Wnd_BattleStatus = m_DD_SaveData.m_Wnd_BattleStatus;
//			m_Scene_RPG.m_Wnd_BattleCommandPanel = m_DD_SaveData.m_Wnd_BattleCommandPanel;
//			m_Scene_RPG.fCurrentATB = m_DD_SaveData.fCurrentATB;
//			m_Scene_RPG.fMaxATB = m_DD_SaveData.fMaxATB;

			s_EnumScenarioStatus = EnumScenarioStatus.RPG;
			ChangeScene(m_Scene_RPG);
		}
		//==================================================================================
		/// <summary>ジンコマンド</summary>
		//==================================================================================
		private void DIN(object sender, InnerPostEventArgs e)
		{
			int nDinId = (int)e.oGenericObject;
			MD_Dins md_din = MD_Dins.GetDinFromID(nDinId);
			//所持金減らす
			s_nMoney = s_nMoney - md_din.nPrice;
			switch(md_din.nType)
			{
				case 2: DIN_AddExp(md_din); break;
				case 3: DIN_RunFromChaser(md_din); break;
				case 4: DIN_Phoenix(md_din); break;
				default:break;
			}
		}
		private void DIN_AddExp(MD_Dins md_din)
		{
			DD_BattleCharacter.IncExp(0, md_din.nValue);
			DD_BattleCharacter.IncExp(1, md_din.nValue);
		}
		private void DIN_RunFromChaser(MD_Dins md_din)
		{
			s_nCurrentDistance += md_din.nValue;
		}
		private void DIN_Phoenix(MD_Dins md_din)
		{
			s_Phoenix.Add(true);
		}
		//==================================================================================
		/// <summary>ジンコマンドを終了する</summary>
		//==================================================================================
		private void BYE_DIN(object sender, InnerPostEventArgs e)
		{
			m_Scene_RPG.ExitDin();
			//シナリオスクリプトチェック
			//もしイベントが存在しなければ通常に戻る
			if (s_CurrentScenarioEvents.Count-1 < s_nNextEventIndex )
			{
				m_Scene_RPG.SetPrimaryInput();
				return;
			}
			//
			s_CuurentEvent = s_CurrentScenarioEvents[s_nNextEventIndex];
			s_szCurrentScriptFileName = s_CuurentEvent.szScript;
			s_EnumScenarioStatus = EnumScenarioStatus.ADV;
			s_CurrentEnumADVStatus = EnumADVStatus.OTHER_EVENT;
			ChangeScene(m_Scene_ADV);
			s_nNextEventIndex++;
		}
		//==================================================================================
		//	
		//==================================================================================
		private void GAME_OVER(object sender, InnerPostEventArgs e)
		{
			ChangeScene(m_Scene_TITLE);
			/*
			DX.StopSoundMem( GameHighace.s_nHANDLE_BGM_01_mp3 );
			DX.PlaySoundMem( GameHighace.s_nHANDLE_SE_GAMEOVER, DX.DX_PLAYTYPE_NORMAL );
			DebugLog( "GAME_OVER.\r\n" );
			ChangeScene( m_Scene_Title );
			return;
			*/
		}
		//==================================================================================
		//	
		//==================================================================================
		private void CLICK_SELECT(object sender, InnerPostEventArgs e)
		{
			/*
			DebugLog("CLICK_SELECT.\r\n");
			m_Scene_ADV.m_NextScene = m_Scene_Title;
			//	キャラID取得
			int nID = ( int )e.oGenericObject;
			//	キャラキーネーム取得
			Character c = Character.GetCharacterFromID( nID );
			string szKeyName = c.m_szKeyName;
			m_Scene_ADV.m_szScriptFileName = s_szGraphicPath + "\\" + c.m_szKeyName + "\\PRE.txt";
			m_Scene_ADV.m_EnumADVPart = EnumADVPart.PRE_SCENARIO;
			m_Scene_ADV.m_Chara = c;
			ChangeScene(m_Scene_ADV);
			return;
			*/
		}
		//==================================================================================
		//	
		//==================================================================================
		private void CLICK_SELECT_MEMORY( object sender, InnerPostEventArgs e )
		{
			/*
			DebugLog("CLICK_SELECT_MEMORY.\r\n");
			m_Scene_ADV.m_NextScene = m_Scene_Title;
			//	キャラID取得
			int nID = ( int )e.oGenericObject;
			//	キャラキーネーム取得
			Character c = Character.GetCharacterFromID( nID );
			string szKeyName = c.m_szKeyName;
			m_Scene_ADV.m_szScriptFileName = s_szGraphicPath + "\\" + c.m_szKeyName + "\\AFTER.txt";
			m_Scene_ADV.m_EnumADVPart = EnumADVPart.AFTER_SCENARIO;
			m_Scene_ADV.m_Chara = c;
			ChangeScene(m_Scene_ADV);
			return;
			*/
		}
		//==================================================================================
		//	
		//==================================================================================
		private void CLICK_MEMORY( object sender, InnerPostEventArgs e )
		{
			/*
			DebugLog("CLICK_MEMORY.\r\n");
			m_Scene_MEMORY.m_SaveData = m_SaveData;
			ChangeScene(m_Scene_MEMORY);
			return;
			*/
		}
		
		//==================================================================================
		//	
		//==================================================================================
		private void CLICK_CGMODE( object sender, InnerPostEventArgs e )
		{
			/*
			DebugLog( "CLICK_CGMODE.\r\n" );
			m_Scene_CGMODE.m_SaveData = m_SaveData;
			ChangeScene(m_Scene_CGMODE);
			return;
			*/
		}

		//==================================================================================
		//	
		//==================================================================================
		private void EVENT_EXIT(object sender, InnerPostEventArgs e)
		{
			//	ADVパートで再生されていたのがオープニングならRPGパート開始
			if(s_CurrentEnumADVStatus == EnumADVStatus.OP)
			{
				s_EnumScenarioStatus = EnumScenarioStatus.RPG;
				ChangeScene(m_Scene_RPG);
				return;
			}
			//その他のイベントならRPG再開
			s_EnumScenarioStatus = EnumScenarioStatus.RPG;
			ChangeScene(m_Scene_RPG);
		}
		//==================================================================================
		//	
		//==================================================================================
		private void GAME_CLEAR( object sender, InnerPostEventArgs e )
		{
			/*
			DX.StopSoundMem( GameHighace.s_nHANDLE_BGM_01_mp3 );
			DX.PlaySoundMem( GameHighace.s_nHANDLE_SE_CLEAR, DX.DX_PLAYTYPE_NORMAL );
			DebugLog( "GAME_CLEAR.\r\n" );
			Character c = (Character)e.oGenericObject;
			FlagCheck( c.m_nID );
			DataSave( );
			DebugLog( c.m_szKeyName + ".\r\n" );
			string szKeyName = c.m_szKeyName;
			m_Scene_ADV.m_szScriptFileName = s_szGraphicPath + "\\" + c.m_szKeyName + "\\AFTER.txt";
			m_Scene_ADV.m_EnumADVPart = EnumADVPart.AFTER_SCENARIO;
			m_Scene_ADV.m_Chara = c;
			ChangeScene( m_Scene_ADV );
			return;
			*/
		}
		//==================================================================================
		/// <summary>指定の女の子のクリアフラグを立てる</summary>
		/// <param name="nID"></param>
		//==================================================================================
		private void FlagCheck( int nID )
		{
			/*
			if( m_SaveData.m_dic_GirlsID_ClearInfo.ContainsKey( nID ) == true )
			{
				m_SaveData.m_dic_GirlsID_ClearInfo[ nID ] = true;
				return;
			}
			else
			{
				m_SaveData.m_dic_GirlsID_ClearInfo.Add( nID, true );
			}
			*/
		}
		//==================================================================================
		//	
		//==================================================================================
		private void DataSave( )
		{
			/*
			FileStream fs = new FileStream( "savedat", FileMode.Create, FileAccess.Write );
			BinaryFormatter bf = new BinaryFormatter( );
			bf.Serialize( fs, m_SaveData );
			fs.Close( );
			*/
		}
		//==================================================================================
		/// <summary>実行すべきスクリプトファイル名を得る</summary>
		//==================================================================================
		public static string GetCurrentScriptFileName()
		{
			return s_sztextPath + s_szCurrentScriptFileName;
		}
		//==================================================================================
		/// <summary>指定IDのシナリオを攻略可能かどうか</summary>
		//==================================================================================
		public static bool CheckEnableScenario(int nId)
		{
			MD_Scenarios sn = MD_Scenarios.GetScenariosFromId(nId);
			if (sn == null) return false;
			for(int i=0;i< sn.list_UnlockScenarioId.Count; i++)
			{
				if (sn.list_UnlockScenarioId[0] == 0) return true;	//シナリオID0が指定されている場合無条件でプレイ可能
				if (s_list_CleardScenarioId.Contains(sn.list_UnlockScenarioId[i]) == false) return false;
			}
			return true;
		}
		//==================================================================================
		/// <summary>指定IDのシナリオの開始準備</summary>
		//==================================================================================
		private void InitScenario(int nScnenarioId)
		{
			s_nCurrentScenarioId = nScnenarioId;	//	シナリオIDをセット
			s_nCurrentWalk = 0;						//	累計歩行数をリセット
			s_nMiddleBossDistance = MD_GameConstants.BOSS_DISTANCE;	//	中ボスまでの距離をセット
			s_nMoney = MD_GameConstants.INIT_MONEY;	//	所持金をセット
			MD_Scenarios s = MD_Scenarios.GetScenariosFromId(nScnenarioId);
			s_CurrentScenarios = s;					//	シナリオをセット
			s_CurrentHero = MD_Characters.GetCharactersFromID(s.nHeroCharacterId);		//	今回の主人公をセット
			s_CurrentHeroin = MD_Characters.GetCharactersFromID(s.nHeroinCharacterId);	//	今回のヒロインをセット
			//	バトル系データの初期化
			DD_BattleCharacter.Initialize();
			DD_BattleCharacter.SetPlayerCharacter(0, s.nHeroCharacterId);
			DD_BattleCharacter.SetPlayerCharacter(1, s.nHeroinCharacterId);
			//	アイテムボックスの初期設定
			DD_ItemBox.InitItemFromCharacter(0);
			DD_ItemBox.InitItemFromCharacter(1);
			//	オープニングスクリプトの取得
			s_szCurrentScriptFileName = null;
			s_szCurrentScriptFileName = MD_Events.GetEventsFromID(s.nOpeningEventId).szScript;
			s_nCurrentDistance = MD_GameConstants.CHASER_DISTANCE;
			//オアシスまでの距離リセット
			nOassisDistance = MD_GameConstants.OASSIS_DISTANCE;
			//	ボスインデックスを初期化
			s_nBossIndex = 0;
			//フェニックスの尾初期化
			s_Phoenix.Clear();
			//	イベントリストをセット
			s_CurrentScenarioEvents.Clear();
			for (int i = 0; i < s_CurrentScenarios.list_EventId.Count; i++)
			{
				s_CurrentScenarioEvents.Add(MD_Events.GetEventsFromID(s_CurrentScenarios.list_EventId[i]));
			}
			//次回イベントのインデックスをリセット
			s_nNextEventIndex = 0;
			//テスト装備追加
			DD_ItemBox.AddItem(9999, -1);
		}
	}
}
