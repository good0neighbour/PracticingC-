using System;

namespace IntegralCalculator
{
    internal class Program
    {
        private static string input;
        private static short len = 31;
        private static string[,] func = {{ "sin", "cos", "tan", "asin", "acos", "atan", "sinh", "cosh", "tanh", "sqrt", "log" },
            { "A" , "B" , "C", "D", "E" , "F" , "G" , "H" , "I" , "J" , "K" } };
        static void Main(string[] args)
        {
            int[] sign = new int[2];
            int[,] index = new int[sign.Length,len];
            int n = 100000;
            double[] x = new double[3];
            double result = 0;
            string copy;

            Help();
            while (true)
            {
                result = 0;

                //함수 입력
                Console.Write("f(x) =  ");
                input = Console.ReadLine();
                input = input.Trim();
                input = input.ToLower();

                //명령어
                input = input.Trim();
                if (input == "gg")
                    break;
                else if (input == "help")
                {
                    Help();
                    continue;
                }
                else if (input == "n")
                {
                    Console.WriteLine("1 이상 자연수 입력. 숫자가 클 수록 높은 정확도, 낮은 성능. (기본값: 100000)");
                    n = Math.Abs(int.Parse(Console.ReadLine()));
                    Console.WriteLine("적분 정확도 {0}(으)로 변경\n", n);
                    continue;
                }

                //괄호 변수 초기 값
                for (short i = 0; i < sign.Length; i++)
                    sign[i] = -1;
                for (short i = 0; i < len; i++)
                {
                    index[0,i] = int.MaxValue;
                    index[1, i] = -1;
                }

                //가공
                FuncDetect();
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
                            sign[0]++;
                            index[0, sign[0]] = i;
                            break;
                        case ')':
                            if (MultiplyMissed(i, "right"))
                                input = input.Insert(i + 1, "*");
                            sign[1]++;
                            index[1, sign[1]] = i;
                            break;
                        case 'x':
                            if (MultiplyMissed(i, "left"))
                            {
                                input = input.Insert(i, "*");
                                i++;
                            }
                            break;
                        default:
                            break;
                    }
                }
                input = input + ' ';
                copy = input;
                
                //범위 입력
                Console.Write("x1 =  ");
                x[0] = double.Parse(Console.ReadLine());
                Console.Write("x2 =  ");
                x[1] = double.Parse(Console.ReadLine());

                //x변화량
                x[2] = (x[1] - x[0]) / n;

