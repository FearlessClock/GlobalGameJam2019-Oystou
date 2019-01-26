using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseManager : MonoBehaviour
{
    public GameObject playerObject;
    public Transform spawnPoint;
    private void Awake()
    {
        GameObject player = Instantiate<GameObject>(playerObject, spawnPoint.position, Quaternion.identity);
    }

    private void Start()
    {
        ExitRoom.OnRoomExit += OnRoomExit;
    }

    private void OnDestroy()
    {
        ExitRoom.OnRoomExit -= OnRoomExit;
    }

    private void OnRoomExit()
    {
        SceneManager.LoadScene("Main");
    }
}
