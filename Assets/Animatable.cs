using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animatable : MonoBehaviour {
    bool isParry;
    bool isFeitn;
    bool isSlash;
    public Rigidbody2D rb;
    protected bool inCombat = false;
    private Animator anim;
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
}
