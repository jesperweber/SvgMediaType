using System;
using System.Configuration;

namespace SvgMediaType.Configuration
{
    public static class SvgMediaTypeConfig
    {
        public static string SvgMediaTypeAlias => GetValue("SvgMediaType.SvgMediaTypeAlias", "svg");
        public static string SvgMediaTypeContentPropertyAlias => GetValue("SvgMediaType.SvgMediaTypeContentPropertyAlias", "content");

        private static T GetValue<T>(string key, T defaultValue) where T : IConvertible
        {
            var setting = ConfigurationManager.AppSettings[key];
            if (setting == null)
                return defaultValue;

            try
            {
                return (T)Convert.ChangeType(setting, typeof(T));
            }
            catch (Exception e)
            {
                throw new ConfigurationErrorsException($"Could not parse {setting} to type of {typeof(T)}", e);
            }
        }
    }
}