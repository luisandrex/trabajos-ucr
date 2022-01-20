using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using PRIME_UCR.Domain.Models.UserAdministration;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using PRIME_UCR.Application.Services.Multimedia;
using PRIME_UCR.Application.Implementations.Multimedia;

namespace PRIME_UCR.Test.UnitTests.Application.Multimedia
{
    public class EncryptionServiceTest
    {
        [Fact]
        public void encryptAndDecryptUsingSameKeyIV() {
            //Encrypt with keyString and ivString
            EncryptionService ES = new EncryptionService(true);
            string keyString = "qXOctUgD1RQCyF6dl4IjgZLAosrLh8Dn8GCklADSmvo=";
            string ivString = "fkmYijInbe9eWQbLoWtTNQ==";
            byte[] ivByte = Convert.FromBase64String(ivString);
            byte[] keyByte = Convert.FromBase64String(keyString);
            //set the key and iv to the ES
            ES.SetKeyIV(ivByte, keyByte);
            //start the test
            string stringTest = "This is a string test for encryption service";
            //test for Encrypt method
            byte [] encryptedArray = ES.Encrypt(stringTest); //The string is already encrypted
            //test for Decrypt method
            string decryptedString = ES.Decrypt(encryptedArray);
            Assert.Equal(decryptedString, stringTest);
        }
        [Fact]
        public void encryptAndDecryptUsingDifferentKey() {
            //ES2 uses a diferent key than ES1 
            EncryptionService ES1 = new EncryptionService(true);
            EncryptionService ES2 = new EncryptionService(true);
            string keyString = "qXOctUgD1RQCyF6dl4IjgZLAosrLh8Dn8GCklADSmvo="; 
            string ivString = "fkmYijInbe9eWQbLoWtTNQ==";
            byte[] ivByte = Convert.FromBase64String(ivString);
            byte[] keyByte = Convert.FromBase64String(keyString);
            byte[] keyByte1 = ES2.generateKey();
            //set the key and iv to the ES1 and ES2
            ES1.SetKeyIV(ivByte, keyByte);
            ES2.SetKeyIV(ivByte, keyByte1);
            //declare a string
            string stringTest = "This is a string test for encryption service";
            //Encrypt with ES1
            byte[] encryptedArray = ES1.Encrypt(stringTest);
            //Decrypt with ES2
            string decryptedString = " ";
            try
            {
                decryptedString = ES2.Decrypt(encryptedArray);
            }
            catch {
                decryptedString = "Cant execute the decryption";
            }
            Assert.NotEqual(decryptedString, stringTest);
        }
        [Fact]
        public void encryptAndDecryptUsingDifferentIV()
        {
            //ES2 uses a diferent IV than ES1 
            EncryptionService ES1 = new EncryptionService(true);
            EncryptionService ES2 = new EncryptionService(true);
            string keyString = "qXOctUgD1RQCyF6dl4IjgZLAosrLh8Dn8GCklADSmvo=";
            string ivString = "fkmYijInbe9eWQbLoWtTNQ==";
            byte[] ivByte = Convert.FromBase64String(ivString);
            byte[] keyByte = Convert.FromBase64String(keyString);
            byte[] ivByte1 = ES2.generateKey();
            //set the key and iv to the ES1 and ES2
            ES1.SetKeyIV(ivByte, keyByte);
            ES2.SetKeyIV(ivByte1, keyByte);
            //declare a string
            string stringTest = "This is a string test for encryption service";
            //Encrypt with ES1
            byte[] encryptedArray = ES1.Encrypt(stringTest);
            //Decrypt with ES2
            string decryptedString = " ";
            try
            {
                decryptedString = ES2.Decrypt(encryptedArray);
            }
            catch
            {
                decryptedString = "Cant execute the decryption";
            }
            Assert.NotEqual(decryptedString, stringTest);
        }
        [Fact]
        public void generateKeyNotNull() {
            EncryptionService ES = new EncryptionService(true);
            byte[] key = null;
            key = ES.generateKey();
            Assert.NotNull(key);
        }
        [Fact]
        public void generateIvNotNull()
        {
            EncryptionService ES = new EncryptionService(true);
            byte[] iv = null;
            iv = ES.generateIV();
            Assert.NotNull(iv);
        }
        [Fact]
        public void keyAndIVNullWithoutSetting()
        {
            EncryptionService ES = new EncryptionService(true);
            Assert.Null(ES.Key);
            Assert.Null(ES.IV);
        }
    }
}
