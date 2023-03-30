/** Sobre o CREATE PROCEDURE 'Prod_Produtos_TRAN' ***/
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Prod_Produtos_TRAN') and sysstat & 0xf = 4)
BEGIN
   PRINT 'Removida a última versão de dbo.Prod_Produtos_TRAN'
   PRINT ' '
   DROP PROCEDURE dbo.Prod_Produtos_TRAN
END
GO

CREATE PROCEDURE dbo.Prod_Produtos_TRAN
	@Modo						integer					,
	@CodigoProduto				int				=	NULL,
	@Descricao					varchar(150)	=	NULL,
	@Referencia					varchar(50)		=	NULL,
	@Localizacao				varchar(100)	=	NULL,
	@Marca						varchar(100)	=	NULL,
	@Categoria					varchar(100)	=	NULL,
	@ValorUnitario				decimal			=	NULL,
	@Quantidade					int				=	NULL,
	@CodigoFornecedor			int				=	NULL,
	@UsuarioTran				int				=	NULL
AS
 
/************************
 ** Nome:         Prod_Produtos_TRAN 5
 ** Autor:        Joao
 ** Descrição:    Transaciona a Tabela de Produtos
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
 
 select * from Produto
 
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
select @NomeTabela = 'Tabela de Produto.'
    
IF @Modo = 1 --Inclusao
begin
	if not exists(select * from Produto where LOWER(Referencia) = LOWER(@Referencia) and Ativo = 1)
		begin
			if exists(select * from Produto where LOWER(Referencia) = LOWER(@Referencia) and Ativo = 0)
				begin
					Update	Produto
					set		Descricao = @Descricao,
							Referencia = @Referencia,
							Localizacao = @Localizacao,
							Marca = @Marca,
							Categoria = @Categoria,
							ValorUnitario = @ValorUnitario,
							Quantidade = @Quantidade,
							Ativo = 1,
							DataUltimaAlteracao = GETDATE(),
							UsuarioUltimaAlteracao = @NomeUsuarioTRAN,
							CodigoFornecedor = @CodigoFornecedor
					where	LOWER(Referencia) = LOWER(@Referencia)

					select 1
				end
			else
				begin
					insert Produto (
									Descricao, Referencia, Localizacao, Marca, Categoria, ValorUnitario, Quantidade,
									Ativo, DataInclusao, UsuarioIncluiu, CodigoFornecedor )
									Output inserted.CodigoProduto
		
					Values		(	
									@Descricao, @Referencia, @Localizacao, @Marca, @Categoria, @ValorUnitario, @Quantidade,
									1, GETDATE(), @NomeUsuarioTRAN, @CodigoFornecedor )
				end
		end
	else
		begin
			select 0
		end	
end

else IF @Modo = 2 -- Alteracao
begin
	if not exists(select * from Produto where CodigoProduto <> @CodigoProduto and LOWER(Referencia) = LOWER(@Referencia) and Ativo = 1)
		begin
			Update	Produto
			set		Descricao = @Descricao,
					Referencia = @Referencia,
					Localizacao = @Localizacao,
					Marca = @Marca,
					Categoria = @Categoria,
					ValorUnitario = @ValorUnitario,
					Quantidade = @Quantidade,
					Ativo = 1,
					DataUltimaAlteracao = GETDATE(),
					UsuarioUltimaAlteracao = @NomeUsuarioTRAN,
					CodigoFornecedor = @CodigoFornecedor
			where	CodigoProduto = @CodigoProduto

			select 1
		end
	else
		begin
			select 0
		end

		select @rowsaffected = @@rowcount, @errorreturned = @@error     
        IF @rowsaffected = 0 OR @errorreturned <> 0
        begin
           select 'Ocorreu uma falha na alteracao ' + @NomeTabela     
           return -100        
        end
end

ELSE IF @Modo = 3 -- Exclusao
begin
		Update	Produto 
		set		Ativo = 0,
				DataUltimaAlteracao = GETDATE(),
				UsuarioUltimaAlteracao = @NomeUsuarioTRAN
		where	CodigoProduto = @CodigoProduto

	    select @errorreturned = @@error     
        IF @errorreturned <> 0
        begin
           select 'Ocorreu uma falha na delecao ' + @NomeTabela     
           return -100        
        end
End

ELSE IF @Modo = 4 -- Consulta Unico Produto
Begin
		Select	*
		From	Produto
		Where	CodigoProduto = @CodigoProduto
		and		Ativo = 1
End

ELSE IF @Modo = 5 -- Consulta Varios Produtos
Begin
		Select	*
		From	Produto
		Where	Ativo = 1
End
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Prod_Produtos_TRAN') and sysstat & 0xf = 4)
BEGIN
	PRINT 'Gerada a stored procedure dbo.Prod_Produtos_TRAN'
END
GO