using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;

namespace WaterfallShader
{
    [CustomEditor(typeof(WaterfallShader))]
    public class WaterfallShaderEditor : Editor
    {
        private WaterfallShader waterfallShader;
        private bool isReady = false;
        private GameObject selected;

        private WaterfallShaderEditor waterfallShaderEditor;
        private WaterfallPresetReceiver waterfallPresetReceiver;
        private string presetName;

        private new SerializedObject serializedObject;

        private SerializedProperty flowSpeed;
        private SerializedProperty offset;
        private SerializedProperty tiling;
        private SerializedProperty whiteColor;
        private SerializedProperty darkenedColor;
        private SerializedProperty blackColor;
        private SerializedProperty waterfallMasks;
        

        private void OnEnable()
        {
            isReady = false;

            selected = Selection.activeGameObject;
            
            if (!selected) return;

            if (!waterfallShader)
            {
                waterfallShader = selected.GetComponent<WaterfallShader>();
            }
            
            serializedObject = new SerializedObject(waterfallShader);

            GetProperties();
            
            EditorUtility.SetDirty(target);

            isReady = true;
        }

        public override void OnInspectorGUI()
        {
            if (!isReady) return;
            
            serializedObject.Update();
            
            ShowWaterfallUI();
            
            Undo.RecordObject(target, "Component");

            serializedObject.ApplyModifiedProperties();
            waterfallShader.SetProperties();

        }

        private void GetProperties()
        {
            if (!selected) return;
            
            waterfallShader.GetProperties();

            flowSpeed = serializedObject.FindProperty("flowSpeed");
            tiling = serializedObject.FindProperty("tiling");
            offset = serializedObject.FindProperty("offset");
            whiteColor = serializedObject.FindProperty("whiteColor");
            darkenedColor = serializedObject.FindProperty("darkenedColor");
            blackColor = serializedObject.FindProperty("blackColor");
            waterfallMasks = serializedObject.FindProperty("waterfallMasks");

        }

        public void ShowWaterfallUI()
        {
            TopUI();
            SettignsUI();
        }


        void TopUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("My first waterfall shader UI", new GUIStyle(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.MiddleLeft,
                    wordWrap = true,
                    fontSize = 11,
                });
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
            
            FontStyle originalFontStyle = EditorStyles.label.fontStyle;
            EditorStyles.label.fontStyle = FontStyle.Bold;
            
            DetailedExplanationBox("R channel: Main color Mask\n" +
                                             "G channel: Highlight pattern \n" + 
                                             "B channel: Variation map");
            EditorGUILayout.ObjectField(waterfallMasks, typeof(Texture2D));
            
            ExplanationBox("Variables");
            
            // Different ways of doing it
            offset.floatValue = EditorGUILayout.Slider("Offset:",offset.floatValue, 0.1f, 1); // Probably more expensive
            EditorGUILayout.Slider(tiling, 1f, 10f, "Tiling");
            EditorGUILayout.Slider(flowSpeed, 0, 1, "Flow Speed");
            
            ExplanationBox("Colors");
            
            EditorGUILayout.PropertyField(whiteColor, new GUIContent("White color:"));
            darkenedColor.colorValue = EditorGUILayout.ColorField("Darkened color:", darkenedColor.colorValue);
            blackColor.colorValue = EditorGUILayout.ColorField("Black color:", blackColor.colorValue);



        }
        private void SettignsUI()
        {
            
        }

        public void OnSavePresetClicked()
        {
            presetName = presetName.Trim();

            if (string.IsNullOrEmpty(presetName))
            {
                EditorUtility.DisplayDialog("Unable to save preset", "Please select and valid name preset", "Close");
                return;
            }
            
            CreatePresetAsset(waterfallShader, presetName);
            EditorUtility.DisplayDialog("Preset Saved", "The preset was saved", "Close");
                
            
        }

        private void CreatePresetAsset(WaterfallShader source, string presetName)
        {
            Preset preset = new Preset(source);
            AssetDatabase.CreateAsset(preset, "Assets/Scripts/" + "WS_" + name + ".preset");
        }


        void DetailedExplanationBox(string inputText)
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label(inputText, new GUIStyle(EditorStyles.helpBox)
                {
                        alignment = TextAnchor.MiddleLeft,
                        wordWrap = true,
                        fontSize = 10
                });
            }
            EditorGUILayout.EndHorizontal();
        }
        
        void ExplanationBox(string inputText)
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label(inputText, new GUIStyle(EditorStyles.helpBox)
                {
                    alignment = TextAnchor.MiddleLeft,
                    wordWrap = true,
                    fontSize = 12
                });
            }
            EditorGUILayout.EndHorizontal();
        }
        
    }

}