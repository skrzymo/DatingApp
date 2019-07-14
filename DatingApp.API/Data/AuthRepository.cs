using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        #region Const&Readonly

        private readonly DataContext _context;

        #endregion

        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Constructors

        public AuthRepository(DataContext context)
        {
            this._context = context;
        }

        #endregion

        #region InterfaceImplementation

        public async Task<User> Login(string username, string password)
        {
            var user = await this._context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
            {
                return null;
            }

            if (!this.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }        

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            this.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await this._context.Users.AddAsync(user);
            await this._context.SaveChangesAsync();

            return user;
        }        

        public async Task<bool> UserExists(string username)
        {
            if (await this._context.Users.AnyAsync(x => x.Username == username))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region OtherMembers

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion        
    }
}
