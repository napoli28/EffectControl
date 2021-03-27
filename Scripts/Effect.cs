using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

namespace EffectControl
{
    public abstract class Effect : MonoBehaviour
    {
        public string Name { get; private set; }
        public Transform Target;
        public float despawnCountdown;
        [HideInInspector]
        public float despawnCountdownRemnant;
        [HideInInspector]
        public bool finished;
        public bool maintain;
        public float? TimeRemnant { get; set; }
        public bool FinishWhenLostTarget;
        public bool DetachWhenFinish;
        public virtual void ManagerInitialize() { }
        public virtual void Initialize(EffectArgs args) { }
        public virtual void Play() { }
        public virtual void Finish() { }
        public virtual void Modify(EffectArgs args) { }
        public virtual void EffectUpdate() { }
        public virtual void EffectLateUpdate() { }

        public void SetName(string name)
        {
            Name = name;
        }
        protected void AutoDespawn()
        {
            if (despawnCountdown == 0)
            {
                EffectManager.ReadyToDespawn(this);
                return;
            }
            Timer timer = new Timer(despawnCountdown * 1000);
            timer.AutoReset = false;
            timer.Elapsed += new ElapsedEventHandler((x, y) =>
            {
                EffectManager.ReadyToDespawn(this);
                timer.Dispose();
            });
            timer.Enabled = true;
            timer.Start();
            finished = true;

        }
        protected void SetParent(Transform transform)
        {
            this.transform.SetParent(transform);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            this.transform.localScale = Vector3.one;
        }
        protected void SetTransform(Transform origin, Transform target)
        {
            origin.transform.position = target.transform.position;
            origin.transform.rotation = target.transform.rotation;
        }
    }
}