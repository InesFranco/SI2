CREATE TABLE activo
(
    activo_id INT NOT NULL PRIMARY KEY,
    nome VARCHAR NOT NULL,
    data_aquisicao DATE NOT NULL,
    state Bit NOT NULL,
    marca VARCHAR,
    modelo VARCHAR,
    tipo INT FOREIGN KEY REFERENCES tipo_activo,
)

CREATE TABLE activo_gerente(
    id_activo INT FOREIGN KEY REFERENCES activo,
    id_gerente INT FOREIGN KEY REFERENCES funcionario,
    PRIMARY KEY (id_activo, id_gerente)
)


CREATE TABLE tipo_activo(
    id_tipo INT PRIMARY KEY,
    description VARCHAR(50)
)

CREATE TABLE activo_preçocomercial
(
    id_activo INT NOT NULL FOREIGN KEY REFERENCES activo,
    preco FLOAT NOT NULL,
    data_alteracao DATE NOT NULL,
    PRIMARY KEY (id_activo, data_alteracao)
)

CREATE TABLE activo_filho
(
    id_activo_pai INT NOT NULL,
    id_activo_filho INT NOT NULL,
    FOREIGN KEY (id_activo_pai) REFERENCES activo,
    FOREIGN KEY (id_activo_filho) REFERENCES activo,
    PRIMARY KEY (id_activo_pai, id_activo_filho)
)

CREATE TABLE funcionario
(
    id_funcionario INT NOT NULL PRIMARY KEY ,
    numero_identificacao INT NOT NULL,
    nome VARCHAR(50),
    data_nascimento DATE,
    endereco VARCHAR(50),
    profissao VARCHAR(15),
    telefone INT,
    email VARCHAR check(email LIKE '%_@__%.__%'),
)

CREATE TABLE competencia
(
    id_competencia INT NOT NULL PRIMARY KEY ,
    descricao VARCHAR NOT NULL
)


CREATE TABLE funcionario_competencia
(
    id_funcionario INT NOT NULL,
    id_competencia INT NOT NULL,
    FOREIGN KEY (id_competencia) REFERENCES competencia,
    FOREIGN KEY (id_funcionario) REFERENCES funcionario,
    PRIMARY KEY(id_funcionario,id_competencia)
)


CREATE TABLE intervencao
(
    id_intervencao INT NOT NULL PRIMARY KEY,
    descricao VARCHAR NOT NULL,
    estado VARCHAR NOT NULL,
    valor FLOAT NOT NULL,
    data_inicio DATE NOT NULL,
    data_fim DATE NOT NULL,
)


CREATE TABLE equipa
(
    codigo_equipa INT NOT NULL PRIMARY KEY,
    localizacao VARCHAR NOT NULL,
    num_elems INT NOT NULL check(num_elems >= 2),
    id_supervisor INT NOT NULL
)

CREATE TABLE funcionario_equipa
(
    codigo_equipa INT NOT NULL FOREIGN KEY REFERENCES equipa,
    id_funcionario INT NOT NULL FOREIGN KEY REFERENCES funcionario,
    PRIMARY KEY(codigo_equipa, id_funcionario)
)

CREATE TABLE intervencao_equipa
(
    codigo_equipa INT NOT NULL FOREIGN KEY REFERENCES equipa,
    id_intervencao INT NOT NULL FOREIGN KEY REFERENCES intervencao,
    data_inicio DATE NOT NULL,
    PRIMARY KEY (codigo_equipa, id_intervencao)
)