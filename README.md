# Comunicação em Rede - APS

## Proposta

Pede-se aos alunos que desenvolvam uma ferramenta para comunicação em rede. O grupo deverá criar uma aplicação que permita que duas ou mais pessoas possam se comunicar em uma rede, utilizando o protocolo TCP/IP.
Além da comunicação, outros elementos poderão ser acrescidos, tais como: componentes gráficos, emoticons, transferência de arquivos, comunicação multicast, e-mail, webcam, etc.

## Tecnologias

## Estrutura do projeto

- ``/ComunicacaoEmRedeApi``: .NET 8.0, EF Core, migrations e sockets;
- ``/ComunicacaoEmRedeFront``: Blazor, inclui todo o frontend;
- ``/Tests``: XUnit, inclui toda parte de testes (integração e unitários).

## Setup do ambiente

Esse projeto faz uso do .NET 8.0. Caso ainda não tiver instalado, faça o download da [SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

Depois disso, clone o repositório através do seguinte comando: 

```
git clone https://github.com/Invokedzz/comunicacao-em-rede-aps
```

Tendo realizado essa etapa, vá para a raiz do ``/ComunicacaoEmRedeApi`` e execute o comando para inicializar o projeto:

```
dotnet run
```

## Como contribuir

Se deseja contribuir nesse repositório, veja o [CONTRIBUTING.md](https://github.com/Invokedzz/comunicacao-em-rede-aps/blob/master/CONTRIBUTING.md) para conferir nossas guidelines.

## Licença

A licença escolhida para este projeto foi a MIT License. Veja a [LICENSE.md](https://github.com/Invokedzz/comunicacao-em-rede-aps/blob/master/LICENSE.md) para mais detalhes.
