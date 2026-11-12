using BC = BCrypt.Net.BCrypt;   // ← alias: BC = class BCrypt

namespace _25DH110506_MyStore.Helpers
{
    public static class PasswordHelper
    {
        public static string Hash(string plainPassword)
        {
            return BC.HashPassword(plainPassword);    // ← dùng alias
        }

        public static bool Verify(string plainPassword, string hashedPassword)
        {
            if (string.IsNullOrEmpty(hashedPassword)) return false;
            try
            {
                return BC.Verify(plainPassword, hashedPassword);
            }
            catch
            {
                return false;
            }
        }
    }
}