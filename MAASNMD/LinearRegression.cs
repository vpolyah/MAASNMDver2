using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearRegression;

namespace MAASNMD
{
    public class LinearRegressionProgram
    {
        public static void mMain(string[] args)
        {
            Console.WriteLine("\nBegin linear regression demo\n");
            int rows = 20;
            int seed = 1;

            Console.WriteLine("Creating " + rows + " rows synthetic data");
            double[][] data = DummyData1(rows, seed);
            Console.WriteLine("Done\n");

            //double[][] data = MatrixLoad("..\\..\\IncomeData.txt", true, ',');

            //Console.WriteLine("Education-Work-Sex-Income data:\n");
            ShowMatrix(data, 2);

            //Console.WriteLine("\nCreating design matrix from data");
            //double[][] design = Design(data); // 'design matrix'
            ////Console.WriteLine("Done\n");

            ////Console.WriteLine("Design matrix:\n");
            ////ShowMatrix(design, 2);

            //Console.WriteLine("\nFinding coefficients using inversion");
            ////double[] coef = Solve(design); // use design matrix
            //Console.WriteLine("Done\n");

            //Console.WriteLine("Coefficients are:\n");
            //ShowVector(coef, 4);
            //Console.WriteLine("");

            //Console.WriteLine("Computing R-squared\n");
            //double R2 = RSquared(data, coef); // use initial data
            //Console.WriteLine("R-squared = " + R2.ToString("F4"));

            //Console.WriteLine("\nPredicting income for ");
            //Console.WriteLine("Education = 14");
            //Console.WriteLine("Work      = 12");
            //Console.WriteLine("Sex       = 0 (male)");

            //double y = Income(14, 12, 0, coef);
            //Console.WriteLine("\nPredicted income = " + y.ToString("F2"));

            //Console.WriteLine("\nEnd linear regression demo\n");
            Console.ReadLine();


        } // Main

        public static double Income(double x1, double x2, double x3, double[] coef)
        {
            // x1 = education, x2 = work, x3 = sex
            double result; // the constant
            result = coef[0] + (x1 * coef[1]) + (x2 * coef[2]) + (x3 * coef[3]);
            return result;
        }

        public static double RSquared(double[][] data, double[] coef)
        {
            // 'coefficient of determination'
            int rows = data.Length;
            int cols = data[0].Length;

            // 1. compute mean of y
            double ySum = 0.0;
            for (int i = 0; i < rows; ++i)
                ySum += data[i][cols - 1]; // last column
            double yMean = ySum / rows;

            // 2. sum of squared residuals & tot sum squares
            double ssr = 0.0;
            double sst = 0.0;
            double y; // actual y value
            double predictedY; // using the coef[] 
            for (int i = 0; i < rows; ++i)
            {
                y = data[i][cols - 1]; // get actual y

                predictedY = coef[0]; // start w/ intercept constant
                for (int j = 0; j < cols - 1; ++j) // j is col of data
                    predictedY += coef[j + 1] * data[i][j]; // careful

                ssr += (y - predictedY) * (y - predictedY);
                sst += (y - yMean) * (y - yMean);
            }

            if (sst == 0.0)
                throw new Exception("All y values equal");
            else
                return 1.0 - (ssr / sst);
        }

