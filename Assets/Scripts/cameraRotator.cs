using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class cameraRotator : MonoBehaviour
{
    [SerializeField] float rotationTime = 0.6f;
    [Range(15f, 180f)]
    [SerializeField] int rotationStep = 90;

    private Tween rotationTween;

    public static event Action<int> onRotate; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "RotateScreen"))
        {
            Rotate();

        }
    }

    private int getNextRotationAngle()
    {
        int nextRotation = (int)transform.rotation.eulerAngles.z + rotationStep;
        nextRotation = ((int)(nextRotation / rotationStep) * rotationStep);


        return nextRotation % 360;

    }

    private void Rotate()
    {
        onRotate?.Invoke(getNextRotationAngle());
        //dont change the offset 
        if (rotationTween == null || !rotationTween.IsPlaying())
        {
            rotationTween = transform.DORotate(new Vector3(0, 0, getNextRotationAngle() + 0.0001f), rotationTime, RotateMode.Fast);
        }

    }
}
