using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using GameFramework;
using DxLibDLL;

namespace PersianNight
{
	public class TextCommand
	{
		public string szCOMMAND = "";
		public object obj = null;
	}
	public enum EnumScriptEvent
	{
		LOAD_GRAPH_BACK,
		LOAD_LAYER_CENTER,
		LOAD_LAYER_RIGHT,
		LOAD_LAYER_LEFT,
		LOAD_LAYER_CENTER_CLEAR,
		LOAD_LAYER_RIGHT_CLEAR,
		LOAD_LAYER_LEFT_CLEAR,
		LOAD_LAYER_2,
		CHARACTER,
		FADE_IN,
		FADE_OUT,
		CLEAR_LAYER_1,
		CLEAR_LAYER_2,
		PLAY_BGM,
		STOP_BGM,
		FORCE_EXIT,
		LOAD_FACE
	}
	public enum EnumADVStatus
	{
		/// <summary>未定義</summary>
		NULL,
		/// <summary>オープニング</summary>
		OP,
		/// <summary>エンディング</summary>
		ED,
		/// <summary>その他イベント</summary>
		OTHER_EVENT
	}
	//================================================================================
	//	
	//================================================================================
	public class Scene_ADV : Scene
	{
		private Wnd_AdventureText m_Wnd_AdventureText;
		private Drawable m_draw_Layer_CENTER;
		private Drawable m_draw_Layer_RIGHT;
		private Drawable m_draw_Layer_LEFT;
		private int m_nHandle_Layer_1;
		/// <summary>イベント終了後の遷移先</summary>
		public Scene m_NextScene;
		/// <summary>実行スクリプトファイル名</summary>
		public string m_szScriptFileName = "";
		/// <summary>どのパートのアドベンチャーシーンか</summary>
		public EnumADVStatus m_EnumADVPart = EnumADVStatus.NULL;

		private bool m_bFADE_IN = false;
		private bool m_bFADE_OUT = false;
		private int m_nFadeCount = 0;

