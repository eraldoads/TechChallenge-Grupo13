# 🚀 TechChallenge-Grupo13
Aplicação para todo sistema de Controle de Pedidos de uma lanchonete - [API] Backend (monolito).

O build e o push da imagem no ECR na AWS são realizados pelo Github Actions.

## 🖥️ Grupo 13 - Integrantes
🧑🏻‍💻 *<b>RM352133</b>*: Eduardo de Jesus Coruja </br>
🧑🏻‍💻 *<b>RM352316</b>*: Eraldo Antonio Rodrigues </br>
🧑🏻‍💻 *<b>RM352032</b>*: Luís Felipe Amengual Tatsch </br>

</br>

## 🔗 Links do projeto
- Vídeo Entrega Fase 2: https://youtu.be/hiPcLCMny-w
- Documento: [DDD](https://1drv.ms/w/s!AntPAkrc0xN9q8kH5tUnZYZQgotMxQ?e=f4ur3f)
- Miro: [Dashboard Miro](https://miro.com/app/board/uXjVNVsDxDM=/?share_link_id=908610551369)
- GIT: [Repositório GIT](https://github.com/eraldoads/TechChallenge-Grupo13)
- Documentação API: [Swagger](http://localhost/swagger/index.html)
- Testes: [Postman](https://www.postman.com/martian-resonance-699333/workspace/grupo-13-tech-challenge-fase-i/collection/13215309-ff36e055-fccf-48db-9965-b76e4ace4e93?tab=overview)

</br>

## ☑️ Arquitetura
O desenho abaixo apresenta uma visão macro contemplando o negócio e a infraestrutura utilizada:
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/ebe265a5-e109-4a81-90ea-c244c521271a)
</br></br>

## ☑️ Criação do ambiente para testes
Realize o download do projeto TechChallenge-Grupo13.
</br></br>
Acesse o site https://webhook.site/ e copie a url para teste do webhook.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/adf0b5a9-ee63-4eb7-8b3b-f0a2abe404df)
</br></br>
Altere o valor da variável <b>WEBHOOK_ENDPOINT</b> dentro do arquivo <b>piklesfastfood-configmap.yaml</b> para a url copiada do site. Este arquivo está localizado na pasta <b>kubernetes</b> dentro do projeto.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/7e837eb0-fe6a-4804-9df7-0e758ac22bfd)
</br></br>
Execute o Docker Engine.
</br></br>
Abra um terminal e execute o comando abaixo para iniciar o minikube:
</br>
```
minikube start
```
</br>

Também no terminal, acesse a pasta <b>kubernetes</b> dentro do projeto e execute os comandos a seguir:
</br>

```
kubectl apply -f mysql-configmap.yaml
kubectl apply -f mysql-pv.yaml
kubectl apply -f mysql-pvc.yaml
kubectl apply -f mysql-secrets.yaml
kubectl apply -f mysql-service.yaml
kubectl apply -f mysql-statefulset.yaml
kubectl apply -f piklesfastfood-configmap.yaml
kubectl apply -f piklesfastfood-deployment.yaml
kubectl apply -f piklesfastfood-hpa.yaml
kubectl apply -f piklesfastfood-secrets.yaml
kubectl apply -f piklesfastfood-service.yaml
kubectl apply -f adminer-deployment.yaml
kubectl apply -f adminer-service.yaml
```
</br>

Em seguida, execute o comando abaixo para habilitar a coleta de métricas no cluster:
</br>
```
minikube addons enable metrics-server
```
</br>

Execute o comando a seguir para visualizar os recursos criados no ambiente Kubernetes:

```
minikube dashboard
```

Será exibida a url para acessar o dashboard com o ambiente Kubernetes.

![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/1d2e9232-b443-4864-809e-48e4f2e85cee)

![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/04a157b5-6787-4c45-9d8a-c9006b3ae91a)

</br>

Abra outro terminal e execute o comando abaixo para expor a API na porta 8080:
</br>

```
kubectl port-forward svc/piklesfastfood 8080:80
```
</br>

Acesse a documentação da API:
http://localhost:8080/swagger/index.html

![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/dbd6fdd3-eb04-442f-b715-7b8f1649fa5c)

Abra outro terminal e execute o comando abaixo para expor o Adminer na porta 8090:
```
kubectl port-forward svc/adminer 8090:8080
```
</br>

Acesse o Adminer no browser: 
http://localhost:8090/


Utilize as seguintes credenciais:

```
Servidor: mysql
Usuário: pikles
Senha: fastfood
Base de Dados: piklesfastfood
```
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/8c6ae06e-a8ae-4bc9-b157-5f985f0445df)

Para criar as tabelas e inserir uma massa de dados no banco Mysql, importe o arquivo <b>init.sql</b> localizado na pasta <b>PIKLESFASTFOOD</b> dentro do projeto e clique no botão <b>Executar</b>.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/4c950fcb-b38f-485d-8df6-8c18fc2ba748)
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/d03e4e66-d90e-4455-8fc6-47bbea81282e)
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/7e2d045b-5c0a-431c-863f-9925c08c4ffe)
</br>

