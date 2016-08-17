using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using GameFramework;
using DxLibDLL;


namespace PersianNight
{
	//==================================================================================
	//	
	//==================================================================================
	public class BattleManager
	{
		//==================================================================================
		//	バトル用動的データ
		//==================================================================================
		/// <summary>攻撃側ダメージ値</summary>
		static private int s_nBattle_A_Damage;
		/// <summary>攻撃側ATK</summary>
		static private int s_nBattle_A_Atk;
		/// <summary>攻撃側武器D補正</summary>
		static private int s_nBattle_A_WeaponDamage;
		/// <summary>攻撃側武器ATK補正</summary>
		static private int s_nBattle_A_WeaponAtk;
		/// <summary>防御側DEF</summary>
		static private int s_nBattle_D_Def;
		/// <summary>防御側防具DEF補正</summary>
		static private int s_nBattle_D_ArmorDef;
		/// <summary>アクションID</summary>
		static private int s_nActionId;

	}
}
