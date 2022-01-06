using System;
using System.Collections;
using System.Collections.Generic;
using u2vis;
using UnityEngine;
using static u2visGeneralController;

public class BarChart3DWrapper : GeneralVisulizationWrapper
{
    /// <summary>
    /// The needed BarChartMesh
    /// </summary>
    [SerializeField]
    protected Mesh _barChartMesh = null;
    /// <summary>
    /// The thickness of the 3D Bars of a 3D Bar Chart (as Vector).
    /// </summary>
    [SerializeField]
    protected Vector2? _3DBarThickness = null;
    public override void UpdateAxes()
    {
        UpdateCompleteVis();
        throw new System.NotImplementedException();
    }

    public override void UpdateContentMeshes()
    {
        UpdateCompleteVis();
        throw new System.NotImplementedException();
    }

    protected override void InitilizeVisSpecific(bool withDefaults)
    {
        _visType = VisType.BarChart3D;
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
}
