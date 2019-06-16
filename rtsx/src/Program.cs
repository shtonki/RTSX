using OpenTK;
using rtsx.src.view;
using rtsx.src.state;
using rtsx.src.util;
using rtsx.src.state.gameEntities;

namespace rtsx
{
    class Program
    {
        static void Main(string[] args)
        {
            GUI.Launch();

            GameState gs = new GameState();

            var ge = new DummyUnit();
            ge.Location = new Coordinate(0.5, 0.5);
            gs.AddEntity(ge);

            ge = new DummyUnit();
            ge.MoveSpeed = 0.002;
            gs.AddEntity(ge);

            Scene scene = new GameScene(gs);

            GUI.Window.Scene = scene;

        }
    }
}
