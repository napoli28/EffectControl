using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectControl
{
    [RequireComponent(typeof(EffectPool))]
    [RequireComponent(typeof(EffectLibrary))]
    public class EffectManager : MonoBehaviour
    {
        #region Singleton
        static EffectManager instance;
        public static EffectManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("EffectManager");
                    instance = go.AddComponent<EffectManager>();
                    DontDestroyOnLoad(go);
                    //instance = new GameObject("EffectManager").AddComponent<EffectManager>();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }
        private void OnEnable()
        {
            Instance = this;
        }
        #endregion

        Transform active;
        internal EffectLibrary library;
        EffectPool pool;
        List<Effect> effects = new List<Effect>(100);

        Dictionary<Transform, Dictionary<string, Effect>> targetSheet = new Dictionary<Transform, Dictionary<string, Effect>>();
        List<Effect> despawnBuffer = new List<Effect>(10);

        float deltaTime;
        int frame;

        #region MonoBehaviour
        private void Awake()
        {
            library = GetComponent<EffectLibrary>();
            pool = GetComponent<EffectPool>();
            if (active) return;
            active = new GameObject("Active").transform;
            active.SetParent(transform);
            var effects = library.library.Values;
            foreach (var item in effects)
            {
                item.ManagerInitialize();
            }
        }
        private void Update()
        {
            RegularClearNullTarget();
            deltaTime = Time.deltaTime;
            for (int i = 0; i < effects.Count; i++)
            {
                var effect = effects[i];
                if (effect.maintain)
                {
                    FinishCheck(effect);
                }
                effect.EffectUpdate();

                if (effect.finished)
                {
                    UpdateFinished(effect);
                }
            }
            ClearDespawnBuffer();
        }
        private void LateUpdate()
        {
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].EffectLateUpdate();
            }
        }
        #endregion

        #region Public Static Method
        public static void AutoEffect(Transform target, string name)
        {

            AutoEffect(new EffectArgs(name, target));
        }
        public static void AutoEffect(EffectArgs args)
        {
            if (Instance.targetSheet.NestedContainsKey(args.target, args.name))
            {
                var effect = Instance.targetSheet[args.target][args.name];
                if (effect.maintain)
                {
                    effect.Modify(args);
                    return;
                }
            }
            else
            {
                Instance.AddEffect(args);
            }
        }
        public static void NewEffect(Transform target, string name, float? timeRemnant = null, object customArgs = null)
        {
            NewEffect(new EffectArgs(name, target, timeRemnant, customArgs));
        }
        public static void NewEffect(EffectArgs args)
        {
            if (Instance.targetSheet.NestedContainsKey(args.target, args.name)) return;
                Instance.AddEffect(args);
        }
        public static void ModifyEffect(Transform target, string name, object customArg)
        {
            ModifyEffect(new EffectArgs(name, target, customArg));
        }
        public static void ModifyEffect(EffectArgs args)
        {
            var effect = Instance.GetEffect(args);
            if (effect)
            {
                effect.Modify(args);
            }
        }

        public static void ReadyToDespawn(Effect effect)
        {
            Instance.despawnBuffer.Add(effect);
        }
        public static void ClearNullTarget()
        {
            var e = Instance.targetSheet.GetEnumerator();
            List<Transform> nullTargets = new List<Transform>();
            while (e.MoveNext())
            {
                if (e.Current.Key == null)
                {
                    nullTargets.Add(e.Current.Key);
                }
            }
            for (int i = 0; i < nullTargets.Count; i++)
            {
                Instance.targetSheet.Remove(nullTargets[i]);
            }
        }
        public static void Detach(Transform target, string name)
        {
            var effect = Instance.GetEffect(new EffectArgs(name, target));
            if (effect)
            {
                Instance.DetachInternal(effect);
            }
        }
        public static void DetachAll(Transform target)
        {
            if (!Instance.targetSheet.ContainsKey(target)) return;
            Effect[] effects = new Effect[Instance.targetSheet[target].Count];
            Instance.targetSheet[target].Values.CopyTo(effects, 0);
            foreach (var item in effects)
            {
                Instance.DetachInternal(item);
            }
        }
        public static void Finish(Transform target, string name)
        {
            var effect = Instance.GetEffect(new EffectArgs(name, target));
            if (effect)
            {
                Instance.Finish(effect);
            }
        }
        public static void FinishEffectOn(Transform target)
        {
            if (!Instance.targetSheet.ContainsKey(target)) return;
            foreach (var item in Instance.targetSheet[target])
            {
                if (item.Value.maintain && !item.Value.finished)
                {
                    Instance.Finish(item.Value);
                }
            }
        }
        public static void FinishAllEffect()
        {
            foreach (var item in Instance.effects)
            {
                if (item.maintain && !item.finished)
                {
                    Instance.Finish(item);
                }
            }
        }
        public static void DespawnEffectOn(Transform target)
        {
            if (!Instance.targetSheet.ContainsKey(target)) return;
            foreach (var item in Instance.targetSheet[target])
            {
                ReadyToDespawn(item.Value);
            }
        }
        public static void DespawnAllEffect()
        {
            foreach (var item in Instance.effects)
            {
                ReadyToDespawn(item);
            }
        }
        #endregion

        #region Private Method
        Effect GetEffect(EffectArgs args)
        {
            Effect effect;
            targetSheet.NestedTryGetValue(args.target, args.name, out effect);
            return effect;
        }
        void AddEffect(EffectArgs args)
        {
            if (args.name == null)
            {
                Debug.LogWarning("name parameter can't be null when creat effect");
                return;
            }
            var effect = library.GetEffectGameObject(args.name);
            if (!effect)
            {
                return;
            }
            effect = pool.Spawn(args.name);
            effects.Add(effect);

            if (effect.maintain)
            {
                targetSheet.NestedTryAdd(args.target, args.name, effect);
            }
            effect.transform.SetParent(active);
            effect.finished = !effect.maintain;
            effect.Initialize(args);
            effect.despawnCountdownRemnant = effect.despawnCountdown;
            effect.gameObject.SetActive(true);
            effect.Play();
        }
        void FinishCheck(Effect effect)
        {
            if (!effect.TimeRemnant.HasValue) return;
            if (effect.TimeRemnant <= 0 || (!effect.Target && effect.FinishWhenLostTarget))
            {
                Finish(effect);
            }
            effect.TimeRemnant -= deltaTime;
        }
        void Finish(Effect effect)
        {
            effect.Finish();
            effect.TimeRemnant = null;
            if (effect.maintain)
            {
                Instance.targetSheet[effect.Target].Remove(effect.Name);
            }
            if (effect.DetachWhenFinish)
            {
                effect.Target = null;
            }
            effect.finished = true;
        }
        void UpdateFinished(Effect effect)
        {
            effect.despawnCountdownRemnant -= deltaTime;
            if (effect.despawnCountdownRemnant <= 0)
            {
                ReadyToDespawn(effect);
            }
        }
        void RegularClearNullTarget()
        {
            frame++;
            if (frame == 1000)
            {
                ClearNullTarget();
                frame = 0;
            }
        }
        void DetachInternal(Effect effect)
        {
            if (effect.FinishWhenLostTarget)
            {
                Finish(effect);
            }
            effect.Target = null;
        }
        void ClearDespawnBuffer()
        {
            for (int i = 0; i < despawnBuffer.Count; i++)
            {
                effects.ToBackRemove(despawnBuffer[i]);
#if UNITY_EDITOR
                Destroy(despawnBuffer[i].gameObject);
#else
                pool.Despawn(despawnBuffer[i]);
#endif
            }
            despawnBuffer.Clear();
        }
        #endregion
    }
}