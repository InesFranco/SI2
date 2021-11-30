USE Project1
GO
--f)
Create PROCEDURE p_criaInter
    (@id_intervencao INT IDENTITY(1,1),
    @descricao VARCHAR(50),
    @estado VARCHAR(50),
    @valor FLOAT,
    @data_inicio DATE,
    @data_fim DATE)

AS
        Begin
            Insert into intervencao (id_intervencao, descricao, estado, valor, data_inicio, data_fim)
            values (@id_intervencao, @descricao, @estado, @valor, @data_inicio, @data_fim)
        end

    set dateformat dmy

exec p_criaInter   1, "banana", "Presente", 6.66, '19/06/1999', '30/11/2021'

--g)
create procedure p_CriaEquipa
    create table equipa
        (@codigo_equipa int identity(1,1),
        @localizacao VARCHAR(50),
        @num_elems int,
        @id_supervisor int)

as
    begin
        insert into equipa (codigo_equipa, localizacao, num_elems, id_supervisor)
        values (@codigo_equipa, @localizacao, @num_elems, @id_supervisor)
    end