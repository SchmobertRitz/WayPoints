//
// MIT License
// Copyright (c) EMP - https://github.com/SchmobertRitz/EMP-Unity
//
using EMP.Editor;
using System.Collections.Generic;
using UnityEngine;
using EMP.Forms;
using System;
using UnityEditor;
using System.IO;
using EMP.Cs;
using System.Security.Cryptography;
using System.Text;

namespace EMP.LivingAsset
{
    public class CreateLivingAssetDialog : FormPopup
    {
        private ILogger Logger = Debug.logger;

        [MenuItem("Assets/Living Asset/Init Living Asset here...", validate = false)]
        public static void LivingAsset_CreateLivingAsset()
        {
            new CreateLivingAssetDialog().Show();
        }

        [MenuItem("Assets/Living Asset/Init Living Asset here...", validate = true)]
        public static bool LivingAsset_Validate()
        {
            return SelectionHelper.IsDirectorySelected();
        }

        protected override Vector2 GetFormSize()
        {
            return new Vector2(500, 300);
        }

        private TextField txtName;
        private TextField txtDescr;
        private Toggle tglCompression;
        private Toggle tglKeys;
        private ValidationLabel lblMessage;
        private ValidationLabel lblNameMessage;
        private Form form;

        protected override void OnCreateForm(Form form)
        {
            this.form = form;
            form.Add(new Headline("Create Living Asset").H(30));

            form.Add(new View() + new ValidationLabel("Please enter a name for the LivingAsset. The name must match a valid namespace pattern.").Bind(out lblNameMessage).W(300).H(30));
            form.Add(new Label("Name:").H(30) + new TextField("", 300).Bind(out txtName));
            form.Add(new View() + new ValidationLabel("Please enter a human-readable description of the LivingAsset.").Bind(out lblMessage).W(300).H(30));
            form.Add(new Label("Description:").H(30) + new TextField("", 300).Bind(out txtDescr));
            form.Add(new View(), 0.125f);
            form.Add(new Toggle(true, "Use Compression").Bind(out tglCompression) .H(30));
            form.Add(new Toggle(true, "Create Keys For Signing").Bind(out tglKeys).H(30));
            form.Add(new View(), 1);
            form.Add(new View() + (new Button("Ok", OnButtonClick) .W(100)) .H(30));

            txtName.Validator = new ValidNamespace().Bind(lblNameMessage);
        }

        private void OnButtonClick(Button button)
        {
            if (form.Validate().Length > 0)
            {
                return;
            }
            
            string path = AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(SelectionHelper.GetSelectedPath(), txtName.Text));

            Manifest manifestEditor = ScriptableObject.CreateInstance<Manifest>();
            manifestEditor.XmlManifest = new XmlManifest();
            manifestEditor.XmlManifest.Name = txtName.Text;
            manifestEditor.XmlManifest.Description = txtDescr.Text;
            manifestEditor.UseCompression = tglCompression.Checked;

            AssetDatabase.CreateAsset(manifestEditor, Path.Combine(path, Manifest.FILE_NAME));
            AssetDatabase.CreateFolder(path, "Scripts");
            AssetDatabase.CreateFolder(path, "Libs");
            AssetDatabase.CreateFolder(path, "Assets");

            SourcesInfo sourceInfo = new SourcesInfo();
            sourceInfo.@namespace = txtName.Text;
            sourceInfo.headerComment = "/* This class is part of the LivingAsset " + txtName.Text + " */\n\n";
            sourceInfo.Save(Path.Combine(Path.Combine(path, "Scripts"), SourcesInfo.FILE_NAME));

            InitializerStubGenerator generator = new InitializerStubGenerator();
            string code = generator.Generate(new Dictionary<string, object> { { "NAMESPACE", txtName.Text } });
            File.WriteAllText(Path.Combine(path, "Scripts/Initializer.cs"), code);

            LivingAssetBuilder.CompileCsSources(path);

            if (tglKeys.Checked)
            {
                SigningKeys keys = ScriptableObject.CreateInstance<SigningKeys>();
                AssetDatabase.CreateAsset(keys, Path.Combine(path, SigningKeys.FILE_NAME));

                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    keys.PublicKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(RSA.ToXmlString(false)));
                    keys.PrivateKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(RSA.ToXmlString(true)));
                }

                manifestEditor.SigningKeys = keys;
            }

            Selection.activeObject = manifestEditor;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Close();
        }
    }

    class InitializerStubGenerator : CsGenerator
    {
        protected override string GetTemplate()
        {
            return
@"using EMP.LivingAsset;
using UnityEngine;

namespace #NAMESPACE#
{
    public class Initializer : IInitializer
    {
        public void Initialize(XmlManifest manifest, AssetBundle[] assetBundles)
        {
            Debug.Log(""LivingAsset '"" + manifest.Name + ""' successfully initialized."");
        }
    }
}";
        }
    }
}
