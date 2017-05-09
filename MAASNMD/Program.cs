using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;


namespace MAASNMD
{

    class ExperObject
    {
        public List<double> X_parameters { get; set; }        
        public double Y_parametr { get; set; }

        public List<double> X_Combination =new List<double>();        
        public double Y_analitic { get; set; }
        public double inaccur1 { get; set; }//абсолютная ошибка        
        public double inaccur2 { get; set; }//относительная ошибка

        public virtual ExperObject toExperObject()
        {
            return new ExperObject
            {
                X_parameters = this.X_parameters,
                Y_parametr = this.Y_parametr,
                X_Combination = this.X_Combination,
                Y_analitic = this.Y_analitic,
                inaccur1=this.inaccur1,
                inaccur2=this.inaccur2
            };
        }
    }

    class ObjectGroup
    {    
        public List<ExperObject> ValueList = new List<ExperObject>();
        public double commonAbsEror { get; set; } //общая абсолютная ошибка для группы
        public double commonRelError { get; set; }//общая относительная ошибка для группы
        public string polynomeView { get; set; } //вид полинома в строковом исполнении
        public List<string> PolynomeMembers = new List<string>();//члены полинома

        public List<string> regressNumbers = new List<string>(); //коэффициенты регрессии

        public string RegresType { get; set; }
    }

    class Process
    {
        public static List<ExperObject> ObjectsList = new List<ExperObject>();
        public List<int[]> polynom = new List<int[]>();        
        public string[] polynomString;
        public double[] regressCoef;
        public List<string> temppolynome = new List<string>();
        

        public void FromFileToList()
        {
            ObjectsList.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (openFileDialog.ShowDialog() == true)
            {
                if (System.IO.Path.GetExtension(openFileDialog.FileName) == ".txt")
                {
                    Global.using_file_name=openFileDialog.FileName;
                    string[] s = File.ReadAllLines(openFileDialog.FileName);
                    List<string[]> ls = new List<string[]>();
                    for (int i = 0; i < s.Length; i++)
                    {
                        ls.Add(s[i].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));

                    }
                    for (int i = 1; i < ls.Count; i++)
                    {
                        List<string> mas = new List<string>();
                        List<double> temp = new List<double>();
                        string result;
                        double res;

                        for (int j = 0; j < ls[i].Length - 1; j++)
                        {
                            mas.Add(ls[i][j]);
                        }
                        result = ls[i][ls[i].Length-1];
                        for (int d = 0; d < mas.Count; d++)
                        {
                            temp.Add(Convert.ToDouble(mas[d]));
                        }
                        res = Convert.ToDouble(result);
                        ObjectsList.Add(new ExperObject { X_parameters = temp, Y_parametr = res });
                        //List<double> temp = new List<double>();

                      
                    }

                }
            }
        }


