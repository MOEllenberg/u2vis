using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using u2vis;
using DataSetHandling;
using System;
using static u2visGeneralController;

public abstract class GeneralVisulizationWrapper : MonoBehaviour
{

    /// <summary>
    /// This struct holds the information 1 axis needs.
    /// </summary>
    public struct AxisInformationStruct{
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
        public int NumberOfTicks { get; private set; }
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
        public AxisInformationStruct(bool isCategorigal, bool showAxis, GenericAxisView axisPrefab, int numberOfTicks, int labelInterval, LabelOrientation labelOrientation, int decimalPlacesOfLabels)
        {
            IsCategorigal = isCategorigal;
            ShowAxis = showAxis;
            AxisPrefab = axisPrefab;
            NumberOfTicks = numberOfTicks;
            LabelInterval = labelInterval;
            LabelOrientation = labelOrientation;
            DecimalPlacesOfLabels = decimalPlacesOfLabels;
        }
    }

    #region serialized fields
    /// <summary>
    /// Id of the visualization
    /// </summary>
    [SerializeField]
    protected string _visID = null;
    /// <summary>
    /// Describes which type the visualization is of. Can only be set at vis creation.
    /// </summary>
    [SerializeField]
    protected VisType _visType;
    /// <summary>
    /// The Id of the VisCreator (either user or device, depends on use case of this framework)
    /// </summary>
    [SerializeField]
    protected string _creatorName;
    /// <summary>
    /// The desired size of the Visualization (Not the scale of the GameObject)
    /// </summary>
    [SerializeField]
    protected Vector3 _visSize;
    /// <summary>
    /// The list of color gradients representing the visualization style.
    /// </summary>
    [SerializeField]
    protected Gradient[] _colorMappings = new Gradient[1];
    /// <summary>
    /// The color for highlighted values. Defaults to red
    /// </summary>
    [SerializeField]
    protected Color _highlightColor = Color.red;
    /// <summary>
    /// The data provider of the visualization. 
    /// </summary>
    [SerializeField]
    protected AbstractDataProvider _dataProvider;
    /// <summary>
    /// The minimum item, where the vis starts. Builds the item range together with  <see cref="_selectedMaxItem"/>
    /// </summary>
    [SerializeField]
    protected int _selectedMinItem;
    /// <summary>
    /// The maximum item, where the vis ends. Builds the item range together with  <see cref="_selectedMinItem"/>
    /// </summary>
    [SerializeField]
    protected int _selectedMaxItem;
    /// <summary>
    /// Array of the AxisInformation. Should have the length equivalent to the number of axes the vis has.
    /// </summary>
    [SerializeField]
    protected AxisInformationStruct[] _axisInformation;

    /// <summary>
    /// Is only used for vis types like BarChart3D. Describes the dimensions which should be aggregated.
    /// </summary>
    [SerializeField]
    protected int[] _indicesOfMultiDimensionDataDimensions = null;


    /// <summary>
    /// Stores the Style refrence of the Vis. 
    /// </summary>
    protected GenericVisualizationStyle _style;

    #endregion

    #region public properties
    //for all properties, see fields.
    public string VisID { get => _visID; }
    public VisType VisType1 { get => _visType; }
    public string CreatorName { get => _creatorName; set => _creatorName = value; }
    public Vector3 VisSize { get => _visSize; set { _visSize = value; UpdateCompleteVis(); } }
    public Gradient[] ColorMappings { get => _colorMappings; set { SetColorMappings(value); UpdateContentMeshes(); } }
    public Color HighlightColor { get => _highlightColor; set { SetHighlightColor(value); UpdateContentMeshes(); } }
    public AbstractDataProvider DataProvider { get => _dataProvider; set { _dataProvider = value; UpdateCompleteVis(); } }
    public int SelectedMinItem { get => _selectedMinItem; set { _selectedMinItem = value; UpdateCompleteVis(); } }
    public int SelectedMaxItem { get => _selectedMaxItem; set { _selectedMaxItem = value; UpdateCompleteVis(); } }
    public AxisInformationStruct[] AxisInformation { get => _axisInformation; set { _axisInformation = value; UpdateAxes(); } }
   // public int[] IndicesOfMultiDimensionDataDimensions { get => _indicesOfMultiDimensionDataDimensions; set { _indicesOfMultiDimensionDataDimensions = value; UpdateCompleteVis(); } }

