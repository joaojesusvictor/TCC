/** Sobre o CREATE PROCEDURE 'Cpa_ContasPagar_TRAN' ***/
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Cpa_ContasPagar_TRAN') and sysstat & 0xf = 4)
BEGIN
   PRINT 'Removida a última versão de dbo.Cpa_ContasPagar_TRAN'
   PRINT ' '
   DROP PROCEDURE dbo.Cpa_ContasPagar_TRAN
END
GO

CREATE PROCEDURE dbo.Cpa_ContasPagar_TRAN
	@Modo						integer					,
	@CodigoCpa					int				=	NULL,
	@CodigoFuncionario			int				=	NULL,
	@Valor						decimal(18,2)	=	NULL,
	@DataVencimento				datetime		=	NULL,
	@DataPagamento				datetime		=	NULL,
	@ServicoCobrado				varchar(200)	=	NULL,
	@ContaPaga					bit				=	NULL,
	@UsuarioTran				int				=	NULL
AS
 
/************************
 ** Nome:         Cpa_ContasPagar_TRAN 5
 ** Autor:        Joao
 ** Descrição:    Transaciona a Tabela de ContasPagar
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
 
 select * from ContasPagar
 
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
select @NomeTabela = 'Tabela de ContasPagar.'
    
IF @Modo = 1 --Inclusao
Begin
	insert ContasPagar ( CodigoFuncionario, Valor, DataVencimento, DataPagamento, ServicoCobrado, Paga, Ativo, DataInclusao, UsuarioIncluiu )
					Output inserted.CodigoCpa
	
	Values		(	@CodigoFuncionario, @Valor, @DataVencimento, @DataPagamento, @ServicoCobrado, @ContaPaga, 1, GETDATE(), @NomeUsuarioTRAN )
End

ELSE IF @Modo = 2 -- Alteracao
Begin
	Update	ContasPagar
	set		CodigoFuncionario = @CodigoFuncionario,
			Valor = @Valor,
			DataVencimento = @DataVencimento,
			DataPagamento = @DataPagamento,
			ServicoCobrado = @ServicoCobrado,
			Paga = @ContaPaga,
			DataUltimaAlteracao = GETDATE(),
			UsuarioUltimaAlteracao = @NomeUsuarioTRAN
	where	CodigoCpa = @CodigoCpa

	select @rowsaffected = @@rowcount, @errorreturned = @@error     
    IF @rowsaffected = 0 OR @errorreturned <> 0
    begin
       select 'Ocorreu uma falha na alteracao ' + @NomeTabela     
       return -100        
    end
End

ELSE IF @Modo = 3 -- Exclusao
Begin
	Update	ContasPagar 
	set		Ativo = 0,
			DataUltimaAlteracao = GETDATE(),
			UsuarioUltimaAlteracao = @NomeUsuarioTRAN
	where	CodigoCpa = @CodigoCpa

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
	From	ContasPagar
	Where	CodigoCpa = @CodigoCpa
	and		Ativo = 1
End

ELSE IF @Modo = 5 -- Consulta Varias Contas Pagas
Begin
	Select	CPA.CodigoCpa,
			CPA.Valor,
			CPA.DataVencimento,
			CPA.DataPagamento,
			CPA.ServicoCobrado,
			CPA.Paga,
			CPA.Ativo,
			CPA.DataInclusao,
			F.NomeFuncionario,
			F.Cpf,
			F.Telefone1
	From	ContasPagar CPA inner join
			Funcionario F on CPA.CodigoFuncionario = F.CodigoFuncionario
	Where	CPA.Ativo = 1
	and		CPA.Paga = 1
End

ELSE IF @Modo = 6 -- Consulta Varias Contas Nao Pagas
Begin
	Select	CPA.CodigoCpa,
			CPA.Valor,
			CPA.DataVencimento,
			CPA.DataPagamento,
			CPA.ServicoCobrado,
			CPA.Paga,
			CPA.Ativo,
			CPA.DataInclusao,
			F.NomeFuncionario,
			F.Cpf,
			F.Telefone1
	From	ContasPagar CPA inner join
			Funcionario F on CPA.CodigoFuncionario = F.CodigoFuncionario
	Where	CPA.Ativo = 1
	and		CPA.Paga = 0
End
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Cpa_ContasPagar_TRAN') and sysstat & 0xf = 4)
BEGIN
	PRINT 'Gerada a stored procedure dbo.Cpa_ContasPagar_TRAN'
END
GO