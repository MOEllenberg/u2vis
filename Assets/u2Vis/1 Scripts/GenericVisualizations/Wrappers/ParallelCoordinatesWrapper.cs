using System;
using System.Collections;
using System.Collections.Generic;
using u2vis;
using UnityEngine;
using static u2visGeneralController;

public class ParallelCoordinatesWrapper : GeneralVisulizationWrapper
{
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

    protected override void InitilizeVisSpecific(bool withDefaults)
    {
        _visType = VisType.ParallelCoordinates;
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
        }
        parallelCoordinates.Initialize(presenter, _axisInformation[0].AxisPrefab, _style);
        parallelCoordinates.ShowAxes = _axisInformation[0].ShowAxis;
        parallelCoordinates.Size = _visSize;
        parallelCoordinates.Rebuild();

        DataPresenter = presenter;
        VisualizationView = parallelCoordinates;
        initilized = true;
    }
}
