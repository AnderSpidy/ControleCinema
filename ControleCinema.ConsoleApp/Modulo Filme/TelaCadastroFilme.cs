using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleCinema.ConsoleApp.Compartilhado;
using ControleCinema.ConsoleApp.ModuloGenero;

namespace ControleCinema.ConsoleApp.Modulo_Filme
{
    public class TelaCadastroFilme : TelaBase, ITelaCadastravel
    {
        private readonly IRepositorio<Filme> repositorioFilme;
        private readonly Notificador notificador;

        private readonly TelaCadastroGenero telaCadastroGenero;
        private readonly IRepositorio<Genero> repositorioGenero;

        public TelaCadastroFilme(IRepositorio<Filme> repositorioFilme, Notificador notificador, TelaCadastroGenero telaCadastroGenero, IRepositorio<Genero> repositorioGenero) : base("Cadastro de Filmes")
        {
            this.repositorioFilme = repositorioFilme;
            this.notificador = notificador;
            this.telaCadastroGenero = telaCadastroGenero;
            this.repositorioGenero = repositorioGenero;
        }

        public void Editar()
        {
            MostrarTitulo("Editando Filme");

            bool temRegistrosCadastrados = VisualizarRegistros("Pesquisando");

            if (temRegistrosCadastrados == false)
            {
                notificador.ApresentarMensagem("Nenhum filme cadastrado para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroGenero = ObterNumeroRegistro();
            Genero generoSelecionado = ObtemGenero();
            Filme filmeAtualizado = ObterFilme(generoSelecionado);

            bool conseguiuEditar = repositorioFilme.Editar(numeroGenero, filmeAtualizado);

            if (!conseguiuEditar)
                notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Filme editado com sucesso!", TipoMensagem.Sucesso);

        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Filme");

            bool temSalasCadastradas = VisualizarRegistros("Pesquisando");

            if (temSalasCadastradas == false)
            {
                notificador.ApresentarMensagem("Nenhum filme cadastrada para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroGenero = ObterNumeroRegistro();

            bool conseguiuExcluir = repositorioFilme.Excluir(numeroGenero);

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
                Console.Write("Digite o ID do filme que deseja: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                numeroRegistroEncontrado = repositorioFilme.ExisteRegistro(numeroRegistro);

                if (numeroRegistroEncontrado == false)
                    notificador.ApresentarMensagem("ID da Sala não foi encontrada, digite novamente", TipoMensagem.Atencao);

            } while (numeroRegistroEncontrado == false);

            return numeroRegistro;
        }

        public void Inserir()
        {
            MostrarTitulo("Cadastro de Nova Sala");

            Genero generoSelecionado = ObtemGenero();
            if (generoSelecionado == null )
            {
                notificador.ApresentarMensagem("Cadastre um genero antes de cadastrar um filme!", TipoMensagem.Atencao);
                return;
            }
            Filme novoFilme = ObterFilme(generoSelecionado);
            repositorioFilme.Inserir(novoFilme);

            notificador.ApresentarMensagem("Gênero de Filme cadastrado com sucesso!", TipoMensagem.Sucesso);

        }

        private Genero ObtemGenero()
        {
            bool existeGeneros = telaCadastroGenero.VisualizarRegistros("");

            if (!existeGeneros)
            {
                notificador.ApresentarMensagem("Não há nenhum genero disponivel para cadastrar Filmes!!!", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o número do genero que irá inserir: ");
            int numGeneroSelecionado = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            Genero generoSelecionado = repositorioGenero.SelecionarRegistro(x => x.id == numGeneroSelecionado);

            return generoSelecionado;
        }

        private Filme ObterFilme(Genero generoSelecionado)
        {
            Console.Write("Digite o titulo do filme: ");
            string titulo = Console.ReadLine();

            Console.Write("Digite a duração do filme em minutos: ");
            int duracao = Convert.ToInt32(Console.ReadLine());

            return new Filme(titulo, duracao, generoSelecionado);
        }
        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Filmes Cadastradas");

            List<Filme> filmes = repositorioFilme.SelecionarTodos();

            if (filmes.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhum filme cadastrado", TipoMensagem.Atencao);
                return false;
            }

            foreach (Filme sala in filmes)
                Console.WriteLine(sala.ToString());

            Console.ReadLine();

            return true;
        }
    }
}
