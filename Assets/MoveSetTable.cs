using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSetTable {
    internal List<string[]> moveHierarchy = new List<string[]>(); 


    public MoveSetTable() {
        string[] slashSet = { "Slash", "Parry" };
        string[] parrySet = { "Parry", "Feint" };
        string[] feintSet = { "Feint", "Slash" };
        Put(slashSet);
        Put(parrySet);
        Put(feintSet);

    }
    public void Put(string[] moveSet) {
        moveHierarchy.Add(moveSet);
    }

    public bool Compare(string attack, string defense) {
        for (int i = 0; i < moveHierarchy.Count; i++) {
            string[] currentMove = moveHierarchy[i];
            if (attack.Equals(currentMove[0])) {
                for (int j = 1; j < currentMove.Length; j++) {
                    if(defense.Equals(currentMove[j])) {
                        return true;
                    }
                }
                return false;
            }
        }
        throw new System.Exception("Invalid attack");
    }
}
