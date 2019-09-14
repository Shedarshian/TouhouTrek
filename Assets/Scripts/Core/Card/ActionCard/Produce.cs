using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 创作，个人影响力+1
    /// </summary>
    public class Produce : ActionCard
    {
        Script script { get; } = new Script("System.IO.File.Create(\"Test.txt\");");
        internal override void DoEffect(Game game, ActionBase target)
        {
            script.run(game, target);
        }
    }
}
