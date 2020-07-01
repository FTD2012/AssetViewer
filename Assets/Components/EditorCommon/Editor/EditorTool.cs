using UnityEditor;
using UnityEngine;
using System.IO;
using LitJson;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace EditorCommon
{
    public static class EditorTool
    {
        // shader instruction cost.md
        public enum Instruction
        {
            // Unity api
            UnityObjectToClipPos,
            UnityObjectToViewPos,
            UnityObjectToWorldNormal,
            ComputeScreenPos,
            COMPUTE_EYEDEPTH,
            // common cg/hlsl/glsl api
            abs,
            saturate,
            floor,
            frac,
            ceil,
            dot,
            min,
            max,
            sin,
            cos,
            tan,
            sincos,
            ddx,
            ddy,
            sqrt,
            rsqrt,
            clamp,
            exp,
            log,
            log10,
            cross,
            length,
            faceforward,
            step,
            lerp,
            round,
            any,
            pow,
            sign,
            distance,
            normalize,
            reflect,
            all,
            fmod,
            fwidth,
            mul,
            transpose,
            smoothstep,
            asin,
            acos,
            atan,
            atan2,
            tex2D
        };

        public static List<object> ToObjectList<T>(List<T> data)
        {
            if (data == null) return null;
            List<object> ret = new List<object>();
            for (int i = 0; i < data.Count; ++i)
            {
                ret.Add(data[i]);
            }
            return ret;
        }

        public static string GetCurrentBuildPlatform()
        {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                return EditorConst.PlatformAndroid;
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                return EditorConst.PlatformIos;
            }
            else
            {
                return EditorConst.PlatformAndroid;
            }
        }

        public static TextureImporterFormat GetPlatformTextureSettings(TextureImporter textureImporter, string platform)
        {
            if (textureImporter == null)
            {
                Debug.LogError("Invalid texture importer.");
                return TextureImporterFormat.Automatic;
            }

            if (platform == string.Empty)
            {
                Debug.LogError("Invlid platform.");
                return TextureImporterFormat.Automatic;
            }

            TextureImporterFormat textureImporterFormat = textureImporter.GetPlatformTextureSettings(platform).format;
            return (textureImporterFormat == TextureImporterFormat.Automatic) ? textureImporter.GetAutomaticFormat(platform) : textureImporterFormat;
        }

        public static bool IsTextureOverriden(TextureImporter textureImporter, string platform)
        {
            if (textureImporter == null)
            {
                Debug.LogError("Invalid texture importer.");
                return false;
            }

            if (platform == string.Empty)
            {
                Debug.LogError("Invlid platform.");
                return false;
            }

            return textureImporter.GetPlatformTextureSettings(platform).overridden;
        }

        // https://docs.unity3d.com/2017.4/Documentation/ScriptReference/TextureFormat.html
        public static float GetBitsPerPixel(TextureImporterFormat format)
        {
            switch (format)
            {
                case TextureImporterFormat.Alpha8:      // Alpha-only texture format.
                    return 8;
                case TextureImporterFormat.ARGB16:      // A 16 bits/pixel texture format. Texture stores color with an alpha channel.
                    return 16;
                case TextureImporterFormat.RGB24:       // Color texture format, 8-bits per channel.
                    return 24;
                case TextureImporterFormat.RGBA32:      // Color with alpha texture format, 8-bits per channel.
                    return 32;
                case TextureImporterFormat.ARGB32:      // Color with alpha texture format, 8-bits per channel.
                    return 32;
                case TextureImporterFormat.RGB16:       // A 16 bit color texture format.
                    return 16;
                case TextureImporterFormat.DXT1:        // Compressed color texture format.(Maybe: DXT is short for DirectX Texture)
                    return 4;
                case TextureImporterFormat.DXT5:        // Compressed color with alpha channel texture format.
                    return 8;
                case TextureImporterFormat.RGBA16:      // Color and alpha texture format, 4 bit per channel.
                    return 16;
                case TextureImporterFormat.RGBAHalf:    // RGB color and alpha texture format, 16 bit floating point per channel.
                    return 64;
                case TextureImporterFormat.PVRTC_RGB2:  // PowerVR (iOS) 2 bits/pixel compressed color texture format.
                    return 2;
                case TextureImporterFormat.PVRTC_RGBA2: // PowerVR (iOS) 2 bits/pixel compressed with alpha channel texture format
                    return 2;
                case TextureImporterFormat.PVRTC_RGB4:  // PowerVR (iOS) 4 bits/pixel compressed color texture format.
                    return 4;
                case TextureImporterFormat.PVRTC_RGBA4: // PowerVR (iOS) 4 bits/pixel compressed with alpha channel texture format
                    return 4;
                case TextureImporterFormat.ETC_RGB4:    // ETC (GLES2.0) 4 bits/pixel compressed RGB texture format.
                    return 4;
                case TextureImporterFormat.ETC2_RGB4:   // ETC2 (GLES 3.0) 4 bits/pixel compressed RGB texture format.
                    return 4;
                case TextureImporterFormat.ETC2_RGBA8:  // ETC2 (GLES 3.0) 8 bits/pixel compressed RGBA texture format.
                    return 8;
                case TextureImporterFormat.ATC_RGB4:    // ATC (ATITC) 4 bits/pixel compressed RGB texture format.
                    return 4;
                case TextureImporterFormat.ATC_RGBA8:   // ATC (ATITC) 8 bits/pixel compressed RGB texture format.
                    return 8;
                case TextureImporterFormat.ASTC_RGB_4x4:    // ASTC (4x4 pixel block in 128 bits) compressed RGB texture format. 4x4: 8 bits per pixel (64KB for a 256x256 Texture)
                    return 8;
                case TextureImporterFormat.ASTC_RGB_5x5:    // ASTC (5x5 pixel block in 128 bits) compressed RGB texture format. 5x5: 5.12 bits per pixel (43264 bytes for a 256x256 Texture)
                    return 5.12f;
                case TextureImporterFormat.ASTC_RGB_6x6:    // ASTC (6x6 pixel block in 128 bits) compressed RGB texture format. 6x6: 3.56 bits per pixel (29584 bytes for a 256x256 Texture)
                    return 3.56f;
                case TextureImporterFormat.ASTC_RGB_8x8:    // ASTC (8x8 pixel block in 128 bits) compressed RGB texture format. 8x8: 2 bits per pixel(16KB for a 256x256 Texture) ;
                    return 2;
                case TextureImporterFormat.ASTC_RGB_10x10:  // ASTC (10x10 pixel block in 128 bits) compressed RGB texture format. 10x10: 1.28 bits per pixel (10816 bytes for a 256x256 Textures)
                    return 1.28f;
                case TextureImporterFormat.ASTC_RGB_12x12:  // ASTC (12x12 pixel block in 128 bits) compressed RGB texture format. 12x12: 0.89 bits per pixel (7744 bytes for a 256x256 Texture)
                    return 0.89f;
                case TextureImporterFormat.ASTC_RGBA_4x4:    // ASTC (4x4 pixel block in 128 bits) compressed RGBA texture format. 4x4: 8 bits per pixel (64KB for a 256x256 Texture)
                    return 8;
                case TextureImporterFormat.ASTC_RGBA_5x5:    // ASTC (5x5 pixel block in 128 bits) compressed RGBA texture format. 5x5: 5.12 bits per pixel (43264 bytes for a 256x256 Texture)
                    return 5.12f;
                case TextureImporterFormat.ASTC_RGBA_6x6:    // ASTC (6x6 pixel block in 128 bits) compressed RGBA texture format. 6x6: 3.56 bits per pixel (29584 bytes for a 256x256 Texture)
                    return 3.56f;
                case TextureImporterFormat.ASTC_RGBA_8x8:    // ASTC (8x8 pixel block in 128 bits) compressed RGBA texture format. 8x8: 2 bits per pixel(16KB for a 256x256 Texture) ;
                    return 2;
                case TextureImporterFormat.ASTC_RGBA_10x10:  // ASTC (10x10 pixel block in 128 bits) compressed RGBA texture format. 10x10: 1.28 bits per pixel (10816 bytes for a 256x256 Textures)
                    return 1.28f;
                case TextureImporterFormat.ASTC_RGBA_12x12:  // ASTC (12x12 pixel block in 128 bits) compressed RGBA texture format. 12x12: 0.89 bits per pixel (7744 bytes for a 256x256 Texture)
                    return 0.89f;
#pragma warning disable 0618
                case TextureImporterFormat.AutomaticCompressed:
                    return 4;
                case TextureImporterFormat.AutomaticTruecolor:
                    return 32;
                default:
                    return 32;
#pragma warning restore 0618
            }
        }

        public static int CalculateTextureSizeBytes(Texture tTexture, TextureImporterFormat format)
        {
            var tWidth = tTexture.width;
            var tHeight = tTexture.height;
            if (tTexture is Texture2D)
            {
                var tTex2D = tTexture as Texture2D;
                var bitsPerPixel = GetBitsPerPixel(format);
                var mipMapCount = tTex2D.mipmapCount;
                var mipLevel = 1;
                var tSize = 0.0f;
                while (mipLevel <= mipMapCount)
                {
                    tSize += tWidth * tHeight * bitsPerPixel / 8; 
                    tWidth = tWidth / 2;
                    tHeight = tHeight / 2;
                    mipLevel++;
                }
                return (int)tSize;
            }

            if (tTexture is Cubemap)
            {
                var bitsPerPixel = GetBitsPerPixel(format);
                return (int)(tWidth * tHeight * 6 * bitsPerPixel / 8);
            }
            return 0;
        }

        public static int CalculateTextureSizeBytes(string path)
        {
            TextureImporter tImport = AssetImporter.GetAtPath(path) as TextureImporter;
            Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(path);
            if (tImport == null || texture == null) return 0;

            TextureImporterPlatformSettings setting = tImport.GetPlatformTextureSettings(GetCurrentBuildPlatform());

            int retSize = 0;
            if (!setting.overridden)
            {
#pragma warning disable 0618
                retSize = CalculateTextureSizeBytes(texture, tImport.textureFormat);
#pragma warning restore 0618
            }
            else
            {
                retSize = CalculateTextureSizeBytes(texture, setting.format);
            }

            Resources.UnloadAsset(texture);

            return retSize;
        }

        public static int GetInstrcutionCount(Instruction instruction)
        {
            switch(instruction)
            {
                case Instruction.UnityObjectToClipPos:
                    return 4;
                case Instruction.UnityObjectToViewPos:
                    return 8;
                case Instruction.UnityObjectToWorldNormal:
                    return 3;
                case Instruction.ComputeScreenPos:
                    return 2;
                case Instruction.COMPUTE_EYEDEPTH:
                    return 8;
                case Instruction.abs:
                case Instruction.saturate:
                    return 0;
                case Instruction.floor:
                case Instruction.frac:
                case Instruction.ceil:
                case Instruction.dot:
                case Instruction.min:
                case Instruction.max:
                case Instruction.sin:
                case Instruction.cos:
                case Instruction.tan:
                case Instruction.sincos:
                case Instruction.ddx:
                case Instruction.ddy:
                case Instruction.sqrt:
                case Instruction.rsqrt:
                    return 1;
                case Instruction.clamp:
                case Instruction.exp:
                case Instruction.log:
                case Instruction.log10:
                case Instruction.cross:
                case Instruction.length:
                case Instruction.faceforward:
                case Instruction.step:
                case Instruction.lerp:
                    return 2;
                case Instruction.round:
                case Instruction.any:
                case Instruction.pow:
                case Instruction.sign:
                case Instruction.distance:
                case Instruction.normalize:
                case Instruction.reflect:
                    return 3;
                case Instruction.all:
                case Instruction.fmod:
                case Instruction.fwidth:
                case Instruction.mul:
                case Instruction.transpose:
                    return 4;
                case Instruction.smoothstep:
                    return 7;
                case Instruction.acos:
                    return 10;
                case Instruction.asin:
                    return 11;
                case Instruction.atan:
                    return 16;
                case Instruction.atan2:
                    return 22;
                case Instruction.tex2D:
                    return 10;
                default:
                    return 0;
            }
        }

        public static int GetShaderInstruction(string shaderText)
        {
            if (shaderText == string.Empty)
            {
                return 0;
            }
            int count = 0;

            foreach(var instruction in Enum.GetNames(typeof(Instruction)))
            {
                MatchCollection matches = Regex.Matches(shaderText, instruction + @"\s*\(");
                count += GetInstrcutionCount((Instruction)Enum.Parse(typeof(Instruction), instruction)) * matches.Count;
            }

            return count;
        }

        public static int GetRuntimeMemorySize(UnityEngine.Object asset)
        {
#pragma warning disable 0618
            return UnityEngine.Profiling.Profiler.GetRuntimeMemorySize(asset);
#pragma warning restore 0618
        }

        public static int CalculateModelSizeBytes(string path)
        {
            int size = 0;
            UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
            for (int i = 0; i < assets.Length; ++i)
            {
                if (assets[i] is Mesh)
                {
                    size += GetRuntimeMemorySize(assets[i]);
                }
                if ((!(assets[i] is GameObject)) && (!(assets[i] is Component)))
                {
                    Resources.UnloadAsset(assets[i]);
                }
            }
            return size;
        }

        public static int CalculateAnimationSizeBytes(string path)
        {
            int size = 0;
            UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
            for (int i = 0; i < assets.Length; ++i)
            {
                if ((assets[i] is AnimationClip) && assets[i].name != EditorConst.EDITOR_ANICLIP_NAME)
                {
                    size += GetRuntimeMemorySize(assets[i]);
                }
                if ((!(assets[i] is GameObject)) && (!(assets[i] is Component)))
                {
                    Resources.UnloadAsset(assets[i]);
                }
            }
            return size;
        }

        public static void CreateDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static T LoadJsonData<T>(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return default(T);
                }
                string str = File.ReadAllText(path);
                if (string.IsNullOrEmpty(str))
                {
                    return default(T);
                }
                T data = JsonMapper.ToObject<T>(str);
                if (data == null)
                {
                    Debug.LogError("Cannot read json data from " + path);
                }

                return data;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return default(T);
            }
        }

        public static void SaveJsonData<T>(T data, string path)
        {
            CreateDirectory(path);

            string jsonStr = JsonFormatter.PrettyPrint(JsonMapper.ToJson(data));
            File.WriteAllText(path, jsonStr);
        }

        public static bool MouseClickInRect(Rect rect)
        {
            return Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition);
        }

    }
}