#region HeadComments
/********************************************************************
* Copyright © 2020-2020 Administrator. All rights reserved.
*文件路径：Assets/Scripts/EffectControl/ReflectionCamera.cs
*作    者：Administrator
*文件版本：1.0.0
*Unity版本：2019.4.6f1
*创建日期：2020/11/16 16:16:21
*功能描述：nothing
*历史记录：
*********************************************************************/
#endregion


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionCamera : MonoBehaviour
{
    public Transform mainCamera;
    public Transform reflectPlane;

    private void Start()
    {
        var pos = reflectPlane.worldToLocalMatrix.MultiplyPoint(mainCamera.position);
        Debug.Log(pos);
        pos.y = -pos.y;
        pos = reflectPlane.localToWorldMatrix.MultiplyPoint(pos);
        transform.position = pos;
        var angles = mainCamera.eulerAngles;
        angles.x = -angles.x;
        transform.eulerAngles = angles;
        GetComponent<Camera>().fieldOfView = mainCamera.GetComponent<Camera>().fieldOfView;
    }
}
