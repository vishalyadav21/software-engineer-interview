using System;

namespace Zip.Installments.WebApi.Helpers
{
    [Serializable]
    public class CustomExceptionFilter : Exception
    {
        public CustomExceptionFilter(string message) : base(message)
        {
        }
    }
}
