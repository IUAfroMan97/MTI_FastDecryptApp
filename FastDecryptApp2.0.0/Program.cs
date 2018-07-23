// Decompiled with JetBrains decompiler
// Type: FastDecryptApp.Program
// Assembly: FastDecryptApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 707BCE7B-2368-4313-AB42-2091C3748DE7
// Assembly location: C:\Users\jgood\Desktop\FastDecryptApp2.0.0.exe

using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Tools;

namespace FastDecryptApp
{
  internal class Program
  {
    private static string enc_ext = ".weapologize";
    private static string privkey = "";
    private static string logPath = "C:\\logs\\fastdecryptlog.txt";
    private static string decryptedFiles = "C:\\logs\\decryptedFiles.txt";
    private static string encryptedFiles = "C:\\logs\\encryptedFiles.txt";
    private static string logPreviouslyDecrypted = "C:\\logs\\previouslydecrypted.txt";
    private static string logFileDeleted = "C:\\logs\\deletedFiles.txt";
    private static string logFileNameTooLong = "C:\\logs\\filenametoolong.txt";
    private static string logFilePathTooLong = "C:\\logs\\filepathtoolong.txt";
    private static string logTooBeDecrypted = "C:\\logs\\tobedecrypted.txt";
    private static int totalFiles = 0;
    private static long totalSize = 0;
    private static int totalDirectories = 0;
    private static int totalEncryptedFiles = 0;
    private static int totalSorryFiles = 0;
    private static long sizeBeforeEncryption = 0;
    private static long sizeAddedWithEncryption = 0;
    private static int previouslyDecrypted = 0;
    private static int filesToDecrypt = 0;
    private static int decrypted = 0;
    private static int deleted = 0;
    private static long totalSizeAfterDecryption = 0;
    private static long sizeSaved = 0;
    private static long sizeOfSorryFiles = 0;
    private static int dirUnaccessable = 0;
    private static int fileNameTooLong = 0;
    private static int filePathTooLong = 0;

    private static void displayStats(string time)
    {
      Console.WriteLine("\n---------- Display Stats ----------");
      Console.WriteLine("# General Information # ");
      Console.WriteLine("   - Total number of files decrypted : {0}", (object) Program.totalFiles);
      Console.WriteLine("   - Total size of all files : {0}", (object) Program.FormatBytes(Program.totalSize));
      Console.WriteLine("   - Total number of directories : {0}", (object) Program.totalDirectories);
      Console.WriteLine("   - Time to proccess : {0}", (object) time);
      Console.WriteLine("   - Date ran : {0}", (object) DateTime.Now);
      Console.WriteLine("\n# Affected Files #");
      Console.WriteLine("   - Total number of encrypted files : {0}", (object) Program.totalEncryptedFiles);
      Console.WriteLine("   - Total number of sorry files : {0}", (object) Program.totalSorryFiles);
      Console.WriteLine("   - Size before encryption : {0}", (object) Program.FormatBytes(Program.sizeBeforeEncryption));
      Console.WriteLine("   - Size added by encryption : {0}", (object) Program.FormatBytes(Program.sizeAddedWithEncryption));
      Console.WriteLine("\n#  Remediation #");
      Console.WriteLine("   - Files previously decrypted : {0}", (object) Program.previouslyDecrypted);
      Console.WriteLine("   - Files to decrypt : {0}", (object) Program.filesToDecrypt);
      Console.WriteLine("   - Files decrypted : {0}", (object) Program.decrypted);
      Console.WriteLine("   - Files deleted : {0}", (object) Program.deleted);
      Console.WriteLine("   - Size after decryption : {0}", (object) Program.FormatBytes(Program.totalSizeAfterDecryption));
      Console.WriteLine("   - Size saved by decryption : {0}", (object) Program.FormatBytes(Program.sizeSaved));
      Console.WriteLine("   - Size of sorry files : {0}", (object) Program.FormatBytes(Program.sizeOfSorryFiles));
      Console.WriteLine("\n# Errors #");
      Console.WriteLine("   - File name too long : {0}", (object) Program.fileNameTooLong);
      Console.WriteLine("   - File path too long : {0}", (object) Program.filePathTooLong);
      Console.WriteLine("   - Directory unaccessable : {0}", (object) Program.dirUnaccessable);
      Console.WriteLine("\n-----------------------------------");
    }

