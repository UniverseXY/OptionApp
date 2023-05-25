using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class Option
    {
            private double B0, S0, K, a, r, b, SCurr, BCurr;
            public bool isLastStep = false; // флаг последнего шага
            private int currStep = 0; // текущий шаг
            public int N { get; }
            private double P; // p* мера
            double[] gammaArr, betaArr; // массивы со значениями бет и гамм
            double[] SHistory; // массив, хранязий историю значений для акций 

        public Option(int N, double B0, double S0, double K, double a, double r, double b)
        {
            this.N = N;
            this.B0 = B0;
            this.S0 = S0;
            this.K = K;
            this.a = a;
            this.r = r;
            this.b = b;
            P = EvaluateP();
            gammaArr = new double[N+1];
            betaArr = new double[N+1];
            SHistory = new double[N+1];
            SCurr = S0;
            SHistory[0] = S0;
            BCurr = B0;
        }
        
        // Функция увеличения шага
        public void StepUp (bool key)
        {
            currStep++;
            SetB();
            if (key == false)
                aS();
            else bS();
            SHistory[currStep] = SCurr;
            if (currStep == N) isLastStep = true;
        }
        
        // Функция уменьшения шага
        public void StepDown()
        {
            currStep--;
            SetB();
            SCurr = SHistory[currStep];
            isLastStep = false;
        }
        
        // Устанавливаем значение счета в облигациях для текущего шага
        private void SetB()
        {
            BCurr = B0 * Math.Pow(1 + r, currStep);
        }

        // Получаем значение цены акции
        public double GetSPrice()
        {
            return SCurr;
        }
        
        // Получение текущего значение счета в облигациях
        public double GetB()
        {
            return BCurr;
        }
        
        // Повышение/понижение цены акции на a
        private void aS()
        {
            SCurr *= (1 + a);
        }
        
        // Повышение цены акции на b
        private void bS()
        {
            SCurr *= (1 + b);
        }

        // Получение текущей гаммы
        public double GetCurrentGamma()
        {   
            return gammaArr[currStep];
        }

        // Получение текущей беты
        public double GetCurrentBeta()
        {
            return betaArr[currStep];
        }

        // Получение предыдущей беты
        public double GetPrevBeta()
        {
            return betaArr[currStep - 1];
        }
        
        
        // Получение предыдущей гаммы
        public double GetPrevGamma()
        {
            return gammaArr[currStep-1];
        }

        // Получение значения текущего шага
        public int GetCurrentStep()
        {
            return currStep;    
        }
        // Перераспределение портфеля с учетом поступившей информации
        public void RedistributePortfolio()
        {
            if (isLastStep == false)
            {
                gammaArr[currStep] = EvaluateGamma(currStep + 1, SCurr);
                betaArr[currStep] = EvaluateBeta(currStep + 1, SCurr);
            }
            else
            {
                gammaArr[currStep] = gammaArr[currStep - 1];
                betaArr[currStep] = betaArr[currStep - 1];
            }
        }
        

        // Функция вычисления гаммы
        private double EvaluateGamma(int n, double prevS)
        {
            return Math.Pow(1 + r, -(N - n)) * (EvaluateF(N - n, prevS * (1 + b), P) - EvaluateF(N - n, prevS * (1 + a), P))
            / (prevS * (b - a));
        }
        
        // Функция вычисления беты
        private double EvaluateBeta(int n, double prevS)
        {
            double lastB = B0 * Math.Pow(1 + r, N);
            return (EvaluateF(N - n + 1, prevS, P) -((1+r) / (b-a) )* (EvaluateF(N - n, prevS * (1 + b), P)
            - EvaluateF(N - n, prevS * (1 + a), P))) / lastB;
        }

        // Вычисление рациональной стоимости опциона
        public double EvaluateRationalPrice()
        {
            return Math.Pow(1 + r, -N) * EvaluateF(N, S0, P);
        }

        // Вычисление значения функции F_n(x, p)
        private double EvaluateF(int n, double x, double p)
        {
            double result = 0;
            for (int k = 0; k <= n; k++)
            {
                result += EvaluatePayoutFunction(x * Math.Pow(1 + b, k) * Math.Pow(1 + a, n - k)) *
                EvaluateBinomCoeff(n, k) * Math.Pow(p, k) * Math.Pow(1 - p, n - k);
            }
            return result;
        }

        // Вычисление новой меры p*
        private double EvaluateP()
        {
            return (r - a) / (b - a);
        }

        // Вычисление функции выплаты f = (S - K)+
        public double EvaluatePayoutFunction(double S)
        {
            return Math.Max(0, S - K);
        }

        // Вычисление биномиальных коэффициентов
        private double EvaluateBinomCoeff(int n, int k)
        {
            return Factorial(n) / (Factorial(k) * Factorial(n - k));
        }

        // Вычисление факториала числа
        private int Factorial(int n)
        {
            if (n == 1 || n == 0) return 1;

            return n * Factorial(n - 1);
        }
}

