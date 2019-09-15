using ZMDFQ.PlayerAction;

namespace ZMDFQ.Effect
{
    class SimpleScriptingEffect : EffectBase<SimpleResponse>
    {
        Script script { get; }
        public SimpleScriptingEffect(Script script)
        {
            this.script = script;
        }
        public override void Enable(Game game, SimpleResponse response)
        {
            script.run(game, response);
        }
    }
}