    private static void writeStatsToLog(string time)
    {
      string contents = Environment.NewLine + "----------Display Stats----------" + Environment.NewLine + "# General Information # " + Environment.NewLine + "   - Total number of files decrypted : " + (object) Program.totalFiles + Environment.NewLine + "   - Total size of all files : " + Program.FormatBytes(Program.totalSize) + Environment.NewLine + "   - Total number of directories : " + (object) Program.totalDirectories + Environment.NewLine + "   - Time to proccess : " + time + Environment.NewLine + "   - Date ran : " + (object) DateTime.Now + Environment.NewLine + Environment.NewLine + "# Affected Files #" + Environment.NewLine + "   - Total number of encrypted files : " + (object) Program.totalEncryptedFiles + Environment.NewLine + "   - Total number of sorry files : " + (object) Program.totalSorryFiles + Environment.NewLine + "   - Size before encryption : " + Program.FormatBytes(Program.sizeBeforeEncryption) + Environment.NewLine + "   - Size added by encryption : " + Program.FormatBytes(Program.sizeAddedWithEncryption) + Environment.NewLine + Environment.NewLine + "#  Remediation #" + Environment.NewLine + "   - Files previously decrypted : " + (object) Program.previouslyDecrypted + Environment.NewLine + "   - Files to decrypt : " + (object) Program.filesToDecrypt + Environment.NewLine + "   - Files decrypted : " + (object) Program.decrypted + Environment.NewLine + "   - Files deleted : " + (object) Program.deleted + Environment.NewLine + "   - Size after decryption : " + Program.FormatBytes(Program.totalSizeAfterDecryption) + Environment.NewLine + "   - Size saved by decryption : " + Program.FormatBytes(Program.sizeSaved) + Environment.NewLine + "   - Size of sorry files : " + Program.FormatBytes(Program.sizeOfSorryFiles) + Environment.NewLine + Environment.NewLine + "# Errors #" + Environment.NewLine + "   - File name too long : " + (object) Program.fileNameTooLong + Environment.NewLine + "   - File path too long : " + (object) Program.filePathTooLong + Environment.NewLine + "   - Directory unaccessable : " + (object) Program.dirUnaccessable + Environment.NewLine + Environment.NewLine + "-----------------------------------" + Environment.NewLine;
      File.AppendAllText(Program.logPath, contents);
    }

    private static string FormatBytes(long bytes)
    {
      string[] strArray = new string[5]
      {
        "B",
        "KB",
        "MB",
        "GB",
        "TB"
      };
      double num = (double) bytes;
      int index = 0;
      while (index < strArray.Length && bytes >= 1024L)
      {
        num = (double) bytes / 1024.0;
        ++index;
        bytes /= 1024L;
      }
      return string.Format("{0:0.##} {1}", (object) num, (object) strArray[index]);
    }

