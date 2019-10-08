using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    private void Start() {
        var obj = GameObject.CreatePrimitive( PrimitiveType.Cube );
        obj.transform.position = new Vector3( 0, 0, 0 );
    }

}