        public static double[][] DummyData1(int rows, int seed)
        {
            double[,] X1mas = new double[20, 1];
            X1mas[0, 0] = 30;
            X1mas[1, 0] = 40;
            X1mas[2, 0] = 50;
            X1mas[3, 0] = 60;
            X1mas[4, 0] = 70;
            X1mas[5, 0] = 30;
            X1mas[6, 0] = 40;
            X1mas[7, 0] = 50;
            X1mas[8, 0] = 60;
            X1mas[9, 0] = 70;
            X1mas[10, 0] = 30;
            X1mas[11, 0] = 40;
            X1mas[12, 0] = 50;
            X1mas[13, 0] = 60;
            X1mas[14, 0] = 70;
            X1mas[15, 0] = 30;
            X1mas[16, 0] = 40;
            X1mas[17, 0] = 50;
            X1mas[18, 0] = 60;
            X1mas[19, 0] = 70;

            double[,] X2mas = new double[20, 1];
            X2mas[0, 0] = 1500;
            X2mas[1, 0] = 1500;
            X2mas[2, 0] = 1500;
            X2mas[3, 0] = 1500;
            X2mas[4, 0] = 1500;
            X2mas[5, 0] = 2000;
            X2mas[6, 0] = 2000;
            X2mas[7, 0] = 2000;
            X2mas[8, 0] = 2000;
            X2mas[9, 0] = 2000;
            X2mas[10, 0] = 2500;
            X2mas[11, 0] = 2500;
            X2mas[12, 0] = 2500;
            X2mas[13, 0] = 2500;
            X2mas[14, 0] = 2500;
            X2mas[15, 0] = 3000;
            X2mas[16, 0] = 3000;
            X2mas[17, 0] = 3000;
            X2mas[18, 0] = 3000;
            X2mas[19, 0] = 3000;

            double[,] Ymas = new double[20, 1];
            Ymas[0, 0] = 922.78;
            Ymas[1, 0] = 70.463;
            Ymas[2, 0] = -1090;
            Ymas[3, 0] = -2313;
            Ymas[4, 0] = -3562;
            Ymas[5, 0] = 2911.4;
            Ymas[6, 0] = 1860.3;
            Ymas[7, 0] = 592.82;
            Ymas[8, 0] = -864.53;
            Ymas[9, 0] = -2445;
            Ymas[10, 0] = 5222;
            Ymas[11, 0] = 4178.5;
            Ymas[12, 0] = 2877.3;
            Ymas[13, 0] = 1338.7;
            Ymas[14, 0] = -379;
            Ymas[15, 0] = 7884;
            Ymas[16, 0] = 6899;
            Ymas[17, 0] = 5636.6;
            Ymas[18, 0] = 4080.6;
            Ymas[19, 0] = 2288.0;

            //// generate dummy data for linear regression problem
            //double b0 = 15.0;
            //double b1 = 0.8; // education years
            //double b2 = 0.5; // work years
            //double b3 = -3.0; // sex = 0 male, 1 female
            //double b4 = 0.5; // work years
            //double b5 = -3.0; // sex = 0 male, 1 female
            //Random rnd = new Random(seed);
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[6];

            for (int i = 0; i < rows; ++i)
            {
                //int x1 = rnd.Next(12, 17); // 12, 16]
                //int x2 = rnd.Next(10, 31); // [10, 30]

                //double y = b0 + (b1 * x1) + (b2 * x2) + (b3 * x1 * x2) + (b4 * x1 * x1)+ (b5 * x2 * x2);
                //y += 10.0 * rnd.NextDouble() - 5.0; // random [-5 +5]

                result[i][0] = X1mas[i, 0];
                result[i][1] = X2mas[i, 0];
                result[i][2] = X1mas[i, 0] * X2mas[i, 0];
                result[i][3] = X1mas[i, 0] * X1mas[i, 0];
                result[i][4] = X2mas[i, 0] * X2mas[i, 0];
                result[i][5] = Ymas[i, 0];
            }
            return result;
        }

        public static double[][] DummyData(int rows, int seed)
        {
            // generate dummy data for linear regression problem
            double b0 = 15.0;
            double b1 = 0.8; // education years
            double b2 = 0.5; // work years
            double b3 = -3.0; // sex = 0 male, 1 female
            Random rnd = new Random(seed);

            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[4];

            for (int i = 0; i < rows; ++i)
            {
                int ed = rnd.Next(12, 17); // 12, 16]
                int work = rnd.Next(10, 31); // [10, 30]
                int sex = rnd.Next(0, 2); // 0 or 1
                double y = b0 + (b1 * ed) + (b2 * work) + (b3 * sex);
                y += 10.0 * rnd.NextDouble() - 5.0; // random [-5 +5]

                result[i][0] = ed;
                result[i][1] = work;
                result[i][2] = sex;
                result[i][3] = y; // income
            }
            return result;
        }

