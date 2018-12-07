using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBean {
    public string EnemyAttack { get; private set; }
    public string CharAttack { get; private set; }
    public bool Win { get; private set; }
    public Animatable animatable { get; private set; }
    public AnimationBean (string enemyAttackInit, string charAttackInit, bool winInit, Animatable animatableInit) {
        EnemyAttack = enemyAttackInit;
        CharAttack = charAttackInit;
        Win = winInit;
        animatable = animatableInit;
    }
}
