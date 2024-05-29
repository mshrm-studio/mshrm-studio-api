namespace Mshrm.Studio.Shared.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Gets an object value by property name
        /// </summary>
        /// <param name="obj">The object to get the property value from</param>
        /// <param name="propName">The property name of the value we want to get</param>
        /// <returns>The property value for a specified name</returns>
        /// <author>Matt Sharp</author>
        /// <date>17 February 2020</date>
        public static dynamic GetPropertyValue(this object obj, string propName)
        {
            return obj.GetType().GetProperty(propName).GetValue(obj);
        }

        public static Guid GenerateSeededGuid(this object value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value.GetHashCode()).CopyTo(bytes, 0);

            return new Guid(bytes);
        }
    }
}
