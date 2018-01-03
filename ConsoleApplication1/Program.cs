using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    
        class GroupPermutations
        {
            static void Main(string[] args)
            {
            //string[] array = { "apple", "peach", "orange" };
            int[] array = { 0, 1 };
                Perm(array, 0);
            Console.ReadKey();
            }

            static void Perm<T>(T[] arr, int k)
            {
                if (k >= arr.Length)
                    Print(arr);
                else
                {
                    Perm(arr, k + 1);
                    for (int i = k + 1; i < arr.Length; i++)
                    {
                        Swap(ref arr[k], ref arr[i]);
                        Perm(arr, k + 1);
                        Swap(ref arr[k], ref arr[i]);
                    }
                }
            }

            private static void Swap<T>(ref T item1, ref T item2)
            {
                T temp = item1;
                item1 = item2;
                item2 = temp;
            }

            private static void Print<T>(T[] arr)
            {
                Console.WriteLine("{" + string.Join(", ", arr) + "}");
            }
        }

       
}
