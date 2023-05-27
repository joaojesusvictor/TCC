/** Sobre o CREATE PROCEDURE 'Ven_Vendas_TRAN' ***/
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Ven_Vendas_TRAN') and sysstat & 0xf = 4)
BEGIN
   PRINT 'Removida a última versão de dbo.Ven_Vendas_TRAN'
   PRINT ' '
   DROP PROCEDURE dbo.Ven_Vendas_TRAN
END
GO

CREATE PROCEDURE dbo.Ven_Vendas_TRAN
	@Modo						integer					,
	@CodigoVenda				int				=	NULL,
	@CodigoProduto				int				=	NULL,
	@CodigoCliente				int				=	NULL,
	@Quantidade					int				=	NULL,	
	@Valor						decimal(18,2)	=	NULL,
	@DataVenda					datetime		=	NULL,
	@UsuarioTran				int				=	NULL
AS
 
/************************
 ** Nome:         Ven_Vendas_TRAN 5
 ** Autor:        Joao
 ** Descrição:    Transaciona a Tabela de Vendas
 **
 ** Valor Retorno: Código     Condição
 **                ------     ---------
 **
 * **************
 **           Histórico Manutenção
 * **************
 **
 ** ---------- --------   	-----------------
 ** 
 
 select * from Venda
 
 ** 
 ***********************/
set nocount on

declare @rowsaffected				integer,
		@errorreturned				integer,
		@NomeTabela					VarChar(80),
		@Qtd						int,
		@QtdAtualVenda				int,
		@QtdAtualProduto			int,
		@QtdLimiteProduto			int,
		@NovaQtdProduto				int
		
-- Variável para obter o nome do usuário que vai transacionar a tabela		
Declare	@NomeUsuarioTRAN			Varchar(30)	
		
if	ISNULL(@UsuarioTran,'') <> '' 
begin
	select	@NomeUsuarioTRAN	=	NomeUsuario 
	from	Usuario	
	where	CodigoUsuario		=	@UsuarioTran
end

-- Constante para exibição de mensagens
select @NomeTabela = 'Tabela de Venda.'

IF @Modo = 1 --Inclusao
Begin
	select @QtdAtualProduto = (select Quantidade from Produto where CodigoProduto = @CodigoProduto)

	IF @QtdAtualProduto >= @Quantidade
		Begin
			select @Qtd = (select @QtdAtualProduto - @Quantidade)

			update	Produto
			set		Quantidade = @Qtd
			where	CodigoProduto = @CodigoProduto

			Insert Venda ( CodigoProduto, CodigoCliente, Quantidade, Valor, DataVenda, Ativo, DataInclusao, UsuarioIncluiu )
							Output inserted.CodigoVenda
			Values		( @CodigoProduto, @CodigoCliente, @Quantidade, @Valor, @DataVenda, 1, GETDATE(), @NomeUsuarioTRAN )
		End
	Else
		Begin
			select 0
		End
End

ELSE IF @Modo = 2 -- Alteracao
Begin
	select @QtdAtualVenda = (select Quantidade from Venda where CodigoVenda = @CodigoVenda)

	If @QtdAtualVenda <> @Quantidade
	Begin
		IF @QtdAtualVenda > @Quantidade
		Begin
			select @Qtd = (select @QtdAtualVenda - @Quantidade)

			select @NovaQtdProduto = (select Quantidade + @Qtd from Produto where CodigoProduto = @CodigoProduto)

			select @QtdLimiteProduto = (select QtdLimite from Produto where CodigoProduto = @CodigoProduto)

			IF @NovaQtdProduto > @QtdLimiteProduto
			Begin
				select @NovaQtdProduto = @QtdLimiteProduto
			End
		End

		If @QtdAtualVenda < @Quantidade
		Begin
			select @Qtd = (select @Quantidade - @QtdAtualVenda)

			select @QtdAtualProduto = (select Quantidade from Produto where CodigoProduto = @CodigoProduto)

			IF @QtdAtualProduto < @Qtd
			Begin
				return 0
			End

			select @NovaQtdProduto = (select @QtdAtualProduto - @Qtd)
		End

		update	Produto
		set		Quantidade = @NovaQtdProduto
		where	CodigoProduto = @CodigoProduto
	End

	Update	Venda
	set		CodigoProduto = @CodigoProduto,
			CodigoCliente = @CodigoCliente,
			Quantidade = @Quantidade,
			Valor = @Valor,
			DataVenda = @DataVenda,
			Ativo = 1,
			DataUltimaAlteracao = GETDATE(),
			UsuarioUltimaAlteracao = @NomeUsuarioTRAN
	where	CodigoVenda = @CodigoVenda

	select @rowsaffected = @@rowcount, @errorreturned = @@error     
	IF @rowsaffected = 0 OR @errorreturned <> 0
	begin
	   select 'Ocorreu uma falha na alteracao ' + @NomeTabela     
	   return -100        
	end

	select 1
End

ELSE IF @Modo = 3 -- Exclusao
Begin
	Update	Venda
	set		Ativo = 0,
			DataUltimaAlteracao = GETDATE(),
			UsuarioUltimaAlteracao = @NomeUsuarioTRAN
	where	CodigoVenda = @CodigoVenda

	select @errorreturned = @@error     
    IF @errorreturned <> 0
    begin
       select 'Ocorreu uma falha na delecao ' + @NomeTabela     
       return -100        
    end
End

ELSE IF @Modo = 4 -- Consulta Unica Venda
Begin
		Select	*
		From	Venda
		Where	CodigoVenda = @CodigoVenda
		and		Ativo = 1
End

ELSE IF @Modo = 5 -- Consulta Varias Vendas
Begin
		Select	*, NomeCliente = isnull((Select NomeCliente From Cliente where CodigoCliente = isnull(V.CodigoCliente, 0)), ''),
		DescricaoProduto = (Select Descricao From Produto where CodigoProduto = V.CodigoProduto)
		From	Venda V
		Where	Ativo = 1
End
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Ven_Vendas_TRAN') and sysstat & 0xf = 4)
BEGIN
	PRINT 'Gerada a stored procedure dbo.Ven_Vendas_TRAN'
END
GO