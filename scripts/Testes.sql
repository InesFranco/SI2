USE Project1
go

--TESTE D -> Insere um funcionario e remove-o logo a seguir
exec p_inserirFuncionario 3322, 'júlio', '1997-04-22', 'rua marinheiro','engenheiro', 927272727, 'jl@hotmail.com'

DECLARE @Expected AS VARCHAR(100)='júlio'
DECLARE @Actual AS VARCHAR(100)
SELECT @Actual = (select nome from funcionario where numero_identificacao = 3322)

print('expected ' + @Expected + ' got ' + @Actual)

DECLARE @funcionario_id int
select @funcionario_id = id_funcionario from funcionario where nome = @actual
exec p_removerFuncionario @funcionario_id


--------Test updating of info for funcionario


------test that the other values not passed to the stored procedure remain unchanged
DECLARE @previous_telefone int
DECLARE @previous_endereco varchar(50)
DECLARE @previous_email varchar(50)
DECLARE @previous_profissao varchar(50)

select @previous_telefone = telefone,
       @previous_endereco = endereco,
       @previous_email=email,
       @previous_profissao = profissao
from funcionario
where id_funcionario = 1


exec p_actualizaInformacaoFuncionario @id_funcionario = 1, @telefone = 900000000

DECLARE @Actual_phoneNumber int
DECLARE @Actual_endereco varchar(50)
DECLARE @Actual_email varchar(50)
DECLARE @Actual_profissao varchar(50)

select @Actual_phoneNumber = telefone,
       @Actual_endereco = endereco,
       @Actual_email= email,
       @Actual_profissao=profissao
from funcionario
where id_funcionario = 1

print('Expected Phone Number: ' + '900000000' + ' Got: ' + cast(@Actual_phoneNumber as varchar(50)))
print('Previous Endereco: ' + @previous_endereco + ' Got: ' + @Actual_endereco)
print('Previous email: ' + @previous_email + ' Got: ' + @Actual_email)
print('Previous profissao: ' + @previous_profissao + ' Got: ' + @Actual_profissao)


exec p_actualizaInformacaoFuncionario @id_funcionario = 1, @telefone = @previous_telefone




--------------------------------------------------------------------------------------------------------------------------------------
------TEST E)
begin tran

declare @Actual_codigo_equipa int
set @Actual_codigo_equipa = dbo.encontrarEquipaParaIntervencao (1)

DECLARE @Expected_codigo_equipa int
set @Expected_codigo_equipa = 2

print('Expected code ' + cast(@Expected_codigo_equipa as varchar(20)) + ' Got: ' + cast(@Actual_codigo_equipa as varchar(20)))

commit tran
--------------------------------------------------------------------------------------------------------------------------------------


-------TEST F

--fails because there no such ativo
exec p_criaIntervencao 10, 'remover carpete', 123, '2021-12-10', '2021-12-12'

--falha porque data de começo maior que data de fim
exec p_criaIntervencao 1, 'remover carpete',  123, '2021-12-10', '2021-12-9'

exec p_criaIntervencao 1, 'remover carpete',  123, '2021-12-9', '2021-12-12'

-------------------------------------------------------------------------------------------------------------------------------------
-------TESTE G

exec p_criaEquipa 'rua sá', 1


-------

--------TESTE H

--Teste que verifica que o numero de elementos de quipa é incrementado e decrementado com inserções e remoções
BEGIN TRAN
    declare @Codigo_Equipa int = 2

    declare @Before_Equipa_elems int
    select @Before_Equipa_elems = num_elems from equipa where codigo_equipa = @Codigo_Equipa

    exec p_adicionarElementoEquipa 2, 4

    declare @After_Equipa_elems int
    select @After_Equipa_elems = num_elems from equipa where codigo_equipa = @Codigo_Equipa

    print('Antes da inserção: ' + cast(@Before_Equipa_elems as varchar(20)) + ' Depois da inserção: ' + cast(@After_Equipa_elems as varchar(20)) )

    exec p_removerElementoEquipa 2,4

    declare @AfterRemocao_Equipa_elems int
    select @AfterRemocao_Equipa_elems = num_elems from equipa where codigo_equipa = @Codigo_Equipa

    print('Após Remoção: ' + cast(@AfterRemocao_Equipa_elems as varchar(20)))

COMMIT TRAN

--------TESTE i
BEGIN TRAN
    declare @Expected_row int = 4
    declare @Actual_rowcount int = (select count(*) from f_listInterventionsOfYear(2021))

    print('Expected row count: ' + cast(@Expected_row as varchar(20)) +  ' Actual row count: ' + cast(@Actual_rowcount as varchar(20)))
COMMIT TRAN
-------

----------------------------------------------------------------------------------------------------------------
-------TESTE J

BEGIN TRAN
    declare @Expected_estado varchar(30) = 'concluido'

    exec p_updateInter 1, 'concluido'

    declare @Actual_estado varchar(30)
    set @Actual_estado = (select estado from intervencao where id_intervencao = 1)

    print('Estado Esperado: ' + @Expected_estadO +  ' Estado Real: ' + @Actual_estado)
COMMIT TRAN

--------------------------------------------------------------------------------------------------------------


-----------TESTE K
BEGIN TRAN
    update vw_Resumo_Intervencao
    set estado = 'concluido'
    where id_intervencao = 1 or id_intervencao = 3
COMMIT TRAN

BEGIN TRAN
    update vw_Resumo_Intervencao
    set estado = 'por atribuir'
    where id_intervencao = 1

    update vw_Resumo_Intervencao
    set estado = 'em execução'
    where id_intervencao = 3
COMMIT TRAN