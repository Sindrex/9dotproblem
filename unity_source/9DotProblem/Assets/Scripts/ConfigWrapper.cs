using UnityEngine;

[System.Serializable]
public class ConfigWrapper
{
    public string Url;
    public int TimeLimitSeconds;
    public bool ShowTimer;
    public string HelpText;
    public bool ShowHelpText;
    public string Title;
    public string RedirectUrl;
    public int RedirectTime;

    public static ConfigWrapper CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ConfigWrapper>(jsonString);
    }
}
