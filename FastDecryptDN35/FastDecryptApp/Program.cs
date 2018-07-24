using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Xml;
using System.Threading;

// Modification History
//DLW  07/20/18  Converted to .Net 3.5 (for Windows Server 2003 machines)
//DLW  07/24/18  Added file number to display of "Deleted" for tracking progress

namespace FastDecryptApp
{
    class Program
    {

        private static string enc_ext = ".weapologize";
        // private static string selfname = Process.GetCurrentProcess().ProcessName + ".exe";
        private static string privkey = "";
        private static string logPath = @"C:\logs\fastdecryptlog.txt";
        private static string decryptedFiles = @"C:\logs\decryptedFiles.txt";
        private static string encryptedFiles = @"C:\logs\encryptedFiles.txt";

        // stats

        private static int totalFiles = 0;
        private static double totalSize = 0.0;
        private static int totalDirectories = 0;

        private static int totalEncryptedFiles = 0;
        private static int totalSorryFiles = 0;
        private static double sizeBeforeEncryption = 0.0;
        private static double sizeAddedWithEncryption = 0.0;

        private static int previouslyDecrypted = 0;
        private static int filesToDecrypt = 0;
        private static int decrypted = 0;
        private static int deleted = 0;
        private static double totalSizeAfterDecryption = 0.0;
        private static double sizeSaved = 0.0;
        private static double sizeOfSorryFiles = 0.0;

        private static int dirUnaccessable = 0;
        private static int fileNameTooLong = 0;
        private static int filePathTooLong = 0;

        static void displayStats(string time)
        {
            Console.WriteLine("\n---------- Display Stats ----------");
            Console.WriteLine("# General Information # ");
            Console.WriteLine("   - Total number of files decrypted : {0}", totalFiles);
            Console.WriteLine("   - Total size of all files : {0}", totalSize);
            Console.WriteLine("   - Total number of directories : {0}", totalDirectories);
            Console.WriteLine("   - Time to proccess : {0}", time);
            Console.WriteLine("\n# Affected Files #");
            Console.WriteLine("   - Total number of encrypted files : {0}", totalEncryptedFiles);
            Console.WriteLine("   - Total number of sorry files : {0}", totalSorryFiles);
            Console.WriteLine("   - Size before encryption : {0}", sizeBeforeEncryption);
            Console.WriteLine("   - Size added by encryption : {0}", sizeAddedWithEncryption);
            Console.WriteLine("\n#  Remediation #");
            Console.WriteLine("   - Files previously decrypted : {0}", previouslyDecrypted);
            Console.WriteLine("   - Files to decrypt : {0}", filesToDecrypt);
            Console.WriteLine("   - Files decrypted : {0}", decrypted);
            Console.WriteLine("   - Files deleted : {0}", deleted);
            Console.WriteLine("   - Size after decryption : {0}", totalSizeAfterDecryption);
            Console.WriteLine("   - Size saved by decryption : {0}", sizeSaved);
            Console.WriteLine("   - Size of sorry files : {0}", sizeOfSorryFiles);
            Console.WriteLine("\n# Errors #");
            Console.WriteLine("   - File name too long : {0}", fileNameTooLong);
            Console.WriteLine("   - File path too long : {0}", filePathTooLong);
            Console.WriteLine("   - Directory unaccessable : {0}", dirUnaccessable);
            Console.WriteLine("\n-----------------------------------");
        }

        static void writeStatsToLog(string time)
        {
            string stats = Environment.NewLine + "----------Display Stats----------" + Environment.NewLine +
                "# General Information # " + Environment.NewLine +
                "   - Total number of files decrypted : {totalFiles}" + Environment.NewLine +
                "   - Total size of all files : {totalSize}" + Environment.NewLine +
                "   - Total number of directories : {totalDirectories}" + Environment.NewLine +
                "   - Time to proccess : {time}" + Environment.NewLine +
                Environment.NewLine + "# Affected Files #" + Environment.NewLine +
                "   - Total number of encrypted files : {totalEncryptedFiles}" + Environment.NewLine +
                "   - Total number of sorry files : {totalSorryFiles}" + Environment.NewLine +
                "   - Size before encryption : {sizeBeforeEncryption}" + Environment.NewLine +
                "   - Size added by encryption : {sizeAddedWithEncryption}" + Environment.NewLine +
                Environment.NewLine + "#  Remediation #" + Environment.NewLine +
                "   - Files previously decrypted : {previouslyDecrypted}" + Environment.NewLine +
                "   - Files to decrypt : {filesToDecrypt}" + Environment.NewLine +
                "   - Files decrypted : {decrypted}" + Environment.NewLine +
                "   - Files deleted : {deleted}" + Environment.NewLine +
                "   - Size after decryption : {totalSizeAfterDecryption}" + Environment.NewLine +
                "   - Size saved by decryption : {sizeSaved}" + Environment.NewLine +
                "   - Size of sorry files : {sizeOfSorryFiles}" + Environment.NewLine +
                Environment.NewLine + "# Errors #" + Environment.NewLine +
                "   - File name too long : {fileNameTooLong}" + Environment.NewLine +
                "   - File path too long : {filePathTooLong}" + Environment.NewLine +
                "   - Directory unaccessable : {dirUnaccessable}" + Environment.NewLine +
                Environment.NewLine + "-----------------------------------" + Environment.NewLine;
            File.AppendAllText(logPath, $"" + stats);
        }





        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Directory.CreateDirectory(@"C:\logs");

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
                            if (numArray.Length != 0)
                                Console.WriteLine("\r\nCORRECT KEY IS:" + file);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                if (args[0] == "-d")
                {
                    Program.privkey = File.ReadAllText(args[1]);
                    File.AppendAllText(logPath, Environment.NewLine + "---------------------------------------" + Environment.NewLine);
                    deleteRec(args[2]);
                    Console.WriteLine("\n-- Done --");

                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);

