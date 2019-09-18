using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 创作，个人影响力+1
    /// </summary>
    class Produce : ActionCard<SimpleRequest, SimpleResponse>
    {
        Script script { get; } = new Script("game.Players.Find(p => { return p.Id == target.playerId; }).Size += 2;");
        protected override SimpleRequest useWay()
        {
            return SimpleRequest.Instance;
        }
        protected override Task doEffect(Game game, SimpleResponse useWay)
        {
            return script.run(game, useWay);
        }
    }
}
