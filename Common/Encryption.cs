using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace Common
{
    public class Encryption
    {
        // Key size can be adjusted (e.g., 2048 or 4096).
        // I think key size greater than 2048 bits is overkill for most applications.
        // Values should be stored it in database and retrive from there.

        // For demo only
        private static int SaltSize = 150;
        private static int PasswordKeySize = 128;
        private static int TokenKeySize = 2048;
        private static int Iterations = 15000;
        public static void GenerateRsaKeys(string privateKeyPath, string publicKeyPath)
        {
            using (var rsa = RSA.Create())
            {
               
                rsa.KeySize = TokenKeySize; 

                var privateKey = rsa.ExportRSAPrivateKey();
                var privateKeyBase64 = Convert.ToBase64String(privateKey);

                var publicKey = rsa.ExportRSAPublicKey();
                var publicKeyBase64 = Convert.ToBase64String(publicKey);
                
                File.WriteAllText(privateKeyPath, privateKeyBase64);
                File.WriteAllText(publicKeyPath, publicKeyBase64);
               
            }
        }
        public string CreateHashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: PasswordKeySize);

            byte[] hashBytes = new byte[SaltSize + PasswordKeySize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, PasswordKeySize);

            string base64Hash = Convert.ToBase64String(hashBytes);
            return base64Hash;
        }
        public bool VerifyHashPassword(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: PasswordKeySize);

            for (int i = 0; i < PasswordKeySize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
