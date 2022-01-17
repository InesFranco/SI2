USE Project1
GO

drop procedure if exists p_adicionarElementoEquipa
drop procedure if exists p_removerElementoEquipa
drop procedure if exists p_adicionarCompetencias
drop procedure if exists p_inserirFuncionario
drop procedure if exists p_removerFuncionario
drop procedure if exists p_actualizaInformacaoFuncionario
drop FUNCTION if exists verificarCompetenciasFuncionario
drop function if exists verificarCompetenciasEquipa
drop function if exists encontrarEquipaParaIntervencao
drop procedure if exists p_criaIntervencao
drop procedure if exists p_criaEquipa
drop function if exists f_listInterventionsOfYear
drop procedure if exists p_updateInter
drop trigger if exists trgr_on_resumo
drop view if exists IntervencoesPorEquipa
drop view if exists vw_Resumo_Intervencao

--h)Actualizar (adicionar ou remover) os elementos de uma equipa e associar as respectivas
--competências
GO
create procedure p_adicionarElementoEquipa
    (@id_equipa int,
    @id_funcionario int)
as
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ
BEGIN TRAN
    --cada funcionario so pode pertencer a uma equipa
    if not exists (select id_funcionario from funcionario_equipa where id_funcionario = @id_funcionario)
        begin
            insert into funcionario_equipa
            values (@id_funcionario, @id_equipa)

            update equipa
            set num_elems = num_elems + 1
            where codigo_equipa = @id_equipa
        end
    else
        begin
            raiserror('Funcionário já pertence a uma equipa', 16, 1);
        end
COMMIT TRAN

go
create procedure p_removerElementoEquipa
    (@id_equipa int,
    @id_funcionario int)
as
begin
    SET TRANSACTION ISOLATION LEVEL REPEATABLE READ
    BEGIN TRAN
        delete from funcionario_equipa
        where (id_funcionario = @id_funcionario and @id_equipa = codigo_equipa)

        update equipa
        set num_elems = num_elems - 1
        where codigo_equipa = @id_equipa
    COMMIT TRAN
end

GO


--adiciona competencias de um funcionario na tabela funcionario_competencia
create procedure p_adicionarCompetencias
    (@id_funcionario int,
    @id_competencia int)
as
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ
    begin
        insert into funcionario_competencia values (@id_funcionario, @id_competencia)
    end
COMMIT TRAN
--h ends here


go



--d)
--insert funcionario
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
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ
begin transaction
    insert into funcionario
    values(@numero_identificacao, @nome, @data_nascimento, @endereco, @profissao, @telefone, @email)
COMMIT TRAN
go
--

--delete funcionario
CREATE PROCEDURE p_removerFuncionario(@id_funcionario int)

as
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ
BEGIN TRAN
    begin
        --delete from competencia_funcionario table
        delete from funcionario_competencia
        where id_funcionario = @id_funcionario

        --delete from team
        declare @rowcount int = (select max(codigo_equipa) from funcionario_equipa where id_funcionario = @id_funcionario)
        declare @equipa_id int = 1

        while @equipa_id <= @rowcount
        begin
            if(@equipa_id in (select codigo_equipa from funcionario_equipa where id_funcionario = @id_funcionario))
                begin
                    exec p_removerElementoEquipa @equipa_id, @id_funcionario
                end
            set @equipa_id += 1
        end

        --remove activo_gerente table entry
        delete from activo_gerente
        where id_gerente = @id_funcionario

        --finally delete funcionario table entry
        delete from funcionario
        where id_funcionario = @id_funcionario

    end
COMMIT TRAN
GO
--

--update data from funcionario
create procedure p_actualizaInformacaoFuncionario
    (
        @id_funcionario int,
        @endereco varchar(50) = NULL,
        @profissao varchar(20) = NULL,
        @telefone int = NULL,
        @email varchar(50) = NULL
    )

as
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ
BEGIN TRAN
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
COMMIT TRAN
--
GO
--d acaba aqui

--e)

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
GO


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

    if exists(select *
      from @descricaoTabela
      where @competenciaNecessaria in (select descricao from @descricaoTabela))
        begin
            return 1
        end
    return 0
end
GO

--

