/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ZMDFQ.UI.Battle
{
	public partial class UI_Main2 : GComponent
	{
		public Controller m_ChooseHero;
		public UI_PlayerSimpleInfo m_PlayerInfo0;
		public UI_PlayerSimpleInfo m_PlayerInfo1;
		public UI_PlayerSimpleInfo m_PlayerInfo2;
		public UI_PlayerSimpleInfo m_PlayerInfo3;
		public UI_PlayerSimpleInfo m_PlayerInfo4;
		public UI_PlayerSimpleInfo m_PlayerInfo5;
		public UI_PlayerSimpleInfo m_PlayerInfo6;
		public UI_PlayerSimpleInfo m_PlayerInfo7;
		public GTextField m_EventDeckCount;
		public GTextField m_ActionDeckCount;
		public UI_MainSize m_MainSize;
		public GTextField m_RoundInfo;
		public GTextField m_TurnInfo;
		public UI_Card m_NowEvent;
		public GList m_DelayAction;
		public GTextField m_ActionDropDeck;
		public GTextField m_EventDropDeckCount;
		public GComponent m_GameInfo;
		public UI_HandCards m_Hand;
		public UI_Card m_SetEvent;
		public GComponent m_NowTheme;
		public GTextField m_ThemeDeckCount;
		public GTextField m_ActivePlayer;
		public GProgressBar m_TimeBar;
		public UI_Card m_NowAction;
		public UI_ChooseHero m_HeroChooseWindow;

		public const string URL = "ui://oacz4rtmgs0sc";

		public static UI_Main2 CreateInstance()
		{
			return (UI_Main2)UIPackage.CreateObject("Battle","Main2");
		}

		public UI_Main2()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_ChooseHero = this.GetControllerAt(0);
			m_PlayerInfo0 = (UI_PlayerSimpleInfo)this.GetChildAt(1);
			m_PlayerInfo1 = (UI_PlayerSimpleInfo)this.GetChildAt(2);
			m_PlayerInfo2 = (UI_PlayerSimpleInfo)this.GetChildAt(3);
			m_PlayerInfo3 = (UI_PlayerSimpleInfo)this.GetChildAt(4);
			m_PlayerInfo4 = (UI_PlayerSimpleInfo)this.GetChildAt(5);
			m_PlayerInfo5 = (UI_PlayerSimpleInfo)this.GetChildAt(6);
			m_PlayerInfo6 = (UI_PlayerSimpleInfo)this.GetChildAt(7);
			m_PlayerInfo7 = (UI_PlayerSimpleInfo)this.GetChildAt(8);
			m_EventDeckCount = (GTextField)this.GetChildAt(13);
			m_ActionDeckCount = (GTextField)this.GetChildAt(17);
			m_MainSize = (UI_MainSize)this.GetChildAt(19);
			m_RoundInfo = (GTextField)this.GetChildAt(20);
			m_TurnInfo = (GTextField)this.GetChildAt(21);
			m_NowEvent = (UI_Card)this.GetChildAt(34);
			m_DelayAction = (GList)this.GetChildAt(35);
			m_ActionDropDeck = (GTextField)this.GetChildAt(37);
			m_EventDropDeckCount = (GTextField)this.GetChildAt(40);
			m_GameInfo = (GComponent)this.GetChildAt(44);
			m_Hand = (UI_HandCards)this.GetChildAt(45);
			m_SetEvent = (UI_Card)this.GetChildAt(46);
			m_NowTheme = (GComponent)this.GetChildAt(49);
			m_ThemeDeckCount = (GTextField)this.GetChildAt(54);
			m_ActivePlayer = (GTextField)this.GetChildAt(56);
			m_TimeBar = (GProgressBar)this.GetChildAt(58);
			m_NowAction = (UI_Card)this.GetChildAt(59);
			m_HeroChooseWindow = (UI_ChooseHero)this.GetChildAt(71);
			Init();
		}
		
		partial void Init();
	}
}