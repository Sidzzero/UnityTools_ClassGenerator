using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
namespace ZDIS_Unity.Tool
{

    /// <summary>
    /// A Abstract base class used for generating a 
    /// basic layout of any class!
    /// </summary>
    public abstract class GenClass
    {
        /// <summary>
        /// Name of thr Code Generated Class
        /// </summary>
        protected string m_strClassName = "ZDIS_ClassGenerated";
        /// <summary>
        /// Name of the Class File Name
        /// </summary>
        protected string m_strFileName = string.Empty;
        /// <summary>
        /// Name of dir to Save
        /// </summary>
        protected string m_strDirectoryLocation = string.Empty;
        /// <summary>
        /// Namespace to put the Class in!
        /// </summary>
        protected string m_strNameSpace = string.Empty;

        /// <summary>
        /// Use to add Header files inside your class
        /// </summary>
        /// <param name="a_refBuilder"></param>
        protected virtual void CreateHeaderFiles(ref StringBuilder a_refBuilder)
        {
            a_refBuilder.AppendLine("// ----- ZDIS_Unity_Tool:AUTO GENERATED CODE ----- //");
            a_refBuilder.AppendLine("// ----- Created through ClassGenerator ----- //");
            a_refBuilder.AppendLine("// ----- Don't edit this class ----- //");
            a_refBuilder.AppendLine("using System;");
            a_refBuilder.AppendLine("using System.Collections.Generic;");
            a_refBuilder.AppendLine("using System.Reflection;");
            a_refBuilder.AppendLine("using UnityEngine;");
            a_refBuilder.AppendLine("/*---------------------------------------------------*/");

        }

        /// <summary>
        /// AddNamespaces at the Begining
        /// </summary>
        /// <param name="a_refBuilder"></param>
        private void StartNamespace(ref StringBuilder a_refBuilder)
        {
            if (string.IsNullOrEmpty(GetNameSpace()))
            {
                return;
            }
            a_refBuilder.AppendLine("namespace " + GetNameSpace() + " {");
        }

        /// <summary>
        /// Adds closing Bracket for Namespace
        /// </summary>
        /// <param name="a_refBuilder"></param>
        private void EndNamespace(ref StringBuilder a_refBuilder)
        {
            if (string.IsNullOrEmpty(GetNameSpace()))
            {
                return;
            }
            a_refBuilder.AppendLine("}");
        }
        /// <summary>
        /// Incase you want to declare Other class and enums but inside same namespace...
        /// </summary>
        protected virtual void CreateOutsideDefinition()
        {

        }

        /// <summary>
        /// Creates the Declaration line for the class
        /// EG.{public static class} GettClassName() ,where
        /// the content inside curly bracket can change!
        /// </summary>
        /// <returns></returns>
        protected virtual string CreateClassDefinition()
        {
            return ("public  class " + GetClassName());
        }


        /// <summary>
        /// The actually functionwhich creates the Class
        /// </summary>
        /// <param name="a_refBuilder"></param>
        public void CreateClass(ref StringBuilder a_refBuilder)
        {
            CreateHeaderFiles(ref a_refBuilder);

            StartNamespace(ref a_refBuilder);

            CreateOutsideDefinition();

            a_refBuilder.AppendLine(CreateClassDefinition());
            a_refBuilder.AppendLine("{");

            CreateBody(ref a_refBuilder);

            a_refBuilder.AppendLine("}");

            EndNamespace(ref a_refBuilder);

        }

        /// <summary>
        /// This is responsible for creating the Body of the class
        /// </summary>
        /// <param name="a_refBuilder"></param>
        protected virtual void CreateBody(ref StringBuilder a_refBuilder)
        {
            a_refBuilder.AppendLine("//Hi you did it!");
        }

        /// <summary>
        /// Logic to do on class creation fail!
        /// </summary>
        /// <param name="exp"></param>
        public virtual void OnCreationFail(Exception exp)
        {
            Debug.LogError("[GenClass]:Creation of Class FaileD!,Expection is:" + exp);

            if (File.Exists(GetFilePath()) == true)
            {
                File.Delete(GetFilePath());
                Debug.LogError("File Created By Mistake,:)..Deleted also...");
            }

        }


