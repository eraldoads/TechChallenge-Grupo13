CREATE TABLE IF NOT EXISTS Cliente (
  Id INT NOT NULL AUTO_INCREMENT,
  Nome VARCHAR(50) NOT NULL,
  Sobrenome VARCHAR(50) NOT NULL,
  Cpf VARCHAR(14) NOT NULL,
  Email VARCHAR(254) NOT NULL,
  PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS Categoria (
  Id INT NOT NULL AUTO_INCREMENT,
  NomeCategoria VARCHAR(255) NOT NULL,
  PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS Produto (
  Id INT NOT NULL AUTO_INCREMENT,
  NomeProduto VARCHAR(100) NOT NULL,
  ValorProduto FLOAT NOT NULL,
  Descricao VARCHAR(500) NOT NULL,
  IdCategoria INT NOT NULL,
  PRIMARY KEY (Id),
  CONSTRAINT fk_categoria_produto FOREIGN KEY (IdCategoria) REFERENCES Categoria(Id)
);

CREATE TABLE IF NOT EXISTS Pedido (
  Id INT NOT NULL AUTO_INCREMENT,
  IdCliente INT NOT NULL,
  DataPedido DATE NOT NULL,
  ValorTotal DECIMAL(10,2) NOT NULL,
  PRIMARY KEY (Id),
  CONSTRAINT fk_cliente_pedido FOREIGN KEY (IdCliente) REFERENCES Cliente(Id)
);

CREATE TABLE IF NOT EXISTS Pedido_Produto (
  IdPedido INT NOT NULL,
  IdProduto INT NOT NULL,
  Quantidade INT NOT NULL,
  PRIMARY KEY (IdPedido, IdProduto),
  CONSTRAINT fk_pedido_pedido_produto FOREIGN KEY (IdPedido) REFERENCES Pedido(Id),
  CONSTRAINT fk_produto_pedido_produto FOREIGN KEY (IdProduto) REFERENCES Produto(Id)
);

INSERT INTO Categoria (NomeCategoria) VALUES ('Lanche');
INSERT INTO Categoria (NomeCategoria) VALUES ('Acompanhamento');
INSERT INTO Categoria (NomeCategoria) VALUES ('Bebida');
INSERT INTO Categoria (NomeCategoria) VALUES ('Sobremesa');