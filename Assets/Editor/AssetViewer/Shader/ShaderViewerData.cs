using UnityEditor;
using System;

namespace AssetViewer
{
    public class ShaderViewerData : ViewerData
    {
        /// Don't modify variable name
        public int MaxLOD;
        public string MaxLODStr;
        public int RenderQueue;
        public string RenderQueueStr;
        public int Pass;
        public int Instruction;
        public string InstructionStr;
        public ulong Variant;
        public int Property;
        public int SubShader;
        public int Sample;
        public string RenderType;

        private ShaderViewerMode _mode;

        public ShaderViewerData(string mode, ShaderInfo shaderInfo)
        {
            _mode = (ShaderViewerMode)Enum.Parse(typeof(ShaderViewerMode), mode);
            MaxLOD = ViewerConst.GetLodIndex(shaderInfo.MaxLOD);
            MaxLODStr = ViewerConst.LoadSizeStr[MaxLOD];
            RenderQueue = ViewerConst.GetRenderQueueIndex(shaderInfo.RenderQueue);
            RenderQueueStr = ViewerConst.RenderQueueStr[RenderQueue];
            Pass = shaderInfo.Pass;
            Instruction = ViewerConst.GetInstructionIndex(shaderInfo.Instruction);
            InstructionStr = ViewerConst.InstructionSizeStr[Instruction];
            Variant = shaderInfo.Variant;
            Property = shaderInfo.Property;
            SubShader = shaderInfo.SubShader;
            Sample = shaderInfo.Sample;
            RenderType = shaderInfo.RenderType;
        }

        public override bool IsMatch(BaseInfo shaderInfo)
        {
            return isMatch((ShaderInfo)shaderInfo);
        }


        private bool isMatch(ShaderInfo shaderInfo)
        {
            switch (_mode)
            {
                case ShaderViewerMode.MaxLOD:
                    return MaxLOD == ViewerConst.GetLodIndex(shaderInfo.MaxLOD);
                case ShaderViewerMode.RenderQueue:
                    return RenderQueue == ViewerConst.GetRenderQueueIndex(shaderInfo.RenderQueue);
                case ShaderViewerMode.Pass:
                    return Pass == shaderInfo.Pass;
                case ShaderViewerMode.Instruction:
                    return Instruction == ViewerConst.GetInstructionIndex(shaderInfo.Instruction);
                case ShaderViewerMode.Variant:
                    return Variant == shaderInfo.Variant;
                case ShaderViewerMode.Property:
                    return Property == shaderInfo.Property;
                case ShaderViewerMode.SubShader:
                    return SubShader == shaderInfo.SubShader;
                case ShaderViewerMode.Sample:
                    return Sample == shaderInfo.Sample;
                case ShaderViewerMode.RenderType:
                    return RenderType == shaderInfo.RenderType;
            }
            return false;
        }

        public override int GetMatchHealthCount(object obj)
        {
            int count = 0;

            foreach (ShaderInfo shaderInfo in _object)
            {
                switch (_mode)
                {
                    case ShaderViewerMode.Sample:
                        count += shaderInfo.Sample > (int)obj ? 1 : 0;
                        break;
                    case ShaderViewerMode.RenderType:
                        count += shaderInfo.RenderType == (string)obj ? 1 : 0;
                        break;
                    case ShaderViewerMode.Pass:
                        count += shaderInfo.Pass > (int)obj ? 1 : 0;
                        break;
                    case ShaderViewerMode.Instruction:
                        count += shaderInfo.Instruction > (int)obj ? 1 : 0;
                        break;
                    case ShaderViewerMode.Variant:
                        count += (int)shaderInfo.Variant > (int)obj ? 1 : 0;
                        break;
                    case ShaderViewerMode.MaxLOD:
                        count += shaderInfo.MaxLOD > (int)obj ? 1 : 0;
                        break;
                    case ShaderViewerMode.Property:
                        count += shaderInfo.Property > (int)obj ? 1 : 0;
                        break;
                    case ShaderViewerMode.SubShader:
                        count += shaderInfo.SubShader > (int)obj ? 1 : 0;
                        break;
                }
            }
            return count;
        }

        public override void AddObject(BaseInfo shaderInfo)
        {
            addObject((ShaderInfo)shaderInfo);
        }

        private void addObject(ShaderInfo shaderInfo)
        {
            _object.Add(shaderInfo);
            Count++;
        }

    }
}