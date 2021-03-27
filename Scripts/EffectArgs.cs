using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectControl
{
    public struct EffectArgs
    {
        public EffectArgs(string name, Transform target, float? timeRemnant = null, object custom = null)
        {
            this.name = name;
            this.target = target;
            this.timeRemnant = timeRemnant;
            customArg = custom;
        }
        public EffectArgs(string name, Transform target, object custom)
        {
            this.name = name;
            this.target = target;
            timeRemnant = null;
            customArg = custom;
        }
        /// <summary>
        /// 特效类型
        /// </summary>
        public string name;
        /// <summary>
        /// 特效目标
        /// </summary>
        public Transform target;
        /// <summary>
        /// 是否可持续
        /// </summary>
        public float? timeRemnant;

        public object customArg;
    }
    public interface ICustomArgument
    {
    }
}