using AF;
using AF.Events;
using TigerForge;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.GlobalVariables;

[CreateAssetMenu(fileName = "GUI Manager", menuName = "System/New GUI Manager", order = 0)]
public class GUIManager : ScriptableObject
{
    public bool hasActiveGUI = false;

#if UNITY_EDITOR
    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            Clear();
        }
    }
#endif

    void Clear()
    {
        hasActiveGUI = false;
    }

    public void SetHasActiveGUI(bool value)
    {
        hasActiveGUI = value;
    }

}
