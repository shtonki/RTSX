using rtsx.src.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtsx.src.state
{
    class BoundingBox
    {
        public Coordinate Size { get; }

        public BoundingBox(Coordinate size)
        {
            Size = size;
        }
    }
}
