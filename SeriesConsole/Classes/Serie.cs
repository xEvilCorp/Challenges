namespace DIO.Series
{
    public class Serie : EntidadeBase
    {
        public Serie(int id, Genero genero, string titulo, string descricao, int ano)
        {
            Id = id;
            Genero = genero;
            Titulo = titulo;
            Descricao = descricao;
            Ano = ano;
            Excluido = false;
        }

        private Genero Genero {get; set;}
        private string Titulo {get; set;}
        private string Descricao {get; set;}
        private int Ano {get; set;}
        private bool Excluido {get; set;}

        public override string ToString() {
            string retorno = "";
            retorno += "Id: " + Id.ToString();
            retorno += " | Genero: " + Genero;
            retorno += " | Titulo: " + Titulo;
            retorno += " | Descricao: " + Descricao;
            retorno += " | Ano: " + Ano.ToString();
            return retorno;
        }

        public string GetTitulo() {
            return Titulo;
        }

        public int GetId() {
            return Id;
        }

        public void Excluir() {
            Excluido = true;
        }

        public bool IsAvailable(){
            return !Excluido;
        }
    }
}