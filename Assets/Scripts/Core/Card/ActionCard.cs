using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ
{
    public abstract class ActionCard : Card
    {
        internal abstract UseWay UseWay { get; }

        internal abstract Task DoEffect(Game game, UseOneCard useWay);
    }

    public abstract class ActionCard<T, K> : ActionCard where T : UseWay where K : UseOneCard
    {
        internal sealed override UseWay UseWay => useWay();

        internal sealed async override  Task DoEffect(Game game, UseOneCard useWay)
        {
            await doEffect(game, useWay as K);
        }

        protected abstract T useWay();

        protected abstract Task doEffect(Game game, K useWay);
    }
}
