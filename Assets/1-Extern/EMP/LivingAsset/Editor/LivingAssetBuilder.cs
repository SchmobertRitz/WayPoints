//
// MIT License
// Copyright (c) EMP - https://github.com/SchmobertRitz/EMP-Unity
//
using UnityEngine;
using UnityEditor;
using System.IO;
using EMP.Editor;
using System.Security;
using System.Security.Cryptography;

namespace EMP.LivingAsset
{
    public class LivingAssetBuilder : MonoBehaviour
    {
        public const string ASSETS_PATH = "Assets";
        public const string API_PATH = "Api";
        public const string LIBRARY_PATH = "Libs";

        private static SecureString privateKeyPassword;

        [MenuItem("Assets/Living Asset/Compile C# Sources", validate = false)]
        public static void LivingAsset_CompileCsSources()
        {
            if (SelectionHelper.IsDirectorySelected())
            {
                string path = SelectionHelper.GetSelectedPath();
                CompileCsSources(path);
            }
        }

        public static void CompileCsSources(string path)
        {
            if (IsFileStructureCorrect(path))
            {
                string manifestPath = string.Format(@"{0}/{1}", path, Manifest.FILE_NAME);
                string buildPath = string.Format(@"{0}/{1}", path, LIBRARY_PATH);
                Manifest manifestEditor = Manifest.CreateFromPath(manifestPath);
                XmlManifest manifest = manifestEditor.XmlManifest;
                DllCompiler dllCompiler = new DllCompiler(path, buildPath, manifest);
                dllCompiler.Compile();
                AssetDatabase.Refresh();
                Debug.Log("*** Finished Compiling C# Sources ***");
            }
        }

        [MenuItem("Assets/Living Asset/Build Living Asset", validate = false)]
        public static void LivingAsset_BuildLivingAsset()
        {
            if (Selection.activeObject != null && Selection.activeObject.GetType().IsAssignableFrom(typeof(DefaultAsset)))
            {
                string sourcePath = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
                if (IsFileStructureCorrect(sourcePath))
                {
                    string manifestPath = string.Format(@"{0}/{1}", sourcePath, Manifest.FILE_NAME);
                    string buildPath = FilesHelper.CreateBuildPath(sourcePath);

                    string libsBuildPath = Path.Combine(buildPath, LIBRARY_PATH);
                    string libsSourcePath = Path.Combine(sourcePath, LIBRARY_PATH);

                    string apiBuildPath = Path.Combine(buildPath, API_PATH);

                    string assetBundleBuildPath = Path.Combine(buildPath, ASSETS_PATH);
                    string assetBundleSourcePath = Path.Combine(sourcePath, ASSETS_PATH);

                    Directory.CreateDirectory(libsBuildPath);
                    Directory.CreateDirectory(assetBundleBuildPath);

                    Manifest manifest = Manifest.CreateFromPath(manifestPath);
                    XmlManifest xmlManifest = manifest.XmlManifest;

                    // Copy Manifest
                    XmlManifest.WriteToPath(xmlManifest, string.Format(@"{0}/{1}", buildPath, XmlManifest.FILE_NAME));

                    // Generate Sources and Libs 
                    DllCompiler dllCompiler = new DllCompiler(sourcePath, libsSourcePath, xmlManifest);
                    ApiGenerator apiGenerator = new ApiGenerator(dllCompiler.GetOutputFilePath(), apiBuildPath, xmlManifest);
                    FilesHelper.CollectFiles(libsSourcePath, file => file.EndsWith(".dll")).ForEach(file => File.Copy(file, Path.Combine(libsBuildPath, Path.GetFileName(file))));

                    // Generate Living Asset
                    AssetBundler assetBundler = new AssetBundler(assetBundleSourcePath, assetBundleBuildPath, xmlManifest);

                    Archiver archiver = new Archiver(
                        libsBuildPath,
                        apiBuildPath,
                        assetBundleBuildPath,
                        buildPath,
                        xmlManifest,
                        false,
                        string.IsNullOrEmpty(manifest.SigningKeys.PrivateKey) ? null : manifest.SigningKeys.PrivateKey
                    );

                    dllCompiler.Compile();
                    AssetDatabase.Refresh();
                    apiGenerator.GenerateApi();
                    assetBundler.GenerateBundle();
                    archiver.GenerateArchive();

                    AssetDatabase.Refresh();

                    Debug.Log("*** Finished Building Living Asset ***");
                }
            }
        }

        [MenuItem("Assets/Living Asset/Compile C# Sources", validate = true)]
        [MenuItem("Assets/Living Asset/Build Living Asset", validate = true)]
        public static bool IsCurrentSelectionAValidPath()
        {
            if (!Application.isPlaying && Selection.activeObject != null && Selection.activeObject.GetType().IsAssignableFrom(typeof(DefaultAsset)))
            {
                string path = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
                return IsFileStructureCorrect(path);
            }
            return false;
        }
        
        public static bool IsFileStructureCorrect(string path)
        {
            return Directory.Exists(path)
                    && ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
                    && File.Exists(Path.Combine(path, Manifest.FILE_NAME));
        }

    }
}