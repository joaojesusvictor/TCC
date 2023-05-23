/** Sobre o CREATE PROCEDURE 'CAX_ControlarCaixa_TRAN' ***/
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.CAX_ControlarCaixa_TRAN') and sysstat & 0xf = 4)
BEGIN
   PRINT 'Removida a última versão de dbo.CAX_ControlarCaixa_TRAN'
   PRINT ' '
   DROP PROCEDURE dbo.CAX_ControlarCaixa_TRAN
END
GO

CREATE PROCEDURE dbo.CAX_ControlarCaixa_TRAN
	@Modo						integer					,
	@CodigoCaixa				int				=	NULL,
	@CodigoCliente				int				=	NULL,
	@Descricao					varchar(200)	=	NULL,
	@ValorTotal					decimal(18,2)	=	NULL,
	@ValorDesconto				decimal(18,2)	=	NULL,
	@ValorEntrada				decimal(18,2)	=	NULL,
	@ValorSaida					decimal(18,2)	=	NULL,
	@DataMovimento				date			=	NULL,
	@FormaMovimento				varchar(3)		=	NULL,
	@UsuarioTran				int				=	NULL
AS
 
/************************
 ** Nome:         CAX_ControlarCaixa_TRAN 5
 ** Autor:        Joao
 ** Descrição:    Transaciona a Tabela de Caixa
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
 
 select * from Caixa
 
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
select @NomeTabela = 'Tabela de Caixa.'
    
IF @Modo = 1 --Inclusao
begin	
	declare @ValorFecha decimal(18,2) = null;

	Select @ValorFecha = (select ValorFechamento from AbreFechaCaixa where DataCaixa = @DataMovimento);

	If @ValorFecha is not null
		Begin
			Select -1
		End
	Else
		Begin
			Select @ValorFecha = (select ValorFechamento from AbreFechaCaixa where DataCaixa = DATEADD(DAY, -1, @DataMovimento));

			If @ValorFecha is null
				Begin
					select 0
				End
			Else
				Begin
					Insert Caixa	(	CodigoCliente, Descricao, ValorTotal, ValorDesconto, ValorEntrada, ValorSaida,
										DataMovimento, FormaMovimento, Ativo, DataInclusao, UsuarioIncluiu )
										Output inserted.CodigoCaixa
					
						Values		(	@CodigoCliente, @Descricao, @ValorTotal, @ValorDesconto, @ValorEntrada, @ValorSaida,
										@DataMovimento, @FormaMovimento, 1, GETDATE(), @NomeUsuarioTRAN )
				End
		End
end

else IF @Modo = 2 -- Alteracao
begin
	declare @EntradaAtual decimal(18,2) = null,
			@SaidaAtual decimal(18,2) = null,
			@Diferenca decimal(18,2) = 0;
	
	select @EntradaAtual = (select ValorEntrada from Caixa where CodigoCaixa = @CodigoCaixa);
	select @SaidaAtual = (select ValorSaida from Caixa where CodigoCaixa = @CodigoCaixa);

	If(@EntradaAtual <> @ValorEntrada)
		Begin
			If(@EntradaAtual > @ValorEntrada)
				Begin
					select @Diferenca = @EntradaAtual - @ValorEntrada;

					update	AbreFechaCaixa
					set		ValorSaldo = ValorSaldo - @Diferenca
					where	DataCaixa = @DataMovimento
				End
			else
				Begin
					select @Diferenca = @ValorEntrada - @EntradaAtual;

					update	AbreFechaCaixa
					set		ValorSaldo = ValorSaldo + @Diferenca
					where	DataCaixa = @DataMovimento
				End
		End

	If(@SaidaAtual <> @ValorSaida)
		Begin
			If(@SaidaAtual > @ValorSaida)
				Begin
					select @Diferenca = @SaidaAtual - @ValorSaida;

					update	AbreFechaCaixa
					set		ValorSaldo = ValorSaldo + @Diferenca
					where	DataCaixa = @DataMovimento
				End
			else
				Begin
					select @Diferenca = @ValorSaida - @SaidaAtual;

					update	AbreFechaCaixa
					set		ValorSaldo = ValorSaldo - @Diferenca
					where	DataCaixa = @DataMovimento
				End
		End

	Update	Caixa
	Set		CodigoCliente = @CodigoCliente,
			Descricao = @Descricao,
			ValorTotal = @ValorTotal,
			ValorDesconto = @ValorDesconto,
			ValorEntrada = @ValorEntrada,
			ValorSaida = @ValorSaida,
			DataMovimento = @DataMovimento,
			FormaMovimento = @FormaMovimento,
			DataUltimaAlteracao = GETDATE(),
			UsuarioUltimaAlteracao = @NomeUsuarioTRAN
	Where	CodigoCaixa = @CodigoCaixa

	select @rowsaffected = @@rowcount, @errorreturned = @@error     
    IF @rowsaffected = 0 OR @errorreturned <> 0
    begin
       select 'Ocorreu uma falha na alteracao ' + @NomeTabela     
       return -100        
    end
end

ELSE IF @Modo = 3 -- Exclusao
begin
	Update	Caixa 
	set		Ativo = 0,
			DataUltimaAlteracao = GETDATE(),
			UsuarioUltimaAlteracao = @NomeUsuarioTRAN
	where	CodigoCaixa = @CodigoCaixa

	select @errorreturned = @@error     
    IF @errorreturned <> 0
    begin
       select 'Ocorreu uma falha na delecao ' + @NomeTabela     
       return -100        
    end
End

ELSE IF @Modo = 4 -- Consulta Unico Caixa Entrada
Begin
	Select	Ca.CodigoCaixa,
			Ca.CodigoCliente,
			Ca.Descricao,
			Ca.ValorTotal,
			Ca.ValorDesconto,
			Ca.ValorEntrada,
			Ca.ValorSaida,
			Ca.DataMovimento,
			Ca.FormaMovimento,
			Ca.DataInclusao
	From	Caixa Ca
	Where	Ca.CodigoCaixa = @CodigoCaixa
	and		Ca.Ativo = 1
End

ELSE IF @Modo = 5 -- Consulta Varios Caixas Entrada
Begin
	Select	Ca.CodigoCaixa,
			Ca.Descricao,
			Ca.ValorTotal,
			Ca.ValorDesconto,
			Ca.ValorEntrada,
			Ca.DataMovimento,
			Ca.FormaMovimento,
			NomeCliente = isnull((Select NomeCliente From Cliente where CodigoCliente = isnull(Ca.CodigoCliente, 0)), '')
	From	Caixa Ca
	Where	Ca.Ativo = 1
	--and		Ca.DataMovimento = @DataMovimento
	and		Ca.ValorSaida is null
End

ELSE IF @Modo = 6 -- Consulta Varios Caixas Saida
Begin
	Select	Ca.CodigoCaixa,
			Ca.Descricao,
			Ca.ValorTotal,
			Ca.ValorDesconto,
			Ca.ValorSaida,
			Ca.DataMovimento,
			Ca.FormaMovimento,
			NomeCliente = isnull((Select NomeCliente From Cliente where CodigoCliente = isnull(Ca.CodigoCliente, 0)), '')
	From	Caixa Ca
	Where	Ca.Ativo = 1
	--and		Ca.DataMovimento = @DataMovimento
	and		Ca.ValorEntrada is null
End
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.CAX_ControlarCaixa_TRAN') and sysstat & 0xf = 4)
BEGIN
	PRINT 'Gerada a stored procedure dbo.CAX_ControlarCaixa_TRAN'
END
GO