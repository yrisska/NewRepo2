using Microsoft.EntityFrameworkCore;
using RssManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.DataAccess
{
    //Simple context, require DB creaing via "Add-migration and update-database"
    public class RssManagerContext : DbContext
    {
        public string DbPath { get; }
        public RssManagerContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "rss-manager.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