Para testar os endpoints da API via Postman, você deverá importar o json da collection e do enviroment, os quais estão disponíveis na pasta <b>Postman</b> dentro do projeto.

![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/69488ce6-4a61-4028-8c4a-ae9855e86eed)
</br>
</br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/1f85cdac-dceb-4908-94f9-408e69d7dd4e)
</br>
</br>

Após seguir todos os passos anteriores, o ambiente estará pronto para os testes.

## ☑️ Testes
Utilizando a collection do Postman, crie alguns pedidos.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/1b58dcaa-ef4f-452f-ae7f-bdf822158f60)

Posteriormente, altere o status de alguns deles utilizando os status permitidos:
</br></br>
<b>1 - Recebido</b>
</br>
<b>2 - Em Preparação</b>
</br>
<b>3 - Pronto</b>
</br>
<b>4 - Finalizado</b>
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/97251cbd-36f2-4c82-bce2-86d4ce61fc52)

Liste todos os pedidos realizados.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/b0b35cd7-ac34-4304-83f7-3e3131828d10)

Crie um pagamento para um pedido que esteja com o status <b>Recebido</b>.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/ac473e5f-a106-4eb0-b2d7-fd89abfdaa5a)
</br></br>
Obtenha o QRCode para pagamento do pedido no Mercado Pago.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/3ffbc7bc-c5ca-4e50-a677-8dc9e592d44d)
</br></br>
Acesse o site https://www.qrcode-monkey.com/ e gere a imagem a partir do QRCODE obtido no campo <b>qr_data</b>. Selecione a aba <b>TEXT</b>, cole o QRCode no campo <b>Your Text</b> e clique no botão <b>Create QR Code</b>.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/cf814600-1f62-41de-af08-d5035aec6b14)
</br></br>
Com o aplicativo do Mercado Pago, faça a leitura do QRCode.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/d4287e51-b44f-4b8e-921e-cc67683427d9)
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/cf1aa19a-6c57-4650-87e5-54d53990729c)
</br></br>
Informe dados inválidos para o cartão de crédito.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/99a923c4-e19f-458b-8b8f-f9c11d4fa37e)
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/dbcb4d1a-6f23-4b77-8c39-3e10e3556550)
</br></br>
Verifique a notificação recebida no Webhook.site e copie o <b>id</b> da <b>merchant_order</b>.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/e576df04-4a95-438a-b851-7483a6920ede)
</br></br>
Simule o recebimento da notificação do webhook de pagamento. 
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/1964f7a8-93a5-4f5e-9fe6-24bc545953d3)
</br></br>

Verifique o status do pagamento do pedido como <b>Rejeitado</b>.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/850937f3-f7d5-4244-8bcc-b4a82e17d1a7)

</br></br>

Repita o processo de pagamento informando um cartão de crédito válido.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/77e83a3b-b08a-43d3-abf8-4edb19e539b9)
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/092e336b-e10f-4d66-b6cf-d40d49d7f244)
</br></br>
Simule novamente o recebimento da notificação do webhook de pagamento.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/1964f7a8-93a5-4f5e-9fe6-24bc545953d3)
</br></br>
Verifique novamente o status do pagamento do pedido como <b>Aprovado</b>.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/e37cc5ba-49dd-4875-a548-c8379938949f)
</br></br>
Liste novamente os pedidos e verifique o status do pedido que recebeu o pagamento como <b>Em Preparação</b>.
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/8b97fb1c-f0d7-4722-8641-c8a702807ddc)
</br></br>

## 🔗 Tecnologias

![Badge](https://img.shields.io/static/v1?label=.NET&message=framework&color=blue&style=for-the-badge&logo=.NET)
![Badge](https://img.shields.io/static/v1?label=csharp&message=linguagem&color=blue&style=for-the-badge&logo=Csharp)
![Badge](https://img.shields.io/static/v1?label=mysql&message=banco-de-dados&color=blue&style=for-the-badge&logo=mysql)
![Badge](https://img.shields.io/static/v1?label=docker&message=Plataforma&color=blue&style=for-the-badge&logo=docker)

</br>

## 🔛 Fluxo:

```mermaid
graph LR
A[Cliente] --> B[API Backend]
B --> C[Databases]
```
