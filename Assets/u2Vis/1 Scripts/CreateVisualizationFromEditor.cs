using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using u2vis;
using UnityEngine;
using static GeneralVisulizationWrapper;
using static u2visGeneralController;

public class CreateVisualizationFromEditor : MonoBehaviour
{
    [SerializeField]
    private u2visGeneralController.VisType _visualizationType;
    [SerializeField]
    private Transform _parentToBe;
    [SerializeField]
    private AbstractDataProvider _dataProvider;
    [SerializeField]
    private List<int> _dimensionIndexes;
    [SerializeField]
    private List<int> _multiDimensionalIndexes;
    [SerializeField]
    private string _creatorName;
    [SerializeField]
    private bool _createWithDefaults;

    /// <summary>
    /// The default is categorical flag.
    /// </summary>
    [SerializeField]
    private bool _categoricalFlag;
    /// <summary>
    /// The default Label Orientation.
    /// </summary>
    [SerializeField]
    private LabelOrientation _labelOrientation;
    /// <summary>
    /// The default Number of Ticks.
    /// </summary>
    [SerializeField]
    private int _numberOfTicks;
    /// <summary>
    /// The default Label Interval
    /// </summary>
    [SerializeField]
    private int _labelInterval;
    /// <summary>
    /// The default number of decimal places for the labels
    /// </summary>
    [SerializeField]
    private int _labelDecimalPlaces;
    /// <summary>
    /// The default Axis Prefab
    /// </summary>
    [SerializeField]
    private GenericAxisView _axisPrefab;
    /// <summary>
    /// The default Show Axis Flag
    /// </summary>
    [SerializeField]
    private bool _showAxisFlag;
    /// <summary>
    /// The default Size
    /// </summary>
    //TODO: Needs rework, because u2vis does not scale Axes with size, only the mesh
    [SerializeField]
    private Vector3 _size;

    /// <summary>
    /// The default min Item
    /// </summary>
    [SerializeField]
    private int _minItem;
    /// <summary>
    /// The default max Item
    /// </summary>
    [SerializeField]
    private int _maxItem;

    /// <summary>
    /// The default style
    /// </summary>
    [SerializeField]
    private GenericVisualizationStyle _style;
    /// <summary>
    /// The default 2D Bar Chart Mesh
    /// </summary>
    [SerializeField]
    private Mesh _2DBarChartMesh;
    /// <summary>
    /// The default 3d Bar Chart Mesh
    /// </summary>
    [SerializeField]
    private Mesh _3DBarChartMesh;
    /// <summary>
    /// The default 2D Bar Chart bar thickness
    /// </summary>
    [SerializeField]
    private float _2DBarThickness = 1f;
    /// <summary>
    /// The default 3D Bar Chart bar thickness
    /// </summary>
    [SerializeField]
    private Vector2 _3DBarThickness = Vector2.one;

    [SerializeField]
    private Material _areaMaterial;

    [SerializeField]
    private Material _lineMaterial;

    [SerializeField]
    private Material _scatterplotMaterial;

    [SerializeField]
    private Vector3 _minZoomLevel = Vector3.zero;

    [SerializeField]
    private Vector3 _maxZoomLevel = Vector3.one;

    [SerializeField]
    private bool _displayRelativeValues = true;


    private AxisInformationStruct[] _axisInformationStructs = new AxisInformationStruct[2];
    private AxisInformationStruct[] _axisInformationStructs3D = new AxisInformationStruct[3];
    private List<AxisInformationStruct> axisInformationStructsList = new List<AxisInformationStruct>();

    private u2visGeneralController _controller;
    public void Start()
    {
        _controller = u2visGeneralController.Instance;
    }

    public void SetInformationStructList(List<AxisInformationStruct> listContent)
    {
        axisInformationStructsList = listContent;
        for(int i = 0; i < listContent.Count; i++)
        {
            Debug.Log($"{i}: label interval: {listContent[i].LabelInterval}");
        }
    }

