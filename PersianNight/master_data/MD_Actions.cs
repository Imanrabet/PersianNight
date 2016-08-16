using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianNight
{
	//==================================================================================
	//	actions
	//==================================================================================
	public class MD_Actions
	{
		static private CSV s_csv_masterdata;
		static private List<MD_Actions> s_list_MD_Actions;
		static private Dictionary<int, MD_Actions> s_dic_id_MD_Actions;
		//----------------------------------------------------------------------------------
		private int m_nId;
		private string m_szName;		//アクション名称
		private int m_nEquippable;		//選択可能対象	1:誰でも 2:主人公 3:ヒロイン 4:敵専用
		private int m_nType;			//行動タイプ	1:攻撃 2:回復 3:防御 4:特殊 5:魔法攻撃
		private int m_nTp;				//消費TP
		private int m_nTarget;			//ターゲットの種類	1:自分自身 2:味方のいずれか 3:味方全体 4:敵のいずれか 5:敵全体
		private int m_nPreventDamage;		//ダメージ減衰率(%) 0ならダメージ0になる。防御用
		private int m_nPreventDamageTurn;	//	ダメージ減衰有効ターン数（1ターンの区切りはコマンド入力時）
		private int m_nVokeTurn;		//挑発有効期間
		private int m_nDamage;			//ダメージ補正値
		private int m_nAtk;				//攻撃力補正値
		private int m_nDef;				//防御力補正値
		private int m_nMagicDamage;		//魔法ダメージ補正値
		private int m_nMagicAtk;		//魔法攻撃力補正値
		private int m_nMagicDef;		//魔法防御力補正値
		private int m_nHeal;			//回復量
		private int m_nPrice;			//販売価格
		//==================================================================================
		//	
		//==================================================================================
		static public bool Initialize()
		{
			s_csv_masterdata = new CSV();
			if (s_csv_masterdata.ImportCSV(GamePersianNight.s_szCsvPath + "actions.csv") == false) return false;
			int nMaxRows = s_csv_masterdata.nRows;	//	データ数
			//	データ取得とインスタンス化
			s_list_MD_Actions = new List<MD_Actions>();
			s_dic_id_MD_Actions = new Dictionary<int, MD_Actions>();
			for (int i = 0; i < nMaxRows;i++ )
			{
				MD_Actions a = new MD_Actions();
				a.m_nId = s_csv_masterdata.GetDataInt(i, "id");
				a.m_szName = s_csv_masterdata.GetData(i, "name");
				a.m_nEquippable = s_csv_masterdata.GetDataInt(i, "equippable");
				a.m_nType = s_csv_masterdata.GetDataInt(i, "type");
				a.m_nTp = s_csv_masterdata.GetDataInt(i, "tp");
				a.m_nTarget = s_csv_masterdata.GetDataInt(i, "target");
				a.m_nPreventDamage = s_csv_masterdata.GetDataInt(i, "prevent_damage");
				a.m_nPreventDamageTurn = s_csv_masterdata.GetDataInt(i, "prevent_damage_turn");
				a.m_nVokeTurn = s_csv_masterdata.GetDataInt(i, "voke_turn");
				a.m_nDamage = s_csv_masterdata.GetDataInt(i, "damage");
				a.m_nAtk = s_csv_masterdata.GetDataInt(i, "atk");
				a.m_nDef = s_csv_masterdata.GetDataInt(i, "def");
				a.m_nMagicDamage = s_csv_masterdata.GetDataInt(i, "magic_damage");
				a.m_nMagicAtk = s_csv_masterdata.GetDataInt(i, "magic_atk");
				a.m_nMagicDef = s_csv_masterdata.GetDataInt(i, "magic_def");
				a.m_nHeal = s_csv_masterdata.GetDataInt(i, "heal");
				a.m_nPrice = s_csv_masterdata.GetDataInt(i, "price");
				
				//	リスト、ハッシュ登録
				s_list_MD_Actions.Add(a);
				s_dic_id_MD_Actions.Add(a.m_nId, a);
			}
			return true;
		}
		//==================================================================================
		/// <summary>IDからcharactersデータを引く</summary>
		//==================================================================================
		static public MD_Actions GetActionsFromID(int nID)
		{
			if (s_dic_id_MD_Actions.ContainsKey(nID) == false) return null;
			return s_dic_id_MD_Actions[nID];
		}
		//==================================================================================
		//	取得用プロパティ
		//==================================================================================
		public int nId { get { return m_nId; } }
		public string szName { get { return m_szName; } }
		public int nEquippable { get { return m_nEquippable; } }
		public int nType { get { return m_nType; } }
		public int nTp { get { return m_nTp; } }
		public int nTarget { get { return m_nTarget; } }
		public int nPreventDamage { get { return m_nPreventDamage; } }
		public int nPreventDamageTurn { get { return m_nPreventDamageTurn; } }
		public int nVokeTurn { get { return m_nVokeTurn; } }
		public int nDamage { get { return m_nDamage; } }
		public int nAtk { get { return m_nAtk; } }
		public int nDef { get { return m_nDef; } }
		public int nMagicDamage { get { return m_nMagicDamage; } }
		public int nMagicAtk { get { return m_nMagicAtk; } }
		public int nMagicDef { get { return m_nMagicDef; } }
		public int nHeal { get { return m_nHeal; } }
		public int nPrice { get { return m_nPrice; } }

	}
}
