namespace EPK.Web.Infrastructure.Utilities
{
    public static class StringExtension
    {
        public static bool StringIsNullEmptyWhiteSpace(this string obj)
        {
            if (string.IsNullOrEmpty(obj) || (string.IsNullOrWhiteSpace(obj)))
                return true;
            return false;
        }
    }
}