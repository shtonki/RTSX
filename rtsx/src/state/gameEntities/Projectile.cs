using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rtsx.src.util;
using rtsx.src.view;

namespace rtsx.src.state.gameEntities
{
    class Projectile : GameEntity
    {
        public Projectile() : base(EntitySize.Tiny)
        {
        }

        public override void Draw(Drawer drawer)
        {
            var sizeHalved = Size / 2;
            drawer.FillRectangle(Location - sizeHalved, Location + sizeHalved, Color.AntiqueWhite);
            //base.Draw(drawer);
        }
    }
}
