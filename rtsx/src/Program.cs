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

            var ge = new GameEntity(new Coordinate(0.1, 0.1));
            ge.MoveTo = new Coordinate(-0.5, -0.3);
            gs.AddEntity(ge);

            Scene scene = new GameScene(gs);

            GUI.Window.Scene = scene;

        }
    }
}
