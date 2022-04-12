using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleCinema.ConsoleApp.ModuloSala;
using ControleCinema.ConsoleApp.Modulo_Filme;
using ControleCinema.ConsoleApp.Compartilhado;

namespace ControleCinema.ConsoleApp.ModuloSessao
{
    public class Sessao : EntidadeBase
    {
        private Sala sala;
        private Filme filme;
        private DateTime horario;
        private int numeroIngressoDisponiveis;

        public Sessao(Sala sala, Filme filme, DateTime horario, int numeroIngressoDisponiveis)
        {
            this.sala = sala;
            this.filme = filme;
            this.horario = horario;
            this.numeroIngressoDisponiveis = numeroIngressoDisponiveis;
        }

        public override string ToString()
        {
            return "Sala: " + sala.ToString() + Environment.NewLine +
                "Filme: " + filme.Titulo + Environment.NewLine +
                "Horario: " + horario.ToString() + Environment.NewLine +
                "Numero de Ingressos Disponíveis: " + numeroIngressoDisponiveis + Environment.NewLine;
        }

    }
}
