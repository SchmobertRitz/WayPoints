//
// MIT License
// Copyright (c) EMP - https://github.com/SchmobertRitz/EMP-Unity
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace EMP.LivingAsset
{
    [XmlRoot]
    [Serializable]
    public class XmlManifest
    {
        public const string FILE_NAME = "Manifest.xml";

        public static XmlManifest CreateFromPath(string manifestPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XmlManifest));
            using (FileStream fs = new FileStream(manifestPath, FileMode.Open))
            {
                XmlManifest manifest = (XmlManifest)serializer.Deserialize(fs);
                return manifest;
            }
        }

        public static XmlManifest CreateFromBytes(byte[] bytes)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XmlManifest));
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                XmlManifest manifest = (XmlManifest)serializer.Deserialize(ms);
                return manifest;
            }
        }

        public static void WriteToPath(XmlManifest manifest, string manifestPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XmlManifest));
            using(FileStream stream = File.OpenWrite(manifestPath))
            {
                serializer.Serialize(stream, manifest);
            }
        }

        [XmlAttribute("name")]
        public string Name;

        [XmlAttribute("description")]
        public string Description;
        
        [XmlArray]
        public List<Dependency> Dependencies;
    }

    [Serializable]
    public class Dependency
    {
        [XmlAttribute("name")]
        public string Name;

        [XmlAttribute("file")]
        public string File;
    }
    
}
