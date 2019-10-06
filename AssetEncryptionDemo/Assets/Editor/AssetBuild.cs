﻿using UnityEngine;
using UnityEditor;
using System.IO;

public static class AssetBuild {

    [MenuItem( "Tools/AssetBundleBuilder/Stream" )]
    static void StreamBuild() {
        var obj = Selection.activeObject;
        if( obj != null ) {
            string path = AssetDatabase.GetAssetPath( obj );
            string abName = path.Replace( Path.GetExtension( path ), ".ab" );
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = abName;
            build.assetNames = new string[] { path };
            BuildPipeline.BuildAssetBundles( Application.streamingAssetsPath, new AssetBundleBuild[] { build }, BuildAssetBundleOptions.None, BuildTarget.Android );

            StreamEncryption.EcryptAssetBundle( Application.streamingAssetsPath + "/" + abName );

            Debug.Log( "打包完成" );
            AssetDatabase.Refresh();
        }
    }

    [MenuItem( "Tools/AssetBundleBuilder/AES" )]
    static void AESBuild() {
        var obj = Selection.activeObject;
        if( obj != null ) {
            string path = AssetDatabase.GetAssetPath( obj );
            string abName = path.Replace( Path.GetExtension( path ), ".ab" );
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = abName;
            build.assetNames = new string[] { path };
            BuildPipeline.BuildAssetBundles( Application.streamingAssetsPath, new AssetBundleBuild[] { build }, BuildAssetBundleOptions.None, BuildTarget.Android );

            AESEncryption.EcryptAssetBundle( Application.streamingAssetsPath + "/" + abName );

            Debug.Log( "打包完成" );
            AssetDatabase.Refresh();
        }
    }
}
