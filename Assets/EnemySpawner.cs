using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IPropertyChangeListener
{
    private const float yMin = 2;
    private const float yMax = 20;
    private const float xMin = -4;
    private const float xMax = 4;
    private int nextSpawnCount = 1;
    public GameObject shadowEnemy;
    private GameObject player;

    void Start () {
        SingletonsCreator.EnemyList().Register(this);
        player = GameObject.Find("Player");
        StartCoroutine(SpawnEnemy(nextSpawnCount));
        nextSpawnCount++;

    }

    // Update is called once per frame
    void Update () {
    }


    IEnumerator SpawnEnemy(int count) {
        SingletonsCreator.EnemyList().Register(this);
        yield return new WaitForSeconds(4);
        for (int i = 0; i < count; i++) {
            float x = PickX();
            float y = PickY();
            Instantiate(shadowEnemy, new Vector3(x, y, -1), new Quaternion(0, 0, 0, 0));
        }
    }

    float PickY()
    {
        int randomChanger = Random.Range(0, 10000);

        Random.InitState(System.DateTime.Now.Millisecond * randomChanger);
        float y = Random.Range(yMin, yMax);
        //Debug.Log("Y: " + y);
        if (Mathf.Abs(player.transform.position.y - y) < 4) {
            y = player.transform.position.y+4; 
        }
        return y;
    }
    float PickX()
    {
        float x = Random.Range(xMin, xMax);
        int randomChanger = Random.Range(0, 10000);
        Random.InitState(System.DateTime.Now.Millisecond * randomChanger);
        //Debug.Log("X: " + x);
        if (Mathf.Abs(player.transform.position.x - x) < 4)
        {
            x = player.transform.position.x+4;
        }
        return x;
    }

    public void NewPropertyChangeEvent(PropertyChangeEvent e)
    {
        if((int)e.NewValue == 0)
        {
            print("checked");
            StartCoroutine(SpawnEnemy(nextSpawnCount));
            nextSpawnCount++;
        }

    }

}
