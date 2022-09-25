namespace SimplySoft.Core.Test.WebUI
{
    public static class Extensions
    {
        public static string ToMask(this string value, char breaker, int iterator = 5, char maskCharacter = '*')
        {
            string[] parts = value.Split(breaker);
            parts[0] = parts[0].Substring(0, 3);
            return parts[0] + new string(maskCharacter, iterator) + breaker + parts[1];
        }
    }
}
