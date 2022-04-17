using System;
using System.Collections;
using System.Collections.Generic;
using u2vis;
using UnityEditor;
using UnityEngine;
using static GeneralVisulizationWrapper;
using static u2visGeneralController;

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
    public List<AxisInformationStruct> structs = new List<AxisInformationStruct>();
    public List<bool> togglesCategorical = new List<bool>();
    public bool testToggle;
    public List<LabelOrientation> labelOrientation = new List<LabelOrientation>();
    public List<int> numberOfTicksList = new List<int>();
    public List<int> labelIntervalList = new List<int>();
    public List<int> labelDecimalPlacesInt = new List<int>();
    public List<GenericAxisView> axisPrefabList = new List<GenericAxisView>();
    public List<bool> showAxisFlagList = new List<bool>();



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
        {
            structs.Clear();
            for(int i=0;i < dimensionIndexes_prop.arraySize; i++)
            {
                    structs.Add(new AxisInformationStruct(togglesCategorical[i], showAxisFlagList[i], axisPrefabList[i], numberOfTicksList[i], labelIntervalList[i], labelOrientation[i], labelDecimalPlacesInt[i]));
            }
            _creatorScript.SetInformationStructList(structs);
            _creatorScript.CreateVisualization();
        }
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
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (!createWithDefaults_prop.boolValue)
        {
            for (int i = 0; i < dimensionIndexes_prop.arraySize; i++) //Create loop here to create axis information structs. not propertyfields but EnumPopUp etc
            {
                switch (i)
                {
                    case 0: 
                        EditorGUILayout.LabelField("X-Axis/Axis 1", EditorStyles.boldLabel);
                        break;
                    case 1:
                        EditorGUILayout.LabelField("Y-Axis/Axis 2", EditorStyles.boldLabel);
                        break;
                    case 2:
                        EditorGUILayout.LabelField("Z-Axis/Axis 3", EditorStyles.boldLabel);
                        break;
                    default:
                        EditorGUILayout.LabelField($"Additional Axis {i-2}/Axis {i+1}", EditorStyles.boldLabel);
                        break;
                }
                EditorGUILayout.Space();
                togglesCategorical.Add(false);
                labelOrientation.Add(LabelOrientation.Diagonal);
                numberOfTicksList.Add(4);
                labelIntervalList.Add(1);
                labelDecimalPlacesInt.Add(1);
                axisPrefabList.Add(u2visGeneralController.Instance.DefaultAxisPrefab);
                showAxisFlagList.Add(true);
                //testToggle = EditorGUILayout.Toggle("is categorical: ", testToggle);
                togglesCategorical[i] = EditorGUILayout.Toggle("is categorical: ", togglesCategorical[i]);
                labelOrientation[i] = (LabelOrientation)EditorGUILayout.EnumPopup("Label Orientation: ", labelOrientation[i]);
                numberOfTicksList[i] = EditorGUILayout.IntField("Number of Ticks: ", numberOfTicksList[i]);
                labelIntervalList[i] = EditorGUILayout.IntField("Label interval: ", labelIntervalList[i]);
                labelDecimalPlacesInt[i] = EditorGUILayout.IntField("Label decimal places: ", labelDecimalPlacesInt[i]);
                axisPrefabList[i] = (GenericAxisView)EditorGUILayout.ObjectField("Axis Prefab: ", axisPrefabList[i], typeof(GenericAxisView), true);
                showAxisFlagList[i] = EditorGUILayout.Toggle("show axis: ", showAxisFlagList[i]);
                EditorGUILayout.Space();
                EditorGUILayout.Space();

            }
            EditorGUILayout.PropertyField(size_prop);
            EditorGUILayout.PropertyField(minItem_prop);
            EditorGUILayout.PropertyField(maxItem_prop);
            EditorGUILayout.PropertyField(style_prop);
            string enumName = visualizationType_prop.enumNames[visualizationType_prop.enumValueIndex];
            if (enumName == VisType.BarChart2D.ToString()) 
            {
                EditorGUILayout.PropertyField(_2DBarChartMesh_prop);
                EditorGUILayout.PropertyField(_2DBarThickness_prop);
            }
            if (enumName == VisType.BarChart3D.ToString())
            {
                EditorGUILayout.PropertyField(_3DBarChartMesh_prop);
                EditorGUILayout.PropertyField(_3DBarThickness_prop);
            }
            if (enumName == VisType.BarChart2D.ToString()
                || enumName == VisType.BarChart3D.ToString()
                || enumName == VisType.HeightMap.ToString() 
                || enumName == VisType.PieChart2D.ToString() 
                || enumName == VisType.PieChart3D.ToString())
                EditorGUILayout.PropertyField(areaMaterial_prop);
            if (enumName == VisType.LineChart2D.ToString()
                || enumName == VisType.LineChart3D.ToString()
                || enumName == VisType.ParallelCoordinates.ToString())
                EditorGUILayout.PropertyField(lineMaterial_prop);
            if (enumName == VisType.Scatterplot.ToString())
            {
                EditorGUILayout.PropertyField(scatterplotMaterial_prop);
                EditorGUILayout.PropertyField(minZoomLevel_prop);
                EditorGUILayout.PropertyField(maxZoomLevel_prop);
                EditorGUILayout.PropertyField(displayRelativeValues_prop);
            }
        }
    }
}
