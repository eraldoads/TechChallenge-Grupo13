# 🚀 TechChallenge-Grupo13
Aplicação para todo sistema de Controle de Pedidos de uma lanchonete - [API] Backend (monolito).

</br>

## 🖥️ Grupo 13 - Integrantes
🧑🏻‍💻 *<b>RM352133</b>*: Eduardo de Jesus Coruja </br>
🧑🏻‍💻 *<b>RM352316</b>*: Eraldo Antonio Rodrigues </br>
🧑🏻‍💻 *<b>RM352032</b>*: Luís Felipe Amengual Tatsch </br>

</br>

## 🔗 Links do projeto
- Documento: [DDD](https://1drv.ms/w/s!AntPAkrc0xN9q8kH5tUnZYZQgotMxQ?e=f4ur3f)
- Miro: [Dashboard Miro](https://miro.com/app/board/uXjVNftHwCM=/)
- GIT: [Repositório GIT](https://github.com/eraldoads/TechChallenge-Grupo13)
- Documentação API: [Swagger](https://www.xxxx.com)
- Testes: [Postman](https://www.xxxx.com)

</br>

## 🔗 Tecnologias

![Badge](https://img.shields.io/static/v1?label=.NET&message=framework&color=blue&style=for-the-badge&logo=.NET)
![Badge](https://img.shields.io/static/v1?label=csharp&message=linguagem&color=blue&style=for-the-badge&logo=Csharp)
![Badge](https://img.shields.io/static/v1?label=mysql&message=banco-de-dados&color=blue&style=for-the-badge&logo=mysql)
![Badge](https://img.shields.io/static/v1?label=docker&message=Plataforma&color=blue&style=for-the-badge&logo=docker)

</br>

## 🔗 Testes

Para executar esta solução, basta executar o comando <b>docker-compose up</b> dentro da pasta <b>PIKLES-FASTFOOD</b>, na qual está localizado o arquivo <b>docker-compose.yml</b>. Nesse momento, serão criados os containers da API e do Banco de Dados MySQL.
</br>
</br>
Também será criado um container para uma interface de admin do banco, onde será possível visualizar as tabelas criadas.
</br>
</br>
Após a criação do banco, serão executados os comandos definidos no arquivo <b>init.sql</b>, o qual contém a criação das tabelas e inserts para criação de uma massa de dados para os testes.


<b>Como acessar</b>:
</br>

<b>API</b>: http://localhost/swagger/index.html
</br>
<b>Interface admin MySQL</b>: http://localhost:8080/
</br>
</br>
<b>Servidor</b>: db
</br>
<b>Usuário</b>: pikles
</br>
<b>Senha</b>: fastfood
</br>
<b>Base de Dados</b>: piklesfastfood
</br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/e7cb3296-c50c-413f-b055-723bb0dca25e)



</br>

## 🔛 Fluxo:

```mermaid
graph LR
A[Cliente] --> B[API Backend]
B --> C[Databases]
```
