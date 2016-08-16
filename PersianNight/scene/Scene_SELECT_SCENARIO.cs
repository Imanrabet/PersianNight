using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using GameFramework;
using DxLibDLL;

namespace PersianNight
{
	public enum Enum_Scene_SELECT_SCENARIO_Event
	{
		OVER_1,
		OVER_2,
		OVER_3,
		OVER_4
	}
	//================================================================================
	//	
	//================================================================================
	public class Scene_SELECT_SCENARIO : Scene
	{
		private Drawable m_draw_character_silhouette;	//	キャラクターシルエット
		private Drawable m_draw_select_button_1;		//	シナリオ選択ボタン１
		private Drawable m_draw_select_button_2;		//	シナリオ選択ボタン１
		private Drawable m_draw_select_button_3;		//	シナリオ選択ボタン１
		private Drawable m_draw_select_button_4;		//	シナリオ選択ボタン１
		private int m_nSelectedScenarioId;				//	選択されたシナリオID
		//================================================================================
		//	
		//================================================================================
		public override void Initialize()
		{
			//	基底初期化
			base.Initialize();
			//	描画設定
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "砂漠.jpg"), 1);
			m_fDrawPos_X = 0;
			m_fDrawPos_Y = 0;
			m_nTransFlag = 0;
			m_bMouseAct = false;					//	マウス動作
			m_bMouseButtonLeftClickEnable = false;	//	左クリック動作
			m_bDefaultMouseLeftClickEvent = false;	//	左クリック動作
			m_bDragEnable = false;					//	ドラッグ処理
			m_nDragTopLeftX = 0;					//	ドラッグ範囲
			m_nDragTopLeftY = 0;					//	ドラッグ範囲
			m_nDragBottomRightX = 0;				//	ドラッグ範囲
			m_nDragBottomRightY = 0;				//	ドラッグ範囲
			//	キャラクターシルエット
			m_draw_character_silhouette = new Drawable();
			m_draw_character_silhouette.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "シナリオ選択画面（シルエット）１.png"), 1);
			m_draw_character_silhouette.nTransFlag = 1;
			m_draw_character_silhouette.fDrawPos_X = 0;
			m_draw_character_silhouette.fDrawPos_Y = 0;
			m_draw_character_silhouette.bMouseAct = false;					//	マウス動作
			AddChildDrawable(m_draw_character_silhouette, this);
			//	シナリオ選択ボタン１
			m_draw_select_button_1 = new Drawable();
			m_draw_select_button_1.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "選択ボタン模様－05.png"), 1);
			m_draw_select_button_1.nTransFlag = 1;
			m_draw_select_button_1.fDrawPos_X = 0;
			m_draw_select_button_1.fDrawPos_Y = 60;
			m_draw_select_button_1.bMouseAct = true;					//	マウス動作
			m_draw_select_button_1.bMouseButtonLeftClickEnable = true;	//	左クリック動作
			m_draw_select_button_1.bDefaultMouseLeftClickEvent = true;	//	左クリック動作
			m_draw_select_button_1.bMouseOverEnable = true;
			m_draw_select_button_1.bDefaultMouseOverEvent = true;
			m_draw_select_button_1.InnerPostEventArgsMouseOver = new InnerPostEventArgs(Enum_Scene_SELECT_SCENARIO_Event.OVER_1);
			InnerPostEventArgs a = new InnerPostEventArgs(EnumGUIEvent.CLICK_SELECT_SCENARIO);
			a.oGenericObject = 1;
			m_draw_select_button_1.InnerPostEventArgsLeftClick = a;
			AddChildDrawable(m_draw_select_button_1, this);
			//	シナリオ選択ボタン２
			m_draw_select_button_2 = new Drawable();
			m_draw_select_button_2.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "選択ボタン模様－05.png"), 1);
			m_draw_select_button_2.nTransFlag = 1;
			m_draw_select_button_2.fDrawPos_X = 0;
			m_draw_select_button_2.fDrawPos_Y = 140;
			m_draw_select_button_2.bMouseAct = true;					//	マウス動作
			m_draw_select_button_2.bMouseButtonLeftClickEnable = true;	//	左クリック動作
			m_draw_select_button_2.bDefaultMouseLeftClickEvent = true;	//	左クリック動作
			m_draw_select_button_2.bMouseOverEnable = true;
			m_draw_select_button_2.bDefaultMouseOverEvent = true;
			m_draw_select_button_2.InnerPostEventArgsMouseOver = new InnerPostEventArgs(Enum_Scene_SELECT_SCENARIO_Event.OVER_2);
			m_draw_select_button_2.InnerPostEventArgs = new InnerPostEventArgs(EnumGUIEvent.CLICK_SELECT_SCENARIO);
			m_draw_select_button_2.InnerPostEventArgs.oGenericObject = 2;
			AddChildDrawable(m_draw_select_button_2, this);
			//	シナリオ選択ボタン３
			m_draw_select_button_3 = new Drawable();
			m_draw_select_button_3.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "選択ボタン模様－05.png"), 1);
			m_draw_select_button_3.nTransFlag = 1;
			m_draw_select_button_3.fDrawPos_X = 0;
			m_draw_select_button_3.fDrawPos_Y = 220;
			m_draw_select_button_3.bMouseAct = true;					//	マウス動作
			m_draw_select_button_3.bMouseButtonLeftClickEnable = true;	//	左クリック動作
			m_draw_select_button_3.bDefaultMouseLeftClickEvent = true;	//	左クリック動作
			m_draw_select_button_3.bMouseOverEnable = true;
			m_draw_select_button_3.bDefaultMouseOverEvent = true;
			m_draw_select_button_3.InnerPostEventArgsMouseOver = new InnerPostEventArgs(Enum_Scene_SELECT_SCENARIO_Event.OVER_3);
			m_draw_select_button_3.InnerPostEventArgs = new InnerPostEventArgs(EnumGUIEvent.CLICK_SELECT_SCENARIO);
			m_draw_select_button_3.InnerPostEventArgs.oGenericObject = 3;
			AddChildDrawable(m_draw_select_button_3, this);
			//	シナリオ選択ボタン４
			m_draw_select_button_4 = new Drawable();
			m_draw_select_button_4.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "選択ボタン模様－05.png"), 1);
			m_draw_select_button_4.nTransFlag = 1;
			m_draw_select_button_4.fDrawPos_X = 0;
			m_draw_select_button_4.fDrawPos_Y = 300;
			m_draw_select_button_4.bMouseAct = true;					//	マウス動作
			m_draw_select_button_4.bMouseButtonLeftClickEnable = true;	//	左クリック動作
			m_draw_select_button_4.bDefaultMouseLeftClickEvent = true;	//	左クリック動作
			m_draw_select_button_4.bMouseOverEnable = true;
			m_draw_select_button_4.bDefaultMouseOverEvent = true;
			m_draw_select_button_4.InnerPostEventArgsMouseOver = new InnerPostEventArgs(Enum_Scene_SELECT_SCENARIO_Event.OVER_4);
			m_draw_select_button_4.InnerPostEventArgs = new InnerPostEventArgs(EnumGUIEvent.CLICK_SELECT_SCENARIO);
			m_draw_select_button_4.InnerPostEventArgs.oGenericObject = 4;
			AddChildDrawable(m_draw_select_button_4, this);
			//------------------------------------------------
			//	イベント登録
			//------------------------------------------------
			AddEventFunc(Enum_Scene_SELECT_SCENARIO_Event.OVER_1, (InnerPostEventHandler)OVER_1);
			AddEventFunc(Enum_Scene_SELECT_SCENARIO_Event.OVER_2, (InnerPostEventHandler)OVER_2);
			AddEventFunc(Enum_Scene_SELECT_SCENARIO_Event.OVER_3, (InnerPostEventHandler)OVER_3);
			AddEventFunc(Enum_Scene_SELECT_SCENARIO_Event.OVER_4, (InnerPostEventHandler)OVER_4);

		}
		//================================================================================
		//	
		//================================================================================
		public override void Update()
		{
			base.Update();
		}
		//================================================================================
		//	
		//================================================================================
		public override void EnterScene()
		{
			base.EnterScene();
			//	ボタン割り当て
			m_draw_select_button_1.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_1).szSelectButton), 1);
			m_draw_select_button_1.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_1).szSelectButtonOver), 2);
			m_draw_select_button_1.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_1).szSelectButtonOver), 3);
			m_draw_select_button_2.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_2).szSelectButton), 1);
			m_draw_select_button_2.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_2).szSelectButtonOver), 2);
			m_draw_select_button_2.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_2).szSelectButtonOver), 3);
			m_draw_select_button_3.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_3).szSelectButton), 1);
			m_draw_select_button_3.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_3).szSelectButtonOver), 2);
			m_draw_select_button_3.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_3).szSelectButtonOver), 3);
			m_draw_select_button_4.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_4).szSelectButton), 1);
			m_draw_select_button_4.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_4).szSelectButtonOver), 2);
			m_draw_select_button_4.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_4).szSelectButtonOver), 3);
			//	第一章
			//GamePersianNight.CheckEnableScenario(GameConstants.SCENARIO_1)


			//	ガベコレ
			GC.Collect();
		}
		//==================================================================================
		//	
		//==================================================================================
		private void OVER_1(object sender, InnerPostEventArgs e)
		{
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_1).szSelectBack), 1);
			m_draw_character_silhouette.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_1).szSelectSilhouette), 1);
			return;
		}
		private void OVER_2(object sender, InnerPostEventArgs e)
		{
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_2).szSelectBack), 1);
			m_draw_character_silhouette.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_2).szSelectSilhouette), 1);
			return;
		}
		private void OVER_3(object sender, InnerPostEventArgs e)
		{
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_3).szSelectBack), 1);
			m_draw_character_silhouette.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_3).szSelectSilhouette), 1);
			return;
		}
		private void OVER_4(object sender, InnerPostEventArgs e)
		{
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_4).szSelectBack), 1);
			m_draw_character_silhouette.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + MD_Scenarios.GetScenariosFromId(MD_GameConstants.SCENARIO_4).szSelectSilhouette), 1);
			return;
		}


	}	//	class end
}	//	namespace end
