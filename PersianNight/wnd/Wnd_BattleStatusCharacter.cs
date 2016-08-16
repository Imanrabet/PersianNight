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
	public class Wnd_BattleStatusCharacter : Drawable
	{
		public int nDDCharaIndex = -1;
		public int nCurrentLevel = 1;
		public int nCurrentExp = 0;
		public int nNextExp = 0;
		public int nCurrentHp = 0;
		public int nMaxHp = 0;
		public int nCurrentTp = 0;
		public int nMaxTp = 0;
		public float fCurrentATB = 0;
		public float fMaxATB = 0;
		public int nATK = 0;
		public int nDEF = 0;
		public int nMATK = 0;
		public int nMDEF = 0;
		public string[] szEquipment;
		private Drawable m_draw_WeaponLeft;
		private Drawable m_draw_WeaponRight;
		private Drawable m_draw_ArmorLeft;
		private Drawable m_draw_ArmorRight;
		/// <summary>表示順位からY座標を引くリスト</summary>
		private List<int> INDEX_TO_Y;
		private int m_nATB_X;
		private int m_nATB_Y;
		private int m_nATB_WIDTH;
		private int m_nATB_Length;
		private int m_nATB_COLOR;
		private int m_nHpColor = COLOR_BLACK;
		//================================================================================
		//	
		//================================================================================
		public override void Initialize()
		{
			//	基底初期化
			base.Initialize();
			//
			INDEX_TO_Y = new List<int>();
			for (int i = 0; i < 15; i++) { INDEX_TO_Y.Add(i * 16); }
			//	描画設定
			SetGraphicHandle(-1, 1);
			m_fDrawPos_X = 0;
			m_fDrawPos_Y = 0;
			m_nTransFlag = 1;
			m_bMouseAct = false;						//	マウス動作
			m_bMouseButtonLeftClickEnable = true;	//	左クリックを処理するか
			m_bDefaultMouseLeftClickEvent = false;	//	左クリックメッセージを親に投げるかどうか
			m_bMouseButtonRightClickEnable = true;	//	右クリックを処理するか
			m_InnerPostEventArgs = new GameFramework.InnerPostEventArgs(EnumGUIEvent.EVENT_EXIT);
			m_bDefaultMouseRightClickEvent = true;	//	右クリックメッセージを親に投げるかどうか
			szEquipment = new string[4] { "－－－－－", "－－－－－", "－－－－－", "－－－－－" };
			//	武器ボタン左
			m_draw_WeaponLeft = new Drawable();
			m_draw_WeaponLeft.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "equip_left.png"), 1);
			m_draw_WeaponLeft.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "equip_left_on.png"), 2);
			m_draw_WeaponLeft.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "equip_left_on.png"), 3);
			m_draw_WeaponLeft.nTransFlag = 1;
			m_draw_WeaponLeft.fDrawPos_X = -4;
			m_draw_WeaponLeft.fDrawPos_Y = INDEX_TO_Y[9]-1;
			m_draw_WeaponLeft.bMouseAct = true;					//	マウス動作
			m_draw_WeaponLeft.bMouseButtonLeftClickEnable = true;	//	左クリックを処理するか
			m_draw_WeaponLeft.bDefaultMouseLeftClickEvent = true;	//	左クリックメッセージを親に投げるかどうか
			m_draw_WeaponLeft.InnerPostEventArgsLeftClick = new InnerPostEventArgs(EnumGUIEvent.EQUIP_CHANGE);
			AddChildDrawable(m_draw_WeaponLeft, this);
			//	武器ボタン右
			m_draw_WeaponRight = new Drawable();
			m_draw_WeaponRight.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "equip_right.png"), 1);
			m_draw_WeaponRight.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "equip_right_on.png"), 2);
			m_draw_WeaponRight.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "equip_right_on.png"), 3);
			m_draw_WeaponRight.nTransFlag = 1;
			m_draw_WeaponRight.fDrawPos_X = 158;
			m_draw_WeaponRight.fDrawPos_Y = INDEX_TO_Y[9]-1;
			m_draw_WeaponRight.bMouseAct = true;					//	マウス動作
			m_draw_WeaponRight.bMouseButtonLeftClickEnable = true;	//	左クリックを処理するか
			m_draw_WeaponRight.bDefaultMouseLeftClickEvent = true;	//	左クリックメッセージを親に投げるかどうか
			m_draw_WeaponRight.InnerPostEventArgsLeftClick = new InnerPostEventArgs(EnumGUIEvent.EQUIP_CHANGE);
			AddChildDrawable(m_draw_WeaponRight, this);
			//	防具ボタン左
			m_draw_ArmorLeft = new Drawable();
			m_draw_ArmorLeft.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "equip_left.png"), 1);
			m_draw_ArmorLeft.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "equip_left_on.png"), 2);
			m_draw_ArmorLeft.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "equip_left_on.png"), 3);
			m_draw_ArmorLeft.nTransFlag = 1;
			m_draw_ArmorLeft.fDrawPos_X = -4;
			m_draw_ArmorLeft.fDrawPos_Y = INDEX_TO_Y[10]-1;
			m_draw_ArmorLeft.bMouseAct = true;					//	マウス動作
			m_draw_ArmorLeft.bMouseButtonLeftClickEnable = true;	//	左クリックを処理するか
			m_draw_ArmorLeft.bDefaultMouseLeftClickEvent = true;	//	左クリックメッセージを親に投げるかどうか
			m_draw_ArmorLeft.InnerPostEventArgsLeftClick = new InnerPostEventArgs(EnumGUIEvent.EQUIP_CHANGE);
			AddChildDrawable(m_draw_ArmorLeft, this);
			//	防具ボタン右
			m_draw_ArmorRight = new Drawable();
			m_draw_ArmorRight.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "equip_right.png"), 1);
			m_draw_ArmorRight.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "equip_right_on.png"), 2);
			m_draw_ArmorRight.SetGraphicHandle(DX.LoadGraph(GamePersianNight.s_szGraphicPath + "equip_right_on.png"), 3);
			m_draw_ArmorRight.nTransFlag = 1;
			m_draw_ArmorRight.fDrawPos_X = 158;
			m_draw_ArmorRight.fDrawPos_Y = INDEX_TO_Y[10]-1;
			m_draw_ArmorRight.bMouseAct = true;					//	マウス動作
			m_draw_ArmorRight.bMouseButtonLeftClickEnable = true;	//	左クリックを処理するか
			m_draw_ArmorRight.bDefaultMouseLeftClickEvent = true;	//	左クリックメッセージを親に投げるかどうか
			m_draw_ArmorRight.InnerPostEventArgsLeftClick = new InnerPostEventArgs(EnumGUIEvent.EQUIP_CHANGE);
			AddChildDrawable(m_draw_ArmorRight, this);
		}
		//================================================================================
		/// <summary>DDキャラIndexを設定する。RPGパートに入る際に一回呼べばよい</summary>
		/// <param name="nDDCharacterIndex"></param>
		//================================================================================
		public void SetDDCharacterIndex(int nDDCharacterIndex)
		{
			m_draw_WeaponLeft.InnerPostEventArgsLeftClick.oGenericObject = new int[] { nDDCharacterIndex, 1, -1 };
			m_draw_WeaponRight.InnerPostEventArgsLeftClick.oGenericObject = new int[] { nDDCharacterIndex, 1, 1 };
			m_draw_ArmorLeft.InnerPostEventArgsLeftClick.oGenericObject = new int[] { nDDCharacterIndex, 2, -1 };
			m_draw_ArmorRight.InnerPostEventArgsLeftClick.oGenericObject = new int[] { nDDCharacterIndex, 2, 1 };
		}
		//================================================================================
		//	
		//================================================================================
		public override void Draw()
		{
			base.Draw();
			int fh = GamePersianNight.s_nFONT_HANDLE_16;
			if (nCurrentHp <= 0) { nCurrentHp = 0; m_nHpColor = COLOR_RED; } else { m_nHpColor = COLOR_BLACK; }
			DX.DrawStringToHandle(this.m_nDrawPosScreen_X + 45, this.m_nDrawPosScreen_Y + INDEX_TO_Y[00], "LEVEL " + nCurrentLevel.ToString(), COLOR_BLACK, fh);
			DX.DrawStringToHandle(this.m_nDrawPosScreen_X + 45, this.m_nDrawPosScreen_Y + INDEX_TO_Y[01], "EXP:" + nCurrentExp.ToString() + " / " + nNextExp.ToString(), COLOR_BLACK, fh);
			DX.DrawStringToHandle(this.m_nDrawPosScreen_X + 45, this.m_nDrawPosScreen_Y + INDEX_TO_Y[02], "HP :" + nCurrentHp.ToString() + " / " + nMaxHp.ToString(), m_nHpColor, fh);
			DX.DrawStringToHandle(this.m_nDrawPosScreen_X + 45, this.m_nDrawPosScreen_Y + INDEX_TO_Y[03], "TP :" + nCurrentTp.ToString() + " / " + nMaxTp.ToString(), COLOR_BLACK, fh);
			DX.DrawStringToHandle(this.m_nDrawPosScreen_X + 45, this.m_nDrawPosScreen_Y + INDEX_TO_Y[04], "ATB", COLOR_BLACK, fh);
			DX.DrawStringToHandle(this.m_nDrawPosScreen_X + 45, this.m_nDrawPosScreen_Y + INDEX_TO_Y[05], "攻撃力:" + nATK.ToString(), COLOR_BLACK, fh);
			DX.DrawStringToHandle(this.m_nDrawPosScreen_X + 45, this.m_nDrawPosScreen_Y + INDEX_TO_Y[06], "防御力:" + nDEF.ToString(), COLOR_BLACK, fh);
			DX.DrawStringToHandle(this.m_nDrawPosScreen_X + 45, this.m_nDrawPosScreen_Y + INDEX_TO_Y[07], "魔法攻撃力:" + nMATK.ToString(), COLOR_BLACK, fh);
			DX.DrawStringToHandle(this.m_nDrawPosScreen_X + 45, this.m_nDrawPosScreen_Y + INDEX_TO_Y[08], "魔法防御力:" + nMDEF.ToString(), COLOR_BLACK, fh);
			DX.DrawStringToHandle(this.m_nDrawPosScreen_X + 14, this.m_nDrawPosScreen_Y + INDEX_TO_Y[09], "武：" + szEquipment[0], COLOR_BLACK, fh);
			DX.DrawStringToHandle(this.m_nDrawPosScreen_X + 14, this.m_nDrawPosScreen_Y + INDEX_TO_Y[10], "防：" + szEquipment[1], COLOR_BLACK, fh);
			DX.DrawStringToHandle(this.m_nDrawPosScreen_X + 14, this.m_nDrawPosScreen_Y + INDEX_TO_Y[11], "技：" + szEquipment[2], COLOR_BLACK, fh);
			DX.DrawStringToHandle(this.m_nDrawPosScreen_X + 14, this.m_nDrawPosScreen_Y + INDEX_TO_Y[12], "技：" + szEquipment[3], COLOR_BLACK, fh);
			//	ATBバー描画
			m_nATB_X = m_nDrawPosScreen_X + 45 + 32;
			m_nATB_Y = m_nDrawPosScreen_Y + INDEX_TO_Y[04] + 4;
			m_nATB_WIDTH = 5;
			int m_nATB_Length = (int)(fCurrentATB / fMaxATB * 100);
			if (m_nATB_Length >= 100) { m_nATB_Length = 100; m_nATB_COLOR = COLOR_RED; } else { m_nATB_COLOR = COLOR_CYAN; }
			DX.DrawBox(m_nATB_X + 0, m_nATB_Y + 0, m_nATB_X + m_nATB_Length + 0, m_nATB_Y + 0 + m_nATB_WIDTH, m_nATB_COLOR, 1);
			DX.DrawBox(m_nATB_X - 1, m_nATB_Y - 1, m_nATB_X + 0000000000100 + 1, m_nATB_Y + 1 + m_nATB_WIDTH,  COLOR_WHITE, 0);
		}
		//================================================================================
		//	
		//================================================================================
		public override void Update()
		{
			base.Update();
		}

	}
}
