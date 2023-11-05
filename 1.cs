using System;

public class Calculator<T>
{
    public delegate T ArithmeticOperation(T a, T b);

    public ArithmeticOperation Add { get; set; }
    public ArithmeticOperation Subtract { get; set; }
    public ArithmeticOperation Multiply { get; set; }
    public ArithmeticOperation Divide { get; set; }

    public Calculator()
    {
        Add = (a, b) => (dynamic)a + b;
        Subtract = (a, b) => (dynamic)a - b;
        Multiply = (a, b) => (dynamic)a * b;
        Divide = (a, b) => (dynamic)(b != 0 ? a / b : throw new ArgumentException("Division by zero"));
    }

    public T PerformOperation(T a, T b, ArithmeticOperation operation)
    {
        return operation(a, b);
    }
}

class Program
{
    static void Main()
    {
        Calculator<int> intCalculator = new Calculator<int>();
        int result = intCalculator.PerformOperation(10, 5, intCalculator.Add);
        Console.WriteLine("Int Add Result: " + result);

        Calculator<double> doubleCalculator = new Calculator<double>();
        double doubleResult = doubleCalculator.PerformOperation(10.5, 0, doubleCalculator.Divide);
        Console.WriteLine("Double Divide Result: " + doubleResult);
    }
}

