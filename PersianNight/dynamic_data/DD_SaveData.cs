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
	//	なんかとにかく全部突っ込んであるので保存不要なものも多いはず
	[Serializable]
	public class DD_SaveData
	{
		/// <summary>現在の実行スクリプトファイル名</summary>
		public string m_szCurrentScriptFileName = "";
		/// <summary>クリア済シナリオリスト</summary>
		public List<int> m_list_CleardScenarioId;
		/// <summary>現在実行中のシナリオID</summary>
		public int m_nCurrentScenarioId;
		/// <summary>現在までのトータル歩行数</summary>
		public int m_nCurrentWalk;
		/// <summary>現在までのトータル歩行数</summary>
		public int nCurrentWalk { get { return m_nCurrentWalk; } }
		/// <summary>中ボスまでの距離</summary>
		public int m_nMiddleBossDistance;
		/// <summary>中ボスまでの距離</summary>
		public int nMiddleBossDistance { get { return m_nMiddleBossDistance; } }
		/// <summary>現在シナリオ攻略中かどうか</summary>
		public bool m_bIsGamePlaying = false;
		/// <summary>RPGモードかADVモードかの管理。ぶっちゃけ廃止したほうがいい</summary>
		public EnumScenarioStatus m_EnumScenarioStatus;
		/// <summary>オープニングかエンディングかその他のイベントか</summary>
		public EnumADVStatus m_CurrentEnumADVStatus;
		/// <summary>追跡者との距離</summary>
		public int m_nCurrentDistance;
		/// <summary>追跡者との距離</summary>
		public int nCurrentDistance { get { return m_nCurrentDistance; } }
		/// <summary>１行動ごとに縮まる追跡者との距離蓄積</summary>
		public int m_nDistanceOnAct;
		/// <summary>１行動ごとに縮まる追跡者との距離蓄積</summary>
		public int nDistanceOnAct { get { return m_nDistanceOnAct; } }
		/// <summary>オアシスまでの距離</summary>
		public int nOassisDistance;
		/// <summary>実行するシナリオ</summary>
		public MD_Scenarios m_CurrentScenarios;
		/// <summary>実行するシナリオ</summary>
		public MD_Scenarios CurrentScenarios { get { return m_CurrentScenarios; } }
		/// <summary>このシナリオでの主人公</summary>
		public MD_Characters m_CurrentHero;
		/// <summary>このシナリオでの主人公</summary>
		public MD_Characters CurrentHero { get { return m_CurrentHero; } }
		/// <summary>このシナリオでのヒロイン</summary>
		public MD_Characters m_CurrentHeroin;
		/// <summary>このシナリオでのヒロイン</summary>
		public MD_Characters CurrentHeroin { get { return m_CurrentHeroin; } }
		/// <summary>現在のシナリオでのイベントリスト</summary>
		public List<MD_Events> m_CurrentScenarioEvents;
		/// <summary>現在発生しているイベント</summary>
		public MD_Events m_CuurentEvent;
		/// <summary>現在発生しているイベント</summary>
		public MD_Events CuurentEvent { get { return m_CuurentEvent; } }
		/// <summary>次回発生イベントのindex</summary>
		public int m_nNextEventIndex;
		/// <summary>所持金</summary>
		public int m_nMoney = 0;
		/// <summary>所持金</summary>
		public int nMoney { get { return m_nMoney; } }
		/// <summary>パーティACTゲージ。行動ごとに少しずつたまり、ゲージマックスまで貯まると追跡者との距離が１縮む</summary>
		public int m_nPartyActGauge = 0;
		/// <summary>パーティACTゲージ。行動ごとに少しずつたまり、ゲージマックスまで貯まると追跡者との距離が１縮む</summary>
		public int nPartyActGauge { get { return m_nPartyActGauge; } }
		/// <summary>遭遇中の敵ベースデータ</summary>
		public MD_Monsters m_CurrentMonster = null;
		/// <summary>遭遇中の敵ベースデータ</summary>
		public MD_Monsters CurrentMonster { get { return m_CurrentMonster; } }
		/// <summary>コマンド実行中のDDインデックス</summary>
		public int m_nCommandCharaIndex;
		/// <summary>コマンド実行のターゲットとなるDDインデックス</summary>
		public List<int> m_list_CommandTargetIndex;
		/// <summary>なんだこれ</summary>
		public int m_nLogIndex;
		/// <summary>次回遭遇ボスのインデックス</summary>
		public int m_nBossIndex;
		/// <summary>フェニックスの尾。count=0なら持ってない。count=1なら持ってる true=未使用 false=使用済</summary>
		public List<bool> m_Phoenix;
		/// <summary>フェニックスの尾。count=0なら持ってない。count=1なら持ってる true=未使用 false=使用済</summary>
		public List<bool> Phoenix { get { return m_Phoenix; } }
		//==================================================================================
		//	バトル用動的データ
		//==================================================================================
		/// <summary>攻撃側ダメージ値</summary>
		public int m_nBattle_A_Damage;
		/// <summary>攻撃側ATK</summary>
		public int m_nBattle_A_Atk;
		/// <summary>攻撃側武器D補正</summary>
		public int m_nBattle_A_WeaponDamage;
		/// <summary>攻撃側武器ATK補正</summary>
		public int m_nBattle_A_WeaponAtk;
		/// <summary>防御側DEF</summary>
		public int m_nBattle_D_Def;
		/// <summary>防御側防具DEF補正</summary>
		public int m_nBattle_D_ArmorDef;
		/// <summary>アクションID</summary>
		public int m_nActionId;
		//==================================================================================
		//	ステータス管理用動的データ
		//==================================================================================
		/// <summary>バトル中かどうか</summary>
		public bool bIsBattle = false;
		/// <summary>ポーズ中かどうか。ポーズ中はATBは増加しない</summary>
		public bool bIsPause = false;
		/// <summary>コマンド選択待機フラグ。ATBバーの更新のためだけにある</summary>
		public bool bIsReadySelectCommand = false;
		/// <summary>ボス戦</summary>
		public bool bIsBoss = false;

		//DD_BattleCharacter
		public List<DD_BattleCharacter> s_list_DD_BattleCharacter;
		public List<int> s_list_CommandStackCharacter;

		//DD_ItemBox
		public List<EquippableItem> s_list_InBoxItems;
		/// <summary>インデックス発行用カウンター。セーブ対象</summary>
		public int s_nIndexCounter = 0;

		//Scene_RPG
//		public Wnd_BattleStatus m_Wnd_BattleStatus;
//		public Wnd_BattleCommandPanel m_Wnd_BattleCommandPanel;
//		public float fCurrentATB;
//		public float fMaxATB;


	}
}
