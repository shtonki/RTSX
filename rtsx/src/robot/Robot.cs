using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Neuro;
using rtsx.src.state;
using rtsx.src.util;

namespace rtsx.src.robot
{
    class Brain
    {
        public ActivationNetwork MovementBrain { get; }

        public Brain()
        {
            int inputs = 2;
            int braincells = 4;
            int outputs = 2;

            MovementBrain = new ActivationNetwork(new ThresholdFunction(),
                                               inputs, braincells, outputs);
            MovementBrain.Randomize();
        }
    }

    class RobotTarget : GameEntity
    {
        public RobotTarget() : base(EntitySize.Tiny)
        {

        }
    }

    class Robot : GameEntity
    {
        public RobotTarget Target;
        Brain Brain;

        const double MaxMove = 0.01;
        const double MaxTurnPct = 0.01;

        double CurMove = 0;
        double CurFace = 0;

        public Robot() : base(EntitySize.Small)
        {
            Brain = new Brain();
        }

        public override void Step(GameState gameState)
        {
            base.Step(gameState);
        }

        private double PctFacting(GameEntity other)
        {
            var a = AngleToInRad(other);
            Logging.Log(a);
            return 0;
        }

        protected override Coordinate DetermineMovement()
        {
            PctFacting(Target);
            return new Coordinate(0.001, 0.0005);
        }


        /*
        private static ActivationNetwork Clonerx(ActivationNetwork source)
        {
            var cln = new ActivationNetwork(new ThresholdFunction(), source.Layers[0].)

            source.Layers.Length
            
            return null;
        }*/
    }
}
