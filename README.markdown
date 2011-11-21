# POCKITKNIFE

...is a small utility assembly with convenient extension methods.
(repository at https://github.com/Krizzzn/PockItKnife)

### Current Features:
* **automagical commandline parser**
	doing this:
	```C#
	    static void Main(params string[] param) {
            var parser = param.ParseCommandlineArguments();
            var setup = new SyncSvc.SyncSettings();
            parser.AutomagicInit(setup);
        }
	```
	will set the properties of the object setup automagically.

* **help file printer**
	checks the commandline for flags like /?, -?, --?, ?, /help, -help, --help or help, and displays the contents of a textfile file embedded in the calling assembly to an Action<string> delegate.

* **humanizing IEnumerable<T>**
	converts lists into "a, b, c and d" or "a, b, c or d"

* **string format injector**
	.Inject(string format, params object[]) extension method for strings

* **WeakCrypto**
	Extention Method for strings, to quickly En- and De-crypt strings. This is not safe, but can be used to avoid clear text passwords in config files.

* **ForEach() for IEnumerable<T>**
	Call an Action<T> for each object in an IEnumerable.

* **Embedded Resource reader extension for System.Reflection.Assembly**

* **Jsonize() Extension for DataTable and DataRow**

* **other string extensions**
	such as: Limit() and IsNullOrEmpty()

The test project contains references to FluentAssertions using NuGet. The binaries are not included into this package.