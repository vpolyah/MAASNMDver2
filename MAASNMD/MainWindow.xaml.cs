using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;
using System.Threading;
using System.Collections.ObjectModel;
using DocumentFormat.OpenXml;
using ClosedXML.Excel;




namespace MAASNMD
{
   
    public partial class MainWindow : Window
    {
        //public DataTable expData = new DataTable();
        //public DataTable matrDT = new DataTable();
        //public double[][] expDataMatr;
        public List<int[]> polynom = new List<int[]>();
        
        public string[] polynomString;
        public double[] regressCoef;
        public int numberOfParameters = 0;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void openData_Click(object sender, RoutedEventArgs e)
        {
            File.Delete("Test.txt");
            File.Delete("Hand_Mode.txt");
            pr.FromFileToList();
            //pr.againFromFile();
            //pr.ExpertValues(1);
            //pr.commonError();

            //раскоментить
            pr.printForStart();

            //infoTextBox.Clear();
            StreamReader rdr = new StreamReader("Hand_Mode.txt", Encoding.UTF8);
            infoTextBox2.Text += rdr.ReadToEnd();
            rdr.Close();
        }

        //private void showData_Click(object sender, RoutedEventArgs e)
        //{
           
        //    infoTextBox.Clear();
        //    infoTextBox.Text += "Информация об экспериментальных данных: \r\n";
        //    infoTextBox.Text += "Количество проведенных экспериментов  -  " + expData.Rows.Count.ToString() + "\r\n";
        //    infoTextBox.Text += "Количество входных параметров  -  " + (expData.Columns.Count - 1).ToString() + "\r\n";
        //}

        private void createPolynom_Click(object sender, RoutedEventArgs e)
        {
            //File.Delete("Test.txt");
            //infoTextBox.Clear();
            //StreamReader rdr = new StreamReader("Test.txt", Encoding.UTF8);
            //infoTextBox.Text += rdr.ReadToEnd();
            //rdr.Close();

            if (polDegreeComboBox.SelectedIndex != -1)
            {
                pr.createPolynom(polDegreeComboBox.SelectedIndex + 1);
                pr.Params_Combi();
                infoTextBox.Clear();
                StreamReader rdr = new StreamReader("Test.txt", Encoding.UTF8);               
                infoTextBox.Text += rdr.ReadToEnd();
                rdr.Close();               
            }
        }

        //private void showSOLE_Click(object sender, RoutedEventArgs e)
        //{
        //    matrDT.Clear();
        //    matrDT = new DataTable();

        //    matrDT.Columns.Add("b0");
        //    for (int i = 0; i < polynomString.Length; i++)
        //        matrDT.Columns.Add("b" + polynomString[i]);
        //    matrDT.Columns.Add("yExp");
        //    matrDT.Columns.Add("yExp1111");

        //    for (int i = 0; i < expData.Rows.Count; i++)
        //    {
        //        DataRow newRow = matrDT.NewRow();

        //        newRow[0] = 1;

        //        for (int j = 0; j < polynomString.Length; j++)
        //        {
        //            double val = 1.0;
        //            for (int g = 0; g < polynom[j].Length; g++)
        //                val *= expDataMatr[i][polynom[j][g] - 1];

        //            newRow[j + 1] = val;
        //            newRow[j + 2] = 33;

        //        }

        //        newRow[polynomString.Length + 1] = expData.Rows[i][expData.Columns.Count - 1];

        //        matrDT.Rows.Add(newRow);

        //    }            

        //}

        //private void solveSOLE1_Click(object sender, RoutedEventArgs e)
        //{

        //    pr.prisvoenie();
        //    for (int j = Global.polynomeNumb; j < Global.polynomeNumb + 1; j++)
        //    {
        //        Global.combinationSize = j/*j*/;
        //        //создание полинома
        //        pr.CombinationMass(Global.polynomeNumb, j);
        //        //количество сочетаний размера j
        //        double combCount = 0;
        //        combCount = function(Global.polynomeNumb, j);
        //        for (int i = 0; i < combCount; i++)
        //        {
        //            pr.polynomeResize(i);
        //            // pr.polynomeResize();
        //            pr.Params_Combi();
                    