    public BaseVisualizationView VisualizationView { get; protected set; }
    public GenericDataPresenter DataPresenter { get; protected set; }
    public GenericVisualizationStyle VisStyle { get=> _style; set { SetVisStyle(value); } }
    #endregion

    #region internal fields
    /// <summary>
    /// flag if the visualization is initialized
    /// </summary>
    protected bool initilized = false;
    protected int[] _dimIndices = null;  //childWrapper have to use this instead of the index of the axis struct.
    #endregion
    void Start()
    {
        
    }

    /// <summary>
    /// Creates a new visualization. Visualization gets its <see cref="VisType"/>, ID and the creator name. It still needs to be initilized with either <see cref="Initilize"/> or <see cref="InitilizeWithDefaults"/>
    /// </summary>
    /// <param name="visType"></param>
    /// <param name="creatorName"></param>
    public void Create(AbstractDataProvider dataProvider, int[] dimIndices, string creatorName)
    {

        //generate id for vis here and set _visID
        _visID = u2visGeneralController.Instance.NextID.ToString();
        _dataProvider = dataProvider;
        _dimIndices = dimIndices;
        _creatorName = creatorName;
    }

    /// <summary>
    /// Initializes the visualization with the given values.
    /// </summary>
    /// <param name="size"></param>
    /// <param name="dataProvider"></param>
    /// <param name="selectedMinItem"></param>
    /// <param name="selectedMaxItem"></param>
    /// <param name="axisInformation"></param>
    /// <param name="indicesOfMultiDimensionDataDimensions">Only needs to be used for Visualizations with a <see cref="MultiDimDataPresenter"/>, otherwise should be null</param>
    /// <param name="colormappings">only needs to be used if no <see cref="GenericVisualizationStyle"/> is provided with style. Will create a new Style together with highlightcolor.</param>
    /// <param name="highlightcolor">only needs to be used if no <see cref="GenericVisualizationStyle"/> is provided with style. Will create a new Style attached to this visualization together with colormappings.</param>
    /// <param name="style">only needs to be used if neither colormappings or highlightcolor are provided. Is a refrence to an extern style, which will be used for the visualization.</param>
    public void Initilize(Vector3 size, int selectedMinItem, int selectedMaxItem, AxisInformationStruct[] axisInformation, int[] indicesOfMultiDimensionDataDimensions = null, Gradient[] colormappings = null, Color? highlightcolor = null, GenericVisualizationStyle style = null)
    {
        if (initilized)
        {
            Debug.LogError("This visualization is already initialized!");
            throw new Exception("vis already initialized!");
        }

        if ((colormappings == null || highlightcolor == null) && style == null)
        {
            Debug.Log(style);
            Debug.LogError("either colormappings and highlightcolor or a style have to be set!");
            throw new Exception("either colormappings and highlightcolor or a style have to be set!");
        }

        if (style == null && colormappings != null && highlightcolor != null)
        {
            GenericVisualizationStyle visStyle = gameObject.AddComponent<GenericVisualizationStyle>();
            visStyle.HighlightColor = (Color)highlightcolor;
            visStyle.SetColorMappings(colormappings);
            visStyle.name = VisID + "_style";
            _colorMappings = colormappings;
            _highlightColor = (Color)highlightcolor;
            _style = visStyle;
        }

        if (style != null && colormappings == null && highlightcolor == null)
        {
            _colorMappings = style.GetColorMappings();
            _highlightColor = style.HighlightColor;
            _style = style;
        }

        //setting the fields

        _visSize = size;
        _selectedMaxItem = selectedMaxItem;
        _selectedMinItem = selectedMinItem;
        _axisInformation = axisInformation;
        if (indicesOfMultiDimensionDataDimensions != null)
        {
            _indicesOfMultiDimensionDataDimensions = indicesOfMultiDimensionDataDimensions;
        }

        //@TODO: Add InitBehaviour!
        InitilizeVisSpecific(false);
        u2visGeneralController.Instance.AddVis(this);
    }


    public abstract GeneralVisulizationWrapper Generate(AbstractDataProvider dataProvider, int[] dimIndices, Transform parent, string name);

    public void RebuildFromEditorCode()
    {
        VisualizationView.RebuildFromEditorCode();
    }

