namespace Taggly.Exceptions.Helpers;

public static class ExceptionHelper
{
    public static string GetFullMessage(Exception ex)
    {
        if (ex == null) return string.Empty;
        return $"{ex.GetType().Name}: {ex.Message} {ex.InnerException?.Message}";
    }
}
