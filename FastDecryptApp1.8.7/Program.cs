// Decompiled with JetBrains decompiler
// Type: FastDecryptApp.Program
// Assembly: FastDecryptApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BB1DC0FB-5660-4104-AAC2-6DAD0F326599
// Assembly location: C:\Users\jgood\Desktop\FastDecryptApp1.8.7.exe

using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace FastDecryptApp
{
  internal class Program
  {
    private static string enc_ext = ".weapologize";
    private static string privkey = "";
    private static bool analysisMode = false;
    private static string logPath = "C:\\logs\\fastdecryptlog.txt";
    private static string decryptedFiles = "C:\\logs\\decryptedFiles.txt";
    private static string encryptedFiles = "C:\\logs\\encryptedFiles.txt";
    private static int encryptedFilesCount = 0;
    private static int decryptedFilesCount = 0;
    private static int totalFiles = 0;
    private static int deletedFiles = 0;
    private static int toDecryptCount = Program.encryptedFilesCount - Program.prevDecrypted;
    private static int filePathTooLongCount = 0;
    private static int prevDecrypted = 0;
    private static int sorryDeleteCount = 0;
    private static int totalSubDirectories = 0;

    private static void Main(string[] args)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      Directory.CreateDirectory("C:\\logs");
      if (args.Length > 1)
      {
        if (args[0] == "-k")
        {
          foreach (string file in Directory.GetFiles(args[1]))
          {
            Program.privkey = File.ReadAllText(file);
            try
            {
              string s1 = "";
              string s2 = "";
              string stringFromBytes = Encipher.GetStringFromBytes(Encipher.GetHeaderBytesFromFile(args[1]));
              XmlDocument xmlDocument = new XmlDocument();
              xmlDocument.LoadXml(stringFromBytes);
              foreach (XmlNode xmlNode in xmlDocument.GetElementsByTagName("AAA"))
                s1 = xmlNode.InnerText;
              foreach (XmlNode xmlNode in xmlDocument.GetElementsByTagName("AA"))
                s2 = xmlNode.InnerText;
              foreach (XmlNode xmlNode in xmlDocument.GetElementsByTagName("AAAAAAAAAAAAAAAAAA"))
                Convert.ToInt64(xmlNode.InnerText);
              byte[] numArray = Encipher.RSADescryptBytes(Convert.FromBase64String(s1), Program.privkey);
              Encipher.RSADescryptBytes(Convert.FromBase64String(s2), Program.privkey);
              if ((uint) numArray.Length > 0U)
                Console.WriteLine("\r\nCORRECT KEY IS:" + file);
            }
            catch (Exception ex)
            {
            }
          }
        }
        if (args[0] == "-d")
        {
          Program.privkey = File.ReadAllText(args[1]);
          File.AppendAllText(Program.logPath, "---------------------------------------" + Environment.NewLine);
          Program.deleteRec(args[2]);
          Console.WriteLine("-- Done --");
          Console.WriteLine("-----------------------------");
          Console.WriteLine("Information");
          Console.WriteLine("Total Scanned Files : " + (object) Program.totalFiles);
          Console.WriteLine("Total Encrypted Files : " + (object) Program.encryptedFilesCount);
          Console.WriteLine("Total Previously Decrypted Files : " + (object) Program.prevDecrypted);
          Console.WriteLine("Total Decrypted Files : " + (object) Program.decryptedFilesCount);
          Console.WriteLine("Total Deleted Files (does not include sorry files) : " + (object) Program.deletedFiles);
          Console.WriteLine("Total Deleted Sorry Files : " + (object) Program.sorryDeleteCount);
          Console.WriteLine("Total Path Too Long : " + (object) Program.filePathTooLongCount);
          File.AppendAllText(Program.logPath, Environment.NewLine + "Stats : Total Scanned Files : " + (object) Program.totalFiles + ", Total Encrypted Files : " + (object) Program.encryptedFilesCount + ", Total Previously Decrypted Files : " + (object) Program.prevDecrypted + ", Total Decrypted Files : " + (object) Program.decryptedFilesCount + ", Total Deleted Files (does not include sorry files) : " + (object) Program.deletedFiles + ", Total Deleted Sorry Files : " + (object) Program.sorryDeleteCount + ", Total Path Too Long : " + (object) Program.filePathTooLongCount + Environment.NewLine);
        }
        if (args[0] == "-a")
        {
          File.AppendAllText(Program.logPath, Environment.NewLine + "---------------------------------------" + Environment.NewLine);
          Program.analysisRec(args[1]);
          Console.WriteLine("-- Done --");
          Console.WriteLine("-----------------------------");
          Console.WriteLine("Information");
          Console.WriteLine("Total Scanned Files : " + (object) Program.totalFiles);
          Console.WriteLine("Total Encrypted Files : " + (object) Program.encryptedFilesCount);
          Console.WriteLine("Total Previously Decrypted Files : " + (object) Program.prevDecrypted);
          Console.WriteLine("Total Files to Decrypt : " + (object) Program.toDecryptCount);
          Console.WriteLine("Total Sorry Count : " + (object) Program.sorryDeleteCount);
          Console.WriteLine("Total Path Too Long : " + (object) Program.filePathTooLongCount);
          File.AppendAllText(Program.logPath, Environment.NewLine + "Stats : Total Scanned Files : " + (object) Program.totalFiles + ", Total Encrypted Files : " + (object) Program.encryptedFilesCount + ", Total Previously Decrypted Files : " + (object) Program.prevDecrypted + ", Total Files to Decrypt : " + (object) Program.toDecryptCount + ", Total Sorry Count : " + (object) Program.sorryDeleteCount + ", Total Path Too Long : " + (object) Program.filePathTooLongCount + Environment.NewLine);
        }
        if (args[0] == "-e")
        {
          Program.privkey = File.ReadAllText(args[1]);
          File.AppendAllText(Program.logPath, "---------------------------------------" + Environment.NewLine);
          Program.decOnly(args[2]);
          Console.WriteLine("-- Done --");
          Console.WriteLine("-----------------------------");
          Console.WriteLine("Information");
          Console.WriteLine("Total Scanned Files : " + (object) Program.totalFiles);
          Console.WriteLine("Total Encrypted Files : " + (object) Program.encryptedFilesCount);
          Console.WriteLine("Total Previously Decrypted Files : " + (object) Program.prevDecrypted);
          Console.WriteLine("Total Decrypted Files : " + (object) Program.decryptedFilesCount);
          Console.WriteLine("Total Deleted Files (does not include sorry files) : " + (object) Program.deletedFiles);
          Console.WriteLine("Total Deleted Sorry Files : " + (object) Program.sorryDeleteCount);
          Console.WriteLine("Total Path Too Long : " + (object) Program.filePathTooLongCount);
          File.AppendAllText(Program.logPath, Environment.NewLine + "Stats : Total Scanned Files : " + (object) Program.totalFiles + ", Total Encrypted Files : " + (object) Program.encryptedFilesCount + ", Total Previously Decrypted Files : " + (object) Program.prevDecrypted + ", Total Decrypted Files : " + (object) Program.decryptedFilesCount + ", Total Deleted Files (does not include sorry files) : " + (object) Program.deletedFiles + ", Total Deleted Sorry Files : " + (object) Program.sorryDeleteCount + ", Total Path Too Long : " + (object) Program.filePathTooLongCount + Environment.NewLine);
        }
        if (args[0] == "-c")
        {
          Console.WriteLine("Counting Files");
          Program.countFiles(args[1]);
          Console.WriteLine("Total Number of Files : " + (object) Program.totalFiles);
        }
      }
      stopwatch.Stop();
      TimeSpan elapsed = stopwatch.Elapsed;
      Console.WriteLine("RunTime " + string.Format("{0:00}:{1:00}:{2:00}.{3:00}", new object[4]
      {
        (object) elapsed.Hours,
        (object) elapsed.Minutes,
        (object) elapsed.Seconds,
        (object) (elapsed.Milliseconds / 10)
      }));
    }

    private static void deleteRec(string path)
    {
      try
      {
        Directory.GetAccessControl(path);
        foreach (string enumerateFile in Directory.EnumerateFiles(path))
        {
          ++Program.totalFiles;
          FileInfo f = new FileInfo(enumerateFile);
          if (f.FullName.Length > 260)
          {
            Console.WriteLine("ERROR (file too long) : " + f.FullName);
            File.AppendAllText(Program.logPath, "ERROR (file too long) : " + f.FullName + Environment.NewLine);
          }
          else
          {
            if (f.Extension == Program.enc_ext)
            {
              ++Program.encryptedFilesCount;
              if (Program.verifyDecryption(path, f))
              {
                File.AppendAllText(Program.logPath, "Previously Decrypted : " + f.FullName + Environment.NewLine);
                ++Program.prevDecrypted;
                try
                {
                  File.AppendAllText(Program.logPath, "FILE DELETED : " + f.FullName + Environment.NewLine);
                  File.Delete(f.FullName);
                  Console.WriteLine("Deleted : " + f.FullName);
                  ++Program.deletedFiles;
                }
                catch (Exception ex)
                {
                  Console.WriteLine("ERROR (couldn't delete) : " + f.FullName);
                  File.AppendAllText(Program.logPath, "DELETE ERROR : " + f.FullName + Environment.NewLine);
                }
              }
              else if (Program.decryptFile(f.FullName))
              {
                try
                {
                  File.AppendAllText(Program.logPath, "FILE DELETED : " + f.FullName + Environment.NewLine);
                  File.Delete(f.FullName);
                  Console.WriteLine("Deleted : " + f.FullName);
                  ++Program.deletedFiles;
                }
                catch (Exception ex)
                {
                  Console.WriteLine("ERROR (couldn't delete) : " + f.FullName);
                  File.AppendAllText(Program.logPath, "DELETE ERROR : " + f.FullName + Environment.NewLine);
                }
              }
              else
              {
                Console.WriteLine("ERROR (file not decrypted) : " + f.FullName);
                File.AppendAllText(Program.logPath, "ERROR (file not decrypted) : " + f.FullName + Environment.NewLine);
              }
            }
            if (f.Name.Contains("SORRY-FOR-FILES.html"))
            {
              File.AppendAllText(Program.logPath, "FILE DELETED : " + f.FullName + Environment.NewLine);
              File.Delete(f.FullName);
              ++Program.sorryDeleteCount;
            }
          }
        }
        foreach (string directory in Directory.GetDirectories(path))
        {
          if (directory.Length > 248)
          {
            Console.WriteLine("ERROR (directory too long) : " + directory);
            File.AppendAllText(Program.logPath, "ERROR (directory too long) : " + directory + Environment.NewLine);
          }
          else if (!(directory == "c:\\Users\\All Users") && !(directory == "c:\\Users\\Default"))
            Program.deleteRec(directory);
        }
      }
      catch (UnauthorizedAccessException ex)
      {
        File.AppendAllText(Program.logPath, "ERROR (UnauthorizedAccessException) : " + path + Environment.NewLine);
      }
      catch (FileNotFoundException ex)
      {
        File.AppendAllText(Program.logPath, "ERROR (FileNotFoundException) : " + path + Environment.NewLine);
      }
      catch (DirectoryNotFoundException ex)
      {
        File.AppendAllText(Program.logPath, "ERROR (DirectoryNotFoundException) : " + path + Environment.NewLine);
      }
    }

    private static void analysisRec(string path)
    {
      try
      {
        Directory.GetAccessControl(path);
        foreach (string enumerateFile in Directory.EnumerateFiles(path))
        {
          FileInfo f = new FileInfo(enumerateFile);
          ++Program.totalFiles;
          if (f.FullName.Length > 260)
          {
            ++Program.filePathTooLongCount;
            File.AppendAllText("c:\\logs\\filepathtoolong.txt", f.FullName + Environment.NewLine);
          }
          if (f.Extension == Program.enc_ext)
          {
            ++Program.encryptedFilesCount;
            File.AppendAllText(Program.encryptedFiles, f.FullName + Environment.NewLine);
            if (Program.verifyDecryption(path, f))
            {
              ++Program.prevDecrypted;
              File.AppendAllText("c:\\logs\\previouslydecrypted.txt", "File Previously Decrypted : " + f.FullName + Environment.NewLine);
            }
            else
            {
              ++Program.toDecryptCount;
              File.AppendAllText("c:\\logs\\tobedecrypted.txt", "File To-Be Decrypted : " + f.FullName + Environment.NewLine);
            }
          }
          if (f.Name.Contains("SORRY-FOR-FILES.html"))
            ++Program.sorryDeleteCount;
        }
        foreach (string enumerateDirectory in Directory.EnumerateDirectories(path))
        {
          if (enumerateDirectory.Length > 248)
          {
            Console.WriteLine("ERROR (directory too long) : " + enumerateDirectory);
            File.AppendAllText("c:\\logs\\directorytoolong.txt", "ERROR (directory too long) : " + enumerateDirectory + Environment.NewLine);
          }
          else if ((!enumerateDirectory.ToLower().StartsWith("c:\\users\\") || !enumerateDirectory.ToLower().Contains("\\appdata\\local\\application data")) && !enumerateDirectory.ToLower().Contains("\\local settings\\application data"))
            Program.analysisRec(enumerateDirectory);
        }
      }
      catch (Exception ex)
      {
      }
    }

    private static void decOnly(string path)
    {
      try
      {
        Directory.GetAccessControl(path);
        foreach (string enumerateFile in Directory.EnumerateFiles(path))
        {
          ++Program.totalFiles;
          FileInfo f = new FileInfo(enumerateFile);
          if (f.FullName.Length > 260)
          {
            Console.WriteLine("ERROR (file too long) : " + f.FullName);
            File.AppendAllText(Program.logPath, "ERROR (file too long) : " + f.FullName + Environment.NewLine);
          }
          else
          {
            if (f.Extension == Program.enc_ext)
            {
              ++Program.encryptedFilesCount;
              if (Program.verifyDecryption(path, f))
              {
                File.AppendAllText(Program.logPath, "Previously Decrypted : " + f.FullName + Environment.NewLine);
                ++Program.prevDecrypted;
              }
              else if (Program.decryptFile(f.FullName))
              {
                Console.WriteLine("File Decrypted : ", (object) f.FullName);
                File.AppendAllText(Program.decryptedFiles, f.FullName + Environment.NewLine);
              }
              else
              {
                Console.WriteLine("ERROR (file not decrypted) : " + f.FullName);
                File.AppendAllText(Program.logPath, "ERROR (file not decrypted) : " + f.FullName + Environment.NewLine);
              }
            }
            if (f.Name.Contains("SORRY-FOR-FILES.html"))
            {
              File.AppendAllText(Program.logPath, "FILE DELETED : " + f.FullName + Environment.NewLine);
              File.Delete(f.FullName);
              ++Program.sorryDeleteCount;
            }
          }
        }
        foreach (string directory in Directory.GetDirectories(path))
        {
          if (directory.Length > 248)
          {
            Console.WriteLine("ERROR (directory too long) : " + directory);
            File.AppendAllText(Program.logPath, "ERROR (directory too long) : " + directory + Environment.NewLine);
          }
          else if (!(directory == "c:\\Users\\All Users") && !(directory == "c:\\Users\\Default"))
            Program.decOnly(directory);
        }
      }
      catch (UnauthorizedAccessException ex)
      {
        File.AppendAllText(Program.logPath, "ERROR (UnauthorizedAccessException) : " + path + Environment.NewLine);
      }
      catch (FileNotFoundException ex)
      {
        File.AppendAllText(Program.logPath, "ERROR (FileNotFoundException) : " + path + Environment.NewLine);
      }
      catch (DirectoryNotFoundException ex)
      {
        File.AppendAllText(Program.logPath, "ERROR (DirectoryNotFoundException) : " + path + Environment.NewLine);
      }
    }

    private static void countFiles(string path)
    {
      try
      {
        Directory.GetAccessControl(path);
        foreach (string enumerateFile in Directory.EnumerateFiles(path))
          ++Program.totalFiles;
        foreach (string enumerateDirectory in Directory.EnumerateDirectories(path))
        {
          ++Program.totalSubDirectories;
          Program.countFiles(enumerateDirectory);
        }
      }
      catch (Exception ex)
      {
      }
    }

    private static bool verifyDecryption(string path, FileInfo f)
    {
      FileInfo fileInfo = new FileInfo(f.FullName.Substring(0, f.FullName.Length - 12));
      foreach (string file in Directory.GetFiles(path))
      {
        if (new FileInfo(file).Name == fileInfo.Name)
          return true;
      }
      return false;
    }

    private static bool decryptFile(string encryptedFilePath)
    {
      FileInfo fileInfo = new FileInfo(encryptedFilePath);
      string str = Program.MakePath(encryptedFilePath, "");
      try
      {
        string s1 = "";
        string s2 = "";
        long lOrgFileSize = 0;
        string stringFromBytes = Encipher.GetStringFromBytes(Encipher.GetHeaderBytesFromFile(encryptedFilePath));
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(stringFromBytes);
        foreach (XmlNode xmlNode in xmlDocument.GetElementsByTagName("AAA"))
          s1 = xmlNode.InnerText;
        foreach (XmlNode xmlNode in xmlDocument.GetElementsByTagName("AA"))
          s2 = xmlNode.InnerText;
        foreach (XmlNode xmlNode in xmlDocument.GetElementsByTagName("AAAAAAAAAAAAAAAAAA"))
          lOrgFileSize = Convert.ToInt64(xmlNode.InnerText);
        byte[] key = Encipher.RSADescryptBytes(Convert.FromBase64String(s1), Program.privkey);
        byte[] iv = Encipher.RSADescryptBytes(Convert.FromBase64String(s2), Program.privkey);
        Encipher.DecryptFile(encryptedFilePath, str, key, iv, lOrgFileSize);
        FileInfo f = new FileInfo(encryptedFilePath);
        return Program.verifyDecryption(f.Directory.ToString(), f);
      }
      catch (FormatException ex)
      {
        Console.WriteLine("\r\n[-] Decryption key is not correct -> " + encryptedFilePath + ex.Message);
        File.AppendAllText(Program.logPath, "ERROR (key not correct) : " + encryptedFilePath + ex.Message + Environment.NewLine);
        if (!File.Exists(str))
          return false;
        File.Delete(str);
      }
      catch (XmlException ex)
      {
        Console.WriteLine("\r\n[-] Encrypted data is not correct -> " + encryptedFilePath + ex.Message);
        File.AppendAllText(Program.logPath, "ERROR (data not correct) : " + encryptedFilePath + ex.Message + Environment.NewLine);
        if (!File.Exists(str))
          return false;
        File.Delete(str);
      }
      return false;
    }

    private static string MakePath(string plainFilePath, string newSuffix)
    {
      string path2 = Path.GetFileNameWithoutExtension(plainFilePath) + newSuffix;
      return Path.Combine(Path.GetDirectoryName(plainFilePath), path2);
    }

    private static bool hasWriteAccessToFolder(string folderPath)
    {
      try
      {
        Directory.GetAccessControl(folderPath);
        return true;
      }
      catch (UnauthorizedAccessException ex)
      {
        return false;
      }
    }
  }
}
