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
            if(fmatrix.matrix!=null)
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
        public static FMatrix<T> Multiply(FMatrix<T> mat, T number)
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
        /// 矩阵乘一个数,重载运算符 *
        /// </summary>
        /// <param name="_matrix">矩阵</param>
        /// <param name="number">数</param>
        /// <returns>矩阵乘数的结果矩阵</returns>
        public static FMatrix<T> operator *(FMatrix<T> _matrix, T number)
        {
            return Multiply(_matrix, number);
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
        public void Multiply(T number)
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
        /// 代数余子式
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
                    result = Multiply(result, (T)(object)(-1));
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

    }

}
