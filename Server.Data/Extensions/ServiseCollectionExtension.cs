using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Extensions
{
    public static class ServiseCollectionExtension
    {
        public static void AddData(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ElCoursesDb");
            services.AddDbContext<ElCoursesDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.Parse("8.0.41-mysql")));
        }
    }
}
