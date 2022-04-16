using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CreateVisualizationFromEditor))]
public class CreateVisualizationFromEditorEditor : Editor
{

    protected SerializedProperty
        visualizationType_prop,
        parentToBe_prop,
        dataProvider_prop,
        dimensionIndexes_prop,
        multiDimensionalIndexes_prop,
        creatorName_prop,
        createWithDefaults_prop,
        categoricalFlag_prop,
        labelOrientation_prop,
        numberOfTicks_prop,
        labelInterval_prop,
        labelDecimalPlaces_prop,
        axisPrefab_prop,
        showAxisFlag_prop,
        size_prop,
        minItem_prop,
        maxItem_prop,
        style_prop,
        _2DBarChartMesh_prop,
        _3DBarChartMesh_prop,
        _2DBarThickness_prop,
        _3DBarThickness_prop,
        areaMaterial_prop,
        lineMaterial_prop,
        scatterplotMaterial_prop,
        minZoomLevel_prop,
        maxZoomLevel_prop,
        displayRelativeValues_prop;

    protected CreateVisualizationFromEditor _creatorScript = null;

    private void OnEnable()
    {
        visualizationType_prop = serializedObject.FindProperty("_visualizationType");
        parentToBe_prop = serializedObject.FindProperty("_parentToBe");
        dataProvider_prop = serializedObject.FindProperty("_dataProvider");
        dimensionIndexes_prop = serializedObject.FindProperty("_dimensionIndexes");
        multiDimensionalIndexes_prop = serializedObject.FindProperty("_multiDimensionalIndexes");
        creatorName_prop = serializedObject.FindProperty("_creatorName");
        createWithDefaults_prop = serializedObject.FindProperty("_createWithDefaults");
        categoricalFlag_prop = serializedObject.FindProperty("_categoricalFlag");
        labelOrientation_prop = serializedObject.FindProperty("_labelOrientation");
        numberOfTicks_prop = serializedObject.FindProperty("_numberOfTicks");
        labelInterval_prop = serializedObject.FindProperty("_labelInterval");
        labelDecimalPlaces_prop = serializedObject.FindProperty("_labelDecimalPlaces");
        axisPrefab_prop = serializedObject.FindProperty("_axisPrefab");
        showAxisFlag_prop = serializedObject.FindProperty("_showAxisFlag");
        size_prop = serializedObject.FindProperty("_size");
        minItem_prop = serializedObject.FindProperty("_minItem");
        maxItem_prop = serializedObject.FindProperty("_maxItem");
        style_prop = serializedObject.FindProperty("_style");
        _2DBarChartMesh_prop = serializedObject.FindProperty("_2DBarChartMesh");
        _3DBarChartMesh_prop = serializedObject.FindProperty("_3DBarChartMesh");
        _2DBarThickness_prop = serializedObject.FindProperty("_2DBarThickness");
        _3DBarThickness_prop = serializedObject.FindProperty("_3DBarThickness");
        areaMaterial_prop = serializedObject.FindProperty("_areaMaterial");
        lineMaterial_prop = serializedObject.FindProperty("_lineMaterial");
        scatterplotMaterial_prop = serializedObject.FindProperty("_scatterplotMaterial");
        minZoomLevel_prop = serializedObject.FindProperty("_minZoomLevel");
        maxZoomLevel_prop = serializedObject.FindProperty("_maxZoomLevel");
        displayRelativeValues_prop = serializedObject.FindProperty("_displayRelativeValues");
        _creatorScript = (CreateVisualizationFromEditor)serializedObject.targetObject;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawGUIItems();
        serializedObject.ApplyModifiedProperties();
        if (GUILayout.Button("Build Visualization"))
            _creatorScript.CreateVisualization();
    }

    private void DrawGUIItems()
    {
        EditorGUILayout.PropertyField(visualizationType_prop);
        EditorGUILayout.PropertyField(parentToBe_prop);
        EditorGUILayout.PropertyField(dataProvider_prop);
        EditorGUILayout.PropertyField(dimensionIndexes_prop);
        EditorGUILayout.PropertyField(multiDimensionalIndexes_prop);
        EditorGUILayout.PropertyField(creatorName_prop);
        EditorGUILayout.PropertyField(createWithDefaults_prop);
        if (!createWithDefaults_prop.boolValue)
        {
            //for (int i = 0; i < dimensionIndexes_prop.arraySize; i++) Create loop here to create axis information structs. not propertyfields but EnumPopUp etc
            //{
                EditorGUILayout.PropertyField(categoricalFlag_prop);
                EditorGUILayout.PropertyField(labelOrientation_prop);
                EditorGUILayout.PropertyField(numberOfTicks_prop);
                EditorGUILayout.PropertyField(labelInterval_prop);
                EditorGUILayout.PropertyField(labelDecimalPlaces_prop);
                EditorGUILayout.PropertyField(axisPrefab_prop);
                EditorGUILayout.PropertyField(showAxisFlag_prop);
            //}
            EditorGUILayout.PropertyField(size_prop);
            EditorGUILayout.PropertyField(minItem_prop);
            EditorGUILayout.PropertyField(maxItem_prop);
            EditorGUILayout.PropertyField(style_prop);
            EditorGUILayout.PropertyField(_2DBarChartMesh_prop);
            EditorGUILayout.PropertyField(_3DBarChartMesh_prop);
            EditorGUILayout.PropertyField(_2DBarThickness_prop);
            EditorGUILayout.PropertyField(_3DBarThickness_prop);
            EditorGUILayout.PropertyField(areaMaterial_prop);
            EditorGUILayout.PropertyField(lineMaterial_prop);
            EditorGUILayout.PropertyField(scatterplotMaterial_prop);
            EditorGUILayout.PropertyField(minZoomLevel_prop);
            EditorGUILayout.PropertyField(maxZoomLevel_prop);
            EditorGUILayout.PropertyField(displayRelativeValues_prop);
        }
    }
}
