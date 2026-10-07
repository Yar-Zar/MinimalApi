using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMinimalApi.Infrastructure.Data;

namespace XunitTestMinimalApi.Utilities
{
    public static class DbContextHelper
    {
        public static AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            return new AppDbContext(options);
        }
    }
}
