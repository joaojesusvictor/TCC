/** Sobre o CREATE PROCEDURE 'OS_OrdemServico_TRAN' ***/
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.OS_OrdemServico_TRAN') and sysstat & 0xf = 4)
BEGIN
   PRINT 'Removida a última versão de dbo.OS_OrdemServico_TRAN'
   PRINT ' '
   DROP PROCEDURE dbo.OS_OrdemServico_TRAN
END
GO

CREATE PROCEDURE dbo.OS_OrdemServico_TRAN
	@Modo						integer					,
	@CodigoOs					int				=	NULL,
	@CodigoCliente				int				=	NULL,
	@CodigoFuncionario			int				=	NULL,
	@DataInicio					datetime		=	NULL,
	@DataPrevisaoTermino		datetime		=	NULL,
	@DescricaoProblema			varchar(200)	=	NULL,
	@Valor						decimal(18,2)	=	NULL,
	@StatusOs					varchar(3)		=	NULL,
	@UsuarioTran				int				=	NULL
AS
 
/************************
 ** Nome:         OS_OrdemServico_TRAN 5
 ** Autor:        Joao
 ** Descrição:    Transaciona a Tabela de OrdemServico
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
 
 select * from OrdemServico
 
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
select @NomeTabela = 'Tabela de OrdemServico.'
    
IF @Modo = 1 --Inclusao
begin
	Insert OrdemServico (	CodigoCliente, CodigoFuncionario, DataInicio, DataPrevisaoTermino, DescricaoProblema,
							Valor, StatusOs, Ativo, DataInclusao, UsuarioIncluiu )
							Output inserted.CodigoOs
	
			Values		(	@CodigoCliente, @CodigoFuncionario, @DataInicio, @DataPrevisaoTermino, @DescricaoProblema,
							@Valor, @StatusOs, 1, GETDATE(), @NomeUsuarioTRAN )
end

else IF @Modo = 2 -- Alteracao
begin
	Update	OrdemServico
	set		CodigoCliente = @CodigoCliente,
			CodigoFuncionario = @CodigoFuncionario,
			DataInicio = @DataInicio,
			DataPrevisaoTermino = @DataPrevisaoTermino,
			DescricaoProblema = @DescricaoProblema,
			Valor = @Valor,
			StatusOs = @StatusOs,
			DataUltimaAlteracao = GETDATE(),
			UsuarioUltimaAlteracao = @NomeUsuarioTRAN
	where	CodigoOs = @CodigoOs

	select @rowsaffected = @@rowcount, @errorreturned = @@error     
    IF @rowsaffected = 0 OR @errorreturned <> 0
    begin
       select 'Ocorreu uma falha na alteracao ' + @NomeTabela     
       return -100        
    end
end

ELSE IF @Modo = 3 -- Exclusao
begin
	Update	OrdemServico 
	set		Ativo = 0,
			DataUltimaAlteracao = GETDATE(),
			UsuarioUltimaAlteracao = @NomeUsuarioTRAN
	where	CodigoOs = @CodigoOs

	select @errorreturned = @@error     
    IF @errorreturned <> 0
    begin
       select 'Ocorreu uma falha na delecao ' + @NomeTabela     
       return -100        
    end
End

ELSE IF @Modo = 4 -- Consulta Unica Ordem
Begin
	Select	OS.CodigoOs,
			OS.CodigoCliente,
			OS.CodigoFuncionario,
			OS.DataInicio,
			OS.DataPrevisaoTermino,
			OS.DescricaoProblema,
			OS.Valor,
			OS.StatusOs,
			OS.DataInclusao,
			C.NomeCliente
	From	OrdemServico OS inner join
			Cliente C on OS.CodigoCliente = C.CodigoCliente
	Where	OS.CodigoOs = @CodigoOs
	and		OS.Ativo = 1
End

ELSE IF @Modo = 5 -- Consulta Varias Ordens
Begin
	Select	OS.CodigoOs,
			OS.CodigoCliente,
			OS.CodigoFuncionario,
			OS.DataInicio,
			OS.DataPrevisaoTermino,
			OS.DescricaoProblema,
			OS.Valor,
			OS.StatusOs,
			C.NomeCliente,
			C.Documento,
			C.Telefone1
	From	OrdemServico OS inner join
			Cliente C on OS.CodigoCliente = C.CodigoCliente
	Where	OS.Ativo = 1
End
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.OS_OrdemServico_TRAN') and sysstat & 0xf = 4)
BEGIN
	PRINT 'Gerada a stored procedure dbo.OS_OrdemServico_TRAN'
END
GO