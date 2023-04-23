using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Reflection;
using TechCompilerCo.Filters;
using TechCompilerCo.Helper;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Controllers
{
    [PaginaParaUsuarioLogado]

    public class GerarOsController : BaseController
    {
        private GerarOsRepository _gerarOsRepository;
        private readonly ISessao _sessao;
        private ClientesRepository _clientesRepository;
        private FuncionariosRepository _funcionariosRepository;

        public GerarOsController(GerarOsRepository gerarOsRepository, ISessao sessao, ClientesRepository clientesRepository, FuncionariosRepository funcionariosRepository)
        {
            _gerarOsRepository = gerarOsRepository;
            _sessao = sessao;
            _clientesRepository = clientesRepository;
            _funcionariosRepository = funcionariosRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<GerarOsRepository.OrdemServico> ordens = await _gerarOsRepository.GetOrdensServicosAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new GerarOsViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            foreach (var o in ordens)
            {
                viewModel.OrdensServicos.Add(new OrdemServicoViewModel()
                {
                    CodigoOs = o.CodigoOs,
                    CodigoCliente = o.CodigoCliente,
                    CodigoFuncionario = o.CodigoFuncionario,
                    DataInicio = o.DataInicio,
                    DataPrevisaoTermino = o.DataPrevisaoTermino,
                    DescricaoProblema = o.DescricaoProblema,
                    Valor = o.Valor,
                    StatusOs = o.StatusOs,
                    NomeCliente = o.NomeCliente,
                    Documento = o.Documento,
                    Telefone1 = o.Telefone1
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> New()
        {
            var comboClientes = await _clientesRepository.ComboClientesAsync();
            var comboFuncionarios = await _funcionariosRepository.ComboFuncionariosAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new GerarOsViewModel()
            {
                ClientesSelect = comboClientes,
                FuncionariosSelect = comboFuncionarios,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create(GerarOsViewModel model)
        {
            string msgErro = Validar(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(New));
            }

            await _gerarOsRepository.CreateAsync(model);

            MostraMsgSucesso("Ordem de Serviço gerada com sucesso!");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var comboClientes = await _clientesRepository.ComboClientesAsync();
            var comboFuncionarios = await _funcionariosRepository.ComboFuncionariosAsync();
            GerarOsRepository.OrdemServico ordem = await _gerarOsRepository.GetOrdemServicoAsync(id);
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new GerarOsViewModel()
            {
                ClientesSelect = comboClientes,
                FuncionariosSelect = comboFuncionarios,
                ModoEdit = true,
                CodigoOs = id,
                CodigoCliente = ordem.CodigoCliente,
                CodigoFuncionario = ordem.CodigoFuncionario,
                DataInicio = ordem.DataInicio,
                DataPrevisaoTermino = ordem.DataPrevisaoTermino,
                DescricaoProblema = ordem.DescricaoProblema,
                Valor = ordem.Valor,
                StatusOs = ordem.StatusOs,
                DataInclusao = ordem.DataInclusao,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Update(GerarOsViewModel model)
        {
            string msgErro = Validar(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(Edit), new { id = model.CodigoOs });
            }

            await _gerarOsRepository.UpdateAsync(model);

            MostraMsgSucesso("Ordem de Serviço alterada com sucesso!");

            return RedirectToAction(nameof(Index));
        }

        public string Validar(GerarOsViewModel model)
        {
            string msg = "";

            if (string.IsNullOrEmpty(model.DescricaoProblema))
                msg = "A Descrição do Problema é necessária! ";

            if (model.Valor == 0)
                msg += "O Preço deve ser maior do que Zero!";

            return msg;
        }

        [HttpGet]
        public async Task<IActionResult> DeletePartial(int id)
        {
            GerarOsRepository.OrdemServico ordem = await _gerarOsRepository.GetOrdemServicoAsync(id);

            var model = new DeletePartialViewModel
            {
                Id = id.ToString(),
                NomeEntidade = ordem.NomeCliente,
                Mensagem1 = $"Deseja realmente excluir a Ordem de Serviço \"{id}\" para o Cliente \"{ordem.NomeCliente}\"?",
                DeleteUrl = Url.Action(nameof(Delete))
            };

            return PartialView("_DeletePartial", model);
        }

        public async Task<IActionResult> Delete(DeletePartialViewModel model)
        {
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            int codigoUsuario = usuario.CodigoUsuario;

            await _gerarOsRepository.DeleteAsync(Convert.ToInt32(model.Id), codigoUsuario);

            MostraMsgSucesso($"A Ordem de Serviço \"{model.Id}\" para o Cliente \"{model.NomeEntidade}\" foi excluída com sucesso!");

            return RedirectToAction(nameof(Index));
        }
    }
}