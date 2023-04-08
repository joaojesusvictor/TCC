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

    public class ProdutosController : BaseController
    {
        private ProdutosRepository _produtosRepository;
        private readonly ISessao _sessao;
        private FornecedoresRepository _fornecedoresRepository;

        public ProdutosController(ProdutosRepository produtosRepository, ISessao sessao, FornecedoresRepository fornecedoresRepository)
        {
            _produtosRepository = produtosRepository;
            _sessao = sessao;
            _fornecedoresRepository = fornecedoresRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ProdutosRepository.Produto> produtos = await _produtosRepository.GetProdutosAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ProdutosViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            foreach (var p in produtos)
            {
                viewModel.Produtos.Add(new ProdutoViewModel()
                {
                    CodigoProduto = p.CodigoProduto,
                    Descricao = p.Descricao,
                    Referencia = p.Referencia,
                    ValorUnitario = p.ValorUnitario,
                    Quantidade = p.Quantidade,
                    CorLinha = p.Quantidade <= 0 ? "red" : "",
                    Validade = p.Validade
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> New()
        {
            var comboFornecedores = await _fornecedoresRepository.ComboFornecedoresAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ProdutosViewModel()
            {
                FornecedoresSelect= comboFornecedores,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create(ProdutosViewModel model)
        {
            if (model.CodigoFornecedor == 0)
            {
                MostraMsgErro("Selecione o Fornecedor");

                return RedirectToAction(nameof(New));
            }

            int gravado = await _produtosRepository.CreateAsync(model);

            if (gravado == 0)
            {
                MostraMsgErro("Já existe um Produto com esta Referência.");

                return RedirectToAction(nameof(New));
            }

            MostraMsgSucesso("Produto incluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var comboFornecedores = await _fornecedoresRepository.ComboFornecedoresAsync();
            ProdutosRepository.Produto produto = await _produtosRepository.GetProdutoAsync(id);
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ProdutosViewModel()
            {
                FornecedoresSelect = comboFornecedores,
                ModoEdit = true,
                CodigoProduto = id,
                Referencia = produto.Referencia,
                Descricao = produto.Descricao,
                Localizacao = produto.Localizacao,
                Marca = produto.Marca,
                Categoria = produto.Categoria,
                ValorUnitario = produto.ValorUnitario,
                Quantidade = produto.Quantidade,
                Validade = produto.Validade,
                CodigoFornecedor = produto.CodigoFornecedor,
                DataInclusao = produto.DataInclusao,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Update(ProdutosViewModel model)
        {            
            if(model.CodigoFornecedor == 0)
            {
                MostraMsgErro("Selecione o Fornecedor");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoProduto });
            }

            int gravado = await _produtosRepository.UpdateAsync(model);

            if(gravado == 0)
            {
                MostraMsgErro("Já existe um Produto com esta Referência.");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoProduto });
            }

            MostraMsgSucesso("Produto alterado com sucesso!");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            int codigoUsuario = usuario.CodigoUsuario;

            bool deletado = await _produtosRepository.DeleteAsync(id, codigoUsuario);

            if (!deletado)
            {
                MostraMsgErro("Não foi possível excluír o Produto, pois há itens em estoque.");

                return RedirectToAction(nameof(Index));
            }

            MostraMsgSucesso("Produto excluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }        
    }
}