using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AF
{

    [CreateAssetMenu(fileName = "GUI Manager", menuName = "System/New GUI Manager", order = 0)]
    public class GUIManager : ScriptableObject
    {
        private Stack<UIWindow> activeWindows = new Stack<UIWindow>();

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
            activeWindows.Clear();
        }

        public void PushWindow(UIWindow window)
        {
            if (window != null && !activeWindows.Contains(window))
            {
                activeWindows.Push(window);
            }
        }

        public void PopWindow()
        {
            if (activeWindows.Count > 0)
            {
                activeWindows.Pop();
            }
        }

        public UIWindow PeekWindow()
        {
            return activeWindows.Count > 0 ? activeWindows.Peek() : null;
        }

        public bool HasActiveWindow()
        {
            return activeWindows.Count > 0;
        }

        public void RemoveWindow(UIWindow window)
        {
            if (activeWindows.Contains(window))
            {
                var tempList = new List<UIWindow>(activeWindows);
                tempList.Remove(window);
                activeWindows.Clear();
                for (int i = tempList.Count - 1; i >= 0; i--)
                {
                    activeWindows.Push(tempList[i]);
                }
            }
        }
    }

}