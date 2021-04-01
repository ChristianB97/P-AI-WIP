using UnityEngine;
using System;
using System.Runtime.CompilerServices;

namespace Save
{
    public class SaveLoadEvent : MonoBehaviour
    {
        public static Action OnSave;
        public static Action OnLoad;

        public static void SaveAll()
        {
            OnSave?.Invoke();
        }

        public static void LoadAll()
        {
            OnLoad?.Invoke();
        }
    }
}
