using UnityEditor.Presets;
using UnityEngine;

namespace WaterfallShader
{
    public class WaterfallPresetReceiver : PresetSelectorReceiver
    {
        private Preset initialValues;
        private WaterfallShader currentSettings;

        public void Init(WaterfallShader cur)
        {
            currentSettings = cur;
            initialValues = new Preset(cur); 
        }

        public override void OnSelectionChanged(Preset selection)
        {
            if (selection != null)
            {
                selection.ApplyTo(currentSettings);
            }
            else
            {
                initialValues.ApplyTo(currentSettings);
            }
            currentSettings.SetProperties();
        }

        public override void OnSelectionClosed(Preset selection)
        {
            OnSelectionChanged(selection);
            DestroyImmediate(this);
        }
    }
}
