Create Table Funcionario(
	CodigoFuncionario int identity(1,1) primary key,
	DataContratacao datetime not null,
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
	Sexo varchar(1) not null,
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
	Ativo bit not null,
	DataInclusao datetime not null,
	UsuarioIncluiu varchar(100) not null,
	DataUltimaAlteracao datetime null,
	UsuarioUltimaAlteracao varchar(100) null,
	CodigoFuncionario int foreign key references Funcionario(CodigoFuncionario)
);

Create Table Cliente(
	CodigoCliente int identity(1,1) primary key,
	DataCadastro datetime not null,
	NomeCliente varchar(100) not null,
	Cep varchar(10) null,
	Endereco varchar(200) null,
	Numero int null,
	Complemento varchar(100) null,
	Bairro varchar(100) null,
	Cidade varchar(100) null,
	Uf varchar(2) null,
	Pais varchar(50) null,
	DataNascimento datetime null,
	Documento varchar(20) not null,
	Sexo varchar(1) null,
	Email varchar(100) null,
	Telefone1 varchar(20) not null,	
	Telefone2 varchar(20) null,
	Telefone3 varchar(20) null,
	Ativo bit not null,
	DataInclusao datetime not null,
	UsuarioIncluiu varchar(100) not null,
	DataUltimaAlteracao datetime null,
	UsuarioUltimaAlteracao varchar(100) null
);

Create Table Fornecedor(
	CodigoFornecedor int identity(1,1) primary key,
	DataCadastro datetime not null,
	RazaoSocial varchar(100) not null,
	NomeFantasia varchar(100) not null,
	Documento varchar(20) not null,
	Cep varchar(10) not null,
	Endereco varchar(200) not null,
	Numero int not null,
	Complemento varchar(100) null,
	Bairro varchar(100) not null,
	Cidade varchar(100) not null,
	Uf varchar(2) not null,
	Pais varchar(50) not null,
	Telefone1 varchar(20) not null,	
	Telefone2 varchar(20) null,
	Telefone3 varchar(20) null,
	Email varchar(100) not null,
	Ativo bit not null,
	DataInclusao datetime not null,
	UsuarioIncluiu varchar(100) not null,
	DataUltimaAlteracao datetime null,
	UsuarioUltimaAlteracao varchar(100) null
);

Create Table Produto(
	CodigoProduto int identity(1,1) primary key,
	Descricao varchar(150) not null,
	Referencia varchar(50) not null,
	Localizacao varchar(100) null,
	Marca varchar(100) null,
	Categoria varchar(100) null,
	ValorUnitario decimal(18,2) not null,
	Quantidade int not null,
	QtdLimite int not null,
	Validade datetime not null,
	Ativo bit not null,
	DataInclusao datetime not null,
	UsuarioIncluiu varchar(100) not null,
	DataUltimaAlteracao datetime null,
	UsuarioUltimaAlteracao varchar(100) null,
	CodigoFornecedor int foreign key references Fornecedor(CodigoFornecedor)
);

Create Table Venda(
	CodigoVenda int identity(1,1) primary key,
	CodigoProduto int foreign key references Produto(CodigoProduto),
	CodigoCliente int foreign key references Cliente(CodigoCliente),
	Quantidade int not null,
	Valor decimal(18,2) not null,
	DataVenda datetime not null,
	Ativo bit not null,
	DataInclusao datetime not null,
	UsuarioIncluiu varchar(100) not null,
	DataUltimaAlteracao datetime null,
	UsuarioUltimaAlteracao varchar(100) null
);

Create Table ContasPagar(
	CodigoCpa int identity(1,1) primary key,
	CodigoFuncionario int foreign key references Funcionario(CodigoFuncionario),
	Valor decimal(18,2) not null,
	DataVencimento datetime not null,
	DataPagamento datetime null,
	ServicoCobrado varchar(200) not null,
	Paga bit not null,
	Ativo bit not null,
	DataInclusao datetime not null,
	UsuarioIncluiu varchar(100) not null,
	DataUltimaAlteracao datetime null,
	UsuarioUltimaAlteracao varchar(100) null
);