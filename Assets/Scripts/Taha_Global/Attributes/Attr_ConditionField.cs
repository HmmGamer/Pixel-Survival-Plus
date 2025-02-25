using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConditionFieldAttribute : PropertyAttribute
{
    public string BoolVariableName;
    public bool Reverse;

    public ConditionFieldAttribute(string boolVariableName, bool reverse = false)
    {
        BoolVariableName = boolVariableName;
        Reverse = reverse;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ConditionFieldAttribute))]
public class ConditionFieldDrawer : PropertyDrawer
{
    private static readonly Dictionary<string, SerializedProperty> BoolPropertyCache = new Dictionary<string, SerializedProperty>();
    private static readonly Dictionary<string, bool> BoolValueCache = new Dictionary<string, bool>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (ShouldShow(property))
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return ShouldShow(property) ? EditorGUI.GetPropertyHeight(property) : 0;
    }

    private bool ShouldShow(SerializedProperty property)
    {
        ConditionFieldAttribute attr = (ConditionFieldAttribute)attribute;
        SerializedObject targetObject = property.serializedObject;
        string cacheKey = GetCacheKey(targetObject, attr.BoolVariableName);

        if (!BoolPropertyCache.TryGetValue(cacheKey, out SerializedProperty boolProp))
        {
            boolProp = targetObject.FindProperty(attr.BoolVariableName);
            BoolPropertyCache[cacheKey] = boolProp;
        }
        else if (boolProp.serializedObject != targetObject)
        {
            boolProp = targetObject.FindProperty(attr.BoolVariableName);
            BoolPropertyCache[cacheKey] = boolProp;
        }

        bool currentValue = boolProp?.boolValue ?? false;
        currentValue = attr.Reverse ? !currentValue : currentValue;

        if (BoolValueCache.TryGetValue(cacheKey, out bool cachedValue))
        {
            if (currentValue != cachedValue)
            {
                BoolValueCache[cacheKey] = currentValue;
                EditorApplication.delayCall += () => RepaintInspector(targetObject.targetObject);
            }
        }
        else
        {
            BoolValueCache[cacheKey] = currentValue;
        }

        return currentValue;
    }

    private static string GetCacheKey(SerializedObject target, string boolPath)
    {
        return $"{target.targetObject.GetInstanceID()}_{boolPath}";
    }

    private static void RepaintInspector(Object target)
    {
        Editor[] editors = ActiveEditorTracker.sharedTracker.activeEditors;
        foreach (Editor editor in editors)
        {
            if (editor.target == target && editor is UnityEditor.Editor)
            {
                editor.Repaint();
                break;
            }
        }
    }
}
#endif