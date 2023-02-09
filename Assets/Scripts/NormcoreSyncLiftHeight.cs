using System;
using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Examples;

[RequireComponent(typeof(Variables))]
public class NormcoreSyncLiftHeight : RealtimeComponent<NormcoreLiftHeightModel>
{

    private const string VariableName = "HeightValue";
    
    private Variables _variables;
    
    // Start is called before the first frame update
    void Awake()
    {
        _variables = GetComponent<Variables>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var h = (float)_variables.declarations.Get(VariableName);

        if (Math.Abs(h - model.height) > 0.01f)
            model.height = h;
    }

    private void UpdateHeightValue()
    {
        _variables.declarations.Set(VariableName, model.height);
    }
    
    protected override void OnRealtimeModelReplaced(NormcoreLiftHeightModel previousModel, NormcoreLiftHeightModel currentModel) {
        if (previousModel != null) {
            // Unregister from events
            previousModel.heightDidChange -= HeightDidChange;
        }
        
        if (currentModel != null) {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
                currentModel.height = (float) _variables.declarations.Get(VariableName);
        
            // Update the mesh render to match the new model
            UpdateHeightValue();

            // Register for events so we'll know if the color changes later
            currentModel.heightDidChange += HeightDidChange;
        }
    }

    private void HeightDidChange(NormcoreLiftHeightModel m, float value) {
        // Update the mesh renderer
        UpdateHeightValue();
    }
}
