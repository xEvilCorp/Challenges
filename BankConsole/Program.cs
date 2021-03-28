using System;
using System.Collections.Generic;

namespace DIO.Bank
{
    class Program
    {

        static List<Conta> listContas = new List<Conta>();
        static void Main(string[] args)
        {
            string opcaoUsuario = ObterOpcaoUsuario();
            while(opcaoUsuario != "X") {
                switch(opcaoUsuario){
                    case "1":
                        ListarContas();
                        break;
                    case "2":
                        InserirConta();
                        break;
                    case "3":
                        Transferir();
                        break;
                    case "4":
                        Sacar();
                        break;
                    case "5":
                        Depositar();
                        break;
                    case "C":
                        Console.Clear();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                opcaoUsuario = ObterOpcaoUsuario();
            }

            Console.WriteLine("Fim.");
            Console.ReadLine();
        }

        private static void Transferir()
        {
            Console.WriteLine("Digite o numero da conta de origem:");
            int indiceContaOrigem = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Digite o numero da conta de destino:");
            int indiceContaDestino = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Digite o valor a ser transferido: ");
            double valorTransferencia = Convert.ToDouble(Console.ReadLine());        

            listContas[indiceContaOrigem].Transferir(valorTransferencia, listContas[indiceContaDestino]);
        }

        private static void Sacar()
        {
            Console.WriteLine("Digite o numero da conta:");
            int indiceConta = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Digite o valor a ser sacado: ");
            double valorSaque = Convert.ToDouble(Console.ReadLine());

            listContas[indiceConta].Sacar(valorSaque);
        }

        private static void Depositar()
        {
            Console.WriteLine("Digite o numero da conta:");
            int indiceConta = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Digite o valor a ser depositado: ");
            double valorDeposito = Convert.ToDouble(Console.ReadLine());

            listContas[indiceConta].Depositar(valorDeposito);
        }

        private static void ListarContas()
        {
            Console.WriteLine("Listar contas.");
            if(listContas.Count == 0) {
                Console.WriteLine("Nenhuma conta cadastrada.");
                return;
            }

            int count = 1;
            foreach(Conta conta in listContas)
            {
                Console.WriteLine(count.ToString() + " - " + conta);
                count++;
            }

        }

        private static void InserirConta()
        {
            Console.WriteLine("Inserir nova conta.");

            Console.WriteLine("Digite 1 para conta fisica, 2 para conta juridica:");
            TipoConta tipoConta = (TipoConta)Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Digite o nome do cliente:");
            string nome = Console.ReadLine();

            Console.WriteLine("Digite o saldo inicial:");
            double saldoInicial = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Digite o credito:");
            double credito = Convert.ToDouble(Console.ReadLine());


            Conta novaConta = new Conta(nome, tipoConta, saldoInicial, credito);
            listContas.Add(novaConta);
        }

        private static string ObterOpcaoUsuario() {
            Console.WriteLine();
            Console.WriteLine("DIO Bank");
            Console.WriteLine("Escolha uma opção:");

            Console.WriteLine("1 - Listar contas.");
            Console.WriteLine("2 - Inserir nova conta.");
            Console.WriteLine("3 - Transferir.");
            Console.WriteLine("4 - Sacar.");
            Console.WriteLine("5 - Depositar.");
            Console.WriteLine("C - Limpar tela.");
            Console.WriteLine("X - Sair.");
            Console.WriteLine("");

            string opcaoUsuario = Console.ReadLine().ToUpper();
            Console.WriteLine("");
            return opcaoUsuario;
        }

        
    }
}
