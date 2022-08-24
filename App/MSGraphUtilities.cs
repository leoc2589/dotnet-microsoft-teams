namespace App;

public static class MSGraphUtilities
{
    public static string MSGraphScope => "https://graph.microsoft.com/.default";
    public static string AuthorityFormat => "https://login.microsoftonline.com/{0}/v2.0";
    public static string MSGraphQueryByUser => "https://graph.microsoft.com/v1.0/users/{0}/events";
    public static string MSGraphQueryByUserAndEvent => "https://graph.microsoft.com/v1.0/users/{0}/events/{1}";
}