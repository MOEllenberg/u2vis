﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using u2vis;
using DataSetHandling;
using System;
using static u2visGeneralController;

public class GeneralVisulizationWrapper : MonoBehaviour
{

    /// <summary>
    /// This struct holds the information 1 axis needs.
    /// </summary>
    public struct AxisInformationStruct{
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
        public AxisInformationStruct(int dimensionIndex, bool isCategorigal, bool showAxis, GenericAxisView axisPrefab, int numberOfTicks, int labelInterval, LabelOrientation labelOrientation, int decimalPlacesOfLabels)
        {
            DimensionIndex = dimensionIndex;
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
    /// <summary>
    /// Array of the AxisInformation. Should have the length equivalent to the number of axes the vis has.
    /// </summary>
    [SerializeField]
    private AxisInformationStruct[] _axisInformation;
    /// <summary>
    /// Is only used for vis types like BarChart3D. Describes the dimensions which should be aggregated.
    /// </summary>
    [SerializeField]
    private int[] _indicesOfMultiDimensionDataDimensions = null;
    /// <summary>
    /// Stores the Style refrence of the Vis. 
    /// </summary>
    private GenericVisualizationStyle _style;
    /// <summary>
    /// The BarChartMesh. Only needed for 2D/3D Bar Charts.
    /// </summary>
    [SerializeField]
    private Mesh _barChartMesh = null;
    /// <summary>
    /// The Thickness of the 2D Bars of a 2D Bar Chart.
    /// </summary>
    [SerializeField]
    private float? _2DBarThickness = null;
    /// <summary>
    /// The thickness of the 3D Bars of a 3D Bar Chart (as Vector).
    /// </summary>
    [SerializeField]
    private Vector2? _3DBarThickness = null;

    [SerializeField]
    private Vector3? _minZoomLevel = null;

    [SerializeField]
    private Vector3? _maxZoomLevel = null;

    [SerializeField]
    private bool? _displayRelativeValues = null;
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
    public int[] IndicesOfMultiDimensionDataDimensions { get => _indicesOfMultiDimensionDataDimensions; set { _indicesOfMultiDimensionDataDimensions = value; UpdateCompleteVis(); } }

    public BaseVisualizationView VisualizationView { get; private set; }
    public GenericDataPresenter DataPresenter { get; private set; }
    public GenericVisualizationStyle VisStyle { get=> _style; set { SetVisStyle(value); } }
    #endregion

    #region internal fields
    /// <summary>
    /// flag if the visualization is initialized
    /// </summary>
    private bool initilized = false;
    #endregion
    void Start()
    {
        
    }

    /// <summary>
    /// Creates a new visualization. Visualization gets its <see cref="VisType"/>, ID and the creator name. It still needs to be initilized with either <see cref="Initilize"/> or <see cref="InitilizeWithDefaults"/>
    /// </summary>
    /// <param name="visType"></param>
    /// <param name="creatorName"></param>
    public void Create(VisType visType, string creatorName)
    {

        //generate id for vis here and set _visID
        _visID = u2visGeneralController.Instance.NextID.ToString();
        _visType = visType;
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
    public void Initilize(Vector3 size, AbstractDataProvider dataProvider, int selectedMinItem, int selectedMaxItem, AxisInformationStruct[] axisInformation, int[] indicesOfMultiDimensionDataDimensions = null, Gradient[] colormappings = null, Color? highlightcolor = null, GenericVisualizationStyle style=null)
    {
        if (initilized)
        {
            Debug.LogError("This visualization is already initialized!");
            throw new Exception("vis already initialized!");
        }

        if ((colormappings == null || highlightcolor == null) && style == null)
        {
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
        _dataProvider = dataProvider;
        _selectedMaxItem = selectedMaxItem;
        _selectedMinItem = selectedMinItem;
        _axisInformation = axisInformation;
        if (indicesOfMultiDimensionDataDimensions != null)
        {
            _indicesOfMultiDimensionDataDimensions = indicesOfMultiDimensionDataDimensions;
        }

        //@TODO: Add InitBehaviour!
        InitilizeSwitcher(false);
        u2visGeneralController.Instance.AddVis(this);
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

        InitilizeSwitcher(true);
        u2visGeneralController.Instance.AddVis(this);
    }

    #region initialize by type

    //in this region diffrent vis types get initilized in their needed ways.
    private void InitilizeStackedBar(bool withDefaults)
    {
        throw new NotImplementedException();
    }

    private void InitilizeScatterplot(bool withDefaults)
    {
        GenericDataPresenter presenter = gameObject.GetComponent<GenericDataPresenter>();
        Scatterplot2D scatterplot = gameObject.GetComponent<Scatterplot2D>();
        if (presenter is MultiDimDataPresenter)
        {
            Debug.LogError($"this vis uses a Generic Data Presenter instead of {presenter.GetType().Name}");
            throw new Exception($"this vis uses a Generic Data Presenter instead of {presenter.GetType().Name}");
        }
        if (!withDefaults)
        {
            if (!(_axisInformation.Length == 2||_axisInformation.Length==3))
            {
                Debug.LogError($"Wrong amount of AxisInformation. {_axisInformation.Length} are given but needed are 2 or 3.");
                throw new Exception("Wrong amount of AxisInformation.");
            }
            List<int> dimIndices = new List<int>();
            foreach (var information in _axisInformation)
            {
                dimIndices.Add(information.DimensionIndex);
            }
            presenter.Initialize(_dataProvider, _selectedMinItem, _selectedMaxItem, dimIndices.ToArray());
            presenter.ResetAxisProperties();
            AxisPresenter[] axisPresenter = presenter.AxisPresenters;
            for (int i = 0; i < axisPresenter.Length; i++)
            {
                axisPresenter[i].IsCategorical = _axisInformation[i].IsCategorigal;
                axisPresenter[i].LabelOrientation = _axisInformation[i].LabelOrientation;
                axisPresenter[i].LabelTickIntervall = _axisInformation[i].LabelInterval;
                axisPresenter[i].TickIntervall = 1f / (float)_axisInformation[i].NumberOfTicks;
                axisPresenter[i].DecimalPlaces = _axisInformation[i].DecimalPlacesOfLabels;
            }
            presenter.SetAxisPresenters(axisPresenter);
            //TODO: Axisview Prefab and show axis should be in DataPresenter, not in vis view, and there defined per axispresenter. Untill done, values of first axis information are used
            if (_minZoomLevel == null)
            {
                _minZoomLevel = u2visGeneralController.Instance.MinZoomLevel;
            }
            if (_maxZoomLevel == null)
            {
                _maxZoomLevel = u2visGeneralController.Instance.MaxZoomLevel;
            }
            if (_displayRelativeValues == null)
            {
                _displayRelativeValues = u2visGeneralController.Instance.DisplayRelativeValues;
            }
        }
        else
        {
            List<int> dimIndices = new List<int>();
            dimIndices.Add(u2visGeneralController.Instance.DefaultDimensionIndices[0]);
            dimIndices.Add(u2visGeneralController.Instance.DefaultDimensionIndices[1]);
            Debug.Log(_dataProvider.gameObject.name);
            presenter.Initialize(_dataProvider, _selectedMinItem, _selectedMaxItem, dimIndices.ToArray());
            presenter.ResetAxisProperties();
            _axisInformation = new AxisInformationStruct[2];
            for (int i = 0; i < _axisInformation.Length; i++)
            {
                _axisInformation[i] = new AxisInformationStruct(
                    u2visGeneralController.Instance.DefaultDimensionIndices[i],
                    u2visGeneralController.Instance.DefaultCategoricalFlag,
                    u2visGeneralController.Instance.DefaultShowAxisFlag,
                    u2visGeneralController.Instance.DefaultAxisPrefab,
                    u2visGeneralController.Instance.DefaultNumberOfTicks,
                    u2visGeneralController.Instance.DefaultLabelIntervall,
                    u2visGeneralController.Instance.DefaultLabelOrientation,
                    u2visGeneralController.Instance.DefaultLabelDecimalPlaces);
            }
            AxisPresenter[] axisPresenter = presenter.AxisPresenters;
            for (int i = 0; i < axisPresenter.Length; i++)
            {
                axisPresenter[i].IsCategorical = _axisInformation[i].IsCategorigal;
                axisPresenter[i].LabelOrientation = _axisInformation[i].LabelOrientation;
                axisPresenter[i].LabelTickIntervall = _axisInformation[i].LabelInterval;
                axisPresenter[i].TickIntervall = 1f / (float)_axisInformation[i].NumberOfTicks;
                axisPresenter[i].DecimalPlaces = _axisInformation[i].DecimalPlacesOfLabels;
            }
            presenter.SetAxisPresenters(axisPresenter);
            //TODO: Axisview Prefab and show axis should be in DataPresenter, not in vis view, and there defined per axispresenter. Untill done, values of first axis information are used
            _minZoomLevel = u2visGeneralController.Instance.MinZoomLevel;
            _maxZoomLevel = u2visGeneralController.Instance.MaxZoomLevel;
            _displayRelativeValues = u2visGeneralController.Instance.DisplayRelativeValues;
        }
        scatterplot.Size = _visSize;
        scatterplot.Initialize(presenter, _axisInformation[0].AxisPrefab, _style);
        scatterplot.ZoomMin = (Vector3)_minZoomLevel;
        scatterplot.ZoomMax = (Vector3)_maxZoomLevel;
        scatterplot.DisplayRelativeValues = (bool)_displayRelativeValues;
        scatterplot.ShowAxes = _axisInformation[0].ShowAxis;
        scatterplot.Rebuild();

        DataPresenter = presenter;
        VisualizationView = scatterplot;
        initilized = true;
    }

    private void InitilizeParallelCoordinates(bool withDefaults)
    {
        GenericDataPresenter presenter = gameObject.GetComponent<GenericDataPresenter>();
        ParallelCoordinates parallelCoordinates = gameObject.GetComponent<ParallelCoordinates>();
        if (presenter is MultiDimDataPresenter)
        {
            Debug.LogError($"this vis uses a Generic Data Presenter instead of {presenter.GetType().Name}");
            throw new Exception($"this vis uses a Generic Data Presenter instead of {presenter.GetType().Name}");
        }
        if (!withDefaults)
        {
            List<int> dimIndices = new List<int>();
            foreach (var information in _axisInformation)
            {
                dimIndices.Add(information.DimensionIndex);
            }
            presenter.Initialize(_dataProvider, _selectedMinItem, _selectedMaxItem, dimIndices.ToArray());
            presenter.ResetAxisProperties();
            AxisPresenter[] axisPresenter = presenter.AxisPresenters;
            for (int i = 0; i < axisPresenter.Length; i++)
            {
                axisPresenter[i].IsCategorical = _axisInformation[i].IsCategorigal;
                axisPresenter[i].LabelOrientation = _axisInformation[i].LabelOrientation;
                axisPresenter[i].LabelTickIntervall = _axisInformation[i].LabelInterval;
                axisPresenter[i].TickIntervall = 1f / (float)_axisInformation[i].NumberOfTicks;
                axisPresenter[i].DecimalPlaces = _axisInformation[i].DecimalPlacesOfLabels;
            }
            presenter.SetAxisPresenters(axisPresenter);
            //TODO: Axisview Prefab and show axis should be in DataPresenter, not in vis view, and there defined per axispresenter. Untill done, values of first axis information are used
        }
        else
        {
            List<int> dimIndices = new List<int>();
            dimIndices.Add(u2visGeneralController.Instance.DefaultDimensionIndices[0]);
            dimIndices.Add(u2visGeneralController.Instance.DefaultDimensionIndices[1]);
            Debug.Log(_dataProvider.gameObject.name);
            presenter.Initialize(_dataProvider, _selectedMinItem, _selectedMaxItem, dimIndices.ToArray());
            presenter.ResetAxisProperties();
            _axisInformation = new AxisInformationStruct[2];
            for (int i = 0; i < _axisInformation.Length; i++)
            {
                _axisInformation[i] = new AxisInformationStruct(
                    u2visGeneralController.Instance.DefaultDimensionIndices[i],
                    u2visGeneralController.Instance.DefaultCategoricalFlag,
                    u2visGeneralController.Instance.DefaultShowAxisFlag,
                    u2visGeneralController.Instance.DefaultAxisPrefab,
                    u2visGeneralController.Instance.DefaultNumberOfTicks,
                    u2visGeneralController.Instance.DefaultLabelIntervall,
                    u2visGeneralController.Instance.DefaultLabelOrientation,
                    u2visGeneralController.Instance.DefaultLabelDecimalPlaces);
            }
            AxisPresenter[] axisPresenter = presenter.AxisPresenters;
            for (int i = 0; i < axisPresenter.Length; i++)
            {
                axisPresenter[i].IsCategorical = _axisInformation[i].IsCategorigal;
                axisPresenter[i].LabelOrientation = _axisInformation[i].LabelOrientation;
                axisPresenter[i].LabelTickIntervall = _axisInformation[i].LabelInterval;
                axisPresenter[i].TickIntervall = 1f / (float)_axisInformation[i].NumberOfTicks;
                axisPresenter[i].DecimalPlaces = _axisInformation[i].DecimalPlacesOfLabels;
            }
            presenter.SetAxisPresenters(axisPresenter);
            //TODO: Axisview Prefab and show axis should be in DataPresenter, not in vis view, and there defined per axispresenter. Untill done, values of first axis information are used
            _barChartMesh = u2visGeneralController.Instance.Default2DBarChartMesh;
            _2DBarThickness = u2visGeneralController.Instance.Default2DBarThickness;
        }
        parallelCoordinates.Initialize(presenter, _axisInformation[0].AxisPrefab, _style);
        parallelCoordinates.ShowAxes = _axisInformation[0].ShowAxis;
        parallelCoordinates.Size = _visSize;
        parallelCoordinates.Rebuild();

        DataPresenter = presenter;
        VisualizationView = parallelCoordinates;
        initilized = true;
    }

    private void InitilizeDefaultMultiDim(bool withDefaults)
    {
        MultiDimDataPresenter presenter = gameObject.GetComponent<MultiDimDataPresenter>();
        BaseVisualizationView multiDimVis;
        switch (_visType)
        {
            case VisType.HeightMap:
                multiDimVis = gameObject.GetComponent<Heightmap>();
                break;
            case VisType.LineChart2D:
                multiDimVis = gameObject.GetComponent<LineChart2D>();
                break;
            case VisType.LineChart3D:
                multiDimVis = gameObject.GetComponent<LineChart3D>();
                _visSize = new Vector3(_visSize.x, _visSize.y, 0.2f); //TODO: Here the size does not scale the axis problem comes back. ave to fix that later.
                break;
            case VisType.PieChart2D:
                multiDimVis = gameObject.GetComponent<PieChart2D>();
                break;
            case VisType.PieChart3D:
                multiDimVis = gameObject.GetComponent<PieChart3D>();
                break;
            default:
                Debug.LogError("this vis type does not use a default multi dim data presenter. Default to 2dLinechart");
                multiDimVis = gameObject.GetComponent<LineChart2D>();
                break;
        }

        if (!withDefaults)
        {
            if (_axisInformation.Length != 3)
            {
                Debug.LogError($"Wrong amount of AxisInformation. {_axisInformation.Length} are given but needed are 2.");
                throw new Exception("Wrong amount of AxisInformation.");
            }
            List<int> dimIndices = new List<int>();
            dimIndices.Add(_axisInformation[1].DimensionIndex);
            dimIndices.Add(_axisInformation[2].DimensionIndex);
            dimIndices.AddRange(_indicesOfMultiDimensionDataDimensions);
            presenter.Initialize(_dataProvider, _selectedMinItem, _selectedMaxItem, _axisInformation[0].DimensionIndex, dimIndices.ToArray());
            presenter.ResetAxisProperties();
            AxisPresenter[] axisPresenter = presenter.AxisPresenters;
            for (int i = 0; i < axisPresenter.Length; i++)
            {
                axisPresenter[i].IsCategorical = _axisInformation[i].IsCategorigal;
                axisPresenter[i].LabelOrientation = _axisInformation[i].LabelOrientation;
                axisPresenter[i].LabelTickIntervall = _axisInformation[i].LabelInterval;
                axisPresenter[i].TickIntervall = 1f / (float)_axisInformation[i].NumberOfTicks;
                axisPresenter[i].DecimalPlaces = _axisInformation[i].DecimalPlacesOfLabels;
            }
            presenter.SetAxisPresenters(axisPresenter);
        }
        else
        {

            _axisInformation = new AxisInformationStruct[3];
            for (int i = 0; i < _axisInformation.Length; i++)
            {
                _axisInformation[i] = new AxisInformationStruct(
                    u2visGeneralController.Instance.DefaultDimensionIndices[i],
                    u2visGeneralController.Instance.DefaultCategoricalFlag,
                    u2visGeneralController.Instance.DefaultShowAxisFlag,
                    u2visGeneralController.Instance.DefaultAxisPrefab,
                    u2visGeneralController.Instance.DefaultNumberOfTicks,
                    u2visGeneralController.Instance.DefaultLabelIntervall,
                    u2visGeneralController.Instance.DefaultLabelOrientation,
                    u2visGeneralController.Instance.DefaultLabelDecimalPlaces);
            }

            List<int> dimIndices = new List<int>();
            dimIndices.Add(_axisInformation[1].DimensionIndex);
            dimIndices.Add(_axisInformation[2].DimensionIndex);
            dimIndices.AddRange(_indicesOfMultiDimensionDataDimensions);
            presenter.Initialize(_dataProvider, _selectedMinItem, _selectedMaxItem, _axisInformation[0].DimensionIndex, dimIndices.ToArray());

            AxisPresenter[] axisPresenter = presenter.AxisPresenters;
            presenter.ResetAxisProperties();

            for (int i = 0; i < axisPresenter.Length; i++)
            {
                axisPresenter[i].IsCategorical = _axisInformation[i].IsCategorigal;
                axisPresenter[i].LabelOrientation = _axisInformation[i].LabelOrientation;
                axisPresenter[i].LabelTickIntervall = _axisInformation[i].LabelInterval;
                axisPresenter[i].TickIntervall = 1f / (float)_axisInformation[i].NumberOfTicks;
                axisPresenter[i].DecimalPlaces = _axisInformation[i].DecimalPlacesOfLabels;
            }
            presenter.SetAxisPresenters(axisPresenter);
        }
        multiDimVis.Initialize(presenter, _axisInformation[0].AxisPrefab, _style);
        multiDimVis.ShowAxes = _axisInformation[0].ShowAxis;
        if (_visType == VisType.PieChart3D || _visType == VisType.PieChart2D)
            multiDimVis.ShowAxes = false;
        multiDimVis.Size = _visSize;
        multiDimVis.Rebuild();

        DataPresenter = presenter;
        VisualizationView = multiDimVis;
        initilized = true;
    }

    private void InitilizeBarChart3D(bool withDefaults)
    {
        MultiDimDataPresenter presenter = gameObject.GetComponent<MultiDimDataPresenter>();
        BarChart3D barChart = gameObject.GetComponent<BarChart3D>();
        if (!withDefaults)
        {
            if (_axisInformation.Length != 3)
            {
                Debug.LogError($"Wrong amount of AxisInformation. {_axisInformation.Length} are given but needed are 3.");
                throw new Exception("Wrong amount of AxisInformation.");
            }
            List<int> dimIndices = new List<int>();
            dimIndices.Add(_axisInformation[1].DimensionIndex);
            dimIndices.Add(_axisInformation[2].DimensionIndex);
            dimIndices.AddRange(_indicesOfMultiDimensionDataDimensions);
            presenter.Initialize(_dataProvider, _selectedMinItem, _selectedMaxItem, _axisInformation[0].DimensionIndex,dimIndices.ToArray());
            presenter.ResetAxisProperties();
            AxisPresenter[] axisPresenter = presenter.AxisPresenters;
            for (int i = 0; i < axisPresenter.Length; i++)
            {
                axisPresenter[i].IsCategorical = _axisInformation[i].IsCategorigal;
                axisPresenter[i].LabelOrientation = _axisInformation[i].LabelOrientation;
                axisPresenter[i].LabelTickIntervall = _axisInformation[i].LabelInterval;
                axisPresenter[i].TickIntervall = 1f / (float)_axisInformation[i].NumberOfTicks;
                axisPresenter[i].DecimalPlaces = _axisInformation[i].DecimalPlacesOfLabels;
            }
            presenter.SetAxisPresenters(axisPresenter);
            //TODO: Axisview Prefab and show axis should be in DataPresenter, not in vis view, and there defined per axispresenter. Untill done, values of first axis information are used
            if (_barChartMesh == null)
            {
                _barChartMesh = u2visGeneralController.Instance.Default3DBarChartMesh;
            }
            if (_3DBarThickness == null)
            {
                _3DBarThickness = u2visGeneralController.Instance.Default3DBarThickness;
            }
        }
        else
        {
            List<int> dimIndices = new List<int>();
            dimIndices.Add(_axisInformation[1].DimensionIndex);
            dimIndices.Add(_axisInformation[2].DimensionIndex);
            dimIndices.AddRange(_indicesOfMultiDimensionDataDimensions);
            presenter.Initialize(_dataProvider, _selectedMinItem, _selectedMaxItem, _axisInformation[0].DimensionIndex, dimIndices.ToArray());
            presenter.ResetAxisProperties();
            _axisInformation = new AxisInformationStruct[3];
            for (int i = 0; i < _axisInformation.Length; i++)
            {
                _axisInformation[i] = new AxisInformationStruct(
                    u2visGeneralController.Instance.DefaultDimensionIndices[i],
                    u2visGeneralController.Instance.DefaultCategoricalFlag,
                    u2visGeneralController.Instance.DefaultShowAxisFlag,
                    u2visGeneralController.Instance.DefaultAxisPrefab,
                    u2visGeneralController.Instance.DefaultNumberOfTicks,
                    u2visGeneralController.Instance.DefaultLabelIntervall,
                    u2visGeneralController.Instance.DefaultLabelOrientation,
                    u2visGeneralController.Instance.DefaultLabelDecimalPlaces);
            }
            AxisPresenter[] axisPresenter = presenter.AxisPresenters;
            Debug.Log(axisPresenter.Length);
            for (int i = 0; i < axisPresenter.Length; i++)
            {
                axisPresenter[i].IsCategorical = _axisInformation[i].IsCategorigal;
                axisPresenter[i].LabelOrientation = _axisInformation[i].LabelOrientation;
                axisPresenter[i].LabelTickIntervall = _axisInformation[i].LabelInterval;
                axisPresenter[i].TickIntervall = 1f / (float)_axisInformation[i].NumberOfTicks;
                axisPresenter[i].DecimalPlaces = _axisInformation[i].DecimalPlacesOfLabels;
            }
            presenter.SetAxisPresenters(axisPresenter);
            //TODO: Axisview Prefab and show axis should be in DataPresenter, not in vis view, and there defined per axispresenter. Untill done, values of first axis information are used
            _barChartMesh = u2visGeneralController.Instance.Default3DBarChartMesh;
            _3DBarThickness = u2visGeneralController.Instance.Default3DBarThickness;
        }
        barChart.Initialize(presenter, _axisInformation[0].AxisPrefab, _style, _barChartMesh);
        barChart.BarThickness = (Vector2)_3DBarThickness;
        barChart.ShowAxes = _axisInformation[0].ShowAxis;
        barChart.Size = _visSize;
        barChart.Rebuild();

        DataPresenter = presenter;
        VisualizationView = barChart;
        initilized = true;
    }
    
    private void InitilizeBarChart2D(bool withDefaults)
    {
        GenericDataPresenter presenter = gameObject.GetComponent<GenericDataPresenter>();
        BarChart2D barChart = gameObject.GetComponent<BarChart2D>();
        if(presenter is MultiDimDataPresenter)
        {
            Debug.LogError($"this vis uses a Generic Data Presenter instead of {presenter.GetType().Name}");
            throw new Exception($"this vis uses a Generic Data Presenter instead of {presenter.GetType().Name}");
        }
        if (!withDefaults)
        {
            if (_axisInformation.Length != 2)
            {
                Debug.LogError($"Wrong amount of AxisInformation. {_axisInformation.Length} are given but needed are 2.");
                throw new Exception("Wrong amount of AxisInformation.");
            }
            List<int> dimIndices = new List<int>();
            foreach (var information in _axisInformation)
            {
                dimIndices.Add(information.DimensionIndex);
            }
            presenter.Initialize(_dataProvider, _selectedMinItem, _selectedMaxItem, dimIndices.ToArray());
            presenter.ResetAxisProperties();
            AxisPresenter[] axisPresenter = presenter.AxisPresenters;
            for (int i = 0; i < axisPresenter.Length; i++)
            {
                axisPresenter[i].IsCategorical = _axisInformation[i].IsCategorigal;
                axisPresenter[i].LabelOrientation = _axisInformation[i].LabelOrientation;
                axisPresenter[i].LabelTickIntervall = _axisInformation[i].LabelInterval;
                axisPresenter[i].TickIntervall = 1f / (float)_axisInformation[i].NumberOfTicks;
                axisPresenter[i].DecimalPlaces = _axisInformation[i].DecimalPlacesOfLabels;
            }
            presenter.SetAxisPresenters(axisPresenter);
            //TODO: Axisview Prefab and show axis should be in DataPresenter, not in vis view, and there defined per axispresenter. Untill done, values of first axis information are used
            if (_barChartMesh == null)
            {
                _barChartMesh = u2visGeneralController.Instance.Default2DBarChartMesh;
            }
            if (_2DBarThickness == null)
            {
                _2DBarThickness = u2visGeneralController.Instance.Default2DBarThickness;
            }
        }
        else
        {
            List<int> dimIndices = new List<int>();
            dimIndices.Add(u2visGeneralController.Instance.DefaultDimensionIndices[0]);
            dimIndices.Add(u2visGeneralController.Instance.DefaultDimensionIndices[1]);
            Debug.Log(_dataProvider.gameObject.name);
            presenter.Initialize(_dataProvider, _selectedMinItem, _selectedMaxItem, dimIndices.ToArray());
            presenter.ResetAxisProperties();
            _axisInformation = new AxisInformationStruct[2];
            for (int i = 0; i < _axisInformation.Length; i++)
            {
                _axisInformation[i] = new AxisInformationStruct(
                    u2visGeneralController.Instance.DefaultDimensionIndices[i],
                    u2visGeneralController.Instance.DefaultCategoricalFlag,
                    u2visGeneralController.Instance.DefaultShowAxisFlag,
                    u2visGeneralController.Instance.DefaultAxisPrefab,
                    u2visGeneralController.Instance.DefaultNumberOfTicks,
                    u2visGeneralController.Instance.DefaultLabelIntervall,
                    u2visGeneralController.Instance.DefaultLabelOrientation,
                    u2visGeneralController.Instance.DefaultLabelDecimalPlaces);
            }
            AxisPresenter[] axisPresenter = presenter.AxisPresenters;
            for (int i = 0; i < axisPresenter.Length; i++)
            {
                axisPresenter[i].IsCategorical = _axisInformation[i].IsCategorigal;
                axisPresenter[i].LabelOrientation = _axisInformation[i].LabelOrientation;
                axisPresenter[i].LabelTickIntervall = _axisInformation[i].LabelInterval;
                axisPresenter[i].TickIntervall = 1f / (float)_axisInformation[i].NumberOfTicks;
                axisPresenter[i].DecimalPlaces = _axisInformation[i].DecimalPlacesOfLabels;
            }
            presenter.SetAxisPresenters(axisPresenter);
            //TODO: Axisview Prefab and show axis should be in DataPresenter, not in vis view, and there defined per axispresenter. Untill done, values of first axis information are used
            _barChartMesh = u2visGeneralController.Instance.Default2DBarChartMesh;
            _2DBarThickness = u2visGeneralController.Instance.Default2DBarThickness;
        }
        barChart.Initialize(presenter, _axisInformation[0].AxisPrefab, _style,_barChartMesh);
        barChart.BarThickness = (float)_2DBarThickness;
        barChart.ShowAxes = _axisInformation[0].ShowAxis;
        barChart.Size = _visSize;
        barChart.Rebuild();

        DataPresenter = presenter;
        VisualizationView = barChart;
        initilized = true;
    }
    #endregion

    #region setters for vis specific values
    public void SetScatterplotValues(Vector3 minZoomLevel, Vector3 maxZoomLevel, bool displayRelativeValues)
    {
        if (_visType == VisType.Scatterplot)
        {
            _displayRelativeValues = displayRelativeValues;
            _minZoomLevel = minZoomLevel;
            _maxZoomLevel = maxZoomLevel;
            if (initilized)
            {
                UpdateContentMeshes();
            }
            else
            {
                Debug.Log("You still need to initilize the vis");
            }
        }
        else
        {
            Debug.LogError("This is not a Scatterplot");
        }
    }
    
    /// <summary>
    /// Sets the BarChart2D specific values.
    /// </summary>
    /// <param name="barThickness"></param>
    /// <param name="barChart2DMesh"></param>
    public void SetBarChart2DValues(float barThickness, Mesh barChart2DMesh)
    {
        if (_visType == VisType.BarChart2D)
        {
            _2DBarThickness = barThickness;
            _barChartMesh = barChart2DMesh;
            if (initilized)
                UpdateContentMeshes();
            else
                Debug.Log("You still need to initilize the vis");
        }
        else
        {
            Debug.LogError("this is not a 2D Barchart");
            return;
        }
    }

    /// <summary>
    /// Sets the BarChar3D specific values.
    /// </summary>
    /// <param name="barThickness"></param>
    /// <param name="barChart3DMesh"></param>
    public void SetBarChart3DValues(Vector2 barThickness, Mesh barChart3DMesh)
    {
        if (_visType == VisType.BarChart3D)
        {
            _3DBarThickness = barThickness;
            _barChartMesh = barChart3DMesh;
            if (initilized)
                UpdateContentMeshes();
            else
                Debug.Log("You still need to initilize the vis");
        }
        else
        {
            Debug.LogError("this is not a 3D Barchart");
            return;
        }
    }
    #endregion

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
    public void SetAxisValuesByAxisIndex(int axisIndex, int? dimensionIndexToBe = null, bool? isCategorical =null, bool? showAxis = null, GenericAxisView axisPrefab=null,int? numberOfTicks=null,int? labelInterval=null, LabelOrientation? labelOrientation=null,int? decimalPlacesOfLabels=null)
    {
        if (axisIndex >= _axisInformation.Length - 1)
        {
            Debug.LogError("Axis Index Does Not Exist!)");
            return;
        }
        AxisInformationStruct oldInfo = _axisInformation[axisIndex];
        int newAxisDim = dimensionIndexToBe != null ? (int)dimensionIndexToBe : oldInfo.DimensionIndex;
        int newAxisDim2 = dimensionIndexToBe ?? oldInfo.DimensionIndex;
        bool newCategorical = isCategorical != null ? (bool)isCategorical : oldInfo.IsCategorigal;
        bool newShowAxis = showAxis != null ? (bool)showAxis : oldInfo.ShowAxis;
        GenericAxisView newAxisPrefab = axisPrefab != null ? axisPrefab : oldInfo.AxisPrefab;
        int newNumberOfTicks = numberOfTicks != null ? (int)numberOfTicks : oldInfo.NumberOfTicks;
        int newLabelInterval = labelInterval != null ? (int)labelInterval : oldInfo.LabelInterval;
        LabelOrientation newLabelOrientation = labelOrientation != null ? (LabelOrientation)labelOrientation : oldInfo.LabelOrientation;
        int newDecimalPlaces = decimalPlacesOfLabels != null ? (int)decimalPlacesOfLabels : oldInfo.DecimalPlacesOfLabels;

        AxisInformation[axisIndex] = new AxisInformationStruct(
            dimensionIndex: newAxisDim, 
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

    #region utils
    /// <summary>
    /// calls the appropriate TypeInitilizer deoending on <see cref="_visType"/>
    /// </summary>
    /// <param name="initWithDefaults">flag if default values should be used.</param>
    private void InitilizeSwitcher(bool initWithDefaults)
    {
        switch (_visType)
        {
            case VisType.BarChart2D:
                InitilizeBarChart2D(initWithDefaults);
                break;
            case VisType.BarChart3D:
                if (initWithDefaults)
                    _indicesOfMultiDimensionDataDimensions = u2visGeneralController.Instance.DefaultMultiDimIndices;
                InitilizeBarChart3D(initWithDefaults);
                break;
            case VisType.HeightMap:
                if (initWithDefaults)
                    _indicesOfMultiDimensionDataDimensions = u2visGeneralController.Instance.DefaultMultiDimIndices;
                InitilizeDefaultMultiDim(initWithDefaults);
                break;
            case VisType.LineChart2D:
                if (initWithDefaults)
                    _indicesOfMultiDimensionDataDimensions = u2visGeneralController.Instance.DefaultMultiDimIndices;
                InitilizeDefaultMultiDim(initWithDefaults);
                break;
            case VisType.LineChart3D:
                if (initWithDefaults)
                    _indicesOfMultiDimensionDataDimensions = u2visGeneralController.Instance.DefaultMultiDimIndices;
                InitilizeDefaultMultiDim(initWithDefaults);
                break;
            case VisType.ParallelCoordinates:
                InitilizeParallelCoordinates(initWithDefaults);
                break;
            case VisType.PieChart2D:
                if (initWithDefaults)
                    _indicesOfMultiDimensionDataDimensions = u2visGeneralController.Instance.DefaultMultiDimIndices;
                InitilizeDefaultMultiDim(initWithDefaults);
                break;
            case VisType.PieChart3D:
                if (initWithDefaults)
                    _indicesOfMultiDimensionDataDimensions = u2visGeneralController.Instance.DefaultMultiDimIndices;
                InitilizeDefaultMultiDim(initWithDefaults);
                break;
            case VisType.Scatterplot:
                InitilizeScatterplot(initWithDefaults);
                break;
        }
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
        InitilizeSwitcher(false);
    }

    /// <summary>
    /// Only updates the axes of a visualization. 
    /// Not yet implemented. Rebuilds all.
    /// </summary>
    public void UpdateAxes()
    {
        UpdateCompleteVis();
        throw new NotImplementedException();

        if (!initilized)
        {
            Debug.LogError("This visualization is not initialized! Initialize it first.");
            throw new Exception("vis not initialized!");
        }
        //@TODO update functionality based on contents of private fields
    }

    /// <summary>
    /// Only updates the Content Mesh of the visualization.
    /// Not yet implemented. Rebuilds all.
    /// </summary>
    public void UpdateContentMeshes()
    {
        UpdateCompleteVis();
        throw new NotImplementedException();

        if (!initilized)
        {
            Debug.LogError("This visualization is not initialized! Initialize it first.");
            throw new Exception("vis not initialized!");
        }
        //@TODO update functionality based on contents of private fields
    }
    
    #endregion

    #region update meshes by type
    //here the meshes will be updated by their type.
    private void UpdateMeshStackedBar(bool withDefaults)
    {
        throw new NotImplementedException();
    }

    private void UpdateMeshScatterplot(bool withDefaults)
    {
        throw new NotImplementedException();
    }

    private void UpdateMeshRevolvedCharts(bool withDefaults)
    {
        throw new NotImplementedException();
    }

    private void UpdateMeshPieChart3D(bool withDefaults)
    {
        throw new NotImplementedException();
    }

    private void UpdateMeshPieChart2D(bool withDefaults)
    {
        throw new NotImplementedException();
    }

    private void UpdateMeshParallelCoordinates(bool withDefaults)
    {
        throw new NotImplementedException();
    }

    private void UpdateMeshLineChart3D(bool withDefaults)
    {
        throw new NotImplementedException();
    }

    private void UpdateMeshLineChart2D(bool withDefaults)
    {
        throw new NotImplementedException();
    }

    private void UpdateMeshHeightMap(bool withDefaults)
    {
        throw new NotImplementedException();
    }

    private void UpdateMeshBarChart3D(bool withDefaults)
    {
        throw new NotImplementedException();
    }

    private void UpdateMeshBarChart2D(bool withDefaults)
    {
        throw new NotImplementedException();
    }
    #endregion

}