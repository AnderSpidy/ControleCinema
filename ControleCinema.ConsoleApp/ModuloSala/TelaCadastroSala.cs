using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleCinema.ConsoleApp.Compartilhado;

namespace ControleCinema.ConsoleApp.ModuloSala
{
    public class TelaCadastroSala : TelaBase, ITelaCadastravel
    {
        private readonly IRepositorio<Sala> repositorioSala;
        private readonly Notificador notificador;
        public TelaCadastroSala(IRepositorio<Sala> repositorioSala, Notificador notificador) : base("Cadastro de Salas")
        {
            this.repositorioSala = repositorioSala;
            this.notificador = notificador;
        }

        public void Editar()
        {
            MostrarTitulo("Editando Sala de Filme");

            bool temRegistrosCadastrados = VisualizarRegistros("Pesquisando");

            if (temRegistrosCadastrados == false)
            {
                notificador.ApresentarMensagem("Nenhum gênero de filme cadastrado para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroGenero = ObterNumeroRegistro();

            Sala salaAtualizada = ObterSala();

            bool conseguiuEditar = repositorioSala.Editar(numeroGenero, salaAtualizada);

            if (!conseguiuEditar)
                notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Sala de Filme editado com sucesso!", TipoMensagem.Sucesso);

        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Sala");

            bool temSalasCadastradas = VisualizarRegistros("Pesquisando");

            if (temSalasCadastradas == false)
            {
                notificador.ApresentarMensagem("Nenhuma sala cadastrada para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroGenero = ObterNumeroRegistro();

            bool conseguiuExcluir = repositorioSala.Excluir(numeroGenero);

            if (!conseguiuExcluir)
                notificador.ApresentarMensagem("Não foi possível excluir.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Gênero de Filme excluído com sucesso1", TipoMensagem.Sucesso);
        }

        private int ObterNumeroRegistro()
        {
            int numeroRegistro;
            bool numeroRegistroEncontrado;

            do
            {
                Console.Write("Digite o ID da sala que deseja: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                numeroRegistroEncontrado = repositorioSala.ExisteRegistro(numeroRegistro);

                if (numeroRegistroEncontrado == false)
                    notificador.ApresentarMensagem("ID da Sala não foi encontrada, digite novamente", TipoMensagem.Atencao);

            } while (numeroRegistroEncontrado == false);

            return numeroRegistro;
        }

        public void Inserir()
        {
            MostrarTitulo("Cadastro de Nova Sala");

            Sala novaSala = ObterSala();

            repositorioSala.Inserir(novaSala);
            
            notificador.ApresentarMensagem("Gênero de Filme cadastrado com sucesso!", TipoMensagem.Sucesso);
        }
        private Sala ObterSala()
        {
            Console.Write("Digite a quantidade de poltrona na sala: ");
            int descricao = Convert.ToInt32(Console.ReadLine());

            return new Sala(descricao);
        }
        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Salas Cadastradas");

            List<Sala> salas = repositorioSala.SelecionarTodos();

            if (salas.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhuma Sala cadastrada", TipoMensagem.Atencao);
                return false;
            }

            foreach (Sala sala in salas)
                Console.WriteLine(sala.ToString());

            Console.ReadLine();

            return true;
        }
    }
}
