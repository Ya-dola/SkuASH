using UnityEngine;
using UnityEngine.Serialization;

public class AutoSaveConfig : ScriptableObject
{
    [FormerlySerializedAs("Enabled")]
    [Tooltip("Enable auto save functionality")]
    public bool enabled;

    [FormerlySerializedAs("Frequency")]
    [Tooltip("The frequency in minutes auto save will activate"), Min(1)]
    public int frequencyMins = 1;

    [FormerlySerializedAs("Logging")]
    [Tooltip("Log a message every time the scene is auto saved")]
    public bool logSave;
}