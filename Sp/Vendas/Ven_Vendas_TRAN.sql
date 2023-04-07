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
		@NomeTabela					VarChar(80)
		
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
begin
	Insert Venda ( CodigoProduto, CodigoCliente, Quantidade, Valor, DataVenda, Ativo, DataInclusao, UsuarioIncluiu )
					Output inserted.CodigoVenda
	Values		( @CodigoProduto, @CodigoCliente, @Quantidade, @Valor, @DataVenda, 1, GETDATE(), @NomeUsuarioTRAN )
end

else IF @Modo = 2 -- Alteracao
begin
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
end

ELSE IF @Modo = 3 -- Exclusao
begin
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
		Select	*, NomeCliente = (Select NomeCliente From Cliente where CodigoCliente = V.CodigoCliente),
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