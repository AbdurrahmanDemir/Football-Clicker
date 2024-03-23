using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(DailyReward))]
public class DailyRewardPD : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        EditorGUI.LabelField(position, property.displayName);

        EditorGUILayout.PropertyField(property.FindPropertyRelative("rewardType"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("amount"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("icon"));

        if(property.FindPropertyRelative("rewardType").enumValueIndex == (int)DailyRewardType.Upgrade)
            EditorGUILayout.PropertyField(property.FindPropertyRelative("upgradeIndex"));
    }
}

#endif
