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
	public class MD_Monsters
	{
		static private CSV s_csv_masterdata;
		static private List<MD_Monsters> s_list_MD_Monsters;
		static private Dictionary<int, MD_Monsters> s_dic_id_MD_Monsters;
		//----------------------------------------------------------------------------------
		private int m_nId;
		private string m_szName;		//モンスター名	全角１２文字以内
		private string m_szImage;		//画像ファイル名
		private int m_nHp;				//最大体力		回復上限になる
		private int m_nSpeed;			//アクションスピード		値が大きいほど次の行動まで短時間
		private int m_nDamage;			//ダメージ値		攻撃した際に対象のHPに与えるダメージの基礎値
		private int m_nAtk;				//攻撃力		攻撃時の攻防関数にて対象の防御力との比較に用いる
		private int m_nDef;				//防御力		防御時の攻防関数にて対象の攻撃力との比較に用いる
		private int m_nMagicDamage;		//魔法ダメージ値		魔法攻撃した際に対象のHPに与えるダメージの基礎値
		private int m_nMagicAtk;		//魔法攻撃力		魔法攻撃時の攻防関数にて対象の魔法防御力との比較に用いる
		private int m_nMagicDef;		//魔法防御力		魔法防御時の攻防関数にて対象の魔法攻撃力との比較に用いる
		private int m_nActionId_1;		//アクション1	actions->id
		private int m_nActionId_2;		//アクション2	actions->id
		private int m_nActionId_3;		//アクション3	actions->id
		private int m_nActionId_4;		//アクション3	actions->id
		private int m_nDropItemId;		//戦利品	items->id	倒した時に得られる可能性のあるアイテム
		private int m_nDropItemProb;	//ドロップ率	1~100	ドロップ確率
		private int m_nDropGold;		//ドロップするお金
		private int m_nExp;				//経験値	0～	倒した時に得られる経験値
		private int m_nAttackTimes;		//攻撃回数	1～	攻撃回数の設定
		//==================================================================================
		//	
		//==================================================================================
		static public bool Initialize()
		{
			s_csv_masterdata = new CSV();
			if (s_csv_masterdata.ImportCSV(GamePersianNight.s_szCsvPath + "monsters.csv") == false) return false;
			int nMaxRows = s_csv_masterdata.nRows;	//	データ数
			//	データ取得とインスタンス化
			s_list_MD_Monsters = new List<MD_Monsters>();
			s_dic_id_MD_Monsters = new Dictionary<int, MD_Monsters>();
			for (int i = 0; i < nMaxRows; i++)
			{
				MD_Monsters m = new MD_Monsters();
				m.m_nId = s_csv_masterdata.GetDataInt(i, "id");
				m.m_szName = s_csv_masterdata.GetData(i, "name");
				m.m_szImage = s_csv_masterdata.GetData(i, "image");
				m.m_nHp = s_csv_masterdata.GetDataInt(i, "hp");
				m.m_nSpeed = s_csv_masterdata.GetDataInt(i, "speed");
				m.m_nDamage = s_csv_masterdata.GetDataInt(i, "damage");
				m.m_nAtk = s_csv_masterdata.GetDataInt(i, "atk");
				m.m_nDef = s_csv_masterdata.GetDataInt(i, "def");
				m.m_nMagicDamage = s_csv_masterdata.GetDataInt(i, "magic_damage");
				m.m_nMagicAtk = s_csv_masterdata.GetDataInt(i, "magic_atk");
				m.m_nMagicDef = s_csv_masterdata.GetDataInt(i, "magic_def");
				m.m_nActionId_1 = s_csv_masterdata.GetDataInt(i, "action_id_1");
				m.m_nActionId_2 = s_csv_masterdata.GetDataInt(i, "action_id_2");
				m.m_nActionId_3 = s_csv_masterdata.GetDataInt(i, "action_id_3");
				m.m_nActionId_4 = s_csv_masterdata.GetDataInt(i, "action_id_4");
				m.m_nDropItemId = s_csv_masterdata.GetDataInt(i, "drop_item_id");
				m.m_nDropItemProb = s_csv_masterdata.GetDataInt(i, "drop_item_prob");
				m.m_nDropGold = s_csv_masterdata.GetDataInt(i, "drop_gold");
				m.m_nExp = s_csv_masterdata.GetDataInt(i, "exp");
				m.m_nAttackTimes = s_csv_masterdata.GetDataInt(i, "attack_times");
				//	リスト、ハッシュ登録
				s_list_MD_Monsters.Add(m);
				s_dic_id_MD_Monsters.Add(m.m_nId, m);
			}
			return true;
		}
		//==================================================================================
		//	
		//==================================================================================
		static public MD_Monsters GetMonstersFromId(int nId)
		{
			if (s_dic_id_MD_Monsters.ContainsKey(nId) == false) return null;
			return s_dic_id_MD_Monsters[nId];
		}
		//==================================================================================
		//	取得用プロパティ
		//==================================================================================
		public int nId { get { return m_nId; } }
		public string szName { get { return m_szName; } }
		public string szImage { get { return m_szImage; } }
		public int nHp { get { return m_nHp; } }
		public int nSpeed { get { return m_nSpeed; } }
		public int nDamage { get { return m_nDamage; } }
		public int nAtk { get { return m_nAtk; } }
		public int nDef { get { return m_nDef; } }
		public int nMagicDamage { get { return m_nMagicDamage; } }
		public int nMagicAtk { get { return m_nMagicAtk; } }
		public int nMagicDef { get { return m_nMagicDef; } }
		public int nActionId_1 { get { return m_nActionId_1; } }
		public int nActionId_2 { get { return m_nActionId_2; } }
		public int nActionId_3 { get { return m_nActionId_3; } }
		public int nActionId_4 { get { return m_nActionId_4; } }
		public int nDropItemId { get { return m_nDropItemId; } }
		public int nDropItemProb { get { return m_nDropItemProb; } }
		public int nDropGold { get { return m_nDropGold; } }
		public int nExp { get { return m_nExp; } }
		public int nAttackTimes { get { return m_nAttackTimes; } }
		//==================================================================================
		/// <summary>ドロップする可能性のあるアイテム名</summary>
		//==================================================================================
		public string GetDropItemName()
		{
			string sz = MD_Items.GetItemsFromID(m_nDropItemId).szName;
			if (sz == null) return "";
			return sz;
		}

	}
}