    /// <summary>
    /// Initializes the visualization with the default values given in <see cref="u2visGeneralController"/>
    /// </summary>
    public void InitilizeWithDefaults()
    {

        if (initilized)
        {
            Debug.LogError("This visualization is already initialized!");
            throw new Exception("vis already initialized!");
        }

        _visSize = u2visGeneralController.Instance.DefaultSize;
        _dataProvider = u2visGeneralController.Instance.DefaultDataprovider; // Dataprovider hat kein default.
        _selectedMaxItem = u2visGeneralController.Instance.DefaultMaxItem;
        _selectedMinItem = u2visGeneralController.Instance.DefaultMinItem;
        _style = u2visGeneralController.Instance.DefaultStyle;
        //@TODO: Add some kind of Event in DataProvider for updated data and listen to it here

        InitilizeVisSpecific(true);
        u2visGeneralController.Instance.AddVis(this);
    }

    /// <summary>
    /// abstract method which needs to be implemented by children to account for vis specific behavior.
    /// </summary>
    /// <param name="withDefaults"></param>
    protected abstract void InitilizeVisSpecific(bool withDefaults);

    #region color related setters
    private void SetVisStyle(GenericVisualizationStyle value)
    {
        throw new NotImplementedException();
    }

    private void SetColorMappings(Gradient[] value)
    {
        throw new NotImplementedException();
    }

    private void SetHighlightColor(Color value)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region set axis values by axis index
    /// <summary>
    /// sets the axis properties for the axis with index axisIndex. if a value of a property is left out, it will be null and then default to the old propertie.
    /// </summary>
    /// <param name="axisIndex"></param>
    /// <param name="dimensionIndexToBe"></param>
    /// <param name="isCategorical"></param>
    /// <param name="showAxis"></param>
    /// <param name="axisPrefab"></param>
    /// <param name="numberOfTicks"></param>
    /// <param name="labelInterval"></param>
    /// <param name="labelOrientation"></param>
    /// <param name="decimalPlacesOfLabels"></param>
    public void SetAxisValuesByAxisIndex(int axisIndex, bool? isCategorical =null, bool? showAxis = null, GenericAxisView axisPrefab=null,int? numberOfTicks=null,int? labelInterval=null, LabelOrientation? labelOrientation=null,int? decimalPlacesOfLabels=null)
    {
        if (axisIndex >= _axisInformation.Length - 1)
        {
            Debug.LogError("Axis Index Does Not Exist!)");
            return;
        }
        AxisInformationStruct oldInfo = _axisInformation[axisIndex];
       // int newAxisDim2 = dimensionIndexToBe ?? oldInfo.DimensionIndex;
        bool newCategorical = isCategorical != null ? (bool)isCategorical : oldInfo.IsCategorigal;
        bool newShowAxis = showAxis != null ? (bool)showAxis : oldInfo.ShowAxis;
        GenericAxisView newAxisPrefab = axisPrefab != null ? axisPrefab : oldInfo.AxisPrefab;
        int newNumberOfTicks = numberOfTicks != null ? (int)numberOfTicks : oldInfo.NumberOfTicks;
        int newLabelInterval = labelInterval != null ? (int)labelInterval : oldInfo.LabelInterval;
        LabelOrientation newLabelOrientation = labelOrientation != null ? (LabelOrientation)labelOrientation : oldInfo.LabelOrientation;
        int newDecimalPlaces = decimalPlacesOfLabels != null ? (int)decimalPlacesOfLabels : oldInfo.DecimalPlacesOfLabels;

        AxisInformation[axisIndex] = new AxisInformationStruct(
            isCategorigal: newCategorical, 
            showAxis: newShowAxis,
            axisPrefab: newAxisPrefab,
            numberOfTicks: newNumberOfTicks,
            labelInterval: newLabelInterval,
            labelOrientation: newLabelOrientation,
            decimalPlacesOfLabels: newDecimalPlaces
        );
        UpdateAxes();
    }

    #endregion

    #region updates
    /// <summary>
    /// Updates the complete visualization with a complete rebuild.
    /// </summary>
    public void UpdateCompleteVis()
    {
        if (!initilized)
        {
            Debug.LogError("This visualization is not initialized! Initialize it first.");
            throw new Exception("vis not initialized!");
        }
        //@TODO update functionality based on contents of private fields

        //workaround. Does Update complete vis, might not be efficient. Might need to be updated.
        InitilizeVisSpecific(false);
    }

    /// <summary>
    /// Only updates the axes of a visualization. 
    /// Not yet implemented. Rebuilds all.
    /// </summary>
    public abstract void UpdateAxes();

    /// <summary>
    /// Only updates the Content Mesh of the visualization.
    /// Not yet implemented. Rebuilds all.
    /// </summary>
    public abstract void UpdateContentMeshes();
    
    #endregion

}