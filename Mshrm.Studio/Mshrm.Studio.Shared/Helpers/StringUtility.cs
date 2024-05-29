using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Mshrm.Studio.Shared.Helpers
{
    /// <summary>
    /// String formatting functions
    /// </summary>
    public static class StringUtility
    {
        /// <summary>
        /// Gets a random string of length n
        /// </summary>
        /// <param name="n">length of random string to be produced</param>
        /// <returns></returns>
        public static string RandomString(int n, string possibilities = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!&$_+=")
        {
            return new string(Enumerable.Repeat(possibilities, n)
              .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Remove the string if its at the end of the string provided
        /// </summary>
        /// <param name="displayName">The </param>
        /// <returns></returns>
        public static string? RemoveEndString(string str, string strToRemove)
        {
            // Stop here if null
            if (string.IsNullOrEmpty(str))
                return null;

            // Check if ends with the str
            if (str.EndsWith(strToRemove))
                return str.Substring(0, str.IndexOf(strToRemove));

            // Otherwise return the original string
            return str;
        }

        /// <summary>
        /// Splits a url to get a query params value
        /// </summary>
        /// <returns>The parameter value specified</returns>
        public static string? GetQueryParameterValue(string url, string paramName)
        {
            var uri = new Uri(url);
            var query = HttpUtility.ParseQueryString(uri.Query);

            return query.Get(paramName);
        }

        /// <summary>
        /// Gets a random string that matches expected password (alphanumeric, upper case, lower case and special characters)
        /// </summary>
        /// <param name="length">The length of the password to generate</param>
        /// <returns></returns>
        public static string GetRandomPassword(int length)
        {
            var lower = "abcdefghijklmnopqrstuvwxyz";
            var capital = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var number = "0123456789";
            var special = "!&$_+=";

            return $"{RandomString(1, lower)}{RandomString(1, capital)}{RandomString(1, number)}{RandomString(1, special)}{RandomString(length - 4)}";
        }

        /// <summary>
        /// Returns all characters in string up to the specified character
        /// </summary>
        /// <param name="input">String to format</param>
        /// <param name="targetCharacter">The target</param>
        /// <returns></returns>
        public static string AllCharactersUntil(this string input, string targetCharacter)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var indexOfCharacter = input.IndexOf(targetCharacter);
            if (indexOfCharacter > 0)
                return input.Substring(0, indexOfCharacter - 1);

            return input;
        }

        /// <summary>
        /// Creates an MD5 checksum from an array of bytes
        /// </summary>
        /// <param name="bytes">The bytes to get checksum for</param>
        /// <returns>A checksum</returns>
        public static string ToMD5Hash(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return string.Empty;
            }

            using (var md5 = MD5.Create())
            {
                return string.Join(string.Empty, md5.ComputeHash(bytes).Select(x => x.ToString("X2")));
            }
        }

        /// <summary>
        /// Lower case the first letter of a string provided
        /// </summary>
        /// <param name="text">The text to lower first letter of</param>
        /// <returns>The value with the first letter in lower case</returns>
        public static string LowercaseFirstLetter(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (Char.IsUpper(text[0]) == true)
            {
                text = text.Replace(text[0], char.ToLower(text[0]));
                return text;
            }

            return text;
        }

        /// <summary>
        /// Base 64 encode a string
        /// </summary>
        /// <param name="plainText">THe plaintext to encode</param>
        /// <returns>An encoded string</returns>
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Base64 decode a string
        /// </summary>
        /// <param name="base64EncodedData">The encoded string</param>
        /// <returns>A decoded string</returns>
        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);

            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Splits a string like "TestString" -> "Test String"
        /// </summary>
        /// <param name="source">The source string</param>
        /// <returns>The source string to split</returns>
        public static List<string> SplitCamelCase(this string source)
        {
            // Basic check
            if (string.IsNullOrEmpty(source?.Trim()))
            {
                return new List<string>();
            }

            // Split by capital letters
            return Regex.Split(source, @"(?<!^)(?=[A-Z])").ToList();
        }

        /// <summary>
        /// Splits a string like "TestString" -> "Test String"
        /// </summary>
        /// <param name="source">The source string</param>
        /// <returns>The source string to split</returns>
        public static string? SplitCamelCaseAndCombine(this string source)
        {
            // Basic check
            if (string.IsNullOrEmpty(source?.Trim()))
            {
                return null;
            }

            // Split by capital letters
            var value = source.SplitCamelCase();

            // Join and return
            return string.Join(" ", value);
        }

        /// <summary>
        /// Loads a string from file and appends values into it
        /// </summary>
        /// <returns></returns>
        public static string LoadAndReplaceString(string filePath, params string[] replaceValues)
        {
            var body = File.ReadAllText(filePath);

            for (int i = 0; i < replaceValues.Length; i++)
            {
                body = body.Replace($"{{{i}}}", replaceValues[i]);
            }

            return body;
        }

        /// <summary>
        /// String html tags from a string
        /// </summary>
        /// <param name="text">The text to strip html from</param>
        /// <returns>Plain text</returns>
        public static string StripHtmlWithRegex(this string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }

        /// <summary>
        /// Reduce the size of a guid
        /// </summary>
        /// <param name="guid">The guid to reduce</param>
        /// <returns>A reduced guid</returns>
        public static string ToTinyUUID(this Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray())[0..^2]  // remove trailing == padding 
                .Replace('+', '-')                          // escape (for filePath)
                .Replace('/', '_');                         // escape (for filePath)
        }
    }
}
