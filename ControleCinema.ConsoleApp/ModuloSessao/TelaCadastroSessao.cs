using ControleCinema.ConsoleApp.Compartilhado;
using ControleCinema.ConsoleApp.Modulo_Filme;
using ControleCinema.ConsoleApp.ModuloSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleCinema.ConsoleApp.ModuloSessao
{
    public class TelaCadastroSessao : TelaBase, ITelaCadastravel
    {
        private readonly IRepositorio<Sessao> repositorioSessao;
        private readonly Notificador notificador;

        private readonly TelaCadastroFilme telaCadastroFilme;
        private readonly IRepositorio<Filme> repositorioFilme;

        private readonly TelaCadastroSala telaCadastroSala;
        private readonly IRepositorio<Sala> repositorioSala;


        public TelaCadastroSessao(IRepositorio<Sessao> repositorioSessao, Notificador notificador,
            TelaCadastroFilme telaCadastroFilme, IRepositorio<Filme> repositorioFilme,
            TelaCadastroSala telaCadastroSala, IRepositorio<Sala> repositorioSala) : base("Cadastro de Sossões")
        {
            this.repositorioSessao = repositorioSessao;
            this.notificador = notificador;
            this.telaCadastroFilme = telaCadastroFilme;
            this.telaCadastroSala = telaCadastroSala;
            this.repositorioFilme = repositorioFilme;
            this.repositorioSala = repositorioSala;
        }
        public void Editar()
        {
            MostrarTitulo("Editando Sessão");

            bool temSessoesRegistradas = VisualizarRegistros("Pesquisando");

            if (temSessoesRegistradas == false)
            {
                notificador.ApresentarMensagem("Nenhuma sessão cadastrada para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroSessao = ObterNumeroRegistro();
            Sala salaAtualizada = ObterSala();
            Filme filmeAtualizado = ObterFilme();

            Sessao sessaoAtualizada = ObtemSessao(salaAtualizada, filmeAtualizado);

            bool conseguiuEditar = repositorioSessao.Editar(numeroSessao, sessaoAtualizada);

            if (!conseguiuEditar)
                notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Sessão editada com sucesso!", TipoMensagem.Sucesso);

        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Sessão");

            bool temSessaoCadastrada = VisualizarRegistros("Pesquisando");

            if (temSessaoCadastrada == false)
            {
                notificador.ApresentarMensagem("Nenhuma Sessão cadastrada para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroSessao = ObterNumeroRegistro();

            bool conseguiuExcluir = repositorioFilme.Excluir(numeroSessao);

            if (!conseguiuExcluir)
                notificador.ApresentarMensagem("Não foi possível excluir.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Sessão de Filme excluído com sucesso", TipoMensagem.Sucesso);
        }

        private int ObterNumeroRegistro()
        {
            int numeroRegistro;
            bool numeroRegistroEncontrado;

            do
            {
                Console.Write("Digite o ID da sessão que deseja selecionar: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                numeroRegistroEncontrado = repositorioSessao.ExisteRegistro(numeroRegistro);

                if (numeroRegistroEncontrado == false)
                    notificador.ApresentarMensagem("ID da Sessão não foi encontrada, digite novamente", TipoMensagem.Atencao);

            } while (numeroRegistroEncontrado == false);

            return numeroRegistro;
        }

        public void Inserir()
        {
            MostrarTitulo("Cadastro de Nova Sessão");
            //Sala
            Sala salaSelecionda = ObterSala();

            if (salaSelecionda == null)
            {
                notificador.ApresentarMensagem("Cadastre uma sala antes de cadastrar uma Sessão!", TipoMensagem.Atencao);
                return;
            }
            //Filme

            Filme filmeSelecionado = ObterFilme();

            if (filmeSelecionado == null)
            {
                notificador.ApresentarMensagem("Cadastre um filme antes de cadastrar uma Sessão!", TipoMensagem.Atencao);
                return;
            }

            Sessao novaSessao = ObtemSessao(salaSelecionda, filmeSelecionado);

            repositorioSessao.Inserir(novaSessao);

            notificador.ApresentarMensagem("Sessão de Filme cadastrado com sucesso!", TipoMensagem.Sucesso);

        }

        private Sessao ObtemSessao(Sala salaSelecionda, Filme filmeSelecionado)
        {
            Console.WriteLine("Digite o horario da Sessão:");
            DateTime horario = Convert.ToDateTime(Console.ReadLine());

            Sessao novaSessao = new Sessao(salaSelecionda, filmeSelecionado, horario, salaSelecionda.Capacidade);
            return novaSessao;
        }

        private Sala ObterSala()
        {
            bool existeSalaDisponivel = telaCadastroSala.VisualizarRegistros("");

            if (!existeSalaDisponivel)
            {
                notificador.ApresentarMensagem("Não há salas disponiveis para cadastrar Sessões!!!", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o número da sala que irá inserir: ");
            int numeroSalaSelecionado = Convert.ToInt32(Console.ReadLine());
            
            Console.WriteLine();

            Sala SalaSelecionada = repositorioSala.SelecionarRegistro(x => x.id == numeroSalaSelecionado);

            return SalaSelecionada;
        }

        private Filme ObterFilme()
        {
            bool temFilmesDisponiveis = telaCadastroFilme.VisualizarRegistros("Pesquisando");

            if (!temFilmesDisponiveis)
            {
                notificador.ApresentarMensagem("Não hánenhum filme disponivel para cadastrar uma sessão!!!", TipoMensagem.Atencao);
                return null;
            }
            Console.WriteLine("Digite o numero do Filme que ira ser selecionado:");
            int numeroFilmeSessao = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            Filme filmeSelecionado = repositorioFilme.SelecionarRegistro(x => x.id == numeroFilmeSessao);
            return filmeSelecionado;
        }
        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Sessao");

            List<Sessao> sessoes = repositorioSessao.SelecionarTodos();

            if (sessoes.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhum filme cadastrado", TipoMensagem.Atencao);
                return false;
            }

            foreach (Sessao sessao in sessoes)
                Console.WriteLine(sessao.ToString());

            Console.ReadLine();

            return true;
        }
    }
}
