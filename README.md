# Filmap (Windows Phone)

## Table of Contents

- [Introdução](#intro)
	- [Equipe](#equipe)
	- [O projeto](#projeto)
- [Especificação](#espec)
	- [Funcionalidades](#func)
	- [Cenários](#cen)
		- [C1 - Abrir o app pela primeira vez](#c1)
		- [C2. Abrir o app pela segunda vez (aka já está logado)](#c2)
		- [C3. Marcar filme como assistido](#c3)
		- [C4. Pesquisar por filme](#c4)
		- [C5. Marcar como quer assistir](#c5)
		- [C6. Ver filmes próximos](#c6)
	- [Checklist de cenários implementados](#checkcen)
	- [Esboço de tabelas](#esboc)
		- [User](#usertable)
		- [Film](#filmtable)
		- [Geo](#geotable)

_____

## <a name="intro"></a>Introdução

App para a disciplina de Desenvolvimento Mobile (IFRN - 2015.2).
API disponível em [filmap_api](https://github.com/duartefq/filmap_api).

### <a name="equipe"></a>Equipe

* [Duarte Fernandes](https://github.com/duartefq)
* [Ivanilson Melo](https://github.com/ivmjunior)

### <a name="projeto"></a>O projeto

Idealmente um aplicativo de gerencialmento de filmes, com o **Filmap** os usuários poderão adicionar a uma lista pessoal os filmes que assistiu. Com o app, ele poderá também saber quais filmes outros usuários estão assistindo *ao seu redor*.

## <a name="espec"></a>Especificação

### <a name="func"></a>Funcionalidades

1. Marcar filmes que assistiu
2. Pesquisar por filmes
3. Ver filmes marcados próximos (usando geolocalização)

### <a name="cen"></a>Cenários

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

### <a name="checkcen"></a>Checklist de cenários implementados

- [ ] [C1: Abrir o app pela primeira vez](#c1)
- [ ] [C2: Abrir o app pela segunda vez (aka já está logado)](#c2)
- [ ] [C3: Marcar filme como assistido](#c3)
- [ ] [C4: Pesquisar por filme](#c4)
- [ ] [C5: Marcar como *quer assistir*](#c5)
- [ ] [C6: Ver *filmes próximos*](#c6)

### <a name="esboc"></a>Esboço de tabelas

> [ User ] 1 --- n [ Film ]

#### <a name="usertable"></a>Users
_____

| id 			| name			| email			| timestamps	|
| ------------- |:-------------:| :------------:|--------------:|
| ... 			| ... 			| ... 			| ... 			|

** Especificação **

- id [integer] : id do user no db
- name [string] : nome do usuario
- email [string] : email para login
- timestamps [Date] : duas colunas, created_at e updated_at.


_____

#### <a name="filmtable"></a>Films

| id 			| omdb			| user_id		| watched		| timestamps	|
| ------------- |:-------------:| :------------:|--------------:|--------------:|
| ... 			| ... 			| ... 			| ... 			| ... 			|

** Especificação **

- id [integer] : id da entrada no db 
- omdb [integer] : id do filme no [OMDb API](http://www.omdbapi.com)
- user_id [integer] : id do user
- watched [boolean] : **true** para assistido, **false** para *quer assistir*
- geo_id [integer] : referência à geo, coordenadas do registro
- timestamps [Date] : duas colunas, created_at e updated_at.

#### <a name="geotable"></a>Geos

| id 			| lat			| long			| film_id		| timestamps	|
| ------------- |:-------------:| :------------:|:-------------:|--------------:|
| ... 			| ... 			| ... 			| ... 			| ... 			|

** Especificação **

- id [integer] : id da entrada no db 
- lat [float(10,6)] : latitude
- lng [float(10,6)] : longitude
- film_id [integer] : referência à films
- timestamps [Date] : duas colunas, created_at e updated_at.

