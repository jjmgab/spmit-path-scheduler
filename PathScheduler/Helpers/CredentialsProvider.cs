using System;

namespace PathScheduler.Helpers
{
    public static class CredentialsProvider
    {
        private static string _apiKey;
        public static string ApiKey {
            get
            {
                PedanticCheck();
                return _apiKey;
            }
        }

        private static bool _isInit = false;

        public static void Initialize(string apiKey)
        {
            _apiKey = apiKey;
            _isInit = true;
        }

        private static void PedanticCheck()
        {
            if (!_isInit)
                throw new InvalidOperationException("Class CredentialsProvider was not initialized.");
        }
    }
}
