using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace ZDIS_Unity.Tool
{

    public class SampleSettingGen : GenClass
    {
        public override string GetClassName()
        {
            return "SampleSettings";
        }

        public override string GetFileDirectory()
        {
            return "_GeneratedFiles";
        }

        public override string GetFileName()
        {
            return "SampleSettings.cs";
        }

        public override string GetNameSpace()
        {
            return "ZDIS.Game";
        }

        protected override void CreateBody(ref StringBuilder a_refBuilder)
        {
            a_refBuilder.AppendLine("private static int m_iPlayerID = -1;");
            a_refBuilder.AppendLine("private static int m_iMaxCount = 2;");
            a_refBuilder.AppendLine("public static int PlayerID{get{return m_iPlayerID; }}");
        }
    }

    public class SampleUsage : MonoBehaviour
    {
        [ContextMenu("GenerateSampleGen")]
        public void GenerateSampleGen()
        {
            ClassGenerator.CreateGenClass(new SampleSettingGen());
         // ZDIS.Game.SampleSettings.PlayerID //=> see what we  did here...
        }
    }
}