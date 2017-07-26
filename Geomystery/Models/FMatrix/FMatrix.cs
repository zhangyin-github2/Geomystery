using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geomystery.Models.FMatrix
{
    /// <summary>
    /// 自定义的矩阵，自由的矩阵
    /// </summary>
    public class FMatrix<T> where T : struct, IConvertible
    {
        /// <summary>
        /// 与足够小的数
        /// </summary>
        public static double DBL_EPSILON = 2.2204460492503131e-016;

        /// <summary>
        /// 一个双精度浮点型是否为0
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool IsZero_DBL(double d)
        {
            if (Math.Abs(d) < DBL_EPSILON)          //足够小
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 行
        /// </summary>
        public int row { get; protected set; }

        /// <summary>
        /// 列
        /// </summary>
        public int column { get; protected set; }

        /// <summary>
        /// 矩阵
        /// </summary>
        public List<List<T>> matrix { get; set; }

        /// <summary>
        /// 空构造函数
        /// </summary>
        public FMatrix()
        {
            row = -1;
            column = -1;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="row">行数</param>
        /// <param name="column">列数</param>
        /// <param name="initializeNumber">初始化元素</param>
        public FMatrix(int row, int column, T initializeNumber)
        {
            this.row = row;
            this.column = column;

            if(row > 0 &&column > 0)
            {
                matrix = new List<List<T>>();
                for(int i = 0; i < row; i++)
                {
                    matrix.Add(new List<T>());
                    for(int j = 0; j < column; j++)
                    {
                        matrix[i].Add(initializeNumber);
                    }
                }
            }
        }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="fmatrix"></param>
        public FMatrix(FMatrix<T> fmatrix)
        {
            row = fmatrix.row;
            column = fmatrix.column;
            if(fmatrix.matrix != null)
            {
                matrix = new List<List<T>>();
                for (int i = 0; i < fmatrix.row; i++)
                {
                    matrix.Add(new List<T>());
                    for (int j = 0; j < fmatrix.column; j++)
                    {
                        matrix[i].Add(fmatrix.matrix[i][j]);
                    }
                }
            }
        }

        /// <summary>
        /// 可以后续初始化矩阵一次
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="initializeNumber"></param>
        /// <returns></returns>
        public bool Initialize(int row, int column, T initializeNumber)
        {
            if (matrix != null) return false;

            this.row = row;
            this.column = column;

            if (row > 0 && column > 0)
            {
                matrix = new List<List<T>>();
                for (int i = 0; i < row; i++)
                {
                    matrix.Add(new List<T>());
                    for (int j = 0; j < column; j++)
                    {
                        matrix[i].Add(initializeNumber);
                    }
                }
            }
            return true;                    //初始化成功
        }

        /// <summary>
        /// 一重索引器
        /// </summary>
        /// <param name="r">行</param>
        /// <returns>矩阵的一行</returns>
        public List<T> this[int r]
        {
            get
            { //检查索引范围
                if (matrix != null && r >= 0 && r < this.row)
                {
                    return matrix[r];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 二重索引器
        /// </summary>
        /// <param name="r">行</param>
        /// <param name="c">列</param>
        /// <returns>矩阵的一列</returns>
        public T? this[int r, int c]
        {
            get
            { //检查索引范围
                if (matrix != null && r >= 0 && r < this.row && c >= 0 && c < this.column)
                {
                    return matrix[r][c];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (matrix != null && r >= 0 && r < this.row && c >= 0 && c < this.column)
                {
                    matrix[r][c] = value.Value;
                }
            }
        }

        /// <summary>
        /// 矩阵乘一个数
        /// </summary>
        /// <param name="mat">原矩阵</param>
        /// <param name="number">数</param>
        /// <returns>结果矩阵</returns>
        public static FMatrix<T> MultiplyNumber(FMatrix<T> mat, T number)
        {
            if (mat.matrix != null)
            {
                FMatrix<T> matrix = new FMatrix<T>(mat.row, mat.column, default(T));
                Type type = number.GetType();
                for (int i = 0; i < mat.row; i++)
                {
                    for (int j = 0; j < mat.column; j++)
                    {
                        switch (type.Name)
                        {
                            case "Int32":
                                {
                                    matrix.matrix[i][j] = (T)(object)(Convert.ToInt32(number) * Convert.ToInt32(mat.matrix[i][j]));
                                    break;
                                }
                            case "Double":
                                {
                                    matrix.matrix[i][j] = (T)(object)(Convert.ToDouble(number) * Convert.ToDouble(mat.matrix[i][j]));
                                    break;
                                }
                            case "Single":
                                {
                                    matrix.matrix[i][j] = (T)(object)(Convert.ToSingle(number) * Convert.ToSingle(mat.matrix[i][j]));
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                }
                return matrix;
            }
            return null;
        }

        /// <summary>
        /// 矩阵加法
        /// </summary>
        /// <param name="mata">矩阵A</param>
        /// <param name="matb">矩阵B</param>
        /// <returns>结果矩阵</returns>
        public static FMatrix<T> Add(FMatrix<T> mata, FMatrix<T> matb)
        {
            if (mata.matrix != null && matb.matrix!=null && mata.row == matb.row && mata.column == matb.column)             //行列相同的矩阵
            {
                FMatrix<T> result = new FMatrix<T>(mata.row, mata.column, default(T));

                for (int i = 0; i < mata.row; i++)
                {
                    for (int j = 0; j < mata.column; j++)
                    {
                        switch (typeof(T).Name)
                        {
                            case "Int32":
                                {
                                    result.matrix[i][j] = (T)(object)(Convert.ToInt32(mata.matrix[i][j]) + Convert.ToInt32(matb.matrix[i][j]));
                                    break;
                                }
                            case "Double":
                                {
                                    result.matrix[i][j] = (T)(object)(Convert.ToDouble(mata.matrix[i][j]) + Convert.ToDouble(matb.matrix[i][j]));
                                    break;
                                }
                            case "Single":
                                {
                                    result.matrix[i][j] = (T)(object)(Convert.ToSingle(mata.matrix[i][j]) + Convert.ToSingle(matb.matrix[i][j]));
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 矩阵乘法 AB，左矩阵的列数等于右矩阵的行数
        /// </summary>
        /// <param name="mata">矩阵A</param>
        /// <param name="matb">矩阵B</param>
        /// <returns>结果矩阵</returns>
        public static FMatrix<T> Multiply(FMatrix<T> mata, FMatrix<T> matb)
        {
            if (mata.matrix != null && matb.matrix != null && mata.column == matb.row)             //行列相同的矩阵
            {
                FMatrix<T> result = new FMatrix<T>(mata.row, matb.column, default(T));

                for (int i = 0; i < mata.row; i++)
                {
                    for (int j = 0; j < matb.column; j++)
                    {
                        result.matrix[i][j] = (T)(object)0;
                        for (int k = 0; k < mata.column;k++)
                        {
                            switch (typeof(T).Name)
                            {
                                case "Int32":
                                    {
                                        result.matrix[i][j] = (T)(object)(Convert.ToInt32(result.matrix[i][j]) + Convert.ToInt32(mata.matrix[i][k]) * Convert.ToInt32(matb.matrix[k][j]));
                                        break;
                                    }
                                case "Double":
                                    {
                                        result.matrix[i][j] = (T)(object)(Convert.ToDouble(result.matrix[i][j]) + Convert.ToDouble(mata.matrix[i][k]) * Convert.ToDouble(matb.matrix[k][j]));
                                        break;
                                    }
                                case "Single":
                                    {
                                        result.matrix[i][j] = (T)(object)(Convert.ToSingle(result.matrix[i][j]) + Convert.ToSingle(mata.matrix[i][k]) * Convert.ToSingle(matb.matrix[k][j]));
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }
                    }
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 矩阵乘一个数,重载运算符 *
        /// </summary>
        /// <param name="_matrix">矩阵</param>
        /// <param name="number">数</param>
        /// <returns>矩阵乘数的结果矩阵</returns>
        public static FMatrix<T> operator *(FMatrix<T> _matrix, T number)
        {
            return MultiplyNumber(_matrix, number);
        }
        public static FMatrix<T> operator *(T number, FMatrix<T> _matrix)
        {
            return MultiplyNumber(_matrix, number);
        }

        /// <summary>
        /// 运算符重载，矩阵乘法
        /// </summary>
        /// <param name="mata">左矩阵</param>
        /// <param name="matb">右矩阵</param>
        /// <returns>结果矩阵</returns>
        public static FMatrix<T> operator *(FMatrix<T> mata, FMatrix<T> matb)
        {
            return Multiply(mata, matb);
        }


        /// <summary>
        /// 矩阵相加，重载运算符 +
        /// </summary>
        /// <param name="mata">矩阵A</param>
        /// <param name="matb">矩阵B</param>
        /// <returns>相加结果</returns>
        public static FMatrix<T> operator +(FMatrix<T> mata, FMatrix<T> matb)
        {
            FMatrix<T> mat = FMatrix<T>.Add(mata,matb);
            return mat;
        }

        /// <summary>
        /// 乘一个数
        /// </summary>
        /// <param name="number">整形或者浮点型的数</param>
        public void MultiplyNumber(T number)
        {
            if(matrix!=null)
            {
                Type type = number.GetType();
                for(int i =0; i < row; i++)
                {
                    for (int j = 0; j < column; j++)
                    {
                        switch (type.Name)
                        {
                            case "Int32":
                                {
                                    matrix[i][j] = (T)(object)(Convert.ToInt32(number) * Convert.ToInt32(matrix[i][j]));
                                    break;
                                }
                            case "Double":
                                {
                                    matrix[i][j] = (T)(object)(Convert.ToDouble(number) * Convert.ToDouble(matrix[i][j]));
                                    break;
                                }
                            case "Single":
                                {
                                    matrix[i][j] = (T)(object)(Convert.ToSingle(number) * Convert.ToSingle(matrix[i][j]));
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    } 
                }
            }
        }

        /// <summary>
        /// 余子式
        /// </summary>
        /// <param name="_matrix">原矩阵</param>
        /// <param name="i">行</param>
        /// <param name="j">列</param>
        /// <returns>返回余子式</returns>
        public static FMatrix<T> Mij(FMatrix<T> _matrix, int i, int j)
        {
            FMatrix<T> result = null;
            int nowi = 0, nowj = 0;
            if(_matrix.matrix != null && _matrix.row > 1 && _matrix.column > 1)                          //矩阵可用
            {
                if (i >= 0 && i < _matrix.row && j >= 0 && i < _matrix.column)                      //去掉的行和列
                {
                    result = new FMatrix<T>(_matrix.row - 1, _matrix.column - 1, default(T));
                    for(int a = 0; a < _matrix.row; a++)
                    {
                        if (a < i) nowi = a;
                        else if (a > i) nowi = a - 1;
                        else continue;
                        for (int b = 0; b < _matrix.column; b++)
                        {
                            if (b < j) nowj = b;
                            else if (b > j) nowj = b - 1;
                            else continue;

                            result.matrix[nowi][nowj] = _matrix.matrix[a][b];
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 代数余子式（不推荐使用，因为矩阵的代数余子式与行列式的代数余子式不同）
        /// </summary>
        /// <param name="_matrix">矩阵</param>
        /// <param name="i">行</param>
        /// <param name="j">列</param>
        /// <returns>代数余子式</returns>
        public static FMatrix<T> Aij(FMatrix<T> _matrix, int i, int j)
        {
            FMatrix<T> result = Mij(_matrix,i,j);
            if(result!=null)
            {
                if((i + j)% 2 == 1)
                {
                    result = MultiplyNumber(result, (T)(object)(-1));
                }
            }
            return result;
        }

        public static T? Determinant(FMatrix<T> mat)
        {
            T? tt = default(T);
            if (mat.row != mat.column) return null;             //必须是方阵
            Type type = typeof(T);
            T? tempT = null;
            if(mat.row == 1)
            {
                return mat.matrix[0][0];                        //最后一个（一行一列）
            }
            int flag;
            for (int j = 0; j < mat.column; j++)
            {
                tempT = Determinant(Mij(mat, 0, j));
                if (!tempT.HasValue) return null;               //返回空则返回空（有行列式为空破坏整个行列式求值）
                flag = 1;
                if (j % 2 == 1) flag = -1;
                switch (type.Name)
                {
                    case "Int32":
                        {
                           tt = (T)(object)(Convert.ToInt32(tt) + flag * Convert.ToInt32(mat.matrix[0][j]) * Convert.ToInt32(tempT));
                            break;
                        }
                    case "Double":
                        {
                            tt = (T)(object)(Convert.ToDouble(tt) + flag * Convert.ToDouble(mat.matrix[0][j]) * Convert.ToDouble(tempT));
                            break;
                        }
                    case "Single":
                        {
                            tt = (T)(object)(Convert.ToSingle(tt) + flag * Convert.ToSingle(mat.matrix[0][j]) * Convert.ToSingle(tempT));
                            break;
                        }
                    default:
                        {
                            return null;
                            break;
                        }
                }
            }
            return tt;
        }

        /// <summary>
        /// 计算方阵的行列式
        /// </summary>
        /// <returns>返回|A|，det(A)</returns>
        public T? Determinant()
        {
            T? t = Determinant(this);
            return t;
        }

        /// <summary>
        /// 矩阵的转置
        /// </summary>
        /// <param name="mat">原矩阵</param>
        /// <returns>转置矩阵</returns>
        public static FMatrix<T> Transpose(FMatrix<T> mat)
        {
            FMatrix<T> result = null;
            if(mat.matrix!=null)
            {
                result = new FMatrix<T>(mat.column, mat.row, default(T));
                for(int i = 0; i < mat.row; i++)
                {
                    for(int j = 0; j < mat.column; j++)
                    {
                        result[j][i] = mat[i][j];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 交换矩阵的r1与r2行
        /// </summary>
        /// <param name="mat">原矩阵</param>
        /// <param name="r1">r1行</param>
        /// <param name="r2">r2行</param>
        /// <returns>交换后的矩阵</returns>
        public static FMatrix<T> ExchangeR1R2(FMatrix<T> mat, int r1, int r2)
        {
            FMatrix<T> result = null;
            if (mat.matrix != null && r1 >= 0 && r1 < mat.row && r2 >= 0 && r2 < mat.row)                //可用的矩阵
            {
                result = new FMatrix<T>(mat);
                result.ExchangeR1R2(r1, r2);
            }
            return result;
        }

        /// <summary>
        /// 交换矩阵的r1与r2行
        /// </summary>
        /// <param name="r1">r1行</param>
        /// <param name="r2">r2行</param>
        /// <returns>交换成功</returns>
        public bool ExchangeR1R2(int r1, int r2)
        {
            bool result = false;
            if(matrix!=null && r1 >= 0 && r1 < row && r2 >=0 && r2 < row)                //可用的矩阵
            {
                if(r1 != r2)
                {
                    T temp;
                    for (int j = 0; j < column; j++)
                    {
                        temp = matrix[r1][j];
                        matrix[r1][j] = matrix[r2][j];
                        matrix[r2][j] = temp;
                    }
                }
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 第r行乘k
        /// </summary>
        /// <param name="mat">原矩阵</param>
        /// <param name="r">第r行</param>
        /// <param name="k">系数</param>
        /// <returns>变换后的函数</returns>
        public static FMatrix<T> RowXMultiplyK(FMatrix<T> mat, int r, T k)
        {
            FMatrix<T> result = null;
            result = new FMatrix<T>(mat);
            result.RowXMultiplyK(r, k);
            return result;
        }

        /// <summary>
        /// 第r行乘k
        /// </summary>
        /// <param name="r">第r行</param>
        /// <param name="k">系数</param>
        /// <returns>变换操作是否成功</returns>
        public bool RowXMultiplyK(int r, T k)
        {
            bool result = false;
            Type type = k.GetType();
            if(matrix != null && r >=0 && r < this.row)
            {
                for(int j = 0; j < this.column; j++)
                {
                    switch (type.Name)
                    {
                        case "Int32":
                            {
                                matrix[r][j] = (T)(object)(Convert.ToInt32(matrix[r][j]) * Convert.ToInt32(k));
                                break;
                            }
                        case "Double":
                            {
                                matrix[r][j] = (T)(object)(Convert.ToDouble(matrix[r][j]) * Convert.ToDouble(k));
                                break;
                            }
                        case "Single":
                            {
                                matrix[r][j] = (T)(object)(Convert.ToSingle(matrix[r][j]) * Convert.ToSingle(k));
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 把第r1行的k倍加到r2上
        /// </summary>
        /// <param name="mat">原矩阵</param>
        /// <param name="row1">第row1行</param>
        /// <param name="kTimes">k倍</param>
        /// <param name="row2">第row2行</param>
        /// <returns>增加后的矩阵</returns>
        public static FMatrix<T> AddKTimesOfRow1ToRow2(FMatrix<T> mat, int row1,T kTimes, int row2)
        {
            FMatrix<T> result = null;
            result = new FMatrix<T>(mat);
            result.AddKTimesOfRow1ToRow2(row1, kTimes, row2);
            return result;
        }

        /// <summary>
        /// 把第r1行的k倍加到r2上
        /// </summary>
        /// <param name="row1">第row1行</param>
        /// <param name="kTimes">k倍</param>
        /// <param name="row2">第row2行</param>
        /// <returns>变换是否成功</returns>
        public bool AddKTimesOfRow1ToRow2(int row1, T kTimes, int row2)
        {
            bool result = false;
            Type type = kTimes.GetType();
            if (matrix != null && row1 >= 0 && row1 < this.row && row2 >= 0 && row2 < this.row)             //合理的矩阵与合理的操作
            {
                for (int j = 0; j < this.column; j++)
                {
                    switch (type.Name)
                    {
                        case "Int32":
                            {
                                matrix[row2][j] = (T)(object)(Convert.ToInt32(matrix[row2][j]) + Convert.ToInt32(matrix[row1][j]) * Convert.ToInt32(kTimes));
                                break;
                            }
                        case "Double":
                            {
                                matrix[row2][j] = (T)(object)(Convert.ToDouble(matrix[row2][j]) + Convert.ToDouble(matrix[row1][j]) * Convert.ToDouble(kTimes));
                                break;
                            }
                        case "Single":
                            {
                                matrix[row2][j] = (T)(object)(Convert.ToSingle(matrix[row2][j]) + Convert.ToSingle(matrix[row1][j]) * Convert.ToSingle(kTimes));
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 矩阵的逆矩阵（返回矩阵内部类型一定是double）
        /// </summary>
        /// <param name="mat">原矩阵</param>
        /// <returns>double的逆矩阵</returns>
        public static FMatrix<double> Inverse(FMatrix<T> mat)
        {
            FMatrix<double> result = null;
            bool flag1;                          //“第一位”为空（每一行都必须非0，这样才能保证）
            int searchi;
            if (mat.matrix != null && mat.row == mat.column && mat.row > 0)                  //合理矩阵，方阵，
            {
                result = new FMatrix<double>(mat.row, mat.row, 0.0);
                FMatrix<double> AE = new FMatrix<double>(mat.row , mat.row + mat.row, 0);        //mat放置一个单位矩阵
                for(int i = 0; i < mat.row; i++)
                {
                    for (int j = 0; j < mat.column; j++)
                    {
                        AE[i][j] = Convert.ToDouble(mat[i][j]);
                        if (i == j)
                        {
                            AE[i][j + mat.row] = 1;                      //对角线
                        }
                        else
                        {
                            AE[i][j + mat.row] = 0;
                        }
                        
                    }
                }

                for(int a = 0; a < AE.row; a++)                                 //当前行
                {
                    flag1 = true;
                    
                    for(searchi = a; searchi < AE.row; searchi++)
                    {
                        if(!IsZero_DBL(AE[searchi][a]))
                        {
                            flag1 = false;                       //找到某一行当前位置不是0
                            break;
                        }
                    }
                    if (flag1) return null;                 //某一列全为零,不存在逆矩阵，返回空
                    if(searchi != a)
                    {
                        AE.ExchangeR1R2(searchi, a);            //交换两行，保证不为零
                    }

                    for(int i = 0; i < AE.row; i++)
                    {
                        if (i == a) continue;                           //绕过当前行
                        AE.AddKTimesOfRow1ToRow2(a, -1.0 * AE[i][a] / AE[a][a], i);         //清零当前列
                    }

                    AE.RowXMultiplyK(a, 1.0 / AE[a][a]);                                  //当前行
                }

                for(int i = 0; i < AE.row; i++)
                {
                    for (int j = 0; j < AE.row; j++)
                    {
                        result[i][j] = AE[i][j + AE.row];
                    }
                }

                return result;
            }
            return null;
        }

        /// <summary>
        /// 行最简形
        /// </summary>
        /// <param name="mat">原矩阵</param>
        /// <returns>行最简形式的矩阵</returns>
        public static FMatrix<double> RowSimplestFormOf(FMatrix<T> mat)
        {
            FMatrix<double> result = null;
            bool flag1;
            int searchi;
            if (mat.matrix != null && mat.column > 0 && mat.row > 0)                  //合理矩阵，方阵，
            {
                result = new FMatrix<double>(mat.row, mat.row, 0.0);

                for (int i = 0; i < mat.row; i++)
                {
                    for (int j = 0; j < mat.column; j++)
                    {
                        result[i][j] = Convert.ToDouble(mat[i][j]);
                    }
                }

                for (int a = 0; a < result.row; a++)                                 //当前行
                {
                    flag1 = true;
                    for (searchi = a; searchi < result.row; searchi++)
                    {
                        if (!IsZero_DBL(result[searchi][a]))
                        {
                            flag1 = false;                       //找到某一行当前位置不是0
                            break;
                        }
                        else
                        {
                            result[searchi][a] = 0.0;
                        }
                    }
                    if (flag1) continue;                 //某一列全为零,换下一列
                    if (searchi != a)
                    {
                        result.ExchangeR1R2(searchi, a);            //交换两行，保证不为零
                    }

                    for (int i = 0; i < result.row; i++)
                    {
                        if (i == a) continue;                           //绕过当前行
                        result.AddKTimesOfRow1ToRow2(a, -1.0 * result[i][a] / result[a][a], i);         //清零当前列
                    }

                    result.RowXMultiplyK(a, 1.0 / result[a][a]);                                  //当前行
                }
            }
            return result;
        }

        /// <summary>
        /// 矩阵的秩
        /// </summary>
        /// <param name="mat">待计算矩阵</param>
        /// <returns>矩阵的秩</returns>
        public static int? Rank(FMatrix<T> mat)
        {
            int? result = null;
            bool flag1;
            if(mat.matrix != null && mat.row > 0 && mat.column > 0)
            {
                result = 0;
                FMatrix<double> simple = FMatrix<T>.RowSimplestFormOf(mat);
                int rcmin = (mat.row <= mat.column) ? mat.row : mat.column;
                for(int i = 0; i < rcmin; i++)
                {
                    flag1 = false;
                    for(int j = 0; j < mat.column; j++)
                    {
                        if(simple[i][j] != 0.0)         //非零行
                        {
                            flag1 = true;
                            break;
                        }
                    }
                    if (flag1) result++;
                }
            }
            return result;
        }

    }
    
}
