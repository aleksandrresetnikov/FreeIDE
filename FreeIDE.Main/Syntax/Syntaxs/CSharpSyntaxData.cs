using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeIDE.Syntax.Syntaxs
{
    class CSharpSyntax
    {
        public static string compiledKeywords = "";
        public static string compiledMethods = "";
        public static string compiledClasses = "String";
        public static string compiledAnnotations = "";

        public static string[] keywords = {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch",
            "char", "checked", "class", "const", "continue", "decimal", "default",
            "delegate", "do", "double", "else", "enum", "event", "explicit", "extern",
            "false", "finally", "fixed", "float", "for", "foreach", "goto", "if",
            "implicit", "in", "int", "interface", "internal", "is", "lock", "long",
            "namespace", "new", "null", "object", "operator", "out", "override",
            "params", "private", "protected", "public", "readonly", "ref", "return",
            "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string",
            "struct", "switch", "this", "throw", "true", "try", "typeof", "uint",
            "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void",
            "volatile", "while", "add", "alias", "ascending", "descending",
            "dynamic", "from", "get", "global", "group", "into", "join", "let",
            "orderby", "partial", "remove", "select", "set", "value", "var", "where", "yield"
        };
        public static string[] methods = { "Equals()", "GetHashCode()", "GetType()", "ToString()" };
        public static string[] snippets = {
            "if(^)\n{\n;\n}", "if(^)\n{\n;\n}\nelse\n{\n;\n}", "for(^;;)\n{\n;\n}", "while(^)\n{\n;\n}",
            "do\n{\n^;\n}while();", "switch(^)\n{\ncase : break;\n}", "switch(^)\n{\ncase : break;\n}\n{\ndefault : break;\n}"
        };
        public static string[] declarationSnippets = {
            "public class ^\n{\n}", "private class ^\n{\n}", "internal class ^\n{\n}",
            "public static class ^\n{\n}", "private static class ^\n{\n}", "internal static class ^\n{\n}",
            "public abstract class ^\n{\n}", "private abstract class ^\n{\n}", "internal abstract class ^\n{\n}",

            "class ^\n{\n}", "class ^\n{\n}", "class ^\n{\n}",
            "static class ^\n{\n}", "static class ^\n{\n}", "static class ^\n{\n}",
            "abstract class ^\n{\n}", "abstract class ^\n{\n}", "abstract class ^\n{\n}",

            "namespace ^ \n{\n;\n}",
            "public struct ^\n{\n;\n}", "private struct ^\n{\n;\n}", "internal struct ^\n{\n;\n}",

            "void ^()\n{\n;\n}", "public void ^()\n{\n;\n}", "private void ^()\n{\n;\n}", "internal void ^()\n{\n;\n}", "protected void ^()\n{\n;\n}", "virtual void ^()\n{\n;\n}",
            "static void ^()\n{\n;\n}", "public static void ^()\n{\n;\n}", "private static void ^()\n{\n;\n}", "internal static void ^()\n{\n;\n}",
            "int ^()\n{\n;\n}", "public int ^()\n{\n;\n}", "private int ^()\n{\n;\n}", "internal int ^()\n{\n;\n}", "protected int ^()\n{\n;\n}", "virtual int ^()\n{\n;\n}",
            "static int ^()\n{\n;\n}", "public static int ^()\n{\n;\n}", "private static int ^()\n{\n;\n}", "internal static int ^()\n{\n;\n}",
            "bool ^()\n{\n;\n}", "public bool ^()\n{\n;\n}", "private bool ^()\n{\n;\n}", "internal bool ^()\n{\n;\n}", "protected bool ^()\n{\n;\n}", "virtual bool ^()\n{\n;\n}",
            "static bool ^()\n{\n;\n}", "public static bool ^()\n{\n;\n}", "private static bool ^()\n{\n;\n}", "internal static bool ^()\n{\n;\n}",
            "long ^()\n{\n;\n}", "public long ^()\n{\n;\n}", "private long ^()\n{\n;\n}", "internal long ^()\n{\n;\n}", "protected long ^()\n{\n;\n}", "virtual long ^()\n{\n;\n}",
            "static long ^()\n{\n;\n}", "public static long ^()\n{\n;\n}", "private static long ^()\n{\n;\n}", "internal static long ^()\n{\n;\n}",
            "float ^()\n{\n;\n}", "public float ^()\n{\n;\n}", "private float ^()\n{\n;\n}", "internal float ^()\n{\n;\n}", "protected float ^()\n{\n;\n}", "virtual float ^()\n{\n;\n}",
            "static float ^()\n{\n;\n}", "public static float ^()\n{\n;\n}", "private static float ^()\n{\n;\n}", "internal static float ^()\n{\n;\n}",
            "byte ^()\n{\n;\n}", "public byte ^()\n{\n;\n}", "private byte ^()\n{\n;\n}", "internal byte ^()\n{\n;\n}", "protected byte ^()\n{\n;\n}", "virtual byte ^()\n{\n;\n}",
            "static byte ^()\n{\n;\n}", "public static byte ^()\n{\n;\n}", "private static byte ^()\n{\n;\n}", "internal static byte ^()\n{\n;\n}",

            "public ^{ get; set; }", "private ^{ get; set; }", "internal ^{ get; set; }", "protected ^{ get; set; }"
        };
        public static string[] namespaces = { // by default:
            "System",
            "System.Buffers",
            "System.Buffers.Binary",
            "System.Buffers.Text",
            "System.CodeDom.Compiler",
            "System.Collections",
            "System.Collections.Concurrent",
            "System.Collections.Generic",
            "System.Collections.Immutable",
            "System.Collections.ObjectModel",
            "System.Collections.Specialized",
            "System.ComponentModel",
            "System.ComponentModel.DataAnnotations",
            "System.ComponentModel.DataAnnotations.Schema",
            "System.ComponentModel.Design",
            "System.ComponentModel.Design.Serialization",
            "System.Configuration.Assemblies",
            "System.Data",
            "System.Data.Common",
            "System.Data.SqlTypes",
            "System.Diagnostics",
            "System.Diagnostics.CodeAnalysis",
            "System.Diagnostics.Contracts",
            "System.Diagnostics.Metrics",
            "System.Diagnostics.SymbolStore",
            "System.Diagnostics.Tracing",
            "System.Drawing",
            "System.Dynamic",
            "System.Formats.Asn1",
            "System.Globalization",
            "System.IO",
            "System.IO.Compression",
            "System.IO.Enumeration",
            "System.IO.IsolatedStorage",
            "System.IO.MemoryMappedFiles",
            "System.IO.Pipes",
            "System.Linq",
            "System.Linq.Expressions",
            "System.Net",
            "System.Net.Cache",
            "System.Net.Http",
            "System.Net.Http.Headers",
            "System.Net.Http.Json",
            "System.Net.Mail",
            "System.Net.Mime",
            "System.Net.NetworkInformation",
            "System.Net.Security",
            "System.Net.Sockets",
            "System.Net.WebSockets",
            "System.Numerics",
            "System.Reflection",
            "System.Reflection.Emit",
            "System.Reflection.Metadata",
            "System.Reflection.Metadata.Ecma335",
            "System.Reflection.PortableExecutable",
            "System.Resources",
            "System.Runtime",
            "System.Runtime.CompilerServices",
            "System.Runtime.ConstrainedExecution",
            "System.Runtime.ExceptionServices",
            "System.Runtime.InteropServices",
            "System.Runtime.InteropServices.ComTypes",
            "System.Runtime.InteropServices.ObjectiveC",
            "System.Runtime.Intrinsics",
            "System.Runtime.Intrinsics.Arm",
            "System.Runtime.Intrinsics.X86",
            "System.Runtime.Loader",
            "System.Runtime.Remoting",
            "System.Runtime.Serialization",
            "System.Runtime.Serialization.Formatters",
            "System.Runtime.Serialization.Formatters.Binary",
            "System.Runtime.Serialization.Json",
            "System.Runtime.Versioning",
            "System.Security",
            "System.Security.AccessControl",
            "System.Security.Authentication",
            "System.Security.Authentication.ExtendedProtection",
            "System.Security.Claims",
            "System.Security.Cryptography",
            "System.Security.Cryptography.X509Certificates",
            "System.Security.Permissions",
            "System.Security.Policy",
            "System.Security.Principal",
            "System.Text",
            "System.Text.Encodings.Web",
            "System.Text.Json",
            "System.Text.Json.Nodes",
            "System.Text.Json.Serialization",
            "System.Text.Json.Serialization.Metadata",
            "System.Text.RegularExpressions",
            "System.Text.Unicode",
            "System.Threading",
            "System.Threading.Channels",
            "System.Threading.Tasks",
            "System.Threading.Tasks.Dataflow",
            "System.Threading.Tasks.Sources",
            "System.Timers",
            "System.Transactions",
            "System.Web",
            "System.Windows.Input",
            "System.Windows.Markup",
            "System.Xml",
            "System.Xml.Linq",
            "System.Xml.Resolvers",
            "System.Xml.Schema",
            "System.Xml.Serialization",
            "System.Xml.XPath",
            "System.Xml.Xsl",
            "Microsoft.CSharp.RuntimeBinder",
            "Microsoft.VisualBasic",
            "Microsoft.VisualBasic.CompilerServices",
            "Microsoft.VisualBasic.FileIO",
            "Microsoft.Win32",
            "Microsoft.Win32.SafeHandles"
        };

        public static FastColoredTextBoxNS.Style invisibleCharsStyle = new InvisibleCharsRenderer(System.Drawing.Pens.Gray);
        public static System.Drawing.Color currentLineColor =
            System.Drawing.Color.FromArgb(100, 210, 210, 255);
        public static FastColoredTextBoxNS.Style sameWordsStyle =
            new FastColoredTextBoxNS.MarkerStyle(
                new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(50, System.Drawing.Color.Gray)));

        public static System.Drawing.Color changedLineColor =
            System.Drawing.Color.FromArgb(255, 230, 230, 255);

        public static string lang = "CSharp (custom highlighter)";

        //styles
        public static FastColoredTextBoxNS.TextStyle BlueStyle =
            new FastColoredTextBoxNS.TextStyle(System.Drawing.Brushes.Blue, null, System.Drawing.FontStyle.Regular);

        public static FastColoredTextBoxNS.TextStyle BoldStyle =
            new FastColoredTextBoxNS.TextStyle(null, null, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline);

        public static FastColoredTextBoxNS.TextStyle GrayStyle =
            new FastColoredTextBoxNS.TextStyle(System.Drawing.Brushes.Gray, null, System.Drawing.FontStyle.Regular);

        public static FastColoredTextBoxNS.TextStyle MagentaStyle =
            new FastColoredTextBoxNS.TextStyle(System.Drawing.Brushes.Magenta, null, System.Drawing.FontStyle.Regular);

        public static FastColoredTextBoxNS.TextStyle GreenStyle =
            new FastColoredTextBoxNS.TextStyle(System.Drawing.Brushes.Green, null, System.Drawing.FontStyle.Italic);

        public static FastColoredTextBoxNS.TextStyle BrownStyle =
            new FastColoredTextBoxNS.TextStyle(System.Drawing.Brushes.Brown, null, System.Drawing.FontStyle.Italic);

        public static FastColoredTextBoxNS.TextStyle MaroonStyle =
            new FastColoredTextBoxNS.TextStyle(System.Drawing.Brushes.Maroon, null, System.Drawing.FontStyle.Regular);

        public static FastColoredTextBoxNS.MarkerStyle SameWordsStyle =
            new FastColoredTextBoxNS.MarkerStyle(new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(40, System.Drawing.Color.Gray)));

    }
}