                    Program.displayStats(elapsedTime);
                    Program.writeStatsToLog(elapsedTime);
                }
                if (args[0] == "-a")
                {
                    File.AppendAllText(logPath, Environment.NewLine + "---------------------------------------" + Environment.NewLine);
                    Program.analysisRec(args[1]);

                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);

                    Program.displayStats(elapsedTime);
                    Program.writeStatsToLog(elapsedTime);
                }
                if (args[0] == "-e")
                {
                    Program.privkey = File.ReadAllText(args[1]);
                    File.AppendAllText(logPath, Environment.NewLine + "---------------------------------------" + Environment.NewLine);
                    decOnly(args[2]);

                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);

                    Program.displayStats(elapsedTime);
                    Program.writeStatsToLog(elapsedTime);
                }
                if (args[0] == "-c")
                {
                    Console.WriteLine("Counting Files");
                    Program.countFiles(args[1]);

                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);

                    Program.displayStats(elapsedTime);
                    Program.writeStatsToLog(elapsedTime);

                    Console.WriteLine("Runtime : {0}", elapsedTime);
                }
            }
            Console.ReadKey();
        }
        static void deleteRec(string path)
        {
            while (true)
            {
                try
                {
                    DirectorySecurity di = Directory.GetAccessControl(path);
                    foreach (string file in Directory.GetFiles(path))
                    {
                        Program.totalFiles++;
                        FileInfo f = new FileInfo(file);

                        if (f.FullName.Length > 260)
                        {
                            Console.WriteLine("ERROR (file too long) : " + f.FullName);
                            File.AppendAllText(logPath, "ERROR (file too long) : " + f.FullName + Environment.NewLine);
                            continue;
                        }

                        if (f.Extension == Program.enc_ext)
                        {
                            Program.totalEncryptedFiles++;
                            if (Program.verifyDecryption(path, f)) // check if file was decrypted previously
                            {
                                File.AppendAllText(logPath, "Previously Decrypted : " + f.FullName + Environment.NewLine);
                                Program.previouslyDecrypted++;
                                try
                                {
                                    File.AppendAllText(logPath, "FILE DELETED : " + f.FullName + Environment.NewLine);
                                    File.Delete(f.FullName);
                                    Console.WriteLine("Deleted # " + Program.totalFiles.ToString() + " :" + f.FullName);
                                    Program.deleted++;
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("ERROR (couldn't delete) : " + f.FullName);
                                    File.AppendAllText(logPath, "DELETE ERROR : " + f.FullName + Environment.NewLine);
                                }
                            }
                            else
                            {
                                if (Program.decryptFile(f.FullName))
                                {
                                    try
                                    {
                                        File.AppendAllText(logPath, "FILE DELETED : " + f.FullName + Environment.NewLine);
                                        File.Delete(f.FullName);
                                        Console.WriteLine("Deleted # " + Program.totalFiles.ToString() + " :" + f.FullName);
                                        Program.deleted++;
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("ERROR (couldn't delete) : " + f.FullName);
                                        File.AppendAllText(logPath, "DELETE ERROR : " + f.FullName + Environment.NewLine);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("ERROR (file not decrypted) : " + f.FullName);
                                    File.AppendAllText(logPath, "ERROR (file not decrypted) : " + f.FullName + Environment.NewLine);
                                }

                            }

                        }
                        if (f.Name.Contains("SORRY-FOR-FILES.html"))
                        {
                            File.AppendAllText(logPath, "FILE DELETED : " + f.FullName + Environment.NewLine);
                            File.Delete(f.FullName);
                            Program.totalSorryFiles++;
                        }
                    }
                    foreach (string directory in Directory.GetDirectories(path))
                    {
                        if (directory.Length > 248)
                        {
                            Console.WriteLine("ERROR (directory too long) : " + directory);
                            File.AppendAllText(logPath, "ERROR (directory too long) : " + directory + Environment.NewLine);
                            continue;
                        }
                        if (directory == @"c:\Users\All Users")
                        {
                            continue;
                        }
                        else if (directory == @"c:\Users\Default")
                        {
                            continue;
                        }
                        else if (directory == @"c:\Users\Default User")
                        {
                            continue;
                        }
                        Program.deleteRec(directory);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    File.AppendAllText(logPath, "ERROR (UnauthorizedAccessException) : " + path + Environment.NewLine);
                }
                catch (FileNotFoundException)
                {
                    File.AppendAllText(logPath, "ERROR (FileNotFoundException) : " + path + Environment.NewLine);
                }
                catch (DirectoryNotFoundException)
                {
                    File.AppendAllText(logPath, "ERROR (DirectoryNotFoundException) : " + path + Environment.NewLine);
                }
                return;

            }
        }

        static void analysisRec(string path)
        {
            while (true)
            {
                try
                {
                    DirectorySecurity ds = Directory.GetAccessControl(path);
                    string root = Directory.GetDirectoryRoot(path);
                    foreach (string currentFile in Directory.GetFiles(path))
                    {
                        FileInfo cf = new FileInfo(currentFile);
                        Program.totalFiles++;
                        

                        if (cf.FullName.Length > 260)
                        {
                            Program.fileNameTooLong++;
                            File.AppendAllText(@"c:\logs\filepathtoolong.txt", cf.FullName + Environment.NewLine);
                        }

                        if (cf.Extension == Program.enc_ext)
                        {
                            Program.totalEncryptedFiles++;
                            File.AppendAllText(encryptedFiles, cf.FullName + Environment.NewLine);
                            if (verifyDecryption(path, cf))
                            {
                                Program.previouslyDecrypted++;
                                File.AppendAllText(@"c:\logs\previouslydecrypted.txt", "File Previously Decrypted : " + cf.FullName + Environment.NewLine);
                            }
                            else
                            {
                                Program.filesToDecrypt++;
                                File.AppendAllText(@"c:\logs\tobedecrypted.txt", "File To-Be Decrypted : " + cf.FullName + Environment.NewLine);
                            }
                        }

                        if (cf.Name.Contains("SORRY-FOR-FILES.html"))
                        {
                            Program.totalSorryFiles++;
                        }
                    }

                    foreach (string subDirectory in Directory.GetDirectories(path))
                    {
                        try
                        {
                            if (subDirectory.Length > 248)
                            {
                                Console.WriteLine("ERROR (directory too long) : " + subDirectory);
                                File.AppendAllText(@"c:\logs\directorytoolong.txt", "ERROR (directory too long) : " + subDirectory + Environment.NewLine);
                                continue;
                            }
                            if (subDirectory.ToLower().StartsWith(root + "\\users\\") && subDirectory.ToLower().Contains("\\appdata\\local\\application data") || subDirectory.ToLower().Contains("\\local settings\\application data"))
                            {
                                continue;
                            }
                            if (subDirectory == root + "\\Users\\All Users")
                            {
                                continue;
                            }
                            else if (subDirectory == root + "\\Users\\Default")
                            {
                                continue;
                            }
                            else if (subDirectory == root + "\\Users\\Default User")
                            {
                                continue;
                            }
                            Program.analysisRec(subDirectory);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
                return;
            }
        }

        static void decOnly(string path)
        {
            while (true)
            {
                try
                {
                    DirectorySecurity di = Directory.GetAccessControl(path);
                    string root = Directory.GetDirectoryRoot(path);

                    foreach (string file in Directory.GetFiles(path))
                    {
                        Program.totalFiles++;
                        FileInfo f = new FileInfo(file);

                        if (f.FullName.Length > 260)
                        {
                            Console.WriteLine("ERROR (file too long) : " + f.FullName);
                            File.AppendAllText(logPath, "ERROR (file too long) : " + f.FullName + Environment.NewLine);
                            continue;
                        }

                        if (f.Extension == Program.enc_ext)
                        {
                            Program.totalEncryptedFiles++;
                            if (Program.verifyDecryption(path, f)) // check if file was decrypted previously
                            {
                                File.AppendAllText(logPath, "Previously Decrypted : " + f.FullName + Environment.NewLine);
                                Program.previouslyDecrypted++;
                            }
                            else
                            {
                                if (Program.decryptFile(f.FullName))
                                {
                                    Console.WriteLine("Decrypted # " + Program.totalFiles.ToString() + " :" + f.FullName);
                                    File.AppendAllText(decryptedFiles, f.FullName + Environment.NewLine);
                                }
                                else
                                {
                                    Console.WriteLine("ERROR (file not decrypted) : " + f.FullName);
                                    File.AppendAllText(logPath, "ERROR (file not decrypted) : " + f.FullName + Environment.NewLine);
                                }

                            }

                        }
                        if (f.Name.Contains("SORRY-FOR-FILES.html"))
                        {
                            File.AppendAllText(logPath, "FILE DELETED : " + f.FullName + Environment.NewLine);
                            File.Delete(f.FullName);
                            Program.totalSorryFiles++;
                        }
                    }
                    foreach (string subDirectory in Directory.GetDirectories(path))
                    {
                        try
                        {
                            if (subDirectory.Length > 248)
                            {
                                Console.WriteLine("ERROR (directory too long) : " + subDirectory);
                                File.AppendAllText(@"c:\logs\directorytoolong.txt", "ERROR (directory too long) : " + subDirectory + Environment.NewLine);
                                continue;
                            }
                            if (subDirectory.ToLower().StartsWith(root + "\\users\\") && subDirectory.ToLower().Contains("\\appdata\\local\\application data") || subDirectory.ToLower().Contains("\\local settings\\application data"))
                            {
                                continue;
                            }
                            if (subDirectory == root + "\\Users\\All Users")
                            {
                                continue;
                            }
                            else if (subDirectory == root + "\\Users\\Default")
                            {
                                continue;
                            }
                            else if (subDirectory == root + "\\Users\\Default User")
                            {
                                continue;
                            }
                            Program.decOnly(subDirectory);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    File.AppendAllText(logPath, "ERROR (UnauthorizedAccessException) : " + path + Environment.NewLine);
                }
                catch (FileNotFoundException)
                {
                    File.AppendAllText(logPath, "ERROR (FileNotFoundException) : " + path + Environment.NewLine);
                }
                catch (DirectoryNotFoundException)
                {
                    File.AppendAllText(logPath, "ERROR (DirectoryNotFoundException) : " + path + Environment.NewLine);
                }
                return;

            }
        }

        static void countFiles(string path)
        {
            while (true)
            {
                try
                {
                    DirectorySecurity ds = Directory.GetAccessControl(path);
                    string root = Directory.GetDirectoryRoot(path);

                    foreach (string file in Directory.GetFiles(path))
                    {
                        Program.totalFiles++;
                    }

                    foreach (string subDirectory in Directory.GetDirectories(path))
                    {
                        Program.totalDirectories++;
                        try
                        {
                            if (subDirectory.Length > 248)
                            {
                                continue;
                            }
                            if (subDirectory.ToLower().StartsWith(root + "\\users\\") && subDirectory.ToLower().Contains("\\appdata\\local\\application data") || subDirectory.ToLower().Contains("\\local settings\\application data"))
                            {
                                continue;
                            }
                            if (subDirectory == root + "\\Users\\All Users")
                            {
                                continue;
                            }
                            else if (subDirectory == root + "\\Users\\Default")
                            {
                                continue;
                            }
                            else if (subDirectory == root + "\\Users\\Default User")
                            {
                                continue;
                            }
                        }
                        catch (Exception)
                        {
                        }
                        Program.countFiles(subDirectory);
                    }
                }
                catch (Exception)
                {
                }
                return;
            }
        }

        static bool verifyDecryption(string path, FileInfo f)
        {
            FileInfo decryptedFile = new FileInfo(f.FullName.Substring(0, f.FullName.Length - 12));
            FileInfo encryptedFile = f;

            foreach (string file in Directory.GetFiles(path))
            {
                FileInfo currentFile = new FileInfo(file);
                if (currentFile.Name == decryptedFile.Name)
                {
                    return true;
                }
            }
            // Console.WriteLine(f.FullName + " not decrypted yet ...");
            // File.AppendAllText(logPath, "File not decrypted " + f.FullName + Environment.NewLine);
            return false;
        }

        static bool decryptFile(string encryptedFilePath)
        {
            FileInfo encryptedCopy = new FileInfo(encryptedFilePath);

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

                FileInfo decryptedFile = new FileInfo(encryptedFilePath);

                if (verifyDecryption(decryptedFile.Directory.ToString(), decryptedFile))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine("\r\n[-] Decryption key is not correct -> " + encryptedFilePath + ex.Message);
                File.AppendAllText(logPath, "ERROR (key not correct) : " + encryptedFilePath + ex.Message + Environment.NewLine);
                if (!File.Exists(str))
                    return false;
                File.Delete(str);
            }
            catch (XmlException ex)
            {
                Console.WriteLine("\r\n[-] Encrypted data is not correct -> " + encryptedFilePath + ex.Message);
                File.AppendAllText(logPath, "ERROR (data not correct) : " + encryptedFilePath + ex.Message + Environment.NewLine);
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
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
    }
}
