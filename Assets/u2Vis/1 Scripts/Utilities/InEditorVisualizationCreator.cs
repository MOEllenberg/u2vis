using System.Collections;
using System.Collections.Generic;
using u2vis;
using UnityEngine;

public class InEditorVisualizationCreator : MonoBehaviour
{
    [SerializeField]
    private u2visGeneralController.VisType _visualizationType;
    [SerializeField]
    private Transform _parent;
    [SerializeField]
    private AbstractDataProvider _dataProvider;
    [SerializeField]
    private List<int> _dimensionIndexes;
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
    /// The default dimension indices
    /// </summary>
    [SerializeField]
    private List<int> _dimensionIndices;
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
    /// The default Indices for multidimenional presenters
    /// </summary>
    [SerializeField]
    private int[] _multiDimIndices;
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

    private u2visGeneralController _controller;
    public void CreateVisualization()
    {
        _controller = u2visGeneralController.Instance;
        if (_createWithDefaults)
        {
            switch (_visualizationType)
            {
                case u2visGeneralController.VisType.BarChart2D:
                    _controller.CreateVis<BarChart2DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parent, _creatorName);
                    break;
                case u2visGeneralController.VisType.BarChart3D:
                    _controller.CreateVis<BarChart3DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parent, _creatorName);
                    break;
                case u2visGeneralController.VisType.HeightMap:
                    _controller.CreateVis<HeightMapWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parent, _creatorName);
                    break;
                case u2visGeneralController.VisType.LineChart2D:
                    _controller.CreateVis<LineChart2DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parent, _creatorName);
                    break;
                case u2visGeneralController.VisType.LineChart3D:
                    _controller.CreateVis<LineChart3DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parent, _creatorName);
                    break;
                case u2visGeneralController.VisType.ParallelCoordinates:
                    _controller.CreateVis<ParallelCoordinatesWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parent, _creatorName);
                    break;
                case u2visGeneralController.VisType.PieChart2D:
                    _controller.CreateVis<PieChart2DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parent, _creatorName);
                    break;
                case u2visGeneralController.VisType.PieChart3D:
                    _controller.CreateVis<PieChart3DWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parent, _creatorName);
                    break;
                case u2visGeneralController.VisType.Scatterplot:
                    _controller.CreateVis<ScatterplotWrapper>(_dataProvider, _dimensionIndexes.ToArray(), _parent, _creatorName);
                    break;
            }
        }
    }

}
