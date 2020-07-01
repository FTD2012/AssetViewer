using EditorCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

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
        // @Note1: "When Unity tries to render an object, it will use the first SubShader
        //          block that contains a shader that the user's machine can run. ... If
        //          were writing a game that could run onmultiple platforms, you could write
        //          custom shaders that were specially tailored to each of you paltforms.
        //          Supporting multip SubShader blocks is how Unity provides that flexiblity."
        //          --《Practical Shader Development: Vertex and Fragment Shaders for Game》P302
        // @Note2: "Define one or more subshader: the first compatiable one with graphic card in
        //          internal use will be run."
        //          -- 《2D to VR with Unity5 and Google Cardboard》CHAPTER 15 Introduce to Shaders
        public int SubShader;
        public int Sample;
        public string RenderType;
        public List<CompiledShaderInfo> CompiledShaderInfoList = new List<CompiledShaderInfo>();

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

            ShaderUtil.OpenCompiledShader(shader, (int)ShaderPlatformModes.Custom, 1 << (int)ShaderUtil.ShaderCompilerPlatformType.D3D11, false);
            CompiledShaderInfo compiledShaderInfo = CompiledShaderInfo.CreateCompiledShaderInfo(shaderText);

            shaderInfo.Path = assetPath;
            shaderInfo.MaxLOD = ShaderUtil.GetLOD(shader);
            shaderInfo.Variant = ShaderUtil.GetVariantCount(shader, true);
            shaderInfo.Property = ShaderUtil.GetPropertyCount(shader);
            shaderInfo.RenderQueue = shader.renderQueue;
            shaderInfo.Pass = compiledShaderInfo.GetPass();
            shaderInfo.Instruction = compiledShaderInfo.GetInstruction();
            shaderInfo.SubShader = compiledShaderInfo.GetSubShaderCount();
            shaderInfo.Sample = compiledShaderInfo.GetSample();
            shaderInfo.RenderType = compiledShaderInfo.GetRenderType();
            shaderInfo.CompiledShaderInfoList.Add(compiledShaderInfo);
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
        private static readonly string DefaultRenderType = "Opaque(default)";

        public struct Variant
        {
            // List<string> Name;
            public string Vertex;
            public string Fragment;
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

            public int GetVertextSample()
            {
                return GetSample(Vertex);
            }

            public int GetInstructionSample()
            {
                return GetSample(Fragment);
            }

            // @Note: Not all sample* instructon are suitable for all shader(e.g. vertex,hull,fragment)
            // @Reference: https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/shader-model-5-assembly--directx-hlsl-
            private int GetSample(string text)
            {
                MatchCollection matches = Regex.Matches(text, @"\b(sample|sample_b|sample_c|sample_c_lz|sample_d|sample_l|sampleinfo|samplepos)\b");
                return matches.Count;
            }

        }

        public struct Pass
        {
            // public string Tags;
            // public bool ZWrite;
            // public string Cull;
            // public string Blend;
            // public string ColorMask;
            public List<Variant> VariantList;
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

            public int GetVertexSample()
            {
                int max = 0;
                foreach (var variant in VariantList)
                {
                    max = Math.Max(max, variant.GetVertextSample());
                }
                return max;
            }

            public int GetFragmentSample()
            {
                int max = 0;
                foreach (var variant in VariantList)
                {
                    max = Math.Max(max, variant.GetInstructionSample());
                }
                return max;
            }
        }

        public struct Tags
        {
            // Suppose to be one of Transparent,Opaque
            public string RenderType; 
            public Tags(string renderType)
            {
                RenderType = renderType;
            }
        }

        public struct SubShader
        {
            public Tags Tags;
            public List<Pass> PassList;
            public SubShader(List<Pass> passList, Tags tags)
            {
                Tags = tags;
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

            public int GetVertexSample()
            {
                int count = 0;
                foreach (var pass in PassList)
                {
                    count += pass.GetVertexSample();
                }
                return count;
            }

            public int GetFragmentSample()
            {
                int count = 0;
                foreach (var pass in PassList)
                {
                    count += pass.GetFragmentSample();
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
                    // Tags
                    string renderType = DefaultRenderType;
                    Match tagsMatch = Regex.Match(subShaderMatch.Groups[0].Value, @"Tags\s*\{(?>[^\{\}]+|\{(?<DEPTH>)|\}(?<-DEPTH>))*(?(DEPTH)(?!))\}");
                    if (tagsMatch.Success)
                    {
                        // RenderType
                        Match renderTypeMatch = Regex.Match(tagsMatch.Groups[0].Value, "\"RenderType\"=\"(?>[^\"\"]+|\"(?<DEPTH>)|\"(?<-DEPTH>))*(?(DEPTH)(?!))\"");
                        if (renderTypeMatch.Success)
                        {
                            renderType = Regex.Match(renderTypeMatch.Groups[0].Value, "\"RenderType\"=\"(.*)\"").Result("$1") ?? DefaultRenderType;
                        }
                    }

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
                                variantList.Add(new Variant(splitShader[2 * i + 1], splitShader[2 * (i + 1)]));
                            }
                            passList.Add(new Pass(variantList));
                        }
                    }
                    compiledShaderInfo.subShaderList.Add(new SubShader(passList, new Tags(renderType)));
                }
            }
            return compiledShaderInfo;
        }

        public int GetInstruction()
        {
            int count = 0;
            foreach(var subShader in subShaderList)
            {
                count = Math.Max(count, subShader.GetVertexInstruction() + subShader.GetFragmentInstruction());
            }
            return count;
        }

        public int GetSubShaderCount()
        {
            return subShaderList.Count;
        }

        public int GetPass()
        {
            int count = 0;
            foreach (var subShader in subShaderList)
            {
                count = Math.Max(count, subShader.PassList.Count);
            }
            return count;
        }

        public int GetSample()
        {
            int count = 0;
            foreach (var subShader in subShaderList)
            {
                count = Math.Max(count, subShader.GetVertexSample() + subShader.GetFragmentSample());
            }
            return count;
        }

        public string GetRenderType()
        {
            if (subShaderList.Count > 0)
            {
                return subShaderList[0].Tags.RenderType;
            }
            else 
            {
                return "None";
            }
        }

    }

#pragma warning restore CS0436 // Type conflicts with imported type

}