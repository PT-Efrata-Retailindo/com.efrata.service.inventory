﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Efrata.Service.Inventory.Lib.Services
{
    public interface IIdentityService
    {
        string Username { get; set; }
        string Token { get; set; }
        int TimezoneOffset { get; set; }
    }
}
