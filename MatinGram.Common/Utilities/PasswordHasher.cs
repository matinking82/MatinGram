using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Common.Utilities
{
    public static class PasswordHasher
    {
        public static string ToHashed(this string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();

            foreach (var i in bytes)
            {
                builder.Append(i.ToString("x2"));
            }
            return builder.ToString();
        }

        public static async Task<string> ToHashedAsync(this string password)
        {
            return await Task.Run(() =>
            {
                using SHA256 sha256 = SHA256.Create();
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                foreach (var i in bytes)
                {
                    builder.Append(i.ToString("x2"));
                }
                return builder.ToString();
            });
        }
    }
}

