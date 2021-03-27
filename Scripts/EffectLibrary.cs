using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectControl
{
    public class EffectLibrary : MonoBehaviour
    {
        public StringEffectDictionary library;
        public Effect GetEffectGameObject(string name)
        {
            if (!library.ContainsKey(name))
            {
                Debug.Log(string.Format("Effect name \"{0}\" not exist", name));
                return null;
            }
            if (!library[name])
            {
                Debug.Log(string.Format("The effect named \"{0}\" is null", name));
                return null;
            }
            return library[name];
        }
    }
}