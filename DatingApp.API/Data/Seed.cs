using DatingApp.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public class Seed
    {

        #region Const&Readonly



        #endregion

        #region Fields

        private readonly DataContext _context;

        #endregion

        #region Properties



        #endregion

        #region Constructors

        public Seed(DataContext context)
        {
            this._context = context;
        }

        #endregion

        #region InterfaceImplementation



        #endregion

        #region OtherMembers

        public void SeedUsers()
        {
            var userData = File.ReadAllText("Data/UserSeedData.json");

            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            foreach (var user in users)
            {
                byte[] passwordHash, passwordSalt;
                this.CreatePasswordHash("password", out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();

                this._context.Users.Add(user);
            }

            this._context.SaveChanges();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        #endregion        
    }
}
