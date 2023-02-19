namespace CadastroClienteBff.Config.Extensions
{
    public static class StringExtension
    {
        public static string ToCamelCase(this string value)
        {
            return value.ToUpperInvariant();
        }

        public static string ToCamelCaseIgnoreCase(this string value) { return value.ToLowerInvariant(); }

        public static string ToNormalizeString(this string value)
        {
            value = value.Replace("-","");
            value = value.Replace(".", "");
            value = value.Replace(",", "");
            value = value.Replace("\\", "");
            value = value.Replace("/", "");
            return value;
        }

        public static string MaskNumber(this string value, string mask)
        {
            try
            {
                return Int64.Parse(value).ToString(mask);
            }
            catch(FormatException ex)
            {
                throw new ArgumentException("Formato de número inválido");
            }
        }
    }
}
