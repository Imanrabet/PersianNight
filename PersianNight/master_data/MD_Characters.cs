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
	[Serializable]
	public class MD_Characters
	{
		static private CSV s_csv_masterdata;
		static private List<MD_Characters> s_list_MD_Characters;
		static private Dictionary<int, MD_Characters> s_dic_id_MD_Characters;
		//----------------------------------------------------------------------------------
		private int m_nId;
		private string m_szName;		//キャラ名称
		private string m_szImageBustup;	//バストアップ画像ファイル名
		private string m_szImageFace;	//顔画像ファイル名
		private int m_InitWeapon;		//初期武器
		private int m_InitArmor;		//初期防具
		private int m_InitSkill_1;		//初期技1
		private int m_InitSkill_2;		//初期技2
		private int m_nActionId_1;	//選択可能なアクションのID
		private int m_nActionId_2;	//選択可能なアクションのID
		private int m_nActionId_3;	//選択可能なアクションのID
		private int m_nActionId_4;	//選択可能なアクションのID
		private int m_nBaseExp;		//基礎経験値
		private int m_nBaseHp;		//基礎HP
		private int m_nBaseTp;		//基礎TP
		private int m_nBaseSpeed;	//基礎スピード
		private int m_nBaseDamage;	//基礎ダメージ
		private int m_nBaseAtk;		//基礎攻撃力
		private int m_nBaseDef;		//基礎防御力
		private int m_nBaseMagicDamage;		//基礎魔法ダメージ
		private int m_nBaseMagicAtk;		//基礎魔法攻撃力
		private int m_nBaseMagicDef;		//基礎魔法防御力
		private int m_nExpLevelTableId;		//レベルアップ成長テーブルID
		private int m_nHpLevelTableId;		//HP成長テーブルID
		private int m_nTpLevelTableId;		//TP成長テーブルID
		private int m_nSpeedLevelTableId;	//SPEED成長テーブルID
		private int m_nDamageLevelTableId;	//ダメージ成長テーブルID
		private int m_nAtkLevelTableId;		//攻撃力成長テーブルID
		private int m_nDefLevelTableId;		//防御力成長テーブルID
		private int m_nMagicDamageLevelTableId;		//魔法ダメージ成長テーブルID
		private int m_nMagicAtkLevelTableId;		//魔法攻撃力成長テーブルID
		private int m_nMagicDefLevelTableId;		//魔法防御力成長テーブルID
		private List<int> m_list_ActionId;
		//==================================================================================
		//	
		//==================================================================================
		static public bool Initialize()
		{
			s_csv_masterdata = new CSV();
			if (s_csv_masterdata.ImportCSV(GamePersianNight.s_szCsvPath + "characters.csv") == false) return false;
			int nMaxRows = s_csv_masterdata.nRows;	//	データ数
			//	データ取得とインスタンス化
			s_list_MD_Characters = new List<MD_Characters>();
			s_dic_id_MD_Characters = new Dictionary<int, MD_Characters>();
			for (int i = 0; i < nMaxRows;i++ ){
				MD_Characters c = new MD_Characters();
				c.m_list_ActionId = new List<int>();
				c.m_nId = s_csv_masterdata.GetDataInt(i, "id");
				c.m_szName = s_csv_masterdata.GetData(i, "name");
				c.m_szImageBustup = s_csv_masterdata.GetData(i, "image_bustup");
				c.m_szImageFace = s_csv_masterdata.GetData(i, "image_face");
				c.m_InitWeapon = s_csv_masterdata.GetDataInt(i, "init_weapon");
				c.m_InitArmor = s_csv_masterdata.GetDataInt(i, "init_armor");
				c.m_InitSkill_1 = s_csv_masterdata.GetDataInt(i, "init_skill_1");
				c.m_InitSkill_2 = s_csv_masterdata.GetDataInt(i, "init_skill_2");
				c.m_nActionId_1 = s_csv_masterdata.GetDataInt(i, "action_id_1");
				c.m_nActionId_2 = s_csv_masterdata.GetDataInt(i, "action_id_2");
				c.m_nActionId_3 = s_csv_masterdata.GetDataInt(i, "action_id_3");
				c.m_nActionId_4 = s_csv_masterdata.GetDataInt(i, "action_id_4");
				c.m_list_ActionId.Add(c.m_nActionId_1);
				c.m_list_ActionId.Add(c.m_nActionId_2);
				c.m_list_ActionId.Add(c.m_nActionId_3);
				c.m_list_ActionId.Add(c.m_nActionId_4);
				c.m_nBaseExp = s_csv_masterdata.GetDataInt(i, "base_exp");
				c.m_nBaseHp = s_csv_masterdata.GetDataInt(i, "base_hp");
				c.m_nBaseTp = s_csv_masterdata.GetDataInt(i, "base_tp");
				c.m_nBaseSpeed = s_csv_masterdata.GetDataInt(i, "base_speed");
				c.m_nBaseDamage = s_csv_masterdata.GetDataInt(i, "base_damage");
				c.m_nBaseAtk = s_csv_masterdata.GetDataInt(i, "base_atk");
				c.m_nBaseDef = s_csv_masterdata.GetDataInt(i, "base_def");
				c.m_nBaseMagicDamage = s_csv_masterdata.GetDataInt(i, "base_magic_damage");
				c.m_nBaseMagicAtk = s_csv_masterdata.GetDataInt(i, "base_magic_atk");
				c.m_nBaseMagicDef = s_csv_masterdata.GetDataInt(i, "base_magic_def");
				c.m_nExpLevelTableId = s_csv_masterdata.GetDataInt(i, "exp_level_table_id");
				c.m_nHpLevelTableId = s_csv_masterdata.GetDataInt(i, "hp_level_table_id");
				c.m_nTpLevelTableId = s_csv_masterdata.GetDataInt(i, "tp_level_table_id");
				c.m_nSpeedLevelTableId = s_csv_masterdata.GetDataInt(i, "speed_level_table_id");
				c.m_nDamageLevelTableId = s_csv_masterdata.GetDataInt(i, "damage_level_table_id");
				c.m_nAtkLevelTableId = s_csv_masterdata.GetDataInt(i, "atk_level_table_id");
				c.m_nDefLevelTableId = s_csv_masterdata.GetDataInt(i, "def_level_table_id");
				c.m_nMagicDamageLevelTableId = s_csv_masterdata.GetDataInt(i, "magic_damage_level_table_id");
				c.m_nMagicAtkLevelTableId = s_csv_masterdata.GetDataInt(i, "magic_atk_level_table_id");
				c.m_nMagicDefLevelTableId = s_csv_masterdata.GetDataInt(i, "magic_def_level_table_id");
				//	リスト、ハッシュ登録
				s_list_MD_Characters.Add(c);
				s_dic_id_MD_Characters.Add(c.m_nId, c);
			}
			return true;
		}
		//==================================================================================
		/// <summary>IDからcharactersデータを引く</summary>
		//==================================================================================
		static public MD_Characters GetCharactersFromID(int nID)
		{
			if (s_dic_id_MD_Characters.ContainsKey(nID) == false) return null;
			return s_dic_id_MD_Characters[nID];
		}
		static public string GetName(int nID)
		{
			if (s_dic_id_MD_Characters.ContainsKey(nID) == false) return null;
			return s_dic_id_MD_Characters[nID].m_szName;
		}
		//==================================================================================
		//	取得用プロパティ
		//==================================================================================
		public int nId { get { return m_nId; } }
		public string szName { get { return m_szName; } }
		public string szImageBustup { get { return m_szImageBustup; } }
		public string szImageFace { get { return m_szImageFace; } }
		public int nInitWeapon { get { return m_InitWeapon; } }
		public int nInitArmor { get { return m_InitArmor; } }
		public int nInitSkill_1 { get { return m_InitSkill_1; } }
		public int nInitSkill_2 { get { return m_InitSkill_2; } }
		public int nActionId_1 { get { return m_nActionId_1; } }
		public int nActionId_2 { get { return m_nActionId_2; } }
		public int nActionId_3 { get { return m_nActionId_3; } }
		public int nActionId_4 { get { return m_nActionId_4; } }
		public int nBaseExp { get { return m_nBaseExp; } }
		public int nBaseHp { get { return m_nBaseHp; } }
		public int nBaseTp { get { return m_nBaseTp; } }
		public int nBaseSpeed { get { return m_nBaseSpeed; } }
		public int nBaseDamage { get { return m_nBaseDamage; } }
		public int nBaseAtk { get { return m_nBaseAtk; } }
		public int nBaseDef { get { return m_nBaseDef; } }
		public int nBaseMagicDamage { get { return m_nBaseMagicDamage; } }
		public int nBaseMagicAtk { get { return m_nBaseMagicAtk; } }
		public int nBaseMagicDef { get { return m_nBaseMagicDef; } }
		public int nExpLevelTableId { get { return m_nExpLevelTableId; } }
		public int nHpLevelTableId { get { return m_nHpLevelTableId; } }
		public int nTpLevelTableId { get { return m_nTpLevelTableId; } }
		public int nSpeedLevelTableId { get { return m_nSpeedLevelTableId; } }
		public int nDamageLevelTableId { get { return m_nDamageLevelTableId; } }
		public int nAtkLevelTableId { get { return m_nAtkLevelTableId; } }
		public int nDefLevelTableId { get { return m_nDefLevelTableId; } }
		public int nMagicDamageLevelTableId { get { return m_nMagicDamageLevelTableId; } }
		public int nMagicAtkLevelTableId { get { return m_nMagicAtkLevelTableId; } }
		public int nMagicDefLevelTableId { get { return m_nMagicDefLevelTableId; } }
		public List<int> list_ActionId { get { return m_list_ActionId; } }
	}
}
