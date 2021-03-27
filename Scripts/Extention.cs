#region HeadComments
/********************************************************************
* Copyright © 2020-2020 Administrator. All rights reserved.
*文件路径：Assets/Scripts/EffectControl/Extention.cs
*作    者：Administrator
*文件版本：1.0.0
*Unity版本：2019.4.6f1
*创建日期：2020/09/24 17:52:15
*功能描述：nothing
*历史记录：
*********************************************************************/
#endregion


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extention
{
    #region List<T>
    public static int ToBackRemove<T>(this List<T> list, T element)
    {
        //if (list.IndexOf(element) > list.Count)
        //{
        //    Debug.LogError(list.Count + " < " + list.IndexOf(element));
        //}
        //if (list == null)
        //{
        //    Debug.LogError("list is null");
        //}
        var index = list.IndexOf(element);
        if (index >= 0)
        {
            list[index] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
        }
        return index;
    }

    public static T ToBackRemoveAt<T>(this List<T> list, int index)
    {
        T element = list[index];
        list[index] = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return element;
    }
    #endregion
    #region Color
    public static Color NormalizeTo(this Color color, float brightness)
    {
        var bright = color.r * 0.299f + color.g * 0.587f + color.b * 0.114f;
        color *= brightness / bright;
        return color;
    }
    #endregion
}
