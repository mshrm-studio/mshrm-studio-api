namespace Mshrm.Studio.Shared.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Lower case the first letter of a string provided
        /// </summary>
        /// <param name="text">The text to lower first letter of</param>
        /// <returns>The value with the first letter in lower case</returns>
        public static string? LowercaseFirstLetter(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            if (Char.IsUpper(text[0]) == true)
            {
                text = text.Replace(text[0], char.ToLower(text[0]));
                return text;
            }

            return text;
        }

        /// <summary>
        /// Lower case the first letter of a string provided
        /// </summary>
        /// <param name="text">The text to lower first letter of</param>
        /// <returns>The value with the first letter in lower case</returns>
        public static string? UppercaseFirstLetter(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            text = text.ToLower();

            if (Char.IsUpper(text[0]) == true)
            {
                text = text.Replace(text[0], char.ToUpper(text[0]));
                return text;
            }

            return text;
        }
    }
}
