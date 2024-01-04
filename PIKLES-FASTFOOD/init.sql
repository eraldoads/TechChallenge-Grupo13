CREATE TABLE Cliente (
  IdCliente int NOT NULL AUTO_INCREMENT,
  Nome varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_520_ci NOT NULL,
  Sobrenome varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_520_ci NOT NULL,
  CPF text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_520_ci NOT NULL,
  Email text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_520_ci NOT NULL,
  PRIMARY KEY (IdCliente)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci;

CREATE TABLE Pedido (
  IdPedido int NOT NULL AUTO_INCREMENT,
  IdCliente int NOT NULL,
  DataPedido datetime NOT NULL,
  StatusPedido text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_520_ci NOT NULL,
  ValorTotal float NOT NULL,
  PRIMARY KEY (IdPedido)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci;

CREATE TABLE Produto (
  IdProduto int NOT NULL AUTO_INCREMENT,
  NomeProduto varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_520_ci NOT NULL,
  ValorProduto float NOT NULL,
  IdCategoria int DEFAULT NULL,
  DescricaoProduto varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_520_ci DEFAULT NULL,
  ImagemProduto longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_520_ci,
  PRIMARY KEY (IdProduto)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci;

CREATE TABLE Combo (
  IdCombo int NOT NULL AUTO_INCREMENT,
  PedidoId int DEFAULT NULL,
  PRIMARY KEY (IdCombo)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci;

CREATE TABLE ComboProduto (
  IdProdutoCombo int NOT NULL AUTO_INCREMENT,
  IdProduto int NOT NULL,
  ComboId int DEFAULT NULL,
  Quantidade int NOT NULL,
  PRIMARY KEY (IdProdutoCombo)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci;

CREATE TABLE Categoria (
  IdCategoria int NOT NULL AUTO_INCREMENT,
  NomeCategoria text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_520_ci,
  PRIMARY KEY (IdCategoria)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci;

CREATE TABLE Pagamento (
  IdPagamento INT NOT NULL AUTO_INCREMENT,
  StatusPagamento VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_520_ci NOT NULL DEFAULT 'Pendente',
  ValorPagamento FLOAT NOT NULL,
  MetodoPagamento VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_520_ci NOT NULL DEFAULT 'QRCode',
  DataPagamento DATETIME NOT NULL,
  IdPedido INT NOT NULL,
  PRIMARY KEY (IdPagamento),
  FOREIGN KEY (IdPedido) REFERENCES Pedido(IdPedido)
)ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_520_ci;


-- Massa de dados para testes
-- Categorias
INSERT INTO Categoria (NomeCategoria) VALUES
('Lanche'),
('Acompanhamento'),
('Bebida'),
('Sobremesa');

-- Produtos
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Burguer de Carne', 26.99, 1, 'Hamburguer de carne suculento, grelhado à perfeição, servido com queijo cheddar derretido, vegetais frescos e um molho especial da casa.', 'https://example.com/imagens/burguer_de_carne.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Burguer de Frango Grelhado', 23.99, 1, 'Hambúrguer de peito de frango grelhado, servido com alface, tomate e maionese temperada, tudo isso dentro de um pão macio de hambúrguer.', 'https://example.com/imagens/burguer_de_frango_grelhado.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Burguer de Picanha', 29.99, 1, 'Suculento hambúrguer feito de carne de picanha, grelhado até atingir a perfeição. Servido com queijo provolone derretido, alface, tomate e um molho especial.', 'https://example.com/imagens/burguer_de_picanha.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Burguer de Salmão', 32.99, 1, 'Delicioso hambúrguer de salmão feito com salmão fresco e temperos exclusivos. Servido com alface, cebola roxa e molho de endro.', 'https://example.com/imagens/burguer_de_salmão.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Burguer Vegetariano', 24.99, 1, 'Delicioso hambúrguer vegetariano feito com uma mistura especial de grãos, cogumelos e temperos. Servido com alface crocante, tomate e molho de iogurte.', 'https://example.com/imagens/burguer_vegetariano.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Burguer de Carne com Bacon', 27.99, 1, 'Hamburguer de carne suculento com bacon crocante, grelhado à perfeição, servido com queijo cheddar derretido, vegetais frescos e um molho especial da casa.', 'https://example.com/imagens/burguer_de_carne_com_bacon.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Burguer de Carne Acebolado', 26.99, 1, 'Hamburguer de carne suculento com cebola, grelhado à perfeição, servido com queijo cheddar derretido, vegetais frescos e um molho especial da casa.', 'https://example.com/imagens/burguer_de_carne_acebolado.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Batata Frita Crocante', 12.99, 2, 'Batatas fritas crocantes e douradas, temperadas com uma pitada de sal. O acompanhamento perfeito para qualquer refeição.', 'https://example.com/imagens/batata_frita_crocante.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Onion Rings', 10.99, 2, 'Anéis de cebola empanados e fritos até ficarem dourados e crocantes. Um acompanhamento delicioso para qualquer hambúrguer ou sanduíche.', 'https://example.com/imagens/onion_rings.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Nuggets de Frango', 14.99, 2, 'Nuggets de frango macios e suculentos, empanados e fritos até atingirem a perfeição. Servidos com molho de mostarda e mel.', 'https://example.com/imagens/nuggets_de_frango.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Anéis de Pimenta', 11.99, 2, 'Pimentas jalapeño empanadas e fritas até ficarem crocantes. Servidas com molho de queijo cheddar para um toque extra de sabor.', 'https://example.com/imagens/aneis_de_pimenta.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Salada de Verão', 9.99, 2, 'Salada fresca de alface, rúcula, tomate cereja e cenoura ralada, temperada com molho de vinagrete balsâmico. Uma opção saudável e deliciosa como acompanhamento.', 'https://example.com/imagens/salada_de_verao.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Refrigerante de Cola', 5.99, 3, 'Refrigerante refrescante sabor cola, servido em uma garrafa gelada de 500ml. O acompanhamento perfeito para qualquer refeição.', 'https://example.com/imagens/refrigerante_de_cola.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Suco de Laranja Natural', 7.99, 3, 'Suco de laranja fresco e natural, espremido na hora. Uma opção saudável e deliciosa como acompanhamento ou bebida refrescante.', 'https://example.com/imagens/suco_de_laranja_natural.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Água Mineral sem Gás', 3.99, 3, 'Água mineral pura e cristalina, perfeita para matar a sede e acompanhar qualquer refeição.', 'https://example.com/imagens/agua_mineral_sem_gas.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Café Expresso', 4.99, 3, 'Café expresso forte e encorpado, servido em uma xícara quente. A bebida perfeita para te dar energia e satisfação.', 'https://example.com/imagens/cafe_expresso.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Chá Gelado de Limão', 6.99, 3, 'Chá gelado de limão, adoçado na medida certa para refrescar seu dia. Uma opção deliciosa e revigorante como acompanhamento.', 'https://example.com/imagens/cha_gelado_de_limao.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Sorvete de Chocolate', 8.99, 4, 'Sorvete cremoso de chocolate belga, servido com raspas de chocolate e calda quente. Uma delícia irresistível para os amantes de chocolate.', 'https://example.com/imagens/sorvete_de_chocolate.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Mousse de Morango', 7.99, 4, 'Mousse de morango suave e aerado, decorado com morangos frescos. Uma sobremesa leve e deliciosa para encerrar qualquer refeição.', 'https://example.com/imagens/mousse_de_morango.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Torta de Limão', 9.99, 4, 'Torta de limão refrescante e azedinha, com uma base crocante de biscoito e cobertura de merengue. Uma sobremesa clássica e irresistível.', 'https://example.com/imagens/torta_de_limao.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Pudim de Leite', 6.99, 4, 'Pudim de leite macio e caramelizado, feito com ingredientes frescos e de alta qualidade. Uma sobremesa tradicional e reconfortante para todos os paladares.', 'https://example.com/imagens/pudim_de_leite.jpg');
INSERT INTO Produto (NomeProduto, ValorProduto, IdCategoria, DescricaoProduto, ImagemProduto) VALUES ('Cheesecake de Frutas Vermelhas', 10.99, 4, 'Cheesecake cremoso e suave com uma cobertura de frutas vermelhas frescas. Uma sobremesa elegante e saborosa para os amantes de doces.', 'https://example.com/imagens/cheesecake_de_frutas_vermelhas.jpg');

-- Clientes
INSERT INTO Cliente (Nome, Sobrenome, CPF, Email) VALUES ('João', 'das Coves Esfomeado', '87908638015', 'joao@esfomeado.com.br');
INSERT INTO Cliente (Nome, Sobrenome, CPF, Email) VALUES ('Maria', 'das Coves Esfomeado', '54817882093', 'maria@esfomeado.com.br');
INSERT INTO Cliente (Nome, Sobrenome, CPF, Email) VALUES ('Chaves', 'do Oito', '86866288002', 'chaves@chaves.com.br');
INSERT INTO Cliente (Nome, Sobrenome, CPF, Email) VALUES ('Chiquinha', 'Chorona', '96790122044', 'chiquinha@chaves.com.br');
INSERT INTO Cliente (Nome, Sobrenome, CPF, Email) VALUES ('Madruga', 'Senhor', '18936055070', 'srmadruga@chaves.com.br');
INSERT INTO Cliente (Nome, Sobrenome, CPF, Email) VALUES ('Kiko', 'Pipipipi', '23466838096', 'kiko@chaves.com.br');
INSERT INTO Cliente (Nome, Sobrenome, CPF, Email) VALUES ('Florinda', 'Dona', '40096924055', 'srbarriga@chaves.com.br');
INSERT INTO Cliente (Nome, Sobrenome, CPF, Email) VALUES ('Barriga', 'Senhor', '46048527071', 'donaflorinda@chaves.com.br');
INSERT INTO Cliente (Nome, Sobrenome, CPF, Email) VALUES ('Nhonho', 'Barriga', '45818274004', 'nhonho@chaves.com.br');