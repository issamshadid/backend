namespace Template.API.ErrorHandling;

/// <summary>
/// </summary>
public static class LoggerExtensions
{
    #region Debug

    /// <summary>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="messageTemplate"></param>
    public static void Debug(this ILogger logger, string messageTemplate)
    {
        logger.Log(LogLevel.Debug, null, messageTemplate);
    }

    /// <summary>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="messageTemplate"></param>
    /// <param name="propertyValues"></param>
    public static void Debug(this ILogger logger, string messageTemplate, params object?[] propertyValues)
    {
        logger.Log(LogLevel.Debug, null, messageTemplate, propertyValues);
    }

    /// <summary>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="messageTemplate"></param>
    /// <param name="propertyValues"></param>
    public static void Debug(this ILogger logger, Exception exception, string messageTemplate,
        params object?[] propertyValues)
    {
        logger.Log(LogLevel.Debug, exception, messageTemplate, propertyValues);
    }

    #endregion

    #region Information

    /// <summary>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="messageTemplate"></param>
    public static void Information(this ILogger logger, string messageTemplate)
    {
        logger.Log(LogLevel.Information, null, messageTemplate);
    }

    /// <summary>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="messageTemplate"></param>
    /// <param name="propertyValues"></param>
    public static void Information(this ILogger logger, string messageTemplate, params object?[] propertyValues)
    {
        logger.Log(LogLevel.Information, null, messageTemplate, propertyValues);
    }

    /// <summary>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="messageTemplate"></param>
    /// <param name="propertyValues"></param>
    public static void Information(this ILogger logger, Exception exception, string messageTemplate,
        params object?[] propertyValues)
    {
        logger.Log(LogLevel.Information, exception, messageTemplate, propertyValues);
    }

    #endregion

    #region Warning

    /// <summary>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="messageTemplate"></param>
    public static void Warning(this ILogger logger, string messageTemplate)
    {
        logger.Log(LogLevel.Warning, null, messageTemplate);
    }

    /// <summary>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="messageTemplate"></param>
    /// <param name="propertyValues"></param>
    public static void Warning(this ILogger logger, string messageTemplate, params object?[] propertyValues)
    {
        logger.Log(LogLevel.Warning, null, messageTemplate, propertyValues);
    }

    /// <summary>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="messageTemplate"></param>
    /// <param name="propertyValues"></param>
    public static void Warning(this ILogger logger, Exception exception, string messageTemplate,
        params object?[] propertyValues)
    {
        logger.Log(LogLevel.Warning, exception, messageTemplate, propertyValues);
    }

    #endregion

    #region Error

    /// <summary>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="messageTemplate"></param>
    public static void Error(this ILogger logger, string messageTemplate)
    {
        logger.Log(LogLevel.Error, null, messageTemplate);
    }

    /// <summary>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="messageTemplate"></param>
    /// <param name="propertyValues"></param>
    public static void Error(this ILogger logger, string messageTemplate, params object?[] propertyValues)
    {
        logger.Log(LogLevel.Error, null, messageTemplate, propertyValues);
    }

    /// <summary>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="messageTemplate"></param>
    /// <param name="propertyValues"></param>
    public static void Error(this ILogger logger, Exception exception, string messageTemplate,
        params object?[] propertyValues)
    {
        logger.Log(LogLevel.Error, exception, messageTemplate, propertyValues);
    }

    #endregion
}