        public static double[][] Design(double[][] data)
        {
            // add a leading col of 1.0 values
            int rows = data.Length;
            int cols = data[0].Length;
            double[][] result = MatrixCreate(rows, cols + 1);
            for (int i = 0; i < rows; ++i)
                result[i][0] = 1.0;

            for (int i = 0; i < rows; ++i)
                for (int j = 0; j < cols; ++j)
                    result[i][j + 1] = data[i][j];

            return result;
        }


        //Супер функция регрессии
        public static void /*double[]*/ MultiDim(Tuple<double, double> x, double[] y, bool intercept = false, DirectRegressionMethod method = DirectRegressionMethod.NormalEquations)
        {
            //return SimpleRegression.Fit(x);
            Matrix<double> m = Matrix<double>.Build.Random(3, 4);
            m = m * m;
            m.Transpose();
            m.Inverse();

        }



        public static double[] Solve(double[][] design, int variant)
        {
            // find linear regression coefficients
            // 1. peel off X matrix and Y vector
            int rows = design.Length;
            int cols = design[0].Length;
            double[][] X = MatrixCreate(rows, cols - 1);
            double[][] Y = MatrixCreate(rows, 1); // a column vector

            int j;
            for (int i = 0; i < rows; ++i)
            {
                for (j = 0; j < cols - 1; ++j)
                {
                    X[i][j] = design[i][j];
                }
                Y[i][0] = design[i][j]; // last column

               
            
            }


            //вставка для моего алг
            double[] YY = new double [rows];

            for (int i=0;i<Y.Length;i++)
            {
                YY[i] = Y[i][0];
            }

            //double[] result = 
            //MultiDim(X, YY, false, DirectRegressionMethod.NormalEquations);

            //Console.WriteLine(Result[0]);

            //Matrix<int> m = Matrix<int>.Build.Random(4, 4);
            //m = m * m;
            //m.Transpose();
            //m.Inverse();

            double[] result = MultipleRegression.QR(X, YY);

            if (variant==2)
            {
                result = MultipleRegression.QR(X, YY);
                return result;
            }
            
            if (variant==3)
            {
                result = MultipleRegression.Svd(X, YY);
                return result;
            }
            

            //


            ////test
            //Y[i][0]

            // 2. B = inv(Xt * X) * Xt * y

            if (variant==1)
            {
                double[][] Xt = MatrixTranspose(X);
                double[][] XtX = MatrixProduct(Xt, X);
                double[][] inv = MatrixInverse(XtX);
                double[][] invXt = MatrixProduct(inv, Xt);

                double[][] mResult = MatrixProduct(invXt, Y);
                result = MatrixToVector(mResult);
                return result;
            }


            //for (int i=0;i<result.Length;i++)
            //{
            //    result[i] *= 1000;
            //}


            //StreamWriter sw = new StreamWriter("Regress.txt");
            //sw.Write("Матрица X");
            //sw.WriteLine();
            //for (int i=0;i<X.Length;i++)
            //{
            //    sw.WriteLine();
            //    for (int k=0;k<X[0].Length;k++)
            //    {
            //        sw.Write("\t{0}",X[i][k]);
            //    }
            //}

            //sw.WriteLine();
            //sw.WriteLine();
            //sw.Write("Транспонированная матрица X");
            //sw.WriteLine();
            //for (int i=0;i<Xt.Length;i++)
            //{
            //    sw.WriteLine();
            //    for (int k=0;k<Xt[0].Length;k++)
            //    {
            //        sw.Write("\t{0}",Xt[i][k]);
            //    }
            //}

            //sw.WriteLine();
            //sw.WriteLine();
            //sw.Write("Матрица Xt * X");
            //sw.WriteLine();
            //for (int i=0;i<XtX.Length;i++)
            //{
            //    sw.WriteLine();
            //    for (int k=0;k<XtX[0].Length;k++)
            //    {
            //        sw.Write("\t{0}",XtX[i][k]);
            //    }
            //}

            //sw.WriteLine();
            //sw.WriteLine();
            //sw.Write("Матрица inv(Xt * X)");
            //sw.WriteLine();
            //for (int i=0;i<inv.Length;i++)
            //{
            //    sw.WriteLine();
            //    for (int k=0;k<inv[0].Length;k++)
            //    {
            //        sw.Write("\t{0}",inv[i][k]);
            //    }
            //}

            //sw.WriteLine();
            //sw.WriteLine();
            //sw.Write("Матрица inv(Xt * X) * Xt");
            //sw.WriteLine();
            //for (int i=0;i<invXt.Length;i++)
            //{
            //    sw.WriteLine();
            //    for (int k=0;k<invXt[0].Length;k++)
            //    {
            //        sw.Write("\t{0}",invXt[i][k]);
            //    }
            //}

            //sw.WriteLine();
            //sw.WriteLine();
            // sw.Write("Матрица inv(Xt * X) * Xt * y");
            //sw.WriteLine();
            //for (int i=0;i<mResult.Length;i++)
            //{
            //    sw.WriteLine();
            //    for (int k=0;k<mResult[0].Length;k++)
            //    {
            //        sw.Write("\t{0}",mResult[i][k]);
            //    }
            //}

            //sw.WriteLine();
            //sw.WriteLine();
            //sw.Write("Коэффициенты");
            //sw.WriteLine();
            //for (int i=0;i<result.Length;i++)
            //{
               
            //        sw.Write("\t{0}",result[i]);
                
            //}
            //sw.Close();
            return result;
        } // Solve


