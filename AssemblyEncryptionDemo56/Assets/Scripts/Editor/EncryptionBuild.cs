using System.IO;
using UnityEditor;
using UnityEngine;

public class EncryptionBuild {

    [MenuItem( "Build/Win x86" )]
    static void BuildWindows_x86() {
        BuildWindows( BuildTarget.StandaloneWindows );
    }

    [MenuItem( "Build/Win x64" )]
    static void BuildWindows_x64() {
        BuildWindows( BuildTarget.StandaloneWindows64 );
    }


    #region Windows打包

    static void BuildWindows( BuildTarget _bt ) {
        //弹出保存路径的弹框
        string path = EditorUtility.SaveFilePanel( _bt.ToString(), EditorPrefs.GetString( "BuildPath" ), PlayerSettings.productName, "exe" );
        if( string.IsNullOrEmpty( path ) )
            return;
        BuildPlayerOptions buildOptions = new BuildPlayerOptions();
        buildOptions.locationPathName = path;
        buildOptions.scenes = EditorBuildSettingsScene.GetActiveSceneList( EditorBuildSettings.scenes );
        buildOptions.target = _bt;
        BuildPipeline.BuildPlayer( buildOptions );
        Debug.Log( "打包完成" );

        //加密
        EncryptAssemblyCSharp( path );
        //替换解密mono.dll
        ReplaceMonoDll( path, _bt );

        int num = path.LastIndexOf( "/" );
        path = path.Substring( 0, num );
        EditorPrefs.SetString( "BuildPath", path );
        EditorUtility.OpenWithDefaultApp( path );
    }

    #endregion


    static void EncryptAssemblyCSharp( string path ) {
        string acsPath = path.Replace( ".exe", "_Data" ) + "/Managed/Assembly-CSharp.dll";
        byte[] readByte = File.ReadAllBytes( acsPath );
        //加密
        byte[] newBytes = new byte[readByte.Length + 1];
        newBytes[0] = 1;
        for( int i = 1; i < newBytes.Length - 1; i++ ) {
            newBytes[i] = readByte[i - 1];
        }
        File.WriteAllBytes( acsPath, newBytes );
        Debug.Log( "加密完成" );
    }

    static void ReplaceMonoDll( string path, BuildTarget _bt ) {
        string mdPath = path.Replace( ".exe", "_Data" ) + "/Mono/mono.dll";
        byte[] readByte = File.ReadAllBytes( Application.dataPath + "/NewMono/" + _bt.ToString() + "/mono.txt" );
        File.WriteAllBytes( mdPath, readByte );
		Debug.Log( "替换完成" );
    }

}
