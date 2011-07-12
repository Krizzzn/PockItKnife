using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PockItKnife
{
    /// <summary>
    /// Utitlity Class that is able to parse commandline parameters or automagically assign property objects.
    /// </summary>
    public class CommandlineArguments
    {
        public const string DEFAULT_HELPFILE = "HELP";

        private string[] _arguments;
        private List<KeyValuePair<string,string>> _parsedArguments;

        private CommandlineArguments(string[] arguments)
        { 
            _arguments = arguments;
        }

        /// <summary>
        /// Count of parsed arguments
        /// </summary>
        public int Count {
            get {
                if (_arguments == null)
                    return 0;
                return _arguments.Length;
            }
        }

        /// <summary>
        /// Gets a single argument pair by key.
        /// </summary>
        /// <param name="key">key is case insensitive</param>
        /// <returns></returns>
        public string this[string key]
        {
            get {
                var kv = _parsedArguments.Find((i) => i.Key.ToLower() == key.ToLower());
                return kv.Value;
            }
        }

        /// <summary>
        /// Get all parsed arguments at once
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> All
        {
            get {
                foreach (KeyValuePair<string, string> kvp in _parsedArguments)
                    yield return kvp;
            }
        }

        private void ParseArguments()
        {
            this._parsedArguments = new List<KeyValuePair<string, string>>();

            if (_arguments == null || _arguments.Length == 0)
                return;

            Func<string[], KeyValuePair<string, string>> NKV = (arr) => new KeyValuePair<string, string>(arr[0], arr[1]);

            string chained = string.Join(" ", _arguments);

            Regex splitEx = new Regex(@"(!?[ :=]+)");
            Regex prefEx = new Regex(@"^[-/]{1,2}");
            Regex sepEx = new Regex(@"^[ :=]{1,2}$");
            Regex suffEx = new Regex(@"[ :=]$");

            var args = new Stack<string>(_arguments.Reverse());

            string[] collect = null;
            while (args.Count > 0)
            {
                var bit = args.Pop();

                if (sepEx.IsMatch(bit))
                    continue;

                if (collect != null && prefEx.Match(bit).Success)
                {
                    if (collect[1].IsNullOrEmpty())
                        collect[1] = "true";
                    this._parsedArguments.Add(NKV(collect));
                    collect = null;
                }

                if (collect == null && !bit.IsNullOrEmpty())
                {
                    while (suffEx.IsMatch(bit))
                        bit = suffEx.Replace(bit, "");

                    bit = prefEx.Replace(bit, "");
                    collect = new[] { bit, "" };
                    continue;
                }

                collect[1] += bit;

                collect[1] = collect[1].Trim();
                collect[1] = collect[1].Replace("\"", "");

                if (collect[1].IsNullOrEmpty())
                    collect[1] = "true";

                this._parsedArguments.Add(NKV(collect));
                collect = null;
            }
            if (collect != null){
                if (collect[1].IsNullOrEmpty())
                    collect[1] = "true";
                this._parsedArguments.Add(NKV(collect));
                collect = null;
            }
        }

        /// <summary>
        /// Automagically assigns parameters to a given object's properties. The object's property names must either conform to the 
        /// commandline arguments, or feature the PockItKnife.CommandlineArguments.AutomagicLoadAttribute.
        /// Can handle boolean, string, int, double, and float values
        /// Properties must be public and must be writable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToInit"></param>
        public void AutomagicInit<T>(T objectToInit)
        {
            if (objectToInit == null)
                throw new ArgumentNullException("objectToInit");

           var prop = typeof(T).GetProperties();

            prop.ForEach((p) => p.CanWrite, (p) => {
                string val;
                var ala = p.GetCustomAttributes(typeof(AutomagicLoadAttribute), true);
                if (ala.Length > 0)
                    val = this[(ala[0] as AutomagicLoadAttribute).ArgumentToLoad];
                else
                    val = this[p.Name];

                if (val.IsNullOrEmpty())
                    return;

                if (p.PropertyType == typeof(bool))
                    p.SetValue((object)objectToInit, Convert.ToBoolean(val), null);
                else if (p.PropertyType == typeof(string))
                    p.SetValue((object)objectToInit, val, null);
                else if (p.PropertyType == typeof(int))
                    p.SetValue((object)objectToInit, int.Parse(val), null);
                else if (p.PropertyType == typeof(double))
                    p.SetValue((object)objectToInit, double.Parse(val), null);
                else if (p.PropertyType == typeof(float))
                    p.SetValue((object)objectToInit, float.Parse(val), null);
            });
        }

        /// <summary>
        /// If the Help flag is omitted, this method reads and prints a file named HELP inside the calling assembly to the Console and returns true.
        /// </summary>
        /// <param name="textFileNameInAssembly"></param>
        /// <param name="printTo"></param>
        /// <returns></returns>
        public bool PrintHelpfileIfRequested()
        {
            if (this["help"] != null)
            {
                PrintHelpfile(DEFAULT_HELPFILE, Console.WriteLine, System.Reflection.Assembly.GetCallingAssembly());
                return true;
            }
            return false;
        }

        /// <summary>
        /// If the Help flag is omitted, this method reads and prints a file named HELP inside the calling assembly to the delegate and returns true.
        /// </summary>
        /// <param name="textFileNameInAssembly"></param>
        /// <param name="printTo"></param>
        /// <returns></returns>
        public bool PrintHelpfileIfRequested(Action<string> printTo)
        {
            if (this["help"] != null)
            {
                PrintHelpfile(DEFAULT_HELPFILE, printTo, System.Reflection.Assembly.GetCallingAssembly());
                return true;
            }
            return false;
        }

        /// <summary>
        /// If the Help flag is omitted, this method reads and prints textFileNameInAssembly to the Console and returns true.
        /// </summary>
        /// <param name="textFileNameInAssembly"></param>
        /// <param name="printTo"></param>
        /// <returns></returns>
        public bool PrintHelpfileIfRequested(string textFileNameInAssembly) {
            if (this["help"] != null)
            {
                PrintHelpfile(textFileNameInAssembly, System.Console.WriteLine, System.Reflection.Assembly.GetCallingAssembly());
                return true;
            }
            return false;
        }

        /// <summary>
        /// If the Help flag is omitted, this method reads and prints textFileNameInAssembly to the delegate and returns true.
        /// </summary>
        /// <param name="textFileNameInAssembly"></param>
        /// <param name="printTo"></param>
        /// <returns></returns>
        public bool PrintHelpfileIfRequested(string textFileNameInAssembly, Action<string> printTo)
        {
            if (this["help"] != null)
            {
                PrintHelpfile(textFileNameInAssembly, printTo, System.Reflection.Assembly.GetCallingAssembly());
                return true;
            }
            return false;
        }

        /// <summary>
        /// Trys to load a File named HELP from the resources of the calling assembly. Prints the contents to the System.Console.
        /// </summary>
        /// <param name="textFileNameInAssembly"></param>
        /// <param name="printTo"></param>
        public void PrintHelpfile()
        {
            PrintHelpfile(DEFAULT_HELPFILE, System.Console.WriteLine, System.Reflection.Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Trys to load a File named HELP from the resources of the calling assembly. Prints the contents to the delegate.
        /// </summary>
        /// <param name="textFileNameInAssembly"></param>
        /// <param name="printTo"></param>
        public void PrintHelpfile(Action<string> printTo)
        {
            PrintHelpfile(DEFAULT_HELPFILE, printTo, System.Reflection.Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Trys to load a File from the resources of the calling assembly. Prints the contenst to the System.Console.
        /// </summary>
        /// <param name="textFileNameInAssembly"></param>
        /// <param name="printTo"></param>
        public void PrintHelpfile(string textFileNameInAssembly)
        {
            PrintHelpfile(textFileNameInAssembly, Console.WriteLine, System.Reflection.Assembly.GetCallingAssembly());            
        }

        /// <summary>
        /// Trys to load a File from the resources of the calling assembly. Prints the contenst to the delegate.
        /// </summary>
        /// <param name="textFileNameInAssembly"></param>
        /// <param name="printTo"></param>
        public void PrintHelpfile(string textFileNameInAssembly, Action<string> printTo)
        {
            PrintHelpfile(textFileNameInAssembly, printTo, System.Reflection.Assembly.GetCallingAssembly());
        }

        private void PrintHelpfile(string textFileNameInAssembly, Action<string> printTo, System.Reflection.Assembly assbly)
        {
            var resName = from name in assbly.GetManifestResourceNames()
                          where name.ToLower().EndsWith(textFileNameInAssembly.ToLower())
                          select name;

            string text = default(string);
            var fullResourceName = resName.FirstOrDefault();

            if (fullResourceName == null)
                throw new System.IO.FileNotFoundException("Could not find file {0} in the resources of assembly {1}.".Inject( textFileNameInAssembly, assbly.FullName ) );

            using (System.IO.Stream stream = assbly.GetManifestResourceStream(fullResourceName))
            {
                Byte[] bytes = new Byte[stream.Length];

                stream.Read(bytes, 0, (int)(stream.Length));
                text = System.Text.UTF8Encoding.Default.GetString(bytes);
            }

            printTo(text);
        }

        /// <summary>
        /// Parses commandline attributes and returns an PockItKnife.CommandlineArguments object that is ready for use.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static CommandlineArguments ParseCommandLineArguments(params string[] arguments)
        {
            var cmd = new CommandlineArguments(arguments);
            cmd.ParseArguments();
            return cmd;
        }

        public bool Contains(string argument)
        {
            return !this[argument].IsNullOrEmpty();
        }

        /// <summary>
        /// assign this attribute to properties of objects, that should be automagically loaded by PockItKnife.CommandlineArguments.AutomagicInit()
        /// this attribute specifies the name of the commandline argument, that shall be loaded.
        /// </summary>
        [AttributeUsage (AttributeTargets.Property)]
        public class AutomagicLoadAttribute : System.Attribute
        {
            /// <summary>
            /// Specifies the commandline argument name, a property decorated with this attribute should load its value from.
            /// </summary>
            /// <param name="argumentToLoad"></param>
            public AutomagicLoadAttribute(string argumentToLoad)
            {
                if (argumentToLoad.IsNullOrEmpty())
                    throw new ArgumentNullException("argumentToLoad");
                ArgumentToLoad = argumentToLoad;
            }

            public string ArgumentToLoad
            {
                get;
                set;
            }
        }
    }
}
