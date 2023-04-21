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

    public class VendasController : BaseController
    {
        private VendasRepository _vendasRepository;
        private readonly ISessao _sessao;
        private ProdutosRepository _produtosRepository;
        private ClientesRepository _clientesRepository;

        public VendasController(VendasRepository vendasRepository, ISessao sessao, ProdutosRepository produtosRepository, ClientesRepository clientesRepository)
        {
            _vendasRepository = vendasRepository;
            _sessao = sessao;
            _produtosRepository = produtosRepository;
            _clientesRepository = clientesRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<VendasRepository.Venda> vendas = await _vendasRepository.GetVendasAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new VendasViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            foreach (var v in vendas)
            {
                viewModel.Vendas.Add(new VendaViewModel()
                {
                    CodigoVenda = v.CodigoVenda,
                    CodigoProduto = v.CodigoProduto,
                    CodigoCliente = v.CodigoCliente,
                    Quantidade = v.Quantidade,
                    Valor = v.Valor,
                    DataVenda = v.DataVenda,
                    NomeCliente = v.NomeCliente,
                    DescricaoProduto = v.DescricaoProduto
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> New()
        {
            var comboProdutos = await _produtosRepository.ComboProdutosAsync();
            var comboClientes = await _clientesRepository.ComboClientesAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new VendasViewModel()
            {
                ProdutosSelect= comboProdutos,
                ClientesSelect= comboClientes,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create(VendasViewModel model)
        {
            if (model.CodigoProduto == 0)
            {
                MostraMsgErro("Selecione o Produto");

                return RedirectToAction(nameof(New));
            }

            if (model.CodigoCliente == 0)
            {
                MostraMsgErro("Selecione o Cliente");

                return RedirectToAction(nameof(New));
            }

            if (model.Quantidade == 0)
            {
                MostraMsgErro("A Quantidade da Venda deve ser maior do que Zero!");

                return RedirectToAction(nameof(New));
            }

            int result = await _vendasRepository.CreateAsync(model);

            if (result <= 0)
            {
                MostraMsgErro("A Quantidade da Venda está maior do que a Quantidade do Produto em Estoque!");

                return RedirectToAction(nameof(New));
            }

            MostraMsgSucesso("Venda incluída com sucesso!");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var comboProdutos = await _produtosRepository.ComboProdutosAsync();
            var comboClientes = await _clientesRepository.ComboClientesAsync();
            VendasRepository.Venda venda = await _vendasRepository.GetVendaAsync(id);
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new VendasViewModel()
            {
                ProdutosSelect = comboProdutos,
                ClientesSelect = comboClientes,
                ModoEdit = true,
                CodigoVenda = id,
                CodigoProduto = venda.CodigoProduto,
                CodigoCliente = venda.CodigoCliente,
                Quantidade = venda.Quantidade,
                Valor = venda.Valor,
                DataVenda = venda.DataVenda,
                DataInclusao = venda.DataInclusao,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Update(VendasViewModel model)
        {
            if(model.CodigoProduto == 0)
            {
                MostraMsgErro("Selecione o Produto");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoVenda });
            }
            
            if(model.CodigoCliente == 0)
            {
                MostraMsgErro("Selecione o Cliente");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoVenda });
            }
            
            if(model.Quantidade == 0)
            {
                MostraMsgErro("A Quantidade da Venda deve ser maior do que Zero!");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoVenda });
            }

            int result = await _vendasRepository.UpdateAsync(model);

            if (result <= 0)
            {
                MostraMsgErro("A Quantidade da Venda está maior do que a Quantidade do Produto em Estoque!");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoVenda });
            }

            MostraMsgSucesso("Venda alterada com sucesso!");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeletePartial(int id)
        {
            VendasRepository.Venda venda = await _vendasRepository.GetVendaAsync(id);

            var model = new DeletePartialViewModel
            {
                Id = id.ToString(),
                Mensagem1 = $"Deseja realmente excluir a Venda do Código de Venda \"{id}\"?",
                DeleteUrl = Url.Action(nameof(Delete))
            };

            return PartialView("_DeletePartial", model);
        }

        public async Task<IActionResult> Delete(DeletePartialViewModel model)
        {
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            int codigoUsuario = usuario.CodigoUsuario;

            await _vendasRepository.DeleteAsync(Convert.ToInt32(model.Id), codigoUsuario);

            MostraMsgSucesso($"A Venda do Código de Venda \"{model.Id}\" foi excluída com sucesso!");

            return RedirectToAction(nameof(Index));
        }
    }
}