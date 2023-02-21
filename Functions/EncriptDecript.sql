if exists(select * from sysobjects where name='EncriptDecript')
	begin
		drop function Encript
	end
go

CREATE FUNCTION dbo.EncriptDecript( @Entrada VarChar(1000), @Acao char(1) )
	RETURNS varchar(1000)
AS

/********************************************************************
 ** Descricao:	Esta funcao encripta/desencripta uma string
  ********************************************************************
  select dbo.Encript()  
  
 ********************************************************************
 ** Data:		Autor:		Descricao:
 ** ----------	--------	-----------------
 ** 2023-02-21	Joao
 ********************************************************************/

BEGIN
	declare @cp varchar(200)
	declare @saida varchar(1000)
	declare @l int
	declare @tmp varchar(1000)

    select @CP = '*!&Wz26M8C9AqB4DkEG5JKsL3NPfQR7STUVXYZabcdeghijFlmnoHprItuvxwy1O 0{}[]()<>@#$%.=;,_', @saida = '', @tmp=''

	if @acao='E'
		begin
			-- ENCRIPTAR *******************************************
			select @tmp = 'TCC2023' + @Entrada

			select @l=1

			while @l <= len(@tmp)
				begin
					If @Saida <> ''
						select @saida=@saida +'-'
                
					select @saida  = @Saida + Right('00' + dbo.tohexa(charindex(substring(@tmp, @L, 1),@cp COLLATE Latin1_General_CS_AS)), 2)

					select @l = @l +1
				end
		end


	if @acao='D'
		begin
			--- DESENCRIPTAR *************************************
			declare @txttmp varchar(1000)

			select @txttmp = replace(@Entrada, '-','')

			while @txttmp<>''
				begin
					select @tmp = left(@txttmp,2)

					if len(@txttmp)>2
						select @txttmp = right(@txttmp, len(@txttmp)-2)
					else
						select @txttmp=''

					select @saida = @saida  + substring(@cp, dbo.hexatoint(@tmp),1)
				end

			if CHARINDEX('TCC2023',@saida)=0
				select @saida=''

			select @saida = replace(@saida, 'TCC2023','')
		end

	return isnull(@saida,'')
END

GO