using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.GUI.Visitor;

namespace Bomberman.GUI.Visitor
{
    interface Visitable
    {
        public void accept(IVisitor visitor);
    }
}
