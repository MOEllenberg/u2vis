using System;
using System.Collections;
using System.Collections.Generic;
using u2vis;
using UnityEngine;
using static u2visGeneralController;

public class BarChart2DWrapper : GeneralVisulizationWrapper
{
    /// <summary>
    /// The needed BarChartMesh
    /// </summary>
    [SerializeField]
    protected Mesh _barChartMesh = null;
    /// <summary>
    /// The Thickness of the 2D Bars of a 2D Bar Chart.
    /// </summary>
    [SerializeField]
    protected float? _2DBarThickness = null;
    protected override void InitilizeVisSpecific(bool withDefaults)
    {
        _visType = VisType.BarChart2D;
        GenericDataPresenter presenter = gameObject.GetComponent<GenericDataPresenter>();
        BarChart2D barChart = gameObject.GetComponent<BarChart2D>();
        if (presenter is MultiDimDataPresenter)
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
        barChart.Initialize(presenter, _axisInformation[0].AxisPrefab, _style, _barChartMesh);
        barChart.BarThickness = (float)_2DBarThickness;
        barChart.ShowAxes = _axisInformation[0].ShowAxis;
        barChart.Size = _visSize;
        barChart.Rebuild();

        DataPresenter = presenter;
        VisualizationView = barChart;
        initilized = true;
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

    public override void UpdateAxes()
    {
        UpdateCompleteVis();
        throw new NotImplementedException();
    }

    public override void UpdateContentMeshes()
    {
        UpdateCompleteVis();
        throw new NotImplementedException();
    }
}