        //            //работа с функцией регрессии
        //            pr.function_ready();
        //            pr.regression_func(Global.SOLE);
        //            //Вычисление точности
        //            pr.InAccuracyCalc();
        //            pr.groupListPush();
        //        }
        //    }
        //    pr.commonErrorTo_groupList();
        //    pr.commonError_Hand_Mode();

        //    textbox_1.Clear();
        //    textbox_1.AppendText(Global.polynome_type);
        //    textbox_2.Clear();
        //    textbox_2.AppendText(Global.polynome_type);
        //    textbox_3.Clear();
        //    textbox_3.AppendText(Global.polynome_type);

        //    // операции по выводу текста из файла 
        //    infoTextBox2.Clear();
        //    StreamReader rdp = new StreamReader("Hand_mode.txt", Encoding.UTF8);
        //    infoTextBox2.Text += rdp.ReadToEnd();
        //    rdp.Close();
        //}

        //private void showPolynom_Click(object sender, RoutedEventArgs e)
        //{
        //    DataTable polDT = new DataTable();

        //    for (int i = 0; i < polynomString.Length; i++)
        //        polDT.Columns.Add("b" + polynomString[i]);
        //    DataRow dr = polDT.NewRow();
        //    for (int i = 0; i < polynomString.Length; i++)
        //        dr[i] = polynomString[i];
        //    polDT.Rows.Add(dr);

            //////////////////////////////////////////////////////

        //    infoTextBox.Clear();
        //    infoTextBox.Text += "Информация об созданном полиноме: \r\n";
        //    infoTextBox.Text += "Количество членов в полиноме  -  " + polynomString.Length.ToString() + "\r\n";
        //    for (int i = 0; i < polynomString.Length; i++)
        //        infoTextBox.Text += polynomString[i] + "\r\n";
        //    infoTextBox.Text += "\r\n";
        //    for (int i = 0; i < polynomString.Length - 1; i++)
        //        infoTextBox.Text += polynomString[i] + " + ";
        //    infoTextBox.Text += polynomString[polynomString.Length - 1];
        //}

        //private void addCoeff_Click(object sender, RoutedEventArgs e)
        //{
        //    if (CheckCoeff(coeffB.Text.Trim(), numberOfParameters))
        //    {
        //        bool alreadyExcist = false;

        //        for (int i = 0; i < polynomString.Length; i++)
        //            if (polynomString[i] == coeffB.Text.Trim())
        //            {
        //                alreadyExcist = true;
        //                break;
        //            }

        //        if (!alreadyExcist)
        //        {
        //            string[] newPS = new string[polynomString.Length + 1];
        //            for (int i = 0; i < polynomString.Length; i++)
        //                newPS[i] = polynomString[i];

        //            newPS[newPS.Length - 1] = coeffB.Text.Trim();

        //            polynomString = newPS;
        //            polynom = Additionals.CreateListOfIndex(polynomString);

        //            coeffB.Clear();
        //            showPolynom_Click(sender, e);
        //        }
        //        else MessageBox.Show("Введенный коэффициент уже существует");
        //    }

        //}

        //public static bool CheckCoeff(string s, int num)
        //{
        //    bool res = true;
        //    Regex reg = new Regex(@"^([\d.,-]+)$");

        //    if (!reg.IsMatch(s))
        //    {
        //        MessageBox.Show("Попытка добавить неподходящий индекс.\r\n Индекс должен состоять только из цифр");
        //        return false;
        //    }

        //    for (int i = 0; i < s.Length; i++)
        //        if ((int)Char.GetNumericValue(s[i]) > num)
        //        {
        //            res = false;
        //            MessageBox.Show("Попытка добавить неподходящий индекс.\r\n Индекс должен состоять только из цифр, значения которых не должны превышать количество рассматриваемых входов. В данном случае их - " + num.ToString());
        //            break;
        //        }
        //    return res;
        //}

        //private void delCoeff_Click(object sender, RoutedEventArgs e)
        //{
        //    if (CheckCoeff(coeffB.Text.Trim(), numberOfParameters))
        //    {
        //        int index = -1;
        //        bool alreadyExcist = false;

        //        for (int i = 0; i < polynomString.Length; i++)
        //            if (polynomString[i] == coeffB.Text.Trim())
        //            {
        //                alreadyExcist = true;
        //                index = i;
        //                break;
        //            }

        //        if (alreadyExcist)
        //        {
        //            string[] newPS = new string[polynomString.Length - 1];

