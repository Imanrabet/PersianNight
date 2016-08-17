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
	public class Wnd_OasisCommandPanel : Drawable
	{
		private List<Drawable> m_list_draw_SelectButtons;
		//================================================================================
		//	
		//================================================================================
		public override void Initialize( )
		{
			//	基底初期化
			base.Initialize( );
			//	描画設定
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "mess-s.png"), 1);
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "mess-s.png"), 2);
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "mess-s.png"), 3);
			m_fDrawPos_X = 0;
			m_fDrawPos_Y = 350;
			m_nTransFlag = 1;
			m_bMouseAct = false;                        //	マウス動作
			m_bMouseButtonLeftClickEnable = true;   //	左クリックを処理するか
			m_bDefaultMouseLeftClickEvent = true;   //	左クリックメッセージを親に投げるかどうか
			m_bMouseButtonRightClickEnable = true;  //	右クリックを処理するか
			m_InnerPostEventArgs = new GameFramework.InnerPostEventArgs(EnumGUIEvent.EVENT_EXIT);
			m_bDefaultMouseRightClickEvent = true;  //	右クリックメッセージを親に投げるかどうか
													//
			m_list_draw_SelectButtons = new List<Drawable>( );
		}

	}
}
