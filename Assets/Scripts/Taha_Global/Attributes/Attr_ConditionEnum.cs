using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConditionalEnumAttribute : PropertyAttribute
{
    public string _enumField;
    public HashSet<int> _targetEnumValues;

    public ConditionalEnumAttribute(string enumFieldName, params int[] targetEnumValues)
    {
        _enumField = enumFieldName;
        _targetEnumValues = new HashSet<int>(targetEnumValues);
    }
}

[CustomPropertyDrawer(typeof(ConditionalEnumAttribute))]
public class ConditionalEnumDrawer : PropertyDrawer
{
    private SerializedObject _serializedObject;
    private SerializedProperty _enumProperty;

    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        // Cache the SerializedObject and enum property
        if (_serializedObject == null || _serializedObject.targetObject != _property.serializedObject.targetObject)
        {
            _serializedObject = _property.serializedObject;
            _enumProperty = _serializedObject.FindProperty(((ConditionalEnumAttribute)attribute)._enumField);
        }

        // Validate if the enum property exists and is valid
        if (_enumProperty == null)
        {
            Debug.LogWarning($"ConditionalEnum: Could not find enum field '{((ConditionalEnumAttribute)attribute)._enumField}'");
            return;
        }

        if (_enumProperty.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning($"ConditionalEnum: Field '{((ConditionalEnumAttribute)attribute)._enumField}' is not an enum.");
            return;
        }

        // Check if the enum value matches the target
        if (((ConditionalEnumAttribute)attribute)._targetEnumValues.Contains(_enumProperty.enumValueIndex))
        {
            // Draw the property field
            EditorGUI.PropertyField(_position, _property, _label);
        }
    }

    public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
    {
        // Reuse the cached SerializedObject and enum property
        if (_enumProperty == null || !((ConditionalEnumAttribute)attribute)._targetEnumValues.Contains(_enumProperty.enumValueIndex))
        {
            // Return zero height to avoid unnecessary space
            return -EditorGUIUtility.standardVerticalSpacing;
        }

        // Return the height if the property is valid
        return EditorGUI.GetPropertyHeight(_property);
    }
}