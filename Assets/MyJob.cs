using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

[BurstCompile]
public struct MyJob : IJobParallelFor
{
    public NativeArray<float> numbers;

    public void Execute(int index)
    {
        numbers[index] = (float)(1.0 / (1.0 + Math.Pow(Math.E, -numbers[index])));
    }
}