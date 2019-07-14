using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {

        #region Const&Readonly



        #endregion

        #region Fields



        #endregion

        #region Properties

        public DbSet<Value> Values { get; set; }

        public DbSet<User> Users { get; set; }

        #endregion

        #region Constructors

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        #endregion

        #region InterfaceImplementation



        #endregion

        #region OtherMembers



        #endregion

    }
}