    private static void Main(string[] args)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      Directory.CreateDirectory("C:\\logs");
      if (args.Length < 1)
        return;
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
        Program.privkey = Program.findKey();
        File.AppendAllText(Program.logPath, Environment.NewLine + "---------------------------------------" + Environment.NewLine);
        Program.deleteRec(args[1]);
        Console.WriteLine("\n-- Done --");
        Program.sizeBeforeEncryption = Program.totalSize - Program.sizeAddedWithEncryption;
        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;
        string time = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", new object[4]
        {
          (object) elapsed.Hours,
          (object) elapsed.Minutes,
          (object) elapsed.Seconds,
          (object) (elapsed.Milliseconds / 10)
        });
        Program.displayStats(time);
        Program.writeStatsToLog(time);
      }
      if (args[0] == "-a")
      {
        File.AppendAllText(Program.logPath, Environment.NewLine + "---------------------------------------" + Environment.NewLine);
        Program.analysisRec(args[1]);
        Console.WriteLine("\n-- Done --");
        Program.sizeBeforeEncryption = Program.totalSize - Program.sizeAddedWithEncryption;
        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;
        string time = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", new object[4]
        {
          (object) elapsed.Hours,
          (object) elapsed.Minutes,
          (object) elapsed.Seconds,
          (object) (elapsed.Milliseconds / 10)
        });
        Program.displayStats(time);
        Program.writeStatsToLog(time);
      }
      if (args[0] == "-e")
      {
        Program.privkey = Program.findKey();
        File.AppendAllText(Program.logPath, Environment.NewLine + "---------------------------------------" + Environment.NewLine);
        Program.decOnly(args[1]);
        Console.WriteLine("\n-- Done --");
        Program.sizeBeforeEncryption = Program.totalSize - Program.sizeAddedWithEncryption;
        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;
        string time = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", new object[4]
        {
          (object) elapsed.Hours,
          (object) elapsed.Minutes,
          (object) elapsed.Seconds,
          (object) (elapsed.Milliseconds / 10)
        });
        Program.displayStats(time);
        Program.writeStatsToLog(time);
      }
      if (args[0] == "-c")
      {
        Console.WriteLine("Counting Files");
        Program.countFiles(args[1]);
        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;
        string time = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", new object[4]
        {
          (object) elapsed.Hours,
          (object) elapsed.Minutes,
          (object) elapsed.Seconds,
          (object) (elapsed.Milliseconds / 10)
        });
        Program.displayStats(time);
        Program.writeStatsToLog(time);
        Console.WriteLine("Runtime : {0}", (object) time);
      }
      if (args[0] == "-j")
      {
        Console.WriteLine("Here");
        Program.findKey();
      }
    }

    private static string findKey()
    {
      string path = "\\\\site.domain\\path\\to\\key\\dir";
      string str = Environment.MachineName.ToUpper() + "_PrivateKey.keyxml";
      using (new Impersonator("domainuser", "domain", "password"))
      {
        foreach (string file in Directory.GetFiles(path))
        {
          if (file.Contains(str))
          {
            Console.WriteLine("Found Key : " + file);
            return file;
          }
        }
      }
      Console.WriteLine("Could not find key for " + Environment.MachineName);
      return "";
    }

    private static void deleteRec(string path)
    {
      try
      {
        Directory.GetAccessControl(path);
        string directoryRoot = Directory.GetDirectoryRoot(path);
        ++Program.totalDirectories;
        foreach (string enumerateFile in Directory.EnumerateFiles(path))
        {
          ++Program.totalFiles;
          FileInfo fileInfo = new FileInfo(enumerateFile);
          Program.totalSize += fileInfo.Length;
          if (fileInfo.FullName.Length > 260)
          {
            Console.WriteLine("ERROR (file name too long) : " + fileInfo.FullName);
            ++Program.fileNameTooLong;
            File.AppendAllText(Program.logFileNameTooLong, fileInfo.FullName + Environment.NewLine);
          }
          else
          {
            if (fileInfo.Extension == Program.enc_ext)
            {
              ++Program.totalEncryptedFiles;
              Program.sizeAddedWithEncryption += 3072L;
              if (Program.verifyDecryption(path, fileInfo))
                Program.deleteFile(fileInfo);
              else if (Program.decryptFile(fileInfo.FullName))
              {
                Program.deleteFile(fileInfo);
              }
              else
              {
                Console.WriteLine("ERROR (file not decrypted) : " + fileInfo.FullName);
                File.AppendAllText(Program.logPath, "ERROR (file not decrypted) : " + fileInfo.FullName + Environment.NewLine);
              }
            }
            if (fileInfo.Name.Contains("SORRY-FOR-FILES.html"))
            {
              Program.deleteFile(fileInfo);
              ++Program.totalSorryFiles;
              Program.sizeOfSorryFiles += fileInfo.Length;
            }
          }
        }
        foreach (string enumerateDirectory in Directory.EnumerateDirectories(path))
        {
          try
          {
            if (enumerateDirectory.Length > 248)
            {
              Console.WriteLine("ERROR (directory too long) : " + enumerateDirectory);
              File.AppendAllText(Program.logFilePathTooLong, "ERROR (directory too long) : " + enumerateDirectory + Environment.NewLine);
              ++Program.filePathTooLong;
            }
            else if ((!enumerateDirectory.ToLower().StartsWith(directoryRoot + "\\users\\") || !enumerateDirectory.ToLower().Contains("\\appdata\\local\\application data")) && !enumerateDirectory.ToLower().Contains("\\local settings\\application data"))
            {
              if (!(enumerateDirectory == directoryRoot + "\\Users\\All Users"))
              {
                if (!(enumerateDirectory == directoryRoot + "\\Users\\Default"))
                {
                  if (!(enumerateDirectory == directoryRoot + "\\Users\\Default User"))
                    Program.deleteRec(enumerateDirectory);
                }
              }
            }
          }
          catch (Exception ex)
          {
          }
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
        string directoryRoot = Directory.GetDirectoryRoot(path);
        ++Program.totalDirectories;
        foreach (string enumerateFile in Directory.EnumerateFiles(path))
        {
          FileInfo f = new FileInfo(enumerateFile);
          ++Program.totalFiles;
          Program.totalSize += f.Length;
          if (f.FullName.Length > 260)
          {
            Console.WriteLine("ERROR (file name too long) : " + f.FullName);
            ++Program.fileNameTooLong;
            File.AppendAllText(Program.logFileNameTooLong, f.FullName + Environment.NewLine);
          }
          else
          {
            if (f.Extension == Program.enc_ext)
            {
              ++Program.totalEncryptedFiles;
              Program.sizeAddedWithEncryption += 3072L;
              File.AppendAllText(Program.encryptedFiles, f.FullName + Environment.NewLine);
              if (!Program.verifyDecryption(path, f))
              {
                ++Program.filesToDecrypt;
                File.AppendAllText(Program.logTooBeDecrypted, f.FullName + Environment.NewLine);
              }
            }
            if (f.Name.Contains("SORRY-FOR-FILES.html"))
            {
              ++Program.totalSorryFiles;
              Program.sizeOfSorryFiles += f.Length;
            }
          }
        }
        foreach (string enumerateDirectory in Directory.EnumerateDirectories(path))
        {
          try
          {
            if (enumerateDirectory.Length > 248)
            {
              Console.WriteLine("ERROR (directory too long) : " + enumerateDirectory);
              File.AppendAllText(Program.logFilePathTooLong, enumerateDirectory + Environment.NewLine);
              ++Program.filePathTooLong;
            }
            else if ((!enumerateDirectory.ToLower().StartsWith(directoryRoot + "\\users\\") || !enumerateDirectory.ToLower().Contains("\\appdata\\local\\application data")) && !enumerateDirectory.ToLower().Contains("\\local settings\\application data"))
            {
              if (!(enumerateDirectory == directoryRoot + "\\Users\\All Users"))
              {
                if (!(enumerateDirectory == directoryRoot + "\\Users\\Default"))
                {
                  if (!(enumerateDirectory == directoryRoot + "\\Users\\Default User"))
                    Program.analysisRec(enumerateDirectory);
                }
              }
            }
          }
          catch (Exception ex)
          {
          }
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
        string directoryRoot = Directory.GetDirectoryRoot(path);
        ++Program.totalDirectories;
        foreach (string enumerateFile in Directory.EnumerateFiles(path))
        {
          ++Program.totalFiles;
          FileInfo fileInfo = new FileInfo(enumerateFile);
          Program.totalSize += fileInfo.Length;
          if (fileInfo.FullName.Length > 260)
          {
            Console.WriteLine("ERROR (file name too long) : " + fileInfo.FullName);
            ++Program.fileNameTooLong;
            File.AppendAllText(Program.logFileNameTooLong, fileInfo.FullName + Environment.NewLine);
          }
          else
          {
            if (fileInfo.Extension == Program.enc_ext)
            {
              ++Program.totalEncryptedFiles;
              Program.sizeAddedWithEncryption += 3072L;
              if (!Program.verifyDecryption(path, fileInfo) && !Program.decryptFile(fileInfo.FullName))
              {
                Console.WriteLine("ERROR (file not decrypted) : " + fileInfo.FullName);
                File.AppendAllText(Program.logPath, "ERROR (file not decrypted) : " + fileInfo.FullName + Environment.NewLine);
              }
            }
            if (fileInfo.Name.Contains("SORRY-FOR-FILES.html"))
            {
              Program.deleteFile(fileInfo);
              ++Program.totalSorryFiles;
              Program.sizeOfSorryFiles += fileInfo.Length;
            }
          }
        }
        foreach (string directory in Directory.GetDirectories(path))
        {
          try
          {
            if (directory.Length > 248)
            {
              Console.WriteLine("ERROR (directory too long) : " + directory);
              File.AppendAllText(Program.logFilePathTooLong, directory + Environment.NewLine);
              ++Program.filePathTooLong;
            }
            else if ((!directory.ToLower().StartsWith(directoryRoot + "\\users\\") || !directory.ToLower().Contains("\\appdata\\local\\application data")) && !directory.ToLower().Contains("\\local settings\\application data"))
            {
              if (!(directory == directoryRoot + "\\Users\\All Users"))
              {
                if (!(directory == directoryRoot + "\\Users\\Default"))
                {
                  if (!(directory == directoryRoot + "\\Users\\Default User"))
                    Program.decOnly(directory);
                }
              }
            }
          }
          catch (Exception ex)
          {
          }
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
        string directoryRoot = Directory.GetDirectoryRoot(path);
        foreach (string enumerateFile in Directory.EnumerateFiles(path))
          ++Program.totalFiles;
        foreach (string enumerateDirectory in Directory.EnumerateDirectories(path))
        {
          ++Program.totalDirectories;
          try
          {
            if (enumerateDirectory.Length <= 248)
            {
              if ((!enumerateDirectory.ToLower().StartsWith(directoryRoot + "\\users\\") || !enumerateDirectory.ToLower().Contains("\\appdata\\local\\application data")) && !enumerateDirectory.ToLower().Contains("\\local settings\\application data"))
              {
                if (!(enumerateDirectory == directoryRoot + "\\Users\\All Users"))
                {
                  if (!(enumerateDirectory == directoryRoot + "\\Users\\Default"))
                  {
                    if (enumerateDirectory == directoryRoot + "\\Users\\Default User")
                      continue;
                  }
                  else
                    continue;
                }
                else
                  continue;
              }
              else
                continue;
            }
            else
              continue;
          }
          catch (Exception ex)
          {
          }
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
        {
          Console.WriteLine("File Previously Decrypted : " + f.FullName);
          File.AppendAllText(Program.logPreviouslyDecrypted, f.FullName + Environment.NewLine);
          ++Program.previouslyDecrypted;
          return true;
        }
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
        if (!Program.verifyDecryption(f.Directory.ToString(), f))
          return false;
        Console.WriteLine("File Decrypted : " + encryptedFilePath);
        File.AppendAllText(Program.decryptedFiles, encryptedFilePath + Environment.NewLine);
        ++Program.decrypted;
        return true;
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

    private static bool deleteFile(FileInfo file)
    {
      try
      {
        if (file.Name.Contains("SORRY-FOR-FILES.html"))
          --Program.deleted;
        Console.WriteLine("Deleted : " + file.FullName);
        File.Delete(file.FullName);
        File.AppendAllText(Program.logFileDeleted, file.FullName + Environment.NewLine);
        ++Program.deleted;
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine("ERROR (couldn't delete) : " + file.FullName);
        File.AppendAllText(Program.logPath, "DELETE ERROR : " + file.FullName + Environment.NewLine);
        return false;
      }
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
