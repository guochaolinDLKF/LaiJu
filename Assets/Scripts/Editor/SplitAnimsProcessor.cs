using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/** 
根据时间表分离骨骼动画. 
时间表名字与模型一致.但后缀是txt. 
模型须以@开头.如 @Model.fbx,时间表如:@Model.txt. 
时间表内容,以 "//"开头的行不处理. 以 ";" 分隔. 
实例：动画名;开始帧;结束帧
*/ 
public class SplitAnimsProcessor : AssetPostprocessor 
{
	// 消息文字
	static readonly string STR_TIME_FILE_NOT_EXIST = "时间文件不存在。";  
	static readonly string STR_SUCC_SPLIT_ANIMATION = "成功分离动画：";
	// 时间文件的后缀 
	public const string EXTENSION_NAME = ".txt";  
	// 每行的分割符号
	public const char SPLIT_SYMBOL = ';'; 

	public void OnPreprocessModel()  
	{
        string dirPath = Directory.GetParent(assetPath).ToString();
        string fileName = Path.GetFileNameWithoutExtension(assetPath);
        string path = dirPath + "/@" + fileName + EXTENSION_NAME;
        if (File.Exists(path))
        {
            Debug.Log(path);
            //读取 动画时间文件,并分离动画  
            SplitAnims(ReadTimeConfig(path));
        }
	} 

	/// <summary>  
	/// 读取 时间配置文件.  
	/// </summary>  
	/// <returns></returns>  
	private List<string> ReadTimeConfig(string path)  
	{  
		List<string> lines = new List<string>();
        FileStream fs = new FileStream(path, FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine();
            if (!string.IsNullOrEmpty(line) && !line.StartsWith("//"))
            {
                lines.Add(line);
            }
        } // end while
        sr.Close();
        fs.Close();
        return lines;
	}  
	/// <summary>  
	/// 分离动画  
	/// </summary>  
	/// <param name="lines"></param>  
	private void SplitAnims(List<string> lines) 
	{  
		string result = STR_TIME_FILE_NOT_EXIST;
		if (lines != null)
		{
			ModelImporter importer = assetImporter as ModelImporter;
			List<ModelImporterClipAnimation> clips = new List<ModelImporterClipAnimation>();
			foreach (string line in lines)
			{  
				string[] timeLine = line.Split(SPLIT_SYMBOL);
				string name = timeLine[0];
				int start = int.Parse(timeLine[1]);
				int end = int.Parse(timeLine[2]);
                bool isLoop = false;
                if (timeLine[3].Equals("1"))
                {
                    isLoop = true;
                }
				//开始裁剪动画
                clips.Add(GenAnim(name, start, end, isLoop));
			}
			importer.clipAnimations = clips.ToArray();
			result = STR_SUCC_SPLIT_ANIMATION + clips.Count;
		}  
		Debug.Log(result);
	}  
	
	/// <summary>  
	/// 创建 动画剪辑.  
	/// </summary>  
	/// <param name="name"></param>  
	/// <param name="startFrame"></param>  
	/// <param name="endFrame"></param>  
	/// <returns></returns>  
	private ModelImporterClipAnimation GenAnim(string name,  int startFrame,  int endFrame,bool isLoop)  
	{  
		ModelImporterClipAnimation clip = new ModelImporterClipAnimation();
		clip.firstFrame = startFrame;
		clip.lastFrame = endFrame;
		clip.name = name;

        clip.lockRootRotation = true;
        clip.lockRootHeightY = true;
        clip.lockRootPositionXZ = true;

        clip.keepOriginalOrientation = true;
        clip.keepOriginalPositionXZ = true;
        clip.keepOriginalPositionY = true;

        clip.loopTime = isLoop;

		return clip;
	}

}// end class
