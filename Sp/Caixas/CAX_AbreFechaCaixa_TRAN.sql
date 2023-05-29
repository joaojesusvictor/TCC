/** Sobre o CREATE PROCEDURE 'CAX_AbreFechaCaixa_TRAN' ***/
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.CAX_AbreFechaCaixa_TRAN') and sysstat & 0xf = 4)
BEGIN
   PRINT 'Removida a última versão de dbo.CAX_AbreFechaCaixa_TRAN'
   PRINT ' '
   DROP PROCEDURE dbo.CAX_AbreFechaCaixa_TRAN
END
GO

CREATE PROCEDURE dbo.CAX_AbreFechaCaixa_TRAN
	@Modo						integer					,
	@CodigoAFCaixa				int				=	NULL,
	@DataCaixa					date			=	NULL,
	@ValorAbertura				decimal(18,2)	=	NULL,
	@ValorSaldo					decimal(18,2)	=	NULL,
	@ValorFechamento			decimal(18,2)	=	NULL,
	@UsuarioTran				int				=	NULL
AS
 
/************************
 ** Nome:         CAX_AbreFechaCaixa_TRAN 5
 ** Autor:        Joao
 ** Descrição:    Transaciona a Tabela de AbreFechaCaixa
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
 
 select * from AbreFechaCaixa
 
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
select @NomeTabela = 'Tabela de AbreFechaCaixa.'
    
IF @Modo = 1 --Inclusao
begin
	If exists(select CodigoAFCaixa from AbreFechaCaixa where DataCaixa = DATEADD(DAY, -1, @DataCaixa))
		Begin
			declare @ValorFecha decimal(18,2) = null;

			Select @ValorFecha = (select ValorFechamento from AbreFechaCaixa where DataCaixa = DATEADD(DAY, -1, @DataCaixa));

			If @ValorFecha is null
				Begin
					select 0
				End
			Else
				Begin
					If exists(Select CodigoAFCaixa From	AbreFechaCaixa Where DataCaixa = @DataCaixa and Ativo = 1)
						Begin
							select -1
						End
					Else
						Begin
							If exists(Select CodigoAFCaixa From	AbreFechaCaixa Where DataCaixa = @DataCaixa and Ativo = 0)
								Begin
									Update	AbreFechaCaixa
									Set		ValorAbertura = @ValorAbertura,
											ValorSaldo = @ValorAbertura,
											ValorFechamento = @ValorFechamento,
											Ativo = 1,
											DataUltimaAlteracao = GETDATE(),
											UsuarioUltimaAlteracao = @NomeUsuarioTRAN
									Where	DataCaixa = @DataCaixa
								End
							Else
								Begin
									Insert AbreFechaCaixa	(	DataCaixa, ValorAbertura, ValorSaldo, ValorFechamento, Ativo, DataInclusao, UsuarioIncluiu )
																Output inserted.CodigoAFCaixa
									
												Values		(	@DataCaixa, @ValorAbertura, @ValorAbertura, @ValorFechamento, 1, GETDATE(), @NomeUsuarioTRAN )
								End
						End
				End				
		End
	Else
		Begin
			Insert AbreFechaCaixa	(	DataCaixa, ValorAbertura, ValorSaldo, ValorFechamento, Ativo, DataInclusao, UsuarioIncluiu )
										Output inserted.CodigoAFCaixa
			
						Values		(	@DataCaixa, @ValorAbertura, @ValorAbertura, @ValorFechamento, 1, GETDATE(), @NomeUsuarioTRAN )
		End
end

else IF @Modo = 2 -- Alteracao
begin
	If exists(Select CodigoAFCaixa From	AbreFechaCaixa Where DataCaixa = @DataCaixa and CodigoAFCaixa <> @CodigoAFCaixa and Ativo = 1)
		Begin
			select 0
		End
	Else
		Begin
			declare @ValorAbAntigo decimal(18,2),
					@TotalEntrada decimal(18,2),
					@TotalSaida decimal(18,2);

			Select @ValorAbAntigo = (select ValorAbertura from AbreFechaCaixa where CodigoAFCaixa = @CodigoAFCaixa);

			If (@ValorAbAntigo <> @ValorAbertura)
				Begin
					Select @TotalEntrada = isnull((select SUM(ValorEntrada) from Caixa where DataMovimento = @DataCaixa), 0);
					Select @TotalSaida = isnull((select SUM(ValorSaida) from Caixa where DataMovimento = @DataCaixa), 0);

					select @ValorSaldo = (@ValorAbertura + @TotalEntrada) - @TotalSaida;

					Update	AbreFechaCaixa
					Set		ValorSaldo = @ValorSaldo
					where CodigoAFCaixa = @CodigoAFCaixa
				End

			Update	AbreFechaCaixa
			Set		ValorAbertura = @ValorAbertura,
					ValorFechamento = @ValorFechamento,
					DataUltimaAlteracao = GETDATE(),
					UsuarioUltimaAlteracao = @NomeUsuarioTRAN
			Where	CodigoAFCaixa = @CodigoAFCaixa

			select 1
		End

	select @rowsaffected = @@rowcount, @errorreturned = @@error     
    IF @rowsaffected = 0 OR @errorreturned <> 0
    begin
       select 'Ocorreu uma falha na alteracao ' + @NomeTabela     
       return -100        
    end
end

ELSE IF @Modo = 3 -- Exclusao
begin
	If exists(select CodigoCaixa from Caixa where DataMovimento = @DataCaixa)
		Begin
			select 0
		End
	Else
		Begin
			Update	AbreFechaCaixa 
			set		Ativo = 0,
					DataUltimaAlteracao = GETDATE(),
					UsuarioUltimaAlteracao = @NomeUsuarioTRAN
			where	CodigoAFCaixa = @CodigoAFCaixa

			select 1
		End
	
	select @errorreturned = @@error     
    IF @errorreturned <> 0
    begin
       select 'Ocorreu uma falha na delecao ' + @NomeTabela     
       return -100        
    end
End

ELSE IF @Modo = 4 -- Consulta Unica Abertura
Begin
	Select	CodigoAFCaixa,
			DataCaixa,
			ValorAbertura,
			ValorSaldo,
			ValorFechamento,
			DataInclusao
	From	AbreFechaCaixa
	Where	CodigoAFCaixa = @CodigoAFCaixa
	and		Ativo = 1
End

ELSE IF @Modo = 5 -- Consulta Varias Aberturas
Begin
	Select	AF.CodigoAFCaixa,
			AF.DataCaixa,
			AF.ValorAbertura,
			AF.ValorSaldo,
			AF.ValorFechamento
	From	AbreFechaCaixa AF
	Where	Ativo = 1
End

ELSE IF @Modo = 6 -- Get Saldo
Begin	
	Select	ValorSaldo
	From	AbreFechaCaixa
	Where	DataCaixa = @DataCaixa
End

ELSE IF @Modo = 7 -- Atualiza Saldo
Begin	
	Update	AbreFechaCaixa
	set		ValorSaldo = @ValorSaldo,
			DataUltimaAlteracao = GETDATE(),
			UsuarioUltimaAlteracao = @NomeUsuarioTRAN
	Where	DataCaixa = @DataCaixa
End
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.CAX_AbreFechaCaixa_TRAN') and sysstat & 0xf = 4)
BEGIN
	PRINT 'Gerada a stored procedure dbo.CAX_AbreFechaCaixa_TRAN'
END
GO