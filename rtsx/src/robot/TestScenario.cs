using rtsx.src.state;
using rtsx.src.util;
using rtsx.src.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtsx.src.robot
{
    class TestScenario
    {
        public static void Test()
        {
            int testlength = 100;
            int subjectCount = 1;
            Robot[] subjects = new Robot[subjectCount];

            GameState test = new GameState();
            RobotTarget target = new RobotTarget();
            test.AddEntity(target);

            double[] scores = new double[subjects.Length];

            GUI.Window.Scene = new GameScene(test);

            for (int i = 0; i < subjects.Length; i++)
            {
                var subject = subjects[i] = new Robot();

                test.AddEntity(subject);
                subject.Target = target;

                target.Location = new Coordinate(0, 0.72);
                subject.Location = new Coordinate(0, 0);

                for (int j = 0; j < testlength; j++)
                {
                    test.Step();
                }

                scores[i] += -(subject.DistanceTo(target));

                subject.Target = null;
                test.RemoveEntity(subject);
            }

            int k = 0;
        }
    }
}