        /// <summary>
        /// This function responsible for creating the File
        /// </summary>
        /// <param name="a_bClearPrevContent">overwrite previous Contents!</param>
        /// <returns></returns>
        public virtual bool CreateFile(bool a_bClearPrevContent = true)
        {
            bool temp_bResult = false;
            string temp_strFileFullPath = string.Empty;

            if (string.IsNullOrEmpty(GetFileName()) || string.IsNullOrEmpty(GetFileDirectory()) || string.IsNullOrEmpty(GetClassName()))
            {
                Debug.LogError("[GenClass]Cannot Create Class: file name or directory location or Class name is empty or null");
                return temp_bResult;
            }

            temp_strFileFullPath = System.IO.Path.Combine(Application.dataPath, GetFileDirectory());
            //CReate or Check Directory
            if (!Directory.Exists(temp_strFileFullPath))
            {
                Directory.CreateDirectory(temp_strFileFullPath);

            }
            string temp_strFileNameFullPath = Path.Combine(temp_strFileFullPath, GetFileName());
            //Delete file if its exists
            if (File.Exists(temp_strFileNameFullPath) && a_bClearPrevContent)
            {
                System.IO.File.WriteAllText(temp_strFileNameFullPath, string.Empty);//Clear contents
                temp_bResult = true;
            }
            else
            {
                try
                {
                    File.Create(temp_strFileNameFullPath).Dispose();
                    temp_bResult = true;
                }
                catch (Exception exp)
                {
                    temp_bResult = false;
                    Debug.LogError("[GenClass]:Creating file!:" + exp);
                }


            }

            return temp_bResult;
        }


        //..These abstract function can be used so that we dont actually need a param constructor
        //for the inherited one ,even though one can do the same
        //Returns the full path of the with File Name

        /// <summary>
        /// Retuns the Namespace for this Class
        /// </summary>
        /// <returns></returns>
        public abstract string GetNameSpace();

        /// <summary>
        /// Gets complete the file path for this class.
        /// It uses other abstracts to stich and create the Name!
        /// </summary>
        /// <returns></returns>
        public virtual string GetFilePath()
        {
            string temp_strFileFullPath = System.IO.Path.Combine(Application.dataPath, GetFileDirectory());
            string temp_strFileNameFullPath = Path.Combine(temp_strFileFullPath, GetFileName());
            return temp_strFileNameFullPath;
        }

        /// <summary>
        /// REturns the FileName for the class
        /// </summary>
        /// <returns></returns>
        public abstract string GetFileName();

        /// <summary>
        /// Returns the Directory location of the class
        /// </summary>
        /// <returns></returns>
        public abstract string GetFileDirectory();
        /// <summary>
        /// Get the Specified Class Name
        /// </summary>
        /// <returns></returns>
        public abstract string GetClassName();

    }


    /// <summary>
    /// A example class to show how Genclass should be inherted 
    /// and used!
    /// </summary>
    public class SampleGenClass : GenClass
    {
        public SampleGenClass(string a_strClassName, string a_strFileName, string a_strDirectoryLocation)
        {
            m_strClassName = a_strClassName;
            m_strDirectoryLocation = a_strDirectoryLocation;
            m_strFileName = a_strFileName;
        }

        public override string GetClassName()
        {
            return m_strClassName;
        }
        public override string GetFileDirectory()
        {
            return m_strDirectoryLocation;
        }
        public override string GetFileName()
        {
            return m_strFileName;
        }
        public override string GetFilePath()
        {
            string temp_strFileFullPath = System.IO.Path.Combine(Application.dataPath, GetFileDirectory());
            string temp_strFileNameFullPath = Path.Combine(temp_strFileFullPath, GetFileName());
            return temp_strFileNameFullPath;
        }

        public override string GetNameSpace()
        {
            return m_strNameSpace;
        }
    }



    /// <summary>
    /// Helper class to create Gen Class!
    /// </summary>
    public class ClassGenerator
    {
        #region Helper_func
        //some utils functions...


        public static string WrapToString(string a_strContent)
        {
            return string.Format(" \" {0} \" ", a_strContent);
        }
        public static string WrapToString(int a_strContent)
        {
            return string.Format(" \" {0} \" ", a_strContent);
        }
        public static string WrapToString(float a_strContent)
        {
            return string.Format(" \" {0} \" ", a_strContent);
        }
        #endregion Helper_func

        #region CREATION_FUNC

        #endregion CREATION_FUNC
        /// <summary>
        /// This functions creates the File as specified by the GenClass!
        /// Use this to create Any time of Gen class
        /// </summary>
        /// <param name="a_refGenClass"></param>
        public static void CreateGenClass(GenClass a_refGenClass)
        {
            //Do the file creation logic here?
            if (a_refGenClass.CreateFile() == false)
            {
                Debug.LogError("[ClassGenerator]:Creating class failed!!:");
                return;
            }


            StringBuilder builder = null;
            StreamWriter writer = null;
            FileStream stream = null;
            try
            {
                var readMode = FileMode.OpenOrCreate;
                stream = File.Open(a_refGenClass.GetFilePath(), readMode, FileAccess.Write);

                builder = new StringBuilder();
                writer = new StreamWriter(stream);

                a_refGenClass.CreateClass(ref builder);

                writer.Write(builder.ToString());

            }
            catch (Exception exp)
            {
                a_refGenClass.OnCreationFail(exp);
                Debug.LogError("[ClassGenerator]:Exception foound...Creating class failed!!:");
            }
            Debug.Log("[ClassGenerator]:Class has been sucessfully generated!");



            writer.Close();
            stream.Close();

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif

        }
    }


}