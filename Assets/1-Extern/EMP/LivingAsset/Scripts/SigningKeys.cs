//
// MIT License
// Copyright (c) EMP - https://github.com/SchmobertRitz/EMP-Unity
//
using UnityEngine;

namespace EMP.LivingAsset
{
    public class SigningKeys : ScriptableObject
    {
        public const string FILE_NAME = "SigningKeys.asset";

        [SerializeField]
        public string PrivateKey;

        [SerializeField]
        public string PublicKey;
    }
}
