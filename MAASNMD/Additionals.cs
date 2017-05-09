using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;


namespace MAASNMD
{
    public class Additionals
    {
        //public static DataTable ReadExcel(string filePath)
        //{
        //    DataTable dtexcel = new DataTable();
        //    bool hasHeaders = false;
        //    string HDR = hasHeaders ? "Yes" : "No";
        //    string strConn;
        //    //if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
        //    if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
        //        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
        //    else
        //        strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
        //    OleDbConnection conn = new OleDbConnection(strConn);
        //    conn.Open();
        //    DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        //    //Looping Total Sheet of Xl File
        //    /*foreach (DataRow schemaRow in schemaTable.Rows)
        //    {
        //    }*/
        //    //Looping a first Sheet of Xl File
        //    DataRow schemaRow = schemaTable.Rows[0];
        //    string sheet = schemaRow["TABLE_NAME"].ToString();
        //    if (!sheet.EndsWith("_"))
        //    {
        //        string query = "SELECT  * FROM [" + sheet + "]";
        //        //string query = "SELECT  * FROM [Лист1]";
        //        OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
        //        dtexcel.Locale = CultureInfo.CurrentCulture;
        //        daexcel.Fill(dtexcel);
        //    }

        //    conn.Close();
        //    return dtexcel;

        //}

        //public void ToExcel(DataTable dt)
        //{
        //    Microsoft.Office.Interop.Excel.Application excel = null;
        //    Microsoft.Office.Interop.Excel.Workbook wb = null;

        //    object missing = Type.Missing;
        //    Microsoft.Office.Interop.Excel.Worksheet ws = null;
        //    Microsoft.Office.Interop.Excel.Range rng = null;

        //    try
        //    {
        //        excel = new Microsoft.Office.Interop.Excel.Application();
        //        wb = excel.Workbooks.Add();
        //        ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.ActiveSheet;

        //        for (int Idx = 0; Idx < dt.Columns.Count; Idx++)
        //        {
        //            ws.Range["A1"].Offset[0, Idx].Value = dt.Columns[Idx].ColumnName; //сохраняет название колонки(необходимо в след цикле поменять а1 на а2)
        //        }

        //        for (int Idx = 0; Idx < dt.Rows.Count; Idx++)
        //        {  // <small>hey! I did not invent this line of code, 
        //            // I found it somewhere on CodeProject.</small> 
        //            // <small>It works to add the whole row at once, pretty cool huh?</small>
        //            ws.Range["A2"].Offset[Idx].Resize[1, dt.Columns.Count].Value =
        //            dt.Rows[Idx].ItemArray;
        //        }

        //        excel.Visible = true;
        //        wb.Activate();
        //    }
        //    catch (COMException ex)
        //    {
        //        MessageBox.Show("Error accessing Excel: " + ex.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error: " + ex.ToString());
        //    }
        //}
        
        //public static double[][] DataTableToMatr(DataTable dt)
        //{
        //    double[][] res = new double[dt.Rows.Count][];
        //    for(int i=0;i<dt.Rows.Count;i++)
        //        res[i] = new double[dt.Columns.Count-1];

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //        for (int j = 0; j < dt.Columns.Count-1; j++)
        //        res[i][j] = (double)Convert.ToDouble(dt.Rows[i][j]);
        //    return res;
        //}

        public static IEnumerable<char[]> Combinations(int length, char startChar, char endChar)
        {
            List<char[]> str = new List<char[]>();
            // Определим общее количество символов, которое будет учавствовать в переборе '0'+'1'+'2'+'3' = 4 символа 
            int numChars = endChar - startChar + 1;  //На самом деле здесь 51-48+1, так как символ '3' имеет порядковый номер 51, а символ '0' - 48 
            // Определим набор первых символов. перменная s содержит массив из трех нулей  '0' '0' '0' - это наш первый вариант перебора
            var s = new String(startChar, length).ToCharArray();
            //Определяем общее количество возможных переборок. Math.Pow(numChars, length) = 4^3(4 в степени 3) = 64 варианта. 
            //Начинаем цикл со второго варианта, так как первый - это 3 нуля(мы их уже определили)
            for (int it = 1; it <= Math.Pow(numChars, length); ++it)
            {
                //Возвратим первый вариант - 3 нуля (ключевое слово yield позволит тебе возвратить результат и дальше продолжить цикл с этого места)
                yield return s;

                //Изменим наш массив символов.
                //С первого по последний символ в массиве запускаем цикл.                
                for (int ix = 0; ix < s.Length; ++ix)
                    //Если символ первый либо последующий в зависимости от номера текущей переборки it 
                    if (ix == 0 || it % Math.Pow(numChars, ix) == 0)
                        //Меняем символ от последнего к первому (char)(....) - по порядковому номеру символа мы получим сам символ :)
                        s[s.Length - 1 - ix] = (char)(startChar + (s[s.Length - 1 - ix] - startChar + 1) % numChars);
            }
        }

        public static bool CheckElement(char[] chr)
        {
            bool res = true;
            for (int i = 0; i < chr.Length - 1; i++)
                if ((int)Char.GetNumericValue(chr[i]) > (int)Char.GetNumericValue(chr[i + 1]))
                {
                    res = false;
                    break;
                }
            return res;
        }

        public static string[] BuildIndex(int numOfX, int degree)
        {
            List<string> res = new List<string>();
            res.Add("0");
            for (int d = 1; d < degree + 1; d++)
                foreach (char[] a in Combinations(d, '1', (char)numOfX.ToString()[0]))
                    if (CheckElement(a))
                        res.Add(new string(a));

            string[] s = new string[res.Count];//менял +1
            s[0] = "0";
            for (int i = 0; i < res.Count; i++)
                s[i] = res[i];//менял +1

            return s;

        }

        public static List<int[]> CreateListOfIndex(string[] s)
        {
            List<int[]> res = new List<int[]>();
            for (int i = 0; i < s.Length; i++)
            {
                int[] temp = new int[s[i].Length];
                for (int j = 0; j < s[i].Length; j++)
                    temp[j] = (int)Char.GetNumericValue(s[i][j]);
                res.Add(temp);
                int t = 0;
            }

            return res;
        }


    }
}