        public static void ShowMatrix(double[][] m, int dec)
        {
            for (int i = 0; i < m.Length; ++i)
            {
                for (int j = 0; j < m[i].Length; ++j)
                {
                    Console.Write(m[i][j].ToString("F" + dec) + "  ");
                }
                Console.WriteLine("");
            }
        }

        public static void ShowVector(double[] v, int dec)
        {
            for (int i = 0; i < v.Length; ++i)
                Console.Write(v[i].ToString("F" + dec) + "  ");
            Console.WriteLine("");
        }


        // ===== Matrix routines

        public static double[][] MatrixCreate(int rows, int cols)
        {
            // allocates/creates a matrix initialized to all 0.0
            // do error checking here
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[cols];
            return result;
        }

        // -------------------------------------------------------------

        public static double[][] MatrixRandom(int rows, int cols,
          double minVal, double maxVal, int seed)
        {
            // return a matrix with random values
            Random ran = new Random(seed);
            double[][] result = MatrixCreate(rows, cols);
            for (int i = 0; i < rows; ++i)
                for (int j = 0; j < cols; ++j)
                    result[i][j] = (maxVal - minVal) *
                      ran.NextDouble() + minVal;
            return result;
        }

        // -------------------------------------------------------------

        public static double[][] MatrixLoad(string file, bool header,
          char sep)
        {
            // load a matrix from a text file
            string line = "";
            string[] tokens = null;
            int ct = 0;
            int rows, cols;
            // determined # rows and cols
            System.IO.FileStream ifs =
              new System.IO.FileStream(file, System.IO.FileMode.Open);
            System.IO.StreamReader sr =
              new System.IO.StreamReader(ifs);
            while ((line = sr.ReadLine()) != null)
            {
                ++ct;
                tokens = line.Split(sep); // do validation here
            }
            sr.Close(); ifs.Close();
            if (header == true)
                rows = ct - 1;
            else
                rows = ct;
            cols = tokens.Length;
            double[][] result = MatrixCreate(rows, cols);

            // load
            int i = 0; // row index
            ifs = new System.IO.FileStream(file, System.IO.FileMode.Open);
            sr = new System.IO.StreamReader(ifs);

            if (header == true)
                line = sr.ReadLine();  // consume header
            while ((line = sr.ReadLine()) != null)
            {
                tokens = line.Split(sep);
                for (int j = 0; j < cols; ++j)
                    result[i][j] = double.Parse(tokens[j]);
                ++i; // next row
            }
            sr.Close(); ifs.Close();
            return result;
        }

