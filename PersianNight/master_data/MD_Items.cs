using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianNight
{
	//==================================================================================
	//	items
	//==================================================================================
	[Serializable]
	public class MD_Items
	{
		static private CSV s_csv_masterdata;
		static private List<MD_Items> s_list_MD_Items;
		static private Dictionary<int, MD_Items> s_dic_id_MD_Items;
		//----------------------------------------------------------------------------------
		private int m_nId;
		private string m_szName;		//アイテム名	全角１２文字以内
		private int m_nCategory;		//装備箇所カテゴリー	1:武器 2:防具 3:アビリティ 4:消耗品
		private int m_nEquippable;		//携行可能対象	1:誰でも 2:主人公 3:ヒロイン
		private int m_nHp;				//体力補正値
		private int m_nSpeed;			//アクションスピード補正値
		private int m_nDamage;			//ダメージ補正値
		private int m_nAtk;				//攻撃力補正値
		private int m_nDef;				//防御力補正値
		private int m_nMagicDamage;		//魔法ダメージ値
		private int m_nMagicAtk;		//魔法攻撃力
		private int m_nMagicDef;		//魔法防御力
		private int m_nPrice;			//販売価格
		private int m_nHeal;			//HP回復量
		private int m_nDash;			//追跡者から逃げる距離
		private int m_nTpHeal;			//TP回復量
		//==================================================================================
		//	
		//==================================================================================
		static public bool Initialize()
		{
			s_csv_masterdata = new CSV();
			if (s_csv_masterdata.ImportCSV(GamePersianNight.s_szCsvPath + "items.csv") == false) return false;
			int nMaxRows = s_csv_masterdata.nRows;	//	データ数
			//	データ取得とインスタンス化
			s_list_MD_Items = new List<MD_Items>();
			s_dic_id_MD_Items = new Dictionary<int, MD_Items>();
			for (int i = 0; i < nMaxRows; i++)
			{
				MD_Items it = new MD_Items();
				it.m_nId = s_csv_masterdata.GetDataInt(i, "id");
				it.m_szName = s_csv_masterdata.GetData(i, "name");
				it.m_nCategory = s_csv_masterdata.GetDataInt(i, "category");
				it.m_nEquippable = s_csv_masterdata.GetDataInt(i, "equippable");
				it.m_nHp = s_csv_masterdata.GetDataInt(i, "hp");
				it.m_nSpeed = s_csv_masterdata.GetDataInt(i, "speed");
				it.m_nDamage = s_csv_masterdata.GetDataInt(i, "damage");
				it.m_nAtk = s_csv_masterdata.GetDataInt(i, "atk");
				it.m_nDef = s_csv_masterdata.GetDataInt(i, "def");
				it.m_nMagicDamage = s_csv_masterdata.GetDataInt(i, "magic_damage");
				it.m_nMagicAtk = s_csv_masterdata.GetDataInt(i, "magic_atk");
				it.m_nMagicDef = s_csv_masterdata.GetDataInt(i, "magic_def");
				it.m_nPrice = s_csv_masterdata.GetDataInt(i, "price");
				it.m_nHeal = s_csv_masterdata.GetDataInt(i, "heal");
				it.m_nDash = s_csv_masterdata.GetDataInt(i, "dash");
				it.m_nTpHeal = s_csv_masterdata.GetDataInt(i, "tp_heal");
				//	リスト、ハッシュ登録
				s_list_MD_Items.Add(it);
				s_dic_id_MD_Items.Add(it.m_nId, it);
			}
			return true;
		}
		//==================================================================================
		/// <summary>IDからcharactersデータを引く</summary>
		//==================================================================================
		static public MD_Items GetItemsFromID(int nID)
		{
			if (s_dic_id_MD_Items.ContainsKey(nID) == false) return null;
			return s_dic_id_MD_Items[nID];
		}
		//==================================================================================
		//	取得用プロパティ
		//==================================================================================
		public int nId { get { return m_nId; } }
		public string szName { get { return m_szName; } }
		public int nCategory { get { return m_nCategory; } }
		public int nEquippable { get { return m_nEquippable; } }
		public int nHp { get { return m_nHp; } }
		public int nSpeed { get { return m_nSpeed; } }
		public int nDamage { get { return m_nDamage; } }
		public int nAtk { get { return m_nAtk; } }
		public int nDef { get { return m_nDef; } }
		public int nMagicDamage { get { return m_nMagicDamage; } }
		public int nMagicAtk { get { return m_nMagicAtk; } }
		public int nMagicDef { get { return m_nMagicDef; } }
		public int nPrice { get { return m_nPrice; } }
		public int nHeal { get { return m_nHeal; } }
		public int nDash { get { return m_nDash; } }
		public int nTpHeal { get { return m_nTpHeal; } }

	}
}
