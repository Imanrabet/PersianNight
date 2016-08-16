using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianNight
{
	//==================================================================================
	//	dins
	//==================================================================================
	public class MD_Dins
	{
		static private CSV s_csv_masterdata;
		static private List<MD_Dins> s_list_MD_Dins;
		static private Dictionary<int, MD_Dins> s_dic_id_MD_Dins;
		//----------------------------------------------------------------------------------
		private int m_nId;
		private int m_nScenarioId;	//シナリオ指定	scenario->id
		private string m_szName;	//選択肢名
		/// <summary>選択肢タイプ	1:HP回復 2:経験値取得 3:逃げる 4:アイテム購入</summary>
		private int m_nType;		//選択肢タイプ	1:HP回復 2:経験値取得 3:逃げる 4:アイテム購入
		/// <summary>選択肢に対する値	typ=1:HP回復量 type=2:取得経験値量 type=3:離れる距離 type=4:items_id</summary>
		private int m_nValue;		//選択肢に対する値	typ=1:HP回復量 type=2:取得経験値量 type=3:離れる距離 type=4:items_id
		private int m_nPrice;		//選択に必要な価格	0～
		//==================================================================================
		//	
		//==================================================================================
		static public bool Initialize()
		{
			s_csv_masterdata = new CSV();
			if (s_csv_masterdata.ImportCSV(GamePersianNight.s_szCsvPath + "dins.csv") == false) return false;
			int nMaxRows = s_csv_masterdata.nRows;	//	データ数
			//	データ取得とインスタンス化
			s_list_MD_Dins = new List<MD_Dins>();
			s_dic_id_MD_Dins = new Dictionary<int, MD_Dins>();
			for (int i = 0; i < nMaxRows; i++)
			{
				MD_Dins d = new MD_Dins();
				d.m_nId = s_csv_masterdata.GetDataInt(i, "id");
				d.m_nScenarioId = s_csv_masterdata.GetDataInt(i, "scenario_id");
				d.m_szName = s_csv_masterdata.GetData(i, "name");
				d.m_nType = s_csv_masterdata.GetDataInt(i, "type");
				d.m_nValue = s_csv_masterdata.GetDataInt(i, "value");
				d.m_nPrice = s_csv_masterdata.GetDataInt(i, "price");
				//	リスト、ハッシュ登録
				s_list_MD_Dins.Add(d);
				s_dic_id_MD_Dins.Add(d.m_nId, d);
			}
			return true;
		}
		//==================================================================================
		/// <summary>IDからDINデータを引く</summary>
		//==================================================================================
		static public MD_Dins GetDinFromID(int nID)
		{
			if (s_dic_id_MD_Dins.ContainsKey(nID) == false) return null;
			return s_dic_id_MD_Dins[nID];
		}
		//==================================================================================
		//	取得用プロパティ
		//==================================================================================
		public int nId { get { return m_nId; } }
		public int nScenarioId { get { return m_nScenarioId; } }
		public string szName { get { return m_szName; } }
		public int nType { get { return m_nType; } }
		public int nValue { get { return m_nValue; } }
		public int nPrice { get { return m_nPrice; } }
		//==================================================================================
		//	取得用プロパティ
		//==================================================================================
		static public List<MD_Dins> GetDinList(int nScenarioId)
		{
			List<MD_Dins> list = new List<MD_Dins>();
			for (int i = 0; i < s_list_MD_Dins.Count; i++)
			{
				if (s_list_MD_Dins[i].m_nScenarioId == nScenarioId) list.Add(s_list_MD_Dins[i]);
			}
			return list;
		}

	}
}
