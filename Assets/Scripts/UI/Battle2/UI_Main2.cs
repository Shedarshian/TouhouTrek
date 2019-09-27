using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;

namespace ZMDFQ.UI.Battle
{
    using PlayerAction;
    using System.Reflection;

    public partial class UI_Main2
    {
        Game game;

        Player self;

        /// <summary>
        /// 当前选中的卡片
        /// </summary>
        List<ActionCard> selectedCards = new List<ActionCard>();

        List<Player> selectedPlayers = new List<Player>(); 

        /// <summary>
        /// 当前选中的技能，最多一个
        /// </summary>
        Skill selectedSkill;

        /// <summary>
        /// 当前的询问状态
        /// </summary>
        Request nowRequest;

        /// <summary>
        /// 处于FreeUse状态时，哪些牌能选择
        /// </summary>
        UseRequest useRequest;

        /// <summary>
        /// 当前收到的回应
        /// </summary>
        Response nowResponse;

        Dictionary<string, List<Action>> dic = new Dictionary<string, List<Action>>();

        /// <summary>
        /// 用于收集partial类中对应的方法
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        class BattleUIAttribute : Attribute
        {
            public string Name;
            public BattleUIAttribute(string Name)
            {
                this.Name = Name;
            }
        }

        partial void Init()
        {
            foreach (var method in GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                foreach (BattleUIAttribute attr in method.GetCustomAttributes(typeof(BattleUIAttribute), true))
                {
                    //通过反射获取对应特性，将相应的方法注册到dic中
                    List<Action> list;
                    dic.TryGetValue(attr.Name, out list);
                    Action action = method.CreateDelegate(typeof(Action), this) as Action;
                    if (list == null)
                    {
                        list = new List<Action>();
                        dic.Add(attr.Name, list);
                    }
                    list.Add(action);
                }
            }
            doDispatch(nameof(Init));
        }

        /// <summary>
        /// 调用对应partial类中注册的方法
        /// </summary>
        /// <param name="name"></param>
        void doDispatch(string name)
        {
            List<Action> list;
            if (dic.TryGetValue(name,out list))
            {
                foreach (var a in list) a();
            }
        }

        protected override void OnUpdate()
        { 
            base.OnUpdate();
            doDispatch(nameof(OnUpdate));
        }

        public void SetGame(Game game,Player self)
        {
            this.game = game;
            this.self = self;
            game.OnRequest += onRequest;
            game.OnResponse += onResponse;
            doDispatch(nameof(SetGame));
        }

        /// <summary>
        /// 刷新页面基本信息
        /// </summary>
        private void flush()
        {
            doDispatch(nameof(flush));
        }

        /// <summary>
        /// 强制刷新页面，无视所有动画
        /// </summary>
        private void flushCompelely()
        {
            doDispatch(nameof(flushCompelely));
        }
        /// <summary>
        /// 被询问时
        /// </summary>
        /// <param name="game"></param>
        /// <param name="request"></param>
        private void onRequest(Game game, Request request)
        {
            //m_ActivePlayer.SetVar("p", request.PlayerId == self.Id ? "你" : request.PlayerId.ToString());
            nowRequest = request;
            doDispatch(nameof(onRequest));
            flush();//感觉这里写的有问题
        }
        /// <summary>
        /// 回应询问时
        /// </summary>
        /// <param name="game"></param>
        /// <param name=""></param>
        private void onResponse(Game game, Response response)
        {
            nowResponse = response;
            doDispatch(nameof(onResponse));
            flush();
        }
    }
}