        // -------------------------------------------------------------

        public static double[] MatrixToVector(double[][] matrix)
        {
            // single column matrix to vector
            int rows = matrix.Length;
            int cols = matrix[0].Length;
            if (cols != 1)
                throw new Exception("Bad matrix");
            double[] result = new double[rows];
            for (int i = 0; i < rows; ++i)
                result[i] = matrix[i][0];
            return result;
        }

        // -------------------------------------------------------------

        public static double[][] MatrixIdentity(int n)
        {
            // return an n x n Identity matrix
            double[][] result = MatrixCreate(n, n);
            for (int i = 0; i < n; ++i)
                result[i][i] = 1.0;

            return result;
        }

        // -------------------------------------------------------------

        public static string MatrixAsString(double[][] matrix, int dec)
        {
            string s = "";
            for (int i = 0; i < matrix.Length; ++i)
            {
                for (int j = 0; j < matrix[i].Length; ++j)
                    s += matrix[i][j].ToString("F" + dec).PadLeft(8) + " ";
                s += Environment.NewLine;
            }
            return s;
        }

        // -------------------------------------------------------------

        public static bool MatrixAreEqual(double[][] matrixA,
          double[][] matrixB, double epsilon)
        {
            // true if all values in matrixA == corresponding values in matrixB
            int aRows = matrixA.Length; int aCols = matrixA[0].Length;
            int bRows = matrixB.Length; int bCols = matrixB[0].Length;
            if (aRows != bRows || aCols != bCols)
                throw new Exception("Non-conformable matrices in MatrixAreEqual");

            for (int i = 0; i < aRows; ++i) // each row of A and B
                for (int j = 0; j < aCols; ++j) // each col of A and B
                    //if (matrixA[i][j] != matrixB[i][j])
                    if (Math.Abs(matrixA[i][j] - matrixB[i][j]) > epsilon)
                        return false;
            return true;
        }

        // -------------------------------------------------------------

        public static double[][] MatrixProduct(double[][] matrixA, double[][] matrixB)
        {
            int aRows = matrixA.Length; int aCols = matrixA[0].Length;
            int bRows = matrixB.Length; int bCols = matrixB[0].Length;
            if (aCols != bRows)
                throw new Exception("Non-conformable matrices in MatrixProduct");

            double[][] result = MatrixCreate(aRows, bCols);

            for (int i = 0; i < aRows; ++i) // each row of A
                for (int j = 0; j < bCols; ++j) // each col of B
                    for (int k = 0; k < aCols; ++k) // could use k < bRows
                        result[i][j] += matrixA[i][k] * matrixB[k][j];

            //Parallel.For(0, aRows, i =>
            //  {
            //    for (int j = 0; j < bCols; ++j) // each col of B
            //      for (int k = 0; k < aCols; ++k) // could use k < bRows
            //        result[i][j] += matrixA[i][k] * matrixB[k][j];
            //  }
            //);

            return result;
        }

         // -------------------------------------------------------------

        public static double[] MatrixVectorProduct(double[][] matrix, double[] vector)
        {
            // result of multiplying an n x m matrix by a m x 1 column vector (yielding an n x 1 column vector)
            int mRows = matrix.Length; int mCols = matrix[0].Length;
            int vRows = vector.Length;
            if (mCols != vRows)
                throw new Exception("Non-conformable matrix and vector in MatrixVectorProduct");
            double[] result = new double[mRows]; // an n x m matrix times a m x 1 column vector is a n x 1 column vector
            for (int i = 0; i < mRows; ++i)
                for (int j = 0; j < mCols; ++j)
                    result[i] += matrix[i][j] * vector[j];
            return result;
        }

        // -------------------------------------------------------------

