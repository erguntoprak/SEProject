using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data
{
    public class EntitiesDbContext : IdentityDbContext<User>
    {
        public EntitiesDbContext(DbContextOptions<EntitiesDbContext> options) : base(options)
        {

        }
    }
}
