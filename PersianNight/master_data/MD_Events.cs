using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianNight
{
	//==================================================================================
	//	events
	//==================================================================================
	[Serializable]
	public class MD_Events
	{
		static private CSV s_csv_masterdata;
		static private List<MD_Events> s_list_MD_Events;
		static private Dictionary<int, MD_Events> s_dic_id_MD_Events;
		//----------------------------------------------------------------------------------
		private int m_nId;
		private string m_szName;		//イベント名
		private string m_szScript;		//スクリプトファイル名
		//==================================================================================
		//	
		//==================================================================================
		static public bool Initialize()
		{
			s_csv_masterdata = new CSV();
			if (s_csv_masterdata.ImportCSV(GamePersianNight.s_szCsvPath + "events.csv") == false) return false;
			int nMaxRows = s_csv_masterdata.nRows;	//	データ数
			//	データ取得とインスタンス化
			s_list_MD_Events = new List<MD_Events>();
			s_dic_id_MD_Events = new Dictionary<int, MD_Events>();
			for (int i = 0; i < nMaxRows; i++)
			{
				MD_Events ev = new MD_Events();
				ev.m_nId = s_csv_masterdata.GetDataInt(i, "id");
				ev.m_szName = s_csv_masterdata.GetData(i, "name");
				ev.m_szScript = s_csv_masterdata.GetData(i, "script");
				//	リスト、ハッシュ登録
				s_list_MD_Events.Add(ev);
				s_dic_id_MD_Events.Add(ev.m_nId, ev);
			}
			return true;
		}
		//==================================================================================
		/// <summary>IDからeventsデータを引く</summary>
		//==================================================================================
		static public MD_Events GetEventsFromID(int nID)
		{
			if (s_dic_id_MD_Events.ContainsKey(nID) == false) return null;
			return s_dic_id_MD_Events[nID];
		}
		//==================================================================================
		//	取得用プロパティ
		//==================================================================================
		public int nId { get { return m_nId; } }
		public string szName { get { return m_szName; } }
		public string szScript { get { return m_szScript; } }


	}
}
