# The Movie App (temporary name)

_____

## Introdução

App para a disciplina de Desenvolvimento Mobile (IFRN - 2015.2). 

### Equipe

* [Duarte Fernandes](https://github.com/duartefq)
* [Ivanilson Melo](https://github.com/ivmjunior)

### O projeto

Idealmente um aplicativo de gerencialmento de filmes, com o **The Movie App** os usuários poderão adicionar a uma lista pessoal os filmes que assistiu. Com o app, ele poderá também saber quais filmes outros usuários estão assistindo *ao seu redor*.

## Especificação

### Funcionalidades

1. Marcar filmes que assistiu
2. Pesquisar por filmes
3. Ver filmes marcados próximos (usando geolocalização)

### Cenários

#### <a name="c1"></a>C1 - Abrir o app pela primeira vez

- Tela de Login é exibida
- Usuário realiza cadastro/login
	- Informações são armazenadas no BD
- Tela com sugestões de filmes é exibida
	- Opcões:
		- Puxar filmes com maior rating
		- *Filmes proximos*
- Usuário marca filmes da lista, [ver caso 3](#c3)

_____

#### <a name="c2"></a>C2. Abrir o app pela segunda vez (aka já está logado)

- Tela da página inicial é exibida, contendo dois blocos:
	- Sugestões baseadas em proximidade
	- Sugestões baseadas em rating
- Usuároi pode:
	- Marcar filme como assistido, [ver caso 3](#c3)
	- Visitar página de busca, [ver caso 4](#c4)

_____

#### <a name="c3"></a>C3. Marcar filme como assistido

- Usuário clica no filme
- Página do filme é exibida, com:
	- Título
	- Pequena sinopse
	- Foto
- Usuário marca como assistido
	- Informações são armazenadas no BD: filme e localização
- Usuário marca como *quer assistir* ([ver caso 5](#c5))

_____

#### <a name="c4"></a>C4. Pesquisar por filme

- Usuário clica no menu de busca
- Usuário entra com informações para busca:
	- Titulo
	- (...)
- Tela é populada com resultados da busca
- Usuário clica em um filme. [Ver caso 3](#c3)
	- Informações são armazenadas no BD: filme e localização
- Usuário marca como *quer assistir* ([ver caso 5](#c5))

_____

#### <a name="c5"></a>C5. Marcar como *quer assistir*

- Usuário clica no filme
- Página do filme é exibida, com:
	- Título
	- Pequena sinopse
	- Foto
- Usuário marca como *quer assistir*
	- Informações são armazenadas no BD: filme.
- Usuário pode ver os filmes marcados como *quer assistir* numa tela exclusiva.

_____

#### <a name="c6"></a>C6. Ver *filmes próximos*

- Usuário clica no menu de *filmes próximos*
- Tela é populada com *filmes próximos* baseados na localização
	- Usuário marca como assistido, [ver caso 3](#c3).
	- Usuário marca como *quer assistir* ([ver caso 5](#c5))

### Checklist de cenários implementados

- [ ] [C1: Abrir o app pela primeira vez](#c1)
- [ ] [C2: Abrir o app pela segunda vez (aka já está logado)](#c2)
- [ ] [C3: Marcar filme como assistido](#c3)
- [ ] [C4: Pesquisar por filme](#c4)
- [ ] [C5: Marcar como *quer assistir*](#c5)
- [ ] [C6: Ver *filmes próximos*](#c6)

### Esboço de tabelas

#### User
_____

| id 			| user			| name			| email			|
| ------------- |:-------------:| :------------:|--------------:|
| ... 			| ... 			| ... 			| ... 			|

** Especificação **

- id [integer] : id do user no db
- user [string] : user para login
- name [string] : nome do usuario
- email [string] : email para contato/login

_____

#### Movie

| id 			| id_omdb		| watched		| geo			|
| ------------- |:-------------:| :------------:|--------------:|
| ... 			| ... 			| ... 			| ... 			|

** Especificação **

- id [integer] : id da entrada no db 
- id_omdb [integer] : id do filme no [OMDb API](http://www.omdbapi.com)
- watched [boolean] : **true** para assistido, **false** para *quer assistir*
- geo [string?] : coordenadas? localização onde filme foi marcado como assistido

