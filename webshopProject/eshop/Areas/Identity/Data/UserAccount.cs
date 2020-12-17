using AspNetCore.Identity.Mongo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop.Areas.Identity.Data
{
    public class UserAccount : MongoUser
    {
        public int CustomerID { get; set; }
    }
}
