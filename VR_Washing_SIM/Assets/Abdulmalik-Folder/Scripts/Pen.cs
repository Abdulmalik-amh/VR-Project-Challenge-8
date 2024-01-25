using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
  [Header("Pen Properties")]

  public Transform tip;
  public Material drawingMat;
  public Material tipMat;
  public float penWidth = 0.01f;
  public Color[] _PenColors;

  [Header("Hands & Grabable")]
  public OVRGrabber rightHand;
  public OVRGrabber leftHand;
  public OVRGrabbable OVRGrabbable;

  private LineRenderer CurrentDrawing;
  private List<Vector3> posistion = new();
  private int index;
  private int currentColorIndex;

  /// <summary>
  /// Start is called on the frame when a script is enabled just before
  /// any of the Update methods is called the first time.
  /// </summary>
  private void Start()
  {
    currentColorIndex = 0;
    tipMat.color = _PenColors[currentColorIndex];
  }
  private void Update() {
    bool isGrabbed = OVRGrabbable.isGrabbed;
    bool isRightHandDrawing = isGrabbed && OVRGrabbable.grabbedBy == rightHand && 
                                OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);

    bool isLeftHandDrawing = isGrabbed && OVRGrabbable.grabbedBy == leftHand && 
                                OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);

    if(isRightHandDrawing || isLeftHandDrawing){
        Draw();

    }

    else if(CurrentDrawing != null){
        CurrentDrawing = null;
    }

    else if(OVRInput.GetDown(OVRInput.Button.One)){
        SwitchColor();
    }
  }

    private void SwitchColor()
    {
       if(currentColorIndex == _PenColors.Length - 1){
        currentColorIndex =0;

       }
       else{
        currentColorIndex++;
       }

       tipMat.color = _PenColors[currentColorIndex];
    }

    private void Draw()
    {
        if(CurrentDrawing == null){
            index= 0;
            CurrentDrawing = new GameObject().AddComponent<LineRenderer>();
            CurrentDrawing.material = drawingMat;
            CurrentDrawing.startColor = CurrentDrawing.endColor = _PenColors[currentColorIndex];
            CurrentDrawing.startWidth = CurrentDrawing.endWidth = penWidth;
            CurrentDrawing.positionCount = 1;
            CurrentDrawing.SetPosition(0,tip.transform.position);
        }

        else{
            var currentPosition = CurrentDrawing.GetPosition(index);
            Debug.Log("Correct");
            if(Vector3.Distance(currentPosition, tip.transform.position) > 0.01f){
                index++;
                CurrentDrawing.positionCount = index + 1;
                CurrentDrawing.SetPosition(index, tip.transform.position);
                
            }
        }
    }
}