        public static double[][] MatrixDecompose(double[][] matrix, out int[] perm,
          out int toggle)
        {
            // Doolittle LUP decomposition with partial pivoting.
            // returns: result is L (with 1s on diagonal) and U;
            // perm holds row permutations; toggle is +1 or -1 (even or odd)
            int rows = matrix.Length;
            int cols = matrix[0].Length;
            if (rows != cols)
                throw new Exception("Non-square mattrix");

            int n = rows; // convenience

            double[][] result = MatrixDuplicate(matrix); // 

            perm = new int[n]; // set up row permutation result
            for (int i = 0; i < n; ++i) { perm[i] = i; }

            toggle = 1; // toggle tracks row swaps

            for (int j = 0; j < n - 1; ++j) // each column
            {
                double colMax = Math.Abs(result[j][j]);
                int pRow = j;
                //for (int i = j + 1; i < n; ++i) // deprecated
                //{
                //  if (result[i][j] > colMax)
                //  {
                //    colMax = result[i][j];
                //    pRow = i;
                //  }
                //}

                for (int i = j + 1; i < n; ++i) // reader Matt V needed this:
                {
                    if (Math.Abs(result[i][j]) > colMax)
                    {
                        colMax = Math.Abs(result[i][j]);
                        pRow = i;
                    }
                }
                // Not sure if this approach is needed always, or not.

                if (pRow != j) // if largest value not on pivot, swap rows
                {
                    double[] rowPtr = result[pRow];
                    result[pRow] = result[j];
                    result[j] = rowPtr;

                    int tmp = perm[pRow]; // and swap perm info
                    perm[pRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }

                // -------------------------------------------------------------
                // This part added later (not in original code) 
                // and replaces the 'return null' below.
                // if there is a 0 on the diagonal, find a good row 
                // from i = j+1 down that doesn't have
                // a 0 in column j, and swap that good row with row j

                if (result[j][j] == 0.0)
                {
                    // find a good row to swap
                    int goodRow = -1;
                    for (int row = j + 1; row < n; ++row)
                    {
                        if (result[row][j] != 0.0)
                            goodRow = row;
                    }

                    if (goodRow == -1)
                        throw new Exception("Cannot use Doolittle's method");

                    // swap rows so 0.0 no longer on diagonal
                    double[] rowPtr = result[goodRow];
                    result[goodRow] = result[j];
                    result[j] = rowPtr;

                    int tmp = perm[goodRow]; // and swap perm info
                    perm[goodRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }
                // -------------------------------------------------------------

                //if (Math.Abs(result[j][j]) < 1.0E-20) // deprecated
                //  return null; // consider a throw

                for (int i = j + 1; i < n; ++i)
                {
                    result[i][j] /= result[j][j];
                    for (int k = j + 1; k < n; ++k)
                    {
                        result[i][k] -= result[i][j] * result[j][k];
                    }
                }

            } // main j column loop

            return result;
        } // MatrixDecompose

        // -------------------------------------------------------------

        public static double[][] MatrixInverse(double[][] matrix)
        {
            int n = matrix.Length;
            double[][] result = MatrixDuplicate(matrix);

            int[] perm;
            int toggle;
            double[][] lum = MatrixDecompose(matrix, out perm, out toggle);
            if (lum == null)
                throw new Exception("Unable to compute inverse");

            double[] b = new double[n];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (i == perm[j])
                        b[j] = 1.0;
                    else
                        b[j] = 0.0;
                }

                double[] x = HelperSolve(lum, b); // use decomposition

                for (int j = 0; j < n; ++j)
                    result[j][i] = x[j];
            }
            return result;
        }

        // -------------------------------------------------------------

        public static double[][] MatrixTranspose(double[][] matrix)
        {
            int rows = matrix.Length;
            int cols = matrix[0].Length;
            double[][] result = MatrixCreate(cols, rows); // note indexing
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    result[j][i] = matrix[i][j];
                }
            }
            

