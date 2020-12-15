using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.GUI.Logger
{
    public interface Logger
    {
        public void setNextLogger(Logger nextLogger);

        public void logMessage(Message log_message);
    }
}
