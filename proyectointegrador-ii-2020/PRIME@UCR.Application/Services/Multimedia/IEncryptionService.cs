using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.Services.Multimedia
{
    public interface IEncryptionService
    {
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }

        public byte[] Encrypt(string plainText);
        public bool EncryptFile(string path);
        public bool DecryptFile(string path);
        public bool ByteArrayToFile(string filePath, byte[] byteArray);
        public string Decrypt(byte[] cipherText);
        public byte[] StringToByteArray(string fileText);
        public string FiletoString(string path);
        public byte[] FileToByteArray(string path);
        public bool StringToFile(string filePath, string decryptedString);
        public void SetKeyIV(byte[]iv,byte[]key);
        public byte[] generateKey();
        public byte[] generateIV();
        public string EncodeString(string uncodedString);
        public string DecodeString(string codedString);
    }
}
