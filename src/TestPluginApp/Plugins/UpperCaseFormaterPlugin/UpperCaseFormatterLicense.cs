using System.Collections;
using Core;

namespace UpperCaseFormatterPlugin;

/// <summary>
/// Класс лицензии для класса UpperCaseFormatterLicense.
/// </summary>
internal class UpperCaseFormatterLicense : ILicense
{
    /// <summary>
    /// Словарь типов доступа к лицензии.
    /// </summary>
    private readonly Dictionary<License.EvaluationType, EvaluationType> _evaluationTypes = new()
    {
        [License.EvaluationType.Runtime_Minutes] = EvaluationType.RuntimeMinutes,
        [License.EvaluationType.Trial_Days] = EvaluationType.TrialDays,
    };

    /// <inheritdoc/>
    public bool IsValidLicenseAvailable => License.Status.Licensed;

    /// <inheritdoc/>
    public bool IsEvaluationLockEnable => License.Status.Evaluation_Lock_Enabled;

    /// <inheritdoc/>
    public int EvaluationTime => License.Status.Evaluation_Time;

    /// <inheritdoc/>
    public int EvaluationTimeCurrent => License.Status.Evaluation_Time_Current;

    /// <inheritdoc/>
    public EvaluationType EvaluationType => _evaluationTypes[License.Status.Evaluation_Type];

    /// <inheritdoc/>
    public bool ExpirationDateLockEnable => License.Status.Expiration_Date_Lock_Enable;

    /// <inheritdoc/>
    public DateTime ExpirationDate => License.Status.Expiration_Date;

    /// <inheritdoc/>
    public bool NumberOfUsesLockEnable => License.Status.Number_Of_Uses_Lock_Enable;

    /// <inheritdoc/>
    public int MaxUses => License.Status.Number_Of_Uses;

    /// <inheritdoc/>
    public int CurrentUses => License.Status.Number_Of_Uses_Current;

    /// <inheritdoc/>
    public bool NumberOfInstancesLockEnable => License.Status.Number_Of_Instances_Lock_Enable;

    /// <inheritdoc/>
    public int MaxInstances => License.Status.Number_Of_Instances;

    /// <inheritdoc/>
    public bool HardwareLockEnabled => License.Status.Hardware_Lock_Enabled;

    /// <inheritdoc/>
    public string LicenseHardwareID => License.Status.License_HardwareID;

    /// <inheritdoc/>
    public string CurrentHardwareID => License.Status.HardwareID;

    /// <inheritdoc/>
    public Dictionary<object, object?> GetAdditonalLicenseInformation()
    {
        var keyValuePair = new Dictionary<object, object?>();
        foreach (DictionaryEntry entry in License.Status.KeyValueList)
        {
            keyValuePair.Add(entry.Key, entry.Value);
        }

        return keyValuePair;
    }

    /// <inheritdoc/>
    public bool CompareHardwareID()
    {
        return License.Status.HardwareID == License.Status.License_HardwareID;
    }

    /// <inheritdoc/>
    public string InvalidateLicense()
    {
        return License.Status.InvalidateLicense();
    }

    /// <inheritdoc/>
    public bool CheckConfirmationCode(string confirmationCode)
    {
        return License.Status.CheckConfirmationCode(License.Status.HardwareID, confirmationCode);
    }

    /// <inheritdoc/>
    public bool ReactivateLicense(string reactivationCode)
    {
        return License.Status.ReactivateLicense(reactivationCode);
    }

    /// <inheritdoc/>
    public bool LoadLicense(string filename)
    {
        License.Status.LoadLicense(filename);
        return License.Status.Licensed;
    }

    /// <inheritdoc/>
    public bool LoadLicense(byte[] license)
    {
        License.Status.LoadLicense(license);
        return License.Status.Licensed;
    }

    /// <inheritdoc/>
    public byte[] GetLicense()
    {
        return License.Status.License;
    }
}