using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSeaguls : MonoBehaviour
{
    [SerializeField] private Transform seagulSpawnPoz;
    [SerializeField] private GameObject seagulPrefab;

    private GameObject seaguls;
    // Start is called before the first frame update
    void Start()
    {
        seaguls = new GameObject(name: "Seaguls");
        seaguls.transform.SetParent(gameObject.transform);
    }

    public void SpawnSeagul(GameObject turtleGameObj)
    {
        seagulPrefab.GetComponent<SeagulBehavior>().turtle = turtleGameObj;
        GameObject newSeagul = Instantiate(seagulPrefab, seagulSpawnPoz);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
