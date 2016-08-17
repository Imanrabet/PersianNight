using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameFramework;
using DxLibDLL;

namespace PersianNight
{
	//================================================================================
	//	
	//================================================================================
	[Serializable]
	public class Wnd_CharaStatus : Drawable
	{
		private Wnd_BattleStatusCharacter[] m_Wnd_BattleStatusCharacter;
		private int m_nMoney = 0;
		public int nMoney { set { m_nMoney = value; } }
		private Drawable[] m_drawable_SelectWaku;
		private bool m_bSubtargetSelectMode = false;
		//================================================================================
		//	
		//================================================================================
		public override void Initialize()
		{
			//	基底初期化
			base.Initialize();
			//	描画設定
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "mew-2.png"), 1);
			m_fDrawPos_X = 590;
			m_fDrawPos_Y = 0;
			m_nTransFlag = 1;
			m_bMouseAct = false;						//	マウス動作
			m_bMouseButtonLeftClickEnable = true;	//	左クリックを処理するか
			m_bDefaultMouseLeftClickEvent = false;	//	左クリックメッセージを親に投げるかどうか
			m_bMouseButtonRightClickEnable = true;	//	右クリックを処理するか
			m_InnerPostEventArgs = new GameFramework.InnerPostEventArgs(EnumGUIEvent.EVENT_EXIT);
			m_bDefaultMouseRightClickEvent = true;	//	右クリックメッセージを親に投げるかどうか
			//
			m_Wnd_BattleStatusCharacter = new Wnd_BattleStatusCharacter[2];
			//
			m_Wnd_BattleStatusCharacter[0] = new Wnd_BattleStatusCharacter();
			m_Wnd_BattleStatusCharacter[0].Initialize();
			m_Wnd_BattleStatusCharacter[0].fDrawPos_X = 10;
			m_Wnd_BattleStatusCharacter[0].fDrawPos_Y = 10;
			m_Wnd_BattleStatusCharacter[0].SetGraphicHandle(-1, 1);
			AddChildDrawable(m_Wnd_BattleStatusCharacter[0], this);
			//
			m_Wnd_BattleStatusCharacter[1] = new Wnd_BattleStatusCharacter();
			m_Wnd_BattleStatusCharacter[1].Initialize();
			m_Wnd_BattleStatusCharacter[1].fDrawPos_X = 10;
			m_Wnd_BattleStatusCharacter[1].fDrawPos_Y = 232;
			m_Wnd_BattleStatusCharacter[1].SetGraphicHandle(-1, 1);
			AddChildDrawable(m_Wnd_BattleStatusCharacter[1], this);
			//
			m_drawable_SelectWaku = new Drawable[2] { new Drawable(), new Drawable() };
			m_drawable_SelectWaku[0].Initialize();
			m_drawable_SelectWaku[0].fDrawPos_X = 5;
			m_drawable_SelectWaku[0].fDrawPos_Y = 5;
			m_drawable_SelectWaku[0].SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "select_waku_clear.png"), 1);
			m_drawable_SelectWaku[0].SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "select_waku.png"), 2);
			m_drawable_SelectWaku[0].SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "select_waku.png"), 3);
			m_drawable_SelectWaku[0].bMouseAct = false;						//	マウス動作
			m_drawable_SelectWaku[0].bMouseButtonLeftClickEnable = true;	//	左クリックを処理するか
			m_drawable_SelectWaku[0].bDefaultMouseLeftClickEvent = true;	//	左クリックメッセージを親に投げるかどうか
			m_drawable_SelectWaku[0].bMouseButtonRightClickEnable = true;	//	右クリックを処理するか
			m_drawable_SelectWaku[0].bDefaultMouseRightClickEvent = true;	//	左クリックメッセージを親に投げるかどうか
			AddChildDrawable(m_drawable_SelectWaku[0], this);
			m_drawable_SelectWaku[1].Initialize();
			m_drawable_SelectWaku[1].fDrawPos_X = 5;
			m_drawable_SelectWaku[1].fDrawPos_Y = 225;
			m_drawable_SelectWaku[1].SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "select_waku_clear.png"), 1);
			m_drawable_SelectWaku[1].SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "select_waku.png"), 2);
			m_drawable_SelectWaku[1].SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "select_waku.png"), 3);
			m_drawable_SelectWaku[1].bMouseAct = false;						//	マウス動作
			m_drawable_SelectWaku[1].bMouseButtonLeftClickEnable = true;	//	左クリックを処理するか
			m_drawable_SelectWaku[1].bDefaultMouseLeftClickEvent = true;	//	左クリックメッセージを親に投げるかどうか
			m_drawable_SelectWaku[1].bMouseButtonRightClickEnable = true;	//	右クリックを処理するか
			m_drawable_SelectWaku[1].bDefaultMouseRightClickEvent = true;	//	左クリックメッセージを親に投げるかどうか
			AddChildDrawable(m_drawable_SelectWaku[1], this);
		}
		//================================================================================
		/// <summary>サブターゲット機能をオン・オフする</summary>
		//================================================================================
		public void SwitchSubtargetFunc(bool bONOFF, int[] argsObj)
		{
			if( bONOFF == false)
			{
				m_bSubtargetSelectMode = false;
				m_drawable_SelectWaku[0].bMouseAct = false;
				m_drawable_SelectWaku[1].bMouseAct = false;
				return;
			}
			m_bSubtargetSelectMode = true;
			m_drawable_SelectWaku[0].bMouseAct = true;
			m_drawable_SelectWaku[1].bMouseAct = true;
			m_drawable_SelectWaku[0].InnerPostEventArgsLeftClick = new InnerPostEventArgs(EnumPanelEvent.ACTION);
			m_drawable_SelectWaku[0].InnerPostEventArgsLeftClick.oGenericObject = new int[] { argsObj[0], 0 };
			m_drawable_SelectWaku[1].InnerPostEventArgsLeftClick = new InnerPostEventArgs(EnumPanelEvent.ACTION);
			m_drawable_SelectWaku[1].InnerPostEventArgsLeftClick.oGenericObject = new int[] { argsObj[0], 1 };
			m_drawable_SelectWaku[0].InnerPostEventArgsRightClick = new InnerPostEventArgs(EnumPanelEvent.CANCEL_SUBTARGET);
			m_drawable_SelectWaku[1].InnerPostEventArgsRightClick = new InnerPostEventArgs(EnumPanelEvent.CANCEL_SUBTARGET);
			//マウスを強制移動
			//DX.SetMousePoint(m_nDrawPosScreen_X + 50, m_nDrawPosScreen_Y + 50);
		}
		//================================================================================
		//	
		//================================================================================
		public override void Draw()
		{
			base.Draw();
			if (m_bDoDraw == true)
			{
				int fh = GamePersianNight.s_nFONT_HANDLE_16;
				DX.DrawStringToHandle(this.m_nDrawPosScreen_X + 80, this.m_nDrawPosScreen_Y + 455, m_nMoney.ToString(), COLOR_BLACK, fh);
			}
		}
		//================================================================================
		//	
		//================================================================================
		public override void Update()
		{
			base.Update();
			if ( GamePersianNight.bIsBattle == true )
			{
				for (int i = 0; i < 2; i++)
				{
					m_Wnd_BattleStatusCharacter[i].nDDCharaIndex = i;
					m_Wnd_BattleStatusCharacter[i].nCurrentLevel = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_LEVEL);
					m_Wnd_BattleStatusCharacter[i].nCurrentHp = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_HP);
					m_Wnd_BattleStatusCharacter[i].nMaxHp = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.MAX_HP);
					m_Wnd_BattleStatusCharacter[i].nCurrentTp = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_TP);
					m_Wnd_BattleStatusCharacter[i].nMaxTp = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.MAX_TP);
					m_Wnd_BattleStatusCharacter[i].nCurrentExp = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_EXP);
					m_Wnd_BattleStatusCharacter[i].nNextExp = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.MAX_EXP);
					m_Wnd_BattleStatusCharacter[i].nATK = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_ATK) + DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_WEAPON_ATK);
					m_Wnd_BattleStatusCharacter[i].nDEF = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_DEF) + DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_ARMOR_DEF);
					m_Wnd_BattleStatusCharacter[i].nMATK = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_MATK + DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_WEAPON_MAGIC_ATK));
					m_Wnd_BattleStatusCharacter[i].nMDEF = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_MDEF + DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_ARMOR_MAGIC_DEF));
					m_Wnd_BattleStatusCharacter[i].szEquipment[0] = MD_Items.GetItemsFromID(DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_WEAPON_ID)).szName;
					m_Wnd_BattleStatusCharacter[i].szEquipment[1] = MD_Items.GetItemsFromID(DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_ARMOR_ID)).szName;
					m_Wnd_BattleStatusCharacter[i].fCurrentATB = (float)(DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_ATB));
					m_Wnd_BattleStatusCharacter[i].fMaxATB = MD_GameConstants.MAX_ACT;
				}
				m_Wnd_BattleStatusCharacter[0].SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + GamePersianNight.CurrentHero.szImageFace), 1);
				m_Wnd_BattleStatusCharacter[1].SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + GamePersianNight.CurrentHeroin.szImageFace), 1);
				m_nMoney = GamePersianNight.nMoney;
			}
		}
		//================================================================================
		//	
		//================================================================================
		public void ClearATB()
		{
			for (int i = 0; i < 2; i++)
			{
				m_Wnd_BattleStatusCharacter[i].fCurrentATB = 0;
			}
		}
		//================================================================================
		//	
		//================================================================================
		public void Setup()
		{
			for (int i = 0; i < 2;i++ )
			{
				m_Wnd_BattleStatusCharacter[i].nCurrentLevel = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_LEVEL);
				m_Wnd_BattleStatusCharacter[i].nCurrentHp = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_HP);
				m_Wnd_BattleStatusCharacter[i].nMaxHp = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.MAX_HP);
				m_Wnd_BattleStatusCharacter[i].nCurrentTp = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_TP);
				m_Wnd_BattleStatusCharacter[i].nMaxTp = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.MAX_TP);
				m_Wnd_BattleStatusCharacter[i].nCurrentExp = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_EXP);
				m_Wnd_BattleStatusCharacter[i].nNextExp = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.MAX_EXP);
				m_Wnd_BattleStatusCharacter[i].nATK = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_ATK) + DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_WEAPON_ATK);
				m_Wnd_BattleStatusCharacter[i].nDEF = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_DEF) + DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_ARMOR_DEF);
				m_Wnd_BattleStatusCharacter[i].nMATK = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_MATK + DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_WEAPON_MAGIC_ATK));
				m_Wnd_BattleStatusCharacter[i].nMDEF = DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_MDEF + DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_ARMOR_MAGIC_DEF));
				m_Wnd_BattleStatusCharacter[i].szEquipment[0] = MD_Items.GetItemsFromID(DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_WEAPON_ID)).szName;
				m_Wnd_BattleStatusCharacter[i].szEquipment[1] = MD_Items.GetItemsFromID(DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CURRENT_ARMOR_ID)).szName;
				m_Wnd_BattleStatusCharacter[i].szEquipment[2] = MD_Actions.GetActionsFromID(MD_Characters.GetCharactersFromID(DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CHARACTER_ID)).list_ActionId[2]).szName;
				m_Wnd_BattleStatusCharacter[i].szEquipment[3] = MD_Actions.GetActionsFromID(MD_Characters.GetCharactersFromID(DD_BattleCharacter.GetCharacterParameter(i, EnumCharacterParameter.CHARACTER_ID)).list_ActionId[3]).szName;
				m_Wnd_BattleStatusCharacter[i].fCurrentATB = 0;
				m_Wnd_BattleStatusCharacter[i].fMaxATB = MD_GameConstants.MAX_ACT;
				m_Wnd_BattleStatusCharacter[i].nDDCharaIndex = i;
				m_Wnd_BattleStatusCharacter[i].SetDDCharacterIndex(i);
			}
			m_Wnd_BattleStatusCharacter[0].SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + GamePersianNight.CurrentHero.szImageFace), 1);
			m_Wnd_BattleStatusCharacter[1].SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + GamePersianNight.CurrentHeroin.szImageFace), 1);
			m_nMoney = GamePersianNight.nMoney;
			
		}
	}
}
