﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GenericTensor.Core
{
    public partial class Tensor<TWrapper, TPrimitive>
    {
        public TensorShape Shape { get; private set; }
        private readonly List<int> AxesOrder = new List<int>();

        public void Transpose(int axis1, int axis2)
        {
            (AxesOrder[axis1], AxesOrder[axis2]) = (AxesOrder[axis2], AxesOrder[axis1]);
            Shape.Swap(axis1, axis2);
        }

        public void TransposeMatrix()
        {
            if (Shape.Count < 2)
                throw new InvalidShapeException("this should be at least matrix");
            Transpose(Shape.Count - 2, Shape.Count - 1);
        }

        private int GetFlattenedIndexWithCheck(int[] indecies)
        {
            if (indecies.Length != Shape.Count)
                throw new ArgumentException($"There should be {Shape.Count} indecies, not {indecies.Length}");
            var res = 0;
            for (int i = 0; i < indecies.Length; i++)
            {
                if (indecies[i] < 0 || indecies[i] >= Shape[i])
                    throw new IndexOutOfRangeException($"Bound vialoting: axis {i} is {Shape[i]} long, your input is {indecies[i]}");
                res += Blocks[AxesOrder[i]] * indecies[i];
            }
            if (res >= Data.Length)
                throw new IndexOutOfRangeException();
            return res + LinOffset;
        }

        private int GetFlattenedIndexSilent(int[] indecies)
        {
            var res = 0;
            for (int i = 0; i < indecies.Length; i++)
            {
                res += Blocks[AxesOrder[i]] * indecies[i];
            }
            return res + LinOffset;
        }

    }
}