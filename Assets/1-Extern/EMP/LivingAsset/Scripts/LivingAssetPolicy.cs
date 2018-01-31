//
// MIT License
// Copyright (c) EMP - https://github.com/SchmobertRitz/EMP-Unity
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMP.LivingAsset
{
    public class LivingAssetPolicy
    {
        private readonly XmlManifest manifest;

        public LivingAssetPolicy(XmlManifest manifest)
        {
            this.manifest = manifest;
        }



        public bool IsNamespaceValid(string className)
        {
            return className.StartsWith(manifest.Name);
        }
    }
}