            return result;
        } // TransposeMatrix

        // -------------------------------------------------------------

        public static double MatrixDeterminant(double[][] matrix)
        {
            int[] perm;
            int toggle;
            double[][] lum = MatrixDecompose(matrix, out perm, out toggle);
            if (lum == null)
                throw new Exception("Unable to compute MatrixDeterminant");
            double result = toggle;
            for (int i = 0; i < lum.Length; ++i)
                result *= lum[i][i];
            return result;
        }

        // -------------------------------------------------------------

        public static double[] HelperSolve(double[][] luMatrix, double[] b)
        {
            // before calling this helper, permute b using the perm array
            // from MatrixDecompose that generated luMatrix
            int n = luMatrix.Length;
            double[] x = new double[n];
            b.CopyTo(x, 0);

            for (int i = 1; i < n; ++i)
            {
                double sum = x[i];
                for (int j = 0; j < i; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum;
            }

            x[n - 1] /= luMatrix[n - 1][n - 1];
            for (int i = n - 2; i >= 0; --i)
            {
                double sum = x[i];
                for (int j = i + 1; j < n; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum / luMatrix[i][i];
            }

            return x;
        }

        
        // -------------------------------------------------------------

        //static double[] SystemSolve(double[][] A, double[] b)
        //{
        //  // Solve Ax = b
        //  int n = A.Length;

        //  // 1. decompose A
        //  int[] perm;
        //  int toggle;
        //  double[][] luMatrix = MatrixDecompose(A, out perm, out toggle);
        //  if (luMatrix == null)
        //    return null;

        //  // 2. permute b according to perm[] into bp
        //  double[] bp = new double[b.Length];
        //  for (int i = 0; i < n; ++i)
        //    bp[i] = b[perm[i]];

        //  // 3. call helper
        //  double[] x = HelperSolve(luMatrix, bp);
        //  return x;
        //} // SystemSolve

        // -------------------------------------------------------------

        public static double[][] MatrixDuplicate(double[][] matrix)
        {
            // allocates/creates a duplicate of a matrix
            double[][] result = MatrixCreate(matrix.Length, matrix[0].Length);
            for (int i = 0; i < matrix.Length; ++i) // copy the values
                for (int j = 0; j < matrix[i].Length; ++j)
                    result[i][j] = matrix[i][j];
            return result;
        }

        // -------------------------------------------------------------

        public static double[][] ExtractLower(double[][] matrix)
        {
            // lower part of a Doolittle decomp (1.0s on diagonal, 0.0s in upper)
            int rows = matrix.Length; int cols = matrix[0].Length;
            double[][] result = MatrixCreate(rows, cols);
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    if (i == j)
                        result[i][j] = 1.0;
                    else if (i > j)
                        result[i][j] = matrix[i][j];
                }
            }
            return result;
        }

        public static double[][] ExtractUpper(double[][] matrix)
        {
            // upper part of a Doolittle decomp (0.0s in the strictly lower part)
            int rows = matrix.Length; int cols = matrix[0].Length;
            double[][] result = MatrixCreate(rows, cols);
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    if (i <= j)
                        result[i][j] = matrix[i][j];
                }
            }
            return result;
        }

        // -------------------------------------------------------------

        public static double[][] PermArrayToMatrix(int[] perm)
        {
            // convert Doolittle perm array to corresponding perm matrix
            int n = perm.Length;
            double[][] result = MatrixCreate(n, n);
            for (int i = 0; i < n; ++i)
                result[i][perm[i]] = 1.0;
            return result;
        }

        public static double[][] UnPermute(double[][] luProduct, int[] perm)
        {
            // unpermute product of Doolittle lower * upper matrix according to perm[]
            // no real use except to demo LU decomposition, or for consistency testing
            double[][] result = MatrixDuplicate(luProduct);

            int[] unperm = new int[perm.Length];
            for (int i = 0; i < perm.Length; ++i)
                unperm[perm[i]] = i;

            for (int r = 0; r < luProduct.Length; ++r)
                result[r] = luProduct[unperm[r]];

            return result;
        } // UnPermute


        // =====

    } // Program
}
