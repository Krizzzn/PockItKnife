using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Class that contains single extensions for several objects.
    /// </summary>
    public static class OtherExtensions
    {
        /// <summary>
        /// Loads an Embedded Resource File from a given assembly. The method looks for files compiled into the assembly ending with the 
        /// given fileNameInAssembly. If multiple files with the same name exist, this returns the first in the list.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="fileNameInAssembly"></param>
        /// <returns></returns>
        /// <exception cref="System.IO.FileNotFoundException">Throws a FileNotFoundException if the file is not compiled into the assembly as Embedded Resource</exception>
        public static string LoadEmbeddedFile(this System.Reflection.Assembly assembly, string fileNameInAssembly) {

            var resName = from name in assembly.GetManifestResourceNames()
                          where name.ToLower().EndsWith(fileNameInAssembly.ToLower())
                          select name;

            string text = default(string);
            var fullResourceName = resName.FirstOrDefault();

            if (fullResourceName == null)
                throw new System.IO.FileNotFoundException("Could not find file {0} in the resources of assembly {1}.".Inject(fileNameInAssembly, assembly.FullName));

            using (System.IO.Stream stream = assembly.GetManifestResourceStream(fullResourceName))
            {
                Byte[] bytes = new Byte[stream.Length];

                stream.Read(bytes, 0, (int)(stream.Length));
                text = System.Text.UTF8Encoding.Default.GetString(bytes);
            }

            return text;
        }
    }
}
