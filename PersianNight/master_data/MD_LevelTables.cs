using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianNight
{
	//==================================================================================
	//	characters
	//==================================================================================
	public class MD_LevelTables
	{
		static private CSV s_csv_masterdata;
		static private List<MD_LevelTables> s_list_MD_LevelTables;
		static private Dictionary<int, MD_LevelTables> s_dic_id_MD_LevelTables;
		//----------------------------------------------------------------------------------
		private int m_nId;
		private List<float> m_list_Level;
		//==================================================================================
		//	
		//==================================================================================
		static public bool Initialize()
		{
			s_csv_masterdata = new CSV();
			if (s_csv_masterdata.ImportCSV(GamePersianNight.s_szCsvPath + "level_tables.csv") == false) return false;
			int nMaxRows = s_csv_masterdata.nRows;	//	データ数
			//	データ取得とインスタンス化
			s_list_MD_LevelTables = new List<MD_LevelTables>();
			s_dic_id_MD_LevelTables = new Dictionary<int, MD_LevelTables>();
			for (int i = 0; i < nMaxRows; i++)
			{
				MD_LevelTables lt = new MD_LevelTables();
				lt.m_list_Level = new List<float>();
				lt.m_nId = s_csv_masterdata.GetDataInt(i, "id");
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv1"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv2"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv3"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv4"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv5"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv6"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv7"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv8"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv9"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv10"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv11"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv12"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv13"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv14"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv15"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv16"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv17"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv18"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv19"));
				lt.m_list_Level.Add(s_csv_masterdata.GetDataFloat(i, "lv20"));
				//	リスト、ハッシュ登録
				s_list_MD_LevelTables.Add(lt);
				s_dic_id_MD_LevelTables.Add(lt.m_nId, lt);
			}
			return true;
		}
		//==================================================================================
		//
		//==================================================================================
		static public MD_LevelTables GetLevelTablesFromID(int nID)
		{
			if (s_dic_id_MD_LevelTables.ContainsKey(nID) == false) return null;
			return s_dic_id_MD_LevelTables[nID];
		}
		//==================================================================================
		//	取得用プロパティ
		//==================================================================================
		public float fValue(int nLevel)
		{
			if (nLevel < 1 || nLevel > MD_GameConstants.MAX_LEVEL) return 0f;
			nLevel--;
			return m_list_Level[nLevel];
		}

	}
}
