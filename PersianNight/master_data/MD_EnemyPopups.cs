using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianNight
{
	//==================================================================================
	//	enemy_popups
	//==================================================================================
	public class MD_EnemyPopups
	{
		static private CSV s_csv_masterdata;
		static private List<MD_EnemyPopups> s_list_MD_EnemyPopups;
		static private Dictionary<int, MD_EnemyPopups> s_dic_id_MD_EnemyPopups;
		//----------------------------------------------------------------------------------
		private int m_nId;			//
		private int m_nScenarioId;	//対象シナリオID	scenario->id	どのシナリオの敵設定か
		private int m_nMonsterId;	//モンスターID	monsters->id	POPさせるモンスターの指定
		private int m_nWalkMin;		//最小歩数	最小歩数<最大歩数となる設定にすること	POPする歩数範囲
		private int m_nWalkMax;		//最大歩数	最小歩数<最大歩数となる設定にすること	POPする歩数範囲
		//==================================================================================
		//	
		//==================================================================================
		static public bool Initialize()
		{
			s_csv_masterdata = new CSV();
			if (s_csv_masterdata.ImportCSV(GamePersianNight.s_szCsvPath + "enemy_popups.csv") == false) return false;
			int nMaxRows = s_csv_masterdata.nRows;	//	データ数
			//	データ取得とインスタンス化
			s_list_MD_EnemyPopups = new List<MD_EnemyPopups>();
			s_dic_id_MD_EnemyPopups = new Dictionary<int, MD_EnemyPopups>();
			for (int i = 0; i < nMaxRows; i++)
			{
				MD_EnemyPopups e = new MD_EnemyPopups();
				e.m_nId = s_csv_masterdata.GetDataInt(i, "id");
				e.m_nScenarioId = s_csv_masterdata.GetDataInt(i, "scenario_id");
				e.m_nMonsterId = s_csv_masterdata.GetDataInt(i, "monster_id");
				e.m_nWalkMin = s_csv_masterdata.GetDataInt(i, "walk_min");
				e.m_nWalkMax = s_csv_masterdata.GetDataInt(i, "walk_max");
				//	リスト、ハッシュ登録
				s_list_MD_EnemyPopups.Add(e);
				s_dic_id_MD_EnemyPopups.Add(e.m_nId, e);
			}
			return true;
		}
		//==================================================================================
		//	
		//==================================================================================
		static public MD_EnemyPopups GetEnemyPopupsFromId(int nId)
		{
			if (s_dic_id_MD_EnemyPopups.ContainsKey(nId) == false) return null;
			return s_dic_id_MD_EnemyPopups[nId];
		}
		//==================================================================================
		//	取得用プロパティ
		//==================================================================================
		public int nId { get { return m_nId; } }
		public int nScenarioId { get { return m_nScenarioId; } }
		public int nMonsterId { get { return m_nMonsterId; } }
		public int nWalkMin { get { return m_nWalkMin; } }
		public int nWalkMax { get { return m_nWalkMax; } }
		//==================================================================================
		/// <summary>シナリオと歩数に適合する雑魚IDリストの取得</summary>
		/// <param name="nScenarioId">シナリオID</param>
		/// <param name="nWalk">何歩目か</param>
		//==================================================================================
		static public List<int>	GetEnemyIdList(int nScenarioId, int nWalk)
		{
			List<int> list = new List<int>();
			for(int i=0;i<s_list_MD_EnemyPopups.Count;i++)
			{
				if (s_list_MD_EnemyPopups[i].m_nScenarioId != nScenarioId) continue;
				if (s_list_MD_EnemyPopups[i].m_nWalkMin <= nWalk && s_list_MD_EnemyPopups[i].m_nWalkMax >= nWalk) list.Add(s_list_MD_EnemyPopups[i].m_nMonsterId);
			}
			return list;
		}
	}
}
