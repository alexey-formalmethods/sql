﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions
{
    public static class Utils
    {
        public static bool WaitFor(int millisecondsDelay)
        {
            System.Threading.Tasks.Task.Delay(millisecondsDelay);
            return true;
        }
    }
}
