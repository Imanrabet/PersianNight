using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianNight
{
	[Serializable]
	public class EquippableItem
	{
		public int nIndex;
		/// <summary>装備中のキャラ -1:未装備 0:主人公 1:ヒロイン</summary>
		public int nEquipCharaIndex = -1;
		public MD_Items MD_Item;
	}
	//==================================================================================
	//	動的データ：アイテムボックス
	//==================================================================================
	[Serializable]
	public class DD_ItemBox
	{
		/// <summary>所持アイテムリスト</summary>
		static public List<EquippableItem> s_list_InBoxItems;
		/// <summary>インデックス発行用カウンター。セーブ対象</summary>
		static public int s_nIndexCounter = 0;
		//==================================================================================
		//	動的データ：アイテムボックス
		//==================================================================================
		static DD_ItemBox()
		{
			s_list_InBoxItems = new List<EquippableItem>();
		}
		//==================================================================================
		/// <summary>アイテムボックスを初期化</summary>
		//==================================================================================
		static public void ClearItemBox()
		{
			s_list_InBoxItems.Clear();
		}
		//==================================================================================
		/// <summary>アイテムindexからEquippableItemを得る</summary>
		//==================================================================================
		static public EquippableItem GetEquippableItemFromIndex(int nItemIndex)
		{
			for(int i=0;i<s_list_InBoxItems.Count;i++)
			{
				if (s_list_InBoxItems[i].nIndex == nItemIndex) return s_list_InBoxItems[i];
			}
			return null;
		}
		//==================================================================================
		/// <summary>アイテムボックスにアイテムを追加する(同一アイテムは追加できない)</summary>
		/// <param name="nItemId">マスターデータのアイテムID</param>
		/// <param name="EquipCharaIndex">装備キャラindex</param>
		//==================================================================================
		static public int AddItem(int nItemId, int nEquipCharaIndex)
		{
			for(int i=0;i<s_list_InBoxItems.Count;i++)
			{
				if (s_list_InBoxItems[i].MD_Item.nId == nItemId) { return -1; }
			}
			EquippableItem ei = new EquippableItem();
			ei.nIndex = s_nIndexCounter;
			s_nIndexCounter++;
			ei.MD_Item = MD_Items.GetItemsFromID(nItemId);
			ei.nEquipCharaIndex = nEquipCharaIndex;
			s_list_InBoxItems.Add(ei);
			return ei.nIndex;
		}
		//==================================================================================
		/// <summary>指定IDのキャラの初期装備をインスタンス化して追加する</summary>
		/// <param name="nId"></param>
		//==================================================================================
		static public void InitItemFromCharacter(int nDDCharaIndex)
		{
			int nInitWeaponId = MD_Characters.GetCharactersFromID(DD_BattleCharacter.GetCharacterParameter(nDDCharaIndex,EnumCharacterParameter.CHARACTER_ID)).nInitWeapon;
			AddItem(nInitWeaponId, nDDCharaIndex);
			int nInitArmorId = MD_Characters.GetCharactersFromID(DD_BattleCharacter.GetCharacterParameter(nDDCharaIndex, EnumCharacterParameter.CHARACTER_ID)).nInitArmor;
			AddItem(nInitArmorId, nDDCharaIndex);
		}
		//==================================================================================
		/// <summary>装備変更</summary>
		/// <param name="nRotate">ローテート方向 -1:← 1:→ </param>
		/// <param name="nCategory">装備カテゴリ 1:武器 2:防具</param>
		/// <param name="nDDCharaIndex">対象キャラ(DDBCのindex)</param>
		/// <returns>変更した装備のIndex</returns>
		//==================================================================================
		static public int Rotate(int nRotate, int nCategory, int nDDCharaIndex)
		{
			//対象カテゴリーのテンポラリリストを生成(自身が装備中か未装備のみ)
			List<EquippableItem> list = new List<EquippableItem>();
			for (int i = 0; i < s_list_InBoxItems.Count; i++) {
				if (s_list_InBoxItems[i].MD_Item.nCategory == nCategory && (s_list_InBoxItems[i].nEquipCharaIndex == -1 || s_list_InBoxItems[i].nEquipCharaIndex == nDDCharaIndex))
				{
					list.Add(s_list_InBoxItems[i]);
				}
			}
//			//装備可能なものがなければ、現在の装備indexをそのまま返す
//			if (list.Count == 0 && nCategory == 1) return DD_BattleCharacter.GetCharacterParameter(nDDCharaIndex, EnumCharacterParameter.EQUIP_WEAPON_INDEX);
//			if (list.Count == 0 && nCategory == 2) return DD_BattleCharacter.GetCharacterParameter(nDDCharaIndex, EnumCharacterParameter.EQUIP_ARMOR_INDEX);
			//現在装備のlist内のindex
			int nCurrentIndex = -1;
			//現在装備の現在位置を検索
			for(int i=0;i<list.Count;i++)
			{
				if (list[i].nEquipCharaIndex == nDDCharaIndex)
				{
					nCurrentIndex = i; break;
				}
			}
			//装備状態を外す
			list[nCurrentIndex].nEquipCharaIndex = -1;
			//次の装備へ
			nCurrentIndex += nRotate;
			//範囲外チェック
			if (nCurrentIndex < 0) nCurrentIndex = list.Count - 1;
			if (nCurrentIndex >= list.Count) nCurrentIndex = 0;
			//装備する
			list[nCurrentIndex].nEquipCharaIndex = nDDCharaIndex;
			DD_BattleCharacter.EquipItem(nDDCharaIndex, nCategory, list[nCurrentIndex].nIndex);
			return list[nCurrentIndex].nIndex;
		}

	}
}
