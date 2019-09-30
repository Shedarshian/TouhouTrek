using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZMDFQ
{
    public class BundleInfo
    {
        public List<string> ParentPaths = new List<string>();
    }

    public enum PlatformType
    {
        None,
        Android,
        IOS,
        PC,
        MacOS,
    }

    public enum BuildType
    {
        Development,
        Release,
    }

    public class BuildEditor : EditorWindow
    {
        private readonly Dictionary<string, BundleInfo> dictionary = new Dictionary<string, BundleInfo>();

        private PlatformType platformType;
        private bool isBuildExe;
        private bool isContainAB;
        private BuildType buildType;
        private BuildOptions buildOptions = BuildOptions.AllowDebugging | BuildOptions.Development;
        private BuildAssetBundleOptions buildAssetBundleOptions = BuildAssetBundleOptions.None;

        [MenuItem("Tools/打包工具")]
        public static void ShowWindow()
        {
            GetWindow(typeof(BuildEditor));
        }

        private void OnGUI()
        {
            this.platformType = (PlatformType)EditorGUILayout.EnumPopup(platformType);
            this.isBuildExe = EditorGUILayout.Toggle("是否打包EXE: ", this.isBuildExe);
            this.isContainAB = EditorGUILayout.Toggle("是否同将资源打进EXE: ", this.isContainAB);
            this.buildType = (BuildType)EditorGUILayout.EnumPopup("BuildType: ", this.buildType);

            switch (buildType)
            {
                case BuildType.Development:
                    this.buildOptions = BuildOptions.Development | BuildOptions.AutoRunPlayer | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging;
                    break;
                case BuildType.Release:
                    this.buildOptions = BuildOptions.None;
                    break;
            }

            this.buildAssetBundleOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumFlagsField("BuildAssetBundleOptions(可多选): ", this.buildAssetBundleOptions);

            if (GUILayout.Button("重新标记"))
            {
                SetPackingTagAndAssetBundle();
            }

            if (GUILayout.Button("开始打包"))
            {
                if (this.platformType == PlatformType.None)
                {
                    Debug.LogError("请选择打包平台!");
                    return;
                }
                BuildHelper.Build(this.platformType, this.buildAssetBundleOptions, this.buildOptions, this.isBuildExe, this.isContainAB);
            }
        }

        private void SetPackingTagAndAssetBundle()
        {
            //ClearPackingTagAndAssetBundle();

            //SetIndependentBundleAndAtlas("Assets/Bundles/Independent");

            SetBundleAndAtlasWithoutShare("Assets/" + PathHelper.DataPath, PathHelper.DataPath);

            SetBundleAndAtlasWithoutShare("Assets/" + PathHelper.StandPicturePath, PathHelper.StandPicturePath);

            SetBundleAndAtlasWithoutShare("Assets/" + PathHelper.SpritesPath, PathHelper.SpritesPath);

            //SetBundleAndAtlasWithoutShare("Assets/Bundles/Timeline", PathHelper.TimeLinePath);

            SetFairyGUI();

            //SetRootBundleOnly("Assets/Bundles/Unit");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
        }

        private static void SetNoAtlas(string dir)
        {
            List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);

            foreach (string path in paths)
            {
                List<string> pathes = CollectDependencies(path);

                foreach (string pt in pathes)
                {
                    if (pt == path)
                    {
                        continue;
                    }

                    SetAtlas(pt, "", true);
                }
            }
        }

        // 会将目录下的每个prefab引用的资源强制打成一个包，不分析共享资源
        //private static void SetBundles(string dir)
        //{
        //	List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);
        //	foreach (string path in paths)
        //	{
        //		string path1 = path.Replace('\\', '/');
        //		Object go = AssetDatabase.Instance.LoadAssetAtPath<Object>(path1);

        //		SetBundle(path1, go.name);
        //	}
        //}

        // 会将目录下的每个prefab引用的资源打成一个包,只给顶层prefab打包
        //private static void SetRootBundleOnly(string dir)
        //{
        //	List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);
        //	foreach (string path in paths)
        //	{
        //		string path1 = path.Replace('\\', '/');
        //		Object go = AssetDatabase.Instance.LoadAssetAtPath<Object>(path1);

        //		SetBundle(path1, go.name);
        //	}
        //}

        private static void SetFairyGUI()
        {
            string end = "_fui.bytes";
            List<string> paths = EditorResHelper.GetPrefabsAndScenes("Assets/" + PathHelper.UIPath);
            foreach (var path in paths)
            {
                if (path.EndsWith(end))
                {
                    string start = path.Substring(0, path.Length - end.Length);
                    //Debug.Log(start);
                    foreach (var path0 in paths)
                    {
                        if (path0.StartsWith(start))
                        {
                            string path1 = path0.Replace('\\', '/');
                            Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

                            SetBundle(path1, Path.Combine(PathHelper.UIPath, Path.GetFileNameWithoutExtension(start) + (path0 == path ? "desc" : "res")));
                        }
                    }
                }
            }
        }

        // 会将目录下的每个prefab引用的资源强制打成一个包，不分析共享资源
        private static void SetIndependentBundleAndAtlas(string dir)
        {
            List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);
            foreach (string path in paths)
            {
                string path1 = path.Replace('\\', '/');
                Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

                AssetImporter importer = AssetImporter.GetAtPath(path1);
                if (importer == null || go == null)
                {
                    Debug.LogError("error: " + path1);
                    continue;
                }
                importer.assetBundleName = $"{go.name}";

                List<string> pathes = CollectDependencies(path1);

                foreach (string pt in pathes)
                {
                    if (pt == path1)
                    {
                        continue;
                    }

                    SetBundleAndAtlas(pt, go.name, true);
                }
            }
        }

        private static void SetBundleAndAtlasWithoutShare(string dir, string dirPath,bool subDir=false,string bundleName=null)
        {
            List<string> paths = EditorResHelper.GetAllResourcePath(dir, subDir);
            foreach (string path in paths)
            {
                string path1 = path.Replace('\\', '/');
                Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

                SetBundle(path1, bundleName == null ? Path.Combine(dirPath, go.name) : bundleName);

                //List<string> pathes = CollectDependencies(path1);
                //foreach (string pt in pathes)
                //{
                //	if (pt == path1)
                //	{
                //		continue;
                //	}
                //
                //	SetBundleAndAtlas(pt, go.name);
                //}
            }
        }

        private static List<string> CollectDependencies(string o)
        {
            string[] paths = AssetDatabase.GetDependencies(o);

            //Log.Debug($"{o} dependecies: " + paths.ToList().ListToString());
            return paths.ToList();
        }

        // 分析共享资源
        private void SetShareBundleAndAtlas(string dir)
        {
            this.dictionary.Clear();
            List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);

            foreach (string path in paths)
            {
                string path1 = path.Replace('\\', '/');
                Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

                SetBundle(path1, go.name);

                List<string> pathes = CollectDependencies(path1);
                foreach (string pt in pathes)
                {
                    if (pt == path1)
                    {
                        continue;
                    }

                    // 不存在则记录下来
                    if (!this.dictionary.ContainsKey(pt))
                    {
                        // 如果已经设置了包
                        if (GetBundleName(pt) != "")
                        {
                            continue;
                        }
                        Debug.Log($"{path1}----{pt}");
                        BundleInfo bundleInfo = new BundleInfo();
                        bundleInfo.ParentPaths.Add(path1);
                        this.dictionary.Add(pt, bundleInfo);

                        SetAtlas(pt, go.name);

                        continue;
                    }

                    // 依赖的父亲不一样
                    BundleInfo info = this.dictionary[pt];
                    if (info.ParentPaths.Contains(path1))
                    {
                        continue;
                    }
                    info.ParentPaths.Add(path1);

                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    string dirName = dirInfo.Name;

                    SetBundleAndAtlas(pt, $"{dirName}-share", true);
                }
            }
        }

        private static void ClearPackingTagAndAssetBundle()
        {
            //List<string> bundlePaths = EditorResHelper.GetAllResourcePath("Assets/Bundles/", true);
            //foreach (string bundlePath in bundlePaths)
            //{
            //	SetBundle(bundlePath, "", true);
            //}

            List<string> paths = EditorResHelper.GetAllResourcePath("Assets/Res", true);
            foreach (string pt in paths)
            {
                SetBundleAndAtlas(pt, "", true);
            }
        }

        private static string GetBundleName(string path)
        {
            string extension = Path.GetExtension(path);
            if (extension == ".cs" || extension == ".dll" || extension == ".js")
            {
                return "";
            }
            if (path.Contains("Resources"))
            {
                return "";
            }

            AssetImporter importer = AssetImporter.GetAtPath(path);
            if (importer == null)
            {
                return "";
            }

            return importer.assetBundleName;
        }

        private static void SetBundle(string path, string name, bool overwrite = true)
        {
            string extension = Path.GetExtension(path);
            if (extension == ".cs" || extension == ".dll" || extension == ".js")
            {
                return;
            }
            if (path.Contains("Resources"))
            {
                return;
            }

            AssetImporter importer = AssetImporter.GetAtPath(path);
            if (importer == null)
            {
                return;
            }

            if (importer.assetBundleName != "" && overwrite == false)
            {
                return;
            }

            if (importer.assetBundleName == name)
            {
                return;
            }

            //Log.Info(path);
            string bundleName = "";
            if (name != "")
            {
                bundleName = $"{name}";
            }

            importer.assetBundleName = bundleName;
        }

        private static void SetAtlas(string path, string name, bool overwrite = false)
        {
            string extension = Path.GetExtension(path);
            if (extension == ".cs" || extension == ".dll" || extension == ".js")
            {
                return;
            }
            if (path.Contains("Resources"))
            {
                return;
            }

            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            if (textureImporter == null)
            {
                return;
            }

            if (textureImporter.spritePackingTag != "" && overwrite == false)
            {
                return;
            }
            if (textureImporter.spritePackingTag==name)
            {
                return;
            }
            textureImporter.spritePackingTag = name;
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
        }

        private static void SetBundleAndAtlas(string path, string name, bool overwrite = false)
        {
            string extension = Path.GetExtension(path);
            if (extension == ".cs" || extension == ".dll" || extension == ".js" || extension == ".mat")
            {
                return;
            }
            if (path.Contains("Resources"))
            {
                return;
            }

            AssetImporter importer = AssetImporter.GetAtPath(path);
            if (importer == null)
            {
                return;
            }

            if (importer.assetBundleName == "" || overwrite)
            {
                string bundleName = "";
                if (name != "")
                {
                    bundleName = $"{name}";
                }

                importer.assetBundleName = bundleName;
            }

            TextureImporter textureImporter = importer as TextureImporter;
            if (textureImporter == null)
            {
                return;
            }

            if (textureImporter.spritePackingTag == "" || overwrite)
            {
                if (textureImporter.spritePackingTag == name) return;
                textureImporter.spritePackingTag = name;
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
            }
        }
    }
}
