using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Monster : MonoBehaviour
{
    //virtual-���� �Լ�
    public virtual void Damaged(int damage) {
        print("parentDamaged");
    }

    

}
