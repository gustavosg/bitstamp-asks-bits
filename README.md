
# Bitstamp Asks and Bits

Projeto desenvolvimento em .NET Core 6, utilizando arquitetura Hexagonal. Ele é composto de 2 projetos, sendo: 

- PriceListener (um projeto standalone terminal para consumo de dados da Bitstamp);
- PriceSimulator (um microserviço para simulação de melhor compra e venda de ativos)

# Docker

No momento, somente a API PriceSimulator está sendo executada via Docker. O projeto PriceListener executa normalmente no próprio host, porém ao iniciar via Docker, a aplicação não prossegue.
Para banco de dados, foi utilizado uma instância MySQL. 
Na aplicação, também há consumo de banco de dados NoSQL, utilizando o CosmosDB;

# Tests

Nem todos os testes estão com status de OK. Há testes comentados, aguardando verificação do problema. 