using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleCinema.ConsoleApp.Compartilhado;
using ControleCinema.ConsoleApp.ModuloGenero;

namespace ControleCinema.ConsoleApp.Modulo_Filme
{
    public class Filme : EntidadeBase
    {
        private readonly string titulo;
        private readonly int duracao;
        public Genero genero;

        public string Titulo { get => titulo; }

        public Filme(string titulo, int duracao, Genero generoSelecionado)
        {
            this.titulo = titulo;
            this.duracao = duracao;
            genero = generoSelecionado;
        }

        public override string ToString()
        {
            return "Id: " + id + Environment.NewLine +
                "Titulo: " + Titulo + Environment.NewLine +
                "Duração: " + duracao + " minutos" + Environment.NewLine +
                "Genero " + genero.Descricao + Environment.NewLine;
        }
    }
}
