using System;
using System.Collections;
using System.Collections.Generic;
using u2vis;
using UnityEngine;
using static u2visGeneralController;

public class ScatterplotWrapper : GeneralVisulizationWrapper
{
    [SerializeField]
    private Vector3? _minZoomLevel = null;

    [SerializeField]
    private Vector3? _maxZoomLevel = null;

    [SerializeField]
    private bool? _displayRelativeValues = null;
    public override void UpdateAxes()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateContentMeshes()
    {
        throw new System.NotImplementedException();
    }

    protected override void InitilizeVisSpecific(bool withDefaults)
    {
        _visType = VisType.Scatterplot;
        GenericDataPresenter presenter = gameObject.GetComponent<GenericDataPresenter>();
        Scatterplot2D scatterplot = gameObject.GetComponent<Scatterplot2D>();
        if (presenter is MultiDimDataPresenter)
        {
            Debug.LogError($"this vis uses a Generic Data Presenter instead of {presenter.GetType().Name}");
            throw new Exception($"this vis uses a Generic Data Presenter instead of {presenter.GetType().Name}");
        }
        if (!withDefaults)
        {
            if (!(_axisInformation.Length == 2 || _axisInformation.Length == 3))
            {
                Debug.LogError($"Wrong amount of AxisInformation. {_axisInformation.Length} are given but needed are 2 or 3.");
                throw new Exception("Wrong amount of AxisInformation.");
            }

            presenter.Initialize(_dataProvider, _selectedMinItem, _selectedMaxItem, _dimIndices);
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
            presenter.Initialize(_dataProvider, _selectedMinItem, _selectedMaxItem, _dimIndices);
            presenter.ResetAxisProperties();
            _axisInformation = new AxisInformationStruct[2];
            for (int i = 0; i < _axisInformation.Length; i++)
            {
                _axisInformation[i] = new AxisInformationStruct(
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
        gameObject.name = $"Scatterplot by: {CreatorName} | ID: {VisID}";
        initilized = true;
    }
    public void SetScatterplotValues(Vector3 minZoomLevel, Vector3 maxZoomLevel, bool displayRelativeValues)
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

    public override GeneralVisulizationWrapper Generate(AbstractDataProvider dataProvider, int[] dimIndices, Transform parent, string name)
    {
        GenericDataPresenter dataPresenter = gameObject.AddComponent<GenericDataPresenter>();
        Scatterplot2D scatterplot = gameObject.AddComponent<Scatterplot2D>();
        scatterplot.BindPresenterBeforeInit(dataPresenter);
        DataPresenter = dataPresenter;
        GetComponent<MeshRenderer>().material = u2visGeneralController.Instance.DefaultScatterplotMaterial;
        Create(dataProvider, dimIndices, name);
        return this;
    }
}
