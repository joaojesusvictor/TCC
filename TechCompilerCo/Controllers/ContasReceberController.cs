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

    public class ContasReceberController : BaseController
    {
        private ContasReceberRepository _contasReceberRepository;
        private readonly ISessao _sessao;
        private ClientesRepository _clientesRepository;

        public ContasReceberController(ContasReceberRepository contasReceberRepository, ISessao sessao, ClientesRepository clientesRepository)
        {
            _contasReceberRepository = contasReceberRepository;
            _sessao = sessao;
            _clientesRepository = clientesRepository;
        }

        public async Task<IActionResult> Index()
        {
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ContasReceberViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ContasRecebidas()
        {
            IEnumerable<ContasReceberRepository.ContaReceber> contas = await _contasReceberRepository.GetContasRecebidasAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ContasReceberViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario,
                ContasRecebidas = true
            };

            foreach (var c in contas)
            {
                viewModel.Contas.Add(new ContaReceberViewModel()
                {
                    CodigoCre = c.CodigoCre,
                    NumeroDocumento= c.NumeroDocumento,
                    DataVencimento = c.DataVencimento,
                    Valor = c.Valor,
                    DataPagamento = c.DataPagamento,
                    FormaPagamento = c.FormaPagamento,
                    ServicoCobrado = c.ServicoCobrado,
                    Recebida = c.Recebida,
                    Ativo = c.Ativo,
                    DataInclusao = c.DataInclusao,
                    NomeCliente = c.NomeCliente,
                    Documento = c.Documento,
                    Telefone1 = c.Telefone1
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> ContasNaoRecebidas()
        {
            IEnumerable<ContasReceberRepository.ContaReceber> contas = await _contasReceberRepository.GetContasNaoRecebidasAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ContasReceberViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario,
                ContasRecebidas = false
            };

            foreach (var c in contas)
            {
                viewModel.Contas.Add(new ContaReceberViewModel()
                {
                    CodigoCre = c.CodigoCre,
                    NumeroDocumento = c.NumeroDocumento,
                    DataVencimento = c.DataVencimento,
                    Valor = c.Valor,
                    DataPagamento = c.DataPagamento,
                    FormaPagamento = c.FormaPagamento,
                    ServicoCobrado = c.ServicoCobrado,
                    Recebida = c.Recebida,
                    Ativo = c.Ativo,
                    DataInclusao = c.DataInclusao,
                    NomeCliente = c.NomeCliente,
                    Documento = c.Documento,
                    Telefone1 = c.Telefone1
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> New(bool contasRecebidas)
        {
            var comboClientes = await _clientesRepository.ComboClientesAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ContasReceberViewModel()
            {
                ClientesSelect = comboClientes,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario,
                ContasRecebidas = contasRecebidas
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create(ContasReceberViewModel model)
        {
            string msgErro = Validar(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(New), new { contasRecebidas = model.ContasRecebidas });
            }

            int incluido = await _contasReceberRepository.CreateAsync(model);

            if (incluido <= 0)
            {
                MostraMsgErro("Já existe uma outra conta com este Número Documento!");

                return RedirectToAction(nameof(New), new { contasRecebidas = model.ContasRecebidas });
            }

            MostraMsgSucesso($"A Conta do Serviço \"{model.ServicoCobrado}\" foi incluída com sucesso!");

            if (model.ContasRecebidas)
                return RedirectToAction(nameof(ContasRecebidas));
            else
                return RedirectToAction(nameof(ContasNaoRecebidas));
        }

        public async Task<IActionResult> Edit(int id, bool contasRecebidas)
        {
            var comboClientes = await _clientesRepository.ComboClientesAsync();
            ContasReceberRepository.ContaReceber conta = await _contasReceberRepository.GetContaReceberAsync(id);
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ContasReceberViewModel()
            {
                ClientesSelect = comboClientes,
                ModoEdit = true,
                CodigoCre = id,
                NomeCliente = conta.NomeCliente,
                NumeroDocumento = conta.NumeroDocumento,
                DataVencimento = conta.DataVencimento,
                Valor = conta.Valor,
                DataPagamento = conta.DataPagamento,
                FormaPagamento = conta.FormaPagamento,
                ServicoCobrado = conta.ServicoCobrado,
                Recebida = conta.Recebida,
                Ativo = conta.Ativo,
                DataInclusao = conta.DataInclusao,
                ContasRecebidas = contasRecebidas,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Update(ContasReceberViewModel model)
        {
            string msgErro = Validar(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(Edit), new { id = model.CodigoCre, contasRecebidas = model.ContasRecebidas });
            }

            int alterado = await _contasReceberRepository.UpdateAsync(model);

            if(alterado <= 0)
            {
                MostraMsgErro("Já existe uma outra conta com este Número Documento!");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoCre, contasRecebidas = model.ContasRecebidas });
            }

            MostraMsgSucesso($"A Conta \"{model.CodigoCre}\" do Serviço \"{model.ServicoCobrado}\" foi alterada com sucesso!");

            if (model.ContasRecebidas)
                return RedirectToAction(nameof(ContasRecebidas));
            else
                return RedirectToAction(nameof(ContasNaoRecebidas));
        }

        public string Validar(ContasReceberViewModel model)
        {
            string msg = "";

            if (string.IsNullOrEmpty(model.NumeroDocumento))
                msg = "O Número Documento é necessário! ";

            if (string.IsNullOrEmpty(model.ServicoCobrado))
                msg += "A Descrição do Serviço é necessária!";

            return msg;
        }

        [HttpGet]
        public async Task<IActionResult> DeletePartial(int id, bool contasRecebidas)
        {
            ContasReceberRepository.ContaReceber conta = await _contasReceberRepository.GetContaReceberAsync(id);

            var model = new DeletePartialViewModel
            {
                Id = id.ToString(),
                NomeEntidade = conta.ServicoCobrado,
                Aux1 = contasRecebidas.ToString(),
                Mensagem1 = $"Deseja realmente excluir a Conta \"{conta.CodigoCre}\" do Serviço \"{conta.ServicoCobrado}\"?",
                DeleteUrl = Url.Action(nameof(Delete))
            };

            return PartialView("_DeletePartial", model);
        }

        public async Task<IActionResult> Delete(DeletePartialViewModel model)
        {
            if (model.Aux1.ToLower() == "true")
            {
                ContasReceberRepository.ContaReceber conta = await _contasReceberRepository.GetContaReceberAsync(Convert.ToInt32(model.Id));

                if (conta.DataPagamento != null)
                {
                    MostraMsgErro("Não é possível excluir uma Conta que Já Foi Recebida!");

                    return RedirectToAction(nameof(ContasRecebidas));
                }
            }

            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            int codigoUsuario = usuario.CodigoUsuario;

            await _contasReceberRepository.DeleteAsync(Convert.ToInt32(model.Id), codigoUsuario);

            MostraMsgSucesso($"A Conta \"{model.Id}\" do Serviço \"{model.NomeEntidade}\" foi excluída com sucesso!");

            if (model.Aux1.ToLower() == "true")
                return RedirectToAction(nameof(ContasRecebidas));
            else
                return RedirectToAction(nameof(ContasNaoRecebidas));
        }
    }
}