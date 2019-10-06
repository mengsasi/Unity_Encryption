using System;
using System.Collections;
using System.IO;
using UnityEngine;

public static class AESEncryption {

    public static void EcryptAssetBundle( string abPath ) {
        byte[] bytes = File.ReadAllBytes( abPath );
        bytes = AES.AESEncrypt( bytes );//AES加密
        File.WriteAllBytes( abPath, bytes );
    }

    public static void DecryptAssetBundle( string abPath, string name, Action<UnityEngine.Object> onLoadedCall ) {
        //使用AES加密的，解密只能用字节转，然后LoadFromMemory
        //使用FileStream那种的（掺和用）报错: CryptographicException: Padding is invalid and cannot be removed.
        byte[] bytes = File.ReadAllBytes( abPath );
        bytes = AES.AESDecrypt( bytes );
        var myLoadedAssetBundle = AssetBundle.LoadFromMemory( bytes );

        var obj = myLoadedAssetBundle.LoadAsset<GameObject>( name );
        try {
            onLoadedCall( obj );
        }
        catch( System.Exception e ) {
            Debug.LogError( e.Message );
        }
        myLoadedAssetBundle.Unload( false );
    }

    public static IEnumerator AsyncLoad( string abPath, string name, Action<UnityEngine.Object> onLoadedCall ) {
        byte[] bytes = File.ReadAllBytes( abPath );
        bytes = AES.AESDecrypt( bytes );
        var bundleLoadRequest = AssetBundle.LoadFromMemoryAsync( bytes );

        yield return bundleLoadRequest;
        var myLoadedAssetBundle = bundleLoadRequest.assetBundle;
        var assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync( name );
        yield return assetLoadRequest;
        try {
            onLoadedCall( assetLoadRequest.asset );
        }
        catch( System.Exception e ) {
            Debug.LogError( e.Message );
        }
        myLoadedAssetBundle.Unload( false );
    }
}
