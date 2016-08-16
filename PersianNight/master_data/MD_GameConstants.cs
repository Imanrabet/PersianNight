using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianNight
{
	//==================================================================================
	//	game_constants
	//==================================================================================
	static public class MD_GameConstants
	{
		static private CSV s_csv_masterdata;
		static public int BOSS_DISTANCE = -1;
		static public int MAX_LEVEL = -1;
		static public int INIT_MONEY = -1;
		static public int MAX_ACT = -1;
		static public int DIN_SELECT_TIME = -1;
		static public string MONEY_NAME = "";
		static public int CHASER_DISTANCE = -1;
		static public int OASSIS_CHASER_MIN = -1;
		static public int OASSIS_CHASER_MAX = -1;
		static public int SCENARIO_1 = -1;
		static public int SCENARIO_2 = -1;
		static public int SCENARIO_3 = -1;
		static public int SCENARIO_4 = -1;
		static public int SCENARIO_5 = -1;
		static public int ACT_INC = -1;
		static public float MAX_DAMAGE_x = 3;
		static public int TP_ATB_INC = 0;
		static public int TP_ATTACK_INC = 0;
		static public int OASSIS_DISTANCE = 0;
		//==================================================================================
		//	
		//==================================================================================
		static public bool Initialize()
		{
			s_csv_masterdata = new CSV();
			if (s_csv_masterdata.ImportCSV(GamePersianNight.s_szCsvPath + "game_constants.csv") == false) return false;
			BOSS_DISTANCE = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "BOSS_DISTANCE"), "value");
			MAX_LEVEL = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "MAX_LEVEL"), "value");
			INIT_MONEY = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "INIT_MONEY"), "value");
			MAX_ACT = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "MAX_ACT"), "value");
			DIN_SELECT_TIME = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "DIN_SELECT_TIME"), "value");
			MONEY_NAME = s_csv_masterdata.GetData(s_csv_masterdata.GetIndex("name", "MONEY_NAME"), "value");
			CHASER_DISTANCE = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "CHASER_DISTANCE"), "value");
			OASSIS_CHASER_MIN = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "OASSIS_CHASER_MIN"), "value");
			OASSIS_CHASER_MAX = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "OASSIS_CHASER_MAX"), "value");
			SCENARIO_1 = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "SCENARIO_1"), "value");
			SCENARIO_2 = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "SCENARIO_2"), "value");
			SCENARIO_3 = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "SCENARIO_3"), "value");
			SCENARIO_4 = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "SCENARIO_4"), "value");
			SCENARIO_5 = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "SCENARIO_5"), "value");
			ACT_INC = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "ACT_INC"), "value");
			MAX_DAMAGE_x = s_csv_masterdata.GetDataFloat(s_csv_masterdata.GetIndex("name", "MAX_DAMAGE_x"), "value");
			TP_ATB_INC = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "TP_ATB_INC"), "value");
			TP_ATTACK_INC = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "TP_ATTACK_INC"), "value");
			OASSIS_DISTANCE = s_csv_masterdata.GetDataInt(s_csv_masterdata.GetIndex("name", "OASSIS_DISTANCE"), "value");
			return true;
		}
	}
}
