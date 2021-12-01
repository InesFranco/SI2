SET XACT_ABORT ON
SET NOCOUNT ON
IF (DB_ID('Project1') IS NULL)
    BEGIN
        CREATE DATABASE Project1;
    end

GO
USE Project1;

----------------------------DROP------------------------------------------------
IF OBJECT_ID('intervencao_equipa') IS NOT NULL
	DROP TABLE intervencao_equipa
IF OBJECT_ID('funcionario_equipa') IS NOT NULL
	DROP TABLE funcionario_equipa
IF OBJECT_ID('equipa') IS NOT NULL
	DROP TABLE equipa
IF OBJECT_ID('intervencao') IS NOT NULL
	DROP TABLE intervencao
IF OBJECT_ID('funcionario_competencia') IS NOT NULL
	DROP TABLE funcionario_competencia
IF OBJECT_ID('competencia') IS NOT NULL
	DROP TABLE competencia
IF OBJECT_ID('activo_gerente') IS NOT NULL
	DROP TABLE activo_gerente
IF OBJECT_ID('funcionario') IS NOT NULL
	DROP TABLE funcionario
IF OBJECT_ID('activo_filho') IS NOT NULL
	DROP TABLE activo_filho
IF OBJECT_ID('activo_preçocomercial') IS NOT NULL
	DROP TABLE activo_preçocomercial
IF OBJECT_ID('activo') IS NOT NULL
	DROP TABLE activo
IF OBJECT_ID('tipo_activo') IS NOT NULL
	DROP TABLE tipo_activo

-----------------------------------CREATE-------------------------------




CREATE TABLE tipo_activo(
    id_tipo INT IDENTITY(1,1) PRIMARY KEY,
    description VARCHAR(50)
)

CREATE TABLE activo
(
    activo_id INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(50) NOT NULL,
    data_aquisicao DATE NOT NULL,
    estado Bit NOT NULL,
    marca VARCHAR(20),
    modelo VARCHAR(20),
    tipo INT FOREIGN KEY REFERENCES tipo_activo,
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
    id_funcionario INT IDENTITY(1,1) PRIMARY KEY ,
    numero_identificacao INT NOT NULL,
    nome VARCHAR(50),
    data_nascimento DATE,
    endereco VARCHAR(50),
    profissao VARCHAR(50),
    telefone INT,
    email VARCHAR(50) check(email LIKE '%_@__%.__%'),
)

CREATE TABLE activo_gerente(
    id_activo INT FOREIGN KEY REFERENCES activo,
    id_gerente INT FOREIGN KEY REFERENCES funcionario,
    PRIMARY KEY (id_activo, id_gerente)
)

CREATE TABLE competencia
(
    id_competencia INT IDENTITY(1,1) PRIMARY KEY,
    descricao VARCHAR(50) NOT NULL
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
    id_intervencao INT IDENTITY(1,1) PRIMARY KEY,
    id_activo INT FOREIGN KEY REFERENCES activo,
    descricao VARCHAR(50) NOT NULL,
    estado VARCHAR(50) NOT NULL,
    valor FLOAT NOT NULL,
    data_inicio DATE NOT NULL,
    data_fim DATE NOT NULL,
)


CREATE TABLE equipa
(
    codigo_equipa INT IDENTITY(1,1) PRIMARY KEY,
    localizacao VARCHAR(50) NOT NULL,
    num_elems INT NOT NULL,
    id_supervisor INT NOT NULL
)

CREATE TABLE funcionario_equipa
(
    id_funcionario INT NOT NULL FOREIGN KEY REFERENCES funcionario,
    codigo_equipa INT NOT NULL FOREIGN KEY REFERENCES equipa,
    PRIMARY KEY(codigo_equipa, id_funcionario)
)

CREATE TABLE intervencao_equipa
(
    codigo_equipa INT NOT NULL FOREIGN KEY REFERENCES equipa,
    id_intervencao INT NOT NULL FOREIGN KEY REFERENCES intervencao,
    data_inicio DATE NOT NULL,
    PRIMARY KEY (codigo_equipa, id_intervencao)
)