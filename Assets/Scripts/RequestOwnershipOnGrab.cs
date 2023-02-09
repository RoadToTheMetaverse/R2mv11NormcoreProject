using System;
using System.Collections;
using System.Collections.Generic;
using Flow.Framework.Events.XRIT;
using Normal.Realtime;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable), typeof(RealtimeView), typeof(RealtimeTransform))]

public class RequestOwnershipOnGrab : MonoBehaviour
{

    private XRGrabInteractable _xrGrabInteractable;
    private RealtimeView _realtimeView;
    private RealtimeTransform _realtimeTransform;
    private Rigidbody _rigidbody;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        _realtimeView = GetComponent<RealtimeView>();
        _realtimeTransform = GetComponent<RealtimeTransform>();
        _xrGrabInteractable = GetComponent<XRGrabInteractable>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _xrGrabInteractable.selectEntered.AddListener( OnSelectEntered );
        _xrGrabInteractable.selectExited.AddListener( OnSelectExited );
    }

    // Update is called once per frame
    void OnDisable()
    {
        _xrGrabInteractable.selectEntered.RemoveListener( OnSelectEntered );
        _xrGrabInteractable.selectExited.RemoveListener( OnSelectExited );
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        _realtimeView.RequestOwnership();
        _realtimeTransform.RequestOwnership();
    }
    
    private void OnSelectExited(SelectExitEventArgs args)
    {
        //_realtimeView.ClearOwnership();
        //_realtimeTransform.ClearOwnership();
        _rigidbody.isKinematic = false;
    }
}
