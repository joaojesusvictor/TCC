/** Sobre o CREATE PROCEDURE 'Cre_ContasReceber_TRAN' ***/
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Cre_ContasReceber_TRAN') and sysstat & 0xf = 4)
BEGIN
   PRINT 'Removida a última versão de dbo.Cre_ContasReceber_TRAN'
   PRINT ' '
   DROP PROCEDURE dbo.Cre_ContasReceber_TRAN
END
GO

CREATE PROCEDURE dbo.Cre_ContasReceber_TRAN
	@Modo						integer					,
	@CodigoCre					int				=	NULL,
	@CodigoCliente				int				=	NULL,
	@NumeroDocumento			varchar(20)		=	NULL,
	@DataVencimento				datetime		=	NULL,
	@Valor						decimal(18,2)	=	NULL,
	@DataPagamento				datetime		=	NULL,
	@FormaPagamento				varchar(3)		=	NULL,
	@ServicoCobrado				varchar(200)	=	NULL,
	@ContaRecebida				bit				=	NULL,
	@UsuarioTran				int				=	NULL
AS
 
/************************
 ** Nome:         Cre_ContasReceber_TRAN 5
 ** Autor:        Joao
 ** Descrição:    Transaciona a Tabela de ContasReceber
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
 
 select * from ContasReceber
 
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
select @NomeTabela = 'Tabela de ContasReceber.'
    
IF @Modo = 1 --Inclusao
Begin
	If exists(select NumeroDocumento from ContasReceber where NumeroDocumento = @NumeroDocumento and Ativo = 1)
		Begin
			Select 0
		End
	Else
		Begin
			If exists(select NumeroDocumento from ContasReceber where NumeroDocumento = @NumeroDocumento and Ativo = 0)
				Begin
					Update	ContasReceber
					set		CodigoCliente = @CodigoCliente,
							DataVencimento = @DataVencimento,
							Valor = @Valor,
							DataPagamento = @DataPagamento,
							FormaPagamento = @FormaPagamento,
							ServicoCobrado = @ServicoCobrado,
							Recebida = @ContaRecebida,
							Ativo = 1,
							DataInclusao = GETDATE(),
							UsuarioIncluiu = @NomeUsuarioTRAN,
							DataUltimaAlteracao = null,
							UsuarioUltimaAlteracao = null
					where	NumeroDocumento = @NumeroDocumento

					Select 1
				End
			Else
				Begin
					Insert ContasReceber ( CodigoCliente, NumeroDocumento, DataVencimento, Valor, DataPagamento, FormaPagamento, ServicoCobrado, Recebida, Ativo, DataInclusao, UsuarioIncluiu )
									Output inserted.CodigoCre
					
					Values		(	@CodigoCliente, @NumeroDocumento, @DataVencimento, @Valor, @DataPagamento, @FormaPagamento, @ServicoCobrado, @ContaRecebida, 1, GETDATE(), @NomeUsuarioTRAN )
				End
		End
End

ELSE IF @Modo = 2 -- Alteracao
Begin
	If exists(select NumeroDocumento from ContasReceber where NumeroDocumento = @NumeroDocumento and CodigoCre <> @CodigoCre and Ativo = 1)
		Begin
			Select 0
		End
	Else
		Begin
			Update	ContasReceber
			set		CodigoCliente = @CodigoCliente,
					NumeroDocumento = @NumeroDocumento,
					DataVencimento = @DataVencimento,
					Valor = @Valor,
					DataPagamento = @DataPagamento,
					FormaPagamento = @FormaPagamento,
					ServicoCobrado = @ServicoCobrado,
					Recebida = @ContaRecebida,
					DataUltimaAlteracao = GETDATE(),
					UsuarioUltimaAlteracao = @NomeUsuarioTRAN
			where	CodigoCre = @CodigoCre

			Select 1
		End

	select @rowsaffected = @@rowcount, @errorreturned = @@error     
    IF @rowsaffected = 0 OR @errorreturned <> 0
    begin
       select 'Ocorreu uma falha na alteracao ' + @NomeTabela     
       return -100        
    end
End

ELSE IF @Modo = 3 -- Exclusao
Begin
	Update	ContasReceber 
	set		Ativo = 0,
			DataUltimaAlteracao = GETDATE(),
			UsuarioUltimaAlteracao = @NomeUsuarioTRAN
	where	CodigoCre = @CodigoCre

	select @errorreturned = @@error     
    IF @errorreturned <> 0
    begin
       select 'Ocorreu uma falha na delecao ' + @NomeTabela     
       return -100        
    end
End

ELSE IF @Modo = 4 -- Consulta Unica Conta
Begin
	Select	*
	From	ContasReceber
	Where	CodigoCre = @CodigoCre
	and		Ativo = 1
End

ELSE IF @Modo = 5 -- Consulta Varias Contas Recebidas
Begin
	Select	CRE.CodigoCre,
			CRE.NumeroDocumento,
			CRE.DataVencimento,
			CRE.Valor,
			CRE.DataPagamento,
			CRE.FormaPagamento,
			CRE.ServicoCobrado,
			CRE.Recebida,
			CRE.Ativo,
			CRE.DataInclusao,
			C.NomeCliente,
			C.Documento,
			C.Telefone1
	From	ContasReceber CRE inner join
			Cliente C on CRE.CodigoCliente = C.CodigoCliente
	Where	CRE.Ativo = 1
	and		CRE.Recebida = 1
End

ELSE IF @Modo = 6 -- Consulta Varias Contas Nao Recebidas
Begin
	Select	CRE.CodigoCre,
			CRE.NumeroDocumento,
			CRE.DataVencimento,
			CRE.Valor,
			CRE.DataPagamento,
			CRE.FormaPagamento,
			CRE.ServicoCobrado,
			CRE.Recebida,
			CRE.Ativo,
			CRE.DataInclusao,
			C.NomeCliente,
			C.Documento,
			C.Telefone1
	From	ContasReceber CRE inner join
			Cliente C on CRE.CodigoCliente = C.CodigoCliente
	Where	CRE.Ativo = 1
	and		CRE.Recebida = 0
End
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Cre_ContasReceber_TRAN') and sysstat & 0xf = 4)
BEGIN
	PRINT 'Gerada a stored procedure dbo.Cre_ContasReceber_TRAN'
END
GO