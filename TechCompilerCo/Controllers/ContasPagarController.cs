using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Drawing;
using System.Reflection;
using TechCompilerCo.Filters;
using TechCompilerCo.Helper;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Controllers
{
    [SomenteAdm]

    public class ContasPagarController : BaseController
    {
        private ContasPagarRepository _contasPagarRepository;
        private readonly ISessao _sessao;
        private FuncionariosRepository _funcionariosRepository;

        public ContasPagarController(ContasPagarRepository contasPagarRepository, ISessao sessao, FuncionariosRepository funcionariosRepository)
        {
            _contasPagarRepository = contasPagarRepository;
            _sessao = sessao;
            _funcionariosRepository = funcionariosRepository;
        }

        public async Task<IActionResult> Index()
        {
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ContasPagarViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ContasPagas()
        {
            IEnumerable<ContasPagarRepository.ContaPagar> contas = await _contasPagarRepository.GetContasPagasAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ContasPagarViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario,
                ContasPagas = true
            };

            foreach (var c in contas)
            {
                viewModel.Contas.Add(new ContaPagarViewModel()
                {
                    CodigoCpa = c.CodigoCpa,
                    Valor = c.Valor,
                    DataVencimento = c.DataVencimento,
                    DataPagamento = c.DataPagamento,
                    ServicoCobrado = c.ServicoCobrado,
                    Paga = c.Paga,
                    Ativo = c.Ativo,
                    DataInclusao = c.DataInclusao,
                    NomeFuncionario = c.NomeFuncionario,
                    Cpf = c.Cpf,
                    Telefone1 = c.Telefone1
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> ContasNaoPagas()
        {
            IEnumerable<ContasPagarRepository.ContaPagar> contas = await _contasPagarRepository.GetContasNaoPagasAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ContasPagarViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario,
                ContasPagas = false
            };

            foreach (var c in contas)
            {
                viewModel.Contas.Add(new ContaPagarViewModel()
                {
                    CodigoCpa = c.CodigoCpa,
                    Valor = c.Valor,
                    DataVencimento = c.DataVencimento,
                    DataPagamento = c.DataPagamento,
                    ServicoCobrado = c.ServicoCobrado,
                    Paga = c.Paga,
                    Ativo = c.Ativo,
                    DataInclusao = c.DataInclusao,
                    NomeFuncionario = c.NomeFuncionario,
                    Cpf = c.Cpf,
                    Telefone1 = c.Telefone1
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> New(bool contasPagas)
        {
            var comboFuncionarios = await _funcionariosRepository.ComboFuncionariosAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ContasPagarViewModel()
            {
                FuncionariosSelect = comboFuncionarios,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario,
                ContasPagas = contasPagas
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create(ContasPagarViewModel model)
        {
            await _contasPagarRepository.CreateAsync(model);

            MostraMsgSucesso("Conta incluída com sucesso!");

            if (model.ContasPagas)
                return RedirectToAction(nameof(ContasPagas));
            else
                return RedirectToAction(nameof(ContasNaoPagas));
        }

        public async Task<IActionResult> Edit(int id, bool contasPagas)
        {
            var comboFuncionarios = await _funcionariosRepository.ComboFuncionariosAsync();
            ContasPagarRepository.ContaPagar conta = await _contasPagarRepository.GetContaPagarAsync(id);
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ContasPagarViewModel()
            {
                FuncionariosSelect = comboFuncionarios,
                ModoEdit = true,
                CodigoCpa = id,
                CodigoFuncionario = conta.CodigoFuncionario,
                Valor = conta.Valor,
                DataVencimento = conta.DataVencimento,
                DataPagamento = conta.DataPagamento,
                ServicoCobrado = conta.ServicoCobrado,
                Paga = conta.Paga,
                Ativo = conta.Ativo,
                DataInclusao = conta.DataInclusao,
                ContasPagas = contasPagas,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Update(ContasPagarViewModel model)
        {
            await _contasPagarRepository.UpdateAsync(model);

            MostraMsgSucesso("Conta alterada com sucesso!");

            if (model.ContasPagas)
                return RedirectToAction(nameof(ContasPagas));
            else
                return RedirectToAction(nameof(ContasNaoPagas));
        }

        [HttpGet]
        public async Task<IActionResult> DeletePartial(int id, bool contasPagas)
        {
            ContasPagarRepository.ContaPagar conta = await _contasPagarRepository.GetContaPagarAsync(id);

            var model = new DeletePartialViewModel
            {
                Id = id.ToString(),
                NomeEntidade = conta.ServicoCobrado,
                Aux1 = contasPagas.ToString(),
                Mensagem1 = $"Deseja realmente excluir a Conta \"{conta.CodigoCpa}\" do Serviço \"{conta.ServicoCobrado}\"?",
                DeleteUrl = Url.Action(nameof(Delete))
            };

            return PartialView("_DeletePartial", model);
        }

        public async Task<IActionResult> Delete(DeletePartialViewModel model)
        {
            if (model.Aux1.ToLower() == "true")
            {
                ContasPagarRepository.ContaPagar conta = await _contasPagarRepository.GetContaPagarAsync(Convert.ToInt32(model.Id));

                if (conta.DataPagamento != null)
                {
                    MostraMsgErro("Não é possível excluir uma Conta que Já Foi Paga!");

                    return RedirectToAction(nameof(ContasPagas));
                }
            }

            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            int codigoUsuario = usuario.CodigoUsuario;

            await _contasPagarRepository.DeleteAsync(Convert.ToInt32(model.Id), codigoUsuario);

            MostraMsgSucesso($"A Conta \"{model.Id}\" do Serviço \"{model.NomeEntidade}\" foi excluída com sucesso!");

            if (model.Aux1.ToLower() == "true")
                return RedirectToAction(nameof(ContasPagas));
            else
                return RedirectToAction(nameof(ContasNaoPagas));
        }
    }
}