		private List<TextCommand> m_list_command_stack = new List<TextCommand>();
		/// <summary>コマンド実行中かどうか</summary>
		private bool m_bEXECCOOMAD = false;
		private int m_nHANDLE_BGM = -1;
		//================================================================================
		//	
		//================================================================================
		public override void Initialize()
		{
			//	基底初期化
			base.Initialize();
			//	描画設定
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "SCENE_ADV.png"), 1);
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
			m_InnerPostEventArgs = new GameFramework.InnerPostEventArgs(EnumGUIEvent.EVENT_EXIT);
			//
			m_draw_Layer_CENTER = new Drawable();
			AddChildDrawable(m_draw_Layer_CENTER, this);
			//
			m_draw_Layer_RIGHT = new Drawable();
			m_draw_Layer_RIGHT.fDrawPos_X += 250;
			AddChildDrawable(m_draw_Layer_RIGHT, this);
			//
			m_draw_Layer_LEFT = new Drawable();
			m_draw_Layer_LEFT.fDrawPos_X -= 250;
			AddChildDrawable(m_draw_Layer_LEFT, this);
			//
			m_Wnd_AdventureText = new Wnd_AdventureText();
			m_Wnd_AdventureText.Initialize();
			AddChildDrawable(m_Wnd_AdventureText, this);
			//
			AddEventFunc(EnumScriptEvent.LOAD_GRAPH_BACK, (InnerPostEventHandler)LOAD_GRAPH_BACK);
			AddEventFunc(EnumScriptEvent.LOAD_LAYER_CENTER, (InnerPostEventHandler)LOAD_LAYER_CENTER);
			AddEventFunc(EnumScriptEvent.LOAD_LAYER_RIGHT, (InnerPostEventHandler)LOAD_LAYER_RIGHT);
			AddEventFunc(EnumScriptEvent.LOAD_LAYER_LEFT, (InnerPostEventHandler)LOAD_LAYER_LEFT);
			AddEventFunc(EnumScriptEvent.LOAD_LAYER_CENTER_CLEAR, (InnerPostEventHandler)LOAD_LAYER_CENTER_CLEAR);
			AddEventFunc(EnumScriptEvent.LOAD_LAYER_RIGHT_CLEAR, (InnerPostEventHandler)LOAD_LAYER_RIGHT_CLEAR);
			AddEventFunc(EnumScriptEvent.LOAD_LAYER_LEFT_CLEAR, (InnerPostEventHandler)LOAD_LAYER_LEFT_CLEAR);
			AddEventFunc(EnumScriptEvent.LOAD_LAYER_2, (InnerPostEventHandler)LOAD_LAYER_2);
			AddEventFunc(EnumGUIEvent.EVENT_EXIT, (InnerPostEventHandler)EVENT_EXIT);
			AddEventFunc(EnumScriptEvent.FADE_IN, (InnerPostEventHandler)FADE_IN);
			AddEventFunc(EnumScriptEvent.FADE_OUT, (InnerPostEventHandler)FADE_OUT);
			AddEventFunc(EnumScriptEvent.CLEAR_LAYER_1, (InnerPostEventHandler)CLEAR_LAYER_1);
			AddEventFunc(EnumScriptEvent.CLEAR_LAYER_2, (InnerPostEventHandler)CLEAR_LAYER_2);
			AddEventFunc(EnumScriptEvent.PLAY_BGM, (InnerPostEventHandler)PLAY_BGM);
			AddEventFunc(EnumScriptEvent.STOP_BGM, (InnerPostEventHandler)STOP_BGM);
			AddEventFunc(EnumScriptEvent.FORCE_EXIT, (InnerPostEventHandler)FORCE_EXIT);
			AddEventFunc(EnumScriptEvent.LOAD_FACE, (InnerPostEventHandler)LOAD_FACE);
			//
		}
		//==================================================================================
		//	
		//==================================================================================
		private void LOAD_GRAPH_BACK(object sender, InnerPostEventArgs e)
		{
			TextCommand tc = new TextCommand();
			tc.szCOMMAND = "LOAD_GRAPH_BACK";
			tc.obj = e.oGenericObject;
			m_list_command_stack.Add(tc);
		}
		private void ExecLOAD_GRAPH_BACK(object o)
		{
			//			string szFileName = ( string )e.oGenericObject;
			string szFileName = (string)o;
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "\\" + szFileName), 1);
			EndCommand();
		}
		//==================================================================================
		//	
		//==================================================================================
		private void LOAD_LAYER_CENTER(object sender, InnerPostEventArgs e)
		{
			string szFileName = (string)e.oGenericObject;
			//			m_draw_Layer_1.SetGraphicHandle(DX.LoadGraph(GameHighace.s_szGraphicPath + szFileName), 1);
			m_draw_Layer_CENTER.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "\\" + szFileName), 1);
		}
		//==================================================================================
		//	
		//==================================================================================
		private void LOAD_LAYER_RIGHT(object sender, InnerPostEventArgs e)
		{
			string szFileName = (string)e.oGenericObject;
			//			m_draw_Layer_1.SetGraphicHandle(DX.LoadGraph(GameHighace.s_szGraphicPath + szFileName), 1);
			m_draw_Layer_RIGHT.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "\\" + szFileName), 1);
		}
		//==================================================================================
		//	
		//==================================================================================
		private void LOAD_LAYER_LEFT(object sender, InnerPostEventArgs e)
		{
			string szFileName = (string)e.oGenericObject;
			//			m_draw_Layer_1.SetGraphicHandle(DX.LoadGraph(GameHighace.s_szGraphicPath + szFileName), 1);
			m_draw_Layer_LEFT.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "\\" + szFileName), 1);
		}
		//==================================================================================
		//	
		//==================================================================================
		private void LOAD_LAYER_CENTER_CLEAR(object sender, InnerPostEventArgs e)
		{
			m_draw_Layer_CENTER.SetGraphicHandle(0, 1);
		}
		//==================================================================================
		//	
		//==================================================================================
		private void LOAD_LAYER_RIGHT_CLEAR(object sender, InnerPostEventArgs e)
		{
			m_draw_Layer_RIGHT.SetGraphicHandle(0, 1);
		}
		//==================================================================================
		//	
		//==================================================================================
		private void LOAD_LAYER_LEFT_CLEAR(object sender, InnerPostEventArgs e)
		{
			m_draw_Layer_LEFT.SetGraphicHandle(0, 1);
		}

		//==================================================================================
		//	
		//==================================================================================
		private void LOAD_LAYER_2(object sender, InnerPostEventArgs e)
		{
			string szFileName = (string)e.oGenericObject;
			//			m_draw_Layer_1.SetGraphicHandle(DX.LoadGraph(GameHighace.s_szGraphicPath + szFileName), 1);
			m_draw_Layer_RIGHT.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "\\" + szFileName), 1);
		}
		//==================================================================================
		//	
		//==================================================================================
		private void LOAD_FACE(object sender, InnerPostEventArgs e)
		{
			string szFileName = (string)e.oGenericObject;
			m_Wnd_AdventureText.m_draw_Face.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "\\" + szFileName), 1);
		}
		//==================================================================================
		//	
		//==================================================================================
		private void CLEAR_LAYER_1(object sender, InnerPostEventArgs e)
		{
			m_draw_Layer_CENTER.SetGraphicHandle(-1, 1);
		}
		//==================================================================================
		//	
		//==================================================================================
		private void CLEAR_LAYER_2(object sender, InnerPostEventArgs e)
		{
			m_draw_Layer_RIGHT.SetGraphicHandle(-1, 1);
		}
		//==================================================================================
		//	
		//==================================================================================
		private void EVENT_EXIT(object sender, InnerPostEventArgs e)
		{
			e.oGenericObject = m_NextScene;
		}
		//==================================================================================
		//	
		//==================================================================================
		private void FADE_IN(object sender, InnerPostEventArgs e)
		{
			TextCommand tc = new TextCommand();
			tc.szCOMMAND = "FADE_IN";
			tc.obj = e.oGenericObject;
			m_list_command_stack.Add(tc);
		}
		private void ExecFADE_IN()
		{
			m_bFADE_IN = true;
			m_nFadeCount = 0;
			m_bEXECCOOMAD = true;
		}
		//==================================================================================
		//	
		//==================================================================================
		private void FADE_OUT(object sender, InnerPostEventArgs e)
		{
			TextCommand tc = new TextCommand();
			tc.szCOMMAND = "FADE_OUT";
			tc.obj = e.oGenericObject;
			m_list_command_stack.Add(tc);
		}
		private void ExecFADE_OUT()
		{
			m_bFADE_OUT = true;
			m_nFadeCount = 255;
			m_bEXECCOOMAD = true;
		}
		//==================================================================================
		//	
		//==================================================================================
		private void PLAY_BGM(object sender, InnerPostEventArgs e)
		{
			string szFileName = (string)e.oGenericObject;
			m_nHANDLE_BGM = DX.LoadSoundMem(GamePersianNight.s_szSoundPath + szFileName);
//			DX.ChangeVolumeSoundMem(40, m_nHANDLE_BGM);
			DX.PlaySoundMem(m_nHANDLE_BGM, DX.DX_PLAYTYPE_LOOP);
		}
		//==================================================================================
		//	
		//==================================================================================
		private void STOP_BGM(object sender, InnerPostEventArgs e)
		{
			DX.StopSoundMem(m_nHANDLE_BGM);
		}
		//==================================================================================
		//	
		//==================================================================================
		private void FORCE_EXIT(object sender, InnerPostEventArgs e)
		{
			m_InnerPostEventArgs = new GameFramework.InnerPostEventArgs(EnumGUIEvent.EVENT_EXIT);
			BridgeEvent();
		}
	
		//==================================================================================
		//	
		//==================================================================================
		public override void EnterScene()
		{
			base.EnterScene();
			//	ガベコレ
			GC.Collect();
			//	スクリプトを読み込む
			Script s = new Script();
			s.ReadTextFile(GamePersianNight.GetCurrentScriptFileName());
			//	読み込みに失敗していたら次の遷移を行う
			if (s.bReadCompleted != true)
			{
				m_InnerPostEventArgs.oGenericObject = m_NextScene;
				BridgeEvent();
				return;
			}
			else
			{
				m_Wnd_AdventureText.SetScript(s);
				m_Wnd_AdventureText.CheckNextLine();
			}
		}
		//==================================================================================
		//==================================================================================
		public override void ExitScene()
		{
			base.ExitScene();
			//	表示物を消す
			m_nHandle_Graphic_OFF = -1;
			m_draw_Layer_CENTER.SetGraphicHandle(-1, 1);
			m_draw_Layer_RIGHT.SetGraphicHandle(-1, 1);
			m_Wnd_AdventureText.m_draw_Face.SetGraphicHandle(-1, 1);
			//
			DX.StopSoundMem(m_nHANDLE_BGM);
		}
		//==================================================================================
		//==================================================================================
		public override void Update()
		{
			base.Update();
			//	CTRLキーが押されていたときのカウンタースキップ
			if (DX.CheckHitKey(DX.KEY_INPUT_RCONTROL) == 1 || DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) == 1)
			{
				if (m_bFADE_OUT == true) m_nFadeCount = 1;
				if (m_bFADE_IN == true) m_nFadeCount = 255;
			}
			//	コマンドスタックをチェック
			CheckStack();
			//	フェード処理
			if (m_bFADE_OUT == true && m_nFadeCount > 0)
			{
				DX.SetDrawBright(m_nFadeCount, m_nFadeCount, m_nFadeCount);
				m_nFadeCount -= 10;
				if (m_nFadeCount <= 0)
				{
					m_bFADE_OUT = false;
					EndCommand();
				}
			}
			else if (m_bFADE_IN == true && m_nFadeCount < 256)
			{
				DX.SetDrawBright(m_nFadeCount, m_nFadeCount, m_nFadeCount);
				m_nFadeCount += 10;
				if (m_nFadeCount >= 256)
				{
					m_bFADE_IN = false;
					EndCommand();
				}
			}

		}
		//==================================================================================
		/// <summary>コマンドスタックをチェック</summary>
		//==================================================================================
		private void CheckStack()
		{
			//	すでに何かのコマンドを実行中なら何もしない
			if (m_bEXECCOOMAD == true) return;
			//	残りのコマンドがあるかチェック
			if (m_list_command_stack.Count == 0) return;
			//	コマンドを取り出して実行状態に移行
			m_bEXECCOOMAD = true;
			if (m_list_command_stack[0].szCOMMAND == "FADE_IN") { ExecFADE_IN(); return; }
			if (m_list_command_stack[0].szCOMMAND == "FADE_OUT") { ExecFADE_OUT(); return; }
			if (m_list_command_stack[0].szCOMMAND == "LOAD_GRAPH_BACK") { ExecLOAD_GRAPH_BACK(m_list_command_stack[0].obj); return; }
		}
		//==================================================================================
		/// <summary>スタックコマンドを実行完了した後の処理</summary>
		//==================================================================================
		private void EndCommand()
		{
			//	コマンド実行状態を解除
			m_bEXECCOOMAD = false;
			//	実行していたコマンドを削除
			m_list_command_stack.RemoveAt(0);
		}
	}
}
