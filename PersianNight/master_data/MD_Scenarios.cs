using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianNight
{
	//==================================================================================
	//	monsters
	//==================================================================================
	[Serializable]
	public class MD_Scenarios
	{
		static private CSV s_csv_masterdata;
		static private List<MD_Scenarios> s_list_MD_Scenarios;
		static private Dictionary<int, MD_Scenarios> s_dic_id_MD_Scenarios;
		//----------------------------------------------------------------------------------
		private int m_nId;
		private string m_szName;				//シナリオ名
		private string m_szSelectBack;			//ステージ選択画面背景
		private string m_szSelectButton;		//ステージ選択画面ボタン
		private string m_szSelectButtonOver;	//ステージ選択画面ボタンマウスオーバー時
		private string m_szSelectSilhouette;	//ステージ選択画面シルエット
		private string m_szBattleBack;			//戦闘時背景
		private int m_nHeroinCharacterId;	//ヒロインID	characters->id
		private int m_nHeroCharacterId;		//主人公ID	characters->id
		private int m_nEnemyProb;			//敵出現率	1～100	百分率。100なら必ず敵と接触。
		private int m_nOasisProb;			//オアシス出現率	1～100	百分率。100なら必ずオアシス出現。
		private List<int> m_list_UnlockScenarioId;	//開放条件となるシナリオID
		private int m_nClearConditionType;	//クリア条件タイプ	1:金を貯める 2:固定ボスを指定数倒す3:すべての固定イベントを見る
		private int m_nClearValue;			//クリア条件値
		private List<int> m_list_EventId;	//イベント1のID	events->id	event_id_1 から順に発生する。空欄の場合イベントなし。
		private int m_nOpeningEventId;		//オープニングのID	events->id	オープニング用イベント。
		private int m_nEndingEventId;		//クリアイベントのID	events->id	クリア用イベント。
		private List<int> m_list_BossId;	//中ボス指定	monsters->id	中ボスの指定。20回目まで設定可能。21回目からは20回目の中ボスが出現。
		private int m_nChaserId;			//追跡者指定	monsters->id	追跡者戦用指定。
		//==================================================================================
		//	
		//==================================================================================
		static public bool Initialize()
		{
			s_csv_masterdata = new CSV();
			if (s_csv_masterdata.ImportCSV(GamePersianNight.s_szCsvPath + "scenarios.csv") == false) return false;
			int nMaxRows = s_csv_masterdata.nRows;	//	データ数
			//	データ取得とインスタンス化
			s_list_MD_Scenarios = new List<MD_Scenarios>();
			s_dic_id_MD_Scenarios = new Dictionary<int, MD_Scenarios>();
			for (int i = 0; i < nMaxRows; i++)
			{
				MD_Scenarios s = new MD_Scenarios();
				s.m_list_BossId = new List<int>();
				s.m_list_EventId = new List<int>();
				s.m_list_UnlockScenarioId = new List<int>();
				s.m_nId = s_csv_masterdata.GetDataInt(i, "id");
				s.m_szName = s_csv_masterdata.GetData(i, "name");
				s.m_szSelectBack = s_csv_masterdata.GetData(i, "select_back");
				s.m_szSelectButton = s_csv_masterdata.GetData(i, "select_button");
				s.m_szSelectButtonOver = s_csv_masterdata.GetData(i, "select_button_over");
				s.m_szSelectSilhouette = s_csv_masterdata.GetData(i, "select_silhouette");
				s.m_szBattleBack = s_csv_masterdata.GetData(i, "battle_back");
				s.m_nHeroinCharacterId = s_csv_masterdata.GetDataInt(i, "heroin_character_id");
				s.m_nHeroCharacterId = s_csv_masterdata.GetDataInt(i, "hero_character_id");
				s.m_nEnemyProb = s_csv_masterdata.GetDataInt(i, "enemy_prob");
				s.m_nOasisProb = s_csv_masterdata.GetDataInt(i, "oasis_prob");
				s.m_nClearConditionType = s_csv_masterdata.GetDataInt(i, "clear_condition_type");
				s.m_nClearValue = s_csv_masterdata.GetDataInt(i, "clear_value");
				s.m_nOpeningEventId = s_csv_masterdata.GetDataInt(i, "opening_event_id");
				s.m_nEndingEventId = s_csv_masterdata.GetDataInt(i, "ending_event_id");
				s.m_nChaserId = s_csv_masterdata.GetDataInt(i, "chaser_id");
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_1"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_2"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_3"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_4"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_5"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_6"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_7"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_8"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_9"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_10"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_11"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_12"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_13"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_14"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_15"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_16"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_17"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_18"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_19"));
				s.m_list_BossId.Add(s_csv_masterdata.GetDataInt(i, "boss_id_20"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_1"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_2"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_3"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_4"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_5"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_6"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_7"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_8"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_9"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_10"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_11"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_12"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_13"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_14"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_15"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_16"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_17"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_18"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_19"));
				s.m_list_EventId.Add(s_csv_masterdata.GetDataInt(i, "event_id_20"));
				string[] sz = s_csv_masterdata.GetData(i, "unlock_scenario_id").Split(new char[]{'/'},StringSplitOptions.RemoveEmptyEntries);
				for (int j = 0; j < sz.Length; j++) { s.m_list_UnlockScenarioId.Add(int.Parse(sz[j])); }
				//	リスト、ハッシュ登録
				s_list_MD_Scenarios.Add(s);
				s_dic_id_MD_Scenarios.Add(s.m_nId, s);
			}
			return true;
		}
		//==================================================================================
		//	
		//==================================================================================
		static public MD_Scenarios GetScenariosFromId(int nId)
		{
			if (s_dic_id_MD_Scenarios.ContainsKey(nId) == false) return null;
			return s_dic_id_MD_Scenarios[nId];
		}
		//==================================================================================
		//	取得用プロパティ
		//==================================================================================
		public int nId { get { return m_nId; } }
		public string szName { get { return m_szName; } }
		public string szSelectBack { get { return m_szSelectBack; } }
		public string szSelectButton { get { return m_szSelectButton; } }
		public string szSelectButtonOver { get { return m_szSelectButtonOver; } }
		public string szSelectSilhouette { get { return m_szSelectSilhouette; } }
		public string szBattleBack { get { return m_szBattleBack; } }
		public int nHeroinCharacterId { get { return m_nHeroinCharacterId; } }
		public int nHeroCharacterId { get { return m_nHeroCharacterId; } }
		public int nEnemyProb { get { return m_nEnemyProb; } }
		public int nOasisProb { get { return m_nOasisProb; } }
		public List<int> list_UnlockScenarioId { get { return m_list_UnlockScenarioId; } }
		public int nClearConditionType { get { return m_nClearConditionType; } }
		public int nClearValue { get { return m_nClearValue; } }
		public List<int> list_EventId { get { return m_list_EventId; } }
		public int nOpeningEventId { get { return m_nOpeningEventId; } }
		public int nEndingEventId { get { return m_nEndingEventId; } }
		public List<int> list_BossId { get { return m_list_BossId; } }
		public int nChaserId { get { return m_nChaserId; } }

	}
}
