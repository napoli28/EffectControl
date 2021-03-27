using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectControl
{
    public partial class EffectMultiParts : Effect
    {
        public bool followPosition;
        public bool followRotation;
        public List<EffectAnchorEnum> anchors;
        List<Transform> anchorsTrans;
        public List<Transform> parts;

        public override void Play()
        {
            PlayAllParts();
        }
        public override void Finish()
        {
            FinishAllParts();
        }
        public override void Initialize(EffectArgs args)
        {
            if (maintain)
            {
                TimeRemnant = args.timeRemnant;
            }
            Target = args.target;
            InitializeAnchors();
            SetAllPartsPosition();
            SetAllPartsRotation();
        }
        public override void EffectUpdate()
        {
            if (Target)
            {
                SetAllTransform();
            }
        }
        protected void InitializeAnchors()
        {
            if (parts == null || parts.Count == 0)
            {
                parts = new List<Transform> { transform };
            }
            var anchor = Target.GetComponent<EffectAnchor>();
            if (anchor == null || anchors == null || anchors.Count == 0)
            {
                anchorsTrans = new List<Transform> { Target };
            }
            else
            {
                anchorsTrans = new List<Transform>(anchors.Count);
                for (int i = 0; i < anchors.Count; i++)
                {
                    anchorsTrans.Add(anchor.GetAnchor(anchors[i]));
                    if (!anchorsTrans[i])
                    {
                        parts[i].gameObject.SetActive(false);
                    }
                }
            }
        }
        protected void SetAllTransform()
        {
            if (followPosition)
            {
                SetAllPartsPosition();
            }
            if (followRotation)
            {
                SetAllPartsRotation();
            }
        }
        protected void SetAllPartsRotation()
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (anchorsTrans[i])
                {
                    parts[i].rotation = anchorsTrans[i].rotation;
                }
            }
        }
        protected void SetAllPartsPosition()
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (anchorsTrans[i])
                {
                    parts[i].position = anchorsTrans[i].position;
                }
            }
        }
        protected void PlayAllParts()
        {
            for (int i = 0; i < parts.Count; i++)
            {
                var particle = parts[i].GetComponent<ParticleSystem>();
                if (particle)
                {
                    particle.Play();
                }
                var trail = parts[i].GetComponent<TrailRenderer>();
                if (trail)
                {
                    trail.emitting = true;
                }
            }
        }
        protected void FinishAllParts()
        {
            for (int i = 0; i < parts.Count; i++)
            {
                var particle = parts[i].GetComponent<ParticleSystem>();
                if (particle)
                {
                    particle.Stop(true);
                }
                var trail = parts[i].GetComponent<TrailRenderer>();
                if (trail)
                {
                    trail.emitting = false;
                    trail.Clear();
                }
            }
        }
        public override void Modify(EffectArgs args)
        {
            TimeRemnant = args.timeRemnant;
        }
    }
}