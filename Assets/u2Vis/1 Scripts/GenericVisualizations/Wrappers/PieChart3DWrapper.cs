using System;
using System.Collections;
using System.Collections.Generic;
using u2vis;
using UnityEngine;
using static u2visGeneralController;

public class PieChart3DWrapper : GeneralVisulizationWrapper
{
    public override void UpdateAxes()
    {
        throw new NotImplementedException();
    }

    public override void UpdateContentMeshes()
    {
        throw new NotImplementedException();
    }

    protected override void InitilizeVisSpecific(bool withDefaults)
    {
        _visType = VisType.PieChart3D;
        MultiDimDataPresenter presenter = gameObject.GetComponent<MultiDimDataPresenter>();
        PieChart3D pieChart3D = gameObject.GetComponent<PieChart3D>();

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
        pieChart3D.Initialize(presenter, _axisInformation[0].AxisPrefab, _style);
        pieChart3D.ShowAxes = _axisInformation[0].ShowAxis;
        if (_visType == VisType.PieChart3D || _visType == VisType.PieChart2D)
            pieChart3D.ShowAxes = false;
        pieChart3D.Size = _visSize;
        pieChart3D.Rebuild();

        DataPresenter = presenter;
        VisualizationView = pieChart3D;
        initilized = true;
    }
}
