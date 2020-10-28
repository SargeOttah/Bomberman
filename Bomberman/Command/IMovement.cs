using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Command
{
    interface IMovement
    {
        void Execute(Player boxTrans, float moveDistance);
        //Move the box
        void Move(Player boxTrans) { }
    }
}
