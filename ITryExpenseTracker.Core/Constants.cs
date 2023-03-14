namespace ITryExpenseTracker.Core;

public static class Constants
{
    public static class UserRoles
    {
        public static string ADMIN = "admin";

        public static string USER = "user";

        public static string[] ALL_ROLES = { ADMIN, USER };
    }

    public static class Recurrence
    {
        public static int MONTHLY = 1;
    }

    public static class MySqlServer
    {
        public static Version GetVersion() => new Version(8, 0, 21);
    }
}
