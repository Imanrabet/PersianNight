using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameFramework;
using DxLibDLL;

namespace PersianNight
{
	public enum EnumPanelEvent
	{
		NULL,
		/// <summary>前へ進む</summary>
		PROCEED,
		/// <summary>データセーブ</summary>
		SAVE,
		/// <summary>戦闘回避</summary>
		ESCAPE,
		/// <summary>戦闘開始</summary>
		BATTLE,
		/// <summary>行動実行</summary>
		ACTION,
		/// <summary>サブターゲット選択せよ</summary>
		SUBTARGET,
		/// <summary>サブターゲットのキャンセル</summary>
		CANCEL_SUBTARGET,
		/// <summary>パネルのポーズからの再開</summary>
		RESUME,
		/// <summary>敵死亡</summary>
		ENEMY_DEAD,
		/// <summary>パーティ全滅</summary>
		PARTY_DEAD,
		/// <summary>オアシスで休む</summary>
		REST,
		/// <summary>ジン選択肢</summary>
		DIN,
		/// <summary>ジン選択終了</summary>
		BYE_DIN
	}
	//================================================================================
	//	
	//================================================================================
	[Serializable]
	public class Wnd_BattleCommandPanel : Drawable
	{
		private List<Drawable> m_list_draw_SelectButtons;
		private string[] m_szSelectButtonString;
		private Drawable m_CharacterFace;
		private string[] m_szDescription;
		private List<int> m_list_COMMAND_TEXT_COLOR;
		//================================================================================
		//	
		//================================================================================
		public override void Initialize()
		{
			//	基底初期化
			base.Initialize();
			//	描画設定
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "mess-s.png"), 1);
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "mess-s.png"), 2);
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "mess-s.png"), 3);
			m_fDrawPos_X = 0;
			m_fDrawPos_Y = 350;
			m_nTransFlag = 1;
			m_bMouseAct = false;						//	マウス動作
			m_bMouseButtonLeftClickEnable = true;	//	左クリックを処理するか
			m_bDefaultMouseLeftClickEvent = true;	//	左クリックメッセージを親に投げるかどうか
			m_bMouseButtonRightClickEnable = true;	//	右クリックを処理するか
			m_InnerPostEventArgs = new GameFramework.InnerPostEventArgs(EnumGUIEvent.EVENT_EXIT);
			m_bDefaultMouseRightClickEvent = true;	//	右クリックメッセージを親に投げるかどうか
			//
			m_list_draw_SelectButtons = new List<Drawable>();
			for (int i = 0; i < 4;i++ )
			{
				Drawable d = new Drawable();
				d.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "button_01.png"), 1);
				d.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "button_02.png"), 2);
				d.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "button_02.png"), 3);
				d.nTransFlag = 1;
				d.fDrawPos_X = 250;
				d.fDrawPos_Y = 46 + i * 18;
				d.bMouseAct = true;					//	マウス動作
				d.bMouseButtonLeftClickEnable = true;	//	左クリック動作
				d.bDefaultMouseLeftClickEvent = true;
				d.InnerPostEventArgs = new GameFramework.InnerPostEventArgs(EnumPanelEvent.NULL);
				AddChildDrawable(d, this);
				m_list_draw_SelectButtons.Add(d);
			}
			//
			m_CharacterFace = new Drawable();
			m_CharacterFace.SetGraphicHandle(-1, 1);
			m_CharacterFace.nTransFlag = 1;
			m_CharacterFace.fDrawPos_X = 180;
			m_CharacterFace.fDrawPos_Y = 10;
			m_CharacterFace.bMouseAct = false;					//	マウス動作
			m_CharacterFace.bMouseButtonLeftClickEnable = false;	//	左クリック動作
			m_CharacterFace.bDefaultMouseLeftClickEvent = false;	//	左クリック動作
			AddChildDrawable(m_CharacterFace, this);

			//
			m_szSelectButtonString = new string[4] { "", "", "", "" };
			m_szDescription = new string[] { "", "", "", "", "", "" };
			//
			m_list_COMMAND_TEXT_COLOR = new List<int>() { COLOR_BLACK, COLOR_BLACK, COLOR_BLACK, COLOR_BLACK };
			//
		}
		//================================================================================
		/// <summary>コマンド文字列の設定</summary>
		//================================================================================
		public void SetButtonString(int nIndex, string szString, EnumPanelEvent EnumPanelEvent, object obj)
		{
			m_szSelectButtonString[nIndex] = szString;
			m_list_draw_SelectButtons[nIndex].InnerPostEventArgs = new GameFramework.InnerPostEventArgs(EnumPanelEvent);
			m_list_draw_SelectButtons[nIndex].InnerPostEventArgs.oGenericObject = obj;
		}
		//================================================================================
		/// <summary>コマンド文字列の設定　COLOR_RED を指定すると、マウスメッセージを発生させない</summary>
		//================================================================================
		public void SetButtonString(int nIndex, string szString, EnumPanelEvent EnumPanelEvent,object obj,int TEXT_COLOR)
		{
			m_list_COMMAND_TEXT_COLOR[nIndex] = TEXT_COLOR;
			SetButtonString(nIndex, szString, EnumPanelEvent, obj);
			if( TEXT_COLOR == COLOR_RED)
			{
				m_list_draw_SelectButtons[nIndex].bMouseAct = false;
			}
			else
			{
				m_list_draw_SelectButtons[nIndex].bMouseAct = true;
			}
		}
		public void SetDescription(int nIndex,string szString)
		{
			m_szDescription[nIndex] = szString;
		}
		public void SetButtonEnable(int nIndex,bool bEnable)
		{
			m_list_draw_SelectButtons[nIndex].bDoDraw = bEnable;
		}
		//================================================================================
		/// <summary>パネルの初期化</summary>
		//================================================================================
		public void Setup()
		{
			SetButtonString(0, "", EnumPanelEvent.NULL, null, COLOR_BLACK);
			SetButtonString(1, "", EnumPanelEvent.NULL, null, COLOR_BLACK);
			SetButtonString(2, "", EnumPanelEvent.NULL, null, COLOR_BLACK);
			SetButtonString(3, "", EnumPanelEvent.NULL, null, COLOR_BLACK);
			SetButtonEnable(0, false);
			SetButtonEnable(1, false);
			SetButtonEnable(2, false);
			SetButtonEnable(3, false);
			SetDescription(0, "");
			SetDescription(1, "");
			SetDescription(2, "");
			SetDescription(3, "");
			SetDescription(4, "");
			SetDescription(5, "");
		}
		//================================================================================
		/// <summary>サブターゲット選択中</summary>
		//================================================================================
		public void DoSubtarget(string sz)
		{
			m_list_draw_SelectButtons[0].bDoDraw = false;
			m_list_draw_SelectButtons[1].bDoDraw = false;
			m_list_draw_SelectButtons[2].bDoDraw = false;
			m_list_draw_SelectButtons[3].bDoDraw = false;
			SetDescription(1, sz+"の対象を選択");
		}
		//================================================================================
		/// <summary>サブターゲットがキャンセルされた</summary>
		//================================================================================
		public void RevertSubtarget()
		{
			SetDescription(1, "「どうする？");
			m_list_draw_SelectButtons[0].bDoDraw = true;
			m_list_draw_SelectButtons[1].bDoDraw = true;
			m_list_draw_SelectButtons[2].bDoDraw = true;
			m_list_draw_SelectButtons[3].bDoDraw = true;
		}
		//================================================================================
		//	
		//================================================================================
		public void SetCharacterFace(MD_Characters ch)
		{
			if( ch==null)
			{
				m_CharacterFace.SetGraphicHandle(-1, 1);
				return;
			}
			m_CharacterFace.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + ch.szImageFace), 1);
		}
		public void SetCharacterFace(int nCharacterId)
		{
			m_CharacterFace.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Characters.GetCharactersFromID(nCharacterId).szImageFace), 1);
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
				DX.DrawStringToHandle(m_list_Child_drawable[0].nDrawPosScreen_X - 16, 360, m_szDescription[0], COLOR_BLACK, fh);
				DX.DrawStringToHandle(m_list_Child_drawable[0].nDrawPosScreen_X - 16, 378, m_szDescription[1], COLOR_BLACK, fh);
				DX.DrawStringToHandle(m_list_Child_drawable[0].nDrawPosScreen_X - 16, 396, m_szDescription[2], COLOR_BLACK, fh);
				DX.DrawStringToHandle(m_list_Child_drawable[0].nDrawPosScreen_X - 16, 414, m_szDescription[3], COLOR_BLACK, fh);
				DX.DrawStringToHandle(m_list_Child_drawable[0].nDrawPosScreen_X - 16, 432, m_szDescription[4], COLOR_BLACK, fh);
				DX.DrawStringToHandle(m_list_Child_drawable[0].nDrawPosScreen_X - 16, 450, m_szDescription[5], COLOR_BLACK, fh);
				if (m_list_Child_drawable[0].bDoDraw == true) DX.DrawStringToHandle(m_list_Child_drawable[0].nDrawPosScreen_X, m_list_Child_drawable[0].nDrawPosScreen_Y, m_szSelectButtonString[0].ToString(), m_list_COMMAND_TEXT_COLOR[0], fh);
				if (m_list_Child_drawable[1].bDoDraw == true) DX.DrawStringToHandle(m_list_Child_drawable[1].nDrawPosScreen_X, m_list_Child_drawable[1].nDrawPosScreen_Y, m_szSelectButtonString[1].ToString(), m_list_COMMAND_TEXT_COLOR[1], fh);
				if (m_list_Child_drawable[2].bDoDraw == true) DX.DrawStringToHandle(m_list_Child_drawable[2].nDrawPosScreen_X, m_list_Child_drawable[2].nDrawPosScreen_Y, m_szSelectButtonString[2].ToString(), m_list_COMMAND_TEXT_COLOR[2], fh);
				if (m_list_Child_drawable[3].bDoDraw == true) DX.DrawStringToHandle(m_list_Child_drawable[3].nDrawPosScreen_X, m_list_Child_drawable[3].nDrawPosScreen_Y, m_szSelectButtonString[3].ToString(), m_list_COMMAND_TEXT_COLOR[3], fh);
			}
		}
		//================================================================================
		/// <summary>バトル突入処理</summary>
		//================================================================================
		public void EnterBattle()
		{
			Setup();
		}

	}
}
