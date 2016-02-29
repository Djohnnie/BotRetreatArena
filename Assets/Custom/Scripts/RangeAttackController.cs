using UnityEngine;
using System.Collections;

namespace com.terranovita.botretreat
{
    public class RangeAttackController : MonoBehaviour {

        public GameObject projectile;

        void start() {
            projectile.SetActive(false);
        }

    	// Use this for initialization
        public void fire (Vector3 startPos, Vector3 targetPos, float height) {
            projectile.transform.position = startPos;
            projectile.SetActive(true);
            Vector3 top = startPos + ((targetPos - startPos) / 2);
            Vector3 centerPos = new Vector3(top.x, top.y + height, top.z);
            //Here i'm telling to the owner of this script to wall by the path created with the name "EnemyPath1" 
            iTween.MoveTo(projectile,iTween.Hash("path", new [] {startPos, centerPos, targetPos},"time",1f, "easetype", "linear", "oncomplete", "doDestroy", "onCompleteTarget", this.gameObject));
    	}

        void doDestroy() {
            Destroy(this.gameObject);
        }
    }
}