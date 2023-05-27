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

    public class ControlarCaixaController : BaseController
    {
        private ControlarCaixaRepository _controlarCaixaRepository;
        private readonly ISessao _sessao;
        private ClientesRepository _clientesRepository;

        public ControlarCaixaController(ControlarCaixaRepository controlarCaixaRepository, ISessao sessao, ClientesRepository clientesRepository)
        {
            _controlarCaixaRepository = controlarCaixaRepository;
            _sessao = sessao;
            _clientesRepository = clientesRepository;
        }

        #region Caixa

        public async Task<IActionResult> Index()
        {
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ControlarCaixaViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Entradas()
        {
            IEnumerable<ControlarCaixaRepository.Caixa> caixas = await _controlarCaixaRepository.GetCaixasEntradaAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ControlarCaixaViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario,
                TelaEntrada = true
            };

            foreach (var c in caixas)
            {
                viewModel.Caixas.Add(new CaixaViewModel()
                {
                    CodigoCaixa = c.CodigoCaixa,
                    Descricao = c.Descricao,
                    ValorTotal = c.ValorTotal,
                    ValorDesconto = c.ValorDesconto,
                    ValorEntrada = c.ValorEntrada,
                    DataMovimento = c.DataMovimento,
                    FormaMovimento = c.FormaMovimento,
                    NomeCliente = c.NomeCliente
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Saidas()
        {
            IEnumerable<ControlarCaixaRepository.Caixa> caixas = await _controlarCaixaRepository.GetCaixasSaidaAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ControlarCaixaViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario,
                TelaEntrada = false
            };

            foreach (var c in caixas)
            {
                viewModel.Caixas.Add(new CaixaViewModel()
                {
                    CodigoCaixa = c.CodigoCaixa,
                    Descricao = c.Descricao,
                    ValorTotal = c.ValorTotal,
                    ValorDesconto = c.ValorDesconto,
                    ValorSaida = c.ValorSaida,
                    DataMovimento = c.DataMovimento,
                    FormaMovimento = c.FormaMovimento,
                    NomeCliente = c.NomeCliente
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> New(bool entrada = false)
        {
            var comboClientes = await _clientesRepository.ComboClientesAsync(true);
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ControlarCaixaViewModel()
            {
                ClientesSelect = comboClientes,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario,
                TelaEntrada = entrada
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create(ControlarCaixaViewModel model)
        {
            string msgErro = Validar(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(New), new { entrada = model.TelaEntrada });
            }

            ControlarCaixaRepository.AbreFechaCaixa saldo = await _controlarCaixaRepository.GetSaldoAFCaixaAsync(model.DataMovimento);

            if (!model.TelaEntrada)
            {
                if (saldo.ValorSaldo < model.ValorSaida)
                {
                    MostraMsgErro("O Valor de Saída não pode ser maior do que o Saldo no Caixa!");

                    return RedirectToAction(nameof(Saidas));
                }
            }

            int gravado = await _controlarCaixaRepository.CreateAsync(model);

            if (gravado == 0)
            {
                MostraMsgErro("Não foi possível salvar, pois não houve o Fechamento de Caixa do Dia Anterior!");

                return RedirectToAction(nameof(New), new { entrada = model.TelaEntrada });
            }

            if (gravado == -1)
            {
                MostraMsgErro("Não foi possível salvar, pois já houve o Fechamento do Caixa para esta Data!");

                return RedirectToAction(nameof(New), new { entrada = model.TelaEntrada });
            }

            if (gravado == -2)
            {
                MostraMsgErro("Não foi possível salvar, pois não há Abertura do Caixa para esta Data!");

                return RedirectToAction(nameof(New), new { entrada = model.TelaEntrada });
            }

            if (model.TelaEntrada)
            {
                decimal? saldoFinal = saldo.ValorSaldo + model.ValorEntrada;

                await _controlarCaixaRepository.UpdateSaldoAFCaixaAsync(model.DataMovimento, saldoFinal, model.CodigoUsuario);
            }
            else
            {
                decimal? saldoFinal = saldo.ValorSaldo - model.ValorSaida;

                await _controlarCaixaRepository.UpdateSaldoAFCaixaAsync(model.DataMovimento, saldoFinal, model.CodigoUsuario);
            }

            MostraMsgSucesso("Controle de Caixa gravado com sucesso!");

            if (model.TelaEntrada)
                return RedirectToAction(nameof(Entradas));
            else
                return RedirectToAction(nameof(Saidas));
        }

        public async Task<IActionResult> Edit(int id, bool entrada = false)
        {
            var comboClientes = await _clientesRepository.ComboClientesAsync(true);
            ControlarCaixaRepository.Caixa caixa = await _controlarCaixaRepository.GetCaixaAsync(id);
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ControlarCaixaViewModel()
            {
                ClientesSelect = comboClientes,
                ModoEdit = true,
                CodigoCaixa = id,
                CodigoCliente = caixa.CodigoCliente,
                Descricao = caixa.Descricao,
                ValorTotal = caixa.ValorTotal,
                ValorDesconto = caixa.ValorDesconto,
                ValorEntrada = caixa.ValorEntrada,
                ValorSaida = caixa.ValorSaida,
                DataMovimento = caixa.DataMovimento,
                DataInclusao = caixa.DataInclusao,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario,
                TelaEntrada = entrada
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Update(ControlarCaixaViewModel model)
        {
            string msgErro = Validar(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(Edit), new { id = model.CodigoCaixa, entrada = model.TelaEntrada });
            }

            await _controlarCaixaRepository.UpdateAsync(model);

            MostraMsgSucesso("Controle de Caixa alterado com sucesso!");

            if (model.TelaEntrada)
                return RedirectToAction(nameof(Entradas));
            else
                return RedirectToAction(nameof(Saidas));
        }

        public string Validar(ControlarCaixaViewModel model)
        {
            string msg = "";

            if (string.IsNullOrEmpty(model.Descricao))
                msg = "A Descrição é necessária! ";

            if (model.ValorTotal == 0)
                msg += "O Preço deve ser maior do que Zero! ";

            if (model.TelaEntrada)
            {
                if (model.ValorEntrada == null || model.ValorEntrada == 0)
                    msg += "Valor de Entrada deve ser maior do que Zero!";
            }

            if (!model.TelaEntrada)
            {
                if (model.ValorSaida == null || model.ValorSaida == 0)
                    msg += "Valor de Saida deve ser maior do que Zero!";
            }

            return msg;
        }

        [HttpGet]
        public async Task<IActionResult> DeletePartial(int id, bool telaEntrada)
        {
            ControlarCaixaRepository.Caixa caixa = await _controlarCaixaRepository.GetCaixaAsync(id);

            var model = new DeletePartialViewModel
            {
                Id = id.ToString(),
                NomeEntidade = caixa.DataMovimento.ToString("dd-MM-yyyy"),
                Aux2 = telaEntrada.ToString(),
                DeleteUrl = Url.Action(nameof(Delete))
            };

            if (telaEntrada)
            {
                model.Mensagem1 = $"Deseja realmente excluir a Entrada \"{id}\" do Dia \"{caixa.DataMovimento.ToString("dd-MM-yyyy")}\"?";
            }
            else
            {
                model.Mensagem1 = $"Deseja realmente excluir a Saida \"{id}\" do Dia \"{caixa.DataMovimento.ToString("dd-MM-yyyy")}\"?";
            }

            return PartialView("_DeletePartial", model);
        }

        public async Task<IActionResult> Delete(DeletePartialViewModel model)
        {
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            int codigoUsuario = usuario.CodigoUsuario;

            await _controlarCaixaRepository.DeleteAsync(Convert.ToInt32(model.Id), codigoUsuario);

            if (model.Aux2.ToLower() == "true")
                MostraMsgSucesso($"A Entrada \"{model.Id}\" do Dia \"{model.NomeEntidade}\" foi excluída com sucesso!");
            else
                MostraMsgSucesso($"A Saida \"{model.Id}\" do Dia \"{model.NomeEntidade}\" foi excluída com sucesso!");

            if (model.Aux2.ToLower() == "true")
                return RedirectToAction(nameof(Entradas));
            else
                return RedirectToAction(nameof(Saidas));
        }

        #endregion

        #region AbreFechaCaixa

        public async Task<IActionResult> ControleCaixa()
        {
            IEnumerable<ControlarCaixaRepository.AbreFechaCaixa> caixas = await _controlarCaixaRepository.GetAFCaixasAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ControlarCaixaViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            foreach (var c in caixas)
            {
                viewModel.AFCaixas.Add(new AbreFechaCaixaViewModel()
                {
                    CodigoAFCaixa = c.CodigoAFCaixa,
                    DataCaixa = c.DataCaixa,
                    ValorAbertura = c.ValorAbertura,
                    ValorSaldo = c.ValorSaldo,
                    ValorFechamento = c.ValorFechamento
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> NewCc()
        {
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ControlarCaixaViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> CreateCc(ControlarCaixaViewModel model)
        {
            string msgErro = ValidarCc(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(NewCc));
            }

            int gravado = await _controlarCaixaRepository.CreateAFCaixaAsync(model);

            if (gravado == 0)
            {
                MostraMsgErro("Não foi possível salvar, pois não houve o Fechamento de Caixa do Dia Anterior!");

                return RedirectToAction(nameof(NewCc));
            }

            if (gravado == -1)
            {
                MostraMsgErro("Já existe uma Abertura de Caixa para esta data!");

                return RedirectToAction(nameof(NewCc));
            }

            MostraMsgSucesso("Controle de Caixa gravado com sucesso!");

            return RedirectToAction(nameof(ControleCaixa));
        }

        public async Task<IActionResult> EditCc(int id)
        {
            ControlarCaixaRepository.AbreFechaCaixa caixa = await _controlarCaixaRepository.GetAFCaixaAsync(id);
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ControlarCaixaViewModel()
            {
                ModoEdit = true,
                CodigoAFCaixa = id,
                DataCaixa = caixa.DataCaixa,
                ValorAbertura = caixa.ValorAbertura,
                ValorSaldo = caixa.ValorSaldo,
                ValorFechamento = caixa.ValorFechamento,
                DataInclusao = caixa.DataInclusao,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> UpdateCc(ControlarCaixaViewModel model)
        {
            string msgErro = ValidarCc(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(EditCc), new { id = model.CodigoAFCaixa });
            }

            int gravado = await _controlarCaixaRepository.UpdateAFCaixaAsync(model);

            if (gravado == 0)
            {
                MostraMsgErro("Já existe uma Abertura de Caixa para esta data!");

                return RedirectToAction(nameof(EditCc), new { id = model.CodigoAFCaixa });
            }

            MostraMsgSucesso("Controle de Caixa alterado com sucesso!");

            return RedirectToAction(nameof(ControleCaixa));
        }

        public string ValidarCc(ControlarCaixaViewModel model)
        {
            string msg = "";

            if (model.DataCaixa == null)
                msg = "A Data de Abertura do Caixa é necessária!";

            return msg;
        }

        [HttpGet]
        public async Task<IActionResult> DeletePartialCc(int id)
        {
            ControlarCaixaRepository.AbreFechaCaixa caixa = await _controlarCaixaRepository.GetAFCaixaAsync(id);

            var model = new DeletePartialViewModel
            {
                Id = id.ToString(),
                NomeEntidade = caixa.DataCaixa.ToString("dd-MM-yyyy"),
                Aux1 = caixa.DataCaixa.ToString("yyyy-MM-dd"),
                Mensagem1 = $"Deseja realmente excluir o Controle de Caixa \"{id}\" do Dia \"{caixa.DataCaixa.ToString("dd-MM-yyyy")}\"?",
                DeleteUrl = Url.Action(nameof(DeleteCc))
            };

            return PartialView("_DeletePartial", model);
        }

        public async Task<IActionResult> DeleteCc(DeletePartialViewModel model)
        {
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            int codigoUsuario = usuario.CodigoUsuario;

            var exluido = await _controlarCaixaRepository.DeleteAFCaixaAsync(Convert.ToInt32(model.Id), codigoUsuario, Convert.ToDateTime(model.Aux1));

            if (!exluido)
            {
                MostraMsgErro($"Não é possível excluir Controle de Caixa que tenha Lançamento de Entrada e/ou Saida!");

                return RedirectToAction(nameof(ControleCaixa));
            }

            MostraMsgSucesso($"O Controle de Caixa \"{model.Id}\" do Dia \"{model.NomeEntidade}\" foi excluído com sucesso!");

            return RedirectToAction(nameof(ControleCaixa));
        }

        #endregion

        #region Relatorio

        public async Task<IActionResult> Relatorio()
        {
            IEnumerable<ControlarCaixaRepository.Relatorio> caixas = await _controlarCaixaRepository.GetRelatorioCaixaAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ControlarCaixaViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            foreach (var c in caixas)
            {
                viewModel.RelatorioCaixas.Add(new RelatorioViewModel()
                {
                    DataCaixa = c.DataCaixa,
                    ValorAbertura = c.ValorAbertura,
                    TotalEntrada = c.TotalEntrada,
                    TotalSaida = c.TotalSaida,
                    ValorFechamento = c.ValorFechamento,
                    CorLinha = c.TotalSaida > c.TotalEntrada ? "Red" : ""
                });
            }

            return View(viewModel);
        }

        #endregion
    }
}