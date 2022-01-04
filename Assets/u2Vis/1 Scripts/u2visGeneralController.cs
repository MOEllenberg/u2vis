using System.Collections;
using System.Collections.Generic;
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
        RevolvedCharts,
        Scatterplot,
        StackedBar
    }

    [SerializeField]
    private List<GameObject> _defaultVisPrefabs = new List<GameObject>();

    [SerializeField]
    private AbstractDataProvider _defaultDataProvider;

    [SerializeField]
    private bool _defaultCategoricalFlag;

    [SerializeField]
    private LabelOrientation _defaultLabelOrientation;

    [SerializeField]
    private int _defaultNumberOfTicks;

    [SerializeField]
    private int _defaultLabelIntervall;

    [SerializeField]
    private int _defaultLabelDecimalPlaces;

    [SerializeField]
    private GenericAxisView _defaultAxisPrefab;

    [SerializeField]
    private bool _defaultShowAxisFlag;

    [SerializeField]
    private Vector3 _defaultSize;

    [SerializeField]
    private List<int> _defaultDimensionIndices;

    [SerializeField]
    private int _defaultMinItem;

    [SerializeField]
    private int _defaultMaxItem;

    [SerializeField]
    private int[] _defaultMultiDimIndices;

    [SerializeField]
    private GenericVisualizationStyle _defaultStyle;

    [SerializeField]
    private Mesh _default2DBarChartMesh;

    [SerializeField]
    private Mesh _default3DBarChartMesh;

    [SerializeField]
    private float _default2DBarThickness = 1f;

    [SerializeField]
    private Vector2 _default3DBarThickness = Vector2.one;
    /// <summary>
    /// Dict for all vis wrappers with the id as key.
    /// </summary>
    private Dictionary<string,GeneralVisulizationWrapper> _visulizations = new Dictionary<string, GeneralVisulizationWrapper>();

    private Dictionary<string, List<GeneralVisulizationWrapper>> _groups = new Dictionary<string, List<GeneralVisulizationWrapper>>();

    public AbstractDataProvider DefaultDataprovider => _defaultDataProvider;

    public Dictionary<string, GeneralVisulizationWrapper> Visulizations { get => _visulizations; }
    public Dictionary<string, List<GeneralVisulizationWrapper>> Groups { get => _groups; }
    public Mesh Default2DBarChartMesh { get => _default2DBarChartMesh; set => _default2DBarChartMesh = value; }
    public bool DefaultCategoricalFlag { get => _defaultCategoricalFlag; set => _defaultCategoricalFlag = value; }
    public LabelOrientation DefaultLabelOrientation { get => _defaultLabelOrientation; set => _defaultLabelOrientation = value; }
    public int DefaultNumberOfTicks { get => _defaultNumberOfTicks; set => _defaultNumberOfTicks = value; }
    public int DefaultLabelIntervall { get => _defaultLabelIntervall; set => _defaultLabelIntervall = value; }
    public int DefaultLabelDecimalPlaces { get => _defaultLabelDecimalPlaces; set => _defaultLabelDecimalPlaces = value; }
    public GenericAxisView DefaultAxisPrefab { get => _defaultAxisPrefab; set => _defaultAxisPrefab = value; }
    public bool DefaultShowAxisFlag { get => _defaultShowAxisFlag; set => _defaultShowAxisFlag = value; }
    public Vector3 DefaultSize { get => _defaultSize; set => _defaultSize = value; }
    public List<int> DefaultDimensionIndices { get => _defaultDimensionIndices; set => _defaultDimensionIndices = value; }
    public int DefaultMinItem { get => _defaultMinItem; set => _defaultMinItem = value; }
    public int DefaultMaxItem { get => _defaultMaxItem; set => _defaultMaxItem = value; }
    public GenericVisualizationStyle DefaultStyle { get => _defaultStyle; set => _defaultStyle = value; }
    public Mesh Default3DBarChartMesh { get => _default3DBarChartMesh; set => _default3DBarChartMesh = value; }
    public float Default2DBarThickness { get => _default2DBarThickness; set => _default2DBarThickness = value; }
    public Vector2 Default3DBarThickness { get => _default3DBarThickness; set => _default3DBarThickness = value; }
    public int[] DefaultMultiDimIndices { get => _defaultMultiDimIndices; set => _defaultMultiDimIndices = value; }

    /// <summary>
    /// Adds a viswrapper to the dict.
    /// </summary>
    /// <param name="wrapper"></param>
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
    /// Creates an Visualization Object by given type as child of parent. Initialize has to be called on the returned wrapper afterwards. New visualization starts with (0,0,0) as local position.
    /// </summary>
    /// <param name="type">Type of the Visualization</param>
    /// <param name="parent">the designated parent of the visualization</param>
    /// <param name="creatorName">the name of the creator</param>
    /// <returns></returns>
    public GeneralVisulizationWrapper CreateVisByType(VisType type, Transform parent, string creatorName)
    {
        GameObject visObject = null;
        switch (type)
        {
            case VisType.BarChart2D:
                foreach(var prefab in _defaultVisPrefabs)
                {
                    if (prefab.GetComponent<BaseVisualizationView>() is BarChart2D)
                        visObject = GameObject.Instantiate(prefab, parent, false);
                }
                break;
            case VisType.BarChart3D:
                foreach (var prefab in _defaultVisPrefabs)
                {
                    if (prefab.GetComponent<BaseVisualizationView>() is BarChart3D)
                        visObject = GameObject.Instantiate(prefab, parent, false);
                }
                break;
            case VisType.HeightMap:
                foreach (var prefab in _defaultVisPrefabs)
                {
                    if (prefab.GetComponent<BaseVisualizationView>() is Heightmap)
                        visObject = GameObject.Instantiate(prefab, parent, false);
                }
                break;
            case VisType.LineChart2D:
                foreach (var prefab in _defaultVisPrefabs)
                {
                    if (prefab.GetComponent<BaseVisualizationView>() is LineChart2D)
                        visObject = GameObject.Instantiate(prefab, parent, false);
                }
                break;
            case VisType.LineChart3D:
                foreach (var prefab in _defaultVisPrefabs)
                {
                    if (prefab.GetComponent<BaseVisualizationView>() is LineChart3D)
                        visObject = GameObject.Instantiate(prefab, parent, false);
                }
                break;
            case VisType.ParallelCoordinates:
                foreach (var prefab in _defaultVisPrefabs)
                {
                    if (prefab.GetComponent<BaseVisualizationView>() is ParallelCoordinates)
                        visObject = GameObject.Instantiate(prefab, parent, false);
                }
                break;
            case VisType.PieChart2D:
                foreach (var prefab in _defaultVisPrefabs)
                {
                    if (prefab.GetComponent<BaseVisualizationView>() is PieChart2D)
                        visObject = GameObject.Instantiate(prefab, parent, false);
                }
                break;
            case VisType.PieChart3D:
                foreach (var prefab in _defaultVisPrefabs)
                {
                    if (prefab.GetComponent<BaseVisualizationView>() is PieChart3D)
                        visObject = GameObject.Instantiate(prefab, parent, false);
                }
                break;
            case VisType.RevolvedCharts:
                foreach (var prefab in _defaultVisPrefabs)
                {
                    if (prefab.GetComponent<BaseVisualizationView>() is RevolvedCharts)
                        visObject = GameObject.Instantiate(prefab, parent, false);
                }
                break;
            case VisType.Scatterplot:
                foreach (var prefab in _defaultVisPrefabs)
                {
                    if (prefab.GetComponent<BaseVisualizationView>() is Scatterplot2D)
                        visObject = GameObject.Instantiate(prefab, parent, false);
                }
                break;
            case VisType.StackedBar:
                foreach (var prefab in _defaultVisPrefabs)
                {
                    if (prefab.GetComponent<BaseVisualizationView>() is BarChart2D)
                        visObject = GameObject.Instantiate(prefab, parent, false);
                }
                break;
            default:
                visObject = GameObject.Instantiate(_defaultVisPrefabs[0], parent, false);
                break;
        }
        if (visObject == null)
        {
            Debug.LogError("Did not find default prefab for given type");
            throw new System.Exception("Did not find default prefab for given type");
        }
        visObject.transform.localPosition = Vector3.zero;
        GeneralVisulizationWrapper wrapper = visObject.AddComponent<GeneralVisulizationWrapper>();
        wrapper.Create(type,creatorName);
        return wrapper;
    }

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
