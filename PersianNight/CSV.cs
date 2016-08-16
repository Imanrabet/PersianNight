using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace PersianNight
{
	//================================================================================
	/// <summary>CSVパーサ</summary>
	//================================================================================
	public class CSV
	{
		/// <summary>接続ファイル名</summary>
		private string m_szFilePath = "";
		/// <summary>標準デリミタ</summary>
		private string m_szDelimiter = ",";
		/// <summary>コメントトークン</summary>
		private string[] m_szCommentTokens;
		/// <summary>データテンポラリ</summary>
		private List<string[]> m_list_rows = new List<string[]>();
		/// <summary>カラムヘッダ</summary>
		private string[] m_array_szColumnHeader;
		/// <summary>データをインポート済みかどうか</summary>
		private bool m_bLoad = false;
		/// <summary>カラム数</summary>
		private int m_nColumns = 0;
		/// <summary>行数</summary>
		public int nRows { get { return m_list_rows.Count; } }
		//================================================================================
		//	setter
		//================================================================================
		/// <summary>標準デリミタ</summary>
		public void SetDelimiter(string sz) { m_szDelimiter = sz; }
		/// <summary>コメントトークン</summary>
		public void SetCommentToken(string[] szTokens) { m_szCommentTokens = szTokens; }
		//================================================================================
		//	
		//================================================================================
		public CSV()
		{

		}
		//================================================================================
		/// <summary>CSVファイルの読み込み(エンコードを指定しない場合はSHIFT-JIS)</summary>
		//================================================================================
		public bool ImportCSV(string szFilePath)
		{
			return ImportCSV(szFilePath, Encoding.GetEncoding("SHIFT_JIS"));
		}
		//================================================================================
		/// <summary>CSVファイルの読み込み</summary>
		/// <param name="szFilePath">ファイルパス</param>
		/// <param name="enc">文字エンコード</param>
		/// <returns>読み込みの成否</returns>
		//================================================================================
		public bool ImportCSV(string szFilePath, Encoding enc)
		{
			//	ファイルの存在チェック
			if (!File.Exists(szFilePath)) return false;
			m_szFilePath = szFilePath;
			//	CSVパーサのインスタンス化と初期化
			TextFieldParser tfp = new TextFieldParser(m_szFilePath, enc);
			tfp.TextFieldType = FieldType.Delimited;
			tfp.SetDelimiters(",");
			//	コメントトークンが設定されている場合は反映
			if (m_szCommentTokens != null) tfp.CommentTokens = m_szCommentTokens;
			//	読み込み準備
			string[] szField;
			bool bHead = false;
			m_nColumns = 0;
			m_list_rows.Clear();
			//	読み込み実行
			while (!tfp.EndOfData)
			{
				//	現在行読み込み
				szField = tfp.ReadFields();
				//	先頭カラムが#ならその行は無視する
				if (szField[0] == "#") continue;
				//	先頭カラムが0ならその行は無視する
				if (szField[0] == "0") continue;
				//	ヘッダ処理
				if (bHead == false)
				{
					bHead = true;
					m_nColumns = szField.Length;
					m_array_szColumnHeader = szField;
					continue;
				}
				m_list_rows.Add(szField);
			}
			m_bLoad = true;
			return true;
		}
		//================================================================================
		/// <summary>読み込んだデータの取得</summary>
		/// <param name="nRowIndex">行Index。行頭は取り込まれていないので、0からデータが入っている</param>
		/// <param name="nColumnIndex">カラムIndex</param>
		/// <returns>データを読み込んでいないとnullを返す</returns>
		//================================================================================
		public string GetData(int nRowIndex, int nColumnIndex)
		{
			if (m_bLoad == false) return null;
			if (nRowIndex < 0 || nRowIndex >= m_list_rows.Count) return null;
			if (nColumnIndex < 0 || nColumnIndex >= m_nColumns) return null;
			return m_list_rows[nRowIndex][nColumnIndex];
		}
		//================================================================================
		/// <summary>読み込んだデータの取得</summary>
		/// <param name="nRowIndex">行Index。行頭は取り込まれていないので、0からデータが入っている</param>
		/// <param name="szColumnHeader">カラム名称</param>
		/// <returns>データを読み込んでいないとnullを返す</returns>
		//================================================================================
		public string GetData(int nRowIndex, string szColumnHeader)
		{
			int n = -1;
			for (int i = 0; i < m_array_szColumnHeader.Length; i++)
			{
				if (m_array_szColumnHeader[i] == szColumnHeader)
				{
					n = i;
					break;
				}
			}
			if (n == -1) return null;
			return GetData(nRowIndex, n);
		}
		public int GetDataInt(int nRowIndex, string szColumnHeader)
		{
			string sz = GetData(nRowIndex, szColumnHeader);
			if (sz == null || sz == "") return 0;
			return int.Parse(sz);
		}
		public float GetDataFloat(int nRowIndex, string szColumnHeader)
		{
			string sz = GetData(nRowIndex, szColumnHeader);
			if (sz == null || sz == "") return float.MinValue;
			return float.Parse(sz);
		}
		//================================================================================
		/// <summary>行インデックスの取得</summary>
		/// <param name="szColumnHeader">カラム名の指定</param>
		/// <param name="szValue">指定したカラムで検索する値</param>
		/// <returns>行インデックス。失敗時は-1</returns>
		//================================================================================
		public int GetIndex(string szColumnHeader,string szValue)
		{
			int n = -1;
			for (int i = 0; i < m_array_szColumnHeader.Length; i++)
			{
				if (m_array_szColumnHeader[i] == szColumnHeader)
				{
					n = i;
					break;
				}
			}
			if (n == -1) return -1;
			for (int i = 0; i < m_list_rows.Count; i++){
				if (szValue == GetData(i, n)) return i;
			}
			return -1;
		}
	}
}
