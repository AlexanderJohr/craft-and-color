using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideOnOff : MonoBehaviour {
    //動く床を実装するときに使えるオブジェクト
    private void OnCollisionEnter2D(Collision2D other)
    {
        var emptyObject = new GameObject(); //一時的に親となる空のオブジェクトを作成する
        emptyObject.transform.parent = this.transform;
        other.transform.parent = emptyObject.transform;//「parent」に入れたものを自分の「親」にする
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        other.transform.parent = null; //離れたときは「null」で親から離れます
        var emptyObject = GameObject.Find("New Game Object");
        Destroy(emptyObject);　//一時的に作った空のオブジェクトを破壊する。
    }

   
}
