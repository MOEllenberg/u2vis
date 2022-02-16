using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using u2vis;
using UnityEngine;
using static GeneralVisulizationWrapper;
using static u2visGeneralController;

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

    AxisInformationStruct[] _axisInformationStructs = new AxisInformationStruct[2];
    AxisInformationStruct[] _axisInformationStructs3D = new AxisInformationStruct[3];
    public void Start()
    {
        
        _axisInformationStructs[0] = new AxisInformationStruct(true, true, axisPrefab, 4, 1, LabelOrientation.Diagonal, 1);
        _axisInformationStructs[1] = new AxisInformationStruct(false, true, axisPrefab, 10, 1, LabelOrientation.Diagonal, 1);

        _axisInformationStructs3D[0] = new AxisInformationStruct(true, true, axisPrefab, 4, 1, LabelOrientation.Diagonal, 1);
        _axisInformationStructs3D[1] = new AxisInformationStruct(false, true, axisPrefab, 10, 1, LabelOrientation.Diagonal, 1);
        _axisInformationStructs3D[2] = new AxisInformationStruct(false, true, axisPrefab, 4, 1, LabelOrientation.Diagonal, 1);
    }

    [ContextMenu("Test generics")]
    public void TestGenerics()
    {
        Start();
        BarChart2DWrapper wrapper = u2visGeneralController.Instance.CreateVis<BarChart2DWrapper>(dataProvider, new int[] { 0, 1 }, parentToBe, "Testing Generics");
        wrapper.SetBarChart2DValues(0.9f, u2visGeneralController.Instance.Default2DBarChartMesh);
        wrapper.Initilize(
            size: new Vector3(0.9f, 0.9f, 0.9f),
            selectedMinItem: 0,
            selectedMaxItem: 10,
            axisInformation: _axisInformationStructs,
            style: style
            );

    }

    [ContextMenu("Test!")]
    public void Test()
    {
        Start();

        int[] multidim = { 0, 1, 2, 3 };
        GeneralVisulizationWrapper finalWrapper;
        switch (visToTest)
        {
            case u2visGeneralController.VisType.BarChart2D:
                BarChart2DWrapper wrapper = u2visGeneralController.Instance.CreateVis<BarChart2DWrapper>(dataProvider, new int[] { 0, 1 }, parentToBe, "Testing Generics");
                wrapper.SetBarChart2DValues(0.9f, u2visGeneralController.Instance.Default2DBarChartMesh);
                wrapper.Initilize(
                    size: new Vector3(0.9f, 0.9f, 0.9f),
                    selectedMinItem: 0,
                    selectedMaxItem: 10,
                    axisInformation: _axisInformationStructs,
                    style: style
                    );
                finalWrapper = wrapper;
                break;
            case u2visGeneralController.VisType.BarChart3D:
                BarChart3DWrapper wrapperBC3D = u2visGeneralController.Instance.CreateVis<BarChart3DWrapper>(dataProvider, new int[] { 0, 1, 2 }, parentToBe, "Testing Generics");
                wrapperBC3D.SetBarChart3DValues(new Vector2(0.9f,0.9f), u2visGeneralController.Instance.Default3DBarChartMesh);
                //TODO: size does not scale axes. Is a u2vis issue, not a wrapper issue. Has to be fixed some day in the future. Maybe use size for scale? Ask Marc
                wrapperBC3D.Initilize(
                    size: new Vector3(0.9f, 0.9f, 0.9f),
                    selectedMinItem: 0,
                    selectedMaxItem: 10,
                    axisInformation: _axisInformationStructs3D,
                    indicesOfMultiDimensionDataDimensions: multidim,
                    style: style
                    );
                finalWrapper = wrapperBC3D;
                break;
            case u2visGeneralController.VisType.HeightMap:
                HeightMapWrapper  wrapperHM = u2visGeneralController.Instance.CreateVis<HeightMapWrapper>(dataProvider, new int[] { 0, 1, 2 }, parentToBe, "Testing");
                wrapperHM.Initilize(
                    size: new Vector3(0.9f, 0.9f, 0.9f),
                    selectedMinItem: 0,
                    selectedMaxItem: 10,
                    axisInformation: _axisInformationStructs3D,
                    indicesOfMultiDimensionDataDimensions: multidim,
                    style: categoricalStyle
                    );
                finalWrapper = wrapperHM;
                break;
            case u2visGeneralController.VisType.LineChart2D:
                LineChart2DWrapper wrapperLC2D = u2visGeneralController.Instance.CreateVis<LineChart2DWrapper>(dataProvider, new int[] { 0, 1 }, parentToBe, "Testing");
                wrapperLC2D.Initilize(
                    size: new Vector3(0.9f, 0.9f, 0.9f),
                    selectedMinItem: 0,
                    selectedMaxItem: 10,
                    axisInformation: _axisInformationStructs3D,
                    indicesOfMultiDimensionDataDimensions: multidim,
                    style: categoricalStyle
                    );
                finalWrapper = wrapperLC2D;
                break;
            case u2visGeneralController.VisType.LineChart3D:
                LineChart3DWrapper wrapperLC3D = u2visGeneralController.Instance.CreateVis<LineChart3DWrapper>(dataProvider, new int[] { 0, 1, 2 }, parentToBe, "Testing");
                wrapperLC3D.Initilize(
                     size: new Vector3(0.9f, 0.9f, 0.9f),
                     selectedMinItem: 0,
                     selectedMaxItem: 10,
                     axisInformation: _axisInformationStructs3D,
                     indicesOfMultiDimensionDataDimensions: multidim,
                     style: categoricalStyle
                     );
                finalWrapper = wrapperLC3D;
                break;
            case u2visGeneralController.VisType.ParallelCoordinates:
                AxisInformationStruct[] axisInformationStructPC = new AxisInformationStruct[4];
                axisInformationStructPC[0] = new AxisInformationStruct(true, true, axisPrefab, 4, 1, LabelOrientation.Diagonal, 1);
                axisInformationStructPC[1] = new AxisInformationStruct(false, true, axisPrefab, 10, 1, LabelOrientation.Diagonal, 1);
                axisInformationStructPC[2] = new AxisInformationStruct(false, true, axisPrefab, 10, 1, LabelOrientation.Diagonal, 1);
                ParallelCoordinatesWrapper wrapperPC = u2visGeneralController.Instance.CreateVis<ParallelCoordinatesWrapper>(dataProvider, new int[] { 0, 1, 2 }, parentToBe, "Testing");
                wrapperPC.Initilize(
                    size: new Vector3(2, 1, 1), 
                    selectedMinItem: 0, 
                    selectedMaxItem: 10, 
                    axisInformation: axisInformationStructPC,
                    style: categoricalStyle
                    );
                finalWrapper = wrapperPC;
                break;
            case u2visGeneralController.VisType.PieChart2D:
                PieChart2DWrapper wrapperPC2D = u2visGeneralController.Instance.CreateVis<PieChart2DWrapper>(dataProvider, new int[] { 0, 1, 2 }, parentToBe, "Testing");
                wrapperPC2D.Initilize(
                    size: new Vector3(0.9f, 0.9f, 0.9f),
                    selectedMinItem: 0,
                    selectedMaxItem: 10,
                    axisInformation: _axisInformationStructs3D,
                    indicesOfMultiDimensionDataDimensions: multidim,
                    style: categoricalStyle
                    );
                finalWrapper = wrapperPC2D;
                break;
            case u2visGeneralController.VisType.PieChart3D:
                PieChart3DWrapper wrapperPC3D = u2visGeneralController.Instance.CreateVis<PieChart3DWrapper>(dataProvider, new int[] { 0, 1, 2 }, parentToBe, "Testing");
                wrapperPC3D.Initilize(
                    size: new Vector3(0.9f, 0.9f, 0.9f),
                    selectedMinItem: 0, 
                    selectedMaxItem: 10, 
                    axisInformation: _axisInformationStructs3D,
                    indicesOfMultiDimensionDataDimensions: multidim,
                    style: categoricalStyle
                    );
                finalWrapper = wrapperPC3D;
                break;
            case u2visGeneralController.VisType.Scatterplot:
                ScatterplotWrapper wrapperSC = u2visGeneralController.Instance.CreateVis<ScatterplotWrapper>(dataProvider, new int[] { 1, 2, 3 }, parentToBe, "Testing");
                wrapperSC.SetScatterplotValues(Vector3.zero, Vector3.one, true);
                AxisInformationStruct[] axisInformationStructSP = new AxisInformationStruct[3];
                axisInformationStructSP[0] = new AxisInformationStruct(false, true, axisPrefab, 10, 1, LabelOrientation.Diagonal, 1);
                axisInformationStructSP[1] = new AxisInformationStruct(false, true, axisPrefab, 10, 1, LabelOrientation.Diagonal, 1);
                axisInformationStructSP[2] = new AxisInformationStruct(false, true, axisPrefab, 10, 1, LabelOrientation.Diagonal, 1);
                wrapperSC.Initilize(
                    size: new Vector3(0.9f, 0.9f, 0.9f),
                    selectedMinItem: 0, 
                    selectedMaxItem: 10, 
                    axisInformation: axisInformationStructSP, 
                    style: style
                    );
                finalWrapper = wrapperSC;
                break;
            default:
                finalWrapper = null;
                break;
        }
        currentWrapper = finalWrapper;
    }

}
