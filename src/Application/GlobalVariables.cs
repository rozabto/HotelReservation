using System;

namespace Application
{
    public static class GlobalVariables
    {
        public static bool IsEnvironmentProduction =
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
    }
}
