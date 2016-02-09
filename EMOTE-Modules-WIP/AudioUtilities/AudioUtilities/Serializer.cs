using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AudioUtilities
{
    /// <summary>
    /// Serializes and deserializes any binary file into strings and back
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Serializes a binary file into a string
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A string rapresenting the binary file</returns>
        static public string FromFile(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Deserialize a string back into a byte array
        /// </summary>
        /// <param name="str">The string to be deserialized</param>
        /// <returns>A byte array that can be output to a file</returns>
        static public byte[] Deserialize(string str)
        {
            return Convert.FromBase64String(str);
        }

        /// <summary>
        /// Deserialize a string into a binary file
        /// </summary>
        /// <param name="str">The string to be deserialized</param>
        /// <param name="path">The path where the output file will be created</param>
        static public void DeserializeToFile(string str, string path)
        {
            byte[] bytes = Deserialize(str);
            using (Stream file = File.OpenWrite(path))
            {
                file.Write(bytes, 0, bytes.Length);
                file.Close();
            }
        }
    }
}
