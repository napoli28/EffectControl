using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnchor : MonoBehaviour
{
    public Transform Head;
    public Transform LeftHand;
    public Transform Righthand;
    public Transform LeftFoot;
    public Transform RightFoot;
    public Transform LeftBackFoot;
    public Transform RightBackFoot;
    public Transform LeftWeapon;
    public Transform RightWeapon;
    public Transform Center;
    public Transform OverHead;

    public Transform WeaponStart;
    public Transform WeaponEnd;

    public Transform GetAnchor(EffectAnchorEnum anchor)
    {
        switch (anchor)
        {
            case EffectAnchorEnum.Root:
                return transform;
            case EffectAnchorEnum.Head:
                return Head;
            case EffectAnchorEnum.LeftHand:
                return LeftHand;
            case EffectAnchorEnum.Righthand:
                return Righthand;
            case EffectAnchorEnum.LeftFoot:
                return LeftFoot;
            case EffectAnchorEnum.RightFoot:
                return RightFoot;
            case EffectAnchorEnum.LeftBackFoot:
                return LeftBackFoot;
            case EffectAnchorEnum.RightBackFoot:
                return RightBackFoot;
            case EffectAnchorEnum.LeftWeapon:
                return LeftWeapon;
            case EffectAnchorEnum.RightWeapon:
                return RightWeapon;
            case EffectAnchorEnum.Center:
                return Center;
            case EffectAnchorEnum.OverHead:
                return OverHead;
            default:
                return null;
        }
    }
}

public enum EffectAnchorEnum
{
    Root,
    Head,
    LeftHand,
    Righthand,
    LeftFoot,
    RightFoot,
    LeftBackFoot,
    RightBackFoot,
    LeftWeapon,
    RightWeapon,
    Center,
    OverHead,
}
