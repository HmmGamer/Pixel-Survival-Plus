using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
/// <summary>
/// this class makes an enum for you based on an string array
/// it can also be called in the editor as well with the [CreateButton] attribute
/// 
/// read comments for more info
/// </summary>
public static class EnumGenerator
{
    private const string GENERATION_PATH = "Assets/Others/GeneratedEnums";

    public static void GenerateEnums<T>(string enumName, T[] dataArray, string fieldName) where T : class
    {
#if UNITY_EDITOR

        if (Application.isPlaying)
        {
            Debug.LogError("Enum Generator Only Works in Editor");
            return;
        }

        if (dataArray == null || dataArray.Length == 0)
        {
            Debug.LogWarning("Data array is empty or null.");
            return;
        }

        Directory.CreateDirectory(GENERATION_PATH);

        string filePath = Path.Combine(GENERATION_PATH, enumName + ".cs");
        string enumCode = "using System;\n\n[Serializable]\npublic enum " + enumName + "\n{\n";

        for (int i = 0; i < dataArray.Length; i++)
        {
            var field = dataArray[i].GetType().GetField(fieldName);
            if (field == null)
            {
                Debug.LogError($"Class does not contain a field named '{fieldName}'.");
                return;
            }

            string enumValue = field.GetValue(dataArray[i]) as string;
            if (string.IsNullOrEmpty(enumValue))
            {
                Debug.LogWarning($"{fieldName} value is null or empty at index {i}.");
                continue;
            }

            enumValue = enumValue.Replace(" ", "_").Replace("-", "_"); // Remove spaces and invalid characters
            enumCode += "    " + enumValue + (i < dataArray.Length - 1 ? "," : "") + "\n";
        }

        enumCode += "}";

        File.WriteAllText(filePath, enumCode);
        AssetDatabase.Refresh();
#endif
    }
    private class _Comments
    {
        //[SerializeField] _Test[] tests;

        //[CreateButton("Make Enum")]
        //private void _MakeEnum()
        //{
        //    EnumGenerator.GenerateEnums("testEnum", tests, nameof(_Test._name));
        //}
        //[Serializable]
        //public class _Test
        //{
        //    public string _name;
        //    public int _value;
        //}
    }
}