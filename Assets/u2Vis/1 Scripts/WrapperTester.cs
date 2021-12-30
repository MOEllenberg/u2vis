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
    private GenericAxisView axisPrefab;

    [ContextMenu("Test!")]
    public void Test()
    {
        GeneralVisulizationWrapper wrapper = u2visGeneralController.Instance.CreateVisByType(visToTest,parentToBe,"Testing");
        switch (visToTest)
        {
            case u2visGeneralController.VisType.BarChart2D:
                AxisInformationStruct[] axisInformationStructs = new AxisInformationStruct[2];
                axisInformationStructs[0] = new AxisInformationStruct(0, true, true, axisPrefab, 4, 1, LabelOrientation.Diagonal, 1);
                axisInformationStructs[1] = new AxisInformationStruct(1, false, true, axisPrefab, 4, 1, LabelOrientation.Diagonal, 1);
                wrapper.SetBarChart2DValues(0.9f, u2visGeneralController.Instance.Default2DBarChartMesh);
                wrapper.Initilize(Vector3.one, dataProvider, 0, 10, axisInformationStructs, null, null, null, style);
                break;
            case u2visGeneralController.VisType.BarChart3D:
                break;
            case u2visGeneralController.VisType.HeightMap:
                break;
            case u2visGeneralController.VisType.LineChart2D:
                break;
            case u2visGeneralController.VisType.LineChart3D:
                break;
            case u2visGeneralController.VisType.ParallelCoordinates:
                break;
            case u2visGeneralController.VisType.PieChart2D:
                break;
            case u2visGeneralController.VisType.PieChart3D:
                break;
            case u2visGeneralController.VisType.RevolvedCharts:
                break;
            case u2visGeneralController.VisType.Scatterplot:
                break;
            case u2visGeneralController.VisType.StackedBar:
                break;
        }

    }

    [ContextMenu("Test with defaults!")]
    public void TestWithDefaults()
    {
        GeneralVisulizationWrapper wrapper = u2visGeneralController.Instance.CreateVisByType(visToTest, parentToBe, "TestingWithDefaults");
        wrapper.InitilizeWithDefaults();
    }

}
