﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadorinator.Infrastructure
{
    static class FormatHelper
    {
        public static string FormatEta(int seconds)
        {
			if (seconds < 60)
			{
				return $"-{seconds}s";
			}
			if (seconds < 3600)
			{
				return $"-{seconds / 60}m";
			}
			if (seconds < 43200)
			{
				return $"-{seconds / 3600}h";
			}
			else
			{
				return $"-{seconds / 43200}d";
			}
		}
    }
}
