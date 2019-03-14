﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sort
{
    public class BenchmarkSort
    {
        IEnumerable<byte> source = new byte[] { 1, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0 };

        [Benchmark]
        public void SortWithIf()
        {
            SortWithIf(source);
        }

        [Benchmark]
        public void SortWithSum()
        {
            SortWithSum(source);
        }

        [Benchmark]
        public void SortWithOneFor()
        {
            SortWithOneFor(source);
        }

        [Benchmark]
        public void SortWithParallel()
        {
            SortWithParallel(source);
        }

        public IEnumerable<byte> SortWithIf(IEnumerable<byte> array)
        {
            int sumOf0 = 0;
            int sumOf1 = 0;

            using (IEnumerator<byte> enumerator = array.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current == 0)
                        sumOf0++;
                    else
                        sumOf1++;
                }
            }

            for (int i = 0; i < sumOf0; i++)
                yield return 0;

            for (int i = 0; i < sumOf1; i++)
                yield return 1;
        }

        public IEnumerable<byte> SortWithSum(IEnumerable<byte> array)
        {
            int sumOf0 = 0;
            int sumOf1 = 0;
            using (IEnumerator<byte> enumerator = array.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    sumOf1 += enumerator.Current;
                    sumOf0++;
                }
            }

            for (int i = 0; i < sumOf0 - sumOf1; i++)
                yield return 0;

            for (int i = 0; i < sumOf1; i++)
                yield return 1;
        }

        public IEnumerable<byte> SortWithOneFor(IEnumerable<byte> array)
        {
            int s = 0;
            using (IEnumerator<byte> enumerator = array.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current == 0)
                        yield return 0;
                    else
                        s++;
                }
            }

            for (int i = 0; i < s; i++)
                yield return 1;
        }

        public IEnumerable<byte> SortWithParallel(IEnumerable<byte> array)
        {
            int sumOf0 = 0;
            int sumOf1 = 0;

            Parallel.ForEach<byte>(array, x => {
                if (x == 0)
                    Interlocked.Increment(ref sumOf0);
                else
                    Interlocked.Increment(ref sumOf1);
            });

            for (int i = 0; i < sumOf0; i++)
                yield return 0;

            for (int i = 0; i < sumOf1; i++)
                yield return 1;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<BenchmarkSort>();
            Console.ReadLine();
        }
    }
}
