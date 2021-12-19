using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using u2vis;
using DataSetHandling;
using System;

public class GeneralVisulizationWrapper : MonoBehaviour
{
    /// <summary>
    /// Enum which represents the type of the visualization
    /// </summary>
    public enum VisType
    {
        BarChart2D,
        BarChart3D,
        HeightMap,
        LineChart2D,
        LineChart3D,
        ParallelCoordinates,
        PieChart2D,
        PieChart3D,
        RevolvedCharts,
        Scatterplot,
        StackedBar
    }
    /// <summary>
    /// Id of the visualization
    /// </summary>
    [SerializeField]
    private string _visID = null;
    /// <summary>
    /// Describes which type the visualization is of. Can only be set at vis creation.
    /// </summary>
    [SerializeField]
    private VisType _visType;
    /// <summary>
    /// The Id of the VisCreator (either user or device, depends on use case of this framework)
    /// </summary>
    [SerializeField]
    private string _creatorName;
    /// <summary>
    /// The desired size of the Visualization (Not the scale of the GameObject)
    /// </summary>
    [SerializeField]
    private Vector3 _visSize;
    /// <summary>
    /// The list of color gradients representing the visualization style.
    /// </summary>
    [SerializeField]
    private Gradient[] _colorMappings = new Gradient[1];
    /// <summary>
    /// The color for highlighted values. Defaults to red
    /// </summary>
    [SerializeField]
    private Color _highlightColor = Color.red;
    /// <summary>
    /// The data provider of the visualization. 
    /// </summary>
    [SerializeField]
    private AbstractDataProvider _dataProvider;
    /// <summary>
    /// The minimum item, where the vis starts. Builds the item range together with  <see cref="_selectedMaxItem"/>
    /// </summary>
    [SerializeField]
    private int _selectedMinItem;
    /// <summary>
    /// The maximum item, where the vis ends. Builds the item range together with  <see cref="_selectedMinItem"/>
    /// </summary>
    [SerializeField]
    private int _selectedMaxItem;
    [SerializeField]
    private AxisInformation[] _axisInformation;
    /// <summary>
    /// Is only used for vis types like BarChart3D. Describes the dimensions which should be aggregated.
    /// </summary>
    [SerializeField]
    private int[] _indicesOfMultiDimensionDataDimensions = null;

    public string VisID { get => _visID; }
    public VisType VisType1 { get => _visType; }
    public string CreatorName { get => _creatorName; set { _creatorName = value; UpdateVis(); } }
    public Vector3 VisSize { get => _visSize; set { _visSize = value; UpdateVis(); } }
    public Gradient[] ColorMappings { get => _colorMappings; set { _colorMappings = value; UpdateVis(); } }
    public Color HighlightColor { get => _highlightColor; set { _highlightColor = value; UpdateVis(); } }
    public AbstractDataProvider DataProvider { get => _dataProvider; set { _dataProvider = value; UpdateVis(); } }
    public int SelectedMinItem { get => _selectedMinItem; set { _selectedMinItem = value; UpdateVis(); } }
    public int SelectedMaxItem { get => _selectedMaxItem; set { _selectedMaxItem = value; UpdateVis(); } }
    internal AxisInformation[] AxisInformation { get => _axisInformation; set { _axisInformation = value; UpdateVis(); } }
    public int[] IndicesOfMultiDimensionDataDimensions { get => _indicesOfMultiDimensionDataDimensions; set { _indicesOfMultiDimensionDataDimensions = value; UpdateVis(); } }

    void Start()
    {
        //@TODO: Add some kind of Event in DataProvider for updated data and listen to it here
    }

    public GeneralVisulizationWrapper Initilize(VisType visType, string creatorName, Vector3 size, Gradient[] colormappings, Color highlightcolor, AbstractDataProvider dataProvider, int selectedMinItem, int selectedMaxItem, AxisInformation[] axisInformation, int[] indicesOfMultiDimensionDataDimensions = null)
    {
        if (_visID != null)
        {
            Debug.LogError("This visualization is already initialized!");
            throw new Exception("vis already initialized!");
        }

        //setting the fields

        //generate id for vis here and set _visID
        _visType = visType;
        _creatorName = creatorName;
        _visSize = size;
        _colorMappings = colormappings;
        _highlightColor = highlightcolor;
        _dataProvider = dataProvider;
        _selectedMaxItem = selectedMaxItem;
        _selectedMinItem = selectedMinItem;
        _axisInformation = axisInformation;
        if (indicesOfMultiDimensionDataDimensions != null)
        {
            _indicesOfMultiDimensionDataDimensions = indicesOfMultiDimensionDataDimensions;
        }

        //@TODO: Add InitBehaviour!
        return this;
    }
    public void UpdateVis()
    {
        throw new NotImplementedException();
        //@TODO update functionality based on contents of private fields
    }

    //@TODO write setters vor each AxisProperty by dimension



}

public class AxisInformation
{
    /// <summary>
    /// The index the dimension has in the dataset.
    /// </summary>
    public int DimensionIndex { get; private set; }
    /// <summary>
    /// flags if the dimension is categorical or not
    /// </summary>
    public bool IsCategorigal { get; private set; }
    /// <summary>
    /// flag if the visual axis and the labels should be drawn
    /// </summary>
    public bool ShowAxis { get; private set; }
    /// <summary>
    /// Prefab for the Axis.
    /// </summary>
    public GenericAxisView AxisPrefab { get; private set; }
    /// <summary>
    /// States how often a tick should be drawn //@TODO: how to define this?
    /// </summary>
    public float TickIntervall { get; private set; }
    /// <summary>
    /// Defines the number n in "Every n-th tick has a label"
    /// </summary>
    public int LabelInterval { get; private set; }
    /// <summary>
    /// Orientation of the Label
    /// </summary>
    public LabelOrientation LabelOrientation { get; private set; }
    /// <summary>
    /// Decimal places of the label. Will be round to the defined number of decimal places.
    /// </summary>
    public int DecimalPlacesOfLabels { get; private set; }
}