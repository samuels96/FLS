﻿using System;
using System.Collections.Generic;

namespace eshop.Persistence.Models
{
    public partial class Admins
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
