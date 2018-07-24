using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;

namespace FastDecryptApp
{
    public class FastDecrypt
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				// Default program
			}
			else
			{
				/* 
                 User entered flags manually
				 Possibilities include :
				 -a : analysis mode
				 -f : just decrypt weapologie files, and delete sorry files
				 -d : decrypt and delete all weapologize and sorry files
				 -m : decrypt and move weapologize files to new location
                */
				switch (args[0])
				{
					case "-a": analysisMode(BuildFS(args[1], new HashSet<FileInfo>()));
						break;
					case "-f": normalDecryptMode(args[1]);
						break;
					case "-d": deleteDecryptMode(args[1]);
						break;
					case "-m": moveMode(args[1], args[2]);
						break;
					default: // throw flag not known exception
						break;
				}
			}
		}

        static HashSet<FileInfo> BuildFS(string path, HashSet<FileInfo> hashSet) // O(n) - add every file from every subdir
        {
            try
            {
                foreach (string file in Directory.EnumerateFiles(path))
                {
                    hashSet.Add(new FileInfo(file));
                }

                foreach (string subDir in Directory.EnumerateDirectories(path))
                {
                    BuildFS(subDir, hashSet);
                }
            }
            catch (Exception)
            {
            }
            return hashSet;
        }

		static void analysisMode(HashSet<FileInfo> fileList)
        {
            foreach(FileInfo)
        }

	}

}
