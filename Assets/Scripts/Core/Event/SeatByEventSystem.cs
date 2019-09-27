using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public class SeatByEventSystem
    {
        public int MaxSeat;

        Game game;

        Dictionary<EventEnum, List<EventItem>> dic = new Dictionary<EventEnum, List<EventItem>>();
        struct EventItem
        {
            public Func<object[], Task> action;
            public int Seat;
            public int sortIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventEnum"></param>
        /// <param name="seat">小于0视为优先发动</param>
        /// <param name="action"></param>
        /// <param name="sortIndex"></param>
        public void Register(EventEnum eventEnum,int seat, Func<object[], Task> action, int sortIndex = 0)
        {
            List<EventItem> list;
            if (!dic.TryGetValue(eventEnum, out list))
            {
                list = new List<EventItem>();
                dic.Add(eventEnum, list);
            }
            //for (int i = 0; i < list.Count; i++)
            //{
            //    if (list[i].sortIndex >= sortIndex)
            //    {
            //        list.Insert(i, new EventItem()
            //        {
            //            action = action,
            //            sortIndex = sortIndex,
            //        });
            //        return;
            //    }
            //}
            list.Add(new EventItem()
            {
                action = action,
                sortIndex = sortIndex,
                Seat = seat,
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
        public async Task Call(EventEnum eventEnum, int seat, params object[] param)
        {
            List<EventItem> list;
            if (dic.TryGetValue(eventEnum, out list))
            {
                list.Sort((x, y) =>
                {
                    int xindex = getDistance(seat, x.Seat);
                    int yindex = getDistance(seat, y.Seat);
                    if (x.Seat - y.Seat == 0)
                    {
                        return x.sortIndex - y.sortIndex;
                    }
                    else
                    {
                        return xindex - yindex;
                    }
                });
                foreach (var eventItem in list)
                {
                    await eventItem.action(param);
                }
            }
        }

        int getDistance(int startSeat,int seat)
        {
            if (seat < 0) return seat;
            int result = seat - startSeat;
            if (result < 0) result += MaxSeat;
            return result;
        }
    }
}
