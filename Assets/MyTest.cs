using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class MyTest : MonoBehaviour
{
    float[] myNumbers;

    private void Awake()
    {
        myNumbers = new float[1000000];
        for (int i = 0; i < myNumbers.Length; i++)
        {
            myNumbers[i] = 542f;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            float startTime = Time.realtimeSinceStartup;
            for (int i = 0; i < myNumbers.Length; i++)
            {
                myNumbers[i] = Sigmoid(myNumbers[i]);
            }
            Debug.Log("Without jobs: " + (Time.realtimeSinceStartup - startTime));
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            float startTime = Time.realtimeSinceStartup;
            NativeArray<float> numbersToCompute = new NativeArray<float>(myNumbers.Length, Allocator.TempJob);
            numbersToCompute.CopyFrom(myNumbers);
            MyJob myJob = new MyJob()
            {
                numbers = numbersToCompute
            };

            JobHandle jobHandle = myJob.Schedule(numbersToCompute.Length, 64);
            jobHandle.Complete();

            Debug.Log("With jobs: " + (Time.realtimeSinceStartup - startTime));
            numbersToCompute.Dispose();
        }
    }

    public static float Sigmoid(double value)
    {
        return (float)(1.0 / (1.0 + Math.Pow(Math.E, -value)));
    }
}
