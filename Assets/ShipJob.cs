using UnityEngine;
using System.Collections;
using UnityEngine.Jobs;
using Unity.Collections;
using Unity.Burst;

[BurstCompile]
public struct ShipJob : IJobParallelForTransform
{
    public float deltaTime;
    public float xRightCoord;
    public float xLeftCoord;

    public void Execute(int index, TransformAccess transform)
    {
        if (transform.position.x > xRightCoord)
        {
            transform.position = new Vector3(xLeftCoord, transform.position.y, transform.position.z);
        }
        transform.position += new Vector3(deltaTime * 10, 0, 0);
    }
}
