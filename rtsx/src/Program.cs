using OpenTK;
using rtsx.src.GUI;
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

            GUI.SetGetDrawablesCallback(() => gs.Entities);
        }
    }
}
