using System.Collections;
using License;

namespace Core;

/// <summary>
/// Класс лицензии для проекта Core.
/// </summary>
public class CoreLicense : ILicense
{
    /// <summary>
    /// Словарь типов доступа к лицензии.
    /// </summary>
    private readonly Dictionary<License.EvaluationType, EvaluationType> _evaluationTypes = new ()
    {
        [License.EvaluationType.Runtime_Minutes] = EvaluationType.RuntimeMinutes,
        [License.EvaluationType.Trial_Days] = EvaluationType.TrialDays,
    };

    /// <inheritdoc/>
    public bool IsValidLicenseAvailable => Status.Licensed;

    /// <inheritdoc/>
    public bool IsEvaluationLockEnable => Status.Evaluation_Lock_Enabled;

    /// <inheritdoc/>
    public int EvaluationTime => Status.Evaluation_Time;

    /// <inheritdoc/>
    public int EvaluationTimeCurrent => Status.Evaluation_Time_Current;

    /// <inheritdoc/>
    public EvaluationType EvaluationType => _evaluationTypes[Status.Evaluation_Type];

    /// <inheritdoc/>
    public bool ExpirationDateLockEnable => Status.Expiration_Date_Lock_Enable;

    /// <inheritdoc/>
    public DateTime ExpirationDate => Status.Expiration_Date;

    /// <inheritdoc/>
    public bool NumberOfUsesLockEnable => Status.Number_Of_Uses_Lock_Enable;

    /// <inheritdoc/>
    public int MaxUses => Status.Number_Of_Uses;

    /// <inheritdoc/>
    public int CurrentUses => Status.Number_Of_Uses_Current;

    /// <inheritdoc/>
    public bool NumberOfInstancesLockEnable => Status.Number_Of_Instances_Lock_Enable;

    /// <inheritdoc/>
    public int MaxInstances => Status.Number_Of_Instances;

    /// <inheritdoc/>
    public bool HardwareLockEnabled => Status.Hardware_Lock_Enabled;

    /// <inheritdoc/>
    public string LicenseHardwareID => Status.License_HardwareID;

    /// <inheritdoc/>
    public string CurrentHardwareID => Status.HardwareID;

    /// <inheritdoc/>
    public Dictionary<object, object?> GetAdditonalLicenseInformation()
    {
        var keyValuePair = new Dictionary<object, object?>();
        foreach (DictionaryEntry entry in Status.KeyValueList)
        {
            keyValuePair.Add(entry.Key, entry.Value);
        }

        return keyValuePair;
    }

    /// <inheritdoc/>
    public bool CompareHardwareID()
    {
        return Status.HardwareID == Status.License_HardwareID;
    }

    /// <inheritdoc/>
    public string InvalidateLicense()
    {
        return Status.InvalidateLicense();
    }

    /// <inheritdoc/>
    public bool CheckConfirmationCode(string confirmationCode)
    {
        return Status.CheckConfirmationCode(Status.HardwareID, confirmationCode);
    }

    /// <inheritdoc/>
    public bool ReactivateLicense(string reactivationCode)
    {
        return Status.ReactivateLicense(reactivationCode);
    }

    /// <inheritdoc/>
    public bool LoadLicense(string filename)
    {
        Status.LoadLicense(filename);
        return Status.Licensed;
    }

    /// <inheritdoc/>
    public bool LoadLicense(byte[] license)
    {
        Status.LoadLicense(license);
        return Status.Licensed;
    }

    /// <inheritdoc/>
    public byte[] GetLicense()
    {
        return Status.License;
    }
}