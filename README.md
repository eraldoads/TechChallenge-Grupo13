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
- Miro: [Dashboard Miro](https://miro.com/app/board/uXjVNVsDxDM=/?share_link_id=908610551369)
- GIT: [Repositório GIT](https://github.com/eraldoads/TechChallenge-Grupo13)
- Documentação API: [Swagger](http://localhost/swagger/index.html)
- Testes: [Postman](https://www.postman.com/martian-resonance-699333/workspace/grupo-13-tech-challenge-fase-i/collection/13215309-ff36e055-fccf-48db-9965-b76e4ace4e93?tab=overview)

</br>

## ☑️ Criação do ambiente para testes

</br>
Realizar o download do projeto TechChallenge-Grupo13.

Acessar site https://webhook.site/ e copiar a url para teste do webhook.
</br>
Alterar valor da variável <b>WEBHOOK_ENDPOINT</b> dentro do arquivo <b>piklesfastfood-configmap.yaml</b> para a url copiada do site.
</br>
Iniciar o Docker Engine.
</br>
Abrir um terminal e executar o comando abaixo:
</br>
```
minikube start
```
</br>

Em seguida, executar o comando para habilitar a coleta de métricas no cluster:
</br>
```
minikube addons enable metrics-server
```
</br>

Abrir o terminal na pasta TechChallenge-Grupo13\kubernetes e executar os comandos apply, conforme abaixo:
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
Rodar o comando abaixo para expor a api na porta 8080:

```
kubectl port-forward svc/piklesfastfood 8080:80
```
</br>

Abrir outro terminal e rodar o comando abaixo para expor o Adminer na porta 8090:
```
kubectl port-forward svc/adminer 8090:8080
```
</br>

Acessar o Adminer no browser :
http://localhost:8090/

```
Servidor: mysql
Usuário: pikles
Senha: fastfood
Base de Dados: piklesfastfood
```
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/6351851a-262c-44b6-99c4-e196b43073d7)

Importar arquivo <b>init.sql</b> localizado na pasta <b>PIKLESFASTFOOD</b> e executar:
</br></br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/4c950fcb-b38f-485d-8df6-8c18fc2ba748)
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/d03e4e66-d90e-4455-8fc6-47bbea81282e)
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/7e2d045b-5c0a-431c-863f-9925c08c4ffe)
</br>

Rodar o comando minikube dashboard para visualizar os recursos criados no ambiente kubernetes

Ambiente pronto para testes

Sequência de testes:

Criação do pedido
Criação do pagamento

Opcional:
Obter QRCODE
Acessar site https://www.qrcode-monkey.com/ e gerar a imagem a partir do QRCODE obtido
Realizar o pagamento via Mercado Pago
Obter o id da merchant_order
Realizar o request para o endpoint webhook passando o id por parâmetro

Relizar o request do endpoint para obter o status do pedido
Realizar o request para o endpoint de atualização do status do pedido

</br>
<b>Como acessar</b>:
</br>

<b>API</b>: http://localhost/swagger/index.html
</br>
<b>Interface admin MySQL</b>: http://localhost:8080/
</br>
</br>
```
Servidor: mysql
Usuário: pikles
Senha: fastfood
Base de Dados: piklesfastfood
```
</br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/3a3723fa-2742-4d11-94e0-8c8351d62e01)
</br>
</br>

<b>⚠️ Atenção:</b> A documentação estará disponível somente depois de executar a solução. Para acessar a documentação do SWAGGER, clique na imagem abaixo:

[![Badge](https://img.shields.io/static/v1?label=swagger&message=Documentação&color=darkgreen&style=for-the-badge&logo=swagger)](https://www.postman.com/martian-resonance-699333/workspace/grupo-13-tech-challenge-fase-i/collection/13215309-ff36e055-fccf-48db-9965-b76e4ace4e93?tab=overview)

Para testar os endpoints da API via Postman, você deverá importar o json da collection e do enviroment, os quais estão disponíveis na pasta <b>Postman</b> dentro do projeto:

![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/69488ce6-4a61-4028-8c4a-ae9855e86eed)
</br>
</br>
![image](https://github.com/eraldoads/TechChallenge-Grupo13/assets/47857203/1f85cdac-dceb-4908-94f9-408e69d7dd4e)
</br>
</br>

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
