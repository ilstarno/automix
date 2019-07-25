namespace Automix_ini.helpers
{
    internal static class Assert
    {
        internal static bool StringHasNoBlankSpaces(string s)
        {
            return !s.Contains(" ");
        }
    }
}