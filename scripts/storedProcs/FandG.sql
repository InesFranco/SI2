USE Project1
GO
--f)Criar o procedimento p_criaInter que permite criar uma intervenção;

Create Procedure p_criaIntervencao
    (@descricao VARCHAR(50),
    @estado VARCHAR(50),
    @valor FLOAT,
    @data_inicio DATE,
    @data_fim DATE)

AS
Begin
    begin
        Insert into intervencao (descricao, estado, valor, data_inicio, data_fim)
        values (@descricao, @estado, @valor, @data_inicio, @data_fim)
    end
end

exec p_criaIntervencao "arranjo", "concluido", 6.66, '19/06/1999', '30/11/2021'
drop procedure p_criaIntervencao

--g)Implementar o mecanismo que permite criar uma equipa;

create procedure p_criaEquipa
    (@localizacao VARCHAR(50),
    @num_elems int,
    @id_supervisor int)
as
begin
    if not exists (select id_funcionario from funcionario where id_funcionario = @id_supervisor)
        begin
            RAISERROR (15600,-1,-1, 'p_criaEquipa');
        end
    begin
        insert into equipa (localizacao, num_elems, id_supervisor)
        values (@localizacao, @num_elems, @id_supervisor)
    end
end

exec p_criaEquipa "Armazém", 5, 15
drop procedure p_criaEquipa



--h)Actualizar (adicionar ou remover) os elementos de uma equipa e associar as respectivas
--competências

create procedure p_adicionarElementoEquipa
    (@id_equipa int,
    @id_funcionario int)
as
begin
    if not exists (select id_funcionario from funcionario where id_funcionario = @id_funcionario)
    or
    not exists (select codigo_equipa from equipa where codigo_equipa = @id_equipa)
    begin
        RAISERROR (15600,-1,-1, 'p_adicionarElementoEquipa');
    end

    insert into funcionario_equipa
    values (@id_funcionario, @id_equipa)
end


exec p_adicionarElementoEquipa 1, 1
drop procedure p_adicionarElementoEquipa


--adiciona competencias de um funcionario na tabela funcionario_competencia
create procedure p_adicionarCompetencias
    (@id_funcionario int,
    @id_competencia int)
as
begin
    if not exists (select id_funcionario from funcionario where id_funcionario = @id_funcionario)
    or
    not exists (select codigo_equipa from equipa where codigo_equipa = @id_equipa)
    begin
        RAISERROR (15600,-1,-1, 'p_adicionarCompetencias');
    end


    insert into funcionario_competencia
    values (@id_funcionario, @id_competencia)
end

--J  Actualizar o estado de uma intervenção;
Create Procedure p_updateInter(
        @id_intervencao int,
        @estado VARCHAR(50))
AS
begin
    update intervencao
    set	estado = @estado
    where id_intervencao = @id_intervencao
end


drop procedure p_updateInter
exec p_updateInter 1, "concluido"