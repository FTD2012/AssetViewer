using UnityEditor;
using System;

namespace ResourceFormat
{
    public class ShaderOverviewData : OverviewData
    {
        /// Don't modify variable name
        public int Count;
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

        private ShaderOverviewMode _mode;

        public ShaderOverviewData(string mode, ShaderInfo shaderInfo)
        {
            _mode = (ShaderOverviewMode)Enum.Parse(typeof(ShaderOverviewMode), mode);
            MaxLOD = OverviewTableConst.GetLodIndex(shaderInfo.MaxLOD);
            MaxLODStr = OverviewTableConst.LoadSizeStr[MaxLOD];
            RenderQueue = OverviewTableConst.GetRenderQueueIndex(shaderInfo.RenderQueue);
            RenderQueueStr = OverviewTableConst.RenderQueueStr[RenderQueue];
            Pass = shaderInfo.Pass;
            Instruction = OverviewTableConst.GetInstructionIndex(shaderInfo.Instruction);
            InstructionStr = OverviewTableConst.InstructionSizeStr[Instruction];
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
                case ShaderOverviewMode.MaxLOD:
                    return MaxLOD == OverviewTableConst.GetLodIndex(shaderInfo.MaxLOD);
                case ShaderOverviewMode.RenderQueue:
                    return RenderQueue == OverviewTableConst.GetRenderQueueIndex(shaderInfo.RenderQueue);
                case ShaderOverviewMode.Pass:
                    return Pass == shaderInfo.Pass;
                case ShaderOverviewMode.Instruction:
                    return Instruction == OverviewTableConst.GetInstructionIndex(shaderInfo.Instruction);
                case ShaderOverviewMode.Variant:
                    return Variant == shaderInfo.Variant;
                case ShaderOverviewMode.Property:
                    return Property == shaderInfo.Property;
                case ShaderOverviewMode.SubShader:
                    return SubShader == shaderInfo.SubShader;
                case ShaderOverviewMode.Sample:
                    return Sample == shaderInfo.Sample;
                case ShaderOverviewMode.RenderType:
                    return RenderType == shaderInfo.RenderType;
            }
            return false;
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