    [ContextMenu("Test!")]
    public void CreateVisualization()
    {
        Start();
        
        //int[] multidim = { 0, 1, 2, 3 };
        if (_createWithDefaults)
        {
            _axisInformationStructs[0] = new AxisInformationStruct(_controller.DefaultCategoricalFlag, _controller.DefaultShowAxisFlag, _controller.DefaultAxisPrefab, _controller.DefaultNumberOfTicks, _controller.DefaultLabelIntervall, _controller.DefaultLabelOrientation, _controller.DefaultLabelDecimalPlaces);
            _axisInformationStructs[1] = new AxisInformationStruct(_controller.DefaultCategoricalFlag, _controller.DefaultShowAxisFlag, _controller.DefaultAxisPrefab, _controller.DefaultNumberOfTicks, _controller.DefaultLabelIntervall, _controller.DefaultLabelOrientation, _controller.DefaultLabelDecimalPlaces);

            _axisInformationStructs3D[0] = new AxisInformationStruct(_controller.DefaultCategoricalFlag, _controller.DefaultShowAxisFlag, _controller.DefaultAxisPrefab, _controller.DefaultNumberOfTicks, _controller.DefaultLabelIntervall, _controller.DefaultLabelOrientation, _controller.DefaultLabelDecimalPlaces);
            _axisInformationStructs3D[1] = new AxisInformationStruct(_controller.DefaultCategoricalFlag, _controller.DefaultShowAxisFlag, _controller.DefaultAxisPrefab, _controller.DefaultNumberOfTicks, _controller.DefaultLabelIntervall, _controller.DefaultLabelOrientation, _controller.DefaultLabelDecimalPlaces);
            _axisInformationStructs3D[2] = new AxisInformationStruct(_controller.DefaultCategoricalFlag, _controller.DefaultShowAxisFlag, _controller.DefaultAxisPrefab, _controller.DefaultNumberOfTicks, _controller.DefaultLabelIntervall, _controller.DefaultLabelOrientation, _controller.DefaultLabelDecimalPlaces);

            switch (_visualizationType)
            {
                case u2visGeneralController.VisType.BarChart2D:
                    BarChart2DWrapper wrapper = _controller.CreateVis<BarChart2DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapper.SetBarChart2DValues(0.9f, _controller.Default2DBarChartMesh);
                    wrapper.Initilize(
                            size: _controller.DefaultSize,
                            selectedMinItem: _controller.DefaultMinItem,
                            selectedMaxItem: _controller.DefaultMaxItem,
                            axisInformation: _axisInformationStructs,
                            style: _controller.DefaultStyle
                        );
                    //because called from editor without update cycle:
                    wrapper.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.BarChart3D:
                    BarChart3DWrapper wrapperBC3D = _controller.CreateVis<BarChart3DWrapper>(_dataProvider, new int[] { 0,1,2}, _parentToBe, _creatorName);
                    wrapperBC3D.SetBarChart3DValues(new Vector2(0.9f, 0.9f), _controller.Default3DBarChartMesh);
                    //TODO: size does not scale axes. Is a u2vis issue, not a wrapper issue. Has to be fixed some day in the future. Maybe use size for scale? Ask Marc
                    wrapperBC3D.Initilize(
                            size: _controller.DefaultSize,
                            selectedMinItem: _controller.DefaultMinItem,
                            selectedMaxItem: _controller.DefaultMaxItem,
                            axisInformation: _axisInformationStructs3D,
                            indicesOfMultiDimensionDataDimensions: _multiDimensionalIndexes.ToArray(),
                            style: _controller.DefaultStyle
                        ) ;
                    //because called from editor without update cycle:
                    wrapperBC3D.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.HeightMap:
                    HeightMapWrapper wrapperHM = _controller.CreateVis<HeightMapWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapperHM.Initilize(
                            size: _controller.DefaultSize,
                            selectedMinItem: _controller.DefaultMinItem,
                            selectedMaxItem: _controller.DefaultMaxItem,
                            axisInformation: _axisInformationStructs3D,
                            indicesOfMultiDimensionDataDimensions: _multiDimensionalIndexes.ToArray(),
                            style: _controller.DefaultCategoricalStyle
                        );
                    //because called from editor without update cycle:
                    wrapperHM.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.LineChart2D:
                    LineChart2DWrapper wrapperLC2D = _controller.CreateVis<LineChart2DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapperLC2D.Initilize(
                            size: _controller.DefaultSize,
                            selectedMinItem: _controller.DefaultMinItem,
                            selectedMaxItem: _controller.DefaultMaxItem,
                            axisInformation: _axisInformationStructs3D,
                            indicesOfMultiDimensionDataDimensions: _multiDimensionalIndexes.ToArray(),
                            style: _controller.DefaultCategoricalStyle
                        );
                    //because called from editor without update cycle:
                    wrapperLC2D.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.LineChart3D:
                    LineChart3DWrapper wrapperLC3D = _controller.CreateVis<LineChart3DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapperLC3D.Initilize(
                            size: _controller.DefaultSize,
                            selectedMinItem: _controller.DefaultMinItem,
                            selectedMaxItem: _controller.DefaultMaxItem,
                            axisInformation: _axisInformationStructs3D,
                            indicesOfMultiDimensionDataDimensions: _multiDimensionalIndexes.ToArray(),
                            style: _controller.DefaultCategoricalStyle
                         );
                    //because called from editor without update cycle:
                    wrapperLC3D.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.ParallelCoordinates:
                    AxisInformationStruct[] axisInformationStructPC = new AxisInformationStruct[4];
                    axisInformationStructPC[0] = new AxisInformationStruct(_controller.DefaultCategoricalFlag, _controller.DefaultShowAxisFlag, _controller.DefaultAxisPrefab, _controller.DefaultNumberOfTicks, _controller.DefaultLabelIntervall, _controller.DefaultLabelOrientation, _controller.DefaultLabelDecimalPlaces);
                    axisInformationStructPC[1] = new AxisInformationStruct(_controller.DefaultCategoricalFlag, _controller.DefaultShowAxisFlag, _controller.DefaultAxisPrefab, _controller.DefaultNumberOfTicks, _controller.DefaultLabelIntervall, _controller.DefaultLabelOrientation, _controller.DefaultLabelDecimalPlaces);
                    axisInformationStructPC[2] = new AxisInformationStruct(_controller.DefaultCategoricalFlag, _controller.DefaultShowAxisFlag, _controller.DefaultAxisPrefab, _controller.DefaultNumberOfTicks, _controller.DefaultLabelIntervall, _controller.DefaultLabelOrientation, _controller.DefaultLabelDecimalPlaces);
                    ParallelCoordinatesWrapper wrapperPC = _controller.CreateVis<ParallelCoordinatesWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapperPC.Initilize(
                        size: new Vector3(2, 1, 1),
                        selectedMinItem: _controller.DefaultMinItem,
                        selectedMaxItem: _controller.DefaultMaxItem,
                        axisInformation: axisInformationStructPC,
                        style: _controller.DefaultCategoricalStyle
                        );
                    //because called from editor without update cycle:
                    wrapperPC.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.PieChart2D:
                    PieChart2DWrapper wrapperPC2D = _controller.CreateVis<PieChart2DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapperPC2D.Initilize(
                        size: _controller.DefaultSize,
                        selectedMinItem: _controller.DefaultMinItem,
                        selectedMaxItem: _controller.DefaultMaxItem,
                        axisInformation: _axisInformationStructs3D,
                        indicesOfMultiDimensionDataDimensions: _multiDimensionalIndexes.ToArray(),
                        style: _controller.DefaultCategoricalStyle
                        );
                    //because called from editor without update cycle:
                    wrapperPC2D.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.PieChart3D:
                    PieChart3DWrapper wrapperPC3D = _controller.CreateVis<PieChart3DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapperPC3D.Initilize(
                        size: _controller.DefaultSize,
                        selectedMinItem: _controller.DefaultMinItem,
                        selectedMaxItem: _controller.DefaultMaxItem,
                        axisInformation: _axisInformationStructs3D,
                        indicesOfMultiDimensionDataDimensions: _multiDimensionalIndexes.ToArray(),
                        style: _controller.DefaultCategoricalStyle
                        );
                    //because called from editor without update cycle:
                    wrapperPC3D.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.Scatterplot:
                    ScatterplotWrapper wrapperSC = _controller.CreateVis<ScatterplotWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapperSC.SetScatterplotValues(Vector3.zero, Vector3.one, true);
                    AxisInformationStruct[] axisInformationStructSP = new AxisInformationStruct[3];
                    axisInformationStructSP[0] = new AxisInformationStruct(_controller.DefaultCategoricalFlag, _controller.DefaultShowAxisFlag, _controller.DefaultAxisPrefab, _controller.DefaultNumberOfTicks, _controller.DefaultLabelIntervall, _controller.DefaultLabelOrientation, _controller.DefaultLabelDecimalPlaces);
                    axisInformationStructSP[1] = new AxisInformationStruct(_controller.DefaultCategoricalFlag, _controller.DefaultShowAxisFlag, _controller.DefaultAxisPrefab, _controller.DefaultNumberOfTicks, _controller.DefaultLabelIntervall, _controller.DefaultLabelOrientation, _controller.DefaultLabelDecimalPlaces);
                    axisInformationStructSP[2] = new AxisInformationStruct(_controller.DefaultCategoricalFlag, _controller.DefaultShowAxisFlag, _controller.DefaultAxisPrefab, _controller.DefaultNumberOfTicks, _controller.DefaultLabelIntervall, _controller.DefaultLabelOrientation, _controller.DefaultLabelDecimalPlaces);
                    wrapperSC.Initilize(
                        size: _controller.DefaultSize,
                        selectedMinItem: _controller.DefaultMinItem,
                        selectedMaxItem: _controller.DefaultMaxItem,
                        axisInformation: axisInformationStructSP,
                        style: _controller.DefaultStyle
                        );
                    wrapperSC.RebuildFromEditorCode();
                    break;
            }
            return;
        }
        else
        {
            #region set null values to defaults
            Mesh barChart2DMesh;
            if (_2DBarChartMesh == null)
                barChart2DMesh = _controller.Default2DBarChartMesh;
            else
                barChart2DMesh = _2DBarChartMesh;

            Mesh barChart3DMesh;
            if (_3DBarChartMesh == null)
                barChart3DMesh = _controller.Default3DBarChartMesh;
            else
                barChart3DMesh = _3DBarChartMesh;

            GenericVisualizationStyle style;
            if (_style == null)
                style = _controller.DefaultStyle;
            else
                style = _style;

            GenericAxisView axisPrefab;
            if (_axisPrefab == null)
                axisPrefab = _controller.DefaultAxisPrefab;
            else
                axisPrefab = _axisPrefab;

            Material areaMaterial;
            if (_areaMaterial == null)
                areaMaterial = _controller.DefaultAreaMaterial;
            else
                areaMaterial = _areaMaterial;

            Material lineMaterial;
            if (_lineMaterial == null)
                lineMaterial = _controller.DefaultLineMaterial;
            else
                lineMaterial = _lineMaterial;

            Material scatterplotMaterial;
            if (_scatterplotMaterial == null)
                scatterplotMaterial = _controller.DefaultScatterplotMaterial;
            else
                scatterplotMaterial = _scatterplotMaterial;

            #endregion

            _axisInformationStructs[0] = new AxisInformationStruct(_categoricalFlag, _showAxisFlag, axisPrefab, _numberOfTicks, _labelInterval, _labelOrientation, _labelDecimalPlaces);
            _axisInformationStructs[1] = new AxisInformationStruct(_categoricalFlag, _showAxisFlag, axisPrefab, _numberOfTicks, _labelInterval, _labelOrientation, _labelDecimalPlaces);

            _axisInformationStructs3D[0] = new AxisInformationStruct(_categoricalFlag, _showAxisFlag, axisPrefab, _numberOfTicks, _labelInterval, _labelOrientation, _labelDecimalPlaces);
            _axisInformationStructs3D[1] = new AxisInformationStruct(_categoricalFlag, _showAxisFlag, axisPrefab, _numberOfTicks, _labelInterval, _labelOrientation, _labelDecimalPlaces);
            _axisInformationStructs3D[2] = new AxisInformationStruct(_categoricalFlag, _showAxisFlag, axisPrefab, _numberOfTicks, _labelInterval, _labelOrientation, _labelDecimalPlaces);

            
            switch (_visualizationType)
            {
                case u2visGeneralController.VisType.BarChart2D:
                    BarChart2DWrapper wrapper = _controller.CreateVis<BarChart2DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    
                    wrapper.SetBarChart2DValues(0.9f, barChart2DMesh);
                    wrapper.Initilize(
                            size: _size,
                            selectedMinItem: _minItem,
                            selectedMaxItem: _maxItem,
                            axisInformation: axisInformationStructsList.ToArray(),
                            style: style
                        );
                    //because called from editor without update cycle:
                    wrapper.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.BarChart3D:
                    BarChart3DWrapper wrapperBC3D = _controller.CreateVis<BarChart3DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapperBC3D.SetBarChart3DValues(new Vector2(0.9f, 0.9f), barChart3DMesh);
                    //TODO: size does not scale axes. Is a u2vis issue, not a wrapper issue. Has to be fixed some day in the future. Maybe use size for scale? Ask Marc
                    wrapperBC3D.Initilize(
                            size: _size,
                            selectedMinItem: _minItem,
                            selectedMaxItem: _maxItem,
                            axisInformation: axisInformationStructsList.ToArray(),
                            indicesOfMultiDimensionDataDimensions: _multiDimensionalIndexes.ToArray(),
                            style: style
                        );
                    //because called from editor without update cycle:
                    wrapperBC3D.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.HeightMap:
                    HeightMapWrapper wrapperHM = _controller.CreateVis<HeightMapWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapperHM.Initilize(
                            size: _size,
                            selectedMinItem: _minItem,
                            selectedMaxItem: _maxItem,
                            axisInformation: axisInformationStructsList.ToArray(),
                            indicesOfMultiDimensionDataDimensions: _multiDimensionalIndexes.ToArray(),
                            style: style
                        );
                    //because called from editor without update cycle:
                    wrapperHM.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.LineChart2D:
                    LineChart2DWrapper wrapperLC2D = _controller.CreateVis<LineChart2DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapperLC2D.Initilize(
                            size: _size,
                            selectedMinItem: _minItem,
                            selectedMaxItem: _maxItem,
                            axisInformation: axisInformationStructsList.ToArray(),
                            indicesOfMultiDimensionDataDimensions: _multiDimensionalIndexes.ToArray(),
                            style: style
                        );
                    //because called from editor without update cycle:
                    wrapperLC2D.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.LineChart3D:
                    LineChart3DWrapper wrapperLC3D = _controller.CreateVis<LineChart3DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapperLC3D.Initilize(
                            size: _size,
                            selectedMinItem: _minItem,
                            selectedMaxItem: _maxItem,
                            axisInformation: axisInformationStructsList.ToArray(),
                            indicesOfMultiDimensionDataDimensions: _multiDimensionalIndexes.ToArray(),
                            style: style
                         );
                    //because called from editor without update cycle:
                    wrapperLC3D.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.ParallelCoordinates:
                    ParallelCoordinatesWrapper wrapperPC = _controller.CreateVis<ParallelCoordinatesWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapperPC.Initilize(
                        size: _size,
                        selectedMinItem: _minItem,
                        selectedMaxItem: _maxItem,
                        axisInformation: axisInformationStructsList.ToArray(),
                        style: style
                        );
                    //because called from editor without update cycle:
                    wrapperPC.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.PieChart2D:
                    PieChart2DWrapper wrapperPC2D = _controller.CreateVis<PieChart2DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapperPC2D.Initilize(
                            size: _size,
                            selectedMinItem: _minItem,
                            selectedMaxItem: _maxItem,
                            axisInformation: axisInformationStructsList.ToArray(),
                            indicesOfMultiDimensionDataDimensions: _multiDimensionalIndexes.ToArray(),
                            style: style
                        );
                    //because called from editor without update cycle:
                    wrapperPC2D.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.PieChart3D:
                    PieChart3DWrapper wrapperPC3D = _controller.CreateVis<PieChart3DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapperPC3D.Initilize(
                            size: _size,
                            selectedMinItem: _minItem,
                            selectedMaxItem: _maxItem,
                            axisInformation: axisInformationStructsList.ToArray(),
                            indicesOfMultiDimensionDataDimensions: _multiDimensionalIndexes.ToArray(),
                            style: style
                        );
                    //because called from editor without update cycle:
                    wrapperPC3D.RebuildFromEditorCode();
                    break;
                case u2visGeneralController.VisType.Scatterplot:
                    ScatterplotWrapper wrapperSC = _controller.CreateVis<ScatterplotWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parentToBe, _creatorName);
                    wrapperSC.SetScatterplotValues(_minZoomLevel, _maxZoomLevel, _displayRelativeValues);
                    wrapperSC.Initilize(
                        size: _size,
                        selectedMinItem: _minItem,
                        selectedMaxItem: _maxItem,
                        axisInformation: axisInformationStructsList.ToArray(),
                        style: style
                        );
                    wrapperSC.RebuildFromEditorCode();
                    break;
            }
        }
        
    }

}
