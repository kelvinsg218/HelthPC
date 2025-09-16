using System;
using System.Diagnostics;
using System.IO;
using NickStrupat;
namespace HealthCheckPC
{
    enum Status
    {
        OK,
        ATENCAO,
        CRITICO
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Verificação de saúde do PC ===\n");
            AnaliseFinal();
        }

        static Status VerificarMemoria()
        {
            var pcInfo = new ComputerInfo();
            ulong totalmemo = pcInfo.TotalPhysicalMemory;
            ulong avaiblememo = pcInfo.AvailablePhysicalMemory;

            double usado = 100 - (avaiblememo * 100.0 / totalmemo);
            Console.WriteLine($"Uso de Memória: {usado:F2}%");

            if (usado > 80)
            {
                Console.WriteLine("Esse computador precisa de upgrade na memória");
                return Status.CRITICO;
            }
            else
            {
                Console.WriteLine("Memória em bom estado");
                return Status.OK;
            }
        }

        static Status VerificarCpu()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            System.Threading.Thread.Sleep(100); 
            float cpuUso = cpuCounter.NextValue();

            Console.WriteLine($"Uso de CPU: {cpuUso:F2}%");

            if (cpuUso >= 85)
            {
                Console.WriteLine("CPU sobrecarregada, precisa de otimização");
                return Status.CRITICO;
            }
            else if (cpuUso >= 70)
            {
                Console.WriteLine("CPU exigida, requer atenção");
                return Status.ATENCAO;
            }
            else
            {
                Console.WriteLine(" CPU em bom estado");
                return Status.OK;
            }
        }

        static Status VerificarDisco()
        {
            DriveInfo driveC = new DriveInfo("C:\\");
            double livre = (driveC.AvailableFreeSpace / (double)driveC.TotalSize) * 100;
            Console.WriteLine($"Espaço livre no Disco C: {livre:F2}%");

            if (livre < 20)
            {
                Console.WriteLine("Pouco espaço no disco, recomenda limpeza");
                return Status.CRITICO;
            }
            else if (livre >= 20 && livre <= 30)
            {
                Console.WriteLine(" Espaço em atenção, pode precisar de manutenção em breve");
                return Status.ATENCAO;
            }
            else
            {
                Console.WriteLine("Disco com bom armazenamento");
                return Status.OK;
            }
        }

        static void AnaliseFinal()
        {
            Console.WriteLine("\n=== Diagnóstico Final ===");

            Status memoria = VerificarMemoria();
            Status cpu = VerificarCpu();
            Status disco = VerificarDisco();

            if (memoria == Status.CRITICO || cpu == Status.CRITICO || disco == Status.CRITICO)
            {
                Console.WriteLine("\n Diagnóstico:  computador precisa de manutenção URGENTE!");
            }
            else if (memoria == Status.ATENCAO || cpu == Status.ATENCAO || disco == Status.ATENCAO)
            {
                Console.WriteLine("\n Diagnóstico:  computador apresenta sinais que exigem ATENÇÃO.");
            }
            else
            {
                Console.WriteLine("\n Diagnóstico: computador está SAUDÁVEL.");
            }
        }
    }
}


