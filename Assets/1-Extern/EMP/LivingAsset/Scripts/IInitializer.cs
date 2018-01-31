﻿//
// MIT License
// Copyright (c) EMP - https://github.com/SchmobertRitz/EMP-Unity
//
namespace EMP.LivingAsset
{
    public interface IInitializer
    {
        void Initialize(XmlManifest manifest, UnityEngine.AssetBundle[] assetBundles);
    }
}

