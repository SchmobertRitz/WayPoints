//
// MIT License
// Copyright (c) EMP - https://github.com/SchmobertRitz/EMP-Unity
//
using UnityEditor;
using UnityEngine;

namespace EMP.LivingAsset
{
    public class Manifest : ScriptableObject
    {
        public const string FILE_NAME = "LivingAsset.asset";

        private ILogger Logger = Debug.logger;

        [SerializeField]
        public XmlManifest XmlManifest;

        [SerializeField]
        public bool UseCompression = true;

        [SerializeField]
        public SigningKeys SigningKeys;
        
        internal static Manifest CreateFromPath(string manifestPath)
        {
            return AssetDatabase.LoadAssetAtPath<Manifest>(manifestPath);
        }
    }
}