        //            for (int i = 0; i < index; i++)
        //                newPS[i] = polynomString[i];
        //            for (int i = index; i < newPS.Length; i++)
        //                newPS[i] = polynomString[i + 1];

        //            polynomString = newPS;
        //            polynom = Additionals.CreateListOfIndex(polynomString);

        //            coeffB.Clear();
        //            showPolynom_Click(sender, e);
        //        }
        //        else
        //            MessageBox.Show("Введенный коэффициент не найден");

        //    }

        //}




        public UInt64 factorial(int numb)
        {
            numb++;
            UInt64 numb1 = Convert.ToUInt64(numb);
            if (numb == 0)
            {
                return 0;
            }
            else
            {
                UInt64 range = 1;
                List<string> str = new List<string>();
                //return Enumerable.Range(1, Convert.ToInt32(numb)).Aggregate((p, x) => p * x);
                for (UInt64 i = 1; i < numb1; i++)
                {
                    range= range * i;
                    str.Add(Convert.ToString(range));
                }
                return range;
            }

        }

        //static int factorial(int i)
        //{
        //    int result=0;

        //    if (i == 1)
        //        return 1;
        //    for (int j = 0; j < i;j++ )
        //    {
        //        result = factorial(i - 1) * i;
        //    }
        //        return result;

        //}


