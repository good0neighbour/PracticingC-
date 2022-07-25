using System;

namespace GaussianElimination
{
    internal class Program
    {
        private static string input;
        private static short n = 0;
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("미지수 개수 입력 혹은 0을 입력하여 프로그램 종료: ");
                input = Console.ReadLine();
                n = short.Parse(input);
                if (n > 0)
                {
                    GaussianSequance(n);
                }
                else
                    break;
            }
        }
        private static void GaussianSequance(short p)
        {
            float[,] a = new float[p+1,p];
            float[,] b = new float[p+1,p];
            float[] s = new float[p];
            float t = 0;
            string m;

            //입력 예시
            Console.Write("\n입력할 식:\n");
            for (int i = 0; i < p; i++)
            {
                Console.Write(" ( )x1");
                for (int j = 1; j < p; j++)
                    Console.Write(" + ( )x{0}", j + 1);
                Console.Write(" = ( )\n");
            }

            //사용자 입력
            for (int i = 0; i < p; i++)
            {
                Console.Write("\n식{0}\n", i+1);
                for (int j = 0; j < p; j++)
                {
                    Console.Write("x{0} 입력: ", j + 1);
                    m = Console.ReadLine();
                    a[j,i] = float.Parse(m);
                }
                Console.Write("상수{0} 입력: ", i+1);
                m = Console.ReadLine();
                a[p, i] = float.Parse(m);
            }

            //입력한 식
            Console.Write("\n입력한 식:\n");
            for (int i = 0; i < p; i++)
            {
                Console.Write(" ({0})x1", a[i,0]);
                for (int j = 1; j < p; j++)
                    Console.Write(" + ({0})x{1}", a[j,i], j + 1);
                Console.Write(" = ({0})\n", a[p,i]);
            }

            //식 검사
            for (int i = 0; i < p; i++)
                if (a[i, i] == 0)
                    t++;
            if (t > 0)
            {
                Console.Write("\n좌측 상단에서 우측 하단까지 대각선 상에 위치한 계수에 0이 들어갈 수 없습니다.\n식의 순서를 바꾸거나 다른 식을 입력하십시오.\n\n");
                return;
            }

            //계산
            for (int i = 0; i < p+1; i++)
                b[i,0] = a[i,0];
            for (int i = 0; i < p-1; i++)
            {
                t = a[i + 1, i] / a[i, i];
                for (int j = i; j < p+1; j++)
                    b[j, i+1] = a[j, i+1] - a[j,i] * t;
            }
            s[p-1] = b[p,p-1] / b[p-1,p-1];
            for (int i = p-2; i >= 0; i--)
            {
                for (int j = p-1; j > i; j--)
                    s[i] += b[j, i] * s[j];
                s[i] = (b[p,i] - s[i]) / b[i,i];
            }

            //결과
            for (int i = 0; i < p; i++)
            {
                if (float.IsNaN(s[i]))
                    t = 0;
                else if (float.IsInfinity(s[i]))
                    t= 1;
            }
            Console.Write("\n결과:\n");
            if (t == 0)
                Console.Write("해가 무수히 많습니다.\n\n다른 연립방정식의 ");
            else if (t == 1)
                Console.Write("해가 없습니다.\n\n다른 연립방정식의 ");
            else
            {
                for (int i = 0; i < p; i++)
                    Console.Write("x{0} = {1}\n", i+1, s[i]);
                Console.Write("\n다른 연립방정식의 ");
            }
        }
    }
}
