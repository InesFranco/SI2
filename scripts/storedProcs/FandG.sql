USE Project1
GO
--f)Criar o procedimento p_criaInter que permite criar uma intervenção;

Create Procedure p_criaIntervencao
    (@id_activo INT,
    @descricao VARCHAR(50),
    @estado VARCHAR(50),
    @valor FLOAT,
    @data_inicio DATE,
    @data_fim DATE)

AS
Begin
    begin
        Insert into intervencao (id_activo, descricao, estado, valor, data_inicio, data_fim)
        values (@id_activo, @descricao, @estado, @valor, @data_inicio, @data_fim)
    end
end

exec p_criaIntervencao 5, "arranjo", "concluido", 6.66, '2021/06/06', '2021/06/10'
drop procedure p_criaIntervencao

--g)Implementar o mecanismo que permite criar uma equipa;

create procedure p_criaEquipa
    (@localizacao VARCHAR(50),
    @id_supervisor int)
as
begin
    if not exists (select id_funcionario from funcionario where id_funcionario = @id_supervisor)
        begin
            RAISERROR (15600,-1,-1, 'p_criaEquipa');
        end
    begin
        insert into equipa (localizacao, num_elems, id_supervisor)
        values (@localizacao, 1, @id_supervisor)
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

    update equipa
    set num_elems = num_elems + 1
    where codigo_equipa = @id_equipa

    insert into funcionario_equipa
    values (@id_funcionario, @id_equipa)
end


exec p_adicionarElementoEquipa 1, 1
drop procedure p_adicionarElementoEquipa



--eliminates team and marks every intervention associated with this team as "por atribuir"
create procedure p_eliminarEquipa (@id_equipa int)
as
begin
    if not exists (select codigo_equipa from equipa where codigo_equipa = @id_equipa)
    begin
        RAISERROR (15600,-1,-1, 'p_eliminarEquipa')
    end

    ---update all intervention status with 'por atribuir'
    DECLARE @intervencao_cursor CURSOR;
    DECLARE @intervencao_id int;
    BEGIN
    SET @intervencao_cursor = CURSOR FOR
    select id_intervencao from intervencao_equipa
    where codigo_equipa = @id_equipa

    OPEN @intervencao_cursor
    FETCH NEXT FROM @intervencao_cursor
    INTO @intervencao_id

    WHILE @@FETCH_STATUS = 0
    BEGIN
        exec p_updateInter @intervencao_id, "Por Atribuir"
    FETCH NEXT FROM @intervencao_cursor
    INTO @intervencao_id
    END;

    CLOSE @intervencao_cursor;
    DEALLOCATE @intervencao_cursor;
    END;


    --delete all table entries with this team
    delete from funcionario_equipa
    where (codigo_equipa = @id_equipa)

    delete from equipa
    where (codigo_equipa = @id_equipa)

end

create procedure p_removerElementoEquipa
    (@id_equipa int,
    @id_funcionario int)
as
begin

    if not exists (select id_funcionario from funcionario where id_funcionario = @id_funcionario)
    or
    not exists (select codigo_equipa from equipa where codigo_equipa = @id_equipa)
    begin
        RAISERROR (15600,-1,-1, 'p_removerElementoEquipa');
    end

    if exists (select id_funcionario,codigo_equipa from funcionario_equipa where id_funcionario = @id_funcionario and @id_equipa = codigo_equipa)
        begin
            delete from funcionario_equipa
            where (id_funcionario = @id_funcionario and @id_equipa = codigo_equipa)

            update equipa
            set num_elems = num_elems - 1
            where codigo_equipa = @id_equipa

            declare @num_elems int
            set @num_elems = (select num_elems from equipa where codigo_equipa = @id_equipa)

            if(@num_elems = 0)
                begin
                    exec p_eliminarEquipa @id_equipa
                end
            end
end


exec p_removerElementoEquipa  1, 2
drop procedure p_removerElementoEquipa

--adiciona competencias de um funcionario na tabela funcionario_competencia
create procedure p_adicionarCompetencias
    (@id_funcionario int,
    @id_competencia int)
as
begin
    if not exists (select id_funcionario from funcionario where id_funcionario = @id_funcionario)
    or
    not exists (select id_competencia from competencia where id_competencia = @id_competencia)
    begin
        RAISERROR (15600,-1,-1, 'p_adicionarCompetencias')
    end

    insert into funcionario_competencia values (@id_funcionario, @id_competencia)
end

exec p_adicionarCompetencias 4, 1
drop procedure p_adicionarCompetencias

----------------------------h) until here-----------------------------


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