                //적분 계산
                for (int i = 0; i < n; i++)
                {
                    input = copy;

                    //x대입
                    while (input.Contains("x"))
                        input = input.Replace("x", (x[2] * i).ToString());

                    //괄호 계산
                    if (sign[1] > -1)
                        OpenClose(index);

                    //전체 계산
                    Calculate(0, input.Length-2);
                    input = input.Trim();

                    //x변화량 곱
                    result += double.Parse(input) * x[2];
                }
                //출력
                result = Math.Round(result * 100) / 100;
                Console.WriteLine("값: {0}\n",result);
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
        static private void OpenClose(int[,] arr)
        {
            int[,] index = arr;
            for (int i = 0; i < len; i++)
            {
                if (index[1, i] > -1)
                {
                    for (int j = 0; j < len; j++)
                    {
                        if (index[0, j] > index[1, i])
                        {
                            Calculate(index[0, j - 1], index[1, i]);
                            index[0, j - 1] = -1;
                            break;
                        }
                    }
                }
                else
                    break;
            }
        }
        static private void Calculate(int a,int b)
        {
            double[] num = new double[len+1];
            char[] sign = new char[len];
            bool[] isNum = new bool[len+1];
            int index = 0;
            double[] temp = new double[3];
            for (int i = 0; i < sign.Length; i++)
            {
                sign[i] = ' ';
                isNum[i] = false;
            }
            isNum[len] = false;

            //인식
            for (int i = a; i <= b; i++)
            {
                switch (input[i])
                {
                    case '(':
                        break;
                    case ')':
                        break;
                    case ' ':
                        break;
                    case ',':
                        break;
                    case '\t':
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
                    default:
                        temp = NumIdentfy(i);
                        num[index] = temp[0];
                        i = (int)temp[1];
                        isNum[index] = true;
                        if (temp[2] == 1)
                            index++;
                        break;
                }
            }
            //TestPrint(sign, num, isNum);
            
            //Sin
            for (int i = 0; i < len; i++)
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
            for (int i = 0; i < len; i++)
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
            for (int i = 0; i < len; i++)
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
            for (int i = 0; i < len; i++)
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
            for (int i = 0; i < len; i++)
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
            for (int i = 0; i < len; i++)
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
            for (int i = 0; i < len; i++)
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
            for (int i = 0; i < len; i++)
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
            for (int i = 0; i < len; i++)
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
            for (int i = 0; i < len; i++)
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

            //팩토리얼
            for (int i = 1; i < len; i++)
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
            for (int i = 1; i < len; i = i + 2)
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
            for (int i = 1; i < len; i = i + 2)
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
                    num[i - 1] = num[i - 1] / num[i + 1];
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
            input = input.Remove(a,b-a+1);
            input = input.Insert(a,num[0].ToString());
            //Console.WriteLine(input);
        }
        static private double[] NumIdentfy(int n)
        {
            bool c = true;
            bool o = true;
            bool noNum = true;
            short d = 0;
            double num = 0;
            double numD = 0;
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
                num += numD / Math.Pow(10,d);
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
        static private char[] Rearrange(char[] sign, int i, short times)
        {
            char[] result = sign;
            for (int k = 0; k < times; k++)
            {
                for (int j = i + 1; j < len; j++)
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
            for (int k = 0; k < times; k++)
            {
                for (int j = i + 1; j < len*1; j++)
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
            for (int k = 0; k < times; k++)
            {
                for (int j = i + 1; j < len+1; j++)
                {
                    result[j - 1] = result[j];
                    result[j] = false;
                }
            }
            return result;
        }
        static private bool MultiplyMissed(int p, string s)
        {
            bool[] a = new bool[2];
            if (s == "left")
            {
                for (int i = p - 1; i >= 0; i--)
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
            else if (s == "right")
            {
                for (int i = p + 1; i < input.Length; i++)
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
                default:
                    result[0] = false;
                    result[1] = false;
                    break;
            }
            return result;
        }
        static private void Help()
        {
            Console.WriteLine("더하기 +\t빼기 -\t\t곱하기 *\t나누기 /");
            Console.WriteLine("거듭제곱 ^\t팩토리얼 !");
            Console.WriteLine("sin sin()\tcos cos()\ttan tan()");
            Console.WriteLine("sinh sinh()\tcosh cosh()\ttanh tanh()");
            Console.WriteLine("arcsin asin()\tarccos acos()\tarctan atan()");
            Console.WriteLine("제곱근 sqrt()\t괄호 ( )");
            Console.WriteLine("미지수 x만 사용 가능");
            Console.WriteLine("띄어쓰기 가능\t자리수 구분(,) 가능\tTab 가능\t이외의 모든 문자 무시");
            Console.WriteLine("연산 기호 없이 두 수가 이웃할 경우 곱하기 기호 생략으로 간주");
            Console.WriteLine("숫자 사이 자리수 구분 기호(,)가 아닌 다른 특수문자가 올 경우 양 옆 수를 분리된 수로 간주");
            Console.WriteLine("프로그램 종료 gg\t도움말 help\t적분 정확도 변경 n\n");
        }
        static private void TestPrint(char[] sign, double[] num, bool[] isNum)
        {
            for (int i = 0; i < sign.Length; i++)
                Console.Write("{0,2} ", sign[i]);
            Console.Write("\n");
            for (int i = 0; i < num.Length; i++)
                Console.Write("{0,2} ", num[i]);
            Console.Write("\n");
            for (int i = 0; i < isNum.Length; i++)
            {
                if (isNum[i])
                    Console.Write("{0,2} ", "T");
                else
                    Console.Write("{0,2} ", "F");
            }
            Console.Write("\n");
        }
    }
}