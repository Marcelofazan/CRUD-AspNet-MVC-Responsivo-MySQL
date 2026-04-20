# CRUD-AspNet-MVC-Responsivo-MySQL

Exemplo de utilização do AdminLTE com Criação de Formulário de Usuário e Registro da Conta em MVC com banco de dados MySQL.

* Template Responsivo AdminLTE 3.2 - rc

## Disponivel em (https://adminlte.io/ )

  - [AdminLTE](https://github.com/ColorlibHQ/AdminLTE/archive/refs/tags/v3.2.0-rc.zip)

## Requisitos

Baixar Pacote de Distribuição de Download da biblioteca e descompactar

Criar uma nova pasta chamada adminlte no Solution do Projeto
- Acessar o diretorio na pasta AdminLTE-3.2.0-rc /dist ->  Arrastar as pastas (css), (img) e (js) e Colar dentro da pasta adminlte
- Acessar AdminLTE-3.2.0-rc /plugins -> Arrastar a pasta (plugins) e Colar dentro da pasta adminlte

Ficara com a seguinte estrutura:

**Solution** Pasta na Raiz do Projeto 
```
| --- adminlte
     |-------- css
     |---------img 
     |---------js
     |---------plugins
```

# O que você vai encontrar neste projeto

- **AdminLTE** - Layout Responsivo JavaScript e uso de bibliotecas (Plugins). 
- **Dicionário de Dados** - Contexto definido realizado manualmente com T-SQL.
- **DataTable** - Tabelas de dados em formato Bootstrap .

## Execução da aplicação

Para executar a aplicação é necessário a execução do Script do MySql. 

## String de conexão do banco

Modifique a string de conexão no arquivo **Web.config**, no trecho indicado:

```bash
...
		server=127.0.0.1;userid=root;password=SUASENHA;database=SEUBANCO;persistsecurityinfo=True;
...

```

O script para criação da tabela do exemplo encontra-se na pasta **Database**.

### Aqui está uma demonstração do projeto:

<img width="1339" height="612" alt="CRUD-AspNet-1-N-AdminLTE" src="https://github.com/user-attachments/assets/5e51fc89-102f-4532-8472-ee729953bac1" />

