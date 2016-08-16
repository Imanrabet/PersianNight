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
	public class Scene_TITLE : Scene
	{
		private Drawable m_draw_TITLE_2;
		private Drawable m_draw_START;
		private Drawable m_draw_MEMORY;
		private Drawable m_draw_CGMODE;
		private Drawable m_draw_EXIT;
		//================================================================================
		//	
		//================================================================================
		public override void Initialize()
		{
			//	基底初期化
			base.Initialize();
			//	描画設定
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "砂漠-夜.png"), 1);
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

			//	タイトルロゴ
			m_draw_TITLE_2 = new Drawable( );
			m_draw_TITLE_2.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "title.png"), 1);
			m_draw_TITLE_2.nTransFlag = 1;
			m_draw_TITLE_2.fDrawPos_X = Drawable.CenterFromWidth( this, m_draw_TITLE_2 );
			m_draw_TITLE_2.fDrawPos_Y = 0;
			m_draw_TITLE_2.bMouseAct = false;					//	マウス動作
			AddChildDrawable( m_draw_TITLE_2, this );

			//	スタートボタン
			m_draw_START = new Drawable();
			m_draw_START.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "title-start.png"), 1);
			m_draw_START.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "title-start-c.png"), 2);
			m_draw_START.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "title-start-c.png"), 3);
			m_draw_START.nTransFlag = 1;
			m_draw_START.fDrawPos_X = Drawable.CenterFromWidth(this, m_draw_START);
			m_draw_START.fDrawPos_Y = 250;
			m_draw_START.bMouseAct = true;					//	マウス動作
			m_draw_START.bMouseButtonLeftClickEnable = true;	//	左クリック動作
			m_draw_START.bDefaultMouseLeftClickEvent = true;	//	左クリック動作
			m_draw_START.InnerPostEventArgs = new InnerPostEventArgs(EnumGUIEvent.CLICK_START);
			AddChildDrawable(m_draw_START, this);

			//	続きから
			m_draw_MEMORY = new Drawable( );
			m_draw_MEMORY.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "title-load.png"), 1);
			m_draw_MEMORY.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "title-load-c.png"), 2);
			m_draw_MEMORY.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "title-load-c.png"), 3);
			m_draw_MEMORY.nTransFlag = 1;
			m_draw_MEMORY.fDrawPos_X = Drawable.CenterFromWidth( this, m_draw_MEMORY );
			m_draw_MEMORY.fDrawPos_Y = 290;
			m_draw_MEMORY.bMouseAct = true;					//	マウス動作
			m_draw_MEMORY.bMouseButtonLeftClickEnable = true;	//	左クリック動作
			m_draw_MEMORY.bDefaultMouseLeftClickEvent = true;	//	左クリック動作
			m_draw_MEMORY.InnerPostEventArgs = new InnerPostEventArgs( EnumGUIEvent.CLICK_LOAD );
			AddChildDrawable( m_draw_MEMORY, this );

			//	おもいで
			m_draw_CGMODE = new Drawable( );
			m_draw_CGMODE.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "title-option.png"), 1);
			m_draw_CGMODE.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "title-option-c.png"), 2);
			m_draw_CGMODE.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "title-option-c.png"), 3);
			m_draw_CGMODE.nTransFlag = 1;
			m_draw_CGMODE.fDrawPos_X = Drawable.CenterFromWidth( this, m_draw_CGMODE );
			m_draw_CGMODE.fDrawPos_Y = 330;
			m_draw_CGMODE.bMouseAct = false;					//	マウス動作
			m_draw_CGMODE.bMouseButtonLeftClickEnable = true;	//	左クリック動作
			m_draw_CGMODE.bDefaultMouseLeftClickEvent = true;	//	左クリック動作
			m_draw_CGMODE.InnerPostEventArgs = new InnerPostEventArgs( EnumGUIEvent.CLICK_CGMODE );
			AddChildDrawable( m_draw_CGMODE, this );

			//	おわり
			m_draw_EXIT = new Drawable();
			m_draw_EXIT.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "title-exit.png"), 1);
			m_draw_EXIT.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "title-exit-c.png"), 2);
			m_draw_EXIT.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "title-exit-c.png"), 3);
			m_draw_EXIT.nTransFlag = 1;
			m_draw_EXIT.fDrawPos_X = Drawable.CenterFromWidth(this, m_draw_EXIT);
			m_draw_EXIT.fDrawPos_Y = 370;
			m_draw_EXIT.bMouseAct = false;					//	マウス動作
			m_draw_EXIT.bMouseButtonLeftClickEnable = true;	//	左クリック動作
			m_draw_EXIT.bDefaultMouseLeftClickEvent = true;	//	左クリック動作
			m_draw_EXIT.InnerPostEventArgs = new InnerPostEventArgs(EnumGUIEvent.CLICK_CGMODE);
			AddChildDrawable(m_draw_EXIT, this);
		}
		//================================================================================
		//	
		//================================================================================
		public override void Update( )
		{
			base.Update( );
			/*
			if( DX.CheckHitKey( DX.KEY_INPUT_RSHIFT ) == 1 && DX.CheckHitKey( DX.KEY_INPUT_RETURN ) == 1 )
			{
				if( m_bSWITCHSCREENMODE == false )
				{
					m_bSWITCHSCREENMODE = true;
					if( m_EnumScreenMode == EnumScreenMode.WINDOW )
					{
						if( DX.SetGraphMode( 1024, 768, 32 ) == DX.DX_CHANGESCREEN_OK )
						{
							m_EnumScreenMode = EnumScreenMode.FULLSCREEN;
						}
					}
					else if( m_EnumScreenMode == EnumScreenMode.FULLSCREEN )
					{
						if( DX.SetGraphMode( 1024, 768, 32 ) == DX.DX_CHANGESCREEN_OK )
						{
							m_EnumScreenMode = EnumScreenMode.WINDOW;
						}
					}
				}
			}
			else
			{
				m_bSWITCHSCREENMODE = false;
			}
			//*/
		}
		public override void EnterScene( )
		{
			base.EnterScene( );
			//	ガベコレ
			GC.Collect( );
//			DX.PlaySoundMem(GamePersianNight.s_nHANDLE_BGM_01_mp3, DX.DX_PLAYTYPE_LOOP);
		}
	}
}
