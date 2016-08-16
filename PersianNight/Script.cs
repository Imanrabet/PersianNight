using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PersianNight
{
	//================================================================================
	//	
	//================================================================================
	public class Script
	{
		private List<string> m_list_string;
		private int m_nCurrentIndex = 0;
		/// <summary>スクリプトファイルの読み込みが完了しているかどうか</summary>
		public bool bReadCompleted = false;
		//================================================================================
		//	
		//================================================================================
		public Script()
		{
			m_list_string = new List<string>();
		}
		//================================================================================
		/// <summary>キャレットを先頭に戻す</summary>
		//================================================================================
		public void Reset()
		{
			m_nCurrentIndex = 0;
		}
		//================================================================================
		/// <summary>テキストを追加する</summary>
		/// <param name="szString"></param>
		//================================================================================
		public void AddText(string szString)
		{
			m_list_string.Add(szString);
		}
		//================================================================================
		/// <summary>次のテキストを返す</summary>
		//================================================================================
		public string GetNextText()
		{
			if (m_list_string.Count <= m_nCurrentIndex) return "";
			m_nCurrentIndex++;
			return m_list_string[m_nCurrentIndex - 1];
		}
		//================================================================================
		/// <summary>テキストファイルを読み込む</summary>
		//================================================================================
		public bool ReadTextFile(string szFileName)
		{
			m_list_string.Clear();
			m_nCurrentIndex = 0;
			string szTemp;
			StreamReader sr = null;
			try
			{
				sr = new StreamReader(szFileName, Encoding.GetEncoding("Shift_JIS"));
				szTemp = sr.ReadToEnd();
				sr.Close();
			}
			catch (Exception e)
			{
				if (sr != null) sr.Close();
				return false;
			}
			string[] szArray = szTemp.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < szArray.Length;i++)
			{
				//	空白行は無視
				if (szArray[i] == "") continue;
				//	コメント行は無視
				if (szArray[i][0] == '#') continue;
				//	\nは無視
				szArray[i] = szArray[i].Replace("\\n", "");
				m_list_string.Add(szArray[i]);
			}
			bReadCompleted = true;
			return true;
		}
	}
}
