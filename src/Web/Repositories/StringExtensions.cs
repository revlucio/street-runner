namespace StreetRunner.Web.Repositories
{
    public static class StringExtensions
    {
        public static string EncodeForFileSystem(this string value)
        {
            return value.Replace('/', '-').Replace('?', '-');
        }
    }
}