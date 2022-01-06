using System.Collections;
using System.Collections.Generic;
using u2vis;
using UnityEngine;
using static GeneralVisulizationWrapper;

public class WrapperTester : MonoBehaviour
{
    [SerializeField]
    private u2visGeneralController.VisType visToTest;
    [SerializeField]
    private Transform parentToBe;
    [SerializeField]
    private AbstractDataProvider dataProvider;
    [SerializeField]
    private GenericVisualizationStyle style;
    [SerializeField]
    private GenericVisualizationStyle categoricalStyle;
    [SerializeField]
    private GenericAxisView axisPrefab;

    private GeneralVisulizationWrapper currentWrapper;

    [ContextMenu("Test!")]
    public void Test()
    {
        GeneralVisulizationWrapper wrapper = u2visGeneralController.Instance.CreateVisByType(visToTest,parentToBe,"Testing");

        AxisInformationStruct[] axisInformationStructs = new AxisInformationStruct[2];
        axisInformationStructs[0] = new AxisInformationStruct(0, true, true, axisPrefab, 4, 1, LabelOrientation.Diagonal, 1);
        axisInformationStructs[1] = new AxisInformationStruct(1, false, true, axisPrefab, 10, 1, LabelOrientation.Diagonal, 1);

        AxisInformationStruct[] axisInformationStructs3D = new AxisInformationStruct[3];
        axisInformationStructs3D[0] = new AxisInformationStruct(0, true, true, axisPrefab, 4, 1, LabelOrientation.Diagonal, 1);
        axisInformationStructs3D[1] = new AxisInformationStruct(1, false, true, axisPrefab, 10, 1, LabelOrientation.Diagonal, 1);
        axisInformationStructs3D[2] = new AxisInformationStruct(1, false, true, axisPrefab, 4, 1, LabelOrientation.Diagonal, 1);

        int[] multidim = { 0, 1, 2, 3 };

        switch (visToTest)
        {
            case u2visGeneralController.VisType.BarChart2D:

                wrapper.SetBarChart2DValues(0.9f, u2visGeneralController.Instance.Default2DBarChartMesh);
                wrapper.Initilize(new Vector3(0.9f,0.9f,0.9f), dataProvider, 0, 10, axisInformationStructs, null, null, null, style);
                break;
            case u2visGeneralController.VisType.BarChart3D:
                
                wrapper.SetBarChart3DValues(new Vector2(0.9f,0.9f), u2visGeneralController.Instance.Default3DBarChartMesh);
                //TODO: size does not scale axes. Is a u2vis issue, not a wrapper issue. Has to be fixed some day in the future. Maybe use size for scale? Ask Marc
                wrapper.Initilize(new Vector3(0.9f, 0.9f, 0.9f), dataProvider, 0, 10, axisInformationStructs3D, multidim, null, null, style);
                break;
            case u2visGeneralController.VisType.HeightMap:
                wrapper.Initilize(new Vector3(0.9f, 0.9f, 0.9f), dataProvider, 0, 10, axisInformationStructs3D, multidim, null, null, categoricalStyle);
                break;
            case u2visGeneralController.VisType.LineChart2D:
                wrapper.Initilize(new Vector3(0.9f, 0.9f, 0.9f), dataProvider, 0, 10, axisInformationStructs3D, multidim, null, null, categoricalStyle);
                break;
            case u2visGeneralController.VisType.LineChart3D:
                wrapper.Initilize(new Vector3(0.9f, 0.9f, 0.9f), dataProvider, 0, 10, axisInformationStructs3D, multidim, null, null, categoricalStyle);
                break;
            case u2visGeneralController.VisType.ParallelCoordinates:
                AxisInformationStruct[] axisInformationStructPC = new AxisInformationStruct[4];
                axisInformationStructPC[0] = new AxisInformationStruct(0, true, true, axisPrefab, 4, 1, LabelOrientation.Diagonal, 1);
                axisInformationStructPC[1] = new AxisInformationStruct(1, false, true, axisPrefab, 10, 1, LabelOrientation.Diagonal, 1);
                axisInformationStructPC[2] = new AxisInformationStruct(2, false, true, axisPrefab, 10, 1, LabelOrientation.Diagonal, 1);
                axisInformationStructPC[3] = new AxisInformationStruct(3, false, true, axisPrefab, 10, 1, LabelOrientation.Diagonal, 1);

                wrapper.Initilize(new Vector3(2, 1, 1), dataProvider, 0, 10, axisInformationStructPC, null, null, null, categoricalStyle);
                break;
            case u2visGeneralController.VisType.PieChart2D:
                wrapper.Initilize(new Vector3(0.9f, 0.9f, 0.9f), dataProvider, 0, 10, axisInformationStructs3D, multidim, null, null, categoricalStyle);
                break;
            case u2visGeneralController.VisType.PieChart3D:
                wrapper.Initilize(new Vector3(0.9f, 0.9f, 0.9f), dataProvider, 0, 10, axisInformationStructs3D, multidim, null, null, categoricalStyle);
                break;
            case u2visGeneralController.VisType.Scatterplot:
                wrapper.SetScatterplotValues(Vector3.zero, Vector3.one, true);
                AxisInformationStruct[] axisInformationStructSP = new AxisInformationStruct[3];
                axisInformationStructSP[0] = new AxisInformationStruct(1, false, true, axisPrefab, 10, 1, LabelOrientation.Diagonal, 1);
                axisInformationStructSP[1] = new AxisInformationStruct(2, false, true, axisPrefab, 10, 1, LabelOrientation.Diagonal, 1);
                axisInformationStructSP[2] = new AxisInformationStruct(3, false, true, axisPrefab, 10, 1, LabelOrientation.Diagonal, 1);
                wrapper.Initilize(new Vector3(0.9f, 0.9f, 0.9f), dataProvider, 0, 10, axisInformationStructSP, null, null, null, style);
                break;
        }
        currentWrapper = wrapper;
    }

    [ContextMenu("Test with defaults!")]
    public void TestWithDefaults()
    {
        GeneralVisulizationWrapper wrapper = u2visGeneralController.Instance.CreateVisByType(visToTest, parentToBe, "TestingWithDefaults");
        wrapper.InitilizeWithDefaults();
        currentWrapper = wrapper;
    }

}
