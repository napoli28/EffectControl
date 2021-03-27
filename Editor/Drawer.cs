using SKUnityToolkit.SerializableDictionary;
using UnityEditor;

namespace EffectControl
{
    [CustomPropertyDrawer(typeof(StringEffectDictionary))]
    public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }
}
