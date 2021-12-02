USE Project1
GO


--d)

--insert

CREATE PROCEDURE p_inserirFuncionario
    (
        @numero_identificacao int,
        @nome varchar(50),
        @data_nascimento date,
        @endereco varchar(50),
        @profissao varchar(20),
        @telefone int,
        @email varchar(50)
    )
as
    begin
        insert into funcionario
        values(@numero_identificacao, @nome, @data_nascimento, @endereco, @profissao, @telefone, @email)
    end

exec p_inserirFuncionario 4444, 'júlio', '1997-04-22', 'rua marinheiro','engenheiro', 927272727, 'jl@hotmail.com'
drop procedure p_inserirFuncionario



--delete

CREATE PROCEDURE p_removerFuncionario(@id_funcionario int)
as
    begin
        --delete from competencia_funcionario table
        delete from funcionario_competencia
        where id_funcionario = @id_funcionario

        --delete from team
        DECLARE @equipa_cursor CURSOR;
        DECLARE @equipa_id int;
        BEGIN
        SET @equipa_cursor = CURSOR FOR
        select codigo_equipa from funcionario_equipa
        where id_funcionario = @id_funcionario

        OPEN @equipa_cursor
        FETCH NEXT FROM @equipa_cursor
        INTO @equipa_id

        WHILE @@FETCH_STATUS = 0
        BEGIN
            exec p_removerElementoEquipa @equipa_id, @id_funcionario
        FETCH NEXT FROM @equipa_cursor
        INTO @equipa_id
        END;

        CLOSE @equipa_cursor;
        DEALLOCATE @equipa_cursor;
        END;

        --remove activo_gerente table entry
        delete from activo_gerente
        where id_gerente = @id_funcionario

        --finally delete funcionario table entry
        delete from funcionario
        where id_funcionario = @id_funcionario

end


exec p_removerFuncionario 1
drop procedure p_removerFuncionario

--update

create procedure p_actualizaInformacaoFuncionario
    (
        @id_funcionario int,
        @endereco varchar(50) = NULL,
        @profissao varchar(20) = NULL,
        @telefone int = NULL,
        @email varchar(50) = NULL
    )
as
    begin
        if(@endereco is not NULL)
            begin
                update funcionario
                set endereco = @endereco
                where id_funcionario = @id_funcionario
            end
        if(@profissao is not NULL)
            begin
                update funcionario
                set profissao = @profissao
                where id_funcionario = @id_funcionario
            end
        if(@telefone is not NULL)
            begin
                update funcionario
                set telefone = @telefone
                where id_funcionario = @id_funcionario
            end
        if(@email is not NULL)
            begin
                update funcionario
                set email = @email
                where id_funcionario = @id_funcionario
            end
end

exec p_actualizaInformacaoFuncionario @id_funcionario = 2, @telefone = 92000000
drop procedure p_actualizaInformacaoFuncionario
 --d até aqui

--e
Create function p_encontrarEquipaParaIntervencao(@id_intervencao int)
            returns int
as
begin
    declare @competenciaNecessaria varchar(50)
    set @competenciaNecessaria = (select descricao from intervencao where @id_intervencao = id_intervencao)

    --procurar equipas que nao tenham mais de 3 intervencoes atribuidas e que tenham as competencias

    declare @equipasValidas table(codigo_equipa int primary key )

    declare @RowCount int
    select @RowCount = max(codigo_equipa) from equipa

    declare @EquipaID int = 1
    while @EquipaID <= @RowCount
    begin

        if exists(select codigo_equipa
            from equipa
            where codigo_equipa = @EquipaID)
            begin
                if dbo.verificarCompetenciasEquipa(@EquipaID, @competenciaNecessaria ) = 1
                    begin
                        insert into @equipasValidas select codigo_equipa from equipa where codigo_equipa = @EquipaID
                    end

            end
        set @EquipaID += 1

         --check latest intervention
    end

    declare @EarliestDate date
    select @EarliestDate = min(data_inicio) from intervencao_equipa

    declare @codigoFinal int
    select @codigoFinal=codigo_equipa from @equipasValidas where codigo_equipa in(
            select codigo_equipa from intervencao_equipa where data_inicio = @EarliestDate
        )

    declare @test int
    select @test= count(*) from @equipasValidas
    return @codigoFinal
end

select  dbo.p_encontrarEquipaParaIntervencao (1)
drop function p_encontrarEquipaParaIntervencao


--create view with the team id, element number and number of interventions already attributed
Create view IntervencoesPorEquipa
    as
    select ie.codigo_equipa, e.num_elems, count(ie.codigo_equipa) as intervencoes_atribuidas
    from intervencao_equipa ie
    join equipa e on ie.codigo_equipa = e.codigo_equipa
    where(
        (select num_elems
        from equipa
        where equipa.codigo_equipa = ie.codigo_equipa) >= 2
        )
    GROUP BY ie.codigo_equipa, e.num_elems

select * from IntervencoesPorEquipa


create function verificarCompetenciasEquipa
    (
        @id_equipa int,
        @competenciaNecessaria varchar(50)
    )
    returns bit
as
    begin

        --verify if number of elemts is at least 2 and if it doesnt already have 3 interventions
        if (select num_elems from IntervencoesPorEquipa where codigo_equipa = @id_equipa )<2
            or (select intervencoes_atribuidas from IntervencoesPorEquipa where codigo_equipa = @id_equipa) = 3
            begin
                return 0
            end

        --save just the workers of the specified team
        declare @funcionarioEquipaX table(id_funcionario int)

        insert into @funcionarioEquipaX
        select id_funcionario from funcionario_equipa
        where codigo_equipa = @id_equipa
        --

        --get row count
        declare @rowCounter int
        select @rowCounter = max(id_funcionario) from funcionario_equipa

        declare @id_funcionario int
        declare @funcionarioIDCounter int = 1

        while @funcionarioIDCounter <= @rowCounter
        BEGIN
            select @id_funcionario = id_funcionario from @funcionarioEquipaX
            where @funcionarioIDCounter = id_funcionario

            if dbo.verificarCompetenciasFuncionario(@id_funcionario, @competenciaNecessaria) = 1
                begin
                    return 1
                end
            set @funcionarioIDCounter += 1
        end
        return 0
    end

select dbo.verificarCompetenciasEquipa(2, 'arranjar piscinas')
drop function verificarCompetenciasEquipa




create function verificarCompetenciasFuncionario(
    @id_funcionario int,
    @competenciaNecessaria varchar(50)
    )
    returns bit
as
    begin
        declare @descricaoTabela table
            (
                id_competencia int primary key,
                descricao varchar(50)
            )

        insert into @descricaoTabela
        select id_competencia, descricao
        from competencia
        where (id_competencia in (
            select id_competencia
            from funcionario_competencia
            where id_funcionario = @id_funcionario
            )
        )

        begin
             if exists(select *
                  from @descricaoTabela
                  where @competenciaNecessaria in (select descricao from @descricaoTabela))
                begin
                    return 1
                end

        end
        return 0
end


drop FUNCTION verificarCompetenciasFuncionario
select dbo.verificarCompetenciasFuncionario(1,'arranjar piscinas')



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


--i)
create function f_listIntervention
    (@id_intervencao INT,
    @year int)
    returns table
    as
    return
            select id_intervencao,
                   descricao
            from intervencao
            where @year = year(data_inicio)
            and id_intervencao = @id_intervencao


drop function f_listIntervention

select * from f_listIntervention (1, '2021')





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