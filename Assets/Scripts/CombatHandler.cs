using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHandler : MonoBehaviour {
    bool Animating = false;
    public bool inCombat = false;
    const float FLOORHEIGHT = -0.6f;
    const int WIN = 1,
                LOSE = -1,
                TIE = 0;
    public LockableQueue<CombatResponse> attackQueue = SingletonsCreator.AttackQueue();
    public LockableQueue<CombatResponse> defenseQueue = SingletonsCreator.DefenseQueue();
    public GenericQueue<AnimationBean> animQueue = SingletonsCreator.AnimationQueue();
    public ANotifyingList<EnemyLogic> enemyList = SingletonsCreator.EnemyList();
    public GameObject player;
    public PlayerController playerController;


    private void Start() {
        player = GameObject.Find("Player");
        playerController = (PlayerController)player.GetComponent("PlayerController");
    }

    void Update() {
        print(enemyList.ListenerCount());
    }

    public void AddEnemy(EnemyLogic enemy) {
        enemyList.Add(enemy);
    }
    public void RemoveEnemy(EnemyLogic enemy) {
        enemyList.Remove(enemy);
    }

    public void EnterCombat() {
        playerController.EnterCombat();
        if (!inCombat) {
            inCombat = true;
            StartCoroutine(RunCombat());
        }
    }

    public IEnumerator RunCombat() {
        while (attackQueue.Length() > 0) {
            CombatResponse attack = attackQueue.Next();
            Debug.DrawLine(attack.Self.transform.position, player.transform.position);
            switch (attack.AttackType) {
                case "Slash":
                    AddIndicator(attack.Self, Color.red);
                    break;
                case "Parry":
                    AddIndicator(attack.Self, Color.yellow);
                    break;
                case "Feint":
                    AddIndicator(attack.Self, Color.blue);
                    break;
            }
            yield return new WaitForSeconds(1);
            FillAnimQueue(attack);
        }
        if (!Animating) {
            StartCoroutine(ExecuteCombat());
        }
    }

    public IEnumerator ExecuteCombat() {
        defenseQueue.Clear();
        Animating = true;
        List<Animatable> deadOnes = new List<Animatable>();
        for (int i = 0; i < enemyList.Count(); i++) {
            enemyList.Get(i).EnterCombat();
        }
        while (animQueue.Length() > 0) {
            yield return new WaitForSeconds(1);
            AnimationBean currentSet = animQueue.Next();
            if (currentSet.Win == WIN) {
                deadOnes.Add(currentSet.Enemy);
            } else if (currentSet.Win == LOSE) {
                deadOnes.Add(playerController);
            }
            RunAnimation(currentSet);
        }
        yield return new WaitForSeconds(2f);
        AnimateDeaths(deadOnes);
        yield return new WaitForSeconds(1);
        ExitCombat();
    }

    private void ExitCombat() {
        playerController.ExitCombat();
        for (int i = 0; i < enemyList.Count(); i++) {
            enemyList.Get(i).ExitCombat();
        }
        inCombat = false;
        Animating = false;

    }
    private void RunAnimation(AnimationBean anim) {
        //Get a unit vector pointing from the anim to the player.
        Vector3 playerPos = player.transform.position;
        Vector3 enemyPos = anim.Enemy.transform.position;
        Vector3 dirVec = playerPos - enemyPos;
        dirVec /= dirVec.magnitude;

        //remove the indicator
        Destroy(anim.Enemy.gameObject.GetComponent<LineRenderer>());
        if (anim.CharAttack == "Parry") {
            playerController.Parry(anim.Enemy);
        }
        else if (anim.CharAttack == "Feint") {
            playerController.Feint(anim.Enemy);
        }
        else if (anim.CharAttack == "Slash") {
            playerController.Slash(anim.Enemy);
        }
        if (anim.EnemyAttack == "Parry") {
            playerController.Parry(playerController);
        }
        else if (anim.EnemyAttack == "Feint") {
            anim.Enemy.Feint(playerController);
        }
        else if (anim.EnemyAttack == "Slash")
        {
            anim.Enemy.Slash(playerController);
        }
    }
    private void AnimateDeaths(List<Animatable> deaths) {
        for (int i = 0; i < deaths.Count; i++) {
            deaths[i].Death();
        }
    }

    public void AddIndicator(GameObject enemy, Color indicatorColor) {
        enemy.AddComponent<LineRenderer>();
        LineRenderer lineRend = enemy.GetComponent<LineRenderer>();
        Vector3[] toPlayer = { player.transform.position, enemy.transform.position };
        lineRend.SetPositions(toPlayer);
        lineRend.endWidth /= 2;
        lineRend.startWidth = 0;
        Material particleMat = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lineRend.material = particleMat;
        lineRend.startColor = indicatorColor;
        lineRend.endColor = indicatorColor;
    }

    public void FillAnimQueue(CombatResponse attack) {
        if (defenseQueue.Length() > 0)
        {
            CombatResponse defense = defenseQueue.Next();
            int victor = attack.Battle(defense);
            if (victor ==  WIN) {
                animQueue.Add(new AnimationBean(attack.AttackType, defense.AttackType, WIN, attack.Self.GetComponent<EnemyLogic>()));
            }
            else if (victor == LOSE) {
                animQueue.Add(new AnimationBean(attack.AttackType, defense.AttackType, LOSE, attack.Self.GetComponent<EnemyLogic>()));
            }
            else
            {
                animQueue.Add(new AnimationBean(attack.AttackType, defense.AttackType, TIE, attack.Self.GetComponent<EnemyLogic>()));
            }
        }
        else {
            animQueue.Add(new AnimationBean(attack.AttackType, "nothing", LOSE, attack.Self.GetComponent<EnemyLogic>()));
        }
    }
    //public Vector3 CheckAboveFloor(Vector3 position) {
    //    if (position.y < FLOORHEIGHT)
    //    {
    //        position.y = FLOORHEIGHT;
    //    }
    //    return position;
    //}

    private IEnumerator MoveAtoB(Vector3 end, GameObject item, float speed) {
       
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
}