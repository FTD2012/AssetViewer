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
        public static string[] DurationSizeStr = { "[0 - 5]", "(5 - 10]", "(10 - 100]", "100 - ..." };

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