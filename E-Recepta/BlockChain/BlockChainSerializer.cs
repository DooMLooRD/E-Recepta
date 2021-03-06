﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain
{
    public class BlockChainSerializer
    {

       public static void serialize(List<Block> prescriptions, string blockChainName)
        {
            CreateDirectory();
            string serializedPrescriptions = JsonConvert.SerializeObject(prescriptions);

            while (!IsFileReady("data" + System.IO.Path.DirectorySeparatorChar.ToString() + blockChainName + ".json") && File.Exists("data" + System.IO.Path.DirectorySeparatorChar.ToString() + blockChainName + ".json")) {
                
            }

            System.IO.File.WriteAllText(@"data" + System.IO.Path.DirectorySeparatorChar.ToString() + blockChainName + ".json", serializedPrescriptions);
        }

        public static List<Block> deserialize(string blockChainName)
        {
            if (File.Exists(@"data" + System.IO.Path.DirectorySeparatorChar.ToString() + blockChainName + ".json")) {
                return JsonConvert.DeserializeObject<List<Block>>(File.ReadAllText(@"data" + System.IO.Path.DirectorySeparatorChar.ToString() + blockChainName + ".json"));
            }

            return null;
        }

        private static void CreateDirectory()
        {
            bool dataDirExists = System.IO.Directory.Exists("data");

            if (!dataDirExists)
            {
                System.IO.Directory.CreateDirectory("data");
            }
        }

        public static bool IsFileReady(string filename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    return inputStream.Length > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
