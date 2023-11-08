using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorAutoSelect : MonoBehaviour
{
    void Awake()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorGUIUtility.PingObject(gameObject);
                UnityEditor.Selection.activeGameObject = gameObject;
        #endif

    }
}