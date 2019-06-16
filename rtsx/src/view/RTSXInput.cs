using OpenTK.Input;
using rtsx.src.state;
using rtsx.src.util;
using System;
using System.Collections.Generic;

namespace rtsx.src.view
{
    class InputToActionMapping
    {
        class Mapping
        {
            public RTSXInput Input { get; }
            public GameAction Action { get; set; }

            public Mapping(RTSXInput input, GameAction action)
            {
                Input = input;
                Action = action;
            }
        }

        private List<Mapping> mappings = new List<Mapping>();

        public void SetMapping(RTSXInput input, GameAction action)
        {
            int paranoiaCounter = 0;

            foreach (var mapping in mappings)
            {
                if (mapping.Input.Equals(input))
                {
                    paranoiaCounter++;

                    mapping.Action = action;
                }
            }

            if (paranoiaCounter == 0)
            {
                mappings.Add(new Mapping(input, action));
            }
            else if (paranoiaCounter > 1)
            {
                throw new RTSXException();
            }

        }

        public GameAction GetMapping(RTSXInput input)
        {
            foreach (var mapping in mappings)
            {
                if (mapping.Input.Equals(input))
                {
                    return mapping.Action;
                }
            }

            return new GameAction(GameActions.None);
        }
    }

    abstract class RTSXInput : IEquatable<RTSXInput>
    {
        public abstract bool Equals(RTSXInput other);
    }



    class MouseInput : RTSXInput
    {
        public enum MouseDirection { Up, Down };

        public MouseDirection Direction { get; }
        public MouseButton Button { get; }

        public MouseInput(MouseDirection direction, MouseButton button)
        {
            Direction = direction;
            Button = button;
        }

        public override bool Equals(RTSXInput other)
        {
            if (other is MouseInput v)
            {
                return
                    v.Button == Button &&
                    v.Direction == Direction;
            }
            return false;
        }
    }
}
