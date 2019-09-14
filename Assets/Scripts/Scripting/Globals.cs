
using ZMDFQ.PlayerAction;

namespace ZMDFQ
{
    class Globals
    {
        public Globals(Game game, ActionBase target)
        {
            api = new GameAPI(game, target);
        }
        public GameAPI api;
    }
}