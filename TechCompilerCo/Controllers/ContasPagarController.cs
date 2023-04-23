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
        private FornecedoresRepository _fornecedoresRepository;

        public ContasPagarController(ContasPagarRepository contasPagarRepository, ISessao sessao, FornecedoresRepository fornecedoresRepository)
        {
            _contasPagarRepository = contasPagarRepository;
            _sessao = sessao;
            _fornecedoresRepository = fornecedoresRepository;
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
                    NumeroDocumento= c.NumeroDocumento,
                    Valor = c.Valor,
                    DataVencimento = c.DataVencimento,
                    DataPagamento = c.DataPagamento,
                    ServicoCobrado = c.ServicoCobrado,
                    Paga = c.Paga,
                    Ativo = c.Ativo,
                    DataInclusao = c.DataInclusao,
                    NomeFantasia = c.NomeFantasia,
                    Documento = c.Documento,
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
                    NumeroDocumento = c.NumeroDocumento,
                    Valor = c.Valor,
                    DataVencimento = c.DataVencimento,
                    DataPagamento = c.DataPagamento,
                    ServicoCobrado = c.ServicoCobrado,
                    Paga = c.Paga,
                    Ativo = c.Ativo,
                    DataInclusao = c.DataInclusao,
                    NomeFantasia = c.NomeFantasia,
                    Documento = c.Documento,
                    Telefone1 = c.Telefone1
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> New(bool contasPagas)
        {
            var comboFornecedores = await _fornecedoresRepository.ComboFornecedoresAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ContasPagarViewModel()
            {
                FornecedoresSelect = comboFornecedores,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario,
                ContasPagas = contasPagas
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create(ContasPagarViewModel model)
        {
            string msgErro = Validar(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(New), new { contasPagas = model.ContasPagas });
            }

            int incluido = await _contasPagarRepository.CreateAsync(model);

            if (incluido <= 0)
            {
                MostraMsgErro("Já existe uma outra conta com este Número Documento!");

                return RedirectToAction(nameof(New), new { contasPagas = model.ContasPagas });
            }

            MostraMsgSucesso($"A Conta do Serviço \"{model.ServicoCobrado}\" foi incluída com sucesso!");

            if (model.ContasPagas)
                return RedirectToAction(nameof(ContasPagas));
            else
                return RedirectToAction(nameof(ContasNaoPagas));
        }

        public async Task<IActionResult> Edit(int id, bool contasPagas)
        {
            var comboFornecedores = await _fornecedoresRepository.ComboFornecedoresAsync();
            ContasPagarRepository.ContaPagar conta = await _contasPagarRepository.GetContaPagarAsync(id);
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ContasPagarViewModel()
            {
                FornecedoresSelect = comboFornecedores,
                ModoEdit = true,
                CodigoCpa = id,
                CodigoFornecedor = conta.CodigoFornecedor,
                NumeroDocumento = conta.NumeroDocumento,
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
            string msgErro = Validar(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(Edit), new { id = model.CodigoCpa, contasPagas = model.ContasPagas });
            }

            int alterado = await _contasPagarRepository.UpdateAsync(model);

            if(alterado <= 0)
            {
                MostraMsgErro("Já existe uma outra conta com este Número Documento!");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoCpa, contasPagas = model.ContasPagas });
            }

            MostraMsgSucesso($"A Conta \"{model.CodigoCpa}\" do Serviço \"{model.ServicoCobrado}\" foi alterada com sucesso!");

            if (model.ContasPagas)
                return RedirectToAction(nameof(ContasPagas));
            else
                return RedirectToAction(nameof(ContasNaoPagas));
        }

        public string Validar(ContasPagarViewModel model)
        {
            string msg = "";

            if (string.IsNullOrEmpty(model.NumeroDocumento))
                msg = "O Número Documento é necessário! ";

            if (string.IsNullOrEmpty(model.ServicoCobrado))
                msg += "A Descrição do Serviço é necessária!";

            return msg;
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