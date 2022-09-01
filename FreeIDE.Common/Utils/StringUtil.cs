using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeIDE.Common.Utils
{
    public static class StringUtil
    {
        public static string Steping(int val)
        {
            string outValue = "";
            for (int i = 0; i < val; i++)
                outValue += "\t";
            return outValue;
        }
        public static string CharFilter(this string text, params char[] filterChars)
        {
            foreach (char c in filterChars)
                text = text.Replace(c.ToString(), "");
            return text;
        }
        public static string GetFixNum(int num)
        {
            if (num < 10) return $"000{num}";
            else if (num < 100) return $"00{num}";
            else if (num < 1000) return $"0{num}";
            else if (num < 10000) return $"{num}";
            else return num.ToString();
        }
        public static string GetUnrangeTitle(this string text, Range range, ref int intValue, char note = ' ', string nullableStr = "0")
        {
            if (text == "") return nullableStr;

            string tVal = "";
            foreach (char item in text)
                if (char.IsNumber(item))
                    tVal += item;

            if (tVal == "") return nullableStr;

            Int64 value = Convert.ToInt64(tVal);
            if (range.max != -1 && value > range.max) value = range.max;
            if (value < range.min) value = range.min;

            intValue = (int)value;
            return value.ToString() + (note == '~' ? "" : note.ToString());
        }
        public static string ReplaceFirstWord(this string context, string oldString, string newString)
        {
            int index = context.IndexOf(oldString);

            if (index >= 0)
                return context.Substring(0, index) + newString + context.Substring(index + oldString.Length);
            else
                return context;
        }
        public static string DeleteFirstWord(this string context, string word)
        {
            return context.ReplaceFirstWord(word, "");
        }
        public static string ReverseString(this string context)
        {
            return new string(context.Reverse().ToArray());
        }
        public static string[] ReadingStringLinePerLine(this string context)
        {
            System.Collections.Generic.Queue<string> OutputValue = new System.Collections.Generic.Queue<string>();

            foreach (var item in context.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                OutputValue.Enqueue(item);

            return OutputValue.ToArray();
        }
        /*public static string ReverseString_Unsafe(this string context)
        {
            int copylen = context.Length;

            IntPtr sptr = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(context); // source strings
            IntPtr dptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(copylen + 1);    // destination string

            unsafe
            {
                byte* src = (byte*)sptr.ToPointer();
                byte* dst = (byte*)dptr.ToPointer();

                if (copylen > 0)
                {
                    src += copylen - 1;

                    while (copylen-- > 0)
                    {
                        *dst++ = *src--;
                    }
                    *dst = 0;
                }
            }

            return System.Runtime.InteropServices.Marshal.PtrToStringAnsi(dptr);
        }*/

        public static string QuotePadLeft(string str, int totalSize)
        {
            int spaces = totalSize - 2 - str.Length;
            if (spaces < 0) spaces = 0;
            return new string(' ', spaces) + '"' + str + '"';
        }

        public static string FilterString(this string context, params string[] filter)
        {
            string output_value = context;

            foreach (string filter_item in filter)
                output_value = output_value.Replace(filter_item, "");

            return output_value;
        }

        public static int[] ConvertStringArrayToIntArray(this string[] string_array)
        {
            int[] output_value = new int[string_array.Length];

            for (int index = 0; index < string_array.Length; index++)
                output_value[index] = Convert.ToInt32(string_array[index]);

            return output_value;
        }

        public static string GetStringElementThroughDelimiter(this string context, char delimiter, int element_index, bool remove_spaces = true)
        {
            string[] string_elements = (remove_spaces ? context.Replace(" ", "") : context).Split(delimiter);
            return string_elements[element_index];
        }

        public static string DeleteStrings(this string context, string[] filter_contexts)
        {
            string output_value = context;

            foreach (string filter_item in filter_contexts)
                output_value = DeleteFirstWord(output_value, filter_item);

            return output_value;
        }

        public static string RemoveSpaces(this string context)
        {
            return context.Replace(" ", "");
        }
    }

    public class Range
    {
        public int min, max;

        public Range(int min, int max)
        { this.min = min; this.max = max; }

        public override string ToString() => $"Min = {this.min}, max = {max}.";
    }
}
