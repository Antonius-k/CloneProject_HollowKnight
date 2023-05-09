using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Monster : MonoBehaviour
{
    //virtual-가상 함수
    public virtual void Damaged(int damage) {
        print("parentDamaged");
    }

    

}
