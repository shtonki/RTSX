using OpenTK;
using rtsx.src.view;
using rtsx.src.state;
using rtsx.src.util;
using rtsx.src.state.gameEntities;
using System.Drawing;

namespace rtsx
{
    class Program
    {
        static void Main(string[] args)
        {
            GUI.Launch();

            GameState gs = new GameState();
            var hero = new Player(true, Color.Green);
            var villain = new Player(false, Color.Red);
            var fixtureOwner = new Player(false, Color.Black);
            GameEntity ge;

            var heroUnit = new Ranger(hero);
            heroUnit.Location = new Coordinate(0.5, 0.5);
            gs.AddEntity(heroUnit);

            var villainUnit = new Warrior(hero);
            villainUnit.MoveSpeed = 0.002;
            villainUnit.Following = heroUnit;
            gs.AddEntity(villainUnit);

            ge = new Fixture(Fixtures.Tree, EntitySize.Medium, fixtureOwner);
            ge.Location = new Coordinate(0.25, 0.25);
            gs.AddEntity(ge);


            Scene scene = new GameScene(gs);
            GUI.Window.Scene = scene;
        }
    }
}