create function verificarCompetenciasEquipa
    (
        @id_equipa int,
        @competenciaNecessaria varchar(50),
        @id_activo int
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

            if
                --se o funcionario for gerente do activo nao pode participar
                @id_funcionario not like( select id_gerente from activo_gerente where id_activo = @id_activo)
            and
               dbo.verificarCompetenciasFuncionario(@id_funcionario, @competenciaNecessaria) = 1
                begin
                    return 1
                end
            set @funcionarioIDCounter += 1
        end
        return 0
    end
GO
--
Create function encontrarEquipaParaIntervencao(@id_intervencao int)
            returns int
as
begin
    declare @competenciaNecessaria varchar(50)
    set @competenciaNecessaria = (select descricao from intervencao where @id_intervencao = id_intervencao)

    declare @id_activo int
    select @id_activo= id_activo from intervencao where @id_intervencao = id_intervencao
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
                if dbo.verificarCompetenciasEquipa(@EquipaID, @competenciaNecessaria, @id_activo) = 1
                    begin
                        insert into @equipasValidas select codigo_equipa from equipa where codigo_equipa = @EquipaID
                    end
            end
        set @EquipaID += 1
    end

    --check earliest intervention
    declare @EarliestDate date
    select @EarliestDate = min(data_inicio) from intervencao_equipa

    declare @codigoFinal int
    select @codigoFinal=codigo_equipa from @equipasValidas where codigo_equipa in(
            select codigo_equipa from intervencao_equipa where data_inicio = @EarliestDate
        )

    return @codigoFinal
end
GO
--
--e) ends here

--f)procedimento p_criaIntervencao que permite criar uma intervenção;
Create Procedure p_criaIntervencao
    (@id_activo INT,
    @descricao VARCHAR(50),
    @valor FLOAT,
    @data_inicio DATE,
    @data_fim DATE
    --@id_intervencao int out
    )

AS
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ
BEGIN TRAN

    DECLARE @estado varchar(30)
    set @estado = 'por atribuir'

    DECLARE @data_obtencao_activo date
    select @data_obtencao_activo = data_aquisicao from activo where @id_activo = @id_activo
    if(@data_obtencao_activo > @data_inicio)
        begin
            Raiserror('Activo obtido antes da data de inicio da intervenção',16, 1 );
        end
    if(@data_inicio <= @data_fim )
        begin
            Insert into intervencao (id_activo, descricao, estado, valor, data_inicio, data_fim)
            values (@id_activo, @descricao, @estado, @valor, @data_inicio, @data_fim)
            select SCOPE_IDENTITY()
        end
    else
        begin
            Raiserror('Data de inicio maior que data de fim', 16, 1)
        end
COMMIT TRAN
GO

--


--g)Implementar o mecanismo que permite criar uma equipa;
create procedure p_criaEquipa
    (@localizacao VARCHAR(50),
    @id_supervisor int)
as
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
BEGIN TRAN
    begin

        insert into equipa (localizacao, num_elems, id_supervisor)
        values (@localizacao, 1, @id_supervisor)

        --adicionar a tablea funcionario equipa
        declare @codigo_equipa int
        set @codigo_equipa = (select max(codigo_equipa) from equipa)
        insert into funcionario_equipa values(@id_supervisor, @codigo_equipa)
    end
COMMIT TRAN
GO
--



--i)
create function f_listInterventionsOfYear(@ano int)
    returns table
as
return
    select id_intervencao,descricao
    from intervencao
    where @ano = year(data_inicio)
GO

--

--J  Actualizar o estado de uma intervenção;
Create Procedure p_updateInter(
        @id_intervencao int,
        @estado VARCHAR(50))
as
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
BEGIN TRAN
    update intervencao
    set	estado = @estado
    where id_intervencao = @id_intervencao
COMMIT TRAN
--
GO

--k)
--create view that aggregates data from intervencao and activo
Create View vw_Resumo_Intervencao
as
    select id_intervencao, descricao, i.estado, valor, data_inicio, data_fim, a.nome as nome_activo
    , a.marca as marca_activo, a.modelo as modelo_activo
    from intervencao i
    join activo a on i.id_activo = a.activo_id

GO


-- create trigger on the view above which, using a cursor, iterates through all the values in the inserted
--table and updates them
create trigger trgr_on_resumo on vw_Resumo_Intervencao
instead of update
as
    declare @rc as int
    set @rc = @@rowcount
    if @rc = 0 return

    SET NOCOUNT ON
    if @rc = 1
        update intervencao
        set estado = (select estado from inserted)
        where id_intervencao = (select id_intervencao from inserted)
    else
        begin
            declare @estado varchar(50)
            declare @id_intervencao int

            declare cursor_intervencao cursor FAST_FORWARD for
            select id_intervencao, estado from inserted
            open cursor_intervencao

            fetch next from cursor_intervencao into @id_intervencao,@estado
            while @@fetch_status = 0
            begin
                exec p_updateInter @id_intervencao, @estado
                fetch next from cursor_intervencao into @id_intervencao, @estado
            end

            CLOSE cursor_intervencao
            DEALLOCATE cursor_intervencao
        end
--k ends here