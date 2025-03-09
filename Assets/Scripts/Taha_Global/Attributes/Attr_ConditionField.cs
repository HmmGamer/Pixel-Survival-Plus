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
    private static readonly Dictionary<string, string> BoolPropertyPathCache = new Dictionary<string, string>();
    private static readonly Dictionary<string, bool> BoolValueCache = new Dictionary<string, bool>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (ShouldShow(property))
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return ShouldShow(property) ? EditorGUI.GetPropertyHeight(property, true) : 0;
    }

    private bool ShouldShow(SerializedProperty property)
    {
        ConditionFieldAttribute attr = (ConditionFieldAttribute)attribute;
        string cacheKey = GetCacheKey(property.serializedObject, attr.BoolVariableName);

        if (!BoolPropertyPathCache.TryGetValue(cacheKey, out string boolPath))
        {
            boolPath = FindBoolPropertyPath(property, attr.BoolVariableName);
            BoolPropertyPathCache[cacheKey] = boolPath;
        }

        if (string.IsNullOrEmpty(boolPath))
            return false;

        SerializedProperty boolProp = property.serializedObject.FindProperty(boolPath);
        if (boolProp == null)
            return false;

        bool currentValue = boolProp.boolValue;
        currentValue = attr.Reverse ? !currentValue : currentValue;

        if (BoolValueCache.TryGetValue(cacheKey, out bool cachedValue))
        {
            if (currentValue != cachedValue)
            {
                BoolValueCache[cacheKey] = currentValue;
                EditorApplication.delayCall += () => RepaintInspector(property.serializedObject.targetObject);
            }
        }
        else
        {
            BoolValueCache[cacheKey] = currentValue;
        }

        return currentValue;
    }

    private string FindBoolPropertyPath(SerializedProperty property, string boolPath)
    {
        SerializedObject targetObject = property.serializedObject;
        SerializedProperty boolProp = targetObject.FindProperty(boolPath);

        if (boolProp != null)
            return boolPath;

        if (property.propertyPath.Contains("."))
        {
            string parentPath = property.propertyPath.Substring(0, property.propertyPath.LastIndexOf('.'));
            SerializedProperty parentProperty = targetObject.FindProperty(parentPath);
            if (parentProperty != null)
            {
                boolProp = parentProperty.FindPropertyRelative(boolPath);
                if (boolProp != null)
                    return parentPath + "." + boolPath;
            }
        }

        return string.Empty;
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
