using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleCinema.ConsoleApp.Compartilhado;

namespace ControleCinema.ConsoleApp.ModuloSala
{
    public class Sala : EntidadeBase
    {
        public int Capacidade { get; set; }


        public Sala(int capacidade)
        {
            Capacidade = capacidade;
        }

        public override string ToString()
        {
            return "Id: " + id + Environment.NewLine +
                "Capacidade " + Capacidade + Environment.NewLine;
        }
    }
}
