using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianNight
{
	/// <summary>パラメータ</summary>
	public enum EnumCharacterParameter
	{
		CURRENT_LEVEL,
		CURRENT_EXP,
		CURRENT_HP,
		CURRENT_ATK,
		CURRENT_DEF,
		CURRENT_MATK,
		CURRENT_MDEF,
		CURRENT_DAMAGE,
		CURRENT_MDAMAGE,
		CURRENT_WEAPON_ID,
		CURRENT_ARMOR_ID,
		CURRENT_TP,
		CURRENT_SPEED,
		CURRENT_ATB,
		CURRENT_WEAPON_DAMAGE,
		CURRENT_WEAPON_ATK,
		CURRENT_ARMOR_DEF,
		CURRENT_WEAPON_MAGIC_DAMAGE,
		CURRENT_WEAPON_MAGIC_ATK,
		CURRENT_ARMOR_MAGIC_DEF,

		PREVENT_DAMAGE,
		PREVENT_DAMAGE_TURN,
		VOKE_INDEX,
		VOKE_TURN,

		MAX_EXP,
		MAX_HP,
		MAX_TP,

		MONSTER_ID,
		CHARACTER_ID,
		EQUIP_WEAPON_INDEX,
		EQUIP_ARMOR_INDEX
	}
	//==================================================================================
	//	動的データ：バトルキャラクター
	//==================================================================================
	[Serializable]
	public class DD_BattleCharacter
	{
		/// <summary>0:主人公 1:ヒロイン 2,3,4:敵　で固定</summary>
		static public List<DD_BattleCharacter> s_list_DD_BattleCharacter;
		/// <summary>ATBコマンド決定待ちキャラのスタック</summary>
		static public List<int> s_list_CommandStackCharacter;
		//----------------------------------------------------------------------------------
		private bool m_bIsCharacter;
		/// <summary>自身のマスターデータ</summary>
		private MD_Characters m_Characters;
		/// <summary>自身のマスターデータ</summary>
		private MD_Monsters m_Monsters;
		/// <summary>現在のレベル</summary>
		private int m_nCurrentLevel = 1;
		/// <summary>最大HP</summary>
		private int m_nMaxHp;
		/// <summary>現在のHP</summary>
		private int m_nCurrentHp;
		/// <summary>現在の経験値</summary>
		private int m_nCurrentExp;
		/// <summary>NEXT経験値</summary>
		private int m_nMaxExp;
		private int m_nAtk;
		private int m_nDef;
		private int m_nMagicAtk;
		private int m_nMagicDef;
		private int m_nDamage;
		private int m_nMagicDamage;
		private int m_nCurrentEquipWeaponId;
		private int m_nCurrentEquipArmorId;
		private int m_nMaxTp;
		private int m_nCurrentTp;
		private int m_nCurrentSpeed;
		private int m_nCurrentATB;
		private int m_nWeaponDamage;
		private int m_nWeaponAtk;
		private int m_nArmorDef;
		private int m_nWeaponMagicDamage;
		private int m_nWeaponMagicAtk;
		private int m_nArmorMagicDef;
		private int m_nGetExp;
		private int m_nEquipIndexWeapon;
		private int m_nEquipIndexArmor;
		/// <summary>名称</summary>
		private string m_szName = "";
		/// <summary>名称</summary>
		public string szName { get { return m_szName; } }
		/// <summary>現在のダメージ減衰効果(%)</summary>
		private int m_nPreventDamage;
		/// <summary>現在のダメージ減衰効果(%)の残り有効ターン数</summary>
		private int m_nPreventDamageTurn;
		/// <summary>挑発の残り有効ターン数（挑発をかけられた側に設定）</summary>
		private int m_nVokeTurn;
		/// <summary>挑発対象</summary>
		private int m_nVokeIndex;
		/// <summary>所有アクションリスト</summary>
		private List<int> m_list_ActionId;
		//==================================================================================
		//	
		//==================================================================================
		static public bool Initialize()
		{
			s_list_DD_BattleCharacter = new List<DD_BattleCharacter>();
			for (int i = 0; i < 5;i++ )
			{
				DD_BattleCharacter bc = new DD_BattleCharacter();
				bc.Init();
				s_list_DD_BattleCharacter.Add(bc);
			}
			s_list_CommandStackCharacter = new List<int>();
			return true;
		}
		//----------------------------------------------------------------------------------
		/// <summary>プレイヤーキャラのセットアップ</summary>
		/// <param name="nType">0:主人公 1:ヒロイン</param>
		/// <param name="nCharacterID">キャラクタID</param>
		//----------------------------------------------------------------------------------
		static public bool SetPlayerCharacter(int nType, int nCharacterID)
		{
			if( nType != 0 && nType != 1) return false;
			//	指定IDのキャラマスター情報を取得
			MD_Characters c = MD_Characters.GetCharactersFromID(nCharacterID);
			if (c == null) return false;
			s_list_DD_BattleCharacter[nType].m_list_ActionId = new List<int>();
			s_list_DD_BattleCharacter[nType].Init();
			s_list_DD_BattleCharacter[nType].m_Characters = c;
			s_list_DD_BattleCharacter[nType].m_szName = c.szName;
			s_list_DD_BattleCharacter[nType].m_bIsCharacter = true;
			s_list_DD_BattleCharacter[nType].m_nCurrentLevel = 1;
			s_list_DD_BattleCharacter[nType].m_nMaxHp = s_list_DD_BattleCharacter[nType].GetParameterFromLevel(EnumCharacterParameter.MAX_HP);
			s_list_DD_BattleCharacter[nType].m_nCurrentHp = s_list_DD_BattleCharacter[nType].m_nMaxHp;
			s_list_DD_BattleCharacter[nType].m_nCurrentExp = 0;
			s_list_DD_BattleCharacter[nType].m_nMaxExp = s_list_DD_BattleCharacter[nType].GetParameterFromLevel(EnumCharacterParameter.MAX_EXP);
			s_list_DD_BattleCharacter[nType].m_nAtk = s_list_DD_BattleCharacter[nType].GetParameterFromLevel(EnumCharacterParameter.CURRENT_ATK);
			s_list_DD_BattleCharacter[nType].m_nDef = s_list_DD_BattleCharacter[nType].GetParameterFromLevel(EnumCharacterParameter.CURRENT_DEF);
			s_list_DD_BattleCharacter[nType].m_nMagicAtk = s_list_DD_BattleCharacter[nType].GetParameterFromLevel(EnumCharacterParameter.CURRENT_MATK);
			s_list_DD_BattleCharacter[nType].m_nMagicDef = s_list_DD_BattleCharacter[nType].GetParameterFromLevel(EnumCharacterParameter.CURRENT_MDEF);
			s_list_DD_BattleCharacter[nType].m_nDamage = s_list_DD_BattleCharacter[nType].GetParameterFromLevel(EnumCharacterParameter.CURRENT_DAMAGE);
			s_list_DD_BattleCharacter[nType].m_nMagicDamage = s_list_DD_BattleCharacter[nType].GetParameterFromLevel(EnumCharacterParameter.CURRENT_MDAMAGE);
			s_list_DD_BattleCharacter[nType].m_nCurrentEquipWeaponId = c.nInitWeapon;
			s_list_DD_BattleCharacter[nType].m_nCurrentEquipArmorId = c.nInitArmor;
			s_list_DD_BattleCharacter[nType].m_nMaxTp = s_list_DD_BattleCharacter[nType].GetParameterFromLevel(EnumCharacterParameter.MAX_TP);
			s_list_DD_BattleCharacter[nType].m_nCurrentTp = 0;
			s_list_DD_BattleCharacter[nType].m_nCurrentSpeed = s_list_DD_BattleCharacter[nType].GetParameterFromLevel(EnumCharacterParameter.CURRENT_SPEED);
			s_list_DD_BattleCharacter[nType].m_nCurrentATB = 0;
			s_list_DD_BattleCharacter[nType].m_nWeaponDamage = MD_Items.GetItemsFromID(s_list_DD_BattleCharacter[nType].m_nCurrentEquipWeaponId).nDamage;
			s_list_DD_BattleCharacter[nType].m_nWeaponAtk = MD_Items.GetItemsFromID(s_list_DD_BattleCharacter[nType].m_nCurrentEquipWeaponId).nAtk;
			s_list_DD_BattleCharacter[nType].m_nArmorDef = MD_Items.GetItemsFromID(s_list_DD_BattleCharacter[nType].m_nCurrentEquipArmorId).nDef;
			s_list_DD_BattleCharacter[nType].m_nWeaponMagicDamage = MD_Items.GetItemsFromID(s_list_DD_BattleCharacter[nType].m_nCurrentEquipWeaponId).nMagicDamage;
			s_list_DD_BattleCharacter[nType].m_nWeaponMagicAtk = MD_Items.GetItemsFromID(s_list_DD_BattleCharacter[nType].m_nCurrentEquipWeaponId).nMagicAtk;
			s_list_DD_BattleCharacter[nType].m_nArmorMagicDef = MD_Items.GetItemsFromID(s_list_DD_BattleCharacter[nType].m_nCurrentEquipArmorId).nMagicDef;
			s_list_DD_BattleCharacter[nType].m_nPreventDamage = 100;	//100がデフォ
			s_list_DD_BattleCharacter[nType].m_nPreventDamageTurn = 0;
			s_list_DD_BattleCharacter[nType].m_nVokeTurn = 0;
			s_list_DD_BattleCharacter[nType].m_nVokeIndex = -1;
			s_list_DD_BattleCharacter[nType].m_list_ActionId.Add(c.list_ActionId[0]);
			s_list_DD_BattleCharacter[nType].m_list_ActionId.Add(c.list_ActionId[1]);
			s_list_DD_BattleCharacter[nType].m_list_ActionId.Add(c.list_ActionId[2]);
			s_list_DD_BattleCharacter[nType].m_list_ActionId.Add(c.list_ActionId[3]);
			return true;
		}
		//----------------------------------------------------------------------------------
		/// <summary>戦闘用キャラデータを得る</summary>
		/// <param name="nIndex">0:主人公 1:ヒロイン 2～4:敵</param>
		//----------------------------------------------------------------------------------
		static public DD_BattleCharacter GetDDBattleCharacter(int nIndex)
		{
			return s_list_DD_BattleCharacter[nIndex];
		}
		//----------------------------------------------------------------------------------
		/// <summary>敵のセットアップ。戦闘開始時の場合事前にClearEnemy()しておくこと</summary>
		/// <param name="nMonsterId">モンスターID</param>
		//----------------------------------------------------------------------------------
		static public bool SetEnemy(int nMonsterId)
		{
			//	設定の無いスロットを検索
			bool b = false;
			int i;
			for (i = 2; i < 4; i++) { if (s_list_DD_BattleCharacter[i].m_Monsters == null) { b = true; break; } }
			if (b == false) return false;	//	空きスロットがなければ失敗
			MD_Monsters m = MD_Monsters.GetMonstersFromId(nMonsterId);
			if (m == null) return false;	//	無効なIDなら失敗
			s_list_DD_BattleCharacter[i].m_list_ActionId = new List<int>();
			s_list_DD_BattleCharacter[i].m_Monsters = m;
			s_list_DD_BattleCharacter[i].m_bIsCharacter = false;
			s_list_DD_BattleCharacter[i].m_szName = m.szName;
			s_list_DD_BattleCharacter[i].m_nCurrentLevel = 1;
			s_list_DD_BattleCharacter[i].m_nMaxHp = m.nHp;
			s_list_DD_BattleCharacter[i].m_nCurrentHp = s_list_DD_BattleCharacter[i].m_nMaxHp;
			s_list_DD_BattleCharacter[i].m_nCurrentSpeed = m.nSpeed;
			s_list_DD_BattleCharacter[i].m_nCurrentATB = 0;
			s_list_DD_BattleCharacter[i].m_nCurrentEquipWeaponId = -1;
			s_list_DD_BattleCharacter[i].m_nCurrentEquipArmorId = -1;
			s_list_DD_BattleCharacter[i].m_nDamage = m.nDamage;
			s_list_DD_BattleCharacter[i].m_nAtk = m.nAtk;
			s_list_DD_BattleCharacter[i].m_nDef = m.nDef;
			s_list_DD_BattleCharacter[i].m_nWeaponDamage = 0;
			s_list_DD_BattleCharacter[i].m_nWeaponAtk = 0;
			s_list_DD_BattleCharacter[i].m_nArmorDef = 0;
			s_list_DD_BattleCharacter[i].m_nWeaponMagicDamage = 0;
			s_list_DD_BattleCharacter[i].m_nWeaponMagicAtk = 0;
			s_list_DD_BattleCharacter[i].m_nArmorMagicDef = 0;
			s_list_DD_BattleCharacter[i].m_nPreventDamage = 100;	//100がデフォ
			s_list_DD_BattleCharacter[i].m_nPreventDamageTurn = 0;
			s_list_DD_BattleCharacter[i].m_nVokeTurn = 0;
			s_list_DD_BattleCharacter[i].m_nVokeIndex = -1;
			s_list_DD_BattleCharacter[i].m_list_ActionId.Add(m.nActionId_1);
			s_list_DD_BattleCharacter[i].m_list_ActionId.Add(m.nActionId_2);
			s_list_DD_BattleCharacter[i].m_list_ActionId.Add(m.nActionId_3);
			s_list_DD_BattleCharacter[i].m_list_ActionId.Add(m.nActionId_4);
			s_list_DD_BattleCharacter[i].m_nGetExp = m.nExp;
			return true;
		}
		//----------------------------------------------------------------------------------
		/// <summary>敵情報の初期化</summary>
		//----------------------------------------------------------------------------------
		static public void ClearEnemy()
		{
			s_list_DD_BattleCharacter[2].Init();
			s_list_DD_BattleCharacter[3].Init();
			s_list_DD_BattleCharacter[4].Init();
		}
		//----------------------------------------------------------------------------------
		/// <summary>戦闘終了時の初期化</summary>
		//----------------------------------------------------------------------------------
		static public void CloseBattle()
		{
			for(int i=0;i<2;i++)
			{
				s_list_DD_BattleCharacter[i].m_nPreventDamage = 100;
				s_list_DD_BattleCharacter[i].m_nPreventDamageTurn = 0;
				s_list_DD_BattleCharacter[i].m_nVokeTurn = 0;
				s_list_DD_BattleCharacter[i].m_nVokeIndex = -1;
				s_list_DD_BattleCharacter[i].m_nCurrentATB = 0;
			}
		}
		//----------------------------------------------------------------------------------
		/// <summary>戦闘不能でないパーティキャラのindexをランダムで返す</summary>
		//----------------------------------------------------------------------------------
		static public int GetRandPartyTarget()
		{
			List<int> list = new List<int>();
			if (s_list_DD_BattleCharacter[0].m_nCurrentHp != 0) list.Add(0);
			if (s_list_DD_BattleCharacter[1].m_nCurrentHp != 0) list.Add(1);
			return list[DxLibDLL.DX.GetRand(list.Count - 1)];
		}
		//----------------------------------------------------------------------------------
		/// <summary>指定キャラの装備を変更する</summary>
		/// <param name="nIndex">キャラindex</param>
		/// <param name="nCategory">1:武器 2:防具</param>
		/// <param name="nItemIndex">アイテムindex</param>
		//----------------------------------------------------------------------------------
		static public void EquipItem(int nIndex, int nCategory, int nItemIndex)
		{
			if (nCategory == 1)
			{
				s_list_DD_BattleCharacter[nIndex].m_nEquipIndexWeapon = nItemIndex;
				s_list_DD_BattleCharacter[nIndex].m_nCurrentEquipWeaponId = DD_ItemBox.GetEquippableItemFromIndex(nItemIndex).MD_Item.nId;
				s_list_DD_BattleCharacter[nIndex].m_nWeaponDamage = DD_ItemBox.GetEquippableItemFromIndex(nItemIndex).MD_Item.nDamage;
				s_list_DD_BattleCharacter[nIndex].m_nWeaponAtk = DD_ItemBox.GetEquippableItemFromIndex(nItemIndex).MD_Item.nAtk;
				s_list_DD_BattleCharacter[nIndex].m_nWeaponMagicDamage = DD_ItemBox.GetEquippableItemFromIndex(nItemIndex).MD_Item.nMagicDamage;
				s_list_DD_BattleCharacter[nIndex].m_nWeaponMagicAtk = DD_ItemBox.GetEquippableItemFromIndex(nItemIndex).MD_Item.nMagicAtk;
			}
			if (nCategory == 2)
			{
				s_list_DD_BattleCharacter[nIndex].m_nEquipIndexArmor = nItemIndex;
				s_list_DD_BattleCharacter[nIndex].m_nCurrentEquipArmorId = DD_ItemBox.GetEquippableItemFromIndex(nItemIndex).MD_Item.nId;
				s_list_DD_BattleCharacter[nIndex].m_nArmorDef = DD_ItemBox.GetEquippableItemFromIndex(nItemIndex).MD_Item.nDef;
				s_list_DD_BattleCharacter[nIndex].m_nArmorMagicDef = DD_ItemBox.GetEquippableItemFromIndex(nItemIndex).MD_Item.nMagicDef;
			}
		}
		//----------------------------------------------------------------------------------
		/// <summary>キャラクターのスピードに応じてACTを貯める</summary>
		//----------------------------------------------------------------------------------
		static public void IncATB()
		{
			for(int i=0;i<5;i++)
			{
				//存在しないキャラは飛ばす
				if (s_list_DD_BattleCharacter[i].m_Characters == null && s_list_DD_BattleCharacter[i].m_Monsters == null) continue;
				//戦闘不能のキャラは飛ばす
				if (s_list_DD_BattleCharacter[i].m_nCurrentHp <= 0) continue;
				//ATBにSPEED分追加する
				s_list_DD_BattleCharacter[i].m_nCurrentATB += s_list_DD_BattleCharacter[i].m_nCurrentSpeed;
				if (s_list_DD_BattleCharacter[i].m_nCurrentATB > MD_GameConstants.MAX_ACT)
				{
					s_list_DD_BattleCharacter[i].m_nCurrentATB = MD_GameConstants.MAX_ACT;
				}
				//	ATBがMAXでコマンドスタックに積まれていない場合積む
				if( s_list_DD_BattleCharacter[i].m_nCurrentATB == MD_GameConstants.MAX_ACT && s_list_CommandStackCharacter.Contains(i)==false)
				{
					s_list_CommandStackCharacter.Add(i);
				}
			}
		}
		//----------------------------------------------------------------------------------
		/// <summary>対象キャラクターに経験値を与える</summary>
		//----------------------------------------------------------------------------------
		static public bool IncExp(int nIndex,int nExp)
		{
			s_list_DD_BattleCharacter[nIndex].m_nCurrentExp += nExp;
			//レベルアップチェック
			int nLvExp = s_list_DD_BattleCharacter[nIndex].GetParameterFromLevel(EnumCharacterParameter.MAX_EXP);
			if( s_list_DD_BattleCharacter[nIndex].m_nCurrentExp >= nLvExp )
			{
				s_list_DD_BattleCharacter[nIndex].m_nCurrentLevel++;
				s_list_DD_BattleCharacter[nIndex].m_nMaxHp = s_list_DD_BattleCharacter[nIndex].GetParameterFromLevel(EnumCharacterParameter.MAX_HP);
				s_list_DD_BattleCharacter[nIndex].m_nCurrentHp = s_list_DD_BattleCharacter[nIndex].m_nMaxHp;
				s_list_DD_BattleCharacter[nIndex].m_nCurrentExp = 0;
				s_list_DD_BattleCharacter[nIndex].m_nMaxExp = s_list_DD_BattleCharacter[nIndex].GetParameterFromLevel(EnumCharacterParameter.MAX_EXP);
				s_list_DD_BattleCharacter[nIndex].m_nAtk = s_list_DD_BattleCharacter[nIndex].GetParameterFromLevel(EnumCharacterParameter.CURRENT_ATK);
				s_list_DD_BattleCharacter[nIndex].m_nDef = s_list_DD_BattleCharacter[nIndex].GetParameterFromLevel(EnumCharacterParameter.CURRENT_DEF);
				s_list_DD_BattleCharacter[nIndex].m_nMagicAtk = s_list_DD_BattleCharacter[nIndex].GetParameterFromLevel(EnumCharacterParameter.CURRENT_MATK);
				s_list_DD_BattleCharacter[nIndex].m_nMagicDef = s_list_DD_BattleCharacter[nIndex].GetParameterFromLevel(EnumCharacterParameter.CURRENT_MDEF);
				s_list_DD_BattleCharacter[nIndex].m_nDamage = s_list_DD_BattleCharacter[nIndex].GetParameterFromLevel(EnumCharacterParameter.CURRENT_DAMAGE);
				s_list_DD_BattleCharacter[nIndex].m_nMagicDamage = s_list_DD_BattleCharacter[nIndex].GetParameterFromLevel(EnumCharacterParameter.CURRENT_MDAMAGE);
				s_list_DD_BattleCharacter[nIndex].m_nMaxTp = s_list_DD_BattleCharacter[nIndex].GetParameterFromLevel(EnumCharacterParameter.MAX_TP);
				s_list_DD_BattleCharacter[nIndex].m_nCurrentSpeed = s_list_DD_BattleCharacter[nIndex].GetParameterFromLevel(EnumCharacterParameter.CURRENT_SPEED);
				return true;
			}
			return false;
		}
		//----------------------------------------------------------------------------------
		/// <summary>対象キャラクターのATBをリセット</summary>
		//----------------------------------------------------------------------------------
		static public void ResetATB(int nIndex)
		{
			s_list_DD_BattleCharacter[nIndex].m_nCurrentATB = 0;
		}
		//----------------------------------------------------------------------------------
		/// <summary>対象キャラのTPを増加</summary>
		//----------------------------------------------------------------------------------
		static public void IncTp(int nIndex, int nTp)
		{
			s_list_DD_BattleCharacter[nIndex].m_nCurrentTp += nTp;
			if (s_list_DD_BattleCharacter[nIndex].m_nCurrentTp > s_list_DD_BattleCharacter[nIndex].m_nMaxTp) s_list_DD_BattleCharacter[nIndex].m_nCurrentTp = s_list_DD_BattleCharacter[nIndex].m_nMaxTp;
		}
		//----------------------------------------------------------------------------------
		/// <summary>対象キャラのTPを消費</summary>
		//----------------------------------------------------------------------------------
		static public void DecTp(int nIndex, int nTp)
		{
			s_list_DD_BattleCharacter[nIndex].m_nCurrentTp -= nTp;
			if (s_list_DD_BattleCharacter[nIndex].m_nCurrentTp < 0) s_list_DD_BattleCharacter[nIndex].m_nCurrentTp = 0;
		}
		//----------------------------------------------------------------------------------
		/// <summary>指定のキャラクターにダメージを与える </summary>
		/// <param name="nIndex">DD内index</param>
		/// <param name="nDamage">与えるダメージ</param>
		/// <returns>残りHP</returns>
		//----------------------------------------------------------------------------------
		static public int DmageToCharacter(int nIndex, int nDamage)
		{
			//現在のHPからダメージ値分減らす
			s_list_DD_BattleCharacter[nIndex].m_nCurrentHp -= nDamage;
			if (s_list_DD_BattleCharacter[nIndex].m_nCurrentHp < 0) s_list_DD_BattleCharacter[nIndex].m_nCurrentHp = 0;
			return s_list_DD_BattleCharacter[nIndex].m_nCurrentHp;
		}
		//----------------------------------------------------------------------------------
		/// <summary>指定キャラを防御状態にする</summary>
		//----------------------------------------------------------------------------------
		static public void PreventDamageBuffToCharacter(int nIndex, int nPreventDamage, int nPreventTurn)
		{
			s_list_DD_BattleCharacter[nIndex].m_nPreventDamage = nPreventDamage;
			s_list_DD_BattleCharacter[nIndex].m_nPreventDamageTurn = nPreventTurn;
		}
		//----------------------------------------------------------------------------------
		/// <summary>指定キャラを挑発状態にする</summary>
		//----------------------------------------------------------------------------------
		static public void VokeBuffToCharacter(int nIndex, int nTargetIndex, int nVokeTurn)
		{
			s_list_DD_BattleCharacter[nIndex].m_nVokeIndex = nTargetIndex;
			s_list_DD_BattleCharacter[nIndex].m_nVokeTurn = nVokeTurn;
		}
		//----------------------------------------------------------------------------------
		/// <summary>指定のキャラクターを回復する</summary>
		/// <param name="nIndex">DD内index</param>
		/// <param name="nHealValue">回復量</param>
		/// <returns>実回復量</returns>
		//----------------------------------------------------------------------------------
		static public int HealToCharacter(int nIndex, int nHealValue)
		{
			int n = nHealValue;
			if (s_list_DD_BattleCharacter[nIndex].m_nCurrentHp + nHealValue > s_list_DD_BattleCharacter[nIndex].m_nMaxHp)
			{
				n = s_list_DD_BattleCharacter[nIndex].m_nMaxHp - s_list_DD_BattleCharacter[nIndex].m_nCurrentHp;
				s_list_DD_BattleCharacter[nIndex].m_nCurrentHp = s_list_DD_BattleCharacter[nIndex].m_nMaxHp;
			}
			else
			{
				s_list_DD_BattleCharacter[nIndex].m_nCurrentHp += nHealValue;
			}
			return n;
		}
		//----------------------------------------------------------------------------------
		/// <summary>指定キャラのバフを更新する</summary>
		//----------------------------------------------------------------------------------
		static public void UpadateBuffTurn(int nIndex)
		{
			s_list_DD_BattleCharacter[nIndex].m_nPreventDamageTurn--;
			if (s_list_DD_BattleCharacter[nIndex].m_nPreventDamageTurn < 0) s_list_DD_BattleCharacter[nIndex].m_nPreventDamageTurn = 0;
			s_list_DD_BattleCharacter[nIndex].m_nVokeTurn--;
			if (s_list_DD_BattleCharacter[nIndex].m_nVokeTurn < 0) s_list_DD_BattleCharacter[nIndex].m_nVokeTurn = 0;
		}
		//----------------------------------------------------------------------------------
		/// <summary>コマンドユニットスタックに積まれているキャラがいるか。いる場合は最初のキャラindexを返す -1:なし 0～:index</summary>
		//----------------------------------------------------------------------------------
		static public int ExistCommandStackCharacter()
		{
			if (s_list_CommandStackCharacter.Count == 0) return -1;
			return s_list_CommandStackCharacter[0];
		}
		//----------------------------------------------------------------------------------
		/// <summary>コマンドユニットスタックの戦闘を削除</summary>
		//----------------------------------------------------------------------------------
		static public bool DeleteCommandStackFirst()
		{
			if (s_list_CommandStackCharacter.Count == 0) return false;
			s_list_CommandStackCharacter.RemoveAt(0);
			return true;
		}
		//----------------------------------------------------------------------------------
		/// <summary>コマンドユニットスタックの初期化</summary>
		//----------------------------------------------------------------------------------
		static public void ClearCommandStack()
		{
			s_list_CommandStackCharacter = new List<int>();
		}
		//----------------------------------------------------------------------------------
		/// <summary>キャラクターのパラメータを得る</summary>
		//----------------------------------------------------------------------------------
		static public int GetCharacterParameter(int nIndex, EnumCharacterParameter ecp)
		{
			switch(ecp)
			{
				case EnumCharacterParameter.CURRENT_LEVEL:
					return s_list_DD_BattleCharacter[nIndex].m_nCurrentLevel;
				case EnumCharacterParameter.CURRENT_EXP:
					return s_list_DD_BattleCharacter[nIndex].m_nCurrentExp;
				case EnumCharacterParameter.MAX_EXP:
					return s_list_DD_BattleCharacter[nIndex].m_nMaxExp;
				case EnumCharacterParameter.CURRENT_HP:
					return s_list_DD_BattleCharacter[nIndex].m_nCurrentHp;
				case EnumCharacterParameter.MAX_HP:
					return s_list_DD_BattleCharacter[nIndex].m_nMaxHp;
				case EnumCharacterParameter.CURRENT_ATK:
					return s_list_DD_BattleCharacter[nIndex].m_nAtk;
				case EnumCharacterParameter.CURRENT_DEF:
					return s_list_DD_BattleCharacter[nIndex].m_nDef;
				case EnumCharacterParameter.CURRENT_MATK:
					return s_list_DD_BattleCharacter[nIndex].m_nMagicAtk;
				case EnumCharacterParameter.CURRENT_MDEF:
					return s_list_DD_BattleCharacter[nIndex].m_nMagicDef;
				case EnumCharacterParameter.CURRENT_WEAPON_ID:
					return s_list_DD_BattleCharacter[nIndex].m_nCurrentEquipWeaponId;
				case EnumCharacterParameter.CURRENT_ARMOR_ID:
					return s_list_DD_BattleCharacter[nIndex].m_nCurrentEquipArmorId;
				case EnumCharacterParameter.MAX_TP:
					return s_list_DD_BattleCharacter[nIndex].m_nMaxTp;
				case EnumCharacterParameter.CURRENT_TP:
					return s_list_DD_BattleCharacter[nIndex].m_nCurrentTp;
				case EnumCharacterParameter.MONSTER_ID:
					return s_list_DD_BattleCharacter[nIndex].m_Monsters.nId;
				case EnumCharacterParameter.CURRENT_SPEED:
					return s_list_DD_BattleCharacter[nIndex].m_nCurrentSpeed;
				case EnumCharacterParameter.CURRENT_ATB:
					return s_list_DD_BattleCharacter[nIndex].m_nCurrentATB;
				case EnumCharacterParameter.CHARACTER_ID:
					return s_list_DD_BattleCharacter[nIndex].m_Characters.nId;
				case EnumCharacterParameter.CURRENT_DAMAGE:
					return s_list_DD_BattleCharacter[nIndex].m_nDamage;
				case EnumCharacterParameter.CURRENT_WEAPON_DAMAGE:
					return s_list_DD_BattleCharacter[nIndex].m_nWeaponDamage;
				case EnumCharacterParameter.CURRENT_WEAPON_ATK:
					return s_list_DD_BattleCharacter[nIndex].m_nWeaponAtk;
				case EnumCharacterParameter.CURRENT_ARMOR_DEF:
					return s_list_DD_BattleCharacter[nIndex].m_nArmorDef;
				case EnumCharacterParameter.CURRENT_WEAPON_MAGIC_DAMAGE:
					return s_list_DD_BattleCharacter[nIndex].m_nWeaponMagicDamage;
				case EnumCharacterParameter.CURRENT_WEAPON_MAGIC_ATK:
					return s_list_DD_BattleCharacter[nIndex].m_nWeaponMagicAtk;
				case EnumCharacterParameter.CURRENT_ARMOR_MAGIC_DEF:
					return s_list_DD_BattleCharacter[nIndex].m_nArmorMagicDef;
				case EnumCharacterParameter.PREVENT_DAMAGE:
					return s_list_DD_BattleCharacter[nIndex].m_nPreventDamage;
				case EnumCharacterParameter.PREVENT_DAMAGE_TURN:
					return s_list_DD_BattleCharacter[nIndex].m_nPreventDamageTurn;
				case EnumCharacterParameter.VOKE_INDEX:
					return s_list_DD_BattleCharacter[nIndex].m_nVokeIndex;
				case EnumCharacterParameter.VOKE_TURN:
					return s_list_DD_BattleCharacter[nIndex].m_nVokeTurn;
				case EnumCharacterParameter.EQUIP_WEAPON_INDEX:
					return s_list_DD_BattleCharacter[nIndex].m_nEquipIndexWeapon;
				case EnumCharacterParameter.EQUIP_ARMOR_INDEX:
					return s_list_DD_BattleCharacter[nIndex].m_nEquipIndexArmor;
				default: break;
			}
			return -1;
		}
		//----------------------------------------------------------------------------------
		/// <summary>現在のレベルに応じたパラメータを得る(初期セットアップ、レベルアップ用)</summary>
		//----------------------------------------------------------------------------------
		private int GetParameterFromLevel(EnumCharacterParameter ecp)
		{
			if (m_Characters != null)
			{
				switch (ecp)
				{
					case EnumCharacterParameter.MAX_EXP:
						return (int)((float)m_Characters.nBaseExp * (float)MD_LevelTables.GetLevelTablesFromID(m_Characters.nExpLevelTableId).fValue(m_nCurrentLevel));
					case EnumCharacterParameter.MAX_HP:
						return (int)((float)m_Characters.nBaseHp * (float)MD_LevelTables.GetLevelTablesFromID(m_Characters.nHpLevelTableId).fValue(m_nCurrentLevel));
					case EnumCharacterParameter.CURRENT_ATK:
						return (int)((float)m_Characters.nBaseAtk * (float)MD_LevelTables.GetLevelTablesFromID(m_Characters.nAtkLevelTableId).fValue(m_nCurrentLevel));
					case EnumCharacterParameter.CURRENT_DEF:
						return (int)((float)m_Characters.nBaseDef * (float)MD_LevelTables.GetLevelTablesFromID(m_Characters.nDefLevelTableId).fValue(m_nCurrentLevel));
					case EnumCharacterParameter.CURRENT_MATK:
						return (int)((float)m_Characters.nBaseMagicAtk * (float)MD_LevelTables.GetLevelTablesFromID(m_Characters.nMagicAtkLevelTableId).fValue(m_nCurrentLevel));
					case EnumCharacterParameter.CURRENT_MDEF:
						return (int)((float)m_Characters.nBaseMagicDef * (float)MD_LevelTables.GetLevelTablesFromID(m_Characters.nMagicDefLevelTableId).fValue(m_nCurrentLevel));
					case EnumCharacterParameter.CURRENT_DAMAGE:
						return (int)((float)m_Characters.nBaseDamage * (float)MD_LevelTables.GetLevelTablesFromID(m_Characters.nDamageLevelTableId).fValue(m_nCurrentLevel));
					case EnumCharacterParameter.CURRENT_MDAMAGE:
						return (int)((float)m_Characters.nBaseMagicDamage * (float)MD_LevelTables.GetLevelTablesFromID(m_Characters.nMagicDamageLevelTableId).fValue(m_nCurrentLevel));
					case EnumCharacterParameter.MAX_TP:
						return (int)((float)m_Characters.nBaseTp * (float)MD_LevelTables.GetLevelTablesFromID(m_Characters.nTpLevelTableId).fValue(m_nCurrentLevel));
					case EnumCharacterParameter.CURRENT_SPEED:
						return (int)((float)m_Characters.nBaseSpeed * (float)MD_LevelTables.GetLevelTablesFromID(m_Characters.nSpeedLevelTableId).fValue(m_nCurrentLevel));
					default: break;
				}
			}
			return -1;
		}
		//----------------------------------------------------------------------------------
		//----------------------------------------------------------------------------------
		private void Init()
		{
			m_bIsCharacter = false;
			m_Characters = null;
			m_Monsters = null;
			m_nCurrentLevel = 1;
			//	レベルに応じたパラメータ設定
			m_nMaxHp = 1;
		}
	}
}
