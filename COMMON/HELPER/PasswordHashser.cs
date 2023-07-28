using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.HELPER
{
    public static class PasswordHashser
    {
        // Hash a password
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);

        }

        // Verify a password
        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
    }
}

