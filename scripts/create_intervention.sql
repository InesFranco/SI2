USE Project1
GO

Create PROCEDURE p_criaInter
    (@id_intervencao
    @descricao VARCHAR(50)
    @estado VARCHAR(50)
    @valor FLOAT
    @data_inicio DATE
    @data_fim DATE)

AS
        Begin
            Insert into intervencao (id_intervencao, descricao, estado, valor, data_inicio, data_fim)
            values (@id_intervencao, @descricao, @estado, @valor, @data_incio, @data_fim)
        end


