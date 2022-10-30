using System;

namespace IntegralCalculator
{
    internal class Program
    {
        private static string input;
        private static short len = 32;
        private static string[,] func =
        {
            { "sin", "cos", "tan", "asin", "acos", "atan", "sinh", "cosh", "tanh", "sqrt", "log" , "ln" , "pi" , "e" },
            { "A" ,  "B" ,  "C" ,  "D" ,   "E" ,   "F" ,   "G" ,   "H" ,   "I" ,   "J" ,   "K" ,   "L" ,  "" ,   "" }
        };
        private static double k = 0;
        private static double[] number = new double[0];
        private static char[] operate = new char[0];
        private static bool[] isNumG = new bool[0];
        private static int[,] opcl = new int[0, 0];
        static void Main(string[] args)
        {
            int n = 1000000;
            short nn = (short)MathF.Floor(MathF.Log10(n));
            int m = 10;
            double[] x = new double[3];
            double result = 0;
            double[] numberC;
            char[] operateC;
            bool[] isNumC;
            int[,] opclC;
            func[1, 12] = Math.PI.ToString();
            func[1, 13] = Math.E.ToString();

            Help();
            while (true)
            {
                //함수 입력
                Console.Write("f(x) =  ");
                input = Console.ReadLine();
                input = input.Trim();
                input = input.ToLower();

                //명령어
                if (input == "gg")
                {
                    return;
                }
                else if (input == "help")
                {
                    Help();
                    continue;
                }
                else if (input == "n")
                {
                ReadE:
                    Console.WriteLine("1 이상 {0}이하 자연수 입력. 숫자가 클 수록 높은 정확도, 낮은 성능. (기본값: 1000000)", int.MaxValue);
                    Console.Write("Σ 마지막 항 n = ");
                    n = (int)Math.Round(NumFilter(Console.ReadLine()));
                    if (n < 1)
                    {
                        Console.WriteLine("다시 입력");
                        goto ReadE;
                    }
                    nn = (short)MathF.Floor(MathF.Log10(n));
                    if (nn < 1)
                        nn = 1;
                    
                    if (m > n)
                    {
                        m = n;
                        Console.WriteLine("적분 정확도 {0}(으)로 변경", n);
                        Console.WriteLine("중간 계산 표시 수 {0}개로 자동 변경\n", m);
                    }
                    else
                        Console.WriteLine("적분 정확도 {0}(으)로 변경\n", n);
                    continue;
                }
                else if (input == "k")
                {
                    Console.WriteLine("0 이상 {0}이하 자연수 입력. (기본값: 10)", n);
                    Console.Write("중간 계산 표시 수>> ");
                    m = (int)Math.Round(NumFilter(Console.ReadLine()));
                    if (m > n)
                        m = n;
                    Console.WriteLine("중간 계산 표시 수 {0}개로 변경\n", m);
                    continue;
                }

                //가공
                FuncDetect();
                FxParse();
                numberC = new double[number.Length];
                operateC = new char[operate.Length];
                isNumC = new bool[isNumG.Length];
                opclC = new int[2, opcl.GetLength(1)];
                for (short i = 0; i < number.Length; i++)
                {
                    numberC[i] = number[i];
                    operateC[i] = operate[i];
                    isNumC[i] = isNumG[i];
                }
                if (opcl[0, 0] != -1)
                {
                    for (int i = 0; i < opcl.GetLength(1); i++)
                    {
                        opclC[0, i] = opcl[0, i];
                        opclC[1, i] = opcl[1, i];
                    }

                }
                
                //범위 입력
                Console.Write("x1 =  ");
                x[0] = NumFilter(Console.ReadLine());
                Console.Write("x2 =  ");
                x[1] = NumFilter(Console.ReadLine());

                //x변화량
                x[2] = (x[1] - x[0]) / n;
                Console.WriteLine(" dx = {0}", x[2]);

                //적분 계산
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < numberC.Length; j++)
                    {
                        number[j] = numberC[j];
                        operate[j] = operateC[j];
                        isNumG[j] = isNumC[j];
                    }
                    k = Math.Round(x[0] + x[2] * i + x[2] / 2, nn + 3);

                    //괄호 계산
                    if (opcl[0, 0] != -1)
                    {
                        for (int j = 0; j < opcl.GetLength(1); j++)
                        {
                            opclC[0, j] = opcl[0, j];
                            opclC[1, j] = opcl[1, j];
                        }
                        OpenClose();
                    }

                    //전체 계산
                    Calculate(0, number.Length - 1);

                    //x변화량 곱
                    result += number[0] * x[2];
                    if (m > 0 && (i + 1) % (n / m) == 0)
                        Console.WriteLine(" x_{0}% = {1}\t\tf(x_{0}%) = {2}\t\t현재 값: {3}", MathF.Round((i + 1) / (float)n * 100), k, number[0], Math.Round(result, nn - 1));
                }

