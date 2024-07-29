namespace Core;

/// <summary>
/// Интерфейс проверки лицензии.
/// </summary>
public interface ILicense
{
    /// <summary>
    /// Возвращает флаг доступности лицензии.
    /// </summary>
    bool IsValidLicenseAvailable { get; }

    /// <summary>
    /// Возвращает включена ли проверка окончания лицензии по интервалу времени.
    /// </summary>
    bool IsEvaluationLockEnable { get; }

    /// <summary>
    /// Возвращает время работы лицензии.
    /// </summary>
    int EvaluationTime { get; }

    /// <summary>
    /// Возвращает текущее остаточное время работы лицензии.
    /// </summary>
    int EvaluationTimeCurrent { get; }

    /// <summary>
    /// Возвращает тип доступа работы лицензии.
    /// </summary>
    EvaluationType EvaluationType { get; }

    /// <summary>
    /// Возвращает включена ли проверка окончания лицензии по определенной дате.
    /// </summary>
    bool ExpirationDateLockEnable { get; }

    /// <summary>
    /// Возвращает дату окончания работы лицензии.
    /// </summary>
    DateTime ExpirationDate { get; }

    /// <summary>
    /// Возвращает включена ли проверка окончания лицензии по количеству использований.
    /// </summary>
    bool NumberOfUsesLockEnable { get; }

    /// <summary>
    /// Возвращает максимальное количество использований.
    /// </summary>
    int MaxUses { get; }

    /// <summary>
    /// Текущее количество использований.
    /// </summary>
    int CurrentUses { get; }

    /// <summary>
    /// Возвращает включена ли проверка окончания лицензии по количеству экземпляров лицензии.
    /// </summary>
    bool NumberOfInstancesLockEnable { get; }

    /// <summary>
    /// Максимальное количество экземпляров лицензии.
    /// </summary>
    int MaxInstances { get; }

    /// <summary>
    /// Возвращает включена ли проверка на конкретный HardwareID.
    /// </summary>
    bool HardwareLockEnabled { get; }

    /// <summary>
    /// Возвращает HardwareID, для которой была сгенерирована лицензия.
    /// </summary>
    string LicenseHardwareID { get; }

    /// <summary>
    /// Возвращает текущий HardwareID компьютера.
    /// </summary>
    string CurrentHardwareID { get; }

    /// <summary>
    /// Возвращает дополнительную информацию о лицензии.
    /// </summary>
    /// <returns>Возвращает словарь.</returns>
    Dictionary<object, object?> GetAdditonalLicenseInformation();

    /// <summary>
    /// Сравнивает HardwareID лицензии и текущего компьютера.
    /// </summary>
    /// <returns>True, если HardwareID одинаковы. Иначе false.</returns>
    bool CompareHardwareID();

    /// <summary>
    /// Делает лицензию недействительной.
    /// </summary>
    /// <returns>Строка с кодом подтверждением.</returns>
    string InvalidateLicense();

    /// <summary>
    /// Проверяет, действителен ли код подтверждения.
    /// </summary>
    /// <param name="confirmationCode">Код подтверждения.</param>
    /// <returns>True, если код подтверждения действителен. Иначе false.</returns>
    bool CheckConfirmationCode(string confirmationCode);

    /// <summary>
    /// Переактивирует лицензию
    /// </summary>
    /// <param name="reactivationCode">Код переактивации.</param>
    /// <returns>True, если код переактивации действителен. Иначе false.</returns>
    bool ReactivateLicense(string reactivationCode);

    /// <summary>
    /// Загружает лицензию из файла.
    /// </summary>
    /// <param name="filename">Путь до файла лицензии.</param>
    /// <returns>True, если лицензия действительна. Иначе false.</returns>
    bool LoadLicense(string filename);

    /// <summary>
    /// Загружает лицензию из массива байт.
    /// </summary>
    /// <param name="license">Массив байт лицензии.</param>
    /// <returns></returns>
    bool LoadLicense(byte[] license);

    /// <summary>
    /// Возвращает лицензию в виде набора байт.
    /// </summary>
    /// <returns>Лицензия в виде набора байт.</returns>
    byte[] GetLicense();
}