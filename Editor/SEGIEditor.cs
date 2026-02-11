using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SEGIRenderer))]
public class SEGIRendererEditor : Editor
{
    SEGIRenderer instance;

    string PresetSavePath
    {
        get
        {
            MonoScript script = MonoScript.FromMonoBehaviour(instance);
            string scriptPath = AssetDatabase.GetAssetPath(script);
            string scriptDirectory = System.IO.Path.GetDirectoryName(scriptPath);

            scriptDirectory = scriptDirectory.Replace("\\", "/");

            return scriptDirectory + "/Resources/Presets";
        }
    }

    GUIStyle vramLabelStyle
    {
        get
        {
            GUIStyle s = new GUIStyle(EditorStyles.boldLabel);
            s.fontStyle = FontStyle.Italic;
            return s;
        }
    }

    bool showPresets = true;

    string presetToSaveName;

    int presetPopupIndex;

    void OnEnable()
    {
        instance = target as SEGIRenderer;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        showPresets = EditorGUILayout.Foldout(showPresets, new GUIContent("Presets"));
        if (showPresets)
        {
            EditorGUI.indentLevel++;
            string[] presetGUIDs = AssetDatabase.FindAssets("t:SEGIPreset");
            string[] presetNames = new string[presetGUIDs.Length];
            string[] presetPaths = new string[presetGUIDs.Length];

            for (int i = 0; i < presetGUIDs.Length; i++)
            {
                presetPaths[i] = AssetDatabase.GUIDToAssetPath(presetGUIDs[i]);
                presetNames[i] = System.IO.Path.GetFileNameWithoutExtension(presetPaths[i]);
            }

            EditorGUILayout.BeginHorizontal();
            presetPopupIndex = EditorGUILayout.Popup("", presetPopupIndex, presetNames);

            if (GUILayout.Button("Load"))
            {
                if (presetPaths.Length > 0)
                {
                    SEGIPreset preset = AssetDatabase.LoadAssetAtPath<SEGIPreset>(presetPaths[presetPopupIndex]);
                    Undo.RecordObject(target, "Loaded SEGI Preset");
                    instance.ApplyPreset(preset);
                    EditorUtility.SetDirty(target);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            presetToSaveName = EditorGUILayout.TextField(presetToSaveName);

            if (GUILayout.Button("Save"))
            {
                SavePreset(presetToSaveName);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("VRAM Usage: " + instance.vramUsage.ToString("F2") + " MB", vramLabelStyle);

        serializedObject.ApplyModifiedProperties();
    }

    void SavePreset(string name)
    {
        if (name == "")
        {
            Debug.LogWarning("SEGI: Type in a name for the preset to be saved!");
            return;
        }

        //SEGIPreset preset = new SEGIPreset();
        SEGIPreset preset = ScriptableObject.CreateInstance<SEGIPreset>();

        preset.voxelResolution = instance.voxelResolution;
        preset.voxelAA = instance.voxelAA;
        preset.innerOcclusionLayers = instance.innerOcclusionLayers;
        preset.infiniteBounces = instance.infiniteBounces;

        preset.temporalBlendWeight = instance.temporalBlendWeight;
        preset.useBilateralFiltering = instance.useBilateralFiltering;
        preset.halfResolution = instance.halfResolution;
        preset.stochasticSampling = instance.stochasticSampling;
        preset.doReflections = instance.doReflections;

        preset.cones = instance.cones;
        preset.coneTraceSteps = instance.coneTraceSteps;
        preset.coneLength = instance.coneLength;
        preset.coneWidth = instance.coneWidth;
        preset.coneTraceBias = instance.coneTraceBias;
        preset.occlusionStrength = instance.occlusionStrength;
        preset.nearOcclusionStrength = instance.nearOcclusionStrength;
        preset.occlusionPower = instance.occlusionPower;
        preset.nearLightGain = instance.nearLightGain;
        preset.giGain = instance.giGain;
        preset.secondaryBounceGain = instance.secondaryBounceGain;

        preset.reflectionSteps = instance.reflectionSteps;
        preset.reflectionOcclusionPower = instance.reflectionOcclusionPower;
        preset.skyReflectionIntensity = instance.skyReflectionIntensity;
        preset.gaussianMipFilter = instance.gaussianMipFilter;

        preset.farOcclusionStrength = instance.farOcclusionStrength;
        preset.farthestOcclusionStrength = instance.farthestOcclusionStrength;
        preset.secondaryCones = instance.secondaryCones;
        preset.secondaryOcclusionStrength = instance.secondaryOcclusionStrength;

        string folderPath = PresetSavePath;

        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh();
        }

        string fullPath = folderPath + "/" + name + ".asset";

        fullPath = AssetDatabase.GenerateUniqueAssetPath(fullPath);

        AssetDatabase.CreateAsset(preset, fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void LoadPreset()
    {

    }
}
