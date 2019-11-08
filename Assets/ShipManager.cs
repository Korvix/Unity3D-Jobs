using System.Collections.Generic;
using TMPro;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class ShipManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefab;
    [SerializeField]
    private TextMeshProUGUI shipsCounterUI;
    private List<Transform> ships;
    private float xRightCoord;
    private float xLeftCoord;
    private Camera mainCamera;
    private int numberOfShipsToAdd = 1000;
    private int numberOfShips;
    private TransformAccessArray transforms;
    private ShipJob movementJob;
    private JobHandle movementJobHandle;
    private Transform tempTransform;

    private void Awake()
    {
        ships = new List<Transform>();
        mainCamera = Camera.main;
        xRightCoord = mainCamera.ViewportToWorldPoint(Vector3.right).x;
        xLeftCoord = mainCamera.ViewportToWorldPoint(Vector3.zero).x;
        transforms = new TransformAccessArray(0, -1);
    }

    private void OnDisable()
    {
        movementJobHandle.Complete();
        transforms.Dispose();
    }

    private void Update()
    {
        movementJobHandle.Complete();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddShips();
            numberOfShips += numberOfShipsToAdd;
            shipsCounterUI.text = numberOfShips.ToString();
        }
        ShipJob movementJob = new ShipJob()
        {
            deltaTime = Time.deltaTime,
            xLeftCoord = xLeftCoord,
            xRightCoord = xRightCoord
        };

        movementJobHandle = movementJob.Schedule(transforms);
        JobHandle.ScheduleBatchedJobs();
    }

    private void AddShips()
    {
        transforms.capacity = transforms.length + numberOfShipsToAdd;
        for (int i = 0; i < numberOfShipsToAdd; i++)
        {
            tempTransform = Instantiate(shipPrefab, RandomScreenPosition(), Quaternion.Euler(Vector3.zero)).transform;
            ships.Add(tempTransform);
            transforms.Add(tempTransform);
        }
    }

    private Vector3 RandomScreenPosition()
    {
        Vector3 temp = mainCamera.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f));
        return new Vector3(temp.x, temp.y, 0f);
    }

    private void NavigateShips(List<Transform> ships)
    {
        for (int i = 0; i < ships.Count; i++)
        {
            if (ships[i].position.x > xRightCoord)
            {
                ships[i].position = new Vector3(xLeftCoord, ships[i].position.y, ships[i].position.z);
            }
            ships[i].position += new Vector3(Time.deltaTime * 10, 0, 0);
        }
    }
}