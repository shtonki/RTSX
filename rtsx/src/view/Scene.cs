using rtsx.src.state;
using rtsx.src.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtsx.src.view
{
    interface Scene
    {
        void Render(Drawer drawer);

        void HandleInput(RTSXInput input);
    }

    class GameScene : Scene
    {
        private GameState GameState;
        private InputToActionMapping InputToActionMap { get; } = new InputToActionMapping();


        public GameScene(GameState gameState)
        {
            GameState = gameState;

            GenerateMappings();
        }

        private void GenerateMappings()
        {
            InputToActionMap.SetMapping(
                new MouseInput(MouseInput.MouseDirection.Down,
                OpenTK.Input.MouseButton.Left),
                new GameAction(GameActions.SelectStart));

            InputToActionMap.SetMapping(
                 new MouseInput(MouseInput.MouseDirection.Up,
                 OpenTK.Input.MouseButton.Left),
                 new GameAction(GameActions.SelectEnd));

            InputToActionMap.SetMapping(
                 new MouseInput(MouseInput.MouseDirection.Down,
                 OpenTK.Input.MouseButton.Right),
                 new GameAction(GameActions.RouteTo));
        }

        public void HandleInput(RTSXInput input)
        {
            var action = InputToActionMap.GetMapping(input);
            Logging.Log(action.Action);
            GameState.HandleAction(action);
        }

        public void Render(Drawer drawer)
        {
            var drawables = GameState.Entities;

            foreach (var drawable in drawables)
            {
                drawable.Draw(drawer);
            }
        }
    }
}
