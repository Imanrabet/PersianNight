using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using GameFramework;
using DxLibDLL;

namespace PersianNight
{
	//================================================================================
	//	
	//================================================================================
	public class Scene_OASIS : Scene
	{
		//================================================================================
		//	
		//================================================================================
		public override void Initialize( )
		{
			//	基底初期化
			base.Initialize( );
			//	描画設定
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "砂漠590.jpg"), 1);
			m_fDrawPos_X = 0;
			m_fDrawPos_Y = 0;
			m_nTransFlag = 0;
			m_bMouseAct = false;                    //	マウス動作
			m_bMouseButtonLeftClickEnable = false;  //	左クリック動作
			m_bDefaultMouseLeftClickEvent = false;  //	左クリック動作
			m_bDragEnable = false;                  //	ドラッグ処理
			m_nDragTopLeftX = 0;                    //	ドラッグ範囲
			m_nDragTopLeftY = 0;                    //	ドラッグ範囲
			m_nDragBottomRightX = 0;                //	ドラッグ範囲
			m_nDragBottomRightY = 0;                //	ドラッグ範囲
		}

	}
}
