using UnityEngine;
using UnityEditor;

namespace WaterfallShader
{
    [CustomEditor(typeof(WaterfallShader))]
    public class WaterfallShaderEditor : Editor
    {
        private WaterfallShader waterfallShader;
        private bool isReady = false;
        private GameObject selected;

        private WaterfallShaderEditor waterfallShaderEditor;

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
            //flowSpeed.floatValue = EditorGUILayout.FloatField("Flow Speed", flowSpeed.floatValue);
            flowSpeed.floatValue = EditorGUILayout.Slider(flowSpeed.floatValue, 0, 1);



        }
        private void SettignsUI()
        {
            
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
        
    }

}