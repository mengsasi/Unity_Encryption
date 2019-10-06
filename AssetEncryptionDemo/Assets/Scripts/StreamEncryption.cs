using System;
using System.Collections;
using System.IO;
using UnityEngine;

public static class StreamEncryption {
    static uint encryKey = 0x6F7A81F3;

    // byte[] 1 2 5 10 29
    // 把原本的字节数组，和密钥进行异或，变成新的字节数组
    // 0001 ^ 0110 1111 0111 1010 1000 0001 1111 0011 = 0110 1111 0111 1010 1000 0001 1111 001[0] ->这里有个1变了

    public static void EcryptAssetBundle( string abPath ) {
        byte[] EncryKey = EncryptionStream.UInt32ToByte4( encryKey );
        byte[] bytes = File.ReadAllBytes( abPath );
        for( int i = 0; i < bytes.Length; ++i ) {
            bytes[i] ^= EncryKey[i % 4];//异或，不同就是1 0^0=0  1^0=1  0^1=1  1^1=0
        }
        File.WriteAllBytes( abPath, bytes );
    }

    public static void DecryptAssetBundle( string abPath, string name, Action<UnityEngine.Object> onLoadedCall ) {
        var fileStream = new EncryptionStream( abPath, FileMode.Open, FileAccess.Read, encryKey );
        //var fileStream = new FileStream( abPath, FileMode.Open, FileAccess.Read );
        var myLoadedAssetBundle = AssetBundle.LoadFromStream( fileStream );

        var obj = myLoadedAssetBundle.LoadAsset<GameObject>( name );
        try {
            onLoadedCall( obj );
        }
        catch( System.Exception e ) {
            Debug.LogError( e.Message );
        }
        myLoadedAssetBundle.Unload( false );
        fileStream.Close();
    }

    public static IEnumerator AsyncLoad( string abPath, string name, Action<UnityEngine.Object> onLoadedCall ) {
        var fileStream = new EncryptionStream( abPath, FileMode.Open, FileAccess.Read, encryKey );
        //var fileStream = new FileStream( abPath, FileMode.Open, FileAccess.Read );
        var bundleLoadRequest = AssetBundle.LoadFromStreamAsync( fileStream );

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
        fileStream.Close();
    }
}