using SimpleBalance.Models;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBalance.Classes
{
    public class InputConverter
    {
        private BalanceInput Input;

        /// <summary>
        ///  Измеренные значения потоков
        /// </summary>
        public double[] X0 { get; set; }

        /// <summary>
        ///  Матрица инцидентности / связей
        /// </summary>
        public double[,] A { get; set; }

        /// <summary>
        ///  Вектор точностей
        /// </summary>
        public double[] T { get; set; }

        /// <summary>
        ///  Вектор измеряемости (I)
        /// </summary>
        public double[] I { get; set; }

        /// <summary>
        ///  Вектор с нижними ограничениями
        /// </summary>
        public double[] Lower { get; set; }

        /// <summary>
        ///  Вектор с верхними ограничениями
        /// </summary>
        public double[] Upper { get; set; }

        /// <summary>
        /// Список узлов
        /// </summary>
        public List<string> NodesList { get; set; }

        /// <summary>
        /// Количество потоков
        /// </summary>
        public int CountOfThreads { get; private set; }

        /// <summary>
        /// Количество узлов
        /// </summary>
        public int CountOfNodes { get; private set; }

        public InputConverter(BalanceInput input)
        {
            Input = input;
            CreateNodesList();
            CountOfThreads = Input.Flows.Count;
            CountOfNodes = NodesList.Count;
        }

        /// <summary>
        /// Конвертирует входные данные в матрицы
        /// </summary>
        public void Convert()
        {
            X0 = ConvertX0();
            A = ConvertA();
            I = ConvertI();
            T = ConvertT();
            Lower = ConvertLower();
            Upper = ConvertUpper();
        }

        /// <summary>
        /// Конвертирование измеренных значений
        /// </summary>
        /// <returns></returns>
        private double[] ConvertX0()
        {
            return Input.Flows.Select(x => x.Measured).ToArray();
        }

        /// <summary>
        /// Конвертирование матрицы инцидентности / связей
        /// </summary>
        /// <returns></returns>
        private double[,] ConvertA()
        {
            var arr = new double[CountOfNodes, CountOfThreads];

            for (int i = 0; i < CountOfThreads; i++)
            {
                string sourceId = Input.Flows.ElementAt(i).SourceId;
                string destinationId = Input.Flows.ElementAt(i).DestinationId;

                for (int j = 0; j < CountOfNodes; j++)
                {
                    string vertexId = NodesList[j];
                    if (destinationId != null && destinationId.Equals(vertexId))
                    {
                        arr[j, i] = 1;
                    }
                    if (sourceId != null && sourceId.Equals(vertexId))
                    {
                        arr[j, i] = -1;
                    }
                }
            }
            return arr;
        }

        /// <summary>
        /// Конвертирование вектора измеряемости
        /// </summary>
        /// <returns></returns>
        private double[] ConvertI()
        {
            return Input.Flows.Select(x => x.IsMeasured == true ? 1.0 : 0.0).ToArray();//если x.IsMeasured == true, то 1.0, иначе 0.0
        }

        /// <summary>
        /// Конвертирование точностей
        /// </summary>
        /// <returns></returns>
        private double[] ConvertT()
        {
            return Input.Flows.Select(x => x.Tolerance).ToArray();
        }

        /// <summary>
        /// Конвертирование нижних ограничений
        /// </summary>
        /// <returns></returns>
        private double[] ConvertLower()
        {
            return Input.Flows.Select(x => x.LowerBound).ToArray();
        }
        
        /// <summary>
        /// Конвертирование верхних ограничений
        /// </summary>
        /// <returns></returns>
        private double[] ConvertUpper()
        {
            return Input.Flows.Select(x => x.UpperBound).ToArray();
        }

        /// <summary>
        /// Создание списка узлов
        /// </summary>
        private void CreateNodesList()
        {
            NodesList = new List<string>();

            for (int i = 0; i < Input.Flows.Count; i++)
            {
                string sourceId = Input.Flows.ElementAt(i).SourceId;
                string destinationId = Input.Flows.ElementAt(i).DestinationId;
                if (sourceId != null)
                {
                    bool isExisted = false;
                    foreach (var vertex in NodesList)
                    {
                        if (sourceId.Equals(vertex))
                        {
                            isExisted = true;
                            break;
                        }
                    }
                    if (!isExisted)
                    {
                        NodesList.Add(sourceId);
                    }
                }
                else if (destinationId != null)
                {
                    bool isExisted = false;
                    foreach (var vertex in NodesList)
                    {
                        if (destinationId.Equals(vertex))
                        {
                            isExisted = true;
                            break;
                        }
                    }
                    if (!isExisted)
                    {
                        NodesList.Add(destinationId);
                    }
                }
            }
        }
    }
}
