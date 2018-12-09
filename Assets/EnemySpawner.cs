using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    private float yMin = 2;
    private float yMax = 20;
    private float xMin = -4;
    private float xMax = 4  ;
    public GameObject shadowEnemy;
    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {
        int i = 1;
		if(this.GetComponent<CombatHandler>().enemyLogics.Count == 0)
        {
            StartCoroutine(SpawnEnemy(i));
            i++;
        }
    }


    IEnumerator SpawnEnemy(int count)
    {
        for (int i = 0; i < count; i++) {
            float x = PickX();
            float y = PickY();
            Instantiate(shadowEnemy, new Vector3(x, y, -1), new Quaternion(0, 0, 0, 0));
        }
        yield return new WaitForSeconds(5);
    }

    float PickY()
    {
        float y = Random.Range(yMin, yMax);
        if (Mathf.Abs(player.transform.position.y - y) < 2) {
            y = Random.Range(yMin, yMax);
        }
        return y;
    }
    float PickX()
    {
        float x = Random.Range(xMin, xMax);
        if (Mathf.Abs(player.transform.position.x - x) < 2)
        {
            x = Random.Range(xMin, xMax);
        }
        return x;
    }
}
