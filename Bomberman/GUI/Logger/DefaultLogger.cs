using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.GUI.Logger
{
    class DefaultLogger : Logger
    {
        private Logger nextLogger;

        public void setNextLogger(Logger nextLogger)
        {
            this.nextLogger = nextLogger;
        }

        public void logMessage(Message log_message)
        {
            if(log_message.getLevel() == 3)
            {
                Console.WriteLine("Default: \n{0}", log_message.getMessage());
            }
            else
            {
                nextLogger.logMessage(log_message);
            }
            
        }
    }
}