        public double function(int n, int k)
        {
            if (n == 0 || k == 0)
            {
                return 0;
            }
                if ((n-k)==0 || n-k==-1)
                {
                    return 1;
                }
            else
            {
                UInt64 a = factorial(n);
                UInt64 b = factorial(k);
                UInt64 c = factorial(n - k);

                return Convert.ToInt32(a / (b * c));
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Global.CompleteListToExcel.Clear();
            Global.FullPolynomeToExcel.Clear();
            
            //Gd.Columns.Clear();
            //Gd.Items.Refresh();
           // Global.CompleteListToExcel.Clear();

            pr.againFromFile();
            pr.printForStart();
            pr.createPolynom(polDegreeComboBox.SelectedIndex + 1);
            File.Delete("Test.txt");

            double up_acc = 0;
            double down_acc = 0;

            Dispatcher.Invoke(new Action(() =>
            {
                pbar.Maximum = Global.polynomeNumb+1;
                up_acc = (double)AccuracyUp.Value;
                down_acc = (double)AccuracyDown.Value;
            }));

            Global.up_accuracy = up_acc;
            Global.down_accuracy = down_acc;


            Thread thread0 = new Thread(delegate()
            {
                StreamWriter sw;
                sw = File.AppendText("Test.txt");
                sw.WriteLine();
                sw.WriteLine("*************************************************************************************************");
                sw.Close();
            });

            thread0.Start();

            Thread thread = new Thread(delegate() {

                thread0.Join();
                thread0.Abort();

 
                pr.prisvoenie();

                #region для другого подхода комбинаторики
                //if (Global.polynomeNumb > 2 && Global.polynomeNumb < 7)
                //{
                //    for (int j = 2; j < 5; j++)
                //    {
                //        pr.CombinationMass(5, j);
                //    }

                //    for (int i = 0; i < Global.finalListSize; i++)
                //    {
                //        pr.polynomeResize(i);
                //        pr.Params_Combi();
                //        pr.function_ready();
                //        pr.regression_func(Global.SOLE);
                //        pr.InAccuracyCalc();
                //        pr.groupListPush();
                //    }

                //    pr.commonErrorTo_groupList();
                //    pr.sortResult();
                //    pr.commonError();
                //}

                //if (Global.polynomeNumb > 5 && Global.polynomeNumb<10)
                //{
                //    for (int j = 2; j < 5; j++)
                //    {
                //        pr.CombinationMass(5, j);
                //    }
                //    for (int j = 5; j < 9; j++)
                //    {
                //        pr.CombinationMass(9, j);
                //    }

                //    for (int i = 0; i < Global.finalListSize; i++)
                //    {
                //        pr.polynomeResize(i);
                //        pr.Params_Combi();
                //        pr.function_ready();
                //        pr.regression_func(Global.SOLE);
                //        pr.InAccuracyCalc();
                //        pr.groupListPush();
                //    }

                //    pr.commonErrorTo_groupList();
                //    pr.sortResult();
                //    pr.commonError();
                //}

                //if (Global.polynomeNumb > 10)
                //{
                //    for (int j = 2; j < 5; j++)
                //    {
                //        pr.CombinationMass(5, j);
                //    }

                //    for (int j = 5; j < 9; j++)
                //    {
                //        pr.CombinationMass(9, j);
                //    }

                //    for (int j = 9; j < 14; j++)
                //    {
                //        pr.CombinationMass(14, j);
                //    }

                //    for (int i = 0; i < Global.finalListSize; i++)
                //    {
                //        pr.polynomeResize(i);
                //        pr.Params_Combi();
                //        pr.function_ready();
                //        pr.regression_func(Global.SOLE);
                //        pr.InAccuracyCalc();
                //        pr.groupListPush();
                //    }

                //    pr.commonErrorTo_groupList();
                //    pr.sortResult();
                //    pr.commonError();
                //}
                #endregion

                for (int j = 5; j < Global.polynomeNumb + 1; j++)
                {
                    Dispatcher.Invoke(new Action(() =>
            {
                    pbar.Value = j;
            }));

                    Global.combinationSize = j;
                    //создание полинома
                    pr.CombinationMass(Global.polynomeNumb, j);

                    //количество сочетаний размера j
                    double combCount = 0;

                    combCount = function(Global.polynomeNumb, j);

                    for (int i = 0; i < combCount; i++)
                    {
                        pr.polynomeResize(i);
                        pr.Params_Combi();
                        //работа с функцией регрессии
                        pr.function_ready();
                        pr.regression_func(Global.SOLE);
                        //Вычисление точности
                        pr.InAccuracyCalc();
                        pr.groupListPush();
                    }
                }
                pr.commonErrorTo_groupList();
                pr.sortResult();
                pr.commonError();
                
                pr.PutPolynomeElementsToList();
                pr.CopyGroupList();
                
                Dispatcher.Invoke(new Action(() =>
                {
                    pbar.Value = pbar.Maximum;
                }));   

                GridRecord(Gd);

                Dispatcher.Invoke(new Action(() =>
                {
                    pbar.Value = 0;
                }));             
            });

            thread.Start();
            Thread thread2 = new Thread(delegate() 
                {
                    PrintResult(thread); 
                    thread.Abort(); 
                });
            thread2.Start();            
    }



        public void PrintResult(Thread A)
        {
            A.Join();
            // операции по выводу текста из файла 
            Dispatcher.Invoke(new Action(() =>
            {
                infoTextBox.Clear();
                StreamReader rdp = new StreamReader("Test.txt", Encoding.UTF8);
                infoTextBox.Text += rdp.ReadToEnd();
                rdp.Close();
            }));
        }

        Process pr = new Process();

        private void polDegreeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            File.Delete("Hand_Mode.txt");
            if (polDegreeComboBox.SelectedIndex != -1)
            {
                pr.againFromFile();
                pr.printForStart();

                pr.createPolynom(polDegreeComboBox.SelectedIndex + 1);


                pr.prisvoenie();
                for (int j = Global.polynomeNumb; j < Global.polynomeNumb + 1; j++)
                {

                    Global.combinationSize = j;
                    //создание полинома
                    pr.CombinationMass(Global.polynomeNumb, j);
                    //количество сочетаний размера j
                    double combCount = 0;
                    combCount = function(Global.polynomeNumb, j);
                    for (int i = 0; i < combCount; i++)
                    {
                        pr.polynomeResize(i);
                        pr.Params_Combi();
                        //работа с функцией регрессии
                        pr.function_ready();
                        pr.regression_func(Global.SOLE);
                        //Вычисление точности
                        pr.InAccuracyCalc();
                        pr.groupListPush();
                    }
                }
            pr.commonErrorTo_groupList();
            pr.commonError_Hand_Mode();

            textbox_1.Clear();
            textbox_1.AppendText(Global.polynome_type + " ");
            //textbox_2.Clear();
            //textbox_2.AppendText(Global.polynome_type + " ");
            //textbox_3.Clear();
            //textbox_3.AppendText(Global.polynome_type + " ");
            infoTextBox2.Clear();
            StreamReader rdp = new StreamReader("Hand_mode.txt", Encoding.UTF8);
            infoTextBox2.Text += rdp.ReadToEnd();
            rdp.Close();
            }
        }

