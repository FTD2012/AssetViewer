using EditorCommon;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceFormat
{
    public static class OverviewConfig
    {
        public const string RootPath = "Assets";
        public const string TextureReportPath = "TextureOverviewReport.md";
        public const string ModelReportPath = "ModelOverviewReport.md";

        public const string TextureReportMenu = "UComponents/ResourcesOverview/TextureOverviewReport";
        public const string ModelReportMenu = "UComponents/ResourcesOverview/ModelOverviewReport";
    }

    public static class OverviewTableConst
    {
        public const int VertexCountMod = 1000;
        public const int TriangleCountMod = 1000;

        public static int[] TextureSize = { 64 * 64, 128 * 128, 256 * 256, 512 * 512, 1024 * 1024, 2048 * 2048, 4096 * 4096 };
        public static string[] TextureSizeStr = { "[0 - 64 X 64]", " (64 X 64 - 128 X 128]", "(128 X 128 - 256 * 256]",
                                   "(256 X 256 - 512 X 512]", "(512 X 512 - 1024 X 1024]", "(1024 X 1024 - 2048 X 2048]",
                                    "(2048 X 2048 - 4096 X 4096]", "(4096 X 4096 - ...]"};
        public static string[] TextureTypeStr = { "Default", "NormalMap", "GUI", "Cubemap",
                                      "Cookie", "Advanced", "Lightmap", "Cursor", "Sprite", "HDRI", "SingleChannel"};

        public static string[] MeshDataStr = { "tangent", "normal", "color", "uv4", "uv3", "uv2", "uv" };

        public static int[] ParticleSize = { 10, 100, 1000, 1000, 1000 };
        public static string[] ParticleSizeStr = { "[0 - 10]", "(10 - 100]", "(100 - 1000]", "(1000 - 1000]", "(1000 - 10000]", "(10000 - ...]" };

        public static float[] DurationSize = { 5, 10, 100 };
        public static string[] DurationSizeStr = { "[0 - 5]", "(5 - 10]", "(10 - 100]", "(100 - ..." };

        public static float[] LodSize = { 0, 50, 150, 300 };
        public static string[] LoadSizeStr = { "Unlimited", "[0 - 50]", "(50 - 150]", "(150 - 300]", "(300 - ... ]" };

        public static int[] RenderQueue = { 1000, 2000, 2450, 2500, 3000, 4000 };
        // https://www.jianshu.com/p/0341f0ab9020
        public static string[] RenderQueueStr = {"Not Avaiable [... - 1000)", "BackGroud [1000 - 2000)", "Geometry [2000 - 2450)",
                                                 "AlphaTest [2450 - 2500)", "Geometry [2500 - 3000)", "Transparent [3000 - 4000)",
                                                 "Overlay [4000 - ...]" };

        public static int[] InstructionSize = { 10, 30, 60, 100 };
        public static string[] InstructionSizeStr = { "[0 - 10]", "(10 - 30]", "(30 - 60]", "(60 - 100]", "(100 - ...]" };

        public static int[] VariantSize = { 10, 20, 40, 80 };
        public static string[] VariantSizeStr = { "[0 - 10]", "(10 - 20]", "(20 - 40]", "(40 - 80]", "[80 - ...)" };

        public static int GetTextureSizeIndex(int width, int height)
        {
            int index = 0, size = width * height;
            while (index < TextureSize.Length && size > TextureSize[index])
                ++index;
            return index;
        }

        public static int GetParticleSizeIndex(int size)
        {
            int index = 0;
            while (index < ParticleSize.Length && size > ParticleSize[index])
                ++index;
            return index;
        }

        public static int GetDurationIndex(float size)
        {
            int index = 0;
            while (index < DurationSize.Length && size > DurationSize[index])
                ++index;
            return index;
        }

        public static int GetLodIndex(float size)
        {
            int index = 0;
            while (index < LodSize.Length && size > LodSize[index])
                ++index;
            return index;
        }

        public static int GetRenderQueueIndex(float size)
        {
            int index = 0;
            while (index < RenderQueue.Length && size+1 > RenderQueue[index])
                ++index;
            return index;
        }

        public static int GetInstructionIndex(float size)
        {
            int index = 0;
            while (index < InstructionSize.Length && size > InstructionSize[index])
                ++index;
            return index;
        }
        public static int GetVariantIndex(float size)
        {
            int index = 0;
            while (index < VariantSize.Length && size > VariantSize[index])
                ++index;
            return index;
        }

        public static string GetPath(Transform transform)
        {
            string path = transform.name;
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = transform.name + "/" + path;
            }
            return path;
        }

        public static float LeftWidth = 0.4f;
    }
}