using PRIME_UCR.Application.Services.Multimedia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Implementations.Multimedia
{
    public class FileService : IFileService
    {
        public string FilePath { get; set; }
        public string KeyString { get; set; }
        public string IVString { get; set; }
        public EncryptionService ES { get; set; }

        public FileService()
        {
            FilePath = "wwwroot/data/";
            ES = new EncryptionService();
        }
        public async Task<bool> StoreFile(string pathDecrypted, string fileName, string extension, Stream fileStream)
        {
            DirectoryInfo info = new DirectoryInfo(pathDecrypted);
            if (!info.Exists) {
                info.Create();
            }
            string completeFileName = fileName + extension;
            string path = Path.Combine(pathDecrypted, completeFileName); 
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                await fileStream.CopyToAsync(outputFileStream);
            }
            ES.EncryptFile(path);

            return true;
        }

        public async Task<string> StoreTextFile(string text, string fileName, string apCode, string callingPlace, string actionName, string checkListName, string checkListItemName)
        {

            string path = GetFilePath(apCode, callingPlace, actionName, checkListName, checkListItemName);

            DirectoryInfo info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                info.Create();
            }

            string fullPath = Path.Join(path, fileName + ".txt");

            using (StreamWriter sw = File.CreateText(fullPath))
            {
                await sw.WriteLineAsync(text);
            }
            ES.EncryptFile(fullPath);
            return fullPath;
        }

        public void SetKeyIV(byte[] iv, byte[] key) {
            ES.SetKeyIV(iv, key);
        }

        public bool DeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public string GetFilePath(string apCode, string callingPlace, string actionName, string checkListName, string checkListItemName)
        {
            string multiString = "Multimedia";
            byte[] multiByte = ES.Encrypt(multiString);
            string multiEncrypted = Convert.ToBase64String(multiByte);
            string path = "wwwroot/data/" + ES.EncodeString(multiEncrypted) + "/";
            byte[] IncidentCodeEncryptedByte = null;
            string IncidentCodeEncryptedString = "";
            string general = "General";
            switch (callingPlace)
            {
                case "action":
                    IncidentCodeEncryptedByte = ES.Encrypt(apCode);
                    IncidentCodeEncryptedString = Convert.ToBase64String(IncidentCodeEncryptedByte);
                    path += ES.EncodeString(IncidentCodeEncryptedString);
                    path += "/";
                    byte[] ActionNameEncryptedByte = ES.Encrypt(actionName);
                    string ActionNameEncryptedString = Convert.ToBase64String(ActionNameEncryptedByte);
                    path += ES.EncodeString(ActionNameEncryptedString);
                    path += "/";
                    break;
                case "checkListItem":
                    IncidentCodeEncryptedByte = ES.Encrypt(apCode);
                    IncidentCodeEncryptedString = Convert.ToBase64String(IncidentCodeEncryptedByte);
                    path += ES.EncodeString(IncidentCodeEncryptedString);
                    path += "/";
                    byte[] checkListNameByte = ES.Encrypt(checkListName);
                    string checkListNameEncryptedString = Convert.ToBase64String(checkListNameByte);
                    path += ES.EncodeString(checkListNameEncryptedString);
                    path += "/";
                    byte[] checkListItemByte = ES.Encrypt(checkListItemName);
                    string checkListItemString = Convert.ToBase64String(checkListItemByte);
                    path += ES.EncodeString(checkListItemString);
                    path += "/";
                    break;
                default:
                    byte[] generalRepositoyByte = ES.Encrypt(general);
                    string generalRepositoyByteString = Convert.ToBase64String(generalRepositoyByte);
                    path += ES.EncodeString(generalRepositoyByteString);
                    path += "/";
                    break;
            }
            return path;
        }

        public async Task<bool> StoreFileNoEncryption(string fileName, Stream fileStream)
        {
            DirectoryInfo info = new DirectoryInfo(FilePath);
            if (!info.Exists) info.Create();

            string path = Path.Combine(FilePath, fileName);
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                await fileStream.CopyToAsync(outputFileStream);
            }
            return true;
        }

        public async Task<string> StoreTextFile(string text, string fileName)
        {
            string path = Path.Combine(FilePath, fileName);
            using (StreamWriter sw = File.CreateText(path))
            {
                await sw.WriteLineAsync(text);
            }
            return path;
        }
    }
}
