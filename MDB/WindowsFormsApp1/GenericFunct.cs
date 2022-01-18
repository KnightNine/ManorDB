using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections.Specialized;
using System.Drawing;


namespace MDB
{
    public static class GenericFunct
    {
        static readonly char[] SpecialChars = "!@#$%^*()/\\.".ToCharArray();

        public static string ValidateNameInput(string str)
        {
            string error = "";
            int indexOf = str.IndexOfAny(SpecialChars);

            if (indexOf != -1)
            {
                error += "cannot contain \" !@#$%^*()/\\.) \"";
            }
            if (str == "")
            {
                error += "is blank";
            }


            if (error != "")
            {
                error += "!";
            }

            return error;
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }


        


    }
        
}