        public int membersCount(string str)
        {
            int k=1;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i]=='+')
                {
                    k++;
                }
            }
            return k;
        }

        public List<string> Elements(string str)
        {
            str += " ";
            List<string> listOfElem = new List<string>();
            string elem="";
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i]!='+' && str[i]!=' ' )
                {
                    elem += str[i];
                }
                else
                {
                    listOfElem.Add(elem);
                    elem = "";
                }
            }
            //listOfElem.Remove(listOfElem[0]);
            return listOfElem;
        }

        private void Hand_regress_Click(object sender, RoutedEventArgs e)
        {
            pr.againFromFile();
            pr.printForStart();
            pr.prisvoenie();
            Global.combinationSize=membersCount(textbox_1.GetLineText(0))-1;
            //membersCount(textbox_2.GetLineText(0));
            //membersCount(textbox_3.GetLineText(0));
            Global.tempElements = Elements(textbox_1.GetLineText(0));
            pr.polynomeResizeForHandMode();
            pr.Params_Combi();
            //работа с функцией регрессии
            pr.function_ready();
            pr.regression_func(Global.SOLE);
            //Вычисление точности
            pr.InAccuracyCalc();
            pr.groupListPush();
            pr.commonErrorTo_groupList();
            pr.commonError_Hand_Mode();        
            infoTextBox2.Clear();
            StreamReader rdp = new StreamReader("Hand_mode.txt", Encoding.UTF8);
            infoTextBox2.Text += rdp.ReadToEnd();
            rdp.Close();
        }

        DataTable Maindt = new DataTable();

         public void GridRecord(DataGrid temp_grid)
         {
                 Dispatcher.Invoke(new Action(() => { Gd.CanUserAddRows = false; }));
                     Dispatcher.Invoke(new Action(() =>
                     {
                         DataTable dt = new DataTable();

                         dt.Columns.Clear();
                         dt.Clear();
                         //Gd.Items.Refresh();
                         Gd.Columns.Clear();

                         dt.Columns.Add("Номер", typeof(double));
                         //dt.Columns.Add("0");
                         for (int i = 0; i < Global.FullPolynomeToExcel.Count; i++)
                         {
                             dt.Columns.Add(Global.FullPolynomeToExcel[i]);
                         }
                         dt.Columns.Add("Искл", typeof(double));
                         dt.Columns.Add("Абс ошиб", typeof(double));
                         dt.Columns.Add("Относ ошиб", typeof(double));
                         dt.Columns.Add("Лучший алг рег");
                         for (int i = 0; i < Global.CompleteListToExcel.Count; i++)
                         {
                             DataRow dr = dt.NewRow();
                             dr["Номер"] = i+1;
                             for (int j = 0; j < Global.CompleteListToExcel[i].PolynomeMembers.Count; j++)
                             {
                                 for (int k = 0; k < Global.FullPolynomeToExcel.Count+1; k++)
                                 {
                                     if (dt.Columns[k+1].ColumnName == Global.CompleteListToExcel[i].PolynomeMembers[j])
                                     {                                         
                                         dr[k+1] = Convert.ToDouble(Global.CompleteListToExcel[i].PolynomeMembers[j]);
                                     }
                                 }
                             }
                             dr["Абс ошиб"] = Convert.ToDouble(Global.CompleteListToExcel[i].commonAbsEror);
                             dr["Относ ошиб"] = Convert.ToDouble(Global.CompleteListToExcel[i].commonRelError);
                             dr["Лучший алг рег"] = Global.CompleteListToExcel[i].RegresType;
                             int count = 0;
                             for (int k = 0; k < Global.FullPolynomeToExcel.Count; k++)
                             {
                                 if (Convert.ToString(dr[k+1]).Length==0)
                                 {
                                     dr[k+1] = "--";
                                     count++;
                                 }
                             }
                             dr["Искл"] = count;                             
                             dt.Rows.Add(dr);
                         }
                         Maindt = dt;
                         Gd.ItemsSource = Maindt.DefaultView;
                     }));
         }


        private void ToExcel_Click(object sender, RoutedEventArgs e)
        {
            var workbook = new XLWorkbook();
            workbook.Worksheets.Add(Maindt, "new");
            workbook.SaveAs("Sample.xlsx");
            workbook.Dispose();
            System.Diagnostics.Process.Start("Sample.xlsx");
        }
        
    }
    
}
