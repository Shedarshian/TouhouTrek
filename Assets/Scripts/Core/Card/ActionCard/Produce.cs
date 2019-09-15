using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 创作，个人影响力+1
    /// </summary>
    class Produce : ActionCard
    {
        Script script { get; } = new Script("game.Players.Find(p => { return p.Id == target.playerId; }).Size += 2;");
        internal override void DoEffect(Game game, Response target)
        {
            script.run(game, target);
        }
    }
}
