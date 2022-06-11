using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    // declarando variaveis
    public FlockManager myManager;
    public float speed;
    bool turning = false;
    // Start is called before the first frame update
    void Start()
    {
        // declarando a velocidade do cardume o min e o max randomicamente
        speed = Random.Range(myManager.minSpeed,
        myManager.maxSpeed);
    }
    void Update()
    {
        // delimita a onde os peixes vao rodar atraves do pilar
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);
        RaycastHit hit = new RaycastHit();

        // 
        Vector3 direction = myManager.transform.position - transform.position;

        // se ele saiu da area limite, para o movimento normal e muda sua direçao para dentro da area de volta
        if (b.Contains(transform.position) == false)
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        }

        // desvia do pilar
        else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
            turning = false;

        // muda a direçao do movimento, para desviar de obstaculos e entrar devolta dentro da area
        if (turning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(direction),
            myManager.rotationSpeed * Time.deltaTime);
        }
        
        // introduz variaçao na velocidade e direao do movimento dos peixes
        else
        {

            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.minSpeed,
                myManager.maxSpeed);
            if (Random.Range(0, 100) < 20)
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }
    
    void ApplyRules()
    {
        //declara a orbitaao, com distancia, velocidade, tamanho do grupo, centro e o circulo que irao percorrer
        GameObject[] gos;
        gos = myManager.allFish;
        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        //  agrupa os peixes conforme as distancias dos outros
        foreach (GameObject go in gos)
        {          
            if (go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                
                if (nDistance <= myManager.neighbourDistance)
                {
                   
                    vcentre += go.transform.position;
                    groupSize++;
                    if (nDistance < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        // define a posiçao central do grupo
        if(groupSize > 0)
        { 
            
            vcentre = vcentre/groupSize + (myManager.goalPos - this.transform.position);
            speed = gSpeed/groupSize;
            Vector3 direction = (vcentre + vavoid) - transform.position;      
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed* Time.deltaTime);
        }
    }
}
