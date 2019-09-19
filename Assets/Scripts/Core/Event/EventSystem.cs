using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public class EventSystem
    {
        Dictionary<EventEnum, List<EventItem>> dic = new Dictionary<EventEnum, List<EventItem>>();
        struct EventItem
        {
            public Func<object[],Task> action;
            public int sortIndex;
        }
        public void Register(EventEnum eventEnum, Func<object[],Task> action, int sortIndex = 0)
        {
            List<EventItem> list;
            if (!dic.TryGetValue(eventEnum, out list))
            {
                list = new List<EventItem>();
                dic.Add(eventEnum, list);
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].sortIndex >= sortIndex)
                {
                    list.Insert(i, new EventItem()
                    {
                        action = action,
                        sortIndex = sortIndex,
                    });
                    return;
                }
            }
            list.Add(new EventItem()
            {
                action = action,
                sortIndex = sortIndex,
            });
        }
        public void Remove(EventEnum eventEnum, Func<object[], Task> action)
        {
            List<EventItem> list;
            if (dic.TryGetValue(eventEnum, out list))
            {
                list.RemoveAll(x => x.action == action);
            }
        }
        public async Task Call(EventEnum eventEnum, params object[] param)
        {
            List<EventItem> list;
            if (dic.TryGetValue(eventEnum, out list))
            {
                foreach (var eventItem in list)
                {
                    await eventItem.action(param);
                }
            }
        }
    }
}
