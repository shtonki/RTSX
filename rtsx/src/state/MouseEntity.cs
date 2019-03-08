using rtsx.src.util;
using rtsx.src.view;


namespace rtsx.src.state
{
    class MouseEntity : GameEntity
    {
        public MouseEntity() : base(new Coordinate(0.2, 0.2))
        {

        }

        protected override void Move()
        {
            Location = GUI.Window.MousePosition;
        }
    }
}
