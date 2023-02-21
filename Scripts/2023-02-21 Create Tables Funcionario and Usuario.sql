Create Table Funcionario(
	CodigoFuncionario int identity(1,1) primary key,
	DataCadastro datetime not null,
	NomeFuncionario varchar(100) not null,
	Cep varchar(10) not null,
	Endereco varchar(200) not null,
	Numero int not null,
	Complemento varchar(100) null,
	Bairro varchar(100) not null,
	Cidade varchar(100) not null,
	Uf varchar(2) not null,
	Pais varchar(50) not null,
	DataNascimento datetime not null,
	Cpf varchar(20) not null,
	Sexo varchar(1) null,
	Telefone1 varchar(20) not null,	
	Telefone2 varchar(20) null,
	Telefone3 varchar(20) null,
	Cargo varchar(100) not null,
	Ativo bit not null,
	DataInclusao datetime not null,
	UsuarioIncluiu varchar(100) not null,
	DataUltimaAlteracao datetime null,
	UsuarioUltimaAlteracao varchar(100) null
);

Create Table Usuario(
	CodigoUsuario int identity(1,1) primary key,
	NomeUsuario varchar(100) not null,
	LoginUsuario varchar(100) not null,
	Email varchar(100) not null,
	Senha varchar(1000) not null,
	UsuarioAdm bit not null,
	DataInclusao datetime not null,
	UsuarioIncluiu varchar(100) not null,
	DataUltimaAlteracao datetime null,
	UsuarioUltimaAlteracao varchar(100) null,
	CodigoFuncionario int foreign key references Funcionario(CodigoFuncionario)
);