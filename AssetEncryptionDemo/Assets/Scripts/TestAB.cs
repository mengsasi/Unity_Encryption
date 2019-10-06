using UnityEngine;

public class TestAB : MonoBehaviour {

    public bool UseStream = true;

    void Start() {
        if( UseStream ) {
            StreamEncryption.DecryptAssetBundle( Application.streamingAssetsPath + "/assets/cube.ab", "Cube", ( prefab ) => {
                var obj = GameObject.Instantiate<GameObject>( prefab as GameObject );
                obj.transform.localPosition = new Vector3( 0, 0, 0 );
                obj.transform.localRotation = Quaternion.Euler( 0, 0, 0 );
            } );

            StartCoroutine( StreamEncryption.AsyncLoad( Application.streamingAssetsPath + "/assets/cube.ab", "Cube", ( prefab ) => {
                var obj = GameObject.Instantiate<GameObject>( prefab as GameObject );
                obj.transform.localPosition = new Vector3( 0, 0, 0 );
                obj.transform.localRotation = Quaternion.Euler( 0, 0, 0 );
                //为什么会有rotation，属性窗口有数值，但是reset，物体也没变化，就是000的
            } ) );
        }
        else {
            AESEncryption.DecryptAssetBundle( Application.streamingAssetsPath + "/assets/cube.ab", "Cube", ( prefab ) => {
                var obj = GameObject.Instantiate<GameObject>( prefab as GameObject );
                obj.transform.localPosition = new Vector3( 0, 0, 0 );
                obj.transform.localRotation = Quaternion.Euler( 0, 0, 0 );
            } );

            StartCoroutine( AESEncryption.AsyncLoad( Application.streamingAssetsPath + "/assets/cube.ab", "Cube", ( prefab ) => {
                var obj = GameObject.Instantiate<GameObject>( prefab as GameObject );
                obj.transform.localPosition = new Vector3( 0, 0, 0 );
                obj.transform.localRotation = Quaternion.Euler( 0, 0, 0 );
                //为什么会有rotation，属性窗口有数值，但是reset，物体也没变化，就是000的
            } ) );
        }
    }

}
