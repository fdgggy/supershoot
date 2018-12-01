using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using System.Xml.Serialization;

[Serializable]
public class AssetBundlesXML
{
    [XmlArray("assetBundleBuilds")]
    [XmlArrayItem("assetBundleBuild")]
    public List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();

    //shader的ab包
    public AssetBundleBuild shaderBundle = new AssetBundleBuild();

    //声音的ab包
    [XmlArray("soundBundles")]
    [XmlArrayItem("assetBundleBuild")]
    public List<AssetBundleBuild> soundBundles = new List<AssetBundleBuild>();

    public AssetBundleBuild[] GetAllBundles()
    {
        int allCount = assetBundleBuilds.Count + soundBundles.Count + 1;
        AssetBundleBuild[] all = new AssetBundleBuild[allCount];
        all[0] = shaderBundle;
        assetBundleBuilds.CopyTo(all, 1);
        soundBundles.CopyTo(all, assetBundleBuilds.Count + 1);

        return all;
    }



    public AssetBundleBuild GetAssetBundle(string abName)
    {
        for (int i = 0; i < assetBundleBuilds.Count; i++)
        {
            if (assetBundleBuilds[i].assetBundleName == abName)
            {
                return assetBundleBuilds[i];
            }
        }
        return default(AssetBundleBuild);
    }


    public void RemoveAssetBundle(string abName)
    {
        int len = assetBundleBuilds.Count;
        for (int i = len - 1; i >= 0; i--)
        {
            if (assetBundleBuilds[i].assetBundleName == abName)
            {
                assetBundleBuilds.RemoveAt(i);
                break;
            }
        }

    }

    public void SetAssetBundle(string abName, AssetBundleBuild build)
    {
        for (int i = 0; i < assetBundleBuilds.Count; i++)
        {
            if (assetBundleBuilds[i].assetBundleName == abName)
            {
                assetBundleBuilds[i] = build;
            }
        }
    }
}

