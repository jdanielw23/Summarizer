using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Andrew_s_Implementation
{
    /// <summary>
    /// This is a two-dimensional matrix class.
    /// Only built to be used as it is by classes in
    /// namespace Summarizer.Model.Andrew_s_Implementation
    /// </summary>
    public class Matrix
    {
        private int[,] matrix;
        private int cols;
        private int rows;

        public Matrix(int rows, int cols)
        {
            matrix = new int[rows, cols];
            this.rows = rows;
            this.cols = cols;
        }

        public void Set(int row, int col, int val)
        {
            if (row <= rows && col <= cols)
            {
                matrix[row, col] = val;
            }
        }

        public int Get(int row, int col)
        {
            if (row < rows && col < cols)
            {
                return matrix[row, col];
            }
            return 0;
        }

        public Matrix Dot(Matrix that)
        {
            if (this.cols != that.rows) return null;
            int m = this.rows;
            int n = this.cols; // == that.rows
            int p = that.cols;
            Matrix product = new Matrix(m, p);
            // going through rows of this
            for (int i = 0; i < m; i++)
            {
                // going through columns of that
                for (int j = 0; j < p; j++)
                {
                    // going through columns of this (aka rows of that)
                    int cell = 0;
                    for (int k = 0; k < n; k++)
                    {
                        cell += (this.Get(i, k) * that.Get(k, j));
                    }
                    product.Set(i, j, cell);
                }
            }
            return product;
        }

        public Matrix Transpose()
        {
            Matrix tran = new Matrix(this.cols, this.rows);
            for (int i = 0; i < tran.rows; i++)
            {
                for (int j = 0; j < tran.cols; j++)
                {
                    tran.Set(i, j, this.Get(j, i));
                }
            }
            return tran;
        }

        // Returns false if matrix is not square,
        // Otherwise, returns true and sets diagonal to zeroes
        public bool ZeroDiagonal()
        {
            if (cols != rows)
            {
                return false;
            }
            for (int i = 0; i < rows; i++)
            {
                this.Set(i, i, 0);
            }
            return true;
        }
    }
}
