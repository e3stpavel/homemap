using ErrorOr;

namespace Homemap.ApplicationCore.Errors
{
    public static class ApplicationErrors
    {
        public static Error EmptyOrCorruptedMessage(string description = "Failed to decode the message. It was empty or corrupted.")
        {
            return Error.Validation(
                code: $"{nameof(ApplicationErrors)}.{nameof(EmptyOrCorruptedMessage)}",
                description);
        }
    }
}
