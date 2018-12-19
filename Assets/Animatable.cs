using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animatable : MonoBehaviour {
    const float FLOORHEIGHT = -0.6f;
    protected bool isParry;
    protected bool isFeint;
    protected bool isSlash;
    public Rigidbody2D rb;
    protected bool inCombat = false;
    protected Animator anim;
    // Use this for initialization
    protected void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void EnterCombat()
    {
        if (!inCombat)
        {
            inCombat = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            anim.SetTrigger("isUnsheathe");
        }
    }

    public virtual void ExitCombat()
    {
        inCombat = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        anim.SetTrigger("isSheathe");
    }
    public abstract void Death();

    public void Feint(Animatable enemy)
    {
        Vector3 thisPos = this.transform.position;
        Vector3 enemyPos = enemy.transform.position;
        Vector3 dirVec = thisPos - enemyPos;
        dirVec /= dirVec.magnitude;
        Vector3 tempPos = CheckAboveFloor(enemyPos + dirVec);
        StartCoroutine(MoveAtoB(tempPos, this.gameObject, 100f));
    }
    public void Parry(Animatable enemy)
    {

    }
    public void Slash(Animatable enemy)
    {
        Vector3 thisPos = this.transform.position;
        Vector3 enemyPos = enemy.transform.position;
        Vector3 dirVec = thisPos - enemyPos;
        dirVec /= dirVec.magnitude;
        Vector3 tempPos = CheckAboveFloor(enemyPos - dirVec);
        StartCoroutine(MoveAtoB(tempPos, this.gameObject, 100f));
    }


    private IEnumerator MoveAtoB(Vector3 end, GameObject item, float speed)
    {

        Vector3 startPos = item.transform.position;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            item.transform.position = Vector3.Lerp(startPos, end, Mathf.SmoothStep(0f, speed, t));
            yield return null;
        }

    }

    public Vector3 CheckAboveFloor(Vector3 position)
    {
        if (position.y < FLOORHEIGHT)
        {
            position.y = FLOORHEIGHT;
        }
        return position;
    }

    protected IEnumerator TrueDeath(int time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
