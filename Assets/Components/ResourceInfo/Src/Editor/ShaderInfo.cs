using EditorCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ResourceFormat
{
#pragma warning disable CS0436 // Type conflicts with imported type

    public class ShaderInfo : BaseInfo
    {
        public enum ShaderPlatformModes
        {
            Current_graphics_device = 0,
            Current_build_platform,
            All_platforms,
            Custom
        }

        public int MaxLOD;
        public int RenderQueue;
        public int Pass;
        public int Instruction;
        public ulong Variant;
        public int Property;
        List<CompiledShaderInfo> compiledShaderInfoList = new List<CompiledShaderInfo>();

        private static Dictionary<string, ShaderInfo> _dictShaderInfo = new Dictionary<string, ShaderInfo>();

        public static ShaderInfo CreateShaderInfo(string assetPath)
        {
            if (!EditorPath.IsShader(assetPath))
            {
                return null;
            }

            ShaderInfo shaderInfo = null;
            if (!_dictShaderInfo.TryGetValue(assetPath, out shaderInfo))
            {
                shaderInfo = new ShaderInfo();
                _dictShaderInfo.Add(assetPath, shaderInfo);
            }
            Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(assetPath);
            string shaderText = File.ReadAllText(assetPath);
            MatchCollection passMatches = Regex.Matches(shaderText, @"Pass\s*{");

            MatchCollection susbShaderMatches = Regex.Matches("Sha(der(21)2)000", @"\((?>[^\(\)]+|\((?<DEPTH>)|\)(?<-DEPTH>))*(?(DEPTH)(?!))\)");
            ShaderUtil.OpenCompiledShader(shader, (int)ShaderPlatformModes.Current_build_platform, 1 << (int)ShaderUtil.ShaderCompilerPlatformType.D3D11, false);
            CompiledShaderInfo compiledShaderInfo = CompiledShaderInfo.CreateCompiledShaderInfo(shaderText);

            shaderInfo.Path = assetPath;
            shaderInfo.MaxLOD = ShaderUtil.GetLOD(shader);
            shaderInfo.Variant = ShaderUtil.GetVariantCount(shader, true);
            shaderInfo.Property = ShaderUtil.GetPropertyCount(shader);
            shaderInfo.RenderQueue = shader.renderQueue;
            shaderInfo.Pass = passMatches.Count;
            shaderInfo.Instruction = compiledShaderInfo.GetInstruction();
            shaderInfo.compiledShaderInfoList.Add(compiledShaderInfo);
            return shaderInfo;
        }

        public static List<ShaderInfo> GetInfoByDirectory(string dir)
        {
            List<ShaderInfo> shaderInfoList = new List<ShaderInfo>();
            List<string> list = new List<string>();
            EditorPath.ScanDirectoryFile(dir, true, list);
            for (int i = 0; i < list.Count; ++i)
            {
                string assetPath = EditorPath.FormatAssetPath(list[i]);
                string name = System.IO.Path.GetFileName(assetPath);
                EditorUtility.DisplayProgressBar("获取Shader数据", name, (i * 1.0f) / list.Count);
                ShaderInfo shaderInfo = CreateShaderInfo(assetPath);
                if (shaderInfo != null)
                {
                    shaderInfoList.Add(shaderInfo);
                }
            }
            EditorUtility.ClearProgressBar();
            return shaderInfoList;
        }
    }

    public class CompiledShaderInfo
    {
        public struct Variant
        {
            // List<string> Name;
            string Vertex;
            string Fragment;
            public Variant(string vertex, string fragment)
            {
                Vertex = vertex;
                Fragment = fragment;
            }


            public int GetVertexInstruction()
            {
                return GetInstruction(Vertex);
            }

            public int GetFragmentInstruction()
            {
                return GetInstruction(Fragment);
            }

            private int GetInstruction(string text)
            {
                Match match = Regex.Match(text, "([0-9]+): ret");
                if (match.Success)
                {
                    return int.Parse(match.Groups[1].Value);
                }
                else
                {
                    return 0;
                }
            }

        }

        public struct Pass
        {
            // string Tags;
            // bool ZWrite;
            // string Cull;
            // string Blend;
            // string ColorMask;
            List<Variant> VariantList;
            public Pass(List<Variant> variantList)
            {
                VariantList = variantList;
            }

            public int GetVertexInstruction()
            {
                int max = 0;
                foreach(var variant in VariantList)
                {
                    max = Math.Max(max, variant.GetVertexInstruction());
                }
                return max;
            }

            public int GetFragmentInstruction()
            {
                int max = 0;
                foreach (var variant in VariantList)
                {
                    max = Math.Max(max, variant.GetFragmentInstruction());
                }
                return max;
            }
        }

        public struct SubShader
        {
            // string Tags;
            List<Pass> PassList;
            public SubShader(List<Pass> passList)
            {
                PassList = passList;
            }
            public int GetVertexInstruction()
            {
                int count = 0;
                foreach(var pass in PassList)
                {
                    count += pass.GetVertexInstruction();
                }
                return count;
            }

            public int GetFragmentInstruction()
            {
                int count = 0;
                foreach (var pass in PassList)
                {
                    count += pass.GetFragmentInstruction();
                }
                return count;
            }

        }

        string shaderName;
        // string Property;
        List<SubShader> subShaderList;
        ShaderUtil.ShaderCompilerPlatformType shaderCompilerPlatformType;

        public static CompiledShaderInfo CreateCompiledShaderInfo(string shaderText)
        {
            if (shaderText == string.Empty)
            {
                throw new ArgumentNullException("shaderText");
            }

            CompiledShaderInfo compiledShaderInfo = new CompiledShaderInfo();
            Match shaderNameMatch = Regex.Match(shaderText, "Shader\\s*\"(.*)\"");
            compiledShaderInfo.shaderName = shaderNameMatch.Success ? shaderNameMatch.Result("$1") : string.Empty;

            string tempShaderText = File.ReadAllText("Temp/Compiled-" + compiledShaderInfo.shaderName.Replace("/", "-") + ".shader");
            if (tempShaderText == string.Empty)
            {
                throw new ArgumentNullException("tempShaderText");
            }

            // Subshader
            MatchCollection subShaderMatches = Regex.Matches(tempShaderText, @"SubShader\s*\{(?>[^\{\}]+|\{(?<DEPTH>)|\}(?<-DEPTH>))*(?(DEPTH)(?!))\}");
            compiledShaderInfo.subShaderList = new List<SubShader>();
            foreach (Match subShaderMatch in subShaderMatches)
            {
                if (subShaderMatch.Success)
                {
                    // Pass
                    List<Pass> passList = new List<Pass>();
                    MatchCollection passMatches = Regex.Matches(subShaderMatch.Groups[0].Value, @"Pass\s*\{(?>[^\{\}]+|\{(?<DEPTH>)|\}(?<-DEPTH>))*(?(DEPTH)(?!))\}");
                    foreach (Match passMatch in passMatches)
                    {
                        if (passMatch.Success)
                        {
                            // Variant
                            List<Variant> variantList = new List<Variant>();
                            string[] splitShader = Regex.Split(passMatch.Groups[0].Value, "shader for \"");
                            for (int i = 0; i < splitShader.Length / 2; i++)    // index-0 is deprecated
                            {
                                variantList.Add(new Variant(splitShader[i + 1], splitShader[2 * (i + 1)]));
                            }
                            passList.Add(new Pass(variantList));
                        }
                    }
                    compiledShaderInfo.subShaderList.Add(new SubShader(passList));
                }
            }
            return compiledShaderInfo;
        }

        public int GetInstruction()
        {
            int count = 0;
            foreach(var subShader in subShaderList)
            {
                count += subShader.GetVertexInstruction();
                count += subShader.GetFragmentInstruction();
            }
            return count;
        }
    }

#pragma warning restore CS0436 // Type conflicts with imported type

}