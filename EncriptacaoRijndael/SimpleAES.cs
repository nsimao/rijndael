using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EncriptacaoRijndael
{
    public class SimpleAES : IDisposable
    {
        private readonly Random random;
        private readonly byte[] key;
        private readonly RijndaelManaged rm;
        private readonly UTF8Encoding encoder;

        public SimpleAES(string keyStr)
        {
            if ((string.IsNullOrEmpty(keyStr)) ||
                (keyStr.Length != 16 && keyStr.Length != 24 && keyStr.Length != 32))
            {
                throw new Exception("A keyStr de criptografia deve possuir 16, 24 ou 32 caracteres.");
            }

            this.random = new Random();
            this.rm = new RijndaelManaged();
            this.encoder = new UTF8Encoding();
            this.key = Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(keyStr)));
        }

        public string EncryptStr(string unencrypted)
        {
            return Convert.ToBase64String(Encrypt(unencrypted));
        }

        public string DecryptStr(string encrypted)
        {
            return this.encoder.GetString(this.Decrypt(encrypted));
        }

        public byte[] Encrypt(string unencrypted)
        {
            var vector = new byte[16];
            this.random.NextBytes(vector);

            var buffer = this.encoder.GetBytes(unencrypted);

            var encryptor = this.rm.CreateEncryptor(this.key, vector);

            var encrypted = this.Transform(buffer, encryptor);

            var cryptogram = vector.Concat(encrypted);

            return cryptogram.ToArray();
        }

        public byte[] Decrypt(string encrypted)
        {
            var cryptogram = Convert.FromBase64String(encrypted);
            if (cryptogram.Length < 17)
            {
                throw new ArgumentException("Not a valid encrypted string", "encrypted");
            }

            var vector = cryptogram.Take(16).ToArray();
            var buffer = cryptogram.Skip(16).ToArray();

            var decryptor = this.rm.CreateDecryptor(this.key, vector);
            return this.Transform(buffer, decryptor);
        }

        private byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            using (var stream = new MemoryStream())
            {
                using (var cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                {
                    cs.Write(buffer, 0, buffer.Length);
                }

                return stream.ToArray();
            }
        }

        public void Dispose()
        {
            if (this.rm != null)
            {
                this.rm.Dispose();
            }
        }
    }
}
