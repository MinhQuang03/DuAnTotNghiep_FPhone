using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FW.DAO
{
    /// <summary>
    /// DbContext thiết lập chung
    /// </summary>
    public abstract class BaseDBContext<TEntity> : IdentityDbContext<TEntity>
    where TEntity : IdentityUser
    {
        //static LoggerFactory object
        //private readonly ILoggerFactory _loggerFactory;


        public BaseDBContext(DbContextOptions options) : base(options)    
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        protected BaseDBContext()
        {
            
        }


        ///// <summary>
        ///// constructor nhận vào thông tin kết nối database
        ///// </summary>
        ///// <param name="connectionInfo"></param>
        //public BaseDBContext(ILoggerFactory loggerFactory, ConnectionInfo connectionInfo)
        //{
        //    _loggerFactory = loggerFactory;
        //    ConnectionInfo = connectionInfo;
        //    this.ChangeTracker.LazyLoadingEnabled = false;
        //}

        ///// <summary>
        ///// cấu hình DBContext
        ///// </summary>
        ///// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // kết nối tới MSSQLServer
            optionsBuilder.UseSqlServer(@"Data Source=MSI;Initial Catalog=PRO219_WebsiteBanDienThoai_v2;Integrated Security=True;TrustServerCertificate=True");
        }

    }
}
