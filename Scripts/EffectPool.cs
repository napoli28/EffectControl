using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectControl
{
    public class EffectPool : MonoBehaviour
    {
        EffectManager manager;
        internal EffectLibrary library;
        Transform poolGO;
        Dictionary<string, List<Effect>> pool;
        void Start()
        {
            manager = EffectManager.Instance;
            library = EffectManager.Instance.library;
            pool = new Dictionary<string, List<Effect>>();
            if (poolGO) return;
            poolGO = new GameObject("Pool").transform;
            poolGO.SetParent(manager.transform);

            foreach (var name in library.library.Keys)
            {
                pool.Add(name, new List<Effect>());
            }

        }
        public Effect Spawn(string name)
        {
            //Debug.Log("Spawn : " + type);
            Effect effect;
            if (pool[name] == null || pool[name].Count == 0)
            {
                var prefab = library.GetEffectGameObject(name);
                if (prefab == null) return null;
                effect = Instantiate(prefab);
                effect.SetName(name);
                effect.gameObject.SetActive(false);
            }
            else
            {
                var list = pool[name];
                effect = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
            }
            return effect;
        }
        public void Despawn(Effect effect)
        {
            if (!pool.ContainsKey(effect.Name))
            {
                pool.Add(effect.Name, new List<Effect>());
            }
            effect.gameObject.SetActive(false);
            pool[effect.Name].Add(effect);
            effect.transform.SetParent(poolGO);
        }

        [ContextMenu("Clear")]
        public void ClearPool()
        {
            foreach (var item in pool.Values)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    Destroy(item[i].gameObject);
                }
                item.Clear();
            }
        }
    }
}