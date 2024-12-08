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

        public static Error NotImplemented(string description = "Functionality is not implemented")
        {
            return Error.Unexpected(
                code: $"{nameof(ApplicationErrors)}.{nameof(NotImplemented)}",
                description);
        }

        public static Error ValueNotPresent(string description = "Value is null or empty.")
        {
            return Error.Unexpected(
                code: $"{nameof(ApplicationErrors)}.{nameof(ValueNotPresent)}",
                description);
        }
    }
}