        public void againFromFile()
        {
            //Очистка мусора
            ObjectsList.Clear();

            groupList.Clear();
            k = 0;//говнокод
            plnm.Clear();
            tempRegres.Clear();
            TempTypeOfRegress.Clear();

            groupList.Clear();
            ///

            string[] s = File.ReadAllLines(Global.using_file_name);
            List<string[]> ls = new List<string[]>();
            for (int i = 0; i < s.Length; i++)
            {
                ls.Add(s[i].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));

            }
            for (int i = 1; i < ls.Count; i++)
            {
                List<string> mas = new List<string>();
                List<double> temp = new List<double>();
                string result;
                double res;

                for (int j = 0; j < ls[i].Length - 1; j++)
                {
                    mas.Add(ls[i][j]);
                }
                result = ls[i][ls[i].Length - 1];
                for (int d = 0; d < mas.Count; d++)
                {
                    temp.Add(Convert.ToDouble(mas[d]));
                }
                res = Convert.ToDouble(result);
                ObjectsList.Add(new ExperObject { X_parameters = temp, Y_parametr = res });
                //List<double> temp = new List<double>();


            }
        }

        public void printForStart()
        {
            

            StreamWriter sw;
            sw = File.AppendText("Hand_Mode.txt");
            sw.WriteLine("Входные данные:");
            sw.Close();
            ExpertValues(1, ObjectsList, "Hand_Mode.txt");
            StreamWriter sw1;
            sw1 = File.AppendText("Hand_Mode.txt");
            sw1.WriteLine();
            sw1.Close();
        }

        #region Подготовка полинома и СЛАУ

        public void createPolynom(int stepen)
        {
            polynome_string = "";
                polynomString = Additionals.BuildIndex(ObjectsList[0].X_parameters.Count, stepen);// создаётся полином (создаются индексы)

                Global.polynomeNumb = polynomString.Length;

                StreamWriter sw;
                sw = File.AppendText("Test.txt");
                //sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("Полный полином имеет вид:");
                //sw.Write("0+");
                for (int i = 0; i < polynomString.Length - 1; i++)
                    sw.Write(polynomString[i] + "+");
                sw.Write(polynomString[polynomString.Length - 1]);
                //sw.WriteLine();
                //sw.WriteLine();
                //sw.WriteLine("////////////////////////////////////////////////////////////////////////////////////////////////");
                sw.Close();

                //StreamWriter sw;
                //sw = File.AppendText("Test.txt");
                //sw.WriteLine();
                //sw.WriteLine();

                //sw.WriteLine("Информация об созданном полиноме: \r");

                //sw.WriteLine();
            for (int i=0;i<polynomString.Length;i++)
            {
                Global.FullPolynomeToExcel.Add(polynomString[i]);
            }
                

                //sw.Close();
                polynom = Additionals.CreateListOfIndex(polynomString);      //создаётся полином (создаётся список индексов)
        }

       
        public void Params_Combi()
        {
            //StreamWriter sw;
            //sw = File.AppendText("Test.txt");

            //sw.WriteLine();
            //sw.WriteLine();
            for (int i = 0; i < ObjectsList.Count; i++)
            {
                ObjectsList[i].X_Combination.Clear();
            }

            for (int i = 0; i < ObjectsList.Count; i++)
            {
                //[i].X_Combination.Add(1); //свободный член (самый первый)
                for (int j = 0; j < polynomString.Length; j++)
                {         
                    //if (polynomString[j]=="0")
                    //{
                    //    ObjectsList[i].X_Combination.Add(1);
                    //}

                   double val = 1.0;
                   for (int g = 0; g < polynom[j].Length; g++)
                   {
                       if ((polynom[j][g] - 1) < 0)
                       {
                           break;
                       }
                       else
                       {
                           val *= ObjectsList[i].X_parameters[polynom[j][g] - 1];
                       }                       
                   }
                       
                   ObjectsList[i].X_Combination.Add(val);
                }
                ObjectsList[i].X_Combination.Add(ObjectsList[i].Y_parametr);//добавления Y
            }

            //polynom = Additionals.CreateListOfIndex(polynomString);
            //sw.Close();
        }
       

        
        public void function_ready()
        {
            Global.SOLE = new double[ObjectsList.Count][];
            for (int i = 0; i < ObjectsList.Count; i++)
                Global.SOLE[i] = new double[ObjectsList[0].X_Combination.Count];

            for (int i = 0; i < ObjectsList.Count; i++)
            {
                for (int j = 0; j < ObjectsList[i].X_Combination.Count; j++)
                {
                    Global.SOLE[i][j] = ObjectsList[i].X_Combination[j];
                }
            }
        }
        #endregion
        
        #region Функция выполнения линейной регрессии


        //public virtual List<string> toNewList()
        //{
        //    return new List<string>
        //    {
        //        listOfRegres = this.listOfRegres

        //    };
        //}

        

        public void Checking(double[][] Mas, double[] reuslt)
        {
            for (int i = 0; i < ObjectsList.Count; i++)
            {
                double temp = 0;
                double Sum = 0;
                for (int j = 0; j < Mas[i].Length - 1; j++)
                {
                    temp = ObjectsList[i].X_Combination[j];
                    temp = temp * reuslt[j];
                    Sum += temp;
                }
                ObjectsList[i].Y_analitic = Sum;
            }
        }

        public double CheckError()
        {
            double Sum_abs = 0;
            double kol = 0;
            for (int j = 0; j < ObjectsList.Count; j++)
            {
                Sum_abs += ObjectsList[j].inaccur1 * ObjectsList[j].inaccur1;
                kol++;
            }
            double absolutSKO = Math.Round((Math.Sqrt(Sum_abs / kol)), 5);
            return absolutSKO;
        }

        public void Clear_Y_analitic()
        {
            for (int i=0;i<ObjectsList.Count;i++)
            {
                ObjectsList[i].Y_analitic = 0;
                ObjectsList[i].inaccur1 = 0;
                ObjectsList[i].inaccur2 = 0;
            }
        }

        public void CopyToRegresList(double[] result)
        {
            Array.Resize(ref regressCoef, result.Length);

            for (int i=0;i<result.Length-1;i++)
            {                
                regressCoef[i] = result[i];
            }
        }

        List<List<string>> tempRegres = new List<List<string>>();
        public List<string> listOfRegres = new List<string>();

        public List<string> TempTypeOfRegress = new List<string>();
        public void regression_func( double[][] Mas)
        {
            //TempTypeOfRegress = 
            listOfRegres.Clear();

            if (ObjectsList[0].X_parameters.Count>2)
            {
                double[] regressCoefTEMP2 = LinearRegressionProgram.Solve(Mas, 2);
                Checking(Global.SOLE, regressCoefTEMP2);
                InAccuracyCalc();
                double Er2 = CheckError();

                double[] regressCoefTEMP3 = LinearRegressionProgram.Solve(Mas, 3);
                Checking(Global.SOLE, regressCoefTEMP3);
                InAccuracyCalc();
                double Er3 = CheckError();

                if ( Er2 < Er3)
                {
                    TempTypeOfRegress.Add("Алг.2");
                    Checking(Global.SOLE, regressCoefTEMP2);
                    regressCoef = LinearRegressionProgram.Solve(Mas, 2);
                    //CopyToRegresList(regressCoefTEMP2);
                    
                    //sw.Close();
                    //tempRegres.Add(ListList);
                    //listOfRegres.Add(("b0  -   " + regressCoef[0]));
                    for (int i = 0; i < regressCoef.Length; i++)
                    {
                        listOfRegres.Add(("b" + polynomString[i] + "  -   " + regressCoef[i]));
                    }
                    List<string> ListList = new List<string>(this.listOfRegres);
                    tempRegres.Add(ListList);
                }

                if ( Er3 < Er2)
                {
                    TempTypeOfRegress.Add("Алг.3");
                    Checking(Global.SOLE, regressCoefTEMP3);
                    regressCoef = LinearRegressionProgram.Solve(Mas, 3);
                    //CopyToRegresList(regressCoefTEMP3);
                    //List<string> ListList = new List<string>(this.listOfRegres);
                    //sw.Close();
                    //tempRegres.Add(ListList);
                    //listOfRegres.Add(("b0  -   " + regressCoef[0]));
                    for (int i = 0; i < regressCoef.Length; i++)
                    {
                        listOfRegres.Add(("b" + polynomString[i] + "  -   " + regressCoef[i]));
                    }
                    List<string> ListList = new List<string>(this.listOfRegres);
                    tempRegres.Add(ListList);
                }

                if ( Er3 == Er2)
                {
                    TempTypeOfRegress.Add("Алг.2/Алг.3");
                   // Checking(Global.SOLE, regressCoefTEMP3);
                    regressCoef = LinearRegressionProgram.Solve(Mas, 3);
                    //CopyToRegresList(regressCoefTEMP3);
                    //List<string> ListList = new List<string>(this.listOfRegres);
                    //sw.Close();
                    //tempRegres.Add(ListList);
                    //listOfRegres.Add(("b0  -   " + regressCoef[0]));
                    for (int i = 0; i < regressCoef.Length; i++)
                    {
                        listOfRegres.Add(("b" + polynomString[i] + "  -   " + regressCoef[i]));
                    }
                    List<string> ListList = new List<string>(this.listOfRegres);
                    tempRegres.Add(ListList);
                }


                else
                {
                    TempTypeOfRegress.Add("Алг.2");
                    Checking(Global.SOLE, regressCoefTEMP2);
                    regressCoef = LinearRegressionProgram.Solve(Mas, 2);
                    //CopyToRegresList(regressCoefTEMP3);
                    //List<string> ListList = new List<string>(this.listOfRegres);
                    //sw.Close();
                    //tempRegres.Add(ListList);
                    //listOfRegres.Add(("b0  -   " + regressCoef[0]));
                    for (int i = 0; i < regressCoef.Length; i++)
                    {
                        listOfRegres.Add(("b" + polynomString[i] + "  -   " + regressCoef[i]));
                    }
                    List<string> ListList = new List<string>(this.listOfRegres);
                    tempRegres.Add(ListList);
                }

            }

            else
            {
                double[] regressCoefTEMP1 = LinearRegressionProgram.Solve(Mas, 1);
                Checking(Global.SOLE, regressCoefTEMP1);
                InAccuracyCalc();
                double Er1 = CheckError();

                double[] regressCoefTEMP2 = LinearRegressionProgram.Solve(Mas, 2);
                Checking(Global.SOLE, regressCoefTEMP2);
                InAccuracyCalc();
                double Er2 = CheckError();

                double[] regressCoefTEMP3 = LinearRegressionProgram.Solve(Mas, 3);
                Checking(Global.SOLE, regressCoefTEMP3);
                InAccuracyCalc();
                double Er3 = CheckError();

                //Clear_Y_analitic();


                if (Er1 < Er2 && Er1 < Er3)
                {
                    TempTypeOfRegress.Add("Алг.1");
                    Checking(Global.SOLE, regressCoefTEMP1);
                    regressCoef = LinearRegressionProgram.Solve(Mas, 1);
                    //CopyToRegresList(regressCoefTEMP1);
                    //List<string> ListList = new List<string>(this.listOfRegres);
                    //sw.Close();
                    //tempRegres.Add(ListList);
                    //listOfRegres.Add(("b0  -   " + regressCoef[0]));
                    for (int i = 0; i < regressCoef.Length; i++)
                    {
                        listOfRegres.Add(("b" + polynomString[i] + "  -   " + regressCoef[i]));
                    }
                    List<string> ListList = new List<string>(this.listOfRegres);
                    tempRegres.Add(ListList);
                }

                if (Er2 < Er1 && Er2 < Er3)
                {
                    TempTypeOfRegress.Add("Алг.2");
                    Checking(Global.SOLE, regressCoefTEMP2);
                    regressCoef = LinearRegressionProgram.Solve(Mas, 2);
                    //CopyToRegresList(regressCoefTEMP2);
                    //List<string> ListList = new List<string>(this.listOfRegres);
                    //sw.Close();
                    //tempRegres.Add(ListList);
                    //listOfRegres.Add(("b0  -   " + regressCoef[0]));
                    for (int i = 0; i < regressCoef.Length; i++)
                    {
                        listOfRegres.Add(("b" + polynomString[i] + "  -   " + regressCoef[i]));
                    }
                    List<string> ListList = new List<string>(this.listOfRegres);
                    tempRegres.Add(ListList);
                }

                if (Er3 < Er1 && Er3 < Er2)
                {
                    TempTypeOfRegress.Add("Алг.3");
                    Checking(Global.SOLE, regressCoefTEMP3);
                    regressCoef = LinearRegressionProgram.Solve(Mas, 3);
                    //CopyToRegresList(regressCoefTEMP3);
                    //List<string> ListList = new List<string>(this.listOfRegres);
                    //sw.Close();
                    //tempRegres.Add(ListList);
                    //listOfRegres.Add(("b0  -   " + regressCoef[0]));
                    for (int i = 0; i < regressCoef.Length; i++)
                    {
                        listOfRegres.Add(("b" + polynomString[i] + "  -   " + regressCoef[i]));
                    }
                    List<string> ListList = new List<string>(this.listOfRegres);
                    tempRegres.Add(ListList);
                }

                if (Er3 == Er1 && Er3 == Er2)
                {
                    TempTypeOfRegress.Add("Алг.1/Алг.2/Алг.3");
                    Checking(Global.SOLE, regressCoefTEMP2);
                    regressCoef = LinearRegressionProgram.Solve(Mas, 2);
                    //CopyToRegresList(regressCoefTEMP3);
                    //List<string> ListList = new List<string>(this.listOfRegres);
                    //sw.Close();
                    //tempRegres.Add(ListList);
                    //listOfRegres.Add(("b0  -   " + regressCoef[0]));
                    for (int i = 0; i < regressCoef.Length; i++)
                    {
                        listOfRegres.Add(("b" + polynomString[i] + "  -   " + regressCoef[i]));
                    }
                    List<string> ListList = new List<string>(this.listOfRegres);
                    tempRegres.Add(ListList);
                }


                if (Er3 == Er1 && Er3 < Er2)
                {
                    TempTypeOfRegress.Add("Алг.1/Алг.3");
                    Checking(Global.SOLE, regressCoefTEMP3);
                    regressCoef = LinearRegressionProgram.Solve(Mas, 3);
                    //CopyToRegresList(regressCoefTEMP3);
                    //List<string> ListList = new List<string>(this.listOfRegres);
                    //sw.Close();
                    //tempRegres.Add(ListList);
                    //listOfRegres.Add(("b0  -   " + regressCoef[0]));
                    for (int i = 0; i < regressCoef.Length; i++)
                    {
                        listOfRegres.Add(("b" + polynomString[i] + "  -   " + regressCoef[i]));
                    }
                    List<string> ListList = new List<string>(this.listOfRegres);
                    tempRegres.Add(ListList);
                }
                if (Er2 == Er1 && Er2 < Er3)
                {
                    TempTypeOfRegress.Add("Алг.1/Алг.2");
                    Checking(Global.SOLE, regressCoefTEMP1);
                    regressCoef = LinearRegressionProgram.Solve(Mas, 1);
                    //CopyToRegresList(regressCoefTEMP3);
                    //List<string> ListList = new List<string>(this.listOfRegres);
                    //sw.Close();
                    //tempRegres.Add(ListList);
                    //listOfRegres.Add(("b0  -   " + regressCoef[0]));
                    for (int i = 0; i < regressCoef.Length; i++)
                    {
                        listOfRegres.Add(("b" + polynomString[i] + "  -   " + regressCoef[i]));
                    }
                    List<string> ListList = new List<string>(this.listOfRegres);
                    tempRegres.Add(ListList);
                }
                if (Er2 == Er3 && Er2 < Er1)
                {
                    TempTypeOfRegress.Add("Алг.2/Алг.3");
                    Checking(Global.SOLE, regressCoefTEMP2);
                    regressCoef = LinearRegressionProgram.Solve(Mas, 2);
                    //CopyToRegresList(regressCoefTEMP3);
                    //List<string> ListList = new List<string>(this.listOfRegres);
                    //sw.Close();
                    //tempRegres.Add(ListList);
                    //listOfRegres.Add(("b0  -   " + regressCoef[0]));
                    for (int i = 0; i < regressCoef.Length; i++)
                    {
                        listOfRegres.Add(("b" + polynomString[i] + "  -   " + regressCoef[i]));
                    }
                    List<string> ListList = new List<string>(this.listOfRegres);
                    tempRegres.Add(ListList);
                }
            }     
        }
        #endregion

        class CompIndivMaxToMin<M> : IComparer<M>
       where M : ExperObject
        {
            // Реализуем интерфейс IComparer<T>
            public int Compare(M x, M y)
            {
                if (x.X_parameters[0] > y.X_parameters[0])
                    return 1;
                if (x.X_parameters[0] < y.X_parameters[0])
                    return -1;
                else return 0;
            }
        }

        class CompErrorMinToMiax<M> : IComparer<M>
      where M : ObjectGroup
        {
            public int Compare(M x, M y)
            {
                if (x.commonAbsEror > y.commonAbsEror)
                    return 1;
                if (x.commonAbsEror < y.commonAbsEror)
                    return -1;
                else return 0;
            }
        }


        public void InAccuracyCalc()
        {
            for (int i=0;i<ObjectsList.Count;i++)
            {
                ObjectsList[i].inaccur1 = ObjectsList[i].Y_analitic - ObjectsList[i].Y_parametr;//абсолютная ошибка
                ObjectsList[i].inaccur2 = (ObjectsList[i].Y_analitic - ObjectsList[i].Y_parametr) / ObjectsList[i].Y_parametr;//относительная ошибка
            }
        }


        public List<int> argumetHighlight(List<ExperObject> List, int num)
        {
            List<double> temp = new List<double>();
            List<int> result = new List<int>();
            if (temp.Count == 0)
            {
                temp.Add(List[0].X_parameters[num]);
                result.Add(0);
            }
            for (int i=1; i < List.Count; i++)
            {
                bool A = false;
                for (int t=0;t<temp.Count;t++)
                {
                    if (temp[t] == List[i].X_parameters[num])
                    {
                        A = true;
                        break;
                    }
                }
                if (A==false)
                {
                    temp.Add(List[i].X_parameters[num]);
                    result.Add(i);
                }                        
            }
            return result;
        }

        public void ExpertValues(int variant, List<ExperObject> objectTempList, string file_name)  //Функция вывода полученных результатов в виде матрицы (экспериментальные и аналитические значения, значения абсолютных и относительных ошибок)
        {
            CompIndivMaxToMin<ExperObject> ResSort = new CompIndivMaxToMin<ExperObject>();
            objectTempList.Sort(ResSort);

            StreamWriter sw;
            sw = File.AppendText(file_name);
            sw.WriteLine();
            if (variant == 1)
                sw.WriteLine("Матрица экспериментальных значений");
            if (variant == 2)
                sw.WriteLine("Матрица аналитических значений");
            if (variant == 3)
                sw.WriteLine("Матрица абсолютных ошибок");
            if (variant == 4)
                sw.WriteLine("Матрица относительных ошибок");
            sw.Write("\t№");
            List<int> objectCount_X = argumetHighlight(objectTempList/*ObjectsList*/, 1);
            for (int i = 0; i < objectCount_X.Count; i++)
            {
                sw.Write("\t{0}", objectTempList[objectCount_X[i]].X_parameters[1]);                  
            }
            List<int> objectCount_Y = argumetHighlight(objectTempList /*objectTempList*/, 0);
            for (int k = 0; k < objectCount_Y.Count; k++)
            {
                sw.WriteLine();
                sw.Write("\t{0}", objectTempList[objectCount_Y[k]].X_parameters[0]);            
                    for (int j = 0; j < objectCount_X.Count; j++)
                    {
                        for (int obj = 0; obj < objectTempList.Count; obj++)
                        {
                            if (objectTempList[obj].X_parameters[0] == objectTempList[objectCount_Y[k]].X_parameters[0] && objectTempList[obj].X_parameters[1] == objectTempList[objectCount_X[j]].X_parameters[1])
                            {
                                if (variant == 1)
                                    sw.Write("\t{0}", Math.Round(objectTempList[obj].Y_parametr, 1));
                                if (variant == 2)
                                    sw.Write("\t{0}", Math.Round(objectTempList[obj].Y_analitic, 1));
                                if (variant == 3)
                                    sw.Write("\t{0}", Math.Round(objectTempList[obj].inaccur1, 1));
                                if (variant == 4)
                                    sw.Write("\t{0}", Math.Round(objectTempList[obj].inaccur2, 3));
                            }
                        }
                    }                
            }
            sw.Close();
        }
        


        List<ObjectGroup> groupList = new List<ObjectGroup>();

        public List<ExperObject> tempValuesList = new List<ExperObject>();

        int k = 0;//ультраговнокод
        public void groupListPush()
        {
            tempValuesList.Clear();

            groupList.Add(new ObjectGroup { });

            for (int i = 0; i < ObjectsList.Count; i++)
            {
                tempValuesList.Add(ObjectsList[i].toExperObject());
            }
            for (int i = 0; i < ObjectsList.Count; i++)
            {
                groupList[k].ValueList.Add(tempValuesList[i]);
                //groupList[k].polynomeView = plnm[i];
            }                
            k++;            
        }

        public void commonErrorTo_groupList()
        {
            for (int i = 0; i < groupList.Count; i++)
            {
                groupList[i].polynomeView = plnm[i];

                groupList[i].regressNumbers = tempRegres[i];

                groupList[i].RegresType = TempTypeOfRegress[i];

                //for (int r = 0; r < groupList[i].regressNumbers.Count;r++ )
                //{
                //    groupList[i].regressNumbers.Add(tempRegres[r][]);
                //}

                double Max_Sum_abs = 0;
                double Sum_otnos = 0;

                double Sum_exp = 0;
                double Sum_otnos1 = 0;

                double kol = 0;

                double fact_value = 0;
                Max_Sum_abs = groupList[i].ValueList[0].inaccur1;
                fact_value = groupList[i].ValueList[0].Y_parametr;

                

                int num=0;
                for (int j = 0; j < groupList[i].ValueList.Count; j++)
                {
                    //Max_Sum_abs = groupList[i].ValueList[0].inaccur1; //* groupList[i].ValueList[j].inaccur1;
                    //Sum_otnos += groupList[i].ValueList[j].inaccur2 * groupList[i].ValueList[j].inaccur2;
                    Sum_exp += Math.Abs(groupList[i].ValueList[j].Y_parametr);
                    //Sum_otnos1 += groupList[i].ValueList[j].inaccur2;
                    if (Max_Sum_abs < groupList[i].ValueList[j].inaccur1)
                    {
                        Max_Sum_abs = groupList[i].ValueList[j].inaccur1;
                        fact_value = groupList[i].ValueList[j].Y_parametr;
                    }
                    kol++;
                    
                }

                double expertAverage = Sum_exp / kol;

                //double absolutAverage = Math.Round((Sum_abs1 / kol),5);
                //double releativeAverage = Math.Round((Sum_otnos1 / kol),5);

                //double sumsum = 0;
                //double sumsum1 = 0;
                //for (int t=0;t<groupList[i].ValueList.Count;t++)
                //{
                //    //sumsum += groupList[i].ValueList[t].inaccur1 - absolutAverage;
                //    //sumsum1 += groupList[i].ValueList[t].inaccur2 - releativeAverage;

                //    sumsum += groupList[i].ValueList[t].inaccur1 - absolutAverage;
                //    sumsum1 += groupList[i].ValueList[t].inaccur2 - releativeAverage;
                //}

                //sumsum = sumsum * sumsum;
                //sumsum1 = sumsum1 * sumsum1;

                double absolutSKO = Max_Sum_abs; //Math.Round((Math.Sqrt(Sum_abs / kol)), 5);
                //double releativeSKO = Math.Round((Math.Sqrt(Sum_otnos / kol)), 5);

                //groupList[i].commonAbsEror = Math.Round((Math.Abs(( absolutSKO))), 8);
                //groupList[i].commonRelError = Math.Round((Math.Abs((releativeSKO))), 5);
                groupList[i].commonAbsEror = Math.Round((Math.Abs((absolutSKO))), 5);

                groupList[i].commonRelError = Math.Round((Math.Abs((absolutSKO / fact_value))), 5);
            }
        }

        public void sortResult()
        {
            CompErrorMinToMiax<ObjectGroup> ResSort = new CompErrorMinToMiax<ObjectGroup>();
            groupList.Sort(ResSort);

            int k = groupList.Count;
            for (int i = 0; i < k; i++)
            {
                if (groupList[i].commonRelError < Global.down_accuracy || groupList[i].commonRelError > Global.up_accuracy)
                {
                    groupList.Remove(groupList[i]);
                    i = -1;
                    k--;
                }
            }
        }

        public void commonError_Hand_Mode()
        {
            for (int i = 0; i < groupList.Count; i++)
            {

                StreamWriter sw;
                sw = File.AppendText("Hand_mode.txt");
                //sw.WriteLine("Сочетание полинома № {0}", i + 1);
                sw.WriteLine("Информация о созданном полиноме:");
                Global.polynome_type = groupList[i].polynomeView;
                sw.WriteLine(groupList[i].polynomeView);
                sw.Write("Коэффициенты регрессии: \n");
                for (int r = 0; r < groupList[i].regressNumbers.Count; r++)
                {
                    sw.WriteLine(groupList[i].regressNumbers[r]);
                }
                sw.Close();

                ExpertValues(1, groupList[i].ValueList, "Hand_mode.txt");
                ExpertValues(2, groupList[i].ValueList, "Hand_mode.txt");
                ExpertValues(3, groupList[i].ValueList, "Hand_mode.txt");
                //ExpertValues(4, groupList[i].ValueList);

                double Sum_abs = 0;
                double Sum_otnos = 0;
                double Sum_abs1 = 0;
                double Sum_otnos1 = 0;
                double kol = 0;

                for (int j = 0; j < groupList[i].ValueList.Count; j++)
                {
                    Sum_abs += groupList[i].ValueList[j].inaccur1 * groupList[i].ValueList[j].inaccur1;
                    Sum_otnos += groupList[i].ValueList[j].inaccur2 * groupList[i].ValueList[j].inaccur2;
                    Sum_abs1 += groupList[i].ValueList[j].inaccur1;
                    Sum_otnos1 += groupList[i].ValueList[j].inaccur2;
                    kol++;
                }

                StreamWriter sw1;
                sw1 = File.AppendText("Hand_mode.txt");
                sw1.WriteLine();
                sw1.WriteLine("Общее абсолютное значение ошибки в точках {0}", groupList[i].commonAbsEror);//СКО отнести к среднему значению модулей величин
                sw1.WriteLine("Общее относительное значение ошибки в точках {0}", groupList[i].commonRelError);//СКО
                sw1.WriteLine();
                sw1.Close();
            }

        }


        public void commonError()
        {
            for (int i = 0; i < groupList.Count; i++)
            {
                
                    StreamWriter sw;
                    sw = File.AppendText("Test.txt");
                    sw.WriteLine("Сочетание полинома № {0}", i + 1);
                    sw.WriteLine("Информация о созданном полиноме:");
                    sw.WriteLine(groupList[i].polynomeView);
                    //sw.Write("Коэффициенты регрессии: \r\n");
                    //for (int r = 0; r < groupList[i].regressNumbers.Count; r++)
                    //{
                    //    sw.WriteLine(groupList[i].regressNumbers[r]);
                    //}
                    sw.Close();

                    //ExpertValues(1, groupList[i].ValueList);
                    //ExpertValues(2, groupList[i].ValueList);
                    //ExpertValues(3, groupList[i].ValueList);
                    //ExpertValues(4, groupList[i].ValueList);

                    double Sum_abs = 0;
                    double Sum_otnos = 0;
                    double Sum_abs1 = 0;
                    double Sum_otnos1 = 0;
                    double kol = 0;

                    for (int j = 0; j < groupList[i].ValueList.Count; j++)
                    {
                        Sum_abs += groupList[i].ValueList[j].inaccur1 * groupList[i].ValueList[j].inaccur1;
                        Sum_otnos += groupList[i].ValueList[j].inaccur2 * groupList[i].ValueList[j].inaccur2;
                        Sum_abs1 += groupList[i].ValueList[j].inaccur1;
                        Sum_otnos1 += groupList[i].ValueList[j].inaccur2;
                        kol++;
                    }

                    StreamWriter sw1;
                    sw1 = File.AppendText("Test.txt");
                    sw1.WriteLine("Общее абсолютное значение ошибки в точках {0}", groupList[i].commonAbsEror);//СКО отнести к среднему значению модулей величин
                    sw1.WriteLine("Общее относительное значение ошибки в точках {0}", groupList[i].commonRelError);//СКО
                    sw1.WriteLine();
                    sw1.Close();
                }
            
        }

        /*
        public ExperObject X00;
        public ExperObject X01;
        public ExperObject X10;
        public ExperObject X11;

        public void InaccurBeetwin()
        {

            StreamWriter sw;
            sw = File.AppendText("Test.txt");
            
            sw.WriteLine();
            sw.WriteLine();
            sw.WriteLine("ОШИБКИ МЕЖДУ ТОЧКАМИ");


            double Sum_abs = 0;
            double Sum_otnos = 0;
            double kol = 0;


            for (int i = 1; i < tempX11[0].Count; i++)
            {
                for (int j = 1; j < tempX11[1].Count; j++)
                {


                    for (int obj = 0; obj < ObjectsList.Count; obj++)
                    {
                        if (ObjectsList[obj].X_parameters[0] == tempX11[0][i-1] && ObjectsList[obj].X_parameters[1] == tempX11[1][j-1])
                        {
                            X00 = ObjectsList[obj];
                        }
                        if (ObjectsList[obj].X_parameters[0] == tempX11[0][i - 1] && ObjectsList[obj].X_parameters[1] == tempX11[1][j])
                        {
                            X01 = ObjectsList[obj];
                        }
                        if (ObjectsList[obj].X_parameters[0] == tempX11[0][i] && ObjectsList[obj].X_parameters[1] == tempX11[1][j-1])
                        {
                            X10 = ObjectsList[obj];
                        }
                        if (ObjectsList[obj].X_parameters[0] == tempX11[0][i] && ObjectsList[obj].X_parameters[1] == tempX11[1][j])
                        {
                            X11 = ObjectsList[obj];
                        }
                    }

                    double Err_abs = (X00.Y_parametr + X01.Y_parametr + X10.Y_parametr + X11.Y_parametr) / 4 - (X00.Y_analitic + X01.Y_analitic + X10.Y_analitic + X11.Y_analitic) / 4;
                    double Err_otnos = ((X00.Y_parametr + X01.Y_parametr + X10.Y_parametr + X11.Y_parametr) / 4 - (X00.Y_analitic + X01.Y_analitic + X10.Y_analitic + X11.Y_analitic) / 4) / ((X00.Y_parametr + X01.Y_parametr + X10.Y_parametr + X11.Y_parametr) / 4);
                    sw.WriteLine("Абсолютная ошибка в квадрате [{0},{1},{2},{3}] равняется {4}", X00.X_parameters[0], X11.X_parameters[0], X00.X_parameters[1], X11.X_parameters[1], Math.Abs(Math.Round(Err_abs,3)));
                    sw.WriteLine("Относительная ошибка в квадрате [{0},{1},{2},{3}] равняется {4}", X00.X_parameters[0], X11.X_parameters[0], X00.X_parameters[1], X11.X_parameters[1], Math.Abs(Math.Round(Err_otnos,3)));
                    sw.WriteLine();

                    Sum_abs += Err_abs * Err_abs;
                    Sum_otnos += Err_otnos * Err_otnos;
                    kol++;
                }
            }

            sw.WriteLine("Общее абсолютное значение ошибки между точками равно {0}", Math.Round((Math.Sqrt(Sum_abs / kol)),3));
            sw.WriteLine("Общее относительное значение ошибки между точками равно {0}", Math.Round((Math.Sqrt(Sum_otnos / kol)), 3));
            sw.Close();

        }
        */


        #region функция очистки мусора
        public void prisvoenie()
        {
            temppolynome.Clear();
            for (int i=0;i<polynomString.Length;i++)
            {
                temppolynome.Add(polynomString[i]);
            }
        }

        #endregion
        
        #region Перебор всех сочетаний

        public List<string> combination = new List<string>();
        public void CombinationMass(int n, int k)
        {
            combination.Clear();
            List<double> B = new List<double>();
            int j;
            for (j = 0; j < k; ++j)
            {
                B.Add(j + 1);
            }
            showArray(B, k);
            for (j = k - 1; j >= 0 && B[0] <= n - k; --j)
            {
                if (B[j] < n + j - k + 1)
                {
                    ++B[j];
                    for (int m = j + 1; m < k; ++m)
                    {
                        B[m] = B[m - 1] + 1;
                    }
                    j = k;
                    showArray(B, k);
                }
            }
        }

        public void showArray(List<double> B, int k)
        {
            string temp = "";
            for (int i = 0; i < k; i++)
            {
                temp += B[i];
            }
            combination.Add(temp);
        }
        #endregion
        
        #region работа с распознаванием строки
        List<int> Numbers = new List<int>();

        public int tempint = 0;

        private void stringSeparate(string str)
        {
            Numbers.Clear();
            for (int i = 0; i < str.Length; i++)
            {
                char temp_num = ' ';
                temp_num = str[i];
                int val = (int)Char.GetNumericValue(temp_num);

                if (str.Length / Global.combinationSize == 2)
                {
                    
                    //for (int j = 0; j < Numbers.Count; j++)
                    //{
                        //if (val == Numbers[j] || val < Numbers[j])
                        //{
                            char temp_num1 = ' ';
                            temp_num1 = str[i + 1];
                            string tempstr = temp_num.ToString() + temp_num1.ToString();
                            val = Convert.ToInt32(tempstr);
                            i++;
                            //j = 0;
                            //Numbers.Add(val);
                            //continue;
                        //}
                    //}
                }

                else
                {
                    for (int j = 0; j < Numbers.Count; j++)
                    {
                        if (val == Numbers[j] || val < Numbers[j])
                        {

                            char temp_num1 = ' ';
                            temp_num1 = str[i + 1];
                            string tempstr = temp_num.ToString() + temp_num1.ToString();
                            val = Convert.ToInt32(tempstr);
                            i++;
                            j = 0;
                            continue;
                        }
                    }
                }
                

                Numbers.Add(val);
            }
        }

        #endregion
        
        #region метод исследования всех вариантов полинома
        public string polynome_string;
        List<string> plnm = new List<string>();


        public void polynomeResize(int index)
        {            
            polynome_string = "";
            Array.Resize(ref polynomString, Global.combinationSize /*stringList[index].Count*/);

            List<string> temparray = new List<string>();

            for (int i = 0; i < Global.combinationSize /*stringList[index].Count*/; i++)
            {
                stringSeparate(combination[index]);
                temparray.Add(temppolynome[Numbers[i]-1]/*Convert.ToInt32(stringList[index][i])-1]*/);
            }
            for (int i = 0; i < temparray.Count; i++)
                {
                    polynomString[i] = temparray[i];
                }
            //polynome_string = ("0+");
            for (int i = 0; i < polynomString.Length - 1; i++)
                polynome_string = polynome_string + (polynomString[i] + "+");
            polynome_string = polynome_string + (polynomString[polynomString.Length - 1]);
                plnm.Add(polynome_string);

            polynom = Additionals.CreateListOfIndex(polynomString);
        }

        public void polynomeResizeForHandMode()
        {
            polynome_string = "";
            Array.Resize(ref polynomString, Global.combinationSize+1 /*stringList[index].Count*/);

            List<string> temparray = new List<string>();

            for (int i = 0; i < Global.combinationSize+1 /*stringList[index].Count*/; i++)
            {
                //stringSeparate(combination[index]);
                temparray.Add(Global.tempElements[i] /*temppolynome[Numbers[i] - 1] Convert.ToInt32(stringList[index][i])-1]*/);
            }
            for (int i = 0; i < temparray.Count; i++)
            {
                polynomString[i] = temparray[i];
            }
            //polynome_string = ("0+");
            for (int i = 0; i < polynomString.Length - 1; i++)
                polynome_string = polynome_string + (polynomString[i] + "+");
            polynome_string = polynome_string + (polynomString[polynomString.Length - 1]);
            plnm.Add(polynome_string);

            polynom = Additionals.CreateListOfIndex(polynomString);
        }

        public void getPolynomFromTestBox()
        {

        }
        //Алгоритм по комбинациям написал сам

        //public List<String> copyString(List<string> Temp)
        //{
        //    return new List<string>(Temp);
        //}

        //List<string> B = new List<string>();
        //public void CombinationMass(int n, int k)
        //{
        //    //stringList.Clear();

        //    B.Clear();
        //    int j;
        //    for (j = 0; j < k; ++j)
        //    {
        //        B.Add(Convert.ToString(j + 1));
        //    }

        //    showArray(B);

        //    int k1 = B.Count;
        //    for (int p = 0; p < (n - k); p++)
        //    {
        //        List<string> C = new List<string>(this.B);

        //        for (int i = k1; i < n; i++)
        //        {
        //            C.Add(Convert.ToString(i + 1));
        //            showArray(C);

        //            for (int f = C.Count; f > k; f--)
        //            {
        //                List<string> D = copyString(C);
        //                D.Remove(D[f - 1]);
        //                showArray(D);
        //            }
        //        }
        //        k1++;
        //    }            
        //}

        //List<List<string>> stringList = new List<List<string>>();
        //public void showArray(List<string> B)
        //{

        //    List<string> temp = new List<string>();

        //    for (int i = 0; i < B.Count; i++)
        //    {
        //        temp.Add(B[i]);
        //    }
        //    bool A = false;
        //    for (int i = 0; i < stringList.Count; i++)
        //    {
        //        int kk = 0;
        //        if (stringList[i].Count == temp.Count)
        //        {
        //            for (int j = 0; j < stringList[i].Count; j++)
        //            {

        //                if (stringList[i][j] == temp[j])
        //                {
        //                    kk++;
        //                }
        //            }
        //        }

        //        if (kk == stringList[i].Count)
        //        {
        //            A = true;
        //            break;
        //        }

        //    }
        //    if (A == false)
        //    {
        //        stringList.Add(copyString(temp));
        //    }

        //    Global.finalListSize = stringList.Count;
        //}








        //public void polynomeResize()
        //{
        //    polynome_string = "";
        //    Array.Resize(ref polynomString, 15);

        //    List<string> temparray = new List<string>();

        //    //for (int i = 0; i < Global.combinationSize; i++)
        //    //{
        //    //    stringSeparate(combination[index]);
        //    //    temparray.Add(temppolynome[Numbers[i] - 1]);
        //    //}

        //    for (int i = 0; i < 15; i++)
        //    {
        //        temparray.Add(Convert.ToString(i + 1));
        //    }
        //    for (int i = 0; i < temparray.Count/*Global.combinationSize*/; i++)
        //    {
        //        polynomString[i] = temparray[i];
        //    }
        //    polynome_string = ("0 +");
        //    for (int i = 0; i < polynomString.Length - 1; i++)
        //        polynome_string = polynome_string + (polynomString[i] + " + ");
        //    polynome_string = polynome_string + (polynomString[polynomString.Length - 1]);
        //    plnm.Add(polynome_string);

        //    polynom = Additionals.CreateListOfIndex(polynomString);
        //}
        #endregion
    
        public void CopyGroupList()
        {
            for (int i=0;i<groupList.Count;i++)
            {
                Global.CompleteListToExcel.Add(groupList[i]);
            }
        }

        public void PutPolynomeElementsToList()
        {
            for (int i=0;i<groupList.Count;i++)
            {
                string temp = "";
                string last_elem = "";

                string temp1 = "";

                for (int k = groupList[i].polynomeView.Length-1; k > 0;k-- )
                {
                    
                    if (groupList[i].polynomeView[k] != '+')
                    {
                        temp1 += groupList[i].polynomeView[k];
                    }
                    else
                    {
                        char[] ch = temp1.ToCharArray();
                        Array.Reverse(ch);
                        last_elem = new string(ch);
                        break;
                        //temp1 = "";
                    }
                }


                    for (int j = 0; j < groupList[i].polynomeView.Length; j++)
                    {
                        if (groupList[i].polynomeView[j] != '+')
                        {
                            temp += groupList[i].polynomeView[j];
                            //if (groupList[i].PolynomeMembers[j]!=null)
                            //{
                            //    groupList[i].PolynomeMembers.Remove(groupList[i].PolynomeMembers[j]);
                            //}                        
                            //groupList[i].PolynomeMembers.Add(temp);
                        }
                        else
                        {
                            groupList[i].PolynomeMembers.Add(temp);
                            temp = "";
                        }
                        if (groupList[i].polynomeView[j] == '\0')
                        {
                            groupList[i].PolynomeMembers.Add(temp);
                            temp = "";
                        }
                        //{
                        //    temp += groupList[i].polynomeView[j];
                        //}
                        //if (groupList[i].polynomeView[j]!='\0')
                        //{
                        //    temp += groupList[i].polynomeView[j];
                        //}
                        //else
                        //{
                        //    groupList[i].PolynomeMembers.Add(temp);
                        //    temp = "";
                        //}
                    }
                    groupList[i].PolynomeMembers.Add(last_elem);
            }
        }


    }

}
