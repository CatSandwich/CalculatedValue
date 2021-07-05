using System;

namespace CalculatedValue
{
    internal class Program
    {
        private static void Main()
        {
            // Simple example
            var cv = new CalculatedValue<int>(2, 5); // 5
            cv[0] += (ref int val) => val += 2; // 7
            cv[0] += (ref int val) => val += 3; // 10
            cv[1] += (ref int val) => val *= 3; // 30
            Console.WriteLine(cv.Calculate());

            // Example with random (to demonstrate use case of CalculatedValue<double> vs. double)
            var cv2 = new CalculatedValue<double>(2, 0f); // 0f
            var r = new Random();
            cv2[0] += (ref double val) => val += r.NextDouble() * 5f; // 0f-5f
            cv2[1] += (ref double val) => val *= 2;                   // 0f-10f
            Console.WriteLine(cv2.Calculate().ToString("F2"));

            // Example with custom type
            var cv3 = new CalculatedValue<Example>(2, new Example(5)); // 5
            cv3[0] += (ref Example val) => val.Value += 2; // 7
            cv3[0] += (ref Example val) => val.Value += 3; // 10
            cv3[1] += (ref Example val) => val.Value *= 3; // 30
            Console.WriteLine(cv3.Calculate().Value);
        }
    }
    
    internal class Example : IEquatable<Example>
    {
        public int Value;

        public Example(int val)
        {
            Value = val;
        }

        public bool Equals(Example other) => !(other is null) && Value == other.Value;
    }
}
