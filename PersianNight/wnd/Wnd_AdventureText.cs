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
	public class Wnd_AdventureText : Drawable
	{
		public Drawable m_draw_Face;
		static private Dictionary<string, EnumScriptEvent> s_dic_command;
		/// <summary>現在のセンテンス</summary>
		private string m_szCurrentSentence;
		/// <summary>センテンス</summary>
		private string[] m_szArraySentence;
		/// <summary>禁則リスト</summary>
		private List<string> m_list_kinsoku;
		public bool m_bIgnore = false;
		private Script m_Script;
		/// <summary>1行に書ける文字数</summary>
		private int m_nTextSize = 25;
		private string m_szName = "";
		/// <summary>メッセージウィンドウのY座標</summary>
		private int m_nBasePoint = 350;
		/// <summary>文字表示座標</summary>
		private int m_nBaseTextX = 240;
		private int m_nBaseTextY = 360;
		/// <summary>行サイズ</summary>
		private int m_nRowSpan = 18;
		private bool m_bRETURNKEYDOWN = false;
		/// <summary>表示行数</summary>
		private int m_nShowLine = 0;
		//================================================================================
		//	
		//================================================================================
		static Wnd_AdventureText()
		{
			s_dic_command = new Dictionary<string, EnumScriptEvent>();
			s_dic_command.Add("[LOAD_GRAPH_BACK]", EnumScriptEvent.LOAD_GRAPH_BACK);
			s_dic_command.Add("[背景]", EnumScriptEvent.LOAD_GRAPH_BACK);
			s_dic_command.Add("[LOAD_LAYER_1]", EnumScriptEvent.LOAD_LAYER_CENTER);
			s_dic_command.Add("[LOAD_LAYER_2]", EnumScriptEvent.LOAD_LAYER_2);
//			s_dic_command.Add("[CHARACTER]", EnumScriptEvent.CHARACTER);
			s_dic_command.Add("[FADE_IN]", EnumScriptEvent.FADE_IN);
			s_dic_command.Add("[フェードイン]", EnumScriptEvent.FADE_IN);
			s_dic_command.Add("[FADE_OUT]", EnumScriptEvent.FADE_OUT);
			s_dic_command.Add("[フェードアウト]", EnumScriptEvent.FADE_OUT);
			s_dic_command.Add("[PLAY_BGM]", EnumScriptEvent.PLAY_BGM);
			s_dic_command.Add("[BGM]", EnumScriptEvent.PLAY_BGM);
			s_dic_command.Add("[STOP_BGM]", EnumScriptEvent.STOP_BGM);
			s_dic_command.Add("[BGM停止]", EnumScriptEvent.STOP_BGM);
			s_dic_command.Add("[強制終了]", EnumScriptEvent.FORCE_EXIT);
			s_dic_command.Add("[中央]", EnumScriptEvent.LOAD_LAYER_CENTER);
			s_dic_command.Add("[右端]", EnumScriptEvent.LOAD_LAYER_RIGHT);
			s_dic_command.Add("[左端]", EnumScriptEvent.LOAD_LAYER_LEFT);
			s_dic_command.Add("[中央消去]", EnumScriptEvent.LOAD_LAYER_CENTER_CLEAR);
			s_dic_command.Add("[右端消去]", EnumScriptEvent.LOAD_LAYER_RIGHT_CLEAR);
			s_dic_command.Add("[左端消去]", EnumScriptEvent.LOAD_LAYER_LEFT_CLEAR);
			s_dic_command.Add("[顔]", EnumScriptEvent.LOAD_FACE);
		}
		//================================================================================
		//	
		//================================================================================
		public override void Initialize()
		{
			//	基底初期化
			base.Initialize();
			//	描画設定
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "mess-l.png"), 1);
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "mess-l.png"), 2);
			SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "mess-l.png"), 3);
			m_fDrawPos_X = 0;
			m_fDrawPos_Y = m_nBasePoint;
			m_nTransFlag = 1;
			m_bMouseAct = true;						//	マウス動作
			m_bMouseButtonLeftClickEnable = true;	//	左クリックを処理するか
			m_bDefaultMouseLeftClickEvent = false;	//	左クリックメッセージを親に投げるかどうか
			m_bMouseButtonRightClickEnable = true;	//	右クリックを処理するか
			m_InnerPostEventArgs = new GameFramework.InnerPostEventArgs(EnumGUIEvent.EVENT_EXIT);
			m_bDefaultMouseRightClickEvent = true;	//	右クリックメッセージを親に投げるかどうか
			m_szArraySentence = new string[] { "", "", "", "", "", "" };
			m_list_kinsoku = new List<string>();
			m_list_kinsoku.Add("。");
			m_list_kinsoku.Add("」");
			m_list_kinsoku.Add("、");
			m_list_kinsoku.Add("ぁ");
			m_list_kinsoku.Add("ぃ");
			m_list_kinsoku.Add("ぅ");
			m_list_kinsoku.Add("ぇ");
			m_list_kinsoku.Add("ぉ");
			m_list_kinsoku.Add("ァ");
			m_list_kinsoku.Add("ィ");
			m_list_kinsoku.Add("ゥ");
			m_list_kinsoku.Add("ェ");
			m_list_kinsoku.Add("ォ");
			m_list_kinsoku.Add("ゃ");
			m_list_kinsoku.Add("ゅ");
			m_list_kinsoku.Add("ょ");
			m_list_kinsoku.Add("ャ");
			m_list_kinsoku.Add("ュ");
			m_list_kinsoku.Add("ョ");
			m_list_kinsoku.Add("！");
			m_list_kinsoku.Add("？");
			m_list_kinsoku.Add("っ");
			m_list_kinsoku.Add("ー");
			m_list_kinsoku.Add("～");
			//
			m_draw_Face = new Drawable();
			m_draw_Face.nTransFlag = 1;
			m_draw_Face.fDrawPos_X = 180;
			m_draw_Face.fDrawPos_Y = 10;
			AddChildDrawable(m_draw_Face, this);
			//

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
				for (int i = 0; i < 6; i++)
				{
					/*
					DX.DrawStringToHandle(m_nBaseTextX - 1, m_nBaseTextY - 1 + m_nRowSpan * i, m_szArraySentence[i], COLOR_BLACK, fh);
					DX.DrawStringToHandle(m_nBaseTextX + 1, m_nBaseTextY - 1 + m_nRowSpan * i, m_szArraySentence[i], COLOR_BLACK, fh);
					DX.DrawStringToHandle(m_nBaseTextX - 1, m_nBaseTextY + 0 + m_nRowSpan * i, m_szArraySentence[i], COLOR_BLACK, fh);
					DX.DrawStringToHandle(m_nBaseTextX + 1, m_nBaseTextY + 0 + m_nRowSpan * i, m_szArraySentence[i], COLOR_BLACK, fh);
					DX.DrawStringToHandle(m_nBaseTextX - 1, m_nBaseTextY + 1 + m_nRowSpan * i, m_szArraySentence[i], COLOR_BLACK, fh);
					DX.DrawStringToHandle(m_nBaseTextX + 1, m_nBaseTextY + 1 + m_nRowSpan * i, m_szArraySentence[i], COLOR_BLACK, fh);
					DX.DrawStringToHandle(m_nBaseTextX + 0, m_nBaseTextY + 0 + m_nRowSpan * i, m_szArraySentence[i], COLOR_WHITE, fh);
					//*/
					DX.DrawStringToHandle(m_nBaseTextX + 0, m_nBaseTextY + 0 + m_nRowSpan * i, m_szArraySentence[i], COLOR_BLACK, fh,COLOR_WHITE);
				}
			}
		}
		//================================================================================
		//	
		//================================================================================
		protected override void MouseLeftClick()
		{
			//	無視フラグが立っているときはクリック無視
			if (m_bIgnore == true) return;
			GamePersianNight.DebugLog("ウィンドウ左クリック\n");
			if (CheckNextLine() == false)
			{
				m_InnerPostEventArgs = new GameFramework.InnerPostEventArgs(EnumGUIEvent.EVENT_EXIT);
				BridgeEvent();
			}
			return;
		}
		//================================================================================
		//	
		//================================================================================
		public override void Update()
		{
			base.Update();
			KeyCheck( );
			return;
		}
		//================================================================================
		/// <summary>キー処理。キー押下に応じてパース処理を実行。キー入力がなければfalse</summary>
		//================================================================================
		private bool KeyCheck()
		{
			if (DX.CheckHitKey(DX.KEY_INPUT_RETURN) == 1)
			{
				if (m_bRETURNKEYDOWN == false)
				{
					m_bRETURNKEYDOWN = true;
					//	ENTERキー押下上で次の行をパース
					if (CheckNextLine( ) == false)
					{
						//	行末或いはパース失敗ならADVシーンを抜ける
						m_InnerPostEventArgs = new GameFramework.InnerPostEventArgs(EnumGUIEvent.EVENT_EXIT);
						BridgeEvent( );
					}
					return true;
				}
			}
			else
			{
				m_bRETURNKEYDOWN = false;
			}
			//
			if (DX.CheckHitKey(DX.KEY_INPUT_RCONTROL) == 1 || DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) == 1)
			{
				//	CTRLキーで次の行をパース（ENTERと異なり押しているだけで有効）
				if (CheckNextLine( ) == false)
				{
					//	行末或いはパース失敗ならADVシーンを抜ける
					m_InnerPostEventArgs = new GameFramework.InnerPostEventArgs(EnumGUIEvent.EVENT_EXIT);
					BridgeEvent( );
				}
				return true;
			}
			return false;
		}
		//================================================================================
		//	
		//================================================================================
		public void SetScript(Script script)
		{
			m_Script = script;
		}
		//================================================================================
		//	
		//================================================================================
		public bool CheckNextLine()
		{
			//	次のテキストを取得してみる
			if (m_Script == null) return false;
			string szNextLine = "";
			while (true)
			{
				szNextLine = m_Script.GetNextText();
				if (szNextLine.Length >= 2)
				{
					if (!(szNextLine[0] == '/' && szNextLine[1] == '/')) break;
				}
				else
				{
					break;
				}
			};
			GamePersianNight.DebugLog(szNextLine + "\n");
			//	コマンド行かどうか
			string szCOMMAND, szARG;
			if (ChekCommand(szNextLine, out szCOMMAND, out szARG) == true)
			{
				m_InnerPostEventArgs = new GameFramework.InnerPostEventArgs(s_dic_command[szCOMMAND]);
				m_InnerPostEventArgs.oGenericObject = szARG;
				BridgeEvent();
				//	フェードアウトの時は次のコマンドを読み込まない
				if (szCOMMAND == "[フェードアウト]") return true;
				CheckNextLine();
				return true;
			}
			if (szNextLine == "") return false;
			//	ここからペルシアンナイト用 -------------------------------
			if (m_nShowLine >= 6)
			{
				m_nShowLine = 0;
				return true;
			}
			else
			{
				ParseTextLine2(szNextLine);
				if (m_nShowLine >= 6)
				{
					m_nShowLine = 0;
					return true;
				}
				CheckNextLine();
			}
			// ------------------------------------------------------------
//			AnalyzeTextLine(szNextLine);
			return true;
		}
		//================================================================================
		/// <summary>コマンド行ならばコマンドと引数を設定</summary>
		//================================================================================
		private bool ChekCommand(string szLine, out string szCOMMAND, out string szARG)
		{
			foreach (string s in s_dic_command.Keys)
			{
				if (szLine.IndexOf(s, StringComparison.OrdinalIgnoreCase) == 0)
				{
					szCOMMAND = s;
					szARG = szLine.Replace(s, "");
					return true;
				}
			}
			szCOMMAND = "";
			szARG = "";
			return false;
		}
		//================================================================================
		/// <summary></summary>
		//================================================================================
		private void ParseTextLine2(string szText)
		{
			for (int i = m_nShowLine; i < 6; i++) { m_szArraySentence[i] = ""; }
			m_szArraySentence[m_nShowLine] = szText.Replace("\\p", "");
			m_nShowLine++;
			if (szText.Contains("\\p") == true) m_nShowLine = 100;
		}
		//================================================================================
		/// <summary></summary>
		//================================================================================
		private void ParseTextLine(string szText)
		{
			m_szArraySentence[0] = "";
			m_szArraySentence[1] = "";
			m_szArraySentence[2] = "";
			m_szArraySentence[3] = "";
			int nCount = 0;
			int nLineIndex = 0;
			foreach (char c in szText)
			{
				if (c == '\\')
				{
					nCount = 0;
					nLineIndex++;
					continue;
				}
				m_szArraySentence[nLineIndex] += c;
				nCount++;
				if (nCount > m_nTextSize)
				{
					nCount = 0;
					nLineIndex++;
					if (nLineIndex == 4) break;
				}
			}
		}

	}
}
