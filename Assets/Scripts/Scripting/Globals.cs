
using ZMDFQ.PlayerAction;

namespace ZMDFQ
{
    public class Globals
    {
        public Globals(Game game, Response target)
        {
            api = new GameAPI(game, target);
        }
        public GameAPI api;
    }
}