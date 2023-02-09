using System;
using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using Notifications;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Examples;

public class NormcoreSyncStepComplete : RealtimeComponent<NormcoreProgressModel>
{

    [SerializeField] private NotificationTypeScriptableObject _networkCompleteNotification;
    [SerializeField] private NotificationTypeScriptableObject _localCompleteNotification;


    private void OnEnable()
    {
        EventBus.Register<CustomEventArgs>(NotificationEventUnit.EventBusHook, args =>
        {
            if (args.name == _localCompleteNotification.NotificationUniqueID)
            {
                var cs = (int) Variables.ActiveScene.Get("CurrentStep");
                model.currentStep = cs + 1;
            }
        });
    }
    
    

    protected override void OnRealtimeModelReplaced(NormcoreProgressModel previousModel, NormcoreProgressModel currentModel) {
        if (previousModel != null) {
            // Unregister from events
            previousModel.currentStepDidChange -= OncurrentStepDidChange;
        }
        
        if (currentModel != null) {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
                currentModel.currentStep = (int) Variables.ActiveScene.Get("CurrentStep");
        
            // Update the mesh render to match the new model
            UpdateProgress();

            // Register for events so we'll know if the color changes later
            currentModel.currentStepDidChange += OncurrentStepDidChange;
        }
    }

    private void UpdateProgress()
    {
        if(model != null)
            EventBus.Trigger( NotificationEventUnit.EventBusHook, new CustomEventArgs(_networkCompleteNotification.NotificationUniqueID, model.currentStep));
        //Variables.ActiveScene.Set("CurrentStep", model.currentStep);
    }

    private void OncurrentStepDidChange(NormcoreProgressModel normcoreProgressModel, int value)
    {
        UpdateProgress();
    }
}
