using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatResponse {
    public static bool WIN = true, 
                        LOSE = false;

    public int HorizDir { get; private set; }
    public int VertDir { get; private set; }
    public string AttackType { get; private set; }
    public GameObject Self { get; private set; }
    public CombatResponse(int horizDirInit, int vertDirInit, string attackTypeInit, GameObject selfInit) {
        HorizDir = horizDirInit;
        VertDir = vertDirInit;
        AttackType = attackTypeInit;
        Self = selfInit;
    }

    public bool Battle(CombatResponse defender)
    {
        if (DirCheck(this.HorizDir, defender.HorizDir) &&
        DirCheck(this.VertDir, defender.VertDir) &&
            SingletonsCreator.SingleMoveSetTable().Compare(this.AttackType, defender.AttackType)) {
            return WIN;
        } else {
            return LOSE;
        }

    }

    private bool DirCheck(int attacker, int defender) {
        if(attacker == 0 && defender == 0) {
            return WIN;
        } else if (defender == -attacker) {
            return WIN;
        } else {
            return LOSE;
        }
    }
}
