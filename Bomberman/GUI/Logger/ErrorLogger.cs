﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.GUI.Logger
{
    class ErrorLogger : Logger
    {
        private Logger nextLogger;
        public void setNextLogger(Logger nextLogger)
        {
            this.nextLogger = nextLogger;
        }

        public void logMessage(Message log_message)
        {
            if (log_message.getLevel() == 1)
            {
                Console.WriteLine("Error: \n{0}", log_message.getMessage());
            }
            else
            {
                nextLogger.logMessage(log_message);
            }
        }
    }
}
