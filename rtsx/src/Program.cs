using OpenTK;
using rtsx.src.view;
using rtsx.src.state;
using rtsx.src.util;

namespace rtsx
{
    class Program
    {
        static void Main(string[] args)
        {
            GUI.Launch();

            GameState gs = new GameState();

            var ge = new GameEntity();
            ge.MoveTo = new Coordinate(-0.5, -0.5);
            gs.AddEntity(ge);

            var me = new MouseEntity();
            gs.AddEntity(me);

            GUI.Window.DrawablesCallback = () => gs.Entities;
        }
    }
}
