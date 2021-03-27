using SKUnityToolkit.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EffectControl
{
    [Serializable]
    public class StringEffectDictionary : SerializableDictionary<string, Effect> { }
}