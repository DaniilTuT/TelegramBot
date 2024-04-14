namespace Domain.Primitives;

/// <summary>
/// Содержит шаблоны сообщений об ошибках для валидации.
/// </summary>
public static class ValidationMessages
{
    /// <summary>
    /// Сообщение об ошибке, когда значение равно null.
    /// </summary>
    public static string NotNull(string paramName)
    {
        return $"{paramName} должно быть заполнено";
    }

    /// <summary>
    /// Сообщение об ошибке, когда значение пустое.
    /// </summary>
    public static string NotEmpty(string paramName)
    {
        return $"{paramName} не должно быть пустым";
    }

    /// <summary>
    /// Сообщение об ошибке, когда дата задана вне допустимых значений.
    /// </summary>
    public static string InvalidDate(string paramName)
    {
        return $"{paramName} является недопустимой датой";
    }

    /// <summary>
    /// Сообщение об ошибке, когда неверный формат ника в Telegram.
    /// </summary>
    public static string InvalidTelegramName(string paramName)
    {
        return $"{paramName} неверный формат ника в Telegram";
    }

    /// <summary>
    /// Сообщение об ошибке, когда неверный формат номера телефона.
    /// </summary>
    public static string InvalidPhoneNumber(string paramName)
    {
        return $"{paramName} неверный формат номера телефона";
    }


    /// <summary>
    /// Сообщение об ошибке, когда неверный формат имени.
    /// </summary>
    public static string InvalidName(string paramName)
    {
        return $"{paramName} неверный формат имени";
    }

    /// <summary>
    /// Сообщение об ошибке, когда перечисление имеет значение по умолчанию.
    /// </summary>
    public static string DefaultEnum(string paramName)
    {
        return $"{paramName} перечисление имеет значение по умолчанию";
    }
}
