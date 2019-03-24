using Common.DbModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Context
{
    public class testDBContext : IdentityDbContext<ApplicationUser>
    {
        public testDBContext(DbContextOptions<testDBContext> options)
            : base(options)
        {

        }
    }
}
