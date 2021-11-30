USE Project1
GO
--f)
Create Procedure p_criaInter
    (@descricao VARCHAR(50),
    @estado VARCHAR(50),
    @valor FLOAT,
    @data_inicio DATE,
    @data_fim DATE)

AS
        Begin
            IF OBJECT_ID('intervencao') IS NOT NULL
            begin
                Insert into intervencao (descricao, estado, valor, data_inicio, data_fim)
                values (@descricao, @estado, @valor, @data_inicio, @data_fim)
            end
        end

exec p_criaInter "banana", "Presente", 6.66, '19/06/1999', '30/11/2021'
drop procedure p_criaInter

--g)
create procedure p_CriaEquipa
    (@localizacao VARCHAR(50),
    @num_elems int,
    @id_supervisor int)
as
    begin
        IF OBJECT_ID('equipa') IS NOT NULL
        begin
            insert into equipa (localizacao, num_elems, id_supervisor)
            values (@localizacao, @num_elems, @id_supervisor)
        end
    end

exec p_CriaEquipa "Armazém", 5, 15
drop procedure p_CriaEquipa