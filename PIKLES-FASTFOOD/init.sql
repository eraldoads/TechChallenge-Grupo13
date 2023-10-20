CREATE TABLE cliente (
  IdCliente int NOT NULL AUTO_INCREMENT,
  Nome varchar(50) NOT NULL,
  Sobrenome varchar(50) NOT NULL,
  CPF text NOT NULL,
  Email text NOT NULL,
  PRIMARY KEY (IdCliente)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE pedido (
  IdPedido int NOT NULL AUTO_INCREMENT,
  IdCliente int NOT NULL,
  DataPedido datetime NOT NULL,
  StatusPedido text NOT NULL,
  ValorTotal float NOT NULL,
  PRIMARY KEY (IdPedido)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE produto (
  IdProduto int NOT NULL AUTO_INCREMENT,
  NomeProduto varchar(100) NOT NULL,
  ValorProduto float NOT NULL,
  idCategoriaProduto int DEFAULT NULL,
  DescricaoProduto varchar(500) DEFAULT NULL,
  PRIMARY KEY (IdProduto)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE combo (
  IdCombo int NOT NULL AUTO_INCREMENT,
  PedidoId int DEFAULT NULL,
  PRIMARY KEY (IdCombo)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE comboproduto (
  IdProdutoCombo int NOT NULL AUTO_INCREMENT,
  IdProduto int NOT NULL,
  ComboId int DEFAULT NULL,
  Quantidade int NOT NULL,
  PRIMARY KEY (IdProdutoCombo)
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE categoria (
  IdCategoria int NOT NULL AUTO_INCREMENT,
  NomeCategoria text,
  PRIMARY KEY (IdCategoria)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO categoria (IdCategoria, NomeCategoria) VALUES
(1, 'Lanche'),
(2, 'Acompanhamento'),
(3, 'Bebida'),
(4, 'Sobremesa');
