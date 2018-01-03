using SequenceGenerator;
using StringAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreesDataGenerator
{
    class Program
    {
        private static void WriteToFile(string path, List<Sequence> sequences)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path))
            {
                foreach (Sequence seq in sequences)
                {
                    file.WriteLine(seq.Name);
                    file.WriteLine(seq.Value);
                }
            }
        }

        static void Main(string[] args)
        {

            Console.WriteLine("I have still been working...");
            for (int i = 100; i <= 3000; i += 100)
            {
                for (int k = 0; k < 3; ++k)
                {
                    List<Sequence> sequences = new List<Sequence>();
                    for (int j = 0; j < i; ++j)
                    {
                        DnaGenerator generator = new DnaGenerator();
                        string str = generator.GenerateSequence(200);
                        Sequence seq = new Sequence(StringAlgorithms.Constants.DNA, $"seq{j}", str);
                        sequences.Add(seq);
                    }
                    WriteToFile($@"C:\Users\Me\Desktop\TestTreeData\New\seq{i}_{k}.fasta", sequences);
                }
                Console.WriteLine($"{i} done");
            }
            Console.WriteLine("I am done ;)");
            Console.ReadKey();
        }
    }
}