                //출력
                result = Math.Round(result, nn - 1);
                Console.WriteLine("최종 값: {0}\n",result);
                result = 0;
            }
        }
        static private void FuncDetect()
        {
            for (short i = 0; i < func.GetLength(1); i++)
            {
                while (input.Contains(func[0,i]))
                    input = input.Replace(func[0,i], func[1, i]);
            }
        }
        static private void FxParse()
        {
            double[] num = new double[len];
            double[] temp = new double[3];
            int[,] oc = new int[2, len];
            char[] sign = new char[len];
            bool[] isNum = new bool[len];
            short index = 0;
            short[] ocIndex = { -1, -1 };
            for (short i = 0; i < sign.Length; i++)
            {
                sign[i] = ' ';
                isNum[i] = false;
            }
            isNum[isNum.Length - 1] = false;
            oc[0, 0] = -1;

            //가공
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '(':
                        if (MultiplyMissed(i, "left"))
                        {
                            input = input.Insert(i, "*");
                            i++;
                        }
                        break;
                    case ')':
                        if (MultiplyMissed(i, "right"))
                            input = input.Insert(i + 1, "*");
                        break;
                    case 'x':
                        if (MultiplyMissed(i, "left"))
                        {
                            input = input.Insert(i, "*");
                            i++;
                        }
                        if (MultiplyMissed(i, "Right"))
                            input = input.Insert(i, "*");
                        break;
                    default:
                        break;
                }
            }
            input = input + ' ';

            //인식
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case ' ':
                        break;
                    case ',':
                        break;
                    case '\t':
                        break;
                    case '(':
                        sign[index] = '(';
                        index++;
                        ocIndex[0]++;
                        oc[0, ocIndex[0]] = index;
                        break;
                    case ')':
                        if (oc[0, 0] != -1)
                        {
                            sign[index] = ')';
                            index++;
                            ocIndex[1]++;
                            oc[1, ocIndex[1]] = index;
                            oc[1, ocIndex[1] + 1] = -1;
                        }
                        break;
                    case '+':
                        sign[index] = '+';
                        index++;
                        break;
                    case '-':
                        sign[index] = '-';
                        index++;
                        break;
                    case '*':
                        sign[index] = '*';
                        index++;
                        break;
                    case '/':
                        sign[index] = '/';
                        index++;
                        break;
                    case '^':
                        sign[index] = '^';
                        index++;
                        break;
                    case '!':
                        sign[index] = '!';
                        index++;
                        break;
                    case 'A':
                        sign[index] = 'A';
                        index++;
                        break;
                    case 'B':
                        sign[index] = 'B';
                        index++;
                        break;
                    case 'C':
                        sign[index] = 'C';
                        index++;
                        break;
                    case 'D':
                        sign[index] = 'D';
                        index++;
                        break;
                    case 'E':
                        sign[index] = 'E';
                        index++;
                        break;
                    case 'F':
                        sign[index] = 'F';
                        index++;
                        break;
                    case 'G':
                        sign[index] = 'G';
                        index++;
                        break;
                    case 'H':
                        sign[index] = 'H';
                        index++;
                        break;
                    case 'I':
                        sign[index] = 'I';
                        index++;
                        break;
                    case 'J':
                        sign[index] = 'J';
                        index++;
                        break;
                    case 'K':
                        sign[index] = 'K';
                        index++;
                        break;
                    case 'L':
                        sign[index] = 'L';
                        index++;
                        break;
                    case 'x':
                        sign[index] = 'x';
                        isNum[index] = true;
                        index++;
                        break;
                    default:
                        temp = NumIdentfy(i);
                        num[index] = temp[0];
                        i = (int)temp[1];
                        if (temp[2] == 1)
                        {
                            isNum[index] = true;
                            index++;
                        }
                        break;
                }
            }

            //반환
            number = new double[index];
            operate = new char[index];
            isNumG = new bool[index];
            for (short i = 0; i < index; i++)
            {
                number[i] = num[i];
                operate[i] = sign[i];
                isNumG[i] = isNum[i];
            }
            if (ocIndex[0] > -1)
            {
                opcl = new int[2, ocIndex[0] + 1];
                for (short i = 0; i < 2; i++)
                {
                    for (short j = 0; j < ocIndex[0]; j++)
                        opcl[i, j] = oc[i, j];
                }
                opcl[0, ocIndex[0]] = int.MaxValue;
                opcl[1, ocIndex[1]] = -1;
            }
            else
                opcl = new int[2, 1] { { -1 }, { -1 } };
            TestPrint(operate, number, isNumG);
            TestPrint2(opcl);
        }
        static private void OpenClose()
        {
            //Console.WriteLine("괄호 계산 진입");
            int[,] arr = new int[opcl.GetLength(0), opcl.GetLength(1)];
            for (short i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                    arr[i, j] = opcl[i, j];
            }
            //TestPrint2(arr);
            for (short i = 0; i < arr.GetLength(1); i++)
            {
                if (arr[0, i] > arr[1, 0])
                {
                    Calculate(arr[0, i - 1], arr[1, 0]);
                    if (arr[1, 1] > -1)
                    {
                        Rearrange(arr, i);
                        i = -1;
                    }
                    else
                        return;
                }
            }
        }
        static private void Calculate(int a, int b)
        {
            //Console.WriteLine("a = {0} b = {1}", a, b);
            //Console.WriteLine("length = {0}", number.Length);
            int n = b - a + 1;
            double[] num = new double[n];
            char[] sign = new char[n];
            bool[] isNum = new bool[n];
            for (int i = a; i <= b; i++)
            {
                //Console.WriteLine("i = {0}", i);
                num[i - a] = number[i];
                sign[i - a] = operate[i];
                isNum[i - a] = isNumG[i];
            }
            //TestPrint(sign, num, isNum);

            //괄호 제거
            if (sign[0] == '(')
            {
                sign[sign.Length - 1] = ')';
                sign = Rearrange(sign, 0, 1);
                num = Rearrange(num, 0, 1);
                isNum = Rearrange(isNum, 0, 1);
                //TestPrint(sign, num, isNum);
            }

            //x 대입
            for (short i = 0; i < sign.Length; i++)
            {
                if (sign[i] == 'x')
                {
                    num[i] = k;
                    sign[i] = ' ';
                    //TestPrint(sign, num, isNum);
                }
            }

            //Sin
            for (int i = 0; i < sign.Length; i++)
            {
                if (sign[i] == 'A')
                {
                    num[i + 1] = Math.Sin(num[i + 1]);
                    sign = Rearrange(sign, i, 1);
                    num = Rearrange(num, i, 1);
                    isNum = Rearrange(isNum, i, 1);
                    //TestPrint(sign, num, isNum);
                }
            }

            //Cos
            for (int i = 0; i < sign.Length; i++)
            {
                if (sign[i] == 'B')
                {
                    num[i + 1] = Math.Cos(num[i + 1]);
                    sign = Rearrange(sign, i, 1);
                    num = Rearrange(num, i, 1);
                    isNum = Rearrange(isNum, i, 1);
                    //TestPrint(sign, num, isNum);
                }
            }

            //Tan
            for (int i = 0; i < sign.Length; i++)
            {
                if (sign[i] == 'C')
                {
                    num[i + 1] = Math.Tan(num[i + 1]);
                    sign = Rearrange(sign, i, 1);
                    num = Rearrange(num, i, 1);
                    isNum = Rearrange(isNum, i, 1);
                    //TestPrint(sign, num, isNum);
                }
            }

            //Asin
            for (int i = 0; i < sign.Length; i++)
            {
                if (sign[i] == 'D')
                {
                    num[i + 1] = Math.Asin(num[i + 1]);
                    sign = Rearrange(sign, i, 1);
                    num = Rearrange(num, i, 1);
                    isNum = Rearrange(isNum, i, 1);
                    //TestPrint(sign, num, isNum);
                }
            }

            //Acos
            for (int i = 0; i < sign.Length; i++)
            {
                if (sign[i] == 'E')
                {
                    num[i + 1] = Math.Acos(num[i + 1]);
                    sign = Rearrange(sign, i, 1);
                    num = Rearrange(num, i, 1);
                    isNum = Rearrange(isNum, i, 1);
                    //TestPrint(sign, num, isNum);
                }
            }

            //Atan
            for (int i = 0; i < sign.Length; i++)
            {
                if (sign[i] == 'F')
                {
                    num[i + 1] = Math.Atan(num[i + 1]);
                    sign = Rearrange(sign, i, 1);
                    num = Rearrange(num, i, 1);
                    isNum = Rearrange(isNum, i, 1);
                    //TestPrint(sign, num, isNum);
                }
            }

            //Sinh
            for (int i = 0; i < sign.Length; i++)
            {
                if (sign[i] == 'G')
                {
                    num[i + 1] = Math.Sinh(num[i + 1]);
                    sign = Rearrange(sign, i, 1);
                    num = Rearrange(num, i, 1);
                    isNum = Rearrange(isNum, i, 1);
                    //TestPrint(sign, num, isNum);
                }
            }

            //Cosh
            for (int i = 0; i < sign.Length; i++)
            {
                if (sign[i] == 'H')
                {
                    num[i + 1] = Math.Cosh(num[i + 1]);
                    sign = Rearrange(sign, i, 1);
                    num = Rearrange(num, i, 1);
                    isNum = Rearrange(isNum, i, 1);
                    //TestPrint(sign, num, isNum);
                }
            }

            //Tanh
            for (int i = 0; i < sign.Length; i++)
            {
                if (sign[i] == 'I')
                {
                    num[i + 1] = Math.Tanh(num[i + 1]);
                    sign = Rearrange(sign, i, 1);
                    num = Rearrange(num, i, 1);
                    isNum = Rearrange(isNum, i, 1);
                    //TestPrint(sign, num, isNum);
                }
            }

            //Sqrt
            for (int i = 0; i < sign.Length; i++)
            {
                if (sign[i] == 'J')
                {
                    num[i + 1] = Math.Sqrt(num[i + 1]);
                    sign = Rearrange(sign, i, 1);
                    num = Rearrange(num, i, 1);
                    isNum = Rearrange(isNum, i, 1);
                    //TestPrint(sign, num, isNum);
                }
            }

            //Log
            for (int i = 0; i < sign.Length; i++)
            {
                if (sign[i] == 'K')
                {
                    if (isNum[i + 1] && isNum[i + 2])
                    {
                        num[i + 2] = Math.Log(num[i + 2], num[i + 1]);
                        sign = Rearrange(sign, i, 2);
                        num = Rearrange(num, i, 2);
                        isNum = Rearrange(isNum, i, 2);
                    }
                    else
                    {
                        num[i + 1] = Math.Log10(num[i + 1]);
                        sign = Rearrange(sign, i, 1);
                        num = Rearrange(num, i, 1);
                        isNum = Rearrange(isNum, i, 1);
                    }
                    //TestPrint(sign, num, isNum);
                }
            }

            //ln
            for (int i = 0; i < sign.Length; i++)
            {
                if (sign[i] == 'L')
                {
                    num[i + 1] = Math.Log(num[i + 1] , Math.E);
                    sign = Rearrange(sign, i, 1);
                    num = Rearrange(num, i, 1);
                    isNum = Rearrange(isNum, i, 1);
                    //TestPrint(sign, num, isNum);
                }
            }

            //팩토리얼
            for (int i = 1; i < sign.Length; i++)
            {
                if (sign[i] == '!')
                {
                    for (int k = (int)(num[i-1] - 1); k > 1; k--)
                        num[i - 1] = num[i - 1] * k;
                    sign = Rearrange(sign, i, 1);
                    num = Rearrange(num, i, 1);
                    isNum = Rearrange(isNum, i, 1);
                    i--;
                    //TestPrint(sign, num, isNum);
                }
            }

            //거듭제곱
            for (int i = 1; i < sign.Length; i = i + 2)
            {
                if (sign[i] == '^')
                {
                    num[i - 1] = Math.Pow(num[i - 1],num[i + 1]);
                    sign = Rearrange(sign, i, 2);
                    num = Rearrange(num, i, 2);
                    isNum = Rearrange(isNum, i, 2);
                    i -= 2;
                    //TestPrint(sign, num, isNum);
                }
                else if (sign[i] == ' ')
                    break;
            }

            //곱하기, 나누기
            for (int i = 1; i < sign.Length; i = i + 2)
            {
                if (sign[i] == '*')
                {
                    num[i - 1] = num[i - 1] * num[i + 1];
                    sign = Rearrange(sign, i, 2);
                    num = Rearrange(num, i, 2);
                    isNum = Rearrange(isNum, i, 2);
                    i -= 2;
                    //TestPrint(sign, num, isNum);
                }
                else if (sign[i] == ' ')
                {
                    if (isNum[i])
                    {
                        num[i - 1] = num[i - 1] * num[i];
                        sign = Rearrange(sign, i, 1);
                        num = Rearrange(num, i, 1);
                        isNum = Rearrange(isNum, i, 1);
                        i -= 2;
                        //TestPrint(sign, num, isNum);
                    }
                    else
                        break;
                }
                else if (sign[i] == '/')
                {
                    num[i - 1] =num[i - 1] / num[i + 1];
                    sign = Rearrange(sign, i, 2);
                    num = Rearrange(num, i, 2);
                    isNum = Rearrange(isNum, i, 2);
                    i -= 2;
                    //TestPrint(sign, num, isNum);
                }
            }

            //더하기, 빼기
            while (true)
            {
                if (sign[1] == '+')
                {
                    num[0] = num[0] + num[2];
                    sign = Rearrange(sign, 1, 2);
                    num = Rearrange(num, 1, 2);
                    //TestPrint(sign, num, isNum);
                }
                else if (sign[1] == '-')
                {
                    num[0] = num[0] - num[2];
                    sign = Rearrange(sign, 1, 2);
                    num = Rearrange(num, 1, 2);
                    //TestPrint(sign, num, isNum);
                }
                else
                    break;
            }

            //결과
            number[a] = num[0];
            operate[a] = ' ';
            isNumG[a] = true;
            for (int i = b + 1; i < number.Length; i++)
            {
                number[i - n] = number[i];
                operate[i - n] = operate[i];
                isNumG[i - n] = isNumG[i];
                isNumG[i] = false;
            }
            //TestPrint(operate, number, isNumG);
        }
        static private double[] NumIdentfy(int n)
        {
            bool c = true;
            bool o = true;
            bool noNum = true;
            short d = 0;
            double num = 0;
            long numD = 0;
            double[] result = new double[3];
            while (c)
            {
                if (o)
                {
                    switch (input[n])
                    {
                        case ',':
                            break;
                        case '0':
                            num = num * 10;
                            noNum = false;
                            break;
                        case '1':
                            num = num * 10 + 1;
                            noNum = false;
                            break;
                        case '2':
                            num = num * 10 + 2;
                            noNum = false;
                            break;
                        case '3':
                            num = num * 10 + 3;
                            noNum = false;
                            break;
                        case '4':
                            num = num * 10 + 4;
                            noNum = false;
                            break;
                        case '5':
                            num = num * 10 + 5;
                            noNum = false;
                            break;
                        case '6':
                            num = num * 10 + 6;
                            noNum = false;
                            break;
                        case '7':
                            num = num * 10 + 7;
                            noNum = false;
                            break;
                        case '8':
                            num = num * 10 + 8;
                            noNum = false;
                            break;
                        case '9':
                            num = num * 10 + 9;
                            noNum = false;
                            break;
                        case 'x':
                            num = k;
                            noNum = false;
                            c = false;
                            break;
                        case '.':
                            o = false;
                            noNum = false;
                            break;
                        default:
                            c = false;
                            break;
                    }
                }
                else
                {
                    switch (input[n])
                    {
                        case ',':
                            break;
                        case '0':
                            numD = numD * 10;
                            d++;
                            break;
                        case '1':
                            numD = numD * 10 + 1;
                            d++;
                            break;
                        case '2':
                            numD = numD * 10 + 2;
                            d++;
                            break;
                        case '3':
                            numD = numD * 10 + 3;
                            d++;
                            break;
                        case '4':
                            numD = numD * 10 + 4;
                            d++;
                            break;
                        case '5':
                            numD = numD * 10 + 5;
                            d++;
                            break;
                        case '6':
                            numD = numD * 10 + 6;
                            d++;
                            break;
                        case '7':
                            numD = numD * 10 + 7;
                            d++;
                            break;
                        case '8':
                            numD = numD * 10 + 8;
                            d++;
                            break;
                        case '9':
                            numD = numD * 10 + 9;
                            d++;
                            break;
                        case '.':
                            break;
                        default:
                            c = false;
                            break;
                    }
                }
                n++;
            }
            if (d > 0)
            {
                num += numD * Math.Pow(10, -d);
            }
            result[0] = num;
            if (noNum)
            {
                result[1] = n-1;
                result[2] = 0;
            }
            else
            {
                result[1] = n-2;
                result[2] = 1;
            }
            return result;
        }
        static private double NumFilter(string s)
        {
            bool o = true;
            short d = 0;
            double num = 0;
            double numD = 0;
            double result = 0;
            for (short i = 0; i < s.Length; i++)
            {
                if (o)
                {
                    switch (s[i])
                    {
                        case '0':
                            num = num * 10;
                            break;
                        case '1':
                            num = num * 10 + 1;
                            break;
                        case '2':
                            num = num * 10 + 2;
                            break;
                        case '3':
                            num = num * 10 + 3;
                            break;
                        case '4':
                            num = num * 10 + 4;
                            break;
                        case '5':
                            num = num * 10 + 5;
                            break;
                        case '6':
                            num = num * 10 + 6;
                            break;
                        case '7':
                            num = num * 10 + 7;
                            break;
                        case '8':
                            num = num * 10 + 8;
                            break;
                        case '9':
                            num = num * 10 + 9;
                            break;
                        case '.':
                            o = false;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (s[i])
                    {
                        case '0':
                            numD = numD * 10;
                            d++;
                            break;
                        case '1':
                            numD = numD * 10 + 1;
                            d++;
                            break;
                        case '2':
                            numD = numD * 10 + 2;
                            d++;
                            break;
                        case '3':
                            numD = numD * 10 + 3;
                            d++;
                            break;
                        case '4':
                            numD = numD * 10 + 4;
                            d++;
                            break;
                        case '5':
                            numD = numD * 10 + 5;
                            d++;
                            break;
                        case '6':
                            numD = numD * 10 + 6;
                            d++;
                            break;
                        case '7':
                            numD = numD * 10 + 7;
                            d++;
                            break;
                        case '8':
                            numD = numD * 10 + 8;
                            d++;
                            break;
                        case '9':
                            numD = numD * 10 + 9;
                            d++;
                            break;
                        case '.':
                            break;
                        default:
                            break;
                    }
                }
            }
            if (d > 0)
                num += numD / Math.Pow(10, d);
            result = num;
            return result;
        }
        static private char[] Rearrange(char[] sign, int i, short times)
        {
            char[] result = sign;
            for (short k = 0; k < times; k++)
            {
                for (int j = i + 1; j < result.Length; j++)
                {
                    result[j - 1] = result[j];
                    result[j] = ' ';
                }
            }
            return result;
        }
        static private double[] Rearrange(double[] num, int i, short times)
        {
            double[] result = num;
            for (short k = 0; k < times; k++)
            {
                for (int j = i + 1; j < result.Length; j++)
                {
                    result[j - 1] = result[j];
                    result[j] = 0;
                }
            }
            return result;
        }
        static private bool[] Rearrange(bool[] isNum, int i, short times)
        {
            bool[] result = isNum;
            for (short k = 0; k < times; k++)
            {
                for (int j = i + 1; j < result.Length; j++)
                {
                    result[j - 1] = result[j];
                    result[j] = false;
                }
            }
            return result;
        }
        static private void Rearrange(int[,] arr, int i)
        {
            for (int j = i + 1; j < arr.GetLength(1); j++)
            {
                arr[0, j - 1] = arr[0, j];
                if (arr[0, j] == int.MaxValue)
                    break;
            }
            for (int j = 1; j < arr.GetLength(1); j++)
            {
                arr[1, j - 1] = arr[1, j];
                if (arr[1, j] == -1)
                    return;
            }
        }
        static private bool MultiplyMissed(int p, string s)
        {
            bool[] a = new bool[2];
            if (s == "left")
            {
                for (int i = p - 1; i >= 0; i--)
                {
                    if (input[i] == '(')
                        return false;
                    else if (input[i] > 64 && input[i] < 63 + func.Length)
                        return false;
                    else
                    {
                        a = MMSwitch(i);
                        if (a[0])
                        {
                            if (a[1])
                                return true;
                            else
                                return false;
                        }
                    }
                    
                }
            }
            else if (s == "right")
            {
                for (int i = p + 1; i < input.Length; i++)
                {
                    if (input[i] == ')')
                        return false;
                    else
                    {
                        a = MMSwitch(i);
                        if (a[0])
                        {
                            if (a[1])
                                return true;
                            else
                                return false;
                        }
                    }
                }
            }
            return false;
        }
        static private bool[] MMSwitch(int i)
        {
            bool[] result = new bool[2];
            switch (input[i])
            {
                case '+':
                    result[0] = true;
                    result[1] = false;
                    break;
                case '-':
                    result[0] = true;
                    result[1] = false;
                    break;
                case '*':
                    result[0] = true;
                    result[1] = false;
                    break;
                case '/':
                    result[0] = true;
                    result[1] = false;
                    break;
                case '!':
                    result[0] = true;
                    result[1] = false;
                    break;
                case '^':
                    result[0] = true;
                    result[1] = false;
                    break;
                case '0':
                    result[0] = true;
                    result[1] = true;
                    break;
                case '1':
                    result[0] = true;
                    result[1] = true;
                    break;
                case '2':
                    result[0] = true;
                    result[1] = true;
                    break;
                case '3':
                    result[0] = true;
                    result[1] = true;
                    break;
                case '4':
                    result[0] = true;
                    result[1] = true;
                    break;
                case '5':
                    result[0] = true;
                    result[1] = true;
                    break;
                case '6':
                    result[0] = true;
                    result[1] = true;
                    break;
                case '7':
                    result[0] = true;
                    result[1] = true;
                    break;
                case '8':
                    result[0] = true;
                    result[1] = true;
                    break;
                case '9':
                    result[0] = true;
                    result[1] = true;
                    break;
                case 'x':
                    result[0] = true;
                    result[1] = true;
                    break;
                default:
                    result[0] = false;
                    result[1] = false;
                    break;
            }
            return result;
        }
        static private void Help()
        {
            Console.WriteLine("더하기 +\t\t빼기 -\t\t\t곱하기 *\t\t나누기 /");
            Console.WriteLine("거듭제곱 ^\t\t팩토리얼 !");
            Console.WriteLine("싸인 sin[수]\t\t코싸인 cos[수]\t\t탄젠트 tan[수]");
            Console.WriteLine("쌍곡싸인 sinh[수]\t쌍곡코싸인 cosh[수]\t쌍곡탄젠트 tanh[수]");
            Console.WriteLine("아크싸인 asin[수]\t아크코싸인 acos[수]\t아크탄젠트 atan[수]");
            Console.WriteLine("제곱근 sqrt[수]\t\t로그 log[밑][진수]\t로그10 log[진수]\t\t자연로그 ln[진수]");
            Console.WriteLine("원주율 pi\t\t자연상수 e\t\t괄호 ( )");
            Console.WriteLine("\n미지수 x만 사용 가능");
            Console.WriteLine("띄어쓰기 가능, 자리수 구분(,) 가능, Tab 가능, 이외의 모든 문자 무시");
            Console.WriteLine("연산 기호 없이 두 수가 이웃할 경우 곱하기 기호 생략으로 간주");
            Console.WriteLine("숫자 사이 자리수 구분 기호(,)가 아닌 다른 특수문자가 올 경우 양 옆 수를 분리된 수로 간주");
            Console.WriteLine("프로그램 종료 gg\t도움말 help\t\t적분 정확도 변경 n\t중간 계산 표시 수 k\n");
        }
        static private void TestPrint(char[] sign, double[] num, bool[] isNum)
        {
            Console.Write("\n");
            for (short i = 0; i < sign.Length; i++)
                Console.Write("{0,2} ", sign[i]);
            Console.Write("\n");
            for (short i = 0; i < num.Length; i++)
                Console.Write("{0,2} ", num[i]);
            Console.Write("\n");
            for (short i = 0; i < isNum.Length; i++)
            {
                if (isNum[i])
                    Console.Write("{0,2} ", "T");
                else
                    Console.Write("{0,2} ", "F");
            }
            Console.Write("\n\n");
        }
        static private void TestPrint2(int[,] index)
        {
            Console.Write("\n");
            for (short j = 0; j < index.GetLength(0); j++)
            {
                for (short k = 0; k < index.GetLength(1); k++)
                    Console.Write("{0,2} ", index[j, k]);
                Console.Write("\n");
            }
            Console.Write("\n\n");
        }
    }
}