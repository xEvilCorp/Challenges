using System;
using System.Linq;

namespace DIO.Series
{
    class Program
    {
        static SerieRepositorio Repositorio = new SerieRepositorio();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string opcaoUsuario = ObterOpcaoUsuario();
            while(opcaoUsuario != "X") {
                switch(opcaoUsuario) {
                    case "1":
                        ListarSeries();
                        break;
                    case "2":
                        InserirSerie();
                        break;
                    case "3":
                        AtualizarSerie();
                        break;
                    case "4":
                        ExcluirSerie();
                        break;
                    case "5":
                        VisualizarSerie();
                        break;
                    case "C":
                        Console.Clear();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                opcaoUsuario = ObterOpcaoUsuario();
            }
            
            Console.WriteLine("Fim");
        }

        private static void VisualizarSerie()
        {
            Console.WriteLine("Visualizar Serie");

            Console.WriteLine("Digite o ID da Serie:");
            int id = Convert.ToInt32(Console.ReadLine());     

            Serie serie = Repositorio.RetornaPorId(id);
            Console.WriteLine(serie);

        }

        private static void ExcluirSerie()
        {
            Console.WriteLine("Excluir Serie");

            Console.WriteLine("Digite o ID da Serie:");
            int id = Convert.ToInt32(Console.ReadLine());    

            Repositorio.Exclui(id);    
        }

        private static void AtualizarSerie()
        {
            Console.WriteLine("Atualizar Serie");

            Console.WriteLine("Digite o ID da Serie:");
            int id = Convert.ToInt32(Console.ReadLine());

            Serie serie = PedirInputDeDadosSerie();
            Repositorio.Atualiza(id, serie);
        }

        private static Serie PedirInputDeDadosSerie() {

            foreach(int i in Enum.GetValues(typeof(Genero))) {
                Console.WriteLine("{0} - {1}", i, Enum.GetName(typeof(Genero), i));
            }
            Console.WriteLine("Digite o genero conforme opções acima: ");
            int entradaGenero = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Digite o titulo da serie: ");
            string entradaTitulo = Console.ReadLine();

            Console.WriteLine("Digite o ano da serie: ");
            int entradaAno = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Digite a descrição da serie: ");
            string entradaDescricao = Console.ReadLine();

            Serie serie = new Serie(Repositorio.ProximoId(), (Genero)entradaGenero, entradaTitulo, entradaDescricao, entradaAno);

            return serie;
        }

        private static void InserirSerie()
        {
            Console.WriteLine("Inserir Serie");
            Serie serie = PedirInputDeDadosSerie();
            Repositorio.Insere(serie);
        }

        private static void ListarSeries()
        {
            Console.WriteLine("Listar Series");

            var lista = Repositorio.Lista().Where(x => x.IsAvailable()).ToList();

            if(lista.Count == 0) {
                Console.WriteLine("Nenhuma serie cadastrada");
                return;
            }
            foreach(Serie serie in lista) {
                Console.WriteLine("#ID {0}: {1}", serie.GetId(), serie.GetTitulo());
            }
        }

        private static string ObterOpcaoUsuario() {
            Console.WriteLine();
            Console.WriteLine("DIO Series");
            Console.WriteLine("Escolha uma opção:");

            Console.WriteLine("1 - Listar");
            Console.WriteLine("2 - Inserir");
            Console.WriteLine("3 - Atualizar");
            Console.WriteLine("4 - Excluir");
            Console.WriteLine("5 - Visualizar Serie");
            Console.WriteLine("C - Limpar tela");
            Console.WriteLine("X - Sair");
            Console.WriteLine();

            string opcaoUsuario = Console.ReadLine().ToUpper();
            Console.WriteLine();
            return opcaoUsuario;
        }
    }
}
