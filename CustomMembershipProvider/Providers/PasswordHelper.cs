using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace CustomMembershipProvider.Providers
{
    public static class PasswordHelper
    {
        private static string GenerateSalt()
        {
            var numArray = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(numArray);
            return Convert.ToBase64String(numArray);
        }

        private static HashAlgorithm GetHashAlgorithm(string password)
        {
            return HashAlgorithm.Create(Membership.HashAlgorithmType);
        }

        public static string StoredPassword(string storedString, out string salt)
        {
            var saltLen = GenerateSalt();
            salt = storedString.Substring(0, saltLen.Length);
            return storedString.Substring(saltLen.Length);
        }

        public static string EncryptOrHashPassword(string pass, string salt, MembershipPasswordFormat passwordFormat)
        {
            var bytes = Encoding.Unicode.GetBytes(pass);
            var saltBytes = Convert.FromBase64String(salt);
            byte[] inArray = new byte[] { };

            if (passwordFormat == MembershipPasswordFormat.Hashed)
            {
                var hashAlgorithm = GetHashAlgorithm(pass);
                if (hashAlgorithm is KeyedHashAlgorithm algorithm)
                {
                    var keyedHashAlgorithm = algorithm;
                    if (keyedHashAlgorithm.Key.Length == saltBytes.Length)
                    {
                        //if the salt bytes is the required key length for the algorithm, use it as-is
                        keyedHashAlgorithm.Key = saltBytes;
                    }
                    else if (keyedHashAlgorithm.Key.Length < saltBytes.Length)
                    {
                        //if the salt bytes is too long for the required key length for the algorithm, reduce it
                        var numArray2 = new byte[keyedHashAlgorithm.Key.Length];
                        Buffer.BlockCopy(saltBytes, 0, numArray2, 0, numArray2.Length);
                        keyedHashAlgorithm.Key = numArray2;
                    }
                    else
                    {
                        //if the salt bytes is too short for the required key length for the algorithm, extend it
                        var numArray2 = new byte[keyedHashAlgorithm.Key.Length];
                        var dstOffset = 0;
                        while (dstOffset < numArray2.Length)
                        {
                            var count = Math.Min(saltBytes.Length, numArray2.Length - dstOffset);
                            Buffer.BlockCopy(saltBytes, 0, numArray2, dstOffset, count);
                            dstOffset += count;
                        }

                        keyedHashAlgorithm.Key = numArray2;
                    }

                    inArray = keyedHashAlgorithm.ComputeHash(bytes);
                }
                else
                {
                    var buffer = new byte[saltBytes.Length + bytes.Length];
                    Buffer.BlockCopy(saltBytes, 0, buffer, 0, saltBytes.Length);
                    Buffer.BlockCopy(bytes, 0, buffer, saltBytes.Length, bytes.Length);
                    inArray = hashAlgorithm.ComputeHash(buffer);
                }
            }

            return Convert.ToBase64String(inArray);
        }
    }
}