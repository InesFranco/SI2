CREATE TABLE activo
(
    id INT NOT NULL,
    nome VARCHAR NOT NULL,
    data_aquisicao DATE NOT NULL,
    state Bit NOT NULL,
    marca VARCHAR,
    modelo VARCHAR,
    tipo INT,
    PRIMARY KEY (id),
    FOREIGN KEY (tipo) REFERENCES tipo_activo
)

CREATE TABLE activo_gerente(
    id_activo INT,
    id_gerente INT,
    FOREIGN KEY (id_activo) REFERENCES activo,
    FOREIGN KEY (id_gerente) REFERENCES funcionario
)


CREATE TABLE tipo_activo(
    id_tipo INT,
    description VARCHAR(50),
    PRIMARY KEY (id_tipo)
)

CREATE TABLE activo_preco
(
    id_activo INT NOT NULL,
    preco FLOAT NOT NULL,
    data_alteracao DATE NOT NULL,
    FOREIGN KEY (id_activo) REFERENCES activo
)

CREATE TABLE activo_filho
(
    id_activo_pai INT NOT NULL,
    id_activo_filho INT NOT NULL,
    FOREIGN KEY (id_activo_pai) REFERENCES activo,
    FOREIGN KEY (id_activo_filho) REFERENCES activo
)

CREATE TABLE funcionario
(
    numero_identificacao INT NOT NULL,
    nome VARCHAR(50),
    data_nascimento DATE,
    endereco VARCHAR(50),
    profissao VARCHAR(15),
    telefone INT,
    email VARCHAR check(email LIKE '%_@__%.__%'),
    id_competencia INT,
    FOREIGN KEY (id_competencia) REFERENCES elemento_equipa_competencia
    PRIMARY KEY (numero_identificacao)
)


//todo
CREATE TABLE elemento_equipa_competencia
(
    funcionario_id INT NOT NULL,
    id_competencia INT NOT NULL,
    FOREIGN KEY (id_competencia) REFERENCES elemento_equipa_competencia
    PRIMARY KEY (numero_identificacao)
)



