using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectControl {
    public partial class EffectGeneric : Effect {
        public bool followPosition;
        public bool followRotation;
        public EffectAnchorEnum positionAnchor;
        public EffectAnchorEnum rotationAnchor;
        Transform positionAnchorTrans;
        Transform rotationAnchorTrans;
        public override void Play() {
            StartEmission();
        }
        public override void Finish() {
            StopEmission();
        }
        public override void Initialize(EffectArgs args) {
            if (maintain) {
                TimeRemnant = args.timeRemnant;
            }
            Target = args.target;
            positionAnchorTrans = Target.GetComponent<EffectAnchor>()?.GetAnchor(positionAnchor) ?? Target;
            rotationAnchorTrans = Target.GetComponent<EffectAnchor>()?.GetAnchor(rotationAnchor) ?? Target;
            SetPosition();
            if (followRotation) {
                SetRotation();
            }
        }
        public override void EffectUpdate() {
            if (Target) {
                SetTransform();
            }
        }
        protected void SetTransform() {
            if (followPosition) {
                SetPosition();
            }
            if (followRotation) {
                SetRotation();
            }
        }
        protected void SetRotation() {
            transform.rotation = rotationAnchorTrans.rotation;
        }
        protected void SetPosition() {
            transform.position = positionAnchorTrans.position;
        }
        protected void StartEmission() {
            var particle = GetComponent<ParticleSystem>();
            if (particle) {
                particle.Play();
            }
            var trail = GetComponent<TrailRenderer>();
            if (trail) {
                trail.emitting = true;
            }
        }
        protected void StopEmission() {
            var particle = transform.GetComponent<ParticleSystem>();
            if (particle) {
                particle.Stop(true);
            }
            var trail = transform.GetComponent<TrailRenderer>();
            if (trail) {
                trail.emitting = false;
                trail.Clear();
            }
        }
        public override void Modify(EffectArgs args) {
            TimeRemnant = args.timeRemnant;
        }
    }
}