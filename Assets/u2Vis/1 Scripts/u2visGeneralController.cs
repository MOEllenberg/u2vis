using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using u2vis;
using UnityEngine;

public class u2visGeneralController : MonoBehaviour
{
    #region Pseudo-Singleton
    public static u2visGeneralController Instance { get; private set; }
  

    public u2visGeneralController()
    {
        Instance = this;
    }
    #endregion

    //idhandling. Should be put elsewhere later
    private int nextID = 0;
    public int NextID { get { nextID++; return nextID; } }

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
        //RevolvedCharts,
        Scatterplot,
        //StackedBar
    }

    /// <summary>
    /// The default Data Provider.
    /// </summary>
    [SerializeField]
    private AbstractDataProvider _defaultDataProvider;
    /// <summary>
    /// The default is categorical flag.
    /// </summary>
    [SerializeField]
    private bool _defaultCategoricalFlag;
    /// <summary>
    /// The default Label Orientation.
    /// </summary>
    [SerializeField]
    private LabelOrientation _defaultLabelOrientation;
    /// <summary>
    /// The default Number of Ticks.
    /// </summary>
    [SerializeField]
    private int _defaultNumberOfTicks;
    /// <summary>
    /// The default Label Interval
    /// </summary>
    [SerializeField]
    private int _defaultLabelInterval;
    /// <summary>
    /// The default number of decimal places for the labels
    /// </summary>
    [SerializeField]
    private int _defaultLabelDecimalPlaces;
    /// <summary>
    /// The default Axis Prefab
    /// </summary>
    [SerializeField]
    private GenericAxisView _defaultAxisPrefab;
    /// <summary>
    /// The default Show Axis Flag
    /// </summary>
    [SerializeField]
    private bool _defaultShowAxisFlag;
    /// <summary>
    /// The default Size
    /// </summary>
    //TODO: Needs rework, because u2vis does not scale Axes with size, only the mesh
    [SerializeField]
    private Vector3 _defaultSize = new Vector3(0.9f, 0.9f, 0.9f);
    /// <summary>
    /// The default dimension indices
    /// </summary>
    [SerializeField]
    private List<int> _defaultDimensionIndices;
    /// <summary>
    /// The default min Item
    /// </summary>
    [SerializeField]
    private int _defaultMinItem;
    /// <summary>
    /// The default max Item
    /// </summary>
    [SerializeField]
    private int _defaultMaxItem;
    /// <summary>
    /// The default Indices for multidimenional presenters
    /// </summary>
    [SerializeField]
    private int[] _defaultMultiDimIndices;
    /// <summary>
    /// The default style
    /// </summary>
    [SerializeField]
    private GenericVisualizationStyle _defaultStyle;    
    /// <summary>
    /// The default categorical style
    /// </summary>
    [SerializeField]
    private GenericVisualizationStyle _defaultCategoricalStyle;
    /// <summary>
    /// The default 2D Bar Chart Mesh
    /// </summary>
    [SerializeField]
    private Mesh _default2DBarChartMesh;
    /// <summary>
    /// The default 3d Bar Chart Mesh
    /// </summary>
    [SerializeField]
    private Mesh _default3DBarChartMesh;
    /// <summary>
    /// The default 2D Bar Chart bar thickness
    /// </summary>
    [SerializeField]
    private float _default2DBarThickness = 1f;
    /// <summary>
    /// The default 3D Bar Chart bar thickness
    /// </summary>
    [SerializeField]
    private Vector2 _default3DBarThickness = Vector2.one;

    [SerializeField]
    private Material _defaultAreaMaterial;

    [SerializeField]
    private Material _defaultLineMaterial;

    [SerializeField]
    private Material _defaultScatterplotMaterial;

    [SerializeField]
    private Vector3 _minZoomLevel = Vector3.zero;

    [SerializeField]
    private Vector3 _maxZoomLevel = Vector3.one;

    [SerializeField]
    private bool _displayRelativeValues = true;
    /// <summary>
    /// Dict for all vis wrappers with the id as key.
    /// </summary>
    private Dictionary<string,GeneralVisulizationWrapper> _visulizations = new Dictionary<string, GeneralVisulizationWrapper>();
    /// <summary>
    /// private dict for Groups
    /// </summary>
    private Dictionary<string, List<GeneralVisulizationWrapper>> _groups = new Dictionary<string, List<GeneralVisulizationWrapper>>();
    /// <summary>
    /// The Property for the default Data Provider
    /// </summary>
    public AbstractDataProvider DefaultDataprovider => _defaultDataProvider;
    /// <summary>
    /// The Property for the visualization dictionary. Holds all visualization wrappers with their ids as keys.
    /// </summary>
    public Dictionary<string, GeneralVisulizationWrapper> Visulizations { get => _visulizations; }
    /// <summary>
    /// The property for the Group Dictionary. Holds Lists of wrappers (groups) with the group name as keys.
    /// </summary>
    public Dictionary<string, List<GeneralVisulizationWrapper>> Groups { get => _groups; }
    /// <summary>
    /// The Property for the default 2d Bar Chart Mehs
    /// </summary>
    public Mesh Default2DBarChartMesh { get => _default2DBarChartMesh; set => _default2DBarChartMesh = value; }
    /// <summary>
    /// The Property for the default Categorical Flag
    /// </summary>
    public bool DefaultCategoricalFlag { get => _defaultCategoricalFlag; set => _defaultCategoricalFlag = value; }
    /// <summary>
    /// The Property for the default <see cref="LabelOrientation"/>
    /// </summary>
    public LabelOrientation DefaultLabelOrientation { get => _defaultLabelOrientation; set => _defaultLabelOrientation = value; }
    /// <summary>
    /// The Property for the default number of ticks
    /// </summary>
    public int DefaultNumberOfTicks { get => _defaultNumberOfTicks; set => _defaultNumberOfTicks = value; }
    /// <summary>
    /// The Property for the default Label Interval
    /// </summary>
    public int DefaultLabelIntervall { get => _defaultLabelInterval; set => _defaultLabelInterval = value; }
    /// <summary>
    /// The Property for the default decimal places for a numeric label
    /// </summary>
    public int DefaultLabelDecimalPlaces { get => _defaultLabelDecimalPlaces; set => _defaultLabelDecimalPlaces = value; }
    /// <summary>
    /// The Property for the default Axis Prefab (<see cref="GenericAxisView"/>)
    /// </summary>
    public GenericAxisView DefaultAxisPrefab { get => _defaultAxisPrefab; set => _defaultAxisPrefab = value; }
    /// <summary>
    /// The Property for the default show axis flag
    /// </summary>
    public bool DefaultShowAxisFlag { get => _defaultShowAxisFlag; set => _defaultShowAxisFlag = value; }
    /// <summary>
    /// The Property for the default size
    /// </summary>
    // has to be fixed. u2vis does only scale mesh, not axis
    public Vector3 DefaultSize { get => _defaultSize; set => _defaultSize = value; }
    /// <summary>
    /// The Property for the default indices for the dimensions
    /// </summary>
    public List<int> DefaultDimensionIndices { get => _defaultDimensionIndices; set => _defaultDimensionIndices = value; }
    /// <summary>
    /// The Property for the default min Item
    /// </summary>
    public int DefaultMinItem { get => _defaultMinItem; set => _defaultMinItem = value; }
    /// <summary>
    /// The Property for the default max item
    /// </summary>
    public int DefaultMaxItem { get => _defaultMaxItem; set => _defaultMaxItem = value; }
    /// <summary>
    /// The Property for the default style
    /// </summary>
    public GenericVisualizationStyle DefaultStyle { get => _defaultStyle; set => _defaultStyle = value; } 
    /// <summary>
    /// The Property for the default categorical Style
    /// </summary>
    public GenericVisualizationStyle DefaultCategoricalStyle { get => _defaultCategoricalStyle; set => _defaultCategoricalStyle = value; }
    /// <summary>
    /// The Property for the default 2D Bar Mesh
    /// </summary>
    public Mesh Default3DBarChartMesh { get => _default3DBarChartMesh; set => _default3DBarChartMesh = value; }
    /// <summary>
    /// The Property for the default 2D Bar Thickness
    /// </summary>
    public float Default2DBarThickness { get => _default2DBarThickness; set => _default2DBarThickness = value; }
    /// <summary>
    /// The Property for the default 3D Bar Thickness
    /// </summary>
    public Vector2 Default3DBarThickness { get => _default3DBarThickness; set => _default3DBarThickness = value; }
    /// <summary>
    /// The Property for the default  indices for multiple dimensions for <see cref="MultiDimDataPresenter"/>
    /// </summary>
    public int[] DefaultMultiDimIndices { get => _defaultMultiDimIndices; set => _defaultMultiDimIndices = value; }
    public Vector2 Default3DBarThickness1 { get => _default3DBarThickness; set => _default3DBarThickness = value; }
    public Vector3 MinZoomLevel { get => _minZoomLevel; set => _minZoomLevel = value; }
    public Vector3 MaxZoomLevel { get => _maxZoomLevel; set => _maxZoomLevel = value; }
    public bool DisplayRelativeValues { get => _displayRelativeValues; set => _displayRelativeValues = value; }
    public Material DefaultAreaMaterial { get => _defaultAreaMaterial; set => _defaultAreaMaterial = value; }
    public Material DefaultLineMaterial { get => _defaultLineMaterial; set => _defaultLineMaterial = value; }
    public Material DefaultScatterplotMaterial { get => _defaultScatterplotMaterial; set => _defaultScatterplotMaterial = value; }

    /// <summary>
    /// Adds a viswrapper to the dict.
    /// </summary>
    /// <param name="wrapper">The wrapper of the visualization to add</param>
    public void AddVis(GeneralVisulizationWrapper wrapper)
    {
        _visulizations.Add(wrapper.VisID, wrapper);
    }


    /// <summary>
    /// removes vis wrapper from dict by id.
    /// </summary>
    /// <param name="id"></param>
    public void RemoveVisById(string id)
    {
        _visulizations.Remove(id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"> This has to be of type GeneralVisualizationWrapper and describes the type of wrapper you want to use and thereby the type of the visualization.</typeparam>
    /// <param name="dataProvider">The dataprovider which should be used for the visualization.</param>
    /// <param name="dimIndices">The dimension indices used for the visualization</param>
    /// <param name="parent">The parent game object from which the visualization should be a child</param>
    /// <param name="creatorName">The name of the creator</param>
    /// <returns></returns>
    public T CreateVis<T>(AbstractDataProvider dataProvider, int[] dimIndices, Transform parent, string creatorName) where T: GeneralVisulizationWrapper
    {
        GameObject object1 = new GameObject();
        object1.transform.parent = parent;
        T wrapper = object1.AddComponent<T>();
        wrapper.Generate(dataProvider,dimIndices,parent,creatorName);
        return wrapper;
    }
    /// <summary>
    ///Creates a Group of visualizations which will be stored by group name in a dict.
    /// </summary>
    /// <param name="groupName">The name of the group to create</param>
    /// <param name="members">The list of the member wrappers</param>
    /// <returns></returns>
    public List<GeneralVisulizationWrapper> CreateGroup(string groupName, List<GeneralVisulizationWrapper> members)
    {
        if(members.Count>0)
            _groups.Add(groupName, members);
        else
        {
            Debug.Log("Don't make empty groups!");
            return null;
        }
        return _groups[groupName];
    }
    /// <summary>
    /// Adds a number of visualizations to a group.
    /// </summary>
    /// <param name="groupName">The group to expand</param>
    /// <param name="newMembers">The new groupmembers</param>
    /// <returns></returns>
    public List<GeneralVisulizationWrapper> AddVisualizationsToGroup(string groupName, List<GeneralVisulizationWrapper> newMembers)
    {
        if (newMembers.Count > 0)
        {
            if (_groups.ContainsKey(groupName))
            {
                _groups[groupName].AddRange(newMembers);
            }
            else
            {
                Debug.LogError("no group with the given name does exist. Consider creating a new one.");
            }
        }
        else
        {
            Debug.Log("Don't add nothing to a group!");
        }
        return _groups[groupName];
    }
    /// <summary>
    /// Removes Visualizations from a group.
    /// </summary>
    /// <param name="groupName">The name of the group from which the visualizations are to be removed</param>
    /// <param name="membersToRemove">the visualizations to be removed</param>
    /// <returns></returns>
    public List<GeneralVisulizationWrapper> RemoveVisualizationsFromGroup(string groupName, List<GeneralVisulizationWrapper> membersToRemove)
    {
        if (membersToRemove.Count > 0)
        {
            if (_groups.ContainsKey(groupName))
            {
                foreach(var member in membersToRemove)
                {
                    if (_groups[groupName].Contains(member))
                    {
                        _groups[groupName].Remove(member);
                    }
                    else
                    {
                        Debug.LogWarning($"The Wrapper with the ID {member.VisID} was not contained in the group {groupName} where you wanted it to remove from.");
                    }
                }
                if (_groups[groupName].Count == 0) //Want to do it this way?
                {
                    _groups.Remove(groupName);
                    Debug.Log("Group was Empty. Therefore Group got deleted.");
                    return null;
                }
            }
            else
            {
                Debug.LogError("no group with the given name does exist. Can't delete Elements from not existing groups.");
            }
        }
        else
        {
            Debug.Log("Don't remove nothing from a group!");
        }
        
        return _groups[groupName];
    }
    /// <summary>
    /// Deletes a group by its groupname
    /// </summary>
    /// <param name="groupName">the name of the group to be deleted</param>
    public void DeleteGroup(string groupName)
    {
        if (_groups.ContainsKey(groupName))
        {
            _groups.Remove(groupName);
        }
        else
        {
            Debug.LogError("The Group you're trying to remove does not exist!");
        }
    }
}
