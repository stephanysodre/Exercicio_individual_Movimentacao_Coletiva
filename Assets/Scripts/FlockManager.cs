using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    // declara variaveis
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos;

    // delara valores da velocidade como min, max,  a distancia entre eles e a rotaçao
    [Header("Configurações do Cardume")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;

    void Start()
    {
        // coloca os peixes na cena aleatoriamente
        allFish = new GameObject[numFish];

        for (int i = 0; i < numFish; i++)
        {
            //coloca os peixes em posiçao aleatoria detro da posiçao que foi declarada em bound
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x,
            swinLimits.x),
            Random.Range(-swinLimits.y,
            swinLimits.y),
            Random.Range(-swinLimits.z,
            swinLimits.z));
            // istancia os peixes
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().myManager = this;
            goalPos = this.transform.position;
        }

    }
    void Update()
    {
        // ponto que fica rodando os peixes
        goalPos = this.transform.position;
        // introduz variaao no ponto de orbitaçao
        if (Random.Range(0, 100) < 10)
            goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x,
            swinLimits.x),
            Random.Range(-swinLimits.y, swinLimits.y),
            Random.Range(-swinLimits.z, swinLimits.z));
    }
}
